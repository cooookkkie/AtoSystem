namespace AdoNetWindow.Pending
{
    partial class ConvertToPending
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConvertToPending));
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvUnpending = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ato_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pending_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cc_status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seaover_indate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seaover_cc_status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.update_status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnpending)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvUnpending);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(439, 423);
            this.panel1.TabIndex = 0;
            // 
            // dgvUnpending
            // 
            this.dgvUnpending.AllowUserToAddRows = false;
            this.dgvUnpending.AllowUserToDeleteRows = false;
            this.dgvUnpending.AllowUserToResizeColumns = false;
            this.dgvUnpending.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUnpending.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvUnpending.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUnpending.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chk,
            this.ato_no,
            this.pending_date,
            this.cc_status,
            this.seaover_indate,
            this.seaover_cc_status,
            this.update_status,
            this.id});
            this.dgvUnpending.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUnpending.EnableHeadersVisualStyles = false;
            this.dgvUnpending.Location = new System.Drawing.Point(0, 0);
            this.dgvUnpending.Name = "dgvUnpending";
            this.dgvUnpending.RowTemplate.Height = 23;
            this.dgvUnpending.Size = new System.Drawing.Size(439, 423);
            this.dgvUnpending.TabIndex = 0;
            this.dgvUnpending.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvUnpending_CellPainting);
            this.dgvUnpending.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvUnpending_ColumnWidthChanged);
            this.dgvUnpending.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvUnpending_Scroll);
            this.dgvUnpending.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvUnpending_Paint);
            // 
            // chk
            // 
            this.chk.HeaderText = "";
            this.chk.Name = "chk";
            this.chk.Width = 30;
            // 
            // ato_no
            // 
            this.ato_no.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ato_no.HeaderText = "AtoNo";
            this.ato_no.Name = "ato_no";
            // 
            // pending_date
            // 
            this.pending_date.HeaderText = "통관일자";
            this.pending_date.Name = "pending_date";
            this.pending_date.Width = 80;
            // 
            // cc_status
            // 
            this.cc_status.HeaderText = "통관";
            this.cc_status.Name = "cc_status";
            this.cc_status.Width = 60;
            // 
            // seaover_indate
            // 
            this.seaover_indate.HeaderText = "입고일자";
            this.seaover_indate.Name = "seaover_indate";
            this.seaover_indate.Width = 80;
            // 
            // seaover_cc_status
            // 
            this.seaover_cc_status.HeaderText = "통관";
            this.seaover_cc_status.Name = "seaover_cc_status";
            this.seaover_cc_status.Width = 60;
            // 
            // update_status
            // 
            this.update_status.HeaderText = "상태";
            this.update_status.Name = "update_status";
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Controls.Add(this.btnSelect);
            this.flowLayoutPanel1.Controls.Add(this.btnExit);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 459);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(439, 46);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelect.ForeColor = System.Drawing.Color.Blue;
            this.btnSelect.Location = new System.Drawing.Point(3, 3);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(68, 37);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.Text = "수정(A)";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(77, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(68, 37);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 423);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(439, 36);
            this.panel2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(5, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(403, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "1) 통관일자가 3개월이 지나면서 씨오버 재고가 없거나 통관인 경우";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(5, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "2) 씨오버 재고에서 통관인 경우";
            // 
            // ConvertToPending
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 505);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConvertToPending";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "씨오버 재고와 비교해 통관처리 되지않은 내역입니다.";
            this.Load += new System.EventHandler(this.ConvertToPending_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ConvertToPending_KeyDown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnpending)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvUnpending;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn ato_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn pending_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn cc_status;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_indate;
        private System.Windows.Forms.DataGridViewTextBoxColumn seaover_cc_status;
        private System.Windows.Forms.DataGridViewTextBoxColumn update_status;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}