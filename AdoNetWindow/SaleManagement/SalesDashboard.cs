using AdoNetWindow.Model;
using Repositories;
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

namespace AdoNetWindow.SaleManagement
{
    public partial class SalesDashboard : Form
    {
        UsersModel um;
        IUsersRepository usersRepository = new UsersRepository();
        ISalesRepository salesRepository = new SalesRepository();
        string[] strHeaders = new string[0];
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public SalesDashboard(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
        }

        private void SalesDashboard_Load(object sender, EventArgs e)
        {
            cbEndYear.Items.Clear();
            cbSttYear.Items.Clear();

            for (int i = 2016; i <= DateTime.Now.Year; i++)
            {
                cbSttYear.Items.Add(i.ToString());
                cbEndYear.Items.Add(i.ToString());
            }
            cbSttYear.Text = DateTime.Now.AddYears(-2).Year.ToString();
            cbEndYear.Text = DateTime.Now.Year.ToString();
            cbSttMonth.Text = "1";
            cbEndMonth.Text = DateTime.Now.Month.ToString();
        }

        #region Method
        private void SetSalesAmountDetail()
        {
            //선택한 유저 매출 상세정보
            string user_id = lbUserId.Text;
            UsersModel model = usersRepository.GetUserInfo(user_id);
            if (model != null)
            {
                lbUsername.Text = model.user_name;
                txtTargetSalesAmount.Text = model.target_sales_amount.ToString("#,##0");

                int sales_terms;
                if (!int.TryParse(cbSalesTerms.Text, out sales_terms))
                {
                    cbSalesTerms.Text = "3";
                    sales_terms = 3;
                }

                int stt_month;
                if (!int.TryParse(cbSttMonth.Text, out stt_month))
                {
                    cbSttMonth.Text = "1";
                    stt_month = 1;
                }

                int end_month;
                if (!int.TryParse(cbEndMonth.Text, out end_month))
                {
                    cbEndMonth.Text = DateTime.Now.Month.ToString();
                    end_month = DateTime.Now.Month;
                }

                int stt_year;
                if (!int.TryParse(cbSttYear.Text, out stt_year))
                {
                    cbSttYear.Text = DateTime.Now.AddYears(-2).Year.ToString();
                    stt_year = DateTime.Now.AddYears(-2).Year;
                }

                int end_year;
                if (!int.TryParse(cbEndYear.Text, out end_year))
                {
                    cbEndYear.Text = DateTime.Now.Year.ToString();
                    end_year = DateTime.Now.Year;
                }

                //매출내역
                DataTable salesDt = salesRepository.GetSalesAmountDashboard(model.user_name, DateTime.Now.AddDays(-1), sales_terms, stt_year, stt_month, end_year, end_month);
                if (salesDt.Rows.Count > 0)
                {
                    double month_amount;
                    if (!double.TryParse(salesDt.Rows[0]["한달매출"].ToString(), out month_amount))
                        month_amount = 0;
                    txtSalesAmountMonth.Text = month_amount.ToString("#,##0");

                    double avg_month_amount;
                    if (!double.TryParse(salesDt.Rows[0]["평균매출"].ToString(), out avg_month_amount))
                        avg_month_amount = 0;

                    //영업일수(그냥 바로 어제부터 적용)
                    int workDays;
                    common.GetWorkDay(DateTime.Now.AddDays(-1).AddMonths(-sales_terms), DateTime.Now.AddDays(-1), out workDays);
                    avg_month_amount = avg_month_amount / workDays * 21;
                    txtAvgSalesAmountMonth.Text = avg_month_amount.ToString("#,##0");

                    dgvUpDownRate.Rows.Clear();
                    dgvUpDownRate.Columns.Clear();
                    for (int i = stt_year; i <= end_year; i++)
                    {
                        dgvUpDownRate.Columns.Add("sales_amount_" + i.ToString(), i.ToString() + "년");
                    }
                    dgvUpDownRate.Rows.Add(2);

                    double pre_year_sales_amount = 0;
                    for (int i = stt_year; i <= end_year; i++)
                    {
                        double year_sales_amount;
                        if (!double.TryParse(salesDt.Rows[0]["매출" + i.ToString()].ToString(), out year_sales_amount))
                            year_sales_amount = 0;

                        dgvUpDownRate.Rows[0].Cells["sales_amount_" + i.ToString()].Value = year_sales_amount.ToString("#,##0");

                        double updownRate = (year_sales_amount - pre_year_sales_amount) / pre_year_sales_amount * 100;
                        if (updownRate > 0)
                        {
                            dgvUpDownRate.Rows[1].Cells["sales_amount_" + i.ToString()].Style.ForeColor = Color.Red;
                            dgvUpDownRate.Rows[1].Cells["sales_amount_" + i.ToString()].Value = "▲" + updownRate.ToString("#,##0.0");
                        }
                        else
                        {
                            dgvUpDownRate.Rows[1].Cells["sales_amount_" + i.ToString()].Style.ForeColor = Color.Blue;
                            dgvUpDownRate.Rows[1].Cells["sales_amount_" + i.ToString()].Value = "▼" + updownRate.ToString("#,##0.0");
                        }
                        pre_year_sales_amount = year_sales_amount;
                    }
                }
            }
        }
        private void SetHeaderStyle()
        {
            dgvSales.Columns["month"].Frozen = true;
            dgvSales.Columns["select_user_name"].Frozen = true;

            dgvSales.RowHeadersDefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
            dgvSales.Columns["month"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSales.Columns["month"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
            dgvSales.Columns["month"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSales.Columns["select_user_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSales.Columns["select_user_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            for (int i = 3; i < dgvSales.Columns.Count; i = i + 6)
            {
                int year = Convert.ToInt16(dgvSales.Columns[i].Name.Split('_')[0]);
                if (!cbMonthRate.Checked && year < DateTime.Now.Year)
                    dgvSales.Columns[i + 1].Visible = false;

                if (!cbYear.Checked)
                    dgvSales.Columns[i + 2].Visible = false;

                dgvSales.Columns[i + 3].Visible = cbBusinessCompany.Checked;
                dgvSales.Columns[i + 4].Visible = cbBusinessCompany.Checked;
                dgvSales.Columns[i + 5].Visible = cbBusinessCompany.Checked;
            }
        }

        private void GetUsers()
        {
            dgvUsers.Rows.Clear();
            //영업부 출력
            DataTable userDt = usersRepository.GetUsers("", txtDepartment.Text, "", txtUsesname.Text, "승인", txtGrade.Text);
            for (int i = 0; i < userDt.Rows.Count; i++)
            { 
                int n = dgvUsers.Rows.Add();
                dgvUsers.Rows[n].Cells["users_id"].Value = userDt.Rows[i]["user_id"].ToString();
                dgvUsers.Rows[n].Cells["user_name"].Value = userDt.Rows[i]["user_name"].ToString();
                dgvUsers.Rows[n].Cells["department"].Value = userDt.Rows[i]["department"].ToString();
                dgvUsers.Rows[n].Cells["grade"].Value = userDt.Rows[i]["grade"].ToString();
            }
        }
        private void GetSales()
        {
            dgvSales.Rows.Clear();
            if (dgvSales.Columns.Count > 3)
            {
                for (int i = dgvSales.Columns.Count - 1; i >= 3; i--)
                    dgvSales.Columns.Remove(dgvSales.Columns[i]);
            }
            //유효성검사
            int sttYear, endYear;
            if (!int.TryParse(cbSttYear.Text, out sttYear) || !int.TryParse(cbEndYear.Text, out endYear))
            {
                messageBox.Show(this,"검색기간을 확인해주세요.");
                this.Activate();
                return;
            }
            else
            {
                //월, 선택한 인원 출력
                List<string> userList = new List<string>();
                for (int i = 0; i < dgvSelectUsers.RowCount; i++)
                    userList.Add(dgvSelectUsers.Rows[i].Cells["select_name"].Value.ToString());
                dgvSales.EndEdit();
                dgvSales.AutoGenerateColumns = false;
                //출력
                Dictionary<string, int> rowIndexDic = new Dictionary<string, int>();
                for (int i = 1; i <= 12; i++)
                {
                    Color col;
                    if (i % 2 == 0)
                        col = Color.White;
                    else
                        col = Color.WhiteSmoke;
                    for (int j = 0; j < userList.Count; j++)
                    {
                        int n = dgvSales.Rows.Add();
                        dgvSales.Rows[n].Cells["month"].Value = i.ToString();
                        dgvSales.Rows[n].Cells["division"].Value = i.ToString();
                        //dgvSales.Rows[n].HeaderCell.Value = i.ToString();

                        dgvSales.Rows[n].Cells["select_user_name"].Value = userList[j];
                        dgvSales.Rows[n].DefaultCellStyle.BackColor = col;

                        if (!rowIndexDic.ContainsKey(i.ToString().PadLeft(2, '0') + userList[j]))
                            rowIndexDic.Add(i.ToString().PadLeft(2, '0') + userList[j], n);
                    }
                }
                //합계
                for (int j = 0; j < userList.Count; j++)
                {
                    int n = dgvSales.Rows.Add();
                    dgvSales.Rows[n].Cells["month"].Value = "합계";
                    dgvSales.Rows[n].Cells["division"].Value = "합계";
                    //dgvSales.Rows[n].HeaderCell.Value = "합계";
                    dgvSales.Rows[n].Cells["select_user_name"].Value = userList[j];
                    dgvSales.Rows[n].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                    dgvSales.Rows[n].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                    if (!rowIndexDic.ContainsKey("합계" + userList[j]))
                        rowIndexDic.Add("합계" + userList[j], n);
                }

                //년도 출력
                Dictionary<string, int> columnIndexDic = new Dictionary<string, int>();
                strHeaders = new string[endYear - sttYear + 1];
                int cIdx = 3;
                for (int i = sttYear; i <= endYear; i++)
                {
                    strHeaders[i - sttYear] = i.ToString().Substring(2, 2) + "년";
                    columnIndexDic.Add(i.ToString(), cIdx);
                    cIdx += 6;

                    dgvSales.Columns.Add(i.ToString() + "_amount", "매출");
                    dgvSales.Columns[i.ToString() + "_amount"].Width = 100;
                    dgvSales.Columns[i.ToString() + "_amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvSales.Columns[i.ToString() + "_amount"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

                    dgvSales.Columns.Add(i.ToString() + "_month_rate", "月증감");
                    dgvSales.Columns[i.ToString() + "_month_rate"].Width = 70;
                    dgvSales.Columns[i.ToString() + "_month_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvSales.Columns[i.ToString() + "_month_rate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

                    dgvSales.Columns.Add(i.ToString() + "_year_rate", "年증감");
                    dgvSales.Columns[i.ToString() + "_year_rate"].Width = 70;
                    dgvSales.Columns[i.ToString() + "_year_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvSales.Columns[i.ToString() + "_year_rate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

                    dgvSales.Columns.Add(i.ToString() + "_business_count", "거래처");
                    dgvSales.Columns[i.ToString() + "_business_count"].Width = 70;
                    dgvSales.Columns[i.ToString() + "_business_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvSales.Columns[i.ToString() + "_business_count"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

                    dgvSales.Columns.Add(i.ToString() + "_new_count", "신규");
                    dgvSales.Columns[i.ToString() + "_new_count"].Width = 70;
                    dgvSales.Columns[i.ToString() + "_new_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvSales.Columns[i.ToString() + "_new_count"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

                    dgvSales.Columns.Add(i.ToString() + "_die_count", "끊김");
                    dgvSales.Columns[i.ToString() + "_die_count"].Width = 70;
                    dgvSales.Columns[i.ToString() + "_die_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvSales.Columns[i.ToString() + "_die_count"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
                }

                //데이터 매칭
                DataTable saleDt = salesRepository.GetSalesForSalesDashborad(userList);
                for (int i = 0; i < saleDt.Rows.Count; i++)
                {
                    DateTime sale_date;
                    if (DateTime.TryParse(saleDt.Rows[i]["매출일자"].ToString().Substring(0, 4) + "-" + saleDt.Rows[i]["매출일자"].ToString().Substring(4, 2) + "-01", out sale_date))
                    {
                        if (rowIndexDic.ContainsKey(sale_date.Month.ToString().PadLeft(2, '0') + saleDt.Rows[i]["매출자"].ToString())
                            && columnIndexDic.ContainsKey(sale_date.Year.ToString().PadLeft(2, '0')))
                        {
                            //당월매출
                            int rowIdx = rowIndexDic[sale_date.Month.ToString().PadLeft(2, '0') + saleDt.Rows[i]["매출자"].ToString()];
                            int colIdx = columnIndexDic[sale_date.Year.ToString().PadLeft(2, '0')];

                            double total_sale_amount;
                            if (dgvSales.Rows[rowIdx].Cells[colIdx].Value == null || !double.TryParse(dgvSales.Rows[rowIdx].Cells[colIdx].Value.ToString(), out total_sale_amount))
                                total_sale_amount = 0;

                            double sale_amount;
                            if (!double.TryParse(saleDt.Rows[i]["매출금액"].ToString(), out sale_amount))
                                sale_amount = 0;
                            total_sale_amount += sale_amount;
                            dgvSales.Rows[rowIdx].Cells[colIdx].Value = total_sale_amount.ToString("#,##0");
                            //합계매출
                            rowIdx = rowIndexDic["합계" + saleDt.Rows[i]["매출자"].ToString()];
                            if (dgvSales.Rows[rowIdx].Cells[colIdx].Value == null || !double.TryParse(dgvSales.Rows[rowIdx].Cells[colIdx].Value.ToString(), out total_sale_amount))
                                total_sale_amount = 0;
                            total_sale_amount += sale_amount;
                            dgvSales.Rows[rowIdx].Cells[colIdx].Value = total_sale_amount.ToString("#,##0");
                        }
                    }
                }
                //월, 년 증감율
                for (int i = 3; i < dgvSales.Columns.Count; i = i + 6)
                {
                    int year = Convert.ToInt16(dgvSales.Columns[i].Name.Split('_')[0]);

                    for (int j = 0; j < dgvSales.Rows.Count; j++)
                    {
                        DateTime sale_date;
                        int month;
                        if (dgvSales.Rows[j].Cells["division"].Value != null && int.TryParse(dgvSales.Rows[j].Cells["division"].Value.ToString(), out month)
                            && DateTime.TryParse(year.ToString() + "-" + month.ToString() + "-01", out sale_date)
                            && sale_date < DateTime.Now)
                        {
                            double sale_amount;
                            if (dgvSales.Rows[j].Cells[i].Value == null || !double.TryParse(dgvSales.Rows[j].Cells[i].Value.ToString(), out sale_amount))
                                sale_amount = 0;
                            //전월대비 증감율
                            DateTime pre_sale_date = sale_date.AddMonths(-1);
                            if (rowIndexDic.ContainsKey(pre_sale_date.Month.ToString().PadLeft(2, '0') + dgvSales.Rows[j].Cells["select_user_name"].Value.ToString())
                                && columnIndexDic.ContainsKey(pre_sale_date.Year.ToString().PadLeft(2, '0')))
                            {
                                int pre_rowIdx = rowIndexDic[pre_sale_date.Month.ToString().PadLeft(2, '0') + dgvSales.Rows[j].Cells["select_user_name"].Value.ToString()];
                                int pre_colIdx = columnIndexDic[pre_sale_date.Year.ToString().PadLeft(2, '0')];

                                double pre_sale_amount;
                                if (dgvSales.Rows[pre_rowIdx].Cells[pre_colIdx].Value == null || !double.TryParse(dgvSales.Rows[pre_rowIdx].Cells[pre_colIdx].Value.ToString(), out pre_sale_amount))
                                    pre_sale_amount = 0;

                                if (pre_sale_amount > 0)
                                {
                                    double sale_rate = ((sale_amount - pre_sale_amount) / pre_sale_amount) * 100;
                                    if (sale_rate > 0)
                                    {
                                        dgvSales.Rows[j].Cells[i + 1].Value = "▲" + sale_rate.ToString("#,##0.0") + "%";
                                        dgvSales.Rows[j].Cells[i + 1].Style.ForeColor = Color.Red;
                                    }
                                    else if (sale_rate < 0)
                                    {
                                        dgvSales.Rows[j].Cells[i + 1].Value = "▼" + (-sale_rate).ToString("#,##0.0") + "%";
                                        dgvSales.Rows[j].Cells[i + 1].Style.ForeColor = Color.Blue;
                                    }
                                }
                            }
                            //전년대비 증감율
                            pre_sale_date = sale_date.AddYears(-1);
                            if (rowIndexDic.ContainsKey(pre_sale_date.Month.ToString().PadLeft(2, '0') + dgvSales.Rows[j].Cells["select_user_name"].Value.ToString())
                                && columnIndexDic.ContainsKey(pre_sale_date.Year.ToString().PadLeft(2, '0')))
                            {
                                int pre_rowIdx = rowIndexDic[pre_sale_date.Month.ToString().PadLeft(2, '0') + dgvSales.Rows[j].Cells["select_user_name"].Value.ToString()];
                                int pre_colIdx = columnIndexDic[pre_sale_date.Year.ToString().PadLeft(2, '0')];

                                double pre_sale_amount;
                                if (dgvSales.Rows[pre_rowIdx].Cells[pre_colIdx].Value == null || !double.TryParse(dgvSales.Rows[pre_rowIdx].Cells[pre_colIdx].Value.ToString(), out pre_sale_amount))
                                    pre_sale_amount = 0;

                                if (pre_sale_amount > 0)
                                {
                                    double sale_rate = ((sale_amount - pre_sale_amount) / pre_sale_amount) * 100;
                                    if (sale_rate > 0)
                                    {
                                        dgvSales.Rows[j].Cells[i + 2].Value = "▲" + sale_rate.ToString("#,##0.0") + "%";
                                        dgvSales.Rows[j].Cells[i + 2].Style.ForeColor = Color.Red;
                                    }
                                    else if (sale_rate < 0)
                                    {
                                        dgvSales.Rows[j].Cells[i + 2].Value = "▼" + (-sale_rate).ToString("#,##0.0") + "%";
                                        dgvSales.Rows[j].Cells[i + 2].Style.ForeColor = Color.Blue;
                                    }
                                }
                            }
                        }
                        //합계
                        else if(dgvSales.Rows[j].Cells["division"].Value != null && dgvSales.Rows[j].Cells["division"].Value.ToString() == "합계")
                        {
                            if (columnIndexDic.ContainsKey((year - 1).ToString()))
                            {
                                int pre_colIdx = columnIndexDic[(year - 1).ToString()];
                                double pre_sale_amount;
                                if (dgvSales.Rows[j].Cells[pre_colIdx].Value == null || !double.TryParse(dgvSales.Rows[j].Cells[pre_colIdx].Value.ToString(), out pre_sale_amount))
                                    pre_sale_amount = 0;
                                if (pre_sale_amount > 0)
                                {
                                    double sale_amount;
                                    if (dgvSales.Rows[j].Cells[i].Value == null || !double.TryParse(dgvSales.Rows[j].Cells[i].Value.ToString(), out sale_amount))
                                        sale_amount = 0;

                                    double sale_rate = ((sale_amount - pre_sale_amount) / pre_sale_amount) * 100;
                                    if (sale_rate > 0)
                                    {
                                        dgvSales.Rows[j].Cells[i + 1].Value = "▲" + sale_rate.ToString("#,##0.0") + "%";
                                        dgvSales.Rows[j].Cells[i + 1].Style.ForeColor = Color.Red;
                                    }
                                    else if (sale_rate < 0)
                                    {
                                        dgvSales.Rows[j].Cells[i + 1].Value = "▼" + (-sale_rate).ToString("#,##0.0") + "%";
                                        dgvSales.Rows[j].Cells[i + 1].Style.ForeColor = Color.Blue;
                                    }
                                }

                            }
                        }

                    }
                }

                //거래처 수, 신규, 끊김
                DataTable businessCompanyDt = salesRepository.GetBusinessCompanyForSalesDashborad(2016, endYear, userList);
                if (dgvSales.Rows.Count > 0 && businessCompanyDt.Rows.Count > 0)
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

                    //출력
                    for (int i = 3; i < dgvSales.ColumnCount; i = i + 6)
                    {
                        int year = Convert.ToInt16(dgvSales.Columns[i].Name.Split('_')[0]);
                        for (int j = 0; j < dgvSales.RowCount; j++)
                        {
                            DateTime current_sale_date;
                            int month;
                            if (dgvSales.Rows[j].Cells["division"].Value != null && int.TryParse(dgvSales.Rows[j].Cells["division"].Value.ToString(), out month)
                                && DateTime.TryParse(year.ToString() + "-" + month.ToString() + "-01", out current_sale_date))
                            {
                                DateTime stt_date = current_sale_date;
                                DateTime end_date = current_sale_date.AddMonths(1).AddDays(-1);

                                string whr = $"sale_date >= '{stt_date.ToString("yyyy-MM-dd")}' AND sale_date <= '{end_date.ToString("yyyy-MM-dd")}' AND 매출자 = '{dgvSales.Rows[j].Cells["select_user_name"].Value.ToString()}'";
                                DataRow[] businessRow = businessCompanyDt.Select(whr);
                                if (businessRow.Length > 0)
                                {
                                    //거래처수
                                    dgvSales.Rows[j].Cells[year.ToString() + "_business_count"].Value = businessRow.Length.ToString("#,##0");

                                    int newCount = 0;
                                    int dieCount = 0;
                                    for (int k = 0; k < businessRow.Length; k++)
                                    {
                                        //신규
                                        whr = $"sale_date < '{stt_date.ToString("yyyy-MM-dd")}' AND 매출자 = '{dgvSales.Rows[j].Cells["select_user_name"].Value.ToString()}' AND 매출처 = '{businessRow[k]["매출처"].ToString()}'";
                                        DataRow[] newRow = businessCompanyDt.Select(whr);
                                        if (newRow.Length == 0)
                                            newCount++;
                                        //끊김
                                        whr = $"die_date = '{stt_date.ToString("yyyy-MM-dd")}' AND 매출자 = '{dgvSales.Rows[j].Cells["select_user_name"].Value.ToString()}' AND 매출처 = '{businessRow[k]["매출처"].ToString()}'";
                                        DataRow[] dieRow = businessCompanyDt.Select(whr);
                                        if (dieRow.Length > 0)
                                            dieCount++;
                                    }
                                    //신규
                                    dgvSales.Rows[j].Cells[year.ToString() + "_new_count"].Value = newCount.ToString("#,##0");
                                    //끊김
                                    if(j < 11)
                                        dgvSales.Rows[j + 1].Cells[year.ToString() + "_die_count"].Value = dieCount.ToString("#,##0");
                                }
                            }
                        }
                    }
                }
            }
            SetHeaderStyle();
            SetSalesAmountDetail();
        }

        #endregion

        #region Key event
        private void txtTargetSalesAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UsersModel model = usersRepository.GetUserInfo(lbUserId.Text);
                if (model != null)
                {
                    if (messageBox.Show(this, model.user_name + "님의 목표 매출금액으로 등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        double sales_amount;
                        if (!double.TryParse(txtTargetSalesAmount.Text, out sales_amount))
                        {
                            messageBox.Show(this,"목표금액을 다시 확인해주시기 바랍니다.");
                            this.Activate();
                            return;
                        }
                        int result = usersRepository.UpdateTargetSalesAmount(model.user_id, sales_amount);
                        if (result == -1)
                        {
                            messageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                            this.Activate();
                        }
                        else
                        {
                            messageBox.Show(this, "등록완료");
                            this.Activate();
                        }
                    }
                }
            }
        }
        private void txtDepartment_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    GetUsers();
                    break;
            }
        }

        private void SalesDashboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearch.PerformClick();
                        break;
                    case Keys.N:
                        txtDepartment.Text = string.Empty;
                        txtUsesname.Text = string.Empty;
                        txtGrade.Text = string.Empty;
                        txtUsesname.Focus();
                        break;
                    case Keys.M:
                        txtUsesname.Focus();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        cbMonthRate.Checked = !cbMonthRate.Checked;
                        break;
                    case Keys.F2:
                        cbYear.Checked = !cbYear.Checked;
                        break;
                    case Keys.F3:
                        cbBusinessCompany.Checked = !cbBusinessCompany.Checked;
                        break;
                    case Keys.F5:
                        btnRefresh.PerformClick();
                        break;
                }
            }
        }
        #endregion

        #region Button
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgvSelectUsers.Rows.Clear();
            dgvSales.Rows.Clear();
            if (dgvSales.Columns.Count > 3)
            {
                for (int i = dgvSales.Columns.Count - 1; i >= 3; i--)
                    dgvSales.Columns.Remove(dgvSales.Columns[i]);
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetSales();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region 우클릭 메뉴
        ContextMenuStrip m = new ContextMenuStrip();
        private void dgvUsers_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvUsers.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;
                    m = new ContextMenuStrip();
                    if (this.dgvUsers.Rows.Count > 0)
                    {
                        m.Items.Add("추가");
                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                        //Create 
                        m.Show(dgvUsers, e.Location);
                        //Selection
                        /*PendingList.ClearSelection();
                        DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                        selectRow.Selected = !selectRow.Selected;*/
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void dgvSelectUsers_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvSelectUsers.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;
                    m = new ContextMenuStrip();
                    if (this.dgvUsers.Rows.Count > 0)
                    {
                        m.Items.Add("삭제");
                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                        //Create 
                        m.Show(dgvSelectUsers, e.Location);
                        //Selection
                        /*PendingList.ClearSelection();
                        DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                        selectRow.Selected = !selectRow.Selected;*/
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "추가":
                    if (dgvUsers.SelectedRows.Count > 0)
                    {
                        for (int i = 0; i < dgvUsers.RowCount; i++)
                        {
                            if (dgvUsers.Rows[i].Selected)
                            {
                                for (int j = 0; j < dgvSelectUsers.Rows.Count; j++)
                                {
                                    if (dgvSelectUsers.Rows[j].Cells["select_name"].Value == dgvUsers.Rows[i].Cells["user_name"].Value)
                                        break;
                                }
                                int n = dgvSelectUsers.Rows.Add();
                                dgvSelectUsers.Rows[n].Cells["select_department"].Value = dgvUsers.Rows[i].Cells["department"].Value.ToString();
                                dgvSelectUsers.Rows[n].Cells["select_grade"].Value = dgvUsers.Rows[i].Cells["grade"].Value.ToString();
                                dgvSelectUsers.Rows[n].Cells["select_name"].Value = dgvUsers.Rows[i].Cells["user_name"].Value.ToString();
                                dgvSelectUsers.Rows[n].Cells["select_user_id"].Value = dgvUsers.Rows[i].Cells["users_id"].Value.ToString();
                            }
                        }
                    }
                    break;
                case "삭제":
                    if (dgvSelectUsers.SelectedRows.Count > 0)
                    {
                        for (int i = dgvSelectUsers.RowCount - 1; i >= 0 ; i--)
                        {
                            if (dgvSelectUsers.Rows[i].Selected)
                                dgvSelectUsers.Rows.Remove(dgvSelectUsers.Rows[i]);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Datagridview 멀티헤더
        private void dgvSales_Paint(object sender, PaintEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            if (strHeaders.Length > 0)
            {
                int col_inx = 2;
                for (int i = 0; i < strHeaders.Length; i++)
                {
                    Color col = Color.FromArgb(226, 239, 218);
                    col_inx += 1;
                    Rectangle r1 = gv.GetCellDisplayRectangle(col_inx, -1, false);
                    col_inx += 1;
                    int width1 = gv.GetCellDisplayRectangle(col_inx, -1, false).Width;
                    col_inx += 1;
                    int width2 = gv.GetCellDisplayRectangle(col_inx, -1, false).Width;
                    col_inx += 1;
                    int width3 = gv.GetCellDisplayRectangle(col_inx, -1, false).Width;
                    col_inx += 1;
                    int width4 = gv.GetCellDisplayRectangle(col_inx, -1, false).Width;
                    col_inx += 1;
                    int width5 = gv.GetCellDisplayRectangle(col_inx, -1, false).Width;
                    r1.X += 1;
                    r1.Y += 1;
                    r1.Width = r1.Width + width1 + width2 + width3 + width4 + width5 - 2;
                    r1.Height = (r1.Height / 2) - 2;
                    e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                    Color colBlue = Color.FromArgb(221, 235, 247);
                    e.Graphics.FillRectangle(new SolidBrush(col), r1);
                    e.Graphics.DrawString(strHeaders[i], gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
                }
            }
        }
        private void dgvSales_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                Rectangle r = e.CellBounds;
                r.Y += e.CellBounds.Height / 2;
                r.Height = e.CellBounds.Height / 2;
                e.PaintBackground(r, true);
                e.PaintContent(r);
                e.Handled = true;


            }
            else
            {
                //셀병합
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
                if (e.RowIndex < 1 || e.ColumnIndex < 0)
                {
                    e.AdvancedBorderStyle.Top = dgvSales.AdvancedCellBorderStyle.Top;
                    return;
                }
                else
                {
                    if (!IsDrawTopBorder(e.ColumnIndex, e.RowIndex))
                        e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                    else
                        e.AdvancedBorderStyle.Top = dgvSales.AdvancedCellBorderStyle.Top;
                }
            }
        }

        private void dgvSales_Scroll(object sender, ScrollEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            Rectangle rtHeader = gv.DisplayRectangle;
            rtHeader.Height = gv.ColumnHeadersHeight / 2;
            gv.Invalidate(rtHeader);


        }

        private void dgvSales_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            Rectangle rtHeader = gv.DisplayRectangle;
            rtHeader.Height = gv.ColumnHeadersHeight / 2;
            gv.Invalidate(rtHeader);
        }
        #endregion

        #region Datagridview 셀 병합
        // 그리드 위쪽 선을 그릴지?
        // 현재 칸이 null이거나 위쪽 셀과 같으면 그리지 않는다.
        private bool IsDrawTopBorder(int column, int row)
        {
            // 첫번째 row는 무조건 그린다.
            if (row == 0)
                return true;

            // 두번째 column 까지만 체크(나머진 모두 그린다)
            if (column >= 2)
                return true;

            // 사용자 추가 허용인 경우 마지막줄은 추가용 빈킨다.
            if (dgvSales.AllowUserToAddRows)
            {
                if (row == dgvSales.Rows.Count - 1)
                    return true;
            }

            // 현재 column만 비교하지 않고 이전 column까지 비교해서
            // 모두 같으면 안그린다.

            // 상위 한가지라도 다르면 그린다.
            for (int col = 0; col <= column; ++col)
            {
                // 현재셀
                DataGridViewCell cell1 = dgvSales[col, row];

                // 위쪽셀
                DataGridViewCell cell2 = dgvSales[col, row - 1];
                if (cell1.Value == null || cell2.Value == null)
                {
                    return false;
                }

                string str1 = cell1.Value.ToString();
                string str2 = cell2.Value.ToString();

                // 현재셀이 null ("") 이면 이전셀과 같은 것으로 판단
                if (str1 == null || str1 == "")
                    continue;

                // 이전셀과 비교해서 다르면 찍어야함
                if (str1 != str2)
                    return true;

            }

            return false;

        }
        private void dgvSales_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 2)
                return;
            if (!IsDrawTopBorder(e.ColumnIndex, e.RowIndex))
            {
                e.Value = "";
                e.FormattingApplied = true;
            }
        }
        #endregion

        #region Datagridview 기타 event
        private void dgvUsers_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvUsers.Rows.Count > 0 && e.RowIndex >= 0)
            {
                for (int i = 0; i < dgvSelectUsers.Rows.Count; i++)
                {
                    if (dgvSelectUsers.Rows[i].Cells["select_name"].Value == dgvUsers.Rows[e.RowIndex].Cells["user_name"].Value)
                        break;
                }
                int n = dgvSelectUsers.Rows.Add();
                dgvSelectUsers.Rows[n].Cells["select_department"].Value = dgvUsers.Rows[e.RowIndex].Cells["department"].Value.ToString();
                dgvSelectUsers.Rows[n].Cells["select_grade"].Value = dgvUsers.Rows[e.RowIndex].Cells["grade"].Value.ToString();
                dgvSelectUsers.Rows[n].Cells["select_name"].Value = dgvUsers.Rows[e.RowIndex].Cells["user_name"].Value.ToString();
                dgvSelectUsers.Rows[n].Cells["select_user_id"].Value = dgvUsers.Rows[e.RowIndex].Cells["users_id"].Value.ToString();
            }
        }
        Libs.Tools.Common common = new Libs.Tools.Common();
        private void dgvSelectUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string user_name = dgvSelectUsers.Rows[e.RowIndex].Cells["select_name"].Value.ToString();

                for (int i = 0; i < dgvSales.Rows.Count; i++)
                {
                    if (dgvSales.Rows[i].Cells["division"].Value.ToString() != "합계")
                    {
                        if (dgvSales.Rows[i].Cells["select_user_name"].Value.ToString() == user_name)
                        {
                            //dgvSales.Rows[i].DefaultCellStyle.Font = new Font("나눔고딕", 10, FontStyle.Regular);
                            dgvSales.Rows[i].DefaultCellStyle.BackColor = Color.Beige;
                        }
                        else
                        {
                            int month = 0;
                            if (dgvSales.Rows[i].Cells["division"].Value != null && int.TryParse(dgvSales.Rows[i].Cells["division"].Value.ToString(), out month))
                            {
                                //dgvSales.Rows[i].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Regular);
                                if (month % 2 == 0)
                                    dgvSales.Rows[i].DefaultCellStyle.BackColor = Color.White;
                                else
                                    dgvSales.Rows[i].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                            }
                        }
                    }
                }
                lbUserId.Text = dgvSelectUsers.Rows[e.RowIndex].Cells["select_user_id"].Value.ToString();
                SetSalesAmountDetail();
            }
        }
        

        #endregion

        #region CheckBox event
        private void cbEndMonth_TabIndexChanged(object sender, EventArgs e)
        {
            SetSalesAmountDetail();
        }
        private void cbMonthRate_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 3; i < dgvSales.Columns.Count; i = i + 6)
            {
                int year = Convert.ToInt16(dgvSales.Columns[i].Name.Split('_')[0]);
                dgvSales.Columns[i + 1].Visible = cbMonthRate.Checked;
            }
        }
        private void cbYear_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 3; i < dgvSales.Columns.Count; i = i + 6)
            {
                int year = Convert.ToInt16(dgvSales.Columns[i].Name.Split('_')[0]);
                dgvSales.Columns[i + 2].Visible = cbYear.Checked;
            }
        }

        private void cbBusinessCompany_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 3; i < dgvSales.Columns.Count; i = i + 6)
            {
                dgvSales.Columns[i + 1].Visible = cbMonthRate.Checked;
                int year = Convert.ToInt16(dgvSales.Columns[i].Name.Split('_')[0]);
                dgvSales.Columns[i + 3].Visible = cbBusinessCompany.Checked;
                dgvSales.Columns[i + 4].Visible = cbBusinessCompany.Checked;
                dgvSales.Columns[i + 5].Visible = cbBusinessCompany.Checked;
            }
        }



        #endregion

        #region 숫자 콤마 자동완성
        string prevValue = string.Empty;
        private void txtTargetSalesAmount_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            string text = tb.Text.Replace(",", "");

            double num = 0;
            if (double.TryParse(text, out num))
            {
                tb.Text = string.Format("{0:#,##0}", num);
                tb.SelectionStart = tb.TextLength;
                tb.SelectionLength = 0;
            }
            else
            {
                tb.Text = prevValue;
            }
            prevValue = tb.Text;
        }
        #endregion
    }
}
