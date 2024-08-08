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

namespace AdoNetWindow.SaleManagement.SalesDashboard2
{
    public partial class SalesDashboardUnit : UserControl
    {
        ISalesRepository salesRepository = new SalesRepository();
        int sttYear, endYear, dashboardType;
        string userId, username;
        bool isIncreaseRateVisible = true;
        public SalesDashboardUnit(int sttYear, int endtYear, string userId, string department, string team, string username, int dashboardType, bool isIncreaseRateVisible
                                , string product = "", string origin= "", string sizes = "")
        {
            InitializeComponent();
            txtDepartment.Text = department;    
            txtTeam.Text = team;
            txtUsername.Text = username;
            this.userId = userId;
            this.username = username;
            this.sttYear = sttYear;
            this.endYear = endtYear;
            this.dashboardType = dashboardType;
            this.isIncreaseRateVisible = isIncreaseRateVisible;
            if (dashboardType == 1)
                GetSalesAmount(product, origin, sizes);
            else
                GetSalesCompany(product, origin, sizes);
        }
        private void SalesDashboardUnit_Load(object sender, EventArgs e)
        {
            foreach(DataGridViewColumn col in dgvSales.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        #region Method
        public string GetUserid()
        {
            return userId;
        }

        public void SetDashboardType(int dashboardType, string product, string origin, string sizes)
        {
            this.dashboardType = (int)dashboardType;
            if (dashboardType == 1)
                GetSalesAmount(product, origin ,sizes);
            else
                GetSalesCompany(product, origin, sizes);
        }
        public void AutoIncreaseVisible(bool isVisible)
        {
            isIncreaseRateVisible = isVisible;
            for (int i = 1; i <= 12; i++)
                dgvSales.Columns["r" + i].Visible = isIncreaseRateVisible;
        }
        public void GetSalesAmount(string product = "", string origin = "", string sizes = "")
        {
            dgvSales.Rows.Clear();
            DataTable salesDt = salesRepository.GetSalesForSalesDashborad(username, product, origin, sizes);
            if (salesDt.Rows.Count > 0)
            {
                int tempYear = sttYear;
                do
                {
                    int n = dgvSales.Rows.Add();
                    dgvSales.Rows[n].Cells["syear"].Value = tempYear.ToString() + "년";


                    DateTime sttDt = new DateTime(tempYear, 1, 1);
                    DateTime endDt = new DateTime(tempYear, 12, 1);

                    DataRow[] salesDr = salesDt.Select($"매출일자 >= {sttDt.ToString("yyyyMM")} AND 매출일자 <= {endDt.ToString("yyyyMM")}");
                    if (salesDr.Length > 0)
                    {
                        double total_sales_amount = 0;
                        foreach (DataRow dr in salesDr)
                        {
                            //당월매출
                            string col = "m" + Convert.ToInt16(dr["매출일자"].ToString().Substring(4, 2));
                            double sales_amount;
                            if(!double.TryParse(dr["매출금액"].ToString(), out sales_amount))
                                sales_amount = 0;

                            dgvSales.Rows[n].Cells[col].Value = sales_amount.ToString("#,##0");
                            total_sales_amount += sales_amount; 
                            //전월매출
                            double pre_sales_amount = 0;
                            DateTime preMonthDt = new DateTime(Convert.ToInt16(dr["매출일자"].ToString().Substring(0, 4))
                                                            , Convert.ToInt16(dr["매출일자"].ToString().Substring(4, 2))
                                                            , 1).AddMonths(-1);
                            DataRow[] preSalesDr = salesDt.Select($"매출일자 = {preMonthDt.ToString("yyyyMM")}");
                            if (preSalesDr.Length > 0)
                            {
                                if (!double.TryParse(preSalesDr[0]["매출금액"].ToString(), out pre_sales_amount))
                                    pre_sales_amount = 0;
                            }
                            //증하감율
                            col = col.Replace("m", "r");
                            if (pre_sales_amount > 0)
                            {
                                double increase_rate = ((double)sales_amount - (double)pre_sales_amount) / (double)pre_sales_amount * 100;
                                if (double.IsNaN(increase_rate))
                                    dgvSales.Rows[n].Cells[col].Value = "-%";
                                else if (increase_rate < 0)
                                {
                                    dgvSales.Rows[n].Cells[col].Value = "▼" + increase_rate.ToString("#,##0.0") + "%";
                                    dgvSales.Rows[n].Cells[col].Style.ForeColor = Color.Blue;
                                }
                                else
                                {
                                    dgvSales.Rows[n].Cells[col].Value = "▲" + increase_rate.ToString("#,##0.0") + "%";
                                    dgvSales.Rows[n].Cells[col].Style.ForeColor = Color.Red;
                                }
                            }
                            else
                                dgvSales.Rows[n].Cells[col].Value = "-%";

                        }
                        dgvSales.Rows[n].Cells["sum"].Value = total_sales_amount.ToString("#,##0");
                    }

                    tempYear++;

                } while (tempYear <= endYear);
                //전년도 전월대비
                int m = dgvSales.Rows.Add();
                for (int i = 0; i < 12; i++) 
                {
                    double current_sales_amount = 0;
                    DataRow[] currentDr = salesDt.Select($"매출일자 = {new DateTime(endYear, i + 1 , 1).ToString("yyyyMM")}");
                    if(currentDr != null && currentDr.Length > 0) 
                    {
                        if (!double.TryParse(currentDr[0]["매출금액"].ToString(), out current_sales_amount))
                            current_sales_amount = 0;
                    }


                    double preMonth_sales_amount = 0;
                    DataRow[] preMonthDr = salesDt.Select($"매출일자 = {new DateTime(endYear, i + 1, 1).AddYears(-1).ToString("yyyyMM")}");
                    if (preMonthDr != null && preMonthDr.Length > 0)
                    {
                        if (!double.TryParse(preMonthDr[0]["매출금액"].ToString(), out preMonth_sales_amount))
                            preMonth_sales_amount = 0;
                    }

                    if (preMonth_sales_amount == 0)
                    dgvSales.Rows[m].Cells["m" + (i + 1)].Value = "-%";
                    else if (current_sales_amount > 0)
                    {
                        double pre_year_increase_rate = ((double)current_sales_amount - (double)preMonth_sales_amount) / (double)preMonth_sales_amount * 100;
                        if (double.IsNaN(pre_year_increase_rate))
                            dgvSales.Rows[m].Cells["m" + (i + 1)].Value = "-%";
                        else if (pre_year_increase_rate < 0)
                        {
                            dgvSales.Rows[m].Cells["m" + (i + 1)].Value = "▼" + pre_year_increase_rate.ToString("#,##0.0") + "%";
                            dgvSales.Rows[m].Cells["m" + (i + 1)].Style.ForeColor = Color.Blue;
                        }
                        else
                        {
                            dgvSales.Rows[m].Cells["m" + (i + 1)].Value = "▲" + pre_year_increase_rate.ToString("#,##0.0") + "%";
                            dgvSales.Rows[m].Cells["m" + (i + 1)].Style.ForeColor = Color.Red;
                        }
                    }
                }
            }
            AutoIncreaseVisible(isIncreaseRateVisible);
        }

        public void GetSalesCompany(string product = "", string origin = "", string sizes = "")
        {
            List<string> userList = new List<string>();
            userList.Add(username);

            dgvSales.Rows.Clear();
            DataTable businessCompanyDt = salesRepository.GetBusinessCompanyForSalesDashborad(sttYear, endYear, userList);
            if (businessCompanyDt.Rows.Count > 0)
            {
                businessCompanyDt.Columns.Add("sale_date", typeof(DateTime));
                businessCompanyDt.Columns.Add("die_date", typeof(DateTime));
                for (int i = 0; i < businessCompanyDt.Rows.Count; i++)
                {
                    DateTime sale_date;
                    if (DateTime.TryParse(businessCompanyDt.Rows[i]["매출일자"].ToString().Substring(0, 4) + "-" + businessCompanyDt.Rows[i]["매출일자"].ToString().Substring(4, 2) + "-01", out sale_date))
                        businessCompanyDt.Rows[i]["sale_date"] = sale_date;
                    DateTime die_date;
                    if (DateTime.TryParse(businessCompanyDt.Rows[i]["마지막결제"].ToString().Substring(0, 4) + "-" + businessCompanyDt.Rows[i]["마지막결제"].ToString().Substring(4, 2) + "-01", out die_date))
                        businessCompanyDt.Rows[i]["die_date"] = die_date;
                }
                businessCompanyDt.AcceptChanges();
                //출력====================================================================================================================================================================
                int tempYear = sttYear;
                do
                {
                    int n = dgvSales.Rows.Add();
                    dgvSales.Rows[n].Cells["syear"].Value = tempYear.ToString() + "년";

                    int total_company_count = 0;
                    for (int i = 1; i <= 12; i++)
                    {
                        //당월 거래처수
                        DateTime businessDt = new DateTime(tempYear, i, 1);
                        DataRow[] businessCompanyDr = businessCompanyDt.Select($"매출일자 = {businessDt.ToString("yyyyMM")}");
                        int currentCompanyCount = businessCompanyDr.Length;
                        total_company_count += currentCompanyCount;
                        string col = "m" + i;
                        dgvSales.Rows[n].Cells[col].Value = currentCompanyCount.ToString("#,##0");

                        //전월 거래처수
                        businessDt = businessDt.AddMonths(-1);
                        DataRow[] preBusinessCompanyDr = businessCompanyDt.Select($"매출일자 = {businessDt.ToString("yyyyMM")}");
                        int preCompanyCount = preBusinessCompanyDr.Length;

                        //증하감율
                        col = col.Replace("m", "r");
                        if (preCompanyCount > 0)
                        {
                            double increase_rate = ((double)currentCompanyCount - (double)preCompanyCount) / (double)preCompanyCount * 100;
                            if (double.IsNaN(increase_rate))
                                dgvSales.Rows[n].Cells[col].Value = "-%";
                            else if (increase_rate < 0)
                            {
                                dgvSales.Rows[n].Cells[col].Value = "▼" + increase_rate.ToString("#,##0.0") + "%";
                                dgvSales.Rows[n].Cells[col].Style.ForeColor = Color.Blue;
                            }
                            else
                            {
                                dgvSales.Rows[n].Cells[col].Value = "▲" + increase_rate.ToString("#,##0.0") + "%";
                                dgvSales.Rows[n].Cells[col].Style.ForeColor = Color.Red;
                            }
                        }
                        else
                            dgvSales.Rows[n].Cells[col].Value = "-%";
                    }

                    dgvSales.Rows[n].Cells["sum"].Value = total_company_count.ToString("#,##0");

                    tempYear++;

                } while (tempYear <= endYear);
                //전년도 전월대비
                int m = dgvSales.Rows.Add();
                for (int i = 0; i < 12; i++)
                {
                    double current_count = 0;
                    DataRow[] currentDr = businessCompanyDt.Select($"매출일자 = {new DateTime(endYear, i + 1, 1).ToString("yyyyMM")}");
                    if (currentDr != null && currentDr.Length > 0)
                        current_count = currentDr.Length;

                    double preMonth_count = 0;
                    DataRow[] preMonthDr = businessCompanyDt.Select($"매출일자 = {new DateTime(endYear, i + 1, 1).AddYears(-1).ToString("yyyyMM")}");
                    if (preMonthDr != null && preMonthDr.Length > 0)
                        preMonth_count = preMonthDr.Length;

                    if(preMonth_count == 0)
                        dgvSales.Rows[m].Cells["m" + (i + 1)].Value = "-%";
                    else if (current_count > 0)
                    {
                        double pre_year_increase_rate = ((double)current_count - (double)preMonth_count) / (double)preMonth_count * 100;
                        if (double.IsNaN(pre_year_increase_rate))
                            dgvSales.Rows[m].Cells["m" + (i + 1)].Value = "-%";
                        else if (pre_year_increase_rate < 0)
                        {
                            dgvSales.Rows[m].Cells["m" + (i + 1)].Value = "▼" + pre_year_increase_rate.ToString("#,##0.0") + "%";
                            dgvSales.Rows[m].Cells["m" + (i + 1)].Style.ForeColor = Color.Blue;
                        }
                        else
                        {
                            dgvSales.Rows[m].Cells["m" + (i + 1)].Value = "▲" + pre_year_increase_rate.ToString("#,##0.0") + "%";
                            dgvSales.Rows[m].Cells["m" + (i + 1)].Style.ForeColor = Color.Red;
                        }
                    }
                }

            }
            AutoIncreaseVisible(isIncreaseRateVisible); 
        }

        #endregion

        #region Cell merge
        private void dgvSales_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            if (e.ColumnIndex % 2 == 1)
                e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
        }
        #endregion

    }
}
