using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.PurchaseManager
{
    public partial class GraphUnit : UserControl
    {
        public GraphUnit(Color col, string txt)
        {
            InitializeComponent();
            pnColor.BackColor = col;
            lbProduct.Text = txt;
            lbProduct.ForeColor = col;
        }
    }
}
