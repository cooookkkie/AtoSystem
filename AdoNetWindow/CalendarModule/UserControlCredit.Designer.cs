namespace AdoNetWindow.CalendarModule
{
    partial class UserControlCredit
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbBank = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtUsanceBalance = new System.Windows.Forms.TextBox();
            this.txtUsanceUsed = new System.Windows.Forms.TextBox();
            this.txtUsanceLimit = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtAtsightBalance = new System.Windows.Forms.TextBox();
            this.txtAtsightUsed = new System.Windows.Forms.TextBox();
            this.txtAtsightLimit = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbBank);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(41, 42);
            this.panel1.TabIndex = 0;
            // 
            // lbBank
            // 
            this.lbBank.BackColor = System.Drawing.Color.White;
            this.lbBank.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBank.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbBank.Location = new System.Drawing.Point(11, 0);
            this.lbBank.Name = "lbBank";
            this.lbBank.Size = new System.Drawing.Size(30, 42);
            this.lbBank.TabIndex = 0;
            this.lbBank.Text = "NULL";
            this.lbBank.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(41, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(253, 42);
            this.panel2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.txtUsanceBalance);
            this.panel4.Controls.Add(this.txtUsanceUsed);
            this.panel4.Controls.Add(this.txtUsanceLimit);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 21);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(253, 21);
            this.panel4.TabIndex = 1;
            // 
            // txtUsanceBalance
            // 
            this.txtUsanceBalance.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtUsanceBalance.ForeColor = System.Drawing.Color.Blue;
            this.txtUsanceBalance.Location = new System.Drawing.Point(168, 0);
            this.txtUsanceBalance.Name = "txtUsanceBalance";
            this.txtUsanceBalance.Size = new System.Drawing.Size(84, 21);
            this.txtUsanceBalance.TabIndex = 5;
            this.txtUsanceBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtUsanceUsed
            // 
            this.txtUsanceUsed.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtUsanceUsed.Location = new System.Drawing.Point(84, 0);
            this.txtUsanceUsed.Name = "txtUsanceUsed";
            this.txtUsanceUsed.Size = new System.Drawing.Size(84, 21);
            this.txtUsanceUsed.TabIndex = 4;
            this.txtUsanceUsed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtUsanceLimit
            // 
            this.txtUsanceLimit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtUsanceLimit.Location = new System.Drawing.Point(0, 0);
            this.txtUsanceLimit.Name = "txtUsanceLimit";
            this.txtUsanceLimit.Size = new System.Drawing.Size(84, 21);
            this.txtUsanceLimit.TabIndex = 3;
            this.txtUsanceLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtAtsightBalance);
            this.panel3.Controls.Add(this.txtAtsightUsed);
            this.panel3.Controls.Add(this.txtAtsightLimit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(253, 21);
            this.panel3.TabIndex = 0;
            // 
            // txtAtsightBalance
            // 
            this.txtAtsightBalance.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtAtsightBalance.ForeColor = System.Drawing.Color.Blue;
            this.txtAtsightBalance.Location = new System.Drawing.Point(168, 0);
            this.txtAtsightBalance.Name = "txtAtsightBalance";
            this.txtAtsightBalance.Size = new System.Drawing.Size(84, 21);
            this.txtAtsightBalance.TabIndex = 2;
            this.txtAtsightBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtAtsightUsed
            // 
            this.txtAtsightUsed.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtAtsightUsed.Location = new System.Drawing.Point(84, 0);
            this.txtAtsightUsed.Name = "txtAtsightUsed";
            this.txtAtsightUsed.Size = new System.Drawing.Size(84, 21);
            this.txtAtsightUsed.TabIndex = 1;
            this.txtAtsightUsed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtAtsightLimit
            // 
            this.txtAtsightLimit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtAtsightLimit.Location = new System.Drawing.Point(0, 0);
            this.txtAtsightLimit.Name = "txtAtsightLimit";
            this.txtAtsightLimit.Size = new System.Drawing.Size(84, 21);
            this.txtAtsightLimit.TabIndex = 0;
            this.txtAtsightLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // UserControlCredit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "UserControlCredit";
            this.Size = new System.Drawing.Size(294, 42);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtUsanceBalance;
        private System.Windows.Forms.TextBox txtUsanceUsed;
        private System.Windows.Forms.TextBox txtUsanceLimit;
        private System.Windows.Forms.TextBox txtAtsightBalance;
        private System.Windows.Forms.TextBox txtAtsightUsed;
        private System.Windows.Forms.TextBox txtAtsightLimit;
        private System.Windows.Forms.Label lbBank;
    }
}
