using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.Common.MultiDatesCalendar
{
    public partial class DayUnit : UserControl
    {
        public bool isSelect = false;
        public DateTime dt;
        bool isHoliday;
        MultiDatesCalendar cal;
        public DayUnit()
        {
            InitializeComponent();
        }
        public DayUnit(MultiDatesCalendar calendar, DateTime sDate, int[] no, string[] name)
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
            SelectChanged();
        }

        public void SelectChanged()
        {
            isSelect = !isSelect;
            if (isSelect)
                this.BackColor = Color.LightGreen;
            else
                this.BackColor = Color.White;
        }

        private void lbDay_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SelectChanged();
            }
        }
    }
}
