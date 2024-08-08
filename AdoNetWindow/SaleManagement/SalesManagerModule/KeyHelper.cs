using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement.SalesManagerModule
{
    public partial class KeyHelper : Form
    {
        public KeyHelper()
        {
            InitializeComponent();
        }

        private void KeyHelper_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { }
            else if (e.Modifiers == Keys.Control)
            { }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        this.Dispose();
                        break;
                }
            }
        }
    }
}
