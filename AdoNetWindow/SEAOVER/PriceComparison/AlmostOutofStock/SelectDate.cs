using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SEAOVER.PriceComparison.AlmostOutofStock
{
    public partial class SelectDate : Form
    {
        int days = 0;
        public SelectDate()
        {
            InitializeComponent();
        }
        public int GetDays()
        {
            this.ShowDialog();

            return days;
        }

        private void btnSixtyDays_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            days = Convert.ToInt16(btn.Text.Replace("일", ""));
            this.Dispose();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            days = 0;
            this.Dispose();
        }
    }
}
