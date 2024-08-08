namespace AdoNetWindow.PurchaseManager
{
    partial class ProductAssortBatchInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductAssortBatchInput));
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.box_weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnInput = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label17 = new System.Windows.Forms.Label();
            this.cbSortType = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvProduct);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(704, 489);
            this.panel1.TabIndex = 0;
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToResizeRows = false;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.product,
            this.origin,
            this.sizes,
            this.sizes2,
            this.box_weight,
            this.cost_unit,
            this.assort});
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.EnableHeadersVisualStyles = false;
            this.dgvProduct.Location = new System.Drawing.Point(0, 0);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.Size = new System.Drawing.Size(704, 489);
            this.dgvProduct.TabIndex = 0;
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
            // sizes2
            // 
            this.sizes2.HeaderText = "규격2";
            this.sizes2.Name = "sizes2";
            // 
            // box_weight
            // 
            this.box_weight.HeaderText = "중량";
            this.box_weight.Name = "box_weight";
            this.box_weight.Width = 60;
            // 
            // cost_unit
            // 
            this.cost_unit.HeaderText = "트레이";
            this.cost_unit.Name = "cost_unit";
            this.cost_unit.Width = 60;
            // 
            // assort
            // 
            this.assort.HeaderText = "수량";
            this.assort.Name = "assort";
            this.assort.Width = 60;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnInput);
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 518);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(704, 42);
            this.panel2.TabIndex = 1;
            // 
            // btnInput
            // 
            this.btnInput.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInput.ForeColor = System.Drawing.Color.Black;
            this.btnInput.Location = new System.Drawing.Point(3, 3);
            this.btnInput.Name = "btnInput";
            this.btnInput.Size = new System.Drawing.Size(70, 37);
            this.btnInput.TabIndex = 6;
            this.btnInput.Text = "입력(A)";
            this.btnInput.UseVisualStyleBackColor = true;
            this.btnInput.Click += new System.EventHandler(this.btnInput_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(79, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 37);
            this.btnExit.TabIndex = 10;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel8);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(704, 29);
            this.panel3.TabIndex = 2;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label17);
            this.panel8.Controls.Add(this.cbSortType);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel8.Location = new System.Drawing.Point(309, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(395, 29);
            this.panel8.TabIndex = 112;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label17.Location = new System.Drawing.Point(161, 9);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(57, 12);
            this.label17.TabIndex = 52;
            this.label17.Text = "정렬기준";
            // 
            // cbSortType
            // 
            this.cbSortType.FormattingEnabled = true;
            this.cbSortType.Items.AddRange(new object[] {
            "품명+원산지+규격",
            "거래처+품명+원산지+규격",
            "품명+원산지+규격+오퍼가",
            "오퍼가+품명+원산지+규격",
            "품명+원산지+규격+오퍼일자",
            "오퍼일자+품명+원산지+규격"});
            this.cbSortType.Location = new System.Drawing.Point(224, 5);
            this.cbSortType.Name = "cbSortType";
            this.cbSortType.Size = new System.Drawing.Size(167, 20);
            this.cbSortType.TabIndex = 51;
            this.cbSortType.Text = "품명+원산지+규격";
            // 
            // ProductAssortBatchInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 560);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProductAssortBatchInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "수량 일괄입력";
            this.Load += new System.EventHandler(this.ProductAssortBatchInput_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProductAssortBatchInput_KeyDown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProduct;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox cbSortType;
        private System.Windows.Forms.Button btnInput;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes2;
        private System.Windows.Forms.DataGridViewTextBoxColumn box_weight;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn assort;
    }
}