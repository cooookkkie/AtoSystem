namespace AdoNetWindow.Common
{
    partial class ProcessBar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessBar));
            this.lbTotalRate = new System.Windows.Forms.Label();
            this.lbCurrentRate = new System.Windows.Forms.Label();
            this.lbTotal = new System.Windows.Forms.Label();
            this.lbCurrent = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbTotalRate
            // 
            this.lbTotalRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbTotalRate.Location = new System.Drawing.Point(12, 9);
            this.lbTotalRate.Name = "lbTotalRate";
            this.lbTotalRate.Size = new System.Drawing.Size(300, 23);
            this.lbTotalRate.TabIndex = 0;
            // 
            // lbCurrentRate
            // 
            this.lbCurrentRate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lbCurrentRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbCurrentRate.Location = new System.Drawing.Point(12, 9);
            this.lbCurrentRate.Name = "lbCurrentRate";
            this.lbCurrentRate.Size = new System.Drawing.Size(300, 23);
            this.lbCurrentRate.TabIndex = 1;
            // 
            // lbTotal
            // 
            this.lbTotal.AutoSize = true;
            this.lbTotal.Location = new System.Drawing.Point(180, 13);
            this.lbTotal.Name = "lbTotal";
            this.lbTotal.Size = new System.Drawing.Size(11, 12);
            this.lbTotal.TabIndex = 2;
            this.lbTotal.Text = "0";
            // 
            // lbCurrent
            // 
            this.lbCurrent.Location = new System.Drawing.Point(119, 13);
            this.lbCurrent.Name = "lbCurrent";
            this.lbCurrent.Size = new System.Drawing.Size(38, 12);
            this.lbCurrent.TabIndex = 3;
            this.lbCurrent.Text = "0";
            this.lbCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(163, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "/";
            // 
            // ProcessBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 40);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbCurrent);
            this.Controls.Add(this.lbTotal);
            this.Controls.Add(this.lbCurrentRate);
            this.Controls.Add(this.lbTotalRate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProcessBar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProcessBar";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbTotalRate;
        private System.Windows.Forms.Label lbCurrentRate;
        private System.Windows.Forms.Label lbTotal;
        private System.Windows.Forms.Label lbCurrent;
        private System.Windows.Forms.Label label3;
    }
}