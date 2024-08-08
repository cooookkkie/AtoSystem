namespace AdoNetWindow.SEAOVER
{
    partial class ExcludedItems
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExcludedItems));
            this.btnExit = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.dgvProduct = new System.Windows.Forms.DataGridView();
            this.category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sales_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price_change = new System.Windows.Forms.DataGridViewButtonColumn();
            this.seaover_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.category_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rowindex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seaover_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.is_delete = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtDivision = new System.Windows.Forms.TextBox();
            this.txtSizes = new System.Windows.Forms.TextBox();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnStockRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(190, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(81, 33);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "닫기(ESC)";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnInsert.ForeColor = System.Drawing.Color.White;
            this.btnInsert.Location = new System.Drawing.Point(5, 2);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(81, 33);
            this.btnInsert.TabIndex = 7;
            this.btnInsert.Text = "적용(A)";
            this.btnInsert.UseVisualStyleBackColor = false;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToResizeRows = false;
            this.dgvProduct.ColumnHeadersHeight = 30;
            this.dgvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.category,
            this.product,
            this.origin,
            this.weight,
            this.sizes,
            this.division,
            this.sales_price,
            this.price_change,
            this.seaover_price,
            this.category_code,
            this.product_code,
            this.origin_code,
            this.sizes_code,
            this.rowindex,
            this.unit,
            this.price_unit,
            this.unit_count,
            this.seaover_unit,
            this.is_delete});
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.Location = new System.Drawing.Point(0, 0);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowHeadersWidth = 40;
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.Size = new System.Drawing.Size(1001, 550);
            this.dgvProduct.TabIndex = 1;
            this.dgvProduct.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellContentClick);
            this.dgvProduct.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProduct_CellMouseClick);
            this.dgvProduct.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvProduct_MouseUp);
            // 
            // category
            // 
            this.category.HeaderText = "대분류";
            this.category.Name = "category";
            this.category.Width = 70;
            // 
            // product
            // 
            this.product.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.product.HeaderText = "품명";
            this.product.Name = "product";
            // 
            // origin
            // 
            this.origin.HeaderText = "산지";
            this.origin.Name = "origin";
            this.origin.Width = 120;
            // 
            // weight
            // 
            this.weight.FillWeight = 150F;
            this.weight.HeaderText = "중량";
            this.weight.Name = "weight";
            this.weight.Width = 150;
            // 
            // sizes
            // 
            this.sizes.HeaderText = "사이즈";
            this.sizes.Name = "sizes";
            this.sizes.Width = 127;
            // 
            // division
            // 
            this.division.HeaderText = "구분";
            this.division.Name = "division";
            this.division.Width = 40;
            // 
            // sales_price
            // 
            this.sales_price.HeaderText = "매출단가";
            this.sales_price.Name = "sales_price";
            this.sales_price.Width = 80;
            // 
            // price_change
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "변경";
            this.price_change.DefaultCellStyle = dataGridViewCellStyle1;
            this.price_change.HeaderText = "변경";
            this.price_change.Name = "price_change";
            this.price_change.Text = "";
            this.price_change.Width = 40;
            // 
            // seaover_price
            // 
            this.seaover_price.HeaderText = "갱신단가";
            this.seaover_price.Name = "seaover_price";
            // 
            // category_code
            // 
            this.category_code.HeaderText = "대분류 코드";
            this.category_code.Name = "category_code";
            this.category_code.Width = 70;
            // 
            // product_code
            // 
            this.product_code.HeaderText = "품목코드";
            this.product_code.Name = "product_code";
            this.product_code.Width = 5;
            // 
            // origin_code
            // 
            this.origin_code.HeaderText = "산지코드";
            this.origin_code.Name = "origin_code";
            this.origin_code.Width = 5;
            // 
            // sizes_code
            // 
            this.sizes_code.HeaderText = "규격코드";
            this.sizes_code.Name = "sizes_code";
            this.sizes_code.Width = 5;
            // 
            // rowindex
            // 
            this.rowindex.HeaderText = "row";
            this.rowindex.Name = "rowindex";
            this.rowindex.Width = 80;
            // 
            // unit
            // 
            this.unit.HeaderText = "unit";
            this.unit.Name = "unit";
            // 
            // price_unit
            // 
            this.price_unit.HeaderText = "price_unit";
            this.price_unit.Name = "price_unit";
            // 
            // unit_count
            // 
            this.unit_count.HeaderText = "unit_count";
            this.unit_count.Name = "unit_count";
            // 
            // seaover_unit
            // 
            this.seaover_unit.HeaderText = "seaover_unit";
            this.seaover_unit.Name = "seaover_unit";
            // 
            // is_delete
            // 
            this.is_delete.HeaderText = "삭제";
            this.is_delete.Name = "is_delete";
            this.is_delete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.is_delete.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.is_delete.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1001, 628);
            this.panel2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dgvProduct);
            this.panel4.Controls.Add(this.panel1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 41);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1001, 587);
            this.panel4.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtDivision);
            this.panel3.Controls.Add(this.txtSizes);
            this.panel3.Controls.Add(this.txtWeight);
            this.panel3.Controls.Add(this.txtOrigin);
            this.panel3.Controls.Add(this.txtProduct);
            this.panel3.Controls.Add(this.txtCategory);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1001, 41);
            this.panel3.TabIndex = 2;
            // 
            // txtDivision
            // 
            this.txtDivision.Location = new System.Drawing.Point(708, 10);
            this.txtDivision.Name = "txtDivision";
            this.txtDivision.Size = new System.Drawing.Size(42, 21);
            this.txtDivision.TabIndex = 6;
            // 
            // txtSizes
            // 
            this.txtSizes.Location = new System.Drawing.Point(581, 10);
            this.txtSizes.Name = "txtSizes";
            this.txtSizes.Size = new System.Drawing.Size(126, 21);
            this.txtSizes.TabIndex = 5;
            // 
            // txtWeight
            // 
            this.txtWeight.Location = new System.Drawing.Point(430, 10);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(149, 21);
            this.txtWeight.TabIndex = 4;
            // 
            // txtOrigin
            // 
            this.txtOrigin.Location = new System.Drawing.Point(311, 10);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.Size = new System.Drawing.Size(118, 21);
            this.txtOrigin.TabIndex = 3;
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(113, 10);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(197, 21);
            this.txtProduct.TabIndex = 2;
            this.txtProduct.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // txtCategory
            // 
            this.txtCategory.Location = new System.Drawing.Point(39, 10);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(72, 21);
            this.txtCategory.TabIndex = 1;
            this.txtCategory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCategory_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "검색";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnStockRefresh);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnInsert);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 550);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1001, 37);
            this.panel1.TabIndex = 2;
            // 
            // btnStockRefresh
            // 
            this.btnStockRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnStockRefresh.ForeColor = System.Drawing.Color.White;
            this.btnStockRefresh.Location = new System.Drawing.Point(92, 2);
            this.btnStockRefresh.Name = "btnStockRefresh";
            this.btnStockRefresh.Size = new System.Drawing.Size(92, 33);
            this.btnStockRefresh.TabIndex = 9;
            this.btnStockRefresh.Text = "재고확인 (Q)";
            this.btnStockRefresh.UseVisualStyleBackColor = false;
            this.btnStockRefresh.Click += new System.EventHandler(this.btnStockRefresh_Click);
            // 
            // ExcludedItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 628);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExcludedItems";
            this.Text = "* 매출단가 0원인 품목중 단가갱신 품목을 선택해주세요!";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ExcludedItems_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExcludedItems_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvProduct;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtDivision;
        private System.Windows.Forms.TextBox txtSizes;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn category;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn weight;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn division;
        private System.Windows.Forms.DataGridViewTextBoxColumn sales_price;
        private System.Windows.Forms.DataGridViewButtonColumn price_change;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn category_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowindex;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn price_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_unit;
        private System.Windows.Forms.DataGridViewCheckBoxColumn is_delete;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnStockRefresh;
    }
}