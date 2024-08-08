using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libs.CustomPanel
{
    public class CustomPanel : FlowLayoutPanel
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style &= ~0x00200000; // WS_HSCROLL
                cp.Style &= ~0x00100000; // WS_VSCROLL
                return cp;
            }
        }

        public CustomPanel() 
        {
            this.AutoScroll = true;
        }
    }
}
