﻿namespace AdoNetWindow.CalendarModule
{
    partial class UserControlArrive
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
            this.lbId = new System.Windows.Forms.Label();
            this.lb = new System.Windows.Forms.Label();
            this.lbSubid = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbId
            // 
            this.lbId.AutoSize = true;
            this.lbId.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbId.Location = new System.Drawing.Point(3, 5);
            this.lbId.Name = "lbId";
            this.lbId.Size = new System.Drawing.Size(0, 12);
            this.lbId.TabIndex = 3;
            this.lbId.Visible = false;
            // 
            // lb
            // 
            this.lb.AutoSize = true;
            this.lb.BackColor = System.Drawing.Color.White;
            this.lb.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb.Location = new System.Drawing.Point(18, 4);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(0, 12);
            this.lb.TabIndex = 2;
            this.lb.Paint += new System.Windows.Forms.PaintEventHandler(this.lb_Paint);
            this.lb.DoubleClick += new System.EventHandler(this.lb_DoubleClick);
            this.lb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lb_MouseUp);
            // 
            // lbSubid
            // 
            this.lbSubid.AutoSize = true;
            this.lbSubid.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSubid.Location = new System.Drawing.Point(8, 4);
            this.lbSubid.Name = "lbSubid";
            this.lbSubid.Size = new System.Drawing.Size(0, 12);
            this.lbSubid.TabIndex = 4;
            this.lbSubid.Visible = false;
            // 
            // UserControlArrive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbSubid);
            this.Controls.Add(this.lbId);
            this.Controls.Add(this.lb);
            this.Name = "UserControlArrive";
            this.Size = new System.Drawing.Size(232, 20);
            this.Load += new System.EventHandler(this.UserControlArrive_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UserControlArrive_Paint);
            this.DoubleClick += new System.EventHandler(this.UserControlArrive_DoubleClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbId;
        private System.Windows.Forms.Label lb;
        private System.Windows.Forms.Label lbSubid;
    }
}
