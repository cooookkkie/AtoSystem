namespace AdoNetWindow.Common.PrintManager
{
    partial class PrintManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintManager));
            this.prtDialog = new System.Windows.Forms.PrintDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.prtPreControl = new System.Windows.Forms.PrintPreviewControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSelectPrinter = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnPrintSelect = new System.Windows.Forms.Button();
            this.prtDocument = new System.Drawing.Printing.PrintDocument();
            this.prtPreDialog = new System.Windows.Forms.PrintPreviewDialog();
            this.pgSetupDialog = new System.Windows.Forms.PageSetupDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPre = new System.Windows.Forms.Button();
            this.nudCurPage = new System.Windows.Forms.NumericUpDown();
            this.nudTotalPage = new System.Windows.Forms.NumericUpDown();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnMinusZoom = new System.Windows.Forms.Button();
            this.nudZoomSize = new System.Windows.Forms.NumericUpDown();
            this.btnPlusZoom = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCurPage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotalPage)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudZoomSize)).BeginInit();
            this.SuspendLayout();
            // 
            // prtDialog
            // 
            this.prtDialog.UseEXDialog = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.prtPreControl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1646, 772);
            this.panel1.TabIndex = 0;
            // 
            // prtPreControl
            // 
            this.prtPreControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prtPreControl.Location = new System.Drawing.Point(0, 0);
            this.prtPreControl.Name = "prtPreControl";
            this.prtPreControl.Size = new System.Drawing.Size(1646, 772);
            this.prtPreControl.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSelectPrinter);
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Controls.Add(this.btnPrintSelect);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 772);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1646, 40);
            this.panel2.TabIndex = 1;
            // 
            // btnSelectPrinter
            // 
            this.btnSelectPrinter.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelectPrinter.Location = new System.Drawing.Point(115, 3);
            this.btnSelectPrinter.Name = "btnSelectPrinter";
            this.btnSelectPrinter.Size = new System.Drawing.Size(106, 34);
            this.btnSelectPrinter.TabIndex = 4;
            this.btnSelectPrinter.Text = "프린터 선택 (Ctrl+O)";
            this.btnSelectPrinter.UseVisualStyleBackColor = true;
            this.btnSelectPrinter.Click += new System.EventHandler(this.btnSelectPrinter_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(227, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(78, 34);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnPrintSelect
            // 
            this.btnPrintSelect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrintSelect.Location = new System.Drawing.Point(3, 3);
            this.btnPrintSelect.Name = "btnPrintSelect";
            this.btnPrintSelect.Size = new System.Drawing.Size(106, 34);
            this.btnPrintSelect.TabIndex = 0;
            this.btnPrintSelect.Text = "인쇄 (Ctrl+P)";
            this.btnPrintSelect.UseVisualStyleBackColor = true;
            this.btnPrintSelect.Click += new System.EventHandler(this.btnPrintSelet_Click);
            // 
            // prtDocument
            // 
            this.prtDocument.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument1_BeginPrint);
            this.prtDocument.EndPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument1_EndPrint);
            this.prtDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // prtPreDialog
            // 
            this.prtPreDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.prtPreDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.prtPreDialog.ClientSize = new System.Drawing.Size(400, 300);
            this.prtPreDialog.Enabled = true;
            this.prtPreDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("prtPreDialog.Icon")));
            this.prtPreDialog.Name = "printPreviewDialog1";
            this.prtPreDialog.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Page";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(127, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "/";
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.White;
            this.btnNext.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNext.BackgroundImage")));
            this.btnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNext.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnNext.Location = new System.Drawing.Point(185, 0);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(30, 22);
            this.btnNext.TabIndex = 35;
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPre
            // 
            this.btnPre.BackColor = System.Drawing.Color.White;
            this.btnPre.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPre.BackgroundImage")));
            this.btnPre.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPre.FlatAppearance.BorderSize = 0;
            this.btnPre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPre.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPre.Location = new System.Drawing.Point(51, 0);
            this.btnPre.Name = "btnPre";
            this.btnPre.Size = new System.Drawing.Size(30, 22);
            this.btnPre.TabIndex = 34;
            this.btnPre.UseVisualStyleBackColor = false;
            this.btnPre.Click += new System.EventHandler(this.btnPre_Click);
            // 
            // nudCurPage
            // 
            this.nudCurPage.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.nudCurPage.Location = new System.Drawing.Point(81, -1);
            this.nudCurPage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCurPage.Name = "nudCurPage";
            this.nudCurPage.Size = new System.Drawing.Size(46, 22);
            this.nudCurPage.TabIndex = 1;
            this.nudCurPage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudTotalPage
            // 
            this.nudTotalPage.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.nudTotalPage.Location = new System.Drawing.Point(139, -1);
            this.nudTotalPage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTotalPage.Name = "nudTotalPage";
            this.nudTotalPage.Size = new System.Drawing.Size(46, 22);
            this.nudTotalPage.TabIndex = 2;
            this.nudTotalPage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.btnMinusZoom);
            this.panel4.Controls.Add(this.nudZoomSize);
            this.panel4.Controls.Add(this.btnPlusZoom);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1646, 24);
            this.panel4.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnPre);
            this.panel5.Controls.Add(this.nudTotalPage);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.btnNext);
            this.panel5.Controls.Add(this.nudCurPage);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(1429, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(215, 22);
            this.panel5.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(5, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "확대/축소";
            // 
            // btnMinusZoom
            // 
            this.btnMinusZoom.BackColor = System.Drawing.Color.White;
            this.btnMinusZoom.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMinusZoom.BackgroundImage")));
            this.btnMinusZoom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMinusZoom.FlatAppearance.BorderSize = 0;
            this.btnMinusZoom.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMinusZoom.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMinusZoom.Location = new System.Drawing.Point(152, 0);
            this.btnMinusZoom.Name = "btnMinusZoom";
            this.btnMinusZoom.Size = new System.Drawing.Size(23, 23);
            this.btnMinusZoom.TabIndex = 2;
            this.btnMinusZoom.UseVisualStyleBackColor = false;
            this.btnMinusZoom.Click += new System.EventHandler(this.btnMinusZoom_Click);
            // 
            // nudZoomSize
            // 
            this.nudZoomSize.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.nudZoomSize.Location = new System.Drawing.Point(71, 0);
            this.nudZoomSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudZoomSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudZoomSize.Name = "nudZoomSize";
            this.nudZoomSize.Size = new System.Drawing.Size(58, 22);
            this.nudZoomSize.TabIndex = 0;
            this.nudZoomSize.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudZoomSize.ValueChanged += new System.EventHandler(this.nudZoomSize_ValueChanged);
            // 
            // btnPlusZoom
            // 
            this.btnPlusZoom.BackColor = System.Drawing.Color.White;
            this.btnPlusZoom.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPlusZoom.BackgroundImage")));
            this.btnPlusZoom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPlusZoom.FlatAppearance.BorderSize = 0;
            this.btnPlusZoom.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPlusZoom.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPlusZoom.Location = new System.Drawing.Point(129, 0);
            this.btnPlusZoom.Name = "btnPlusZoom";
            this.btnPlusZoom.Size = new System.Drawing.Size(23, 23);
            this.btnPlusZoom.TabIndex = 1;
            this.btnPlusZoom.UseVisualStyleBackColor = false;
            this.btnPlusZoom.Click += new System.EventHandler(this.btnPlusZoom_Click);
            // 
            // PrintManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1646, 812);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "PrintManager";
            this.Text = "미리하기";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PrintManager_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PrintManager_KeyPress);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudCurPage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotalPage)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudZoomSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PrintDialog prtDialog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PrintPreviewControl prtPreControl;
        private System.Windows.Forms.Panel panel2;
        private System.Drawing.Printing.PrintDocument prtDocument;
        private System.Windows.Forms.PrintPreviewDialog prtPreDialog;
        private System.Windows.Forms.Button btnPrintSelect;
        private System.Windows.Forms.PageSetupDialog pgSetupDialog;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btnNext;
        internal System.Windows.Forms.Button btnPre;
        private System.Windows.Forms.NumericUpDown nudTotalPage;
        private System.Windows.Forms.NumericUpDown nudCurPage;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.NumericUpDown nudZoomSize;
        internal System.Windows.Forms.Button btnPlusZoom;
        internal System.Windows.Forms.Button btnMinusZoom;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSelectPrinter;
    }
}