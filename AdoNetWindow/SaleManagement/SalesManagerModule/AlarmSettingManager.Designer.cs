namespace AdoNetWindow.SaleManagement.SalesManagerModule
{
    partial class AlarmSettingManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlarmSettingManager));
            this.btnExit = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvAlarm = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.alarm_division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alarm_category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alarm_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alarm_remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.edit_user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbAlarmMonth = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbFriday = new System.Windows.Forms.CheckBox();
            this.cbAlarmDivision = new System.Windows.Forms.ComboBox();
            this.cbThursday = new System.Windows.Forms.CheckBox();
            this.label25 = new System.Windows.Forms.Label();
            this.cbMonday = new System.Windows.Forms.CheckBox();
            this.cbWednesday = new System.Windows.Forms.CheckBox();
            this.cbTuseday = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.pnDays = new System.Windows.Forms.FlowLayoutPanel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.btnAfterMonth = new System.Windows.Forms.Button();
            this.btnPreMonth = new System.Windows.Forms.Button();
            this.cbMonth = new System.Windows.Forms.ComboBox();
            this.cbYear = new System.Windows.Forms.ComboBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.btnTomorrow = new System.Windows.Forms.Button();
            this.btnToday = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlarm)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel10.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(822, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 36);
            this.btnExit.TabIndex = 45;
            this.btnExit.Text = "닫기(ESC)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRegister.ForeColor = System.Drawing.Color.Blue;
            this.btnRegister.Location = new System.Drawing.Point(3, 3);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(65, 36);
            this.btnRegister.TabIndex = 41;
            this.btnRegister.Text = "등록(Enter)";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRefresh.Location = new System.Drawing.Point(74, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(66, 36);
            this.btnRefresh.TabIndex = 43;
            this.btnRefresh.Text = "초기화(F5)";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvAlarm);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(894, 178);
            this.panel1.TabIndex = 46;
            // 
            // dgvAlarm
            // 
            this.dgvAlarm.AllowUserToAddRows = false;
            this.dgvAlarm.AllowUserToDeleteRows = false;
            this.dgvAlarm.AllowUserToResizeColumns = false;
            this.dgvAlarm.AllowUserToResizeRows = false;
            this.dgvAlarm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvAlarm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.alarm_division,
            this.alarm_category,
            this.alarm_date,
            this.alarm_remark,
            this.edit_user,
            this.updatetime});
            this.dgvAlarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAlarm.EnableHeadersVisualStyles = false;
            this.dgvAlarm.Location = new System.Drawing.Point(0, 0);
            this.dgvAlarm.Name = "dgvAlarm";
            this.dgvAlarm.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvAlarm.RowTemplate.Height = 23;
            this.dgvAlarm.Size = new System.Drawing.Size(894, 178);
            this.dgvAlarm.TabIndex = 33;
            // 
            // alarm_division
            // 
            this.alarm_division.HeaderText = "구분";
            this.alarm_division.Name = "alarm_division";
            // 
            // alarm_category
            // 
            this.alarm_category.HeaderText = "카테고리";
            this.alarm_category.Name = "alarm_category";
            // 
            // alarm_date
            // 
            this.alarm_date.HeaderText = "날짜";
            this.alarm_date.Name = "alarm_date";
            // 
            // alarm_remark
            // 
            this.alarm_remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.alarm_remark.HeaderText = "비고";
            this.alarm_remark.Name = "alarm_remark";
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
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnTomorrow);
            this.panel2.Controls.Add(this.btnToday);
            this.panel2.Controls.Add(this.cbAlarmMonth);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.cbFriday);
            this.panel2.Controls.Add(this.cbAlarmDivision);
            this.panel2.Controls.Add(this.cbThursday);
            this.panel2.Controls.Add(this.label25);
            this.panel2.Controls.Add(this.cbMonday);
            this.panel2.Controls.Add(this.cbWednesday);
            this.panel2.Controls.Add(this.cbTuseday);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(894, 28);
            this.panel2.TabIndex = 47;
            // 
            // cbAlarmMonth
            // 
            this.cbAlarmMonth.FormattingEnabled = true;
            this.cbAlarmMonth.Items.AddRange(new object[] {
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
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.cbAlarmMonth.Location = new System.Drawing.Point(821, 4);
            this.cbAlarmMonth.Name = "cbAlarmMonth";
            this.cbAlarmMonth.Size = new System.Drawing.Size(67, 20);
            this.cbAlarmMonth.TabIndex = 107;
            this.cbAlarmMonth.SelectedIndexChanged += new System.EventHandler(this.cbAlarmMonth_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(766, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 12);
            this.label9.TabIndex = 112;
            this.label9.Text = "월 알람";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(10, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 12);
            this.label8.TabIndex = 109;
            this.label8.Text = "알람구분";
            // 
            // cbFriday
            // 
            this.cbFriday.AutoSize = true;
            this.cbFriday.Location = new System.Drawing.Point(713, 6);
            this.cbFriday.Name = "cbFriday";
            this.cbFriday.Size = new System.Drawing.Size(36, 16);
            this.cbFriday.TabIndex = 106;
            this.cbFriday.Text = "금";
            this.cbFriday.UseVisualStyleBackColor = true;
            this.cbFriday.CheckedChanged += new System.EventHandler(this.cbMonday_CheckedChanged);
            // 
            // cbAlarmDivision
            // 
            this.cbAlarmDivision.FormattingEnabled = true;
            this.cbAlarmDivision.Items.AddRange(new object[] {
            "발주",
            "결제",
            "신규",
            "기타"});
            this.cbAlarmDivision.Location = new System.Drawing.Point(70, 3);
            this.cbAlarmDivision.Name = "cbAlarmDivision";
            this.cbAlarmDivision.Size = new System.Drawing.Size(55, 20);
            this.cbAlarmDivision.TabIndex = 108;
            this.cbAlarmDivision.Text = "발주";
            // 
            // cbThursday
            // 
            this.cbThursday.AutoSize = true;
            this.cbThursday.Location = new System.Drawing.Point(671, 6);
            this.cbThursday.Name = "cbThursday";
            this.cbThursday.Size = new System.Drawing.Size(36, 16);
            this.cbThursday.TabIndex = 105;
            this.cbThursday.Text = "목";
            this.cbThursday.UseVisualStyleBackColor = true;
            this.cbThursday.CheckedChanged += new System.EventHandler(this.cbMonday_CheckedChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label25.Location = new System.Drawing.Point(490, 8);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(49, 12);
            this.label25.TabIndex = 102;
            this.label25.Text = "주 알람";
            // 
            // cbMonday
            // 
            this.cbMonday.AutoSize = true;
            this.cbMonday.Location = new System.Drawing.Point(545, 6);
            this.cbMonday.Name = "cbMonday";
            this.cbMonday.Size = new System.Drawing.Size(36, 16);
            this.cbMonday.TabIndex = 101;
            this.cbMonday.Text = "월";
            this.cbMonday.UseVisualStyleBackColor = true;
            this.cbMonday.CheckedChanged += new System.EventHandler(this.cbMonday_CheckedChanged);
            // 
            // cbWednesday
            // 
            this.cbWednesday.AutoSize = true;
            this.cbWednesday.Location = new System.Drawing.Point(629, 6);
            this.cbWednesday.Name = "cbWednesday";
            this.cbWednesday.Size = new System.Drawing.Size(36, 16);
            this.cbWednesday.TabIndex = 104;
            this.cbWednesday.Text = "수";
            this.cbWednesday.UseVisualStyleBackColor = true;
            this.cbWednesday.CheckedChanged += new System.EventHandler(this.cbMonday_CheckedChanged);
            // 
            // cbTuseday
            // 
            this.cbTuseday.AutoSize = true;
            this.cbTuseday.Location = new System.Drawing.Point(587, 6);
            this.cbTuseday.Name = "cbTuseday";
            this.cbTuseday.Size = new System.Drawing.Size(36, 16);
            this.cbTuseday.TabIndex = 103;
            this.cbTuseday.Text = "화";
            this.cbTuseday.UseVisualStyleBackColor = true;
            this.cbTuseday.CheckedChanged += new System.EventHandler(this.cbMonday_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnRefresh);
            this.panel3.Controls.Add(this.btnRegister);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 206);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(894, 42);
            this.panel3.TabIndex = 48;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.panel7);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(894, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(249, 248);
            this.panel4.TabIndex = 49;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.pnDays);
            this.panel7.Controls.Add(this.panel10);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel7.Location = new System.Drawing.Point(1, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(246, 246);
            this.panel7.TabIndex = 34;
            // 
            // pnDays
            // 
            this.pnDays.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnDays.Location = new System.Drawing.Point(0, 37);
            this.pnDays.Name = "pnDays";
            this.pnDays.Size = new System.Drawing.Size(246, 209);
            this.pnDays.TabIndex = 1;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.btnAfterMonth);
            this.panel10.Controls.Add(this.btnPreMonth);
            this.panel10.Controls.Add(this.cbMonth);
            this.panel10.Controls.Add(this.cbYear);
            this.panel10.Controls.Add(this.label29);
            this.panel10.Controls.Add(this.label24);
            this.panel10.Controls.Add(this.label28);
            this.panel10.Controls.Add(this.label17);
            this.panel10.Controls.Add(this.label19);
            this.panel10.Controls.Add(this.label16);
            this.panel10.Controls.Add(this.label15);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel10.Location = new System.Drawing.Point(0, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(246, 37);
            this.panel10.TabIndex = 0;
            // 
            // btnAfterMonth
            // 
            this.btnAfterMonth.Font = new System.Drawing.Font("한컴 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAfterMonth.ForeColor = System.Drawing.Color.Black;
            this.btnAfterMonth.Location = new System.Drawing.Point(171, -2);
            this.btnAfterMonth.Name = "btnAfterMonth";
            this.btnAfterMonth.Size = new System.Drawing.Size(73, 21);
            this.btnAfterMonth.TabIndex = 101;
            this.btnAfterMonth.Text = ">>";
            this.btnAfterMonth.UseVisualStyleBackColor = true;
            this.btnAfterMonth.Click += new System.EventHandler(this.btnAfterMonth_Click);
            // 
            // btnPreMonth
            // 
            this.btnPreMonth.Font = new System.Drawing.Font("한컴 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPreMonth.ForeColor = System.Drawing.Color.Black;
            this.btnPreMonth.Location = new System.Drawing.Point(0, -1);
            this.btnPreMonth.Name = "btnPreMonth";
            this.btnPreMonth.Size = new System.Drawing.Size(73, 21);
            this.btnPreMonth.TabIndex = 100;
            this.btnPreMonth.Text = "<<";
            this.btnPreMonth.UseVisualStyleBackColor = true;
            this.btnPreMonth.Click += new System.EventHandler(this.btnPreMonth_Click);
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
            this.cbMonth.Location = new System.Drawing.Point(136, -1);
            this.cbMonth.Name = "cbMonth";
            this.cbMonth.Size = new System.Drawing.Size(35, 20);
            this.cbMonth.TabIndex = 33;
            this.cbMonth.Text = "12";
            // 
            // cbYear
            // 
            this.cbYear.FormattingEnabled = true;
            this.cbYear.Items.AddRange(new object[] {
            "발주",
            "결제",
            "신규",
            "기타"});
            this.cbYear.Location = new System.Drawing.Point(73, -1);
            this.cbYear.Name = "cbYear";
            this.cbYear.Size = new System.Drawing.Size(57, 20);
            this.cbYear.TabIndex = 32;
            this.cbYear.Text = "2023";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.ForeColor = System.Drawing.Color.Blue;
            this.label29.Location = new System.Drawing.Point(219, 22);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(17, 12);
            this.label29.TabIndex = 6;
            this.label29.Text = "토";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(184, 22);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(17, 12);
            this.label24.TabIndex = 5;
            this.label24.Text = "금";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(149, 22);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(17, 12);
            this.label28.TabIndex = 4;
            this.label28.Text = "목";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(114, 22);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(17, 12);
            this.label17.TabIndex = 3;
            this.label17.Text = "수";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(79, 22);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(17, 12);
            this.label19.TabIndex = 2;
            this.label19.Text = "화";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(43, 22);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(17, 12);
            this.label16.TabIndex = 1;
            this.label16.Text = "월";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(8, 22);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(17, 12);
            this.label15.TabIndex = 0;
            this.label15.Text = "일";
            // 
            // btnTomorrow
            // 
            this.btnTomorrow.BackColor = System.Drawing.Color.White;
            this.btnTomorrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTomorrow.Location = new System.Drawing.Point(415, 1);
            this.btnTomorrow.Name = "btnTomorrow";
            this.btnTomorrow.Size = new System.Drawing.Size(48, 23);
            this.btnTomorrow.TabIndex = 114;
            this.btnTomorrow.Text = "내일";
            this.btnTomorrow.UseVisualStyleBackColor = false;
            this.btnTomorrow.Click += new System.EventHandler(this.btnTomorrow_Click);
            // 
            // btnToday
            // 
            this.btnToday.BackColor = System.Drawing.Color.White;
            this.btnToday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToday.Location = new System.Drawing.Point(362, 1);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(48, 23);
            this.btnToday.TabIndex = 113;
            this.btnToday.Text = "오늘";
            this.btnToday.UseVisualStyleBackColor = false;
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            // 
            // AlarmSettingManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 248);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlarmSettingManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "영업알림";
            this.Load += new System.EventHandler(this.AlarmSettingManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AlarmSettingManager_KeyDown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlarm)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel panel1;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvAlarm;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridViewTextBoxColumn alarm_division;
        private System.Windows.Forms.DataGridViewTextBoxColumn alarm_category;
        private System.Windows.Forms.DataGridViewTextBoxColumn alarm_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn alarm_remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn edit_user;
        private System.Windows.Forms.DataGridViewTextBoxColumn updatetime;
        private System.Windows.Forms.ComboBox cbAlarmMonth;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbFriday;
        private System.Windows.Forms.ComboBox cbAlarmDivision;
        private System.Windows.Forms.CheckBox cbThursday;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.CheckBox cbMonday;
        private System.Windows.Forms.CheckBox cbWednesday;
        private System.Windows.Forms.CheckBox cbTuseday;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.FlowLayoutPanel pnDays;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Button btnAfterMonth;
        private System.Windows.Forms.Button btnPreMonth;
        private System.Windows.Forms.ComboBox cbMonth;
        private System.Windows.Forms.ComboBox cbYear;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnTomorrow;
        private System.Windows.Forms.Button btnToday;
    }
}