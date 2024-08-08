namespace AdoNetWindow.Pending
{
    partial class PendingList2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PendingList2));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbExactly = new System.Windows.Forms.CheckBox();
            this.txtBoxWeight = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cbCcStatus = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.txtSizes = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtBlNo = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.nudEndYear = new System.Windows.Forms.NumericUpDown();
            this.nudSttYear = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAtoNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtContractNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtShipper = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSearching = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sub_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contract_year = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ato_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pi_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shipper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contract_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bl_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.etd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehousing_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cc_status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weight_calculate = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.box_weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contract_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shipping_exchange_rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.payment_exchange_rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_price_per_box = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trq_cost_price_per_box = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trq_cost_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.custom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.incidental_expense = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.production_days = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rbPaymentExchangeRate = new System.Windows.Forms.RadioButton();
            this.rbShippingExchangeRate = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEndYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSttYear)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbPaymentExchangeRate);
            this.panel1.Controls.Add(this.rbShippingExchangeRate);
            this.panel1.Controls.Add(this.cbExactly);
            this.panel1.Controls.Add(this.txtBoxWeight);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.cbCcStatus);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtOrigin);
            this.panel1.Controls.Add(this.txtManager);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.txtProduct);
            this.panel1.Controls.Add(this.txtSizes);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txtBlNo);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.nudEndYear);
            this.panel1.Controls.Add(this.nudSttYear);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtAtoNo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtContractNo);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtShipper);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1885, 54);
            this.panel1.TabIndex = 0;
            // 
            // cbExactly
            // 
            this.cbExactly.AutoSize = true;
            this.cbExactly.Location = new System.Drawing.Point(822, 28);
            this.cbExactly.Name = "cbExactly";
            this.cbExactly.Size = new System.Drawing.Size(60, 16);
            this.cbExactly.TabIndex = 54;
            this.cbExactly.Text = "정확히";
            this.cbExactly.UseVisualStyleBackColor = true;
            // 
            // txtBoxWeight
            // 
            this.txtBoxWeight.Location = new System.Drawing.Point(1108, 25);
            this.txtBoxWeight.Name = "txtBoxWeight";
            this.txtBoxWeight.Size = new System.Drawing.Size(104, 21);
            this.txtBoxWeight.TabIndex = 45;
            this.txtBoxWeight.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAtoNo_KeyDown);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(1106, 9);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 12);
            this.label16.TabIndex = 53;
            this.label16.Text = "박스중량";
            // 
            // cbCcStatus
            // 
            this.cbCcStatus.FormattingEnabled = true;
            this.cbCcStatus.Items.AddRange(new object[] {
            "전체",
            "미통관",
            "통관",
            "확정"});
            this.cbCcStatus.Location = new System.Drawing.Point(1328, 25);
            this.cbCcStatus.Name = "cbCcStatus";
            this.cbCcStatus.Size = new System.Drawing.Size(85, 20);
            this.cbCcStatus.TabIndex = 47;
            this.cbCcStatus.Text = "전체";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(1326, 10);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(29, 12);
            this.label15.TabIndex = 52;
            this.label15.Text = "상태";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(579, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 49;
            this.label4.Text = "품목명";
            // 
            // txtOrigin
            // 
            this.txtOrigin.Location = new System.Drawing.Point(998, 25);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.Size = new System.Drawing.Size(104, 21);
            this.txtOrigin.TabIndex = 44;
            this.txtOrigin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAtoNo_KeyDown);
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(1218, 25);
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(104, 21);
            this.txtManager.TabIndex = 46;
            this.txtManager.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAtoNo_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(886, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 48;
            this.label6.Text = "사이즈";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1216, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 51;
            this.label11.Text = "담당자";
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(581, 25);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(235, 21);
            this.txtProduct.TabIndex = 42;
            this.txtProduct.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAtoNo_KeyDown);
            // 
            // txtSizes
            // 
            this.txtSizes.Location = new System.Drawing.Point(888, 25);
            this.txtSizes.Name = "txtSizes";
            this.txtSizes.Size = new System.Drawing.Size(104, 21);
            this.txtSizes.TabIndex = 43;
            this.txtSizes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAtoNo_KeyDown);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(996, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 50;
            this.label10.Text = "원산지";
            // 
            // txtBlNo
            // 
            this.txtBlNo.Location = new System.Drawing.Point(471, 25);
            this.txtBlNo.Name = "txtBlNo";
            this.txtBlNo.Size = new System.Drawing.Size(104, 21);
            this.txtBlNo.TabIndex = 40;
            this.txtBlNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAtoNo_KeyDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(469, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 12);
            this.label8.TabIndex = 41;
            this.label8.Text = "Bl No.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(62, 29);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 12);
            this.label9.TabIndex = 39;
            this.label9.Text = "~";
            // 
            // nudEndYear
            // 
            this.nudEndYear.Location = new System.Drawing.Point(82, 24);
            this.nudEndYear.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.nudEndYear.Minimum = new decimal(new int[] {
            2015,
            0,
            0,
            0});
            this.nudEndYear.Name = "nudEndYear";
            this.nudEndYear.Size = new System.Drawing.Size(46, 21);
            this.nudEndYear.TabIndex = 31;
            this.nudEndYear.Value = new decimal(new int[] {
            2022,
            0,
            0,
            0});
            // 
            // nudSttYear
            // 
            this.nudSttYear.Location = new System.Drawing.Point(10, 24);
            this.nudSttYear.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.nudSttYear.Minimum = new decimal(new int[] {
            2015,
            0,
            0,
            0});
            this.nudSttYear.Name = "nudSttYear";
            this.nudSttYear.Size = new System.Drawing.Size(46, 21);
            this.nudSttYear.TabIndex = 30;
            this.nudSttYear.Value = new decimal(new int[] {
            2015,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 34;
            this.label1.Text = "계약연도";
            // 
            // txtAtoNo
            // 
            this.txtAtoNo.Location = new System.Drawing.Point(141, 25);
            this.txtAtoNo.Name = "txtAtoNo";
            this.txtAtoNo.Size = new System.Drawing.Size(104, 21);
            this.txtAtoNo.TabIndex = 32;
            this.txtAtoNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAtoNo_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(139, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 12);
            this.label2.TabIndex = 36;
            this.label2.Text = "ATO No.";
            // 
            // txtContractNo
            // 
            this.txtContractNo.Location = new System.Drawing.Point(251, 25);
            this.txtContractNo.Name = "txtContractNo";
            this.txtContractNo.Size = new System.Drawing.Size(104, 21);
            this.txtContractNo.TabIndex = 33;
            this.txtContractNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAtoNo_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(359, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 37;
            this.label3.Text = "거래처";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(249, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 38;
            this.label5.Text = "계약번호";
            // 
            // txtShipper
            // 
            this.txtShipper.Location = new System.Drawing.Point(361, 25);
            this.txtShipper.Name = "txtShipper";
            this.txtShipper.Size = new System.Drawing.Size(104, 21);
            this.txtShipper.TabIndex = 35;
            this.txtShipper.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAtoNo_KeyDown);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvProduct);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 54);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1885, 778);
            this.panel2.TabIndex = 1;
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToResizeRows = false;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.sub_id,
            this.contract_year,
            this.ato_no,
            this.pi_date,
            this.shipper,
            this.contract_no,
            this.bl_no,
            this.warehouse,
            this.etd,
            this.eta,
            this.warehousing_date,
            this.cc_status,
            this.product,
            this.origin,
            this.sizes,
            this.weight_calculate,
            this.box_weight,
            this.cost_unit,
            this.contract_qty,
            this.unit_price,
            this.shipping_exchange_rate,
            this.payment_exchange_rate,
            this.cost_price_per_box,
            this.cost_price,
            this.trq,
            this.trq_cost_price_per_box,
            this.trq_cost_price,
            this.custom,
            this.tax,
            this.incidental_expense,
            this.production_days});
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.EnableHeadersVisualStyles = false;
            this.dgvProduct.Location = new System.Drawing.Point(0, 0);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProduct.Size = new System.Drawing.Size(1885, 778);
            this.dgvProduct.TabIndex = 3;
            this.dgvProduct.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProduct_CellMouseClick);
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            this.dgvProduct.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dgvProduct_SortCompare);
            this.dgvProduct.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvProduct_MouseUp);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSearching);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 832);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1885, 40);
            this.panel3.TabIndex = 2;
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.ForeColor = System.Drawing.Color.Black;
            this.btnSearching.Location = new System.Drawing.Point(3, 2);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(75, 37);
            this.btnSearching.TabIndex = 5;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(82, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.Visible = false;
            // 
            // sub_id
            // 
            this.sub_id.HeaderText = "sub_id";
            this.sub_id.Name = "sub_id";
            this.sub_id.Visible = false;
            // 
            // contract_year
            // 
            this.contract_year.HeaderText = "계약연도";
            this.contract_year.Name = "contract_year";
            this.contract_year.Width = 60;
            // 
            // ato_no
            // 
            this.ato_no.HeaderText = "AtoNo";
            this.ato_no.Name = "ato_no";
            this.ato_no.Width = 60;
            // 
            // pi_date
            // 
            this.pi_date.HeaderText = "PI DATE";
            this.pi_date.Name = "pi_date";
            this.pi_date.Width = 80;
            // 
            // shipper
            // 
            this.shipper.HeaderText = "거래처";
            this.shipper.Name = "shipper";
            this.shipper.Width = 80;
            // 
            // contract_no
            // 
            this.contract_no.HeaderText = "계약번호";
            this.contract_no.Name = "contract_no";
            this.contract_no.Width = 80;
            // 
            // bl_no
            // 
            this.bl_no.HeaderText = "B/L No";
            this.bl_no.Name = "bl_no";
            this.bl_no.Width = 80;
            // 
            // warehouse
            // 
            this.warehouse.HeaderText = "창고";
            this.warehouse.Name = "warehouse";
            this.warehouse.Width = 80;
            // 
            // etd
            // 
            this.etd.HeaderText = "ETD";
            this.etd.Name = "etd";
            this.etd.Width = 80;
            // 
            // eta
            // 
            this.eta.HeaderText = "ETA";
            this.eta.Name = "eta";
            this.eta.Width = 80;
            // 
            // warehousing_date
            // 
            this.warehousing_date.HeaderText = "창고입고일";
            this.warehousing_date.Name = "warehousing_date";
            this.warehousing_date.Width = 80;
            // 
            // cc_status
            // 
            this.cc_status.HeaderText = "상태";
            this.cc_status.Name = "cc_status";
            this.cc_status.Width = 60;
            // 
            // product
            // 
            this.product.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.product.HeaderText = "품명";
            this.product.Name = "product";
            // 
            // origin
            // 
            this.origin.HeaderText = "원산지";
            this.origin.Name = "origin";
            this.origin.Width = 80;
            // 
            // sizes
            // 
            this.sizes.HeaderText = "규격";
            this.sizes.Name = "sizes";
            this.sizes.Width = 80;
            // 
            // weight_calculate
            // 
            this.weight_calculate.HeaderText = "";
            this.weight_calculate.Name = "weight_calculate";
            this.weight_calculate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.weight_calculate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.weight_calculate.Width = 30;
            // 
            // box_weight
            // 
            this.box_weight.HeaderText = "중량";
            this.box_weight.Name = "box_weight";
            this.box_weight.Width = 60;
            // 
            // cost_unit
            // 
            this.cost_unit.HeaderText = "트레이";
            this.cost_unit.Name = "cost_unit";
            this.cost_unit.Width = 60;
            // 
            // contract_qty
            // 
            this.contract_qty.HeaderText = "수량";
            this.contract_qty.Name = "contract_qty";
            this.contract_qty.Width = 60;
            // 
            // unit_price
            // 
            this.unit_price.HeaderText = "단가";
            this.unit_price.Name = "unit_price";
            this.unit_price.Width = 60;
            // 
            // shipping_exchange_rate
            // 
            this.shipping_exchange_rate.HeaderText = "선적환율";
            this.shipping_exchange_rate.Name = "shipping_exchange_rate";
            this.shipping_exchange_rate.Width = 80;
            // 
            // payment_exchange_rate
            // 
            this.payment_exchange_rate.HeaderText = "결제환율";
            this.payment_exchange_rate.Name = "payment_exchange_rate";
            this.payment_exchange_rate.Width = 80;
            // 
            // cost_price_per_box
            // 
            this.cost_price_per_box.HeaderText = "원가계산";
            this.cost_price_per_box.Name = "cost_price_per_box";
            this.cost_price_per_box.Width = 80;
            // 
            // cost_price
            // 
            this.cost_price.HeaderText = "총 원가계산";
            this.cost_price.Name = "cost_price";
            this.cost_price.Width = 80;
            // 
            // trq
            // 
            this.trq.HeaderText = "TRQ";
            this.trq.Name = "trq";
            this.trq.Visible = false;
            this.trq.Width = 80;
            // 
            // trq_cost_price_per_box
            // 
            this.trq_cost_price_per_box.HeaderText = "TRQ";
            this.trq_cost_price_per_box.Name = "trq_cost_price_per_box";
            this.trq_cost_price_per_box.Width = 80;
            // 
            // trq_cost_price
            // 
            this.trq_cost_price.HeaderText = "총 TRQ";
            this.trq_cost_price.Name = "trq_cost_price";
            this.trq_cost_price.Width = 80;
            // 
            // custom
            // 
            this.custom.HeaderText = "관세";
            this.custom.Name = "custom";
            this.custom.Visible = false;
            // 
            // tax
            // 
            this.tax.HeaderText = "과세";
            this.tax.Name = "tax";
            this.tax.Visible = false;
            // 
            // incidental_expense
            // 
            this.incidental_expense.HeaderText = "부대비용";
            this.incidental_expense.Name = "incidental_expense";
            this.incidental_expense.Visible = false;
            // 
            // production_days
            // 
            this.production_days.HeaderText = "생산일";
            this.production_days.Name = "production_days";
            this.production_days.Visible = false;
            // 
            // rbPaymentExchangeRate
            // 
            this.rbPaymentExchangeRate.AutoSize = true;
            this.rbPaymentExchangeRate.Location = new System.Drawing.Point(1775, 32);
            this.rbPaymentExchangeRate.Name = "rbPaymentExchangeRate";
            this.rbPaymentExchangeRate.Size = new System.Drawing.Size(98, 16);
            this.rbPaymentExchangeRate.TabIndex = 56;
            this.rbPaymentExchangeRate.Text = "결제환율 (F2)";
            this.rbPaymentExchangeRate.UseVisualStyleBackColor = true;
            this.rbPaymentExchangeRate.CheckedChanged += new System.EventHandler(this.rbShippingExchangeRate_CheckedChanged);
            // 
            // rbShippingExchangeRate
            // 
            this.rbShippingExchangeRate.AutoSize = true;
            this.rbShippingExchangeRate.Checked = true;
            this.rbShippingExchangeRate.Location = new System.Drawing.Point(1671, 32);
            this.rbShippingExchangeRate.Name = "rbShippingExchangeRate";
            this.rbShippingExchangeRate.Size = new System.Drawing.Size(98, 16);
            this.rbShippingExchangeRate.TabIndex = 55;
            this.rbShippingExchangeRate.TabStop = true;
            this.rbShippingExchangeRate.Text = "선적환율 (F1)";
            this.rbShippingExchangeRate.UseVisualStyleBackColor = true;
            this.rbShippingExchangeRate.CheckedChanged += new System.EventHandler(this.rbShippingExchangeRate_CheckedChanged);
            // 
            // PendingList2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1885, 872);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "PendingList2";
            this.Text = "팬딩조회";
            this.Load += new System.EventHandler(this.PendingList2_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PendingList2_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEndYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSttYear)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProduct;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudEndYear;
        private System.Windows.Forms.NumericUpDown nudSttYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAtoNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtContractNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtShipper;
        private System.Windows.Forms.TextBox txtBlNo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBoxWeight;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cbCcStatus;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.TextBox txtSizes;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.CheckBox cbExactly;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn sub_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn contract_year;
        private System.Windows.Forms.DataGridViewTextBoxColumn ato_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn pi_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn shipper;
        private System.Windows.Forms.DataGridViewTextBoxColumn contract_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn bl_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehouse;
        private System.Windows.Forms.DataGridViewTextBoxColumn etd;
        private System.Windows.Forms.DataGridViewTextBoxColumn eta;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehousing_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn cc_status;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes;
        private System.Windows.Forms.DataGridViewCheckBoxColumn weight_calculate;
        private System.Windows.Forms.DataGridViewTextBoxColumn box_weight;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn contract_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn shipping_exchange_rate;
        private System.Windows.Forms.DataGridViewTextBoxColumn payment_exchange_rate;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost_price_per_box;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn trq;
        private System.Windows.Forms.DataGridViewTextBoxColumn trq_cost_price_per_box;
        private System.Windows.Forms.DataGridViewTextBoxColumn trq_cost_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn custom;
        private System.Windows.Forms.DataGridViewTextBoxColumn tax;
        private System.Windows.Forms.DataGridViewTextBoxColumn incidental_expense;
        private System.Windows.Forms.DataGridViewTextBoxColumn production_days;
        private System.Windows.Forms.RadioButton rbPaymentExchangeRate;
        private System.Windows.Forms.RadioButton rbShippingExchangeRate;
    }
}