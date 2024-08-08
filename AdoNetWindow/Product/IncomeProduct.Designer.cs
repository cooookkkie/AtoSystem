namespace AdoNetWindow.Product
{
    partial class IncomeProduct
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IncomeProduct));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDivision = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.dgvUserProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.user_product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user_origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel7 = new System.Windows.Forms.Panel();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtUserOrigin = new System.Windows.Forms.TextBox();
            this.txtUserProduct = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnAddDivision = new System.Windows.Forms.Button();
            this.txtInputDivision = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnPrinting = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserProduct)).BeginInit();
            this.panel7.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtManager);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtOrigin);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtProduct);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtDivision);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1645, 35);
            this.panel1.TabIndex = 0;
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(652, 8);
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(107, 21);
            this.txtManager.TabIndex = 7;
            this.txtManager.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtManager_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(605, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "담당자";
            // 
            // txtOrigin
            // 
            this.txtOrigin.Location = new System.Drawing.Point(492, 8);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.Size = new System.Drawing.Size(107, 21);
            this.txtOrigin.TabIndex = 5;
            this.txtOrigin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtManager_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(445, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "원산지";
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(334, 8);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(107, 21);
            this.txtProduct.TabIndex = 3;
            this.txtProduct.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtManager_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(299, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "품명";
            // 
            // txtDivision
            // 
            this.txtDivision.Location = new System.Drawing.Point(44, 8);
            this.txtDivision.Name = "txtDivision";
            this.txtDivision.Size = new System.Drawing.Size(250, 21);
            this.txtDivision.TabIndex = 1;
            this.txtDivision.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtManager_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "구분";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvProduct);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 35);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1645, 874);
            this.panel2.TabIndex = 1;
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProduct.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProduct.ColumnHeadersHeight = 46;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.EnableHeadersVisualStyles = false;
            this.dgvProduct.Location = new System.Drawing.Point(0, 29);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.Size = new System.Drawing.Size(1369, 845);
            this.dgvProduct.TabIndex = 0;
            this.dgvProduct.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvProduct_CellPainting);
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            this.dgvProduct.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvProduct_ColumnWidthChanged);
            this.dgvProduct.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvProduct_Scroll);
            this.dgvProduct.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvProduct_Paint);
            this.dgvProduct.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvUsers_MouseUp);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel8);
            this.panel6.Controls.Add(this.panel7);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(1369, 29);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(276, 845);
            this.panel6.TabIndex = 2;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.dgvUserProduct);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(0, 24);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(276, 821);
            this.panel8.TabIndex = 1;
            // 
            // dgvUserProduct
            // 
            this.dgvUserProduct.AllowUserToAddRows = false;
            this.dgvUserProduct.AllowUserToDeleteRows = false;
            this.dgvUserProduct.AllowUserToResizeColumns = false;
            this.dgvUserProduct.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUserProduct.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvUserProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.user_product,
            this.user_origin,
            this.manager});
            this.dgvUserProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUserProduct.EnableHeadersVisualStyles = false;
            this.dgvUserProduct.Location = new System.Drawing.Point(0, 0);
            this.dgvUserProduct.Name = "dgvUserProduct";
            this.dgvUserProduct.RowHeadersWidth = 20;
            this.dgvUserProduct.RowTemplate.Height = 23;
            this.dgvUserProduct.Size = new System.Drawing.Size(276, 821);
            this.dgvUserProduct.TabIndex = 1;
            this.dgvUserProduct.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvUserProduct_MouseUp);
            // 
            // user_product
            // 
            this.user_product.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.user_product.HeaderText = "품명";
            this.user_product.Name = "user_product";
            // 
            // user_origin
            // 
            this.user_origin.HeaderText = "원산지";
            this.user_origin.Name = "user_origin";
            this.user_origin.Width = 60;
            // 
            // manager
            // 
            this.manager.HeaderText = "담당자";
            this.manager.Name = "manager";
            this.manager.Width = 60;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.txtUserName);
            this.panel7.Controls.Add(this.txtUserOrigin);
            this.panel7.Controls.Add(this.txtUserProduct);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(276, 24);
            this.panel7.TabIndex = 0;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(215, 1);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(61, 21);
            this.txtUserName.TabIndex = 2;
            this.txtUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserProduct_KeyDown);
            // 
            // txtUserOrigin
            // 
            this.txtUserOrigin.Location = new System.Drawing.Point(154, 1);
            this.txtUserOrigin.Name = "txtUserOrigin";
            this.txtUserOrigin.Size = new System.Drawing.Size(61, 21);
            this.txtUserOrigin.TabIndex = 1;
            this.txtUserOrigin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserProduct_KeyDown);
            // 
            // txtUserProduct
            // 
            this.txtUserProduct.Location = new System.Drawing.Point(21, 1);
            this.txtUserProduct.Name = "txtUserProduct";
            this.txtUserProduct.Size = new System.Drawing.Size(132, 21);
            this.txtUserProduct.TabIndex = 0;
            this.txtUserProduct.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserProduct_KeyDown);
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.btnAddDivision);
            this.panel4.Controls.Add(this.txtInputDivision);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1645, 29);
            this.panel4.TabIndex = 1;
            // 
            // btnAddDivision
            // 
            this.btnAddDivision.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAddDivision.ForeColor = System.Drawing.Color.Blue;
            this.btnAddDivision.Location = new System.Drawing.Point(302, 4);
            this.btnAddDivision.Name = "btnAddDivision";
            this.btnAddDivision.Size = new System.Drawing.Size(62, 21);
            this.btnAddDivision.TabIndex = 23;
            this.btnAddDivision.Text = "추가(S)";
            this.btnAddDivision.UseVisualStyleBackColor = true;
            this.btnAddDivision.Click += new System.EventHandler(this.btnAddDivision_Click);
            // 
            // txtInputDivision
            // 
            this.txtInputDivision.Location = new System.Drawing.Point(44, 4);
            this.txtInputDivision.Name = "txtInputDivision";
            this.txtInputDivision.Size = new System.Drawing.Size(252, 21);
            this.txtInputDivision.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "구분";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnRegister);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 909);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1645, 45);
            this.panel3.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnPrinting);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(1433, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(212, 45);
            this.panel5.TabIndex = 2;
            // 
            // btnPrinting
            // 
            this.btnPrinting.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrinting.Location = new System.Drawing.Point(130, 4);
            this.btnPrinting.Name = "btnPrinting";
            this.btnPrinting.Size = new System.Drawing.Size(79, 38);
            this.btnPrinting.TabIndex = 23;
            this.btnPrinting.Text = "인쇄 (Ctrl+P)";
            this.btnPrinting.UseVisualStyleBackColor = true;
            this.btnPrinting.Click += new System.EventHandler(this.btnPrinting_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(3, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(62, 38);
            this.btnSearch.TabIndex = 23;
            this.btnSearch.Text = "검색(Q)";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(139, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(62, 38);
            this.btnExit.TabIndex = 22;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRegister.ForeColor = System.Drawing.Color.Blue;
            this.btnRegister.Location = new System.Drawing.Point(71, 3);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(62, 38);
            this.btnRegister.TabIndex = 21;
            this.btnRegister.Text = "등록(A)";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // IncomeProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1645, 954);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "IncomeProduct";
            this.Text = "수입예정품목";
            this.Load += new System.EventHandler(this.IncomeProduct_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IncomeProduct_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserProduct)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProduct;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDivision;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddDivision;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox txtInputDivision;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnSearch;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnPrinting;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel8;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvUserProduct;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtUserOrigin;
        private System.Windows.Forms.TextBox txtUserProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_product;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn manager;
    }
}