namespace AdoNetWindow.OverseaManufacturingBusiness
{
    partial class HandlingProduct
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HandlingProduct));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnEndDateCalendar = new System.Windows.Forms.Button();
            this.btnSttDateCalendar = new System.Windows.Forms.Button();
            this.txtEnddate = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSttdate = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cbSortType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pname_kor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pname_eng = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_country = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.e_country = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.current_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lbCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
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
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1222, 33);
            this.panel1.TabIndex = 0;
            // 
            // btnEndDateCalendar
            // 
            this.btnEndDateCalendar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEndDateCalendar.BackgroundImage")));
            this.btnEndDateCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEndDateCalendar.FlatAppearance.BorderSize = 0;
            this.btnEndDateCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEndDateCalendar.Location = new System.Drawing.Point(259, 4);
            this.btnEndDateCalendar.Name = "btnEndDateCalendar";
            this.btnEndDateCalendar.Size = new System.Drawing.Size(22, 23);
            this.btnEndDateCalendar.TabIndex = 23;
            this.btnEndDateCalendar.UseVisualStyleBackColor = true;
            this.btnEndDateCalendar.Click += new System.EventHandler(this.btnSttDateCalendar_Click);
            // 
            // btnSttDateCalendar
            // 
            this.btnSttDateCalendar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSttDateCalendar.BackgroundImage")));
            this.btnSttDateCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSttDateCalendar.FlatAppearance.BorderSize = 0;
            this.btnSttDateCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSttDateCalendar.Location = new System.Drawing.Point(134, 4);
            this.btnSttDateCalendar.Name = "btnSttDateCalendar";
            this.btnSttDateCalendar.Size = new System.Drawing.Size(22, 23);
            this.btnSttDateCalendar.TabIndex = 21;
            this.btnSttDateCalendar.UseVisualStyleBackColor = true;
            this.btnSttDateCalendar.Click += new System.EventHandler(this.btnSttDateCalendar_Click);
            // 
            // txtEnddate
            // 
            this.txtEnddate.Location = new System.Drawing.Point(187, 6);
            this.txtEnddate.Name = "txtEnddate";
            this.txtEnddate.Size = new System.Drawing.Size(71, 21);
            this.txtEnddate.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(167, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 12);
            this.label9.TabIndex = 25;
            this.label9.Text = "~";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 24;
            this.label8.Text = "검색기간";
            // 
            // txtSttdate
            // 
            this.txtSttdate.Location = new System.Drawing.Point(62, 6);
            this.txtSttdate.Name = "txtSttdate";
            this.txtSttdate.Size = new System.Drawing.Size(71, 21);
            this.txtSttdate.TabIndex = 20;
            this.txtSttdate.Text = "2020-01-01";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.cbSortType);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(874, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(348, 33);
            this.panel4.TabIndex = 1;
            // 
            // cbSortType
            // 
            this.cbSortType.FormattingEnabled = true;
            this.cbSortType.Items.AddRange(new object[] {
            "거래처+품명(한글)+품명(영문)+최근일자",
            "거래처+품명(영문)+품명(한글)+최근일자",
            "거래처+제조국+수출국+품명(영문)+품명(한글)",
            "거래처+수출국+제조국+품명(영문)+품명(한글)",
            "품명(한글)+품명(영문)+거래처",
            "품명(영문)+품명(한글)+거래처"});
            this.cbSortType.Location = new System.Drawing.Point(62, 6);
            this.cbSortType.Name = "cbSortType";
            this.cbSortType.Size = new System.Drawing.Size(274, 20);
            this.cbSortType.TabIndex = 1;
            this.cbSortType.Text = "거래처+품명(한글)+품명(영문)+최근일자";
            this.cbSortType.SelectedIndexChanged += new System.EventHandler(this.cbSortType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "정렬기준";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvProduct);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1222, 590);
            this.panel2.TabIndex = 1;
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToResizeRows = false;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.company,
            this.pname_kor,
            this.pname_eng,
            this.product_type,
            this.m_country,
            this.e_country,
            this.qty,
            this.current_date});
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.EnableHeadersVisualStyles = false;
            this.dgvProduct.Location = new System.Drawing.Point(0, 0);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.Size = new System.Drawing.Size(1222, 590);
            this.dgvProduct.TabIndex = 0;
            this.dgvProduct.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProduct_CellMouseClick);
            this.dgvProduct.SelectionChanged += new System.EventHandler(this.dgvProduct_SelectionChanged);
            this.dgvProduct.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvProduct_MouseUp);
            // 
            // company
            // 
            this.company.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.company.HeaderText = "거래처";
            this.company.Name = "company";
            // 
            // pname_kor
            // 
            this.pname_kor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.pname_kor.HeaderText = "품명(한글)";
            this.pname_kor.Name = "pname_kor";
            // 
            // pname_eng
            // 
            this.pname_eng.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.pname_eng.HeaderText = "품명(영문)";
            this.pname_eng.Name = "pname_eng";
            // 
            // product_type
            // 
            this.product_type.HeaderText = "품목(유형)";
            this.product_type.Name = "product_type";
            // 
            // m_country
            // 
            this.m_country.HeaderText = "제조국";
            this.m_country.Name = "m_country";
            // 
            // e_country
            // 
            this.e_country.HeaderText = "수출국";
            this.e_country.Name = "e_country";
            // 
            // qty
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.qty.DefaultCellStyle = dataGridViewCellStyle2;
            this.qty.HeaderText = "수량";
            this.qty.Name = "qty";
            this.qty.Width = 70;
            // 
            // current_date
            // 
            this.current_date.HeaderText = "최근일자";
            this.current_date.Name = "current_date";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.btnPreview);
            this.panel3.Controls.Add(this.btnExcel);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 643);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1222, 44);
            this.panel3.TabIndex = 2;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(3, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(70, 37);
            this.btnSearch.TabIndex = 15;
            this.btnSearch.Text = "검색 (Q)";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPreview.Location = new System.Drawing.Point(225, 3);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(70, 37);
            this.btnPreview.TabIndex = 5;
            this.btnPreview.Text = "인쇄 (Ctrl+P)";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExcel.Location = new System.Drawing.Point(152, 3);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(67, 36);
            this.btnExcel.TabIndex = 14;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(79, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(67, 36);
            this.btnExit.TabIndex = 13;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 623);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1222, 20);
            this.panel5.TabIndex = 3;
            // 
            // lbCount
            // 
            this.lbCount.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbCount.Location = new System.Drawing.Point(99, 4);
            this.lbCount.Name = "lbCount";
            this.lbCount.Size = new System.Drawing.Size(60, 14);
            this.lbCount.TabIndex = 0;
            this.lbCount.Text = "00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "선택한 셀 합 :";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label2);
            this.panel6.Controls.Add(this.lbCount);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(1059, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(163, 20);
            this.panel6.TabIndex = 2;
            // 
            // HandlingProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1222, 687);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "HandlingProduct";
            this.Text = "취급품목";
            this.Load += new System.EventHandler(this.HandlingProduct_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandlingProduct_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProduct;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox cbSortType;
        private System.Windows.Forms.Label label1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnEndDateCalendar;
        private System.Windows.Forms.Button btnSttDateCalendar;
        private System.Windows.Forms.TextBox txtEnddate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSttdate;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn company;
        private System.Windows.Forms.DataGridViewTextBoxColumn pname_kor;
        private System.Windows.Forms.DataGridViewTextBoxColumn pname_eng;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_country;
        private System.Windows.Forms.DataGridViewTextBoxColumn e_country;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn current_date;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lbCount;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label2;
    }
}