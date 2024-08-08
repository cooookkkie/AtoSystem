namespace AdoNetWindow.SEAOVER.PriceComparison
{
    partial class calendarUnit
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbShipment = new System.Windows.Forms.Label();
            this.lbStock = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbDay
            // 
            this.lbDay.AutoSize = true;
            this.lbDay.Location = new System.Drawing.Point(2, 4);
            this.lbDay.Name = "lbDay";
            this.lbDay.Size = new System.Drawing.Size(17, 12);
            this.lbDay.TabIndex = 0;
            this.lbDay.Text = "00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(3, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "선적";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(3, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "재고";
            // 
            // lbShipment
            // 
            this.lbShipment.Location = new System.Drawing.Point(56, 43);
            this.lbShipment.Name = "lbShipment";
            this.lbShipment.Size = new System.Drawing.Size(50, 17);
            this.lbShipment.TabIndex = 5;
            this.lbShipment.Text = "00";
            this.lbShipment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbStock
            // 
            this.lbStock.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbStock.Location = new System.Drawing.Point(56, 62);
            this.lbStock.Name = "lbStock";
            this.lbStock.Size = new System.Drawing.Size(50, 17);
            this.lbStock.TabIndex = 6;
            this.lbStock.Text = "00";
            this.lbStock.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // calendarUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lbStock);
            this.Controls.Add(this.lbShipment);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbDay);
            this.Name = "calendarUnit";
            this.Size = new System.Drawing.Size(112, 86);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.calendarUnit_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbDay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbShipment;
        private System.Windows.Forms.Label lbStock;
    }
}
