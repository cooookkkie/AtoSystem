namespace AdoNetWindow.Pending
{
    partial class MissingShipmentManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MissingShipmentManager));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSearching = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStandardDate = new System.Windows.Forms.Button();
            this.txtStandardDate = new System.Windows.Forms.TextBox();
            this.txtAtono = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvPending = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.custom_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contract_year = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ato_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contract_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bl_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shipment_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.etd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnShipmentDateUpdate = new System.Windows.Forms.Button();
            this.btnEtdUpdate = new System.Windows.Forms.Button();
            this.btnEtaUpdate = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPending)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtManager);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtAtono);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnStandardDate);
            this.panel1.Controls.Add(this.txtStandardDate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(683, 29);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnEtaUpdate);
            this.panel2.Controls.Add(this.btnEtdUpdate);
            this.panel2.Controls.Add(this.btnShipmentDateUpdate);
            this.panel2.Controls.Add(this.dgvPending);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 29);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(683, 503);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnUpdate);
            this.panel3.Controls.Add(this.btnSearching);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 532);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(683, 40);
            this.panel3.TabIndex = 2;
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.ForeColor = System.Drawing.Color.Black;
            this.btnSearching.Location = new System.Drawing.Point(3, 2);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(66, 37);
            this.btnSearching.TabIndex = 7;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            this.btnSearching.Click += new System.EventHandler(this.btnSearching_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(147, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "기준일자";
            // 
            // btnStandardDate
            // 
            this.btnStandardDate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStandardDate.BackgroundImage")));
            this.btnStandardDate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStandardDate.FlatAppearance.BorderSize = 0;
            this.btnStandardDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStandardDate.Location = new System.Drawing.Point(132, 3);
            this.btnStandardDate.Name = "btnStandardDate";
            this.btnStandardDate.Size = new System.Drawing.Size(22, 23);
            this.btnStandardDate.TabIndex = 110;
            this.btnStandardDate.UseVisualStyleBackColor = true;
            this.btnStandardDate.Click += new System.EventHandler(this.btnStandardDate_Click);
            // 
            // txtStandardDate
            // 
            this.txtStandardDate.Location = new System.Drawing.Point(61, 4);
            this.txtStandardDate.MaxLength = 10;
            this.txtStandardDate.Name = "txtStandardDate";
            this.txtStandardDate.Size = new System.Drawing.Size(69, 21);
            this.txtStandardDate.TabIndex = 109;
            this.txtStandardDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtStandardDate_KeyDown);
            // 
            // txtAtono
            // 
            this.txtAtono.Location = new System.Drawing.Point(218, 4);
            this.txtAtono.MaxLength = 10;
            this.txtAtono.Name = "txtAtono";
            this.txtAtono.Size = new System.Drawing.Size(128, 21);
            this.txtAtono.TabIndex = 112;
            this.txtAtono.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtStandardDate_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(173, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 12);
            this.label2.TabIndex = 111;
            this.label2.Text = "Ato No";
            // 
            // dgvPending
            // 
            this.dgvPending.AllowUserToAddRows = false;
            this.dgvPending.AllowUserToDeleteRows = false;
            this.dgvPending.AllowUserToResizeRows = false;
            this.dgvPending.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPending.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.custom_id,
            this.contract_year,
            this.ato_no,
            this.contract_no,
            this.bl_no,
            this.shipment_date,
            this.etd,
            this.eta,
            this.manager});
            this.dgvPending.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPending.EnableHeadersVisualStyles = false;
            this.dgvPending.Location = new System.Drawing.Point(0, 0);
            this.dgvPending.Name = "dgvPending";
            this.dgvPending.RowTemplate.Height = 23;
            this.dgvPending.Size = new System.Drawing.Size(683, 503);
            this.dgvPending.TabIndex = 0;
            this.dgvPending.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPending_CellValueChanged);
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(412, 4);
            this.txtManager.MaxLength = 10;
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(128, 21);
            this.txtManager.TabIndex = 114;
            this.txtManager.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtStandardDate_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(367, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 113;
            this.label3.Text = "담당자";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUpdate.ForeColor = System.Drawing.Color.Blue;
            this.btnUpdate.Location = new System.Drawing.Point(75, 2);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(66, 37);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "수정(A)";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // custom_id
            // 
            this.custom_id.HeaderText = "custom_id";
            this.custom_id.Name = "custom_id";
            this.custom_id.Visible = false;
            // 
            // contract_year
            // 
            this.contract_year.HeaderText = "계약연도";
            this.contract_year.Name = "contract_year";
            this.contract_year.Width = 60;
            // 
            // ato_no
            // 
            this.ato_no.HeaderText = "Ato No";
            this.ato_no.Name = "ato_no";
            this.ato_no.Width = 60;
            // 
            // contract_no
            // 
            this.contract_no.HeaderText = "C/N";
            this.contract_no.Name = "contract_no";
            // 
            // bl_no
            // 
            this.bl_no.HeaderText = "B/L";
            this.bl_no.Name = "bl_no";
            // 
            // shipment_date
            // 
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.Linen;
            this.shipment_date.DefaultCellStyle = dataGridViewCellStyle13;
            this.shipment_date.HeaderText = "계약선적일";
            this.shipment_date.Name = "shipment_date";
            this.shipment_date.Width = 75;
            // 
            // etd
            // 
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.Linen;
            this.etd.DefaultCellStyle = dataGridViewCellStyle14;
            this.etd.HeaderText = "ETD";
            this.etd.Name = "etd";
            this.etd.Width = 75;
            // 
            // eta
            // 
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.Linen;
            this.eta.DefaultCellStyle = dataGridViewCellStyle15;
            this.eta.HeaderText = "ETA";
            this.eta.Name = "eta";
            this.eta.Width = 75;
            // 
            // manager
            // 
            this.manager.HeaderText = "담당자";
            this.manager.Name = "manager";
            this.manager.Width = 60;
            // 
            // btnShipmentDateUpdate
            // 
            this.btnShipmentDateUpdate.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnShipmentDateUpdate.ForeColor = System.Drawing.Color.Blue;
            this.btnShipmentDateUpdate.Location = new System.Drawing.Point(362, 3);
            this.btnShipmentDateUpdate.Name = "btnShipmentDateUpdate";
            this.btnShipmentDateUpdate.Size = new System.Drawing.Size(73, 20);
            this.btnShipmentDateUpdate.TabIndex = 10;
            this.btnShipmentDateUpdate.Text = "계약선적일";
            this.btnShipmentDateUpdate.UseVisualStyleBackColor = true;
            this.btnShipmentDateUpdate.Click += new System.EventHandler(this.btnShipmentDateUpdate_Click);
            // 
            // btnEtdUpdate
            // 
            this.btnEtdUpdate.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnEtdUpdate.ForeColor = System.Drawing.Color.Blue;
            this.btnEtdUpdate.Location = new System.Drawing.Point(437, 3);
            this.btnEtdUpdate.Name = "btnEtdUpdate";
            this.btnEtdUpdate.Size = new System.Drawing.Size(73, 20);
            this.btnEtdUpdate.TabIndex = 11;
            this.btnEtdUpdate.Text = "ETD";
            this.btnEtdUpdate.UseVisualStyleBackColor = true;
            this.btnEtdUpdate.Click += new System.EventHandler(this.btnEtdUpdate_Click);
            // 
            // btnEtaUpdate
            // 
            this.btnEtaUpdate.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnEtaUpdate.ForeColor = System.Drawing.Color.Blue;
            this.btnEtaUpdate.Location = new System.Drawing.Point(513, 3);
            this.btnEtaUpdate.Name = "btnEtaUpdate";
            this.btnEtaUpdate.Size = new System.Drawing.Size(73, 20);
            this.btnEtaUpdate.TabIndex = 12;
            this.btnEtaUpdate.Text = "ETA";
            this.btnEtaUpdate.UseVisualStyleBackColor = true;
            this.btnEtaUpdate.Click += new System.EventHandler(this.btnEtaUpdate_Click);
            // 
            // MissingShipmentManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 572);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MissingShipmentManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ETD/ETA 미기입 선적내역";
            this.Load += new System.EventHandler(this.MissingShipmentManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MissingShipmentManager_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPending)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnStandardDate;
        private System.Windows.Forms.TextBox txtStandardDate;
        private System.Windows.Forms.TextBox txtAtono;
        private System.Windows.Forms.Label label2;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvPending;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn custom_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn contract_year;
        private System.Windows.Forms.DataGridViewTextBoxColumn ato_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn contract_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn bl_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn shipment_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn etd;
        private System.Windows.Forms.DataGridViewTextBoxColumn eta;
        private System.Windows.Forms.DataGridViewTextBoxColumn manager;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnEtaUpdate;
        private System.Windows.Forms.Button btnEtdUpdate;
        private System.Windows.Forms.Button btnShipmentDateUpdate;
    }
}