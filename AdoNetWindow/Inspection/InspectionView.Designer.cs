namespace AdoNetWindow.Inspection
{
    partial class InspectionView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InspectionView));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cbFiles = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPre = new System.Windows.Forms.Button();
            this.pbInspection = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInspection)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbFiles);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(807, 29);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "파일목록";
            // 
            // cbFiles
            // 
            this.cbFiles.FormattingEnabled = true;
            this.cbFiles.Location = new System.Drawing.Point(74, 5);
            this.cbFiles.Name = "cbFiles";
            this.cbFiles.Size = new System.Drawing.Size(258, 20);
            this.cbFiles.TabIndex = 0;
            this.cbFiles.SelectedIndexChanged += new System.EventHandler(this.cbFiles_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pbInspection);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 29);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(807, 755);
            this.panel2.TabIndex = 1;
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.White;
            this.btnNext.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNext.BackgroundImage")));
            this.btnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNext.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnNext.Location = new System.Drawing.Point(68, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(22, 22);
            this.btnNext.TabIndex = 35;
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPre
            // 
            this.btnPre.BackColor = System.Drawing.Color.White;
            this.btnPre.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPre.BackgroundImage")));
            this.btnPre.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPre.FlatAppearance.BorderSize = 0;
            this.btnPre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPre.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPre.Location = new System.Drawing.Point(40, 3);
            this.btnPre.Name = "btnPre";
            this.btnPre.Size = new System.Drawing.Size(22, 22);
            this.btnPre.TabIndex = 34;
            this.btnPre.UseVisualStyleBackColor = false;
            this.btnPre.Click += new System.EventHandler(this.btnPre_Click);
            // 
            // pbInspection
            // 
            this.pbInspection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbInspection.Location = new System.Drawing.Point(0, 0);
            this.pbInspection.Name = "pbInspection";
            this.pbInspection.Size = new System.Drawing.Size(807, 755);
            this.pbInspection.TabIndex = 0;
            this.pbInspection.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnPre);
            this.panel3.Controls.Add(this.btnNext);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(712, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(95, 29);
            this.panel3.TabIndex = 36;
            // 
            // InspectionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 784);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InspectionView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VIEW";
            this.Load += new System.EventHandler(this.InspectionView_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbInspection)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbFiles;
        private System.Windows.Forms.Panel panel2;
        internal System.Windows.Forms.Button btnNext;
        internal System.Windows.Forms.Button btnPre;
        private System.Windows.Forms.PictureBox pbInspection;
        private System.Windows.Forms.Panel panel3;
    }
}