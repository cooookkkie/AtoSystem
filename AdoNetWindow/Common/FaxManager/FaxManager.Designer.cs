namespace AdoNetWindow.Common.FaxManager
{
    partial class FaxManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FaxManager));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnResultDownload = new System.Windows.Forms.Button();
            this.btnAttachmentSelect = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel14 = new System.Windows.Forms.Panel();
            this.panel18 = new System.Windows.Forms.Panel();
            this.panel19 = new System.Windows.Forms.Panel();
            this.panel20 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel16 = new System.Windows.Forms.Panel();
            this.panel21 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel17 = new System.Windows.Forms.Panel();
            this.btnDeleteComplete = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbIndividualAttachmentFile = new System.Windows.Forms.RadioButton();
            this.btnGetData = new System.Windows.Forms.Button();
            this.rbBatchAttachmentFile = new System.Windows.Forms.RadioButton();
            this.btnAddressBook = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.panel11 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel13 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbMinute = new System.Windows.Forms.ComboBox();
            this.cbHour = new System.Windows.Forms.ComboBox();
            this.cbDay = new System.Windows.Forms.ComboBox();
            this.cbMonth = new System.Windows.Forms.ComboBox();
            this.rbReservation = new System.Windows.Forms.RadioButton();
            this.rbImmediately = new System.Windows.Forms.RadioButton();
            this.cbYear = new System.Windows.Forms.ComboBox();
            this.panel15 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDialog = new System.Windows.Forms.DataGridViewButtonColumn();
            this.real_attachment_path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.attachment_path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.send_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reDial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fax_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.job_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvFaxNumber = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.real_path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvAttachments = new Libs.MultiHeaderGrid.MultiHeaderGrid();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel14.SuspendLayout();
            this.panel18.SuspendLayout();
            this.panel19.SuspendLayout();
            this.panel20.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel16.SuspendLayout();
            this.panel21.SuspendLayout();
            this.panel17.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel13.SuspendLayout();
            this.panel15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFaxNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttachments)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Controls.Add(this.btnSend);
            this.flowLayoutPanel1.Controls.Add(this.btnPause);
            this.flowLayoutPanel1.Controls.Add(this.btnExit);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 696);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1413, 41);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSend.Location = new System.Drawing.Point(3, 3);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 34);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "보내기(A)";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnPause
            // 
            this.btnPause.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPause.ForeColor = System.Drawing.Color.Red;
            this.btnPause.Location = new System.Drawing.Point(84, 3);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(75, 34);
            this.btnPause.TabIndex = 4;
            this.btnPause.Text = "팩스중단";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(165, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 34);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnResultDownload
            // 
            this.btnResultDownload.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnResultDownload.Location = new System.Drawing.Point(106, -1);
            this.btnResultDownload.Name = "btnResultDownload";
            this.btnResultDownload.Size = new System.Drawing.Size(109, 28);
            this.btnResultDownload.TabIndex = 4;
            this.btnResultDownload.Text = "결과 내려받기";
            this.btnResultDownload.UseVisualStyleBackColor = true;
            this.btnResultDownload.Click += new System.EventHandler(this.btnResultDownload_Click);
            // 
            // btnAttachmentSelect
            // 
            this.btnAttachmentSelect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAttachmentSelect.Location = new System.Drawing.Point(294, -2);
            this.btnAttachmentSelect.Name = "btnAttachmentSelect";
            this.btnAttachmentSelect.Size = new System.Drawing.Size(88, 28);
            this.btnAttachmentSelect.TabIndex = 1;
            this.btnAttachmentSelect.Text = "파일선택";
            this.btnAttachmentSelect.UseVisualStyleBackColor = true;
            this.btnAttachmentSelect.Click += new System.EventHandler(this.btnAttachmentSelect_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel14);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1413, 181);
            this.panel4.TabIndex = 0;
            // 
            // panel14
            // 
            this.panel14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel14.Controls.Add(this.panel18);
            this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel14.Location = new System.Drawing.Point(200, 0);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(1213, 181);
            this.panel14.TabIndex = 1;
            // 
            // panel18
            // 
            this.panel18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel18.Controls.Add(this.panel19);
            this.panel18.Controls.Add(this.panel20);
            this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel18.Location = new System.Drawing.Point(0, 0);
            this.panel18.Name = "panel18";
            this.panel18.Size = new System.Drawing.Size(1211, 179);
            this.panel18.TabIndex = 2;
            // 
            // panel19
            // 
            this.panel19.Controls.Add(this.dgvAttachments);
            this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel19.Location = new System.Drawing.Point(0, 25);
            this.panel19.Name = "panel19";
            this.panel19.Size = new System.Drawing.Size(1209, 152);
            this.panel19.TabIndex = 1;
            // 
            // panel20
            // 
            this.panel20.Controls.Add(this.panel3);
            this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel20.Location = new System.Drawing.Point(0, 0);
            this.panel20.Name = "panel20";
            this.panel20.Size = new System.Drawing.Size(1209, 25);
            this.panel20.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnAttachmentSelect);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(829, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(380, 25);
            this.panel3.TabIndex = 20;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(200, 181);
            this.panel5.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 181);
            this.label1.TabIndex = 1;
            this.label1.Text = "일괄발송 첨부파일";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.panel9);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel12);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1413, 696);
            this.panel1.TabIndex = 0;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel7);
            this.panel6.Controls.Add(this.panel8);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 181);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1413, 452);
            this.panel6.TabIndex = 1;
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.panel16);
            this.panel7.Controls.Add(this.panel17);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(200, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1213, 452);
            this.panel7.TabIndex = 1;
            // 
            // panel16
            // 
            this.panel16.Controls.Add(this.dgvFaxNumber);
            this.panel16.Controls.Add(this.panel21);
            this.panel16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel16.Location = new System.Drawing.Point(0, 25);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(1211, 425);
            this.panel16.TabIndex = 1;
            // 
            // panel21
            // 
            this.panel21.Controls.Add(this.progressBar1);
            this.panel21.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel21.Location = new System.Drawing.Point(0, 400);
            this.panel21.Name = "panel21";
            this.panel21.Size = new System.Drawing.Size(1211, 25);
            this.panel21.TabIndex = 1;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(0, 0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1211, 25);
            this.progressBar1.TabIndex = 0;
            // 
            // panel17
            // 
            this.panel17.Controls.Add(this.btnDeleteComplete);
            this.panel17.Controls.Add(this.panel2);
            this.panel17.Controls.Add(this.btnResultDownload);
            this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel17.Location = new System.Drawing.Point(0, 0);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(1211, 25);
            this.panel17.TabIndex = 2;
            // 
            // btnDeleteComplete
            // 
            this.btnDeleteComplete.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDeleteComplete.ForeColor = System.Drawing.Color.Blue;
            this.btnDeleteComplete.Location = new System.Drawing.Point(-1, -1);
            this.btnDeleteComplete.Name = "btnDeleteComplete";
            this.btnDeleteComplete.Size = new System.Drawing.Size(109, 28);
            this.btnDeleteComplete.TabIndex = 20;
            this.btnDeleteComplete.Text = "성공내역 삭제";
            this.btnDeleteComplete.UseVisualStyleBackColor = true;
            this.btnDeleteComplete.Click += new System.EventHandler(this.btnDeleteComplete_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbIndividualAttachmentFile);
            this.panel2.Controls.Add(this.btnGetData);
            this.panel2.Controls.Add(this.rbBatchAttachmentFile);
            this.panel2.Controls.Add(this.btnAddressBook);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(742, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(469, 25);
            this.panel2.TabIndex = 19;
            // 
            // rbIndividualAttachmentFile
            // 
            this.rbIndividualAttachmentFile.AutoSize = true;
            this.rbIndividualAttachmentFile.Checked = true;
            this.rbIndividualAttachmentFile.Location = new System.Drawing.Point(141, 5);
            this.rbIndividualAttachmentFile.Name = "rbIndividualAttachmentFile";
            this.rbIndividualAttachmentFile.Size = new System.Drawing.Size(99, 16);
            this.rbIndividualAttachmentFile.TabIndex = 21;
            this.rbIndividualAttachmentFile.TabStop = true;
            this.rbIndividualAttachmentFile.Text = "개별 첨부파일";
            this.rbIndividualAttachmentFile.UseVisualStyleBackColor = true;
            // 
            // btnGetData
            // 
            this.btnGetData.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnGetData.ForeColor = System.Drawing.Color.Blue;
            this.btnGetData.Location = new System.Drawing.Point(274, -2);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(109, 28);
            this.btnGetData.TabIndex = 4;
            this.btnGetData.Text = "파일 불러오기";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // rbBatchAttachmentFile
            // 
            this.rbBatchAttachmentFile.AutoSize = true;
            this.rbBatchAttachmentFile.Location = new System.Drawing.Point(12, 5);
            this.rbBatchAttachmentFile.Name = "rbBatchAttachmentFile";
            this.rbBatchAttachmentFile.Size = new System.Drawing.Size(123, 16);
            this.rbBatchAttachmentFile.TabIndex = 18;
            this.rbBatchAttachmentFile.Text = "일괄발송 첨부파일";
            this.rbBatchAttachmentFile.UseVisualStyleBackColor = true;
            // 
            // btnAddressBook
            // 
            this.btnAddressBook.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAddressBook.Location = new System.Drawing.Point(381, -2);
            this.btnAddressBook.Name = "btnAddressBook";
            this.btnAddressBook.Size = new System.Drawing.Size(88, 28);
            this.btnAddressBook.TabIndex = 17;
            this.btnAddressBook.Text = "주소록 관리";
            this.btnAddressBook.UseVisualStyleBackColor = true;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label2);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(200, 452);
            this.panel8.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 452);
            this.label2.TabIndex = 1;
            this.label2.Text = "수신처";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.panel10);
            this.panel9.Controls.Add(this.panel11);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel9.Location = new System.Drawing.Point(0, 633);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1413, 28);
            this.panel9.TabIndex = 2;
            // 
            // panel10
            // 
            this.panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel10.Controls.Add(this.txtSubject);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(200, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(1213, 28);
            this.panel10.TabIndex = 1;
            // 
            // txtSubject
            // 
            this.txtSubject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSubject.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSubject.Location = new System.Drawing.Point(0, 0);
            this.txtSubject.Multiline = true;
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(1211, 26);
            this.txtSubject.TabIndex = 0;
            this.txtSubject.Text = "Ato handling product";
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.label3);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel11.Location = new System.Drawing.Point(0, 0);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(200, 28);
            this.panel11.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 28);
            this.label3.TabIndex = 1;
            this.label3.Text = "팩스 제목";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.panel13);
            this.panel12.Controls.Add(this.panel15);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel12.Location = new System.Drawing.Point(0, 661);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(1413, 35);
            this.panel12.TabIndex = 3;
            // 
            // panel13
            // 
            this.panel13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel13.Controls.Add(this.label7);
            this.panel13.Controls.Add(this.label6);
            this.panel13.Controls.Add(this.label5);
            this.panel13.Controls.Add(this.cbMinute);
            this.panel13.Controls.Add(this.cbHour);
            this.panel13.Controls.Add(this.cbDay);
            this.panel13.Controls.Add(this.cbMonth);
            this.panel13.Controls.Add(this.rbReservation);
            this.panel13.Controls.Add(this.rbImmediately);
            this.panel13.Controls.Add(this.cbYear);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel13.Location = new System.Drawing.Point(200, 0);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(1213, 35);
            this.panel13.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(382, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(10, 12);
            this.label7.TabIndex = 11;
            this.label7.Text = ":";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(266, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(12, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "-";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(212, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "-";
            // 
            // cbMinute
            // 
            this.cbMinute.Enabled = false;
            this.cbMinute.FormattingEnabled = true;
            this.cbMinute.Items.AddRange(new object[] {
            "00",
            "05",
            "10",
            "15",
            "20",
            "25",
            "30",
            "35",
            "40",
            "45",
            "50",
            "55"});
            this.cbMinute.Location = new System.Drawing.Point(393, 7);
            this.cbMinute.Name = "cbMinute";
            this.cbMinute.Size = new System.Drawing.Size(38, 20);
            this.cbMinute.TabIndex = 8;
            this.cbMinute.Text = "00";
            // 
            // cbHour
            // 
            this.cbHour.Enabled = false;
            this.cbHour.FormattingEnabled = true;
            this.cbHour.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24"});
            this.cbHour.Location = new System.Drawing.Point(343, 7);
            this.cbHour.Name = "cbHour";
            this.cbHour.Size = new System.Drawing.Size(38, 20);
            this.cbHour.TabIndex = 7;
            this.cbHour.Text = "12";
            // 
            // cbDay
            // 
            this.cbDay.Enabled = false;
            this.cbDay.FormattingEnabled = true;
            this.cbDay.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.cbDay.Location = new System.Drawing.Point(279, 7);
            this.cbDay.Name = "cbDay";
            this.cbDay.Size = new System.Drawing.Size(38, 20);
            this.cbDay.TabIndex = 6;
            // 
            // cbMonth
            // 
            this.cbMonth.Enabled = false;
            this.cbMonth.FormattingEnabled = true;
            this.cbMonth.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.cbMonth.Location = new System.Drawing.Point(226, 7);
            this.cbMonth.Name = "cbMonth";
            this.cbMonth.Size = new System.Drawing.Size(38, 20);
            this.cbMonth.TabIndex = 5;
            this.cbMonth.Text = "12";
            // 
            // rbReservation
            // 
            this.rbReservation.AutoSize = true;
            this.rbReservation.Location = new System.Drawing.Point(89, 9);
            this.rbReservation.Name = "rbReservation";
            this.rbReservation.Size = new System.Drawing.Size(71, 16);
            this.rbReservation.TabIndex = 1;
            this.rbReservation.Text = "예약발송";
            this.rbReservation.UseVisualStyleBackColor = true;
            this.rbReservation.Click += new System.EventHandler(this.rbReservation_Click);
            // 
            // rbImmediately
            // 
            this.rbImmediately.AutoSize = true;
            this.rbImmediately.Checked = true;
            this.rbImmediately.Location = new System.Drawing.Point(5, 8);
            this.rbImmediately.Name = "rbImmediately";
            this.rbImmediately.Size = new System.Drawing.Size(71, 16);
            this.rbImmediately.TabIndex = 0;
            this.rbImmediately.TabStop = true;
            this.rbImmediately.Text = "즉시발송";
            this.rbImmediately.UseVisualStyleBackColor = true;
            this.rbImmediately.Click += new System.EventHandler(this.rbReservation_Click);
            // 
            // cbYear
            // 
            this.cbYear.Enabled = false;
            this.cbYear.FormattingEnabled = true;
            this.cbYear.Items.AddRange(new object[] {
            "2022",
            "2023",
            "2024",
            "2025",
            "2026",
            "2027",
            "2028",
            "2029",
            "2030"});
            this.cbYear.Location = new System.Drawing.Point(162, 7);
            this.cbYear.Name = "cbYear";
            this.cbYear.Size = new System.Drawing.Size(48, 20);
            this.cbYear.TabIndex = 4;
            // 
            // panel15
            // 
            this.panel15.Controls.Add(this.label4);
            this.panel15.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel15.Location = new System.Drawing.Point(0, 0);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(200, 35);
            this.panel15.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(200, 35);
            this.label4.TabIndex = 1;
            this.label4.Text = "발송옵션";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDialog
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "가져오기";
            this.btnDialog.DefaultCellStyle = dataGridViewCellStyle1;
            this.btnDialog.HeaderText = "";
            this.btnDialog.Name = "btnDialog";
            this.btnDialog.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnDialog.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.btnDialog.Width = 80;
            // 
            // real_attachment_path
            // 
            this.real_attachment_path.HeaderText = "첨부파일";
            this.real_attachment_path.Name = "real_attachment_path";
            this.real_attachment_path.Visible = false;
            // 
            // attachment_path
            // 
            this.attachment_path.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.attachment_path.HeaderText = "첨부파일";
            this.attachment_path.Name = "attachment_path";
            // 
            // send_time
            // 
            this.send_time.HeaderText = "전송시간";
            this.send_time.Name = "send_time";
            // 
            // reDial
            // 
            this.reDial.HeaderText = "재다이얼";
            this.reDial.Name = "reDial";
            // 
            // status
            // 
            this.status.FillWeight = 50.5461F;
            this.status.HeaderText = "상태";
            this.status.Name = "status";
            // 
            // fax_number
            // 
            this.fax_number.HeaderText = "팩스번호";
            this.fax_number.Name = "fax_number";
            // 
            // name
            // 
            this.name.HeaderText = "To";
            this.name.Name = "name";
            // 
            // job_id
            // 
            this.job_id.HeaderText = "TASK ID";
            this.job_id.Name = "job_id";
            this.job_id.Width = 50;
            // 
            // dgvFaxNumber
            // 
            this.dgvFaxNumber.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFaxNumber.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.job_id,
            this.name,
            this.fax_number,
            this.status,
            this.reDial,
            this.send_time,
            this.attachment_path,
            this.real_attachment_path,
            this.btnDialog});
            this.dgvFaxNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFaxNumber.EnableHeadersVisualStyles = false;
            this.dgvFaxNumber.Location = new System.Drawing.Point(0, 0);
            this.dgvFaxNumber.Name = "dgvFaxNumber";
            this.dgvFaxNumber.RowTemplate.Height = 23;
            this.dgvFaxNumber.Size = new System.Drawing.Size(1211, 400);
            this.dgvFaxNumber.TabIndex = 0;
            this.dgvFaxNumber.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFaxNumber_CellContentClick);
            this.dgvFaxNumber.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvFaxNumber_CellMouseDoubleClick);
            this.dgvFaxNumber.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFaxNumber_CellValueChanged);
            // 
            // real_path
            // 
            this.real_path.HeaderText = "첨부파일";
            this.real_path.Name = "real_path";
            this.real_path.Visible = false;
            // 
            // path
            // 
            this.path.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.path.FillWeight = 75.44193F;
            this.path.HeaderText = "첨부파일";
            this.path.Name = "path";
            // 
            // dgvAttachments
            // 
            this.dgvAttachments.AllowDrop = true;
            this.dgvAttachments.AllowUserToAddRows = false;
            this.dgvAttachments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttachments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.path,
            this.real_path});
            this.dgvAttachments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAttachments.EnableHeadersVisualStyles = false;
            this.dgvAttachments.Location = new System.Drawing.Point(0, 0);
            this.dgvAttachments.Name = "dgvAttachments";
            this.dgvAttachments.RowTemplate.Height = 23;
            this.dgvAttachments.Size = new System.Drawing.Size(1209, 152);
            this.dgvAttachments.TabIndex = 0;
            this.dgvAttachments.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvAttachments_RowsAdded);
            this.dgvAttachments.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvAttachments_DragDrop);
            this.dgvAttachments.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgvAttachments_DragEnter);
            // 
            // FaxManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1413, 737);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FaxManager";
            this.Text = "FaxManager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FaxManager_FormClosing);
            this.Load += new System.EventHandler(this.FaxManager_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel14.ResumeLayout(false);
            this.panel18.ResumeLayout(false);
            this.panel19.ResumeLayout(false);
            this.panel20.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel16.ResumeLayout(false);
            this.panel21.ResumeLayout(false);
            this.panel17.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.panel15.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFaxNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttachments)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnAttachmentSelect;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel18;
        private System.Windows.Forms.Panel panel19;
        private System.Windows.Forms.Panel panel20;
        private System.Windows.Forms.Button btnAddressBook;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbMinute;
        private System.Windows.Forms.ComboBox cbHour;
        private System.Windows.Forms.ComboBox cbMonth;
        private System.Windows.Forms.RadioButton rbReservation;
        private System.Windows.Forms.RadioButton rbImmediately;
        private System.Windows.Forms.ComboBox cbYear;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbDay;
        private System.Windows.Forms.Button btnGetData;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnResultDownload;
        private System.Windows.Forms.Button btnDeleteComplete;
        private System.Windows.Forms.RadioButton rbIndividualAttachmentFile;
        private System.Windows.Forms.RadioButton rbBatchAttachmentFile;
        private System.Windows.Forms.Panel panel21;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnPause;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvAttachments;
        private System.Windows.Forms.DataGridViewTextBoxColumn path;
        private System.Windows.Forms.DataGridViewTextBoxColumn real_path;
        private Libs.MultiHeaderGrid.MultiHeaderGrid dgvFaxNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn job_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn fax_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn reDial;
        private System.Windows.Forms.DataGridViewTextBoxColumn send_time;
        private System.Windows.Forms.DataGridViewTextBoxColumn attachment_path;
        private System.Windows.Forms.DataGridViewTextBoxColumn real_attachment_path;
        private System.Windows.Forms.DataGridViewButtonColumn btnDialog;
    }
}