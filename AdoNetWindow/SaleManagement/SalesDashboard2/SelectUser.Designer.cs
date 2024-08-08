namespace AdoNetWindow.SaleManagement.SalesDashboard2
{
    partial class SelectUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectUser));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtGrade = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTeam = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.dgvSelectManager = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgvManager = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSearching = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.user_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.department = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.team = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_user_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_department = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_team = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_grade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_user_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectManager)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvManager)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtManager);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtGrade);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtTeam);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtDepartment);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(987, 34);
            this.panel1.TabIndex = 0;
            // 
            // txtGrade
            // 
            this.txtGrade.Location = new System.Drawing.Point(406, 6);
            this.txtGrade.Name = "txtGrade";
            this.txtGrade.Size = new System.Drawing.Size(134, 21);
            this.txtGrade.TabIndex = 5;
            this.txtGrade.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDepartment_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(371, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "직급";
            // 
            // txtTeam
            // 
            this.txtTeam.Location = new System.Drawing.Point(223, 6);
            this.txtTeam.Name = "txtTeam";
            this.txtTeam.Size = new System.Drawing.Size(134, 21);
            this.txtTeam.TabIndex = 3;
            this.txtTeam.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDepartment_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(200, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "팀";
            // 
            // txtDepartment
            // 
            this.txtDepartment.Location = new System.Drawing.Point(47, 5);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(134, 21);
            this.txtDepartment.TabIndex = 1;
            this.txtDepartment.Text = "영업";
            this.txtDepartment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDepartment_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "부서";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(987, 556);
            this.panel2.TabIndex = 1;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.dgvSelectManager);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(546, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(441, 556);
            this.panel6.TabIndex = 3;
            // 
            // dgvSelectManager
            // 
            this.dgvSelectManager.AllowUserToAddRows = false;
            this.dgvSelectManager.AllowUserToResizeRows = false;
            this.dgvSelectManager.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelectManager.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.select_user_id,
            this.select_department,
            this.select_team,
            this.select_grade,
            this.select_user_name});
            this.dgvSelectManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSelectManager.EnableHeadersVisualStyles = false;
            this.dgvSelectManager.Location = new System.Drawing.Point(0, 0);
            this.dgvSelectManager.Name = "dgvSelectManager";
            this.dgvSelectManager.RowTemplate.Height = 23;
            this.dgvSelectManager.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelectManager.Size = new System.Drawing.Size(441, 556);
            this.dgvSelectManager.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel11);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(441, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(546, 556);
            this.panel5.TabIndex = 2;
            // 
            // panel11
            // 
            this.panel11.BackgroundImage = global::AdoNetWindow.Properties.Resources.Right_button_icon;
            this.panel11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel11.Location = new System.Drawing.Point(19, 234);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(69, 53);
            this.panel11.TabIndex = 2;
            this.panel11.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel11_MouseClick);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dgvManager);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(441, 556);
            this.panel4.TabIndex = 1;
            // 
            // dgvManager
            // 
            this.dgvManager.AllowUserToAddRows = false;
            this.dgvManager.AllowUserToDeleteRows = false;
            this.dgvManager.AllowUserToResizeRows = false;
            this.dgvManager.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvManager.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.user_id,
            this.department,
            this.team,
            this.grade,
            this.user_name});
            this.dgvManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvManager.EnableHeadersVisualStyles = false;
            this.dgvManager.Location = new System.Drawing.Point(0, 0);
            this.dgvManager.Name = "dgvManager";
            this.dgvManager.RowTemplate.Height = 23;
            this.dgvManager.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvManager.Size = new System.Drawing.Size(441, 556);
            this.dgvManager.TabIndex = 0;
            this.dgvManager.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvManager_CellMouseDoubleClick);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSearching);
            this.panel3.Controls.Add(this.btnInsert);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 590);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(987, 44);
            this.panel3.TabIndex = 2;
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.ForeColor = System.Drawing.Color.Black;
            this.btnSearching.Location = new System.Drawing.Point(3, 3);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(66, 37);
            this.btnSearching.TabIndex = 7;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInsert.ForeColor = System.Drawing.Color.Blue;
            this.btnInsert.Location = new System.Drawing.Point(74, 3);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(107, 37);
            this.btnInsert.TabIndex = 5;
            this.btnInsert.Text = "담당자 추가(A)";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(186, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // user_id
            // 
            this.user_id.HeaderText = "user_id";
            this.user_id.Name = "user_id";
            this.user_id.Visible = false;
            // 
            // department
            // 
            this.department.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.department.HeaderText = "부서";
            this.department.Name = "department";
            // 
            // team
            // 
            this.team.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.team.HeaderText = "팀";
            this.team.Name = "team";
            // 
            // grade
            // 
            this.grade.HeaderText = "직급";
            this.grade.Name = "grade";
            // 
            // user_name
            // 
            this.user_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.user_name.HeaderText = "담당자";
            this.user_name.Name = "user_name";
            // 
            // select_user_id
            // 
            this.select_user_id.HeaderText = "user_id";
            this.select_user_id.Name = "select_user_id";
            this.select_user_id.Visible = false;
            // 
            // select_department
            // 
            this.select_department.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.select_department.HeaderText = "부서";
            this.select_department.Name = "select_department";
            // 
            // select_team
            // 
            this.select_team.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.select_team.HeaderText = "팀";
            this.select_team.Name = "select_team";
            // 
            // select_grade
            // 
            this.select_grade.HeaderText = "직급";
            this.select_grade.Name = "select_grade";
            // 
            // select_user_name
            // 
            this.select_user_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.select_user_name.HeaderText = "담당자";
            this.select_user_name.Name = "select_user_name";
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(593, 6);
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(134, 21);
            this.txtManager.TabIndex = 7;
            this.txtManager.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDepartment_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(546, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "담당자";
            // 
            // SelectUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 634);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "담당자 선택";
            this.Load += new System.EventHandler(this.SelectUser_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SelectUser_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectManager)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvManager)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtTeam;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtGrade;
        private System.Windows.Forms.Label label3;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvManager;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvSelectManager;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_user_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_department;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_team;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_grade;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_user_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn department;
        private System.Windows.Forms.DataGridViewTextBoxColumn team;
        private System.Windows.Forms.DataGridViewTextBoxColumn grade;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_name;
    }
}