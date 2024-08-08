namespace AdoNetWindow.RecoveryPrincipal.RecoveryPrincipalmanager2
{
    partial class RecoveryPrincipalManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecoveryPrincipalManager));
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearching = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.lbCompanyCode = new System.Windows.Forms.Label();
            this.btnStandardDate = new System.Windows.Forms.Button();
            this.txtStandardDate = new System.Windows.Forms.TextBox();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnManagment3 = new System.Windows.Forms.Button();
            this.cbIsManagement = new System.Windows.Forms.CheckBox();
            this.btnManagment4 = new System.Windows.Forms.Button();
            this.btnManagment = new System.Windows.Forms.Button();
            this.cbIsManagement4 = new System.Windows.Forms.CheckBox();
            this.cbIsManagement2 = new System.Windows.Forms.CheckBox();
            this.btnManagment2 = new System.Windows.Forms.Button();
            this.cbIsManagement3 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbThreeMonths = new System.Windows.Forms.RadioButton();
            this.txtBusinessTerms = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rbOneMonth = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.rbTwoMonths = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvRecovery = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pre_month_12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pre_month_11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pre_month_10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pre_month_9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pre_month_8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pre_month_7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pre_month_6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pre_month_5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pre_month_4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pre_month_3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pre_month_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pre_month_1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pre_month_0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel4 = new System.Windows.Forms.Panel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.pnGlass = new Libs.GlassPanel.GlassPanel();
            this.loading1 = new Libs.GlassPanel.GlassElementhost();
            this.loading2 = new AdoNetWindow.SEAOVER.TwoLine.Loading();
            this.panel5.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecovery)).BeginInit();
            this.panel4.SuspendLayout();
            this.pnGlass.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnInsert);
            this.panel5.Controls.Add(this.btnExit);
            this.panel5.Controls.Add(this.btnSearching);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 953);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1847, 43);
            this.panel5.TabIndex = 10;
            // 
            // btnInsert
            // 
            this.btnInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInsert.Location = new System.Drawing.Point(77, 2);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(68, 38);
            this.btnInsert.TabIndex = 4;
            this.btnInsert.Text = "등록(A)";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(151, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(68, 38);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.Location = new System.Drawing.Point(3, 2);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(68, 38);
            this.btnSearching.TabIndex = 0;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.lbCompanyCode);
            this.panel7.Controls.Add(this.btnStandardDate);
            this.panel7.Controls.Add(this.txtStandardDate);
            this.panel7.Controls.Add(this.txtCompany);
            this.panel7.Controls.Add(this.label1);
            this.panel7.Controls.Add(this.panel3);
            this.panel7.Controls.Add(this.label2);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1847, 31);
            this.panel7.TabIndex = 11;
            // 
            // lbCompanyCode
            // 
            this.lbCompanyCode.AutoSize = true;
            this.lbCompanyCode.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbCompanyCode.Location = new System.Drawing.Point(601, 8);
            this.lbCompanyCode.Name = "lbCompanyCode";
            this.lbCompanyCode.Size = new System.Drawing.Size(61, 12);
            this.lbCompanyCode.TabIndex = 232;
            this.lbCompanyCode.Text = "00000000";
            this.lbCompanyCode.Visible = false;
            // 
            // btnStandardDate
            // 
            this.btnStandardDate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStandardDate.BackgroundImage")));
            this.btnStandardDate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStandardDate.FlatAppearance.BorderSize = 0;
            this.btnStandardDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStandardDate.Location = new System.Drawing.Point(145, 2);
            this.btnStandardDate.Name = "btnStandardDate";
            this.btnStandardDate.Size = new System.Drawing.Size(22, 23);
            this.btnStandardDate.TabIndex = 231;
            this.btnStandardDate.UseVisualStyleBackColor = true;
            this.btnStandardDate.Click += new System.EventHandler(this.btnStandardDate_Click);
            // 
            // txtStandardDate
            // 
            this.txtStandardDate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtStandardDate.Location = new System.Drawing.Point(68, 4);
            this.txtStandardDate.Name = "txtStandardDate";
            this.txtStandardDate.Size = new System.Drawing.Size(76, 21);
            this.txtStandardDate.TabIndex = 230;
            this.txtStandardDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtStandardDate_KeyDown);
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(231, 4);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(354, 21);
            this.txtCompany.TabIndex = 58;
            this.txtCompany.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtStandardDate_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(181, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 12);
            this.label1.TabIndex = 57;
            this.label1.Text = "거래처";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnManagment3);
            this.panel3.Controls.Add(this.cbIsManagement);
            this.panel3.Controls.Add(this.btnManagment4);
            this.panel3.Controls.Add(this.btnManagment);
            this.panel3.Controls.Add(this.cbIsManagement4);
            this.panel3.Controls.Add(this.cbIsManagement2);
            this.panel3.Controls.Add(this.btnManagment2);
            this.panel3.Controls.Add(this.cbIsManagement3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(1494, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(351, 29);
            this.panel3.TabIndex = 56;
            // 
            // btnManagment3
            // 
            this.btnManagment3.BackgroundImage = global::AdoNetWindow.Properties.Resources.Star_empty_icon;
            this.btnManagment3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnManagment3.FlatAppearance.BorderSize = 0;
            this.btnManagment3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManagment3.Location = new System.Drawing.Point(270, -2);
            this.btnManagment3.Name = "btnManagment3";
            this.btnManagment3.Size = new System.Drawing.Size(30, 30);
            this.btnManagment3.TabIndex = 51;
            this.btnManagment3.UseVisualStyleBackColor = true;
            this.btnManagment3.Click += new System.EventHandler(this.btnManagment3_Click);
            // 
            // cbIsManagement
            // 
            this.cbIsManagement.AutoSize = true;
            this.cbIsManagement.Location = new System.Drawing.Point(148, 9);
            this.cbIsManagement.Name = "cbIsManagement";
            this.cbIsManagement.Size = new System.Drawing.Size(15, 14);
            this.cbIsManagement.TabIndex = 9;
            this.cbIsManagement.UseVisualStyleBackColor = true;
            this.cbIsManagement.Visible = false;
            this.cbIsManagement.CheckedChanged += new System.EventHandler(this.cbIsManagement_CheckedChanged);
            // 
            // btnManagment4
            // 
            this.btnManagment4.BackgroundImage = global::AdoNetWindow.Properties.Resources.Star_empty_icon;
            this.btnManagment4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnManagment4.FlatAppearance.BorderSize = 0;
            this.btnManagment4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManagment4.Location = new System.Drawing.Point(321, -2);
            this.btnManagment4.Name = "btnManagment4";
            this.btnManagment4.Size = new System.Drawing.Size(30, 30);
            this.btnManagment4.TabIndex = 53;
            this.btnManagment4.UseVisualStyleBackColor = true;
            this.btnManagment4.Click += new System.EventHandler(this.btnManagment4_Click);
            // 
            // btnManagment
            // 
            this.btnManagment.BackgroundImage = global::AdoNetWindow.Properties.Resources.Star_empty_icon;
            this.btnManagment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnManagment.FlatAppearance.BorderSize = 0;
            this.btnManagment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManagment.Location = new System.Drawing.Point(169, -2);
            this.btnManagment.Name = "btnManagment";
            this.btnManagment.Size = new System.Drawing.Size(30, 30);
            this.btnManagment.TabIndex = 40;
            this.btnManagment.UseVisualStyleBackColor = true;
            this.btnManagment.Click += new System.EventHandler(this.btnManagment_Click);
            // 
            // cbIsManagement4
            // 
            this.cbIsManagement4.AutoSize = true;
            this.cbIsManagement4.Location = new System.Drawing.Point(302, 8);
            this.cbIsManagement4.Name = "cbIsManagement4";
            this.cbIsManagement4.Size = new System.Drawing.Size(15, 14);
            this.cbIsManagement4.TabIndex = 54;
            this.cbIsManagement4.UseVisualStyleBackColor = true;
            this.cbIsManagement4.Visible = false;
            this.cbIsManagement4.CheckedChanged += new System.EventHandler(this.cbIsManagement4_CheckedChanged);
            // 
            // cbIsManagement2
            // 
            this.cbIsManagement2.AutoSize = true;
            this.cbIsManagement2.Location = new System.Drawing.Point(200, 8);
            this.cbIsManagement2.Name = "cbIsManagement2";
            this.cbIsManagement2.Size = new System.Drawing.Size(15, 14);
            this.cbIsManagement2.TabIndex = 50;
            this.cbIsManagement2.UseVisualStyleBackColor = true;
            this.cbIsManagement2.Visible = false;
            this.cbIsManagement2.CheckedChanged += new System.EventHandler(this.cbIsManagement2_CheckedChanged);
            // 
            // btnManagment2
            // 
            this.btnManagment2.BackgroundImage = global::AdoNetWindow.Properties.Resources.Star_empty_icon;
            this.btnManagment2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnManagment2.FlatAppearance.BorderSize = 0;
            this.btnManagment2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManagment2.Location = new System.Drawing.Point(219, -2);
            this.btnManagment2.Name = "btnManagment2";
            this.btnManagment2.Size = new System.Drawing.Size(30, 30);
            this.btnManagment2.TabIndex = 49;
            this.btnManagment2.UseVisualStyleBackColor = true;
            this.btnManagment2.Click += new System.EventHandler(this.btnManagment2_Click);
            // 
            // cbIsManagement3
            // 
            this.cbIsManagement3.AutoSize = true;
            this.cbIsManagement3.Location = new System.Drawing.Point(251, 8);
            this.cbIsManagement3.Name = "cbIsManagement3";
            this.cbIsManagement3.Size = new System.Drawing.Size(15, 14);
            this.cbIsManagement3.TabIndex = 52;
            this.cbIsManagement3.UseVisualStyleBackColor = true;
            this.cbIsManagement3.Visible = false;
            this.cbIsManagement3.CheckedChanged += new System.EventHandler(this.cbIsManagement3_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "기준년도";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.rbThreeMonths);
            this.panel1.Controls.Add(this.txtBusinessTerms);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.rbOneMonth);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.rbTwoMonths);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1847, 31);
            this.panel1.TabIndex = 6;
            // 
            // rbThreeMonths
            // 
            this.rbThreeMonths.AutoSize = true;
            this.rbThreeMonths.Checked = true;
            this.rbThreeMonths.Location = new System.Drawing.Point(415, 7);
            this.rbThreeMonths.Name = "rbThreeMonths";
            this.rbThreeMonths.Size = new System.Drawing.Size(53, 16);
            this.rbThreeMonths.TabIndex = 235;
            this.rbThreeMonths.TabStop = true;
            this.rbThreeMonths.Text = "3개월";
            this.rbThreeMonths.UseVisualStyleBackColor = true;
            // 
            // txtBusinessTerms
            // 
            this.txtBusinessTerms.Location = new System.Drawing.Point(123, 5);
            this.txtBusinessTerms.Name = "txtBusinessTerms";
            this.txtBusinessTerms.Size = new System.Drawing.Size(32, 21);
            this.txtBusinessTerms.TabIndex = 45;
            this.txtBusinessTerms.Text = "0";
            this.txtBusinessTerms.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(170, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 12);
            this.label4.TabIndex = 234;
            this.label4.Text = "매출/평잔 반영기간";
            // 
            // rbOneMonth
            // 
            this.rbOneMonth.AutoSize = true;
            this.rbOneMonth.Location = new System.Drawing.Point(297, 7);
            this.rbOneMonth.Name = "rbOneMonth";
            this.rbOneMonth.Size = new System.Drawing.Size(53, 16);
            this.rbOneMonth.TabIndex = 232;
            this.rbOneMonth.Text = "1개월";
            this.rbOneMonth.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(9, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 12);
            this.label3.TabIndex = 44;
            this.label3.Text = "거래기간(개월수)";
            // 
            // rbTwoMonths
            // 
            this.rbTwoMonths.AutoSize = true;
            this.rbTwoMonths.Location = new System.Drawing.Point(356, 7);
            this.rbTwoMonths.Name = "rbTwoMonths";
            this.rbTwoMonths.Size = new System.Drawing.Size(53, 16);
            this.rbTwoMonths.TabIndex = 233;
            this.rbTwoMonths.Text = "2개월";
            this.rbTwoMonths.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1847, 996);
            this.panel2.TabIndex = 7;
            // 
            // dgvRecovery
            // 
            this.dgvRecovery.AllowUserToAddRows = false;
            this.dgvRecovery.AllowUserToDeleteRows = false;
            this.dgvRecovery.AllowUserToResizeColumns = false;
            this.dgvRecovery.AllowUserToResizeRows = false;
            this.dgvRecovery.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRecovery.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvRecovery.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.division,
            this.pre_month_12,
            this.pre_month_11,
            this.pre_month_10,
            this.pre_month_9,
            this.pre_month_8,
            this.pre_month_7,
            this.pre_month_6,
            this.pre_month_5,
            this.pre_month_4,
            this.pre_month_3,
            this.pre_month_2,
            this.pre_month_1,
            this.pre_month_0});
            this.dgvRecovery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecovery.EnableHeadersVisualStyles = false;
            this.dgvRecovery.Location = new System.Drawing.Point(0, 0);
            this.dgvRecovery.Name = "dgvRecovery";
            this.dgvRecovery.RowTemplate.Height = 23;
            this.dgvRecovery.Size = new System.Drawing.Size(1845, 889);
            this.dgvRecovery.TabIndex = 0;
            this.dgvRecovery.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvRecovery_CellPainting);
            this.dgvRecovery.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRecovery_CellValueChanged);
            // 
            // division
            // 
            this.division.FillWeight = 177.665F;
            this.division.HeaderText = "구분";
            this.division.Name = "division";
            // 
            // pre_month_12
            // 
            this.pre_month_12.FillWeight = 94.02576F;
            this.pre_month_12.HeaderText = "전월";
            this.pre_month_12.Name = "pre_month_12";
            // 
            // pre_month_11
            // 
            this.pre_month_11.FillWeight = 94.02576F;
            this.pre_month_11.HeaderText = "00월";
            this.pre_month_11.Name = "pre_month_11";
            // 
            // pre_month_10
            // 
            this.pre_month_10.FillWeight = 94.02576F;
            this.pre_month_10.HeaderText = "00월";
            this.pre_month_10.Name = "pre_month_10";
            // 
            // pre_month_9
            // 
            this.pre_month_9.FillWeight = 94.02576F;
            this.pre_month_9.HeaderText = "00월";
            this.pre_month_9.Name = "pre_month_9";
            // 
            // pre_month_8
            // 
            this.pre_month_8.FillWeight = 94.02576F;
            this.pre_month_8.HeaderText = "00월";
            this.pre_month_8.Name = "pre_month_8";
            // 
            // pre_month_7
            // 
            this.pre_month_7.FillWeight = 94.02576F;
            this.pre_month_7.HeaderText = "00월";
            this.pre_month_7.Name = "pre_month_7";
            // 
            // pre_month_6
            // 
            this.pre_month_6.FillWeight = 94.02576F;
            this.pre_month_6.HeaderText = "00월";
            this.pre_month_6.Name = "pre_month_6";
            // 
            // pre_month_5
            // 
            this.pre_month_5.FillWeight = 94.02576F;
            this.pre_month_5.HeaderText = "00월";
            this.pre_month_5.Name = "pre_month_5";
            // 
            // pre_month_4
            // 
            this.pre_month_4.FillWeight = 94.02576F;
            this.pre_month_4.HeaderText = "00월";
            this.pre_month_4.Name = "pre_month_4";
            // 
            // pre_month_3
            // 
            this.pre_month_3.FillWeight = 94.02576F;
            this.pre_month_3.HeaderText = "00월";
            this.pre_month_3.Name = "pre_month_3";
            // 
            // pre_month_2
            // 
            this.pre_month_2.FillWeight = 94.02576F;
            this.pre_month_2.HeaderText = "00월";
            this.pre_month_2.Name = "pre_month_2";
            // 
            // pre_month_1
            // 
            this.pre_month_1.FillWeight = 94.02576F;
            this.pre_month_1.HeaderText = "00월";
            this.pre_month_1.Name = "pre_month_1";
            // 
            // pre_month_0
            // 
            this.pre_month_0.FillWeight = 94.02576F;
            this.pre_month_0.HeaderText = "00월";
            this.pre_month_0.Name = "pre_month_0";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.dgvRecovery);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 62);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1847, 891);
            this.panel4.TabIndex = 12;
            // 
            // pnGlass
            // 
            this.pnGlass.Controls.Add(this.loading1);
            this.pnGlass.Location = new System.Drawing.Point(855, 429);
            this.pnGlass.Name = "pnGlass";
            this.pnGlass.Opacity = 50;
            this.pnGlass.Size = new System.Drawing.Size(137, 139);
            this.pnGlass.TabIndex = 15;
            this.pnGlass.Visible = false;
            // 
            // loading1
            // 
            this.loading1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loading1.Location = new System.Drawing.Point(0, 0);
            this.loading1.Name = "loading1";
            this.loading1.Opacity = 50;
            this.loading1.Size = new System.Drawing.Size(137, 139);
            this.loading1.TabIndex = 2;
            this.loading1.Text = "glassElementhost1";
            this.loading1.Child = this.loading2;
            // 
            // RecoveryPrincipalManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1847, 996);
            this.Controls.Add(this.pnGlass);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "RecoveryPrincipalManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "원금회수율 관리";
            this.Load += new System.EventHandler(this.RecoveryPrincipalManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RecoveryPrincipalManager_KeyDown);
            this.panel5.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecovery)).EndInit();
            this.panel4.ResumeLayout(false);
            this.pnGlass.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnManagment4;
        private System.Windows.Forms.CheckBox cbIsManagement4;
        private System.Windows.Forms.Button btnManagment3;
        private System.Windows.Forms.CheckBox cbIsManagement3;
        private System.Windows.Forms.Button btnManagment2;
        private System.Windows.Forms.CheckBox cbIsManagement2;
        private System.Windows.Forms.Button btnManagment;
        private System.Windows.Forms.CheckBox cbIsManagement;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtBusinessTerms;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.Label label1;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvRecovery;
        private System.Windows.Forms.Panel panel4;
        public System.Windows.Forms.Button btnStandardDate;
        public System.Windows.Forms.TextBox txtStandardDate;
        private System.Windows.Forms.RadioButton rbThreeMonths;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rbOneMonth;
        private System.Windows.Forms.RadioButton rbTwoMonths;
        private System.Windows.Forms.Label lbCompanyCode;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.DataGridViewTextBoxColumn division;
        private System.Windows.Forms.DataGridViewTextBoxColumn pre_month_12;
        private System.Windows.Forms.DataGridViewTextBoxColumn pre_month_11;
        private System.Windows.Forms.DataGridViewTextBoxColumn pre_month_10;
        private System.Windows.Forms.DataGridViewTextBoxColumn pre_month_9;
        private System.Windows.Forms.DataGridViewTextBoxColumn pre_month_8;
        private System.Windows.Forms.DataGridViewTextBoxColumn pre_month_7;
        private System.Windows.Forms.DataGridViewTextBoxColumn pre_month_6;
        private System.Windows.Forms.DataGridViewTextBoxColumn pre_month_5;
        private System.Windows.Forms.DataGridViewTextBoxColumn pre_month_4;
        private System.Windows.Forms.DataGridViewTextBoxColumn pre_month_3;
        private System.Windows.Forms.DataGridViewTextBoxColumn pre_month_2;
        private System.Windows.Forms.DataGridViewTextBoxColumn pre_month_1;
        private System.Windows.Forms.DataGridViewTextBoxColumn pre_month_0;
        private System.Windows.Forms.Button btnInsert;
        private Libs.GlassPanel.GlassPanel pnGlass;
        private Libs.GlassPanel.GlassElementhost loading1;
        private SEAOVER.TwoLine.Loading loading2;
    }
}