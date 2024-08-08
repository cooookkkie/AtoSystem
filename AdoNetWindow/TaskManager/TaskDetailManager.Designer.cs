namespace AdoNetWindow.TaskManager
{
    partial class TaskDetailManager
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.dgvTask = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtRemark = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPre = new System.Windows.Forms.Button();
            this.task_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.task_contents = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.task_manager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status_waiting = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.status_proceeding = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.status_completion = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cbCompletion = new System.Windows.Forms.CheckBox();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTask)).BeginInit();
            this.panel7.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnInsert);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 583);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1022, 45);
            this.panel3.TabIndex = 2;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(80, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(68, 36);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // btnInsert
            // 
            this.btnInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInsert.Location = new System.Drawing.Point(6, 4);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(68, 36);
            this.btnInsert.TabIndex = 5;
            this.btnInsert.Text = "등록(A)";
            this.btnInsert.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.label7);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 560);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1022, 23);
            this.panel5.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(931, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "yyyy-mm-dd";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(832, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "생성일";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(752, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "null";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(653, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "생성자";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel8);
            this.panel6.Controls.Add(this.panel7);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 123);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1022, 437);
            this.panel6.TabIndex = 6;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.panel9);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(74, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(948, 437);
            this.panel8.TabIndex = 7;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.btnPre);
            this.panel9.Controls.Add(this.dgvTask);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(948, 437);
            this.panel9.TabIndex = 0;
            // 
            // dgvTask
            // 
            this.dgvTask.AllowUserToAddRows = false;
            this.dgvTask.AllowUserToDeleteRows = false;
            this.dgvTask.AllowUserToOrderColumns = true;
            this.dgvTask.AllowUserToResizeColumns = false;
            this.dgvTask.AllowUserToResizeRows = false;
            this.dgvTask.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvTask.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTask.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.task_id,
            this.btnDelete,
            this.task_contents,
            this.remark,
            this.task_manager,
            this.status_waiting,
            this.status_proceeding,
            this.status_completion});
            this.dgvTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTask.EnableHeadersVisualStyles = false;
            this.dgvTask.Location = new System.Drawing.Point(0, 0);
            this.dgvTask.Name = "dgvTask";
            this.dgvTask.RowHeadersVisible = false;
            this.dgvTask.RowHeadersWidth = 30;
            this.dgvTask.RowTemplate.Height = 23;
            this.dgvTask.Size = new System.Drawing.Size(948, 437);
            this.dgvTask.TabIndex = 0;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label8);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(74, 437);
            this.panel7.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(3, 5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "세부내역";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(6, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "비고";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.txtRemark);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 30);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1022, 93);
            this.panel4.TabIndex = 4;
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(74, 4);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(945, 81);
            this.txtRemark.TabIndex = 1;
            this.txtRemark.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(774, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "부서";
            // 
            // txtDepartment
            // 
            this.txtDepartment.Location = new System.Drawing.Point(815, 5);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(204, 21);
            this.txtDepartment.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "제목";
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(134, 5);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(613, 21);
            this.txtSubject.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbCompletion);
            this.panel1.Controls.Add(this.txtDepartment);
            this.panel1.Controls.Add(this.txtSubject);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1022, 30);
            this.panel1.TabIndex = 0;
            // 
            // btnPre
            // 
            this.btnPre.BackColor = System.Drawing.Color.White;
            this.btnPre.BackgroundImage = global::AdoNetWindow.Properties.Resources.Plus2_btn;
            this.btnPre.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPre.FlatAppearance.BorderSize = 0;
            this.btnPre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPre.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPre.Location = new System.Drawing.Point(5, 5);
            this.btnPre.Name = "btnPre";
            this.btnPre.Size = new System.Drawing.Size(20, 20);
            this.btnPre.TabIndex = 33;
            this.btnPre.UseVisualStyleBackColor = false;
            // 
            // task_id
            // 
            this.task_id.HeaderText = "id";
            this.task_id.Name = "task_id";
            this.task_id.Visible = false;
            // 
            // btnDelete
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle1.NullValue = "-";
            this.btnDelete.DefaultCellStyle = dataGridViewCellStyle1;
            this.btnDelete.HeaderText = "";
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnDelete.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.btnDelete.Width = 30;
            // 
            // task_contents
            // 
            this.task_contents.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.task_contents.HeaderText = "업무";
            this.task_contents.Name = "task_contents";
            // 
            // remark
            // 
            this.remark.HeaderText = "비고";
            this.remark.Name = "remark";
            this.remark.Width = 150;
            // 
            // task_manager
            // 
            this.task_manager.HeaderText = "담당자";
            this.task_manager.Name = "task_manager";
            // 
            // status_waiting
            // 
            this.status_waiting.HeaderText = "대기";
            this.status_waiting.Name = "status_waiting";
            this.status_waiting.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.status_waiting.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.status_waiting.Width = 30;
            // 
            // status_proceeding
            // 
            this.status_proceeding.HeaderText = "진행";
            this.status_proceeding.Name = "status_proceeding";
            this.status_proceeding.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.status_proceeding.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.status_proceeding.Width = 30;
            // 
            // status_completion
            // 
            this.status_completion.HeaderText = "완료";
            this.status_completion.Name = "status_completion";
            this.status_completion.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.status_completion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.status_completion.Width = 30;
            // 
            // cbCompletion
            // 
            this.cbCompletion.AutoSize = true;
            this.cbCompletion.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbCompletion.ForeColor = System.Drawing.Color.Blue;
            this.cbCompletion.Location = new System.Drawing.Point(74, 8);
            this.cbCompletion.Name = "cbCompletion";
            this.cbCompletion.Size = new System.Drawing.Size(54, 17);
            this.cbCompletion.TabIndex = 3;
            this.cbCompletion.Text = "완료";
            this.cbCompletion.UseVisualStyleBackColor = true;
            // 
            // TaskDetailManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 628);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "TaskDetailManager";
            this.Text = "TaskDetailManager";
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTask)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvTask;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RichTextBox txtRemark;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Button btnPre;
        private System.Windows.Forms.DataGridViewTextBoxColumn task_id;
        private System.Windows.Forms.DataGridViewButtonColumn btnDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn task_contents;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn task_manager;
        private System.Windows.Forms.DataGridViewCheckBoxColumn status_waiting;
        private System.Windows.Forms.DataGridViewCheckBoxColumn status_proceeding;
        private System.Windows.Forms.DataGridViewCheckBoxColumn status_completion;
        private System.Windows.Forms.CheckBox cbCompletion;
    }
}