namespace AdoNetWindow.Pending
{
    partial class PendingView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PendingView));
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dgvProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.txtTotalBoxWeight = new System.Windows.Forms.TextBox();
            this.txtTotalContractQty = new System.Windows.Forms.TextBox();
            this.txtTotalWarehouseQty = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbManager = new System.Windows.Forms.Label();
            this.shipper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.box_weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contract_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouse_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.etd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehousing_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pending_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvProduct);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1155, 468);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.lbManager);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.txtTotalWarehouseQty);
            this.panel3.Controls.Add(this.txtTotalContractQty);
            this.panel3.Controls.Add(this.txtTotalBoxWeight);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 447);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1155, 21);
            this.panel3.TabIndex = 2;
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToOrderColumns = true;
            this.dgvProduct.AllowUserToResizeColumns = false;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.shipper,
            this.product,
            this.origin,
            this.sizes,
            this.box_weight,
            this.contract_qty,
            this.warehouse_qty,
            this.etd,
            this.eta,
            this.warehousing_date,
            this.pending_date});
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.EnableHeadersVisualStyles = false;
            this.dgvProduct.Location = new System.Drawing.Point(0, 0);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.Size = new System.Drawing.Size(1155, 468);
            this.dgvProduct.TabIndex = 3;
            // 
            // txtTotalBoxWeight
            // 
            this.txtTotalBoxWeight.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTotalBoxWeight.Location = new System.Drawing.Point(593, -1);
            this.txtTotalBoxWeight.Name = "txtTotalBoxWeight";
            this.txtTotalBoxWeight.Size = new System.Drawing.Size(77, 21);
            this.txtTotalBoxWeight.TabIndex = 0;
            this.txtTotalBoxWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtTotalContractQty
            // 
            this.txtTotalContractQty.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTotalContractQty.Location = new System.Drawing.Point(670, -1);
            this.txtTotalContractQty.Name = "txtTotalContractQty";
            this.txtTotalContractQty.Size = new System.Drawing.Size(77, 21);
            this.txtTotalContractQty.TabIndex = 1;
            this.txtTotalContractQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtTotalWarehouseQty
            // 
            this.txtTotalWarehouseQty.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTotalWarehouseQty.Location = new System.Drawing.Point(747, -1);
            this.txtTotalWarehouseQty.Name = "txtTotalWarehouseQty";
            this.txtTotalWarehouseQty.Size = new System.Drawing.Size(77, 21);
            this.txtTotalWarehouseQty.TabIndex = 2;
            this.txtTotalWarehouseQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(6, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "* SUM";
            // 
            // lbManager
            // 
            this.lbManager.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbManager.Location = new System.Drawing.Point(989, -1);
            this.lbManager.Name = "lbManager";
            this.lbManager.Size = new System.Drawing.Size(162, 23);
            this.lbManager.TabIndex = 4;
            this.lbManager.Text = "담당자 : ";
            this.lbManager.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // shipper
            // 
            this.shipper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.shipper.HeaderText = "거래처";
            this.shipper.Name = "shipper";
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
            this.sizes.Width = 80;
            // 
            // box_weight
            // 
            this.box_weight.HeaderText = "박스중량";
            this.box_weight.Name = "box_weight";
            this.box_weight.Width = 80;
            // 
            // contract_qty
            // 
            this.contract_qty.HeaderText = "계약수량";
            this.contract_qty.Name = "contract_qty";
            this.contract_qty.Width = 80;
            // 
            // warehouse_qty
            // 
            this.warehouse_qty.HeaderText = "입고수량";
            this.warehouse_qty.Name = "warehouse_qty";
            this.warehouse_qty.Width = 80;
            // 
            // etd
            // 
            this.etd.HeaderText = "ETD";
            this.etd.Name = "etd";
            this.etd.Width = 80;
            // 
            // eta
            // 
            this.eta.HeaderText = "ETA";
            this.eta.Name = "eta";
            this.eta.Width = 80;
            // 
            // warehousing_date
            // 
            this.warehousing_date.HeaderText = "입고예정";
            this.warehousing_date.Name = "warehousing_date";
            this.warehousing_date.Width = 80;
            // 
            // pending_date
            // 
            this.pending_date.HeaderText = "통관예정";
            this.pending_date.Name = "pending_date";
            this.pending_date.Width = 80;
            // 
            // PendingView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 468);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PendingView";
            this.Text = "계약정보";
            this.Load += new System.EventHandler(this.PendingView_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PendingView_KeyDown);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProduct;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTotalWarehouseQty;
        private System.Windows.Forms.TextBox txtTotalContractQty;
        private System.Windows.Forms.TextBox txtTotalBoxWeight;
        private System.Windows.Forms.Label lbManager;
        private System.Windows.Forms.DataGridViewTextBoxColumn shipper;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn box_weight;
        private System.Windows.Forms.DataGridViewTextBoxColumn contract_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehouse_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn etd;
        private System.Windows.Forms.DataGridViewTextBoxColumn eta;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehousing_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn pending_date;
    }
}