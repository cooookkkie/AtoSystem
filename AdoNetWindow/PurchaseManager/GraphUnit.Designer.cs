namespace AdoNetWindow.PurchaseManager
{
    partial class GraphUnit
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
            this.lbProduct = new System.Windows.Forms.Label();
            this.pnColor = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lbProduct
            // 
            this.lbProduct.AutoSize = true;
            this.lbProduct.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbProduct.Location = new System.Drawing.Point(29, 4);
            this.lbProduct.Name = "lbProduct";
            this.lbProduct.Size = new System.Drawing.Size(44, 12);
            this.lbProduct.TabIndex = 0;
            this.lbProduct.Text = "label1";
            // 
            // pnColor
            // 
            this.pnColor.Location = new System.Drawing.Point(8, 7);
            this.pnColor.Name = "pnColor";
            this.pnColor.Size = new System.Drawing.Size(15, 5);
            this.pnColor.TabIndex = 1;
            // 
            // GraphUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnColor);
            this.Controls.Add(this.lbProduct);
            this.Name = "GraphUnit";
            this.Size = new System.Drawing.Size(284, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbProduct;
        private System.Windows.Forms.Panel pnColor;
    }
}
