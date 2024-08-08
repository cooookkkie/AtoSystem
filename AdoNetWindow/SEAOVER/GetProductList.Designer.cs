namespace AdoNetWindow.SEAOVER
{
    partial class GetProductList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetProductList));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.cbVat = new System.Windows.Forms.ComboBox();
            this.txtWarehouse = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCategoryCode = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDivision = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSizes = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbCategory = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvProduct = new System.Windows.Forms.DataGridView();
            this.rightMouseMenu = new MySql.Data.MySqlClient.CustomInstaller();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.rbDecrease = new System.Windows.Forms.RadioButton();
            this.rbIncrease = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.rbNoFilter = new System.Windows.Forms.RadioButton();
            this.cbIsStock = new System.Windows.Forms.CheckBox();
            this.cbMinPrice = new System.Windows.Forms.CheckBox();
            this.cbPrice = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.cbVat);
            this.panel1.Controls.Add(this.txtWarehouse);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtCategoryCode);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtUnit);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtManager);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtDivision);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtSizes);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtOrigin);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtProduct);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtCategory);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1515, 27);
            this.panel1.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1370, 8);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 18;
            this.label10.Text = "과세여부";
            // 
            // cbVat
            // 
            this.cbVat.FormattingEnabled = true;
            this.cbVat.Items.AddRange(new object[] {
            "전체",
            "과세",
            "면세"});
            this.cbVat.Location = new System.Drawing.Point(1425, 3);
            this.cbVat.Name = "cbVat";
            this.cbVat.Size = new System.Drawing.Size(87, 20);
            this.cbVat.TabIndex = 17;
            this.cbVat.Text = "전체";
            // 
            // txtWarehouse
            // 
            this.txtWarehouse.Location = new System.Drawing.Point(1264, 2);
            this.txtWarehouse.Name = "txtWarehouse";
            this.txtWarehouse.Size = new System.Drawing.Size(100, 21);
            this.txtWarehouse.TabIndex = 15;
            this.txtWarehouse.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtManager_KeyDown);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1229, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 16;
            this.label9.Text = "창고";
            // 
            // txtCategoryCode
            // 
            this.txtCategoryCode.Location = new System.Drawing.Point(82, 2);
            this.txtCategoryCode.Name = "txtCategoryCode";
            this.txtCategoryCode.Size = new System.Drawing.Size(91, 21);
            this.txtCategoryCode.TabIndex = 0;
            this.txtCategoryCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCategoryCode_KeyDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "대분류 코드";
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(712, 2);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(69, 21);
            this.txtUnit.TabIndex = 5;
            this.txtUnit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUnit_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(676, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "단위";
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(1121, 2);
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(100, 21);
            this.txtManager.TabIndex = 8;
            this.txtManager.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtManager_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1073, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "담당자";
            // 
            // txtDivision
            // 
            this.txtDivision.Location = new System.Drawing.Point(979, 2);
            this.txtDivision.Name = "txtDivision";
            this.txtDivision.Size = new System.Drawing.Size(86, 21);
            this.txtDivision.TabIndex = 7;
            this.txtDivision.Text = "10 20 30";
            this.txtDivision.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDivision_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(943, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "구분";
            // 
            // txtSizes
            // 
            this.txtSizes.Location = new System.Drawing.Point(542, 2);
            this.txtSizes.Name = "txtSizes";
            this.txtSizes.Size = new System.Drawing.Size(124, 21);
            this.txtSizes.TabIndex = 4;
            this.txtSizes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSizes_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(506, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "규격";
            // 
            // txtOrigin
            // 
            this.txtOrigin.Location = new System.Drawing.Point(834, 2);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.Size = new System.Drawing.Size(100, 21);
            this.txtOrigin.TabIndex = 6;
            this.txtOrigin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtOrigin_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(787, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "원산지";
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(357, 2);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(142, 21);
            this.txtProduct.TabIndex = 3;
            this.txtProduct.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(323, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "품명";
            // 
            // txtCategory
            // 
            this.txtCategory.Location = new System.Drawing.Point(224, 2);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(91, 21);
            this.txtCategory.TabIndex = 1;
            this.txtCategory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCategory_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(179, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "대분류";
            // 
            // cbCategory
            // 
            this.cbCategory.AutoSize = true;
            this.cbCategory.Location = new System.Drawing.Point(10, 3);
            this.cbCategory.Name = "cbCategory";
            this.cbCategory.Size = new System.Drawing.Size(151, 16);
            this.cbCategory.TabIndex = 0;
            this.cbCategory.Text = "대분류 있는 품목만(F1)";
            this.cbCategory.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvProduct);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 48);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1515, 873);
            this.panel2.TabIndex = 2;
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToOrderColumns = true;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.Location = new System.Drawing.Point(0, 0);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowHeadersWidth = 25;
            this.dgvProduct.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProduct.Size = new System.Drawing.Size(1515, 873);
            this.dgvProduct.TabIndex = 0;
            this.dgvProduct.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProduct_CellMouseClick);
            this.dgvProduct.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProduct_CellMouseDoubleClick);
            this.dgvProduct.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvProduct_KeyDown);
            this.dgvProduct.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvProduct_MouseUp);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1517, 923);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.AliceBlue;
            this.panel4.Controls.Add(this.rbDecrease);
            this.panel4.Controls.Add(this.rbIncrease);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Controls.Add(this.rbNoFilter);
            this.panel4.Controls.Add(this.cbIsStock);
            this.panel4.Controls.Add(this.cbMinPrice);
            this.panel4.Controls.Add(this.cbPrice);
            this.panel4.Controls.Add(this.cbCategory);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1515, 21);
            this.panel4.TabIndex = 0;
            // 
            // rbDecrease
            // 
            this.rbDecrease.AutoSize = true;
            this.rbDecrease.ForeColor = System.Drawing.Color.Blue;
            this.rbDecrease.Location = new System.Drawing.Point(925, 3);
            this.rbDecrease.Name = "rbDecrease";
            this.rbDecrease.Size = new System.Drawing.Size(47, 16);
            this.rbDecrease.TabIndex = 8;
            this.rbDecrease.Text = "인하";
            this.rbDecrease.UseVisualStyleBackColor = true;
            // 
            // rbIncrease
            // 
            this.rbIncrease.AutoSize = true;
            this.rbIncrease.ForeColor = System.Drawing.Color.Red;
            this.rbIncrease.Location = new System.Drawing.Point(872, 3);
            this.rbIncrease.Name = "rbIncrease";
            this.rbIncrease.Size = new System.Drawing.Size(47, 16);
            this.rbIncrease.TabIndex = 7;
            this.rbIncrease.Text = "인상";
            this.rbIncrease.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(672, 5);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(127, 12);
            this.label11.TabIndex = 6;
            this.label11.Text = "매출단가 인상/하 필터";
            // 
            // rbNoFilter
            // 
            this.rbNoFilter.AutoSize = true;
            this.rbNoFilter.Checked = true;
            this.rbNoFilter.Location = new System.Drawing.Point(811, 3);
            this.rbNoFilter.Name = "rbNoFilter";
            this.rbNoFilter.Size = new System.Drawing.Size(55, 16);
            this.rbNoFilter.TabIndex = 5;
            this.rbNoFilter.TabStop = true;
            this.rbNoFilter.Text = "필터X";
            this.rbNoFilter.UseVisualStyleBackColor = true;
            // 
            // cbIsStock
            // 
            this.cbIsStock.AutoSize = true;
            this.cbIsStock.Location = new System.Drawing.Point(518, 3);
            this.cbIsStock.Name = "cbIsStock";
            this.cbIsStock.Size = new System.Drawing.Size(135, 16);
            this.cbIsStock.TabIndex = 4;
            this.cbIsStock.Text = "재고있는 품목만(F4)";
            this.cbIsStock.UseVisualStyleBackColor = true;
            // 
            // cbMinPrice
            // 
            this.cbMinPrice.AutoSize = true;
            this.cbMinPrice.Location = new System.Drawing.Point(325, 3);
            this.cbMinPrice.Name = "cbMinPrice";
            this.cbMinPrice.Size = new System.Drawing.Size(187, 16);
            this.cbMinPrice.TabIndex = 3;
            this.cbMinPrice.Text = "일반시세보다 싼 품목보기(F3)";
            this.cbMinPrice.UseVisualStyleBackColor = true;
            // 
            // cbPrice
            // 
            this.cbPrice.AutoSize = true;
            this.cbPrice.Location = new System.Drawing.Point(167, 3);
            this.cbPrice.Name = "cbPrice";
            this.cbPrice.Size = new System.Drawing.Size(145, 16);
            this.cbPrice.TabIndex = 1;
            this.cbPrice.Text = "매입단가 0원 초과(F2)";
            this.cbPrice.UseVisualStyleBackColor = true;
            // 
            // GetProductList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1517, 923);
            this.Controls.Add(this.panel3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GetProductList";
            this.Text = "품목추가";
            this.Load += new System.EventHandler(this.GetProductList_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GetProductList_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvProduct;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDivision;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSizes;
        private System.Windows.Forms.Label label4;
        private MySql.Data.MySqlClient.CustomInstaller rightMouseMenu;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cbCategory;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCategoryCode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox cbPrice;
        private System.Windows.Forms.CheckBox cbMinPrice;
        private System.Windows.Forms.TextBox txtWarehouse;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox cbIsStock;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbVat;
        private System.Windows.Forms.RadioButton rbDecrease;
        private System.Windows.Forms.RadioButton rbIncrease;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.RadioButton rbNoFilter;
    }
}