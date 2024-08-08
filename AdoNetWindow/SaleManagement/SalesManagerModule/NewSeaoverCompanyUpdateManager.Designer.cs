namespace AdoNetWindow.SaleManagement
{
    partial class NewSeaoverCompanyUpdateManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewSeaoverCompanyUpdateManager));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvCompany = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.company_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.table_index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ato_company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ato_manager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.arrow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seaover_company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seaover_company_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.current_sales_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sales_manager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnRegistrator = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(900, 26);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(10, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(718, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "* 8개월 이내 SEAOVER에서 매출이 있는 중복 거래처입니다. 매출자의 거래중(SEAOVER)에서 영업중이니 삭제됩니다!";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvCompany);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 26);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(900, 436);
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
            this.table_index,
            this.division,
            this.ato_company,
            this.ato_manager,
            this.arrow,
            this.seaover_company,
            this.seaover_company_code,
            this.current_sales_date,
            this.sales_manager});
            this.dgvCompany.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCompany.EnableHeadersVisualStyles = false;
            this.dgvCompany.Location = new System.Drawing.Point(0, 0);
            this.dgvCompany.Name = "dgvCompany";
            this.dgvCompany.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvCompany.RowTemplate.Height = 23;
            this.dgvCompany.Size = new System.Drawing.Size(900, 436);
            this.dgvCompany.TabIndex = 0;
            // 
            // company_id
            // 
            this.company_id.HeaderText = "company_id";
            this.company_id.Name = "company_id";
            this.company_id.Visible = false;
            // 
            // table_index
            // 
            this.table_index.HeaderText = "table_index";
            this.table_index.Name = "table_index";
            this.table_index.Visible = false;
            // 
            // division
            // 
            this.division.HeaderText = "구분";
            this.division.Name = "division";
            // 
            // ato_company
            // 
            this.ato_company.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ato_company.HeaderText = "거래처";
            this.ato_company.Name = "ato_company";
            // 
            // ato_manager
            // 
            this.ato_manager.HeaderText = "담당자";
            this.ato_manager.Name = "ato_manager";
            // 
            // arrow
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.arrow.DefaultCellStyle = dataGridViewCellStyle1;
            this.arrow.HeaderText = "";
            this.arrow.Name = "arrow";
            this.arrow.Width = 40;
            // 
            // seaover_company
            // 
            this.seaover_company.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Blue;
            this.seaover_company.DefaultCellStyle = dataGridViewCellStyle2;
            this.seaover_company.HeaderText = "거래처";
            this.seaover_company.Name = "seaover_company";
            // 
            // seaover_company_code
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Blue;
            this.seaover_company_code.DefaultCellStyle = dataGridViewCellStyle3;
            this.seaover_company_code.HeaderText = "씨오버코드";
            this.seaover_company_code.Name = "seaover_company_code";
            // 
            // current_sales_date
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Blue;
            this.current_sales_date.DefaultCellStyle = dataGridViewCellStyle4;
            this.current_sales_date.HeaderText = "매출일";
            this.current_sales_date.Name = "current_sales_date";
            // 
            // sales_manager
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Blue;
            this.sales_manager.DefaultCellStyle = dataGridViewCellStyle5;
            this.sales_manager.HeaderText = "매출자";
            this.sales_manager.Name = "sales_manager";
            this.sales_manager.Width = 70;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnRegistrator);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 462);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(900, 41);
            this.panel3.TabIndex = 2;
            // 
            // btnRegistrator
            // 
            this.btnRegistrator.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRegistrator.ForeColor = System.Drawing.Color.Blue;
            this.btnRegistrator.Location = new System.Drawing.Point(79, 2);
            this.btnRegistrator.Name = "btnRegistrator";
            this.btnRegistrator.Size = new System.Drawing.Size(75, 37);
            this.btnRegistrator.TabIndex = 5;
            this.btnRegistrator.Text = "삭제(ENTER)";
            this.btnRegistrator.UseVisualStyleBackColor = true;
            this.btnRegistrator.Visible = false;
            this.btnRegistrator.Click += new System.EventHandler(this.btnRegistrator_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(3, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 37);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "닫기(ESC)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // NewSeaoverCompanyUpdateManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 503);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewSeaoverCompanyUpdateManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "신규거래처";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewSeaoverCompanyUpdateManager_FormClosing);
            this.Load += new System.EventHandler(this.NewSeaoverCompanyUpdateManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NewSeaoverCompanyUpdateManager_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvCompany;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRegistrator;
        private System.Windows.Forms.DataGridViewTextBoxColumn company_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn table_index;
        private System.Windows.Forms.DataGridViewTextBoxColumn division;
        private System.Windows.Forms.DataGridViewTextBoxColumn ato_company;
        private System.Windows.Forms.DataGridViewTextBoxColumn ato_manager;
        private System.Windows.Forms.DataGridViewTextBoxColumn arrow;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_company;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_company_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn current_sales_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn sales_manager;
    }
}