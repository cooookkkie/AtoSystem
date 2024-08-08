namespace AdoNetWindow.SaleManagement
{
    partial class SalesDashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SalesDashboard));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgvUsers = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.department = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.users_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel12 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.dgvSelectUsers = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.select_department = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_grade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_user_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel10 = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtUsesname = new System.Windows.Forms.TextBox();
            this.txtGrade = new System.Windows.Forms.TextBox();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.dgvSales = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.month = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_user_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel13 = new System.Windows.Forms.Panel();
            this.cbMonthRate = new System.Windows.Forms.CheckBox();
            this.cbBusinessCompany = new System.Windows.Forms.CheckBox();
            this.cbYear = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbEndYear = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbSttYear = new System.Windows.Forms.ComboBox();
            this.panel16 = new System.Windows.Forms.Panel();
            this.panel15 = new System.Windows.Forms.Panel();
            this.dgvUpDownRate = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel17 = new System.Windows.Forms.Panel();
            this.lbUsername = new System.Windows.Forms.Label();
            this.lbUserId = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbSttMonth = new System.Windows.Forms.ComboBox();
            this.cbSalesTerms = new System.Windows.Forms.ComboBox();
            this.txtTargetSalesAmount = new System.Windows.Forms.TextBox();
            this.cbEndMonth = new System.Windows.Forms.ComboBox();
            this.txtAvgSalesAmountMonth = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSalesAmountMonth = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel14 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.panel12.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectUsers)).BeginInit();
            this.panel10.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).BeginInit();
            this.panel7.SuspendLayout();
            this.panel13.SuspendLayout();
            this.panel16.SuspendLayout();
            this.panel15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUpDownRate)).BeginInit();
            this.panel17.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel9);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(290, 883);
            this.panel1.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.dgvUsers);
            this.panel5.Controls.Add(this.panel12);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 194);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(290, 689);
            this.panel5.TabIndex = 1;
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AllowUserToResizeRows = false;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.department,
            this.grade,
            this.user_name,
            this.users_id});
            this.dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUsers.EnableHeadersVisualStyles = false;
            this.dgvUsers.Location = new System.Drawing.Point(0, 25);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.RowTemplate.Height = 23;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(290, 664);
            this.dgvUsers.TabIndex = 0;
            this.dgvUsers.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvUsers_CellMouseDoubleClick);
            this.dgvUsers.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvUsers_MouseUp);
            // 
            // department
            // 
            this.department.FillWeight = 131.1057F;
            this.department.HeaderText = "부서";
            this.department.Name = "department";
            this.department.Width = 80;
            // 
            // grade
            // 
            this.grade.FillWeight = 121.014F;
            this.grade.HeaderText = "직급";
            this.grade.Name = "grade";
            this.grade.Width = 60;
            // 
            // user_name
            // 
            this.user_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.user_name.FillWeight = 113.7893F;
            this.user_name.HeaderText = "이름";
            this.user_name.Name = "user_name";
            // 
            // users_id
            // 
            this.users_id.HeaderText = "user_id";
            this.users_id.Name = "users_id";
            this.users_id.Visible = false;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.label5);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel12.Location = new System.Drawing.Point(0, 0);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(290, 25);
            this.panel12.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(12, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 16);
            this.label5.TabIndex = 1;
            this.label5.Text = "* 전체내역";
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.panel11);
            this.panel9.Controls.Add(this.panel10);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(0, 30);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(290, 164);
            this.panel9.TabIndex = 2;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.dgvSelectUsers);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel11.Location = new System.Drawing.Point(0, 25);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(290, 139);
            this.panel11.TabIndex = 1;
            // 
            // dgvSelectUsers
            // 
            this.dgvSelectUsers.AllowUserToAddRows = false;
            this.dgvSelectUsers.AllowUserToDeleteRows = false;
            this.dgvSelectUsers.AllowUserToResizeRows = false;
            this.dgvSelectUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelectUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chk,
            this.select_department,
            this.select_grade,
            this.select_name,
            this.select_user_id});
            this.dgvSelectUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSelectUsers.EnableHeadersVisualStyles = false;
            this.dgvSelectUsers.Location = new System.Drawing.Point(0, 0);
            this.dgvSelectUsers.Name = "dgvSelectUsers";
            this.dgvSelectUsers.RowHeadersVisible = false;
            this.dgvSelectUsers.RowTemplate.Height = 23;
            this.dgvSelectUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelectUsers.Size = new System.Drawing.Size(290, 139);
            this.dgvSelectUsers.TabIndex = 1;
            this.dgvSelectUsers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSelectUsers_CellClick);
            this.dgvSelectUsers.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvSelectUsers_MouseUp);
            // 
            // chk
            // 
            this.chk.FillWeight = 34.09091F;
            this.chk.HeaderText = "";
            this.chk.Name = "chk";
            this.chk.Visible = false;
            this.chk.Width = 30;
            // 
            // select_department
            // 
            this.select_department.FillWeight = 131.1057F;
            this.select_department.HeaderText = "부서";
            this.select_department.Name = "select_department";
            this.select_department.Width = 80;
            // 
            // select_grade
            // 
            this.select_grade.FillWeight = 121.014F;
            this.select_grade.HeaderText = "직급";
            this.select_grade.Name = "select_grade";
            this.select_grade.Width = 60;
            // 
            // select_name
            // 
            this.select_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.select_name.FillWeight = 113.7893F;
            this.select_name.HeaderText = "이름";
            this.select_name.Name = "select_name";
            // 
            // select_user_id
            // 
            this.select_user_id.HeaderText = "user_id";
            this.select_user_id.Name = "select_user_id";
            this.select_user_id.Visible = false;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.btnRefresh);
            this.panel10.Controls.Add(this.label4);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel10.Location = new System.Drawing.Point(0, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(290, 25);
            this.panel10.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRefresh.ForeColor = System.Drawing.Color.Blue;
            this.btnRefresh.Location = new System.Drawing.Point(204, 0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(86, 25);
            this.btnRefresh.TabIndex = 20;
            this.btnRefresh.Text = "초기화(F5)";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(12, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "* 선택내역";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.txtUsesname);
            this.panel4.Controls.Add(this.txtGrade);
            this.panel4.Controls.Add(this.txtDepartment);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(290, 30);
            this.panel4.TabIndex = 0;
            // 
            // txtUsesname
            // 
            this.txtUsesname.Location = new System.Drawing.Point(173, 6);
            this.txtUsesname.Name = "txtUsesname";
            this.txtUsesname.Size = new System.Drawing.Size(114, 21);
            this.txtUsesname.TabIndex = 3;
            this.txtUsesname.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDepartment_KeyDown);
            // 
            // txtGrade
            // 
            this.txtGrade.Location = new System.Drawing.Point(110, 6);
            this.txtGrade.Name = "txtGrade";
            this.txtGrade.Size = new System.Drawing.Size(62, 21);
            this.txtGrade.TabIndex = 2;
            this.txtGrade.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDepartment_KeyDown);
            // 
            // txtDepartment
            // 
            this.txtDepartment.Location = new System.Drawing.Point(30, 6);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(79, 21);
            this.txtDepartment.TabIndex = 1;
            this.txtDepartment.Text = "영업";
            this.txtDepartment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDepartment_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(11, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "*";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel16);
            this.panel2.Controls.Add(this.panel14);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(290, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1351, 883);
            this.panel2.TabIndex = 1;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel8);
            this.panel6.Controls.Add(this.panel7);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(821, 883);
            this.panel6.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.dgvSales);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(0, 30);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(821, 853);
            this.panel8.TabIndex = 1;
            // 
            // dgvSales
            // 
            this.dgvSales.AllowUserToAddRows = false;
            this.dgvSales.AllowUserToDeleteRows = false;
            this.dgvSales.AllowUserToResizeRows = false;
            this.dgvSales.ColumnHeadersHeight = 46;
            this.dgvSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSales.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.month,
            this.select_user_name,
            this.division});
            this.dgvSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSales.EnableHeadersVisualStyles = false;
            this.dgvSales.Location = new System.Drawing.Point(0, 0);
            this.dgvSales.Name = "dgvSales";
            this.dgvSales.RowHeadersWidth = 20;
            this.dgvSales.RowTemplate.Height = 30;
            this.dgvSales.Size = new System.Drawing.Size(821, 853);
            this.dgvSales.TabIndex = 0;
            this.dgvSales.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSales_CellFormatting);
            this.dgvSales.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvSales_CellPainting);
            this.dgvSales.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvSales_ColumnWidthChanged);
            this.dgvSales.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvSales_Scroll);
            this.dgvSales.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvSales_Paint);
            // 
            // month
            // 
            this.month.HeaderText = "월";
            this.month.Name = "month";
            this.month.Width = 40;
            // 
            // select_user_name
            // 
            this.select_user_name.HeaderText = "이름";
            this.select_user_name.Name = "select_user_name";
            this.select_user_name.Width = 80;
            // 
            // division
            // 
            this.division.HeaderText = "구분";
            this.division.Name = "division";
            this.division.Visible = false;
            this.division.Width = 50;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.panel13);
            this.panel7.Controls.Add(this.label3);
            this.panel7.Controls.Add(this.cbEndYear);
            this.panel7.Controls.Add(this.label2);
            this.panel7.Controls.Add(this.cbSttYear);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(821, 30);
            this.panel7.TabIndex = 0;
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.cbMonthRate);
            this.panel13.Controls.Add(this.cbBusinessCompany);
            this.panel13.Controls.Add(this.cbYear);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel13.Location = new System.Drawing.Point(489, 0);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(332, 30);
            this.panel13.TabIndex = 6;
            // 
            // cbMonthRate
            // 
            this.cbMonthRate.AutoSize = true;
            this.cbMonthRate.Location = new System.Drawing.Point(32, 8);
            this.cbMonthRate.Name = "cbMonthRate";
            this.cbMonthRate.Size = new System.Drawing.Size(95, 16);
            this.cbMonthRate.TabIndex = 1;
            this.cbMonthRate.Text = "전월증감(F1)";
            this.cbMonthRate.UseVisualStyleBackColor = true;
            this.cbMonthRate.CheckedChanged += new System.EventHandler(this.cbMonthRate_CheckedChanged);
            // 
            // cbBusinessCompany
            // 
            this.cbBusinessCompany.AutoSize = true;
            this.cbBusinessCompany.Location = new System.Drawing.Point(234, 8);
            this.cbBusinessCompany.Name = "cbBusinessCompany";
            this.cbBusinessCompany.Size = new System.Drawing.Size(95, 16);
            this.cbBusinessCompany.TabIndex = 5;
            this.cbBusinessCompany.Text = "거래처수(F3)";
            this.cbBusinessCompany.UseVisualStyleBackColor = true;
            this.cbBusinessCompany.CheckedChanged += new System.EventHandler(this.cbBusinessCompany_CheckedChanged);
            // 
            // cbYear
            // 
            this.cbYear.AutoSize = true;
            this.cbYear.Location = new System.Drawing.Point(133, 8);
            this.cbYear.Name = "cbYear";
            this.cbYear.Size = new System.Drawing.Size(95, 16);
            this.cbYear.TabIndex = 4;
            this.cbYear.Text = "전년증감(F2)";
            this.cbYear.UseVisualStyleBackColor = true;
            this.cbYear.CheckedChanged += new System.EventHandler(this.cbYear_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(162, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "~";
            // 
            // cbEndYear
            // 
            this.cbEndYear.FormattingEnabled = true;
            this.cbEndYear.Location = new System.Drawing.Point(188, 6);
            this.cbEndYear.Name = "cbEndYear";
            this.cbEndYear.Size = new System.Drawing.Size(58, 20);
            this.cbEndYear.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(6, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "* 검색연도";
            // 
            // cbSttYear
            // 
            this.cbSttYear.FormattingEnabled = true;
            this.cbSttYear.Location = new System.Drawing.Point(99, 6);
            this.cbSttYear.Name = "cbSttYear";
            this.cbSttYear.Size = new System.Drawing.Size(58, 20);
            this.cbSttYear.TabIndex = 0;
            // 
            // panel16
            // 
            this.panel16.Controls.Add(this.panel15);
            this.panel16.Controls.Add(this.panel17);
            this.panel16.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel16.Location = new System.Drawing.Point(821, 0);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(530, 883);
            this.panel16.TabIndex = 3;
            // 
            // panel15
            // 
            this.panel15.Controls.Add(this.dgvUpDownRate);
            this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel15.Location = new System.Drawing.Point(0, 179);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(530, 73);
            this.panel15.TabIndex = 2;
            // 
            // dgvUpDownRate
            // 
            this.dgvUpDownRate.AllowUserToAddRows = false;
            this.dgvUpDownRate.AllowUserToDeleteRows = false;
            this.dgvUpDownRate.AllowUserToResizeColumns = false;
            this.dgvUpDownRate.AllowUserToResizeRows = false;
            this.dgvUpDownRate.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUpDownRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUpDownRate.EnableHeadersVisualStyles = false;
            this.dgvUpDownRate.Location = new System.Drawing.Point(0, 0);
            this.dgvUpDownRate.Name = "dgvUpDownRate";
            this.dgvUpDownRate.RowHeadersVisible = false;
            this.dgvUpDownRate.RowTemplate.Height = 23;
            this.dgvUpDownRate.Size = new System.Drawing.Size(530, 73);
            this.dgvUpDownRate.TabIndex = 0;
            // 
            // panel17
            // 
            this.panel17.Controls.Add(this.lbUsername);
            this.panel17.Controls.Add(this.lbUserId);
            this.panel17.Controls.Add(this.label8);
            this.panel17.Controls.Add(this.cbSttMonth);
            this.panel17.Controls.Add(this.cbSalesTerms);
            this.panel17.Controls.Add(this.txtTargetSalesAmount);
            this.panel17.Controls.Add(this.cbEndMonth);
            this.panel17.Controls.Add(this.txtAvgSalesAmountMonth);
            this.panel17.Controls.Add(this.label12);
            this.panel17.Controls.Add(this.label9);
            this.panel17.Controls.Add(this.label6);
            this.panel17.Controls.Add(this.txtSalesAmountMonth);
            this.panel17.Controls.Add(this.label11);
            this.panel17.Controls.Add(this.label10);
            this.panel17.Controls.Add(this.label7);
            this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel17.Location = new System.Drawing.Point(0, 0);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(530, 179);
            this.panel17.TabIndex = 18;
            // 
            // lbUsername
            // 
            this.lbUsername.AutoSize = true;
            this.lbUsername.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbUsername.ForeColor = System.Drawing.Color.Blue;
            this.lbUsername.Location = new System.Drawing.Point(3, 10);
            this.lbUsername.Name = "lbUsername";
            this.lbUsername.Size = new System.Drawing.Size(58, 16);
            this.lbUsername.TabIndex = 2;
            this.lbUsername.Text = "홍길동";
            // 
            // lbUserId
            // 
            this.lbUserId.AutoSize = true;
            this.lbUserId.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbUserId.ForeColor = System.Drawing.Color.Blue;
            this.lbUserId.Location = new System.Drawing.Point(70, 9);
            this.lbUserId.Name = "lbUserId";
            this.lbUserId.Size = new System.Drawing.Size(58, 16);
            this.lbUserId.TabIndex = 17;
            this.lbUserId.Text = "홍길동";
            this.lbUserId.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(79, 98);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 12);
            this.label8.TabIndex = 9;
            this.label8.Text = "개월 평균 매출";
            // 
            // cbSttMonth
            // 
            this.cbSttMonth.FormattingEnabled = true;
            this.cbSttMonth.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.cbSttMonth.Location = new System.Drawing.Point(73, 141);
            this.cbSttMonth.Name = "cbSttMonth";
            this.cbSttMonth.Size = new System.Drawing.Size(33, 20);
            this.cbSttMonth.TabIndex = 10;
            this.cbSttMonth.Text = "1";
            this.cbSttMonth.TabIndexChanged += new System.EventHandler(this.cbEndMonth_TabIndexChanged);
            // 
            // cbSalesTerms
            // 
            this.cbSalesTerms.FormattingEnabled = true;
            this.cbSalesTerms.Items.AddRange(new object[] {
            "2",
            "3",
            "6",
            "12"});
            this.cbSalesTerms.Location = new System.Drawing.Point(40, 93);
            this.cbSalesTerms.Name = "cbSalesTerms";
            this.cbSalesTerms.Size = new System.Drawing.Size(33, 20);
            this.cbSalesTerms.TabIndex = 8;
            this.cbSalesTerms.Text = "3";
            this.cbSalesTerms.TabIndexChanged += new System.EventHandler(this.cbEndMonth_TabIndexChanged);
            // 
            // txtTargetSalesAmount
            // 
            this.txtTargetSalesAmount.ForeColor = System.Drawing.Color.Blue;
            this.txtTargetSalesAmount.Location = new System.Drawing.Point(376, 41);
            this.txtTargetSalesAmount.Name = "txtTargetSalesAmount";
            this.txtTargetSalesAmount.Size = new System.Drawing.Size(145, 21);
            this.txtTargetSalesAmount.TabIndex = 16;
            this.txtTargetSalesAmount.Text = "0";
            this.txtTargetSalesAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTargetSalesAmount.TextChanged += new System.EventHandler(this.txtTargetSalesAmount_TextChanged);
            this.txtTargetSalesAmount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTargetSalesAmount_KeyDown);
            // 
            // cbEndMonth
            // 
            this.cbEndMonth.FormattingEnabled = true;
            this.cbEndMonth.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.cbEndMonth.Location = new System.Drawing.Point(143, 141);
            this.cbEndMonth.Name = "cbEndMonth";
            this.cbEndMonth.Size = new System.Drawing.Size(33, 20);
            this.cbEndMonth.TabIndex = 11;
            this.cbEndMonth.Text = "1";
            this.cbEndMonth.TabIndexChanged += new System.EventHandler(this.cbEndMonth_TabIndexChanged);
            // 
            // txtAvgSalesAmountMonth
            // 
            this.txtAvgSalesAmountMonth.Location = new System.Drawing.Point(376, 95);
            this.txtAvgSalesAmountMonth.Name = "txtAvgSalesAmountMonth";
            this.txtAvgSalesAmountMonth.Size = new System.Drawing.Size(145, 21);
            this.txtAvgSalesAmountMonth.TabIndex = 7;
            this.txtAvgSalesAmountMonth.Text = "0";
            this.txtAvgSalesAmountMonth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(10, 44);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 12);
            this.label12.TabIndex = 15;
            this.label12.Text = "목표 매출금액";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(108, 145);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 12);
            this.label9.TabIndex = 12;
            this.label9.Text = "월 ~";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "최근 30일 매출";
            // 
            // txtSalesAmountMonth
            // 
            this.txtSalesAmountMonth.Location = new System.Drawing.Point(376, 68);
            this.txtSalesAmountMonth.Name = "txtSalesAmountMonth";
            this.txtSalesAmountMonth.Size = new System.Drawing.Size(145, 21);
            this.txtSalesAmountMonth.TabIndex = 6;
            this.txtSalesAmountMonth.Text = "0";
            this.txtSalesAmountMonth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(179, 145);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(113, 12);
            this.label11.TabIndex = 14;
            this.label11.Text = "월 연별 매출 증감율";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 145);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 13;
            this.label10.Text = "매출기간";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 12);
            this.label7.TabIndex = 4;
            this.label7.Text = "최근 ";
            // 
            // panel14
            // 
            this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel14.Location = new System.Drawing.Point(0, 0);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(1351, 883);
            this.panel14.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 883);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1641, 39);
            this.panel3.TabIndex = 2;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(72, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 20;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.ForeColor = System.Drawing.Color.Blue;
            this.btnSearch.Location = new System.Drawing.Point(3, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(66, 37);
            this.btnSearch.TabIndex = 19;
            this.btnSearch.Text = "검색(Q)";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // SalesDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1641, 922);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "SalesDashboard";
            this.Text = "매출내역 대시보드";
            this.Load += new System.EventHandler(this.SalesDashboard_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SalesDashboard_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectUsers)).EndInit();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.panel16.ResumeLayout(false);
            this.panel15.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUpDownRate)).EndInit();
            this.panel17.ResumeLayout(false);
            this.panel17.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel5;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvUsers;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtUsesname;
        private System.Windows.Forms.TextBox txtGrade;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel8;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvSales;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbEndYear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbSttYear;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.CheckBox cbYear;
        private System.Windows.Forms.CheckBox cbMonthRate;
        private System.Windows.Forms.CheckBox cbBusinessCompany;
        private System.Windows.Forms.DataGridViewTextBoxColumn month;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_user_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn division;
        private System.Windows.Forms.DataGridViewTextBoxColumn department;
        private System.Windows.Forms.DataGridViewTextBoxColumn grade;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn users_id;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel11;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvSelectUsers;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_department;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_grade;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_user_id;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel panel15;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvUpDownRate;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.TextBox txtTargetSalesAmount;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbEndMonth;
        private System.Windows.Forms.ComboBox cbSttMonth;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbSalesTerms;
        private System.Windows.Forms.TextBox txtAvgSalesAmountMonth;
        private System.Windows.Forms.TextBox txtSalesAmountMonth;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbUsername;
        private System.Windows.Forms.Label lbUserId;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Panel panel17;
    }
}