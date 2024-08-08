namespace AdoNetWindow.Pending
{
    partial class AdvancePaymentManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancePaymentManager));
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.dgvProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtAtono = new System.Windows.Forms.TextBox();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.txtBlno = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDivision = new System.Windows.Forms.TextBox();
            this.txtLcno = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAgency = new System.Windows.Forms.TextBox();
            this.txtUsance = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel15 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.txtPaymentBank = new System.Windows.Forms.TextBox();
            this.txtRemark = new System.Windows.Forms.RichTextBox();
            this.cbPaymentDateStatus = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbPaymentCurrency = new System.Windows.Forms.ComboBox();
            this.txtPaymentDate = new System.Windows.Forms.TextBox();
            this.lbPayId = new System.Windows.Forms.Label();
            this.btnPaymentDateCalendar = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPaymentAmount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnComfirm = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnUpadate = new System.Windows.Forms.Button();
            this.btnPayComplete = new System.Windows.Forms.Button();
            this.btnDeadline = new System.Windows.Forms.Button();
            this.btnDetail = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel15.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panel15);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(852, 360);
            this.panel3.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel2);
            this.panel5.Controls.Add(this.panel8);
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(852, 252);
            this.panel5.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 26);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(852, 200);
            this.panel2.TabIndex = 13;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.dgvProduct);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(227, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(623, 198);
            this.panel6.TabIndex = 114;
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToResizeColumns = false;
            this.dgvProduct.AllowUserToResizeRows = false;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.product,
            this.origin,
            this.sizes,
            this.unit,
            this.qty});
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.EnableHeadersVisualStyles = false;
            this.dgvProduct.Location = new System.Drawing.Point(0, 0);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.Size = new System.Drawing.Size(623, 198);
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
            // unit
            // 
            this.unit.HeaderText = "단위";
            this.unit.Name = "unit";
            // 
            // qty
            // 
            this.qty.HeaderText = "수량";
            this.qty.Name = "qty";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.label12);
            this.panel4.Controls.Add(this.txtAtono);
            this.panel4.Controls.Add(this.txtManager);
            this.panel4.Controls.Add(this.txtBlno);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.txtDivision);
            this.panel4.Controls.Add(this.txtLcno);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.txtAgency);
            this.panel4.Controls.Add(this.txtUsance);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(227, 198);
            this.panel4.TabIndex = 113;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(9, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 12);
            this.label9.TabIndex = 100;
            this.label9.Text = "ATO No";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(9, 169);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 12);
            this.label12.TabIndex = 112;
            this.label12.Text = "담당자";
            // 
            // txtAtono
            // 
            this.txtAtono.Location = new System.Drawing.Point(78, 4);
            this.txtAtono.MaxLength = 10;
            this.txtAtono.Name = "txtAtono";
            this.txtAtono.Size = new System.Drawing.Size(143, 21);
            this.txtAtono.TabIndex = 5;
            this.txtAtono.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(78, 166);
            this.txtManager.MaxLength = 10;
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(143, 21);
            this.txtManager.TabIndex = 111;
            this.txtManager.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtBlno
            // 
            this.txtBlno.Location = new System.Drawing.Point(78, 31);
            this.txtBlno.MaxLength = 10;
            this.txtBlno.Name = "txtBlno";
            this.txtBlno.Size = new System.Drawing.Size(143, 21);
            this.txtBlno.TabIndex = 101;
            this.txtBlno.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(9, 142);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 12);
            this.label11.TabIndex = 110;
            this.label11.Text = "화주";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(9, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 12);
            this.label1.TabIndex = 102;
            this.label1.Text = "B/L No";
            // 
            // txtDivision
            // 
            this.txtDivision.Location = new System.Drawing.Point(78, 139);
            this.txtDivision.MaxLength = 10;
            this.txtDivision.Name = "txtDivision";
            this.txtDivision.Size = new System.Drawing.Size(143, 21);
            this.txtDivision.TabIndex = 109;
            this.txtDivision.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtLcno
            // 
            this.txtLcno.Location = new System.Drawing.Point(78, 58);
            this.txtLcno.MaxLength = 10;
            this.txtLcno.Name = "txtLcno";
            this.txtLcno.Size = new System.Drawing.Size(143, 21);
            this.txtLcno.TabIndex = 103;
            this.txtLcno.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(9, 117);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 12);
            this.label10.TabIndex = 108;
            this.label10.Text = "대행";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(9, 63);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 104;
            this.label7.Text = "L/C No";
            // 
            // txtAgency
            // 
            this.txtAgency.Location = new System.Drawing.Point(78, 112);
            this.txtAgency.MaxLength = 10;
            this.txtAgency.Name = "txtAgency";
            this.txtAgency.Size = new System.Drawing.Size(143, 21);
            this.txtAgency.TabIndex = 107;
            this.txtAgency.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtUsance
            // 
            this.txtUsance.Location = new System.Drawing.Point(78, 85);
            this.txtUsance.MaxLength = 10;
            this.txtUsance.Name = "txtUsance";
            this.txtUsance.Size = new System.Drawing.Size(143, 21);
            this.txtUsance.TabIndex = 105;
            this.txtUsance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(9, 90);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 12);
            this.label8.TabIndex = 106;
            this.label8.Text = "수입구분";
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Controls.Add(this.label3);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(852, 26);
            this.panel8.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(393, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 16);
            this.label3.TabIndex = 102;
            this.label3.Text = "팬딩정보";
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.label6);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(0, 226);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(852, 26);
            this.panel7.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(393, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 16);
            this.label6.TabIndex = 102;
            this.label6.Text = "결제정보";
            // 
            // panel15
            // 
            this.panel15.Controls.Add(this.label13);
            this.panel15.Controls.Add(this.txtPaymentBank);
            this.panel15.Controls.Add(this.txtRemark);
            this.panel15.Controls.Add(this.cbPaymentDateStatus);
            this.panel15.Controls.Add(this.label5);
            this.panel15.Controls.Add(this.cbPaymentCurrency);
            this.panel15.Controls.Add(this.txtPaymentDate);
            this.panel15.Controls.Add(this.lbPayId);
            this.panel15.Controls.Add(this.btnPaymentDateCalendar);
            this.panel15.Controls.Add(this.label4);
            this.panel15.Controls.Add(this.txtPaymentAmount);
            this.panel15.Controls.Add(this.label2);
            this.panel15.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel15.Location = new System.Drawing.Point(0, 252);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(852, 108);
            this.panel15.TabIndex = 12;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(12, 43);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(75, 16);
            this.label13.TabIndex = 107;
            this.label13.Text = "결제은행";
            // 
            // txtPaymentBank
            // 
            this.txtPaymentBank.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPaymentBank.Location = new System.Drawing.Point(121, 38);
            this.txtPaymentBank.MaxLength = 10;
            this.txtPaymentBank.Name = "txtPaymentBank";
            this.txtPaymentBank.Size = new System.Drawing.Size(282, 26);
            this.txtPaymentBank.TabIndex = 106;
            this.txtPaymentBank.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(485, 10);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(355, 86);
            this.txtRemark.TabIndex = 6;
            this.txtRemark.Text = "";
            // 
            // cbPaymentDateStatus
            // 
            this.cbPaymentDateStatus.Font = new System.Drawing.Font("굴림", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbPaymentDateStatus.FormattingEnabled = true;
            this.cbPaymentDateStatus.Items.AddRange(new object[] {
            "확정",
            "확정(마감)",
            "결제완료"});
            this.cbPaymentDateStatus.Location = new System.Drawing.Point(121, 7);
            this.cbPaymentDateStatus.Name = "cbPaymentDateStatus";
            this.cbPaymentDateStatus.Size = new System.Drawing.Size(113, 25);
            this.cbPaymentDateStatus.TabIndex = 1;
            this.cbPaymentDateStatus.Text = "확정(마감)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(437, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 16);
            this.label5.TabIndex = 102;
            this.label5.Text = "비고";
            // 
            // cbPaymentCurrency
            // 
            this.cbPaymentCurrency.Enabled = false;
            this.cbPaymentCurrency.Font = new System.Drawing.Font("굴림", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbPaymentCurrency.FormattingEnabled = true;
            this.cbPaymentCurrency.Location = new System.Drawing.Point(121, 71);
            this.cbPaymentCurrency.Name = "cbPaymentCurrency";
            this.cbPaymentCurrency.Size = new System.Drawing.Size(64, 25);
            this.cbPaymentCurrency.TabIndex = 4;
            this.cbPaymentCurrency.Text = "USD";
            // 
            // txtPaymentDate
            // 
            this.txtPaymentDate.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPaymentDate.Location = new System.Drawing.Point(243, 7);
            this.txtPaymentDate.MaxLength = 10;
            this.txtPaymentDate.Name = "txtPaymentDate";
            this.txtPaymentDate.Size = new System.Drawing.Size(160, 26);
            this.txtPaymentDate.TabIndex = 2;
            // 
            // lbPayId
            // 
            this.lbPayId.AutoSize = true;
            this.lbPayId.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbPayId.Location = new System.Drawing.Point(438, 70);
            this.lbPayId.Name = "lbPayId";
            this.lbPayId.Size = new System.Drawing.Size(31, 12);
            this.lbPayId.TabIndex = 104;
            this.lbPayId.Text = "Null";
            this.lbPayId.Visible = false;
            // 
            // btnPaymentDateCalendar
            // 
            this.btnPaymentDateCalendar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPaymentDateCalendar.BackgroundImage")));
            this.btnPaymentDateCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPaymentDateCalendar.FlatAppearance.BorderSize = 0;
            this.btnPaymentDateCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPaymentDateCalendar.Location = new System.Drawing.Point(409, 8);
            this.btnPaymentDateCalendar.Name = "btnPaymentDateCalendar";
            this.btnPaymentDateCalendar.Size = new System.Drawing.Size(22, 23);
            this.btnPaymentDateCalendar.TabIndex = 3;
            this.btnPaymentDateCalendar.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(12, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 16);
            this.label4.TabIndex = 100;
            this.label4.Text = "결제금액";
            // 
            // txtPaymentAmount
            // 
            this.txtPaymentAmount.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPaymentAmount.Location = new System.Drawing.Point(191, 70);
            this.txtPaymentAmount.MaxLength = 10;
            this.txtPaymentAmount.Name = "txtPaymentAmount";
            this.txtPaymentAmount.Size = new System.Drawing.Size(212, 26);
            this.txtPaymentAmount.TabIndex = 5;
            this.txtPaymentAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "결제일자";
            // 
            // btnComfirm
            // 
            this.btnComfirm.BackColor = System.Drawing.SystemColors.Window;
            this.btnComfirm.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnComfirm.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnComfirm.ForeColor = System.Drawing.Color.Blue;
            this.btnComfirm.Location = new System.Drawing.Point(1, 0);
            this.btnComfirm.Name = "btnComfirm";
            this.btnComfirm.Size = new System.Drawing.Size(69, 35);
            this.btnComfirm.TabIndex = 13;
            this.btnComfirm.Text = "확정";
            this.btnComfirm.UseVisualStyleBackColor = false;
            this.btnComfirm.Click += new System.EventHandler(this.btnComfirm_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.Window;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.ForeColor = System.Drawing.Color.Red;
            this.btnDelete.Location = new System.Drawing.Point(619, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(69, 35);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnUpadate);
            this.panel1.Controls.Add(this.btnPayComplete);
            this.panel1.Controls.Add(this.btnDeadline);
            this.panel1.Controls.Add(this.btnDetail);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnComfirm);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 360);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(852, 35);
            this.panel1.TabIndex = 4;
            // 
            // btnUpadate
            // 
            this.btnUpadate.BackColor = System.Drawing.SystemColors.Window;
            this.btnUpadate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUpadate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUpadate.ForeColor = System.Drawing.Color.Black;
            this.btnUpadate.Location = new System.Drawing.Point(549, 0);
            this.btnUpadate.Name = "btnUpadate";
            this.btnUpadate.Size = new System.Drawing.Size(69, 35);
            this.btnUpadate.TabIndex = 18;
            this.btnUpadate.Text = "수정(A)";
            this.btnUpadate.UseVisualStyleBackColor = false;
            this.btnUpadate.Click += new System.EventHandler(this.btnUpadate_Click);
            // 
            // btnPayComplete
            // 
            this.btnPayComplete.BackColor = System.Drawing.SystemColors.Window;
            this.btnPayComplete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPayComplete.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPayComplete.ForeColor = System.Drawing.Color.Blue;
            this.btnPayComplete.Location = new System.Drawing.Point(154, 0);
            this.btnPayComplete.Name = "btnPayComplete";
            this.btnPayComplete.Size = new System.Drawing.Size(82, 35);
            this.btnPayComplete.TabIndex = 17;
            this.btnPayComplete.Text = "결제완료";
            this.btnPayComplete.UseVisualStyleBackColor = false;
            this.btnPayComplete.Click += new System.EventHandler(this.btnPayComplete_Click);
            // 
            // btnDeadline
            // 
            this.btnDeadline.BackColor = System.Drawing.SystemColors.Window;
            this.btnDeadline.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDeadline.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDeadline.ForeColor = System.Drawing.Color.Blue;
            this.btnDeadline.Location = new System.Drawing.Point(71, 0);
            this.btnDeadline.Name = "btnDeadline";
            this.btnDeadline.Size = new System.Drawing.Size(82, 35);
            this.btnDeadline.TabIndex = 16;
            this.btnDeadline.Text = "확정(마감)";
            this.btnDeadline.UseVisualStyleBackColor = false;
            this.btnDeadline.Click += new System.EventHandler(this.btnDeadline_Click);
            // 
            // btnDetail
            // 
            this.btnDetail.BackColor = System.Drawing.SystemColors.Window;
            this.btnDetail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDetail.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDetail.ForeColor = System.Drawing.Color.Black;
            this.btnDetail.Location = new System.Drawing.Point(689, 0);
            this.btnDetail.Name = "btnDetail";
            this.btnDetail.Size = new System.Drawing.Size(93, 35);
            this.btnDetail.TabIndex = 15;
            this.btnDetail.Text = "팬딩정보(W)";
            this.btnDetail.UseVisualStyleBackColor = false;
            this.btnDetail.Click += new System.EventHandler(this.btnDetail_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Window;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(783, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(69, 35);
            this.btnExit.TabIndex = 14;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // AdvancePaymentManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 395);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdvancePaymentManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "선결제관리";
            this.Load += new System.EventHandler(this.AdvancePaymentManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AdvancePaymentManager_KeyDown);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel15.ResumeLayout(false);
            this.panel15.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.ComboBox cbPaymentDateStatus;
        private System.Windows.Forms.Label lbPayId;
        private System.Windows.Forms.RichTextBox txtRemark;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnComfirm;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox txtPaymentDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnPaymentDateCalendar;
        private System.Windows.Forms.TextBox txtPaymentAmount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbPaymentCurrency;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnDetail;
        private System.Windows.Forms.Button btnPayComplete;
        private System.Windows.Forms.Button btnDeadline;
        private System.Windows.Forms.Button btnUpadate;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtAtono;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtUsance;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtLcno;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBlno;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtDivision;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtAgency;
        private System.Windows.Forms.Panel panel6;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProduct;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtPaymentBank;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label6;
    }
}