namespace AdoNetWindow.SEAOVER.PriceComparison
{
    partial class StockManager
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StockManager));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtExhaust = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtEtd = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtContractDate = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtStock = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtRoundStock = new System.Windows.Forms.TextBox();
            this.txtAvgStockDay = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtAvgStockMonth = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtPendingTerm = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMakeTerm = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtShippingTerm = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSizes = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgvContract = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.etd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pending_term = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehousing_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouse_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exhausted_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exhausted_day_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.month_around = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.after_qty_exhausted_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recommend_etd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recommend_contract_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ato_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contract_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bl_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnMinus = new System.Windows.Forms.Button();
            this.btnPlus = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnCalendar = new System.Windows.Forms.Button();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContract)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(302, 517);
            this.panel1.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.txtExhaust);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.txtEtd);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.txtContractDate);
            this.groupBox4.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox4.Location = new System.Drawing.Point(8, 402);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(286, 110);
            this.groupBox4.TabIndex = 27;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "추천 계약일";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(7, 30);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 13);
            this.label12.TabIndex = 14;
            this.label12.Text = "소진일자";
            // 
            // txtExhaust
            // 
            this.txtExhaust.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtExhaust.Location = new System.Drawing.Point(170, 27);
            this.txtExhaust.Name = "txtExhaust";
            this.txtExhaust.Size = new System.Drawing.Size(112, 22);
            this.txtExhaust.TabIndex = 15;
            this.txtExhaust.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtExhaust_MouseDoubleClick);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(7, 57);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(62, 13);
            this.label13.TabIndex = 16;
            this.label13.Text = "최소 ETD";
            // 
            // txtEtd
            // 
            this.txtEtd.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtEtd.Location = new System.Drawing.Point(170, 54);
            this.txtEtd.Name = "txtEtd";
            this.txtEtd.Size = new System.Drawing.Size(112, 22);
            this.txtEtd.TabIndex = 17;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(7, 84);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(89, 13);
            this.label15.TabIndex = 18;
            this.label15.Text = "추천 계약일자";
            // 
            // txtContractDate
            // 
            this.txtContractDate.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtContractDate.Location = new System.Drawing.Point(170, 81);
            this.txtContractDate.Name = "txtContractDate";
            this.txtContractDate.Size = new System.Drawing.Size(112, 22);
            this.txtContractDate.TabIndex = 19;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.txtStock);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.txtRoundStock);
            this.groupBox3.Controls.Add(this.txtAvgStockDay);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.txtAvgStockMonth);
            this.groupBox3.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox3.Location = new System.Drawing.Point(8, 257);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(286, 139);
            this.groupBox3.TabIndex = 26;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "재고 및 판매량";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(7, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "재고량";
            // 
            // txtStock
            // 
            this.txtStock.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtStock.Location = new System.Drawing.Point(170, 27);
            this.txtStock.Name = "txtStock";
            this.txtStock.Size = new System.Drawing.Size(112, 22);
            this.txtStock.TabIndex = 15;
            this.txtStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtStock.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInsertPendingterm_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(7, 57);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "일 평균판매량";
            // 
            // txtRoundStock
            // 
            this.txtRoundStock.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtRoundStock.Location = new System.Drawing.Point(170, 108);
            this.txtRoundStock.Name = "txtRoundStock";
            this.txtRoundStock.Size = new System.Drawing.Size(112, 22);
            this.txtRoundStock.TabIndex = 23;
            this.txtRoundStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRoundStock.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInsertPendingterm_KeyPress);
            // 
            // txtAvgStockDay
            // 
            this.txtAvgStockDay.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtAvgStockDay.Location = new System.Drawing.Point(170, 54);
            this.txtAvgStockDay.Name = "txtAvgStockDay";
            this.txtAvgStockDay.Size = new System.Drawing.Size(112, 22);
            this.txtAvgStockDay.TabIndex = 17;
            this.txtAvgStockDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtAvgStockDay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInsertPendingterm_KeyPress);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(7, 111);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(46, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "회전율";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(7, 84);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "월 평균판매량";
            // 
            // txtAvgStockMonth
            // 
            this.txtAvgStockMonth.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtAvgStockMonth.Location = new System.Drawing.Point(170, 81);
            this.txtAvgStockMonth.Name = "txtAvgStockMonth";
            this.txtAvgStockMonth.Size = new System.Drawing.Size(112, 22);
            this.txtAvgStockMonth.TabIndex = 19;
            this.txtAvgStockMonth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtAvgStockMonth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInsertPendingterm_KeyPress);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtPendingTerm);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtMakeTerm);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtShippingTerm);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox2.Location = new System.Drawing.Point(8, 144);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(288, 107);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "통관기간";
            // 
            // txtPendingTerm
            // 
            this.txtPendingTerm.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPendingTerm.Location = new System.Drawing.Point(170, 78);
            this.txtPendingTerm.Name = "txtPendingTerm";
            this.txtPendingTerm.Size = new System.Drawing.Size(112, 22);
            this.txtPendingTerm.TabIndex = 13;
            this.txtPendingTerm.Text = "5";
            this.txtPendingTerm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPendingTerm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInsertPendingterm_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(7, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "생산기간";
            // 
            // txtMakeTerm
            // 
            this.txtMakeTerm.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtMakeTerm.Location = new System.Drawing.Point(170, 24);
            this.txtMakeTerm.Name = "txtMakeTerm";
            this.txtMakeTerm.Size = new System.Drawing.Size(112, 22);
            this.txtMakeTerm.TabIndex = 9;
            this.txtMakeTerm.Text = "20";
            this.txtMakeTerm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtMakeTerm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInsertPendingterm_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(7, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "배송기간";
            // 
            // txtShippingTerm
            // 
            this.txtShippingTerm.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtShippingTerm.Location = new System.Drawing.Point(170, 51);
            this.txtShippingTerm.Name = "txtShippingTerm";
            this.txtShippingTerm.Size = new System.Drawing.Size(112, 22);
            this.txtShippingTerm.TabIndex = 11;
            this.txtShippingTerm.Text = "0";
            this.txtShippingTerm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtShippingTerm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInsertPendingterm_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(7, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "통관기간";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtProduct);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtOrigin);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtSizes);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtUnit);
            this.groupBox1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(8, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(288, 134);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "품목정보";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "품목";
            // 
            // txtProduct
            // 
            this.txtProduct.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtProduct.Location = new System.Drawing.Point(106, 24);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(176, 22);
            this.txtProduct.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(6, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "원산지";
            // 
            // txtOrigin
            // 
            this.txtOrigin.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtOrigin.Location = new System.Drawing.Point(106, 51);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.Size = new System.Drawing.Size(176, 22);
            this.txtOrigin.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(6, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "규격";
            // 
            // txtSizes
            // 
            this.txtSizes.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSizes.Location = new System.Drawing.Point(106, 78);
            this.txtSizes.Name = "txtSizes";
            this.txtSizes.Size = new System.Drawing.Size(176, 22);
            this.txtSizes.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(6, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "단위";
            // 
            // txtUnit
            // 
            this.txtUnit.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtUnit.Location = new System.Drawing.Point(106, 105);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(176, 22);
            this.txtUnit.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(302, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1253, 471);
            this.panel2.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.dgvContract);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 23);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1251, 446);
            this.panel5.TabIndex = 1;
            // 
            // dgvContract
            // 
            this.dgvContract.AllowUserToAddRows = false;
            this.dgvContract.AllowUserToDeleteRows = false;
            this.dgvContract.AllowUserToResizeRows = false;
            this.dgvContract.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvContract.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.etd,
            this.eta,
            this.pending_term,
            this.warehousing_date,
            this.qty,
            this.warehouse_qty,
            this.exhausted_date,
            this.exhausted_day_count,
            this.month_around,
            this.after_qty_exhausted_date,
            this.recommend_etd,
            this.recommend_contract_date,
            this.ato_no,
            this.contract_no,
            this.bl_no});
            this.dgvContract.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvContract.EnableHeadersVisualStyles = false;
            this.dgvContract.Location = new System.Drawing.Point(0, 0);
            this.dgvContract.Name = "dgvContract";
            this.dgvContract.RowTemplate.Height = 23;
            this.dgvContract.Size = new System.Drawing.Size(1251, 446);
            this.dgvContract.TabIndex = 0;
            this.dgvContract.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellDoubleClick);
            this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            // 
            // etd
            // 
            this.etd.HeaderText = "ETD";
            this.etd.Name = "etd";
            this.etd.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.etd.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.etd.Width = 80;
            // 
            // eta
            // 
            this.eta.HeaderText = "ETA";
            this.eta.Name = "eta";
            this.eta.Width = 80;
            // 
            // pending_term
            // 
            this.pending_term.HeaderText = "통관기간";
            this.pending_term.Name = "pending_term";
            this.pending_term.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.pending_term.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.pending_term.Width = 60;
            // 
            // warehousing_date
            // 
            this.warehousing_date.HeaderText = "입고일자";
            this.warehousing_date.Name = "warehousing_date";
            this.warehousing_date.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.warehousing_date.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.warehousing_date.Width = 80;
            // 
            // qty
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.qty.DefaultCellStyle = dataGridViewCellStyle1;
            this.qty.HeaderText = "입고량";
            this.qty.Name = "qty";
            this.qty.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.qty.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.qty.Width = 60;
            // 
            // warehouse_qty
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.warehouse_qty.DefaultCellStyle = dataGridViewCellStyle2;
            this.warehouse_qty.HeaderText = "입고후 재고";
            this.warehouse_qty.Name = "warehouse_qty";
            this.warehouse_qty.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.warehouse_qty.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.warehouse_qty.Width = 75;
            // 
            // exhausted_date
            // 
            this.exhausted_date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.exhausted_date.HeaderText = "쇼트기간";
            this.exhausted_date.Name = "exhausted_date";
            this.exhausted_date.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.exhausted_date.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // exhausted_day_count
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.exhausted_day_count.DefaultCellStyle = dataGridViewCellStyle3;
            this.exhausted_day_count.HeaderText = "쇼트일수";
            this.exhausted_day_count.Name = "exhausted_day_count";
            this.exhausted_day_count.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.exhausted_day_count.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.exhausted_day_count.Width = 60;
            // 
            // month_around
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.month_around.DefaultCellStyle = dataGridViewCellStyle4;
            this.month_around.HeaderText = "회전율";
            this.month_around.Name = "month_around";
            this.month_around.Width = 60;
            // 
            // after_qty_exhausted_date
            // 
            this.after_qty_exhausted_date.HeaderText = "입고후 쇼트";
            this.after_qty_exhausted_date.Name = "after_qty_exhausted_date";
            this.after_qty_exhausted_date.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.after_qty_exhausted_date.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.after_qty_exhausted_date.Width = 80;
            // 
            // recommend_etd
            // 
            this.recommend_etd.HeaderText = "추천선적일";
            this.recommend_etd.Name = "recommend_etd";
            this.recommend_etd.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.recommend_etd.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.recommend_etd.Width = 80;
            // 
            // recommend_contract_date
            // 
            this.recommend_contract_date.HeaderText = "추천계약일";
            this.recommend_contract_date.Name = "recommend_contract_date";
            this.recommend_contract_date.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.recommend_contract_date.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.recommend_contract_date.Width = 80;
            // 
            // ato_no
            // 
            this.ato_no.HeaderText = "Ato no.";
            this.ato_no.Name = "ato_no";
            this.ato_no.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ato_no.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // contract_no
            // 
            this.contract_no.HeaderText = "Contract no.";
            this.contract_no.Name = "contract_no";
            this.contract_no.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.contract_no.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // bl_no
            // 
            this.bl_no.HeaderText = "B/L no.";
            this.bl_no.Name = "bl_no";
            this.bl_no.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.bl_no.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnMinus);
            this.panel4.Controls.Add(this.btnPlus);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1251, 23);
            this.panel4.TabIndex = 0;
            // 
            // btnMinus
            // 
            this.btnMinus.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMinus.ForeColor = System.Drawing.Color.Black;
            this.btnMinus.Location = new System.Drawing.Point(25, -2);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(29, 26);
            this.btnMinus.TabIndex = 5;
            this.btnMinus.Text = "-";
            this.btnMinus.UseVisualStyleBackColor = true;
            this.btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPlus.ForeColor = System.Drawing.Color.Black;
            this.btnPlus.Location = new System.Drawing.Point(-1, -2);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(29, 26);
            this.btnPlus.TabIndex = 4;
            this.btnPlus.Text = "+";
            this.btnPlus.UseVisualStyleBackColor = true;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.btnCalendar);
            this.panel3.Controls.Add(this.btnCalculate);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(302, 471);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1253, 46);
            this.panel3.TabIndex = 0;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnExit);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(1163, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(88, 44);
            this.panel6.TabIndex = 4;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(10, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 37);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnCalendar
            // 
            this.btnCalendar.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCalendar.ForeColor = System.Drawing.Color.Black;
            this.btnCalendar.Location = new System.Drawing.Point(86, 4);
            this.btnCalendar.Name = "btnCalendar";
            this.btnCalendar.Size = new System.Drawing.Size(119, 37);
            this.btnCalendar.TabIndex = 2;
            this.btnCalendar.Text = "달력으로보기(W)";
            this.btnCalendar.UseVisualStyleBackColor = true;
            this.btnCalendar.Click += new System.EventHandler(this.btnCalendar_Click);
            // 
            // btnCalculate
            // 
            this.btnCalculate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCalculate.ForeColor = System.Drawing.Color.Blue;
            this.btnCalculate.Location = new System.Drawing.Point(5, 4);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(75, 37);
            this.btnCalculate.TabIndex = 0;
            this.btnCalculate.Text = "재계산(Q)";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // StockManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1555, 517);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StockManager";
            this.Text = "재고관리";
            this.Load += new System.EventHandler(this.StockManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StockManager_KeyDown);
            this.panel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvContract)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtStock;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtRoundStock;
        private System.Windows.Forms.TextBox txtAvgStockDay;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtAvgStockMonth;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtPendingTerm;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMakeTerm;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtShippingTerm;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSizes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtExhaust;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtEtd;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtContractDate;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnCalendar;
        private System.Windows.Forms.Panel panel5;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvContract;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.DataGridViewTextBoxColumn etd;
        private System.Windows.Forms.DataGridViewTextBoxColumn eta;
        private System.Windows.Forms.DataGridViewTextBoxColumn pending_term;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehousing_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehouse_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn exhausted_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn exhausted_day_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn month_around;
        private System.Windows.Forms.DataGridViewTextBoxColumn after_qty_exhausted_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn recommend_etd;
        private System.Windows.Forms.DataGridViewTextBoxColumn recommend_contract_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn ato_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn contract_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn bl_no;
    }
}