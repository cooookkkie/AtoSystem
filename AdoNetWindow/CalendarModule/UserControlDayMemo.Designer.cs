namespace AdoNetWindow.CalendarModule
{
    partial class UserControlDayMemo
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
            this.lb = new System.Windows.Forms.Label();
            this.lbId = new System.Windows.Forms.Label();
            this.lbEnd = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lb
            // 
            this.lb.AutoSize = true;
            this.lb.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lb.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb.ForeColor = System.Drawing.Color.Black;
            this.lb.Location = new System.Drawing.Point(17, 4);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(0, 11);
            this.lb.TabIndex = 1;
            this.lb.Paint += new System.Windows.Forms.PaintEventHandler(this.lb_Paint);
            this.lb.DoubleClick += new System.EventHandler(this.lb_DoubleClick);
            this.lb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lb_MouseUp);
            // 
            // lbId
            // 
            this.lbId.AutoSize = true;
            this.lbId.Location = new System.Drawing.Point(175, 4);
            this.lbId.Name = "lbId";
            this.lbId.Size = new System.Drawing.Size(0, 12);
            this.lbId.TabIndex = 2;
            this.lbId.Visible = false;
            // 
            // lbEnd
            // 
            this.lbEnd.AutoSize = true;
            this.lbEnd.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbEnd.ForeColor = System.Drawing.Color.Green;
            this.lbEnd.Location = new System.Drawing.Point(5, 4);
            this.lbEnd.Name = "lbEnd";
            this.lbEnd.Size = new System.Drawing.Size(10, 12);
            this.lbEnd.TabIndex = 3;
            this.lbEnd.Text = "!";
            this.lbEnd.Visible = false;
            // 
            // UserControlDayMemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbEnd);
            this.Controls.Add(this.lbId);
            this.Controls.Add(this.lb);
            this.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "UserControlDayMemo";
            this.Size = new System.Drawing.Size(232, 20);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UserControlDayMemo_Paint);
            this.DoubleClick += new System.EventHandler(this.UserControlDayMemo_DoubleClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb;
        private System.Windows.Forms.Label lbId;
        private System.Windows.Forms.Label lbEnd;
    }
}
