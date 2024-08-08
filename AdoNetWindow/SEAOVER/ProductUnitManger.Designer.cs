namespace AdoNetWindow.SEAOVER.TwoLine
{
    partial class ProductUnitManger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductUnitManger));
            this.txtContents = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnAllSetting = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtContents
            // 
            this.txtContents.Location = new System.Drawing.Point(4, 3);
            this.txtContents.Multiline = true;
            this.txtContents.Name = "txtContents";
            this.txtContents.Size = new System.Drawing.Size(278, 177);
            this.txtContents.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.ForeColor = System.Drawing.Color.Blue;
            this.btnSave.Location = new System.Drawing.Point(148, 186);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(62, 31);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "저장(A)";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(216, 186);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 31);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnAllSetting
            // 
            this.btnAllSetting.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.btnAllSetting.Location = new System.Drawing.Point(4, 186);
            this.btnAllSetting.Name = "btnAllSetting";
            this.btnAllSetting.Size = new System.Drawing.Size(138, 31);
            this.btnAllSetting.TabIndex = 3;
            this.btnAllSetting.Text = "모든 품목서 적용(Z)";
            this.btnAllSetting.UseVisualStyleBackColor = true;
            this.btnAllSetting.Click += new System.EventHandler(this.btnAllSetting_Click);
            // 
            // ProductUnitManger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 221);
            this.Controls.Add(this.btnAllSetting);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtContents);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProductUnitManger";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "명칭변경";
            this.Load += new System.EventHandler(this.ProductUnitManger_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProductUnitManger_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtContents;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnAllSetting;
    }
}