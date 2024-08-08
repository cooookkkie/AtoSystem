namespace AdoNetWindow.SaleManagement.AddSaleCompany
{
    partial class FinanceManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FinanceManager));
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtYear = new System.Windows.Forms.TextBox();
            this.txtCapitalAmount = new System.Windows.Forms.TextBox();
            this.txtDebtAmount = new System.Windows.Forms.TextBox();
            this.txtSalesAmount = new System.Windows.Forms.TextBox();
            this.txtNetIncomeAmount = new System.Windows.Forms.TextBox();
            this.btnRegistration = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "연도";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "자본총계";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "부채총계";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "매출총액";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "당기순이익";
            // 
            // txtYear
            // 
            this.txtYear.Location = new System.Drawing.Point(190, 12);
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(100, 21);
            this.txtYear.TabIndex = 6;
            this.txtYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtCapitalAmount
            // 
            this.txtCapitalAmount.Location = new System.Drawing.Point(108, 40);
            this.txtCapitalAmount.Name = "txtCapitalAmount";
            this.txtCapitalAmount.Size = new System.Drawing.Size(182, 21);
            this.txtCapitalAmount.TabIndex = 8;
            this.txtCapitalAmount.Text = "0";
            this.txtCapitalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCapitalAmount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCapitalAmount_KeyDown);
            // 
            // txtDebtAmount
            // 
            this.txtDebtAmount.Location = new System.Drawing.Point(108, 67);
            this.txtDebtAmount.Name = "txtDebtAmount";
            this.txtDebtAmount.Size = new System.Drawing.Size(182, 21);
            this.txtDebtAmount.TabIndex = 9;
            this.txtDebtAmount.Text = "0";
            this.txtDebtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDebtAmount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCapitalAmount_KeyDown);
            // 
            // txtSalesAmount
            // 
            this.txtSalesAmount.Location = new System.Drawing.Point(108, 93);
            this.txtSalesAmount.Name = "txtSalesAmount";
            this.txtSalesAmount.Size = new System.Drawing.Size(182, 21);
            this.txtSalesAmount.TabIndex = 10;
            this.txtSalesAmount.Text = "0";
            this.txtSalesAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSalesAmount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCapitalAmount_KeyDown);
            // 
            // txtNetIncomeAmount
            // 
            this.txtNetIncomeAmount.Location = new System.Drawing.Point(108, 120);
            this.txtNetIncomeAmount.Name = "txtNetIncomeAmount";
            this.txtNetIncomeAmount.Size = new System.Drawing.Size(182, 21);
            this.txtNetIncomeAmount.TabIndex = 11;
            this.txtNetIncomeAmount.Text = "0";
            this.txtNetIncomeAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNetIncomeAmount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCapitalAmount_KeyDown);
            // 
            // btnRegistration
            // 
            this.btnRegistration.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRegistration.ForeColor = System.Drawing.Color.Blue;
            this.btnRegistration.Location = new System.Drawing.Point(12, 159);
            this.btnRegistration.Name = "btnRegistration";
            this.btnRegistration.Size = new System.Drawing.Size(72, 37);
            this.btnRegistration.TabIndex = 21;
            this.btnRegistration.Text = "등록 (A)";
            this.btnRegistration.UseVisualStyleBackColor = true;
            this.btnRegistration.Click += new System.EventHandler(this.btnRegistration_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(217, 159);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(73, 37);
            this.btnExit.TabIndex = 22;
            this.btnExit.Text = "닫기 (X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.ForeColor = System.Drawing.Color.Red;
            this.btnDelete.Location = new System.Drawing.Point(90, 159);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(72, 37);
            this.btnDelete.TabIndex = 23;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // FinanceManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 205);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRegistration);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.txtNetIncomeAmount);
            this.Controls.Add(this.txtSalesAmount);
            this.Controls.Add(this.txtDebtAmount);
            this.Controls.Add(this.txtCapitalAmount);
            this.Controls.Add(this.txtYear);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FinanceManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "재무재표 등록";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FinanceManager_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtYear;
        private System.Windows.Forms.TextBox txtCapitalAmount;
        private System.Windows.Forms.TextBox txtDebtAmount;
        private System.Windows.Forms.TextBox txtSalesAmount;
        private System.Windows.Forms.TextBox txtNetIncomeAmount;
        private System.Windows.Forms.Button btnRegistration;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnDelete;
    }
}