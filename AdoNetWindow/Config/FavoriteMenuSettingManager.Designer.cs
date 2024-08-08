namespace AdoNetWindow.Config
{
    partial class FavoriteMenuSettingManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FavoriteMenuSettingManager));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.dgvSettingMenu = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.form_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvAllMenu = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.setting = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.api = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inspection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.basic_information = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recovery_principal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.import = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pendding = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SEAOVER = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSettingMenu)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllMenu)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1373, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(411, 654);
            this.panel1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(411, 654);
            this.panel4.TabIndex = 1;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.dgvSettingMenu);
            this.panel6.Controls.Add(this.panel5);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(411, 654);
            this.panel6.TabIndex = 2;
            // 
            // dgvSettingMenu
            // 
            this.dgvSettingMenu.AllowUserToAddRows = false;
            this.dgvSettingMenu.AllowUserToDeleteRows = false;
            this.dgvSettingMenu.AllowUserToResizeColumns = false;
            this.dgvSettingMenu.AllowUserToResizeRows = false;
            this.dgvSettingMenu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSettingMenu.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.category,
            this.form_name,
            this.btnDelete});
            this.dgvSettingMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSettingMenu.EnableHeadersVisualStyles = false;
            this.dgvSettingMenu.Location = new System.Drawing.Point(0, 23);
            this.dgvSettingMenu.Name = "dgvSettingMenu";
            this.dgvSettingMenu.RowHeadersWidth = 20;
            this.dgvSettingMenu.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvSettingMenu.RowTemplate.Height = 23;
            this.dgvSettingMenu.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSettingMenu.Size = new System.Drawing.Size(411, 631);
            this.dgvSettingMenu.TabIndex = 0;
            this.dgvSettingMenu.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSettingMenu_CellContentClick);
            this.dgvSettingMenu.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvSettingMenu_RowPostPaint);
            // 
            // category
            // 
            this.category.HeaderText = "구분";
            this.category.Name = "category";
            // 
            // form_name
            // 
            this.form_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.form_name.HeaderText = "메뉴";
            this.form_name.Name = "form_name";
            // 
            // btnDelete
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "삭제";
            this.btnDelete.DefaultCellStyle = dataGridViewCellStyle1;
            this.btnDelete.HeaderText = "";
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnDelete.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.btnDelete.Width = 30;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label2);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(411, 23);
            this.panel5.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "* 내 즐겨찾기 항목";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvAllMenu);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1373, 654);
            this.panel2.TabIndex = 1;
            // 
            // dgvAllMenu
            // 
            this.dgvAllMenu.AllowUserToAddRows = false;
            this.dgvAllMenu.AllowUserToDeleteRows = false;
            this.dgvAllMenu.AllowUserToResizeColumns = false;
            this.dgvAllMenu.AllowUserToResizeRows = false;
            this.dgvAllMenu.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAllMenu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAllMenu.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.setting,
            this.api,
            this.inspection,
            this.basic_information,
            this.recovery_principal,
            this.import,
            this.pendding,
            this.SEAOVER});
            this.dgvAllMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAllMenu.Location = new System.Drawing.Point(0, 23);
            this.dgvAllMenu.Name = "dgvAllMenu";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAllMenu.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvAllMenu.RowHeadersVisible = false;
            this.dgvAllMenu.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvAllMenu.RowTemplate.Height = 23;
            this.dgvAllMenu.Size = new System.Drawing.Size(1373, 631);
            this.dgvAllMenu.TabIndex = 0;
            this.dgvAllMenu.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAllMenu_CellDoubleClick);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1373, 23);
            this.panel3.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(9, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(415, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "* 즐겨찾기 항목 (더블클릭하시면 내 즐겨찾기 항목으로 추가됩니다.)";
            // 
            // setting
            // 
            this.setting.HeaderText = "설정";
            this.setting.Name = "setting";
            // 
            // api
            // 
            this.api.HeaderText = "공공데이터";
            this.api.Name = "api";
            // 
            // inspection
            // 
            this.inspection.HeaderText = "검품리스트";
            this.inspection.Name = "inspection";
            // 
            // basic_information
            // 
            this.basic_information.HeaderText = "기준정보";
            this.basic_information.Name = "basic_information";
            // 
            // recovery_principal
            // 
            this.recovery_principal.HeaderText = "영업거래처 관리";
            this.recovery_principal.Name = "recovery_principal";
            // 
            // import
            // 
            this.import.HeaderText = "수입관리";
            this.import.Name = "import";
            // 
            // pendding
            // 
            this.pendding.HeaderText = "팬딩관리";
            this.pendding.Name = "pendding";
            // 
            // SEAOVER
            // 
            this.SEAOVER.HeaderText = "씨오버";
            this.SEAOVER.Name = "SEAOVER";
            // 
            // FavoriteMenuSettingManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1784, 654);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FavoriteMenuSettingManager";
            this.Text = "즐겨찾기";
            this.Load += new System.EventHandler(this.FavoriteMenuSettingManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FavoriteMenuSettingManager_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSettingMenu)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllMenu)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.DataGridView dgvAllMenu;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvSettingMenu;
        private System.Windows.Forms.DataGridViewTextBoxColumn category;
        private System.Windows.Forms.DataGridViewTextBoxColumn form_name;
        private System.Windows.Forms.DataGridViewButtonColumn btnDelete;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn setting;
        private System.Windows.Forms.DataGridViewTextBoxColumn api;
        private System.Windows.Forms.DataGridViewTextBoxColumn inspection;
        private System.Windows.Forms.DataGridViewTextBoxColumn basic_information;
        private System.Windows.Forms.DataGridViewTextBoxColumn recovery_principal;
        private System.Windows.Forms.DataGridViewTextBoxColumn import;
        private System.Windows.Forms.DataGridViewTextBoxColumn pendding;
        private System.Windows.Forms.DataGridViewTextBoxColumn SEAOVER;
    }
}