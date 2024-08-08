namespace AdoNetWindow.MiniGame.Tetris
{
    partial class Tetris
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
            this.components = new System.ComponentModel.Container();
            this.timer_down = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer_down
            // 
            this.timer_down.Enabled = true;
            this.timer_down.Interval = 500;
            this.timer_down.Tick += new System.EventHandler(this.timer_down_Tick);
            // 
            // Tetris
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 485);
            this.Name = "Tetris";
            this.Text = "Tetris";
            this.Load += new System.EventHandler(this.Tetris_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Tetris_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Tetris_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer_down;
    }
}