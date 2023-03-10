using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Csv;
using Esperecyan.NCVVCasVideoRequestList.Properties;

namespace Esperecyan.NCVVCasVideoRequestList
{
    public partial class Window : Form
    {
        internal BindingList<Request> Requests = new BindingList<Request>();

        private Color defaultLinkColor;
        private Color defaultVisitedLinkColor;

        public Window()
        {
            this.InitializeComponent();

            if (Settings.Default.WindowLeft != default)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Left = Settings.Default.WindowLeft;
                this.Top = Settings.Default.WindowTop;
                this.Width = Settings.Default.WindowWidth;
                this.Height = Settings.Default.WindowHeight;
            }

            this.notPushingAnonymousCommentToVCICheckBox.Checked = Settings.Default.NotPushingAnonymousCommentToVCI;

            this.DataGridView.DataSource = this.Requests;
            this.DataGridView.DataBindingComplete += (sender, e) =>
            {
                if (e.ListChangedType != ListChangedType.ItemAdded)
                {
                    return;
                }

                var index = this.Requests.Count - 1;
                var color = this.Requests[index].CommentBackgroundColor;
                if (color == null)
                {
                    return;
                }
                this.DataGridView.Rows[index].DefaultCellStyle.BackColor = color.Value;
            };
            this.Requests.ListChanged += (sender, e) =>
            {
                if (e.ListChangedType != ListChangedType.ItemChanged)
                {
                    return;
                }

                var color = default(Color);
                switch (this.Requests[e.NewIndex].VirtualCastSupport)
                {
                    case "待機中":
                        return;

                    case "○":
                        color = Color.LimeGreen;
                        break;

                    default:
                        color = Color.Crimson;
                        break;
                }
                this.DataGridView[this.DataGridView.Columns["VirtualCastSupport"].Index, e.NewIndex].Style.ForeColor
                    = color;

                var row = this.DataGridView.Rows[e.NewIndex];
                var alreadyPlayed = this.Requests[e.NewIndex].AlreadyPlayed;
                row.DefaultCellStyle.ForeColor = alreadyPlayed ? Color.Gray : default;
                foreach (var cell in row.Cells)
                {
                    if (cell is DataGridViewLinkCell linkCell)
                    {
                        if (this.defaultLinkColor == default)
                        {
                            this.defaultLinkColor = linkCell.LinkColor;
                        }
                        linkCell.LinkColor = alreadyPlayed ? row.DefaultCellStyle.ForeColor : this.defaultLinkColor;
                        if (this.defaultVisitedLinkColor == default)
                        {
                            this.defaultVisitedLinkColor = linkCell.VisitedLinkColor;
                        }
                        linkCell.VisitedLinkColor = alreadyPlayed
                            ? row.DefaultCellStyle.ForeColor
                            : this.defaultVisitedLinkColor;
                    }
                }
                row.DefaultCellStyle.BackColor = alreadyPlayed ? Color.LightGray : default;
            };
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            Settings.Default.WindowLeft = this.Left;
            Settings.Default.WindowTop = this.Top;
            Settings.Default.WindowWidth = this.Width;
            Settings.Default.WindowHeight = this.Height;
            Settings.Default.Save();
        }

        private void DataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // チェックを切り替えた時点で強制的に確定させる
            // 【C#】DataGridViewでセルを変更した瞬間にイベントを発生させる【Form】 | Hiyo Code <https://hiyo-code.com/dvg-cellchange/>
            if (this.DataGridView.IsCurrentCellDirty)
            {
                this.DataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void NotPushingAnonymousCommentToVCICheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.NotPushingAnonymousCommentToVCI = ((CheckBox)sender).Checked;
            Settings.Default.Save();
        }

        private void ListCopyButton_Click(object sender, EventArgs e)
        {
            var requests = this.Requests.Where(request => request.VirtualCastSupport == "○" && request.AlreadyPlayed);
            if (requests.Count() == 0)
            {
                MessageBox.Show(
                    "読み込み可能な動画、かつ「済」へチェックが入っているリクエストが一つもありません。",
                    caption: this.Text
                );
                return;
            }

            Clipboard.SetText(CsvWriter.WriteToText(
                headers: new string[4],
                requests.Select(request => new[]
                {
                    request.Title,
                    request.UserNameOrId,
                    request.URL,
                    request.Used.ToString(),
                }),
                separator: '\t',
                skipHeaderRow: true
            ));
        }
    }
}
