using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.CalendarModule
{
    public partial class PendingMemo : UserControl
    {
        
        public PendingMemo()
        {
            InitializeComponent();
        }

        private void PendingMemo_Load(object sender, EventArgs e)
        {
            SetBtnStyle(button1);
        }

        private void SetBtnStyle(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;//  
            btn.ForeColor = Color.Transparent;//  
            btn.BackColor = Color.Transparent;//   
            btn.FlatAppearance.BorderSize = 0;//   
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;//    
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;// 
        }


        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020;//WS_EX_TRANSPARENT
                return cp;
            }
        }
        private int opacity;

        public int Opacity
        {
            get { return opacity; }
            set
            {
                opacity = value;
                this.InvalidateEx();
            }
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Color bk = Color.FromArgb(Opacity, this.BackColor);
            e.Graphics.FillRectangle(new SolidBrush(bk), e.ClipRectangle);
        }

        protected void InvalidateEx()
        {
            if (Parent == null)
                return;
            Rectangle rc = new Rectangle(this.Location, this.Size);
            Parent.Invalidate(rc, true);
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.FlatAppearance.BorderSize = 1;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.FlatAppearance.BorderSize = 0;
        }
    }
}