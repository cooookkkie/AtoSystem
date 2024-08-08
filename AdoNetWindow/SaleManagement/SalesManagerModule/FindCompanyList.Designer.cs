namespace AdoNetWindow.SaleManagement.SalesManagerModule
{
    partial class FindCompanyList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindCompanyList));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtAtoManager = new System.Windows.Forms.TextBox();
            this.lbAtoManager = new System.Windows.Forms.Label();
            this.txtDistribution = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtHandlingItem = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRegistrationNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTel = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvCompany = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.company_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seaover_company_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.table_index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ceo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.registration_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.other_phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.handling_item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seaover_handling_item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.distribution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ato_manager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isNotSendFax = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnUpdate = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearching = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtAtoManager);
            this.panel1.Controls.Add(this.lbAtoManager);
            this.panel1.Controls.Add(this.txtDistribution);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtHandlingItem);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtRegistrationNumber);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtTel);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtCompany);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1363, 33);
            this.panel1.TabIndex = 0;
            // 
            // txtAtoManager
            // 
            this.txtAtoManager.Enabled = false;
            this.txtAtoManager.Location = new System.Drawing.Point(1185, 6);
            this.txtAtoManager.Name = "txtAtoManager";
            this.txtAtoManager.Size = new System.Drawing.Size(124, 21);
            this.txtAtoManager.TabIndex = 11;
            this.txtAtoManager.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompany_KeyDown);
            // 
            // lbAtoManager
            // 
            this.lbAtoManager.AutoSize = true;
            this.lbAtoManager.Location = new System.Drawing.Point(1138, 10);
            this.lbAtoManager.Name = "lbAtoManager";
            this.lbAtoManager.Size = new System.Drawing.Size(41, 12);
            this.lbAtoManager.TabIndex = 10;
            this.lbAtoManager.Text = "담당자";
            // 
            // txtDistribution
            // 
            this.txtDistribution.Enabled = false;
            this.txtDistribution.Location = new System.Drawing.Point(972, 6);
            this.txtDistribution.Name = "txtDistribution";
            this.txtDistribution.Size = new System.Drawing.Size(163, 21);
            this.txtDistribution.TabIndex = 9;
            this.txtDistribution.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompany_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(937, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "유통";
            // 
            // txtHandlingItem
            // 
            this.txtHandlingItem.Enabled = false;
            this.txtHandlingItem.Location = new System.Drawing.Point(768, 6);
            this.txtHandlingItem.Name = "txtHandlingItem";
            this.txtHandlingItem.Size = new System.Drawing.Size(163, 21);
            this.txtHandlingItem.TabIndex = 7;
            this.txtHandlingItem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompany_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(709, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "취급품목";
            // 
            // txtRegistrationNumber
            // 
            this.txtRegistrationNumber.Enabled = false;
            this.txtRegistrationNumber.Location = new System.Drawing.Point(540, 6);
            this.txtRegistrationNumber.Name = "txtRegistrationNumber";
            this.txtRegistrationNumber.Size = new System.Drawing.Size(163, 21);
            this.txtRegistrationNumber.TabIndex = 5;
            this.txtRegistrationNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompany_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(469, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "사업자번호";
            // 
            // txtTel
            // 
            this.txtTel.Enabled = false;
            this.txtTel.Location = new System.Drawing.Point(299, 6);
            this.txtTel.Name = "txtTel";
            this.txtTel.Size = new System.Drawing.Size(163, 21);
            this.txtTel.TabIndex = 3;
            this.txtTel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompany_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(240, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "전화번호";
            // 
            // txtCompany
            // 
            this.txtCompany.Enabled = false;
            this.txtCompany.Location = new System.Drawing.Point(71, 6);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(163, 21);
            this.txtCompany.TabIndex = 1;
            this.txtCompany.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompany_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "거래처명";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvCompany);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1363, 607);
            this.panel2.TabIndex = 1;
            // 
            // dgvCompany
            // 
            this.dgvCompany.AllowUserToAddRows = false;
            this.dgvCompany.AllowUserToDeleteRows = false;
            this.dgvCompany.AllowUserToOrderColumns = true;
            this.dgvCompany.AllowUserToResizeRows = false;
            this.dgvCompany.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCompany.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.company_id,
            this.seaover_company_code,
            this.table_index,
            this.division,
            this.company,
            this.ceo,
            this.registration_number,
            this.tel,
            this.fax,
            this.phone,
            this.other_phone,
            this.handling_item,
            this.seaover_handling_item,
            this.distribution,
            this.ato_manager,
            this.isNotSendFax,
            this.btnUpdate});
            this.dgvCompany.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCompany.EnableHeadersVisualStyles = false;
            this.dgvCompany.Location = new System.Drawing.Point(0, 0);
            this.dgvCompany.Name = "dgvCompany";
            this.dgvCompany.RowHeadersWidth = 30;
            this.dgvCompany.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvCompany.RowTemplate.Height = 23;
            this.dgvCompany.Size = new System.Drawing.Size(1363, 607);
            this.dgvCompany.TabIndex = 0;
            this.dgvCompany.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellContentClick);
            // 
            // company_id
            // 
            this.company_id.HeaderText = "company_id";
            this.company_id.Name = "company_id";
            this.company_id.Visible = false;
            // 
            // seaover_company_code
            // 
            this.seaover_company_code.HeaderText = "seaover_company_code";
            this.seaover_company_code.Name = "seaover_company_code";
            this.seaover_company_code.Visible = false;
            // 
            // table_index
            // 
            this.table_index.HeaderText = "table_index";
            this.table_index.Name = "table_index";
            this.table_index.Visible = false;
            // 
            // division
            // 
            this.division.HeaderText = "카테고리";
            this.division.Name = "division";
            // 
            // company
            // 
            this.company.HeaderText = "거래처";
            this.company.Name = "company";
            // 
            // ceo
            // 
            this.ceo.HeaderText = "대표자";
            this.ceo.Name = "ceo";
            // 
            // registration_number
            // 
            this.registration_number.HeaderText = "사업자번호";
            this.registration_number.Name = "registration_number";
            // 
            // tel
            // 
            this.tel.HeaderText = "TEL";
            this.tel.Name = "tel";
            // 
            // fax
            // 
            this.fax.HeaderText = "FAX";
            this.fax.Name = "fax";
            // 
            // phone
            // 
            this.phone.HeaderText = "PHONE";
            this.phone.Name = "phone";
            // 
            // other_phone
            // 
            this.other_phone.HeaderText = "기타연락처";
            this.other_phone.Name = "other_phone";
            // 
            // handling_item
            // 
            this.handling_item.HeaderText = "취급품목";
            this.handling_item.Name = "handling_item";
            // 
            // seaover_handling_item
            // 
            this.seaover_handling_item.HeaderText = "취급품목2";
            this.seaover_handling_item.Name = "seaover_handling_item";
            // 
            // distribution
            // 
            this.distribution.HeaderText = "유통";
            this.distribution.Name = "distribution";
            // 
            // ato_manager
            // 
            this.ato_manager.HeaderText = "담당자";
            this.ato_manager.Name = "ato_manager";
            // 
            // isNotSendFax
            // 
            this.isNotSendFax.HeaderText = "팩스X";
            this.isNotSendFax.Name = "isNotSendFax";
            this.isNotSendFax.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isNotSendFax.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isNotSendFax.Width = 50;
            // 
            // btnUpdate
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.NullValue = "수정";
            this.btnUpdate.DefaultCellStyle = dataGridViewCellStyle1;
            this.btnUpdate.HeaderText = "";
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnUpdate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.btnUpdate.Width = 60;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnSearching);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 640);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1363, 41);
            this.panel3.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label4);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(1060, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(303, 41);
            this.panel5.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(29, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(274, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "* 수정버튼은 \'팩스X\' 만 수정할 수 있습니다!";
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(3, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.ForeColor = System.Drawing.Color.Black;
            this.btnSearching.Location = new System.Drawing.Point(71, 1);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(66, 37);
            this.btnSearching.TabIndex = 3;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Visible = false;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // FindCompanyList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1363, 681);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FindCompanyList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "거래처 검색";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindCompanyList_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtRegistrationNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvCompany;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDistribution;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtHandlingItem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAtoManager;
        private System.Windows.Forms.Label lbAtoManager;
        private System.Windows.Forms.DataGridViewTextBoxColumn company_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_company_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn table_index;
        private System.Windows.Forms.DataGridViewTextBoxColumn division;
        private System.Windows.Forms.DataGridViewTextBoxColumn company;
        private System.Windows.Forms.DataGridViewTextBoxColumn ceo;
        private System.Windows.Forms.DataGridViewTextBoxColumn registration_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn tel;
        private System.Windows.Forms.DataGridViewTextBoxColumn fax;
        private System.Windows.Forms.DataGridViewTextBoxColumn phone;
        private System.Windows.Forms.DataGridViewTextBoxColumn other_phone;
        private System.Windows.Forms.DataGridViewTextBoxColumn handling_item;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_handling_item;
        private System.Windows.Forms.DataGridViewTextBoxColumn distribution;
        private System.Windows.Forms.DataGridViewTextBoxColumn ato_manager;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isNotSendFax;
        private System.Windows.Forms.DataGridViewButtonColumn btnUpdate;
    }
}