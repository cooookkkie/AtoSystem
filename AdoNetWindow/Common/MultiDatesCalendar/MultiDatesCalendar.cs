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

namespace AdoNetWindow.Common.MultiDatesCalendar
{
    public partial class MultiDatesCalendar : Form
    {
        Libs.Tools.Common common = new Libs.Tools.Common();
        string[] selectDates = null;
        bool isKeyClose = false;
        public MultiDatesCalendar()
        {
            InitializeComponent();
        }

        public string[] GetSelectDate()
        {
            this.ShowDialog();

            if (isKeyClose)
                return selectDates;
                    
            int cnt = 0;
            foreach (Control con in daycontainer1.Controls)
            {
                if (con.Name == "DayUnit")
                {
                    DayUnit du = (DayUnit)con;
                    if (du.isSelect)
                        cnt++;
                }
            }
            foreach (Control con in daycontainer2.Controls)
            {
                if (con.Name == "DayUnit")
                {
                    DayUnit du = (DayUnit)con;
                    if (du.isSelect)
                        cnt++;
                }
            }

            selectDates = null;
            if (cnt > 0)
            {
                selectDates = new string[cnt];
                cnt = 0;
                foreach (Control con in daycontainer1.Controls)
                {
                    if (con.Name == "DayUnit")
                    {
                        DayUnit du = (DayUnit)con;
                        if (du.isSelect)
                            selectDates[cnt++] = du.dt.ToString("yyyy-MM-dd");
                    }
                }
                foreach (Control con in daycontainer2.Controls)
                {
                    if (con.Name == "DayUnit")
                    {
                        DayUnit du = (DayUnit)con;
                        if (du.isSelect)
                            selectDates[cnt++] = du.dt.ToString("yyyy-MM-dd");
                    }
                }
            }
            return selectDates;
        }

        private void MultiDatesCalendar_Load(object sender, EventArgs e)
        {
            cbPreYear.Text = DateTime.Now.Year.ToString();
            cbPreMonth.Text = DateTime.Now.Month.ToString();

            cbNextYear.Text = DateTime.Now.AddMonths(1).Year.ToString();
            cbNextMonth.Text = DateTime.Now.AddMonths(1).Month.ToString();

            SetDays();

            this.cbPreMonth.SelectedIndexChanged += new System.EventHandler(this.cbPreMonth_SelectedIndexChanged);
            this.cbPreYear.SelectedIndexChanged += new System.EventHandler(this.cbPreYear_SelectedIndexChanged);
            this.cbNextMonth.SelectedIndexChanged += new System.EventHandler(this.cbNextMonth_SelectedIndexChanged);
            this.cbNextYear.SelectedIndexChanged += new System.EventHandler(this.cbNextYear_SelectedIndexChanged);
        }

        #region Method
        private void SetDays(string select_date = "")
        {
            //clear container1===========================================================
            daycontainer1.Controls.Clear();
            int sMonth = Convert.ToInt16(cbPreMonth.Text);
            int sYear = Convert.ToInt16(cbPreYear.Text);
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
                daycontainer1.Controls.Add(ucblack);
            }
            //==========================================================================
            //now lets create usercontrol for days
            for (int i = 1; i <= days; i++)
            {
                DateTime sDate = new DateTime(sYear, sMonth, i);
                DayUnit ucdays = new DayUnit(this, sDate, no, name);
                if (!string.IsNullOrEmpty(select_date) && DateTime.TryParse(select_date, out DateTime selectDt) && selectDt.ToString("yyyy-MM-dd") == sDate.ToString("yyyy-MM-dd"))
                    ucdays.SelectChanged();
                daycontainer1.Controls.Add(ucdays);
            }

            //clear container2===========================================================
            daycontainer2.Controls.Clear();
            sMonth = Convert.ToInt16(cbNextMonth.Text);
            sYear = Convert.ToInt16(cbNextYear.Text);
            monthname = DateTimeFormatInfo.CurrentInfo.GetMonthName(sMonth);

            //Lets get the first day of the month
            startofthemonth = new DateTime(sYear, sMonth, 1);

            //get the count of days of the month
            days = DateTime.DaysInMonth(sYear, sMonth);

            //convert the startofthemonth to integer
            dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 1;

            //Holiday List
            common.getRedDay(sYear, sMonth, out no, out name);

            //==========================================================================
            //first lets create a blank usercontrol
            for (int i = 1; i < dayoftheweek; i++)
            {
                BlankUnit ucblack = new BlankUnit();
                daycontainer2.Controls.Add(ucblack);
            }
            //==========================================================================
            //now lets create usercontrol for days
            for (int i = 1; i <= days; i++)
            {
                DateTime sDate = new DateTime(sYear, sMonth, i);
                DayUnit ucdays = new DayUnit(this, sDate, no, name);
                if (!string.IsNullOrEmpty(select_date) && DateTime.TryParse(select_date, out DateTime selectDt) && selectDt.ToString("yyyy-MM-dd") == sDate.ToString("yyyy-MM-dd"))
                    ucdays.SelectChanged();
                daycontainer2.Controls.Add(ucdays);
            }
        }
        #endregion

        #region Button event
        private void btnBefore_Click(object sender, EventArgs e)
        {
            this.cbPreMonth.SelectedIndexChanged -= new System.EventHandler(this.cbPreMonth_SelectedIndexChanged);
            this.cbPreYear.SelectedIndexChanged -= new System.EventHandler(this.cbPreYear_SelectedIndexChanged);
            this.cbNextMonth.SelectedIndexChanged -= new System.EventHandler(this.cbNextMonth_SelectedIndexChanged);
            this.cbNextYear.SelectedIndexChanged -= new System.EventHandler(this.cbNextYear_SelectedIndexChanged);

            DateTime sdate = new DateTime(Convert.ToInt16(cbPreYear.Text), Convert.ToInt16(cbPreMonth.Text), 1);
            cbNextMonth.Text = sdate.Month.ToString();
            cbNextYear.Text = sdate.Month.ToString();

            sdate = sdate.AddMonths(-1);
            cbPreMonth.Text = sdate.Month.ToString();
            cbPreYear.Text = sdate.Month.ToString();

            SetDays();
            this.cbPreMonth.SelectedIndexChanged += new System.EventHandler(this.cbPreMonth_SelectedIndexChanged);
            this.cbPreYear.SelectedIndexChanged += new System.EventHandler(this.cbPreYear_SelectedIndexChanged);
            this.cbNextMonth.SelectedIndexChanged += new System.EventHandler(this.cbNextMonth_SelectedIndexChanged);
            this.cbNextYear.SelectedIndexChanged += new System.EventHandler(this.cbNextYear_SelectedIndexChanged);
        }

        private void btnAfter_Click(object sender, EventArgs e)
        {
            this.cbPreMonth.SelectedIndexChanged -= new System.EventHandler(this.cbPreMonth_SelectedIndexChanged);
            this.cbPreYear.SelectedIndexChanged -= new System.EventHandler(this.cbPreYear_SelectedIndexChanged);
            this.cbNextMonth.SelectedIndexChanged -= new System.EventHandler(this.cbNextMonth_SelectedIndexChanged);
            this.cbNextYear.SelectedIndexChanged -= new System.EventHandler(this.cbNextYear_SelectedIndexChanged);

            DateTime sdate = new DateTime(Convert.ToInt16(cbNextYear.Text), Convert.ToInt16(cbNextMonth.Text), 1);
            cbPreMonth.Text = sdate.Month.ToString();
            cbPreYear.Text = sdate.Year.ToString();

            sdate = sdate.AddMonths(1);
            cbNextMonth.Text = sdate.Month.ToString();
            cbNextYear.Text = sdate.Year.ToString();

            SetDays();

            this.cbPreMonth.SelectedIndexChanged += new System.EventHandler(this.cbPreMonth_SelectedIndexChanged);
            this.cbPreYear.SelectedIndexChanged += new System.EventHandler(this.cbPreYear_SelectedIndexChanged);
            this.cbNextMonth.SelectedIndexChanged += new System.EventHandler(this.cbNextMonth_SelectedIndexChanged);
            this.cbNextYear.SelectedIndexChanged += new System.EventHandler(this.cbNextYear_SelectedIndexChanged);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            SetDays();
        }

        private void btnGotoToday_Click(object sender, EventArgs e)
        {
            this.cbPreMonth.SelectedIndexChanged -= new System.EventHandler(this.cbPreMonth_SelectedIndexChanged);
            this.cbPreYear.SelectedIndexChanged -= new System.EventHandler(this.cbPreYear_SelectedIndexChanged);
            this.cbNextMonth.SelectedIndexChanged -= new System.EventHandler(this.cbNextMonth_SelectedIndexChanged);
            this.cbNextYear.SelectedIndexChanged -= new System.EventHandler(this.cbNextYear_SelectedIndexChanged);

            DateTime sdate = DateTime.Now;
            cbPreMonth.Text = sdate.Month.ToString();
            cbPreYear.Text = sdate.Year.ToString();

            sdate = sdate.AddMonths(1);
            cbNextMonth.Text = sdate.Month.ToString();
            cbNextYear.Text = sdate.Year.ToString();

            SetDays(DateTime.Now.ToString("yyyy-MM-dd"));
            this.cbPreMonth.SelectedIndexChanged += new System.EventHandler(this.cbPreMonth_SelectedIndexChanged);
            this.cbPreYear.SelectedIndexChanged += new System.EventHandler(this.cbPreYear_SelectedIndexChanged);
            this.cbNextMonth.SelectedIndexChanged += new System.EventHandler(this.cbNextMonth_SelectedIndexChanged);
            this.cbNextYear.SelectedIndexChanged += new System.EventHandler(this.cbNextYear_SelectedIndexChanged);
        }
        private void btnTomorrow_Click(object sender, EventArgs e)
        {
            this.cbPreMonth.SelectedIndexChanged -= new System.EventHandler(this.cbPreMonth_SelectedIndexChanged);
            this.cbPreYear.SelectedIndexChanged -= new System.EventHandler(this.cbPreYear_SelectedIndexChanged);
            this.cbNextMonth.SelectedIndexChanged -= new System.EventHandler(this.cbNextMonth_SelectedIndexChanged);
            this.cbNextYear.SelectedIndexChanged -= new System.EventHandler(this.cbNextYear_SelectedIndexChanged);

            DateTime sdate = DateTime.Now;
            cbPreMonth.Text = sdate.Month.ToString();
            cbPreYear.Text = sdate.Year.ToString();

            sdate = sdate.AddMonths(1);
            cbNextMonth.Text = sdate.Month.ToString();
            cbNextYear.Text = sdate.Year.ToString();

            SetDays(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
            this.cbPreMonth.SelectedIndexChanged += new System.EventHandler(this.cbPreMonth_SelectedIndexChanged);
            this.cbPreYear.SelectedIndexChanged += new System.EventHandler(this.cbPreYear_SelectedIndexChanged);
            this.cbNextMonth.SelectedIndexChanged += new System.EventHandler(this.cbNextMonth_SelectedIndexChanged);
            this.cbNextYear.SelectedIndexChanged += new System.EventHandler(this.cbNextYear_SelectedIndexChanged);
        }
        #endregion

        #region ComboBox event
        private void cbPreYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cbPreMonth.SelectedIndexChanged -= new System.EventHandler(this.cbPreMonth_SelectedIndexChanged);
            this.cbPreYear.SelectedIndexChanged -= new System.EventHandler(this.cbPreYear_SelectedIndexChanged);
            this.cbNextMonth.SelectedIndexChanged -= new System.EventHandler(this.cbNextMonth_SelectedIndexChanged);
            this.cbNextYear.SelectedIndexChanged -= new System.EventHandler(this.cbNextYear_SelectedIndexChanged);

            int month = Convert.ToInt16(cbPreMonth.Text);
            int year = Convert.ToInt16(cbPreYear.Text);
            DateTime sdate = new DateTime(year, month, 1);

            sdate = sdate.AddMonths(1);
            cbNextMonth.Text = sdate.Month.ToString();
            cbNextYear.Text = sdate.Month.ToString();

            SetDays();

            this.cbPreMonth.SelectedIndexChanged += new System.EventHandler(this.cbPreMonth_SelectedIndexChanged);
            this.cbPreYear.SelectedIndexChanged += new System.EventHandler(this.cbPreYear_SelectedIndexChanged);
            this.cbNextMonth.SelectedIndexChanged += new System.EventHandler(this.cbNextMonth_SelectedIndexChanged);
            this.cbNextYear.SelectedIndexChanged += new System.EventHandler(this.cbNextYear_SelectedIndexChanged);
        }

        private void cbPreMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cbPreMonth.SelectedIndexChanged -= new System.EventHandler(this.cbPreMonth_SelectedIndexChanged);
            this.cbPreYear.SelectedIndexChanged -= new System.EventHandler(this.cbPreYear_SelectedIndexChanged);
            this.cbNextMonth.SelectedIndexChanged -= new System.EventHandler(this.cbNextMonth_SelectedIndexChanged);
            this.cbNextYear.SelectedIndexChanged -= new System.EventHandler(this.cbNextYear_SelectedIndexChanged);

            int month = Convert.ToInt16(cbPreMonth.Text);
            int year = Convert.ToInt16(cbPreYear.Text);
            DateTime sdate = new DateTime(year, month, 1);

            sdate = sdate.AddMonths(1);
            cbNextMonth.Text = sdate.Month.ToString();
            cbNextYear.Text = sdate.Month.ToString();

            SetDays();

            this.cbPreMonth.SelectedIndexChanged += new System.EventHandler(this.cbPreMonth_SelectedIndexChanged);
            this.cbPreYear.SelectedIndexChanged += new System.EventHandler(this.cbPreYear_SelectedIndexChanged);
            this.cbNextMonth.SelectedIndexChanged += new System.EventHandler(this.cbNextMonth_SelectedIndexChanged);
            this.cbNextYear.SelectedIndexChanged += new System.EventHandler(this.cbNextYear_SelectedIndexChanged);
        }

        private void cbNextYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cbPreMonth.SelectedIndexChanged -= new System.EventHandler(this.cbPreMonth_SelectedIndexChanged);
            this.cbPreYear.SelectedIndexChanged -= new System.EventHandler(this.cbPreYear_SelectedIndexChanged);
            this.cbNextMonth.SelectedIndexChanged -= new System.EventHandler(this.cbNextMonth_SelectedIndexChanged);
            this.cbNextYear.SelectedIndexChanged -= new System.EventHandler(this.cbNextYear_SelectedIndexChanged);

            int month = Convert.ToInt16(cbNextMonth.Text);
            int year = Convert.ToInt16(cbNextYear.Text);
            DateTime sdate = new DateTime(year, month, 1);

            sdate = sdate.AddMonths(-1);
            cbPreMonth.Text = sdate.Month.ToString();
            cbPreYear.Text = sdate.Month.ToString();

            SetDays();

            this.cbPreMonth.SelectedIndexChanged += new System.EventHandler(this.cbPreMonth_SelectedIndexChanged);
            this.cbPreYear.SelectedIndexChanged += new System.EventHandler(this.cbPreYear_SelectedIndexChanged);
            this.cbNextMonth.SelectedIndexChanged += new System.EventHandler(this.cbNextMonth_SelectedIndexChanged);
            this.cbNextYear.SelectedIndexChanged += new System.EventHandler(this.cbNextYear_SelectedIndexChanged);
        }

        private void cbNextMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cbPreMonth.SelectedIndexChanged -= new System.EventHandler(this.cbPreMonth_SelectedIndexChanged);
            this.cbPreYear.SelectedIndexChanged -= new System.EventHandler(this.cbPreYear_SelectedIndexChanged);
            this.cbNextMonth.SelectedIndexChanged -= new System.EventHandler(this.cbNextMonth_SelectedIndexChanged);
            this.cbNextYear.SelectedIndexChanged -= new System.EventHandler(this.cbNextYear_SelectedIndexChanged);

            int month = Convert.ToInt16(cbNextMonth.Text);
            int year = Convert.ToInt16(cbNextYear.Text);
            DateTime sdate = new DateTime(year, month, 1);

            sdate = sdate.AddMonths(-1);
            cbPreMonth.Text = sdate.Month.ToString();
            cbPreYear.Text = sdate.Month.ToString();

            SetDays();

            this.cbPreMonth.SelectedIndexChanged += new System.EventHandler(this.cbPreMonth_SelectedIndexChanged);
            this.cbPreYear.SelectedIndexChanged += new System.EventHandler(this.cbPreYear_SelectedIndexChanged);
            this.cbNextMonth.SelectedIndexChanged += new System.EventHandler(this.cbNextMonth_SelectedIndexChanged);
            this.cbNextYear.SelectedIndexChanged += new System.EventHandler(this.cbNextYear_SelectedIndexChanged);
        }
        #endregion

        #region Key event
        private void MultiDatesCalendar_KeyDown(object sender, KeyEventArgs e)
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
                        {
                            int cnt = 0;
                            foreach (Control con in daycontainer1.Controls)
                            {
                                if (con.Name == "DayUnit")
                                {
                                    DayUnit du = (DayUnit)con;
                                    if (du.isSelect)
                                        cnt++;
                                }
                            }
                            foreach (Control con in daycontainer2.Controls)
                            {
                                if (con.Name == "DayUnit")
                                {
                                    DayUnit du = (DayUnit)con;
                                    if (du.isSelect)
                                        cnt++;
                                }
                            }

                            selectDates = null;
                            if (cnt > 0)
                            {
                                selectDates = new string[cnt];
                                cnt = 0;
                                foreach (Control con in daycontainer1.Controls)
                                {
                                    if (con.Name == "DayUnit")
                                    {
                                        DayUnit du = (DayUnit)con;
                                        if (du.isSelect)
                                            selectDates[cnt++] = du.dt.ToString("yyyy-MM-dd");
                                    }
                                }
                                foreach (Control con in daycontainer2.Controls)
                                {
                                    if (con.Name == "DayUnit")
                                    {
                                        DayUnit du = (DayUnit)con;
                                        if (du.isSelect)
                                            selectDates[cnt++] = du.dt.ToString("yyyy-MM-dd");
                                    }
                                }
                            }
                            isKeyClose = true;
                            this.Dispose();
                        }
                        break;
                    case Keys.Enter:
                        {
                            int cnt = 0;
                            foreach (Control con in daycontainer1.Controls)
                            {
                                if (con.Name == "DayUnit")
                                {
                                    DayUnit du = (DayUnit)con;
                                    if (du.isSelect)
                                        cnt++;
                                }
                            }
                            foreach (Control con in daycontainer2.Controls)
                            {
                                if (con.Name == "DayUnit")
                                {
                                    DayUnit du = (DayUnit)con;
                                    if (du.isSelect)
                                        cnt++;
                                }
                            }

                            selectDates = null;
                            if (cnt > 0)
                            {
                                selectDates = new string[cnt];
                                cnt = 0;
                                foreach (Control con in daycontainer1.Controls)
                                {
                                    if (con.Name == "DayUnit")
                                    {
                                        DayUnit du = (DayUnit)con;
                                        if (du.isSelect)
                                            selectDates[cnt++] = du.dt.ToString("yyyy-MM-dd");
                                    }
                                }
                                foreach (Control con in daycontainer2.Controls)
                                {
                                    if (con.Name == "DayUnit")
                                    {
                                        DayUnit du = (DayUnit)con;
                                        if (du.isSelect)
                                            selectDates[cnt++] = du.dt.ToString("yyyy-MM-dd");
                                    }
                                }
                            }
                            isKeyClose = true;
                            this.Dispose();
                        }
                        break;
                }
            }
        }

        #endregion

        private void MultiDatesCalendar_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            int cnt = 0;
            foreach (Control con in daycontainer1.Controls)
            {
                if (con.Name == "DayUnit")
                {
                    DayUnit du = (DayUnit)con;
                    if (du.isSelect)
                        cnt++;
                }
            }
            foreach (Control con in daycontainer2.Controls)
            {
                if (con.Name == "DayUnit")
                {
                    DayUnit du = (DayUnit)con;
                    if (du.isSelect)
                        cnt++;
                }
            }

            selectDates = null;
            if (cnt > 0)
            {
                selectDates = new string[cnt];
                cnt = 0;
                foreach (Control con in daycontainer1.Controls)
                {
                    if (con.Name == "DayUnit")
                    {
                        DayUnit du = (DayUnit)con;
                        if (du.isSelect)
                            selectDates[cnt++] = du.dt.ToString("yyyy-MM-dd");
                    }
                }
                foreach (Control con in daycontainer2.Controls)
                {
                    if (con.Name == "DayUnit")
                    {
                        DayUnit du = (DayUnit)con;
                        if (du.isSelect)
                            selectDates[cnt++] = du.dt.ToString("yyyy-MM-dd");
                    }
                }
            }
            isKeyClose = true;
            this.Dispose();
        }
    }
}
