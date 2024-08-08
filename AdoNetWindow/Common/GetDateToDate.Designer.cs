namespace AdoNetWindow.Common
{
    partial class GetDateToDate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetDateToDate));
            this.label1 = new System.Windows.Forms.Label();
            this.txtSttdate = new System.Windows.Forms.TextBox();
            this.txtEnddate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSttdate = new System.Windows.Forms.Button();
            this.btnEnddate = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(8, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "선택기간";
            // 
            // txtSttdate
            // 
            this.txtSttdate.Location = new System.Drawing.Point(73, 8);
            this.txtSttdate.Name = "txtSttdate";
            this.txtSttdate.Size = new System.Drawing.Size(92, 21);
            this.txtSttdate.TabIndex = 1;
            // 
            // txtEnddate
            // 
            this.txtEnddate.Location = new System.Drawing.Point(216, 9);
            this.txtEnddate.Name = "txtEnddate";
            this.txtEnddate.Size = new System.Drawing.Size(92, 21);
            this.txtEnddate.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(195, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "~";
            // 
            // btnSttdate
            // 
            this.btnSttdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSttdate.BackgroundImage")));
            this.btnSttdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSttdate.FlatAppearance.BorderSize = 0;
            this.btnSttdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSttdate.Location = new System.Drawing.Point(167, 7);
            this.btnSttdate.Name = "btnSttdate";
            this.btnSttdate.Size = new System.Drawing.Size(22, 23);
            this.btnSttdate.TabIndex = 109;
            this.btnSttdate.UseVisualStyleBackColor = true;
            this.btnSttdate.Click += new System.EventHandler(this.btnSttdate_Click);
            // 
            // btnEnddate
            // 
            this.btnEnddate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEnddate.BackgroundImage")));
            this.btnEnddate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEnddate.FlatAppearance.BorderSize = 0;
            this.btnEnddate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnddate.Location = new System.Drawing.Point(310, 8);
            this.btnEnddate.Name = "btnEnddate";
            this.btnEnddate.Size = new System.Drawing.Size(22, 23);
            this.btnEnddate.TabIndex = 110;
            this.btnEnddate.UseVisualStyleBackColor = true;
            this.btnEnddate.Click += new System.EventHandler(this.btnEnddate_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(415, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(63, 33);
            this.btnExit.TabIndex = 112;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelect.Location = new System.Drawing.Point(346, 4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(63, 33);
            this.btnSelect.TabIndex = 111;
            this.btnSelect.Text = "선택(A)";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // GetDateToDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 40);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.btnEnddate);
            this.Controls.Add(this.btnSttdate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtEnddate);
            this.Controls.Add(this.txtSttdate);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "GetDateToDate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "기간선택";
            this.Load += new System.EventHandler(this.GetDateToDate_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GetDateToDate_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSttdate;
        private System.Windows.Forms.TextBox txtEnddate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSttdate;
        private System.Windows.Forms.Button btnEnddate;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSelect;
    }
}