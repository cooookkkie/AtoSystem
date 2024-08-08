namespace AdoNetWindow.MiniGame
{
    partial class ListManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListManager));
            this.btnBlcokBreaker = new System.Windows.Forms.Button();
            this.btnTetris = new System.Windows.Forms.Button();
            this.btnAvoidStar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBlcokBreaker
            // 
            this.btnBlcokBreaker.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnBlcokBreaker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlcokBreaker.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBlcokBreaker.Location = new System.Drawing.Point(30, 35);
            this.btnBlcokBreaker.Name = "btnBlcokBreaker";
            this.btnBlcokBreaker.Size = new System.Drawing.Size(275, 47);
            this.btnBlcokBreaker.TabIndex = 0;
            this.btnBlcokBreaker.Text = "블록깨기";
            this.btnBlcokBreaker.UseVisualStyleBackColor = false;
            this.btnBlcokBreaker.Click += new System.EventHandler(this.btnBlcokBreaker_Click);
            // 
            // btnTetris
            // 
            this.btnTetris.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnTetris.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTetris.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnTetris.Location = new System.Drawing.Point(30, 110);
            this.btnTetris.Name = "btnTetris";
            this.btnTetris.Size = new System.Drawing.Size(275, 47);
            this.btnTetris.TabIndex = 1;
            this.btnTetris.Text = "테트리스";
            this.btnTetris.UseVisualStyleBackColor = false;
            this.btnTetris.Click += new System.EventHandler(this.btnTetris_Click);
            // 
            // btnAvoidStar
            // 
            this.btnAvoidStar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnAvoidStar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAvoidStar.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAvoidStar.Location = new System.Drawing.Point(30, 184);
            this.btnAvoidStar.Name = "btnAvoidStar";
            this.btnAvoidStar.Size = new System.Drawing.Size(275, 47);
            this.btnAvoidStar.TabIndex = 2;
            this.btnAvoidStar.Text = "별피하기";
            this.btnAvoidStar.UseVisualStyleBackColor = false;
            this.btnAvoidStar.Click += new System.EventHandler(this.btnAvoidStar_Click);
            // 
            // ListManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 276);
            this.Controls.Add(this.btnAvoidStar);
            this.Controls.Add(this.btnTetris);
            this.Controls.Add(this.btnBlcokBreaker);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ListManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "미니게임";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBlcokBreaker;
        private System.Windows.Forms.Button btnTetris;
        private System.Windows.Forms.Button btnAvoidStar;
    }
}