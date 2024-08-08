namespace AdoNetWindow.SaleManagement
{
    partial class SelectCompanyDivision
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectCompanyDivision));
            this.rbCommon = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.rbRandom = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.rbPotential1 = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.rbPotential2 = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.rbNotSales = new System.Windows.Forms.RadioButton();
            this.cbNotHandling = new System.Windows.Forms.CheckBox();
            this.cbNotSendFax = new System.Windows.Forms.CheckBox();
            this.cbOutOfBusiness = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAddCompany = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cbDuplicateCommonData = new System.Windows.Forms.CheckBox();
            this.cbDuplicatePotential2 = new System.Windows.Forms.CheckBox();
            this.cbDuplicatePotential1 = new System.Windows.Forms.CheckBox();
            this.cbDuplicateMyData = new System.Windows.Forms.CheckBox();
            this.cbPotentialCountLimit = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbDuplicateTrading = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // rbCommon
            // 
            this.rbCommon.AutoSize = true;
            this.rbCommon.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbCommon.Location = new System.Drawing.Point(24, 39);
            this.rbCommon.Name = "rbCommon";
            this.rbCommon.Size = new System.Drawing.Size(125, 25);
            this.rbCommon.TabIndex = 0;
            this.rbCommon.TabStop = true;
            this.rbCommon.Text = "공용DATA";
            this.rbCommon.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(155, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(287, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "*담당자가 지정되지 않은 공용 데이터로 등록됩니다.";
            // 
            // rbRandom
            // 
            this.rbRandom.AutoSize = true;
            this.rbRandom.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbRandom.Location = new System.Drawing.Point(24, 80);
            this.rbRandom.Name = "rbRandom";
            this.rbRandom.Size = new System.Drawing.Size(103, 25);
            this.rbRandom.TabIndex = 2;
            this.rbRandom.TabStop = true;
            this.rbRandom.Text = "내DATA";
            this.rbRandom.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(133, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(231, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "*등록한 유저의 담당거래처로 등록됩니다.";
            // 
            // rbPotential1
            // 
            this.rbPotential1.AutoSize = true;
            this.rbPotential1.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbPotential1.Location = new System.Drawing.Point(24, 125);
            this.rbPotential1.Name = "rbPotential1";
            this.rbPotential1.Size = new System.Drawing.Size(84, 25);
            this.rbPotential1.TabIndex = 4;
            this.rbPotential1.TabStop = true;
            this.rbPotential1.Text = "잠재1";
            this.rbPotential1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(114, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(282, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "*등록한 유저의 담당거래처 중 잠재1로 등록됩니다.";
            // 
            // rbPotential2
            // 
            this.rbPotential2.AutoSize = true;
            this.rbPotential2.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbPotential2.Location = new System.Drawing.Point(24, 168);
            this.rbPotential2.Name = "rbPotential2";
            this.rbPotential2.Size = new System.Drawing.Size(84, 25);
            this.rbPotential2.TabIndex = 6;
            this.rbPotential2.TabStop = true;
            this.rbPotential2.Text = "잠재2";
            this.rbPotential2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(114, 178);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(282, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "*등록한 유저의 담당거래처 중 잠재2로 등록됩니다.";
            // 
            // rbNotSales
            // 
            this.rbNotSales.AutoSize = true;
            this.rbNotSales.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbNotSales.Location = new System.Drawing.Point(24, 212);
            this.rbNotSales.Name = "rbNotSales";
            this.rbNotSales.Size = new System.Drawing.Size(190, 25);
            this.rbNotSales.TabIndex = 8;
            this.rbNotSales.TabStop = true;
            this.rbNotSales.Text = "영업금지 거래처";
            this.rbNotSales.UseVisualStyleBackColor = true;
            this.rbNotSales.CheckedChanged += new System.EventHandler(this.rbNotSales_CheckedChanged);
            // 
            // cbNotHandling
            // 
            this.cbNotHandling.AutoSize = true;
            this.cbNotHandling.Enabled = false;
            this.cbNotHandling.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbNotHandling.Location = new System.Drawing.Point(43, 253);
            this.cbNotHandling.Name = "cbNotHandling";
            this.cbNotHandling.Size = new System.Drawing.Size(77, 23);
            this.cbNotHandling.TabIndex = 9;
            this.cbNotHandling.Text = "취급X";
            this.cbNotHandling.UseVisualStyleBackColor = true;
            // 
            // cbNotSendFax
            // 
            this.cbNotSendFax.AutoSize = true;
            this.cbNotSendFax.Enabled = false;
            this.cbNotSendFax.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbNotSendFax.Location = new System.Drawing.Point(157, 253);
            this.cbNotSendFax.Name = "cbNotSendFax";
            this.cbNotSendFax.Size = new System.Drawing.Size(77, 23);
            this.cbNotSendFax.TabIndex = 10;
            this.cbNotSendFax.Text = "팩스X";
            this.cbNotSendFax.UseVisualStyleBackColor = true;
            // 
            // cbOutOfBusiness
            // 
            this.cbOutOfBusiness.AutoSize = true;
            this.cbOutOfBusiness.Enabled = false;
            this.cbOutOfBusiness.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbOutOfBusiness.Location = new System.Drawing.Point(278, 253);
            this.cbOutOfBusiness.Name = "cbOutOfBusiness";
            this.cbOutOfBusiness.Size = new System.Drawing.Size(66, 23);
            this.cbOutOfBusiness.TabIndex = 11;
            this.cbOutOfBusiness.Text = "폐업";
            this.cbOutOfBusiness.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(220, 222);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(282, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "*아래 세가지 유형중 하나 이상 필수";
            // 
            // btnAddCompany
            // 
            this.btnAddCompany.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAddCompany.ForeColor = System.Drawing.Color.Blue;
            this.btnAddCompany.Location = new System.Drawing.Point(12, 294);
            this.btnAddCompany.Name = "btnAddCompany";
            this.btnAddCompany.Size = new System.Drawing.Size(120, 37);
            this.btnAddCompany.TabIndex = 17;
            this.btnAddCompany.Text = "등록 (ENTER)";
            this.btnAddCompany.UseVisualStyleBackColor = true;
            this.btnAddCompany.Click += new System.EventHandler(this.btnAddCompany_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(362, 294);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(89, 37);
            this.btnExit.TabIndex = 18;
            this.btnExit.Text = "취소 (ESC)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(12, 377);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(232, 21);
            this.label6.TabIndex = 21;
            this.label6.Text = "중복 거래처 등록 여부";
            this.label6.Visible = false;
            // 
            // cbDuplicateCommonData
            // 
            this.cbDuplicateCommonData.AutoSize = true;
            this.cbDuplicateCommonData.Checked = true;
            this.cbDuplicateCommonData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDuplicateCommonData.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbDuplicateCommonData.Location = new System.Drawing.Point(43, 415);
            this.cbDuplicateCommonData.Name = "cbDuplicateCommonData";
            this.cbDuplicateCommonData.Size = new System.Drawing.Size(111, 23);
            this.cbDuplicateCommonData.TabIndex = 12;
            this.cbDuplicateCommonData.Text = "공용DATA";
            this.cbDuplicateCommonData.UseVisualStyleBackColor = true;
            this.cbDuplicateCommonData.Visible = false;
            // 
            // cbDuplicatePotential2
            // 
            this.cbDuplicatePotential2.AutoSize = true;
            this.cbDuplicatePotential2.Checked = true;
            this.cbDuplicatePotential2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDuplicatePotential2.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbDuplicatePotential2.Location = new System.Drawing.Point(366, 415);
            this.cbDuplicatePotential2.Name = "cbDuplicatePotential2";
            this.cbDuplicatePotential2.Size = new System.Drawing.Size(76, 23);
            this.cbDuplicatePotential2.TabIndex = 16;
            this.cbDuplicatePotential2.Text = "잠재2";
            this.cbDuplicatePotential2.UseVisualStyleBackColor = true;
            this.cbDuplicatePotential2.Visible = false;
            // 
            // cbDuplicatePotential1
            // 
            this.cbDuplicatePotential1.AutoSize = true;
            this.cbDuplicatePotential1.Checked = true;
            this.cbDuplicatePotential1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDuplicatePotential1.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbDuplicatePotential1.Location = new System.Drawing.Point(278, 415);
            this.cbDuplicatePotential1.Name = "cbDuplicatePotential1";
            this.cbDuplicatePotential1.Size = new System.Drawing.Size(76, 23);
            this.cbDuplicatePotential1.TabIndex = 15;
            this.cbDuplicatePotential1.Text = "잠재1";
            this.cbDuplicatePotential1.UseVisualStyleBackColor = true;
            this.cbDuplicatePotential1.Visible = false;
            // 
            // cbDuplicateMyData
            // 
            this.cbDuplicateMyData.AutoSize = true;
            this.cbDuplicateMyData.Checked = true;
            this.cbDuplicateMyData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDuplicateMyData.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbDuplicateMyData.Location = new System.Drawing.Point(170, 415);
            this.cbDuplicateMyData.Name = "cbDuplicateMyData";
            this.cbDuplicateMyData.Size = new System.Drawing.Size(92, 23);
            this.cbDuplicateMyData.TabIndex = 13;
            this.cbDuplicateMyData.Text = "내DATA";
            this.cbDuplicateMyData.UseVisualStyleBackColor = true;
            this.cbDuplicateMyData.Visible = false;
            // 
            // cbPotentialCountLimit
            // 
            this.cbPotentialCountLimit.FormattingEnabled = true;
            this.cbPotentialCountLimit.Items.AddRange(new object[] {
            "3",
            "2",
            "1"});
            this.cbPotentialCountLimit.Location = new System.Drawing.Point(411, 389);
            this.cbPotentialCountLimit.Name = "cbPotentialCountLimit";
            this.cbPotentialCountLimit.Size = new System.Drawing.Size(31, 20);
            this.cbPotentialCountLimit.TabIndex = 14;
            this.cbPotentialCountLimit.Text = "3";
            this.cbPotentialCountLimit.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(278, 392);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(127, 12);
            this.label7.TabIndex = 27;
            this.label7.Text = "*잠재거래처 중복 제한";
            this.label7.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(12, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(128, 21);
            this.label8.TabIndex = 28;
            this.label8.Text = "거래처 구분";
            // 
            // cbDuplicateTrading
            // 
            this.cbDuplicateTrading.AutoSize = true;
            this.cbDuplicateTrading.Checked = true;
            this.cbDuplicateTrading.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDuplicateTrading.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbDuplicateTrading.Location = new System.Drawing.Point(43, 444);
            this.cbDuplicateTrading.Name = "cbDuplicateTrading";
            this.cbDuplicateTrading.Size = new System.Drawing.Size(85, 23);
            this.cbDuplicateTrading.TabIndex = 29;
            this.cbDuplicateTrading.Text = "거래중";
            this.cbDuplicateTrading.UseVisualStyleBackColor = true;
            this.cbDuplicateTrading.Visible = false;
            // 
            // SelectCompanyDivision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 345);
            this.Controls.Add(this.cbDuplicateTrading);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cbPotentialCountLimit);
            this.Controls.Add(this.cbDuplicateMyData);
            this.Controls.Add(this.cbDuplicateCommonData);
            this.Controls.Add(this.cbDuplicatePotential2);
            this.Controls.Add(this.cbDuplicatePotential1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnAddCompany);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbOutOfBusiness);
            this.Controls.Add(this.cbNotSendFax);
            this.Controls.Add(this.cbNotHandling);
            this.Controls.Add(this.rbNotSales);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rbPotential2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rbPotential1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rbRandom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbCommon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectCompanyDivision";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = ".";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SelectCompanyDivision_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbCommon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbRandom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbPotential1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbPotential2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rbNotSales;
        private System.Windows.Forms.CheckBox cbNotHandling;
        private System.Windows.Forms.CheckBox cbNotSendFax;
        private System.Windows.Forms.CheckBox cbOutOfBusiness;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAddCompany;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cbDuplicateCommonData;
        private System.Windows.Forms.CheckBox cbDuplicatePotential2;
        private System.Windows.Forms.CheckBox cbDuplicatePotential1;
        private System.Windows.Forms.CheckBox cbDuplicateMyData;
        private System.Windows.Forms.ComboBox cbPotentialCountLimit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbDuplicateTrading;
    }
}