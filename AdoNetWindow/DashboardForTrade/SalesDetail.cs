using AdoNetWindow.Model;
using Repositories.SEAOVER.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.Dashboard
{
    public partial class SalesDetail : Form
    {
        ISalesRepository salesRepository = new SalesRepository();
        UsersModel um;
        string[] strHeaders = null;
        string product, origin, sizes, unit;
        Libs.Tools.Common common = new Libs.Tools.Common();
        public SalesDetail(UsersModel uModel, string txt_product, string txt_origin, string txt_sizes, string txt_unit)
        {
            InitializeComponent();
            um = uModel;
            product = txt_product;
            origin = txt_origin;
            sizes = txt_sizes;
            unit = txt_unit;

            for (int i = 2016; i <= DateTime.Now.Year; i++)
            { 
                cbSttYear.Items.Add(i.ToString());
                cbEndYear.Items.Add(i.ToString());
            }
            GetData();
        }

        #region Method
        public void GetData()
        {
            //초기화
            dgvDetails.Rows.Clear();
            dgvDetails.Columns.Clear();
            //기준년월
            DataGridViewColumn col = new DataGridViewTextBoxColumn();
            col.Name = "base_date";
            col.HeaderText = "기준";
            col.Width = 40;
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetails.Columns.Add(col);
            int stt_year, end_year;
            if (!int.TryParse(cbSttYear.Text, out stt_year) || !int.TryParse(cbEndYear.Text, out end_year))
            {
                MessageBox.Show(this,"매출기간을 확인해주세요.");
                this.Activate();
                return;
            }
            DateTime sttdate = new DateTime(stt_year, 1, 1);
            DateTime enddate = new DateTime(end_year, 12, 1);

            int terms_type;
            switch (cbTermsType.Text)
            {
                case "초~말":
                    terms_type = 1;
                    break;
                case "5일":
                    terms_type = 5;
                    break;
                case "10일":
                    terms_type = 10;
                    break;
                case "15일":
                    terms_type = 15;
                    break;
                case "20일":
                    terms_type = 20;
                    break;
                case "25일":
                    terms_type = 25;
                    break;
                default:
                    terms_type = 1;
                    break;
            }
            //Get Data
            //DataTable salesDt = salesRepository.GetSalesGroupMonth(sttdate, enddate, product, origin, sizes, unit, cbAllUnit.Checked, terms_type);
            DataTable salesDt = new DataTable();
            if (salesDt.Rows.Count > 0)
            {
                for (int i = stt_year; i <= end_year; i++)
                {
                    //매출수
                    DataGridViewColumn col1 = new DataGridViewTextBoxColumn();
                    col1.Name = "sales_qty_" + i;
                    col1.HeaderText = i + "년";
                    col1.ToolTipText = i.ToString();
                    col1.Width = 60;
                    col1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvDetails.Columns.Add(col1);

                    DataGridViewColumn col2 = new DataGridViewTextBoxColumn();
                    col2.Name = "prepare_pre_month_sales_qty_" + i;
                    col2.HeaderText = "전월";
                    col2.ToolTipText = i.ToString();
                    col2.Width = 50;
                    col2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col2.DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);
                    dgvDetails.Columns.Add(col2);
                    col2.Visible = false;
                }
                //마지막 년도는 항상 띄워놓기
                DataGridViewColumn col3 = new DataGridViewTextBoxColumn();
                col3.Name = "prepare_pre_month_sales_qty";
                col3.HeaderText = "전월";
                col3.Width = 60;
                col3.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDetails.Columns.Add(col3);

                DataGridViewColumn col4 = new DataGridViewTextBoxColumn();
                col4.Name = "prepare_pre_year_sales_qty";
                col4.HeaderText = "전년";
                col4.Width = 60;
                col4.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDetails.Columns.Add(col4);

                //월출력
                int n;
                for (int i = 1; i <= 12; i++)
                {
                    n = dgvDetails.Rows.Add();
                    dgvDetails.Rows[n].Cells["base_date"].Value = i.ToString() + "월";
                    n = dgvDetails.Rows.Add();
                    dgvDetails.Rows[n].Cells["base_date"].Value = "전년";
                    dgvDetails.Rows[n].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
                    dgvDetails.Rows[n].Visible = false;
                }
                n = dgvDetails.Rows.Add();
                dgvDetails.Rows[n].Cells["base_date"].Value = "합계";

                //데이터출력
                Dictionary<DateTime, double> salesDic = new Dictionary<DateTime, double>();
                for (int i = 0; i < salesDt.Rows.Count; i++)
                {
                    DateTime base_date = Convert.ToDateTime(salesDt.Rows[i]["기준일자"].ToString().Substring(0, 4) + "-" + salesDt.Rows[i]["기준일자"].ToString().Substring(4, 2) + "-" + "01");
                    double sales;
                    if (!double.TryParse(salesDt.Rows[i][cbDivision.Text].ToString(), out sales))
                        sales = 0;
                    dgvDetails.Rows[(base_date.Month - 1) * 2].Cells["sales_qty_" + base_date.Year.ToString()].Value = sales.ToString("#,##0");
                    //Dictionary 추가
                    salesDic.Add(base_date, sales);
                }
                //전월, 전년
                if (dgvDetails.Columns.Count > 1)
                {
                    for (int i = 1; i < dgvDetails.Columns.Count - 2; i = i + 2)
                    {
                        double total_amount = 0;
                        for (int j = 0; j < dgvDetails.Rows.Count - 1; j = j + 2)
                        {
                            DateTime base_date = Convert.ToDateTime(dgvDetails.Columns[i].ToolTipText + "-" + dgvDetails.Rows[j].Cells[0].Value.ToString().Replace("월", "") + "-" + "01");
                            if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) >= base_date)
                            {
                                double qty;
                                if (dgvDetails.Rows[j].Cells[i].Value == null || !double.TryParse(dgvDetails.Rows[j].Cells[i].Value.ToString(), out qty))
                                    qty = 0;
                                total_amount += qty;
                                //전월==================================================================================================================================
                                double change_rate = 0;
                                DateTime before_month_date = base_date.AddMonths(-1);
                                if (salesDic.ContainsKey(before_month_date))
                                {
                                    //전월 매출량
                                    double before_qty = salesDic[before_month_date];
                                    //변화폭
                                    if (cbChangeType.Text == "증감")
                                        change_rate = (qty - before_qty) / before_qty;  //증감률
                                    else
                                        change_rate = qty - before_qty;                 //수차
                                    //출력
                                    if (!double.IsNaN(change_rate))
                                    {
                                        if (change_rate < 0)
                                        {
                                            dgvDetails.Rows[j].Cells[i + 1].Style.ForeColor = Color.Blue;
                                            dgvDetails.Rows[j].Cells[i + 1].Value = change_rate.ToString("#,##0.0") + " ▼";
                                        }
                                        else
                                        {
                                            dgvDetails.Rows[j].Cells[i + 1].Style.ForeColor = Color.Red;
                                            dgvDetails.Rows[j].Cells[i + 1].Value = change_rate.ToString("#,##0.0") + " ▲";
                                        }
                                    }
                                }
                                //당해년도 전월
                                if (dgvDetails.Columns[i].HeaderText == DateTime.Now.Year.ToString() + "년")
                                {
                                    if (!double.IsNaN(change_rate))
                                    {
                                        if (change_rate < 0)
                                        {
                                            dgvDetails.Rows[j].Cells[dgvDetails.Columns.Count - 2].Style.ForeColor = Color.Blue;
                                            dgvDetails.Rows[j].Cells[dgvDetails.Columns.Count - 2].Value = change_rate.ToString("#,##0.0") + " ▼";
                                        }
                                        else
                                        {
                                            dgvDetails.Rows[j].Cells[dgvDetails.Columns.Count - 2].Style.ForeColor = Color.Red;
                                            dgvDetails.Rows[j].Cells[dgvDetails.Columns.Count - 2].Value = change_rate.ToString("#,##0.0") + " ▲";
                                        }
                                    }
                                }
                                //전년==================================================================================================================================
                                change_rate = 0;
                                DateTime before_year_date = base_date.AddYears(-1);
                                if (salesDic.ContainsKey(before_year_date))
                                {
                                    double before_qty = salesDic[before_year_date];
                                    //변화폭
                                    if (cbChangeType.Text == "증감")
                                        change_rate = (qty - before_qty) / before_qty;  //증감률
                                    else
                                        change_rate = qty - before_qty;                 //수차
                                    if (!double.IsNaN(change_rate))
                                    {
                                        if (change_rate < 0)
                                        {
                                            dgvDetails.Rows[j + 1].Cells[i].Style.ForeColor = Color.Blue;
                                            dgvDetails.Rows[j + 1].Cells[i].Value = change_rate.ToString("#,##0.0") + " ▼";
                                        }
                                        else
                                        {
                                            dgvDetails.Rows[j + 1].Cells[i].Style.ForeColor = Color.Red;
                                            dgvDetails.Rows[j + 1].Cells[i].Value = change_rate.ToString("#,##0.0") + " ▲";
                                        }
                                    }
                                }
                                //당해년도 전월
                                if (dgvDetails.Columns[i].HeaderText == DateTime.Now.Year.ToString() + "년")
                                {
                                    if (!double.IsNaN(change_rate))
                                    {
                                        if (change_rate < 0)
                                        {
                                            dgvDetails.Rows[j].Cells[dgvDetails.Columns.Count - 1].Style.ForeColor = Color.Blue;
                                            dgvDetails.Rows[j].Cells[dgvDetails.Columns.Count - 1].Value = change_rate.ToString("#,##0.0") + " ▼";
                                        }
                                        else
                                        {
                                            dgvDetails.Rows[j].Cells[dgvDetails.Columns.Count - 1].Style.ForeColor = Color.Red;
                                            dgvDetails.Rows[j].Cells[dgvDetails.Columns.Count - 1].Value = change_rate.ToString("#,##0.0") + " ▲";
                                        }
                                    }
                                }
                            }
                        }
                        //합계
                        dgvDetails.Rows[dgvDetails.Rows.Count - 1].Cells[i].Value = total_amount.ToString("#,##0");
                        dgvDetails.Rows[dgvDetails.Rows.Count - 1].Cells[i].Style.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                    }
                }
            }

            GetAverageSaleQty();
            GetSalesByWeek();
        }

        public void GetSalesByWeek()
        {
            //초기화
            dgvDetailsByWeek.Rows.Clear();
            //기준년월
            DateTime sttdate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime enddate = sttdate.AddMonths(-3);

            //Get Data
            DataTable salesDt = salesRepository.GetSales(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"), product, origin, sizes, unit, cbAllUnit.Checked);
            if (salesDt.Rows.Count > 0)
            {
                //주차출력
                for (int i = 0; i <= 12; i++)
                {
                    int n = dgvDetailsByWeek.Rows.Add();
                    dgvDetailsByWeek.Rows[n].Cells[0].Value = i.ToString() + "주전";
                }

                //데이터출력
                for (int i = 0; i < salesDt.Rows.Count; i++)
                {
                    int idx = Convert.ToInt32(salesDt.Rows[i]["주차"].ToString());
                    if (idx <= 12)
                    {
                        double qty;
                        if (!double.TryParse(salesDt.Rows[i]["매출수"].ToString(), out qty))
                            qty = 0;
                        dgvDetailsByWeek.Rows[idx].Cells["sales_qty"].Value = qty.ToString("#,##0");
                        double sales_amount;
                        if (!double.TryParse(salesDt.Rows[i]["매출금액"].ToString(), out sales_amount))
                            sales_amount = 0;
                        dgvDetailsByWeek.Rows[idx].Cells["sales_price"].Value = sales_amount.ToString("#,##0");
                        double margin;
                        if (!double.TryParse(salesDt.Rows[i]["마진율"].ToString(), out margin))
                            margin = 0;
                        dgvDetailsByWeek.Rows[idx].Cells["margin"].Value = margin.ToString("#,##0.0") + "%";
                        double margin_amount;
                        if (!double.TryParse(salesDt.Rows[i]["마진금액"].ToString(), out margin_amount))
                            margin_amount = 0;
                        dgvDetailsByWeek.Rows[idx].Cells["margin_amount"].Value = margin_amount.ToString("#,##0");
                    }
                }

                //추이표시
                double before_qty = 0;
                for (int i = dgvDetailsByWeek.Rows.Count - 1; i >= 0; i--)
                {
                    double sales_qty;
                    if (dgvDetailsByWeek.Rows[i].Cells["sales_qty"].Value == null || !double.TryParse(dgvDetailsByWeek.Rows[i].Cells["sales_qty"].Value.ToString(), out sales_qty))
                        sales_qty = 0;
                    double rate_change;
                    if (cbChangeType.Text == "증감")
                        rate_change = (sales_qty - before_qty) / before_qty * 100;   //증감률
                    else
                        rate_change = sales_qty - before_qty;                        //수차


                    if (double.IsInfinity(rate_change))
                        rate_change = 0;
                    string rate_change_arrow;
                    if (rate_change <= 0)
                        dgvDetailsByWeek.Rows[i].Cells["rate_change"].Style.ForeColor = Color.Blue;
                    else
                        dgvDetailsByWeek.Rows[i].Cells["rate_change"].Style.ForeColor = Color.Red;
                    dgvDetailsByWeek.Rows[i].Cells["rate_change"].Value = rate_change.ToString("#,##0.0");

                    before_qty = sales_qty;
                }
            }
        }

        private void GetAverageSaleQty()
        {
            if (string.IsNullOrEmpty(cbDivision.Text))
            {
                MessageBox.Show(this,"구분을 선택해주세요.");
                this.Activate();
                return;
            }
            dgvSalesByMonth.Rows.Clear();
            dgvSalesByMonth.Rows.Add();
            DataTable avgDt = salesRepository.GetAverageSalesByMonth(product, origin, sizes, unit, cbAllUnit.Checked, cbDivision.Text);
            if (avgDt.Rows.Count > 0)
            {
                int wDays;
                common.GetWorkDay(DateTime.Now.AddDays(-1).AddMonths(-1), DateTime.Now.AddDays(-1), out wDays);
                double qty;
                if (!double.TryParse(avgDt.Rows[0][0].ToString(), out qty))
                    qty = 0;
                dgvSalesByMonth.Rows[0].Cells[0].Value = (qty / wDays * 21).ToString("#,##0");
                //45일
                common.GetWorkDay(DateTime.Now.AddDays(-1).AddDays(-45), DateTime.Now.AddDays(-1), out wDays);
                if (!double.TryParse(avgDt.Rows[0][1].ToString(), out qty))
                    qty = 0;
                dgvSalesByMonth.Rows[0].Cells[1].Value = (qty / wDays * 21).ToString("#,##0");
                //2개월
                common.GetWorkDay(DateTime.Now.AddDays(-1).AddMonths(-2), DateTime.Now.AddDays(-1), out wDays);
                if (!double.TryParse(avgDt.Rows[0][2].ToString(), out qty))
                    qty = 0;
                dgvSalesByMonth.Rows[0].Cells[2].Value = (qty / wDays * 21).ToString("#,##0");
                //3개월
                common.GetWorkDay(DateTime.Now.AddDays(-1).AddMonths(-3), DateTime.Now.AddDays(-1), out wDays);
                if (!double.TryParse(avgDt.Rows[0][3].ToString(), out qty))
                    qty = 0;
                dgvSalesByMonth.Rows[0].Cells[3].Value = (qty / wDays * 21).ToString("#,##0");
                //6개월
                common.GetWorkDay(DateTime.Now.AddDays(-1).AddMonths(-6), DateTime.Now.AddDays(-1), out wDays);
                if (!double.TryParse(avgDt.Rows[0][4].ToString(), out qty))
                    qty = 0;
                dgvSalesByMonth.Rows[0].Cells[4].Value = (qty / wDays * 21).ToString("#,##0");
                //12개월
                common.GetWorkDay(DateTime.Now.AddDays(-1).AddMonths(-12), DateTime.Now.AddDays(-1), out wDays);
                if (!double.TryParse(avgDt.Rows[0][5].ToString(), out qty))
                    qty = 0;
                dgvSalesByMonth.Rows[0].Cells[5].Value = (qty / wDays * 21).ToString("#,##0");
                //18개월
                common.GetWorkDay(DateTime.Now.AddDays(-1).AddMonths(-18), DateTime.Now.AddDays(-1), out wDays);
                if (!double.TryParse(avgDt.Rows[0][6].ToString(), out qty))
                    qty = 0;
                dgvSalesByMonth.Rows[0].Cells[6].Value = (qty / wDays * 21).ToString("#,##0");
            }
        }
        #endregion

        #region Change event
        private void cbCompareMonth_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvDetails.Columns.Count > 1)
            {
                for (int i = 2; i < dgvDetails.Columns.Count - 2; i = i + 2)
                {
                    dgvDetails.Columns[i].Visible = cbCompareMonth.Checked;
                }

                if (!cbCompareMonth.Checked && !cbCompareYear.Checked)
                {
                    dgvDetails.Columns["prepare_pre_month_sales_qty"].Visible = true;
                    dgvDetails.Columns["prepare_pre_year_sales_qty"].Visible = true;
                }
                else
                {
                    dgvDetails.Columns["prepare_pre_month_sales_qty"].Visible = false;
                    dgvDetails.Columns["prepare_pre_year_sales_qty"].Visible = false;
                }
            }
        }

        private void cbCompareYear_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvDetails.Rows.Count > 1)
            {
                for (int i = 1; i < dgvDetails.Rows.Count; i = i + 2)
                {
                    dgvDetails.Rows[i].Visible = cbCompareYear.Checked;
                }

                if (!cbCompareMonth.Checked && !cbCompareYear.Checked)
                {
                    dgvDetails.Columns["prepare_pre_month_sales_qty"].Visible = true;
                    dgvDetails.Columns["prepare_pre_year_sales_qty"].Visible = true;
                }
                else
                {
                    dgvDetails.Columns["prepare_pre_month_sales_qty"].Visible = false;
                    dgvDetails.Columns["prepare_pre_year_sales_qty"].Visible = false;
                }
            }
        }

        private void cbAllUnit_CheckedChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void cbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetData();
        }
        #endregion

        #region Key event
        private void SalesDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        GetData();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        cbCompareMonth.Checked = !cbCompareMonth.Checked;
                        break;
                    case Keys.F2:
                        cbCompareYear.Checked = !cbCompareYear.Checked;
                        break;
                }
            }
        }
        #endregion
    }
}

