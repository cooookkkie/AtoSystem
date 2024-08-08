namespace AdoNetWindow.CalendarModule
{
    partial class HideManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HideManager));
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvGuarantee = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.until_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.edit_user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGuarantee)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvGuarantee);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(441, 274);
            this.panel1.TabIndex = 0;
            // 
            // dgvGuarantee
            // 
            this.dgvGuarantee.AllowUserToAddRows = false;
            this.dgvGuarantee.AllowUserToDeleteRows = false;
            this.dgvGuarantee.AllowUserToResizeRows = false;
            this.dgvGuarantee.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGuarantee.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGuarantee.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.category,
            this.until_date,
            this.edit_user,
            this.updatetime});
            this.dgvGuarantee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGuarantee.EnableHeadersVisualStyles = false;
            this.dgvGuarantee.Location = new System.Drawing.Point(0, 0);
            this.dgvGuarantee.Name = "dgvGuarantee";
            this.dgvGuarantee.RowTemplate.Height = 23;
            this.dgvGuarantee.Size = new System.Drawing.Size(441, 274);
            this.dgvGuarantee.TabIndex = 0;
            this.dgvGuarantee.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.multiHeaderGrid1_CellMouseClick);
            this.dgvGuarantee.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGuarantee_CellMouseDoubleClick);
            this.dgvGuarantee.MouseUp += new System.Windows.Forms.MouseEventHandler(this.multiHeaderGrid1_MouseUp);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 274);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(441, 41);
            this.panel2.TabIndex = 1;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(370, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(68, 32);
            this.btnExit.TabIndex = 102;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            // 
            // category
            // 
            this.category.HeaderText = "Ato No";
            this.category.Name = "category";
            // 
            // until_date
            // 
            this.until_date.HeaderText = "제외일자";
            this.until_date.Name = "until_date";
            // 
            // edit_user
            // 
            this.edit_user.HeaderText = "수정자";
            this.edit_user.Name = "edit_user";
            // 
            // updatetime
            // 
            this.updatetime.HeaderText = "수정일자";
            this.updatetime.Name = "updatetime";
            // 
            // HideManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(441, 315);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HideManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "숨긴재고";
            this.Load += new System.EventHandler(this.HideManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HideManager_KeyDown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGuarantee)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvGuarantee;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn category;
        private System.Windows.Forms.DataGridViewTextBoxColumn until_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn edit_user;
        private System.Windows.Forms.DataGridViewTextBoxColumn updatetime;
    }
}