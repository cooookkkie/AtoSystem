namespace AdoNetWindow.SEAOVER.PriceComparison
{
    partial class MarginDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MarginDetails));
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seaover_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seaover_stock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pending_stock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.offer_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sales_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.normal_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_temp_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seaover_cost_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pending_cost_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.offer_cost_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.average_cost_price1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.average_cost_price2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_temp_cost_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_temp_cost_margin_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_temp_cost_margin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.txtTotalOrderQty = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.txtTotalOrderMarginAmount1 = new System.Windows.Forms.TextBox();
            this.txtTotalOrderMarginAmount0 = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.txtTotalOrderMarginAmount3 = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.txtTotalOrderMarginAmount2 = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.rbCostPriceSPO = new System.Windows.Forms.RadioButton();
            this.rbCostPriceSP = new System.Windows.Forms.RadioButton();
            this.rbCostPriceO = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.rbCostPriceS = new System.Windows.Forms.RadioButton();
            this.rbCostPriceP = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.rbSalesPrice = new System.Windows.Forms.RadioButton();
            this.rbNormalPrice = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvProduct);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1115, 663);
            this.panel1.TabIndex = 0;
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToResizeColumns = false;
            this.dgvProduct.AllowUserToResizeRows = false;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.product,
            this.origin,
            this.sizes,
            this.unit,
            this.price_unit,
            this.unit_count,
            this.seaover_unit,
            this.seaover_stock,
            this.pending_stock,
            this.offer_qty,
            this.sales_price,
            this.normal_price,
            this.total_temp_amount,
            this.seaover_cost_price,
            this.pending_cost_price,
            this.offer_cost_price,
            this.average_cost_price1,
            this.average_cost_price2,
            this.total_temp_cost_amount,
            this.total_temp_cost_margin_amount,
            this.total_temp_cost_margin});
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.EnableHeadersVisualStyles = false;
            this.dgvProduct.Location = new System.Drawing.Point(0, 0);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.Size = new System.Drawing.Size(1115, 663);
            this.dgvProduct.TabIndex = 0;
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
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
            this.origin.Width = 60;
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
            this.unit.Width = 50;
            // 
            // price_unit
            // 
            this.price_unit.HeaderText = "가격단위";
            this.price_unit.Name = "price_unit";
            this.price_unit.Visible = false;
            this.price_unit.Width = 50;
            // 
            // unit_count
            // 
            this.unit_count.HeaderText = "단위수량";
            this.unit_count.Name = "unit_count";
            this.unit_count.Visible = false;
            this.unit_count.Width = 50;
            // 
            // seaover_unit
            // 
            this.seaover_unit.HeaderText = "S단위";
            this.seaover_unit.Name = "seaover_unit";
            this.seaover_unit.Width = 50;
            // 
            // seaover_stock
            // 
            this.seaover_stock.HeaderText = "S재고";
            this.seaover_stock.Name = "seaover_stock";
            this.seaover_stock.Width = 50;
            // 
            // pending_stock
            // 
            this.pending_stock.HeaderText = "P재고";
            this.pending_stock.Name = "pending_stock";
            this.pending_stock.Width = 50;
            // 
            // offer_qty
            // 
            this.offer_qty.HeaderText = "O수량";
            this.offer_qty.Name = "offer_qty";
            this.offer_qty.Width = 50;
            // 
            // sales_price
            // 
            this.sales_price.HeaderText = "매출단가";
            this.sales_price.Name = "sales_price";
            this.sales_price.Width = 70;
            // 
            // normal_price
            // 
            this.normal_price.HeaderText = "일반시세";
            this.normal_price.Name = "normal_price";
            this.normal_price.Width = 70;
            // 
            // total_temp_amount
            // 
            this.total_temp_amount.HeaderText = "단가총액";
            this.total_temp_amount.Name = "total_temp_amount";
            // 
            // seaover_cost_price
            // 
            this.seaover_cost_price.HeaderText = "S원가";
            this.seaover_cost_price.Name = "seaover_cost_price";
            this.seaover_cost_price.Width = 70;
            // 
            // pending_cost_price
            // 
            this.pending_cost_price.HeaderText = "P원가";
            this.pending_cost_price.Name = "pending_cost_price";
            this.pending_cost_price.Width = 70;
            // 
            // offer_cost_price
            // 
            this.offer_cost_price.HeaderText = "O원가";
            this.offer_cost_price.Name = "offer_cost_price";
            this.offer_cost_price.Width = 70;
            // 
            // average_cost_price1
            // 
            this.average_cost_price1.HeaderText = "SP원가";
            this.average_cost_price1.Name = "average_cost_price1";
            this.average_cost_price1.Width = 70;
            // 
            // average_cost_price2
            // 
            this.average_cost_price2.HeaderText = "SPO원가";
            this.average_cost_price2.Name = "average_cost_price2";
            this.average_cost_price2.Width = 70;
            // 
            // total_temp_cost_amount
            // 
            this.total_temp_cost_amount.HeaderText = "원가총액";
            this.total_temp_cost_amount.Name = "total_temp_cost_amount";
            // 
            // total_temp_cost_margin_amount
            // 
            this.total_temp_cost_margin_amount.HeaderText = "손익금액";
            this.total_temp_cost_margin_amount.Name = "total_temp_cost_margin_amount";
            // 
            // total_temp_cost_margin
            // 
            this.total_temp_cost_margin.HeaderText = "마진(%)";
            this.total_temp_cost_margin.Name = "total_temp_cost_margin";
            this.total_temp_cost_margin.Width = 70;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 690);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1115, 23);
            this.panel2.TabIndex = 1;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.txtTotalOrderQty);
            this.panel7.Controls.Add(this.label29);
            this.panel7.Controls.Add(this.txtTotalOrderMarginAmount1);
            this.panel7.Controls.Add(this.txtTotalOrderMarginAmount0);
            this.panel7.Controls.Add(this.label26);
            this.panel7.Controls.Add(this.txtTotalOrderMarginAmount3);
            this.panel7.Controls.Add(this.label23);
            this.panel7.Controls.Add(this.label28);
            this.panel7.Controls.Add(this.label27);
            this.panel7.Controls.Add(this.txtTotalOrderMarginAmount2);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel7.Location = new System.Drawing.Point(149, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(964, 21);
            this.panel7.TabIndex = 23;
            // 
            // txtTotalOrderQty
            // 
            this.txtTotalOrderQty.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTotalOrderQty.Location = new System.Drawing.Point(70, 0);
            this.txtTotalOrderQty.Name = "txtTotalOrderQty";
            this.txtTotalOrderQty.Size = new System.Drawing.Size(100, 22);
            this.txtTotalOrderQty.TabIndex = 13;
            this.txtTotalOrderQty.Text = "0";
            this.txtTotalOrderQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label29.Location = new System.Drawing.Point(332, 5);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(15, 12);
            this.label29.TabIndex = 22;
            this.label29.Text = "O";
            // 
            // txtTotalOrderMarginAmount1
            // 
            this.txtTotalOrderMarginAmount1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTotalOrderMarginAmount1.ForeColor = System.Drawing.Color.Blue;
            this.txtTotalOrderMarginAmount1.Location = new System.Drawing.Point(512, 0);
            this.txtTotalOrderMarginAmount1.Name = "txtTotalOrderMarginAmount1";
            this.txtTotalOrderMarginAmount1.Size = new System.Drawing.Size(124, 22);
            this.txtTotalOrderMarginAmount1.TabIndex = 15;
            this.txtTotalOrderMarginAmount1.Text = "0";
            this.txtTotalOrderMarginAmount1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtTotalOrderMarginAmount0
            // 
            this.txtTotalOrderMarginAmount0.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTotalOrderMarginAmount0.ForeColor = System.Drawing.Color.Blue;
            this.txtTotalOrderMarginAmount0.Location = new System.Drawing.Point(353, 0);
            this.txtTotalOrderMarginAmount0.Name = "txtTotalOrderMarginAmount0";
            this.txtTotalOrderMarginAmount0.Size = new System.Drawing.Size(124, 22);
            this.txtTotalOrderMarginAmount0.TabIndex = 21;
            this.txtTotalOrderMarginAmount0.Text = "0";
            this.txtTotalOrderMarginAmount0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label26.Location = new System.Drawing.Point(492, 5);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(14, 12);
            this.label26.TabIndex = 16;
            this.label26.Text = "S";
            // 
            // txtTotalOrderMarginAmount3
            // 
            this.txtTotalOrderMarginAmount3.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTotalOrderMarginAmount3.ForeColor = System.Drawing.Color.Blue;
            this.txtTotalOrderMarginAmount3.Location = new System.Drawing.Point(840, 0);
            this.txtTotalOrderMarginAmount3.Name = "txtTotalOrderMarginAmount3";
            this.txtTotalOrderMarginAmount3.Size = new System.Drawing.Size(124, 22);
            this.txtTotalOrderMarginAmount3.TabIndex = 19;
            this.txtTotalOrderMarginAmount3.Text = "0";
            this.txtTotalOrderMarginAmount3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label23.Location = new System.Drawing.Point(9, 5);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(57, 12);
            this.label23.TabIndex = 14;
            this.label23.Text = "오퍼수량";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label28.Location = new System.Drawing.Point(801, 5);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(33, 12);
            this.label28.TabIndex = 20;
            this.label28.Text = "SPO";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label27.Location = new System.Drawing.Point(642, 5);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(23, 12);
            this.label27.TabIndex = 18;
            this.label27.Text = "SP";
            // 
            // txtTotalOrderMarginAmount2
            // 
            this.txtTotalOrderMarginAmount2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTotalOrderMarginAmount2.ForeColor = System.Drawing.Color.Blue;
            this.txtTotalOrderMarginAmount2.Location = new System.Drawing.Point(671, 0);
            this.txtTotalOrderMarginAmount2.Name = "txtTotalOrderMarginAmount2";
            this.txtTotalOrderMarginAmount2.Size = new System.Drawing.Size(124, 22);
            this.txtTotalOrderMarginAmount2.TabIndex = 17;
            this.txtTotalOrderMarginAmount2.Text = "0";
            this.txtTotalOrderMarginAmount2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1115, 27);
            this.panel3.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.rbCostPriceSPO);
            this.panel5.Controls.Add(this.rbCostPriceSP);
            this.panel5.Controls.Add(this.rbCostPriceO);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.rbCostPriceS);
            this.panel5.Controls.Add(this.rbCostPriceP);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(307, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(383, 27);
            this.panel5.TabIndex = 4;
            // 
            // rbCostPriceSPO
            // 
            this.rbCostPriceSPO.AutoSize = true;
            this.rbCostPriceSPO.Location = new System.Drawing.Point(246, 7);
            this.rbCostPriceSPO.Name = "rbCostPriceSPO";
            this.rbCostPriceSPO.Size = new System.Drawing.Size(48, 16);
            this.rbCostPriceSPO.TabIndex = 5;
            this.rbCostPriceSPO.Text = "SPO";
            this.rbCostPriceSPO.UseVisualStyleBackColor = true;
            this.rbCostPriceSPO.CheckedChanged += new System.EventHandler(this.rbSalesPrice_CheckedChanged);
            // 
            // rbCostPriceSP
            // 
            this.rbCostPriceSP.AutoSize = true;
            this.rbCostPriceSP.Location = new System.Drawing.Point(201, 7);
            this.rbCostPriceSP.Name = "rbCostPriceSP";
            this.rbCostPriceSP.Size = new System.Drawing.Size(39, 16);
            this.rbCostPriceSP.TabIndex = 4;
            this.rbCostPriceSP.Text = "SP";
            this.rbCostPriceSP.UseVisualStyleBackColor = true;
            this.rbCostPriceSP.CheckedChanged += new System.EventHandler(this.rbSalesPrice_CheckedChanged);
            // 
            // rbCostPriceO
            // 
            this.rbCostPriceO.AutoSize = true;
            this.rbCostPriceO.Location = new System.Drawing.Point(163, 7);
            this.rbCostPriceO.Name = "rbCostPriceO";
            this.rbCostPriceO.Size = new System.Drawing.Size(32, 16);
            this.rbCostPriceO.TabIndex = 3;
            this.rbCostPriceO.Text = "O";
            this.rbCostPriceO.UseVisualStyleBackColor = true;
            this.rbCostPriceO.CheckedChanged += new System.EventHandler(this.rbSalesPrice_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "비교원가";
            // 
            // rbCostPriceS
            // 
            this.rbCostPriceS.AutoSize = true;
            this.rbCostPriceS.Checked = true;
            this.rbCostPriceS.Location = new System.Drawing.Point(88, 7);
            this.rbCostPriceS.Name = "rbCostPriceS";
            this.rbCostPriceS.Size = new System.Drawing.Size(31, 16);
            this.rbCostPriceS.TabIndex = 0;
            this.rbCostPriceS.TabStop = true;
            this.rbCostPriceS.Text = "S";
            this.rbCostPriceS.UseVisualStyleBackColor = true;
            this.rbCostPriceS.CheckedChanged += new System.EventHandler(this.rbSalesPrice_CheckedChanged);
            // 
            // rbCostPriceP
            // 
            this.rbCostPriceP.AutoSize = true;
            this.rbCostPriceP.Location = new System.Drawing.Point(126, 7);
            this.rbCostPriceP.Name = "rbCostPriceP";
            this.rbCostPriceP.Size = new System.Drawing.Size(31, 16);
            this.rbCostPriceP.TabIndex = 1;
            this.rbCostPriceP.Text = "P";
            this.rbCostPriceP.UseVisualStyleBackColor = true;
            this.rbCostPriceP.CheckedChanged += new System.EventHandler(this.rbSalesPrice_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.rbSalesPrice);
            this.panel4.Controls.Add(this.rbNormalPrice);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(307, 27);
            this.panel4.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "비교단가";
            // 
            // rbSalesPrice
            // 
            this.rbSalesPrice.AutoSize = true;
            this.rbSalesPrice.Checked = true;
            this.rbSalesPrice.Location = new System.Drawing.Point(79, 7);
            this.rbSalesPrice.Name = "rbSalesPrice";
            this.rbSalesPrice.Size = new System.Drawing.Size(71, 16);
            this.rbSalesPrice.TabIndex = 0;
            this.rbSalesPrice.TabStop = true;
            this.rbSalesPrice.Text = "매출단가";
            this.rbSalesPrice.UseVisualStyleBackColor = true;
            this.rbSalesPrice.CheckedChanged += new System.EventHandler(this.rbSalesPrice_CheckedChanged);
            // 
            // rbNormalPrice
            // 
            this.rbNormalPrice.AutoSize = true;
            this.rbNormalPrice.Location = new System.Drawing.Point(156, 7);
            this.rbNormalPrice.Name = "rbNormalPrice";
            this.rbNormalPrice.Size = new System.Drawing.Size(71, 16);
            this.rbNormalPrice.TabIndex = 1;
            this.rbNormalPrice.Text = "일반시세";
            this.rbNormalPrice.UseVisualStyleBackColor = true;
            this.rbNormalPrice.CheckedChanged += new System.EventHandler(this.rbSalesPrice_CheckedChanged);
            // 
            // MarginDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1115, 713);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MarginDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "손익 자세히";
            this.Load += new System.EventHandler(this.MarginDetails_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProduct;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbNormalPrice;
        private System.Windows.Forms.RadioButton rbSalesPrice;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.RadioButton rbCostPriceSPO;
        private System.Windows.Forms.RadioButton rbCostPriceSP;
        private System.Windows.Forms.RadioButton rbCostPriceO;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbCostPriceS;
        private System.Windows.Forms.RadioButton rbCostPriceP;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn price_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_stock;
        private System.Windows.Forms.DataGridViewTextBoxColumn pending_stock;
        private System.Windows.Forms.DataGridViewTextBoxColumn offer_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn sales_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn normal_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_temp_amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_cost_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn pending_cost_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn offer_cost_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn average_cost_price1;
        private System.Windows.Forms.DataGridViewTextBoxColumn average_cost_price2;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_temp_cost_amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_temp_cost_margin_amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_temp_cost_margin;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.TextBox txtTotalOrderQty;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox txtTotalOrderMarginAmount1;
        private System.Windows.Forms.TextBox txtTotalOrderMarginAmount0;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox txtTotalOrderMarginAmount3;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox txtTotalOrderMarginAmount2;
    }
}