namespace AdoNetWindow.Arrive
{
    partial class ArriveMemo
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
            this.lbContent = new System.Windows.Forms.Label();
            this.lbManager = new System.Windows.Forms.Label();
            this.lbEditDate = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbContent
            // 
            this.lbContent.BackColor = System.Drawing.SystemColors.Control;
            this.lbContent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbContent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbContent.ForeColor = System.Drawing.Color.Black;
            this.lbContent.Location = new System.Drawing.Point(4, 3);
            this.lbContent.Name = "lbContent";
            this.lbContent.Size = new System.Drawing.Size(580, 40);
            this.lbContent.TabIndex = 0;
            this.lbContent.Text = "null";
            // 
            // lbManager
            // 
            this.lbManager.AutoSize = true;
            this.lbManager.Location = new System.Drawing.Point(590, 9);
            this.lbManager.Name = "lbManager";
            this.lbManager.Size = new System.Drawing.Size(41, 12);
            this.lbManager.TabIndex = 1;
            this.lbManager.Text = "담당자";
            // 
            // lbEditDate
            // 
            this.lbEditDate.AutoSize = true;
            this.lbEditDate.Location = new System.Drawing.Point(590, 27);
            this.lbEditDate.Name = "lbEditDate";
            this.lbEditDate.Size = new System.Drawing.Size(143, 12);
            this.lbEditDate.TabIndex = 2;
            this.lbEditDate.Text = "yyyy-MM-dd hh:mm:ss";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.lbEditDate);
            this.panel1.Controls.Add(this.lbManager);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(736, 47);
            this.panel1.TabIndex = 4;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.Control;
            this.btnDelete.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Location = new System.Drawing.Point(716, -1);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(20, 21);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "x";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbContent);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(586, 47);
            this.panel2.TabIndex = 4;
            // 
            // ArriveMemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.SaddleBrown;
            this.Name = "ArriveMemo";
            this.Size = new System.Drawing.Size(736, 47);
            this.Load += new System.EventHandler(this.ArriveMemo_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbContent;
        private System.Windows.Forms.Label lbManager;
        private System.Windows.Forms.Label lbEditDate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnDelete;
    }
}
