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
    public partial class DayUnit : UserControl
    {
        DateTime dt;
        bool isHoliday;
        Calendar cal;
        public DayUnit(Calendar calendar, DateTime sDate, int[] no, string[] name)
        {
            InitializeComponent();
            dt = sDate;
            cal = calendar;
            //공휴일 확인
            for (int i = 0; i < no.Length; i++)
            {
                DateTime tmpDate = new DateTime(sDate.Year, sDate.Month, no[i]);
                if (sDate.Day == no[i])
                {
                    isHoliday = true;
                }

                if (sDate.Year == DateTime.Now.Year && sDate.Month == DateTime.Now.Month && sDate.Day == DateTime.Now.Day)
                    this.BackColor = Color.LightBlue;
            }
        }
        public DayUnit(Calendar calendar, DateTime sDate, int[] no, string[] name, DateTime today)
        {
            InitializeComponent();
            dt = sDate;
            cal = calendar;
            //공휴일 확인
            for (int i = 0; i < no.Length; i++)
            {
                DateTime tmpDate = new DateTime(sDate.Year, sDate.Month, no[i]);
                if (sDate.Day == no[i])
                {
                    isHoliday = true;
                }

                if (sDate.Year == today.Year && sDate.Month == today.Month && sDate.Day == today.Day)
                    this.BackColor = Color.LightBlue;
            }
        }

        private void DayUnit_Load(object sender, EventArgs e)
        {
            lbDay.Text = dt.Day.ToString();
            if (dt.DayOfWeek == DayOfWeek.Sunday || isHoliday)
            {
                lbDay.ForeColor = Color.Red;
            }
            else if (dt.DayOfWeek == DayOfWeek.Saturday)
            {   
                lbDay.ForeColor = Color.Blue;
            }
        }

        private void lbDay_Click(object sender, EventArgs e)
        {
            int sDay = Convert.ToInt16(lbDay.Text);
            cal.SelectDay(sDay);
        }
    }
}
