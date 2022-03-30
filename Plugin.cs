using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Plugin;

namespace Esperecyan.NCVVCasVideoRequestList
{
    public class Plugin : IPlugin
    {
        private static readonly Regex SupportedURLPattern = new Regex(@"https?://(
            (www\.nicovideo\.jp/watch/|nico\.ms/)[a-z]{2}[0-9]+
            |(www\.youtube\.com/watch\?v=|youtu\.be/)[0-9A-Za-z]+
        )", RegexOptions.IgnorePatternWhitespace);

        public IPluginHost Host { get; set; }

        public bool IsAutoRun => false;

        public string Description => ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(
            Assembly.GetExecutingAssembly(),
            typeof(AssemblyTitleAttribute)
        )).Title;

        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(fieldCount: 3);

        public string Name => ((AssemblyProductAttribute)Attribute.GetCustomAttribute(
            Assembly.GetExecutingAssembly(),
            typeof(AssemblyProductAttribute)
        )).Product;

        private Window window;
        private IEnumerable<UserSettingInPlugin.UserData> userDataList;

        public void AutoRun()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            if (this.window != null && !this.window.IsDisposed)
            {
                this.window.Focus();
                return;
            }
            this.window = new Window()
            {
                Text = this.Name + " " + this.Version,
            };
            this.window.Show();
            this.userDataList = this.Host.GetUserSettingInPlugin().UserDataList;
            this.Host.ReceivedComment += this.Host_ReceivedComment;
            this.window.FormClosing += (sender, args) => this.Host.ReceivedComment -= this.Host_ReceivedComment;
            var dataGridView = this.window.DataGridView;
            dataGridView.CellContentClick += (object sender, DataGridViewCellEventArgs e) =>
            {
                switch (dataGridView.Columns[e.ColumnIndex].Name)
                {
                    case "Copy":
                        Clipboard.SetText((string)dataGridView.Rows[e.RowIndex].Cells.Cast<DataGridViewCell>()
                            .First(cell => cell.OwningColumn.Name == "URL").Value);
                        break;
                }
            };
        }

        private void Host_ReceivedComment(object sender, ReceivedCommentEventArgs e)
        {

            var rows = this.window.DataGridView.Rows;
            foreach (var commentData in e.CommentDataList)
            {
                if (commentData.IsNGUser || commentData.IsNGComment
                    || commentData.IsOfficialNGUser || commentData.IsOfficialNGWord)
                {
                    continue;
                }

                foreach (var match in Plugin.SupportedURLPattern.Matches(commentData.Comment).Cast<Match>())
                {
                    var userData = this.userDataList.FirstOrDefault(data => data.UserId == commentData.UserId);

                    var index = rows.Add(new string[] {
                        commentData.No,
                        userData?.NickName ?? commentData.UserId,
                        match.Value,
                        "クリップボードへコピー",
                    });
                    if (userData != null)
                    {
                        rows[index].DefaultCellStyle.BackColor = userData.BGColor;
                    }
                }
            }
        }
    }
}
