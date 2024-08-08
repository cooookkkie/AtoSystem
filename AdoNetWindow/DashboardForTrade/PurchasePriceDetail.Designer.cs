namespace AdoNetWindow.Dashboard
{
    partial class PurchasePriceDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PurchasePriceDetail));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtEnddate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSttdate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvPurchase = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.purchase_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchase_company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchase_margin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchase_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtAveragePurchasePrice = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchase)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtAveragePurchasePrice);
            this.panel1.Controls.Add(this.txtEnddate);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtSttdate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(833, 31);
            this.panel1.TabIndex = 0;
            // 
            // txtEnddate
            // 
            this.txtEnddate.Location = new System.Drawing.Point(181, 5);
            this.txtEnddate.Name = "txtEnddate";
            this.txtEnddate.Size = new System.Drawing.Size(88, 21);
            this.txtEnddate.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(161, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "~";
            // 
            // txtSttdate
            // 
            this.txtSttdate.Location = new System.Drawing.Point(67, 5);
            this.txtSttdate.Name = "txtSttdate";
            this.txtSttdate.Size = new System.Drawing.Size(88, 21);
            this.txtSttdate.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "매입기간";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvPurchase);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 31);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(833, 186);
            this.panel2.TabIndex = 1;
            // 
            // dgvPurchase
            // 
            this.dgvPurchase.AllowUserToAddRows = false;
            this.dgvPurchase.AllowUserToDeleteRows = false;
            this.dgvPurchase.AllowUserToResizeRows = false;
            this.dgvPurchase.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPurchase.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.purchase_date,
            this.division,
            this.purchase_company,
            this.warehouse,
            this.remark,
            this.qty,
            this.unit_price,
            this.purchase_margin,
            this.purchase_price,
            this.total_amount});
            this.dgvPurchase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPurchase.EnableHeadersVisualStyles = false;
            this.dgvPurchase.Location = new System.Drawing.Point(0, 0);
            this.dgvPurchase.Name = "dgvPurchase";
            this.dgvPurchase.RowHeadersWidth = 25;
            this.dgvPurchase.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvPurchase.RowTemplate.Height = 23;
            this.dgvPurchase.Size = new System.Drawing.Size(833, 186);
            this.dgvPurchase.TabIndex = 0;
            // 
            // purchase_date
            // 
            this.purchase_date.HeaderText = "매입일자";
            this.purchase_date.Name = "purchase_date";
            this.purchase_date.Width = 80;
            // 
            // division
            // 
            this.division.HeaderText = "구분";
            this.division.Name = "division";
            this.division.Width = 50;
            // 
            // purchase_company
            // 
            this.purchase_company.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.purchase_company.HeaderText = "매입처";
            this.purchase_company.Name = "purchase_company";
            // 
            // warehouse
            // 
            this.warehouse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.warehouse.HeaderText = "보관처";
            this.warehouse.Name = "warehouse";
            // 
            // remark
            // 
            this.remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.remark.HeaderText = "비고";
            this.remark.Name = "remark";
            // 
            // qty
            // 
            this.qty.HeaderText = "수량";
            this.qty.Name = "qty";
            this.qty.Width = 50;
            // 
            // unit_price
            // 
            this.unit_price.HeaderText = "구매단가";
            this.unit_price.Name = "unit_price";
            this.unit_price.Width = 70;
            // 
            // purchase_margin
            // 
            this.purchase_margin.HeaderText = "매입마진";
            this.purchase_margin.Name = "purchase_margin";
            this.purchase_margin.Width = 50;
            // 
            // purchase_price
            // 
            this.purchase_price.HeaderText = "반영단가";
            this.purchase_price.Name = "purchase_price";
            this.purchase_price.Width = 70;
            // 
            // total_amount
            // 
            this.total_amount.HeaderText = "총금액";
            this.total_amount.Name = "total_amount";
            this.total_amount.Visible = false;
            this.total_amount.Width = 80;
            // 
            // txtAveragePurchasePrice
            // 
            this.txtAveragePurchasePrice.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtAveragePurchasePrice.Location = new System.Drawing.Point(679, 1);
            this.txtAveragePurchasePrice.Name = "txtAveragePurchasePrice";
            this.txtAveragePurchasePrice.Size = new System.Drawing.Size(153, 29);
            this.txtAveragePurchasePrice.TabIndex = 4;
            this.txtAveragePurchasePrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(578, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "평균 매입단가";
            // 
            // PurchasePriceDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 217);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PurchasePriceDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "매입내역";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PurchasePriceDetail_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchase)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtEnddate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSttdate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvPurchase;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchase_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn division;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchase_company;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehouse;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchase_margin;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchase_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_amount;
        private System.Windows.Forms.TextBox txtAveragePurchasePrice;
        private System.Windows.Forms.Label label3;
    }
}