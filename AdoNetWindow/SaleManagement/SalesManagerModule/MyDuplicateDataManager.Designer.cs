namespace AdoNetWindow.SaleManagement
{
    partial class MyDuplicateDataManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyDuplicateDataManager));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cbTrading = new System.Windows.Forms.CheckBox();
            this.cbPotential2 = new System.Windows.Forms.CheckBox();
            this.cbPotential1 = new System.Windows.Forms.CheckBox();
            this.cbMydata = new System.Windows.Forms.CheckBox();
            this.cbCommonData = new System.Windows.Forms.CheckBox();
            this.txtAtoManager = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvData = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.company_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seaover_company_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.registration_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ceo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.other_phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ato_manager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sales_updatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sales_contents = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sales_edit_user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sales_remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.duplicate_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dulicate_complete = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.table_index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.division_int = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnDuplicateDelete = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtDivision = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.cbTrading);
            this.panel1.Controls.Add(this.cbPotential2);
            this.panel1.Controls.Add(this.cbPotential1);
            this.panel1.Controls.Add(this.cbMydata);
            this.panel1.Controls.Add(this.cbCommonData);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1272, 29);
            this.panel1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(1011, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(261, 29);
            this.panel4.TabIndex = 7;
            // 
            // cbTrading
            // 
            this.cbTrading.AutoSize = true;
            this.cbTrading.Checked = true;
            this.cbTrading.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTrading.Location = new System.Drawing.Point(292, 7);
            this.cbTrading.Name = "cbTrading";
            this.cbTrading.Size = new System.Drawing.Size(60, 16);
            this.cbTrading.TabIndex = 5;
            this.cbTrading.Text = "거래중";
            this.cbTrading.UseVisualStyleBackColor = true;
            // 
            // cbPotential2
            // 
            this.cbPotential2.AutoSize = true;
            this.cbPotential2.Checked = true;
            this.cbPotential2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPotential2.Location = new System.Drawing.Point(232, 7);
            this.cbPotential2.Name = "cbPotential2";
            this.cbPotential2.Size = new System.Drawing.Size(54, 16);
            this.cbPotential2.TabIndex = 4;
            this.cbPotential2.Text = "잠재2";
            this.cbPotential2.UseVisualStyleBackColor = true;
            // 
            // cbPotential1
            // 
            this.cbPotential1.AutoSize = true;
            this.cbPotential1.Checked = true;
            this.cbPotential1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPotential1.Location = new System.Drawing.Point(172, 7);
            this.cbPotential1.Name = "cbPotential1";
            this.cbPotential1.Size = new System.Drawing.Size(54, 16);
            this.cbPotential1.TabIndex = 3;
            this.cbPotential1.Text = "잠재1";
            this.cbPotential1.UseVisualStyleBackColor = true;
            // 
            // cbMydata
            // 
            this.cbMydata.AutoSize = true;
            this.cbMydata.Checked = true;
            this.cbMydata.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMydata.Location = new System.Drawing.Point(98, 7);
            this.cbMydata.Name = "cbMydata";
            this.cbMydata.Size = new System.Drawing.Size(68, 16);
            this.cbMydata.TabIndex = 2;
            this.cbMydata.Text = "내DATA";
            this.cbMydata.UseVisualStyleBackColor = true;
            // 
            // cbCommonData
            // 
            this.cbCommonData.AutoSize = true;
            this.cbCommonData.Location = new System.Drawing.Point(12, 7);
            this.cbCommonData.Name = "cbCommonData";
            this.cbCommonData.Size = new System.Drawing.Size(80, 16);
            this.cbCommonData.TabIndex = 1;
            this.cbCommonData.Text = "공용DATA";
            this.cbCommonData.UseVisualStyleBackColor = true;
            // 
            // txtAtoManager
            // 
            this.txtAtoManager.Location = new System.Drawing.Point(266, 4);
            this.txtAtoManager.Name = "txtAtoManager";
            this.txtAtoManager.Size = new System.Drawing.Size(143, 21);
            this.txtAtoManager.TabIndex = 1;
            this.txtAtoManager.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDivision_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(195, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "영업담당자";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvData);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 58);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1272, 713);
            this.panel2.TabIndex = 1;
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.AllowUserToResizeColumns = false;
            this.dgvData.AllowUserToResizeRows = false;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.company_id,
            this.seaover_company_code,
            this.division,
            this.company,
            this.registration_number,
            this.ceo,
            this.tel,
            this.fax,
            this.phone,
            this.other_phone,
            this.ato_manager,
            this.sales_updatetime,
            this.sales_contents,
            this.sales_edit_user,
            this.sales_remark,
            this.duplicate_id,
            this.dulicate_complete,
            this.delete,
            this.table_index,
            this.division_int});
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.EnableHeadersVisualStyles = false;
            this.dgvData.Location = new System.Drawing.Point(0, 0);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvData.RowTemplate.Height = 23;
            this.dgvData.Size = new System.Drawing.Size(1272, 713);
            this.dgvData.TabIndex = 0;
            this.dgvData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellContentClick);
            this.dgvData.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvData_CellMouseClick);
            this.dgvData.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvData_MouseUp);
            // 
            // company_id
            // 
            this.company_id.HeaderText = "ID";
            this.company_id.Name = "company_id";
            this.company_id.Width = 50;
            // 
            // seaover_company_code
            // 
            this.seaover_company_code.HeaderText = "씨오버코드";
            this.seaover_company_code.Name = "seaover_company_code";
            this.seaover_company_code.Visible = false;
            this.seaover_company_code.Width = 80;
            // 
            // division
            // 
            this.division.HeaderText = "구분";
            this.division.Name = "division";
            // 
            // company
            // 
            this.company.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.company.HeaderText = "거래처";
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
            this.phone.HeaderText = "PHONE";
            this.phone.Name = "phone";
            // 
            // other_phone
            // 
            this.other_phone.HeaderText = "기타연락처";
            this.other_phone.Name = "other_phone";
            // 
            // ato_manager
            // 
            this.ato_manager.HeaderText = "담당자";
            this.ato_manager.Name = "ato_manager";
            // 
            // sales_updatetime
            // 
            this.sales_updatetime.HeaderText = "매출일자";
            this.sales_updatetime.Name = "sales_updatetime";
            this.sales_updatetime.Width = 80;
            // 
            // sales_contents
            // 
            this.sales_contents.HeaderText = "sales_contents";
            this.sales_contents.Name = "sales_contents";
            this.sales_contents.Visible = false;
            // 
            // sales_edit_user
            // 
            this.sales_edit_user.HeaderText = "sales_edit_user";
            this.sales_edit_user.Name = "sales_edit_user";
            this.sales_edit_user.Visible = false;
            // 
            // sales_remark
            // 
            this.sales_remark.HeaderText = "sales_remark";
            this.sales_remark.Name = "sales_remark";
            this.sales_remark.Visible = false;
            // 
            // duplicate_id
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.duplicate_id.DefaultCellStyle = dataGridViewCellStyle1;
            this.duplicate_id.HeaderText = "중복";
            this.duplicate_id.Name = "duplicate_id";
            this.duplicate_id.Width = 50;
            // 
            // dulicate_complete
            // 
            this.dulicate_complete.HeaderText = "";
            this.dulicate_complete.Name = "dulicate_complete";
            this.dulicate_complete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dulicate_complete.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dulicate_complete.Visible = false;
            // 
            // delete
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle2.NullValue = "삭제";
            this.delete.DefaultCellStyle = dataGridViewCellStyle2;
            this.delete.HeaderText = "삭제";
            this.delete.Name = "delete";
            this.delete.Width = 50;
            // 
            // table_index
            // 
            this.table_index.HeaderText = "table_index";
            this.table_index.Name = "table_index";
            this.table_index.Visible = false;
            // 
            // division_int
            // 
            this.division_int.HeaderText = "division_int";
            this.division_int.Name = "division_int";
            this.division_int.Visible = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnDuplicateDelete);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 771);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1272, 43);
            this.panel3.TabIndex = 2;
            // 
            // btnDuplicateDelete
            // 
            this.btnDuplicateDelete.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDuplicateDelete.ForeColor = System.Drawing.Color.Blue;
            this.btnDuplicateDelete.Location = new System.Drawing.Point(75, 3);
            this.btnDuplicateDelete.Name = "btnDuplicateDelete";
            this.btnDuplicateDelete.Size = new System.Drawing.Size(189, 37);
            this.btnDuplicateDelete.TabIndex = 21;
            this.btnDuplicateDelete.Text = "중복데이터 하나만 남기기";
            this.btnDuplicateDelete.UseVisualStyleBackColor = true;
            this.btnDuplicateDelete.Click += new System.EventHandler(this.btnDuplicateDelete_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(270, 3);
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
            this.btnSearch.Location = new System.Drawing.Point(3, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(66, 37);
            this.btnSearch.TabIndex = 19;
            this.btnSearch.Text = "검색(Q)";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.txtDivision);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.txtAtoManager);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 29);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1272, 29);
            this.panel5.TabIndex = 3;
            // 
            // txtDivision
            // 
            this.txtDivision.Location = new System.Drawing.Point(46, 4);
            this.txtDivision.Name = "txtDivision";
            this.txtDivision.Size = new System.Drawing.Size(143, 21);
            this.txtDivision.TabIndex = 0;
            this.txtDivision.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDivision_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "구분";
            // 
            // MyDuplicateDataManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1272, 814);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MyDuplicateDataManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "중복데이터 삭제";
            this.Load += new System.EventHandler(this.MyDuplicateDataManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MyDuplicateDataManager_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvData;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox txtAtoManager;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbTrading;
        private System.Windows.Forms.CheckBox cbPotential2;
        private System.Windows.Forms.CheckBox cbPotential1;
        private System.Windows.Forms.CheckBox cbMydata;
        private System.Windows.Forms.CheckBox cbCommonData;
        private System.Windows.Forms.Button btnDuplicateDelete;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox txtDivision;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn company_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_company_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn division;
        private System.Windows.Forms.DataGridViewTextBoxColumn company;
        private System.Windows.Forms.DataGridViewTextBoxColumn registration_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn ceo;
        private System.Windows.Forms.DataGridViewTextBoxColumn tel;
        private System.Windows.Forms.DataGridViewTextBoxColumn fax;
        private System.Windows.Forms.DataGridViewTextBoxColumn phone;
        private System.Windows.Forms.DataGridViewTextBoxColumn other_phone;
        private System.Windows.Forms.DataGridViewTextBoxColumn ato_manager;
        private System.Windows.Forms.DataGridViewTextBoxColumn sales_updatetime;
        private System.Windows.Forms.DataGridViewTextBoxColumn sales_contents;
        private System.Windows.Forms.DataGridViewTextBoxColumn sales_edit_user;
        private System.Windows.Forms.DataGridViewTextBoxColumn sales_remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn duplicate_id;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dulicate_complete;
        private System.Windows.Forms.DataGridViewButtonColumn delete;
        private System.Windows.Forms.DataGridViewTextBoxColumn table_index;
        private System.Windows.Forms.DataGridViewTextBoxColumn division_int;
    }
}