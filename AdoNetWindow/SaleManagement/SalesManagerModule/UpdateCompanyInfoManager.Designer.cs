namespace AdoNetWindow.SaleManagement
{
    partial class UpdateCompanyInfoManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateCompanyInfoManager));
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbNoneHandled = new System.Windows.Forms.RadioButton();
            this.rbTrading = new System.Windows.Forms.RadioButton();
            this.rbPotential2 = new System.Windows.Forms.RadioButton();
            this.rbPotential1 = new System.Windows.Forms.RadioButton();
            this.rbMyData = new System.Windows.Forms.RadioButton();
            this.rbCommonData = new System.Windows.Forms.RadioButton();
            this.rbUpdateInfo = new System.Windows.Forms.RadioButton();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnRegister);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 314);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(212, 43);
            this.panel3.TabIndex = 3;
            // 
            // btnRegister
            // 
            this.btnRegister.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRegister.ForeColor = System.Drawing.Color.Blue;
            this.btnRegister.Location = new System.Drawing.Point(3, 3);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(116, 37);
            this.btnRegister.TabIndex = 23;
            this.btnRegister.Text = "수정(ENTER)";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(130, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(79, 37);
            this.btnExit.TabIndex = 22;
            this.btnExit.Text = "닫기(ESC)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbNoneHandled);
            this.panel2.Controls.Add(this.rbTrading);
            this.panel2.Controls.Add(this.rbPotential2);
            this.panel2.Controls.Add(this.rbPotential1);
            this.panel2.Controls.Add(this.rbMyData);
            this.panel2.Controls.Add(this.rbCommonData);
            this.panel2.Controls.Add(this.rbUpdateInfo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(212, 314);
            this.panel2.TabIndex = 0;
            // 
            // rbNoneHandled
            // 
            this.rbNoneHandled.AutoSize = true;
            this.rbNoneHandled.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbNoneHandled.Location = new System.Drawing.Point(12, 269);
            this.rbNoneHandled.Name = "rbNoneHandled";
            this.rbNoneHandled.Size = new System.Drawing.Size(138, 25);
            this.rbNoneHandled.TabIndex = 24;
            this.rbNoneHandled.Text = "취급X 전환";
            this.rbNoneHandled.UseVisualStyleBackColor = true;
            // 
            // rbTrading
            // 
            this.rbTrading.AutoSize = true;
            this.rbTrading.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbTrading.Location = new System.Drawing.Point(12, 227);
            this.rbTrading.Name = "rbTrading";
            this.rbTrading.Size = new System.Drawing.Size(146, 25);
            this.rbTrading.TabIndex = 23;
            this.rbTrading.Text = "거래중 전환";
            this.rbTrading.UseVisualStyleBackColor = true;
            // 
            // rbPotential2
            // 
            this.rbPotential2.AutoSize = true;
            this.rbPotential2.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbPotential2.Location = new System.Drawing.Point(12, 185);
            this.rbPotential2.Name = "rbPotential2";
            this.rbPotential2.Size = new System.Drawing.Size(136, 25);
            this.rbPotential2.TabIndex = 21;
            this.rbPotential2.Text = "잠재2 전환";
            this.rbPotential2.UseVisualStyleBackColor = true;
            // 
            // rbPotential1
            // 
            this.rbPotential1.AutoSize = true;
            this.rbPotential1.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbPotential1.Location = new System.Drawing.Point(12, 141);
            this.rbPotential1.Name = "rbPotential1";
            this.rbPotential1.Size = new System.Drawing.Size(136, 25);
            this.rbPotential1.TabIndex = 19;
            this.rbPotential1.Text = "잠재1 전환";
            this.rbPotential1.UseVisualStyleBackColor = true;
            // 
            // rbMyData
            // 
            this.rbMyData.AutoSize = true;
            this.rbMyData.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbMyData.Location = new System.Drawing.Point(12, 98);
            this.rbMyData.Name = "rbMyData";
            this.rbMyData.Size = new System.Drawing.Size(155, 25);
            this.rbMyData.TabIndex = 17;
            this.rbMyData.Text = "내DATA 전환";
            this.rbMyData.UseVisualStyleBackColor = true;
            // 
            // rbCommonData
            // 
            this.rbCommonData.AutoSize = true;
            this.rbCommonData.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbCommonData.Location = new System.Drawing.Point(12, 53);
            this.rbCommonData.Name = "rbCommonData";
            this.rbCommonData.Size = new System.Drawing.Size(177, 25);
            this.rbCommonData.TabIndex = 15;
            this.rbCommonData.Text = "공용DATA 전환";
            this.rbCommonData.UseVisualStyleBackColor = true;
            // 
            // rbUpdateInfo
            // 
            this.rbUpdateInfo.AutoSize = true;
            this.rbUpdateInfo.Checked = true;
            this.rbUpdateInfo.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbUpdateInfo.Location = new System.Drawing.Point(12, 12);
            this.rbUpdateInfo.Name = "rbUpdateInfo";
            this.rbUpdateInfo.Size = new System.Drawing.Size(190, 25);
            this.rbUpdateInfo.TabIndex = 13;
            this.rbUpdateInfo.TabStop = true;
            this.rbUpdateInfo.Text = "거래처정보 수정";
            this.rbUpdateInfo.UseVisualStyleBackColor = true;
            // 
            // UpdateCompanyInfoManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(212, 357);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateCompanyInfoManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "수정";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UpdateCompanyInfoManager_KeyDown);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rbNoneHandled;
        private System.Windows.Forms.RadioButton rbTrading;
        private System.Windows.Forms.RadioButton rbPotential2;
        private System.Windows.Forms.RadioButton rbPotential1;
        private System.Windows.Forms.RadioButton rbMyData;
        private System.Windows.Forms.RadioButton rbCommonData;
        private System.Windows.Forms.RadioButton rbUpdateInfo;
    }
}