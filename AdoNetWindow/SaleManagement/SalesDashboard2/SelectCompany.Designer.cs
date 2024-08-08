namespace AdoNetWindow.SaleManagement.SalesDashboard2
{
    partial class SelectCompany
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectCompany));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtRegistrationNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgvCompany = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.btnInsert = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSearching = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.dgvSelectCompany = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCeo = new System.Windows.Forms.TextBox();
            this.seaover_company_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.registration_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ceo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_seaover_company_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_registration_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_ceo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select_manager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectCompany)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtManager);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtCeo);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtRegistrationNumber);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtCompany);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(999, 34);
            this.panel1.TabIndex = 6;
            // 
            // txtRegistrationNumber
            // 
            this.txtRegistrationNumber.Location = new System.Drawing.Point(270, 6);
            this.txtRegistrationNumber.Name = "txtRegistrationNumber";
            this.txtRegistrationNumber.Size = new System.Drawing.Size(134, 21);
            this.txtRegistrationNumber.TabIndex = 3;
            this.txtRegistrationNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompany_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(199, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "사업자번호";
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(59, 5);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(134, 21);
            this.txtCompany.TabIndex = 1;
            this.txtCompany.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompany_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "거래처";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(999, 533);
            this.panel2.TabIndex = 7;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.dgvSelectCompany);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(562, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(437, 533);
            this.panel6.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel11);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(437, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(562, 533);
            this.panel5.TabIndex = 2;
            // 
            // panel11
            // 
            this.panel11.BackgroundImage = global::AdoNetWindow.Properties.Resources.Right_button_icon;
            this.panel11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel11.Location = new System.Drawing.Point(29, 230);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(69, 53);
            this.panel11.TabIndex = 2;
            this.panel11.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel11_MouseClick);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dgvCompany);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(437, 533);
            this.panel4.TabIndex = 1;
            // 
            // dgvCompany
            // 
            this.dgvCompany.AllowUserToAddRows = false;
            this.dgvCompany.AllowUserToDeleteRows = false;
            this.dgvCompany.AllowUserToResizeRows = false;
            this.dgvCompany.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCompany.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCompany.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.seaover_company_code,
            this.company,
            this.registration_number,
            this.ceo,
            this.manager});
            this.dgvCompany.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCompany.EnableHeadersVisualStyles = false;
            this.dgvCompany.Location = new System.Drawing.Point(0, 0);
            this.dgvCompany.Name = "dgvCompany";
            this.dgvCompany.RowTemplate.Height = 23;
            this.dgvCompany.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCompany.Size = new System.Drawing.Size(437, 533);
            this.dgvCompany.TabIndex = 0;
            this.dgvCompany.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCompany_CellMouseDoubleClick);
            // 
            // btnInsert
            // 
            this.btnInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInsert.ForeColor = System.Drawing.Color.Blue;
            this.btnInsert.Location = new System.Drawing.Point(74, 3);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(107, 37);
            this.btnInsert.TabIndex = 5;
            this.btnInsert.Text = "거래처 추가(A)";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSearching);
            this.panel3.Controls.Add(this.btnInsert);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 567);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(999, 44);
            this.panel3.TabIndex = 8;
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.ForeColor = System.Drawing.Color.Black;
            this.btnSearching.Location = new System.Drawing.Point(3, 3);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(66, 37);
            this.btnSearching.TabIndex = 7;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(186, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // dgvSelectCompany
            // 
            this.dgvSelectCompany.AllowUserToAddRows = false;
            this.dgvSelectCompany.AllowUserToDeleteRows = false;
            this.dgvSelectCompany.AllowUserToResizeRows = false;
            this.dgvSelectCompany.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSelectCompany.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelectCompany.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.select_seaover_company_code,
            this.select_company,
            this.select_registration_number,
            this.select_ceo,
            this.select_manager});
            this.dgvSelectCompany.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSelectCompany.EnableHeadersVisualStyles = false;
            this.dgvSelectCompany.Location = new System.Drawing.Point(0, 0);
            this.dgvSelectCompany.Name = "dgvSelectCompany";
            this.dgvSelectCompany.RowTemplate.Height = 23;
            this.dgvSelectCompany.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelectCompany.Size = new System.Drawing.Size(437, 533);
            this.dgvSelectCompany.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(410, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "대표자";
            // 
            // txtCeo
            // 
            this.txtCeo.Location = new System.Drawing.Point(457, 6);
            this.txtCeo.Name = "txtCeo";
            this.txtCeo.Size = new System.Drawing.Size(134, 21);
            this.txtCeo.TabIndex = 5;
            this.txtCeo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompany_KeyDown);
            // 
            // seaover_company_code
            // 
            this.seaover_company_code.HeaderText = "S코드";
            this.seaover_company_code.Name = "seaover_company_code";
            this.seaover_company_code.Visible = false;
            // 
            // company
            // 
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
            // manager
            // 
            this.manager.HeaderText = "담당자";
            this.manager.Name = "manager";
            // 
            // select_seaover_company_code
            // 
            this.select_seaover_company_code.HeaderText = "S코드";
            this.select_seaover_company_code.Name = "select_seaover_company_code";
            this.select_seaover_company_code.Visible = false;
            // 
            // select_company
            // 
            this.select_company.HeaderText = "거래처";
            this.select_company.Name = "select_company";
            // 
            // select_registration_number
            // 
            this.select_registration_number.HeaderText = "사업자번호";
            this.select_registration_number.Name = "select_registration_number";
            // 
            // select_ceo
            // 
            this.select_ceo.HeaderText = "대표자";
            this.select_ceo.Name = "select_ceo";
            // 
            // select_manager
            // 
            this.select_manager.HeaderText = "담당자";
            this.select_manager.Name = "select_manager";
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(643, 5);
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(134, 21);
            this.txtManager.TabIndex = 7;
            this.txtManager.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompany_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(596, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "담당자";
            // 
            // SelectCompany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(999, 611);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectCompany";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "거래처 선택";
            this.Load += new System.EventHandler(this.SelectCompany_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SelectCompany_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectCompany)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtRegistrationNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel4;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvCompany;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtCeo;
        private System.Windows.Forms.Label label3;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvSelectCompany;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_seaover_company_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_company;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_registration_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_ceo;
        private System.Windows.Forms.DataGridViewTextBoxColumn select_manager;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_company_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn company;
        private System.Windows.Forms.DataGridViewTextBoxColumn registration_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn ceo;
        private System.Windows.Forms.DataGridViewTextBoxColumn manager;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.Label label4;
    }
}