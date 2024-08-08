namespace AdoNetWindow.SaleManagement.DuplicateCompany
{
    partial class RemoveDuplicateCompanyManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoveDuplicateCompanyManager));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvLimitSetting = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbLimitCount = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.limit_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbLimitTerms = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.limit_terms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddCompany = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLimitSetting)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 19);
            this.label1.TabIndex = 51;
            this.label1.Text = "*중복거래처 삭제";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(485, 36);
            this.panel1.TabIndex = 53;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(303, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 12);
            this.label2.TabIndex = 52;
            this.label2.Text = "*개월(이하)";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvLimitSetting);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 36);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(485, 174);
            this.panel2.TabIndex = 54;
            // 
            // dgvLimitSetting
            // 
            this.dgvLimitSetting.AllowUserToAddRows = false;
            this.dgvLimitSetting.AllowUserToDeleteRows = false;
            this.dgvLimitSetting.AllowUserToResizeColumns = false;
            this.dgvLimitSetting.AllowUserToResizeRows = false;
            this.dgvLimitSetting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvLimitSetting.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.division,
            this.cbLimitCount,
            this.limit_count,
            this.cbLimitTerms,
            this.limit_terms});
            this.dgvLimitSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLimitSetting.EnableHeadersVisualStyles = false;
            this.dgvLimitSetting.Location = new System.Drawing.Point(0, 0);
            this.dgvLimitSetting.Name = "dgvLimitSetting";
            this.dgvLimitSetting.RowHeadersVisible = false;
            this.dgvLimitSetting.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvLimitSetting.RowTemplate.Height = 23;
            this.dgvLimitSetting.Size = new System.Drawing.Size(485, 174);
            this.dgvLimitSetting.TabIndex = 0;
            // 
            // division
            // 
            this.division.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.division.HeaderText = "구분";
            this.division.Name = "division";
            // 
            // cbLimitCount
            // 
            this.cbLimitCount.HeaderText = "";
            this.cbLimitCount.Name = "cbLimitCount";
            this.cbLimitCount.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cbLimitCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cbLimitCount.Width = 30;
            // 
            // limit_count
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.limit_count.DefaultCellStyle = dataGridViewCellStyle1;
            this.limit_count.HeaderText = "제한수";
            this.limit_count.Name = "limit_count";
            this.limit_count.Width = 70;
            // 
            // cbLimitTerms
            // 
            this.cbLimitTerms.HeaderText = "";
            this.cbLimitTerms.Name = "cbLimitTerms";
            this.cbLimitTerms.Width = 30;
            // 
            // limit_terms
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.limit_terms.DefaultCellStyle = dataGridViewCellStyle2;
            this.limit_terms.HeaderText = "영업기간";
            this.limit_terms.Name = "limit_terms";
            this.limit_terms.Width = 70;
            // 
            // btnAddCompany
            // 
            this.btnAddCompany.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAddCompany.ForeColor = System.Drawing.Color.Blue;
            this.btnAddCompany.Location = new System.Drawing.Point(5, 3);
            this.btnAddCompany.Name = "btnAddCompany";
            this.btnAddCompany.Size = new System.Drawing.Size(105, 37);
            this.btnAddCompany.TabIndex = 49;
            this.btnAddCompany.Text = "삭제 (ENTER)";
            this.btnAddCompany.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnAddCompany);
            this.panel3.Location = new System.Drawing.Point(0, 213);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(485, 43);
            this.panel3.TabIndex = 55;
            // 
            // RemoveDuplicateCompanyManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 210);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RemoveDuplicateCompanyManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "중복거래처 삭제";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RemoveDuplicateCompanyManager_FormClosing);
            this.Load += new System.EventHandler(this.RemoveDuplicateCompanyManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RemoveDuplicateCompanyManager_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLimitSetting)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvLimitSetting;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn division;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cbLimitCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn limit_count;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cbLimitTerms;
        private System.Windows.Forms.DataGridViewTextBoxColumn limit_terms;
        private System.Windows.Forms.Button btnAddCompany;
        private System.Windows.Forms.Panel panel3;
    }
}