namespace AdoNetWindow.Config
{
    partial class GroupManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupManager));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgvItem = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.item_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSelectedManager = new System.Windows.Forms.TextBox();
            this.lbId = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSelectedEditUser = new System.Windows.Forms.TextBox();
            this.txtSelectedUpdatetime = new System.Windows.Forms.TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSelectedGroupName = new System.Windows.Forms.TextBox();
            this.txtSelectedDivision = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgvGroup = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.txtDivision = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnGetItem = new System.Windows.Forms.Button();
            this.btnDeleteGroup = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.btnAddGroup = new System.Windows.Forms.Button();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSelectGroup = new System.Windows.Forms.DataGridViewButtonColumn();
            this.division = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.group_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItem)).BeginInit();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroup)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1000, 521);
            this.panel1.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.dgvItem);
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(392, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(608, 521);
            this.panel5.TabIndex = 2;
            // 
            // dgvItem
            // 
            this.dgvItem.AllowUserToAddRows = false;
            this.dgvItem.AllowUserToDeleteRows = false;
            this.dgvItem.AllowUserToResizeColumns = false;
            this.dgvItem.AllowUserToResizeRows = false;
            this.dgvItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.item_code});
            this.dgvItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItem.EnableHeadersVisualStyles = false;
            this.dgvItem.Location = new System.Drawing.Point(0, 22);
            this.dgvItem.Name = "dgvItem";
            this.dgvItem.RowTemplate.Height = 23;
            this.dgvItem.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItem.Size = new System.Drawing.Size(606, 475);
            this.dgvItem.TabIndex = 0;
            // 
            // item_code
            // 
            this.item_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.item_code.HeaderText = "ITEM";
            this.item_code.Name = "item_code";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label6);
            this.panel7.Controls.Add(this.txtSelectedManager);
            this.panel7.Controls.Add(this.lbId);
            this.panel7.Controls.Add(this.label4);
            this.panel7.Controls.Add(this.label5);
            this.panel7.Controls.Add(this.txtSelectedEditUser);
            this.panel7.Controls.Add(this.txtSelectedUpdatetime);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(0, 497);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(606, 22);
            this.panel7.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(26, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "담당자";
            // 
            // txtSelectedManager
            // 
            this.txtSelectedManager.Enabled = false;
            this.txtSelectedManager.Location = new System.Drawing.Point(76, 1);
            this.txtSelectedManager.Name = "txtSelectedManager";
            this.txtSelectedManager.Size = new System.Drawing.Size(135, 21);
            this.txtSelectedManager.TabIndex = 7;
            // 
            // lbId
            // 
            this.lbId.AutoSize = true;
            this.lbId.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbId.Location = new System.Drawing.Point(5, 6);
            this.lbId.Name = "lbId";
            this.lbId.Size = new System.Drawing.Size(40, 12);
            this.lbId.TabIndex = 6;
            this.lbId.Text = "NULL";
            this.lbId.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(421, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "수정자";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(214, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "수정일자";
            // 
            // txtSelectedEditUser
            // 
            this.txtSelectedEditUser.Enabled = false;
            this.txtSelectedEditUser.Location = new System.Drawing.Point(471, 1);
            this.txtSelectedEditUser.Name = "txtSelectedEditUser";
            this.txtSelectedEditUser.Size = new System.Drawing.Size(135, 21);
            this.txtSelectedEditUser.TabIndex = 3;
            // 
            // txtSelectedUpdatetime
            // 
            this.txtSelectedUpdatetime.Enabled = false;
            this.txtSelectedUpdatetime.Location = new System.Drawing.Point(280, 1);
            this.txtSelectedUpdatetime.Name = "txtSelectedUpdatetime";
            this.txtSelectedUpdatetime.Size = new System.Drawing.Size(135, 21);
            this.txtSelectedUpdatetime.TabIndex = 2;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label2);
            this.panel6.Controls.Add(this.label1);
            this.panel6.Controls.Add(this.txtSelectedGroupName);
            this.panel6.Controls.Add(this.txtSelectedDivision);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(606, 22);
            this.panel6.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(214, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "그룹명";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(10, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "구분";
            // 
            // txtSelectedGroupName
            // 
            this.txtSelectedGroupName.Enabled = false;
            this.txtSelectedGroupName.Location = new System.Drawing.Point(264, 1);
            this.txtSelectedGroupName.Name = "txtSelectedGroupName";
            this.txtSelectedGroupName.Size = new System.Drawing.Size(344, 21);
            this.txtSelectedGroupName.TabIndex = 3;
            // 
            // txtSelectedDivision
            // 
            this.txtSelectedDivision.Enabled = false;
            this.txtSelectedDivision.Location = new System.Drawing.Point(46, 1);
            this.txtSelectedDivision.Name = "txtSelectedDivision";
            this.txtSelectedDivision.Size = new System.Drawing.Size(162, 21);
            this.txtSelectedDivision.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.dgvGroup);
            this.panel4.Controls.Add(this.panel3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(392, 521);
            this.panel4.TabIndex = 1;
            // 
            // dgvGroup
            // 
            this.dgvGroup.AllowUserToAddRows = false;
            this.dgvGroup.AllowUserToDeleteRows = false;
            this.dgvGroup.AllowUserToResizeColumns = false;
            this.dgvGroup.AllowUserToResizeRows = false;
            this.dgvGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGroup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.btnSelectGroup,
            this.division,
            this.group_name,
            this.manager});
            this.dgvGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGroup.EnableHeadersVisualStyles = false;
            this.dgvGroup.Location = new System.Drawing.Point(0, 22);
            this.dgvGroup.Name = "dgvGroup";
            this.dgvGroup.RowHeadersVisible = false;
            this.dgvGroup.RowTemplate.Height = 23;
            this.dgvGroup.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGroup.Size = new System.Drawing.Size(390, 497);
            this.dgvGroup.TabIndex = 0;
            this.dgvGroup.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGroup_CellContentClick);
            this.dgvGroup.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGroup_CellMouseClick);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.txtManager);
            this.panel3.Controls.Add(this.txtGroupName);
            this.panel3.Controls.Add(this.txtDivision);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(390, 22);
            this.panel3.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(10, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "검색";
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(288, 1);
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(106, 21);
            this.txtManager.TabIndex = 2;
            this.txtManager.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDivision_KeyDown);
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(144, 1);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(145, 21);
            this.txtGroupName.TabIndex = 1;
            this.txtGroupName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDivision_KeyDown);
            // 
            // txtDivision
            // 
            this.txtDivision.Location = new System.Drawing.Point(45, 1);
            this.txtDivision.Name = "txtDivision";
            this.txtDivision.Size = new System.Drawing.Size(100, 21);
            this.txtDivision.TabIndex = 0;
            this.txtDivision.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDivision_KeyDown);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSelect);
            this.panel2.Controls.Add(this.btnGetItem);
            this.panel2.Controls.Add(this.btnDeleteGroup);
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Controls.Add(this.btnAddItem);
            this.panel2.Controls.Add(this.btnAddGroup);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 521);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1000, 43);
            this.panel2.TabIndex = 1;
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelect.ForeColor = System.Drawing.Color.Blue;
            this.btnSelect.Location = new System.Drawing.Point(3, 3);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(68, 35);
            this.btnSelect.TabIndex = 13;
            this.btnSelect.Text = "선택(S)";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Visible = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnGetItem
            // 
            this.btnGetItem.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnGetItem.ForeColor = System.Drawing.Color.Blue;
            this.btnGetItem.Location = new System.Drawing.Point(787, 4);
            this.btnGetItem.Name = "btnGetItem";
            this.btnGetItem.Size = new System.Drawing.Size(133, 35);
            this.btnGetItem.TabIndex = 12;
            this.btnGetItem.Text = "ITEM 불러오기(F4)";
            this.btnGetItem.UseVisualStyleBackColor = true;
            this.btnGetItem.Click += new System.EventHandler(this.btnGetItem_Click);
            // 
            // btnDeleteGroup
            // 
            this.btnDeleteGroup.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDeleteGroup.ForeColor = System.Drawing.Color.Red;
            this.btnDeleteGroup.Location = new System.Drawing.Point(216, 4);
            this.btnDeleteGroup.Name = "btnDeleteGroup";
            this.btnDeleteGroup.Size = new System.Drawing.Size(77, 35);
            this.btnDeleteGroup.TabIndex = 10;
            this.btnDeleteGroup.Text = "그룹삭제";
            this.btnDeleteGroup.UseVisualStyleBackColor = true;
            this.btnDeleteGroup.Click += new System.EventHandler(this.btnDeleteGroup_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(926, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(71, 35);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAddItem.ForeColor = System.Drawing.Color.Black;
            this.btnAddItem.Location = new System.Drawing.Point(681, 4);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(100, 35);
            this.btnAddItem.TabIndex = 8;
            this.btnAddItem.Text = "ITEM 저장(A)";
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAddGroup.Location = new System.Drawing.Point(299, 4);
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Size = new System.Drawing.Size(88, 35);
            this.btnAddGroup.TabIndex = 6;
            this.btnAddGroup.Text = "그룹추가(W)";
            this.btnAddGroup.UseVisualStyleBackColor = true;
            this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.Width = 5;
            // 
            // btnSelectGroup
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "선택";
            this.btnSelectGroup.DefaultCellStyle = dataGridViewCellStyle1;
            this.btnSelectGroup.HeaderText = "";
            this.btnSelectGroup.Name = "btnSelectGroup";
            this.btnSelectGroup.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnSelectGroup.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.btnSelectGroup.Width = 40;
            // 
            // division
            // 
            this.division.HeaderText = "구분";
            this.division.Name = "division";
            // 
            // group_name
            // 
            this.group_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.group_name.HeaderText = "그룹명";
            this.group_name.Name = "group_name";
            // 
            // manager
            // 
            this.manager.HeaderText = "담당자";
            this.manager.Name = "manager";
            // 
            // GroupManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 564);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "GroupManager";
            this.Text = "그룹관리";
            this.Load += new System.EventHandler(this.GroupManager_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GroupManager_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItem)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroup)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvItem;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSelectedGroupName;
        private System.Windows.Forms.TextBox txtSelectedDivision;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvGroup;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.TextBox txtDivision;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Button btnAddGroup;
        private System.Windows.Forms.Button btnDeleteGroup;
        private System.Windows.Forms.Button btnGetItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSelectedEditUser;
        private System.Windows.Forms.TextBox txtSelectedUpdatetime;
        private System.Windows.Forms.Label lbId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSelectedManager;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_code;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewButtonColumn btnSelectGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn division;
        private System.Windows.Forms.DataGridViewTextBoxColumn group_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn manager;
    }
}