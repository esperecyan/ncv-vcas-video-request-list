using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;
using Plugin;
using NicoLibrary.NicoLiveData;

namespace Esperecyan.NCVVCasVideoRequestList
{
    internal class Request : IDisposable, INotifyPropertyChanged
    {
        private static readonly Regex SupportedURLPattern = new Regex(@"
            (?<niconico>(sm|nm|so)[0-9]{1,11}) # 2022年現在の動画IDは8桁
            |(https?://www\.youtube\.com/watch\?[^#]*?v=|youtu\.be/)(?<youtube>[-_0-9A-Za-z]{11})
        ", RegexOptions.IgnorePatternWhitespace);
        private static readonly TimeSpan MinimumInterval = TimeSpan.FromSeconds(10);

        private static readonly IDictionary<VideoStreamingServices, DateTimeOffset>
            VideoStreamingServiceNextFetchingTimePairs = new Dictionary<VideoStreamingServices, DateTimeOffset>();
        private static HttpClient HttpClient;

        public event PropertyChangedEventHandler PropertyChanged;
        public string CommentNumber => this.commentData.No;
        public string UserNameOrId => this.userData?.NickName ?? this.commentData.UserId;
        public string URL
        {
            get
            {
                var prefix = "";
                switch (this.videoStreamingService)
                {
                    case VideoStreamingServices.Niconico:
                        prefix = "https://nico.ms/";
                        break;
                    case VideoStreamingServices.YouTube:
                        prefix = "https://youtu.be/";
                        break;
                }
                return prefix + this.videoId;
            }
        }
        public string Title { get; private set; }
        public string VirtualCastSupport { get; private set; } = "待機中";
        private bool alreadyPlayed;
        public bool AlreadyPlayed
        {
            get => this.alreadyPlayed; set
            {
                this.alreadyPlayed = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.AlreadyPlayed)));
            }
        }
        private bool used;
        public bool Used
        {
            get => this.used; set
            {
                this.used = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Used)));
                if (this.used)
                {
                    this.AlreadyPlayed = true;
                }
            }
        }

        internal Color? CommentBackgroundColor => this.userData?.BGColor;
        internal bool IsAnonymity => this.commentData.IsAnonymity;
        internal string UserId => this.commentData.UserId;
        internal string ServiceName
        {
            get
            {
                switch (this.videoStreamingService)
                {
                    case VideoStreamingServices.Niconico:
                        return "ニコニコ";
                    case VideoStreamingServices.YouTube:
                        return "YouTube";
                    default:
                        return null;
                }
            }
        }

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private LiveCommentData commentData;
        private UserSettingInPlugin.UserData userData;
        private VideoStreamingServices videoStreamingService;
        private string videoId;

        public void Dispose()
        {
            this.cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// コメントからリクエストを作成します。
        /// </summary>
        /// <param name="commentData"></param>
        /// <param name="userDataList"></param>
        /// <returns>存在しなければ空になります。</returns>
        internal static IEnumerable<Request> Create(
            LiveCommentData commentData,
            IEnumerable<UserSettingInPlugin.UserData> userDataList
        )
        {
            if (commentData.IsNGUser || commentData.IsNGComment
                || commentData.IsOfficialNGUser || commentData.IsOfficialNGWord)
            {
                return new List<Request>();
            }

            return Request.SupportedURLPattern.Matches(commentData.Comment).Cast<Match>().Select(match =>
            {
                var userData = userDataList.FirstOrDefault(data => data.UserId == commentData.UserId);
                var videoStreamingService = default(VideoStreamingServices);
                var videoId = "";
                if (match.Groups["niconico"].Value != "")
                {
                    videoStreamingService = VideoStreamingServices.Niconico;
                    videoId = match.Groups["niconico"].Value;
                }
                else if (match.Groups["youtube"].Value != "")
                {
                    videoStreamingService = VideoStreamingServices.YouTube;
                    videoId = match.Groups["youtube"].Value;
                }

                var request = new Request()
                {
                    commentData = commentData,
                    userData = userData,
                    videoStreamingService = videoStreamingService,
                    videoId = videoId,
                };
                request.FetchVideoInformation();
                return request;
            });
        }

        /// <summary>
        /// 同一のサービスへの要求のあいだに、一定の時間を空けるようにします。
        /// </summary>
        /// <param name="videoStreamingService"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private static async Task WaitRequest(
            VideoStreamingServices videoStreamingService,
            CancellationToken cancellationToken
        )
        {
            var sleepTimeSpan = TimeSpan.Zero;
            if (Request.VideoStreamingServiceNextFetchingTimePairs.ContainsKey(videoStreamingService)
                && Request.VideoStreamingServiceNextFetchingTimePairs[videoStreamingService] > DateTimeOffset.Now)
            {
                sleepTimeSpan
                    = Request.VideoStreamingServiceNextFetchingTimePairs[videoStreamingService] - DateTimeOffset.Now;
            }

            Request.VideoStreamingServiceNextFetchingTimePairs[videoStreamingService]
                = DateTimeOffset.Now + sleepTimeSpan + Request.MinimumInterval;
            if (sleepTimeSpan > TimeSpan.Zero)
            {
                await Task.Delay(sleepTimeSpan, cancellationToken);
            }
        }

        /// <summary>
        /// 指定したURLから文字列データを取得します。
        /// </summary>
        /// <param name="url"></param>
        /// <returns>HTTPステータスエラーの場合は <c>null</c> を返します。</returns>
        private async Task<string> Fetch(Uri url)
        {
            if (Request.HttpClient == null)
            {
                Request.HttpClient = new HttpClient();
            }

            var response = await Request.HttpClient.GetAsync(url, this.cancellationTokenSource.Token);
            if (!response.IsSuccessStatusCode)
            {
                this.VirtualCastSupport = response.StatusCode.ToString();
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }

        private async void FetchVideoInformation()
        {
            try
            {
                await Request.WaitRequest(this.videoStreamingService, this.cancellationTokenSource.Token);

                switch (this.videoStreamingService)
                {
                    case VideoStreamingServices.Niconico:
                        var niconicoContent = await this.Fetch(
                            new Uri("https://ext.nicovideo.jp/api/getthumbinfo/" + this.videoId)
                        );
                        if (niconicoContent == null)
                        {
                            break;
                        }

                        var niconicoDoc = new XmlDocument();
                        niconicoDoc.LoadXml(niconicoContent);
                        if (niconicoDoc.DocumentElement.GetAttribute("status") != "ok")
                        {
                            this.VirtualCastSupport = "404";
                            break;
                        }

                        this.Title = niconicoDoc.GetElementsByTagName("title")[0].InnerText;
                        switch (this.videoId.Substring(startIndex: 0, length: 2))
                        {
                            case "sm":
                                this.VirtualCastSupport
                                    = niconicoDoc.GetElementsByTagName("embeddable")[0].InnerText == "1"
                                        ? "○"
                                        : "埋込不可";
                                break;
                            case "nm":
                                this.VirtualCastSupport = "NMM";
                                break;
                            case "so":
                                this.VirtualCastSupport = "公式ch";
                                break;
                        }
                        break;

                    case VideoStreamingServices.YouTube:
                        var youtubeContent = await this.Fetch(
                            new Uri("https://www.youtube.com/watch?v=" + this.videoId)
                        );
                        if (youtubeContent == null)
                        {
                            break;
                        }

                        var youtubeDoc = new HtmlDocument();
                        youtubeDoc.LoadHtml(youtubeContent);
                        var titleMetaNode = youtubeDoc.DocumentNode.SelectSingleNode("//meta[@name='twitter:title']");
                        if (titleMetaNode == null)
                        {
                            this.VirtualCastSupport = "404";
                            break;
                        }

                        this.Title = HtmlEntity.DeEntitize(titleMetaNode.GetAttributeValue("content", def: null));
                        this.VirtualCastSupport
                            = youtubeDoc.DocumentNode.SelectSingleNode("//meta[@name='twitter:player']") != null
                                ? "○"
                                : "埋込不可";
                        break;
                }

                foreach (var propertyName in new[] { nameof(this.Title), nameof(this.VirtualCastSupport) })
                {
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }
            catch (TaskCanceledException) { }
        }
    }
}
