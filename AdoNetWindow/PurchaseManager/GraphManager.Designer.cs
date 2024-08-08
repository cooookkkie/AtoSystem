namespace AdoNetWindow.PurchaseManager
{
    partial class GraphManager
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphManager));
            this.panel8 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cbGraphType = new System.Windows.Forms.ComboBox();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.dgvPurchasePrice = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.updatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchase_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnGetProduct = new System.Windows.Forms.Button();
            this.panel9 = new System.Windows.Forms.Panel();
            this.dgvProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel11 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCalendarEnddate = new System.Windows.Forms.Button();
            this.btnCalendarSttdate = new System.Windows.Forms.Button();
            this.txtEnddate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSttdate = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel8.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchasePrice)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.panel11.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label8);
            this.panel8.Controls.Add(this.label9);
            this.panel8.Controls.Add(this.cbGraphType);
            this.panel8.Controls.Add(this.cbType);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel8.Location = new System.Drawing.Point(405, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(320, 29);
            this.panel8.TabIndex = 110;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(71, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 105;
            this.label8.Text = "집계방식";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(197, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 107;
            this.label9.Text = "타입";
            // 
            // cbGraphType
            // 
            this.cbGraphType.FormattingEnabled = true;
            this.cbGraphType.Items.AddRange(new object[] {
            "일별",
            "월별"});
            this.cbGraphType.Location = new System.Drawing.Point(133, 5);
            this.cbGraphType.Name = "cbGraphType";
            this.cbGraphType.Size = new System.Drawing.Size(55, 20);
            this.cbGraphType.TabIndex = 104;
            this.cbGraphType.Text = "일별";
            // 
            // cbType
            // 
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "점선",
            "세로막대",
            "가로막대"});
            this.cbType.Location = new System.Drawing.Point(232, 5);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(82, 20);
            this.cbType.TabIndex = 106;
            this.cbType.Text = "점선";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(3, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 12);
            this.label6.TabIndex = 109;
            this.label6.Text = "* 비교 그래프";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(725, 29);
            this.panel1.TabIndex = 111;
            this.panel1.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.zedGraphControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 29);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(725, 646);
            this.panel2.TabIndex = 112;
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControl1.Location = new System.Drawing.Point(0, 0);
            this.zedGraphControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(725, 646);
            this.zedGraphControl1.TabIndex = 5;
            this.zedGraphControl1.UseExtendedPrintDialog = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel12);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.panel9);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(623, 677);
            this.panel4.TabIndex = 114;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.dgvPurchasePrice);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel12.Location = new System.Drawing.Point(0, 308);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(623, 325);
            this.panel12.TabIndex = 3;
            // 
            // dgvPurchasePrice
            // 
            this.dgvPurchasePrice.AllowUserToAddRows = false;
            this.dgvPurchasePrice.AllowUserToDeleteRows = false;
            this.dgvPurchasePrice.AllowUserToResizeRows = false;
            this.dgvPurchasePrice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPurchasePrice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.updatetime,
            this.purchase_price,
            this.company});
            this.dgvPurchasePrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPurchasePrice.EnableHeadersVisualStyles = false;
            this.dgvPurchasePrice.Location = new System.Drawing.Point(0, 0);
            this.dgvPurchasePrice.Name = "dgvPurchasePrice";
            this.dgvPurchasePrice.RowHeadersWidth = 30;
            this.dgvPurchasePrice.RowTemplate.Height = 23;
            this.dgvPurchasePrice.Size = new System.Drawing.Size(623, 325);
            this.dgvPurchasePrice.TabIndex = 1;
            this.dgvPurchasePrice.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvPurchasePrice_RowPostPaint);
            // 
            // updatetime
            // 
            this.updatetime.HeaderText = "오퍼일자";
            this.updatetime.Name = "updatetime";
            // 
            // purchase_price
            // 
            this.purchase_price.HeaderText = "오퍼가";
            this.purchase_price.Name = "purchase_price";
            // 
            // company
            // 
            this.company.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.company.HeaderText = "거래처";
            this.company.Name = "company";
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.btnExit);
            this.panel5.Controls.Add(this.btnSearch);
            this.panel5.Controls.Add(this.btnRefresh);
            this.panel5.Controls.Add(this.btnGetProduct);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 633);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(623, 44);
            this.panel5.TabIndex = 2;
            // 
            // btnExit
            // 
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(555, 5);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(62, 32);
            this.btnExit.TabIndex = 12;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Location = new System.Drawing.Point(252, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(77, 32);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "최신화(Q)";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRefresh.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRefresh.ForeColor = System.Drawing.Color.Red;
            this.btnRefresh.Location = new System.Drawing.Point(5, 5);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(111, 32);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "품목 초기화(F5)";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnGetProduct
            // 
            this.btnGetProduct.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGetProduct.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnGetProduct.ForeColor = System.Drawing.Color.Blue;
            this.btnGetProduct.Location = new System.Drawing.Point(122, 5);
            this.btnGetProduct.Name = "btnGetProduct";
            this.btnGetProduct.Size = new System.Drawing.Size(124, 32);
            this.btnGetProduct.TabIndex = 4;
            this.btnGetProduct.Text = "품목 불러오기(F4)";
            this.btnGetProduct.UseVisualStyleBackColor = true;
            this.btnGetProduct.Click += new System.EventHandler(this.btnGetProduct_Click);
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.dgvProduct);
            this.panel9.Controls.Add(this.panel11);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(623, 308);
            this.panel9.TabIndex = 0;
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToResizeRows = false;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.product,
            this.origin,
            this.sizes,
            this.unit,
            this.cost_unit,
            this.price,
            this.chk});
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.EnableHeadersVisualStyles = false;
            this.dgvProduct.Location = new System.Drawing.Point(0, 30);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowHeadersVisible = false;
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProduct.Size = new System.Drawing.Size(623, 278);
            this.dgvProduct.TabIndex = 0;
            this.dgvProduct.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellClick);
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
            this.unit.Width = 70;
            // 
            // cost_unit
            // 
            this.cost_unit.HeaderText = "트레이";
            this.cost_unit.Name = "cost_unit";
            this.cost_unit.Width = 70;
            // 
            // price
            // 
            this.price.HeaderText = "단가";
            this.price.Name = "price";
            // 
            // chk
            // 
            this.chk.HeaderText = "";
            this.chk.Name = "chk";
            this.chk.Width = 30;
            // 
            // panel11
            // 
            this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel11.Controls.Add(this.label3);
            this.panel11.Controls.Add(this.btnCalendarEnddate);
            this.panel11.Controls.Add(this.btnCalendarSttdate);
            this.panel11.Controls.Add(this.txtEnddate);
            this.panel11.Controls.Add(this.label2);
            this.panel11.Controls.Add(this.txtSttdate);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel11.Location = new System.Drawing.Point(0, 0);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(623, 30);
            this.panel11.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(4, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "*검색기간";
            // 
            // btnCalendarEnddate
            // 
            this.btnCalendarEnddate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCalendarEnddate.BackgroundImage")));
            this.btnCalendarEnddate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCalendarEnddate.FlatAppearance.BorderSize = 0;
            this.btnCalendarEnddate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalendarEnddate.Location = new System.Drawing.Point(290, 4);
            this.btnCalendarEnddate.Name = "btnCalendarEnddate";
            this.btnCalendarEnddate.Size = new System.Drawing.Size(20, 19);
            this.btnCalendarEnddate.TabIndex = 4;
            this.btnCalendarEnddate.UseVisualStyleBackColor = true;
            this.btnCalendarEnddate.Click += new System.EventHandler(this.btnCalendarEnddate_Click);
            // 
            // btnCalendarSttdate
            // 
            this.btnCalendarSttdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCalendarSttdate.BackgroundImage")));
            this.btnCalendarSttdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCalendarSttdate.FlatAppearance.BorderSize = 0;
            this.btnCalendarSttdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalendarSttdate.Location = new System.Drawing.Point(164, 4);
            this.btnCalendarSttdate.Name = "btnCalendarSttdate";
            this.btnCalendarSttdate.Size = new System.Drawing.Size(20, 19);
            this.btnCalendarSttdate.TabIndex = 2;
            this.btnCalendarSttdate.UseVisualStyleBackColor = true;
            this.btnCalendarSttdate.Click += new System.EventHandler(this.btnCalendarSttdate_Click);
            // 
            // txtEnddate
            // 
            this.txtEnddate.Location = new System.Drawing.Point(200, 3);
            this.txtEnddate.Name = "txtEnddate";
            this.txtEnddate.Size = new System.Drawing.Size(89, 21);
            this.txtEnddate.TabIndex = 3;
            this.txtEnddate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSttdate_KeyDown);
            this.txtEnddate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSttdate_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "~";
            // 
            // txtSttdate
            // 
            this.txtSttdate.Location = new System.Drawing.Point(74, 3);
            this.txtSttdate.Name = "txtSttdate";
            this.txtSttdate.Size = new System.Drawing.Size(89, 21);
            this.txtSttdate.TabIndex = 1;
            this.txtSttdate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSttdate_KeyDown);
            this.txtSttdate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSttdate_KeyPress);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(623, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(727, 677);
            this.panel3.TabIndex = 113;
            // 
            // GraphManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 677);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "GraphManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "그래프";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GraphManager_KeyDown);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchasePrice)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbGraphType;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel9;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProduct;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel11;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvPurchasePrice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCalendarEnddate;
        private System.Windows.Forms.Button btnCalendarSttdate;
        private System.Windows.Forms.TextBox txtEnddate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSttdate;
        private System.Windows.Forms.Button btnGetProduct;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn updatetime;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchase_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn company;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn price;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private ZedGraph.ZedGraphControl zedGraphControl1;
    }
}