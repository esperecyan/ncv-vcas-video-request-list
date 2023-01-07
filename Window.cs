using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Esperecyan.NCVVCasVideoRequestList.Properties;

namespace Esperecyan.NCVVCasVideoRequestList
{
    public partial class Window : Form
    {
        internal BindingList<Request> Requests = new BindingList<Request>();

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
    }
}
