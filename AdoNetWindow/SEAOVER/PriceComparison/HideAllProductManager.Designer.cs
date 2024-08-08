namespace AdoNetWindow.SEAOVER.PriceComparison
{
    partial class HideAllProductManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HideAllProductManager));
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbStock = new System.Windows.Forms.RadioButton();
            this.cbDown = new System.Windows.Forms.CheckBox();
            this.lbId = new System.Windows.Forms.Label();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.cbUp = new System.Windows.Forms.CheckBox();
            this.txtDownStock = new System.Windows.Forms.TextBox();
            this.txtUpStock = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUntildate = new System.Windows.Forms.TextBox();
            this.btnUntildateCalendar = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.txtRemark);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.txtUntildate);
            this.panel2.Controls.Add(this.btnUntildateCalendar);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(580, 252);
            this.panel2.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(12, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 12);
            this.label4.TabIndex = 108;
            this.label4.Text = "비고";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(11, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 12);
            this.label10.TabIndex = 97;
            this.label10.Text = "제외일자";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(12, 57);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(263, 152);
            this.txtRemark.TabIndex = 2;
            this.txtRemark.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.rbStock);
            this.groupBox1.Controls.Add(this.cbDown);
            this.groupBox1.Controls.Add(this.lbId);
            this.groupBox1.Controls.Add(this.rbAll);
            this.groupBox1.Controls.Add(this.cbUp);
            this.groupBox1.Controls.Add(this.txtDownStock);
            this.groupBox1.Controls.Add(this.txtUpStock);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(304, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(272, 204);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "설정";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(24, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 12);
            this.label1.TabIndex = 105;
            this.label1.Text = "*제외일자까지 조건없이 모든계산 제외";
            // 
            // rbStock
            // 
            this.rbStock.AutoSize = true;
            this.rbStock.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbStock.Location = new System.Drawing.Point(8, 85);
            this.rbStock.Name = "rbStock";
            this.rbStock.Size = new System.Drawing.Size(119, 16);
            this.rbStock.TabIndex = 2;
            this.rbStock.Text = "재고수 설정제외";
            this.rbStock.UseVisualStyleBackColor = true;
            this.rbStock.CheckedChanged += new System.EventHandler(this.rbStock_CheckedChanged);
            // 
            // cbDown
            // 
            this.cbDown.AutoSize = true;
            this.cbDown.Enabled = false;
            this.cbDown.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbDown.ForeColor = System.Drawing.Color.Blue;
            this.cbDown.Location = new System.Drawing.Point(26, 162);
            this.cbDown.Name = "cbDown";
            this.cbDown.Size = new System.Drawing.Size(78, 16);
            this.cbDown.TabIndex = 5;
            this.cbDown.Text = "▼ (내림)";
            this.cbDown.UseVisualStyleBackColor = true;
            this.cbDown.CheckedChanged += new System.EventHandler(this.cbDown_CheckedChanged);
            // 
            // lbId
            // 
            this.lbId.Location = new System.Drawing.Point(212, 19);
            this.lbId.Name = "lbId";
            this.lbId.Size = new System.Drawing.Size(50, 12);
            this.lbId.TabIndex = 106;
            this.lbId.Text = "NULL";
            this.lbId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbId.Visible = false;
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Checked = true;
            this.rbAll.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbAll.Location = new System.Drawing.Point(8, 29);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(93, 16);
            this.rbAll.TabIndex = 1;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "무조건 제외";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // cbUp
            // 
            this.cbUp.AutoSize = true;
            this.cbUp.Enabled = false;
            this.cbUp.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbUp.ForeColor = System.Drawing.Color.Red;
            this.cbUp.Location = new System.Drawing.Point(26, 112);
            this.cbUp.Name = "cbUp";
            this.cbUp.Size = new System.Drawing.Size(78, 16);
            this.cbUp.TabIndex = 3;
            this.cbUp.Text = "▲ (올림)";
            this.cbUp.UseVisualStyleBackColor = true;
            this.cbUp.CheckedChanged += new System.EventHandler(this.cbUp_CheckedChanged);
            // 
            // txtDownStock
            // 
            this.txtDownStock.Enabled = false;
            this.txtDownStock.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtDownStock.Location = new System.Drawing.Point(139, 157);
            this.txtDownStock.Name = "txtDownStock";
            this.txtDownStock.Size = new System.Drawing.Size(123, 21);
            this.txtDownStock.TabIndex = 6;
            this.txtDownStock.Text = "0";
            this.txtDownStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtUpStock
            // 
            this.txtUpStock.Enabled = false;
            this.txtUpStock.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtUpStock.Location = new System.Drawing.Point(139, 107);
            this.txtUpStock.Name = "txtUpStock";
            this.txtUpStock.Size = new System.Drawing.Size(123, 21);
            this.txtUpStock.TabIndex = 4;
            this.txtUpStock.Text = "0";
            this.txtUpStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(24, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(195, 12);
            this.label3.TabIndex = 100;
            this.label3.Text = "*설정한 재고수 이상일 경우만 적용";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(24, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(195, 12);
            this.label2.TabIndex = 99;
            this.label2.Text = "*설정한 재고수 이하일 경우만 적용";
            // 
            // txtUntildate
            // 
            this.txtUntildate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtUntildate.Location = new System.Drawing.Point(147, 18);
            this.txtUntildate.Name = "txtUntildate";
            this.txtUntildate.Size = new System.Drawing.Size(100, 21);
            this.txtUntildate.TabIndex = 0;
            this.txtUntildate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUntildate_KeyPress);
            // 
            // btnUntildateCalendar
            // 
            this.btnUntildateCalendar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUntildateCalendar.BackgroundImage")));
            this.btnUntildateCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnUntildateCalendar.FlatAppearance.BorderSize = 0;
            this.btnUntildateCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUntildateCalendar.Location = new System.Drawing.Point(253, 16);
            this.btnUntildateCalendar.Name = "btnUntildateCalendar";
            this.btnUntildateCalendar.Size = new System.Drawing.Size(22, 23);
            this.btnUntildateCalendar.TabIndex = 1;
            this.btnUntildateCalendar.UseVisualStyleBackColor = true;
            this.btnUntildateCalendar.Click += new System.EventHandler(this.btnUntildateCalendar_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnInsert);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 216);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(580, 36);
            this.panel3.TabIndex = 19;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(510, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(65, 30);
            this.btnExit.TabIndex = 13;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInsert.Location = new System.Drawing.Point(3, 2);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(65, 30);
            this.btnInsert.TabIndex = 12;
            this.btnInsert.Text = "확인(A)";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // HideAllProductManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 252);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HideAllProductManager";
            this.Text = "제외설정";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HideAllProductManager_KeyDown);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RichTextBox txtRemark;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbStock;
        private System.Windows.Forms.CheckBox cbDown;
        private System.Windows.Forms.Label lbId;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.CheckBox cbUp;
        private System.Windows.Forms.TextBox txtDownStock;
        private System.Windows.Forms.TextBox txtUpStock;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUntildate;
        private System.Windows.Forms.Button btnUntildateCalendar;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnInsert;
    }
}