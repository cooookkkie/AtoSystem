namespace AdoNetWindow.Config
{
    partial class AdminConfigManager
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminConfigManager));
            this.btnDepartmentAdd = new System.Windows.Forms.Button();
            this.txtAuthLevel = new System.Windows.Forms.TextBox();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgvDepartment = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.department_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.department_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.auth_level = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnUpdate1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtWorkplace = new System.Windows.Forms.TextBox();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGrade = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtUserDepartment = new System.Windows.Forms.TextBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.dgvUsers = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.btnAuthorityManager = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.user_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.workplace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.department = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.team = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user_in_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user_out_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.seaover_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.excel_password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnUpdate2 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTeam = new System.Windows.Forms.TextBox();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDepartment)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDepartmentAdd
            // 
            this.btnDepartmentAdd.Location = new System.Drawing.Point(271, 5);
            this.btnDepartmentAdd.Name = "btnDepartmentAdd";
            this.btnDepartmentAdd.Size = new System.Drawing.Size(41, 23);
            this.btnDepartmentAdd.TabIndex = 4;
            this.btnDepartmentAdd.Text = "등록";
            this.btnDepartmentAdd.UseVisualStyleBackColor = true;
            this.btnDepartmentAdd.Click += new System.EventHandler(this.btnDepartmentAdd_Click);
            // 
            // txtAuthLevel
            // 
            this.txtAuthLevel.Location = new System.Drawing.Point(217, 5);
            this.txtAuthLevel.Name = "txtAuthLevel";
            this.txtAuthLevel.Size = new System.Drawing.Size(50, 21);
            this.txtAuthLevel.TabIndex = 3;
            // 
            // txtDepartment
            // 
            this.txtDepartment.Location = new System.Drawing.Point(3, 5);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(208, 21);
            this.txtDepartment.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnDepartmentAdd);
            this.panel4.Controls.Add(this.txtAuthLevel);
            this.panel4.Controls.Add(this.txtDepartment);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(317, 31);
            this.panel4.TabIndex = 0;
            // 
            // dgvDepartment
            // 
            this.dgvDepartment.AllowUserToAddRows = false;
            this.dgvDepartment.AllowUserToDeleteRows = false;
            this.dgvDepartment.AllowUserToOrderColumns = true;
            this.dgvDepartment.AllowUserToResizeColumns = false;
            this.dgvDepartment.AllowUserToResizeRows = false;
            this.dgvDepartment.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.department_id,
            this.department_name,
            this.auth_level,
            this.btnUpdate1});
            this.dgvDepartment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDepartment.EnableHeadersVisualStyles = false;
            this.dgvDepartment.Location = new System.Drawing.Point(0, 0);
            this.dgvDepartment.Name = "dgvDepartment";
            this.dgvDepartment.RowHeadersVisible = false;
            this.dgvDepartment.RowHeadersWidth = 10;
            this.dgvDepartment.RowTemplate.Height = 23;
            this.dgvDepartment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvDepartment.Size = new System.Drawing.Size(317, 483);
            this.dgvDepartment.TabIndex = 0;
            this.dgvDepartment.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDepartment_CellMouseClick);
            // 
            // department_id
            // 
            this.department_id.HeaderText = "id";
            this.department_id.Name = "department_id";
            this.department_id.Visible = false;
            // 
            // department_name
            // 
            this.department_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.department_name.HeaderText = "부서";
            this.department_name.Name = "department_name";
            // 
            // auth_level
            // 
            this.auth_level.HeaderText = "권한";
            this.auth_level.Name = "auth_level";
            this.auth_level.Width = 50;
            // 
            // btnUpdate1
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "수정";
            this.btnUpdate1.DefaultCellStyle = dataGridViewCellStyle1;
            this.btnUpdate1.HeaderText = "";
            this.btnUpdate1.Name = "btnUpdate1";
            this.btnUpdate1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnUpdate1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.btnUpdate1.Width = 50;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.dgvDepartment);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 31);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(317, 483);
            this.panel5.TabIndex = 1;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label6);
            this.panel8.Controls.Add(this.txtTeam);
            this.panel8.Controls.Add(this.label5);
            this.panel8.Controls.Add(this.txtWorkplace);
            this.panel8.Controls.Add(this.cbStatus);
            this.panel8.Controls.Add(this.label4);
            this.panel8.Controls.Add(this.label3);
            this.panel8.Controls.Add(this.label2);
            this.panel8.Controls.Add(this.label1);
            this.panel8.Controls.Add(this.txtGrade);
            this.panel8.Controls.Add(this.txtUserName);
            this.panel8.Controls.Add(this.txtUserDepartment);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(1090, 31);
            this.panel8.TabIndex = 0;
            this.panel8.Paint += new System.Windows.Forms.PaintEventHandler(this.panel8_Paint);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "사업장";
            // 
            // txtWorkplace
            // 
            this.txtWorkplace.Location = new System.Drawing.Point(56, 5);
            this.txtWorkplace.Name = "txtWorkplace";
            this.txtWorkplace.Size = new System.Drawing.Size(98, 21);
            this.txtWorkplace.TabIndex = 0;
            this.txtWorkplace.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserDepartment_KeyDown);
            // 
            // cbStatus
            // 
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Items.AddRange(new object[] {
            "전체",
            "승인",
            "대기",
            "퇴사",
            "삭제"});
            this.cbStatus.Location = new System.Drawing.Point(749, 5);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(56, 20);
            this.cbStatus.TabIndex = 8;
            this.cbStatus.Text = "대기";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(575, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "직급";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(714, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "승인";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(436, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "이름";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(162, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "부서";
            // 
            // txtGrade
            // 
            this.txtGrade.Location = new System.Drawing.Point(610, 5);
            this.txtGrade.Name = "txtGrade";
            this.txtGrade.Size = new System.Drawing.Size(98, 21);
            this.txtGrade.TabIndex = 6;
            this.txtGrade.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserDepartment_KeyDown);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(471, 5);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(98, 21);
            this.txtUserName.TabIndex = 4;
            this.txtUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserDepartment_KeyDown);
            // 
            // txtUserDepartment
            // 
            this.txtUserDepartment.Location = new System.Drawing.Point(197, 5);
            this.txtUserDepartment.Name = "txtUserDepartment";
            this.txtUserDepartment.Size = new System.Drawing.Size(98, 21);
            this.txtUserDepartment.TabIndex = 2;
            this.txtUserDepartment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserDepartment_KeyDown);
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelect.Location = new System.Drawing.Point(3, 3);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(70, 36);
            this.btnSelect.TabIndex = 16;
            this.btnSelect.Text = "검색(Q)";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.dgvUsers);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 31);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1090, 439);
            this.panel7.TabIndex = 1;
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AllowUserToOrderColumns = true;
            this.dgvUsers.AllowUserToResizeColumns = false;
            this.dgvUsers.AllowUserToResizeRows = false;
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.user_id,
            this.workplace,
            this.department,
            this.team,
            this.user_name,
            this.user_in_date,
            this.user_out_date,
            this.grade,
            this.tel,
            this.status,
            this.seaover_id,
            this.excel_password,
            this.btnUpdate2});
            this.dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUsers.EnableHeadersVisualStyles = false;
            this.dgvUsers.Location = new System.Drawing.Point(0, 0);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.RowHeadersWidth = 10;
            this.dgvUsers.RowTemplate.Height = 23;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvUsers.Size = new System.Drawing.Size(1090, 439);
            this.dgvUsers.TabIndex = 0;
            this.dgvUsers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellContentClick);
            this.dgvUsers.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellEnter);
            this.dgvUsers.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvUsers_CurrentCellDirtyStateChanged);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel7);
            this.panel6.Controls.Add(this.panel3);
            this.panel6.Controls.Add(this.panel8);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(317, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1090, 514);
            this.panel6.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel9);
            this.panel3.Controls.Add(this.btnUpdate);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnSelect);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 470);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1090, 44);
            this.panel3.TabIndex = 2;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.btnAuthorityManager);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel9.Location = new System.Drawing.Point(907, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(183, 44);
            this.panel9.TabIndex = 20;
            // 
            // btnAuthorityManager
            // 
            this.btnAuthorityManager.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAuthorityManager.Location = new System.Drawing.Point(90, 3);
            this.btnAuthorityManager.Name = "btnAuthorityManager";
            this.btnAuthorityManager.Size = new System.Drawing.Size(90, 36);
            this.btnAuthorityManager.TabIndex = 19;
            this.btnAuthorityManager.Text = "권한 관리";
            this.btnAuthorityManager.UseVisualStyleBackColor = true;
            this.btnAuthorityManager.Click += new System.EventHandler(this.btnAuthorityManager_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUpdate.ForeColor = System.Drawing.Color.Blue;
            this.btnUpdate.Location = new System.Drawing.Point(79, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(90, 36);
            this.btnUpdate.TabIndex = 17;
            this.btnUpdate.Text = "일괄수정(A)";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(175, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 36);
            this.btnExit.TabIndex = 18;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1407, 514);
            this.panel2.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(317, 514);
            this.panel1.TabIndex = 0;
            // 
            // user_id
            // 
            this.user_id.HeaderText = "ID";
            this.user_id.Name = "user_id";
            this.user_id.Width = 80;
            // 
            // workplace
            // 
            this.workplace.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.workplace.HeaderText = "사업장";
            this.workplace.Name = "workplace";
            // 
            // department
            // 
            this.department.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.department.HeaderText = "부서";
            this.department.Name = "department";
            this.department.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // team
            // 
            this.team.HeaderText = "팀";
            this.team.Name = "team";
            // 
            // user_name
            // 
            this.user_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.user_name.HeaderText = "이름";
            this.user_name.Name = "user_name";
            // 
            // user_in_date
            // 
            this.user_in_date.HeaderText = "입사일";
            this.user_in_date.Name = "user_in_date";
            this.user_in_date.Width = 70;
            // 
            // user_out_date
            // 
            this.user_out_date.HeaderText = "퇴사일";
            this.user_out_date.Name = "user_out_date";
            this.user_out_date.Width = 70;
            // 
            // grade
            // 
            this.grade.HeaderText = "직급";
            this.grade.Name = "grade";
            this.grade.Width = 50;
            // 
            // tel
            // 
            this.tel.HeaderText = "TEL";
            this.tel.Name = "tel";
            // 
            // status
            // 
            this.status.HeaderText = "승인";
            this.status.Name = "status";
            this.status.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.status.Width = 50;
            // 
            // seaover_id
            // 
            this.seaover_id.HeaderText = "S사번";
            this.seaover_id.Name = "seaover_id";
            this.seaover_id.Width = 80;
            // 
            // excel_password
            // 
            this.excel_password.HeaderText = "Excel PW";
            this.excel_password.Name = "excel_password";
            // 
            // btnUpdate2
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "수정";
            this.btnUpdate2.DefaultCellStyle = dataGridViewCellStyle2;
            this.btnUpdate2.HeaderText = "";
            this.btnUpdate2.Name = "btnUpdate2";
            this.btnUpdate2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnUpdate2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.btnUpdate2.Width = 50;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(305, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 19;
            this.label6.Text = "팀";
            // 
            // txtTeam
            // 
            this.txtTeam.Location = new System.Drawing.Point(328, 5);
            this.txtTeam.Name = "txtTeam";
            this.txtTeam.Size = new System.Drawing.Size(98, 21);
            this.txtTeam.TabIndex = 3;
            this.txtTeam.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserDepartment_KeyDown);
            // 
            // AdminConfigManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1407, 514);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdminConfigManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "관리자 설정";
            this.Load += new System.EventHandler(this.AdminConfigManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AdminConfigManager_KeyDown);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDepartment)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnDepartmentAdd;
        private System.Windows.Forms.TextBox txtAuthLevel;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.Panel panel4;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvDepartment;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel7;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvUsers;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtGrade;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtUserDepartment;
        private System.Windows.Forms.DataGridViewTextBoxColumn department_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn department_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn auth_level;
        private System.Windows.Forms.DataGridViewButtonColumn btnUpdate1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtWorkplace;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnAuthorityManager;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn workplace;
        private System.Windows.Forms.DataGridViewTextBoxColumn department;
        private System.Windows.Forms.DataGridViewTextBoxColumn team;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_in_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_out_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn grade;
        private System.Windows.Forms.DataGridViewTextBoxColumn tel;
        private System.Windows.Forms.DataGridViewComboBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn excel_password;
        private System.Windows.Forms.DataGridViewButtonColumn btnUpdate2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTeam;
    }
}