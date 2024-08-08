namespace AdoNetWindow.SEAOVER.PriceComparison
{
    partial class MergeProduct
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MergeProduct));
            this.dgvMergeProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.pnMergeProduct = new System.Windows.Forms.Panel();
            this.panel13 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.label25 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMergeProduct)).BeginInit();
            this.pnMergeProduct.SuspendLayout();
            this.panel13.SuspendLayout();
            this.panel12.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvMergeProduct
            // 
            this.dgvMergeProduct.AllowUserToAddRows = false;
            this.dgvMergeProduct.AllowUserToDeleteRows = false;
            this.dgvMergeProduct.AllowUserToOrderColumns = true;
            this.dgvMergeProduct.AllowUserToResizeColumns = false;
            this.dgvMergeProduct.AllowUserToResizeRows = false;
            this.dgvMergeProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvMergeProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMergeProduct.EnableHeadersVisualStyles = false;
            this.dgvMergeProduct.Location = new System.Drawing.Point(0, 0);
            this.dgvMergeProduct.Name = "dgvMergeProduct";
            this.dgvMergeProduct.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvMergeProduct.RowTemplate.Height = 23;
            this.dgvMergeProduct.Size = new System.Drawing.Size(1197, 146);
            this.dgvMergeProduct.TabIndex = 0;
            this.dgvMergeProduct.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvMergeProduct_CellMouseClick);
            this.dgvMergeProduct.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMergeProduct_CellMouseEnter);
            this.dgvMergeProduct.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvMergeProduct_MouseUp);
            // 
            // pnMergeProduct
            // 
            this.pnMergeProduct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnMergeProduct.Controls.Add(this.panel13);
            this.pnMergeProduct.Controls.Add(this.panel12);
            this.pnMergeProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMergeProduct.Location = new System.Drawing.Point(0, 0);
            this.pnMergeProduct.Name = "pnMergeProduct";
            this.pnMergeProduct.Size = new System.Drawing.Size(1199, 172);
            this.pnMergeProduct.TabIndex = 4;
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.dgvMergeProduct);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel13.Location = new System.Drawing.Point(0, 24);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(1197, 146);
            this.panel13.TabIndex = 1;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.label25);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel12.Location = new System.Drawing.Point(0, 0);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(1197, 24);
            this.panel12.TabIndex = 0;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label25.Location = new System.Drawing.Point(3, 5);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(170, 12);
            this.label25.TabIndex = 2;
            this.label25.Text = "* 병합된 품목 리스트입니다";
            // 
            // MergeProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 172);
            this.Controls.Add(this.pnMergeProduct);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MergeProduct";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "병합품목";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MergeProduct_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMergeProduct)).EndInit();
            this.pnMergeProduct.ResumeLayout(false);
            this.panel13.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvMergeProduct;
        private System.Windows.Forms.Panel pnMergeProduct;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Label label25;
    }
}