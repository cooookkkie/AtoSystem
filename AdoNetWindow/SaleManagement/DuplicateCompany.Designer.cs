namespace AdoNetWindow.SaleManagement
{
    partial class DuplicateCompany
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DuplicateCompany));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.dgvCompany = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.group_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ceo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.registration_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.distribution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.handling_item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.position = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.web = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.edit_user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.complete_duplicate = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.is_duplicate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.current_sale_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.txtCurrent = new System.Windows.Forms.TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.cbDivision = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAddCompany = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnDeleteDuplicate = new System.Windows.Forms.Button();
            this.btnSearching = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel7);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1750, 785);
            this.panel1.TabIndex = 0;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label3);
            this.panel7.Controls.Add(this.dgvCompany);
            this.panel7.Controls.Add(this.panel3);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 22);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1750, 763);
            this.panel7.TabIndex = 3;
            // 
            // dgvCompany
            // 
            this.dgvCompany.AllowUserToAddRows = false;
            this.dgvCompany.AllowUserToDeleteRows = false;
            this.dgvCompany.AllowUserToResizeRows = false;
            this.dgvCompany.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCompany.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.division,
            this.group_name,
            this.company,
            this.ceo,
            this.tel,
            this.fax,
            this.phone,
            this.registration_number,
            this.address,
            this.distribution,
            this.handling_item,
            this.manager,
            this.position,
            this.email,
            this.remark,
            this.web,
            this.edit_user,
            this.complete_duplicate,
            this.is_duplicate,
            this.current_sale_date});
            this.dgvCompany.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCompany.EnableHeadersVisualStyles = false;
            this.dgvCompany.Location = new System.Drawing.Point(0, 0);
            this.dgvCompany.Name = "dgvCompany";
            this.dgvCompany.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvCompany.RowTemplate.Height = 23;
            this.dgvCompany.Size = new System.Drawing.Size(1750, 743);
            this.dgvCompany.TabIndex = 2;
            this.dgvCompany.SelectionChanged += new System.EventHandler(this.dgvCompany_SelectionChanged);
            // 
            // division
            // 
            this.division.HeaderText = "구분";
            this.division.Name = "division";
            this.division.Visible = false;
            this.division.Width = 50;
            // 
            // group_name
            // 
            this.group_name.HeaderText = "그룹";
            this.group_name.Name = "group_name";
            this.group_name.Width = 80;
            // 
            // company
            // 
            this.company.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Info;
            this.company.DefaultCellStyle = dataGridViewCellStyle1;
            this.company.HeaderText = "거래처명";
            this.company.Name = "company";
            // 
            // ceo
            // 
            this.ceo.HeaderText = "대표자";
            this.ceo.Name = "ceo";
            this.ceo.Width = 50;
            // 
            // tel
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tel.DefaultCellStyle = dataGridViewCellStyle2;
            this.tel.HeaderText = "TEL";
            this.tel.Name = "tel";
            this.tel.Width = 80;
            // 
            // fax
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.fax.DefaultCellStyle = dataGridViewCellStyle3;
            this.fax.HeaderText = "FAX";
            this.fax.Name = "fax";
            this.fax.Width = 80;
            // 
            // phone
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.phone.DefaultCellStyle = dataGridViewCellStyle4;
            this.phone.HeaderText = "휴대폰";
            this.phone.Name = "phone";
            this.phone.Width = 80;
            // 
            // registration_number
            // 
            this.registration_number.HeaderText = "사업자번호";
            this.registration_number.Name = "registration_number";
            this.registration_number.Width = 80;
            // 
            // address
            // 
            this.address.HeaderText = "주소";
            this.address.Name = "address";
            // 
            // distribution
            // 
            this.distribution.HeaderText = "유통";
            this.distribution.Name = "distribution";
            this.distribution.Width = 80;
            // 
            // handling_item
            // 
            this.handling_item.HeaderText = "취급품목";
            this.handling_item.Name = "handling_item";
            // 
            // manager
            // 
            this.manager.HeaderText = "업체담당자";
            this.manager.Name = "manager";
            this.manager.Width = 50;
            // 
            // position
            // 
            this.position.HeaderText = "직책";
            this.position.Name = "position";
            this.position.Width = 50;
            // 
            // email
            // 
            this.email.HeaderText = "E-MAIL";
            this.email.Name = "email";
            // 
            // remark
            // 
            this.remark.HeaderText = "비고";
            this.remark.Name = "remark";
            this.remark.Width = 150;
            // 
            // web
            // 
            this.web.HeaderText = "웹사이트";
            this.web.Name = "web";
            // 
            // edit_user
            // 
            this.edit_user.HeaderText = "담당자";
            this.edit_user.Name = "edit_user";
            this.edit_user.Width = 50;
            // 
            // complete_duplicate
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle5.NullValue = false;
            this.complete_duplicate.DefaultCellStyle = dataGridViewCellStyle5;
            this.complete_duplicate.HeaderText = "";
            this.complete_duplicate.Name = "complete_duplicate";
            this.complete_duplicate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.complete_duplicate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.complete_duplicate.Width = 30;
            // 
            // is_duplicate
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Red;
            this.is_duplicate.DefaultCellStyle = dataGridViewCellStyle6;
            this.is_duplicate.HeaderText = "결과";
            this.is_duplicate.Name = "is_duplicate";
            this.is_duplicate.Width = 150;
            // 
            // current_sale_date
            // 
            this.current_sale_date.HeaderText = "최근매출일";
            this.current_sale_date.Name = "current_sale_date";
            this.current_sale_date.Width = 80;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 743);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1750, 20);
            this.panel3.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.txtTotal);
            this.panel4.Controls.Add(this.txtCurrent);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(1511, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(239, 20);
            this.panel4.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(20, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "레코드";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(148, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "/";
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(70, -1);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(73, 21);
            this.txtTotal.TabIndex = 0;
            this.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTotal_KeyDown);
            // 
            // txtCurrent
            // 
            this.txtCurrent.Location = new System.Drawing.Point(166, -1);
            this.txtCurrent.Name = "txtCurrent";
            this.txtCurrent.Size = new System.Drawing.Size(73, 21);
            this.txtCurrent.TabIndex = 1;
            this.txtCurrent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCurrent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCurrent_KeyDown);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.cbDivision);
            this.panel6.Controls.Add(this.label4);
            this.panel6.Controls.Add(this.label5);
            this.panel6.Controls.Add(this.txtGroupName);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1750, 22);
            this.panel6.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(188, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "* 거래처명은 필수입니다.";
            // 
            // cbDivision
            // 
            this.cbDivision.FormattingEnabled = true;
            this.cbDivision.Items.AddRange(new object[] {
            "공용DATA",
            "무작위DATA",
            "잠재1",
            "잠재2",
            "취급X",
            "팩스X",
            "폐업"});
            this.cbDivision.Location = new System.Drawing.Point(509, 4);
            this.cbDivision.Name = "cbDivision";
            this.cbDivision.Size = new System.Drawing.Size(94, 20);
            this.cbDivision.TabIndex = 11;
            this.cbDivision.Text = "공용DATA";
            this.cbDivision.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(474, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "구분";
            this.label4.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(609, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "그룹명";
            this.label5.Visible = false;
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(656, 3);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(217, 21);
            this.txtGroupName.TabIndex = 7;
            this.txtGroupName.Visible = false;
            this.txtGroupName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtGroupName_KeyDown);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnAddCompany);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.btnSearching);
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 785);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1750, 41);
            this.panel2.TabIndex = 1;
            // 
            // btnAddCompany
            // 
            this.btnAddCompany.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAddCompany.ForeColor = System.Drawing.Color.Blue;
            this.btnAddCompany.Location = new System.Drawing.Point(98, 1);
            this.btnAddCompany.Name = "btnAddCompany";
            this.btnAddCompany.Size = new System.Drawing.Size(66, 37);
            this.btnAddCompany.TabIndex = 16;
            this.btnAddCompany.Text = "등록(A)";
            this.btnAddCompany.UseVisualStyleBackColor = true;
            this.btnAddCompany.Click += new System.EventHandler(this.btnAddCompany_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnDeleteDuplicate);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(1448, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(300, 39);
            this.panel5.TabIndex = 15;
            // 
            // btnDeleteDuplicate
            // 
            this.btnDeleteDuplicate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDeleteDuplicate.ForeColor = System.Drawing.Color.Red;
            this.btnDeleteDuplicate.Location = new System.Drawing.Point(146, 0);
            this.btnDeleteDuplicate.Name = "btnDeleteDuplicate";
            this.btnDeleteDuplicate.Size = new System.Drawing.Size(155, 37);
            this.btnDeleteDuplicate.TabIndex = 14;
            this.btnDeleteDuplicate.Text = "중복 및 취급X 행 삭제";
            this.btnDeleteDuplicate.UseVisualStyleBackColor = true;
            this.btnDeleteDuplicate.Click += new System.EventHandler(this.btnDeleteDuplicate_Click);
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.ForeColor = System.Drawing.Color.Black;
            this.btnSearching.Location = new System.Drawing.Point(3, 0);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(89, 37);
            this.btnSearching.TabIndex = 0;
            this.btnSearching.Text = "중복검사(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(170, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // DuplicateCompany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1750, 826);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "DuplicateCompany";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "거래처 중복확인";
            this.Load += new System.EventHandler(this.AddBusinessCompany_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddBusinessCompany_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvCompany;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnDeleteDuplicate;
        private System.Windows.Forms.Button btnAddCompany;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.ComboBox cbDivision;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtCurrent;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn division;
        private System.Windows.Forms.DataGridViewTextBoxColumn group_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn company;
        private System.Windows.Forms.DataGridViewTextBoxColumn ceo;
        private System.Windows.Forms.DataGridViewTextBoxColumn tel;
        private System.Windows.Forms.DataGridViewTextBoxColumn fax;
        private System.Windows.Forms.DataGridViewTextBoxColumn phone;
        private System.Windows.Forms.DataGridViewTextBoxColumn registration_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn address;
        private System.Windows.Forms.DataGridViewTextBoxColumn distribution;
        private System.Windows.Forms.DataGridViewTextBoxColumn handling_item;
        private System.Windows.Forms.DataGridViewTextBoxColumn manager;
        private System.Windows.Forms.DataGridViewTextBoxColumn position;
        private System.Windows.Forms.DataGridViewTextBoxColumn email;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn web;
        private System.Windows.Forms.DataGridViewTextBoxColumn edit_user;
        private System.Windows.Forms.DataGridViewCheckBoxColumn complete_duplicate;
        private System.Windows.Forms.DataGridViewTextBoxColumn is_duplicate;
        private System.Windows.Forms.DataGridViewTextBoxColumn current_sale_date;
    }
}