namespace AdoNetWindow.CalendarModule
{
    partial class UserControlDays
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
            this.lbdays = new System.Windows.Forms.Label();
            this.lbname = new System.Windows.Forms.Label();
            this.pPending = new System.Windows.Forms.FlowLayoutPanel();
            this.lbAdd = new System.Windows.Forms.Label();
            this.lbPay = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbdays
            // 
            this.lbdays.AutoSize = true;
            this.lbdays.BackColor = System.Drawing.Color.White;
            this.lbdays.Location = new System.Drawing.Point(3, 6);
            this.lbdays.Name = "lbdays";
            this.lbdays.Size = new System.Drawing.Size(17, 12);
            this.lbdays.TabIndex = 3;
            this.lbdays.Text = "00";
            this.lbdays.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbdays_MouseDoubleClick);
            // 
            // lbname
            // 
            this.lbname.AutoSize = true;
            this.lbname.BackColor = System.Drawing.Color.White;
            this.lbname.Location = new System.Drawing.Point(26, 8);
            this.lbname.Name = "lbname";
            this.lbname.Size = new System.Drawing.Size(0, 12);
            this.lbname.TabIndex = 5;
            // 
            // pPending
            // 
            this.pPending.AutoScroll = true;
            this.pPending.BackColor = System.Drawing.Color.White;
            this.pPending.Location = new System.Drawing.Point(1, 23);
            this.pPending.Name = "pPending";
            this.pPending.Size = new System.Drawing.Size(257, 106);
            this.pPending.TabIndex = 13;
            // 
            // lbAdd
            // 
            this.lbAdd.AutoSize = true;
            this.lbAdd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbAdd.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbAdd.Location = new System.Drawing.Point(3, 5);
            this.lbAdd.Name = "lbAdd";
            this.lbAdd.Size = new System.Drawing.Size(13, 13);
            this.lbAdd.TabIndex = 14;
            this.lbAdd.Text = "+";
            this.lbAdd.Click += new System.EventHandler(this.lbAdd_Click);
            // 
            // lbPay
            // 
            this.lbPay.AutoEllipsis = true;
            this.lbPay.BackColor = System.Drawing.Color.White;
            this.lbPay.Font = new System.Drawing.Font("궁서", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbPay.ForeColor = System.Drawing.Color.DarkRed;
            this.lbPay.Location = new System.Drawing.Point(57, 4);
            this.lbPay.Name = "lbPay";
            this.lbPay.Size = new System.Drawing.Size(180, 18);
            this.lbPay.TabIndex = 15;
            this.lbPay.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.lbPay);
            this.panel1.Controls.Add(this.lbdays);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(259, 23);
            this.panel1.TabIndex = 16;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbAdd);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(240, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(19, 23);
            this.panel2.TabIndex = 15;
            // 
            // UserControlDays
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pPending);
            this.Controls.Add(this.lbname);
            this.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "UserControlDays";
            this.Size = new System.Drawing.Size(259, 131);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UserControlDays_Paint);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbdays;
        private System.Windows.Forms.Label lbname;
        internal System.Windows.Forms.FlowLayoutPanel pPending;
        private System.Windows.Forms.Label lbAdd;
        private System.Windows.Forms.Label lbPay;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}
