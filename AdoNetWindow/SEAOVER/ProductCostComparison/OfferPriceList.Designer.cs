namespace AdoNetWindow.SEAOVER.ProductCostComparison
{
    partial class OfferPriceList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OfferPriceList));
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvOfferPrice = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbDirection = new System.Windows.Forms.Label();
            this.lbRemark = new System.Windows.Forms.Label();
            this.updatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.offer_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fixed_tariff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chk = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOfferPrice)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvOfferPrice);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(492, 134);
            this.panel1.TabIndex = 0;
            // 
            // dgvOfferPrice
            // 
            this.dgvOfferPrice.AllowUserToAddRows = false;
            this.dgvOfferPrice.AllowUserToDeleteRows = false;
            this.dgvOfferPrice.AllowUserToResizeRows = false;
            this.dgvOfferPrice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvOfferPrice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.updatetime,
            this.offer_price,
            this.fixed_tariff,
            this.company,
            this.chk});
            this.dgvOfferPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOfferPrice.EnableHeadersVisualStyles = false;
            this.dgvOfferPrice.Location = new System.Drawing.Point(0, 0);
            this.dgvOfferPrice.Name = "dgvOfferPrice";
            this.dgvOfferPrice.RowHeadersVisible = false;
            this.dgvOfferPrice.RowTemplate.Height = 23;
            this.dgvOfferPrice.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOfferPrice.Size = new System.Drawing.Size(492, 134);
            this.dgvOfferPrice.TabIndex = 0;
            this.dgvOfferPrice.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOfferPrice_CellClick);
            this.dgvOfferPrice.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOfferPrice_CellDoubleClick);
            this.dgvOfferPrice.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvOfferPrice_DataError);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lbDirection);
            this.panel2.Controls.Add(this.lbRemark);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 134);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(492, 23);
            this.panel2.TabIndex = 1;
            // 
            // lbDirection
            // 
            this.lbDirection.AutoSize = true;
            this.lbDirection.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbDirection.ForeColor = System.Drawing.Color.Red;
            this.lbDirection.Location = new System.Drawing.Point(468, -1);
            this.lbDirection.Name = "lbDirection";
            this.lbDirection.Size = new System.Drawing.Size(26, 21);
            this.lbDirection.TabIndex = 3;
            this.lbDirection.Text = "➤";
            // 
            // lbRemark
            // 
            this.lbRemark.AutoSize = true;
            this.lbRemark.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbRemark.ForeColor = System.Drawing.Color.Red;
            this.lbRemark.Location = new System.Drawing.Point(4, 6);
            this.lbRemark.Name = "lbRemark";
            this.lbRemark.Size = new System.Drawing.Size(426, 11);
            this.lbRemark.TabIndex = 2;
            this.lbRemark.Text = "* 최신기준 일주일 상간의 오퍼내역입니다. 사용하실 오퍼가를 선택해주세요.";
            // 
            // updatetime
            // 
            this.updatetime.HeaderText = "오퍼일자";
            this.updatetime.Name = "updatetime";
            // 
            // offer_price
            // 
            this.offer_price.HeaderText = "오퍼가";
            this.offer_price.Name = "offer_price";
            // 
            // fixed_tariff
            // 
            this.fixed_tariff.HeaderText = "고지가";
            this.fixed_tariff.Name = "fixed_tariff";
            this.fixed_tariff.Width = 50;
            // 
            // company
            // 
            this.company.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.company.HeaderText = "거래처";
            this.company.Name = "company";
            // 
            // chk
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.NullValue = "선택";
            this.chk.DefaultCellStyle = dataGridViewCellStyle1;
            this.chk.HeaderText = "";
            this.chk.Name = "chk";
            this.chk.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chk.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.chk.Width = 40;
            // 
            // OfferPriceList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(492, 157);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OfferPriceList";
            this.Text = "OfferPriceList";
            this.Load += new System.EventHandler(this.OfferPriceList_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OfferPriceList_KeyDown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOfferPrice)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvOfferPrice;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbDirection;
        private System.Windows.Forms.Label lbRemark;
        private System.Windows.Forms.DataGridViewTextBoxColumn updatetime;
        private System.Windows.Forms.DataGridViewTextBoxColumn offer_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn fixed_tariff;
        private System.Windows.Forms.DataGridViewTextBoxColumn company;
        private System.Windows.Forms.DataGridViewButtonColumn chk;
    }
}