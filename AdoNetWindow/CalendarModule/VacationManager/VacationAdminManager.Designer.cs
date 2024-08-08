namespace AdoNetWindow.CalendarModule.VacationManager
{
    partial class VacationAdminManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VacationAdminManager));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgvAnnual = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.annual_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.annual_user_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.annual_user_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.annual_department = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.annual_division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sttdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.empty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.enddate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.annual_use_days = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbDivision = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.dgvUserAnnual = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSearching = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.user_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.department = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.used_days = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAnnual)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserAnnual)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1174, 31);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(828, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "* 직원별 남은 연차";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "* 승인 대기목록";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 31);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1174, 502);
            this.panel2.TabIndex = 1;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(819, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(12, 502);
            this.panel6.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.dgvAnnual);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(831, 502);
            this.panel4.TabIndex = 1;
            // 
            // dgvAnnual
            // 
            this.dgvAnnual.AllowUserToAddRows = false;
            this.dgvAnnual.AllowUserToDeleteRows = false;
            this.dgvAnnual.AllowUserToResizeColumns = false;
            this.dgvAnnual.AllowUserToResizeRows = false;
            this.dgvAnnual.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAnnual.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.annual_id,
            this.annual_user_id,
            this.chk,
            this.annual_user_name,
            this.annual_department,
            this.annual_division,
            this.sttdate,
            this.empty,
            this.enddate,
            this.annual_use_days,
            this.remark,
            this.status});
            this.dgvAnnual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAnnual.EnableHeadersVisualStyles = false;
            this.dgvAnnual.Location = new System.Drawing.Point(0, 27);
            this.dgvAnnual.Name = "dgvAnnual";
            this.dgvAnnual.RowTemplate.Height = 23;
            this.dgvAnnual.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAnnual.Size = new System.Drawing.Size(829, 473);
            this.dgvAnnual.TabIndex = 0;
            this.dgvAnnual.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAnnual_CellMouseClick);
            this.dgvAnnual.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvAnnual_MouseUp);
            // 
            // annual_id
            // 
            this.annual_id.HeaderText = "id";
            this.annual_id.Name = "annual_id";
            this.annual_id.Visible = false;
            // 
            // annual_user_id
            // 
            this.annual_user_id.HeaderText = "user_id";
            this.annual_user_id.Name = "annual_user_id";
            this.annual_user_id.Visible = false;
            // 
            // chk
            // 
            this.chk.HeaderText = "";
            this.chk.Name = "chk";
            this.chk.Visible = false;
            this.chk.Width = 30;
            // 
            // annual_user_name
            // 
            this.annual_user_name.HeaderText = "이름";
            this.annual_user_name.Name = "annual_user_name";
            // 
            // annual_department
            // 
            this.annual_department.HeaderText = "부서";
            this.annual_department.Name = "annual_department";
            // 
            // annual_division
            // 
            this.annual_division.HeaderText = "구분";
            this.annual_division.Name = "annual_division";
            this.annual_division.Width = 70;
            // 
            // sttdate
            // 
            this.sttdate.HeaderText = "";
            this.sttdate.Name = "sttdate";
            this.sttdate.Width = 70;
            // 
            // empty
            // 
            this.empty.HeaderText = "~";
            this.empty.Name = "empty";
            this.empty.Width = 30;
            // 
            // enddate
            // 
            this.enddate.HeaderText = "";
            this.enddate.Name = "enddate";
            this.enddate.Width = 70;
            // 
            // annual_use_days
            // 
            this.annual_use_days.HeaderText = "사용일";
            this.annual_use_days.Name = "annual_use_days";
            this.annual_use_days.Width = 70;
            // 
            // remark
            // 
            this.remark.HeaderText = "비고";
            this.remark.Name = "remark";
            this.remark.Width = 150;
            // 
            // status
            // 
            this.status.HeaderText = "상태";
            this.status.Name = "status";
            this.status.Width = 70;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.cbStatus);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.cbDivision);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.txtDepartment);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Controls.Add(this.txtUserName);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(829, 27);
            this.panel5.TabIndex = 1;
            // 
            // cbStatus
            // 
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Items.AddRange(new object[] {
            "전체",
            "승인",
            "대기",
            "반려"});
            this.cbStatus.Location = new System.Drawing.Point(500, 3);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(79, 20);
            this.cbStatus.TabIndex = 7;
            this.cbStatus.Text = "대기";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(465, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "상태";
            // 
            // cbDivision
            // 
            this.cbDivision.FormattingEnabled = true;
            this.cbDivision.Items.AddRange(new object[] {
            "전체",
            "연차",
            "반차",
            "휴가",
            "병가"});
            this.cbDivision.Location = new System.Drawing.Point(377, 3);
            this.cbDivision.Name = "cbDivision";
            this.cbDivision.Size = new System.Drawing.Size(79, 20);
            this.cbDivision.TabIndex = 5;
            this.cbDivision.Text = "전체";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(342, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "구분";
            // 
            // txtDepartment
            // 
            this.txtDepartment.Location = new System.Drawing.Point(210, 3);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(126, 21);
            this.txtDepartment.TabIndex = 3;
            this.txtDepartment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserName_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(175, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "부서";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(43, 3);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(126, 21);
            this.txtUserName.TabIndex = 1;
            this.txtUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserName_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "이름";
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.dgvUserAnnual);
            this.panel7.Controls.Add(this.panel8);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel7.Location = new System.Drawing.Point(831, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(343, 502);
            this.panel7.TabIndex = 3;
            // 
            // dgvUserAnnual
            // 
            this.dgvUserAnnual.AllowUserToAddRows = false;
            this.dgvUserAnnual.AllowUserToDeleteRows = false;
            this.dgvUserAnnual.AllowUserToResizeColumns = false;
            this.dgvUserAnnual.AllowUserToResizeRows = false;
            this.dgvUserAnnual.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUserAnnual.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.user_name,
            this.department,
            this.used_days});
            this.dgvUserAnnual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUserAnnual.EnableHeadersVisualStyles = false;
            this.dgvUserAnnual.Location = new System.Drawing.Point(0, 27);
            this.dgvUserAnnual.Name = "dgvUserAnnual";
            this.dgvUserAnnual.RowTemplate.Height = 23;
            this.dgvUserAnnual.Size = new System.Drawing.Size(341, 473);
            this.dgvUserAnnual.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(341, 27);
            this.panel8.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSearching);
            this.panel3.Controls.Add(this.btnRegister);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 533);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1174, 43);
            this.panel3.TabIndex = 2;
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.Location = new System.Drawing.Point(3, 3);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(68, 37);
            this.btnSearching.TabIndex = 107;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRegister.Location = new System.Drawing.Point(77, 3);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(99, 37);
            this.btnRegister.TabIndex = 105;
            this.btnRegister.Text = "신규등록(A)";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(182, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(68, 37);
            this.btnExit.TabIndex = 106;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
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
            // used_days
            // 
            this.used_days.HeaderText = "사용일자";
            this.used_days.Name = "used_days";
            this.used_days.Width = 80;
            // 
            // VacationAdminManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 576);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "VacationAdminManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "연차관리";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VacationAdminManager_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAnnual)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserAnnual)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel4;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvAnnual;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel7;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvUserAnnual;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbDivision;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn annual_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn annual_user_id;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn annual_user_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn annual_department;
        private System.Windows.Forms.DataGridViewTextBoxColumn annual_division;
        private System.Windows.Forms.DataGridViewTextBoxColumn sttdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn empty;
        private System.Windows.Forms.DataGridViewTextBoxColumn enddate;
        private System.Windows.Forms.DataGridViewTextBoxColumn annual_use_days;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn department;
        private System.Windows.Forms.DataGridViewTextBoxColumn used_days;
    }
}