using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.Common
{
    public partial class YearAndMonthSelect : Form
    {
        string sdate = null;
        public YearAndMonthSelect()
        {
            InitializeComponent();
        }

        public string GetYearAndMonth()
        {
            int year = Convert.ToInt16(DateTime.Now.ToString("yyyy"));
            for (int i = year - 20; i < year + 21; i++)
            {
                cbYear.Items.Add(i);
            }
            cbYear.Text = year.ToString();
            cbMonth.Text = DateTime.Now.ToString("MM");
            this.ShowDialog();
            return sdate;
        }


        private void YearAndMonthSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        btnSelect.PerformClick();  
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            int year;
            int month;
            if (!int.TryParse(cbYear.Text, out year))
            {
                MessageBox.Show(this,"연도를 다시 선택해주세요.");
                return;
            }
            if (!int.TryParse(cbMonth.Text, out month))
            {
                MessageBox.Show(this,"월을 다시 선택해주세요.");
                return;
            }

            sdate = new DateTime(year, month, 1).ToString("yyyy-MM-dd");
            this.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            sdate = null;
            this.Dispose();
        }
    }
}
