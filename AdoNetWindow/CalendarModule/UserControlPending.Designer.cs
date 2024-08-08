﻿namespace AdoNetWindow.CalendarModule
{
    partial class UserControlPending
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
            this.components = new System.ComponentModel.Container();
            this.lb = new System.Windows.Forms.Label();
            this.lbId = new System.Windows.Forms.Label();
            this.lbEnd = new System.Windows.Forms.Label();
            this.btnLg = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lb
            // 
            this.lb.AutoSize = true;
            this.lb.BackColor = System.Drawing.Color.White;
            this.lb.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb.Location = new System.Drawing.Point(17, 3);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(0, 12);
            this.lb.TabIndex = 0;
            this.lb.Paint += new System.Windows.Forms.PaintEventHandler(this.lb_Paint);
            this.lb.DoubleClick += new System.EventHandler(this.lb_DoubleClick);
            this.lb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnLg_MouseUp);
            // 
            // lbId
            // 
            this.lbId.AutoSize = true;
            this.lbId.Location = new System.Drawing.Point(175, 4);
            this.lbId.Name = "lbId";
            this.lbId.Size = new System.Drawing.Size(0, 12);
            this.lbId.TabIndex = 1;
            this.lbId.Visible = false;
            // 
            // lbEnd
            // 
            this.lbEnd.AutoSize = true;
            this.lbEnd.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbEnd.Location = new System.Drawing.Point(5, 4);
            this.lbEnd.Name = "lbEnd";
            this.lbEnd.Size = new System.Drawing.Size(10, 12);
            this.lbEnd.TabIndex = 2;
            this.lbEnd.Text = "!";
            this.lbEnd.Visible = false;
            this.lbEnd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnLg_MouseUp);
            // 
            // btnLg
            // 
            this.btnLg.BackColor = System.Drawing.Color.White;
            this.btnLg.BackgroundImage = global::AdoNetWindow.Properties.Resources.TT_icon;
            this.btnLg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLg.Enabled = false;
            this.btnLg.FlatAppearance.BorderSize = 0;
            this.btnLg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLg.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLg.Location = new System.Drawing.Point(1, 1);
            this.btnLg.Name = "btnLg";
            this.btnLg.Size = new System.Drawing.Size(15, 15);
            this.btnLg.TabIndex = 33;
            this.btnLg.UseVisualStyleBackColor = false;
            this.btnLg.Visible = false;
            this.btnLg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnLg_MouseUp);
            // 
            // UserControlPending
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnLg);
            this.Controls.Add(this.lbEnd);
            this.Controls.Add(this.lbId);
            this.Controls.Add(this.lb);
            this.Name = "UserControlPending";
            this.Size = new System.Drawing.Size(232, 20);
            this.Load += new System.EventHandler(this.UserControlPending_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UserControlPending_Paint);
            this.DoubleClick += new System.EventHandler(this.UserControlPending_DoubleClick);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnLg_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb;
        private System.Windows.Forms.Label lbId;
        private System.Windows.Forms.Label lbEnd;
        internal System.Windows.Forms.Button btnLg;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
