namespace AdoNetWindow.SaleManagement.SalesManagerModule
{
    partial class InsertNotSendFax
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InsertNotSendFax));
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvFax = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.fax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnGetExcel = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
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
            this.panel2.Size = new System.Drawing.Size(388, 410);
            this.panel2.TabIndex = 1;
            // 
            // dgvFax
            // 
            this.dgvFax.AllowUserToResizeColumns = false;
            this.dgvFax.AllowUserToResizeRows = false;
            this.dgvFax.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvFax.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFax.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fax});
            this.dgvFax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFax.EnableHeadersVisualStyles = false;
            this.dgvFax.Location = new System.Drawing.Point(0, 0);
            this.dgvFax.Name = "dgvFax";
            this.dgvFax.RowTemplate.Height = 23;
            this.dgvFax.Size = new System.Drawing.Size(388, 410);
            this.dgvFax.TabIndex = 0;
            // 
            // fax
            // 
            this.fax.HeaderText = "FAX";
            this.fax.Name = "fax";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnGetExcel);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnInsert);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 410);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(388, 40);
            this.panel3.TabIndex = 2;
            // 
            // btnGetExcel
            // 
            this.btnGetExcel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnGetExcel.ForeColor = System.Drawing.Color.Blue;
            this.btnGetExcel.Location = new System.Drawing.Point(75, 1);
            this.btnGetExcel.Name = "btnGetExcel";
            this.btnGetExcel.Size = new System.Drawing.Size(77, 37);
            this.btnGetExcel.TabIndex = 27;
            this.btnGetExcel.Text = "불러오기";
            this.btnGetExcel.UseVisualStyleBackColor = true;
            this.btnGetExcel.Click += new System.EventHandler(this.btnGetExcel_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(158, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 26;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInsert.ForeColor = System.Drawing.Color.Blue;
            this.btnInsert.Location = new System.Drawing.Point(3, 1);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(66, 37);
            this.btnInsert.TabIndex = 25;
            this.btnInsert.Text = "등록(A)";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // InsertNotSendFax
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InsertNotSendFax";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "팩스금지 거래처 등록";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InsertNotSendFax_KeyDown);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFax)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvFax;
        private System.Windows.Forms.DataGridViewTextBoxColumn fax;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnGetExcel;
        private System.Windows.Forms.Button btnExit;
    }
}