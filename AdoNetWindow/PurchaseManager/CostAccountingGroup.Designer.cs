namespace AdoNetWindow.PurchaseManager
{
    partial class CostAccountingGroup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CostAccountingGroup));
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExcel = new System.Windows.Forms.Button();
            this.btnBatchInput = new System.Windows.Forms.Button();
            this.btnProduct = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.pnMain = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbPerBox = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.rbSalesPrice = new System.Windows.Forms.RadioButton();
            this.rbPurchaseprice = new System.Windows.Forms.RadioButton();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label17 = new System.Windows.Forms.Label();
            this.cbSortType = new System.Windows.Forms.ComboBox();
            this.txtTrq = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtExchangeRate = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnPreview = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.btnBatchInput);
            this.panel2.Controls.Add(this.btnProduct);
            this.panel2.Controls.Add(this.btnAdd);
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Controls.Add(this.btnRefresh);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 834);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1662, 42);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnPreview);
            this.panel3.Controls.Add(this.btnExcel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(1387, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(273, 40);
            this.panel3.TabIndex = 6;
            // 
            // btnExcel
            // 
            this.btnExcel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExcel.ForeColor = System.Drawing.Color.Black;
            this.btnExcel.Location = new System.Drawing.Point(130, 2);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(64, 37);
            this.btnExcel.TabIndex = 4;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnBatchInput
            // 
            this.btnBatchInput.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBatchInput.Location = new System.Drawing.Point(182, 2);
            this.btnBatchInput.Name = "btnBatchInput";
            this.btnBatchInput.Size = new System.Drawing.Size(113, 37);
            this.btnBatchInput.TabIndex = 2;
            this.btnBatchInput.Text = "수량 일괄입력";
            this.btnBatchInput.UseVisualStyleBackColor = true;
            this.btnBatchInput.Click += new System.EventHandler(this.btnBatchInput_Click);
            // 
            // btnProduct
            // 
            this.btnProduct.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnProduct.ForeColor = System.Drawing.Color.Black;
            this.btnProduct.Location = new System.Drawing.Point(79, 2);
            this.btnProduct.Name = "btnProduct";
            this.btnProduct.Size = new System.Drawing.Size(97, 37);
            this.btnProduct.TabIndex = 1;
            this.btnProduct.Text = "원가선택(F4)";
            this.btnProduct.UseVisualStyleBackColor = true;
            this.btnProduct.Click += new System.EventHandler(this.btnProduct_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAdd.ForeColor = System.Drawing.Color.Blue;
            this.btnAdd.Location = new System.Drawing.Point(3, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(70, 37);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "추가(A)";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(368, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 37);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRefresh.ForeColor = System.Drawing.Color.Red;
            this.btnRefresh.Location = new System.Drawing.Point(300, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(62, 37);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "초기화";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // pnMain
            // 
            this.pnMain.AutoScroll = true;
            this.pnMain.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMain.Location = new System.Drawing.Point(0, 27);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new System.Drawing.Size(1662, 807);
            this.pnMain.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cbPerBox);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.rbSalesPrice);
            this.panel1.Controls.Add(this.rbPurchaseprice);
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Controls.Add(this.txtTrq);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtExchangeRate);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1662, 27);
            this.panel1.TabIndex = 2;
            // 
            // cbPerBox
            // 
            this.cbPerBox.AutoSize = true;
            this.cbPerBox.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbPerBox.Location = new System.Drawing.Point(585, 4);
            this.cbPerBox.Name = "cbPerBox";
            this.cbPerBox.Size = new System.Drawing.Size(103, 16);
            this.cbPerBox.TabIndex = 115;
            this.cbPerBox.Text = "단품단가(F7)";
            this.cbPerBox.UseVisualStyleBackColor = true;
            this.cbPerBox.Visible = false;
            this.cbPerBox.CheckedChanged += new System.EventHandler(this.cbPerBox_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(298, 6);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 12);
            this.label10.TabIndex = 114;
            this.label10.Text = "마진율 계산(F6)";
            // 
            // rbSalesPrice
            // 
            this.rbSalesPrice.AutoSize = true;
            this.rbSalesPrice.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbSalesPrice.Location = new System.Drawing.Point(483, 4);
            this.rbSalesPrice.Name = "rbSalesPrice";
            this.rbSalesPrice.Size = new System.Drawing.Size(83, 16);
            this.rbSalesPrice.TabIndex = 113;
            this.rbSalesPrice.Text = "국내판매가";
            this.rbSalesPrice.UseVisualStyleBackColor = true;
            this.rbSalesPrice.CheckedChanged += new System.EventHandler(this.rbPurchaseprice_CheckedChanged);
            // 
            // rbPurchaseprice
            // 
            this.rbPurchaseprice.AutoSize = true;
            this.rbPurchaseprice.Checked = true;
            this.rbPurchaseprice.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbPurchaseprice.Location = new System.Drawing.Point(406, 4);
            this.rbPurchaseprice.Name = "rbPurchaseprice";
            this.rbPurchaseprice.Size = new System.Drawing.Size(71, 16);
            this.rbPurchaseprice.TabIndex = 112;
            this.rbPurchaseprice.TabStop = true;
            this.rbPurchaseprice.Text = "매입단가";
            this.rbPurchaseprice.UseVisualStyleBackColor = true;
            this.rbPurchaseprice.CheckedChanged += new System.EventHandler(this.rbPurchaseprice_CheckedChanged);
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label17);
            this.panel8.Controls.Add(this.cbSortType);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel8.Location = new System.Drawing.Point(1265, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(395, 25);
            this.panel8.TabIndex = 111;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label17.Location = new System.Drawing.Point(161, 6);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(57, 12);
            this.label17.TabIndex = 52;
            this.label17.Text = "정렬기준";
            // 
            // cbSortType
            // 
            this.cbSortType.FormattingEnabled = true;
            this.cbSortType.Items.AddRange(new object[] {
            "품명+원산지+규격",
            "거래처+품명+원산지+규격",
            "품명+원산지+규격+오퍼가",
            "오퍼가+품명+원산지+규격",
            "품명+원산지+규격+오퍼일자",
            "오퍼일자+품명+원산지+규격"});
            this.cbSortType.Location = new System.Drawing.Point(224, 2);
            this.cbSortType.Name = "cbSortType";
            this.cbSortType.Size = new System.Drawing.Size(167, 20);
            this.cbSortType.TabIndex = 51;
            this.cbSortType.Text = "품명+원산지+규격";
            this.cbSortType.SelectedIndexChanged += new System.EventHandler(this.cbSortType_SelectedIndexChanged);
            // 
            // txtTrq
            // 
            this.txtTrq.Location = new System.Drawing.Point(197, 2);
            this.txtTrq.Name = "txtTrq";
            this.txtTrq.Size = new System.Drawing.Size(84, 21);
            this.txtTrq.TabIndex = 108;
            this.txtTrq.Text = "1,625";
            this.txtTrq.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTrq.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtExchangeRate_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(10, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 12);
            this.label5.TabIndex = 109;
            this.label5.Text = "현재환율";
            // 
            // txtExchangeRate
            // 
            this.txtExchangeRate.Location = new System.Drawing.Point(72, 2);
            this.txtExchangeRate.Name = "txtExchangeRate";
            this.txtExchangeRate.Size = new System.Drawing.Size(84, 21);
            this.txtExchangeRate.TabIndex = 107;
            this.txtExchangeRate.Text = "0";
            this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtExchangeRate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtExchangeRate_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(162, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 12);
            this.label6.TabIndex = 110;
            this.label6.Text = "TRQ";
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPreview.Location = new System.Drawing.Point(200, 2);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(70, 37);
            this.btnPreview.TabIndex = 6;
            this.btnPreview.Text = "인쇄 (ctrl+P)";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // CostAccountingGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1662, 876);
            this.Controls.Add(this.pnMain);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "CostAccountingGroup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "다중 원가계산";
            this.Load += new System.EventHandler(this.CostAccountingGroup_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CostAccountingGroup_KeyDown);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.FlowLayoutPanel pnMain;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtTrq;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtExchangeRate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnProduct;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox cbSortType;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button btnBatchInput;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RadioButton rbSalesPrice;
        private System.Windows.Forms.RadioButton rbPurchaseprice;
        private System.Windows.Forms.CheckBox cbPerBox;
        private System.Windows.Forms.Button btnPreview;
    }
}