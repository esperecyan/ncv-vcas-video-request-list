using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Plugin;

namespace Esperecyan.NCVVCasVideoRequestList
{
    public class Plugin : IPlugin
    {
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
                var url = this.window.Requests[e.RowIndex].URL;
                switch (dataGridView.Columns[e.ColumnIndex].Name)
                {
                    case "URL":
                        Process.Start(new ProcessStartInfo()
                        {
                            UseShellExecute = true,
                            FileName = url,
                        });
                        break;
                    case "Copy":
                        Clipboard.SetText(url);
                        var row = dataGridView.Rows[e.RowIndex];
                        row.DefaultCellStyle.ForeColor = Color.Gray;
                        foreach (var cell in row.Cells)
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
            foreach (var commentData in e.CommentDataList)
            {
                foreach (var request in Request.Create(commentData, this.userDataList))
                {
                    this.window.Requests.Add(request);
                }
            }
        }
    }
}
