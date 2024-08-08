namespace AdoNetWindow.SaleManagement.SalesDashboard2.PojectManager
{
    partial class ProjectManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectManager));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.txtEnddate = new System.Windows.Forms.TextBox();
            this.btnCalendarEnddate = new System.Windows.Forms.Button();
            this.txtSttdate = new System.Windows.Forms.TextBox();
            this.btnCalendarSttdate = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dgvProject = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.btnSearching = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProject)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtEnddate);
            this.panel1.Controls.Add(this.btnCalendarEnddate);
            this.panel1.Controls.Add(this.txtSttdate);
            this.panel1.Controls.Add(this.btnCalendarSttdate);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1096, 31);
            this.panel1.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(166, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(15, 12);
            this.label9.TabIndex = 126;
            this.label9.Text = "~";
            // 
            // txtEnddate
            // 
            this.txtEnddate.Location = new System.Drawing.Point(187, 5);
            this.txtEnddate.Name = "txtEnddate";
            this.txtEnddate.Size = new System.Drawing.Size(67, 21);
            this.txtEnddate.TabIndex = 124;
            // 
            // btnCalendarEnddate
            // 
            this.btnCalendarEnddate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCalendarEnddate.BackgroundImage")));
            this.btnCalendarEnddate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCalendarEnddate.FlatAppearance.BorderSize = 0;
            this.btnCalendarEnddate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalendarEnddate.Location = new System.Drawing.Point(256, 5);
            this.btnCalendarEnddate.Name = "btnCalendarEnddate";
            this.btnCalendarEnddate.Size = new System.Drawing.Size(20, 19);
            this.btnCalendarEnddate.TabIndex = 125;
            this.btnCalendarEnddate.UseVisualStyleBackColor = true;
            // 
            // txtSttdate
            // 
            this.txtSttdate.Location = new System.Drawing.Point(71, 5);
            this.txtSttdate.Name = "txtSttdate";
            this.txtSttdate.Size = new System.Drawing.Size(67, 21);
            this.txtSttdate.TabIndex = 122;
            // 
            // btnCalendarSttdate
            // 
            this.btnCalendarSttdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCalendarSttdate.BackgroundImage")));
            this.btnCalendarSttdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCalendarSttdate.FlatAppearance.BorderSize = 0;
            this.btnCalendarSttdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalendarSttdate.Location = new System.Drawing.Point(140, 5);
            this.btnCalendarSttdate.Name = "btnCalendarSttdate";
            this.btnCalendarSttdate.Size = new System.Drawing.Size(20, 19);
            this.btnCalendarSttdate.TabIndex = 123;
            this.btnCalendarSttdate.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 121;
            this.label4.Text = "검색기간";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvProject);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 31);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1096, 495);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSearching);
            this.panel3.Controls.Add(this.btnInsert);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 526);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1096, 41);
            this.panel3.TabIndex = 2;
            // 
            // dgvProject
            // 
            this.dgvProject.AllowUserToAddRows = false;
            this.dgvProject.AllowUserToDeleteRows = false;
            this.dgvProject.AllowUserToResizeRows = false;
            this.dgvProject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProject.EnableHeadersVisualStyles = false;
            this.dgvProject.Location = new System.Drawing.Point(0, 0);
            this.dgvProject.Name = "dgvProject";
            this.dgvProject.RowTemplate.Height = 23;
            this.dgvProject.Size = new System.Drawing.Size(1096, 495);
            this.dgvProject.TabIndex = 0;
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.ForeColor = System.Drawing.Color.Black;
            this.btnSearching.Location = new System.Drawing.Point(3, 2);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(66, 37);
            this.btnSearching.TabIndex = 10;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            // 
            // btnInsert
            // 
            this.btnInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInsert.ForeColor = System.Drawing.Color.Blue;
            this.btnInsert.Location = new System.Drawing.Point(74, 2);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(74, 37);
            this.btnInsert.TabIndex = 8;
            this.btnInsert.Text = "프로젝트 추가(A)";
            this.btnInsert.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(154, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // ProjectManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1096, 567);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "ProjectManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "집중품목 관리";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProject)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtEnddate;
        private System.Windows.Forms.Button btnCalendarEnddate;
        private System.Windows.Forms.TextBox txtSttdate;
        private System.Windows.Forms.Button btnCalendarSttdate;
        private System.Windows.Forms.Label label4;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProject;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnExit;
    }
}