using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.Common
{
    public partial class Calendar : Form
    {
        string selectDate = null;
        DateTime sdate;
        Libs.Tools.Common common = new Libs.Tools.Common();
        public Calendar()
        {
            InitializeComponent();
            sdate = DateTime.Now;
        }
        public Calendar(DateTime sdate)
        {
            InitializeComponent();
            this.sdate = sdate;
        }

        public string GetDate(bool isShowDialog = false)
        {
            //ComboBox Setting
            for (int i = DateTime.Now.Year - 20; i < DateTime.Now.Year + 20; i++)
            {
                cbYear.Items.Add(i);
            }
            this.cbYear.SelectedValueChanged -= new System.EventHandler(this.cbYear_SelectedValueChanged);
            cbYear.Text = sdate.Year.ToString();
            cbMonth.Text = sdate.Month.ToString();
            this.cbYear.SelectedValueChanged += new System.EventHandler(this.cbYear_SelectedValueChanged);
            //days Setting 
            SetMonth();
            if (isShowDialog)
            {
                this.ShowDialog();
            }
            else
            {
                this.Show();
            }
            return selectDate;
        }
        #region Method
        private void SetMonth()
        {
            //clear container
            daycontainer.Controls.Clear();
            int sMonth = Convert.ToInt16(cbMonth.Text);
            int sYear = Convert.ToInt16(cbYear.Text);
            string monthname = DateTimeFormatInfo.CurrentInfo.GetMonthName(sMonth);

            //Lets get the first day of the month
            DateTime startofthemonth = new DateTime(sYear, sMonth, 1);

            //get the count of days of the month
            int days = DateTime.DaysInMonth(sYear, sMonth);

            //convert the startofthemonth to integer
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 1;

            //Holiday List
            int[] no;
            string[] name;
            common.getRedDay(sYear, sMonth, out no, out name);

            //==========================================================================
            //first lets create a blank usercontrol
            for (int i = 1; i < dayoftheweek; i++)
            {
                BlankUnit ucblack = new BlankUnit();
                daycontainer.Controls.Add(ucblack);
            }
            //==========================================================================
            //now lets create usercontrol for days
            for (int i = 1; i <= days; i++)
            {
                DateTime sDate = new DateTime(sYear, sMonth, i);
                DayUnit ucdays = new DayUnit(this, sDate, no, name, sdate);
                daycontainer.Controls.Add(ucdays);
            }

        }

        public void SelectDay(int sDay)
        {
            int sMonth = Convert.ToInt16(cbMonth.Text);
            int sYear = Convert.ToInt16(cbYear.Text);
            DateTime tmpDate = new DateTime(sYear, sMonth, sDay);
            selectDate = tmpDate.ToString("yyyy-MM-dd");
            this.Dispose();
        }
        #endregion

        #region 음력 공휴일계산=========================================================
        private void getRedDay(int year, int month, out int[] no, out string[] name1)
        {
            List<int> no2 = new List<int>();
            List<string> name2 = new List<string>();

            //특수 공휴일
            if (year == 2022)
            {
                switch (month)
                {
                    case 3:
                        no2.Add(9);
                        name2.Add("대통령 선거");
                        break;
                    case 6:
                        no2.Add(1);
                        name2.Add("지방 선거");
                        break;
                    default:
                        no = null;
                        name1 = null;

                        break;
                }
            }
            //법정 공휴일
            switch (month)
            {
                case 1:
                    no2.Add(1);
                    name2.Add("신정");
                    break;
                case 3:
                    no2.Add(1);
                    name2.Add("삼일절");
                    no2.Add(1);
                    name2.Add("삼일절");
                    break;
                case 5:
                    no2.Add(5);
                    name2.Add("어린이날");
                    break;
                case 6:
                    no2.Add(6);
                    name2.Add("현충일");
                    break;
                case 8:
                    no2.Add(15);
                    name2.Add("광복절");
                    break;
                case 9:
                    break;
                case 10:
                    no2.Add(3);
                    name2.Add("개천절");
                    no2.Add(9);
                    name2.Add("한글날");
                    break;
                case 12:
                    no2.Add(25);
                    name2.Add("크리스마스");
                    break;
                default:
                    no = null;
                    name1 = null;
                    break;
            }
            DateTime dt = new DateTime(year - 1, 12, 30);
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);//설날
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("설날");
            }

            dt = new DateTime(year, 1, 1);
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);//설날
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("설날");
            }

            dt = new DateTime(year, 1, 2);//설날
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);//설날
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("설날");
            }

            dt = new DateTime(year, 4, 8);//석가탄신일
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);//석가탄신일
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("석가탄신일");
            }

            dt = new DateTime(year, 8, 14);//추석
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("추석");
            }

            dt = new DateTime(year, 8, 15);//추석
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("추석");
            }

            dt = new DateTime(year, 8, 16);//추석
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("추석");
            }

            no = no2.ToArray();
            name1 = name2.ToArray();
        }

        private DateTime convertKoreanMonth(int n음력년, int n음력월, int n음력일)//음력을 양력 변환
        {
            System.Globalization.KoreanLunisolarCalendar 음력 =
            new System.Globalization.KoreanLunisolarCalendar();

            bool b달 = 음력.IsLeapMonth(n음력년, n음력월);
            int n윤월;

            if (음력.GetMonthsInYear(n음력년) > 12)
            {
                n윤월 = 음력.GetLeapMonth(n음력년);
                if (b달)
                    n음력월++;
                if (n음력월 > n윤월)
                    n음력월++;
            }
            try
            {
                음력.ToDateTime(n음력년, n음력월, n음력일, 0, 0, 0, 0);
            }
            catch
            {
                return 음력.ToDateTime(n음력년, n음력월, 음력.GetDaysInMonth(n음력년, n음력월), 0, 0, 0, 0);//음력은 마지막 날짜가 매달 다르기 때문에 예외 뜨면 그날 맨 마지막 날로 지정
            }

            return 음력.ToDateTime(n음력년, n음력월, n음력일, 0, 0, 0, 0);
        }
        #endregion

        #region ComboBox 
        private void cbYear_SelectedValueChanged(object sender, EventArgs e)
        {
            SetMonth();
        }
        #endregion

        #region Button
        private void btnBefore_Click(object sender, EventArgs e)
        {
            int sMonth = Convert.ToInt16(cbMonth.Text);
            int sYear = Convert.ToInt16(cbYear.Text);

            if (sMonth == 1)
            {
                sYear -= 1;
                sMonth = 12;
            }
            else 
            {
                sMonth -= 1;
            }

            cbYear.Text = sYear.ToString();
            cbMonth.Text = sMonth.ToString();
        }

        private void btnAfter_Click(object sender, EventArgs e)
        {
            int sMonth = Convert.ToInt16(cbMonth.Text);
            int sYear = Convert.ToInt16(cbYear.Text);

            if (sMonth == 12)
            {
                sYear += 1;
                sMonth = 1;
            }
            else
            {
                sMonth += 1;
            }

            cbYear.Text = sYear.ToString();
            cbMonth.Text = sMonth.ToString();
        }
        #endregion

        private void Calendar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
