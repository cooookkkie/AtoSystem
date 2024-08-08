namespace AdoNetWindow.SEAOVER.PriceComparison
{
    partial class HideProductManagement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HideProductManagement));
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.cbUp = new System.Windows.Forms.CheckBox();
            this.cbDown = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvHide = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.hide_mode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.until_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hide_details = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.edit_user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbStock = new System.Windows.Forms.RadioButton();
            this.lbId = new System.Windows.Forms.Label();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.txtDownStock = new System.Windows.Forms.TextBox();
            this.txtUpStock = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUntildate = new System.Windows.Forms.TextBox();
            this.btnUntildateCalendar = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lbUnit = new System.Windows.Forms.Label();
            this.lbSizes = new System.Windows.Forms.Label();
            this.lbOrigin = new System.Windows.Forms.Label();
            this.lbProduct = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHide)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.ForeColor = System.Drawing.Color.Red;
            this.btnDelete.Location = new System.Drawing.Point(3, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(65, 30);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "삭제(D)";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInsert.Location = new System.Drawing.Point(74, 2);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(65, 30);
            this.btnInsert.TabIndex = 12;
            this.btnInsert.Text = "추가(A)";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // cbUp
            // 
            this.cbUp.AutoSize = true;
            this.cbUp.Enabled = false;
            this.cbUp.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbUp.ForeColor = System.Drawing.Color.Red;
            this.cbUp.Location = new System.Drawing.Point(26, 112);
            this.cbUp.Name = "cbUp";
            this.cbUp.Size = new System.Drawing.Size(78, 16);
            this.cbUp.TabIndex = 3;
            this.cbUp.Text = "▲ (올림)";
            this.cbUp.UseVisualStyleBackColor = true;
            this.cbUp.CheckedChanged += new System.EventHandler(this.cbUp_CheckedChanged);
            // 
            // cbDown
            // 
            this.cbDown.AutoSize = true;
            this.cbDown.Enabled = false;
            this.cbDown.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbDown.ForeColor = System.Drawing.Color.Blue;
            this.cbDown.Location = new System.Drawing.Point(26, 162);
            this.cbDown.Name = "cbDown";
            this.cbDown.Size = new System.Drawing.Size(78, 16);
            this.cbDown.TabIndex = 5;
            this.cbDown.Text = "▼ (내림)";
            this.cbDown.UseVisualStyleBackColor = true;
            this.cbDown.CheckedChanged += new System.EventHandler(this.cbDown_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvHide);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(588, 199);
            this.panel1.TabIndex = 19;
            // 
            // dgvHide
            // 
            this.dgvHide.AllowUserToAddRows = false;
            this.dgvHide.AllowUserToDeleteRows = false;
            this.dgvHide.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHide.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.chk,
            this.hide_mode,
            this.until_date,
            this.hide_details,
            this.remark,
            this.edit_user,
            this.updatetime});
            this.dgvHide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHide.EnableHeadersVisualStyles = false;
            this.dgvHide.Location = new System.Drawing.Point(0, 0);
            this.dgvHide.Name = "dgvHide";
            this.dgvHide.RowTemplate.Height = 23;
            this.dgvHide.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHide.Size = new System.Drawing.Size(588, 199);
            this.dgvHide.TabIndex = 21;
            this.dgvHide.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHide_CellClick);
            this.dgvHide.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvHide_CellPainting);
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.Width = 5;
            // 
            // chk
            // 
            this.chk.HeaderText = "";
            this.chk.Name = "chk";
            this.chk.Width = 30;
            // 
            // hide_mode
            // 
            this.hide_mode.HeaderText = "구분";
            this.hide_mode.Name = "hide_mode";
            this.hide_mode.Width = 5;
            // 
            // until_date
            // 
            this.until_date.HeaderText = "제외일자";
            this.until_date.Name = "until_date";
            // 
            // hide_details
            // 
            this.hide_details.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.hide_details.HeaderText = "설정상세";
            this.hide_details.Name = "hide_details";
            // 
            // remark
            // 
            this.remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.remark.HeaderText = "비고";
            this.remark.Name = "remark";
            // 
            // edit_user
            // 
            this.edit_user.HeaderText = "수정자";
            this.edit_user.Name = "edit_user";
            // 
            // updatetime
            // 
            this.updatetime.HeaderText = "수정일자";
            this.updatetime.Name = "updatetime";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.txtRemark);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.txtUntildate);
            this.panel2.Controls.Add(this.btnUntildateCalendar);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 199);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(588, 251);
            this.panel2.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(12, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 12);
            this.label4.TabIndex = 108;
            this.label4.Text = "비고";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(11, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 12);
            this.label10.TabIndex = 97;
            this.label10.Text = "제외일자";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(15, 57);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(260, 152);
            this.txtRemark.TabIndex = 2;
            this.txtRemark.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.rbStock);
            this.groupBox1.Controls.Add(this.cbDown);
            this.groupBox1.Controls.Add(this.lbId);
            this.groupBox1.Controls.Add(this.rbAll);
            this.groupBox1.Controls.Add(this.cbUp);
            this.groupBox1.Controls.Add(this.txtDownStock);
            this.groupBox1.Controls.Add(this.txtUpStock);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(304, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(272, 204);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "설정";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(24, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 12);
            this.label1.TabIndex = 105;
            this.label1.Text = "*제외일자까지 조건없이 모든계산 제외";
            // 
            // rbStock
            // 
            this.rbStock.AutoSize = true;
            this.rbStock.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbStock.Location = new System.Drawing.Point(8, 85);
            this.rbStock.Name = "rbStock";
            this.rbStock.Size = new System.Drawing.Size(119, 16);
            this.rbStock.TabIndex = 2;
            this.rbStock.Text = "재고수 설정제외";
            this.rbStock.UseVisualStyleBackColor = true;
            this.rbStock.CheckedChanged += new System.EventHandler(this.rbStock_CheckedChanged);
            // 
            // lbId
            // 
            this.lbId.Location = new System.Drawing.Point(212, 19);
            this.lbId.Name = "lbId";
            this.lbId.Size = new System.Drawing.Size(50, 12);
            this.lbId.TabIndex = 106;
            this.lbId.Text = "NULL";
            this.lbId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbId.Visible = false;
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Checked = true;
            this.rbAll.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbAll.Location = new System.Drawing.Point(8, 29);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(93, 16);
            this.rbAll.TabIndex = 1;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "무조건 제외";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // txtDownStock
            // 
            this.txtDownStock.Enabled = false;
            this.txtDownStock.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtDownStock.Location = new System.Drawing.Point(139, 157);
            this.txtDownStock.Name = "txtDownStock";
            this.txtDownStock.Size = new System.Drawing.Size(123, 21);
            this.txtDownStock.TabIndex = 6;
            this.txtDownStock.Text = "0";
            this.txtDownStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDownStock.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDownStock_KeyPress);
            // 
            // txtUpStock
            // 
            this.txtUpStock.Enabled = false;
            this.txtUpStock.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtUpStock.Location = new System.Drawing.Point(139, 107);
            this.txtUpStock.Name = "txtUpStock";
            this.txtUpStock.Size = new System.Drawing.Size(123, 21);
            this.txtUpStock.TabIndex = 4;
            this.txtUpStock.Text = "0";
            this.txtUpStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtUpStock.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDownStock_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(24, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(195, 12);
            this.label3.TabIndex = 100;
            this.label3.Text = "*설정한 재고수 이상일 경우만 적용";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(24, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(195, 12);
            this.label2.TabIndex = 99;
            this.label2.Text = "*설정한 재고수 이하일 경우만 적용";
            // 
            // txtUntildate
            // 
            this.txtUntildate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtUntildate.Location = new System.Drawing.Point(147, 18);
            this.txtUntildate.Name = "txtUntildate";
            this.txtUntildate.Size = new System.Drawing.Size(100, 21);
            this.txtUntildate.TabIndex = 0;
            this.txtUntildate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUntildate_KeyDown);
            // 
            // btnUntildateCalendar
            // 
            this.btnUntildateCalendar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUntildateCalendar.BackgroundImage")));
            this.btnUntildateCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnUntildateCalendar.FlatAppearance.BorderSize = 0;
            this.btnUntildateCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUntildateCalendar.Location = new System.Drawing.Point(253, 16);
            this.btnUntildateCalendar.Name = "btnUntildateCalendar";
            this.btnUntildateCalendar.Size = new System.Drawing.Size(22, 23);
            this.btnUntildateCalendar.TabIndex = 1;
            this.btnUntildateCalendar.UseVisualStyleBackColor = true;
            this.btnUntildateCalendar.Click += new System.EventHandler(this.btnUntildateCalendar_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnDelete);
            this.panel3.Controls.Add(this.btnInsert);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 215);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(588, 36);
            this.panel3.TabIndex = 19;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(519, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(65, 30);
            this.btnExit.TabIndex = 13;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lbUnit
            // 
            this.lbUnit.AutoSize = true;
            this.lbUnit.Location = new System.Drawing.Point(471, 23);
            this.lbUnit.Name = "lbUnit";
            this.lbUnit.Size = new System.Drawing.Size(36, 12);
            this.lbUnit.TabIndex = 27;
            this.lbUnit.Text = "NULL";
            this.lbUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbSizes
            // 
            this.lbSizes.AutoSize = true;
            this.lbSizes.Location = new System.Drawing.Point(353, 23);
            this.lbSizes.Name = "lbSizes";
            this.lbSizes.Size = new System.Drawing.Size(36, 12);
            this.lbSizes.TabIndex = 26;
            this.lbSizes.Text = "NULL";
            this.lbSizes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbOrigin
            // 
            this.lbOrigin.AutoSize = true;
            this.lbOrigin.Location = new System.Drawing.Point(210, 23);
            this.lbOrigin.Name = "lbOrigin";
            this.lbOrigin.Size = new System.Drawing.Size(36, 12);
            this.lbOrigin.TabIndex = 25;
            this.lbOrigin.Text = "NULL";
            this.lbOrigin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbProduct
            // 
            this.lbProduct.AutoSize = true;
            this.lbProduct.Location = new System.Drawing.Point(12, 23);
            this.lbProduct.Name = "lbProduct";
            this.lbProduct.Size = new System.Drawing.Size(36, 12);
            this.lbProduct.TabIndex = 24;
            this.lbProduct.Text = "NULL";
            this.lbProduct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(471, 4);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 12);
            this.label9.TabIndex = 23;
            this.label9.Text = "SEAOVER단위";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(353, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 12);
            this.label8.TabIndex = 22;
            this.label8.Text = "규격";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(210, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "원산지";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(11, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "품명";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.lbProduct);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Controls.Add(this.lbUnit);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.lbSizes);
            this.panel4.Controls.Add(this.lbOrigin);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 158);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(588, 41);
            this.panel4.TabIndex = 21;
            // 
            // HideProductManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 450);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "HideProductManagement";
            this.Text = "제외일자 설정";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HideProductManagement_KeyDown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHide)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.CheckBox cbUp;
        private System.Windows.Forms.CheckBox cbDown;
        private System.Windows.Forms.Panel panel1;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvHide;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbUnit;
        private System.Windows.Forms.Label lbSizes;
        private System.Windows.Forms.Label lbOrigin;
        private System.Windows.Forms.Label lbProduct;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtUntildate;
        private System.Windows.Forms.Button btnUntildateCalendar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtDownStock;
        private System.Windows.Forms.TextBox txtUpStock;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbStock;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.Label lbId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox txtRemark;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn hide_mode;
        private System.Windows.Forms.DataGridViewTextBoxColumn until_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn hide_details;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn edit_user;
        private System.Windows.Forms.DataGridViewTextBoxColumn updatetime;
    }
}