namespace AdoNetWindow.API
{
    partial class ProcessingCompany
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
            this.dgvCompany = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.txtPageNo = new System.Windows.Forms.TextBox();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSdate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInid = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvCompany
            // 
            this.dgvCompany.AllowUserToAddRows = false;
            this.dgvCompany.AllowUserToDeleteRows = false;
            this.dgvCompany.AllowUserToOrderColumns = true;
            this.dgvCompany.AllowUserToResizeRows = false;
            this.dgvCompany.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCompany.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCompany.EnableHeadersVisualStyles = false;
            this.dgvCompany.Location = new System.Drawing.Point(0, 0);
            this.dgvCompany.Name = "dgvCompany";
            this.dgvCompany.RowTemplate.Height = 23;
            this.dgvCompany.Size = new System.Drawing.Size(1142, 622);
            this.dgvCompany.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvCompany);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 31);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1142, 622);
            this.panel2.TabIndex = 7;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 653);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1142, 39);
            this.panel3.TabIndex = 8;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(3, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(70, 36);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "검색(Q)";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(79, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 36);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtInid);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.txtCompany);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtSdate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1142, 31);
            this.panel1.TabIndex = 6;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnRight);
            this.panel4.Controls.Add(this.btnLeft);
            this.panel4.Controls.Add(this.txtPageNo);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(975, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(167, 31);
            this.panel4.TabIndex = 4;
            // 
            // btnRight
            // 
            this.btnRight.Location = new System.Drawing.Point(133, 4);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(29, 23);
            this.btnRight.TabIndex = 11;
            this.btnRight.Text = ">";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.Location = new System.Drawing.Point(65, 4);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(29, 23);
            this.btnLeft.TabIndex = 10;
            this.btnLeft.Text = "<";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // txtPageNo
            // 
            this.txtPageNo.Location = new System.Drawing.Point(97, 5);
            this.txtPageNo.Name = "txtPageNo";
            this.txtPageNo.Size = new System.Drawing.Size(32, 21);
            this.txtPageNo.TabIndex = 8;
            this.txtPageNo.Text = "1";
            this.txtPageNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPageNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPageNo_KeyPress);
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(220, 5);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(100, 21);
            this.txtCompany.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(173, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "업소명";
            // 
            // txtSdate
            // 
            this.txtSdate.Location = new System.Drawing.Point(67, 5);
            this.txtSdate.Name = "txtSdate";
            this.txtSdate.Size = new System.Drawing.Size(100, 21);
            this.txtSdate.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "변경일자";
            // 
            // txtInid
            // 
            this.txtInid.Location = new System.Drawing.Point(404, 5);
            this.txtInid.Name = "txtInid";
            this.txtInid.Size = new System.Drawing.Size(100, 21);
            this.txtInid.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(333, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "인허가번호";
            // 
            // ProcessingCompany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 692);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "ProcessingCompany";
            this.Text = "ProcessingCompany";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProcessingCompany_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompany)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvCompany;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.TextBox txtPageNo;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSdate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInid;
        private System.Windows.Forms.Label label3;
    }
}