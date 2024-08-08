namespace AdoNetWindow.CalendarModule.LoanManager
{
    partial class LoanManger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoanManger));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDivision = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAtsightLimit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUsanceLimit = new System.Windows.Forms.TextBox();
            this.txtBank = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.lvBankList = new System.Windows.Forms.ListView();
            this.bank = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.division = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.usance_loan_limit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.atsight_loan_limit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtDivision);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtAtsightLimit);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtUsanceLimit);
            this.panel1.Controls.Add(this.txtBank);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(389, 51);
            this.panel1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(87, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "구분";
            // 
            // txtDivision
            // 
            this.txtDivision.Location = new System.Drawing.Point(87, 23);
            this.txtDivision.Name = "txtDivision";
            this.txtDivision.Size = new System.Drawing.Size(77, 21);
            this.txtDivision.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(272, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "일람불 한도";
            // 
            // txtAtsightLimit
            // 
            this.txtAtsightLimit.Location = new System.Drawing.Point(274, 23);
            this.txtAtsightLimit.Name = "txtAtsightLimit";
            this.txtAtsightLimit.Size = new System.Drawing.Size(108, 21);
            this.txtAtsightLimit.TabIndex = 3;
            this.txtAtsightLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtAtsightLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLoanLimit_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(163, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "유산스 한도";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "은행";
            // 
            // txtUsanceLimit
            // 
            this.txtUsanceLimit.Location = new System.Drawing.Point(165, 23);
            this.txtUsanceLimit.Name = "txtUsanceLimit";
            this.txtUsanceLimit.Size = new System.Drawing.Size(108, 21);
            this.txtUsanceLimit.TabIndex = 2;
            this.txtUsanceLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtUsanceLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLoanLimit_KeyPress);
            // 
            // txtBank
            // 
            this.txtBank.Location = new System.Drawing.Point(9, 23);
            this.txtBank.Name = "txtBank";
            this.txtBank.Size = new System.Drawing.Size(77, 21);
            this.txtBank.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Controls.Add(this.btnInsert);
            this.panel2.Controls.Add(this.lvBankList);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 51);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(389, 306);
            this.panel2.TabIndex = 1;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(300, 274);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(77, 26);
            this.btnExit.TabIndex = 11;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.ForeColor = System.Drawing.Color.Red;
            this.btnDelete.Location = new System.Drawing.Point(87, 274);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(77, 26);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "삭제(D)";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInsert.ForeColor = System.Drawing.Color.Blue;
            this.btnInsert.Location = new System.Drawing.Point(9, 274);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(77, 26);
            this.btnInsert.TabIndex = 9;
            this.btnInsert.Text = "등록(A)";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // lvBankList
            // 
            this.lvBankList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.bank,
            this.division,
            this.usance_loan_limit,
            this.atsight_loan_limit});
            this.lvBankList.FullRowSelect = true;
            this.lvBankList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvBankList.HideSelection = false;
            this.lvBankList.Location = new System.Drawing.Point(9, 6);
            this.lvBankList.Name = "lvBankList";
            this.lvBankList.Size = new System.Drawing.Size(373, 262);
            this.lvBankList.TabIndex = 8;
            this.lvBankList.UseCompatibleStateImageBehavior = false;
            this.lvBankList.View = System.Windows.Forms.View.Details;
            this.lvBankList.SelectedIndexChanged += new System.EventHandler(this.lvBankList_SelectedIndexChanged);
            // 
            // bank
            // 
            this.bank.Text = "은행";
            this.bank.Width = 80;
            // 
            // division
            // 
            this.division.Text = "구분";
            this.division.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.division.Width = 80;
            // 
            // usance_loan_limit
            // 
            this.usance_loan_limit.Text = "대출한도";
            this.usance_loan_limit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.usance_loan_limit.Width = 105;
            // 
            // atsight_loan_limit
            // 
            this.atsight_loan_limit.Text = "일람불";
            this.atsight_loan_limit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.atsight_loan_limit.Width = 105;
            // 
            // LoanManger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 357);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "LoanManger";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "신용장 한도";
            this.Load += new System.EventHandler(this.LoanManger_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoanManger_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUsanceLimit;
        private System.Windows.Forms.TextBox txtBank;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.ListView lvBankList;
        private System.Windows.Forms.ColumnHeader bank;
        private System.Windows.Forms.ColumnHeader usance_loan_limit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAtsightLimit;
        private System.Windows.Forms.ColumnHeader atsight_loan_limit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDivision;
        private System.Windows.Forms.ColumnHeader division;
    }
}