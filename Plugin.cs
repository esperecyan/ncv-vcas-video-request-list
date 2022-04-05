using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
            |(www\.youtube\.com/watch\?v=|youtu\.be/)[-_0-9A-Za-z]+
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
            this.Host_ReceivedComment(sender: null, new ReceivedCommentEventArgs(this.Host.GetAcquiredComment()));
            this.Host.ReceivedComment += this.Host_ReceivedComment;
            this.window.FormClosing += (sender, args) => this.Host.ReceivedComment -= this.Host_ReceivedComment;
            var dataGridView = this.window.DataGridView;
            dataGridView.CellContentClick += (object sender, DataGridViewCellEventArgs e) =>
            {
                switch (dataGridView.Columns[e.ColumnIndex].Name)
                {
                    case "URL":
                        Process.Start(new ProcessStartInfo()
                        {
                            UseShellExecute = true,
                            FileName = (string)dataGridView[e.ColumnIndex, e.RowIndex].Value,
                        });
                        break;
                    case "Copy":
                        var row = dataGridView.Rows[e.RowIndex];
                        var cells = row.Cells.Cast<DataGridViewCell>();
                        Clipboard.SetText((string)cells.First(cell => cell.OwningColumn.Name == "URL").Value);
                        row.DefaultCellStyle.ForeColor = Color.Gray;
                        foreach (var cell in cells)
                        {
                            if (cell is DataGridViewLinkCell linkCell)
                            {
                                linkCell.LinkColor = row.DefaultCellStyle.ForeColor;
                            }
                        }
                        row.DefaultCellStyle.BackColor = Color.LightGray;
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
