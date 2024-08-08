namespace AdoNetWindow.PurchaseManager
{
    partial class CostAccountingGroupUnit
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtTotalWeghit = new System.Windows.Forms.TextBox();
            this.txtTotalAssortMargin2 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtTotalAssortMargin1 = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbTitle = new System.Windows.Forms.Label();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weight_calculate = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.box_weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.custom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.incidental_expense = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fixed_tariff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weight_rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assort_weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exchange_rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.margin_rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchase_price1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.domestic_sales_price1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trq_margin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.income_amount1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.income_amount2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.dgvProduct);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 39);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1627, 224);
            this.panel1.TabIndex = 0;
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chk,
            this.product,
            this.origin,
            this.sizes,
            this.sizes2,
            this.weight_calculate,
            this.box_weight,
            this.cost_unit,
            this.company,
            this.updatetime,
            this.unit_price,
            this.custom,
            this.tax,
            this.incidental_expense,
            this.fixed_tariff,
            this.assort,
            this.weight_rate,
            this.assort_weight,
            this.exchange_rate,
            this.cost_price,
            this.margin_rate,
            this.purchase_price1,
            this.domestic_sales_price1,
            this.trq,
            this.trq_margin,
            this.income_amount1,
            this.income_amount2});
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.EnableHeadersVisualStyles = false;
            this.dgvProduct.Location = new System.Drawing.Point(0, 0);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.Size = new System.Drawing.Size(1625, 222);
            this.dgvProduct.TabIndex = 2;
            this.dgvProduct.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProduct_CellMouseClick);
            this.dgvProduct.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgvProduct_PreviewKeyDown);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 263);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1627, 26);
            this.panel2.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClose.ForeColor = System.Drawing.Color.Red;
            this.btnClose.Location = new System.Drawing.Point(-2, -2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(59, 27);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "삭 제";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label15);
            this.panel7.Controls.Add(this.label14);
            this.panel7.Controls.Add(this.txtTotalWeghit);
            this.panel7.Controls.Add(this.txtTotalAssortMargin2);
            this.panel7.Controls.Add(this.label13);
            this.panel7.Controls.Add(this.txtTotalAssortMargin1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel7.Location = new System.Drawing.Point(659, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(966, 24);
            this.panel7.TabIndex = 2;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(37, 4);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(97, 16);
            this.label15.TabIndex = 113;
            this.label15.Text = "총 중량(kg)";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(642, 4);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(146, 16);
            this.label14.TabIndex = 113;
            this.label14.Text = "총 손익금액(TRQ)";
            // 
            // txtTotalWeghit
            // 
            this.txtTotalWeghit.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTotalWeghit.Location = new System.Drawing.Point(142, -1);
            this.txtTotalWeghit.Name = "txtTotalWeghit";
            this.txtTotalWeghit.Size = new System.Drawing.Size(175, 26);
            this.txtTotalWeghit.TabIndex = 112;
            this.txtTotalWeghit.Text = "0";
            this.txtTotalWeghit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtTotalAssortMargin2
            // 
            this.txtTotalAssortMargin2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTotalAssortMargin2.Location = new System.Drawing.Point(790, -1);
            this.txtTotalAssortMargin2.Name = "txtTotalAssortMargin2";
            this.txtTotalAssortMargin2.Size = new System.Drawing.Size(175, 26);
            this.txtTotalAssortMargin2.TabIndex = 112;
            this.txtTotalAssortMargin2.Text = "0";
            this.txtTotalAssortMargin2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(336, 4);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(98, 16);
            this.label13.TabIndex = 111;
            this.label13.Text = "총 손익금액";
            // 
            // txtTotalAssortMargin1
            // 
            this.txtTotalAssortMargin1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTotalAssortMargin1.Location = new System.Drawing.Point(440, -1);
            this.txtTotalAssortMargin1.Name = "txtTotalAssortMargin1";
            this.txtTotalAssortMargin1.Size = new System.Drawing.Size(175, 26);
            this.txtTotalAssortMargin1.TabIndex = 0;
            this.txtTotalAssortMargin1.Text = "0";
            this.txtTotalAssortMargin1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel3.Controls.Add(this.lbTitle);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1627, 39);
            this.panel3.TabIndex = 2;
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbTitle.Location = new System.Drawing.Point(3, 20);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(16, 16);
            this.lbTitle.TabIndex = 114;
            this.lbTitle.Text = "0";
            // 
            // chk
            // 
            this.chk.HeaderText = "";
            this.chk.Name = "chk";
            this.chk.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chk.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.chk.Visible = false;
            this.chk.Width = 30;
            // 
            // product
            // 
            this.product.HeaderText = "품명";
            this.product.Name = "product";
            this.product.Width = 118;
            // 
            // origin
            // 
            this.origin.HeaderText = "원산지";
            this.origin.Name = "origin";
            this.origin.Width = 80;
            // 
            // sizes
            // 
            this.sizes.HeaderText = "규격";
            this.sizes.Name = "sizes";
            this.sizes.Width = 70;
            // 
            // sizes2
            // 
            this.sizes2.HeaderText = "규격2";
            this.sizes2.Name = "sizes2";
            // 
            // weight_calculate
            // 
            this.weight_calculate.HeaderText = "";
            this.weight_calculate.Name = "weight_calculate";
            this.weight_calculate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.weight_calculate.Width = 20;
            // 
            // box_weight
            // 
            this.box_weight.HeaderText = "중량";
            this.box_weight.Name = "box_weight";
            this.box_weight.Width = 50;
            // 
            // cost_unit
            // 
            this.cost_unit.HeaderText = "트레이";
            this.cost_unit.Name = "cost_unit";
            this.cost_unit.Width = 50;
            // 
            // company
            // 
            this.company.HeaderText = "거래처";
            this.company.Name = "company";
            // 
            // updatetime
            // 
            this.updatetime.HeaderText = "오퍼일자";
            this.updatetime.Name = "updatetime";
            this.updatetime.Width = 70;
            // 
            // unit_price
            // 
            this.unit_price.HeaderText = "오퍼가";
            this.unit_price.Name = "unit_price";
            this.unit_price.Width = 55;
            // 
            // custom
            // 
            this.custom.HeaderText = "관세(%)";
            this.custom.Name = "custom";
            this.custom.Width = 60;
            // 
            // tax
            // 
            this.tax.HeaderText = "과세(%)";
            this.tax.Name = "tax";
            this.tax.Width = 60;
            // 
            // incidental_expense
            // 
            this.incidental_expense.HeaderText = "부대비용";
            this.incidental_expense.Name = "incidental_expense";
            this.incidental_expense.Width = 60;
            // 
            // fixed_tariff
            // 
            this.fixed_tariff.HeaderText = "고지가";
            this.fixed_tariff.Name = "fixed_tariff";
            this.fixed_tariff.Width = 60;
            // 
            // assort
            // 
            this.assort.HeaderText = "수량";
            this.assort.Name = "assort";
            this.assort.Width = 50;
            // 
            // weight_rate
            // 
            this.weight_rate.HeaderText = "비율";
            this.weight_rate.Name = "weight_rate";
            this.weight_rate.Width = 50;
            // 
            // assort_weight
            // 
            this.assort_weight.HeaderText = "총 중량";
            this.assort_weight.Name = "assort_weight";
            this.assort_weight.Width = 60;
            // 
            // exchange_rate
            // 
            this.exchange_rate.HeaderText = "환율";
            this.exchange_rate.Name = "exchange_rate";
            this.exchange_rate.Width = 60;
            // 
            // cost_price
            // 
            this.cost_price.HeaderText = "원가";
            this.cost_price.Name = "cost_price";
            this.cost_price.Width = 70;
            // 
            // margin_rate
            // 
            this.margin_rate.HeaderText = "마진율";
            this.margin_rate.Name = "margin_rate";
            this.margin_rate.Width = 60;
            // 
            // purchase_price1
            // 
            this.purchase_price1.HeaderText = "매입가";
            this.purchase_price1.Name = "purchase_price1";
            this.purchase_price1.Width = 70;
            // 
            // domestic_sales_price1
            // 
            this.domestic_sales_price1.HeaderText = "판매가";
            this.domestic_sales_price1.Name = "domestic_sales_price1";
            this.domestic_sales_price1.Width = 70;
            // 
            // trq
            // 
            this.trq.HeaderText = "TRQ";
            this.trq.Name = "trq";
            this.trq.Width = 70;
            // 
            // trq_margin
            // 
            this.trq_margin.HeaderText = "마진율(%)";
            this.trq_margin.Name = "trq_margin";
            this.trq_margin.Width = 70;
            // 
            // income_amount1
            // 
            this.income_amount1.HeaderText = "손익금액1";
            this.income_amount1.Name = "income_amount1";
            this.income_amount1.Width = 70;
            // 
            // income_amount2
            // 
            this.income_amount2.HeaderText = "손익금액2";
            this.income_amount2.Name = "income_amount2";
            this.income_amount2.Width = 70;
            // 
            // CostAccountingGroupUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Name = "CostAccountingGroupUnit";
            this.Size = new System.Drawing.Size(1627, 289);
            this.Load += new System.EventHandler(this.CostAccountingGroupUnit_Load);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.CostAccountingGroupUnit_PreviewKeyDown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtTotalWeghit;
        private System.Windows.Forms.TextBox txtTotalAssortMargin2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtTotalAssortMargin1;
        private System.Windows.Forms.Button btnClose;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProduct;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn weight_calculate;
        private System.Windows.Forms.DataGridViewTextBoxColumn box_weight;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn company;
        private System.Windows.Forms.DataGridViewTextBoxColumn updatetime;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn custom;
        private System.Windows.Forms.DataGridViewTextBoxColumn tax;
        private System.Windows.Forms.DataGridViewTextBoxColumn incidental_expense;
        private System.Windows.Forms.DataGridViewTextBoxColumn fixed_tariff;
        private System.Windows.Forms.DataGridViewTextBoxColumn assort;
        private System.Windows.Forms.DataGridViewTextBoxColumn weight_rate;
        private System.Windows.Forms.DataGridViewTextBoxColumn assort_weight;
        private System.Windows.Forms.DataGridViewTextBoxColumn exchange_rate;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn margin_rate;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchase_price1;
        private System.Windows.Forms.DataGridViewTextBoxColumn domestic_sales_price1;
        private System.Windows.Forms.DataGridViewTextBoxColumn trq;
        private System.Windows.Forms.DataGridViewTextBoxColumn trq_margin;
        private System.Windows.Forms.DataGridViewTextBoxColumn income_amount1;
        private System.Windows.Forms.DataGridViewTextBoxColumn income_amount2;
    }
}
