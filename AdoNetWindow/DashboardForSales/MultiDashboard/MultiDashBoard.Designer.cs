namespace AdoNetWindow.DashboardForSales.MultiDashboard
{
    partial class MultiDashBoard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiDashBoard));
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pnDashboard = new System.Windows.Forms.FlowLayoutPanel();
            this.pnTop = new System.Windows.Forms.Panel();
            this.cbPendingDetail = new System.Windows.Forms.CheckBox();
            this.label25 = new System.Windows.Forms.Label();
            this.cbShipment = new System.Windows.Forms.CheckBox();
            this.cbReserved = new System.Windows.Forms.CheckBox();
            this.cbSeaoverUnpending = new System.Windows.Forms.CheckBox();
            this.cbSeaoverPending = new System.Windows.Forms.CheckBox();
            this.label42 = new System.Windows.Forms.Label();
            this.cbSaleTerm = new System.Windows.Forms.ComboBox();
            this.cbAtoSale = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbPurchasePriceType = new System.Windows.Forms.ComboBox();
            this.txtSalesSttdate = new System.Windows.Forms.TextBox();
            this.label44 = new System.Windows.Forms.Label();
            this.btnSalesSttdate = new System.Windows.Forms.Button();
            this.btnSalesEnddate = new System.Windows.Forms.Button();
            this.txtSalesEnddate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pnBottom = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnProductSort = new System.Windows.Forms.Button();
            this.btnSelectProduct = new System.Windows.Forms.Button();
            this.btnControlRefresh = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panel10 = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnUnitSizeToggle = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pnTop.SuspendLayout();
            this.pnBottom.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.pnTop);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1988, 974);
            this.panel2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.pnDashboard);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 26);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1988, 948);
            this.panel4.TabIndex = 7;
            // 
            // pnDashboard
            // 
            this.pnDashboard.AutoScroll = true;
            this.pnDashboard.BackColor = System.Drawing.SystemColors.HighlightText;
            this.pnDashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnDashboard.Location = new System.Drawing.Point(0, 0);
            this.pnDashboard.Name = "pnDashboard";
            this.pnDashboard.Size = new System.Drawing.Size(1988, 948);
            this.pnDashboard.TabIndex = 1;
            // 
            // pnTop
            // 
            this.pnTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnTop.Controls.Add(this.panel1);
            this.pnTop.Controls.Add(this.cbPendingDetail);
            this.pnTop.Controls.Add(this.label25);
            this.pnTop.Controls.Add(this.cbShipment);
            this.pnTop.Controls.Add(this.cbReserved);
            this.pnTop.Controls.Add(this.cbSeaoverUnpending);
            this.pnTop.Controls.Add(this.cbSeaoverPending);
            this.pnTop.Controls.Add(this.label42);
            this.pnTop.Controls.Add(this.cbSaleTerm);
            this.pnTop.Controls.Add(this.cbAtoSale);
            this.pnTop.Controls.Add(this.label3);
            this.pnTop.Controls.Add(this.cbPurchasePriceType);
            this.pnTop.Controls.Add(this.txtSalesSttdate);
            this.pnTop.Controls.Add(this.label44);
            this.pnTop.Controls.Add(this.btnSalesSttdate);
            this.pnTop.Controls.Add(this.btnSalesEnddate);
            this.pnTop.Controls.Add(this.txtSalesEnddate);
            this.pnTop.Controls.Add(this.label2);
            this.pnTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTop.Location = new System.Drawing.Point(0, 0);
            this.pnTop.Name = "pnTop";
            this.pnTop.Size = new System.Drawing.Size(1988, 26);
            this.pnTop.TabIndex = 0;
            // 
            // cbPendingDetail
            // 
            this.cbPendingDetail.AutoSize = true;
            this.cbPendingDetail.Checked = true;
            this.cbPendingDetail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPendingDetail.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbPendingDetail.Location = new System.Drawing.Point(1379, 6);
            this.cbPendingDetail.Name = "cbPendingDetail";
            this.cbPendingDetail.Size = new System.Drawing.Size(107, 16);
            this.cbPendingDetail.TabIndex = 159;
            this.cbPendingDetail.Text = "임의선적 보기";
            this.cbPendingDetail.UseVisualStyleBackColor = true;
            this.cbPendingDetail.CheckedChanged += new System.EventHandler(this.cbPendingDetail_CheckedChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label25.Location = new System.Drawing.Point(885, 7);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(44, 12);
            this.label25.TabIndex = 154;
            this.label25.Text = "실재고";
            // 
            // cbShipment
            // 
            this.cbShipment.AutoSize = true;
            this.cbShipment.Checked = true;
            this.cbShipment.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShipment.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbShipment.Location = new System.Drawing.Point(935, 6);
            this.cbShipment.Name = "cbShipment";
            this.cbShipment.Size = new System.Drawing.Size(48, 16);
            this.cbShipment.TabIndex = 155;
            this.cbShipment.Text = "선적";
            this.cbShipment.UseVisualStyleBackColor = true;
            this.cbShipment.CheckedChanged += new System.EventHandler(this.cbShipment_CheckedChanged);
            // 
            // cbReserved
            // 
            this.cbReserved.AutoSize = true;
            this.cbReserved.Checked = true;
            this.cbReserved.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbReserved.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbReserved.Location = new System.Drawing.Point(1108, 6);
            this.cbReserved.Name = "cbReserved";
            this.cbReserved.Size = new System.Drawing.Size(48, 16);
            this.cbReserved.TabIndex = 158;
            this.cbReserved.Text = "예약";
            this.cbReserved.UseVisualStyleBackColor = true;
            this.cbReserved.CheckedChanged += new System.EventHandler(this.cbShipment_CheckedChanged);
            // 
            // cbSeaoverUnpending
            // 
            this.cbSeaoverUnpending.AutoSize = true;
            this.cbSeaoverUnpending.Checked = true;
            this.cbSeaoverUnpending.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSeaoverUnpending.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSeaoverUnpending.Location = new System.Drawing.Point(988, 6);
            this.cbSeaoverUnpending.Name = "cbSeaoverUnpending";
            this.cbSeaoverUnpending.Size = new System.Drawing.Size(60, 16);
            this.cbSeaoverUnpending.TabIndex = 156;
            this.cbSeaoverUnpending.Text = "미통관";
            this.cbSeaoverUnpending.UseVisualStyleBackColor = true;
            this.cbSeaoverUnpending.CheckedChanged += new System.EventHandler(this.cbShipment_CheckedChanged);
            // 
            // cbSeaoverPending
            // 
            this.cbSeaoverPending.AutoSize = true;
            this.cbSeaoverPending.Checked = true;
            this.cbSeaoverPending.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSeaoverPending.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSeaoverPending.Location = new System.Drawing.Point(1054, 6);
            this.cbSeaoverPending.Name = "cbSeaoverPending";
            this.cbSeaoverPending.Size = new System.Drawing.Size(48, 16);
            this.cbSeaoverPending.TabIndex = 157;
            this.cbSeaoverPending.Text = "통관";
            this.cbSeaoverPending.UseVisualStyleBackColor = true;
            this.cbSeaoverPending.CheckedChanged += new System.EventHandler(this.cbShipment_CheckedChanged);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label42.Location = new System.Drawing.Point(758, 7);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(57, 12);
            this.label42.TabIndex = 152;
            this.label42.Text = "매출기간";
            // 
            // cbSaleTerm
            // 
            this.cbSaleTerm.FormattingEnabled = true;
            this.cbSaleTerm.Items.AddRange(new object[] {
            "1개월",
            "45일",
            "2개월",
            "3개월",
            "6개월",
            "12개월",
            "18개월"});
            this.cbSaleTerm.Location = new System.Drawing.Point(821, 3);
            this.cbSaleTerm.Name = "cbSaleTerm";
            this.cbSaleTerm.Size = new System.Drawing.Size(58, 20);
            this.cbSaleTerm.TabIndex = 153;
            this.cbSaleTerm.Text = "1개월";
            this.cbSaleTerm.SelectedIndexChanged += new System.EventHandler(this.cbSaleTerm_SelectedIndexChanged);
            // 
            // cbAtoSale
            // 
            this.cbAtoSale.AutoSize = true;
            this.cbAtoSale.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbAtoSale.Location = new System.Drawing.Point(1165, 6);
            this.cbAtoSale.Name = "cbAtoSale";
            this.cbAtoSale.Size = new System.Drawing.Size(198, 16);
            this.cbAtoSale.TabIndex = 151;
            this.cbAtoSale.Text = "매출량 B/L 있는 내역만 집계";
            this.cbAtoSale.UseVisualStyleBackColor = true;
            this.cbAtoSale.CheckedChanged += new System.EventHandler(this.cbAtoSale_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(433, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 12);
            this.label3.TabIndex = 150;
            this.label3.Text = "매입/오퍼/팬딩 구분";
            // 
            // cbPurchasePriceType
            // 
            this.cbPurchasePriceType.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbPurchasePriceType.FormattingEnabled = true;
            this.cbPurchasePriceType.Items.AddRange(new object[] {
            "오퍼가+팬딩+매입",
            "오퍼가+팬딩",
            "오퍼가+매입",
            "매입내역",
            "오퍼가평균"});
            this.cbPurchasePriceType.Location = new System.Drawing.Point(566, 3);
            this.cbPurchasePriceType.Name = "cbPurchasePriceType";
            this.cbPurchasePriceType.Size = new System.Drawing.Size(186, 20);
            this.cbPurchasePriceType.TabIndex = 149;
            this.cbPurchasePriceType.Text = "오퍼가+팬딩";
            this.cbPurchasePriceType.SelectedIndexChanged += new System.EventHandler(this.cbPurchasePriceType_SelectedIndexChanged);
            // 
            // txtSalesSttdate
            // 
            this.txtSalesSttdate.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSalesSttdate.Location = new System.Drawing.Point(203, 3);
            this.txtSalesSttdate.Name = "txtSalesSttdate";
            this.txtSalesSttdate.Size = new System.Drawing.Size(83, 20);
            this.txtSalesSttdate.TabIndex = 143;
            this.txtSalesSttdate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSalesSttdate_KeyDown_1);
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label44.Location = new System.Drawing.Point(309, 8);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(11, 11);
            this.label44.TabIndex = 147;
            this.label44.Text = "~";
            // 
            // btnSalesSttdate
            // 
            this.btnSalesSttdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSalesSttdate.BackgroundImage")));
            this.btnSalesSttdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSalesSttdate.FlatAppearance.BorderSize = 0;
            this.btnSalesSttdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalesSttdate.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSalesSttdate.Location = new System.Drawing.Point(287, 3);
            this.btnSalesSttdate.Name = "btnSalesSttdate";
            this.btnSalesSttdate.Size = new System.Drawing.Size(20, 19);
            this.btnSalesSttdate.TabIndex = 144;
            this.btnSalesSttdate.UseVisualStyleBackColor = true;
            this.btnSalesSttdate.Click += new System.EventHandler(this.btnSalesSttdate_Click);
            // 
            // btnSalesEnddate
            // 
            this.btnSalesEnddate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSalesEnddate.BackgroundImage")));
            this.btnSalesEnddate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSalesEnddate.FlatAppearance.BorderSize = 0;
            this.btnSalesEnddate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalesEnddate.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSalesEnddate.Location = new System.Drawing.Point(405, 3);
            this.btnSalesEnddate.Name = "btnSalesEnddate";
            this.btnSalesEnddate.Size = new System.Drawing.Size(20, 19);
            this.btnSalesEnddate.TabIndex = 146;
            this.btnSalesEnddate.UseVisualStyleBackColor = true;
            this.btnSalesEnddate.Click += new System.EventHandler(this.btnSalesEnddate_Click);
            // 
            // txtSalesEnddate
            // 
            this.txtSalesEnddate.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSalesEnddate.Location = new System.Drawing.Point(321, 3);
            this.txtSalesEnddate.Name = "txtSalesEnddate";
            this.txtSalesEnddate.Size = new System.Drawing.Size(83, 20);
            this.txtSalesEnddate.TabIndex = 145;
            this.txtSalesEnddate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSalesSttdate_KeyDown_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(13, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 12);
            this.label2.TabIndex = 148;
            this.label2.Text = "업체별시세 매출단가 검색기간";
            // 
            // pnBottom
            // 
            this.pnBottom.Controls.Add(this.btnExit);
            this.pnBottom.Controls.Add(this.btnProductSort);
            this.pnBottom.Controls.Add(this.btnSelectProduct);
            this.pnBottom.Controls.Add(this.btnControlRefresh);
            this.pnBottom.Controls.Add(this.btnRefresh);
            this.pnBottom.Controls.Add(this.panel10);
            this.pnBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnBottom.Location = new System.Drawing.Point(0, 974);
            this.pnBottom.Name = "pnBottom";
            this.pnBottom.Size = new System.Drawing.Size(1988, 36);
            this.pnBottom.TabIndex = 3;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(507, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(64, 32);
            this.btnExit.TabIndex = 108;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnProductSort
            // 
            this.btnProductSort.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnProductSort.Location = new System.Drawing.Point(415, 2);
            this.btnProductSort.Name = "btnProductSort";
            this.btnProductSort.Size = new System.Drawing.Size(86, 32);
            this.btnProductSort.TabIndex = 107;
            this.btnProductSort.Text = "품목 정렬";
            this.btnProductSort.UseVisualStyleBackColor = true;
            this.btnProductSort.Click += new System.EventHandler(this.btnProductSort_Click);
            // 
            // btnSelectProduct
            // 
            this.btnSelectProduct.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelectProduct.Location = new System.Drawing.Point(3, 2);
            this.btnSelectProduct.Name = "btnSelectProduct";
            this.btnSelectProduct.Size = new System.Drawing.Size(168, 32);
            this.btnSelectProduct.TabIndex = 106;
            this.btnSelectProduct.Text = "거래처/품목 검색 (F4)";
            this.btnSelectProduct.UseVisualStyleBackColor = true;
            this.btnSelectProduct.Click += new System.EventHandler(this.btnSelectProduct_Click);
            // 
            // btnControlRefresh
            // 
            this.btnControlRefresh.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnControlRefresh.Location = new System.Drawing.Point(269, 2);
            this.btnControlRefresh.Name = "btnControlRefresh";
            this.btnControlRefresh.Size = new System.Drawing.Size(140, 32);
            this.btnControlRefresh.TabIndex = 104;
            this.btnControlRefresh.Text = "선택품목 최신화 (Q)";
            this.btnControlRefresh.UseVisualStyleBackColor = true;
            this.btnControlRefresh.Click += new System.EventHandler(this.btnControlRefresh_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRefresh.Location = new System.Drawing.Point(177, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(86, 32);
            this.btnRefresh.TabIndex = 104;
            this.btnRefresh.Text = "초기화 (F5)";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.btnPrint);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel10.Location = new System.Drawing.Point(1627, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(361, 36);
            this.panel10.TabIndex = 105;
            // 
            // btnPrint
            // 
            this.btnPrint.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.Location = new System.Drawing.Point(264, 2);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(94, 32);
            this.btnPrint.TabIndex = 107;
            this.btnPrint.Text = "인쇄(Ctrl+P)";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnUnitSizeToggle
            // 
            this.btnUnitSizeToggle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUnitSizeToggle.Location = new System.Drawing.Point(5, 0);
            this.btnUnitSizeToggle.Name = "btnUnitSizeToggle";
            this.btnUnitSizeToggle.Size = new System.Drawing.Size(202, 24);
            this.btnUnitSizeToggle.TabIndex = 160;
            this.btnUnitSizeToggle.Text = "대시보드 최대화/최소화 (F12)";
            this.btnUnitSizeToggle.UseVisualStyleBackColor = true;
            this.btnUnitSizeToggle.Click += new System.EventHandler(this.btnUnitSizeToggle_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnUnitSizeToggle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1780, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(206, 24);
            this.panel1.TabIndex = 161;
            // 
            // MultiDashBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1988, 1010);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnBottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MultiDashBoard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "다중 대시보드";
            this.Load += new System.EventHandler(this.MultiDashBoard_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MultiDashBoard_KeyDown);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.pnTop.ResumeLayout(false);
            this.pnTop.PerformLayout();
            this.pnBottom.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.FlowLayoutPanel pnDashboard;
        private System.Windows.Forms.Panel pnTop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSalesSttdate;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Button btnSalesSttdate;
        private System.Windows.Forms.Button btnSalesEnddate;
        private System.Windows.Forms.TextBox txtSalesEnddate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbPurchasePriceType;
        private System.Windows.Forms.CheckBox cbAtoSale;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.ComboBox cbSaleTerm;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.CheckBox cbShipment;
        private System.Windows.Forms.CheckBox cbReserved;
        private System.Windows.Forms.CheckBox cbSeaoverUnpending;
        private System.Windows.Forms.CheckBox cbSeaoverPending;
        private System.Windows.Forms.Panel pnBottom;
        private System.Windows.Forms.Button btnProductSort;
        private System.Windows.Forms.Button btnSelectProduct;
        private System.Windows.Forms.Button btnControlRefresh;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox cbPendingDetail;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnUnitSizeToggle;
        private System.Windows.Forms.Panel panel1;
    }
}