namespace AdoNetWindow.SEAOVER.TwoLine
{
    partial class ProductUnit
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
            this.lbContents = new System.Windows.Forms.Label();
            this.txtUpdate = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbContents
            // 
            this.lbContents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbContents.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbContents.Location = new System.Drawing.Point(0, 0);
            this.lbContents.Margin = new System.Windows.Forms.Padding(0);
            this.lbContents.Name = "lbContents";
            this.lbContents.Size = new System.Drawing.Size(150, 23);
            this.lbContents.TabIndex = 1;
            this.lbContents.Text = "label1";
            this.lbContents.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbContents.DoubleClick += new System.EventHandler(this.lbContents_DoubleClick);
            this.lbContents.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbContents_MouseDown);
            // 
            // txtUpdate
            // 
            this.txtUpdate.Location = new System.Drawing.Point(1, 0);
            this.txtUpdate.Multiline = true;
            this.txtUpdate.Name = "txtUpdate";
            this.txtUpdate.Size = new System.Drawing.Size(12, 21);
            this.txtUpdate.TabIndex = 2;
            this.txtUpdate.Visible = false;
            this.txtUpdate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUpdate_KeyDown);
            // 
            // ProductUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtUpdate);
            this.Controls.Add(this.lbContents);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ProductUnit";
            this.Size = new System.Drawing.Size(150, 21);
            this.Load += new System.EventHandler(this.ProductUnit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbContents;
        private System.Windows.Forms.TextBox txtUpdate;
    }
}
