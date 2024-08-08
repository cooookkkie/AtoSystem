namespace AdoNetWindow.SaleManagement
{
    partial class TodaySalesDashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TodaySalesDashboard));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbDepartment = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbYear = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvSales = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.sYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sMonth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sWeek = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.edate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.work_days = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearching = new System.Windows.Forms.Button();
            this.year = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.month = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.week = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sttdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.enddate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbDepartment);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtManager);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbYear);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1592, 28);
            this.panel1.TabIndex = 0;
            // 
            // cbDepartment
            // 
            this.cbDepartment.FormattingEnabled = true;
            this.cbDepartment.Location = new System.Drawing.Point(177, 4);
            this.cbDepartment.Name = "cbDepartment";
            this.cbDepartment.Size = new System.Drawing.Size(134, 20);
            this.cbDepartment.TabIndex = 1;
            this.cbDepartment.Text = "영업부(대리)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(142, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 23;
            this.label3.Text = "부서";
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(364, 4);
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(140, 21);
            this.txtManager.TabIndex = 22;
            this.txtManager.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtManager_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(317, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 21;
            this.label2.Text = "담당자";
            // 
            // cbYear
            // 
            this.cbYear.FormattingEnabled = true;
            this.cbYear.Location = new System.Drawing.Point(65, 4);
            this.cbYear.Name = "cbYear";
            this.cbYear.Size = new System.Drawing.Size(71, 20);
            this.cbYear.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "기준년도";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvSales);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1592, 957);
            this.panel2.TabIndex = 1;
            // 
            // dgvSales
            // 
            this.dgvSales.AllowUserToAddRows = false;
            this.dgvSales.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSales.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSales.ColumnHeadersHeight = 46;
            this.dgvSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSales.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sYear,
            this.sMonth,
            this.sWeek,
            this.sdate,
            this.edate,
            this.work_days});
            this.dgvSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSales.EnableHeadersVisualStyles = false;
            this.dgvSales.Location = new System.Drawing.Point(0, 0);
            this.dgvSales.Name = "dgvSales";
            this.dgvSales.RowTemplate.Height = 23;
            this.dgvSales.Size = new System.Drawing.Size(1592, 957);
            this.dgvSales.TabIndex = 3;
            this.dgvSales.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvSales_CellPainting);
            this.dgvSales.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvSales_ColumnWidthChanged);
            this.dgvSales.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvSales_Scroll);
            this.dgvSales.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvSales_Paint);
            // 
            // sYear
            // 
            this.sYear.HeaderText = "연도";
            this.sYear.Name = "sYear";
            this.sYear.Width = 40;
            // 
            // sMonth
            // 
            this.sMonth.HeaderText = "월";
            this.sMonth.Name = "sMonth";
            this.sMonth.Width = 30;
            // 
            // sWeek
            // 
            this.sWeek.HeaderText = "주차";
            this.sWeek.Name = "sWeek";
            this.sWeek.Visible = false;
            this.sWeek.Width = 40;
            // 
            // sdate
            // 
            this.sdate.HeaderText = "시작일";
            this.sdate.Name = "sdate";
            this.sdate.Width = 70;
            // 
            // edate
            // 
            this.edate.HeaderText = "종료일";
            this.edate.Name = "edate";
            this.edate.Width = 70;
            // 
            // work_days
            // 
            this.work_days.HeaderText = "일수";
            this.work_days.Name = "work_days";
            this.work_days.Width = 40;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnSearching);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 985);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1592, 39);
            this.panel3.TabIndex = 1;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(75, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.ForeColor = System.Drawing.Color.Black;
            this.btnSearching.Location = new System.Drawing.Point(3, 1);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(66, 37);
            this.btnSearching.TabIndex = 14;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // year
            // 
            this.year.HeaderText = "년도";
            this.year.Name = "year";
            this.year.Width = 50;
            // 
            // month
            // 
            this.month.HeaderText = "월";
            this.month.Name = "month";
            this.month.Width = 40;
            // 
            // week
            // 
            this.week.HeaderText = "주차";
            this.week.Name = "week";
            this.week.Width = 50;
            // 
            // sttdate
            // 
            this.sttdate.HeaderText = "시작일";
            this.sttdate.Name = "sttdate";
            this.sttdate.Width = 70;
            // 
            // enddate
            // 
            this.enddate.HeaderText = "종료일";
            this.enddate.Name = "enddate";
            this.enddate.Width = 70;
            // 
            // TodaySalesDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1592, 1024);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "TodaySalesDashboard";
            this.Text = "영업 대시보드";
            this.Load += new System.EventHandler(this.TodaySalesDashboard_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TodaySalesDashboard_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cbYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn year;
        private System.Windows.Forms.DataGridViewTextBoxColumn month;
        private System.Windows.Forms.DataGridViewTextBoxColumn week;
        private System.Windows.Forms.DataGridViewTextBoxColumn sttdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn enddate;
        private System.Windows.Forms.Button btnSearching;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvSales;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ComboBox cbDepartment;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn sYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn sMonth;
        private System.Windows.Forms.DataGridViewTextBoxColumn sWeek;
        private System.Windows.Forms.DataGridViewTextBoxColumn sdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn edate;
        private System.Windows.Forms.DataGridViewTextBoxColumn work_days;
    }
}