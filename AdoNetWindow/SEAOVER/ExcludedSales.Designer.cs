namespace AdoNetWindow.SEAOVER
{
    partial class ExcludedSales
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExcludedSales));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSeaoverUnit = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtUnitCount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPriceUnit = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSizes = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.dgvProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSearching = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSttdate = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtEnddate = new System.Windows.Forms.TextBox();
            this.btnSttDateCalendar = new System.Windows.Forms.Button();
            this.btnEndDateCalendar = new System.Windows.Forms.Button();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seaover_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnEndDateCalendar);
            this.panel1.Controls.Add(this.btnSttDateCalendar);
            this.panel1.Controls.Add(this.txtEnddate);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtSttdate);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtSeaoverUnit);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtUnitCount);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtPriceUnit);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtUnit);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtSizes);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtOrigin);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtProduct);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1257, 31);
            this.panel1.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1138, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "S단위";
            // 
            // txtSeaoverUnit
            // 
            this.txtSeaoverUnit.Location = new System.Drawing.Point(1181, 4);
            this.txtSeaoverUnit.Name = "txtSeaoverUnit";
            this.txtSeaoverUnit.Size = new System.Drawing.Size(64, 21);
            this.txtSeaoverUnit.TabIndex = 12;
            this.txtSeaoverUnit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1009, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "단위수량";
            // 
            // txtUnitCount
            // 
            this.txtUnitCount.Location = new System.Drawing.Point(1068, 5);
            this.txtUnitCount.Name = "txtUnitCount";
            this.txtUnitCount.Size = new System.Drawing.Size(64, 21);
            this.txtUnitCount.TabIndex = 10;
            this.txtUnitCount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(880, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "가격단위";
            // 
            // txtPriceUnit
            // 
            this.txtPriceUnit.Location = new System.Drawing.Point(939, 5);
            this.txtPriceUnit.Name = "txtPriceUnit";
            this.txtPriceUnit.Size = new System.Drawing.Size(64, 21);
            this.txtPriceUnit.TabIndex = 8;
            this.txtPriceUnit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(777, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "단위";
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(812, 5);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(64, 21);
            this.txtUnit.TabIndex = 6;
            this.txtUnit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(634, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "규격";
            // 
            // txtSizes
            // 
            this.txtSizes.Location = new System.Drawing.Point(665, 5);
            this.txtSizes.Name = "txtSizes";
            this.txtSizes.Size = new System.Drawing.Size(106, 21);
            this.txtSizes.TabIndex = 4;
            this.txtSizes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(477, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "원산지";
            // 
            // txtOrigin
            // 
            this.txtOrigin.Location = new System.Drawing.Point(524, 5);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.Size = new System.Drawing.Size(106, 21);
            this.txtOrigin.TabIndex = 3;
            this.txtOrigin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(266, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "품명";
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(300, 5);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(170, 21);
            this.txtProduct.TabIndex = 2;
            this.txtProduct.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToResizeRows = false;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.product,
            this.origin,
            this.sizes,
            this.unit,
            this.price_unit,
            this.unit_count,
            this.seaover_unit,
            this.sale_date,
            this.sale_company,
            this.sale_qty,
            this.remark});
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.EnableHeadersVisualStyles = false;
            this.dgvProduct.Location = new System.Drawing.Point(0, 0);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProduct.Size = new System.Drawing.Size(1257, 559);
            this.dgvProduct.TabIndex = 14;
            this.dgvProduct.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProduct_CellMouseClick);
            this.dgvProduct.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvProduct_MouseUp);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvProduct);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 31);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1257, 559);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSearching);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 590);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1257, 43);
            this.panel3.TabIndex = 2;
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.ForeColor = System.Drawing.Color.Black;
            this.btnSearching.Location = new System.Drawing.Point(3, 3);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(64, 37);
            this.btnSearching.TabIndex = 5;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(71, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 15;
            this.label8.Text = "기간";
            // 
            // txtSttdate
            // 
            this.txtSttdate.Location = new System.Drawing.Point(39, 5);
            this.txtSttdate.Name = "txtSttdate";
            this.txtSttdate.Size = new System.Drawing.Size(74, 21);
            this.txtSttdate.TabIndex = 0;
            this.txtSttdate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSttdate_KeyDown);
            this.txtSttdate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSttdate_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(143, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 12);
            this.label9.TabIndex = 16;
            this.label9.Text = "~";
            // 
            // txtEnddate
            // 
            this.txtEnddate.Location = new System.Drawing.Point(163, 5);
            this.txtEnddate.Name = "txtEnddate";
            this.txtEnddate.Size = new System.Drawing.Size(74, 21);
            this.txtEnddate.TabIndex = 1;
            this.txtEnddate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSttdate_KeyDown);
            this.txtEnddate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSttdate_KeyPress);
            // 
            // btnSttDateCalendar
            // 
            this.btnSttDateCalendar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSttDateCalendar.BackgroundImage")));
            this.btnSttDateCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSttDateCalendar.FlatAppearance.BorderSize = 0;
            this.btnSttDateCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSttDateCalendar.Location = new System.Drawing.Point(115, 3);
            this.btnSttDateCalendar.Name = "btnSttDateCalendar";
            this.btnSttDateCalendar.Size = new System.Drawing.Size(22, 23);
            this.btnSttDateCalendar.TabIndex = 30;
            this.btnSttDateCalendar.UseVisualStyleBackColor = true;
            this.btnSttDateCalendar.Click += new System.EventHandler(this.btnSttDateCalendar_Click);
            // 
            // btnEndDateCalendar
            // 
            this.btnEndDateCalendar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEndDateCalendar.BackgroundImage")));
            this.btnEndDateCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEndDateCalendar.FlatAppearance.BorderSize = 0;
            this.btnEndDateCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEndDateCalendar.Location = new System.Drawing.Point(238, 3);
            this.btnEndDateCalendar.Name = "btnEndDateCalendar";
            this.btnEndDateCalendar.Size = new System.Drawing.Size(22, 23);
            this.btnEndDateCalendar.TabIndex = 31;
            this.btnEndDateCalendar.UseVisualStyleBackColor = true;
            this.btnEndDateCalendar.Click += new System.EventHandler(this.btnEndDateCalendar_Click);
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.Visible = false;
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
            this.unit.Width = 80;
            // 
            // price_unit
            // 
            this.price_unit.HeaderText = "가격단위";
            this.price_unit.Name = "price_unit";
            this.price_unit.Width = 80;
            // 
            // unit_count
            // 
            this.unit_count.HeaderText = "단위수량";
            this.unit_count.Name = "unit_count";
            this.unit_count.Width = 80;
            // 
            // seaover_unit
            // 
            this.seaover_unit.HeaderText = "S단위";
            this.seaover_unit.Name = "seaover_unit";
            this.seaover_unit.Width = 80;
            // 
            // sale_date
            // 
            this.sale_date.HeaderText = "판매일자";
            this.sale_date.Name = "sale_date";
            this.sale_date.Width = 80;
            // 
            // sale_company
            // 
            this.sale_company.HeaderText = "판매처";
            this.sale_company.Name = "sale_company";
            this.sale_company.Width = 150;
            // 
            // sale_qty
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.sale_qty.DefaultCellStyle = dataGridViewCellStyle5;
            this.sale_qty.HeaderText = "수량";
            this.sale_qty.Name = "sale_qty";
            this.sale_qty.Width = 60;
            // 
            // remark
            // 
            this.remark.HeaderText = "비고";
            this.remark.Name = "remark";
            this.remark.Width = 200;
            // 
            // ExcludedSales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1257, 633);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "ExcludedSales";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "제외판매";
            this.Load += new System.EventHandler(this.ExcludedSales_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExcludedSales_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProduct;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSeaoverUnit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtUnitCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPriceUnit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSizes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtEnddate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSttdate;
        private System.Windows.Forms.Button btnEndDateCalendar;
        private System.Windows.Forms.Button btnSttDateCalendar;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn price_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_company;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
    }
}