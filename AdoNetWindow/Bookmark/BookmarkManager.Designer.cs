namespace AdoNetWindow.SEAOVER
{
    partial class BookmarkManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BookmarkManager));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtDivision = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFormName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvBookmark = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSelect = new System.Windows.Forms.DataGridViewButtonColumn();
            this.division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.group_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.form_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lbId = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookmark)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtDivision);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtFormName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtGroupName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(750, 28);
            this.panel1.TabIndex = 1;
            // 
            // txtDivision
            // 
            this.txtDivision.Location = new System.Drawing.Point(44, 4);
            this.txtDivision.Name = "txtDivision";
            this.txtDivision.Size = new System.Drawing.Size(127, 21);
            this.txtDivision.TabIndex = 0;
            this.txtDivision.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDivision_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(7, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "구분";
            // 
            // txtFormName
            // 
            this.txtFormName.Location = new System.Drawing.Point(456, 4);
            this.txtFormName.Name = "txtFormName";
            this.txtFormName.Size = new System.Drawing.Size(283, 21);
            this.txtFormName.TabIndex = 3;
            this.txtFormName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDivision_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(419, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "제목";
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(214, 4);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(199, 21);
            this.txtGroupName.TabIndex = 1;
            this.txtGroupName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDivision_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(177, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "그룹";
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAdd.ForeColor = System.Drawing.Color.Blue;
            this.btnAdd.Location = new System.Drawing.Point(79, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(70, 36);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "추가(A)";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvBookmark);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(750, 360);
            this.panel2.TabIndex = 2;
            // 
            // dgvBookmark
            // 
            this.dgvBookmark.AllowUserToAddRows = false;
            this.dgvBookmark.AllowUserToDeleteRows = false;
            this.dgvBookmark.AllowUserToResizeRows = false;
            this.dgvBookmark.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBookmark.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.btnSelect,
            this.division,
            this.group_name,
            this.form_name,
            this.updatetime,
            this.createdatetime});
            this.dgvBookmark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBookmark.EnableHeadersVisualStyles = false;
            this.dgvBookmark.Location = new System.Drawing.Point(0, 0);
            this.dgvBookmark.Name = "dgvBookmark";
            this.dgvBookmark.RowTemplate.Height = 23;
            this.dgvBookmark.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBookmark.Size = new System.Drawing.Size(750, 360);
            this.dgvBookmark.TabIndex = 4;
            this.dgvBookmark.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProduct_CellMouseClick);
            this.dgvBookmark.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvBookmark_CellMouseDoubleClick);
            this.dgvBookmark.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvBookmark_MouseUp);
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.Width = 20;
            // 
            // btnSelect
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "선택";
            this.btnSelect.DefaultCellStyle = dataGridViewCellStyle1;
            this.btnSelect.HeaderText = "";
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Width = 30;
            // 
            // division
            // 
            this.division.HeaderText = "구분";
            this.division.Name = "division";
            // 
            // group_name
            // 
            this.group_name.HeaderText = "그룹";
            this.group_name.Name = "group_name";
            this.group_name.Width = 150;
            // 
            // form_name
            // 
            this.form_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.form_name.HeaderText = "제목";
            this.form_name.Name = "form_name";
            // 
            // updatetime
            // 
            this.updatetime.HeaderText = "수정일";
            this.updatetime.Name = "updatetime";
            this.updatetime.Width = 80;
            // 
            // createdatetime
            // 
            this.createdatetime.HeaderText = "생성일";
            this.createdatetime.Name = "createdatetime";
            this.createdatetime.Width = 80;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.btnAdd);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.lbId);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 388);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(750, 42);
            this.panel3.TabIndex = 3;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(3, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(70, 36);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "검색(Q)";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(155, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 36);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lbId
            // 
            this.lbId.AutoSize = true;
            this.lbId.Location = new System.Drawing.Point(702, 15);
            this.lbId.Name = "lbId";
            this.lbId.Size = new System.Drawing.Size(36, 12);
            this.lbId.TabIndex = 1;
            this.lbId.Text = "NULL";
            this.lbId.Visible = false;
            // 
            // BookmarkManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 430);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BookmarkManager";
            this.Text = "즐겨찾기";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.BookmarkManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BookmarkManager_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookmark)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.Label label1;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvBookmark;
        private System.Windows.Forms.Label lbId;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtFormName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtDivision;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewButtonColumn btnSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn division;
        private System.Windows.Forms.DataGridViewTextBoxColumn group_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn form_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn updatetime;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdatetime;
    }
}