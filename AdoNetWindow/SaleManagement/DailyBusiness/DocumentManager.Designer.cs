namespace AdoNetWindow.SaleManagement.DailyBusiness
{
    partial class DocumentManager
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentManager));
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnCurrentDocument = new System.Windows.Forms.Button();
            this.btnSeaching = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnNesDocument = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tabPc = new System.Windows.Forms.TabPage();
            this.dgvPcDocument = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.document_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.update_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.update_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.save_path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabServer = new System.Windows.Forms.TabPage();
            this.dgvServerDocument = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.update_date2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.update_time2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnServerBackup = new System.Windows.Forms.Button();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tabPc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPcDocument)).BeginInit();
            this.tabServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServerDocument)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnCurrentDocument);
            this.panel3.Controls.Add(this.btnSeaching);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnNesDocument);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 408);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(292, 42);
            this.panel3.TabIndex = 2;
            // 
            // btnCurrentDocument
            // 
            this.btnCurrentDocument.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCurrentDocument.ForeColor = System.Drawing.Color.Blue;
            this.btnCurrentDocument.Location = new System.Drawing.Point(4, 2);
            this.btnCurrentDocument.Name = "btnCurrentDocument";
            this.btnCurrentDocument.Size = new System.Drawing.Size(66, 37);
            this.btnCurrentDocument.TabIndex = 22;
            this.btnCurrentDocument.Text = "최근문서";
            this.btnCurrentDocument.UseVisualStyleBackColor = true;
            this.btnCurrentDocument.Click += new System.EventHandler(this.btnCurrentDocument_Click);
            // 
            // btnSeaching
            // 
            this.btnSeaching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSeaching.ForeColor = System.Drawing.Color.Black;
            this.btnSeaching.Location = new System.Drawing.Point(148, 2);
            this.btnSeaching.Name = "btnSeaching";
            this.btnSeaching.Size = new System.Drawing.Size(66, 37);
            this.btnSeaching.TabIndex = 21;
            this.btnSeaching.Text = "검색(Q)";
            this.btnSeaching.UseVisualStyleBackColor = true;
            this.btnSeaching.Click += new System.EventHandler(this.btnSeaching_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(220, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 37);
            this.btnExit.TabIndex = 20;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnNesDocument
            // 
            this.btnNesDocument.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnNesDocument.ForeColor = System.Drawing.Color.Blue;
            this.btnNesDocument.Location = new System.Drawing.Point(76, 2);
            this.btnNesDocument.Name = "btnNesDocument";
            this.btnNesDocument.Size = new System.Drawing.Size(66, 37);
            this.btnNesDocument.TabIndex = 19;
            this.btnNesDocument.Text = "새문서";
            this.btnNesDocument.UseVisualStyleBackColor = true;
            this.btnNesDocument.Click += new System.EventHandler(this.btnNesDocument_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tcMain);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(292, 408);
            this.panel2.TabIndex = 3;
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tabPc);
            this.tcMain.Controls.Add(this.tabServer);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(292, 408);
            this.tcMain.TabIndex = 4;
            this.tcMain.SelectedIndexChanged += new System.EventHandler(this.tcMain_SelectedIndexChanged);
            // 
            // tabPc
            // 
            this.tabPc.Controls.Add(this.dgvPcDocument);
            this.tabPc.Location = new System.Drawing.Point(4, 22);
            this.tabPc.Name = "tabPc";
            this.tabPc.Size = new System.Drawing.Size(284, 382);
            this.tabPc.TabIndex = 0;
            this.tabPc.Text = "PC저장 백업";
            this.tabPc.UseVisualStyleBackColor = true;
            // 
            // dgvPcDocument
            // 
            this.dgvPcDocument.AllowUserToAddRows = false;
            this.dgvPcDocument.AllowUserToDeleteRows = false;
            this.dgvPcDocument.AllowUserToResizeColumns = false;
            this.dgvPcDocument.AllowUserToResizeRows = false;
            this.dgvPcDocument.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPcDocument.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPcDocument.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.document_id,
            this.update_date,
            this.update_time,
            this.save_path});
            this.dgvPcDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPcDocument.EnableHeadersVisualStyles = false;
            this.dgvPcDocument.Location = new System.Drawing.Point(0, 0);
            this.dgvPcDocument.Name = "dgvPcDocument";
            this.dgvPcDocument.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvPcDocument.RowTemplate.Height = 23;
            this.dgvPcDocument.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPcDocument.Size = new System.Drawing.Size(284, 382);
            this.dgvPcDocument.TabIndex = 0;
            this.dgvPcDocument.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvPcDocumnet_CellMouseDoubleClick);
            // 
            // document_id
            // 
            this.document_id.HeaderText = "document_id";
            this.document_id.Name = "document_id";
            this.document_id.Visible = false;
            // 
            // update_date
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.update_date.DefaultCellStyle = dataGridViewCellStyle1;
            this.update_date.HeaderText = "저장일자";
            this.update_date.Name = "update_date";
            // 
            // update_time
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.update_time.DefaultCellStyle = dataGridViewCellStyle2;
            this.update_time.HeaderText = "저장일시";
            this.update_time.Name = "update_time";
            // 
            // save_path
            // 
            this.save_path.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.save_path.HeaderText = "저장위치";
            this.save_path.Name = "save_path";
            this.save_path.Visible = false;
            // 
            // tabServer
            // 
            this.tabServer.Controls.Add(this.dgvServerDocument);
            this.tabServer.Controls.Add(this.panel1);
            this.tabServer.Location = new System.Drawing.Point(4, 22);
            this.tabServer.Name = "tabServer";
            this.tabServer.Size = new System.Drawing.Size(284, 382);
            this.tabServer.TabIndex = 1;
            this.tabServer.Text = "서버 백업";
            this.tabServer.UseVisualStyleBackColor = true;
            // 
            // dgvServerDocument
            // 
            this.dgvServerDocument.AllowUserToAddRows = false;
            this.dgvServerDocument.AllowUserToDeleteRows = false;
            this.dgvServerDocument.AllowUserToResizeColumns = false;
            this.dgvServerDocument.AllowUserToResizeRows = false;
            this.dgvServerDocument.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvServerDocument.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvServerDocument.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.update_date2,
            this.update_time2});
            this.dgvServerDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvServerDocument.EnableHeadersVisualStyles = false;
            this.dgvServerDocument.Location = new System.Drawing.Point(0, 0);
            this.dgvServerDocument.Name = "dgvServerDocument";
            this.dgvServerDocument.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvServerDocument.RowTemplate.Height = 23;
            this.dgvServerDocument.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvServerDocument.Size = new System.Drawing.Size(284, 342);
            this.dgvServerDocument.TabIndex = 1;
            this.dgvServerDocument.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvServerDocument_CellMouseDoubleClick);
            // 
            // id
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.id.DefaultCellStyle = dataGridViewCellStyle3;
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.Visible = false;
            // 
            // update_date2
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.update_date2.DefaultCellStyle = dataGridViewCellStyle4;
            this.update_date2.HeaderText = "저장일자";
            this.update_date2.Name = "update_date2";
            // 
            // update_time2
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.update_time2.DefaultCellStyle = dataGridViewCellStyle5;
            this.update_time2.HeaderText = "저장일시";
            this.update_time2.Name = "update_time2";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnServerBackup);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 342);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 40);
            this.panel1.TabIndex = 2;
            // 
            // btnServerBackup
            // 
            this.btnServerBackup.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnServerBackup.ForeColor = System.Drawing.Color.Blue;
            this.btnServerBackup.Location = new System.Drawing.Point(0, 0);
            this.btnServerBackup.Name = "btnServerBackup";
            this.btnServerBackup.Size = new System.Drawing.Size(284, 37);
            this.btnServerBackup.TabIndex = 23;
            this.btnServerBackup.Text = "SERVER 백업 생성";
            this.btnServerBackup.UseVisualStyleBackColor = true;
            this.btnServerBackup.Click += new System.EventHandler(this.btnServerBackup_Click);
            // 
            // DocumentManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DocumentManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "문서목록";
            this.Load += new System.EventHandler(this.DocumentManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DocumentManager_KeyDown);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tcMain.ResumeLayout(false);
            this.tabPc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPcDocument)).EndInit();
            this.tabServer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvServerDocument)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnNesDocument;
        private System.Windows.Forms.Button btnSeaching;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tabPc;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvPcDocument;
        private System.Windows.Forms.TabPage tabServer;
        private System.Windows.Forms.Button btnCurrentDocument;
        private System.Windows.Forms.DataGridViewTextBoxColumn document_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn update_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn update_time;
        private System.Windows.Forms.DataGridViewTextBoxColumn save_path;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvServerDocument;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn update_date2;
        private System.Windows.Forms.DataGridViewTextBoxColumn update_time2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnServerBackup;
    }
}