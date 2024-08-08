namespace AdoNetWindow.DashboardForSales.MultiDashboard
{
    partial class SelectProduct2
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectProduct2));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnWarehouse = new System.Windows.Forms.FlowLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dgvSelectProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.select_product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_price_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_unit_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_seaover_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSelect = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgvWarehouseProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seaover_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shipment_stock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shipping_stock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unpending_stock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pending_stock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reserved_stock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sales_total_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sales_day_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sales_month_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.month_around = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lbWarehouse = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.cbSaleTerm = new System.Windows.Forms.ComboBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectProduct)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWarehouseProduct)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 793);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(986, 40);
            this.panel1.TabIndex = 0;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(8, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(64, 32);
            this.btnExit.TabIndex = 109;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pnWarehouse);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(986, 255);
            this.panel2.TabIndex = 1;
            // 
            // pnWarehouse
            // 
            this.pnWarehouse.AutoScroll = true;
            this.pnWarehouse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnWarehouse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnWarehouse.Location = new System.Drawing.Point(503, 0);
            this.pnWarehouse.Name = "pnWarehouse";
            this.pnWarehouse.Size = new System.Drawing.Size(483, 255);
            this.pnWarehouse.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.dgvSelectProduct);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(503, 255);
            this.panel3.TabIndex = 0;
            // 
            // dgvSelectProduct
            // 
            this.dgvSelectProduct.AllowUserToAddRows = false;
            this.dgvSelectProduct.AllowUserToDeleteRows = false;
            this.dgvSelectProduct.AllowUserToResizeRows = false;
            this.dgvSelectProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelectProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.select_product,
            this.select_origin,
            this.select_sizes,
            this.select_unit,
            this.select_price_unit,
            this.select_unit_count,
            this.select_seaover_unit,
            this.btnSelect});
            this.dgvSelectProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSelectProduct.EnableHeadersVisualStyles = false;
            this.dgvSelectProduct.Location = new System.Drawing.Point(0, 21);
            this.dgvSelectProduct.Name = "dgvSelectProduct";
            this.dgvSelectProduct.RowHeadersWidth = 20;
            this.dgvSelectProduct.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvSelectProduct.RowTemplate.Height = 23;
            this.dgvSelectProduct.Size = new System.Drawing.Size(501, 232);
            this.dgvSelectProduct.TabIndex = 1;
            this.dgvSelectProduct.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSelectProduct_CellContentClick);
            // 
            // select_product
            // 
            this.select_product.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.select_product.HeaderText = "품명";
            this.select_product.Name = "select_product";
            // 
            // select_origin
            // 
            this.select_origin.HeaderText = "원산지";
            this.select_origin.Name = "select_origin";
            this.select_origin.Width = 70;
            // 
            // select_sizes
            // 
            this.select_sizes.HeaderText = "규격";
            this.select_sizes.Name = "select_sizes";
            // 
            // select_unit
            // 
            this.select_unit.HeaderText = "단위";
            this.select_unit.Name = "select_unit";
            this.select_unit.Width = 40;
            // 
            // select_price_unit
            // 
            this.select_price_unit.HeaderText = "P/U";
            this.select_price_unit.Name = "select_price_unit";
            this.select_price_unit.Width = 40;
            // 
            // select_unit_count
            // 
            this.select_unit_count.HeaderText = "U/C";
            this.select_unit_count.Name = "select_unit_count";
            this.select_unit_count.Width = 40;
            // 
            // select_seaover_unit
            // 
            this.select_seaover_unit.HeaderText = "S/U";
            this.select_seaover_unit.Name = "select_seaover_unit";
            this.select_seaover_unit.Width = 40;
            // 
            // btnSelect
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.NullValue = "선택";
            this.btnSelect.DefaultCellStyle = dataGridViewCellStyle1;
            this.btnSelect.HeaderText = "";
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.btnSelect.Width = 40;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(501, 21);
            this.panel4.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "* 품목리스트";
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.dgvWarehouseProduct);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 255);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(986, 538);
            this.panel5.TabIndex = 2;
            // 
            // dgvWarehouseProduct
            // 
            this.dgvWarehouseProduct.AllowUserToAddRows = false;
            this.dgvWarehouseProduct.AllowUserToDeleteRows = false;
            this.dgvWarehouseProduct.AllowUserToResizeRows = false;
            this.dgvWarehouseProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvWarehouseProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.product,
            this.origin,
            this.sizes,
            this.unit,
            this.price_unit,
            this.unit_count,
            this.seaover_unit,
            this.manager,
            this.stock,
            this.shipment_stock,
            this.shipping_stock,
            this.unpending_stock,
            this.pending_stock,
            this.reserved_stock,
            this.sales_total_count,
            this.sales_day_count,
            this.sales_month_count,
            this.month_around});
            this.dgvWarehouseProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvWarehouseProduct.EnableHeadersVisualStyles = false;
            this.dgvWarehouseProduct.Location = new System.Drawing.Point(0, 21);
            this.dgvWarehouseProduct.Name = "dgvWarehouseProduct";
            this.dgvWarehouseProduct.RowHeadersWidth = 20;
            this.dgvWarehouseProduct.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvWarehouseProduct.RowTemplate.Height = 23;
            this.dgvWarehouseProduct.Size = new System.Drawing.Size(984, 515);
            this.dgvWarehouseProduct.TabIndex = 3;
            this.dgvWarehouseProduct.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvWarehouseProduct_CellMouseClick);
            this.dgvWarehouseProduct.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvWarehouseProduct_MouseUp);
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
            this.origin.Width = 70;
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
            this.unit.Width = 40;
            // 
            // price_unit
            // 
            this.price_unit.HeaderText = "P/U";
            this.price_unit.Name = "price_unit";
            this.price_unit.Width = 40;
            // 
            // unit_count
            // 
            this.unit_count.HeaderText = "U/C";
            this.unit_count.Name = "unit_count";
            this.unit_count.Width = 40;
            // 
            // seaover_unit
            // 
            this.seaover_unit.HeaderText = "S/U";
            this.seaover_unit.Name = "seaover_unit";
            this.seaover_unit.Width = 40;
            // 
            // manager
            // 
            this.manager.HeaderText = "담당자";
            this.manager.Name = "manager";
            this.manager.Width = 50;
            // 
            // stock
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.stock.DefaultCellStyle = dataGridViewCellStyle2;
            this.stock.HeaderText = "총재고";
            this.stock.Name = "stock";
            this.stock.Width = 50;
            // 
            // shipment_stock
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.shipment_stock.DefaultCellStyle = dataGridViewCellStyle3;
            this.shipment_stock.HeaderText = "선적전";
            this.shipment_stock.Name = "shipment_stock";
            this.shipment_stock.Width = 50;
            // 
            // shipping_stock
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.shipping_stock.DefaultCellStyle = dataGridViewCellStyle4;
            this.shipping_stock.HeaderText = "배송중";
            this.shipping_stock.Name = "shipping_stock";
            this.shipping_stock.Width = 50;
            // 
            // unpending_stock
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.unpending_stock.DefaultCellStyle = dataGridViewCellStyle5;
            this.unpending_stock.HeaderText = "미통관";
            this.unpending_stock.Name = "unpending_stock";
            this.unpending_stock.Width = 50;
            // 
            // pending_stock
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.pending_stock.DefaultCellStyle = dataGridViewCellStyle6;
            this.pending_stock.HeaderText = "통관";
            this.pending_stock.Name = "pending_stock";
            this.pending_stock.Width = 50;
            // 
            // reserved_stock
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.reserved_stock.DefaultCellStyle = dataGridViewCellStyle7;
            this.reserved_stock.HeaderText = "예약";
            this.reserved_stock.Name = "reserved_stock";
            this.reserved_stock.Width = 50;
            // 
            // sales_total_count
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.sales_total_count.DefaultCellStyle = dataGridViewCellStyle8;
            this.sales_total_count.HeaderText = "총매출";
            this.sales_total_count.Name = "sales_total_count";
            this.sales_total_count.Visible = false;
            this.sales_total_count.Width = 50;
            // 
            // sales_day_count
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.sales_day_count.DefaultCellStyle = dataGridViewCellStyle9;
            this.sales_day_count.HeaderText = "일매출";
            this.sales_day_count.Name = "sales_day_count";
            this.sales_day_count.Width = 50;
            // 
            // sales_month_count
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.sales_month_count.DefaultCellStyle = dataGridViewCellStyle10;
            this.sales_month_count.HeaderText = "월매출";
            this.sales_month_count.Name = "sales_month_count";
            this.sales_month_count.Width = 50;
            // 
            // month_around
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.month_around.DefaultCellStyle = dataGridViewCellStyle11;
            this.month_around.HeaderText = "회전율";
            this.month_around.Name = "month_around";
            this.month_around.Width = 50;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel7);
            this.panel6.Controls.Add(this.lbWarehouse);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(984, 21);
            this.panel6.TabIndex = 2;
            // 
            // lbWarehouse
            // 
            this.lbWarehouse.AutoSize = true;
            this.lbWarehouse.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbWarehouse.Location = new System.Drawing.Point(5, 5);
            this.lbWarehouse.Name = "lbWarehouse";
            this.lbWarehouse.Size = new System.Drawing.Size(17, 12);
            this.lbWarehouse.TabIndex = 156;
            this.lbWarehouse.Text = "* ";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label42.Location = new System.Drawing.Point(62, 5);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(57, 12);
            this.label42.TabIndex = 154;
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
            this.cbSaleTerm.Location = new System.Drawing.Point(125, 1);
            this.cbSaleTerm.Name = "cbSaleTerm";
            this.cbSaleTerm.Size = new System.Drawing.Size(58, 20);
            this.cbSaleTerm.TabIndex = 155;
            this.cbSaleTerm.Text = "1개월";
            this.cbSaleTerm.SelectedIndexChanged += new System.EventHandler(this.cbSaleTerm_SelectedIndexChanged);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label42);
            this.panel7.Controls.Add(this.cbSaleTerm);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel7.Location = new System.Drawing.Point(801, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(183, 21);
            this.panel7.TabIndex = 157;
            // 
            // SelectProduct2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 833);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "SelectProduct2";
            this.Text = "SelectProduct2";
            this.Load += new System.EventHandler(this.SelectProduct2_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SelectProduct2_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectProduct)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWarehouseProduct)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.FlowLayoutPanel pnWarehouse;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvWarehouseProduct;
        private System.Windows.Forms.Panel panel6;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvSelectProduct;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.ComboBox cbSaleTerm;
        private System.Windows.Forms.Label lbWarehouse;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_product;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_sizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_price_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_unit_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_seaover_unit;
        private System.Windows.Forms.DataGridViewButtonColumn btnSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn price_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn manager;
        private System.Windows.Forms.DataGridViewTextBoxColumn stock;
        private System.Windows.Forms.DataGridViewTextBoxColumn shipment_stock;
        private System.Windows.Forms.DataGridViewTextBoxColumn shipping_stock;
        private System.Windows.Forms.DataGridViewTextBoxColumn unpending_stock;
        private System.Windows.Forms.DataGridViewTextBoxColumn pending_stock;
        private System.Windows.Forms.DataGridViewTextBoxColumn reserved_stock;
        private System.Windows.Forms.DataGridViewTextBoxColumn sales_total_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn sales_day_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn sales_month_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn month_around;
        private System.Windows.Forms.Panel panel7;
    }
}