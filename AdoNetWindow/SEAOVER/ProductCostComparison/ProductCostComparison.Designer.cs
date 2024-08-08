namespace AdoNetWindow.SEAOVER.ProductCostComparison
{
    partial class ProductCostComparison
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductCostComparison));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtDivision = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cbDongwon = new System.Windows.Forms.CheckBox();
            this.cbPendingInStock = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSizes = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.txtEnddate = new System.Windows.Forms.TextBox();
            this.btnEnddateCalendar = new System.Windows.Forms.Button();
            this.cbCp = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnSttdateCalendar = new System.Windows.Forms.Button();
            this.txtSttdate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTrq = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shipment_stock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reserved_stock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reserved_stock_detail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.real_stock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sales_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sales_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.day_sales_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.month_sales_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.month_around = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.average_cost_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sales_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchase_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.normal_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.offer_updatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.offer_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.offer_company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exchange_rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.custom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.incidental_expense = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.offer_cost_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.order_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_stock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_average_cost_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_month_around = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exhausted_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.btnSearching = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label18 = new System.Windows.Forms.Label();
            this.rbSixMonths = new System.Windows.Forms.RadioButton();
            this.rbeighteenMonths = new System.Windows.Forms.RadioButton();
            this.rbtwelveMonths = new System.Windows.Forms.RadioButton();
            this.panel6 = new System.Windows.Forms.Panel();
            this.cbOffer = new System.Windows.Forms.CheckBox();
            this.cbSales = new System.Windows.Forms.CheckBox();
            this.cbStock = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rbTrq = new System.Windows.Forms.RadioButton();
            this.txtExchangeRate = new System.Windows.Forms.TextBox();
            this.rbCostprice = new System.Windows.Forms.RadioButton();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.btnAdvancedSearch = new System.Windows.Forms.Button();
            this.pnAdvancedSearch = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnAdvancedSearchExit = new System.Windows.Forms.Button();
            this.cbStockZero = new System.Windows.Forms.CheckBox();
            this.cbStockExist = new System.Windows.Forms.CheckBox();
            this.txtEndPrice = new System.Windows.Forms.TextBox();
            this.cbPriceTypeDropdown = new System.Windows.Forms.ComboBox();
            this.cbPurchasePrice = new System.Windows.Forms.CheckBox();
            this.cbRoundStock = new System.Windows.Forms.CheckBox();
            this.cbSalesPrice = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtSttPrice = new System.Windows.Forms.TextBox();
            this.nudEndRound = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.nudSttRound = new System.Windows.Forms.NumericUpDown();
            this.cbReservedStock = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel10.SuspendLayout();
            this.pnAdvancedSearch.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEndRound)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSttRound)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtDivision);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtManager);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtSizes);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtProduct);
            this.panel1.Controls.Add(this.txtUnit);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtOrigin);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1763, 31);
            this.panel1.TabIndex = 0;
            // 
            // txtDivision
            // 
            this.txtDivision.Location = new System.Drawing.Point(803, 4);
            this.txtDivision.Name = "txtDivision";
            this.txtDivision.Size = new System.Drawing.Size(116, 21);
            this.txtDivision.TabIndex = 25;
            this.txtDivision.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDivision_KeyDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(766, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 12);
            this.label8.TabIndex = 109;
            this.label8.Text = "구분";
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(975, 4);
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(116, 21);
            this.txtManager.TabIndex = 29;
            this.txtManager.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDivision_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(925, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 12);
            this.label7.TabIndex = 107;
            this.label7.Text = "담당자";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.Controls.Add(this.cbDongwon);
            this.panel5.Controls.Add(this.cbPendingInStock);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(1271, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(490, 29);
            this.panel5.TabIndex = 105;
            // 
            // cbDongwon
            // 
            this.cbDongwon.AutoSize = true;
            this.cbDongwon.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbDongwon.Location = new System.Drawing.Point(437, 7);
            this.cbDongwon.Name = "cbDongwon";
            this.cbDongwon.Size = new System.Drawing.Size(50, 16);
            this.cbDongwon.TabIndex = 114;
            this.cbDongwon.Text = "동원";
            this.cbDongwon.UseVisualStyleBackColor = true;
            this.cbDongwon.CheckedChanged += new System.EventHandler(this.cbDongwon_CheckedChanged);
            // 
            // cbPendingInStock
            // 
            this.cbPendingInStock.AutoSize = true;
            this.cbPendingInStock.Checked = true;
            this.cbPendingInStock.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPendingInStock.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbPendingInStock.Location = new System.Drawing.Point(312, 8);
            this.cbPendingInStock.Name = "cbPendingInStock";
            this.cbPendingInStock.Size = new System.Drawing.Size(120, 16);
            this.cbPendingInStock.TabIndex = 113;
            this.cbPendingInStock.Text = "선적수 재고포함";
            this.cbPendingInStock.UseVisualStyleBackColor = true;
            this.cbPendingInStock.CheckedChanged += new System.EventHandler(this.cbPendingInStock_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(450, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 12);
            this.label4.TabIndex = 25;
            this.label4.Text = "규격";
            // 
            // txtSizes
            // 
            this.txtSizes.Location = new System.Drawing.Point(487, 4);
            this.txtSizes.Name = "txtSizes";
            this.txtSizes.Size = new System.Drawing.Size(116, 21);
            this.txtSizes.TabIndex = 21;
            this.txtSizes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDivision_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(609, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 12);
            this.label5.TabIndex = 29;
            this.label5.Text = "단위";
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(44, 4);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(228, 21);
            this.txtProduct.TabIndex = 0;
            this.txtProduct.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDivision_KeyDown);
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(646, 4);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(116, 21);
            this.txtUnit.TabIndex = 23;
            this.txtUnit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDivision_KeyDown);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(7, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 12);
            this.label9.TabIndex = 33;
            this.label9.Text = "품명";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(278, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 12);
            this.label6.TabIndex = 30;
            this.label6.Text = "원산지";
            // 
            // txtOrigin
            // 
            this.txtOrigin.Location = new System.Drawing.Point(328, 4);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.Size = new System.Drawing.Size(116, 21);
            this.txtOrigin.TabIndex = 1;
            this.txtOrigin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDivision_KeyDown);
            // 
            // txtEnddate
            // 
            this.txtEnddate.Location = new System.Drawing.Point(378, 4);
            this.txtEnddate.MaxLength = 10;
            this.txtEnddate.Name = "txtEnddate";
            this.txtEnddate.Size = new System.Drawing.Size(82, 21);
            this.txtEnddate.TabIndex = 103;
            this.txtEnddate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEnddate_KeyDown);
            // 
            // btnEnddateCalendar
            // 
            this.btnEnddateCalendar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEnddateCalendar.BackgroundImage")));
            this.btnEnddateCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEnddateCalendar.FlatAppearance.BorderSize = 0;
            this.btnEnddateCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnddateCalendar.Location = new System.Drawing.Point(461, 2);
            this.btnEnddateCalendar.Name = "btnEnddateCalendar";
            this.btnEnddateCalendar.Size = new System.Drawing.Size(22, 23);
            this.btnEnddateCalendar.TabIndex = 104;
            this.btnEnddateCalendar.UseVisualStyleBackColor = true;
            this.btnEnddateCalendar.Click += new System.EventHandler(this.btnEnddateCalendar_Click);
            // 
            // cbCp
            // 
            this.cbCp.FormattingEnabled = true;
            this.cbCp.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cbCp.Location = new System.Drawing.Point(119, 5);
            this.cbCp.Name = "cbCp";
            this.cbCp.Size = new System.Drawing.Size(34, 20);
            this.cbCp.TabIndex = 35;
            this.cbCp.Text = "3";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(12, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 12);
            this.label10.TabIndex = 43;
            this.label10.Text = "일반시세 업체수";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(359, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 12);
            this.label1.TabIndex = 102;
            this.label1.Text = "~";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(158, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 12);
            this.label12.TabIndex = 45;
            this.label12.Text = "일반시세 기간";
            // 
            // btnSttdateCalendar
            // 
            this.btnSttdateCalendar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSttdateCalendar.BackgroundImage")));
            this.btnSttdateCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSttdateCalendar.FlatAppearance.BorderSize = 0;
            this.btnSttdateCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSttdateCalendar.Location = new System.Drawing.Point(335, 2);
            this.btnSttdateCalendar.Name = "btnSttdateCalendar";
            this.btnSttdateCalendar.Size = new System.Drawing.Size(22, 23);
            this.btnSttdateCalendar.TabIndex = 101;
            this.btnSttdateCalendar.UseVisualStyleBackColor = true;
            this.btnSttdateCalendar.Click += new System.EventHandler(this.btnSttdateCalendar_Click);
            // 
            // txtSttdate
            // 
            this.txtSttdate.Location = new System.Drawing.Point(252, 4);
            this.txtSttdate.MaxLength = 10;
            this.txtSttdate.Name = "txtSttdate";
            this.txtSttdate.Size = new System.Drawing.Size(82, 21);
            this.txtSttdate.TabIndex = 100;
            this.txtSttdate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEnddate_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(132, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 12);
            this.label2.TabIndex = 105;
            this.label2.Text = "TRQ 금액";
            // 
            // txtTrq
            // 
            this.txtTrq.Location = new System.Drawing.Point(202, 4);
            this.txtTrq.MaxLength = 10;
            this.txtTrq.Name = "txtTrq";
            this.txtTrq.Size = new System.Drawing.Size(82, 21);
            this.txtTrq.TabIndex = 106;
            this.txtTrq.Text = "2,000";
            this.txtTrq.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvProduct);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 124);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1763, 667);
            this.panel2.TabIndex = 1;
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToOrderColumns = true;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.product,
            this.origin,
            this.sizes,
            this.unit,
            this.cost_unit,
            this.shipment_stock,
            this.stock,
            this.reserved_stock,
            this.reserved_stock_detail,
            this.real_stock,
            this.sales_count,
            this.sales_amount,
            this.day_sales_count,
            this.month_sales_count,
            this.month_around,
            this.average_cost_price,
            this.sales_price,
            this.purchase_price,
            this.normal_price,
            this.offer_updatetime,
            this.offer_price,
            this.offer_company,
            this.exchange_rate,
            this.custom,
            this.tax,
            this.incidental_expense,
            this.offer_cost_price,
            this.order_qty,
            this.total_stock,
            this.total_average_cost_price,
            this.total_month_around,
            this.exhausted_date,
            this.manager});
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.EnableHeadersVisualStyles = false;
            this.dgvProduct.Location = new System.Drawing.Point(0, 0);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.Size = new System.Drawing.Size(1763, 667);
            this.dgvProduct.TabIndex = 3;
            this.dgvProduct.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellDoubleClick);
            this.dgvProduct.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProduct_CellMouseClick);
            this.dgvProduct.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellMouseEnter);
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            this.dgvProduct.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvProduct_MouseUp);
            // 
            // product
            // 
            this.product.HeaderText = "품명";
            this.product.Name = "product";
            // 
            // origin
            // 
            this.origin.HeaderText = "원산지";
            this.origin.Name = "origin";
            // 
            // sizes
            // 
            this.sizes.HeaderText = "규격";
            this.sizes.Name = "sizes";
            // 
            // unit
            // 
            this.unit.HeaderText = "단위";
            this.unit.Name = "unit";
            // 
            // cost_unit
            // 
            this.cost_unit.HeaderText = "트레이";
            this.cost_unit.Name = "cost_unit";
            // 
            // shipment_stock
            // 
            this.shipment_stock.HeaderText = "선적수";
            this.shipment_stock.Name = "shipment_stock";
            // 
            // stock
            // 
            this.stock.HeaderText = "재고수";
            this.stock.Name = "stock";
            // 
            // reserved_stock
            // 
            this.reserved_stock.HeaderText = "예약수";
            this.reserved_stock.Name = "reserved_stock";
            // 
            // reserved_stock_detail
            // 
            this.reserved_stock_detail.HeaderText = "예약상세";
            this.reserved_stock_detail.Name = "reserved_stock_detail";
            // 
            // real_stock
            // 
            this.real_stock.HeaderText = "실재고";
            this.real_stock.Name = "real_stock";
            // 
            // sales_count
            // 
            this.sales_count.HeaderText = "매출수";
            this.sales_count.Name = "sales_count";
            // 
            // sales_amount
            // 
            this.sales_amount.HeaderText = "매출금액";
            this.sales_amount.Name = "sales_amount";
            // 
            // day_sales_count
            // 
            this.day_sales_count.HeaderText = "일 판매량";
            this.day_sales_count.Name = "day_sales_count";
            // 
            // month_sales_count
            // 
            this.month_sales_count.HeaderText = "월 판매량";
            this.month_sales_count.Name = "month_sales_count";
            // 
            // month_around
            // 
            this.month_around.HeaderText = "회전율";
            this.month_around.Name = "month_around";
            // 
            // average_cost_price
            // 
            this.average_cost_price.HeaderText = "평균원가";
            this.average_cost_price.Name = "average_cost_price";
            // 
            // sales_price
            // 
            this.sales_price.HeaderText = "매출가";
            this.sales_price.Name = "sales_price";
            // 
            // purchase_price
            // 
            this.purchase_price.HeaderText = "최저가";
            this.purchase_price.Name = "purchase_price";
            // 
            // normal_price
            // 
            this.normal_price.HeaderText = "일반시세";
            this.normal_price.Name = "normal_price";
            // 
            // offer_updatetime
            // 
            this.offer_updatetime.HeaderText = "기준일자";
            this.offer_updatetime.Name = "offer_updatetime";
            // 
            // offer_price
            // 
            this.offer_price.HeaderText = "오퍼가";
            this.offer_price.Name = "offer_price";
            // 
            // offer_company
            // 
            this.offer_company.HeaderText = "거래처";
            this.offer_company.Name = "offer_company";
            // 
            // exchange_rate
            // 
            this.exchange_rate.HeaderText = "환율";
            this.exchange_rate.Name = "exchange_rate";
            // 
            // custom
            // 
            this.custom.HeaderText = "관세";
            this.custom.Name = "custom";
            // 
            // tax
            // 
            this.tax.HeaderText = "과세";
            this.tax.Name = "tax";
            // 
            // incidental_expense
            // 
            this.incidental_expense.HeaderText = "부대비용";
            this.incidental_expense.Name = "incidental_expense";
            // 
            // offer_cost_price
            // 
            this.offer_cost_price.HeaderText = "원가계산";
            this.offer_cost_price.Name = "offer_cost_price";
            // 
            // order_qty
            // 
            this.order_qty.HeaderText = "오더수량";
            this.order_qty.Name = "order_qty";
            // 
            // total_stock
            // 
            this.total_stock.HeaderText = "수량합계";
            this.total_stock.Name = "total_stock";
            // 
            // total_average_cost_price
            // 
            this.total_average_cost_price.HeaderText = "평균원가";
            this.total_average_cost_price.Name = "total_average_cost_price";
            // 
            // total_month_around
            // 
            this.total_month_around.HeaderText = "회전율";
            this.total_month_around.Name = "total_month_around";
            // 
            // exhausted_date
            // 
            this.exhausted_date.HeaderText = "소진일자";
            this.exhausted_date.Name = "exhausted_date";
            // 
            // manager
            // 
            this.manager.HeaderText = "담당자";
            this.manager.Name = "manager";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnCalculate);
            this.panel3.Controls.Add(this.btnSearching);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 791);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1763, 44);
            this.panel3.TabIndex = 2;
            // 
            // btnCalculate
            // 
            this.btnCalculate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCalculate.ForeColor = System.Drawing.Color.Black;
            this.btnCalculate.Location = new System.Drawing.Point(84, 3);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(119, 37);
            this.btnCalculate.TabIndex = 8;
            this.btnCalculate.Text = "소진일자 계산(W)";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.ForeColor = System.Drawing.Color.Black;
            this.btnSearching.Location = new System.Drawing.Point(3, 3);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(75, 37);
            this.btnSearching.TabIndex = 4;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(209, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 37);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.panel7);
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.cbOffer);
            this.panel4.Controls.Add(this.cbSales);
            this.panel4.Controls.Add(this.cbStock);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1763, 31);
            this.panel4.TabIndex = 3;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label18);
            this.panel7.Controls.Add(this.rbSixMonths);
            this.panel7.Controls.Add(this.rbeighteenMonths);
            this.panel7.Controls.Add(this.rbtwelveMonths);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel7.Location = new System.Drawing.Point(987, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(285, 29);
            this.panel7.TabIndex = 113;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label18.Location = new System.Drawing.Point(19, 9);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(57, 12);
            this.label18.TabIndex = 112;
            this.label18.Text = "매출기간";
            // 
            // rbSixMonths
            // 
            this.rbSixMonths.AutoSize = true;
            this.rbSixMonths.Checked = true;
            this.rbSixMonths.Location = new System.Drawing.Point(82, 7);
            this.rbSixMonths.Name = "rbSixMonths";
            this.rbSixMonths.Size = new System.Drawing.Size(53, 16);
            this.rbSixMonths.TabIndex = 109;
            this.rbSixMonths.TabStop = true;
            this.rbSixMonths.Text = "6개월";
            this.rbSixMonths.UseVisualStyleBackColor = true;
            // 
            // rbeighteenMonths
            // 
            this.rbeighteenMonths.AutoSize = true;
            this.rbeighteenMonths.Location = new System.Drawing.Point(206, 7);
            this.rbeighteenMonths.Name = "rbeighteenMonths";
            this.rbeighteenMonths.Size = new System.Drawing.Size(59, 16);
            this.rbeighteenMonths.TabIndex = 111;
            this.rbeighteenMonths.Text = "18개월";
            this.rbeighteenMonths.UseVisualStyleBackColor = true;
            // 
            // rbtwelveMonths
            // 
            this.rbtwelveMonths.AutoSize = true;
            this.rbtwelveMonths.Location = new System.Drawing.Point(141, 7);
            this.rbtwelveMonths.Name = "rbtwelveMonths";
            this.rbtwelveMonths.Size = new System.Drawing.Size(59, 16);
            this.rbtwelveMonths.TabIndex = 110;
            this.rbtwelveMonths.Text = "12개월";
            this.rbtwelveMonths.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.txtEnddate);
            this.panel6.Controls.Add(this.btnEnddateCalendar);
            this.panel6.Controls.Add(this.txtSttdate);
            this.panel6.Controls.Add(this.cbCp);
            this.panel6.Controls.Add(this.btnSttdateCalendar);
            this.panel6.Controls.Add(this.label10);
            this.panel6.Controls.Add(this.label12);
            this.panel6.Controls.Add(this.label1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(1272, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(489, 29);
            this.panel6.TabIndex = 106;
            // 
            // cbOffer
            // 
            this.cbOffer.AutoSize = true;
            this.cbOffer.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbOffer.Location = new System.Drawing.Point(289, 8);
            this.cbOffer.Name = "cbOffer";
            this.cbOffer.Size = new System.Drawing.Size(134, 16);
            this.cbOffer.TabIndex = 2;
            this.cbOffer.Text = "오퍼내역 상세(F3)";
            this.cbOffer.UseVisualStyleBackColor = true;
            this.cbOffer.CheckedChanged += new System.EventHandler(this.cbOffer_CheckedChanged);
            // 
            // cbSales
            // 
            this.cbSales.AutoSize = true;
            this.cbSales.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSales.Location = new System.Drawing.Point(149, 8);
            this.cbSales.Name = "cbSales";
            this.cbSales.Size = new System.Drawing.Size(134, 16);
            this.cbSales.TabIndex = 1;
            this.cbSales.Text = "판매내역 상세(F2)";
            this.cbSales.UseVisualStyleBackColor = true;
            this.cbSales.CheckStateChanged += new System.EventHandler(this.cbSales_CheckStateChanged);
            // 
            // cbStock
            // 
            this.cbStock.AutoSize = true;
            this.cbStock.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbStock.Location = new System.Drawing.Point(9, 8);
            this.cbStock.Name = "cbStock";
            this.cbStock.Size = new System.Drawing.Size(134, 16);
            this.cbStock.TabIndex = 0;
            this.cbStock.Text = "재고내역 상세(F1)";
            this.cbStock.UseVisualStyleBackColor = true;
            this.cbStock.CheckedChanged += new System.EventHandler(this.cbStock_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(7, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 12);
            this.label3.TabIndex = 107;
            this.label3.Text = "환율";
            // 
            // rbTrq
            // 
            this.rbTrq.AutoSize = true;
            this.rbTrq.Location = new System.Drawing.Point(389, 7);
            this.rbTrq.Name = "rbTrq";
            this.rbTrq.Size = new System.Drawing.Size(72, 16);
            this.rbTrq.TabIndex = 1;
            this.rbTrq.Text = "TRQ계산";
            this.rbTrq.UseVisualStyleBackColor = true;
            this.rbTrq.CheckedChanged += new System.EventHandler(this.rbCostprice_CheckedChanged);
            // 
            // txtExchangeRate
            // 
            this.txtExchangeRate.Location = new System.Drawing.Point(44, 4);
            this.txtExchangeRate.MaxLength = 10;
            this.txtExchangeRate.Name = "txtExchangeRate";
            this.txtExchangeRate.Size = new System.Drawing.Size(82, 21);
            this.txtExchangeRate.TabIndex = 108;
            this.txtExchangeRate.Text = "0";
            this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtExchangeRate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtExchangeRate_KeyDown);
            // 
            // rbCostprice
            // 
            this.rbCostprice.AutoSize = true;
            this.rbCostprice.Checked = true;
            this.rbCostprice.Location = new System.Drawing.Point(312, 7);
            this.rbCostprice.Name = "rbCostprice";
            this.rbCostprice.Size = new System.Drawing.Size(71, 16);
            this.rbCostprice.TabIndex = 0;
            this.rbCostprice.TabStop = true;
            this.rbCostprice.Text = "원가계산";
            this.rbCostprice.UseVisualStyleBackColor = true;
            this.rbCostprice.CheckedChanged += new System.EventHandler(this.rbCostprice_CheckedChanged);
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.White;
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel9.Controls.Add(this.panel10);
            this.panel9.Controls.Add(this.label3);
            this.panel9.Controls.Add(this.txtTrq);
            this.panel9.Controls.Add(this.rbTrq);
            this.panel9.Controls.Add(this.label2);
            this.panel9.Controls.Add(this.txtExchangeRate);
            this.panel9.Controls.Add(this.rbCostprice);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(0, 62);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1763, 31);
            this.panel9.TabIndex = 4;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.btnAdvancedSearch);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel10.Location = new System.Drawing.Point(1434, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(327, 29);
            this.panel10.TabIndex = 110;
            // 
            // btnAdvancedSearch
            // 
            this.btnAdvancedSearch.BackColor = System.Drawing.Color.White;
            this.btnAdvancedSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdvancedSearch.BackgroundImage")));
            this.btnAdvancedSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAdvancedSearch.FlatAppearance.BorderSize = 0;
            this.btnAdvancedSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdvancedSearch.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAdvancedSearch.Location = new System.Drawing.Point(299, 1);
            this.btnAdvancedSearch.Name = "btnAdvancedSearch";
            this.btnAdvancedSearch.Size = new System.Drawing.Size(25, 25);
            this.btnAdvancedSearch.TabIndex = 109;
            this.btnAdvancedSearch.UseVisualStyleBackColor = false;
            this.btnAdvancedSearch.Click += new System.EventHandler(this.btnAdvancedSearch_Click);
            // 
            // pnAdvancedSearch
            // 
            this.pnAdvancedSearch.BackColor = System.Drawing.Color.White;
            this.pnAdvancedSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnAdvancedSearch.Controls.Add(this.cbReservedStock);
            this.pnAdvancedSearch.Controls.Add(this.panel8);
            this.pnAdvancedSearch.Controls.Add(this.cbStockZero);
            this.pnAdvancedSearch.Controls.Add(this.cbStockExist);
            this.pnAdvancedSearch.Controls.Add(this.txtEndPrice);
            this.pnAdvancedSearch.Controls.Add(this.cbPriceTypeDropdown);
            this.pnAdvancedSearch.Controls.Add(this.cbPurchasePrice);
            this.pnAdvancedSearch.Controls.Add(this.cbRoundStock);
            this.pnAdvancedSearch.Controls.Add(this.cbSalesPrice);
            this.pnAdvancedSearch.Controls.Add(this.label16);
            this.pnAdvancedSearch.Controls.Add(this.label15);
            this.pnAdvancedSearch.Controls.Add(this.txtSttPrice);
            this.pnAdvancedSearch.Controls.Add(this.nudEndRound);
            this.pnAdvancedSearch.Controls.Add(this.label14);
            this.pnAdvancedSearch.Controls.Add(this.nudSttRound);
            this.pnAdvancedSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnAdvancedSearch.Location = new System.Drawing.Point(0, 93);
            this.pnAdvancedSearch.Name = "pnAdvancedSearch";
            this.pnAdvancedSearch.Size = new System.Drawing.Size(1763, 31);
            this.pnAdvancedSearch.TabIndex = 5;
            this.pnAdvancedSearch.Visible = false;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.btnAdvancedSearchExit);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel8.Location = new System.Drawing.Point(1434, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(327, 29);
            this.panel8.TabIndex = 49;
            // 
            // btnAdvancedSearchExit
            // 
            this.btnAdvancedSearchExit.BackColor = System.Drawing.Color.White;
            this.btnAdvancedSearchExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdvancedSearchExit.BackgroundImage")));
            this.btnAdvancedSearchExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAdvancedSearchExit.FlatAppearance.BorderSize = 0;
            this.btnAdvancedSearchExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdvancedSearchExit.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAdvancedSearchExit.Location = new System.Drawing.Point(300, 3);
            this.btnAdvancedSearchExit.Name = "btnAdvancedSearchExit";
            this.btnAdvancedSearchExit.Size = new System.Drawing.Size(25, 25);
            this.btnAdvancedSearchExit.TabIndex = 43;
            this.btnAdvancedSearchExit.UseVisualStyleBackColor = false;
            this.btnAdvancedSearchExit.Click += new System.EventHandler(this.btnAdvancedSearchExit_Click);
            // 
            // cbStockZero
            // 
            this.cbStockZero.AutoSize = true;
            this.cbStockZero.Checked = true;
            this.cbStockZero.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbStockZero.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbStockZero.Location = new System.Drawing.Point(134, 7);
            this.cbStockZero.Name = "cbStockZero";
            this.cbStockZero.Size = new System.Drawing.Size(107, 16);
            this.cbStockZero.TabIndex = 30;
            this.cbStockZero.Text = "재고소진 품목";
            this.cbStockZero.UseVisualStyleBackColor = true;
            // 
            // cbStockExist
            // 
            this.cbStockExist.AutoSize = true;
            this.cbStockExist.Checked = true;
            this.cbStockExist.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbStockExist.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbStockExist.Location = new System.Drawing.Point(9, 7);
            this.cbStockExist.Name = "cbStockExist";
            this.cbStockExist.Size = new System.Drawing.Size(107, 16);
            this.cbStockExist.TabIndex = 29;
            this.cbStockExist.Text = "재고있는 품목";
            this.cbStockExist.UseVisualStyleBackColor = true;
            // 
            // txtEndPrice
            // 
            this.txtEndPrice.Location = new System.Drawing.Point(577, 4);
            this.txtEndPrice.Name = "txtEndPrice";
            this.txtEndPrice.Size = new System.Drawing.Size(63, 21);
            this.txtEndPrice.TabIndex = 39;
            this.txtEndPrice.Text = "1,000,000";
            this.txtEndPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbPriceTypeDropdown
            // 
            this.cbPriceTypeDropdown.FormattingEnabled = true;
            this.cbPriceTypeDropdown.Items.AddRange(new object[] {
            "매출단가 <= 최저단가",
            "매출단가 <= 일반시세",
            "매출단가 >= 일반시세"});
            this.cbPriceTypeDropdown.Location = new System.Drawing.Point(785, 4);
            this.cbPriceTypeDropdown.Name = "cbPriceTypeDropdown";
            this.cbPriceTypeDropdown.Size = new System.Drawing.Size(155, 20);
            this.cbPriceTypeDropdown.TabIndex = 41;
            this.cbPriceTypeDropdown.Text = "매출단가 <= 최저단가";
            // 
            // cbPurchasePrice
            // 
            this.cbPurchasePrice.AutoSize = true;
            this.cbPurchasePrice.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbPurchasePrice.Location = new System.Drawing.Point(373, 7);
            this.cbPurchasePrice.Name = "cbPurchasePrice";
            this.cbPurchasePrice.Size = new System.Drawing.Size(107, 16);
            this.cbPurchasePrice.TabIndex = 36;
            this.cbPurchasePrice.Text = "매입단가 검색";
            this.cbPurchasePrice.UseVisualStyleBackColor = true;
            // 
            // cbRoundStock
            // 
            this.cbRoundStock.AutoSize = true;
            this.cbRoundStock.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbRoundStock.Location = new System.Drawing.Point(974, 7);
            this.cbRoundStock.Name = "cbRoundStock";
            this.cbRoundStock.Size = new System.Drawing.Size(94, 16);
            this.cbRoundStock.TabIndex = 33;
            this.cbRoundStock.Text = "재고 회전율";
            this.cbRoundStock.UseVisualStyleBackColor = true;
            // 
            // cbSalesPrice
            // 
            this.cbSalesPrice.AutoSize = true;
            this.cbSalesPrice.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSalesPrice.Location = new System.Drawing.Point(673, 7);
            this.cbSalesPrice.Name = "cbSalesPrice";
            this.cbSalesPrice.Size = new System.Drawing.Size(107, 16);
            this.cbSalesPrice.TabIndex = 40;
            this.cbSalesPrice.Text = "매출단가 비교";
            this.cbSalesPrice.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.cbSalesPrice.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label16.Location = new System.Drawing.Point(555, 9);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(15, 12);
            this.label16.TabIndex = 42;
            this.label16.Text = "~";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(1108, 9);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 12);
            this.label15.TabIndex = 37;
            this.label15.Text = "개월~";
            // 
            // txtSttPrice
            // 
            this.txtSttPrice.Location = new System.Drawing.Point(485, 4);
            this.txtSttPrice.Name = "txtSttPrice";
            this.txtSttPrice.Size = new System.Drawing.Size(63, 21);
            this.txtSttPrice.TabIndex = 38;
            this.txtSttPrice.Text = "1";
            this.txtSttPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nudEndRound
            // 
            this.nudEndRound.Location = new System.Drawing.Point(1151, 4);
            this.nudEndRound.Name = "nudEndRound";
            this.nudEndRound.Size = new System.Drawing.Size(41, 21);
            this.nudEndRound.TabIndex = 35;
            this.nudEndRound.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(1191, 9);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(31, 12);
            this.label14.TabIndex = 43;
            this.label14.Text = "개월";
            // 
            // nudSttRound
            // 
            this.nudSttRound.Location = new System.Drawing.Point(1068, 4);
            this.nudSttRound.Name = "nudSttRound";
            this.nudSttRound.Size = new System.Drawing.Size(41, 21);
            this.nudSttRound.TabIndex = 34;
            this.nudSttRound.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // cbReservedStock
            // 
            this.cbReservedStock.AutoSize = true;
            this.cbReservedStock.Checked = true;
            this.cbReservedStock.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbReservedStock.Font = new System.Drawing.Font("굴림", 6.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbReservedStock.Location = new System.Drawing.Point(247, 8);
            this.cbReservedStock.Name = "cbReservedStock";
            this.cbReservedStock.Size = new System.Drawing.Size(72, 14);
            this.cbReservedStock.TabIndex = 50;
            this.cbReservedStock.Text = "예약수 포함";
            this.cbReservedStock.UseVisualStyleBackColor = true;
            // 
            // ProductCostComparison
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1763, 835);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnAdvancedSearch);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "ProductCostComparison";
            this.Text = "품명별원가비교";
            this.Load += new System.EventHandler(this.ProductCostComparison_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProductCostComparison_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.pnAdvancedSearch.ResumeLayout(false);
            this.pnAdvancedSearch.PerformLayout();
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudEndRound)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSttRound)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProduct;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSizes;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbCp;
        private System.Windows.Forms.Button btnEnddateCalendar;
        private System.Windows.Forms.TextBox txtEnddate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSttdateCalendar;
        private System.Windows.Forms.TextBox txtSttdate;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox cbSales;
        private System.Windows.Forms.CheckBox cbStock;
        private System.Windows.Forms.CheckBox cbOffer;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTrq;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.RadioButton rbTrq;
        private System.Windows.Forms.RadioButton rbCostprice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtExchangeRate;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.RadioButton rbeighteenMonths;
        private System.Windows.Forms.RadioButton rbtwelveMonths;
        private System.Windows.Forms.RadioButton rbSixMonths;
        private System.Windows.Forms.CheckBox cbPendingInStock;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.CheckBox cbDongwon;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.TextBox txtDivision;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel pnAdvancedSearch;
        internal System.Windows.Forms.Button btnAdvancedSearch;
        private System.Windows.Forms.CheckBox cbStockZero;
        private System.Windows.Forms.CheckBox cbStockExist;
        private System.Windows.Forms.TextBox txtEndPrice;
        private System.Windows.Forms.ComboBox cbPriceTypeDropdown;
        private System.Windows.Forms.CheckBox cbPurchasePrice;
        private System.Windows.Forms.CheckBox cbRoundStock;
        private System.Windows.Forms.CheckBox cbSalesPrice;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtSttPrice;
        private System.Windows.Forms.NumericUpDown nudEndRound;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown nudSttRound;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel8;
        internal System.Windows.Forms.Button btnAdvancedSearchExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn shipment_stock;
        private System.Windows.Forms.DataGridViewTextBoxColumn stock;
        private System.Windows.Forms.DataGridViewTextBoxColumn reserved_stock;
        private System.Windows.Forms.DataGridViewTextBoxColumn reserved_stock_detail;
        private System.Windows.Forms.DataGridViewTextBoxColumn real_stock;
        private System.Windows.Forms.DataGridViewTextBoxColumn sales_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn sales_amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn day_sales_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn month_sales_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn month_around;
        private System.Windows.Forms.DataGridViewTextBoxColumn average_cost_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn sales_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchase_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn normal_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn offer_updatetime;
        private System.Windows.Forms.DataGridViewTextBoxColumn offer_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn offer_company;
        private System.Windows.Forms.DataGridViewTextBoxColumn exchange_rate;
        private System.Windows.Forms.DataGridViewTextBoxColumn custom;
        private System.Windows.Forms.DataGridViewTextBoxColumn tax;
        private System.Windows.Forms.DataGridViewTextBoxColumn incidental_expense;
        private System.Windows.Forms.DataGridViewTextBoxColumn offer_cost_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn order_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_stock;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_average_cost_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_month_around;
        private System.Windows.Forms.DataGridViewTextBoxColumn exhausted_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn manager;
        private System.Windows.Forms.CheckBox cbReservedStock;
    }
}