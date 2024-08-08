namespace AdoNetWindow.SaleManagement
{
    partial class TxtFindManger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TxtFindManger));
            this.label1 = new System.Windows.Forms.Label();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.txtExcept = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearching = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTel = new System.Windows.Forms.TextBox();
            this.tabFindManager = new System.Windows.Forms.TabControl();
            this.tabCompany = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRnum = new System.Windows.Forms.TextBox();
            this.tabWord = new System.Windows.Forms.TabPage();
            this.tabEtcInfo = new System.Windows.Forms.TabPage();
            this.txtHandlingItem = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDistribution = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tabFindManager.SuspendLayout();
            this.tabCompany.SuspendLayout();
            this.tabWord.SuspendLayout();
            this.tabEtcInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "찾을 단어";
            // 
            // txtFind
            // 
            this.txtFind.Location = new System.Drawing.Point(98, 9);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(195, 21);
            this.txtFind.TabIndex = 1;
            // 
            // txtExcept
            // 
            this.txtExcept.Location = new System.Drawing.Point(98, 36);
            this.txtExcept.Name = "txtExcept";
            this.txtExcept.Size = new System.Drawing.Size(195, 21);
            this.txtExcept.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "제외 단어";
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.Location = new System.Drawing.Point(160, 135);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(88, 30);
            this.btnSearching.TabIndex = 6;
            this.btnSearching.Text = "찾기(Enter)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(254, 135);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(61, 30);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "거래처명";
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(98, 9);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(195, 21);
            this.txtCompany.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "전화/팩스번호";
            // 
            // txtTel
            // 
            this.txtTel.Location = new System.Drawing.Point(98, 36);
            this.txtTel.Name = "txtTel";
            this.txtTel.Size = new System.Drawing.Size(195, 21);
            this.txtTel.TabIndex = 10;
            // 
            // tabFindManager
            // 
            this.tabFindManager.Controls.Add(this.tabCompany);
            this.tabFindManager.Controls.Add(this.tabWord);
            this.tabFindManager.Controls.Add(this.tabEtcInfo);
            this.tabFindManager.Location = new System.Drawing.Point(12, 12);
            this.tabFindManager.Name = "tabFindManager";
            this.tabFindManager.SelectedIndex = 0;
            this.tabFindManager.Size = new System.Drawing.Size(307, 117);
            this.tabFindManager.TabIndex = 12;
            this.tabFindManager.SelectedIndexChanged += new System.EventHandler(this.tabFindManager_SelectedIndexChanged);
            // 
            // tabCompany
            // 
            this.tabCompany.Controls.Add(this.label5);
            this.tabCompany.Controls.Add(this.txtCompany);
            this.tabCompany.Controls.Add(this.txtRnum);
            this.tabCompany.Controls.Add(this.label2);
            this.tabCompany.Controls.Add(this.txtTel);
            this.tabCompany.Controls.Add(this.label4);
            this.tabCompany.Location = new System.Drawing.Point(4, 22);
            this.tabCompany.Name = "tabCompany";
            this.tabCompany.Padding = new System.Windows.Forms.Padding(3);
            this.tabCompany.Size = new System.Drawing.Size(299, 91);
            this.tabCompany.TabIndex = 1;
            this.tabCompany.Text = "거래처검색(F1)";
            this.tabCompany.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "사업자번호";
            // 
            // txtRnum
            // 
            this.txtRnum.Location = new System.Drawing.Point(98, 63);
            this.txtRnum.Name = "txtRnum";
            this.txtRnum.Size = new System.Drawing.Size(195, 21);
            this.txtRnum.TabIndex = 13;
            // 
            // tabWord
            // 
            this.tabWord.Controls.Add(this.txtFind);
            this.tabWord.Controls.Add(this.label1);
            this.tabWord.Controls.Add(this.txtExcept);
            this.tabWord.Controls.Add(this.label3);
            this.tabWord.Location = new System.Drawing.Point(4, 22);
            this.tabWord.Name = "tabWord";
            this.tabWord.Padding = new System.Windows.Forms.Padding(3);
            this.tabWord.Size = new System.Drawing.Size(299, 91);
            this.tabWord.TabIndex = 0;
            this.tabWord.Text = "단어검색(F2)";
            this.tabWord.UseVisualStyleBackColor = true;
            // 
            // tabEtcInfo
            // 
            this.tabEtcInfo.Controls.Add(this.txtHandlingItem);
            this.tabEtcInfo.Controls.Add(this.label6);
            this.tabEtcInfo.Controls.Add(this.txtDistribution);
            this.tabEtcInfo.Controls.Add(this.label7);
            this.tabEtcInfo.Location = new System.Drawing.Point(4, 22);
            this.tabEtcInfo.Name = "tabEtcInfo";
            this.tabEtcInfo.Size = new System.Drawing.Size(299, 91);
            this.tabEtcInfo.TabIndex = 2;
            this.tabEtcInfo.Text = "기타정보(F3)";
            this.tabEtcInfo.UseVisualStyleBackColor = true;
            // 
            // txtHandlingItem
            // 
            this.txtHandlingItem.Location = new System.Drawing.Point(98, 9);
            this.txtHandlingItem.Name = "txtHandlingItem";
            this.txtHandlingItem.Size = new System.Drawing.Size(195, 21);
            this.txtHandlingItem.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "취급품목";
            // 
            // txtDistribution
            // 
            this.txtDistribution.Location = new System.Drawing.Point(98, 36);
            this.txtDistribution.Name = "txtDistribution";
            this.txtDistribution.Size = new System.Drawing.Size(195, 21);
            this.txtDistribution.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 9;
            this.label7.Text = "유통";
            // 
            // TxtFindManger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 174);
            this.Controls.Add(this.tabFindManager);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSearching);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TxtFindManger";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "찾기";
            this.Load += new System.EventHandler(this.TxtFindManger_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtFindManger_KeyDown);
            this.tabFindManager.ResumeLayout(false);
            this.tabCompany.ResumeLayout(false);
            this.tabCompany.PerformLayout();
            this.tabWord.ResumeLayout(false);
            this.tabWord.PerformLayout();
            this.tabEtcInfo.ResumeLayout(false);
            this.tabEtcInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.TextBox txtExcept;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTel;
        private System.Windows.Forms.TabControl tabFindManager;
        private System.Windows.Forms.TabPage tabWord;
        private System.Windows.Forms.TabPage tabCompany;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRnum;
        private System.Windows.Forms.TabPage tabEtcInfo;
        private System.Windows.Forms.TextBox txtHandlingItem;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDistribution;
        private System.Windows.Forms.Label label7;
    }
}