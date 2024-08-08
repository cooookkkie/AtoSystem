namespace AdoNetWindow.CalendarModule.VacationManager
{
    partial class VacationDashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VacationDashboard));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbWorkplace = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbDepartment = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cbSortType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbYear = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvVacation = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.user_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.in_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accrued_annual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jen_sttdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jen_annual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.feb_sttdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.feb_annual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mar_sttdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mar_annual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.apr_sttdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.apr_annual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.may_sttdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.may_annual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jun_sttdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jun_annual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jul_sttdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jul_annual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aug_sttdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aug_annual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sep_sttdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sep_annual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oct_sttdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oct_annual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nov_sttdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nov_annual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dec_sttdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dec_annual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_annual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leave_annual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.cbRetire = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVacation)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbRetire);
            this.panel1.Controls.Add(this.cbWorkplace);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cbDepartment);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.txtUsername);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbYear);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1894, 29);
            this.panel1.TabIndex = 0;
            // 
            // cbWorkplace
            // 
            this.cbWorkplace.FormattingEnabled = true;
            this.cbWorkplace.Items.AddRange(new object[] {
            "전체",
            "아토무역",
            "에이티오",
            "아토코리아"});
            this.cbWorkplace.Location = new System.Drawing.Point(152, 5);
            this.cbWorkplace.Name = "cbWorkplace";
            this.cbWorkplace.Size = new System.Drawing.Size(96, 20);
            this.cbWorkplace.TabIndex = 2;
            this.cbWorkplace.Text = "전체";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(105, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "사업장";
            // 
            // cbDepartment
            // 
            this.cbDepartment.FormattingEnabled = true;
            this.cbDepartment.Items.AddRange(new object[] {
            "전체",
            "관리",
            "영업",
            "경리",
            "전산",
            "기타"});
            this.cbDepartment.Location = new System.Drawing.Point(289, 5);
            this.cbDepartment.Name = "cbDepartment";
            this.cbDepartment.Size = new System.Drawing.Size(96, 20);
            this.cbDepartment.TabIndex = 6;
            this.cbDepartment.Text = "전체";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(255, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "부서";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.cbSortType);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(1615, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(279, 29);
            this.panel4.TabIndex = 4;
            // 
            // cbSortType
            // 
            this.cbSortType.FormattingEnabled = true;
            this.cbSortType.Items.AddRange(new object[] {
            "입사일",
            "이름",
            "부서+이름"});
            this.cbSortType.Location = new System.Drawing.Point(150, 5);
            this.cbSortType.Name = "cbSortType";
            this.cbSortType.Size = new System.Drawing.Size(121, 20);
            this.cbSortType.TabIndex = 1;
            this.cbSortType.Text = "입사일";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(91, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "정렬방식";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(426, 4);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(182, 21);
            this.txtUsername.TabIndex = 8;
            this.txtUsername.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUsername_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(391, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "이름";
            // 
            // cbYear
            // 
            this.cbYear.FormattingEnabled = true;
            this.cbYear.Location = new System.Drawing.Point(47, 5);
            this.cbYear.Name = "cbYear";
            this.cbYear.Size = new System.Drawing.Size(52, 20);
            this.cbYear.TabIndex = 1;
            this.cbYear.Text = "2023";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "년도";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvVacation);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 29);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1894, 650);
            this.panel2.TabIndex = 1;
            // 
            // dgvVacation
            // 
            this.dgvVacation.AllowUserToAddRows = false;
            this.dgvVacation.AllowUserToDeleteRows = false;
            this.dgvVacation.AllowUserToResizeColumns = false;
            this.dgvVacation.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvVacation.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvVacation.ColumnHeadersHeight = 46;
            this.dgvVacation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvVacation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.user_id,
            this.user_name,
            this.in_date,
            this.accrued_annual,
            this.jen_sttdate,
            this.jen_annual,
            this.feb_sttdate,
            this.feb_annual,
            this.mar_sttdate,
            this.mar_annual,
            this.apr_sttdate,
            this.apr_annual,
            this.may_sttdate,
            this.may_annual,
            this.jun_sttdate,
            this.jun_annual,
            this.jul_sttdate,
            this.jul_annual,
            this.aug_sttdate,
            this.aug_annual,
            this.sep_sttdate,
            this.sep_annual,
            this.oct_sttdate,
            this.oct_annual,
            this.nov_sttdate,
            this.nov_annual,
            this.dec_sttdate,
            this.dec_annual,
            this.total_annual,
            this.leave_annual});
            this.dgvVacation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVacation.EnableHeadersVisualStyles = false;
            this.dgvVacation.Location = new System.Drawing.Point(0, 0);
            this.dgvVacation.Name = "dgvVacation";
            this.dgvVacation.RowTemplate.Height = 23;
            this.dgvVacation.Size = new System.Drawing.Size(1894, 650);
            this.dgvVacation.TabIndex = 0;
            this.dgvVacation.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvVacation_CellFormatting);
            this.dgvVacation.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvSales_CellPainting);
            this.dgvVacation.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVacation_CellValueChanged);
            this.dgvVacation.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvSales_ColumnWidthChanged);
            this.dgvVacation.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvSales_Scroll);
            this.dgvVacation.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvSales_Paint);
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
            // in_date
            // 
            this.in_date.HeaderText = "입사일    /    퇴사일";
            this.in_date.Name = "in_date";
            this.in_date.Width = 70;
            // 
            // accrued_annual
            // 
            this.accrued_annual.HeaderText = "발생";
            this.accrued_annual.Name = "accrued_annual";
            this.accrued_annual.Width = 60;
            // 
            // jen_sttdate
            // 
            this.jen_sttdate.HeaderText = "날짜";
            this.jen_sttdate.Name = "jen_sttdate";
            this.jen_sttdate.Width = 70;
            // 
            // jen_annual
            // 
            this.jen_annual.HeaderText = "사용";
            this.jen_annual.Name = "jen_annual";
            this.jen_annual.Width = 55;
            // 
            // feb_sttdate
            // 
            this.feb_sttdate.HeaderText = "날짜";
            this.feb_sttdate.Name = "feb_sttdate";
            this.feb_sttdate.Width = 70;
            // 
            // feb_annual
            // 
            this.feb_annual.HeaderText = "사용";
            this.feb_annual.Name = "feb_annual";
            this.feb_annual.Width = 55;
            // 
            // mar_sttdate
            // 
            this.mar_sttdate.HeaderText = "날짜";
            this.mar_sttdate.Name = "mar_sttdate";
            this.mar_sttdate.Width = 70;
            // 
            // mar_annual
            // 
            this.mar_annual.HeaderText = "사용";
            this.mar_annual.Name = "mar_annual";
            this.mar_annual.Width = 55;
            // 
            // apr_sttdate
            // 
            this.apr_sttdate.HeaderText = "날짜";
            this.apr_sttdate.Name = "apr_sttdate";
            this.apr_sttdate.Width = 70;
            // 
            // apr_annual
            // 
            this.apr_annual.HeaderText = "사용";
            this.apr_annual.Name = "apr_annual";
            this.apr_annual.Width = 55;
            // 
            // may_sttdate
            // 
            this.may_sttdate.HeaderText = "날짜";
            this.may_sttdate.Name = "may_sttdate";
            this.may_sttdate.Width = 70;
            // 
            // may_annual
            // 
            this.may_annual.HeaderText = "사용";
            this.may_annual.Name = "may_annual";
            this.may_annual.Width = 55;
            // 
            // jun_sttdate
            // 
            this.jun_sttdate.HeaderText = "날짜";
            this.jun_sttdate.Name = "jun_sttdate";
            this.jun_sttdate.Width = 70;
            // 
            // jun_annual
            // 
            this.jun_annual.HeaderText = "사용";
            this.jun_annual.Name = "jun_annual";
            this.jun_annual.Width = 55;
            // 
            // jul_sttdate
            // 
            this.jul_sttdate.HeaderText = "날짜";
            this.jul_sttdate.Name = "jul_sttdate";
            this.jul_sttdate.Width = 70;
            // 
            // jul_annual
            // 
            this.jul_annual.HeaderText = "사용";
            this.jul_annual.Name = "jul_annual";
            this.jul_annual.Width = 55;
            // 
            // aug_sttdate
            // 
            this.aug_sttdate.HeaderText = "날짜";
            this.aug_sttdate.Name = "aug_sttdate";
            this.aug_sttdate.Width = 70;
            // 
            // aug_annual
            // 
            this.aug_annual.HeaderText = "사용";
            this.aug_annual.Name = "aug_annual";
            this.aug_annual.Width = 55;
            // 
            // sep_sttdate
            // 
            this.sep_sttdate.HeaderText = "날짜";
            this.sep_sttdate.Name = "sep_sttdate";
            this.sep_sttdate.Width = 70;
            // 
            // sep_annual
            // 
            this.sep_annual.HeaderText = "사용";
            this.sep_annual.Name = "sep_annual";
            this.sep_annual.Width = 55;
            // 
            // oct_sttdate
            // 
            this.oct_sttdate.HeaderText = "날짜";
            this.oct_sttdate.Name = "oct_sttdate";
            this.oct_sttdate.Width = 70;
            // 
            // oct_annual
            // 
            this.oct_annual.HeaderText = "사용";
            this.oct_annual.Name = "oct_annual";
            this.oct_annual.Width = 55;
            // 
            // nov_sttdate
            // 
            this.nov_sttdate.HeaderText = "날짜";
            this.nov_sttdate.Name = "nov_sttdate";
            this.nov_sttdate.Width = 70;
            // 
            // nov_annual
            // 
            this.nov_annual.HeaderText = "사용";
            this.nov_annual.Name = "nov_annual";
            this.nov_annual.Width = 55;
            // 
            // dec_sttdate
            // 
            this.dec_sttdate.HeaderText = "날짜";
            this.dec_sttdate.Name = "dec_sttdate";
            this.dec_sttdate.Width = 70;
            // 
            // dec_annual
            // 
            this.dec_annual.HeaderText = "사용";
            this.dec_annual.Name = "dec_annual";
            this.dec_annual.Width = 55;
            // 
            // total_annual
            // 
            this.total_annual.HeaderText = "총 사용";
            this.total_annual.Name = "total_annual";
            this.total_annual.Width = 70;
            // 
            // leave_annual
            // 
            this.leave_annual.HeaderText = "잔여";
            this.leave_annual.Name = "leave_annual";
            this.leave_annual.Width = 70;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 679);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1894, 45);
            this.panel3.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnPreview);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(1607, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(287, 45);
            this.panel5.TabIndex = 107;
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPreview.Location = new System.Drawing.Point(182, 5);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(102, 37);
            this.btnPreview.TabIndex = 106;
            this.btnPreview.Text = "출력(Ctrl + P)";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(77, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(68, 37);
            this.btnExit.TabIndex = 105;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(3, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(68, 37);
            this.btnSearch.TabIndex = 104;
            this.btnSearch.Text = "검색(Q)";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // cbRetire
            // 
            this.cbRetire.AutoSize = true;
            this.cbRetire.Location = new System.Drawing.Point(614, 7);
            this.cbRetire.Name = "cbRetire";
            this.cbRetire.Size = new System.Drawing.Size(88, 16);
            this.cbRetire.TabIndex = 3;
            this.cbRetire.Text = "퇴사자 포함";
            this.cbRetire.UseVisualStyleBackColor = true;
            // 
            // VacationDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1894, 724);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "VacationDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "연차휴가 관리대장";
            this.Load += new System.EventHandler(this.VacationDashboard_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VacationDashboard_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVacation)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvVacation;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox cbSortType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn in_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn accrued_annual;
        private System.Windows.Forms.DataGridViewTextBoxColumn jen_sttdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn jen_annual;
        private System.Windows.Forms.DataGridViewTextBoxColumn feb_sttdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn feb_annual;
        private System.Windows.Forms.DataGridViewTextBoxColumn mar_sttdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn mar_annual;
        private System.Windows.Forms.DataGridViewTextBoxColumn apr_sttdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn apr_annual;
        private System.Windows.Forms.DataGridViewTextBoxColumn may_sttdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn may_annual;
        private System.Windows.Forms.DataGridViewTextBoxColumn jun_sttdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn jun_annual;
        private System.Windows.Forms.DataGridViewTextBoxColumn jul_sttdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn jul_annual;
        private System.Windows.Forms.DataGridViewTextBoxColumn aug_sttdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn aug_annual;
        private System.Windows.Forms.DataGridViewTextBoxColumn sep_sttdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn sep_annual;
        private System.Windows.Forms.DataGridViewTextBoxColumn oct_sttdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn oct_annual;
        private System.Windows.Forms.DataGridViewTextBoxColumn nov_sttdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn nov_annual;
        private System.Windows.Forms.DataGridViewTextBoxColumn dec_sttdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dec_annual;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_annual;
        private System.Windows.Forms.DataGridViewTextBoxColumn leave_annual;
        private System.Windows.Forms.ComboBox cbDepartment;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbWorkplace;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnPreview;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.CheckBox cbRetire;
    }
}