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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.label1 = new System.Windows.Forms.Label();
            this.notPushingAnonymousCommentToVCICheckBox = new System.Windows.Forms.CheckBox();
            this.ListCopyButton = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.URL = new System.Windows.Forms.DataGridViewLinkColumn();
            this.VirtualCastSupport = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Copy = new System.Windows.Forms.DataGridViewButtonColumn();
            this.AlreadyPlayed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Used = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.VirtualCastSupport,
            this.Copy,
            this.AlreadyPlayed,
            this.Used,
            this.Title});
            this.DataGridView.DataSource = this.requestBindingSource;
            this.DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView.Location = new System.Drawing.Point(0, 24);
            this.DataGridView.Name = "DataGridView";
            this.DataGridView.RowTemplate.Height = 21;
            this.DataGridView.Size = new System.Drawing.Size(800, 426);
            this.DataGridView.TabIndex = 0;
            this.DataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridView_CurrentCellDirtyStateChanged);
            // 
            // requestBindingSource
            // 
            this.requestBindingSource.DataSource = typeof(Esperecyan.NCVVCasVideoRequestList.Request);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(10, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "VCIへのデータ送信";
            // 
            // notPushingAnonymousCommentToVCICheckBox
            // 
            this.notPushingAnonymousCommentToVCICheckBox.AutoSize = true;
            this.notPushingAnonymousCommentToVCICheckBox.Location = new System.Drawing.Point(122, 5);
            this.notPushingAnonymousCommentToVCICheckBox.Name = "notPushingAnonymousCommentToVCICheckBox";
            this.notPushingAnonymousCommentToVCICheckBox.Size = new System.Drawing.Size(104, 16);
            this.notPushingAnonymousCommentToVCICheckBox.TabIndex = 4;
            this.notPushingAnonymousCommentToVCICheckBox.Text = "184を送信しない";
            this.notPushingAnonymousCommentToVCICheckBox.UseVisualStyleBackColor = true;
            this.notPushingAnonymousCommentToVCICheckBox.CheckedChanged += new System.EventHandler(this.NotPushingAnonymousCommentToVCICheckBox_CheckedChanged);
            // 
            // ListCopyButton
            // 
            this.ListCopyButton.Location = new System.Drawing.Point(284, 1);
            this.ListCopyButton.Name = "ListCopyButton";
            this.ListCopyButton.Size = new System.Drawing.Size(160, 23);
            this.ListCopyButton.TabIndex = 5;
            this.ListCopyButton.Text = "一覧をクリップボードへコピー";
            this.toolTip1.SetToolTip(this.ListCopyButton, "読み込み可能な動画、かつ「済」へチェックが入っているリクエストを、TSV形式でクリップボードへコピーします。\r\n\r\n「タイトル」「ユーザーID」「URL」「流した" +
        "」列の内容がこの順番でコピーされます。\r\n「流した」列は、チェックが入っていれば「True」、入っていなければ「False」になります。\r\nなお、TSVのヘッダ" +
        "行は含まれません。");
            this.ListCopyButton.UseVisualStyleBackColor = true;
            this.ListCopyButton.Click += new System.EventHandler(this.ListCopyButton_Click);
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
            this.UserId.DataPropertyName = "NickNameOrId";
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
            // VirtualCastSupport
            // 
            this.VirtualCastSupport.DataPropertyName = "VirtualCastSupport";
            this.VirtualCastSupport.HeaderText = "読込可能";
            this.VirtualCastSupport.Name = "VirtualCastSupport";
            this.VirtualCastSupport.ReadOnly = true;
            this.VirtualCastSupport.Width = 78;
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
            // AlreadyPlayed
            // 
            this.AlreadyPlayed.DataPropertyName = "AlreadyPlayed";
            this.AlreadyPlayed.HeaderText = "済";
            this.AlreadyPlayed.Name = "AlreadyPlayed";
            this.AlreadyPlayed.Width = 23;
            // 
            // Used
            // 
            this.Used.DataPropertyName = "Used";
            this.Used.HeaderText = "流した";
            this.Used.Name = "Used";
            this.Used.ToolTipText = "実際に流したか否か。楽曲利用申請等が必要かあとから確認するためのものです。この列へチェックを入れると、「済」列へも自動でチェックが入ります。";
            this.Used.Width = 41;
            // 
            // Title
            // 
            this.Title.DataPropertyName = "Title";
            this.Title.HeaderText = "タイトル";
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            this.Title.Width = 65;
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ListCopyButton);
            this.Controls.Add(this.notPushingAnonymousCommentToVCICheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DataGridView);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Window";
            this.Text = "Window";
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.requestBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.DataGridView DataGridView;
        private System.Windows.Forms.BindingSource requestBindingSource;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox notPushingAnonymousCommentToVCICheckBox;
        private System.Windows.Forms.Button ListCopyButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserId;
        private System.Windows.Forms.DataGridViewLinkColumn URL;
        private System.Windows.Forms.DataGridViewTextBoxColumn VirtualCastSupport;
        private System.Windows.Forms.DataGridViewButtonColumn Copy;
        private System.Windows.Forms.DataGridViewCheckBoxColumn AlreadyPlayed;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Used;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
    }
}
