namespace AdoNetWindow.Domestic
{
    partial class SimpleDomesticManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleDomesticManager));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lbId = new System.Windows.Forms.Label();
            this.cbTransportationFeeVat = new System.Windows.Forms.CheckBox();
            this.txtTransportationFeePerBox = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.btnCalendarWarehousingDate = new System.Windows.Forms.Button();
            this.txtWarehousingDate = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.txtTransportationFee = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnRegister = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seaover_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.dgvEtcExpense = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expense_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expense_amount_per_box = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.is_vat = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label23 = new System.Windows.Forms.Label();
            this.txtTotalAmount2 = new System.Windows.Forms.TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnAddDomestic = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txtCostPrice = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRealQty = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTotalCostPrice = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtWarehouseDate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEtcExpense)).BeginInit();
            this.panel8.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbId
            // 
            this.lbId.AutoSize = true;
            this.lbId.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbId.Location = new System.Drawing.Point(140, 63);
            this.lbId.Name = "lbId";
            this.lbId.Size = new System.Drawing.Size(51, 16);
            this.lbId.TabIndex = 129;
            this.lbId.Text = "NULL";
            this.lbId.Visible = false;
            // 
            // cbTransportationFeeVat
            // 
            this.cbTransportationFeeVat.AutoSize = true;
            this.cbTransportationFeeVat.Checked = true;
            this.cbTransportationFeeVat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTransportationFeeVat.Location = new System.Drawing.Point(96, 121);
            this.cbTransportationFeeVat.Name = "cbTransportationFeeVat";
            this.cbTransportationFeeVat.Size = new System.Drawing.Size(88, 16);
            this.cbTransportationFeeVat.TabIndex = 5;
            this.cbTransportationFeeVat.Text = "부가세 포함";
            this.cbTransportationFeeVat.UseVisualStyleBackColor = true;
            // 
            // txtTransportationFeePerBox
            // 
            this.txtTransportationFeePerBox.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTransportationFeePerBox.Location = new System.Drawing.Point(260, 90);
            this.txtTransportationFeePerBox.Name = "txtTransportationFeePerBox";
            this.txtTransportationFeePerBox.Size = new System.Drawing.Size(132, 26);
            this.txtTransportationFeePerBox.TabIndex = 4;
            this.txtTransportationFeePerBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label18.ForeColor = System.Drawing.Color.Blue;
            this.label18.Location = new System.Drawing.Point(12, 91);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(69, 19);
            this.label18.TabIndex = 52;
            this.label18.Text = "운반비";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label21.Location = new System.Drawing.Point(398, 94);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(39, 16);
            this.label21.TabIndex = 48;
            this.label21.Text = "Box";
            // 
            // btnCalendarWarehousingDate
            // 
            this.btnCalendarWarehousingDate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCalendarWarehousingDate.BackgroundImage")));
            this.btnCalendarWarehousingDate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCalendarWarehousingDate.FlatAppearance.BorderSize = 0;
            this.btnCalendarWarehousingDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalendarWarehousingDate.Location = new System.Drawing.Point(709, 4);
            this.btnCalendarWarehousingDate.Name = "btnCalendarWarehousingDate";
            this.btnCalendarWarehousingDate.Size = new System.Drawing.Size(30, 30);
            this.btnCalendarWarehousingDate.TabIndex = 128;
            this.btnCalendarWarehousingDate.UseVisualStyleBackColor = true;
            // 
            // txtWarehousingDate
            // 
            this.txtWarehousingDate.BackColor = System.Drawing.Color.PeachPuff;
            this.txtWarehousingDate.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtWarehousingDate.Location = new System.Drawing.Point(603, 7);
            this.txtWarehousingDate.Name = "txtWarehousingDate";
            this.txtWarehousingDate.Size = new System.Drawing.Size(104, 26);
            this.txtWarehousingDate.TabIndex = 26;
            this.txtWarehousingDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label24.Location = new System.Drawing.Point(525, 12);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(75, 16);
            this.label24.TabIndex = 25;
            this.label24.Text = "입고일자";
            // 
            // txtTransportationFee
            // 
            this.txtTransportationFee.BackColor = System.Drawing.Color.PeachPuff;
            this.txtTransportationFee.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTransportationFee.Location = new System.Drawing.Point(96, 89);
            this.txtTransportationFee.Name = "txtTransportationFee";
            this.txtTransportationFee.Size = new System.Drawing.Size(132, 26);
            this.txtTransportationFee.TabIndex = 3;
            this.txtTransportationFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTransportationFee.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTransportationFee_KeyDown);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(76, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(65, 35);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnRegister);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 518);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(492, 41);
            this.panel3.TabIndex = 6;
            // 
            // btnRegister
            // 
            this.btnRegister.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRegister.Location = new System.Drawing.Point(5, 2);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(65, 35);
            this.btnRegister.TabIndex = 8;
            this.btnRegister.Text = "등록(A)";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvProduct);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(490, 51);
            this.panel1.TabIndex = 4;
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToResizeRows = false;
            this.dgvProduct.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.product,
            this.origin,
            this.sizes,
            this.unit,
            this.price_unit,
            this.unit_count,
            this.seaover_unit});
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.EnableHeadersVisualStyles = false;
            this.dgvProduct.Location = new System.Drawing.Point(0, 0);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowHeadersVisible = false;
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.Size = new System.Drawing.Size(490, 51);
            this.dgvProduct.TabIndex = 0;
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
            // price_unit
            // 
            this.price_unit.HeaderText = "가격단위";
            this.price_unit.Name = "price_unit";
            // 
            // unit_count
            // 
            this.unit_count.HeaderText = "단위수량";
            this.unit_count.Name = "unit_count";
            // 
            // seaover_unit
            // 
            this.seaover_unit.HeaderText = "S단위";
            this.seaover_unit.Name = "seaover_unit";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Controls.Add(this.panel8);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(492, 559);
            this.panel2.TabIndex = 5;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.dgvEtcExpense);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 290);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(492, 243);
            this.panel7.TabIndex = 46;
            // 
            // dgvEtcExpense
            // 
            this.dgvEtcExpense.AllowUserToAddRows = false;
            this.dgvEtcExpense.AllowUserToDeleteRows = false;
            this.dgvEtcExpense.AllowUserToOrderColumns = true;
            this.dgvEtcExpense.AllowUserToResizeRows = false;
            this.dgvEtcExpense.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEtcExpense.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.division,
            this.expense_amount,
            this.expense_amount_per_box,
            this.is_vat,
            this.btnDelete});
            this.dgvEtcExpense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEtcExpense.EnableHeadersVisualStyles = false;
            this.dgvEtcExpense.Location = new System.Drawing.Point(0, 0);
            this.dgvEtcExpense.Name = "dgvEtcExpense";
            this.dgvEtcExpense.RowHeadersVisible = false;
            this.dgvEtcExpense.RowTemplate.Height = 23;
            this.dgvEtcExpense.Size = new System.Drawing.Size(492, 243);
            this.dgvEtcExpense.TabIndex = 0;
            this.dgvEtcExpense.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEtcExpense_CellContentClick);
            this.dgvEtcExpense.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEtcExpense_CellValueChanged);
            // 
            // division
            // 
            this.division.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.division.HeaderText = "구분";
            this.division.Name = "division";
            // 
            // expense_amount
            // 
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.PeachPuff;
            this.expense_amount.DefaultCellStyle = dataGridViewCellStyle13;
            this.expense_amount.HeaderText = "금액";
            this.expense_amount.Name = "expense_amount";
            // 
            // expense_amount_per_box
            // 
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.PeachPuff;
            this.expense_amount_per_box.DefaultCellStyle = dataGridViewCellStyle14;
            this.expense_amount_per_box.HeaderText = "개별 금액";
            this.expense_amount_per_box.Name = "expense_amount_per_box";
            this.expense_amount_per_box.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.expense_amount_per_box.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // is_vat
            // 
            this.is_vat.HeaderText = "부가세 포함";
            this.is_vat.Name = "is_vat";
            this.is_vat.Width = 80;
            // 
            // btnDelete
            // 
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle15.NullValue = "삭제";
            this.btnDelete.DefaultCellStyle = dataGridViewCellStyle15;
            this.btnDelete.HeaderText = "삭제";
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Width = 50;
            // 
            // panel8
            // 
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Controls.Add(this.label23);
            this.panel8.Controls.Add(this.txtTotalAmount2);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.Location = new System.Drawing.Point(0, 533);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(492, 26);
            this.panel8.TabIndex = 47;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label23.Location = new System.Drawing.Point(204, 3);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(64, 16);
            this.label23.TabIndex = 49;
            this.label23.Text = "총 금액";
            // 
            // txtTotalAmount2
            // 
            this.txtTotalAmount2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTotalAmount2.Location = new System.Drawing.Point(273, 0);
            this.txtTotalAmount2.Name = "txtTotalAmount2";
            this.txtTotalAmount2.Size = new System.Drawing.Size(132, 26);
            this.txtTotalAmount2.TabIndex = 48;
            this.txtTotalAmount2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.button1);
            this.panel6.Controls.Add(this.txtWarehouseDate);
            this.panel6.Controls.Add(this.label4);
            this.panel6.Controls.Add(this.btnAddDomestic);
            this.panel6.Controls.Add(this.label2);
            this.panel6.Controls.Add(this.cbTransportationFeeVat);
            this.panel6.Controls.Add(this.label19);
            this.panel6.Controls.Add(this.txtCostPrice);
            this.panel6.Controls.Add(this.label1);
            this.panel6.Controls.Add(this.label18);
            this.panel6.Controls.Add(this.txtRealQty);
            this.panel6.Controls.Add(this.txtTransportationFeePerBox);
            this.panel6.Controls.Add(this.label21);
            this.panel6.Controls.Add(this.txtTransportationFee);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 88);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(492, 202);
            this.panel6.TabIndex = 0;
            // 
            // btnAddDomestic
            // 
            this.btnAddDomestic.BackgroundImage = global::AdoNetWindow.Properties.Resources.Plus2_btn;
            this.btnAddDomestic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddDomestic.FlatAppearance.BorderSize = 0;
            this.btnAddDomestic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddDomestic.Location = new System.Drawing.Point(183, 166);
            this.btnAddDomestic.Name = "btnAddDomestic";
            this.btnAddDomestic.Size = new System.Drawing.Size(23, 23);
            this.btnAddDomestic.TabIndex = 6;
            this.btnAddDomestic.UseVisualStyleBackColor = true;
            this.btnAddDomestic.Click += new System.EventHandler(this.btnAddDomestic_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(256, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 19);
            this.label2.TabIndex = 135;
            this.label2.Text = "제조원가";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label19.ForeColor = System.Drawing.Color.Blue;
            this.label19.Location = new System.Drawing.Point(10, 166);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(160, 21);
            this.label19.TabIndex = 43;
            this.label19.Text = "창고/기타 비용";
            // 
            // txtCostPrice
            // 
            this.txtCostPrice.BackColor = System.Drawing.Color.PeachPuff;
            this.txtCostPrice.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtCostPrice.Location = new System.Drawing.Point(347, 47);
            this.txtCostPrice.Name = "txtCostPrice";
            this.txtCostPrice.Size = new System.Drawing.Size(132, 26);
            this.txtCostPrice.TabIndex = 2;
            this.txtCostPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCostPrice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRealQty_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(12, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 19);
            this.label1.TabIndex = 133;
            this.label1.Text = "수량";
            // 
            // txtRealQty
            // 
            this.txtRealQty.BackColor = System.Drawing.Color.PeachPuff;
            this.txtRealQty.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtRealQty.Location = new System.Drawing.Point(96, 47);
            this.txtRealQty.Name = "txtRealQty";
            this.txtRealQty.Size = new System.Drawing.Size(132, 26);
            this.txtRealQty.TabIndex = 1;
            this.txtRealQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRealQty.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRealQty_KeyDown);
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.txtTotalCostPrice);
            this.panel4.Controls.Add(this.panel1);
            this.panel4.Controls.Add(this.btnCalendarWarehousingDate);
            this.panel4.Controls.Add(this.lbId);
            this.panel4.Controls.Add(this.txtWarehousingDate);
            this.panel4.Controls.Add(this.label24);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(492, 88);
            this.panel4.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.BlueViolet;
            this.label3.Location = new System.Drawing.Point(3, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 24);
            this.label3.TabIndex = 137;
            this.label3.Text = "제품원가";
            // 
            // txtTotalCostPrice
            // 
            this.txtTotalCostPrice.BackColor = System.Drawing.Color.SandyBrown;
            this.txtTotalCostPrice.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTotalCostPrice.Location = new System.Drawing.Point(235, 51);
            this.txtTotalCostPrice.Name = "txtTotalCostPrice";
            this.txtTotalCostPrice.Size = new System.Drawing.Size(255, 35);
            this.txtTotalCostPrice.TabIndex = 136;
            this.txtTotalCostPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(202, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 30);
            this.button1.TabIndex = 138;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtWarehouseDate
            // 
            this.txtWarehouseDate.BackColor = System.Drawing.Color.PeachPuff;
            this.txtWarehouseDate.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtWarehouseDate.Location = new System.Drawing.Point(96, 6);
            this.txtWarehouseDate.Name = "txtWarehouseDate";
            this.txtWarehouseDate.Size = new System.Drawing.Size(104, 26);
            this.txtWarehouseDate.TabIndex = 137;
            this.txtWarehouseDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtWarehouseDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtWarehouseDate_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(13, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 16);
            this.label4.TabIndex = 136;
            this.label4.Text = "입고일자";
            // 
            // SimpleDomesticManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 559);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SimpleDomesticManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "비용단가등록";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SimpleDomesticManager_KeyDown);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEtcExpense)).EndInit();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbId;
        private System.Windows.Forms.CheckBox cbTransportationFeeVat;
        private System.Windows.Forms.TextBox txtTransportationFeePerBox;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button btnCalendarWarehousingDate;
        private System.Windows.Forms.TextBox txtWarehousingDate;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox txtTransportationFee;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Panel panel1;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProduct;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel7;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvEtcExpense;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txtTotalAmount2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnAddDomestic;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn price_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_unit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTotalCostPrice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCostPrice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRealQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn division;
        private System.Windows.Forms.DataGridViewTextBoxColumn expense_amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn expense_amount_per_box;
        private System.Windows.Forms.DataGridViewCheckBoxColumn is_vat;
        private System.Windows.Forms.DataGridViewButtonColumn btnDelete;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtWarehouseDate;
        private System.Windows.Forms.Label label4;
    }
}