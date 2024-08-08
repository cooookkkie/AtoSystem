namespace AdoNetWindow.Common.MultiDatesCalendar
{
    partial class DayUnit
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
            this.lbDay = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbDay
            // 
            this.lbDay.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbDay.Location = new System.Drawing.Point(0, 0);
            this.lbDay.Name = "lbDay";
            this.lbDay.Size = new System.Drawing.Size(60, 54);
            this.lbDay.TabIndex = 1;
            this.lbDay.Text = "0";
            this.lbDay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbDay.Click += new System.EventHandler(this.lbDay_Click);
            this.lbDay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbDay_MouseMove);
            // 
            // DayUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lbDay);
            this.Name = "DayUnit";
            this.Size = new System.Drawing.Size(60, 54);
            this.Load += new System.EventHandler(this.DayUnit_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbDay;
    }
}
