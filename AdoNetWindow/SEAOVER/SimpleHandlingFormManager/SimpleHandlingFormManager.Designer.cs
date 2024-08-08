namespace AdoNetWindow.SEAOVER.SimpleHandlingFormManager
{
    partial class SimpleHandlingFormManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleHandlingFormManager));
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cbStockMax = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label21 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lvBookmark = new System.Windows.Forms.ListView();
            this.form_id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.from_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.edit_user = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtManager1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFormname1 = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnDailyBusinessCopy = new System.Windows.Forms.Button();
            this.btnGetProduct = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSearching = new System.Windows.Forms.Button();
            this.chComplete = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.product_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.package_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seaover_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sales_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.real_product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.real_origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.real_sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isTax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rbInStock = new System.Windows.Forms.RadioButton();
            this.rbAllStock = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvProduct);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1164, 651);
            this.panel1.TabIndex = 0;
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToResizeColumns = false;
            this.dgvProduct.AllowUserToResizeRows = false;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chComplete,
            this.product_code,
            this.product,
            this.origin_code,
            this.origin,
            this.sizes_code,
            this.sizes,
            this.unit,
            this.unit_count,
            this.price_unit,
            this.package_count,
            this.seaover_unit,
            this.sales_price,
            this.warehouse,
            this.real_product,
            this.real_origin,
            this.real_sizes,
            this.isTax});
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.EnableHeadersVisualStyles = false;
            this.dgvProduct.Location = new System.Drawing.Point(0, 26);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.Size = new System.Drawing.Size(924, 625);
            this.dgvProduct.TabIndex = 0;
            this.dgvProduct.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellContentClick);
            this.dgvProduct.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProduct_ColumnHeaderMouseDoubleClick);
            this.dgvProduct.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvProduct_DataError);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label1);
            this.panel5.Controls.Add(this.rbAllStock);
            this.panel5.Controls.Add(this.rbInStock);
            this.panel5.Controls.Add(this.cbStockMax);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(924, 26);
            this.panel5.TabIndex = 2;
            // 
            // cbStockMax
            // 
            this.cbStockMax.AutoSize = true;
            this.cbStockMax.Checked = true;
            this.cbStockMax.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbStockMax.Location = new System.Drawing.Point(115, 6);
            this.cbStockMax.Name = "cbStockMax";
            this.cbStockMax.Size = new System.Drawing.Size(195, 16);
            this.cbStockMax.TabIndex = 0;
            this.cbStockMax.Text = "가장 많은 재고의 창고선택 (F1)";
            this.cbStockMax.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label21);
            this.panel4.Controls.Add(this.label13);
            this.panel4.Controls.Add(this.lvBookmark);
            this.panel4.Controls.Add(this.txtManager1);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.txtFormname1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(924, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(240, 651);
            this.panel4.TabIndex = 1;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(14, 66);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(41, 12);
            this.label21.TabIndex = 13;
            this.label21.Text = "담당자";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(26, 42);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 12;
            this.label13.Text = "제목";
            // 
            // lvBookmark
            // 
            this.lvBookmark.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.form_id,
            this.from_name,
            this.edit_user});
            this.lvBookmark.FullRowSelect = true;
            this.lvBookmark.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvBookmark.HideSelection = false;
            this.lvBookmark.Location = new System.Drawing.Point(11, 90);
            this.lvBookmark.Name = "lvBookmark";
            this.lvBookmark.Size = new System.Drawing.Size(223, 500);
            this.lvBookmark.TabIndex = 9;
            this.lvBookmark.UseCompatibleStateImageBehavior = false;
            this.lvBookmark.View = System.Windows.Forms.View.Details;
            this.lvBookmark.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvBookmark_MouseClick);
            // 
            // form_id
            // 
            this.form_id.Text = "";
            this.form_id.Width = 30;
            // 
            // from_name
            // 
            this.from_name.Text = "제목";
            this.from_name.Width = 200;
            // 
            // txtManager1
            // 
            this.txtManager1.Location = new System.Drawing.Point(64, 62);
            this.txtManager1.Name = "txtManager1";
            this.txtManager1.Size = new System.Drawing.Size(168, 21);
            this.txtManager1.TabIndex = 2;
            this.txtManager1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFormname1_KeyDown);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label3.Location = new System.Drawing.Point(57, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "즐겨찾기 리스트";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtFormname1
            // 
            this.txtFormname1.Location = new System.Drawing.Point(64, 36);
            this.txtFormname1.Name = "txtFormname1";
            this.txtFormname1.Size = new System.Drawing.Size(168, 21);
            this.txtFormname1.TabIndex = 1;
            this.txtFormname1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFormname1_KeyDown);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnDailyBusinessCopy);
            this.panel2.Controls.Add(this.btnGetProduct);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.btnExcel);
            this.panel2.Controls.Add(this.btnRefresh);
            this.panel2.Controls.Add(this.btnSearching);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 651);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1164, 45);
            this.panel2.TabIndex = 1;
            // 
            // btnDailyBusinessCopy
            // 
            this.btnDailyBusinessCopy.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDailyBusinessCopy.Location = new System.Drawing.Point(432, 4);
            this.btnDailyBusinessCopy.Name = "btnDailyBusinessCopy";
            this.btnDailyBusinessCopy.Size = new System.Drawing.Size(76, 36);
            this.btnDailyBusinessCopy.TabIndex = 6;
            this.btnDailyBusinessCopy.Text = "영업일지 규격복사";
            this.btnDailyBusinessCopy.UseVisualStyleBackColor = true;
            this.btnDailyBusinessCopy.Click += new System.EventHandler(this.btnDailyBusinessCopy_Click);
            // 
            // btnGetProduct
            // 
            this.btnGetProduct.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnGetProduct.Location = new System.Drawing.Point(325, 4);
            this.btnGetProduct.Name = "btnGetProduct";
            this.btnGetProduct.Size = new System.Drawing.Size(101, 36);
            this.btnGetProduct.TabIndex = 5;
            this.btnGetProduct.Text = "품목선택(F4)";
            this.btnGetProduct.UseVisualStyleBackColor = true;
            this.btnGetProduct.Click += new System.EventHandler(this.btnGetProduct_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(1068, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(96, 45);
            this.panel3.TabIndex = 4;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(30, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(63, 36);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExcel.Location = new System.Drawing.Point(230, 4);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(89, 36);
            this.btnExcel.TabIndex = 2;
            this.btnExcel.Text = "엑셀다운(E)";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRefresh.Location = new System.Drawing.Point(135, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(89, 36);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "초기화(F5)";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.Location = new System.Drawing.Point(8, 4);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(121, 36);
            this.btnSearching.TabIndex = 0;
            this.btnSearching.Text = "단가/창고검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // chComplete
            // 
            this.chComplete.HeaderText = "완료";
            this.chComplete.Name = "chComplete";
            this.chComplete.Width = 35;
            // 
            // product_code
            // 
            this.product_code.HeaderText = "품목코드";
            this.product_code.Name = "product_code";
            this.product_code.Visible = false;
            // 
            // product
            // 
            this.product.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.product.FillWeight = 57.13521F;
            this.product.HeaderText = "품명";
            this.product.Name = "product";
            // 
            // origin_code
            // 
            this.origin_code.HeaderText = "원산지규격";
            this.origin_code.Name = "origin_code";
            this.origin_code.Visible = false;
            // 
            // origin
            // 
            this.origin.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.origin.FillWeight = 29.62566F;
            this.origin.HeaderText = "원산지";
            this.origin.Name = "origin";
            // 
            // sizes_code
            // 
            this.sizes_code.HeaderText = "규격코드";
            this.sizes_code.Name = "sizes_code";
            this.sizes_code.Visible = false;
            // 
            // sizes
            // 
            this.sizes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sizes.FillWeight = 29.62566F;
            this.sizes.HeaderText = "규격";
            this.sizes.Name = "sizes";
            // 
            // unit
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.unit.DefaultCellStyle = dataGridViewCellStyle1;
            this.unit.FillWeight = 29.62566F;
            this.unit.HeaderText = "단위";
            this.unit.Name = "unit";
            this.unit.Width = 60;
            // 
            // unit_count
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.unit_count.DefaultCellStyle = dataGridViewCellStyle2;
            this.unit_count.HeaderText = "단위수량";
            this.unit_count.Name = "unit_count";
            this.unit_count.Width = 60;
            // 
            // price_unit
            // 
            this.price_unit.HeaderText = "가격단위";
            this.price_unit.Name = "price_unit";
            this.price_unit.Width = 60;
            // 
            // package_count
            // 
            this.package_count.HeaderText = "묶음수";
            this.package_count.Name = "package_count";
            this.package_count.Visible = false;
            // 
            // seaover_unit
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.seaover_unit.DefaultCellStyle = dataGridViewCellStyle3;
            this.seaover_unit.HeaderText = "S단위";
            this.seaover_unit.Name = "seaover_unit";
            this.seaover_unit.Width = 60;
            // 
            // sales_price
            // 
            this.sales_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.sales_price.DefaultCellStyle = dataGridViewCellStyle4;
            this.sales_price.FillWeight = 29.62566F;
            this.sales_price.HeaderText = "매출단가";
            this.sales_price.Name = "sales_price";
            // 
            // warehouse
            // 
            this.warehouse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.warehouse.FillWeight = 39.9177F;
            this.warehouse.HeaderText = "창고";
            this.warehouse.Name = "warehouse";
            // 
            // real_product
            // 
            this.real_product.HeaderText = "품명";
            this.real_product.Name = "real_product";
            this.real_product.Visible = false;
            // 
            // real_origin
            // 
            this.real_origin.HeaderText = "원산지";
            this.real_origin.Name = "real_origin";
            this.real_origin.Visible = false;
            // 
            // real_sizes
            // 
            this.real_sizes.HeaderText = "규격";
            this.real_sizes.Name = "real_sizes";
            this.real_sizes.Visible = false;
            // 
            // isTax
            // 
            this.isTax.HeaderText = "과세";
            this.isTax.Name = "isTax";
            this.isTax.Width = 50;
            // 
            // rbInStock
            // 
            this.rbInStock.AutoSize = true;
            this.rbInStock.Checked = true;
            this.rbInStock.Location = new System.Drawing.Point(608, 5);
            this.rbInStock.Name = "rbInStock";
            this.rbInStock.Size = new System.Drawing.Size(179, 16);
            this.rbInStock.TabIndex = 1;
            this.rbInStock.TabStop = true;
            this.rbInStock.Text = "재고있는 품목만 단가 최신화";
            this.rbInStock.UseVisualStyleBackColor = true;
            // 
            // rbAllStock
            // 
            this.rbAllStock.AutoSize = true;
            this.rbAllStock.Location = new System.Drawing.Point(793, 6);
            this.rbAllStock.Name = "rbAllStock";
            this.rbAllStock.Size = new System.Drawing.Size(127, 16);
            this.rbAllStock.TabIndex = 2;
            this.rbAllStock.Text = "전품목 단가 최신화";
            this.rbAllStock.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "단가최신화";
            // 
            // SimpleHandlingFormManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 696);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "SimpleHandlingFormManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "품목단가표";
            this.Load += new System.EventHandler(this.SimpleHandlingFormManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SimpleHandlingFormManager_KeyDown);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.SimpleHandlingFormManager_PreviewKeyDown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProduct;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnGetProduct;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ListView lvBookmark;
        private System.Windows.Forms.ColumnHeader form_id;
        private System.Windows.Forms.ColumnHeader from_name;
        private System.Windows.Forms.ColumnHeader edit_user;
        private System.Windows.Forms.TextBox txtManager1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFormname1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.CheckBox cbStockMax;
        private System.Windows.Forms.Button btnDailyBusinessCopy;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chComplete;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn price_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn package_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn sales_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehouse;
        private System.Windows.Forms.DataGridViewTextBoxColumn real_product;
        private System.Windows.Forms.DataGridViewTextBoxColumn real_origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn real_sizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn isTax;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbAllStock;
        private System.Windows.Forms.RadioButton rbInStock;
    }
}