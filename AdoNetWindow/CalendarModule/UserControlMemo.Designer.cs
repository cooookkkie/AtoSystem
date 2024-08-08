namespace AdoNetWindow.CalendarModule
{
    partial class UserControlMemo
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
            this.label1 = new System.Windows.Forms.Label();
            this.lbManager = new System.Windows.Forms.Label();
            this.lbEdit = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtMemo = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbId
            // 
            this.lbId.AutoSize = true;
            this.lbId.Location = new System.Drawing.Point(49, 7);
            this.lbId.Name = "lbId";
            this.lbId.Size = new System.Drawing.Size(38, 12);
            this.lbId.TabIndex = 1;
            this.lbId.Text = "label1";
            this.lbId.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(29, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 11);
            this.label1.TabIndex = 2;
            this.label1.Text = "X";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // lbManager
            // 
            this.lbManager.AutoSize = true;
            this.lbManager.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbManager.Location = new System.Drawing.Point(2, 7);
            this.lbManager.Name = "lbManager";
            this.lbManager.Size = new System.Drawing.Size(34, 13);
            this.lbManager.TabIndex = 3;
            this.lbManager.Text = "Null";
            // 
            // lbEdit
            // 
            this.lbEdit.AutoSize = true;
            this.lbEdit.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbEdit.Location = new System.Drawing.Point(2, 3);
            this.lbEdit.Name = "lbEdit";
            this.lbEdit.Size = new System.Drawing.Size(20, 16);
            this.lbEdit.TabIndex = 4;
            this.lbEdit.Text = "✐";
            this.lbEdit.Click += new System.EventHandler(this.lbEdit_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.lbId);
            this.panel1.Controls.Add(this.lbManager);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(220, 23);
            this.panel1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lbEdit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(174, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(46, 23);
            this.panel2.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtMemo);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 23);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(220, 108);
            this.panel3.TabIndex = 6;
            // 
            // txtMemo
            // 
            this.txtMemo.BackColor = System.Drawing.Color.White;
            this.txtMemo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMemo.Location = new System.Drawing.Point(0, 0);
            this.txtMemo.Multiline = true;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.ReadOnly = true;
            this.txtMemo.Size = new System.Drawing.Size(220, 106);
            this.txtMemo.TabIndex = 0;
            this.txtMemo.Text = "k";
            // 
            // UserControlMemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "UserControlMemo";
            this.Size = new System.Drawing.Size(220, 131);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbManager;
        private System.Windows.Forms.Label lbEdit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtMemo;
    }
}
