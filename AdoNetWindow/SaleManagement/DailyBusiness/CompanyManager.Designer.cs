namespace AdoNetWindow.SaleManagement.DailyBusiness
{
    partial class CompanyManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompanyManager));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtRemark2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDistribution = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtHandlingItem = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvCompany = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.company_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.handling_item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.distribution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSearching = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtRemark2);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtDistribution);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtHandlingItem);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtCompany);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1097, 32);
            this.panel1.TabIndex = 0;
            // 
            // txtRemark2
            // 
            this.txtRemark2.Location = new System.Drawing.Point(860, 5);
            this.txtRemark2.Name = "txtRemark2";
            this.txtRemark2.Size = new System.Drawing.Size(218, 21);
            this.txtRemark2.TabIndex = 19;
            this.txtRemark2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompany_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(825, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "비고";
            // 
            // txtDistribution
            // 
            this.txtDistribution.Location = new System.Drawing.Point(601, 5);
            this.txtDistribution.Name = "txtDistribution";
            this.txtDistribution.Size = new System.Drawing.Size(218, 21);
            this.txtDistribution.TabIndex = 17;
            this.txtDistribution.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompany_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(566, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "유통";
            // 
            // txtHandlingItem
            // 
            this.txtHandlingItem.Location = new System.Drawing.Point(342, 5);
            this.txtHandlingItem.Name = "txtHandlingItem";
            this.txtHandlingItem.Size = new System.Drawing.Size(218, 21);
            this.txtHandlingItem.TabIndex = 15;
            this.txtHandlingItem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompany_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(283, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "문의내용";
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(59, 5);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(218, 21);
            this.txtCompany.TabIndex = 9;
            this.txtCompany.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompany_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "거래처";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvCompany);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 32);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1097, 567);
            this.panel2.TabIndex = 1;
            // 
            // dgvCompany
            // 
            this.dgvCompany.AllowUserToAddRows = false;
            this.dgvCompany.AllowUserToDeleteRows = false;
            this.dgvCompany.AllowUserToResizeRows = false;
            this.dgvCompany.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCompany.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCompany.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.company_code,
            this.company,
            this.handling_item,
            this.distribution,
            this.remark2});
            this.dgvCompany.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCompany.EnableHeadersVisualStyles = false;
            this.dgvCompany.Location = new System.Drawing.Point(0, 0);
            this.dgvCompany.Name = "dgvCompany";
            this.dgvCompany.RowTemplate.Height = 23;
            this.dgvCompany.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCompany.Size = new System.Drawing.Size(1097, 567);
            this.dgvCompany.TabIndex = 3;
            this.dgvCompany.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCompany_CellMouseDoubleClick);
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.Visible = false;
            // 
            // company_code
            // 
            this.company_code.HeaderText = "거래처코드";
            this.company_code.Name = "company_code";
            this.company_code.Visible = false;
            // 
            // company
            // 
            this.company.HeaderText = "거래처";
            this.company.Name = "company";
            // 
            // handling_item
            // 
            this.handling_item.HeaderText = "문의내용";
            this.handling_item.Name = "handling_item";
            // 
            // distribution
            // 
            this.distribution.HeaderText = "유통";
            this.distribution.Name = "distribution";
            // 
            // remark2
            // 
            this.remark2.HeaderText = "비고";
            this.remark2.Name = "remark2";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSelect);
            this.panel3.Controls.Add(this.btnSearching);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 599);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1097, 39);
            this.panel3.TabIndex = 2;
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.ForeColor = System.Drawing.Color.Blue;
            this.btnSearching.Location = new System.Drawing.Point(75, 1);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(66, 37);
            this.btnSearching.TabIndex = 20;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(147, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 19;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelect.ForeColor = System.Drawing.Color.Black;
            this.btnSelect.Location = new System.Drawing.Point(3, 1);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(66, 37);
            this.btnSelect.TabIndex = 21;
            this.btnSelect.Text = "선택(S)";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // CompanyManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 638);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "CompanyManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "거래처 검색";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CompanyManager_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.TextBox txtRemark2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDistribution;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtHandlingItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn company_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn company;
        private System.Windows.Forms.DataGridViewTextBoxColumn handling_item;
        private System.Windows.Forms.DataGridViewTextBoxColumn distribution;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark2;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSelect;
    }
}