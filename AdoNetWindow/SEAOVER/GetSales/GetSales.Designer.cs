namespace AdoNetWindow.SEAOVER.GetSales
{
    partial class GetSales
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetSales));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtPurchaseCompany = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSizes = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSaleCompany = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnEnddateCalendar = new System.Windows.Forms.Button();
            this.txtEnddate = new System.Windows.Forms.TextBox();
            this.btnSttdateCalendar = new System.Windows.Forms.Button();
            this.txtSttdate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvSales = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.sale_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.is_tax = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.purchase_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchase_company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnSearching = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbCurrent = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtPurchaseCompany);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtUnit);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtSizes);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtOrigin);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtProduct);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtSaleCompany);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnEnddateCalendar);
            this.panel1.Controls.Add(this.txtEnddate);
            this.panel1.Controls.Add(this.btnSttdateCalendar);
            this.panel1.Controls.Add(this.txtSttdate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1467, 27);
            this.panel1.TabIndex = 0;
            // 
            // txtPurchaseCompany
            // 
            this.txtPurchaseCompany.Location = new System.Drawing.Point(1257, 3);
            this.txtPurchaseCompany.Name = "txtPurchaseCompany";
            this.txtPurchaseCompany.Size = new System.Drawing.Size(198, 21);
            this.txtPurchaseCompany.TabIndex = 19;
            this.txtPurchaseCompany.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSttdate_KeyDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1210, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 18;
            this.label8.Text = "매입처";
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(956, 3);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(94, 21);
            this.txtUnit.TabIndex = 13;
            this.txtUnit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSttdate_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(921, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "단위";
            // 
            // txtSizes
            // 
            this.txtSizes.Location = new System.Drawing.Point(781, 3);
            this.txtSizes.Name = "txtSizes";
            this.txtSizes.Size = new System.Drawing.Size(134, 21);
            this.txtSizes.TabIndex = 12;
            this.txtSizes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSttdate_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(746, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "규격";
            // 
            // txtOrigin
            // 
            this.txtOrigin.Location = new System.Drawing.Point(1103, 3);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.Size = new System.Drawing.Size(101, 21);
            this.txtOrigin.TabIndex = 14;
            this.txtOrigin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSttdate_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1056, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "원산지";
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(571, 3);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(169, 21);
            this.txtProduct.TabIndex = 11;
            this.txtProduct.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSttdate_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(536, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "품명";
            // 
            // txtSaleCompany
            // 
            this.txtSaleCompany.Location = new System.Drawing.Point(361, 3);
            this.txtSaleCompany.Name = "txtSaleCompany";
            this.txtSaleCompany.Size = new System.Drawing.Size(169, 21);
            this.txtSaleCompany.TabIndex = 9;
            this.txtSaleCompany.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSttdate_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(314, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "매출처";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(181, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "~";
            // 
            // btnEnddateCalendar
            // 
            this.btnEnddateCalendar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEnddateCalendar.BackgroundImage")));
            this.btnEnddateCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEnddateCalendar.FlatAppearance.BorderSize = 0;
            this.btnEnddateCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnddateCalendar.Location = new System.Drawing.Point(286, 1);
            this.btnEnddateCalendar.Name = "btnEnddateCalendar";
            this.btnEnddateCalendar.Size = new System.Drawing.Size(22, 23);
            this.btnEnddateCalendar.TabIndex = 6;
            this.btnEnddateCalendar.UseVisualStyleBackColor = true;
            this.btnEnddateCalendar.Click += new System.EventHandler(this.btnEnddateCalendar_Click);
            // 
            // txtEnddate
            // 
            this.txtEnddate.Location = new System.Drawing.Point(201, 3);
            this.txtEnddate.Name = "txtEnddate";
            this.txtEnddate.Size = new System.Drawing.Size(84, 21);
            this.txtEnddate.TabIndex = 5;
            this.txtEnddate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSttdate_KeyDown);
            // 
            // btnSttdateCalendar
            // 
            this.btnSttdateCalendar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSttdateCalendar.BackgroundImage")));
            this.btnSttdateCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSttdateCalendar.FlatAppearance.BorderSize = 0;
            this.btnSttdateCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSttdateCalendar.Location = new System.Drawing.Point(153, 1);
            this.btnSttdateCalendar.Name = "btnSttdateCalendar";
            this.btnSttdateCalendar.Size = new System.Drawing.Size(22, 23);
            this.btnSttdateCalendar.TabIndex = 4;
            this.btnSttdateCalendar.UseVisualStyleBackColor = true;
            this.btnSttdateCalendar.Click += new System.EventHandler(this.btnSttdateCalendar_Click);
            // 
            // txtSttdate
            // 
            this.txtSttdate.Location = new System.Drawing.Point(68, 3);
            this.txtSttdate.Name = "txtSttdate";
            this.txtSttdate.Size = new System.Drawing.Size(84, 21);
            this.txtSttdate.TabIndex = 1;
            this.txtSttdate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSttdate_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "매출기간";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvSales);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 54);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1467, 854);
            this.panel2.TabIndex = 1;
            // 
            // dgvSales
            // 
            this.dgvSales.AllowUserToAddRows = false;
            this.dgvSales.AllowUserToDeleteRows = false;
            this.dgvSales.AllowUserToResizeRows = false;
            this.dgvSales.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sale_date,
            this.division,
            this.sale_company,
            this.product,
            this.origin,
            this.sizes,
            this.unit,
            this.warehouse,
            this.sale_qty,
            this.sale_price,
            this.is_tax,
            this.purchase_price,
            this.purchase_company,
            this.manager});
            this.dgvSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSales.EnableHeadersVisualStyles = false;
            this.dgvSales.Location = new System.Drawing.Point(0, 0);
            this.dgvSales.Name = "dgvSales";
            this.dgvSales.RowTemplate.Height = 23;
            this.dgvSales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSales.Size = new System.Drawing.Size(1467, 854);
            this.dgvSales.TabIndex = 19;
            this.dgvSales.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSales_CellMouseDoubleClick);
            this.dgvSales.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvSales_DataError);
            // 
            // sale_date
            // 
            this.sale_date.HeaderText = "매출일자";
            this.sale_date.Name = "sale_date";
            this.sale_date.Width = 80;
            // 
            // division
            // 
            this.division.HeaderText = "구분";
            this.division.Name = "division";
            this.division.Width = 50;
            // 
            // sale_company
            // 
            this.sale_company.HeaderText = "매출처";
            this.sale_company.Name = "sale_company";
            this.sale_company.Width = 200;
            // 
            // product
            // 
            this.product.HeaderText = "품명";
            this.product.Name = "product";
            this.product.Width = 200;
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
            this.sizes.Width = 150;
            // 
            // unit
            // 
            this.unit.HeaderText = "단위";
            this.unit.Name = "unit";
            this.unit.Width = 50;
            // 
            // warehouse
            // 
            this.warehouse.HeaderText = "보관처";
            this.warehouse.Name = "warehouse";
            // 
            // sale_qty
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.sale_qty.DefaultCellStyle = dataGridViewCellStyle4;
            this.sale_qty.HeaderText = "매출수";
            this.sale_qty.Name = "sale_qty";
            this.sale_qty.Width = 60;
            // 
            // sale_price
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.sale_price.DefaultCellStyle = dataGridViewCellStyle5;
            this.sale_price.HeaderText = "매출단가";
            this.sale_price.Name = "sale_price";
            this.sale_price.Width = 80;
            // 
            // is_tax
            // 
            this.is_tax.HeaderText = "과세";
            this.is_tax.Name = "is_tax";
            this.is_tax.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.is_tax.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.is_tax.Width = 40;
            // 
            // purchase_price
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.purchase_price.DefaultCellStyle = dataGridViewCellStyle6;
            this.purchase_price.HeaderText = "매입단가";
            this.purchase_price.Name = "purchase_price";
            this.purchase_price.Width = 80;
            // 
            // purchase_company
            // 
            this.purchase_company.HeaderText = "매입처";
            this.purchase_company.Name = "purchase_company";
            this.purchase_company.Width = 150;
            // 
            // manager
            // 
            this.manager.HeaderText = "담당자";
            this.manager.Name = "manager";
            this.manager.Width = 80;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.btnSelect);
            this.panel3.Controls.Add(this.btnSearching);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 908);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1467, 39);
            this.panel3.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.ForeColor = System.Drawing.Color.Blue;
            this.label9.Location = new System.Drawing.Point(8, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 16);
            this.label9.TabIndex = 19;
            this.label9.Text = "ALT + ";
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelect.ForeColor = System.Drawing.Color.Blue;
            this.btnSelect.Location = new System.Drawing.Point(212, 1);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(66, 37);
            this.btnSelect.TabIndex = 18;
            this.btnSelect.Text = "선택(S)";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Visible = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.ForeColor = System.Drawing.Color.Black;
            this.btnSearching.Location = new System.Drawing.Point(68, 1);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(66, 37);
            this.btnSearching.TabIndex = 16;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(140, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 17;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.rbCurrent);
            this.panel4.Controls.Add(this.rbAll);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1467, 27);
            this.panel4.TabIndex = 3;
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Checked = true;
            this.rbAll.Location = new System.Drawing.Point(12, 5);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(74, 16);
            this.rbAll.TabIndex = 0;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "전체 (F2)";
            this.rbAll.UseVisualStyleBackColor = true;
            this.rbAll.CheckedChanged += new System.EventHandler(this.rbAll_CheckedChanged);
            // 
            // rbCurrent
            // 
            this.rbCurrent.AutoSize = true;
            this.rbCurrent.Location = new System.Drawing.Point(92, 5);
            this.rbCurrent.Name = "rbCurrent";
            this.rbCurrent.Size = new System.Drawing.Size(98, 16);
            this.rbCurrent.TabIndex = 1;
            this.rbCurrent.Text = "최근매출 (F2)";
            this.rbCurrent.UseVisualStyleBackColor = true;
            this.rbCurrent.CheckedChanged += new System.EventHandler(this.rbAll_CheckedChanged);
            // 
            // GetSales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1467, 947);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "GetSales";
            this.Text = "매출내역";
            this.Load += new System.EventHandler(this.GetSales_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GetSales_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtSttdate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnEnddateCalendar;
        private System.Windows.Forms.TextBox txtEnddate;
        private System.Windows.Forms.Button btnSttdateCalendar;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSizes;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSaleCompany;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvSales;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtPurchaseCompany;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn division;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_company;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehouse;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_price;
        private System.Windows.Forms.DataGridViewCheckBoxColumn is_tax;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchase_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchase_company;
        private System.Windows.Forms.DataGridViewTextBoxColumn manager;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton rbCurrent;
        private System.Windows.Forms.RadioButton rbAll;
    }
}