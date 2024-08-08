namespace AdoNetWindow.SaleManagement.DailyBusiness
{
    partial class SheetNameManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SheetNameManager));
            this.txtSheetName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtSheetName
            // 
            this.txtSheetName.Location = new System.Drawing.Point(0, 0);
            this.txtSheetName.Name = "txtSheetName";
            this.txtSheetName.Size = new System.Drawing.Size(100, 21);
            this.txtSheetName.TabIndex = 0;
            // 
            // SheetNameManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(100, 21);
            this.ControlBox = false;
            this.Controls.Add(this.txtSheetName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SheetNameManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "시트명 변경";
            this.TransparencyKey = System.Drawing.Color.Red;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SheetNameManager_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSheetName;
    }
}