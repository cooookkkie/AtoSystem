using AdoNetWindow.Model;
using Repositories.OverseaManufacturing;
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

namespace AdoNetWindow.OverseaManufacturingBusiness
{
    public partial class UploadDataCalendar : Form
    {
        IOverseaManufacturingRepository overseaManufacturingRepository = new OverseaManufacturingRepository();
        UsersModel um;
        Libs.Tools.Common common = new Libs.Tools.Common();
        public UploadDataCalendar(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
        }

        private void UploadDataCalendar_Load(object sender, EventArgs e)
        {
            for(int i = DateTime.Now.Year - 5; i <= DateTime.Now.Year; i++)
                cbYear.Items.Add(i.ToString());

            cbYear.Text = DateTime.Now.Year.ToString();
            cbMonth.Text = DateTime.Now.Month.ToString();

            SetCalendar();
        }

        #region 캘린더 채우기
        private void SetCalendar()
        {
            //clear container
            pnMain.Controls.Clear();

            int sYear;
            if (!int.TryParse(cbYear.Text, out sYear))
            {
                MessageBox.Show(this,"기준년월을 설정해주세요.");
                this.Activate();
                return;
            }
            int sMonth;
            if (!int.TryParse(cbMonth.Text, out sMonth))
            {
                MessageBox.Show(this,"기준년월을 설정해주세요.");
                this.Activate();
                return;
            }
            string monthname = DateTimeFormatInfo.CurrentInfo.GetMonthName(sMonth);
            string stt_date = new DateTime(sYear, sMonth, 1).ToString("yyyy-MM-dd");
            string end_date = new DateTime(sYear, sMonth, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");

            //Upload Data
            DataTable uploadDt = overseaManufacturingRepository.GetUploadData(stt_date, end_date);

            //Title
            //LBDATE.Text = sYear + "년 " + monthname;

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

            DateTime sttdate = new DateTime(sYear, sMonth, 1, 0, 0, 0);
            DateTime sttdate2 = new DateTime(sYear, sMonth, 1, 0, 0, 0).AddDays(-7);
            DateTime enddate = new DateTime(sYear, sMonth, 1, 0, 0, 0).AddMonths(1);
            int dayofnextmonth = 0;
            //마지막주의 금요일까지
            while (!(enddate.DayOfWeek == DayOfWeek.Saturday || enddate.DayOfWeek == DayOfWeek.Sunday))
            {
                enddate = enddate.AddDays(1);
                dayofnextmonth++;      //다음달 오버되는 일짜
            }
            //==========================================================================
            //first lets create a blank usercontrol
            for (int i = 1; i < dayoftheweek; i++)
            {
                DateTime dt = new DateTime(sYear, sMonth, 1).AddDays(-dayoftheweek + i);
                //if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday)
                {
                    UploadDataCalendarUnit ucblack = new UploadDataCalendarUnit(dt.Year, dt.Month, 0);
                    pnMain.Controls.Add(ucblack);
                }
            }
            //==========================================================================
            //now lets create usercontrol for days
            int total_seaFood = 0;
            int total_processedFood = 0;
            int total_livestockProduct = 0;
            int total_agriculturalProduct = 0;
            for (int i = 1; i <= days; i++)
            {
                DateTime dt = new DateTime(sYear, sMonth, i);
                //if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday)
                {
                    int seaFood = 0;
                    int prcessedFood = 0;
                    int livestockProduct = 0;
                    int agriculturalProduct = 0;

                    if (uploadDt.Rows.Count > 0)
                    {
                        DataRow[] dr = uploadDt.Select($"days = {i}");
                        if (dr.Length > 0)
                        {
                            if (!int.TryParse(dr[0]["processed_food"].ToString(), out prcessedFood))
                                prcessedFood = 0;
                            if (!int.TryParse(dr[0]["sea_food"].ToString(), out seaFood))
                                seaFood = 0;
                            if (!int.TryParse(dr[0]["livestock_product"].ToString(), out livestockProduct))
                                livestockProduct = 0;
                            if (!int.TryParse(dr[0]["agricultural_product"].ToString(), out agriculturalProduct))
                                agriculturalProduct = 0;
                        }
                    }
                    //total
                    total_seaFood += seaFood;
                    total_processedFood += prcessedFood;
                    total_livestockProduct += livestockProduct;
                    total_agriculturalProduct += agriculturalProduct;

                    UploadDataCalendarUnit ucblack = new UploadDataCalendarUnit(sYear, sMonth, i, seaFood, prcessedFood, livestockProduct , agriculturalProduct);
                    pnMain.Controls.Add(ucblack);
                }
            }
            //==========================================================================
            //day of same week and over month
            sttdate = new DateTime(sYear, sMonth, 1).AddMonths(1);
            if (dayofnextmonth > 0)
            {
                //Holiday List
                common.getRedDay(sttdate.Year, sttdate.Month, out no, out name);
                for (int i = 1; i <= dayofnextmonth; i++)
                {
                    UploadDataCalendarUnit ucblack = new UploadDataCalendarUnit(sttdate.Year, sttdate.Month, 0);
                    pnMain.Controls.Add(ucblack);
                }
            }

            //통계
            txtSeafood.Text = total_seaFood.ToString("#,##0");
            txtProcessedFood.Text = total_processedFood.ToString("#,##0");
            txtLivestockProduct.Text = total_livestockProduct.ToString("#,##0");
            txtAgriculturalProduct.Text = total_agriculturalProduct.ToString("#,##0");
            txtTotalCount.Text = (total_seaFood + total_processedFood + total_livestockProduct + total_agriculturalProduct).ToString("#,##0");
        }

        #endregion

        #region Key event
        private void UploadDataCalendar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                { 
                    case Keys.Q:
                        btnSelect.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.M:
                        cbMonth.Focus();
                        break;
                    case Keys.N:
                        cbYear.Text = DateTime.Now.Year.ToString();
                        cbMonth.Text = DateTime.Now.Month.ToString();
                        cbMonth.Focus();
                        break;
                }
            }
        }
        #endregion

        #region Button
        private void btnPre_Click(object sender, EventArgs e)
        {
            DateTime sdate;
            if (!DateTime.TryParse(cbYear.Text + "-" + cbMonth.Text + "-01", out sdate))
                sdate = DateTime.Now;
            cbYear.Text = sdate.AddMonths(-1).Year.ToString();
            cbMonth.Text = sdate.AddMonths(-1).Month.ToString();
            btnSelect.PerformClick();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            DateTime sdate;
            if (!DateTime.TryParse(cbYear.Text + "-" + cbMonth.Text + "-01", out sdate))
                sdate = DateTime.Now;
            cbYear.Text = sdate.AddMonths(1).Year.ToString();
            cbMonth.Text = sdate.AddMonths(1).Month.ToString();
            btnSelect.PerformClick();
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            SetCalendar();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        
    }
}
