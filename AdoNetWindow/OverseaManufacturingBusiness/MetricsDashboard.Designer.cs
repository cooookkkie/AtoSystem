namespace AdoNetWindow.OverseaManufacturingBusiness
{
    partial class MetricsDashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MetricsDashboard));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearchProduct = new System.Windows.Forms.Button();
            this.btnEndDateCalendar = new System.Windows.Forms.Button();
            this.btnSearchCompany = new System.Windows.Forms.Button();
            this.btnSttDateCalendar = new System.Windows.Forms.Button();
            this.txtEnddate = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSttdate = new System.Windows.Forms.TextBox();
            this.rbManufacturing = new System.Windows.Forms.RadioButton();
            this.rbImporter = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.product_kor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_eng = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSearchMetrics = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.rbOnlyKorean = new System.Windows.Forms.RadioButton();
            this.rbOnlyEnglish = new System.Windows.Forms.RadioButton();
            this.rbAllLanguage = new System.Windows.Forms.RadioButton();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.rbContain = new System.Windows.Forms.RadioButton();
            this.rbExactly = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnSearchProduct);
            this.panel1.Controls.Add(this.btnEndDateCalendar);
            this.panel1.Controls.Add(this.btnSearchCompany);
            this.panel1.Controls.Add(this.btnSttDateCalendar);
            this.panel1.Controls.Add(this.txtEnddate);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtSttdate);
            this.panel1.Controls.Add(this.rbManufacturing);
            this.panel1.Controls.Add(this.rbImporter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1614, 33);
            this.panel1.TabIndex = 0;
            // 
            // btnSearchProduct
            // 
            this.btnSearchProduct.BackColor = System.Drawing.Color.White;
            this.btnSearchProduct.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSearchProduct.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearchProduct.Location = new System.Drawing.Point(549, 3);
            this.btnSearchProduct.Name = "btnSearchProduct";
            this.btnSearchProduct.Size = new System.Drawing.Size(106, 26);
            this.btnSearchProduct.TabIndex = 26;
            this.btnSearchProduct.Text = "품목 검색(F4)";
            this.btnSearchProduct.UseVisualStyleBackColor = false;
            this.btnSearchProduct.Click += new System.EventHandler(this.btnSearchProduct_Click);
            // 
            // btnEndDateCalendar
            // 
            this.btnEndDateCalendar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEndDateCalendar.BackgroundImage")));
            this.btnEndDateCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEndDateCalendar.FlatAppearance.BorderSize = 0;
            this.btnEndDateCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEndDateCalendar.Location = new System.Drawing.Point(278, 4);
            this.btnEndDateCalendar.Name = "btnEndDateCalendar";
            this.btnEndDateCalendar.Size = new System.Drawing.Size(22, 23);
            this.btnEndDateCalendar.TabIndex = 23;
            this.btnEndDateCalendar.UseVisualStyleBackColor = true;
            this.btnEndDateCalendar.Click += new System.EventHandler(this.btnEndDateCalendar_Click);
            // 
            // btnSearchCompany
            // 
            this.btnSearchCompany.BackColor = System.Drawing.Color.White;
            this.btnSearchCompany.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSearchCompany.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearchCompany.Location = new System.Drawing.Point(661, 3);
            this.btnSearchCompany.Name = "btnSearchCompany";
            this.btnSearchCompany.Size = new System.Drawing.Size(134, 26);
            this.btnSearchCompany.TabIndex = 15;
            this.btnSearchCompany.Text = "거래처 검색(F6)";
            this.btnSearchCompany.UseVisualStyleBackColor = false;
            this.btnSearchCompany.Click += new System.EventHandler(this.btnSearchCompany_Click);
            // 
            // btnSttDateCalendar
            // 
            this.btnSttDateCalendar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSttDateCalendar.BackgroundImage")));
            this.btnSttDateCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSttDateCalendar.FlatAppearance.BorderSize = 0;
            this.btnSttDateCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSttDateCalendar.Location = new System.Drawing.Point(153, 4);
            this.btnSttDateCalendar.Name = "btnSttDateCalendar";
            this.btnSttDateCalendar.Size = new System.Drawing.Size(22, 23);
            this.btnSttDateCalendar.TabIndex = 21;
            this.btnSttDateCalendar.UseVisualStyleBackColor = true;
            this.btnSttDateCalendar.Click += new System.EventHandler(this.btnSttDateCalendar_Click);
            // 
            // txtEnddate
            // 
            this.txtEnddate.Location = new System.Drawing.Point(206, 6);
            this.txtEnddate.Name = "txtEnddate";
            this.txtEnddate.Size = new System.Drawing.Size(71, 21);
            this.txtEnddate.TabIndex = 22;
            this.txtEnddate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSttdate_KeyDown);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(186, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 12);
            this.label9.TabIndex = 25;
            this.label9.Text = "~";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(12, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "검색기간";
            // 
            // txtSttdate
            // 
            this.txtSttdate.Location = new System.Drawing.Point(81, 6);
            this.txtSttdate.Name = "txtSttdate";
            this.txtSttdate.Size = new System.Drawing.Size(71, 21);
            this.txtSttdate.TabIndex = 20;
            this.txtSttdate.Text = "2020-01-01";
            this.txtSttdate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSttdate_KeyDown);
            // 
            // rbManufacturing
            // 
            this.rbManufacturing.AutoSize = true;
            this.rbManufacturing.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbManufacturing.Location = new System.Drawing.Point(428, 8);
            this.rbManufacturing.Name = "rbManufacturing";
            this.rbManufacturing.Size = new System.Drawing.Size(109, 17);
            this.rbManufacturing.TabIndex = 1;
            this.rbManufacturing.Text = "제조공장(F3)";
            this.rbManufacturing.UseVisualStyleBackColor = true;
            // 
            // rbImporter
            // 
            this.rbImporter.AutoSize = true;
            this.rbImporter.Checked = true;
            this.rbImporter.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbImporter.Location = new System.Drawing.Point(327, 8);
            this.rbImporter.Name = "rbImporter";
            this.rbImporter.Size = new System.Drawing.Size(95, 17);
            this.rbImporter.TabIndex = 0;
            this.rbImporter.TabStop = true;
            this.rbImporter.Text = "수입처(F1)";
            this.rbImporter.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvProduct);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 56);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1614, 635);
            this.panel2.TabIndex = 1;
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToResizeRows = false;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.product_kor,
            this.product_eng});
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.EnableHeadersVisualStyles = false;
            this.dgvProduct.Location = new System.Drawing.Point(0, 0);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.Size = new System.Drawing.Size(1614, 635);
            this.dgvProduct.TabIndex = 0;
            // 
            // product_kor
            // 
            this.product_kor.HeaderText = "품명(한글)";
            this.product_kor.Name = "product_kor";
            this.product_kor.Width = 200;
            // 
            // product_eng
            // 
            this.product_eng.HeaderText = "품명(영어)";
            this.product_eng.Name = "product_eng";
            this.product_eng.Width = 200;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.btnRefresh);
            this.panel3.Controls.Add(this.btnSearchMetrics);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 691);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1614, 41);
            this.panel3.TabIndex = 2;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(223, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(67, 36);
            this.btnExit.TabIndex = 13;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnPrint);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(1420, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(194, 41);
            this.panel4.TabIndex = 18;
            // 
            // btnPrint
            // 
            this.btnPrint.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.Location = new System.Drawing.Point(124, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(67, 36);
            this.btnPrint.TabIndex = 14;
            this.btnPrint.Text = "인쇄(Ctrl+P)";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRefresh.Location = new System.Drawing.Point(135, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(82, 36);
            this.btnRefresh.TabIndex = 17;
            this.btnRefresh.Text = "초기화(F5)";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSearchMetrics
            // 
            this.btnSearchMetrics.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearchMetrics.Location = new System.Drawing.Point(3, 2);
            this.btnSearchMetrics.Name = "btnSearchMetrics";
            this.btnSearchMetrics.Size = new System.Drawing.Size(126, 36);
            this.btnSearchMetrics.TabIndex = 16;
            this.btnSearchMetrics.Text = "매트릭스 검색(Q)";
            this.btnSearchMetrics.UseVisualStyleBackColor = true;
            this.btnSearchMetrics.Click += new System.EventHandler(this.btnSearchMetrics_Click);
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 33);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1614, 23);
            this.panel5.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(11, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "매칭조건";
            // 
            // rbOnlyKorean
            // 
            this.rbOnlyKorean.AutoSize = true;
            this.rbOnlyKorean.Location = new System.Drawing.Point(6, 4);
            this.rbOnlyKorean.Name = "rbOnlyKorean";
            this.rbOnlyKorean.Size = new System.Drawing.Size(97, 16);
            this.rbOnlyKorean.TabIndex = 26;
            this.rbOnlyKorean.Text = "품명(한글) 만";
            this.rbOnlyKorean.UseVisualStyleBackColor = true;
            // 
            // rbOnlyEnglish
            // 
            this.rbOnlyEnglish.AutoSize = true;
            this.rbOnlyEnglish.Location = new System.Drawing.Point(105, 4);
            this.rbOnlyEnglish.Name = "rbOnlyEnglish";
            this.rbOnlyEnglish.Size = new System.Drawing.Size(97, 16);
            this.rbOnlyEnglish.TabIndex = 27;
            this.rbOnlyEnglish.Text = "품명(영문) 만";
            this.rbOnlyEnglish.UseVisualStyleBackColor = true;
            // 
            // rbAllLanguage
            // 
            this.rbAllLanguage.AutoSize = true;
            this.rbAllLanguage.Checked = true;
            this.rbAllLanguage.Location = new System.Drawing.Point(208, 4);
            this.rbAllLanguage.Name = "rbAllLanguage";
            this.rbAllLanguage.Size = new System.Drawing.Size(169, 16);
            this.rbAllLanguage.TabIndex = 28;
            this.rbAllLanguage.TabStop = true;
            this.rbAllLanguage.Text = "품명(한글, 영문) 둘중 하나";
            this.rbAllLanguage.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.rbOnlyEnglish);
            this.panel6.Controls.Add(this.rbAllLanguage);
            this.panel6.Controls.Add(this.rbOnlyKorean);
            this.panel6.Location = new System.Drawing.Point(80, -1);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(379, 23);
            this.panel6.TabIndex = 29;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.rbContain);
            this.panel7.Controls.Add(this.rbExactly);
            this.panel7.Location = new System.Drawing.Point(617, -1);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(207, 23);
            this.panel7.TabIndex = 30;
            // 
            // rbContain
            // 
            this.rbContain.AutoSize = true;
            this.rbContain.Location = new System.Drawing.Point(105, 4);
            this.rbContain.Name = "rbContain";
            this.rbContain.Size = new System.Drawing.Size(87, 16);
            this.rbContain.TabIndex = 27;
            this.rbContain.Text = "문자열 포함";
            this.rbContain.UseVisualStyleBackColor = true;
            // 
            // rbExactly
            // 
            this.rbExactly.AutoSize = true;
            this.rbExactly.Checked = true;
            this.rbExactly.Location = new System.Drawing.Point(6, 4);
            this.rbExactly.Name = "rbExactly";
            this.rbExactly.Size = new System.Drawing.Size(87, 16);
            this.rbExactly.TabIndex = 26;
            this.rbExactly.TabStop = true;
            this.rbExactly.Text = "정확히 일치";
            this.rbExactly.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(546, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "검색조건";
            // 
            // MetricsDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1614, 732);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MetricsDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "해외 제조업소 및 수입처 대시보드";
            this.Load += new System.EventHandler(this.MetricsDashboard_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MetricsDashboard_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProduct;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_kor;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_eng;
        private System.Windows.Forms.RadioButton rbManufacturing;
        private System.Windows.Forms.RadioButton rbImporter;
        private System.Windows.Forms.Button btnSearchMetrics;
        private System.Windows.Forms.Button btnEndDateCalendar;
        private System.Windows.Forms.Button btnSttDateCalendar;
        private System.Windows.Forms.TextBox txtEnddate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSttdate;
        private System.Windows.Forms.Button btnSearchProduct;
        private System.Windows.Forms.Button btnSearchCompany;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.RadioButton rbAllLanguage;
        private System.Windows.Forms.RadioButton rbOnlyEnglish;
        private System.Windows.Forms.RadioButton rbOnlyKorean;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.RadioButton rbContain;
        private System.Windows.Forms.RadioButton rbExactly;
        private System.Windows.Forms.Panel panel6;
    }
}