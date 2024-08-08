namespace AdoNetWindow.PurchaseManager
{
    partial class PurchaseUnitPriceInfo
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PurchaseUnitPriceInfo));
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnSearching = new System.Windows.Forms.Button();
            this.btnCostCalculate = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnCompanyAdd = new System.Windows.Forms.Button();
            this.btnRefreshCompany = new System.Windows.Forms.Button();
            this.btnRefreshProduct = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.pnDgv = new System.Windows.Forms.Panel();
            this.dgvUnitPrice = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weight_calculate = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cost_unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUpdattime = new System.Windows.Forms.TextBox();
            this.btnCalendarUpdatetime = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnProductSearching = new System.Windows.Forms.Button();
            this.btnCompanySearching = new System.Windows.Forms.Button();
            this.cbCompany = new System.Windows.Forms.ComboBox();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSizes = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rbFOB = new System.Windows.Forms.RadioButton();
            this.rbCFR = new System.Windows.Forms.RadioButton();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pnDgv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnitPrice)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.btnSelect);
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Controls.Add(this.btnInsert);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 756);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1481, 43);
            this.panel2.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnSearching);
            this.panel4.Controls.Add(this.btnCostCalculate);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(1200, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(281, 43);
            this.panel4.TabIndex = 109;
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.Location = new System.Drawing.Point(90, 3);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(87, 36);
            this.btnSearching.TabIndex = 5;
            this.btnSearching.Text = "품목조회(E)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // btnCostCalculate
            // 
            this.btnCostCalculate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCostCalculate.Location = new System.Drawing.Point(183, 3);
            this.btnCostCalculate.Name = "btnCostCalculate";
            this.btnCostCalculate.Size = new System.Drawing.Size(92, 36);
            this.btnCostCalculate.TabIndex = 4;
            this.btnCostCalculate.Text = "원가계산(W)";
            this.btnCostCalculate.UseVisualStyleBackColor = true;
            this.btnCostCalculate.Click += new System.EventHandler(this.btnCostCalculate_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelect.Location = new System.Drawing.Point(73, 3);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(94, 36);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "품목검색(Q)";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(171, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 36);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInsert.ForeColor = System.Drawing.Color.Blue;
            this.btnInsert.Location = new System.Drawing.Point(3, 3);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(66, 36);
            this.btnInsert.TabIndex = 0;
            this.btnInsert.Text = "등록(A)";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnCompanyAdd
            // 
            this.btnCompanyAdd.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCompanyAdd.ForeColor = System.Drawing.Color.IndianRed;
            this.btnCompanyAdd.Location = new System.Drawing.Point(1362, 5);
            this.btnCompanyAdd.Name = "btnCompanyAdd";
            this.btnCompanyAdd.Size = new System.Drawing.Size(110, 22);
            this.btnCompanyAdd.TabIndex = 12;
            this.btnCompanyAdd.Text = "거채처 추가(S)";
            this.btnCompanyAdd.UseVisualStyleBackColor = true;
            this.btnCompanyAdd.Click += new System.EventHandler(this.btnCompanyAdd_Click);
            // 
            // btnRefreshCompany
            // 
            this.btnRefreshCompany.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRefreshCompany.Location = new System.Drawing.Point(373, 2);
            this.btnRefreshCompany.Name = "btnRefreshCompany";
            this.btnRefreshCompany.Size = new System.Drawing.Size(105, 23);
            this.btnRefreshCompany.TabIndex = 111;
            this.btnRefreshCompany.Text = "거래처 초기화";
            this.btnRefreshCompany.UseVisualStyleBackColor = true;
            this.btnRefreshCompany.Click += new System.EventHandler(this.btnRefreshCompany_Click);
            // 
            // btnRefreshProduct
            // 
            this.btnRefreshProduct.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRefreshProduct.Location = new System.Drawing.Point(264, 2);
            this.btnRefreshProduct.Name = "btnRefreshProduct";
            this.btnRefreshProduct.Size = new System.Drawing.Size(103, 23);
            this.btnRefreshProduct.TabIndex = 110;
            this.btnRefreshProduct.Text = "품목 초기화";
            this.btnRefreshProduct.UseVisualStyleBackColor = true;
            this.btnRefreshProduct.Click += new System.EventHandler(this.btnRefreshProduct_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRefresh.Location = new System.Drawing.Point(178, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 23);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "초기화(F5)";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // pnDgv
            // 
            this.pnDgv.Controls.Add(this.dgvUnitPrice);
            this.pnDgv.Controls.Add(this.panel5);
            this.pnDgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnDgv.Location = new System.Drawing.Point(0, 59);
            this.pnDgv.Name = "pnDgv";
            this.pnDgv.Size = new System.Drawing.Size(1481, 697);
            this.pnDgv.TabIndex = 2;
            // 
            // dgvUnitPrice
            // 
            this.dgvUnitPrice.AllowUserToAddRows = false;
            this.dgvUnitPrice.AllowUserToDeleteRows = false;
            this.dgvUnitPrice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.product,
            this.origin,
            this.sizes,
            this.unit,
            this.unit2,
            this.weight_calculate,
            this.cost_unit,
            this.weight,
            this.manager});
            this.dgvUnitPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUnitPrice.EnableHeadersVisualStyles = false;
            this.dgvUnitPrice.Location = new System.Drawing.Point(0, 0);
            this.dgvUnitPrice.Name = "dgvUnitPrice";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUnitPrice.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvUnitPrice.RowHeadersWidth = 40;
            this.dgvUnitPrice.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvUnitPrice.RowTemplate.Height = 23;
            this.dgvUnitPrice.Size = new System.Drawing.Size(1481, 678);
            this.dgvUnitPrice.TabIndex = 2;
            this.dgvUnitPrice.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnitPrice_CellDoubleClick);
            this.dgvUnitPrice.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvUnitPrice_CellMouseClick);
            this.dgvUnitPrice.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnitPrice_CellMouseEnter);
            this.dgvUnitPrice.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvUnitPrice_CellPainting);
            this.dgvUnitPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvUnitPrice_KeyPress);
            this.dgvUnitPrice.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvUnitPrice_MouseUp);
            // 
            // product
            // 
            this.product.HeaderText = "품명";
            this.product.Name = "product";
            this.product.Width = 200;
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
            this.sizes.Width = 70;
            // 
            // unit
            // 
            this.unit.HeaderText = "단위";
            this.unit.Name = "unit";
            this.unit.Width = 70;
            // 
            // unit2
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.unit2.DefaultCellStyle = dataGridViewCellStyle1;
            this.unit2.HeaderText = "단위";
            this.unit2.Name = "unit2";
            this.unit2.Width = 70;
            // 
            // weight_calculate
            // 
            this.weight_calculate.HeaderText = "중량계산";
            this.weight_calculate.Name = "weight_calculate";
            this.weight_calculate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.weight_calculate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.weight_calculate.Width = 60;
            // 
            // cost_unit
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cost_unit.DefaultCellStyle = dataGridViewCellStyle2;
            this.cost_unit.HeaderText = "트레이";
            this.cost_unit.Name = "cost_unit";
            this.cost_unit.Width = 70;
            // 
            // weight
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.weight.DefaultCellStyle = dataGridViewCellStyle3;
            this.weight.HeaderText = "중량";
            this.weight.Name = "weight";
            this.weight.Width = 70;
            // 
            // manager
            // 
            this.manager.HeaderText = "담당자";
            this.manager.Name = "manager";
            this.manager.Width = 70;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.label10);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 678);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1481, 19);
            this.panel5.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(3, 3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(561, 12);
            this.label10.TabIndex = 107;
            this.label10.Text = "* 비공개 오퍼가는 셀을 더블클릭하여 배경색이 입혀진 상태로 등록하시면 비공개처리 됩니다.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "품명";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(385, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "원산지";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "등록일자";
            // 
            // txtUpdattime
            // 
            this.txtUpdattime.Location = new System.Drawing.Point(78, 3);
            this.txtUpdattime.Name = "txtUpdattime";
            this.txtUpdattime.Size = new System.Drawing.Size(95, 21);
            this.txtUpdattime.TabIndex = 1;
            this.txtUpdattime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUpdattime_KeyDown);
            this.txtUpdattime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUpdattime_KeyPress);
            // 
            // btnCalendarUpdatetime
            // 
            this.btnCalendarUpdatetime.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCalendarUpdatetime.BackgroundImage")));
            this.btnCalendarUpdatetime.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCalendarUpdatetime.FlatAppearance.BorderSize = 0;
            this.btnCalendarUpdatetime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalendarUpdatetime.Location = new System.Drawing.Point(175, 1);
            this.btnCalendarUpdatetime.Name = "btnCalendarUpdatetime";
            this.btnCalendarUpdatetime.Size = new System.Drawing.Size(22, 23);
            this.btnCalendarUpdatetime.TabIndex = 2;
            this.btnCalendarUpdatetime.UseVisualStyleBackColor = true;
            this.btnCalendarUpdatetime.Click += new System.EventHandler(this.btnCalendarUpdatetime_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(215, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 99;
            this.label5.Text = "담당자";
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(262, 3);
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(95, 21);
            this.txtManager.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnProductSearching);
            this.panel1.Controls.Add(this.btnCompanySearching);
            this.panel1.Controls.Add(this.btnCompanyAdd);
            this.panel1.Controls.Add(this.cbCompany);
            this.panel1.Controls.Add(this.txtOrigin);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtProduct);
            this.panel1.Controls.Add(this.txtUnit);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtSizes);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1481, 32);
            this.panel1.TabIndex = 0;
            // 
            // btnProductSearching
            // 
            this.btnProductSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnProductSearching.ForeColor = System.Drawing.Color.IndianRed;
            this.btnProductSearching.Location = new System.Drawing.Point(294, 5);
            this.btnProductSearching.Name = "btnProductSearching";
            this.btnProductSearching.Size = new System.Drawing.Size(74, 22);
            this.btnProductSearching.TabIndex = 1;
            this.btnProductSearching.Text = "검색 (F4)";
            this.btnProductSearching.UseVisualStyleBackColor = true;
            this.btnProductSearching.Click += new System.EventHandler(this.btnProductSearching_Click);
            // 
            // btnCompanySearching
            // 
            this.btnCompanySearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCompanySearching.ForeColor = System.Drawing.Color.IndianRed;
            this.btnCompanySearching.Location = new System.Drawing.Point(1282, 5);
            this.btnCompanySearching.Name = "btnCompanySearching";
            this.btnCompanySearching.Size = new System.Drawing.Size(74, 22);
            this.btnCompanySearching.TabIndex = 10;
            this.btnCompanySearching.Text = "검색 (F9)";
            this.btnCompanySearching.UseVisualStyleBackColor = true;
            this.btnCompanySearching.Click += new System.EventHandler(this.btnCompanySearching_Click);
            // 
            // cbCompany
            // 
            this.cbCompany.AllowDrop = true;
            this.cbCompany.FormattingEnabled = true;
            this.cbCompany.Location = new System.Drawing.Point(989, 6);
            this.cbCompany.Name = "cbCompany";
            this.cbCompany.Size = new System.Drawing.Size(287, 20);
            this.cbCompany.TabIndex = 8;
            // 
            // txtOrigin
            // 
            this.txtOrigin.Location = new System.Drawing.Point(432, 5);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.Size = new System.Drawing.Size(139, 21);
            this.txtOrigin.TabIndex = 2;
            this.txtOrigin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUnit_KeyDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(942, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 105;
            this.label8.Text = "거래처";
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(47, 5);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(241, 21);
            this.txtProduct.TabIndex = 0;
            this.txtProduct.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUnit_KeyDown);
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(798, 5);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(129, 21);
            this.txtUnit.TabIndex = 6;
            this.txtUnit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUnit_KeyDown);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(763, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 106;
            this.label9.Text = "단위";
            // 
            // txtSizes
            // 
            this.txtSizes.Location = new System.Drawing.Point(628, 5);
            this.txtSizes.Name = "txtSizes";
            this.txtSizes.Size = new System.Drawing.Size(129, 21);
            this.txtSizes.TabIndex = 4;
            this.txtSizes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUnit_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(593, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 102;
            this.label3.Text = "규격";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rbFOB);
            this.panel3.Controls.Add(this.rbCFR);
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.txtManager);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.txtUpdattime);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.btnCalendarUpdatetime);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 32);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1481, 27);
            this.panel3.TabIndex = 1;
            // 
            // rbFOB
            // 
            this.rbFOB.AutoSize = true;
            this.rbFOB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbFOB.ForeColor = System.Drawing.Color.Blue;
            this.rbFOB.Location = new System.Drawing.Point(483, 6);
            this.rbFOB.Name = "rbFOB";
            this.rbFOB.Size = new System.Drawing.Size(82, 16);
            this.rbFOB.TabIndex = 103;
            this.rbFOB.Text = "FOB (F3)";
            this.rbFOB.UseVisualStyleBackColor = true;
            // 
            // rbCFR
            // 
            this.rbCFR.AutoSize = true;
            this.rbCFR.Checked = true;
            this.rbCFR.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbCFR.ForeColor = System.Drawing.Color.Red;
            this.rbCFR.Location = new System.Drawing.Point(398, 6);
            this.rbCFR.Name = "rbCFR";
            this.rbCFR.Size = new System.Drawing.Size(82, 16);
            this.rbCFR.TabIndex = 102;
            this.rbCFR.TabStop = true;
            this.rbCFR.Text = "CFR (F1)";
            this.rbCFR.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnRefreshCompany);
            this.panel6.Controls.Add(this.btnRefreshProduct);
            this.panel6.Controls.Add(this.btnRefresh);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(997, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(484, 27);
            this.panel6.TabIndex = 101;
            // 
            // PurchaseUnitPriceInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1481, 799);
            this.Controls.Add(this.pnDgv);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "PurchaseUnitPriceInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " 매입단가 일괄등록";
            this.Load += new System.EventHandler(this.PurchaseUnitPriceInfo_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PurchaseUnitPriceInfo_KeyDown);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.pnDgv.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnitPrice)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnDgv;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnSelect;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvUnitPrice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUpdattime;
        private System.Windows.Forms.Button btnCalendarUpdatetime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtSizes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbCompany;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnCompanyAdd;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Button btnCostCalculate;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnRefreshCompany;
        private System.Windows.Forms.Button btnRefreshProduct;
        private System.Windows.Forms.RadioButton rbFOB;
        private System.Windows.Forms.RadioButton rbCFR;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn weight_calculate;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn weight;
        private System.Windows.Forms.DataGridViewTextBoxColumn manager;
        private System.Windows.Forms.Button btnProductSearching;
        private System.Windows.Forms.Button btnCompanySearching;
    }
}