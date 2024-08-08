using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.DashboardForSales.MultiDashboard
{
    public partial class ProductUnit : UserControl
    {
        public ProductUnit(string product, string origin, int heightCnt)
        {
            InitializeComponent();

            int adjustHeight = 4;
            if(heightCnt < 2)
                adjustHeight = 0;
            else if (heightCnt < 3)
                adjustHeight = 2;


            lbProduct.Text = product;   
            this.Height = this.Height * heightCnt + (adjustHeight * (heightCnt));
            lbProduct.BackColor = Color.Beige;
        }
        public ProductUnit(string txt)
        {
            InitializeComponent();

            lbProduct.Text = txt;
            lbProduct.BackColor = Color.Linen;
        }
    }
}
