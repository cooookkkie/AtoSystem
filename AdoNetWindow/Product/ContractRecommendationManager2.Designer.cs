namespace AdoNetWindow.Product
{
    partial class ContractRecommendationManager2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContractRecommendationManager2));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtOperating = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtContract = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvProduct = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jan = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.feb = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.mar = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.apr = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.may = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.jun = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.jul = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.aug = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.sep = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.oct = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.nov = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dec = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnSearching = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtOperating);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtContract);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtManager);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtOrigin);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtProduct);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1188, 32);
            this.panel1.TabIndex = 0;
            // 
            // txtOperating
            // 
            this.txtOperating.Location = new System.Drawing.Point(919, 5);
            this.txtOperating.Name = "txtOperating";
            this.txtOperating.Size = new System.Drawing.Size(155, 21);
            this.txtOperating.TabIndex = 19;
            this.txtOperating.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            this.txtOperating.MouseHover += new System.EventHandler(this.txtContract_MouseHover);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(856, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "조업시기";
            // 
            // txtContract
            // 
            this.txtContract.Location = new System.Drawing.Point(695, 5);
            this.txtContract.Name = "txtContract";
            this.txtContract.Size = new System.Drawing.Size(155, 21);
            this.txtContract.TabIndex = 17;
            this.txtContract.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            this.txtContract.MouseHover += new System.EventHandler(this.txtContract_MouseHover);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(632, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 12);
            this.label1.TabIndex = 16;
            this.label1.Text = "계약시기";
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(471, 5);
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(155, 21);
            this.txtManager.TabIndex = 15;
            this.txtManager.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(421, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "담당자";
            // 
            // txtOrigin
            // 
            this.txtOrigin.Location = new System.Drawing.Point(260, 5);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.Size = new System.Drawing.Size(155, 21);
            this.txtOrigin.TabIndex = 13;
            this.txtOrigin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(210, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "원산지";
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(49, 5);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(155, 21);
            this.txtProduct.TabIndex = 11;
            this.txtProduct.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProduct_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "품명";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvProduct);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 32);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1188, 686);
            this.panel2.TabIndex = 1;
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToOrderColumns = true;
            this.dgvProduct.AllowUserToResizeColumns = false;
            this.dgvProduct.AllowUserToResizeRows = false;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.product,
            this.origin,
            this.division,
            this.jan,
            this.feb,
            this.mar,
            this.apr,
            this.may,
            this.jun,
            this.jul,
            this.aug,
            this.sep,
            this.oct,
            this.nov,
            this.dec,
            this.remark,
            this.manager});
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.EnableHeadersVisualStyles = false;
            this.dgvProduct.Location = new System.Drawing.Point(0, 0);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.Size = new System.Drawing.Size(1188, 686);
            this.dgvProduct.TabIndex = 0;
            this.dgvProduct.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellContentClick);
            this.dgvProduct.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvProduct_CellFormatting);
            this.dgvProduct.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvProduct_CellPainting);
            this.dgvProduct.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvProduct_MouseUp);
            // 
            // product
            // 
            this.product.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.product.DefaultCellStyle = dataGridViewCellStyle1;
            this.product.HeaderText = "품명";
            this.product.Name = "product";
            this.product.ReadOnly = true;
            // 
            // origin
            // 
            this.origin.HeaderText = "원산지";
            this.origin.Name = "origin";
            this.origin.ReadOnly = true;
            // 
            // division
            // 
            this.division.HeaderText = "구분";
            this.division.Name = "division";
            this.division.Width = 70;
            // 
            // jan
            // 
            this.jan.HeaderText = "1월";
            this.jan.Name = "jan";
            this.jan.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.jan.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.jan.Width = 40;
            // 
            // feb
            // 
            this.feb.HeaderText = "2월";
            this.feb.Name = "feb";
            this.feb.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.feb.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.feb.Width = 40;
            // 
            // mar
            // 
            this.mar.HeaderText = "3월";
            this.mar.Name = "mar";
            this.mar.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.mar.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.mar.Width = 40;
            // 
            // apr
            // 
            this.apr.HeaderText = "4월";
            this.apr.Name = "apr";
            this.apr.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.apr.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.apr.Width = 40;
            // 
            // may
            // 
            this.may.HeaderText = "5월";
            this.may.Name = "may";
            this.may.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.may.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.may.Width = 40;
            // 
            // jun
            // 
            this.jun.HeaderText = "6월";
            this.jun.Name = "jun";
            this.jun.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.jun.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.jun.Width = 40;
            // 
            // jul
            // 
            this.jul.HeaderText = "7월";
            this.jul.Name = "jul";
            this.jul.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.jul.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.jul.Width = 40;
            // 
            // aug
            // 
            this.aug.HeaderText = "8월";
            this.aug.Name = "aug";
            this.aug.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.aug.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.aug.Width = 40;
            // 
            // sep
            // 
            this.sep.HeaderText = "9월";
            this.sep.Name = "sep";
            this.sep.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.sep.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.sep.Width = 40;
            // 
            // oct
            // 
            this.oct.HeaderText = "10월";
            this.oct.Name = "oct";
            this.oct.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.oct.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.oct.Width = 40;
            // 
            // nov
            // 
            this.nov.HeaderText = "11월";
            this.nov.Name = "nov";
            this.nov.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.nov.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.nov.Width = 40;
            // 
            // dec
            // 
            this.dec.HeaderText = "12월";
            this.dec.Name = "dec";
            this.dec.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dec.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dec.Width = 40;
            // 
            // remark
            // 
            this.remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.remark.HeaderText = "비고";
            this.remark.Name = "remark";
            // 
            // manager
            // 
            this.manager.HeaderText = "담당자";
            this.manager.Name = "manager";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnInsert);
            this.panel3.Controls.Add(this.btnSearching);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 718);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1188, 40);
            this.panel3.TabIndex = 2;
            // 
            // btnInsert
            // 
            this.btnInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInsert.ForeColor = System.Drawing.Color.Blue;
            this.btnInsert.Location = new System.Drawing.Point(76, 4);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(67, 33);
            this.btnInsert.TabIndex = 1;
            this.btnInsert.Text = "등록(A)";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.Location = new System.Drawing.Point(3, 4);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(67, 33);
            this.btnSearching.TabIndex = 0;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(149, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(67, 33);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // ContractRecommendationManager2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1188, 758);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "ContractRecommendationManager2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "조업/계약시즌 관리";
            this.Load += new System.EventHandler(this.ContractRecommendationManager2_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ContractRecommendationManager2_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Label label2;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvProduct;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.TextBox txtOperating;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtContract;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn division;
        private System.Windows.Forms.DataGridViewCheckBoxColumn jan;
        private System.Windows.Forms.DataGridViewCheckBoxColumn feb;
        private System.Windows.Forms.DataGridViewCheckBoxColumn mar;
        private System.Windows.Forms.DataGridViewCheckBoxColumn apr;
        private System.Windows.Forms.DataGridViewCheckBoxColumn may;
        private System.Windows.Forms.DataGridViewCheckBoxColumn jun;
        private System.Windows.Forms.DataGridViewCheckBoxColumn jul;
        private System.Windows.Forms.DataGridViewCheckBoxColumn aug;
        private System.Windows.Forms.DataGridViewCheckBoxColumn sep;
        private System.Windows.Forms.DataGridViewCheckBoxColumn oct;
        private System.Windows.Forms.DataGridViewCheckBoxColumn nov;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dec;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn manager;
    }
}