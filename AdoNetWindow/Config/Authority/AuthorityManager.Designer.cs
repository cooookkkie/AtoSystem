namespace AdoNetWindow.Config
{
    partial class AuthorityManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuthorityManager));
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbUsers = new System.Windows.Forms.RadioButton();
            this.rbGroup = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvAuthority = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.user_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.department = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnAddGroup = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuthority)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbUsers);
            this.panel1.Controls.Add(this.rbGroup);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1135, 29);
            this.panel1.TabIndex = 0;
            // 
            // rbUsers
            // 
            this.rbUsers.AutoSize = true;
            this.rbUsers.Location = new System.Drawing.Point(72, 7);
            this.rbUsers.Name = "rbUsers";
            this.rbUsers.Size = new System.Drawing.Size(59, 16);
            this.rbUsers.TabIndex = 1;
            this.rbUsers.Text = "유저별";
            this.rbUsers.UseVisualStyleBackColor = true;
            // 
            // rbGroup
            // 
            this.rbGroup.AutoSize = true;
            this.rbGroup.Checked = true;
            this.rbGroup.Location = new System.Drawing.Point(5, 7);
            this.rbGroup.Name = "rbGroup";
            this.rbGroup.Size = new System.Drawing.Size(59, 16);
            this.rbGroup.TabIndex = 0;
            this.rbGroup.TabStop = true;
            this.rbGroup.Text = "그룹별";
            this.rbGroup.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvAuthority);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 29);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1135, 561);
            this.panel2.TabIndex = 1;
            // 
            // dgvAuthority
            // 
            this.dgvAuthority.AllowUserToAddRows = false;
            this.dgvAuthority.AllowUserToDeleteRows = false;
            this.dgvAuthority.AllowUserToResizeColumns = false;
            this.dgvAuthority.AllowUserToResizeRows = false;
            this.dgvAuthority.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvAuthority.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.user_id,
            this.user_name,
            this.department});
            this.dgvAuthority.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAuthority.EnableHeadersVisualStyles = false;
            this.dgvAuthority.Location = new System.Drawing.Point(0, 0);
            this.dgvAuthority.Name = "dgvAuthority";
            this.dgvAuthority.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvAuthority.RowTemplate.Height = 23;
            this.dgvAuthority.Size = new System.Drawing.Size(1135, 561);
            this.dgvAuthority.TabIndex = 0;
            // 
            // user_id
            // 
            this.user_id.HeaderText = "user_id";
            this.user_id.Name = "user_id";
            this.user_id.Visible = false;
            // 
            // user_name
            // 
            this.user_name.HeaderText = "이름";
            this.user_name.Name = "user_name";
            // 
            // department
            // 
            this.department.HeaderText = "부서";
            this.department.Name = "department";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnAddGroup);
            this.panel3.Controls.Add(this.btnUpdate);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 590);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1135, 43);
            this.panel3.TabIndex = 2;
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAddGroup.Location = new System.Drawing.Point(175, 3);
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Size = new System.Drawing.Size(70, 36);
            this.btnAddGroup.TabIndex = 22;
            this.btnAddGroup.Text = "그룹추가";
            this.btnAddGroup.UseVisualStyleBackColor = true;
            this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUpdate.Location = new System.Drawing.Point(79, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(90, 36);
            this.btnUpdate.TabIndex = 20;
            this.btnUpdate.Text = "일괄수정(A)";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(251, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 36);
            this.btnExit.TabIndex = 21;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(3, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(70, 36);
            this.btnSearch.TabIndex = 19;
            this.btnSearch.Text = "검색(Q)";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // AuthorityManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1135, 633);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "AuthorityManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "권한관리";
            this.Load += new System.EventHandler(this.AuthorityManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AuthorityManager_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuthority)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rbUsers;
        private System.Windows.Forms.RadioButton rbGroup;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvAuthority;
        private System.Windows.Forms.Button btnAddGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn department;
    }
}