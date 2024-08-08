namespace AdoNetWindow.Config.AuthorityManager
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
            this.rbDepartment = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvAuthority = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.group_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.form_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isAdd = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isUpdate = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isDelete = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isExcel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isPrint = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isAdmin = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel7 = new System.Windows.Forms.Panel();
            this.cbAuthorityTemplate = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUpdateTarget = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.dgvAuthorityTarget = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbUsername = new System.Windows.Forms.Label();
            this.lbDepartment = new System.Windows.Forms.Label();
            this.lbUserid = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnRegistration = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnAuthorityRefresh = new System.Windows.Forms.Button();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.department = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.is_registration = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuthority)).BeginInit();
            this.panel7.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuthorityTarget)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbUsers);
            this.panel1.Controls.Add(this.rbDepartment);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(307, 30);
            this.panel1.TabIndex = 0;
            // 
            // rbUsers
            // 
            this.rbUsers.AutoSize = true;
            this.rbUsers.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbUsers.ForeColor = System.Drawing.Color.Blue;
            this.rbUsers.Location = new System.Drawing.Point(80, 5);
            this.rbUsers.Name = "rbUsers";
            this.rbUsers.Size = new System.Drawing.Size(76, 20);
            this.rbUsers.TabIndex = 1;
            this.rbUsers.TabStop = true;
            this.rbUsers.Text = "사용자";
            this.rbUsers.UseVisualStyleBackColor = true;
            this.rbUsers.CheckedChanged += new System.EventHandler(this.rbDepartment_CheckedChanged);
            // 
            // rbDepartment
            // 
            this.rbDepartment.AutoSize = true;
            this.rbDepartment.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbDepartment.ForeColor = System.Drawing.Color.Blue;
            this.rbDepartment.Location = new System.Drawing.Point(12, 5);
            this.rbDepartment.Name = "rbDepartment";
            this.rbDepartment.Size = new System.Drawing.Size(59, 20);
            this.rbDepartment.TabIndex = 0;
            this.rbDepartment.TabStop = true;
            this.rbDepartment.Text = "부서";
            this.rbDepartment.UseVisualStyleBackColor = true;
            this.rbDepartment.CheckedChanged += new System.EventHandler(this.rbDepartment_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvAuthority);
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Controls.Add(this.panel8);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1161, 752);
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
            this.group_name,
            this.form_name,
            this.isVisible,
            this.isAdd,
            this.isUpdate,
            this.isDelete,
            this.isExcel,
            this.isPrint,
            this.isAdmin});
            this.dgvAuthority.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAuthority.EnableHeadersVisualStyles = false;
            this.dgvAuthority.Location = new System.Drawing.Point(327, 30);
            this.dgvAuthority.Name = "dgvAuthority";
            this.dgvAuthority.RowTemplate.Height = 23;
            this.dgvAuthority.Size = new System.Drawing.Size(834, 722);
            this.dgvAuthority.TabIndex = 3;
            this.dgvAuthority.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAuthority_CellMouseClick);
            this.dgvAuthority.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAuthority_CellMouseDoubleClick);
            this.dgvAuthority.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvAuthority_CellPainting);
            this.dgvAuthority.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvAuthority_MouseUp);
            // 
            // group_name
            // 
            this.group_name.HeaderText = "그룹";
            this.group_name.Name = "group_name";
            // 
            // form_name
            // 
            this.form_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.form_name.HeaderText = "페이지";
            this.form_name.Name = "form_name";
            // 
            // isVisible
            // 
            this.isVisible.HeaderText = "사용";
            this.isVisible.Name = "isVisible";
            this.isVisible.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isVisible.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isVisible.Width = 70;
            // 
            // isAdd
            // 
            this.isAdd.HeaderText = "등록";
            this.isAdd.Name = "isAdd";
            this.isAdd.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isAdd.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isAdd.Width = 70;
            // 
            // isUpdate
            // 
            this.isUpdate.HeaderText = "수정";
            this.isUpdate.Name = "isUpdate";
            this.isUpdate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isUpdate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isUpdate.Width = 70;
            // 
            // isDelete
            // 
            this.isDelete.HeaderText = "삭제";
            this.isDelete.Name = "isDelete";
            this.isDelete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isDelete.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isDelete.Width = 70;
            // 
            // isExcel
            // 
            this.isExcel.HeaderText = "엑셀";
            this.isExcel.Name = "isExcel";
            this.isExcel.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isExcel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isExcel.Width = 70;
            // 
            // isPrint
            // 
            this.isPrint.HeaderText = "인쇄";
            this.isPrint.Name = "isPrint";
            this.isPrint.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isPrint.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isPrint.Width = 70;
            // 
            // isAdmin
            // 
            this.isAdmin.HeaderText = "관리자";
            this.isAdmin.Name = "isAdmin";
            this.isAdmin.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isAdmin.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isAdmin.Width = 70;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.cbAuthorityTemplate);
            this.panel7.Controls.Add(this.label4);
            this.panel7.Controls.Add(this.txtUpdateTarget);
            this.panel7.Controls.Add(this.label2);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(327, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(834, 30);
            this.panel7.TabIndex = 2;
            // 
            // cbAuthorityTemplate
            // 
            this.cbAuthorityTemplate.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbAuthorityTemplate.FormattingEnabled = true;
            this.cbAuthorityTemplate.Location = new System.Drawing.Point(633, 4);
            this.cbAuthorityTemplate.Name = "cbAuthorityTemplate";
            this.cbAuthorityTemplate.Size = new System.Drawing.Size(198, 24);
            this.cbAuthorityTemplate.TabIndex = 5;
            this.cbAuthorityTemplate.SelectedIndexChanged += new System.EventHandler(this.cbAuthorityTemplate_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(507, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "*권한 불러오기";
            // 
            // txtUpdateTarget
            // 
            this.txtUpdateTarget.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtUpdateTarget.Location = new System.Drawing.Point(114, 2);
            this.txtUpdateTarget.Name = "txtUpdateTarget";
            this.txtUpdateTarget.Size = new System.Drawing.Size(239, 26);
            this.txtUpdateTarget.TabIndex = 3;
            this.txtUpdateTarget.Text = " ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "*선택한 대상";
            // 
            // panel8
            // 
            this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel8.Location = new System.Drawing.Point(307, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(20, 752);
            this.panel8.TabIndex = 4;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.panel1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(307, 752);
            this.panel4.TabIndex = 0;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.dgvAuthorityTarget);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 52);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(307, 700);
            this.panel6.TabIndex = 2;
            // 
            // dgvAuthorityTarget
            // 
            this.dgvAuthorityTarget.AllowUserToAddRows = false;
            this.dgvAuthorityTarget.AllowUserToDeleteRows = false;
            this.dgvAuthorityTarget.AllowUserToResizeColumns = false;
            this.dgvAuthorityTarget.AllowUserToResizeRows = false;
            this.dgvAuthorityTarget.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvAuthorityTarget.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chk,
            this.department,
            this.user_id,
            this.user_name,
            this.is_registration});
            this.dgvAuthorityTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAuthorityTarget.EnableHeadersVisualStyles = false;
            this.dgvAuthorityTarget.Location = new System.Drawing.Point(0, 0);
            this.dgvAuthorityTarget.Name = "dgvAuthorityTarget";
            this.dgvAuthorityTarget.RowHeadersWidth = 20;
            this.dgvAuthorityTarget.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvAuthorityTarget.RowTemplate.Height = 23;
            this.dgvAuthorityTarget.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAuthorityTarget.Size = new System.Drawing.Size(307, 700);
            this.dgvAuthorityTarget.TabIndex = 0;
            this.dgvAuthorityTarget.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAuthorityTarget_CellMouseClick);
            this.dgvAuthorityTarget.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAuthorityTarget_CellMouseDoubleClick);
            this.dgvAuthorityTarget.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvAuthorityTarget_CellPainting);
            this.dgvAuthorityTarget.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAuthorityTarget_CellValueChanged);
            this.dgvAuthorityTarget.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvAuthorityTarget_MouseUp);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label1);
            this.panel5.Controls.Add(this.txtUserName);
            this.panel5.Controls.Add(this.txtDepartment);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 30);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(307, 22);
            this.panel5.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(30, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "*";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(177, 0);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(130, 21);
            this.txtUserName.TabIndex = 1;
            this.txtUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDepartment_KeyDown);
            // 
            // txtDepartment
            // 
            this.txtDepartment.Location = new System.Drawing.Point(49, 0);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(130, 21);
            this.txtDepartment.TabIndex = 0;
            this.txtDepartment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDepartment_KeyDown);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnAuthorityRefresh);
            this.panel3.Controls.Add(this.lbUsername);
            this.panel3.Controls.Add(this.lbDepartment);
            this.panel3.Controls.Add(this.lbUserid);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.btnRegistration);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 752);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1161, 36);
            this.panel3.TabIndex = 2;
            // 
            // lbUsername
            // 
            this.lbUsername.AutoSize = true;
            this.lbUsername.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbUsername.Location = new System.Drawing.Point(1100, 11);
            this.lbUsername.Name = "lbUsername";
            this.lbUsername.Size = new System.Drawing.Size(51, 16);
            this.lbUsername.TabIndex = 83;
            this.lbUsername.Text = "NULL";
            this.lbUsername.Visible = false;
            // 
            // lbDepartment
            // 
            this.lbDepartment.AutoSize = true;
            this.lbDepartment.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbDepartment.Location = new System.Drawing.Point(1043, 11);
            this.lbDepartment.Name = "lbDepartment";
            this.lbDepartment.Size = new System.Drawing.Size(51, 16);
            this.lbDepartment.TabIndex = 82;
            this.lbDepartment.Text = "NULL";
            this.lbDepartment.Visible = false;
            // 
            // lbUserid
            // 
            this.lbUserid.AutoSize = true;
            this.lbUserid.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbUserid.Location = new System.Drawing.Point(986, 11);
            this.lbUserid.Name = "lbUserid";
            this.lbUserid.Size = new System.Drawing.Size(51, 16);
            this.lbUserid.TabIndex = 81;
            this.lbUserid.Text = "NULL";
            this.lbUserid.Visible = false;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Location = new System.Drawing.Point(3, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(64, 31);
            this.btnSearch.TabIndex = 80;
            this.btnSearch.Text = "검색(Q)";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnRegistration
            // 
            this.btnRegistration.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRegistration.ForeColor = System.Drawing.Color.Blue;
            this.btnRegistration.Location = new System.Drawing.Point(73, 3);
            this.btnRegistration.Name = "btnRegistration";
            this.btnRegistration.Size = new System.Drawing.Size(64, 31);
            this.btnRegistration.TabIndex = 78;
            this.btnRegistration.Text = "저장(A)";
            this.btnRegistration.UseVisualStyleBackColor = true;
            this.btnRegistration.Click += new System.EventHandler(this.btnRegistration_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(236, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(64, 31);
            this.btnExit.TabIndex = 79;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnAuthorityRefresh
            // 
            this.btnAuthorityRefresh.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAuthorityRefresh.ForeColor = System.Drawing.Color.Red;
            this.btnAuthorityRefresh.Location = new System.Drawing.Point(143, 3);
            this.btnAuthorityRefresh.Name = "btnAuthorityRefresh";
            this.btnAuthorityRefresh.Size = new System.Drawing.Size(87, 31);
            this.btnAuthorityRefresh.TabIndex = 84;
            this.btnAuthorityRefresh.Text = "권한 초기화";
            this.btnAuthorityRefresh.UseVisualStyleBackColor = true;
            this.btnAuthorityRefresh.Click += new System.EventHandler(this.btnAuthorityRefresh_Click);
            // 
            // chk
            // 
            this.chk.HeaderText = "";
            this.chk.Name = "chk";
            this.chk.Width = 30;
            // 
            // department
            // 
            this.department.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.department.HeaderText = "부서명";
            this.department.Name = "department";
            // 
            // user_id
            // 
            this.user_id.HeaderText = "user_id";
            this.user_id.Name = "user_id";
            this.user_id.Visible = false;
            // 
            // user_name
            // 
            this.user_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.user_name.HeaderText = "이름";
            this.user_name.Name = "user_name";
            // 
            // is_registration
            // 
            this.is_registration.HeaderText = "등록";
            this.is_registration.Name = "is_registration";
            this.is_registration.Width = 50;
            // 
            // AuthorityManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1161, 788);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "AuthorityManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "권한설정";
            this.Load += new System.EventHandler(this.AuthorityManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AuthorityManager_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuthority)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuthorityTarget)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbUsers;
        private System.Windows.Forms.RadioButton rbDepartment;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel6;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvAuthorityTarget;
        private System.Windows.Forms.Panel panel5;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvAuthority;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.TextBox txtUpdateTarget;
        private System.Windows.Forms.ComboBox cbAuthorityTemplate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnRegistration;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lbUsername;
        private System.Windows.Forms.Label lbDepartment;
        private System.Windows.Forms.Label lbUserid;
        private System.Windows.Forms.DataGridViewTextBoxColumn group_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn form_name;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isVisible;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isAdd;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isUpdate;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isDelete;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isExcel;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isPrint;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isAdmin;
        private System.Windows.Forms.Button btnAuthorityRefresh;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn department;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_name;
        private System.Windows.Forms.DataGridViewCheckBoxColumn is_registration;
    }
}