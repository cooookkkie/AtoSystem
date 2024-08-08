namespace AdoNetWindow.CalendarModule
{
    partial class UserControlCreditTitle
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
            this.lbDivision = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbDivision
            // 
            this.lbDivision.BackColor = System.Drawing.Color.White;
            this.lbDivision.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbDivision.ForeColor = System.Drawing.Color.Red;
            this.lbDivision.Location = new System.Drawing.Point(-2, 0);
            this.lbDivision.Name = "lbDivision";
            this.lbDivision.Size = new System.Drawing.Size(309, 29);
            this.lbDivision.TabIndex = 0;
            this.lbDivision.Text = "NUll";
            this.lbDivision.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UserControlCreditTitle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lbDivision);
            this.Name = "UserControlCreditTitle";
            this.Size = new System.Drawing.Size(306, 28);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbDivision;
    }
}
