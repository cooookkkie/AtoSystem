namespace AdoNetWindow.LoginForm
{
    partial class UsersManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UsersManager));
            this.UsersList = new System.Windows.Forms.ListView();
            this.id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Delivery = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtId = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtTel = new System.Windows.Forms.TextBox();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // UsersList
            // 
            this.UsersList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.id,
            this.Delivery,
            this.tel,
            this.status});
            this.UsersList.FullRowSelect = true;
            this.UsersList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.UsersList.HideSelection = false;
            this.UsersList.Location = new System.Drawing.Point(9, 39);
            this.UsersList.Name = "UsersList";
            this.UsersList.Size = new System.Drawing.Size(382, 262);
            this.UsersList.TabIndex = 8;
            this.UsersList.UseCompatibleStateImageBehavior = false;
            this.UsersList.View = System.Windows.Forms.View.Details;
            this.UsersList.SelectedIndexChanged += new System.EventHandler(this.UsersList_SelectedIndexChanged);
            // 
            // id
            // 
            this.id.Text = "ID";
            this.id.Width = 100;
            // 
            // Delivery
            // 
            this.Delivery.Text = "이름";
            this.Delivery.Width = 100;
            // 
            // tel
            // 
            this.tel.Text = "전화번호";
            this.tel.Width = 100;
            // 
            // status
            // 
            this.status.Text = "승인";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(9, 12);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(100, 21);
            this.txtId.TabIndex = 9;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(110, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 21);
            this.txtName.TabIndex = 10;
            // 
            // txtTel
            // 
            this.txtTel.Location = new System.Drawing.Point(211, 12);
            this.txtTel.Name = "txtTel";
            this.txtTel.Size = new System.Drawing.Size(100, 21);
            this.txtTel.TabIndex = 11;
            // 
            // cbStatus
            // 
            this.cbStatus.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Items.AddRange(new object[] {
            "대기",
            "승인",
            "삭제"});
            this.cbStatus.Location = new System.Drawing.Point(312, 12);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(79, 21);
            this.cbStatus.TabIndex = 12;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUpdate.ForeColor = System.Drawing.Color.Blue;
            this.btnUpdate.Location = new System.Drawing.Point(9, 307);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(63, 30);
            this.btnUpdate.TabIndex = 13;
            this.btnUpdate.Text = "수정(A)";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnDelete.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.ForeColor = System.Drawing.Color.Red;
            this.btnDelete.Location = new System.Drawing.Point(78, 307);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(63, 30);
            this.btnDelete.TabIndex = 14;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(324, 307);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(63, 30);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // UsersManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 343);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.cbStatus);
            this.Controls.Add(this.txtTel);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.UsersList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "UsersManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "사용자 관리";
            this.Load += new System.EventHandler(this.UsersManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UsersManager_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView UsersList;
        private System.Windows.Forms.ColumnHeader id;
        private System.Windows.Forms.ColumnHeader Delivery;
        private System.Windows.Forms.ColumnHeader tel;
        private System.Windows.Forms.ColumnHeader status;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtTel;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnExit;
    }
}