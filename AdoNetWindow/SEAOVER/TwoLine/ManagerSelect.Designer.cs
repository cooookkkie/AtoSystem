namespace AdoNetWindow.SEAOVER.TwoLine
{
    partial class ManagerSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagerSelect));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtRemark = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvManager = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTel = new System.Windows.Forms.TextBox();
            this.txtGrade = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.department = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvManager)).BeginInit();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(333, 450);
            this.panel1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnRefresh);
            this.panel4.Controls.Add(this.btnExit);
            this.panel4.Controls.Add(this.btnSelect);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 400);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(333, 50);
            this.panel4.TabIndex = 1;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRefresh.Location = new System.Drawing.Point(82, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 40);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "초기화(F5)";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(257, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 40);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelect.Location = new System.Drawing.Point(6, 4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(70, 40);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.Text = "확인(A)";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtRemark);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(333, 450);
            this.panel3.TabIndex = 0;
            // 
            // txtRemark
            // 
            this.txtRemark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemark.Location = new System.Drawing.Point(0, 0);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(333, 450);
            this.txtRemark.TabIndex = 0;
            this.txtRemark.Text = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvManager);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(333, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(598, 450);
            this.panel2.TabIndex = 1;
            // 
            // dgvManager
            // 
            this.dgvManager.AllowUserToAddRows = false;
            this.dgvManager.AllowUserToDeleteRows = false;
            this.dgvManager.AllowUserToResizeColumns = false;
            this.dgvManager.AllowUserToResizeRows = false;
            this.dgvManager.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvManager.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chk,
            this.department,
            this.user_name,
            this.grade,
            this.tel});
            this.dgvManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvManager.EnableHeadersVisualStyles = false;
            this.dgvManager.Location = new System.Drawing.Point(0, 22);
            this.dgvManager.Name = "dgvManager";
            this.dgvManager.RowTemplate.Height = 23;
            this.dgvManager.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvManager.Size = new System.Drawing.Size(598, 428);
            this.dgvManager.TabIndex = 0;
            this.dgvManager.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvManager_CellMouseDoubleClick);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.txtDepartment);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Controls.Add(this.txtTel);
            this.panel5.Controls.Add(this.txtGrade);
            this.panel5.Controls.Add(this.txtUsername);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(598, 22);
            this.panel5.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(21, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "* 검색";
            // 
            // txtTel
            // 
            this.txtTel.Location = new System.Drawing.Point(464, 0);
            this.txtTel.Name = "txtTel";
            this.txtTel.Size = new System.Drawing.Size(133, 21);
            this.txtTel.TabIndex = 3;
            this.txtTel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUsername_KeyDown);
            // 
            // txtGrade
            // 
            this.txtGrade.Location = new System.Drawing.Point(332, 0);
            this.txtGrade.Name = "txtGrade";
            this.txtGrade.Size = new System.Drawing.Size(133, 21);
            this.txtGrade.TabIndex = 2;
            this.txtGrade.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUsername_KeyDown);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(200, 0);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(133, 21);
            this.txtUsername.TabIndex = 1;
            this.txtUsername.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUsername_KeyDown);
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
            this.department.HeaderText = "부서";
            this.department.Name = "department";
            // 
            // user_name
            // 
            this.user_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.user_name.HeaderText = "이름";
            this.user_name.Name = "user_name";
            // 
            // grade
            // 
            this.grade.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.grade.HeaderText = "직급";
            this.grade.Name = "grade";
            // 
            // tel
            // 
            this.tel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tel.HeaderText = "TEL";
            this.tel.Name = "tel";
            // 
            // txtDepartment
            // 
            this.txtDepartment.Location = new System.Drawing.Point(68, 0);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(133, 21);
            this.txtDepartment.TabIndex = 0;
            this.txtDepartment.Text = "영업";
            this.txtDepartment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUsername_KeyDown);
            // 
            // ManagerSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManagerSelect";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ManagerSelect_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvManager)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RichTextBox txtRemark;
        private System.Windows.Forms.Panel panel2;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvManager;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTel;
        private System.Windows.Forms.TextBox txtGrade;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn department;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn grade;
        private System.Windows.Forms.DataGridViewTextBoxColumn tel;
        private System.Windows.Forms.TextBox txtDepartment;
    }
}