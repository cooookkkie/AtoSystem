namespace AdoNetWindow.Pending
{
    partial class ArrivalSchedule
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArrivalSchedule));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtAgency = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtWarehouse = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCustomOfficer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.nudEndYear = new System.Windows.Forms.NumericUpDown();
            this.nudSttYear = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDivision = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBlNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtShippingCompany = new System.Windows.Forms.TextBox();
            this.txtBlStatus = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAtono = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvArrival = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnInOut = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.rbAfterQuarantine = new System.Windows.Forms.RadioButton();
            this.rbReceiptDocument = new System.Windows.Forms.RadioButton();
            this.rbNotyetDocument = new System.Windows.Forms.RadioButton();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ato_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bl_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.forwarder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bl_status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.products = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_cnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.custom_officer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehousing_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.agency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sanitary_certificate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quarantine_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.result_estimated_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.is_quarantine = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEndYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSttYear)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArrival)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtAgency);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txtWarehouse);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtCustomOfficer);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.nudEndYear);
            this.panel1.Controls.Add(this.nudSttYear);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtDivision);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtBlNo);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtShippingCompany);
            this.panel1.Controls.Add(this.txtBlStatus);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtAtono);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1521, 28);
            this.panel1.TabIndex = 0;
            // 
            // txtAgency
            // 
            this.txtAgency.Location = new System.Drawing.Point(1412, 3);
            this.txtAgency.Name = "txtAgency";
            this.txtAgency.Size = new System.Drawing.Size(105, 21);
            this.txtAgency.TabIndex = 48;
            this.txtAgency.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nudSttYear_KeyDown);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1380, 8);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 12);
            this.label10.TabIndex = 49;
            this.label10.Text = "대행";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtWarehouse
            // 
            this.txtWarehouse.Location = new System.Drawing.Point(1269, 3);
            this.txtWarehouse.Name = "txtWarehouse";
            this.txtWarehouse.Size = new System.Drawing.Size(105, 21);
            this.txtWarehouse.TabIndex = 46;
            this.txtWarehouse.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nudSttYear_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1237, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 47;
            this.label6.Text = "창고";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCustomOfficer
            // 
            this.txtCustomOfficer.Location = new System.Drawing.Point(1126, 3);
            this.txtCustomOfficer.Name = "txtCustomOfficer";
            this.txtCustomOfficer.Size = new System.Drawing.Size(105, 21);
            this.txtCustomOfficer.TabIndex = 44;
            this.txtCustomOfficer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nudSttYear_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1083, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 45;
            this.label4.Text = "관세사";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(112, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 12);
            this.label9.TabIndex = 43;
            this.label9.Text = "~";
            // 
            // nudEndYear
            // 
            this.nudEndYear.Location = new System.Drawing.Point(132, 4);
            this.nudEndYear.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.nudEndYear.Minimum = new decimal(new int[] {
            2015,
            0,
            0,
            0});
            this.nudEndYear.Name = "nudEndYear";
            this.nudEndYear.Size = new System.Drawing.Size(46, 21);
            this.nudEndYear.TabIndex = 1;
            this.nudEndYear.Value = new decimal(new int[] {
            2022,
            0,
            0,
            0});
            this.nudEndYear.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nudSttYear_KeyDown);
            // 
            // nudSttYear
            // 
            this.nudSttYear.Location = new System.Drawing.Point(62, 4);
            this.nudSttYear.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.nudSttYear.Minimum = new decimal(new int[] {
            2015,
            0,
            0,
            0});
            this.nudSttYear.Name = "nudSttYear";
            this.nudSttYear.Size = new System.Drawing.Size(46, 21);
            this.nudSttYear.TabIndex = 0;
            this.nudSttYear.Value = new decimal(new int[] {
            2015,
            0,
            0,
            0});
            this.nudSttYear.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nudSttYear_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 34;
            this.label1.Text = "계약연도";
            // 
            // txtDivision
            // 
            this.txtDivision.Location = new System.Drawing.Point(214, 3);
            this.txtDivision.Name = "txtDivision";
            this.txtDivision.Size = new System.Drawing.Size(105, 21);
            this.txtDivision.TabIndex = 2;
            this.txtDivision.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nudSttYear_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 36;
            this.label2.Text = "화주";
            // 
            // txtBlNo
            // 
            this.txtBlNo.Location = new System.Drawing.Point(539, 3);
            this.txtBlNo.Name = "txtBlNo";
            this.txtBlNo.Size = new System.Drawing.Size(171, 21);
            this.txtBlNo.TabIndex = 33;
            this.txtBlNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nudSttYear_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(716, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 39;
            this.label3.Text = "선사";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(489, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 12);
            this.label5.TabIndex = 40;
            this.label5.Text = "B/L No.";
            // 
            // txtShippingCompany
            // 
            this.txtShippingCompany.Location = new System.Drawing.Point(747, 3);
            this.txtShippingCompany.Name = "txtShippingCompany";
            this.txtShippingCompany.Size = new System.Drawing.Size(171, 21);
            this.txtShippingCompany.TabIndex = 35;
            this.txtShippingCompany.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nudSttYear_KeyDown);
            // 
            // txtBlStatus
            // 
            this.txtBlStatus.Location = new System.Drawing.Point(976, 3);
            this.txtBlStatus.Name = "txtBlStatus";
            this.txtBlStatus.Size = new System.Drawing.Size(105, 21);
            this.txtBlStatus.TabIndex = 37;
            this.txtBlStatus.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nudSttYear_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(924, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 12);
            this.label7.TabIndex = 41;
            this.label7.Text = "B/L상태";
            // 
            // txtAtono
            // 
            this.txtAtono.Location = new System.Drawing.Point(378, 3);
            this.txtAtono.Name = "txtAtono";
            this.txtAtono.Size = new System.Drawing.Size(105, 21);
            this.txtAtono.TabIndex = 3;
            this.txtAtono.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nudSttYear_KeyDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(325, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 42;
            this.label8.Text = "Ato No.";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvArrival);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 56);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1521, 571);
            this.panel2.TabIndex = 1;
            // 
            // dgvArrival
            // 
            this.dgvArrival.AllowUserToAddRows = false;
            this.dgvArrival.AllowUserToDeleteRows = false;
            this.dgvArrival.AllowUserToResizeRows = false;
            this.dgvArrival.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvArrival.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chk,
            this.id,
            this.division,
            this.ato_no,
            this.bl_no,
            this.forwarder,
            this.eta,
            this.bl_status,
            this.products,
            this.product_cnt,
            this.custom_officer,
            this.warehouse,
            this.warehousing_date,
            this.agency,
            this.sanitary_certificate,
            this.remark,
            this.quarantine_type,
            this.result_estimated_date,
            this.is_quarantine});
            this.dgvArrival.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvArrival.EnableHeadersVisualStyles = false;
            this.dgvArrival.Location = new System.Drawing.Point(0, 0);
            this.dgvArrival.Name = "dgvArrival";
            this.dgvArrival.RowTemplate.Height = 23;
            this.dgvArrival.Size = new System.Drawing.Size(1521, 571);
            this.dgvArrival.TabIndex = 0;
            this.dgvArrival.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvArrival_CellMouseClick);
            this.dgvArrival.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvArrival_CellPainting);
            this.dgvArrival.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArrival_CellValueChanged);
            this.dgvArrival.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvArrival_MouseUp);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnInOut);
            this.panel3.Controls.Add(this.btnUpdate);
            this.panel3.Controls.Add(this.btnSelect);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 627);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1521, 39);
            this.panel3.TabIndex = 2;
            // 
            // btnInOut
            // 
            this.btnInOut.BackColor = System.Drawing.SystemColors.Control;
            this.btnInOut.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInOut.ForeColor = System.Drawing.Color.Blue;
            this.btnInOut.Location = new System.Drawing.Point(227, 3);
            this.btnInOut.Name = "btnInOut";
            this.btnInOut.Size = new System.Drawing.Size(79, 34);
            this.btnInOut.TabIndex = 3;
            this.btnInOut.Text = "입고처리";
            this.btnInOut.UseVisualStyleBackColor = false;
            this.btnInOut.Visible = false;
            this.btnInOut.Click += new System.EventHandler(this.btnInOut_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.SystemColors.Control;
            this.btnUpdate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUpdate.ForeColor = System.Drawing.Color.Blue;
            this.btnUpdate.Location = new System.Drawing.Point(5, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(65, 34);
            this.btnUpdate.TabIndex = 0;
            this.btnUpdate.Text = "수정(A)";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.BackColor = System.Drawing.SystemColors.Control;
            this.btnSelect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelect.ForeColor = System.Drawing.Color.Black;
            this.btnSelect.Location = new System.Drawing.Point(76, 3);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(68, 34);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "검색(Q)";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Control;
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(150, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(71, 34);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.rbAfterQuarantine);
            this.panel4.Controls.Add(this.rbReceiptDocument);
            this.panel4.Controls.Add(this.rbNotyetDocument);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1521, 28);
            this.panel4.TabIndex = 3;
            // 
            // rbAfterQuarantine
            // 
            this.rbAfterQuarantine.AutoSize = true;
            this.rbAfterQuarantine.Location = new System.Drawing.Point(237, 6);
            this.rbAfterQuarantine.Name = "rbAfterQuarantine";
            this.rbAfterQuarantine.Size = new System.Drawing.Size(110, 16);
            this.rbAfterQuarantine.TabIndex = 2;
            this.rbAfterQuarantine.Text = "창고입고후 (F3)";
            this.rbAfterQuarantine.UseVisualStyleBackColor = true;
            this.rbAfterQuarantine.CheckedChanged += new System.EventHandler(this.rbAfterQuarantine_CheckedChanged);
            // 
            // rbReceiptDocument
            // 
            this.rbReceiptDocument.AutoSize = true;
            this.rbReceiptDocument.Location = new System.Drawing.Point(121, 6);
            this.rbReceiptDocument.Name = "rbReceiptDocument";
            this.rbReceiptDocument.Size = new System.Drawing.Size(110, 16);
            this.rbReceiptDocument.TabIndex = 1;
            this.rbReceiptDocument.Text = "입항반입후 (F2)";
            this.rbReceiptDocument.UseVisualStyleBackColor = true;
            this.rbReceiptDocument.CheckedChanged += new System.EventHandler(this.rbReceiptDocument_CheckedChanged);
            // 
            // rbNotyetDocument
            // 
            this.rbNotyetDocument.AutoSize = true;
            this.rbNotyetDocument.Checked = true;
            this.rbNotyetDocument.Location = new System.Drawing.Point(9, 6);
            this.rbNotyetDocument.Name = "rbNotyetDocument";
            this.rbNotyetDocument.Size = new System.Drawing.Size(110, 16);
            this.rbNotyetDocument.TabIndex = 0;
            this.rbNotyetDocument.TabStop = true;
            this.rbNotyetDocument.Text = "입항반입전 (F1)";
            this.rbNotyetDocument.UseVisualStyleBackColor = true;
            this.rbNotyetDocument.CheckedChanged += new System.EventHandler(this.rbNotyetDocument_CheckedChanged);
            // 
            // chk
            // 
            this.chk.FillWeight = 167.5127F;
            this.chk.HeaderText = "";
            this.chk.Name = "chk";
            this.chk.Width = 30;
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.Visible = false;
            // 
            // division
            // 
            this.division.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.division.FillWeight = 63.95833F;
            this.division.HeaderText = "화주";
            this.division.Name = "division";
            // 
            // ato_no
            // 
            this.ato_no.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ato_no.FillWeight = 63.95833F;
            this.ato_no.HeaderText = "Ato No.";
            this.ato_no.Name = "ato_no";
            // 
            // bl_no
            // 
            this.bl_no.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.bl_no.FillWeight = 79.9387F;
            this.bl_no.HeaderText = "B/L No.";
            this.bl_no.Name = "bl_no";
            // 
            // forwarder
            // 
            this.forwarder.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.forwarder.FillWeight = 81.74441F;
            this.forwarder.HeaderText = "선사";
            this.forwarder.Name = "forwarder";
            // 
            // eta
            // 
            this.eta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.eta.FillWeight = 63.95833F;
            this.eta.HeaderText = "입항일";
            this.eta.Name = "eta";
            // 
            // bl_status
            // 
            this.bl_status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.bl_status.FillWeight = 63.95833F;
            this.bl_status.HeaderText = "B/L상태";
            this.bl_status.Name = "bl_status";
            // 
            // products
            // 
            this.products.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.products.FillWeight = 195.7908F;
            this.products.HeaderText = "품명";
            this.products.Name = "products";
            // 
            // product_cnt
            // 
            this.product_cnt.HeaderText = "품목수";
            this.product_cnt.Name = "product_cnt";
            this.product_cnt.Width = 50;
            // 
            // custom_officer
            // 
            this.custom_officer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.custom_officer.FillWeight = 83.75411F;
            this.custom_officer.HeaderText = "관세사";
            this.custom_officer.Name = "custom_officer";
            // 
            // warehouse
            // 
            this.warehouse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.warehouse.FillWeight = 85.99088F;
            this.warehouse.HeaderText = "창고";
            this.warehouse.Name = "warehouse";
            // 
            // warehousing_date
            // 
            this.warehousing_date.HeaderText = "창고반입일";
            this.warehousing_date.Name = "warehousing_date";
            this.warehousing_date.Width = 70;
            // 
            // agency
            // 
            this.agency.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.agency.FillWeight = 63.95833F;
            this.agency.HeaderText = "대행";
            this.agency.Name = "agency";
            // 
            // sanitary_certificate
            // 
            this.sanitary_certificate.HeaderText = "HC,CO";
            this.sanitary_certificate.Name = "sanitary_certificate";
            this.sanitary_certificate.Width = 60;
            // 
            // remark
            // 
            this.remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.remark.FillWeight = 185.4768F;
            this.remark.HeaderText = "비고";
            this.remark.Name = "remark";
            // 
            // quarantine_type
            // 
            this.quarantine_type.HeaderText = "검역종류";
            this.quarantine_type.Name = "quarantine_type";
            this.quarantine_type.Width = 70;
            // 
            // result_estimated_date
            // 
            this.result_estimated_date.HeaderText = "결과예상일자";
            this.result_estimated_date.Name = "result_estimated_date";
            this.result_estimated_date.Width = 70;
            // 
            // is_quarantine
            // 
            this.is_quarantine.HeaderText = "is_quarantine";
            this.is_quarantine.Name = "is_quarantine";
            this.is_quarantine.Visible = false;
            // 
            // ArrivalSchedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1521, 666);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "ArrivalSchedule";
            this.Text = "입항일정";
            this.Load += new System.EventHandler(this.ArrivalSchedule_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ArrivalSchedule_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEndYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSttYear)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvArrival)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExit;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvArrival;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudEndYear;
        private System.Windows.Forms.NumericUpDown nudSttYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDivision;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBlNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtShippingCompany;
        private System.Windows.Forms.TextBox txtBlStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtAtono;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtAgency;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtWarehouse;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCustomOfficer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton rbReceiptDocument;
        private System.Windows.Forms.RadioButton rbNotyetDocument;
        private System.Windows.Forms.Button btnInOut;
        private System.Windows.Forms.RadioButton rbAfterQuarantine;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn division;
        private System.Windows.Forms.DataGridViewTextBoxColumn ato_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn bl_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn forwarder;
        private System.Windows.Forms.DataGridViewTextBoxColumn eta;
        private System.Windows.Forms.DataGridViewTextBoxColumn bl_status;
        private System.Windows.Forms.DataGridViewTextBoxColumn products;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_cnt;
        private System.Windows.Forms.DataGridViewTextBoxColumn custom_officer;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehouse;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehousing_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn agency;
        private System.Windows.Forms.DataGridViewTextBoxColumn sanitary_certificate;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn quarantine_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn result_estimated_date;
        private System.Windows.Forms.DataGridViewCheckBoxColumn is_quarantine;
    }
}