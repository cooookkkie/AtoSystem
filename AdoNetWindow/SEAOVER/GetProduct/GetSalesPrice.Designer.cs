namespace AdoNetWindow.SaleManagement
{
    partial class GetSalesPrice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetSalesPrice));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtPurchaseCompany = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtWarehouse = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDivision = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSizes = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.package_cnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seaover_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.input_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchase_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchase_company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchase_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnSearching = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtPurchaseCompany);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtWarehouse);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtDivision);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtUnit);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtSizes);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtOrigin);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtProduct);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1442, 30);
            this.panel1.TabIndex = 0;
            // 
            // txtPurchaseCompany
            // 
            this.txtPurchaseCompany.Location = new System.Drawing.Point(1019, 4);
            this.txtPurchaseCompany.Name = "txtPurchaseCompany";
            this.txtPurchaseCompany.Size = new System.Drawing.Size(109, 21);
            this.txtPurchaseCompany.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(972, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "매입처";
            // 
            // txtWarehouse
            // 
            this.txtWarehouse.Location = new System.Drawing.Point(853, 4);
            this.txtWarehouse.Name = "txtWarehouse";
            this.txtWarehouse.Size = new System.Drawing.Size(109, 21);
            this.txtWarehouse.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(806, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "보관처";
            // 
            // txtDivision
            // 
            this.txtDivision.Location = new System.Drawing.Point(42, 4);
            this.txtDivision.Name = "txtDivision";
            this.txtDivision.Size = new System.Drawing.Size(105, 21);
            this.txtDivision.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "구분";
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(526, 4);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(109, 21);
            this.txtUnit.TabIndex = 7;
            this.txtUnit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(491, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "단위";
            // 
            // txtSizes
            // 
            this.txtSizes.Location = new System.Drawing.Point(375, 4);
            this.txtSizes.Name = "txtSizes";
            this.txtSizes.Size = new System.Drawing.Size(109, 21);
            this.txtSizes.TabIndex = 5;
            this.txtSizes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(340, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "규격";
            // 
            // txtOrigin
            // 
            this.txtOrigin.Location = new System.Drawing.Point(691, 4);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.Size = new System.Drawing.Size(109, 21);
            this.txtOrigin.TabIndex = 9;
            this.txtOrigin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(644, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "원산지";
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(187, 4);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(144, 21);
            this.txtProduct.TabIndex = 1;
            this.txtProduct.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(156, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "품명";
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToResizeRows = false;
            this.dgvProduct.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.division,
            this.product,
            this.origin,
            this.sizes,
            this.unit,
            this.unit_count,
            this.price_unit,
            this.package_cnt,
            this.seaover_unit,
            this.input_qty,
            this.sale_price,
            this.purchase_price,
            this.purchase_company,
            this.warehouse,
            this.purchase_date,
            this.remark1,
            this.remark2});
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.EnableHeadersVisualStyles = false;
            this.dgvProduct.Location = new System.Drawing.Point(0, 0);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.Size = new System.Drawing.Size(1442, 838);
            this.dgvProduct.TabIndex = 8;
            this.dgvProduct.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProduct_CellMouseDoubleClick);
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            // 
            // division
            // 
            this.division.HeaderText = "구분";
            this.division.Name = "division";
            this.division.Width = 54;
            // 
            // product
            // 
            this.product.HeaderText = "품명";
            this.product.Name = "product";
            this.product.Width = 54;
            // 
            // origin
            // 
            this.origin.HeaderText = "원산지";
            this.origin.Name = "origin";
            this.origin.Width = 66;
            // 
            // sizes
            // 
            this.sizes.HeaderText = "규격";
            this.sizes.Name = "sizes";
            this.sizes.Width = 54;
            // 
            // unit
            // 
            this.unit.HeaderText = "단위";
            this.unit.Name = "unit";
            this.unit.Width = 54;
            // 
            // unit_count
            // 
            this.unit_count.HeaderText = "단위수량";
            this.unit_count.Name = "unit_count";
            this.unit_count.Width = 78;
            // 
            // price_unit
            // 
            this.price_unit.HeaderText = "가격단위";
            this.price_unit.Name = "price_unit";
            this.price_unit.Width = 78;
            // 
            // package_cnt
            // 
            this.package_cnt.HeaderText = "묶음수";
            this.package_cnt.Name = "package_cnt";
            this.package_cnt.Width = 66;
            // 
            // seaover_unit
            // 
            this.seaover_unit.HeaderText = "S단위";
            this.seaover_unit.Name = "seaover_unit";
            this.seaover_unit.Width = 62;
            // 
            // input_qty
            // 
            this.input_qty.HeaderText = "수량";
            this.input_qty.Name = "input_qty";
            this.input_qty.Width = 54;
            // 
            // sale_price
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.sale_price.DefaultCellStyle = dataGridViewCellStyle1;
            this.sale_price.HeaderText = "매출단가";
            this.sale_price.Name = "sale_price";
            this.sale_price.Width = 78;
            // 
            // purchase_price
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.purchase_price.DefaultCellStyle = dataGridViewCellStyle2;
            this.purchase_price.HeaderText = "매입단가";
            this.purchase_price.Name = "purchase_price";
            this.purchase_price.Width = 78;
            // 
            // purchase_company
            // 
            this.purchase_company.HeaderText = "매입처";
            this.purchase_company.Name = "purchase_company";
            this.purchase_company.Width = 66;
            // 
            // warehouse
            // 
            this.warehouse.HeaderText = "보관처";
            this.warehouse.Name = "warehouse";
            this.warehouse.Width = 66;
            // 
            // purchase_date
            // 
            this.purchase_date.HeaderText = "단가수정일";
            this.purchase_date.Name = "purchase_date";
            this.purchase_date.Width = 90;
            // 
            // remark1
            // 
            this.remark1.HeaderText = "비고1";
            this.remark1.Name = "remark1";
            this.remark1.Width = 60;
            // 
            // remark2
            // 
            this.remark2.HeaderText = "비고2";
            this.remark2.Name = "remark2";
            this.remark2.Width = 60;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvProduct);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1442, 838);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.btnSelect);
            this.panel3.Controls.Add(this.btnSearching);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 868);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1442, 39);
            this.panel3.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.ForeColor = System.Drawing.Color.Blue;
            this.label9.Location = new System.Drawing.Point(10, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 16);
            this.label9.TabIndex = 20;
            this.label9.Text = "ALT + ";
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelect.ForeColor = System.Drawing.Color.Blue;
            this.btnSelect.Location = new System.Drawing.Point(69, 2);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(66, 37);
            this.btnSelect.TabIndex = 15;
            this.btnSelect.Text = "선택(S)";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.ForeColor = System.Drawing.Color.Black;
            this.btnSearching.Location = new System.Drawing.Point(141, 2);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(66, 37);
            this.btnSearching.TabIndex = 13;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(213, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 14;
            this.btnExit.Text = "닫기(ESC)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // GetSalesPrice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1442, 907);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "GetSalesPrice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "단가 가져오기";
            this.Load += new System.EventHandler(this.GetSalesPrice_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GetSalesPrice_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProduct;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSizes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtDivision;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPurchaseCompany;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtWarehouse;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridViewTextBoxColumn division;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn price_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn package_cnt;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn input_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchase_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchase_company;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehouse;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchase_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark1;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark2;
    }
}