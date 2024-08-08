namespace AdoNetWindow.SaleManagement.DuplicateCompany
{
    partial class ExcelColumnManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExcelColumnManager));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnRegistration = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.input_column_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.input_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.div = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.excel_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColumns = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumns)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvColumns);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(382, 408);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnRegistration);
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 408);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(382, 42);
            this.panel2.TabIndex = 1;
            // 
            // btnRegistration
            // 
            this.btnRegistration.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRegistration.ForeColor = System.Drawing.Color.Blue;
            this.btnRegistration.Location = new System.Drawing.Point(3, 2);
            this.btnRegistration.Name = "btnRegistration";
            this.btnRegistration.Size = new System.Drawing.Size(66, 37);
            this.btnRegistration.TabIndex = 18;
            this.btnRegistration.Text = "확인(A)";
            this.btnRegistration.UseVisualStyleBackColor = true;
            this.btnRegistration.Click += new System.EventHandler(this.btnAddCompany_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(75, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 17;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // input_column_name
            // 
            this.input_column_name.HeaderText = "input_column_name";
            this.input_column_name.Name = "input_column_name";
            this.input_column_name.Visible = false;
            // 
            // input_column
            // 
            this.input_column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.input_column.HeaderText = "컬럼";
            this.input_column.Name = "input_column";
            // 
            // div
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.div.DefaultCellStyle = dataGridViewCellStyle1;
            this.div.HeaderText = "";
            this.div.Name = "div";
            this.div.Width = 50;
            // 
            // excel_column
            // 
            this.excel_column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.excel_column.HeaderText = "엑셀";
            this.excel_column.Name = "excel_column";
            // 
            // dgvColumns
            // 
            this.dgvColumns.AllowUserToAddRows = false;
            this.dgvColumns.AllowUserToDeleteRows = false;
            this.dgvColumns.AllowUserToResizeColumns = false;
            this.dgvColumns.AllowUserToResizeRows = false;
            this.dgvColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.excel_column,
            this.div,
            this.input_column,
            this.input_column_name});
            this.dgvColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvColumns.EnableHeadersVisualStyles = false;
            this.dgvColumns.Location = new System.Drawing.Point(0, 0);
            this.dgvColumns.Name = "dgvColumns";
            this.dgvColumns.RowTemplate.Height = 23;
            this.dgvColumns.Size = new System.Drawing.Size(382, 408);
            this.dgvColumns.TabIndex = 0;
            this.dgvColumns.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvColumns_CellValueChanged);
            // 
            // ExcelColumnManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExcelColumnManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "컬럼 매칭하기";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExcelColumnManager_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumns)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnRegistration;
        private System.Windows.Forms.Button btnExit;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvColumns;
        private System.Windows.Forms.DataGridViewTextBoxColumn excel_column;
        private System.Windows.Forms.DataGridViewTextBoxColumn div;
        private System.Windows.Forms.DataGridViewTextBoxColumn input_column;
        private System.Windows.Forms.DataGridViewTextBoxColumn input_column_name;
    }
}