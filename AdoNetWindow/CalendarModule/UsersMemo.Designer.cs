namespace AdoNetWindow.CalendarModule
{
    partial class UsersMemo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UsersMemo));
            this.txtContents = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbUpdatetime = new System.Windows.Forms.Label();
            this.lbid = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.nudSize = new System.Windows.Forms.NumericUpDown();
            this.cbItalic = new System.Windows.Forms.ComboBox();
            this.btnFont = new System.Windows.Forms.Button();
            this.btnFontColor = new System.Windows.Forms.Button();
            this.btnColor = new System.Windows.Forms.Button();
            this.lbPay = new System.Windows.Forms.Label();
            this.cbBank = new System.Windows.Forms.ComboBox();
            this.cbCurrency = new System.Windows.Forms.ComboBox();
            this.txtPayAmount = new System.Windows.Forms.TextBox();
            this.cbPayStatus = new System.Windows.Forms.ComboBox();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnPaymentDateCalendar = new System.Windows.Forms.Button();
            this.txtPaymentDate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSize)).BeginInit();
            this.SuspendLayout();
            // 
            // txtContents
            // 
            this.txtContents.Location = new System.Drawing.Point(12, 117);
            this.txtContents.Multiline = true;
            this.txtContents.Name = "txtContents";
            this.txtContents.Size = new System.Drawing.Size(330, 280);
            this.txtContents.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(184, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "작성자";
            // 
            // lbUpdatetime
            // 
            this.lbUpdatetime.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbUpdatetime.Location = new System.Drawing.Point(142, 403);
            this.lbUpdatetime.Name = "lbUpdatetime";
            this.lbUpdatetime.Size = new System.Drawing.Size(200, 12);
            this.lbUpdatetime.TabIndex = 4;
            this.lbUpdatetime.Text = "NULL";
            this.lbUpdatetime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbid
            // 
            this.lbid.AutoSize = true;
            this.lbid.Location = new System.Drawing.Point(150, 443);
            this.lbid.Name = "lbid";
            this.lbid.Size = new System.Drawing.Size(35, 12);
            this.lbid.TabIndex = 7;
            this.lbid.Text = "NULL";
            this.lbid.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lbid.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.nudSize);
            this.panel1.Controls.Add(this.cbItalic);
            this.panel1.Controls.Add(this.btnFont);
            this.panel1.Controls.Add(this.btnFontColor);
            this.panel1.Controls.Add(this.btnColor);
            this.panel1.Location = new System.Drawing.Point(11, 93);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(331, 23);
            this.panel1.TabIndex = 10;
            // 
            // nudSize
            // 
            this.nudSize.AutoSize = true;
            this.nudSize.Location = new System.Drawing.Point(286, 1);
            this.nudSize.Name = "nudSize";
            this.nudSize.Size = new System.Drawing.Size(42, 21);
            this.nudSize.TabIndex = 4;
            this.nudSize.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            // 
            // cbItalic
            // 
            this.cbItalic.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbItalic.FormattingEnabled = true;
            this.cbItalic.Items.AddRange(new object[] {
            "보통",
            "기울게",
            "굵게",
            "굵고 기울게"});
            this.cbItalic.Location = new System.Drawing.Point(192, 1);
            this.cbItalic.Name = "cbItalic";
            this.cbItalic.Size = new System.Drawing.Size(90, 21);
            this.cbItalic.TabIndex = 3;
            this.cbItalic.Text = "보통";
            // 
            // btnFont
            // 
            this.btnFont.Location = new System.Drawing.Point(109, 0);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(80, 23);
            this.btnFont.TabIndex = 2;
            this.btnFont.Text = "굴림";
            this.btnFont.UseVisualStyleBackColor = true;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // btnFontColor
            // 
            this.btnFontColor.BackColor = System.Drawing.Color.Black;
            this.btnFontColor.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnFontColor.ForeColor = System.Drawing.Color.White;
            this.btnFontColor.Location = new System.Drawing.Point(55, 0);
            this.btnFontColor.Name = "btnFontColor";
            this.btnFontColor.Size = new System.Drawing.Size(53, 23);
            this.btnFontColor.TabIndex = 1;
            this.btnFontColor.Text = "폰트";
            this.btnFontColor.UseVisualStyleBackColor = false;
            this.btnFontColor.Click += new System.EventHandler(this.btnFontColor_Click);
            // 
            // btnColor
            // 
            this.btnColor.BackColor = System.Drawing.Color.White;
            this.btnColor.Location = new System.Drawing.Point(0, 0);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(53, 23);
            this.btnColor.TabIndex = 0;
            this.btnColor.Text = "바탕";
            this.btnColor.UseVisualStyleBackColor = false;
            this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // lbPay
            // 
            this.lbPay.AutoSize = true;
            this.lbPay.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbPay.Location = new System.Drawing.Point(10, 69);
            this.lbPay.Name = "lbPay";
            this.lbPay.Size = new System.Drawing.Size(57, 12);
            this.lbPay.TabIndex = 11;
            this.lbPay.Text = "결제금액";
            // 
            // cbBank
            // 
            this.cbBank.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbBank.FormattingEnabled = true;
            this.cbBank.Items.AddRange(new object[] {
            "국민",
            "기업",
            "부산",
            "신한",
            "수협",
            "우리",
            "하나"});
            this.cbBank.Location = new System.Drawing.Point(247, 38);
            this.cbBank.Name = "cbBank";
            this.cbBank.Size = new System.Drawing.Size(93, 21);
            this.cbBank.TabIndex = 4;
            // 
            // cbCurrency
            // 
            this.cbCurrency.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbCurrency.FormattingEnabled = true;
            this.cbCurrency.Items.AddRange(new object[] {
            "USD",
            "KRW"});
            this.cbCurrency.Location = new System.Drawing.Point(74, 65);
            this.cbCurrency.Name = "cbCurrency";
            this.cbCurrency.Size = new System.Drawing.Size(103, 21);
            this.cbCurrency.TabIndex = 5;
            // 
            // txtPayAmount
            // 
            this.txtPayAmount.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPayAmount.Location = new System.Drawing.Point(186, 66);
            this.txtPayAmount.Name = "txtPayAmount";
            this.txtPayAmount.Size = new System.Drawing.Size(154, 22);
            this.txtPayAmount.TabIndex = 6;
            this.txtPayAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPayAmount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPayAmount_KeyDown);
            this.txtPayAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPayAmount_KeyPress);
            // 
            // cbPayStatus
            // 
            this.cbPayStatus.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbPayStatus.FormattingEnabled = true;
            this.cbPayStatus.ItemHeight = 13;
            this.cbPayStatus.Items.AddRange(new object[] {
            "미확정",
            "확정",
            "확정(마감)",
            "결제완료"});
            this.cbPayStatus.Location = new System.Drawing.Point(74, 38);
            this.cbPayStatus.Name = "cbPayStatus";
            this.cbPayStatus.Size = new System.Drawing.Size(103, 21);
            this.cbPayStatus.TabIndex = 3;
            // 
            // btnInsert
            // 
            this.btnInsert.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInsert.Location = new System.Drawing.Point(12, 430);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(64, 32);
            this.btnInsert.TabIndex = 16;
            this.btnInsert.Text = "등록(A)";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.ForeColor = System.Drawing.Color.Red;
            this.btnDelete.Location = new System.Drawing.Point(82, 430);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(64, 32);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(283, 430);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(64, 32);
            this.btnExit.TabIndex = 18;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(10, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "결제일자";
            // 
            // btnPaymentDateCalendar
            // 
            this.btnPaymentDateCalendar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPaymentDateCalendar.BackgroundImage")));
            this.btnPaymentDateCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPaymentDateCalendar.FlatAppearance.BorderSize = 0;
            this.btnPaymentDateCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPaymentDateCalendar.Location = new System.Drawing.Point(155, 7);
            this.btnPaymentDateCalendar.Name = "btnPaymentDateCalendar";
            this.btnPaymentDateCalendar.Size = new System.Drawing.Size(22, 23);
            this.btnPaymentDateCalendar.TabIndex = 1;
            this.btnPaymentDateCalendar.UseVisualStyleBackColor = true;
            this.btnPaymentDateCalendar.Click += new System.EventHandler(this.btnPaymentDateCalendar_Click);
            // 
            // txtPaymentDate
            // 
            this.txtPaymentDate.Location = new System.Drawing.Point(74, 9);
            this.txtPaymentDate.MaxLength = 10;
            this.txtPaymentDate.Name = "txtPaymentDate";
            this.txtPaymentDate.Size = new System.Drawing.Size(79, 21);
            this.txtPaymentDate.TabIndex = 0;
            this.txtPaymentDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPaymentDate_KeyDown);
            this.txtPaymentDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPaymentDate_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(10, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 12);
            this.label4.TabIndex = 98;
            this.label4.Text = "결제상태";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(184, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 12);
            this.label5.TabIndex = 100;
            this.label5.Text = "결제은행";
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(247, 9);
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(93, 21);
            this.txtManager.TabIndex = 2;
            // 
            // UsersMemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(351, 469);
            this.Controls.Add(this.txtManager);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnPaymentDateCalendar);
            this.Controls.Add(this.txtPaymentDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.cbPayStatus);
            this.Controls.Add(this.txtPayAmount);
            this.Controls.Add(this.cbCurrency);
            this.Controls.Add(this.cbBank);
            this.Controls.Add(this.lbPay);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbid);
            this.Controls.Add(this.lbUpdatetime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtContents);
            this.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "UsersMemo";
            this.Text = "기타결제";
            this.Load += new System.EventHandler(this.UsersMemo_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UsersMemo_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtContents;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbUpdatetime;
        private System.Windows.Forms.Label lbid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnColor;
        private System.Windows.Forms.Button btnFont;
        private System.Windows.Forms.Button btnFontColor;
        private System.Windows.Forms.ComboBox cbItalic;
        private System.Windows.Forms.NumericUpDown nudSize;
        private System.Windows.Forms.Label lbPay;
        private System.Windows.Forms.ComboBox cbBank;
        private System.Windows.Forms.ComboBox cbCurrency;
        private System.Windows.Forms.TextBox txtPayAmount;
        private System.Windows.Forms.ComboBox cbPayStatus;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnPaymentDateCalendar;
        private System.Windows.Forms.TextBox txtPaymentDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtManager;
    }
}