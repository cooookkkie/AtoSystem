namespace AdoNetWindow.SEAOVER
{
    partial class GetFormData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetFormData));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbIsFavorite = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCreateuser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFormname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGroup = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvFormList = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.rbSushi = new System.Windows.Forms.RadioButton();
            this.rbNormal = new System.Windows.Forms.RadioButton();
            this.is_favorite = new System.Windows.Forms.DataGridViewImageColumn();
            this.is_notification = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.group_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.form_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.create_user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.edit_user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.form_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFormList)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbIsFavorite);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtCreateuser);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtFormname);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtGroup);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(819, 32);
            this.panel1.TabIndex = 0;
            // 
            // cbIsFavorite
            // 
            this.cbIsFavorite.AutoSize = true;
            this.cbIsFavorite.Location = new System.Drawing.Point(577, 9);
            this.cbIsFavorite.Name = "cbIsFavorite";
            this.cbIsFavorite.Size = new System.Drawing.Size(84, 16);
            this.cbIsFavorite.TabIndex = 9;
            this.cbIsFavorite.Text = "즐겨찾기만";
            this.cbIsFavorite.UseVisualStyleBackColor = true;
            this.cbIsFavorite.CheckedChanged += new System.EventHandler(this.cbIsFavorite_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(408, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "생성자";
            // 
            // txtCreateuser
            // 
            this.txtCreateuser.Location = new System.Drawing.Point(455, 6);
            this.txtCreateuser.Name = "txtCreateuser";
            this.txtCreateuser.Size = new System.Drawing.Size(103, 21);
            this.txtCreateuser.TabIndex = 8;
            this.txtCreateuser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtGroup_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(155, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "제목";
            // 
            // txtFormname
            // 
            this.txtFormname.Location = new System.Drawing.Point(189, 6);
            this.txtFormname.Name = "txtFormname";
            this.txtFormname.Size = new System.Drawing.Size(213, 21);
            this.txtFormname.TabIndex = 6;
            this.txtFormname.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtGroup_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "그룹";
            // 
            // txtGroup
            // 
            this.txtGroup.Location = new System.Drawing.Point(46, 6);
            this.txtGroup.Name = "txtGroup";
            this.txtGroup.Size = new System.Drawing.Size(103, 21);
            this.txtGroup.TabIndex = 4;
            this.txtGroup.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtGroup_KeyDown);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvFormList);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 58);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(819, 482);
            this.panel2.TabIndex = 1;
            // 
            // dgvFormList
            // 
            this.dgvFormList.AllowUserToAddRows = false;
            this.dgvFormList.AllowUserToDeleteRows = false;
            this.dgvFormList.AllowUserToResizeColumns = false;
            this.dgvFormList.AllowUserToResizeRows = false;
            this.dgvFormList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvFormList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.is_favorite,
            this.is_notification,
            this.id,
            this.group_name,
            this.form_name,
            this.create_user,
            this.edit_user,
            this.updatetime,
            this.form_type});
            this.dgvFormList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFormList.EnableHeadersVisualStyles = false;
            this.dgvFormList.Location = new System.Drawing.Point(0, 0);
            this.dgvFormList.Name = "dgvFormList";
            this.dgvFormList.RowHeadersWidth = 25;
            this.dgvFormList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvFormList.RowTemplate.Height = 23;
            this.dgvFormList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFormList.Size = new System.Drawing.Size(819, 482);
            this.dgvFormList.TabIndex = 0;
            this.dgvFormList.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvFormList_CellMouseDoubleClick);
            this.dgvFormList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvFormList_MouseUp);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnSelect);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 540);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(819, 40);
            this.panel3.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(3, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(70, 35);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "검색(Q)";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(155, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 35);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelect.ForeColor = System.Drawing.Color.Blue;
            this.btnSelect.Location = new System.Drawing.Point(79, 3);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(70, 35);
            this.btnSelect.TabIndex = 3;
            this.btnSelect.Text = "선택(S)";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(14, 7);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(47, 16);
            this.rbAll.TabIndex = 10;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "전체";
            this.rbAll.UseVisualStyleBackColor = true;
            this.rbAll.Click += new System.EventHandler(this.rbSushi_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.rbSushi);
            this.panel4.Controls.Add(this.rbNormal);
            this.panel4.Controls.Add(this.rbAll);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(819, 26);
            this.panel4.TabIndex = 2;
            // 
            // rbSushi
            // 
            this.rbSushi.AutoSize = true;
            this.rbSushi.Location = new System.Drawing.Point(155, 7);
            this.rbSushi.Name = "rbSushi";
            this.rbSushi.Size = new System.Drawing.Size(83, 16);
            this.rbSushi.TabIndex = 12;
            this.rbSushi.TabStop = true;
            this.rbSushi.Text = "초밥품목서";
            this.rbSushi.UseVisualStyleBackColor = true;
            this.rbSushi.Click += new System.EventHandler(this.rbSushi_Click);
            // 
            // rbNormal
            // 
            this.rbNormal.AutoSize = true;
            this.rbNormal.Location = new System.Drawing.Point(67, 7);
            this.rbNormal.Name = "rbNormal";
            this.rbNormal.Size = new System.Drawing.Size(83, 16);
            this.rbNormal.TabIndex = 11;
            this.rbNormal.TabStop = true;
            this.rbNormal.Text = "일반품목서";
            this.rbNormal.UseVisualStyleBackColor = true;
            this.rbNormal.Click += new System.EventHandler(this.rbSushi_Click);
            // 
            // is_favorite
            // 
            this.is_favorite.HeaderText = "";
            this.is_favorite.Name = "is_favorite";
            this.is_favorite.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.is_favorite.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.is_favorite.Width = 30;
            // 
            // is_notification
            // 
            this.is_notification.HeaderText = "is_notification";
            this.is_notification.Name = "is_notification";
            this.is_notification.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.is_notification.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.is_notification.Visible = false;
            // 
            // id
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.id.DefaultCellStyle = dataGridViewCellStyle1;
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.Width = 40;
            // 
            // group_name
            // 
            this.group_name.HeaderText = "그룹";
            this.group_name.Name = "group_name";
            // 
            // form_name
            // 
            this.form_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.form_name.HeaderText = "품목서 제목";
            this.form_name.Name = "form_name";
            // 
            // create_user
            // 
            this.create_user.HeaderText = "생성자";
            this.create_user.Name = "create_user";
            this.create_user.Width = 70;
            // 
            // edit_user
            // 
            this.edit_user.HeaderText = "수정자";
            this.edit_user.Name = "edit_user";
            this.edit_user.Width = 70;
            // 
            // updatetime
            // 
            this.updatetime.HeaderText = "수정일자";
            this.updatetime.Name = "updatetime";
            this.updatetime.Width = 80;
            // 
            // form_type
            // 
            this.form_type.HeaderText = "form_type";
            this.form_type.Name = "form_type";
            this.form_type.Visible = false;
            // 
            // GetFormData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 580);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GetFormData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "품목서 불러오기";
            this.Load += new System.EventHandler(this.GetFormData_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GetFormData_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFormList)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvFormList;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFormname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGroup;
        private System.Windows.Forms.CheckBox cbIsFavorite;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCreateuser;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton rbSushi;
        private System.Windows.Forms.RadioButton rbNormal;
        private System.Windows.Forms.DataGridViewImageColumn is_favorite;
        private System.Windows.Forms.DataGridViewCheckBoxColumn is_notification;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn group_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn form_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn create_user;
        private System.Windows.Forms.DataGridViewTextBoxColumn edit_user;
        private System.Windows.Forms.DataGridViewTextBoxColumn updatetime;
        private System.Windows.Forms.DataGridViewTextBoxColumn form_type;
    }
}