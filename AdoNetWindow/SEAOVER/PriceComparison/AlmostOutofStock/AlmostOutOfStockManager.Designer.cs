namespace AdoNetWindow.SEAOVER.PriceComparison.AlmostOutofStock
{
    partial class AlmostOutOfStockManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlmostOutOfStockManager));
            this.rbNew = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtContetns = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDivision = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSizes = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.cbSaleTerm = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.cbSalesDetail = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbAllSaleTerms = new System.Windows.Forms.CheckBox();
            this.cbStockDetail = new System.Windows.Forms.CheckBox();
            this.rbIncome = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.nudLimitDays = new System.Windows.Forms.NumericUpDown();
            this.rbLimitDaYs = new System.Windows.Forms.RadioButton();
            this.rbNotLimitDaYs = new System.Windows.Forms.RadioButton();
            this.label17 = new System.Windows.Forms.Label();
            this.cbSortType = new System.Windows.Forms.ComboBox();
            this.tabDgv = new System.Windows.Forms.TabControl();
            this.tpNew = new System.Windows.Forms.TabPage();
            this.dgvProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.pnRecord = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.txtTotalRecord = new System.Windows.Forms.TextBox();
            this.txtCurrentRecord = new System.Windows.Forms.TextBox();
            this.tpShortHold = new System.Windows.Forms.TabPage();
            this.tpLongHold = new System.Windows.Forms.TabPage();
            this.tpHide = new System.Windows.Forms.TabPage();
            this.tpAll = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimitDays)).BeginInit();
            this.tabDgv.SuspendLayout();
            this.tpNew.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.pnRecord.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbNew
            // 
            this.rbNew.AutoSize = true;
            this.rbNew.Location = new System.Drawing.Point(609, 6);
            this.rbNew.Name = "rbNew";
            this.rbNew.Size = new System.Drawing.Size(87, 16);
            this.rbNew.TabIndex = 103;
            this.rbNew.Text = "신규 품목만";
            this.rbNew.UseVisualStyleBackColor = true;
            this.rbNew.AppearanceChanged += new System.EventHandler(this.rbAll_AppearanceChanged);
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(439, 6);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(47, 16);
            this.rbAll.TabIndex = 101;
            this.rbAll.Text = "전체";
            this.rbAll.UseVisualStyleBackColor = true;
            this.rbAll.AppearanceChanged += new System.EventHandler(this.rbAll_AppearanceChanged);
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelect.ForeColor = System.Drawing.Color.Black;
            this.btnSelect.Location = new System.Drawing.Point(3, 2);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(70, 36);
            this.btnSelect.TabIndex = 10;
            this.btnSelect.Text = "검색(Q)";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(79, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 36);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSelect);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 894);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1761, 41);
            this.panel3.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txtRemark);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtContetns);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtDivision);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtManager);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtSizes);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtProduct);
            this.panel1.Controls.Add(this.txtUnit);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtOrigin);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1761, 28);
            this.panel1.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(1256, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 12);
            this.label10.TabIndex = 32;
            this.label10.Text = "비고";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(1293, 4);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(116, 21);
            this.txtRemark.TabIndex = 31;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(1097, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 12);
            this.label7.TabIndex = 30;
            this.label7.Text = "내용";
            // 
            // txtContetns
            // 
            this.txtContetns.Location = new System.Drawing.Point(1134, 4);
            this.txtContetns.Name = "txtContetns";
            this.txtContetns.Size = new System.Drawing.Size(116, 21);
            this.txtContetns.TabIndex = 29;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(939, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 12);
            this.label8.TabIndex = 28;
            this.label8.Text = "구분";
            // 
            // txtDivision
            // 
            this.txtDivision.Location = new System.Drawing.Point(976, 4);
            this.txtDivision.Name = "txtDivision";
            this.txtDivision.Size = new System.Drawing.Size(116, 21);
            this.txtDivision.TabIndex = 10;
            this.txtDivision.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(766, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 12);
            this.label1.TabIndex = 26;
            this.label1.Text = "담당자";
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(816, 4);
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(116, 21);
            this.txtManager.TabIndex = 8;
            this.txtManager.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(448, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 12);
            this.label4.TabIndex = 21;
            this.label4.Text = "규격";
            // 
            // txtSizes
            // 
            this.txtSizes.Location = new System.Drawing.Point(485, 4);
            this.txtSizes.Name = "txtSizes";
            this.txtSizes.Size = new System.Drawing.Size(116, 21);
            this.txtSizes.TabIndex = 4;
            this.txtSizes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(607, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 12);
            this.label5.TabIndex = 22;
            this.label5.Text = "단위";
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(45, 4);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(228, 21);
            this.txtProduct.TabIndex = 0;
            this.txtProduct.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(644, 4);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(116, 21);
            this.txtUnit.TabIndex = 6;
            this.txtUnit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(8, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 12);
            this.label9.TabIndex = 24;
            this.label9.Text = "품명";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(277, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 12);
            this.label6.TabIndex = 23;
            this.label6.Text = "원산지";
            // 
            // txtOrigin
            // 
            this.txtOrigin.Location = new System.Drawing.Point(326, 4);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.Size = new System.Drawing.Size(116, 21);
            this.txtOrigin.TabIndex = 2;
            this.txtOrigin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
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
            this.cbSaleTerm.Location = new System.Drawing.Point(379, 4);
            this.cbSaleTerm.Name = "cbSaleTerm";
            this.cbSaleTerm.Size = new System.Drawing.Size(58, 20);
            this.cbSaleTerm.TabIndex = 102;
            this.cbSaleTerm.Text = "45일";
            this.cbSaleTerm.SelectedIndexChanged += new System.EventHandler(this.cbSaleTerm_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(316, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 12);
            this.label2.TabIndex = 101;
            this.label2.Text = "매출기간";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.cbSalesDetail);
            this.panel7.Controls.Add(this.label3);
            this.panel7.Controls.Add(this.cbAllSaleTerms);
            this.panel7.Controls.Add(this.cbStockDetail);
            this.panel7.Controls.Add(this.rbNew);
            this.panel7.Controls.Add(this.rbIncome);
            this.panel7.Controls.Add(this.rbAll);
            this.panel7.Controls.Add(this.panel4);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1761, 28);
            this.panel7.TabIndex = 7;
            // 
            // cbSalesDetail
            // 
            this.cbSalesDetail.AutoSize = true;
            this.cbSalesDetail.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSalesDetail.Location = new System.Drawing.Point(125, 7);
            this.cbSalesDetail.Name = "cbSalesDetail";
            this.cbSalesDetail.Size = new System.Drawing.Size(108, 16);
            this.cbSalesDetail.TabIndex = 107;
            this.cbSalesDetail.Text = "판매상세 (F3)";
            this.cbSalesDetail.UseVisualStyleBackColor = true;
            this.cbSalesDetail.CheckedChanged += new System.EventHandler(this.cbSalesDetail_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(376, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 12);
            this.label3.TabIndex = 106;
            this.label3.Text = "품목타입";
            // 
            // cbAllSaleTerms
            // 
            this.cbAllSaleTerms.AutoSize = true;
            this.cbAllSaleTerms.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbAllSaleTerms.Location = new System.Drawing.Point(239, 7);
            this.cbAllSaleTerms.Name = "cbAllSaleTerms";
            this.cbAllSaleTerms.Size = new System.Drawing.Size(126, 16);
            this.cbAllSaleTerms.TabIndex = 105;
            this.cbAllSaleTerms.Text = "전체 판매량 (F4)";
            this.cbAllSaleTerms.UseVisualStyleBackColor = true;
            this.cbAllSaleTerms.CheckedChanged += new System.EventHandler(this.cbAllSaleTerms_CheckedChanged);
            // 
            // cbStockDetail
            // 
            this.cbStockDetail.AutoSize = true;
            this.cbStockDetail.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbStockDetail.Location = new System.Drawing.Point(11, 7);
            this.cbStockDetail.Name = "cbStockDetail";
            this.cbStockDetail.Size = new System.Drawing.Size(108, 16);
            this.cbStockDetail.TabIndex = 104;
            this.cbStockDetail.Text = "재고상세 (F1)";
            this.cbStockDetail.UseVisualStyleBackColor = true;
            this.cbStockDetail.CheckedChanged += new System.EventHandler(this.cbStockDetail_CheckedChanged);
            // 
            // rbIncome
            // 
            this.rbIncome.AutoSize = true;
            this.rbIncome.Checked = true;
            this.rbIncome.Location = new System.Drawing.Point(492, 6);
            this.rbIncome.Name = "rbIncome";
            this.rbIncome.Size = new System.Drawing.Size(111, 16);
            this.rbIncome.TabIndex = 102;
            this.rbIncome.TabStop = true;
            this.rbIncome.Text = "수입했던 품목만";
            this.rbIncome.UseVisualStyleBackColor = true;
            this.rbIncome.AppearanceChanged += new System.EventHandler(this.rbAll_AppearanceChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.cbSaleTerm);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label17);
            this.panel4.Controls.Add(this.cbSortType);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(976, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(785, 28);
            this.panel4.TabIndex = 100;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label12);
            this.panel6.Controls.Add(this.nudLimitDays);
            this.panel6.Controls.Add(this.rbLimitDaYs);
            this.panel6.Controls.Add(this.rbNotLimitDaYs);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(310, 28);
            this.panel6.TabIndex = 104;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(225, 8);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(45, 12);
            this.label12.TabIndex = 107;
            this.label12.Text = "일 미만";
            // 
            // nudLimitDays
            // 
            this.nudLimitDays.Location = new System.Drawing.Point(174, 4);
            this.nudLimitDays.Name = "nudLimitDays";
            this.nudLimitDays.Size = new System.Drawing.Size(49, 21);
            this.nudLimitDays.TabIndex = 106;
            this.nudLimitDays.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // rbLimitDaYs
            // 
            this.rbLimitDaYs.AutoSize = true;
            this.rbLimitDaYs.Checked = true;
            this.rbLimitDaYs.Location = new System.Drawing.Point(115, 6);
            this.rbLimitDaYs.Name = "rbLimitDaYs";
            this.rbLimitDaYs.Size = new System.Drawing.Size(59, 16);
            this.rbLimitDaYs.TabIndex = 105;
            this.rbLimitDaYs.TabStop = true;
            this.rbLimitDaYs.Text = "임박일";
            this.rbLimitDaYs.UseVisualStyleBackColor = true;
            // 
            // rbNotLimitDaYs
            // 
            this.rbNotLimitDaYs.AutoSize = true;
            this.rbNotLimitDaYs.Location = new System.Drawing.Point(14, 6);
            this.rbNotLimitDaYs.Name = "rbNotLimitDaYs";
            this.rbNotLimitDaYs.Size = new System.Drawing.Size(83, 16);
            this.rbNotLimitDaYs.TabIndex = 104;
            this.rbNotLimitDaYs.Text = "임박 제한X";
            this.rbNotLimitDaYs.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label17.Location = new System.Drawing.Point(455, 8);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(57, 12);
            this.label17.TabIndex = 52;
            this.label17.Text = "정렬기준";
            // 
            // cbSortType
            // 
            this.cbSortType.FormattingEnabled = true;
            this.cbSortType.Items.AddRange(new object[] {
            "신규+추천계약일+품목+원산지+사이즈+단위",
            "신규+추천계약일+원산지+품목+사이즈+단위",
            "신규+쇼트일자+품목+원산지+사이즈+단위",
            "신규+쇼트일자+원산지+품목+사이즈+단위",
            "추천계약일+품목+원산지+사이즈+단위",
            "추천계약일+원산지+품목+사이즈+단위",
            "쇼트일자+품목+원산지+사이즈+단위",
            "쇼트일자+원산지+품목+사이즈+단위",
            "품목+원산지+사이즈+단위",
            "원산지+품목+사이즈+단위"});
            this.cbSortType.Location = new System.Drawing.Point(518, 4);
            this.cbSortType.Name = "cbSortType";
            this.cbSortType.Size = new System.Drawing.Size(263, 20);
            this.cbSortType.TabIndex = 51;
            this.cbSortType.Text = "신규+추천계약일+품목+원산지+사이즈+단위";
            this.cbSortType.TextChanged += new System.EventHandler(this.cbSortType_TextChanged);
            // 
            // tabDgv
            // 
            this.tabDgv.Controls.Add(this.tpNew);
            this.tabDgv.Controls.Add(this.tpShortHold);
            this.tabDgv.Controls.Add(this.tpLongHold);
            this.tabDgv.Controls.Add(this.tpHide);
            this.tabDgv.Controls.Add(this.tpAll);
            this.tabDgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDgv.Location = new System.Drawing.Point(0, 0);
            this.tabDgv.Name = "tabDgv";
            this.tabDgv.SelectedIndex = 0;
            this.tabDgv.Size = new System.Drawing.Size(1761, 838);
            this.tabDgv.TabIndex = 4;
            this.tabDgv.SelectedIndexChanged += new System.EventHandler(this.tabDgv_SelectedIndexChanged);
            // 
            // tpNew
            // 
            this.tpNew.Controls.Add(this.dgvProduct);
            this.tpNew.Controls.Add(this.pnRecord);
            this.tpNew.Location = new System.Drawing.Point(4, 22);
            this.tpNew.Name = "tpNew";
            this.tpNew.Size = new System.Drawing.Size(1753, 812);
            this.tpNew.TabIndex = 0;
            this.tpNew.Text = "계약임박";
            this.tpNew.UseVisualStyleBackColor = true;
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToResizeRows = false;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.EnableHeadersVisualStyles = false;
            this.dgvProduct.Location = new System.Drawing.Point(0, 0);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowHeadersWidth = 70;
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.Size = new System.Drawing.Size(1753, 791);
            this.dgvProduct.TabIndex = 0;
            this.dgvProduct.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProduct_CellMouseClick);
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            this.dgvProduct.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProduct_ColumnHeaderMouseDoubleClick);
            this.dgvProduct.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvProduct_RowPostPaint);
            this.dgvProduct.SelectionChanged += new System.EventHandler(this.dgvProduct_SelectionChanged);
            this.dgvProduct.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dgvProduct_SortCompare);
            this.dgvProduct.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvProduct_MouseUp);
            // 
            // pnRecord
            // 
            this.pnRecord.Controls.Add(this.label11);
            this.pnRecord.Controls.Add(this.txtTotalRecord);
            this.pnRecord.Controls.Add(this.txtCurrentRecord);
            this.pnRecord.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnRecord.Location = new System.Drawing.Point(0, 791);
            this.pnRecord.Name = "pnRecord";
            this.pnRecord.Size = new System.Drawing.Size(1753, 21);
            this.pnRecord.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(58, 5);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(12, 12);
            this.label11.TabIndex = 2;
            this.label11.Text = "/";
            // 
            // txtTotalRecord
            // 
            this.txtTotalRecord.Location = new System.Drawing.Point(75, 0);
            this.txtTotalRecord.Name = "txtTotalRecord";
            this.txtTotalRecord.Size = new System.Drawing.Size(53, 21);
            this.txtTotalRecord.TabIndex = 1;
            this.txtTotalRecord.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtCurrentRecord
            // 
            this.txtCurrentRecord.Location = new System.Drawing.Point(0, 0);
            this.txtCurrentRecord.Name = "txtCurrentRecord";
            this.txtCurrentRecord.Size = new System.Drawing.Size(53, 21);
            this.txtCurrentRecord.TabIndex = 0;
            this.txtCurrentRecord.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tpShortHold
            // 
            this.tpShortHold.Location = new System.Drawing.Point(4, 22);
            this.tpShortHold.Name = "tpShortHold";
            this.tpShortHold.Size = new System.Drawing.Size(1753, 812);
            this.tpShortHold.TabIndex = 4;
            this.tpShortHold.Text = "단기보류";
            this.tpShortHold.UseVisualStyleBackColor = true;
            // 
            // tpLongHold
            // 
            this.tpLongHold.Location = new System.Drawing.Point(4, 22);
            this.tpLongHold.Name = "tpLongHold";
            this.tpLongHold.Size = new System.Drawing.Size(1753, 812);
            this.tpLongHold.TabIndex = 5;
            this.tpLongHold.Text = "장기보류";
            this.tpLongHold.UseVisualStyleBackColor = true;
            // 
            // tpHide
            // 
            this.tpHide.Location = new System.Drawing.Point(4, 22);
            this.tpHide.Name = "tpHide";
            this.tpHide.Size = new System.Drawing.Size(1753, 812);
            this.tpHide.TabIndex = 2;
            this.tpHide.Text = "취급X";
            this.tpHide.UseVisualStyleBackColor = true;
            // 
            // tpAll
            // 
            this.tpAll.Location = new System.Drawing.Point(4, 22);
            this.tpAll.Name = "tpAll";
            this.tpAll.Size = new System.Drawing.Size(1753, 812);
            this.tpAll.TabIndex = 3;
            this.tpAll.Text = "전체";
            this.tpAll.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabDgv);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 56);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1761, 838);
            this.panel2.TabIndex = 5;
            // 
            // AlmostOutOfStockManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1761, 935);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "AlmostOutOfStockManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "품절/계약 임박 품목리스트";
            this.Load += new System.EventHandler(this.AlmostOutOfStockManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AlmostOutOfStockManager_KeyDown);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimitDays)).EndInit();
            this.tabDgv.ResumeLayout(false);
            this.tpNew.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.pnRecord.ResumeLayout(false);
            this.pnRecord.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RadioButton rbNew;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbSaleTerm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDivision;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSizes;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.RadioButton rbIncome;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox cbSortType;
        private System.Windows.Forms.TabControl tabDgv;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabPage tpHide;
        private System.Windows.Forms.TabPage tpAll;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbAllSaleTerms;
        private System.Windows.Forms.CheckBox cbStockDetail;
        private System.Windows.Forms.CheckBox cbSalesDetail;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtContetns;
        private System.Windows.Forms.TabPage tpNew;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProduct;
        private System.Windows.Forms.TabPage tpShortHold;
        private System.Windows.Forms.TabPage tpLongHold;
        private System.Windows.Forms.Panel pnRecord;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtTotalRecord;
        private System.Windows.Forms.TextBox txtCurrentRecord;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.NumericUpDown nudLimitDays;
        private System.Windows.Forms.RadioButton rbLimitDaYs;
        private System.Windows.Forms.RadioButton rbNotLimitDaYs;
        private System.Windows.Forms.Label label12;
    }
}