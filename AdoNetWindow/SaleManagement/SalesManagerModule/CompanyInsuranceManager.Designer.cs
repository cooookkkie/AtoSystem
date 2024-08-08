namespace AdoNetWindow.SaleManagement.SalesManagerModule
{
    partial class CompanyInsuranceManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompanyInsuranceManager));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbExactly = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cbOnlyInsurance = new System.Windows.Forms.CheckBox();
            this.cbLimitCount = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCeo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCorporationNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvCompany = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.company_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.registration_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.corporation_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ceo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kc_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.insurance_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.insurance_sttdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.insurance_enddate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.left_days = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnSearching = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbExactly);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.txtCeo);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtCorporationNumber);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtCompany);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1129, 34);
            this.panel1.TabIndex = 0;
            // 
            // cbExactly
            // 
            this.cbExactly.AutoSize = true;
            this.cbExactly.Location = new System.Drawing.Point(240, 11);
            this.cbExactly.Name = "cbExactly";
            this.cbExactly.Size = new System.Drawing.Size(60, 16);
            this.cbExactly.TabIndex = 9;
            this.cbExactly.Text = "정확히";
            this.cbExactly.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.cbOnlyInsurance);
            this.panel4.Controls.Add(this.cbLimitCount);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(866, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(263, 34);
            this.panel4.TabIndex = 8;
            // 
            // cbOnlyInsurance
            // 
            this.cbOnlyInsurance.AutoSize = true;
            this.cbOnlyInsurance.Location = new System.Drawing.Point(13, 10);
            this.cbOnlyInsurance.Name = "cbOnlyInsurance";
            this.cbOnlyInsurance.Size = new System.Drawing.Size(112, 16);
            this.cbOnlyInsurance.TabIndex = 10;
            this.cbOnlyInsurance.Text = "임박 보험내역만";
            this.cbOnlyInsurance.UseVisualStyleBackColor = true;
            // 
            // cbLimitCount
            // 
            this.cbLimitCount.FormattingEnabled = true;
            this.cbLimitCount.Items.AddRange(new object[] {
            "100",
            "500",
            "1000",
            "2000",
            "5000"});
            this.cbLimitCount.Location = new System.Drawing.Point(193, 7);
            this.cbLimitCount.Name = "cbLimitCount";
            this.cbLimitCount.Size = new System.Drawing.Size(66, 20);
            this.cbLimitCount.TabIndex = 6;
            this.cbLimitCount.Text = "100";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(146, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "출력수";
            // 
            // txtCeo
            // 
            this.txtCeo.Location = new System.Drawing.Point(623, 7);
            this.txtCeo.Name = "txtCeo";
            this.txtCeo.Size = new System.Drawing.Size(77, 21);
            this.txtCeo.TabIndex = 5;
            this.txtCeo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompany_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(576, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "대표자";
            // 
            // txtCorporationNumber
            // 
            this.txtCorporationNumber.Location = new System.Drawing.Point(407, 7);
            this.txtCorporationNumber.Name = "txtCorporationNumber";
            this.txtCorporationNumber.Size = new System.Drawing.Size(163, 21);
            this.txtCorporationNumber.TabIndex = 3;
            this.txtCorporationNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompany_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(306, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "사업자/법인번호";
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(71, 7);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(163, 21);
            this.txtCompany.TabIndex = 1;
            this.txtCompany.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompany_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "거래처명";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvCompany);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1129, 505);
            this.panel2.TabIndex = 1;
            // 
            // dgvCompany
            // 
            this.dgvCompany.AllowUserToAddRows = false;
            this.dgvCompany.AllowUserToDeleteRows = false;
            this.dgvCompany.AllowUserToResizeRows = false;
            this.dgvCompany.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCompany.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chk,
            this.company_id,
            this.company,
            this.registration_number,
            this.corporation_number,
            this.ceo,
            this.kc_number,
            this.insurance_amount,
            this.insurance_sttdate,
            this.insurance_enddate,
            this.left_days});
            this.dgvCompany.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCompany.EnableHeadersVisualStyles = false;
            this.dgvCompany.Location = new System.Drawing.Point(0, 0);
            this.dgvCompany.Name = "dgvCompany";
            this.dgvCompany.RowTemplate.Height = 23;
            this.dgvCompany.Size = new System.Drawing.Size(1129, 505);
            this.dgvCompany.TabIndex = 0;
            this.dgvCompany.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvCompany_CellPainting);
            this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            // 
            // chk
            // 
            this.chk.HeaderText = "";
            this.chk.Name = "chk";
            this.chk.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chk.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.chk.Width = 30;
            // 
            // company_id
            // 
            this.company_id.HeaderText = "company_id";
            this.company_id.Name = "company_id";
            this.company_id.Visible = false;
            // 
            // company
            // 
            this.company.HeaderText = "거래처명";
            this.company.Name = "company";
            this.company.Width = 200;
            // 
            // registration_number
            // 
            this.registration_number.HeaderText = "사업자번호";
            this.registration_number.Name = "registration_number";
            // 
            // corporation_number
            // 
            this.corporation_number.HeaderText = "법인번호";
            this.corporation_number.Name = "corporation_number";
            // 
            // ceo
            // 
            this.ceo.HeaderText = "대표자";
            this.ceo.Name = "ceo";
            this.ceo.Width = 50;
            // 
            // kc_number
            // 
            this.kc_number.HeaderText = "KC";
            this.kc_number.Name = "kc_number";
            // 
            // insurance_amount
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.insurance_amount.DefaultCellStyle = dataGridViewCellStyle1;
            this.insurance_amount.HeaderText = "보험금액";
            this.insurance_amount.Name = "insurance_amount";
            // 
            // insurance_sttdate
            // 
            this.insurance_sttdate.HeaderText = "시작일";
            this.insurance_sttdate.Name = "insurance_sttdate";
            this.insurance_sttdate.Width = 70;
            // 
            // insurance_enddate
            // 
            this.insurance_enddate.HeaderText = "종료일";
            this.insurance_enddate.Name = "insurance_enddate";
            this.insurance_enddate.Width = 70;
            // 
            // left_days
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.left_days.DefaultCellStyle = dataGridViewCellStyle2;
            this.left_days.HeaderText = "남은일";
            this.left_days.Name = "left_days";
            this.left_days.Width = 50;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnUpdate);
            this.panel3.Controls.Add(this.btnSearching);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 539);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1129, 42);
            this.panel3.TabIndex = 2;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUpdate.ForeColor = System.Drawing.Color.Blue;
            this.btnUpdate.Location = new System.Drawing.Point(72, 4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(66, 34);
            this.btnUpdate.TabIndex = 7;
            this.btnUpdate.Text = "수정(A)";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.ForeColor = System.Drawing.Color.Black;
            this.btnSearching.Location = new System.Drawing.Point(3, 4);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(66, 34);
            this.btnSearching.TabIndex = 5;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(141, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 34);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // CompanyInsuranceManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 581);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "CompanyInsuranceManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "거래처 보험관리";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CompanyInsuranceManager_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvCompany;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtCeo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCorporationNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox cbLimitCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbExactly;
        private System.Windows.Forms.CheckBox cbOnlyInsurance;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn company_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn company;
        private System.Windows.Forms.DataGridViewTextBoxColumn registration_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn corporation_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn ceo;
        private System.Windows.Forms.DataGridViewTextBoxColumn kc_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn insurance_amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn insurance_sttdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn insurance_enddate;
        private System.Windows.Forms.DataGridViewTextBoxColumn left_days;
    }
}