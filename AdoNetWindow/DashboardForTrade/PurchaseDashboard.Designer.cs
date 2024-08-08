namespace AdoNetWindow.DashboardForTrade
{
    partial class PurchaseDashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PurchaseDashboard));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbComparisonTerm = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.txtSttdate = new System.Windows.Forms.TextBox();
            this.btnCalendarSttdate = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvPurchase = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnPrinting = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnGetProduct = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchase)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbComparisonTerm);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label37);
            this.panel1.Controls.Add(this.txtSttdate);
            this.panel1.Controls.Add(this.btnCalendarSttdate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1560, 26);
            this.panel1.TabIndex = 0;
            // 
            // cbComparisonTerm
            // 
            this.cbComparisonTerm.FormattingEnabled = true;
            this.cbComparisonTerm.Items.AddRange(new object[] {
            "주(7일) 비교",
            "보름(15일) 비교",
            "월(약 30일) 비교"});
            this.cbComparisonTerm.Location = new System.Drawing.Point(228, 3);
            this.cbComparisonTerm.Name = "cbComparisonTerm";
            this.cbComparisonTerm.Size = new System.Drawing.Size(135, 20);
            this.cbComparisonTerm.TabIndex = 133;
            this.cbComparisonTerm.Text = "주(7일) 비교";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(169, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 132;
            this.label1.Text = "비교구분";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label37.Location = new System.Drawing.Point(3, 7);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(53, 12);
            this.label37.TabIndex = 131;
            this.label37.Text = "기준일자";
            // 
            // txtSttdate
            // 
            this.txtSttdate.Location = new System.Drawing.Point(58, 3);
            this.txtSttdate.Name = "txtSttdate";
            this.txtSttdate.Size = new System.Drawing.Size(83, 21);
            this.txtSttdate.TabIndex = 126;
            this.txtSttdate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSttdate_KeyDown);
            // 
            // btnCalendarSttdate
            // 
            this.btnCalendarSttdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCalendarSttdate.BackgroundImage")));
            this.btnCalendarSttdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCalendarSttdate.FlatAppearance.BorderSize = 0;
            this.btnCalendarSttdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalendarSttdate.Location = new System.Drawing.Point(143, 3);
            this.btnCalendarSttdate.Name = "btnCalendarSttdate";
            this.btnCalendarSttdate.Size = new System.Drawing.Size(20, 19);
            this.btnCalendarSttdate.TabIndex = 127;
            this.btnCalendarSttdate.UseVisualStyleBackColor = true;
            this.btnCalendarSttdate.Click += new System.EventHandler(this.btnCalendarSttdate_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvPurchase);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 26);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1560, 775);
            this.panel2.TabIndex = 1;
            // 
            // dgvPurchase
            // 
            this.dgvPurchase.AllowUserToAddRows = false;
            this.dgvPurchase.AllowUserToDeleteRows = false;
            this.dgvPurchase.AllowUserToResizeColumns = false;
            this.dgvPurchase.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPurchase.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPurchase.ColumnHeadersHeight = 60;
            this.dgvPurchase.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPurchase.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.product,
            this.origin,
            this.sizes,
            this.unit});
            this.dgvPurchase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPurchase.EnableHeadersVisualStyles = false;
            this.dgvPurchase.Location = new System.Drawing.Point(0, 0);
            this.dgvPurchase.Name = "dgvPurchase";
            this.dgvPurchase.RowTemplate.Height = 23;
            this.dgvPurchase.Size = new System.Drawing.Size(1560, 775);
            this.dgvPurchase.TabIndex = 0;
            this.dgvPurchase.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvPurchase_CellPainting);
            this.dgvPurchase.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvPurchase_ColumnHeaderMouseDoubleClick);
            this.dgvPurchase.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvPurchase_ColumnWidthChanged);
            this.dgvPurchase.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvPurchase_Scroll);
            this.dgvPurchase.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvPurchase_Paint);
            // 
            // product
            // 
            this.product.HeaderText = "품명";
            this.product.Name = "product";
            this.product.Width = 150;
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
            this.unit.Width = 60;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.btnRefresh);
            this.panel3.Controls.Add(this.btnGetProduct);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 801);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1560, 38);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnPrinting);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(1359, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(201, 38);
            this.panel4.TabIndex = 15;
            // 
            // btnPrinting
            // 
            this.btnPrinting.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrinting.Location = new System.Drawing.Point(98, 2);
            this.btnPrinting.Name = "btnPrinting";
            this.btnPrinting.Size = new System.Drawing.Size(100, 35);
            this.btnPrinting.TabIndex = 13;
            this.btnPrinting.Text = "인쇄(Ctrl+P)";
            this.btnPrinting.UseVisualStyleBackColor = true;
            this.btnPrinting.Click += new System.EventHandler(this.btnPrinting_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRefresh.Location = new System.Drawing.Point(212, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(95, 35);
            this.btnRefresh.TabIndex = 10;
            this.btnRefresh.Text = "새로고침(F5)";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnGetProduct
            // 
            this.btnGetProduct.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnGetProduct.Location = new System.Drawing.Point(76, 2);
            this.btnGetProduct.Name = "btnGetProduct";
            this.btnGetProduct.Size = new System.Drawing.Size(130, 35);
            this.btnGetProduct.TabIndex = 2;
            this.btnGetProduct.Text = "품목 불러오기(F4)";
            this.btnGetProduct.UseVisualStyleBackColor = true;
            this.btnGetProduct.Click += new System.EventHandler(this.btnGetProduct_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(313, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(65, 35);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(5, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(65, 35);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "검색(Q)";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // PurchaseDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1560, 839);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "PurchaseDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "매입단가 대시보드";
            this.Load += new System.EventHandler(this.PurchaseDashboard_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PurchaseDashboard_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchase)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvPurchase;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox txtSttdate;
        private System.Windows.Forms.Button btnCalendarSttdate;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.Button btnGetProduct;
        private System.Windows.Forms.ComboBox cbComparisonTerm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnPrinting;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
    }
}