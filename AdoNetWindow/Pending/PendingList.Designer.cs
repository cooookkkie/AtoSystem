namespace AdoNetWindow
{
    partial class UnconfirmedPending
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnconfirmedPending));
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.PendingList = new System.Windows.Forms.DataGridView();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label20 = new System.Windows.Forms.Label();
            this.txtCount = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel9 = new System.Windows.Forms.Panel();
            this.btnArrivalSchedule = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cbCcStatus = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.cbDivision = new System.Windows.Forms.ComboBox();
            this.txtBoxWeight = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cbPayment = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbAgency = new System.Windows.Forms.ComboBox();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.cbUsance = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.txtSizes = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cbisStart = new System.Windows.Forms.CheckBox();
            this.txtImportNumber = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.panel11 = new System.Windows.Forms.Panel();
            this.label21 = new System.Windows.Forms.Label();
            this.cbSortType = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.nudEndYear = new System.Windows.Forms.NumericUpDown();
            this.nudSttYear = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAtoNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtContractNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtShipper = new System.Windows.Forms.TextBox();
            this.txtLcNo = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBlNo = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.rbDeleteCustom = new System.Windows.Forms.RadioButton();
            this.rbNotDeleteCustom = new System.Windows.Forms.RadioButton();
            this.rbTotalCustom = new System.Windows.Forms.RadioButton();
            this.panel10 = new System.Windows.Forms.Panel();
            this.nudFontsize = new System.Windows.Forms.NumericUpDown();
            this.btnPlus = new System.Windows.Forms.Button();
            this.btnMinus = new System.Windows.Forms.Button();
            this.label32 = new System.Windows.Forms.Label();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbComfirm = new System.Windows.Forms.RadioButton();
            this.rbCustoms = new System.Windows.Forms.RadioButton();
            this.rbAfterStock = new System.Windows.Forms.RadioButton();
            this.rbShipping = new System.Windows.Forms.RadioButton();
            this.rbContract = new System.Windows.Forms.RadioButton();
            this.cms.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PendingList)).BeginInit();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEndYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSttYear)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFontsize)).BeginInit();
            this.SuspendLayout();
            // 
            // cms
            // 
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
            this.cms.Name = "contextMenuStrip1";
            this.cms.Size = new System.Drawing.Size(82, 48);
            this.cms.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.cms_Closed);
            this.cms.Opened += new System.EventHandler(this.cms_Opened);
            this.cms.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cms_PreviewKeyDown);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(81, 22);
            this.toolStripMenuItem2.Text = "1";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(81, 22);
            this.toolStripMenuItem3.Text = "2";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.PendingList);
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 81);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1892, 782);
            this.panel2.TabIndex = 2;
            // 
            // PendingList
            // 
            this.PendingList.AllowUserToAddRows = false;
            this.PendingList.AllowUserToDeleteRows = false;
            this.PendingList.AllowUserToResizeRows = false;
            this.PendingList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PendingList.Location = new System.Drawing.Point(0, 0);
            this.PendingList.Name = "PendingList";
            this.PendingList.RowTemplate.Height = 23;
            this.PendingList.Size = new System.Drawing.Size(1892, 718);
            this.PendingList.TabIndex = 0;
            this.PendingList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PendingList_CellDoubleClick);
            this.PendingList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.PendingList_CellFormatting);
            this.PendingList.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.PendingList_CellMouseClick);
            this.PendingList.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.PendingList_CellPainting);
            this.PendingList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.PendingList_CellValueChanged);
            this.PendingList.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.PendingList_ColumnHeaderMouseClick);
            this.PendingList.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.PendingList_ColumnHeaderMouseDoubleClick);
            this.PendingList.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.PendingList_ColumnWidthChanged);
            this.PendingList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.PendingList_DataError);
            this.PendingList.RowHeightChanged += new System.Windows.Forms.DataGridViewRowEventHandler(this.PendingList_RowHeightChanged);
            this.PendingList.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.PendingList_RowPostPaint);
            this.PendingList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PendingList_MouseUp);
            this.PendingList.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.PendingList_PreviewKeyDown);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.panel8);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(0, 718);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1892, 26);
            this.panel7.TabIndex = 2;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label20);
            this.panel8.Controls.Add(this.txtCount);
            this.panel8.Controls.Add(this.txtPrice);
            this.panel8.Controls.Add(this.label18);
            this.panel8.Controls.Add(this.label19);
            this.panel8.Controls.Add(this.txtWeight);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel8.Location = new System.Drawing.Point(1307, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(583, 24);
            this.panel8.TabIndex = 4;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label20.Location = new System.Drawing.Point(411, 6);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(31, 12);
            this.label20.TabIndex = 5;
            this.label20.Text = "금액";
            // 
            // txtCount
            // 
            this.txtCount.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtCount.Location = new System.Drawing.Point(90, -1);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(136, 26);
            this.txtCount.TabIndex = 0;
            this.txtCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtPrice
            // 
            this.txtPrice.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPrice.Location = new System.Drawing.Point(448, -1);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(136, 26);
            this.txtPrice.TabIndex = 4;
            this.txtPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label18.Location = new System.Drawing.Point(53, 6);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(31, 12);
            this.label18.TabIndex = 1;
            this.label18.Text = "수량";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label19.Location = new System.Drawing.Point(232, 6);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(31, 12);
            this.label19.TabIndex = 3;
            this.label19.Text = "중량";
            // 
            // txtWeight
            // 
            this.txtWeight.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtWeight.Location = new System.Drawing.Point(269, -1);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(136, 26);
            this.txtWeight.TabIndex = 2;
            this.txtWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnSearch);
            this.panel6.Controls.Add(this.panel9);
            this.panel6.Controls.Add(this.btnArrivalSchedule);
            this.panel6.Controls.Add(this.btnExcel);
            this.panel6.Controls.Add(this.btnExit);
            this.panel6.Controls.Add(this.btnInsert);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 744);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1892, 38);
            this.panel6.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.SystemColors.Control;
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Location = new System.Drawing.Point(3, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(66, 34);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "검색(Q)";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panel9
            // 
            this.panel9.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel9.Location = new System.Drawing.Point(1580, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(312, 38);
            this.panel9.TabIndex = 5;
            // 
            // btnArrivalSchedule
            // 
            this.btnArrivalSchedule.BackColor = System.Drawing.SystemColors.Control;
            this.btnArrivalSchedule.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnArrivalSchedule.ForeColor = System.Drawing.Color.Black;
            this.btnArrivalSchedule.Location = new System.Drawing.Point(166, 2);
            this.btnArrivalSchedule.Name = "btnArrivalSchedule";
            this.btnArrivalSchedule.Size = new System.Drawing.Size(79, 34);
            this.btnArrivalSchedule.TabIndex = 2;
            this.btnArrivalSchedule.Text = "입항일정";
            this.btnArrivalSchedule.UseVisualStyleBackColor = false;
            this.btnArrivalSchedule.Click += new System.EventHandler(this.btnArrivalSchedule_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.BackColor = System.Drawing.SystemColors.Control;
            this.btnExcel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExcel.ForeColor = System.Drawing.Color.Black;
            this.btnExcel.Location = new System.Drawing.Point(322, 2);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(89, 34);
            this.btnExcel.TabIndex = 4;
            this.btnExcel.Text = "Excel 다운";
            this.btnExcel.UseVisualStyleBackColor = false;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Control;
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(248, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(71, 34);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.BackColor = System.Drawing.SystemColors.Control;
            this.btnInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInsert.ForeColor = System.Drawing.Color.Blue;
            this.btnInsert.Location = new System.Drawing.Point(72, 2);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(91, 34);
            this.btnInsert.TabIndex = 1;
            this.btnInsert.Text = "신규등록(A)";
            this.btnInsert.UseVisualStyleBackColor = false;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1892, 58);
            this.panel1.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.panel5.Controls.Add(this.cbCcStatus);
            this.panel5.Controls.Add(this.label17);
            this.panel5.Controls.Add(this.cbDivision);
            this.panel5.Controls.Add(this.txtBoxWeight);
            this.panel5.Controls.Add(this.label16);
            this.panel5.Controls.Add(this.cbPayment);
            this.panel5.Controls.Add(this.label15);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Controls.Add(this.cbAgency);
            this.panel5.Controls.Add(this.txtOrigin);
            this.panel5.Controls.Add(this.txtManager);
            this.panel5.Controls.Add(this.cbUsance);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.label11);
            this.panel5.Controls.Add(this.txtProduct);
            this.panel5.Controls.Add(this.txtSizes);
            this.panel5.Controls.Add(this.label14);
            this.panel5.Controls.Add(this.label10);
            this.panel5.Controls.Add(this.label12);
            this.panel5.Controls.Add(this.label13);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 29);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1892, 29);
            this.panel5.TabIndex = 28;
            // 
            // cbCcStatus
            // 
            this.cbCcStatus.FormattingEnabled = true;
            this.cbCcStatus.Items.AddRange(new object[] {
            "전체",
            "미통관",
            "통관",
            "확정"});
            this.cbCcStatus.Location = new System.Drawing.Point(1637, 4);
            this.cbCcStatus.Name = "cbCcStatus";
            this.cbCcStatus.Size = new System.Drawing.Size(72, 20);
            this.cbCcStatus.TabIndex = 36;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(1578, 9);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(53, 12);
            this.label17.TabIndex = 35;
            this.label17.Text = "통관여부";
            // 
            // cbDivision
            // 
            this.cbDivision.FormattingEnabled = true;
            this.cbDivision.Items.AddRange(new object[] {
            "전체",
            "아토",
            "에이티오",
            "동일",
            "수리미",
            "해금",
            "에스제이",
            "푸드마을",
            "에프원에프엔비",
            "동양섬유",
            "이안수산"});
            this.cbDivision.Location = new System.Drawing.Point(1500, 4);
            this.cbDivision.Name = "cbDivision";
            this.cbDivision.Size = new System.Drawing.Size(72, 20);
            this.cbDivision.TabIndex = 34;
            // 
            // txtBoxWeight
            // 
            this.txtBoxWeight.Location = new System.Drawing.Point(705, 4);
            this.txtBoxWeight.Name = "txtBoxWeight";
            this.txtBoxWeight.Size = new System.Drawing.Size(152, 21);
            this.txtBoxWeight.TabIndex = 3;
            this.txtBoxWeight.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtContractyear_KeyDown);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(646, 9);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 12);
            this.label16.TabIndex = 33;
            this.label16.Text = "박스중량";
            // 
            // cbPayment
            // 
            this.cbPayment.FormattingEnabled = true;
            this.cbPayment.Items.AddRange(new object[] {
            "",
            "미확정",
            "확정",
            "확정(마감)",
            "확정(LG)",
            "결제완료"});
            this.cbPayment.Location = new System.Drawing.Point(1124, 4);
            this.cbPayment.Name = "cbPayment";
            this.cbPayment.Size = new System.Drawing.Size(85, 20);
            this.cbPayment.TabIndex = 5;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(1070, 8);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 12);
            this.label15.TabIndex = 31;
            this.label15.Text = "결제여부";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "품목명";
            // 
            // cbAgency
            // 
            this.cbAgency.FormattingEnabled = true;
            this.cbAgency.Items.AddRange(new object[] {
            "전체",
            "아토",
            "에이티오",
            "동일",
            "수리미",
            "해금",
            "에스제이",
            "푸드마을",
            "에프원에프엔비",
            "동양섬유"});
            this.cbAgency.Location = new System.Drawing.Point(1387, 4);
            this.cbAgency.Name = "cbAgency";
            this.cbAgency.Size = new System.Drawing.Size(72, 20);
            this.cbAgency.TabIndex = 7;
            // 
            // txtOrigin
            // 
            this.txtOrigin.Location = new System.Drawing.Point(488, 4);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.Size = new System.Drawing.Size(152, 21);
            this.txtOrigin.TabIndex = 2;
            this.txtOrigin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtContractyear_KeyDown);
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(910, 4);
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(152, 21);
            this.txtManager.TabIndex = 4;
            this.txtManager.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtContractyear_KeyDown);
            // 
            // cbUsance
            // 
            this.cbUsance.FormattingEnabled = true;
            this.cbUsance.Items.AddRange(new object[] {
            "US",
            "AT",
            "T/T"});
            this.cbUsance.Location = new System.Drawing.Point(1274, 4);
            this.cbUsance.Name = "cbUsance";
            this.cbUsance.Size = new System.Drawing.Size(72, 20);
            this.cbUsance.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(233, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "사이즈";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(863, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 20;
            this.label11.Text = "담당자";
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(68, 4);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(159, 21);
            this.txtProduct.TabIndex = 0;
            this.txtProduct.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtContractyear_KeyDown);
            // 
            // txtSizes
            // 
            this.txtSizes.Location = new System.Drawing.Point(280, 4);
            this.txtSizes.Name = "txtSizes";
            this.txtSizes.Size = new System.Drawing.Size(152, 21);
            this.txtSizes.TabIndex = 1;
            this.txtSizes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtContractyear_KeyDown);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(1352, 8);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(29, 12);
            this.label14.TabIndex = 26;
            this.label14.Text = "대행";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(441, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 18;
            this.label10.Text = "원산지";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1215, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 22;
            this.label12.Text = "수입구분";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(1465, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 24;
            this.label13.Text = "화주";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel4.Controls.Add(this.cbisStart);
            this.panel4.Controls.Add(this.txtImportNumber);
            this.panel4.Controls.Add(this.label22);
            this.panel4.Controls.Add(this.panel11);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.nudEndYear);
            this.panel4.Controls.Add(this.nudSttYear);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.txtAtoNo);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.txtContractNo);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.txtShipper);
            this.panel4.Controls.Add(this.txtLcNo);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.txtBlNo);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1892, 29);
            this.panel4.TabIndex = 27;
            // 
            // cbisStart
            // 
            this.cbisStart.AutoSize = true;
            this.cbisStart.Location = new System.Drawing.Point(371, 7);
            this.cbisStart.Name = "cbisStart";
            this.cbisStart.Size = new System.Drawing.Size(76, 16);
            this.cbisStart.TabIndex = 33;
            this.cbisStart.Text = "으로 시작";
            this.cbisStart.UseVisualStyleBackColor = true;
            // 
            // txtImportNumber
            // 
            this.txtImportNumber.Location = new System.Drawing.Point(1490, 3);
            this.txtImportNumber.Name = "txtImportNumber";
            this.txtImportNumber.Size = new System.Drawing.Size(136, 21);
            this.txtImportNumber.TabIndex = 7;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(1407, 8);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(77, 12);
            this.label22.TabIndex = 32;
            this.label22.Text = "수입신고번호";
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.label21);
            this.panel11.Controls.Add(this.cbSortType);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel11.Location = new System.Drawing.Point(1637, 0);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(255, 29);
            this.panel11.TabIndex = 30;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label21.Location = new System.Drawing.Point(94, 7);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(57, 12);
            this.label21.TabIndex = 52;
            this.label21.Text = "정렬기준";
            // 
            // cbSortType
            // 
            this.cbSortType.FormattingEnabled = true;
            this.cbSortType.Items.AddRange(new object[] {
            "등록순",
            "수정순",
            "AtoNo",
            "PI DATE",
            "ETD",
            "ETA",
            "창고입고일"});
            this.cbSortType.Location = new System.Drawing.Point(157, 3);
            this.cbSortType.Name = "cbSortType";
            this.cbSortType.Size = new System.Drawing.Size(96, 20);
            this.cbSortType.TabIndex = 51;
            this.cbSortType.Text = "등록순";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(123, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 12);
            this.label9.TabIndex = 29;
            this.label9.Text = "~";
            // 
            // nudEndYear
            // 
            this.nudEndYear.Location = new System.Drawing.Point(143, 4);
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
            this.nudEndYear.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtContractyear_KeyDown);
            // 
            // nudSttYear
            // 
            this.nudSttYear.Location = new System.Drawing.Point(71, 4);
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
            this.nudSttYear.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtContractyear_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "계약연도";
            // 
            // txtAtoNo
            // 
            this.txtAtoNo.Location = new System.Drawing.Point(261, 4);
            this.txtAtoNo.Name = "txtAtoNo";
            this.txtAtoNo.Size = new System.Drawing.Size(104, 21);
            this.txtAtoNo.TabIndex = 2;
            this.txtAtoNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtContractyear_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(201, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "ATO No.";
            // 
            // txtContractNo
            // 
            this.txtContractNo.Location = new System.Drawing.Point(563, 4);
            this.txtContractNo.Name = "txtContractNo";
            this.txtContractNo.Size = new System.Drawing.Size(171, 21);
            this.txtContractNo.TabIndex = 3;
            this.txtContractNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtContractyear_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(740, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "거래처";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(481, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "Contract No.";
            // 
            // txtShipper
            // 
            this.txtShipper.Location = new System.Drawing.Point(787, 4);
            this.txtShipper.Name = "txtShipper";
            this.txtShipper.Size = new System.Drawing.Size(199, 21);
            this.txtShipper.TabIndex = 4;
            this.txtShipper.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtContractyear_KeyDown);
            // 
            // txtLcNo
            // 
            this.txtLcNo.Location = new System.Drawing.Point(1048, 4);
            this.txtLcNo.Name = "txtLcNo";
            this.txtLcNo.Size = new System.Drawing.Size(148, 21);
            this.txtLcNo.TabIndex = 5;
            this.txtLcNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtContractyear_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(991, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "L/C No.";
            // 
            // txtBlNo
            // 
            this.txtBlNo.Location = new System.Drawing.Point(1248, 4);
            this.txtBlNo.Name = "txtBlNo";
            this.txtBlNo.Size = new System.Drawing.Size(153, 21);
            this.txtBlNo.TabIndex = 6;
            this.txtBlNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtContractyear_KeyDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1202, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 12);
            this.label8.TabIndex = 15;
            this.label8.Text = "Bl No.";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.panel3.Controls.Add(this.panel12);
            this.panel3.Controls.Add(this.panel10);
            this.panel3.Controls.Add(this.rbAll);
            this.panel3.Controls.Add(this.rbComfirm);
            this.panel3.Controls.Add(this.rbCustoms);
            this.panel3.Controls.Add(this.rbAfterStock);
            this.panel3.Controls.Add(this.rbShipping);
            this.panel3.Controls.Add(this.rbContract);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1892, 23);
            this.panel3.TabIndex = 3;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.rbDeleteCustom);
            this.panel12.Controls.Add(this.rbNotDeleteCustom);
            this.panel12.Controls.Add(this.rbTotalCustom);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel12.Location = new System.Drawing.Point(1447, 0);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(255, 23);
            this.panel12.TabIndex = 29;
            // 
            // rbDeleteCustom
            // 
            this.rbDeleteCustom.AutoSize = true;
            this.rbDeleteCustom.Location = new System.Drawing.Point(123, 4);
            this.rbDeleteCustom.Name = "rbDeleteCustom";
            this.rbDeleteCustom.Size = new System.Drawing.Size(56, 16);
            this.rbDeleteCustom.TabIndex = 2;
            this.rbDeleteCustom.Text = "취소O";
            this.rbDeleteCustom.UseVisualStyleBackColor = true;
            // 
            // rbNotDeleteCustom
            // 
            this.rbNotDeleteCustom.AutoSize = true;
            this.rbNotDeleteCustom.Checked = true;
            this.rbNotDeleteCustom.Location = new System.Drawing.Point(62, 4);
            this.rbNotDeleteCustom.Name = "rbNotDeleteCustom";
            this.rbNotDeleteCustom.Size = new System.Drawing.Size(55, 16);
            this.rbNotDeleteCustom.TabIndex = 1;
            this.rbNotDeleteCustom.TabStop = true;
            this.rbNotDeleteCustom.Text = "취소X";
            this.rbNotDeleteCustom.UseVisualStyleBackColor = true;
            // 
            // rbTotalCustom
            // 
            this.rbTotalCustom.AutoSize = true;
            this.rbTotalCustom.Location = new System.Drawing.Point(9, 4);
            this.rbTotalCustom.Name = "rbTotalCustom";
            this.rbTotalCustom.Size = new System.Drawing.Size(47, 16);
            this.rbTotalCustom.TabIndex = 0;
            this.rbTotalCustom.Text = "전체";
            this.rbTotalCustom.UseVisualStyleBackColor = true;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.nudFontsize);
            this.panel10.Controls.Add(this.btnPlus);
            this.panel10.Controls.Add(this.btnMinus);
            this.panel10.Controls.Add(this.label32);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel10.Location = new System.Drawing.Point(1702, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(190, 23);
            this.panel10.TabIndex = 28;
            // 
            // nudFontsize
            // 
            this.nudFontsize.Location = new System.Drawing.Point(146, 1);
            this.nudFontsize.Name = "nudFontsize";
            this.nudFontsize.Size = new System.Drawing.Size(43, 21);
            this.nudFontsize.TabIndex = 23;
            this.nudFontsize.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nudFontsize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nudFontsize_KeyDown);
            // 
            // btnPlus
            // 
            this.btnPlus.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPlus.Location = new System.Drawing.Point(94, -1);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(28, 24);
            this.btnPlus.TabIndex = 21;
            this.btnPlus.Text = "+";
            this.btnPlus.UseVisualStyleBackColor = true;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // btnMinus
            // 
            this.btnMinus.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMinus.Location = new System.Drawing.Point(119, -1);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(28, 24);
            this.btnMinus.TabIndex = 22;
            this.btnMinus.Text = "-";
            this.btnMinus.UseVisualStyleBackColor = true;
            this.btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(41, 5);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(53, 12);
            this.label32.TabIndex = 24;
            this.label32.Text = "확대축소";
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(477, 3);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(70, 16);
            this.rbAll.TabIndex = 27;
            this.rbAll.Text = "(F6)전체";
            this.rbAll.UseVisualStyleBackColor = true;
            this.rbAll.Click += new System.EventHandler(this.rbComfirm_Click);
            // 
            // rbComfirm
            // 
            this.rbComfirm.AutoSize = true;
            this.rbComfirm.Location = new System.Drawing.Point(401, 3);
            this.rbComfirm.Name = "rbComfirm";
            this.rbComfirm.Size = new System.Drawing.Size(70, 16);
            this.rbComfirm.TabIndex = 26;
            this.rbComfirm.Text = "(F5)확정";
            this.rbComfirm.UseVisualStyleBackColor = true;
            this.rbComfirm.Click += new System.EventHandler(this.rbComfirm_Click);
            // 
            // rbCustoms
            // 
            this.rbCustoms.AutoSize = true;
            this.rbCustoms.Location = new System.Drawing.Point(313, 3);
            this.rbCustoms.Name = "rbCustoms";
            this.rbCustoms.Size = new System.Drawing.Size(82, 16);
            this.rbCustoms.TabIndex = 25;
            this.rbCustoms.Text = "(F4)통관후";
            this.rbCustoms.UseVisualStyleBackColor = true;
            this.rbCustoms.Click += new System.EventHandler(this.rbComfirm_Click);
            // 
            // rbAfterStock
            // 
            this.rbAfterStock.AutoSize = true;
            this.rbAfterStock.Location = new System.Drawing.Point(225, 3);
            this.rbAfterStock.Name = "rbAfterStock";
            this.rbAfterStock.Size = new System.Drawing.Size(82, 16);
            this.rbAfterStock.TabIndex = 2;
            this.rbAfterStock.Text = "(F3)입고후";
            this.rbAfterStock.UseVisualStyleBackColor = true;
            this.rbAfterStock.Click += new System.EventHandler(this.rbComfirm_Click);
            // 
            // rbShipping
            // 
            this.rbShipping.AutoSize = true;
            this.rbShipping.Location = new System.Drawing.Point(91, 3);
            this.rbShipping.Name = "rbShipping";
            this.rbShipping.Size = new System.Drawing.Size(128, 16);
            this.rbShipping.TabIndex = 1;
            this.rbShipping.Text = "(F2)선적후(입고전)";
            this.rbShipping.UseVisualStyleBackColor = true;
            this.rbShipping.Click += new System.EventHandler(this.rbComfirm_Click);
            // 
            // rbContract
            // 
            this.rbContract.AutoSize = true;
            this.rbContract.Checked = true;
            this.rbContract.Location = new System.Drawing.Point(3, 3);
            this.rbContract.Name = "rbContract";
            this.rbContract.Size = new System.Drawing.Size(82, 16);
            this.rbContract.TabIndex = 0;
            this.rbContract.TabStop = true;
            this.rbContract.Text = "(F1)선적전";
            this.rbContract.UseVisualStyleBackColor = true;
            this.rbContract.Click += new System.EventHandler(this.rbComfirm_Click);
            // 
            // UnconfirmedPending
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1892, 863);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UnconfirmedPending";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "팬딩 조회";
            this.Load += new System.EventHandler(this.UnconfirmedPending_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UnconfirmedPending_KeyDown);
            this.cms.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PendingList)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEndYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSttYear)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFontsize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView PendingList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAtoNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtContractNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtShipper;
        private System.Windows.Forms.TextBox txtLcNo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBlNo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSizes;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cbUsance;
        private System.Windows.Forms.ComboBox cbAgency;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rbAfterStock;
        private System.Windows.Forms.RadioButton rbShipping;
        private System.Windows.Forms.RadioButton rbContract;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.NumericUpDown nudFontsize;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudEndYear;
        private System.Windows.Forms.NumericUpDown nudSttYear;
        private System.Windows.Forms.ComboBox cbPayment;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.TextBox txtBoxWeight;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.RadioButton rbComfirm;
        private System.Windows.Forms.RadioButton rbCustoms;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.ComboBox cbDivision;
        private System.Windows.Forms.Button btnArrivalSchedule;
        private System.Windows.Forms.ComboBox cbCcStatus;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtCount;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ComboBox cbSortType;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtImportNumber;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.RadioButton rbDeleteCustom;
        private System.Windows.Forms.RadioButton rbNotDeleteCustom;
        private System.Windows.Forms.RadioButton rbTotalCustom;
        private System.Windows.Forms.CheckBox cbisStart;
    }
}