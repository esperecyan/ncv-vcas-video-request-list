using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Esperecyan.NCVVCasVideoRequestList
{
    public partial class Window : Form
    {
        internal BindingList<Request> Requests = new BindingList<Request>();

        public Window()
        {
            this.InitializeComponent();
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
    }
}
