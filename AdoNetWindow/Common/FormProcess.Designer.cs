namespace AdoNetWindow.Common
{
    partial class FormProcess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProcess));
            this.progProcess = new System.Windows.Forms.ProgressBar();
            this.lbProcessRate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progProcess
            // 
            this.progProcess.Location = new System.Drawing.Point(4, 4);
            this.progProcess.Name = "progProcess";
            this.progProcess.Size = new System.Drawing.Size(384, 23);
            this.progProcess.TabIndex = 0;
            // 
            // lbProcessRate
            // 
            this.lbProcessRate.AutoSize = true;
            this.lbProcessRate.Location = new System.Drawing.Point(176, 10);
            this.lbProcessRate.Name = "lbProcessRate";
            this.lbProcessRate.Size = new System.Drawing.Size(38, 12);
            this.lbProcessRate.TabIndex = 1;
            this.lbProcessRate.Text = "label1";
            // 
            // FormProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 31);
            this.Controls.Add(this.lbProcessRate);
            this.Controls.Add(this.progProcess);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProcess";
            this.Text = "Process";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormProcess_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progProcess;
        private System.Windows.Forms.Label lbProcessRate;
    }
}