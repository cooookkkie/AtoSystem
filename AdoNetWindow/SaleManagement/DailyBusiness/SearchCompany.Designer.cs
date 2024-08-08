namespace AdoNetWindow.SaleManagement.DailyBusiness
{
    partial class SearchCompany
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchCompany));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbMyCompany = new System.Windows.Forms.CheckBox();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvCompany = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.company_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSeaching = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbMyCompany);
            this.panel1.Controls.Add(this.txtCompany);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(324, 31);
            this.panel1.TabIndex = 0;
            // 
            // cbMyCompany
            // 
            this.cbMyCompany.AutoSize = true;
            this.cbMyCompany.Location = new System.Drawing.Point(234, 8);
            this.cbMyCompany.Name = "cbMyCompany";
            this.cbMyCompany.Size = new System.Drawing.Size(88, 16);
            this.cbMyCompany.TabIndex = 19;
            this.cbMyCompany.Text = "내 거래처만";
            this.cbMyCompany.UseVisualStyleBackColor = true;
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(72, 4);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(156, 21);
            this.txtCompany.TabIndex = 1;
            this.txtCompany.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCompany_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "거래처명";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvCompany);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 31);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(324, 347);
            this.panel2.TabIndex = 1;
            // 
            // dgvCompany
            // 
            this.dgvCompany.AllowUserToAddRows = false;
            this.dgvCompany.AllowUserToDeleteRows = false;
            this.dgvCompany.AllowUserToResizeColumns = false;
            this.dgvCompany.AllowUserToResizeRows = false;
            this.dgvCompany.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCompany.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.company_code,
            this.company});
            this.dgvCompany.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCompany.EnableHeadersVisualStyles = false;
            this.dgvCompany.Location = new System.Drawing.Point(0, 0);
            this.dgvCompany.Name = "dgvCompany";
            this.dgvCompany.RowHeadersWidth = 30;
            this.dgvCompany.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvCompany.RowTemplate.Height = 23;
            this.dgvCompany.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCompany.Size = new System.Drawing.Size(324, 347);
            this.dgvCompany.TabIndex = 0;
            this.dgvCompany.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCompany_CellMouseDoubleClick);
            // 
            // company_code
            // 
            this.company_code.HeaderText = "거래처코드";
            this.company_code.Name = "company_code";
            this.company_code.Visible = false;
            // 
            // company
            // 
            this.company.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.company.HeaderText = "거래처명";
            this.company.Name = "company";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSeaching);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 378);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(324, 43);
            this.panel3.TabIndex = 2;
            // 
            // btnSeaching
            // 
            this.btnSeaching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSeaching.ForeColor = System.Drawing.Color.Black;
            this.btnSeaching.Location = new System.Drawing.Point(3, 3);
            this.btnSeaching.Name = "btnSeaching";
            this.btnSeaching.Size = new System.Drawing.Size(66, 37);
            this.btnSeaching.TabIndex = 23;
            this.btnSeaching.Text = "검색(Q)";
            this.btnSeaching.UseVisualStyleBackColor = true;
            this.btnSeaching.Click += new System.EventHandler(this.btnSeaching_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(75, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 22;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // SearchCompany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 421);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchCompany";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "거래처검색";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchCompany_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvCompany;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridViewTextBoxColumn company_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn company;
        private System.Windows.Forms.Button btnSeaching;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.CheckBox cbMyCompany;
    }
}