namespace AdoNetWindow.OverseaManufacturingBusiness
{
    partial class AddData
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddData));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lbLastImdate = new System.Windows.Forms.Label();
            this.btnRemoveDuplicate = new System.Windows.Forms.Button();
            this.btnDataUpload = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.dgvData = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pname_kor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pname_eng = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manufacturing = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.im_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.until_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_country = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.e_country = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.frozen_num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lot_num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1315, 676);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.lbLastImdate);
            this.panel2.Controls.Add(this.btnRemoveDuplicate);
            this.panel2.Controls.Add(this.btnDataUpload);
            this.panel2.Controls.Add(this.btnInsert);
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 676);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1315, 40);
            this.panel2.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(576, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 36);
            this.button1.TabIndex = 14;
            this.button1.Text = "중복데이터 삭제";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbLastImdate
            // 
            this.lbLastImdate.Location = new System.Drawing.Point(1028, 12);
            this.lbLastImdate.Name = "lbLastImdate";
            this.lbLastImdate.Size = new System.Drawing.Size(190, 19);
            this.lbLastImdate.TabIndex = 13;
            this.lbLastImdate.Text = "최근 처리일자 : yyyy-MM-dd";
            this.lbLastImdate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnRemoveDuplicate
            // 
            this.btnRemoveDuplicate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRemoveDuplicate.ForeColor = System.Drawing.Color.Red;
            this.btnRemoveDuplicate.Location = new System.Drawing.Point(179, 2);
            this.btnRemoveDuplicate.Name = "btnRemoveDuplicate";
            this.btnRemoveDuplicate.Size = new System.Drawing.Size(78, 36);
            this.btnRemoveDuplicate.TabIndex = 12;
            this.btnRemoveDuplicate.Text = "중복데이터 삭제";
            this.btnRemoveDuplicate.UseVisualStyleBackColor = true;
            this.btnRemoveDuplicate.Click += new System.EventHandler(this.btnRemoveDuplicate_Click);
            // 
            // btnDataUpload
            // 
            this.btnDataUpload.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDataUpload.ForeColor = System.Drawing.Color.Blue;
            this.btnDataUpload.Location = new System.Drawing.Point(76, 2);
            this.btnDataUpload.Name = "btnDataUpload";
            this.btnDataUpload.Size = new System.Drawing.Size(97, 36);
            this.btnDataUpload.TabIndex = 11;
            this.btnDataUpload.Text = "파일불러오기";
            this.btnDataUpload.UseVisualStyleBackColor = true;
            this.btnDataUpload.Click += new System.EventHandler(this.btnDataUpload_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInsert.ForeColor = System.Drawing.Color.Blue;
            this.btnInsert.Location = new System.Drawing.Point(3, 2);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(67, 37);
            this.btnInsert.TabIndex = 3;
            this.btnInsert.Text = "등록(A)";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(263, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(67, 37);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.AllowUserToResizeColumns = false;
            this.dgvData.AllowUserToResizeRows = false;
            this.dgvData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.no,
            this.division,
            this.importer,
            this.pname_kor,
            this.pname_eng,
            this.product_type,
            this.manufacturing,
            this.im_date,
            this.until_date,
            this.m_country,
            this.e_country,
            this.frozen_num,
            this.lot_num,
            this.remark});
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.EnableHeadersVisualStyles = false;
            this.dgvData.Location = new System.Drawing.Point(0, 0);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvData.RowTemplate.Height = 23;
            this.dgvData.Size = new System.Drawing.Size(1315, 676);
            this.dgvData.TabIndex = 2;
            // 
            // no
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.no.DefaultCellStyle = dataGridViewCellStyle1;
            this.no.FillWeight = 33.8036F;
            this.no.HeaderText = "no";
            this.no.Name = "no";
            // 
            // division
            // 
            this.division.FillWeight = 45.96668F;
            this.division.HeaderText = "구분";
            this.division.Name = "division";
            // 
            // importer
            // 
            this.importer.FillWeight = 146.2262F;
            this.importer.HeaderText = "수입업체";
            this.importer.Name = "importer";
            // 
            // pname_kor
            // 
            this.pname_kor.FillWeight = 155.9757F;
            this.pname_kor.HeaderText = "품명(한글)";
            this.pname_kor.Name = "pname_kor";
            // 
            // pname_eng
            // 
            this.pname_eng.FillWeight = 167.5128F;
            this.pname_eng.HeaderText = "품명(영어)";
            this.pname_eng.Name = "pname_eng";
            // 
            // product_type
            // 
            this.product_type.HeaderText = "품목(유형)";
            this.product_type.Name = "product_type";
            // 
            // manufacturing
            // 
            this.manufacturing.FillWeight = 91.7526F;
            this.manufacturing.HeaderText = "해외제조업소";
            this.manufacturing.Name = "manufacturing";
            // 
            // im_date
            // 
            this.im_date.HeaderText = "처리일자";
            this.im_date.Name = "im_date";
            // 
            // until_date
            // 
            this.until_date.HeaderText = "유통기한";
            this.until_date.Name = "until_date";
            // 
            // m_country
            // 
            this.m_country.FillWeight = 91.7526F;
            this.m_country.HeaderText = "제조국";
            this.m_country.Name = "m_country";
            // 
            // e_country
            // 
            this.e_country.FillWeight = 91.7526F;
            this.e_country.HeaderText = "수출국";
            this.e_country.Name = "e_country";
            // 
            // frozen_num
            // 
            this.frozen_num.FillWeight = 91.7526F;
            this.frozen_num.HeaderText = "냉동전환번호";
            this.frozen_num.Name = "frozen_num";
            // 
            // lot_num
            // 
            this.lot_num.FillWeight = 91.7526F;
            this.lot_num.HeaderText = "이력추적번호";
            this.lot_num.Name = "lot_num";
            // 
            // remark
            // 
            this.remark.FillWeight = 91.7526F;
            this.remark.HeaderText = "비고";
            this.remark.Name = "remark";
            // 
            // AddData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1315, 716);
            this.Controls.Add(this.dgvData);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "AddData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "데이터 등록";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddData_KeyDown);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnExit;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvData;
        private System.Windows.Forms.Button btnDataUpload;
        private System.Windows.Forms.Button btnRemoveDuplicate;
        private System.Windows.Forms.Label lbLastImdate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn no;
        private System.Windows.Forms.DataGridViewTextBoxColumn division;
        private System.Windows.Forms.DataGridViewTextBoxColumn importer;
        private System.Windows.Forms.DataGridViewTextBoxColumn pname_kor;
        private System.Windows.Forms.DataGridViewTextBoxColumn pname_eng;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn manufacturing;
        private System.Windows.Forms.DataGridViewTextBoxColumn im_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn until_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_country;
        private System.Windows.Forms.DataGridViewTextBoxColumn e_country;
        private System.Windows.Forms.DataGridViewTextBoxColumn frozen_num;
        private System.Windows.Forms.DataGridViewTextBoxColumn lot_num;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
    }
}