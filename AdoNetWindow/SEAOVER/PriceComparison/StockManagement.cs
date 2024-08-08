using AdoNetWindow.Model;
using Repositories;
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

namespace AdoNetWindow.SEAOVER.PriceComparison
{
    public partial class StockManagement : Form
    {
        ICustomsRepository customsRepository = new CustomsRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        DataTable unpendingTable = new DataTable();
        ShortManagerModel model;
        List<shipmentModel> list;
        int sYear;
        int sMonth;
        DateTime shortDate;
        DataRow dtRow;
        DataTable pendingDt;
        public StockManagement(ShortManagerModel sModel, List<shipmentModel> sList, DataTable dt)
        {
            InitializeComponent();
            model = sModel;
            list = sList;
            pendingDt = dt;
            //dtRow = datarow;
        }

        private void Stock_Management_Load(object sender, EventArgs e)
        {
            //기준년월
            txtSttdate.Text = Convert.ToDateTime(model.enddate).ToString("yyyy-MM-dd");
            txtEnddate.Text = Convert.ToDateTime(model.enddate).AddYears(1).ToString("yyyy-MM-dd");
            sYear = Convert.ToDateTime(model.enddate).Year;
            sMonth= Convert.ToDateTime(model.enddate).Month;
            //연도
            for (int i = 0; i <= 50; i++)
            {
                cbYear.Items.Add(sYear + i);
            }
            cbYear.Text = sYear.ToString();
            //월
            cbMonth.Text = sMonth.ToString();
            //소진일자
            shortDate = Convert.ToDateTime(model.enddate);
            if (model.avg_sales_day > 0)
            {
                common.GetShortDay2(shortDate, model.real_stock, model.avg_sales_day, out shortDate);
            }
            //입고내역
            //unpendingTable = customsRepository.GetUnpending(model.product, model.origin, model.sizes, model.unit, shipDays);
            //캘린더 만들기
            SetCalendar(shortDate);
            //리스트 만들기
            dgvStock.Columns["sdate"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            dgvStock.Columns["sdate"].HeaderCell.Style.BackColor = Color.FromArgb(204, 255, 255);
            dgvStock.Columns["shipment"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            dgvStock.Columns["shipment"].HeaderCell.Style.BackColor = Color.FromArgb(176, 208, 207);
            dgvStock.Columns["shipment"].HeaderCell.Style.ForeColor = Color.Blue;
            dgvStock.Columns["sales"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            dgvStock.Columns["sales"].HeaderCell.Style.BackColor = Color.FromArgb(176, 208, 207);
            dgvStock.Columns["sales"].HeaderCell.Style.ForeColor = Color.Red;
            dgvStock.Columns["stock"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            dgvStock.Columns["stock"].HeaderCell.Style.BackColor = Color.FromArgb(204, 255, 255);
            dgvStock.Columns[1].ReadOnly = false;
            dgvStock.Columns[2].ReadOnly = false;
            dgvStock.Columns[3].ReadOnly = false;
            SetStockList();
            //Event Handeler
            this.cbYear.SelectedIndexChanged += new System.EventHandler(this.cbYear_SelectedIndexChanged);
            this.cbMonth.SelectedIndexChanged += new System.EventHandler(this.cbMonth_SelectedIndexChanged);
            
        }

        #region Method
        //재고 리스트만들기
        private void SetStockList()
        {
            this.dgvStock.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStock_CellValueChanged);
            dgvStock.Rows.Clear();
            string monthname = DateTimeFormatInfo.CurrentInfo.GetMonthName(sMonth);
            //Lets get the first day of the month
            DateTime sdate = Convert.ToDateTime(model.enddate);

            //get the count of days of the month
            int days = DateTime.DaysInMonth(sYear, sMonth);

            //Holiday List
            int[] no;
            string[] name;
            common.getRedDay(sYear, sMonth, out no, out name);
            //==========================================================================
            //재고계산
            double stock = model.real_stock;
            double sales_count = model.avg_sales_day;
            stock -= sales_count;
            
            stock += sales_count;
            bool isHoliday;
            List<DateTime> holiday = new List<DateTime>();
            Dictionary<int, List<DateTime>> yearDic = new Dictionary<int, List<DateTime>>();
            //==========================================================================
            while (sdate < Convert.ToDateTime(txtEnddate.Text))
            {
                //주말 or 휴일
                isHoliday = common.HolidayCheck(sdate);
                //입고량==================================================================
                double qty = 0;
                if (pendingDt.Rows.Count > 0)
                {
                    DateTime dt;
                    for (int i = 0; i < pendingDt.Rows.Count; i++)
                    {
                        if (DateTime.TryParse(pendingDt.Rows[i]["warehousing_date"].ToString(), out dt))
                        {
                            if (dt == sdate)
                            {
                                qty = Convert.ToDouble(pendingDt.Rows[i]["qty"].ToString());
                                if(stock < 0)
                                    stock = qty;
                                else
                                    stock += qty;
                            }
                        }
                    }   
                }
                //판매량==================================================================
                stock -= sales_count;

                int n = dgvStock.Rows.Add();
                dgvStock.Rows[n].Cells["sdate"].Value = sdate.ToString("yyyy-MM-dd");
                dgvStock.Rows[n].Cells["shipment"].Value = qty.ToString("#,##0");
                if (isHoliday)
                {
                    stock += sales_count;
                    dgvStock.Rows[n].Cells["sdate"].Style.ForeColor = Color.Red;
                    dgvStock.Rows[n].Cells["sales"].Value = 0;
                    dgvStock.Rows[n].Cells["stock"].Value = stock.ToString("#,##0.00");
                }
                else
                {
                    dgvStock.Rows[n].Cells["sales"].Value = sales_count.ToString("#,##0.00");
                    dgvStock.Rows[n].Cells["stock"].Value = stock.ToString("#,##0.00");
                }
                //다음날
                sdate = sdate.AddDays(1);
            }
            txtTotalShipment.Text = GetCount("shipment").ToString("#,##0.00");
            txtTotalSales.Text = GetCount("sales").ToString("#,##0.00");
            txtTotalStock.Text = GetCount("stock").ToString("#,##0.00");

            sdate = sdate.AddDays(-1);
            if (sdate < Convert.ToDateTime(model.enddate))
            {
                sdate = Convert.ToDateTime(model.enddate);
            }
            this.dgvStock.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStock_CellValueChanged);
        }

        //캘린더 만들기
        private void SetCalendar(DateTime shortDate)
        {
            flpCalendar.Controls.Clear();
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
                calendarEmptyUnit ucblack = new calendarEmptyUnit();
                flpCalendar.Controls.Add(ucblack);
            }
            
            //==========================================================================
            //전달 재고계산
            double stock = model.real_stock;
            double sales_count = (model.avg_sales_day);
            DateTime preDate = Convert.ToDateTime(model.enddate);

            DateTime curMonth = new DateTime(preDate.Year, preDate.Month, 1);
            DateTime SelectDate = new DateTime(sYear, sMonth, 1);

            //시작달이면 필요없는 일
            if (curMonth == SelectDate)
            { 
                //필요없는 일
                for (int i = 1; i < preDate.Day; i++)
                {
                    calendarEmptyUnit ucblack = new calendarEmptyUnit();
                    flpCalendar.Controls.Add(ucblack);
                }
                //필요한 일
                for (int i = preDate.Day; i < SelectDate.AddMonths(1).AddDays(-1).Day; i++)
                {
                    DateTime sdate = new DateTime(preDate.Year, preDate.Month, i);
                    //휴일
                    bool isHoliday = common.HolidayCheck(sdate);
                    //영업일이면 재고소진
                    if (!isHoliday)
                        stock -= sales_count;
                    //입고량==================================================================
                    double qty = 0;
                    if (pendingDt.Rows.Count > 0)
                    {
                        DateTime dt;
                        for (int j = 0; j < pendingDt.Rows.Count; j++)
                        {
                            if (DateTime.TryParse(pendingDt.Rows[j]["warehousing_date"].ToString(), out dt))
                            {
                                if (dt == sdate)
                                {
                                    qty = Convert.ToDouble(pendingDt.Rows[j]["qty"].ToString());
                                    if (stock <= 0)
                                        stock = qty;
                                    else
                                        stock += qty;
                                }
                            }
                        }
                    }
                    calendarUnit ucdays = new calendarUnit(isHoliday, sdate.DayOfWeek, i, qty, stock, sales_count, false);
                    flpCalendar.Controls.Add(ucdays);
                }
            }
            else if (curMonth > SelectDate)
            {
                for (int i = 1; i < SelectDate.Day; i++)
                {
                    calendarEmptyUnit ucblack = new calendarEmptyUnit();
                    flpCalendar.Controls.Add(ucblack);
                }
            }
            else
            {
                //전달까지 재고계산
                common.GetPreMonthStock(preDate, SelectDate.AddDays(-1), stock, sales_count, pendingDt, out stock);
                //필요한 일
                for (int i = 1; i < SelectDate.AddMonths(1).AddDays(-1).Day; i++)
                {
                    DateTime sdate = new DateTime(SelectDate.Year, SelectDate.Month, i);
                    //입고량==================================================================
                    double qty = 0;
                    if (pendingDt.Rows.Count > 0)
                    {
                        DateTime dt;
                        for (int j = 0; j < pendingDt.Rows.Count; j++)
                        {
                            if (DateTime.TryParse(pendingDt.Rows[j]["warehousing_date"].ToString(), out dt))
                            {
                                if (dt == sdate)
                                {
                                    qty = Convert.ToDouble(pendingDt.Rows[j]["qty"].ToString());
                                    if (stock <= 0)
                                        stock = qty;
                                    else
                                        stock += qty;
                                }
                            }
                        }
                    }
                    //휴일
                    bool isHoliday = common.HolidayCheck(sdate);
                    //영업일이면 재고소진
                    if (!isHoliday)
                        stock -= sales_count;
                    calendarUnit ucdays = new calendarUnit(isHoliday, sdate.DayOfWeek, i, qty, stock, sales_count, false);
                    flpCalendar.Controls.Add(ucdays);
                }


            }
        }
        private DateTime convertKoreanMonth(int n음력년, int n음력월, int n음력일)//음력을 양력 변환
        {
            System.Globalization.KoreanLunisolarCalendar 음력 =
            new System.Globalization.KoreanLunisolarCalendar();

            if (n음력년 > 2050)
            {
                return new DateTime();
            }

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
                if (n음력월 > 12)
                {
                    n음력년 += 1;
                    n음력월 = n음력월 - 12;
                }

                if (n음력년 > 2050)
                {
                    return new DateTime();
                }

                return 음력.ToDateTime(n음력년, n음력월, 음력.GetDaysInMonth(n음력년, n음력월), 0, 0, 0, 0);//음력은 마지막 날짜가 매달 다르기 때문에 예외 뜨면 그날 맨 마지막 날로 지정
            }

            return 음력.ToDateTime(n음력년, n음력월, n음력일, 0, 0, 0, 0);
        }
        //리스트 총합가져오기
        private double GetCount(string colName)
        {
            double cnt = 0;
            DataGridView dgv = dgvStock;

            if (colName == "stock")
            {
                if (dgv.Rows.Count > 0)
                {
                    if (dgv.Rows[0].Cells[colName].Value != null)
                    {
                        cnt = Convert.ToDouble(dgv.Rows[0].Cells[colName].Value);
                    }
                    else
                    {
                        cnt = 0;
                    }
                }
                else
                {
                    cnt = 0;
                }
            }
            else 
            { 
            
                if (dgv.Rows.Count > 0)
                {
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    { 
                        if(dgv.Rows[i].Cells[colName].Value != null)
                        {
                            cnt += Convert.ToDouble(dgv.Rows[i].Cells[colName].Value);
                        }
                    }
                }
            }
            return cnt;
        }

        #endregion

        #region Button
        private void btnPrevious_Click(object sender, EventArgs e)
        {

            if (Convert.ToInt16(cbMonth.Text) - 1 == 0)
            {
                cbYear.Text = (Convert.ToInt16(cbYear.Text) - 1).ToString();
                cbMonth.Text = "12";
            }
            else
            {
                cbMonth.Text = (Convert.ToInt16(cbMonth.Text) - 1).ToString();
            }
        }

        private void btnAfter_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt16(cbMonth.Text) + 1 == 13)
            {
                cbYear.Text = (Convert.ToInt16(cbYear.Text) + 1).ToString();
                cbMonth.Text = "1";
            }
            else
            {
                cbMonth.Text = (Convert.ToInt16(cbMonth.Text) + 1).ToString();
            }
        }
        private void btnLSttdateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtSttdate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnLEnddateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtEnddate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }
        #endregion

        #region Changed Event
        private void cbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            sYear = Convert.ToInt16(cbYear.Text);
            sMonth = Convert.ToInt16(cbMonth.Text);
            SetCalendar(shortDate);
        }

        private void cbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            sYear = Convert.ToInt16(cbYear.Text);
            sMonth = Convert.ToInt16(cbMonth.Text);
            SetCalendar(shortDate);
        }
        private void dgvStock_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            txtTotalShipment.Text = GetCount("shipment").ToString("#,##0.00");
            txtTotalSales.Text = GetCount("sales").ToString("#,##0.00");
            txtTotalStock.Text = GetCount("stock").ToString("#,##0.00");
        }
        #endregion

        #region Key event
        private void StockManagement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        {
                            this.Dispose();
                            break;
                        }
                }
            }
        }
        private void txtSttdate_KeyDown(object sender, KeyEventArgs e)
        {
            Control tb = (Control)sender;
            //ato no 체크

            if (e.KeyCode == Keys.Enter)
            {
                //날짜 완성
                Control tbb = (Control)sender;
                tbb.Text = common.strDatetime(tbb.Text);

            }
        }
        #endregion

        #region Datagridview event
        private void dgvStock_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvStock.Columns[e.ColumnIndex].Name == "stock")
                {
                    double tmp;
                    if (double.TryParse(dgvStock.Rows[e.RowIndex].Cells["stock"].Value.ToString(), out tmp))
                    {
                        if (tmp < 0)
                        {
                            dgvStock.Rows[e.RowIndex].Cells["stock"].Style.ForeColor = Color.Red;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
