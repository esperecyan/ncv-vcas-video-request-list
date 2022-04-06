using System.ComponentModel;
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
        }
    }
}
