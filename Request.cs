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
            (www\.nicovideo\.jp/watch/|nico\.ms/)[a-z]{2}[0-9]+
            |(www\.youtube\.com/watch\?v=|youtu\.be/)[-_0-9A-Za-z]+
        )", RegexOptions.IgnorePatternWhitespace);

        public string CommentNumber => this.commentData.No;
        public string UserNameOrId => this.userData.NickName ?? this.commentData.UserId;
        public string URL { get; private set; }

        internal Color? CommentBackgroundColor => this.userData.BGColor;

        private LiveCommentData commentData;
        private UserSettingInPlugin.UserData userData;

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

                return new Request()
                {
                    commentData = commentData,
                    userData = userData,
                    URL = match.Value,
                };
            });
        }
    }
}
