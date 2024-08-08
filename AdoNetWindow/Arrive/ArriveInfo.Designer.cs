namespace AdoNetWindow.Arrive
{
    partial class ArriveInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArriveInfo));
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbOrigin = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lbBoxweight = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lbSizes = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lbProduct = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lbQty = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.scheduleList = new System.Windows.Forms.ListView();
            this.cc_status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.etd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.eta = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.warehousing_date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pending_date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.box_weight = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.qty = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.manager = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sub_id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.eta_status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.arriveMemoPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnInserMemo = new System.Windows.Forms.Button();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("한컴 고딕", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(12, 217);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 14);
            this.label10.TabIndex = 8;
            this.label10.Text = "*정확도*";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("한컴 고딕", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(13, 230);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(462, 14);
            this.label8.TabIndex = 10;
            this.label8.Text = "1. 40% : ETD(선적일) 기준으로 ETA(입항일)과 창고예정일을 계산후 +5일 이후를 통관일자로 계산.";
            // 
            // lbOrigin
            // 
            this.lbOrigin.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbOrigin.Location = new System.Drawing.Point(134, 17);
            this.lbOrigin.Name = "lbOrigin";
            this.lbOrigin.Size = new System.Drawing.Size(150, 12);
            this.lbOrigin.TabIndex = 19;
            this.lbOrigin.Text = "null";
            this.lbOrigin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(41, 17);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(44, 12);
            this.label13.TabIndex = 18;
            this.label13.Text = "원산지";
            // 
            // lbBoxweight
            // 
            this.lbBoxweight.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbBoxweight.Location = new System.Drawing.Point(605, 17);
            this.lbBoxweight.Name = "lbBoxweight";
            this.lbBoxweight.Size = new System.Drawing.Size(150, 12);
            this.lbBoxweight.TabIndex = 17;
            this.lbBoxweight.Text = "null";
            this.lbBoxweight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(512, 17);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(120, 12);
            this.label15.TabIndex = 16;
            this.label15.Text = "박스중량(kg)";
            // 
            // lbSizes
            // 
            this.lbSizes.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSizes.Location = new System.Drawing.Point(134, 66);
            this.lbSizes.Name = "lbSizes";
            this.lbSizes.Size = new System.Drawing.Size(150, 12);
            this.lbSizes.TabIndex = 15;
            this.lbSizes.Text = "null";
            this.lbSizes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label17.Location = new System.Drawing.Point(41, 66);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(44, 12);
            this.label17.TabIndex = 14;
            this.label17.Text = "사이즈";
            // 
            // lbProduct
            // 
            this.lbProduct.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbProduct.Location = new System.Drawing.Point(134, 41);
            this.lbProduct.Name = "lbProduct";
            this.lbProduct.Size = new System.Drawing.Size(150, 12);
            this.lbProduct.TabIndex = 13;
            this.lbProduct.Text = "null";
            this.lbProduct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label19.Location = new System.Drawing.Point(41, 41);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(44, 12);
            this.label19.TabIndex = 12;
            this.label19.Text = "품목명";
            // 
            // lbQty
            // 
            this.lbQty.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbQty.Location = new System.Drawing.Point(605, 43);
            this.lbQty.Name = "lbQty";
            this.lbQty.Size = new System.Drawing.Size(150, 12);
            this.lbQty.TabIndex = 23;
            this.lbQty.Text = "null";
            this.lbQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label21.Location = new System.Drawing.Point(512, 43);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(75, 12);
            this.label21.TabIndex = 22;
            this.label21.Text = "총 계약수량";
            // 
            // scheduleList
            // 
            this.scheduleList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cc_status,
            this.etd,
            this.eta,
            this.warehousing_date,
            this.pending_date,
            this.box_weight,
            this.qty,
            this.manager,
            this.id,
            this.sub_id,
            this.eta_status});
            this.scheduleList.FullRowSelect = true;
            this.scheduleList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.scheduleList.HideSelection = false;
            this.scheduleList.Location = new System.Drawing.Point(16, 97);
            this.scheduleList.Name = "scheduleList";
            this.scheduleList.Size = new System.Drawing.Size(764, 112);
            this.scheduleList.TabIndex = 24;
            this.scheduleList.UseCompatibleStateImageBehavior = false;
            this.scheduleList.View = System.Windows.Forms.View.Details;
            // 
            // cc_status
            // 
            this.cc_status.Text = "상태";
            // 
            // etd
            // 
            this.etd.Text = "ETD(선적일)";
            this.etd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.etd.Width = 100;
            // 
            // eta
            // 
            this.eta.Text = "ETA(입항일)";
            this.eta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.eta.Width = 100;
            // 
            // warehousing_date
            // 
            this.warehousing_date.Text = "창고입고예정일";
            this.warehousing_date.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.warehousing_date.Width = 100;
            // 
            // pending_date
            // 
            this.pending_date.Text = "통관예정일";
            this.pending_date.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.pending_date.Width = 100;
            // 
            // box_weight
            // 
            this.box_weight.Text = "박스중량(kg)";
            this.box_weight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.box_weight.Width = 100;
            // 
            // qty
            // 
            this.qty.Text = "계약수량";
            this.qty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.qty.Width = 70;
            // 
            // manager
            // 
            this.manager.Text = "담당자";
            this.manager.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.manager.Width = 70;
            // 
            // id
            // 
            this.id.Text = "id";
            this.id.Width = 0;
            // 
            // sub_id
            // 
            this.sub_id.Text = "sub_id";
            this.sub_id.Width = 0;
            // 
            // eta_status
            // 
            this.eta_status.Text = "정확도";
            this.eta_status.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("한컴 고딕", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(13, 244);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(423, 14);
            this.label1.TabIndex = 25;
            this.label1.Text = "2. 70% : ETA(입항일) 기준으로 +5일 이후를 통관일자로 계산하여 변경될 가능성 다소 높음.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("한컴 고딕", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(13, 258);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(306, 14);
            this.label2.TabIndex = 26;
            this.label2.Text = "3. 80% : 입력된 창고입고예정일 +5일  이후를 통관일자로 계산함. ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("한컴 고딕", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(13, 272);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(215, 14);
            this.label3.TabIndex = 27;
            this.label3.Text = "4. 90% : 매입 담당자가 직접 입력한 통관일자.";
            // 
            // arriveMemoPanel
            // 
            this.arriveMemoPanel.AutoScroll = true;
            this.arriveMemoPanel.BackColor = System.Drawing.Color.White;
            this.arriveMemoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.arriveMemoPanel.Location = new System.Drawing.Point(17, 314);
            this.arriveMemoPanel.Name = "arriveMemoPanel";
            this.arriveMemoPanel.Size = new System.Drawing.Size(764, 215);
            this.arriveMemoPanel.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("한컴 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(14, 297);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 17);
            this.label4.TabIndex = 29;
            this.label4.Text = "메모";
            // 
            // btnInserMemo
            // 
            this.btnInserMemo.BackColor = System.Drawing.Color.White;
            this.btnInserMemo.Font = new System.Drawing.Font("휴먼모음T", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInserMemo.Location = new System.Drawing.Point(603, 535);
            this.btnInserMemo.Name = "btnInserMemo";
            this.btnInserMemo.Size = new System.Drawing.Size(177, 63);
            this.btnInserMemo.TabIndex = 32;
            this.btnInserMemo.Text = "등    록";
            this.btnInserMemo.UseVisualStyleBackColor = false;
            this.btnInserMemo.Click += new System.EventHandler(this.btnInserMemo_Click);
            // 
            // txtContent
            // 
            this.txtContent.Location = new System.Drawing.Point(17, 535);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(580, 63);
            this.txtContent.TabIndex = 33;
            this.txtContent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtContent_KeyDown);
            // 
            // ArriveInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(796, 610);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.btnInserMemo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.arriveMemoPanel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.scheduleList);
            this.Controls.Add(this.lbQty);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.lbOrigin);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.lbBoxweight);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.lbSizes);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.lbProduct);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label10);
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ArriveInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "입고예정";
            this.Load += new System.EventHandler(this.ArriveInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbOrigin;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lbBoxweight;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lbSizes;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lbProduct;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lbQty;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ListView scheduleList;
        private System.Windows.Forms.ColumnHeader cc_status;
        private System.Windows.Forms.ColumnHeader etd;
        private System.Windows.Forms.ColumnHeader eta;
        private System.Windows.Forms.ColumnHeader warehousing_date;
        private System.Windows.Forms.ColumnHeader pending_date;
        private System.Windows.Forms.ColumnHeader box_weight;
        private System.Windows.Forms.ColumnHeader qty;
        private System.Windows.Forms.ColumnHeader manager;
        private System.Windows.Forms.ColumnHeader id;
        private System.Windows.Forms.ColumnHeader sub_id;
        private System.Windows.Forms.ColumnHeader eta_status;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel arriveMemoPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnInserMemo;
        private System.Windows.Forms.TextBox txtContent;
    }
}