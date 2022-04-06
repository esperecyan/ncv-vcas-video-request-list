using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Plugin;
using NicoLibrary.NicoLiveData;

namespace Esperecyan.NCVVCasVideoRequestList
{
    internal class Request
    {
        private static readonly Regex SupportedURLPattern = new Regex(@"https?://(
            (www\.nicovideo\.jp/watch/|nico\.ms/)(?<niconico>(sm|nm|so)[0-9]{1,11}) # 2022年現在の動画IDは8桁
            |(www\.youtube\.com/watch\?v=|youtu\.be/)(?<youtube>[-_0-9A-Za-z]{11})
        )", RegexOptions.IgnorePatternWhitespace);

        public string CommentNumber => this.commentData.No;
        public string UserNameOrId => this.userData.NickName ?? this.commentData.UserId;
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

        internal Color? CommentBackgroundColor => this.userData.BGColor;

        private LiveCommentData commentData;
        private UserSettingInPlugin.UserData userData;
        private VideoStreamingServices videoStreamingService;
        private string videoId;

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

                return new Request()
                {
                    commentData = commentData,
                    userData = userData,
                    videoStreamingService = videoStreamingService,
                    videoId = videoId,
                };
            });
        }
    }
}
