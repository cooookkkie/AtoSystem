namespace AdoNetWindow.CalendarModule
{
    partial class ReadingDay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReadingDay));
            this.lbPay = new System.Windows.Forms.Label();
            this.lbAdd = new System.Windows.Forms.Label();
            this.pPending = new System.Windows.Forms.FlowLayoutPanel();
            this.lbname = new System.Windows.Forms.Label();
            this.lbdays = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbPay
            // 
            this.lbPay.AutoEllipsis = true;
            this.lbPay.BackColor = System.Drawing.Color.White;
            this.lbPay.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbPay.ForeColor = System.Drawing.Color.DarkRed;
            this.lbPay.Location = new System.Drawing.Point(6, 1);
            this.lbPay.Name = "lbPay";
            this.lbPay.Size = new System.Drawing.Size(364, 30);
            this.lbPay.TabIndex = 20;
            this.lbPay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbAdd
            // 
            this.lbAdd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbAdd.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbAdd.Location = new System.Drawing.Point(377, 2);
            this.lbAdd.Name = "lbAdd";
            this.lbAdd.Size = new System.Drawing.Size(29, 31);
            this.lbAdd.TabIndex = 19;
            this.lbAdd.Text = "+";
            this.lbAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbAdd.Click += new System.EventHandler(this.lbAdd_Click);
            // 
            // pPending
            // 
            this.pPending.AutoScroll = true;
            this.pPending.BackColor = System.Drawing.Color.White;
            this.pPending.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pPending.Location = new System.Drawing.Point(0, 38);
            this.pPending.Name = "pPending";
            this.pPending.Size = new System.Drawing.Size(538, 300);
            this.pPending.TabIndex = 18;
            // 
            // lbname
            // 
            this.lbname.AutoSize = true;
            this.lbname.BackColor = System.Drawing.Color.White;
            this.lbname.Location = new System.Drawing.Point(40, 6);
            this.lbname.Name = "lbname";
            this.lbname.Size = new System.Drawing.Size(0, 12);
            this.lbname.TabIndex = 17;
            // 
            // lbdays
            // 
            this.lbdays.AutoSize = true;
            this.lbdays.BackColor = System.Drawing.Color.White;
            this.lbdays.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbdays.Location = new System.Drawing.Point(4, 7);
            this.lbdays.Name = "lbdays";
            this.lbdays.Size = new System.Drawing.Size(34, 21);
            this.lbdays.TabIndex = 16;
            this.lbdays.Text = "00";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.lbdays);
            this.panel1.Controls.Add(this.lbname);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(538, 38);
            this.panel1.TabIndex = 21;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbPay);
            this.panel2.Controls.Add(this.lbAdd);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(127, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(409, 36);
            this.panel2.TabIndex = 21;
            // 
            // ReadingDay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(538, 338);
            this.Controls.Add(this.pPending);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "ReadingDay";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ReadingDay_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbPay;
        private System.Windows.Forms.Label lbAdd;
        internal System.Windows.Forms.FlowLayoutPanel pPending;
        private System.Windows.Forms.Label lbname;
        private System.Windows.Forms.Label lbdays;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}