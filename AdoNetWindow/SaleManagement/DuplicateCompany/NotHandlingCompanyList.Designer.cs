namespace AdoNetWindow.SaleManagement.DuplicateCompany
{
    partial class NotHandlingCompanyList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotHandlingCompanyList));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRecovery = new System.Windows.Forms.Button();
            this.btnIgnore = new System.Windows.Forms.Button();
            this.dgvCompany = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnRegister = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rowindex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.registration_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.other_phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isNotSendFax = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isIgnore = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isRecovery = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isDelete = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnRecovery);
            this.panel1.Controls.Add(this.btnIgnore);
            this.panel1.Controls.Add(this.dgvCompany);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1002, 491);
            this.panel1.TabIndex = 1;
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.ForeColor = System.Drawing.Color.Red;
            this.btnDelete.Location = new System.Drawing.Point(932, 1);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(49, 23);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRecovery
            // 
            this.btnRecovery.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRecovery.ForeColor = System.Drawing.Color.Black;
            this.btnRecovery.Location = new System.Drawing.Point(882, 1);
            this.btnRecovery.Name = "btnRecovery";
            this.btnRecovery.Size = new System.Drawing.Size(49, 23);
            this.btnRecovery.TabIndex = 4;
            this.btnRecovery.Text = "복구";
            this.btnRecovery.UseVisualStyleBackColor = true;
            this.btnRecovery.Click += new System.EventHandler(this.btnRecovery_Click);
            // 
            // btnIgnore
            // 
            this.btnIgnore.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnIgnore.ForeColor = System.Drawing.Color.Blue;
            this.btnIgnore.Location = new System.Drawing.Point(832, 1);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(49, 23);
            this.btnIgnore.TabIndex = 1;
            this.btnIgnore.Text = "무시";
            this.btnIgnore.UseVisualStyleBackColor = true;
            this.btnIgnore.Click += new System.EventHandler(this.btnIgnore_Click);
            // 
            // dgvCompany
            // 
            this.dgvCompany.AllowUserToAddRows = false;
            this.dgvCompany.AllowUserToResizeRows = false;
            this.dgvCompany.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCompany.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.rowindex,
            this.company,
            this.registration_number,
            this.tel,
            this.fax,
            this.phone,
            this.other_phone,
            this.isNotSendFax,
            this.isIgnore,
            this.isRecovery,
            this.isDelete});
            this.dgvCompany.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCompany.EnableHeadersVisualStyles = false;
            this.dgvCompany.Location = new System.Drawing.Point(0, 0);
            this.dgvCompany.Name = "dgvCompany";
            this.dgvCompany.RowHeadersWidth = 50;
            this.dgvCompany.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvCompany.RowTemplate.Height = 23;
            this.dgvCompany.Size = new System.Drawing.Size(1002, 491);
            this.dgvCompany.TabIndex = 3;
            this.dgvCompany.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellContentClick);
            this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnRegister);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 547);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1002, 45);
            this.panel2.TabIndex = 0;
            // 
            // btnRegister
            // 
            this.btnRegister.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRegister.ForeColor = System.Drawing.Color.Blue;
            this.btnRegister.Location = new System.Drawing.Point(3, 4);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(66, 37);
            this.btnRegister.TabIndex = 18;
            this.btnRegister.Text = "확인(Enter)";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1002, 56);
            this.panel3.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(436, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(544, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "※무시 : 무시하고 등록      ※복구 : 취급X 거래처를 복구      ※삭제 : 등록하지 않음";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(480, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "※ 취급X 거래처에 등록된 거래처 리스트입니다. 처리방식을 선택해주세요!";
            // 
            // rowindex
            // 
            this.rowindex.HeaderText = "rowindex";
            this.rowindex.Name = "rowindex";
            this.rowindex.Visible = false;
            this.rowindex.Width = 80;
            // 
            // company
            // 
            this.company.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Info;
            this.company.DefaultCellStyle = dataGridViewCellStyle1;
            this.company.HeaderText = "거래처명";
            this.company.Name = "company";
            this.company.Width = 281;
            // 
            // registration_number
            // 
            this.registration_number.HeaderText = "사업자번호";
            this.registration_number.Name = "registration_number";
            // 
            // tel
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tel.DefaultCellStyle = dataGridViewCellStyle2;
            this.tel.HeaderText = "전화번호";
            this.tel.Name = "tel";
            // 
            // fax
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.fax.DefaultCellStyle = dataGridViewCellStyle3;
            this.fax.HeaderText = "팩스번호";
            this.fax.Name = "fax";
            // 
            // phone
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.phone.DefaultCellStyle = dataGridViewCellStyle4;
            this.phone.HeaderText = "휴대폰";
            this.phone.Name = "phone";
            // 
            // other_phone
            // 
            this.other_phone.HeaderText = "기타연락처";
            this.other_phone.Name = "other_phone";
            // 
            // isNotSendFax
            // 
            this.isNotSendFax.HeaderText = "팩스X";
            this.isNotSendFax.Name = "isNotSendFax";
            this.isNotSendFax.Visible = false;
            this.isNotSendFax.Width = 50;
            // 
            // isIgnore
            // 
            this.isIgnore.HeaderText = "무시";
            this.isIgnore.Name = "isIgnore";
            this.isIgnore.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isIgnore.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isIgnore.Width = 50;
            // 
            // isRecovery
            // 
            this.isRecovery.HeaderText = "복구";
            this.isRecovery.Name = "isRecovery";
            this.isRecovery.Width = 50;
            // 
            // isDelete
            // 
            this.isDelete.HeaderText = "삭제";
            this.isDelete.Name = "isDelete";
            this.isDelete.Width = 50;
            // 
            // NotHandlingCompanyList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 592);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NotHandlingCompanyList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "취급X 거래처";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NotHandlingCompanyList_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NotHandlingCompanyList_KeyDown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvCompany;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRecovery;
        private System.Windows.Forms.Button btnIgnore;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowindex;
        private System.Windows.Forms.DataGridViewTextBoxColumn company;
        private System.Windows.Forms.DataGridViewTextBoxColumn registration_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn tel;
        private System.Windows.Forms.DataGridViewTextBoxColumn fax;
        private System.Windows.Forms.DataGridViewTextBoxColumn phone;
        private System.Windows.Forms.DataGridViewTextBoxColumn other_phone;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isNotSendFax;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isIgnore;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isRecovery;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isDelete;
    }
}