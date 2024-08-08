namespace AdoNetWindow.TaskManager
{
    partial class TaskManager
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbIsCompletion = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSizes = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvTask = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearching = new System.Windows.Forms.Button();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.department = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.start_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.end_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.target = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.is_completion = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.create_user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.create_datetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTask)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbIsCompletion);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtSizes);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtOrigin);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(991, 33);
            this.panel1.TabIndex = 0;
            // 
            // cbIsCompletion
            // 
            this.cbIsCompletion.FormattingEnabled = true;
            this.cbIsCompletion.Items.AddRange(new object[] {
            "전체",
            "완료",
            "진행중",
            "대기",
            "보류",
            "취소"});
            this.cbIsCompletion.Location = new System.Drawing.Point(817, 6);
            this.cbIsCompletion.Name = "cbIsCompletion";
            this.cbIsCompletion.Size = new System.Drawing.Size(53, 20);
            this.cbIsCompletion.TabIndex = 115;
            this.cbIsCompletion.Text = "전체";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(782, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 114;
            this.label6.Text = "완료";
            // 
            // txtSizes
            // 
            this.txtSizes.Location = new System.Drawing.Point(494, 6);
            this.txtSizes.Name = "txtSizes";
            this.txtSizes.Size = new System.Drawing.Size(122, 21);
            this.txtSizes.TabIndex = 109;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(459, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 113;
            this.label5.Text = "부서";
            // 
            // txtOrigin
            // 
            this.txtOrigin.Location = new System.Drawing.Point(40, 6);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.Size = new System.Drawing.Size(413, 21);
            this.txtOrigin.TabIndex = 108;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(147, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 12);
            this.label4.TabIndex = 112;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 111;
            this.label3.Text = "제목";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvTask);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(991, 541);
            this.panel2.TabIndex = 1;
            // 
            // dgvTask
            // 
            this.dgvTask.AllowUserToAddRows = false;
            this.dgvTask.AllowUserToDeleteRows = false;
            this.dgvTask.AllowUserToOrderColumns = true;
            this.dgvTask.AllowUserToResizeColumns = false;
            this.dgvTask.AllowUserToResizeRows = false;
            this.dgvTask.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.subject,
            this.department,
            this.remark,
            this.start_date,
            this.end_date,
            this.target,
            this.is_completion,
            this.create_user,
            this.create_datetime});
            this.dgvTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTask.EnableHeadersVisualStyles = false;
            this.dgvTask.Location = new System.Drawing.Point(0, 0);
            this.dgvTask.Name = "dgvTask";
            this.dgvTask.RowTemplate.Height = 23;
            this.dgvTask.Size = new System.Drawing.Size(991, 541);
            this.dgvTask.TabIndex = 0;
            this.dgvTask.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTask_CellContentClick);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnAdd);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnSearching);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 574);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(991, 41);
            this.panel3.TabIndex = 2;
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAdd.Location = new System.Drawing.Point(75, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(68, 36);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "생성(A)";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(147, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(68, 36);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearching
            // 
            this.btnSearching.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearching.Location = new System.Drawing.Point(4, 2);
            this.btnSearching.Name = "btnSearching";
            this.btnSearching.Size = new System.Drawing.Size(68, 36);
            this.btnSearching.TabIndex = 5;
            this.btnSearching.Text = "검색(Q)";
            this.btnSearching.UseVisualStyleBackColor = true;
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.Visible = false;
            // 
            // subject
            // 
            this.subject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.subject.HeaderText = "프로젝트";
            this.subject.Name = "subject";
            // 
            // department
            // 
            this.department.HeaderText = "부서";
            this.department.Name = "department";
            // 
            // remark
            // 
            this.remark.HeaderText = "비고";
            this.remark.Name = "remark";
            this.remark.Visible = false;
            // 
            // start_date
            // 
            this.start_date.HeaderText = "시작일";
            this.start_date.Name = "start_date";
            this.start_date.Width = 70;
            // 
            // end_date
            // 
            this.end_date.HeaderText = "종료일";
            this.end_date.Name = "end_date";
            this.end_date.Width = 70;
            // 
            // target
            // 
            this.target.HeaderText = "진행률";
            this.target.Name = "target";
            this.target.Width = 70;
            // 
            // is_completion
            // 
            this.is_completion.HeaderText = "완료";
            this.is_completion.Name = "is_completion";
            this.is_completion.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.is_completion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.is_completion.Width = 60;
            // 
            // create_user
            // 
            this.create_user.HeaderText = "생성자";
            this.create_user.Name = "create_user";
            this.create_user.Visible = false;
            this.create_user.Width = 80;
            // 
            // create_datetime
            // 
            this.create_datetime.HeaderText = "생성일";
            this.create_datetime.Name = "create_datetime";
            this.create_datetime.Visible = false;
            this.create_datetime.Width = 80;
            // 
            // TaskManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 615);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "TaskManager";
            this.Text = "TaskManager";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTask)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvTask;
        private System.Windows.Forms.ComboBox cbIsCompletion;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSizes;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearching;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn subject;
        private System.Windows.Forms.DataGridViewTextBoxColumn department;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn start_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn end_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn target;
        private System.Windows.Forms.DataGridViewCheckBoxColumn is_completion;
        private System.Windows.Forms.DataGridViewTextBoxColumn create_user;
        private System.Windows.Forms.DataGridViewTextBoxColumn create_datetime;
    }
}