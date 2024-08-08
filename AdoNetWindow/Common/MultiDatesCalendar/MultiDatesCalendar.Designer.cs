namespace AdoNetWindow.Common.MultiDatesCalendar
{
    partial class MultiDatesCalendar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiDatesCalendar));
            this.panel1 = new System.Windows.Forms.Panel();
            this.daycontainer1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnTomorrow = new System.Windows.Forms.Button();
            this.btnGotoToday = new System.Windows.Forms.Button();
            this.btnBefore = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbPreMonth = new System.Windows.Forms.ComboBox();
            this.cbPreYear = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.btnAfter = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cbNextMonth = new System.Windows.Forms.ComboBox();
            this.cbNextYear = new System.Windows.Forms.ComboBox();
            this.daycontainer2 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnSelect = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.daycontainer1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(464, 435);
            this.panel1.TabIndex = 0;
            // 
            // daycontainer1
            // 
            this.daycontainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.daycontainer1.Location = new System.Drawing.Point(0, 72);
            this.daycontainer1.Name = "daycontainer1";
            this.daycontainer1.Size = new System.Drawing.Size(462, 361);
            this.daycontainer1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(1)))));
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(462, 44);
            this.panel2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(91, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "월";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(421, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(18, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "토";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(27, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "일";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(155, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "화";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(354, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(18, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "금";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(222, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "수";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(287, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(18, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "목";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnTomorrow);
            this.panel3.Controls.Add(this.btnGotoToday);
            this.panel3.Controls.Add(this.btnBefore);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.cbPreMonth);
            this.panel3.Controls.Add(this.cbPreYear);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(462, 28);
            this.panel3.TabIndex = 5;
            // 
            // btnTomorrow
            // 
            this.btnTomorrow.BackColor = System.Drawing.Color.White;
            this.btnTomorrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTomorrow.Location = new System.Drawing.Point(409, 2);
            this.btnTomorrow.Name = "btnTomorrow";
            this.btnTomorrow.Size = new System.Drawing.Size(48, 23);
            this.btnTomorrow.TabIndex = 7;
            this.btnTomorrow.Text = "내일";
            this.btnTomorrow.UseVisualStyleBackColor = false;
            this.btnTomorrow.Click += new System.EventHandler(this.btnTomorrow_Click);
            // 
            // btnGotoToday
            // 
            this.btnGotoToday.BackColor = System.Drawing.Color.White;
            this.btnGotoToday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGotoToday.Location = new System.Drawing.Point(356, 2);
            this.btnGotoToday.Name = "btnGotoToday";
            this.btnGotoToday.Size = new System.Drawing.Size(48, 23);
            this.btnGotoToday.TabIndex = 6;
            this.btnGotoToday.Text = "오늘";
            this.btnGotoToday.UseVisualStyleBackColor = false;
            this.btnGotoToday.Click += new System.EventHandler(this.btnGotoToday_Click);
            // 
            // btnBefore
            // 
            this.btnBefore.BackColor = System.Drawing.Color.White;
            this.btnBefore.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBefore.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBefore.Location = new System.Drawing.Point(133, 2);
            this.btnBefore.Name = "btnBefore";
            this.btnBefore.Size = new System.Drawing.Size(23, 23);
            this.btnBefore.TabIndex = 4;
            this.btnBefore.Text = "<";
            this.btnBefore.UseVisualStyleBackColor = false;
            this.btnBefore.Click += new System.EventHandler(this.btnBefore_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(286, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 12);
            this.label9.TabIndex = 3;
            this.label9.Text = "월";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(226, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(18, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "년";
            // 
            // cbPreMonth
            // 
            this.cbPreMonth.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbPreMonth.FormattingEnabled = true;
            this.cbPreMonth.Items.AddRange(new object[] {
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
            this.cbPreMonth.Location = new System.Drawing.Point(244, 3);
            this.cbPreMonth.Name = "cbPreMonth";
            this.cbPreMonth.Size = new System.Drawing.Size(39, 21);
            this.cbPreMonth.TabIndex = 1;
            // 
            // cbPreYear
            // 
            this.cbPreYear.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbPreYear.FormattingEnabled = true;
            this.cbPreYear.Location = new System.Drawing.Point(163, 3);
            this.cbPreYear.Name = "cbPreYear";
            this.cbPreYear.Size = new System.Drawing.Size(60, 21);
            this.cbPreYear.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(1)))));
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.label13);
            this.panel4.Controls.Add(this.label14);
            this.panel4.Controls.Add(this.label15);
            this.panel4.Controls.Add(this.label16);
            this.panel4.Controls.Add(this.label17);
            this.panel4.Controls.Add(this.label18);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 28);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(462, 44);
            this.panel4.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(89, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 12);
            this.label10.TabIndex = 8;
            this.label10.Text = "월";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.ForeColor = System.Drawing.Color.Blue;
            this.label13.Location = new System.Drawing.Point(419, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(18, 12);
            this.label13.TabIndex = 13;
            this.label13.Text = "토";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(25, 16);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(18, 12);
            this.label14.TabIndex = 7;
            this.label14.Text = "일";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(153, 16);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(18, 12);
            this.label15.TabIndex = 9;
            this.label15.Text = "화";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.Location = new System.Drawing.Point(352, 16);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(18, 12);
            this.label16.TabIndex = 12;
            this.label16.Text = "금";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label17.ForeColor = System.Drawing.Color.Black;
            this.label17.Location = new System.Drawing.Point(220, 16);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(18, 12);
            this.label17.TabIndex = 10;
            this.label17.Text = "수";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label18.ForeColor = System.Drawing.Color.Black;
            this.label18.Location = new System.Drawing.Point(285, 16);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(18, 12);
            this.label18.TabIndex = 11;
            this.label18.Text = "목";
            // 
            // btnAfter
            // 
            this.btnAfter.BackColor = System.Drawing.Color.White;
            this.btnAfter.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAfter.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAfter.Location = new System.Drawing.Point(308, 3);
            this.btnAfter.Name = "btnAfter";
            this.btnAfter.Size = new System.Drawing.Size(23, 23);
            this.btnAfter.TabIndex = 5;
            this.btnAfter.Text = ">";
            this.btnAfter.UseVisualStyleBackColor = false;
            this.btnAfter.Click += new System.EventHandler(this.btnAfter_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(283, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(18, 12);
            this.label11.TabIndex = 3;
            this.label11.Text = "월";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(223, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(18, 12);
            this.label12.TabIndex = 2;
            this.label12.Text = "년";
            // 
            // cbNextMonth
            // 
            this.cbNextMonth.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbNextMonth.FormattingEnabled = true;
            this.cbNextMonth.Items.AddRange(new object[] {
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
            this.cbNextMonth.Location = new System.Drawing.Point(241, 4);
            this.cbNextMonth.Name = "cbNextMonth";
            this.cbNextMonth.Size = new System.Drawing.Size(39, 21);
            this.cbNextMonth.TabIndex = 1;
            // 
            // cbNextYear
            // 
            this.cbNextYear.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbNextYear.FormattingEnabled = true;
            this.cbNextYear.Location = new System.Drawing.Point(160, 4);
            this.cbNextYear.Name = "cbNextYear";
            this.cbNextYear.Size = new System.Drawing.Size(60, 21);
            this.cbNextYear.TabIndex = 0;
            // 
            // daycontainer2
            // 
            this.daycontainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.daycontainer2.Location = new System.Drawing.Point(0, 72);
            this.daycontainer2.Name = "daycontainer2";
            this.daycontainer2.Size = new System.Drawing.Size(462, 361);
            this.daycontainer2.TabIndex = 4;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnSelect);
            this.panel5.Controls.Add(this.btnRefresh);
            this.panel5.Controls.Add(this.btnAfter);
            this.panel5.Controls.Add(this.cbNextYear);
            this.panel5.Controls.Add(this.cbNextMonth);
            this.panel5.Controls.Add(this.label11);
            this.panel5.Controls.Add(this.label12);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(462, 28);
            this.panel5.TabIndex = 5;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.White;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Location = new System.Drawing.Point(3, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "초기화(F5)";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.daycontainer2);
            this.panel6.Controls.Add(this.panel4);
            this.panel6.Controls.Add(this.panel5);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(464, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(464, 435);
            this.panel6.TabIndex = 1;
            // 
            // btnSelect
            // 
            this.btnSelect.BackColor = System.Drawing.Color.White;
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelect.ForeColor = System.Drawing.Color.Blue;
            this.btnSelect.Location = new System.Drawing.Point(354, 3);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(105, 23);
            this.btnSelect.TabIndex = 8;
            this.btnSelect.Text = "확인(Enter)";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // MultiDatesCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(928, 435);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultiDatesCalendar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "캘린더";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MultiDatesCalendar_FormClosing);
            this.Load += new System.EventHandler(this.MultiDatesCalendar_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MultiDatesCalendar_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnBefore;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbPreMonth;
        private System.Windows.Forms.ComboBox cbPreYear;
        private System.Windows.Forms.FlowLayoutPanel daycontainer1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnAfter;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbNextMonth;
        private System.Windows.Forms.ComboBox cbNextYear;
        private System.Windows.Forms.FlowLayoutPanel daycontainer2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnGotoToday;
        private System.Windows.Forms.Button btnTomorrow;
        private System.Windows.Forms.Button btnSelect;
    }
}