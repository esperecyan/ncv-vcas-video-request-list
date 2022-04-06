namespace Esperecyan.NCVVCasVideoRequestList
{
    partial class Window
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.DataGridView = new System.Windows.Forms.DataGridView();
            this.requestBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.URL = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Copy = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.requestBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGridView
            // 
            this.DataGridView.AllowUserToAddRows = false;
            this.DataGridView.AllowUserToDeleteRows = false;
            this.DataGridView.AutoGenerateColumns = false;
            this.DataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Number,
            this.UserId,
            this.URL,
            this.Copy});
            this.DataGridView.DataSource = this.requestBindingSource;
            this.DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView.Location = new System.Drawing.Point(0, 0);
            this.DataGridView.Name = "DataGridView";
            this.DataGridView.ReadOnly = true;
            this.DataGridView.RowTemplate.Height = 21;
            this.DataGridView.Size = new System.Drawing.Size(800, 450);
            this.DataGridView.TabIndex = 0;
            // 
            // requestBindingSource
            // 
            this.requestBindingSource.DataSource = typeof(Esperecyan.NCVVCasVideoRequestList.Request);
            // 
            // Number
            // 
            this.Number.DataPropertyName = "CommentNumber";
            this.Number.HeaderText = "コメ番";
            this.Number.Name = "Number";
            this.Number.ReadOnly = true;
            this.Number.Width = 58;
            // 
            // UserId
            // 
            this.UserId.DataPropertyName = "UserNameOrId";
            this.UserId.HeaderText = "ユーザーID";
            this.UserId.Name = "UserId";
            this.UserId.ReadOnly = true;
            this.UserId.Width = 81;
            // 
            // URL
            // 
            this.URL.DataPropertyName = "URL";
            this.URL.HeaderText = "URL";
            this.URL.Name = "URL";
            this.URL.ReadOnly = true;
            this.URL.Width = 33;
            // 
            // Copy
            // 
            this.Copy.HeaderText = "コピー";
            this.Copy.Name = "Copy";
            this.Copy.ReadOnly = true;
            this.Copy.Text = "クリップボードへコピー";
            this.Copy.UseColumnTextForButtonValue = true;
            this.Copy.Width = 38;
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.DataGridView);
            this.Name = "Window";
            this.Text = "Window";
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.requestBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.DataGridView DataGridView;
        private System.Windows.Forms.BindingSource requestBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserId;
        private System.Windows.Forms.DataGridViewLinkColumn URL;
        private System.Windows.Forms.DataGridViewButtonColumn Copy;
    }
}
