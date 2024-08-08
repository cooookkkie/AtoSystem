namespace AdoNetWindow.SaleManagement
{
    partial class CompanyGroupManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompanyGroupManager));
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvCompany = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.company_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.registration_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ceo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ato_manager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sales_updatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sales_edit_user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sales_contents = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sales_remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.table_index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvCompany);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(698, 281);
            this.panel2.TabIndex = 1;
            // 
            // dgvCompany
            // 
            this.dgvCompany.AllowUserToAddRows = false;
            this.dgvCompany.AllowUserToDeleteRows = false;
            this.dgvCompany.AllowUserToResizeColumns = false;
            this.dgvCompany.AllowUserToResizeRows = false;
            this.dgvCompany.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCompany.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.company_id,
            this.chk,
            this.company,
            this.registration_number,
            this.ceo,
            this.tel,
            this.fax,
            this.phone,
            this.ato_manager,
            this.sales_updatetime,
            this.sales_edit_user,
            this.sales_contents,
            this.sales_remark,
            this.table_index});
            this.dgvCompany.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCompany.EnableHeadersVisualStyles = false;
            this.dgvCompany.Location = new System.Drawing.Point(0, 0);
            this.dgvCompany.Name = "dgvCompany";
            this.dgvCompany.RowHeadersWidth = 30;
            this.dgvCompany.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvCompany.RowTemplate.Height = 23;
            this.dgvCompany.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCompany.Size = new System.Drawing.Size(698, 281);
            this.dgvCompany.TabIndex = 0;
            this.dgvCompany.SelectionChanged += new System.EventHandler(this.dgvCompany_SelectionChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnRegister);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 281);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(698, 42);
            this.panel3.TabIndex = 2;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(629, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 36);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRegister.Location = new System.Drawing.Point(3, 3);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(90, 36);
            this.btnRegister.TabIndex = 0;
            this.btnRegister.Text = "등록(Enter)";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // company_id
            // 
            this.company_id.HeaderText = "company_id";
            this.company_id.Name = "company_id";
            this.company_id.Visible = false;
            // 
            // chk
            // 
            this.chk.HeaderText = "대표";
            this.chk.Name = "chk";
            this.chk.Width = 50;
            // 
            // company
            // 
            this.company.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.company.HeaderText = "거래처명";
            this.company.Name = "company";
            // 
            // registration_number
            // 
            this.registration_number.HeaderText = "사업자번호";
            this.registration_number.Name = "registration_number";
            // 
            // ceo
            // 
            this.ceo.HeaderText = "대표자";
            this.ceo.Name = "ceo";
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
            this.phone.HeaderText = "휴대폰";
            this.phone.Name = "phone";
            // 
            // ato_manager
            // 
            this.ato_manager.HeaderText = "ato_manager";
            this.ato_manager.Name = "ato_manager";
            this.ato_manager.Visible = false;
            // 
            // sales_updatetime
            // 
            this.sales_updatetime.HeaderText = "sales_updatetime";
            this.sales_updatetime.Name = "sales_updatetime";
            this.sales_updatetime.Visible = false;
            // 
            // sales_edit_user
            // 
            this.sales_edit_user.HeaderText = "sales_edit_user";
            this.sales_edit_user.Name = "sales_edit_user";
            this.sales_edit_user.Visible = false;
            // 
            // sales_contents
            // 
            this.sales_contents.HeaderText = "sales_contents";
            this.sales_contents.Name = "sales_contents";
            this.sales_contents.Visible = false;
            // 
            // sales_remark
            // 
            this.sales_remark.HeaderText = "sales_remark";
            this.sales_remark.Name = "sales_remark";
            this.sales_remark.Visible = false;
            // 
            // table_index
            // 
            this.table_index.HeaderText = "table_index";
            this.table_index.Name = "table_index";
            this.table_index.Visible = false;
            // 
            // CompanyGroupManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 323);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CompanyGroupManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "대표업체 설정";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CompanyGroupManager_KeyDown);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRegister;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvCompany;
        private System.Windows.Forms.DataGridViewTextBoxColumn company_id;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn company;
        private System.Windows.Forms.DataGridViewTextBoxColumn registration_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn ceo;
        private System.Windows.Forms.DataGridViewTextBoxColumn tel;
        private System.Windows.Forms.DataGridViewTextBoxColumn fax;
        private System.Windows.Forms.DataGridViewTextBoxColumn phone;
        private System.Windows.Forms.DataGridViewTextBoxColumn ato_manager;
        private System.Windows.Forms.DataGridViewTextBoxColumn sales_updatetime;
        private System.Windows.Forms.DataGridViewTextBoxColumn sales_edit_user;
        private System.Windows.Forms.DataGridViewTextBoxColumn sales_contents;
        private System.Windows.Forms.DataGridViewTextBoxColumn sales_remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn table_index;
    }
}