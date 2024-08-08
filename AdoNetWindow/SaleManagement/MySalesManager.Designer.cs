namespace AdoNetWindow.SaleManagement
{
    partial class MySalesManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MySalesManager));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.lbManager = new System.Windows.Forms.Label();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.cbExactly = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtEnddate = new System.Windows.Forms.TextBox();
            this.btnEnddateCalendar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSttdate = new System.Windows.Forms.TextBox();
            this.btnSttdateCalendar = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvSales = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPotential2 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPotential1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPm = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAm = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearching = new System.Windows.Forms.Button();
            this.company_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sub_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updatetime_detail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.div1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.am = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pm = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.div2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.is_sales = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.div3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_log = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_contents = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.edit_user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.div4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isPotential1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isPotential2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtManager);
            this.panel1.Controls.Add(this.lbManager);
            this.panel1.Controls.Add(this.txtCompany);
            this.panel1.Controls.Add(this.cbExactly);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtEnddate);
            this.panel1.Controls.Add(this.btnEnddateCalendar);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtSttdate);
            this.panel1.Controls.Add(this.btnSttdateCalendar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1151, 30);
            this.panel1.TabIndex = 0;
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(1022, 4);
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(117, 21);
            this.txtManager.TabIndex = 26;
            // 
            // lbManager
            // 
            this.lbManager.AutoSize = true;
            this.lbManager.Location = new System.Drawing.Point(951, 8);
            this.lbManager.Name = "lbManager";
            this.lbManager.Size = new System.Drawing.Size(65, 12);
            this.lbManager.TabIndex = 27;
            this.lbManager.Text = "영업담당자";
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(386, 4);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(217, 21);
            this.txtCompany.TabIndex = 11;
            // 
            // cbExactly
            // 
            this.cbExactly.AutoSize = true;
            this.cbExactly.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbExactly.Location = new System.Drawing.Point(609, 7);
            this.cbExactly.Name = "cbExactly";
            this.cbExactly.Size = new System.Drawing.Size(60, 16);
            this.cbExactly.TabIndex = 12;
            this.cbExactly.Text = "정확히";
            this.cbExactly.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(327, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "거래처명";
            // 
            // txtEnddate
            // 
            this.txtEnddate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtEnddate.Location = new System.Drawing.Point(194, 4);
            this.txtEnddate.Name = "txtEnddate";
            this.txtEnddate.Size = new System.Drawing.Size(81, 21);
            this.txtEnddate.TabIndex = 6;
            this.txtEnddate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSttdate_KeyDown);
            // 
            // btnEnddateCalendar
            // 
            this.btnEnddateCalendar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEnddateCalendar.BackgroundImage")));
            this.btnEnddateCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEnddateCalendar.FlatAppearance.BorderSize = 0;
            this.btnEnddateCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnddateCalendar.Location = new System.Drawing.Point(278, 3);
            this.btnEnddateCalendar.Name = "btnEnddateCalendar";
            this.btnEnddateCalendar.Size = new System.Drawing.Size(22, 23);
            this.btnEnddateCalendar.TabIndex = 7;
            this.btnEnddateCalendar.UseVisualStyleBackColor = true;
            this.btnEnddateCalendar.Click += new System.EventHandler(this.btnEnddateCalendar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(174, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "~";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "검색기간";
            // 
            // txtSttdate
            // 
            this.txtSttdate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSttdate.Location = new System.Drawing.Point(62, 4);
            this.txtSttdate.Name = "txtSttdate";
            this.txtSttdate.Size = new System.Drawing.Size(81, 21);
            this.txtSttdate.TabIndex = 2;
            this.txtSttdate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSttdate_KeyDown);
            // 
            // btnSttdateCalendar
            // 
            this.btnSttdateCalendar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSttdateCalendar.BackgroundImage")));
            this.btnSttdateCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSttdateCalendar.FlatAppearance.BorderSize = 0;
            this.btnSttdateCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSttdateCalendar.Location = new System.Drawing.Point(146, 3);
            this.btnSttdateCalendar.Name = "btnSttdateCalendar";
            this.btnSttdateCalendar.Size = new System.Drawing.Size(22, 23);
            this.btnSttdateCalendar.TabIndex = 3;
            this.btnSttdateCalendar.UseVisualStyleBackColor = true;
            this.btnSttdateCalendar.Click += new System.EventHandler(this.btnSttdateCalendar_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.dgvSales);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1151, 594);
            this.panel2.TabIndex = 1;
            // 
            // dgvSales
            // 
            this.dgvSales.AllowUserToAddRows = false;
            this.dgvSales.AllowUserToDeleteRows = false;
            this.dgvSales.AllowUserToResizeColumns = false;
            this.dgvSales.AllowUserToResizeRows = false;
            this.dgvSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSales.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.company_id,
            this.sub_id,
            this.chk,
            this.company,
            this.updatetime,
            this.updatetime_detail,
            this.div1,
            this.am,
            this.pm,
            this.div2,
            this.is_sales,
            this.div3,
            this.sale_log,
            this.sale_contents,
            this.sale_remark,
            this.edit_user,
            this.div4,
            this.isPotential1,
            this.isPotential2});
            this.dgvSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSales.EnableHeadersVisualStyles = false;
            this.dgvSales.Location = new System.Drawing.Point(0, 0);
            this.dgvSales.Name = "dgvSales";
            this.dgvSales.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvSales.RowTemplate.Height = 23;
            this.dgvSales.Size = new System.Drawing.Size(1149, 571);
            this.dgvSales.TabIndex = 0;
            this.dgvSales.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSales_CellContentClick);
            this.dgvSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSales_CellValueChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.txtPotential2);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.txtPotential1);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Controls.Add(this.txtPm);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.txtAm);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.txtTotal);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 571);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1149, 21);
            this.panel4.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(492, 4);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(317, 12);
            this.label10.TabIndex = 10;
            this.label10.Text = "* \'영업\' 구분으로 체크된 항목만 집계에 포함됩니다!";
            // 
            // txtPotential2
            // 
            this.txtPotential2.Location = new System.Drawing.Point(432, 0);
            this.txtPotential2.Name = "txtPotential2";
            this.txtPotential2.Size = new System.Drawing.Size(54, 21);
            this.txtPotential2.TabIndex = 9;
            this.txtPotential2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(388, 4);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 12);
            this.label9.TabIndex = 8;
            this.label9.Text = "잠재2";
            // 
            // txtPotential1
            // 
            this.txtPotential1.Location = new System.Drawing.Point(328, 0);
            this.txtPotential1.Name = "txtPotential1";
            this.txtPotential1.Size = new System.Drawing.Size(54, 21);
            this.txtPotential1.TabIndex = 7;
            this.txtPotential1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(284, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 12);
            this.label8.TabIndex = 6;
            this.label8.Text = "잠재1";
            // 
            // txtPm
            // 
            this.txtPm.Location = new System.Drawing.Point(226, 0);
            this.txtPm.Name = "txtPm";
            this.txtPm.Size = new System.Drawing.Size(54, 21);
            this.txtPm.TabIndex = 5;
            this.txtPm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(191, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 12);
            this.label7.TabIndex = 4;
            this.label7.Text = "오후";
            // 
            // txtAm
            // 
            this.txtAm.Location = new System.Drawing.Point(134, 0);
            this.txtAm.Name = "txtAm";
            this.txtAm.Size = new System.Drawing.Size(54, 21);
            this.txtAm.TabIndex = 3;
            this.txtAm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(99, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "오전";
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(40, 0);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(54, 21);
            this.txtTotal.TabIndex = 1;
            this.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(5, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "합계";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnInsert);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnSearching);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 624);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1151, 44);
            this.panel3.TabIndex = 2;
            // 
            // btnInsert
            // 
            this.btnInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInsert.ForeColor = System.Drawing.Color.Blue;
            this.btnInsert.Location = new System.Drawing.Point(77, 3);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(66, 37);
            this.btnInsert.TabIndex = 1;
            this.btnInsert.Text = "수정(A)";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(149, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.ForeColor = System.Drawing.Color.Black;
            this.btnSearching.Location = new System.Drawing.Point(5, 3);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(66, 37);
            this.btnSearching.TabIndex = 0;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // company_id
            // 
            this.company_id.HeaderText = "id";
            this.company_id.Name = "company_id";
            this.company_id.Visible = false;
            // 
            // sub_id
            // 
            this.sub_id.HeaderText = "sub_id";
            this.sub_id.Name = "sub_id";
            this.sub_id.Visible = false;
            // 
            // chk
            // 
            this.chk.HeaderText = "";
            this.chk.Name = "chk";
            this.chk.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chk.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.chk.Width = 30;
            // 
            // company
            // 
            this.company.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.company.HeaderText = "거래처명";
            this.company.Name = "company";
            // 
            // updatetime
            // 
            this.updatetime.HeaderText = "영업일자";
            this.updatetime.Name = "updatetime";
            this.updatetime.Width = 80;
            // 
            // updatetime_detail
            // 
            this.updatetime_detail.HeaderText = "영업일자";
            this.updatetime_detail.Name = "updatetime_detail";
            this.updatetime_detail.Visible = false;
            // 
            // div1
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Silver;
            this.div1.DefaultCellStyle = dataGridViewCellStyle5;
            this.div1.HeaderText = "";
            this.div1.Name = "div1";
            this.div1.Width = 10;
            // 
            // am
            // 
            this.am.HeaderText = "am";
            this.am.Name = "am";
            this.am.Width = 30;
            // 
            // pm
            // 
            this.pm.HeaderText = "pm";
            this.pm.Name = "pm";
            this.pm.Width = 30;
            // 
            // div2
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Silver;
            this.div2.DefaultCellStyle = dataGridViewCellStyle6;
            this.div2.HeaderText = "";
            this.div2.Name = "div2";
            this.div2.Width = 10;
            // 
            // is_sales
            // 
            this.is_sales.HeaderText = "영업";
            this.is_sales.Name = "is_sales";
            this.is_sales.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.is_sales.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.is_sales.Width = 50;
            // 
            // div3
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Silver;
            this.div3.DefaultCellStyle = dataGridViewCellStyle7;
            this.div3.HeaderText = "";
            this.div3.Name = "div3";
            this.div3.Width = 10;
            // 
            // sale_log
            // 
            this.sale_log.HeaderText = "로그";
            this.sale_log.Name = "sale_log";
            // 
            // sale_contents
            // 
            this.sale_contents.HeaderText = "영업내용";
            this.sale_contents.Name = "sale_contents";
            // 
            // sale_remark
            // 
            this.sale_remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sale_remark.HeaderText = "비고";
            this.sale_remark.Name = "sale_remark";
            // 
            // edit_user
            // 
            this.edit_user.HeaderText = "수정자";
            this.edit_user.Name = "edit_user";
            // 
            // div4
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.Silver;
            this.div4.DefaultCellStyle = dataGridViewCellStyle8;
            this.div4.HeaderText = "";
            this.div4.Name = "div4";
            this.div4.Width = 10;
            // 
            // isPotential1
            // 
            this.isPotential1.HeaderText = "잠재1";
            this.isPotential1.Name = "isPotential1";
            this.isPotential1.Width = 50;
            // 
            // isPotential2
            // 
            this.isPotential2.HeaderText = "잠재2";
            this.isPotential2.Name = "isPotential2";
            this.isPotential2.Width = 50;
            // 
            // MySalesManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1151, 668);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MySalesManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "영업내역";
            this.Load += new System.EventHandler(this.MySalesManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MySalesManager_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtEnddate;
        private System.Windows.Forms.Button btnEnddateCalendar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSttdate;
        private System.Windows.Forms.Button btnSttdateCalendar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.CheckBox cbExactly;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.Label lbManager;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvSales;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox txtPotential2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtPotential1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPm;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtAm;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridViewTextBoxColumn company_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn sub_id;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn company;
        private System.Windows.Forms.DataGridViewTextBoxColumn updatetime;
        private System.Windows.Forms.DataGridViewTextBoxColumn updatetime_detail;
        private System.Windows.Forms.DataGridViewTextBoxColumn div1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn am;
        private System.Windows.Forms.DataGridViewCheckBoxColumn pm;
        private System.Windows.Forms.DataGridViewTextBoxColumn div2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn is_sales;
        private System.Windows.Forms.DataGridViewTextBoxColumn div3;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_log;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_contents;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn edit_user;
        private System.Windows.Forms.DataGridViewTextBoxColumn div4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isPotential1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isPotential2;
    }
}