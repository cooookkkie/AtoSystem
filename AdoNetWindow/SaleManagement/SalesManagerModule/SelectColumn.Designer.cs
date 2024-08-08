namespace AdoNetWindow.SaleManagement.SalesManagerModule
{
    partial class SelectColumn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectColumn));
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvFax = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.column_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bgw = new System.ComponentModel.BackgroundWorker();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFax)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvFax);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(307, 410);
            this.panel2.TabIndex = 3;
            // 
            // dgvFax
            // 
            this.dgvFax.AllowUserToAddRows = false;
            this.dgvFax.AllowUserToResizeColumns = false;
            this.dgvFax.AllowUserToResizeRows = false;
            this.dgvFax.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvFax.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFax.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chk,
            this.column_name});
            this.dgvFax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFax.EnableHeadersVisualStyles = false;
            this.dgvFax.Location = new System.Drawing.Point(0, 0);
            this.dgvFax.Name = "dgvFax";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFax.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFax.RowTemplate.Height = 23;
            this.dgvFax.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFax.Size = new System.Drawing.Size(307, 410);
            this.dgvFax.TabIndex = 0;
            this.dgvFax.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFax_CellDoubleClick);
            this.dgvFax.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvFax_CellPainting);
            this.dgvFax.SelectionChanged += new System.EventHandler(this.dgvFax_SelectionChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnSelect);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 410);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(307, 40);
            this.panel3.TabIndex = 4;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(75, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 26;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelect.ForeColor = System.Drawing.Color.Blue;
            this.btnSelect.Location = new System.Drawing.Point(3, 1);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(66, 37);
            this.btnSelect.TabIndex = 25;
            this.btnSelect.Text = "선택(A)";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // chk
            // 
            this.chk.FillWeight = 30.45685F;
            this.chk.HeaderText = "";
            this.chk.Name = "chk";
            // 
            // column_name
            // 
            this.column_name.FillWeight = 169.5432F;
            this.column_name.HeaderText = "컬럼명";
            this.column_name.Name = "column_name";
            // 
            // SelectColumn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectColumn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "팩스 컬럼을 선택해주세요..";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SelectColumn_KeyDown);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFax)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvFax;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_name;
        private System.ComponentModel.BackgroundWorker bgw;
    }
}