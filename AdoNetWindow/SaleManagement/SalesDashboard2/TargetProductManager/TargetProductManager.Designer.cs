namespace AdoNetWindow.SaleManagement.SalesDashboard2
{
    partial class TargetProductManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TargetProductManager));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvProject = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSearching = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtProject = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtEnddate = new System.Windows.Forms.TextBox();
            this.btnCalendarEnddate = new System.Windows.Forms.Button();
            this.txtSttdate = new System.Windows.Forms.TextBox();
            this.btnCalendarSttdate = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTeam = new System.Windows.Forms.TextBox();
            this.txtGrade = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.project_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sttdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.enddate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.department = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.team = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.edit_user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.processing_rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProject)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtOrigin);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtProduct);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtGrade);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtTeam);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtDepartment);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtEnddate);
            this.panel1.Controls.Add(this.btnCalendarEnddate);
            this.panel1.Controls.Add(this.txtSttdate);
            this.panel1.Controls.Add(this.btnCalendarSttdate);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtProject);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1408, 30);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvProject);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1408, 815);
            this.panel2.TabIndex = 1;
            // 
            // dgvProject
            // 
            this.dgvProject.AllowUserToAddRows = false;
            this.dgvProject.AllowUserToDeleteRows = false;
            this.dgvProject.AllowUserToResizeRows = false;
            this.dgvProject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProject.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.project_name,
            this.sttdate,
            this.enddate,
            this.department,
            this.team,
            this.grade,
            this.user_name,
            this.product,
            this.edit_user,
            this.updatetime,
            this.processing_rate});
            this.dgvProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProject.EnableHeadersVisualStyles = false;
            this.dgvProject.Location = new System.Drawing.Point(0, 0);
            this.dgvProject.Name = "dgvProject";
            this.dgvProject.RowTemplate.Height = 23;
            this.dgvProject.Size = new System.Drawing.Size(1408, 815);
            this.dgvProject.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSearching);
            this.panel3.Controls.Add(this.btnInsert);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 845);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1408, 44);
            this.panel3.TabIndex = 2;
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.ForeColor = System.Drawing.Color.Black;
            this.btnSearching.Location = new System.Drawing.Point(3, 3);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(66, 37);
            this.btnSearching.TabIndex = 10;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInsert.ForeColor = System.Drawing.Color.Blue;
            this.btnInsert.Location = new System.Drawing.Point(75, 3);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(117, 37);
            this.btnInsert.TabIndex = 8;
            this.btnInsert.Text = "프로젝트 추가(A)";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(198, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(285, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "프로젝트";
            // 
            // txtProject
            // 
            this.txtProject.Location = new System.Drawing.Point(341, 5);
            this.txtProject.Name = "txtProject";
            this.txtProject.Size = new System.Drawing.Size(203, 21);
            this.txtProject.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(161, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(15, 12);
            this.label9.TabIndex = 126;
            this.label9.Text = "~";
            // 
            // txtEnddate
            // 
            this.txtEnddate.Location = new System.Drawing.Point(182, 4);
            this.txtEnddate.Name = "txtEnddate";
            this.txtEnddate.Size = new System.Drawing.Size(67, 21);
            this.txtEnddate.TabIndex = 2;
            // 
            // btnCalendarEnddate
            // 
            this.btnCalendarEnddate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCalendarEnddate.BackgroundImage")));
            this.btnCalendarEnddate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCalendarEnddate.FlatAppearance.BorderSize = 0;
            this.btnCalendarEnddate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalendarEnddate.Location = new System.Drawing.Point(251, 4);
            this.btnCalendarEnddate.Name = "btnCalendarEnddate";
            this.btnCalendarEnddate.Size = new System.Drawing.Size(20, 19);
            this.btnCalendarEnddate.TabIndex = 3;
            this.btnCalendarEnddate.UseVisualStyleBackColor = true;
            // 
            // txtSttdate
            // 
            this.txtSttdate.Location = new System.Drawing.Point(66, 4);
            this.txtSttdate.Name = "txtSttdate";
            this.txtSttdate.Size = new System.Drawing.Size(67, 21);
            this.txtSttdate.TabIndex = 0;
            // 
            // btnCalendarSttdate
            // 
            this.btnCalendarSttdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCalendarSttdate.BackgroundImage")));
            this.btnCalendarSttdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCalendarSttdate.FlatAppearance.BorderSize = 0;
            this.btnCalendarSttdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalendarSttdate.Location = new System.Drawing.Point(135, 4);
            this.btnCalendarSttdate.Name = "btnCalendarSttdate";
            this.btnCalendarSttdate.Size = new System.Drawing.Size(20, 19);
            this.btnCalendarSttdate.TabIndex = 1;
            this.btnCalendarSttdate.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 121;
            this.label4.Text = "검색기간";
            // 
            // txtDepartment
            // 
            this.txtDepartment.Location = new System.Drawing.Point(585, 5);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(126, 21);
            this.txtDepartment.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(550, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 127;
            this.label2.Text = "부서";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(717, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 129;
            this.label3.Text = "팀";
            // 
            // txtTeam
            // 
            this.txtTeam.Location = new System.Drawing.Point(740, 4);
            this.txtTeam.Name = "txtTeam";
            this.txtTeam.Size = new System.Drawing.Size(126, 21);
            this.txtTeam.TabIndex = 9;
            // 
            // txtGrade
            // 
            this.txtGrade.Location = new System.Drawing.Point(907, 4);
            this.txtGrade.Name = "txtGrade";
            this.txtGrade.Size = new System.Drawing.Size(126, 21);
            this.txtGrade.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(872, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 131;
            this.label5.Text = "직급";
            // 
            // txtOrigin
            // 
            this.txtOrigin.Location = new System.Drawing.Point(1257, 4);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.Size = new System.Drawing.Size(126, 21);
            this.txtOrigin.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1210, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 135;
            this.label6.Text = "원산지";
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(1074, 4);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(126, 21);
            this.txtProduct.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1039, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 133;
            this.label7.Text = "품명";
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.Visible = false;
            // 
            // project_name
            // 
            this.project_name.HeaderText = "프로젝트";
            this.project_name.Name = "project_name";
            // 
            // sttdate
            // 
            this.sttdate.HeaderText = "시작일";
            this.sttdate.Name = "sttdate";
            // 
            // enddate
            // 
            this.enddate.HeaderText = "종료일";
            this.enddate.Name = "enddate";
            // 
            // department
            // 
            this.department.HeaderText = "부서";
            this.department.Name = "department";
            // 
            // team
            // 
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
            this.user_name.HeaderText = "담당자";
            this.user_name.Name = "user_name";
            // 
            // product
            // 
            this.product.HeaderText = "품명";
            this.product.Name = "product";
            this.product.Width = 150;
            // 
            // edit_user
            // 
            this.edit_user.HeaderText = "수정자";
            this.edit_user.Name = "edit_user";
            // 
            // updatetime
            // 
            this.updatetime.HeaderText = "수정일자";
            this.updatetime.Name = "updatetime";
            // 
            // processing_rate
            // 
            this.processing_rate.HeaderText = "진행률";
            this.processing_rate.Name = "processing_rate";
            this.processing_rate.Width = 200;
            // 
            // TargetProductManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1408, 889);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TargetProductManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "집중품목 관리";
            this.Load += new System.EventHandler(this.TargetProductManager_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProject)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnExit;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProject;
        private System.Windows.Forms.TextBox txtProject;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtGrade;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTeam;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtEnddate;
        private System.Windows.Forms.Button btnCalendarEnddate;
        private System.Windows.Forms.TextBox txtSttdate;
        private System.Windows.Forms.Button btnCalendarSttdate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn project_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn sttdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn enddate;
        private System.Windows.Forms.DataGridViewTextBoxColumn department;
        private System.Windows.Forms.DataGridViewTextBoxColumn team;
        private System.Windows.Forms.DataGridViewTextBoxColumn grade;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn edit_user;
        private System.Windows.Forms.DataGridViewTextBoxColumn updatetime;
        private System.Windows.Forms.DataGridViewTextBoxColumn processing_rate;
    }
}