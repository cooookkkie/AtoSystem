namespace AdoNetWindow.SaleManagement.SalesManagerModule
{
    partial class ColumnSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColumnSetting));
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvColumn = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.column_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column_header_txt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.column_width = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnRegistration = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumn)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvColumn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(324, 469);
            this.panel1.TabIndex = 0;
            // 
            // dgvColumn
            // 
            this.dgvColumn.AllowUserToAddRows = false;
            this.dgvColumn.AllowUserToDeleteRows = false;
            this.dgvColumn.AllowUserToResizeColumns = false;
            this.dgvColumn.AllowUserToResizeRows = false;
            this.dgvColumn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvColumn.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.column_name,
            this.column_header_txt,
            this.isVisible,
            this.column_width});
            this.dgvColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvColumn.EnableHeadersVisualStyles = false;
            this.dgvColumn.Location = new System.Drawing.Point(0, 0);
            this.dgvColumn.Name = "dgvColumn";
            this.dgvColumn.RowTemplate.Height = 23;
            this.dgvColumn.Size = new System.Drawing.Size(324, 469);
            this.dgvColumn.TabIndex = 0;
            this.dgvColumn.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvColumn_CellContentClick);
            // 
            // column_name
            // 
            this.column_name.HeaderText = "column_name";
            this.column_name.Name = "column_name";
            this.column_name.Visible = false;
            // 
            // column_header_txt
            // 
            this.column_header_txt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.column_header_txt.HeaderText = "컬럼명";
            this.column_header_txt.Name = "column_header_txt";
            // 
            // isVisible
            // 
            this.isVisible.HeaderText = "보이기";
            this.isVisible.Name = "isVisible";
            this.isVisible.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isVisible.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isVisible.Width = 50;
            // 
            // column_width
            // 
            this.column_width.HeaderText = "넓이";
            this.column_width.Name = "column_width";
            this.column_width.Width = 60;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnRegistration);
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 469);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(324, 42);
            this.panel2.TabIndex = 1;
            // 
            // btnRegistration
            // 
            this.btnRegistration.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRegistration.ForeColor = System.Drawing.Color.Blue;
            this.btnRegistration.Location = new System.Drawing.Point(3, 3);
            this.btnRegistration.Name = "btnRegistration";
            this.btnRegistration.Size = new System.Drawing.Size(66, 37);
            this.btnRegistration.TabIndex = 20;
            this.btnRegistration.Text = "적용(A)";
            this.btnRegistration.UseVisualStyleBackColor = true;
            this.btnRegistration.Click += new System.EventHandler(this.btnRegistration_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(75, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 19;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // ColumnSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 511);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColumnSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "컬럼 설정";
            this.Load += new System.EventHandler(this.ColumnSetting_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ColumnSetting_KeyDown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumn)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvColumn;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnRegistration;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_header_txt;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_width;
    }
}