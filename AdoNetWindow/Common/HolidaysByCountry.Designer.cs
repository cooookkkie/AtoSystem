namespace AdoNetWindow.Common
{
    partial class HolidaysByCountry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HolidaysByCountry));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbMonth = new System.Windows.Forms.ComboBox();
            this.cbYear = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbCountry = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvHoliday = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.sdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.holiday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.country = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoliday)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbMonth);
            this.panel1.Controls.Add(this.cbYear);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbCountry);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(666, 33);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // cbMonth
            // 
            this.cbMonth.FormattingEnabled = true;
            this.cbMonth.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.cbMonth.Location = new System.Drawing.Point(396, 6);
            this.cbMonth.Name = "cbMonth";
            this.cbMonth.Size = new System.Drawing.Size(49, 20);
            this.cbMonth.TabIndex = 4;
            // 
            // cbYear
            // 
            this.cbYear.FormattingEnabled = true;
            this.cbYear.Location = new System.Drawing.Point(317, 6);
            this.cbYear.Name = "cbYear";
            this.cbYear.Size = new System.Drawing.Size(73, 20);
            this.cbYear.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(258, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "기준년월";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "국가";
            // 
            // cbCountry
            // 
            this.cbCountry.FormattingEnabled = true;
            this.cbCountry.Location = new System.Drawing.Point(47, 6);
            this.cbCountry.Name = "cbCountry";
            this.cbCountry.Size = new System.Drawing.Size(192, 20);
            this.cbCountry.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvHoliday);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(666, 442);
            this.panel2.TabIndex = 1;
            // 
            // dgvHoliday
            // 
            this.dgvHoliday.AllowUserToAddRows = false;
            this.dgvHoliday.AllowUserToDeleteRows = false;
            this.dgvHoliday.AllowUserToResizeColumns = false;
            this.dgvHoliday.AllowUserToResizeRows = false;
            this.dgvHoliday.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHoliday.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHoliday.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sdate,
            this.holiday,
            this.type,
            this.country});
            this.dgvHoliday.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHoliday.EnableHeadersVisualStyles = false;
            this.dgvHoliday.Location = new System.Drawing.Point(0, 0);
            this.dgvHoliday.Name = "dgvHoliday";
            this.dgvHoliday.RowTemplate.Height = 23;
            this.dgvHoliday.Size = new System.Drawing.Size(666, 442);
            this.dgvHoliday.TabIndex = 0;
            // 
            // sdate
            // 
            this.sdate.HeaderText = "날짜";
            this.sdate.Name = "sdate";
            // 
            // holiday
            // 
            this.holiday.HeaderText = "공휴일";
            this.holiday.Name = "holiday";
            // 
            // type
            // 
            this.type.HeaderText = "유형";
            this.type.Name = "type";
            // 
            // country
            // 
            this.country.HeaderText = "나라";
            this.country.Name = "country";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 475);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(666, 39);
            this.panel3.TabIndex = 2;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(3, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(63, 33);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "검색(Q)";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(71, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(63, 33);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // HolidaysByCountry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 514);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "HolidaysByCountry";
            this.Text = "나라별 공휴일 검색";
            this.Load += new System.EventHandler(this.HolidaysByCountry_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HolidaysByCountry_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoliday)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbMonth;
        private System.Windows.Forms.ComboBox cbYear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbCountry;
        private System.Windows.Forms.Panel panel2;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvHoliday;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn sdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn holiday;
        private System.Windows.Forms.DataGridViewTextBoxColumn type;
        private System.Windows.Forms.DataGridViewTextBoxColumn country;
        private System.Windows.Forms.Button btnExit;
    }
}