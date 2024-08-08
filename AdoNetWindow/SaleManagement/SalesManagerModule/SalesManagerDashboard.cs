using AdoNetWindow.Model;
using Libs.MultiHeaderGrid;
using MySqlX.XDevAPI.Common;
using Repositories;
using Repositories.Calender;
using Repositories.Company;
using Repositories.Config;
using Repositories.SalesPartner;
using Repositories.SEAOVER.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement.SalesManagerModule
{
    public partial class SalesManagerDashboard : Form
    {
        ISalesPartnerRepository salesPartnerRepository = new SalesPartnerRepository();
        ICompanyGroupRepository companyGroupRepository = new CompanyGroupRepository();
        ISalesRepository salesRepository = new SalesRepository();
        IDepartmentRepository departmentRepository = new DepartmentRepository();
        IUsersRepository usersRepository = new UsersRepository();
        IAnnualRepository annualRepository = new AnnualRepository();
        ICommonRepository commonRepository = new CommonRepository();    
        DataTable atoDt = new DataTable();
        DataTable mainDt = new DataTable();
        UsersModel um;
        Libs.MessageBox messagebox = new Libs.MessageBox();
        DataGridViewRow currentRow = null;
        public SalesManagerDashboard(UsersModel um, DataTable mainDt)
        {
            InitializeComponent();
            this.um = um;
            this.mainDt = mainDt;
        }

        private void SalesManagerDashboard_Load(object sender, EventArgs e)
        {
            //RefreshTable();
            txtSttdate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            //프로그래스 바
            DataGridViewProgressColumn dailyProgressColumn = new DataGridViewProgressColumn();
            dailyProgressColumn.Name = "daily_success_rate";
            dailyProgressColumn.HeaderText = "일목표";

            DataGridViewProgressColumn monthlyProgressColumn = new DataGridViewProgressColumn();
            monthlyProgressColumn.Name = "monthly_success_rate";
            monthlyProgressColumn.HeaderText = "월목표";

            DataGridViewButtonColumn selectBtnColumn = new DataGridViewButtonColumn();
            selectBtnColumn.Name = "btnSelect";
            selectBtnColumn.HeaderText = "";
            selectBtnColumn.Width = 40;
            selectBtnColumn.DefaultCellStyle.NullValue = "선택";

            dgvEmplyee.Columns.Add(dailyProgressColumn);
            dgvEmplyee.Columns.Add(monthlyProgressColumn);
            dgvEmplyee.Columns.Add(selectBtnColumn);
        }

        #region Method

        private void VacationSuccessRate(DataGridViewRow selectRow)
        {
            DateTime sttdate, enddate;
            if (!DateTime.TryParse(txtSttdate.Text, out sttdate) || !DateTime.TryParse(txtEnddate.Text, out enddate))
            {
                messagebox.Show(this, "검색일자를 확인해주세요!");
                return;
            }
            DateTime monthSttdate = new DateTime(sttdate.Year, sttdate.Month, 1), monthEnddate = new DateTime(enddate.Year, enddate.Month,1).AddMonths(1).AddDays(-1);
            
            //영업일
            int work_days;
            common.GetWorkDay(monthSttdate, monthEnddate, out work_days);
            txtWorkDays.Text = work_days.ToString();

            int used_vacation;
            if (!int.TryParse(txtUsedVacation.Text, out used_vacation))
                used_vacation = 0;

            //일 목표량
            int daily_target_amount;
            if (!int.TryParse(txtDailyTargetAmount.Text, out daily_target_amount))
                daily_target_amount = 0;

            //월 목표량
            int monthly_target_amount = daily_target_amount * (work_days - used_vacation);
            txtMonthTargetAmount.Text = monthly_target_amount.ToString("#,##0");

            //일일 목표량
            DataTable dailyDt = salesPartnerRepository.GetUserSaleDashboard(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), selectRow.Cells["user_name"].Value.ToString());
            int daily_success_amount = 0;
            if (dailyDt != null && dailyDt.Rows.Count > 0)
                daily_success_amount = Convert.ToInt32(dailyDt.Rows[0]["am_cnt"].ToString()) + Convert.ToInt32(dailyDt.Rows[0]["pm_cnt"].ToString());

            txtDailySuccessRate.Text = daily_success_amount.ToString("#,##0") + " / " + daily_target_amount.ToString("#,##0");
            pbDailySuccessRate.Maximum = daily_target_amount;
            if (daily_success_amount > daily_target_amount)
                daily_success_amount = daily_target_amount;
            pbDailySuccessRate.Value = daily_success_amount;

            double daily_rate = 0;
            if (daily_target_amount > 0)
                daily_rate = (double)daily_success_amount / (double)daily_target_amount;
            if (double.IsNaN(daily_rate))
                daily_rate = 0;
            else if (double.IsInfinity(daily_rate))
                daily_rate = 1;

            lbDailySuccessRate.Text = (daily_rate * 100).ToString("#,##0") + "%";

            //월별 목표량
            DataTable monthlyDt = salesPartnerRepository.GetUserSaleDashboard(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"), selectRow.Cells["user_name"].Value.ToString());
            int monthly_success_amount = 0;
            if (monthlyDt != null && monthlyDt.Rows.Count > 0)
            {
                foreach (DataRow dr in monthlyDt.Rows)
                    monthly_success_amount += Convert.ToInt32(dr["am_cnt"].ToString()) + Convert.ToInt32(dr["pm_cnt"].ToString());
            }                

            txtMonthlySuccessRate.Text = monthly_success_amount.ToString("#,##0") + " / " + monthly_target_amount.ToString("#,##0");
            pbMonthlySuccessRate.Maximum = monthly_target_amount;
            if (monthly_success_amount > monthly_target_amount)
                monthly_success_amount = monthly_target_amount;
            pbMonthlySuccessRate.Value = monthly_success_amount;

            double monthly_rate = 0;
            if (monthly_target_amount > 0)
                monthly_rate = (double)monthly_success_amount / (double)monthly_target_amount;
            if (double.IsNaN(monthly_rate))
                monthly_rate = 0;
            else if (double.IsInfinity(monthly_rate))
                monthly_rate = 1;
            lbMonthlySuccessRate.Text = (monthly_rate * 100).ToString("#,##0") + "%";
        }

        private void GetSalesDashboard()
        {
            dgvSales.Rows.Clear();

            if (currentRow == null)
                return;

            DateTime sttdate, enddate;
            if (!DateTime.TryParse(txtSttdate.Text, out sttdate) || !DateTime.TryParse(txtEnddate.Text, out enddate))
            {
                messagebox.Show(this, "검색일자를 다시 확인해주세요.");
                return;
            }

            txtDailyTargetAmount.Text = currentRow.Cells["daily_work_goals_amount"].Value.ToString();
            txtUsedVacation.Text = currentRow.Cells["used_vacation_count"].Value.ToString(); 
            string user_name = currentRow.Cells["user_name"].Value.ToString();

            DataTable resultDt = salesPartnerRepository.GetUserSaleDashboard(txtSttdate.Text, txtEnddate.Text, user_name);
            if (resultDt.Rows.Count > 0)
            {
                double add_cnt= 0, total_cnt = 0, am_cnt = 0, pm_cnt = 0, potential1_cnt = 0, potential2_cnt = 0;
                for (DateTime tempDt = enddate; tempDt >= sttdate; tempDt = tempDt.AddDays(-1))
                {
                    int n = dgvSales.Rows.Add();
                    dgvSales.Rows[n].Cells["sales_date"].Value = tempDt.ToString("yyyy-MM-dd");
                    dgvSales.Rows[n].Cells["sales_day_of_week"].Value = GetDayOfWeek(tempDt);

                    string whr = $"updatetime = '{tempDt.ToString("yyyy-MM-dd")}'";
                    DataRow[] dr = resultDt.Select(whr);
                    if (dr.Length > 0)
                    {
                        add_cnt += Convert.ToDouble(dr[0]["add_cnt"].ToString());
                        dgvSales.Rows[n].Cells["add_cnt"].Value = Convert.ToDouble(dr[0]["add_cnt"].ToString()).ToString("#,##0");
                        total_cnt += Convert.ToDouble(dr[0]["am_cnt"].ToString()) + Convert.ToDouble(dr[0]["pm_cnt"].ToString());
                        dgvSales.Rows[n].Cells["total_cnt"].Value = (Convert.ToDouble(dr[0]["am_cnt"].ToString()) + Convert.ToDouble(dr[0]["pm_cnt"].ToString())).ToString("#,##0");
                        am_cnt += Convert.ToDouble(dr[0]["am_cnt"].ToString());
                        dgvSales.Rows[n].Cells["am_cnt"].Value = Convert.ToDouble(dr[0]["am_cnt"].ToString()).ToString("#,##0");
                        pm_cnt += Convert.ToDouble(dr[0]["pm_cnt"].ToString());
                        dgvSales.Rows[n].Cells["pm_cnt"].Value = Convert.ToDouble(dr[0]["pm_cnt"].ToString()).ToString("#,##0");
                        potential1_cnt += Convert.ToDouble(dr[0]["potential1_cnt"].ToString());
                        dgvSales.Rows[n].Cells["potential1_cnt"].Value = Convert.ToDouble(dr[0]["potential1_cnt"].ToString()).ToString("#,##0");
                        potential2_cnt += Convert.ToDouble(dr[0]["potential2_cnt"].ToString());
                        dgvSales.Rows[n].Cells["potential2_cnt"].Value = Convert.ToDouble(dr[0]["potential2_cnt"].ToString()).ToString("#,##0");
                    }
                }

                int m = dgvSales.Rows.Add();
                dgvSales.Rows[m].DefaultCellStyle.BackColor = Color.LightGray;
                dgvSales.Rows[m].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);

                dgvSales.Rows[m].Cells["sales_date"].Value = "총계";
                dgvSales.Rows[m].Cells["add_cnt"].Value = add_cnt.ToString("#,##0");
                dgvSales.Rows[m].Cells["total_cnt"].Value = total_cnt.ToString("#,##0");
                dgvSales.Rows[m].Cells["am_cnt"].Value = am_cnt.ToString("#,##0");
                dgvSales.Rows[m].Cells["pm_cnt"].Value = pm_cnt.ToString("#,##0");
                dgvSales.Rows[m].Cells["potential1_cnt"].Value = potential1_cnt.ToString("#,##0");
                dgvSales.Rows[m].Cells["potential2_cnt"].Value = potential2_cnt.ToString("#,##0");

                VacationSuccessRate(currentRow);
            }
        }


        private string GetDayOfWeek(DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Monday)
                return "월";
            else if (dt.DayOfWeek == DayOfWeek.Tuesday)
                return "화";
            else if (dt.DayOfWeek == DayOfWeek.Wednesday)
                return "수";
            else if (dt.DayOfWeek == DayOfWeek.Thursday)
                return "목";
            else if (dt.DayOfWeek == DayOfWeek.Friday)
                return "금";
            else if (dt.DayOfWeek == DayOfWeek.Saturday)
                return "토";
            else if (dt.DayOfWeek == DayOfWeek.Sunday)
                return "일";

            return "";
        }
        /*public void RefreshTable()
        {
            //매출처 정보
            DataTable companyDt = salesPartnerRepository.GetSalesPartner2("", "", false, ""
                                            , "", "", " ", true, false, false, false, false, false, false, false, "");

            //최근영업내용
            DataTable salesDt = salesPartnerRepository.GetCurrentCompanySales();


            //씨오버 거래정보 + 아토전산 거래처
            var atoTemp = from p in companyDt.AsEnumerable()
                          join t in salesDt.AsEnumerable()
                          on p.Field<Int32>("id") equals t.Field<Int32>("company_id")
                          into outer
                          from t in outer.DefaultIfEmpty()
                          select new
                          {
                              id = p.Field<Int32>("id"),
                              group_name = p.Field<string>("group_name"),
                              company = p.Field<string>("name"),
                              seaover_company_code = p.Field<string>("seaover_company_code"),
                              tel = p.Field<string>("tel"),
                              fax = p.Field<string>("fax"),
                              phone = p.Field<string>("phone"),
                              other_phone = p.Field<string>("other_phone"),
                              ceo = p.Field<string>("ceo"),
                              registration_number = p.Field<string>("registration_number"),
                              address = p.Field<string>("address"),
                              origin = p.Field<string>("origin"),
                              company_manager = p.Field<string>("company_manager"),
                              company_manager_position = p.Field<string>("company_manager_position"),
                              email = p.Field<string>("email"),
                              web = p.Field<string>("web"),
                              sns1 = p.Field<string>("sns1"),
                              sns2 = p.Field<string>("sns2"),
                              sns3 = p.Field<string>("sns3"),

                              isManagement1 = p.Field<string>("isManagement1"),
                              isManagement2 = p.Field<string>("isManagement2"),
                              isManagement3 = p.Field<string>("isManagement3"),
                              isManagement4 = p.Field<string>("isManagement4"),
                              isHide = p.Field<string>("isHide"),
                              createtime = p.Field<string>("createtime"),

                              ato_manager = p.Field<string>("ato_manager"),
                              distribution = p.Field<string>("distribution"),
                              handling_item = p.Field<string>("handling_item"),
                              remark = p.Field<string>("remark"),
                              remark2 = p.Field<string>("remark2"),
                              isPotential1 = p.Field<string>("isPotential1"),
                              isPotential2 = p.Field<string>("isPotential2"),
                              isTrading = p.Field<string>("isTrading"),

                              current_sale_date = "",
                              current_sale_manager = "",

                              isNonHandled = p.Field<string>("isNonHandled"),
                              isOutBusiness = p.Field<string>("isOutBusiness"),
                              isNotSendFax = p.Field<string>("isNotSendFax"),

                              sales_updatetime = (t == null) ? "" : t.Field<string>("updatetime"),
                              sales_edit_user = (t == null) ? "" : t.Field<string>("edit_user"),
                              sales_contents = (t == null) ? "" : t.Field<string>("contents"),
                              sales_log = (t == null) ? "" : t.Field<string>("log"),
                              sales_remark = (t == null) ? "" : t.Field<string>("remark"),
                              division = "ATO",

                              duplicate_common_count = 0,
                              duplicate_myData_count = 0,
                              duplicate_potential1_count = 0,
                              duplicate_potential2_count = 0,
                              duplicate_trading_count = 0,
                              duplicate_nonHandled_count = 0,
                              duplicate_notSendFax_count = 0,
                              duplicate_outBusiness_count = 0,
                              duplicate_result = "",

                              isDelete = p.Field<string>("isDelete"),

                              industry_type = p.Field<string>("industry_type"),
                              industry_type_rank = 0,

                              main_id = 0,
                              sub_id = 0
                          };
            companyDt = ConvertListToDatatable(atoTemp);
            companyDt.AcceptChanges();

            //거래처별 그룹
            DataTable groupDt = companyGroupRepository.GetCompanyGroup();

            //아토 전산에만 저장된 거래처정보
            DataRow[] atoCompanyDt = companyDt.Select("seaover_company_code = ''");
            atoDt = new DataTable();
            if (atoCompanyDt.Length > 0)
                atoDt = atoCompanyDt.CopyToDataTable();

            //씨오버 거래처 
            DataTable seaDt = salesRepository.GetSaleCompany("", false, "", "", "", "", "", "", false, false);
            //씨오버 거래정보 + 아토전산 거래처
            var seaoverTemp = from p in seaDt.AsEnumerable()
                              join t in companyDt.AsEnumerable()
                              on p.Field<string>("거래처코드") equals t.Field<string>("seaover_company_code")
                              into outer
                              from t in outer.DefaultIfEmpty()
                              select new
                              {
                                  id = (t == null) ? 0 : t.Field<Int32>("id"),
                                  group_name = (t == null) ? "" : t.Field<string>("group_name"),
                                  company = p.Field<string>("거래처명"),
                                  seaover_company_code = p.Field<string>("거래처코드"),
                                  tel = p.Field<string>("전화번호"),
                                  fax = p.Field<string>("팩스번호"),
                                  phone = p.Field<string>("휴대폰"),
                                  other_phone = p.Field<string>("기타연락처"),
                                  ceo = p.Field<string>("대표자명"),
                                  registration_number = p.Field<string>("사업자번호"),
                                  address = (t == null) ? "" : t.Field<string>("address"),

                                  origin = (t == null) ? "" : t.Field<string>("origin"),
                                  company_manager = (t == null) ? "" : t.Field<string>("company_manager"),
                                  company_manager_position = (t == null) ? "" : t.Field<string>("company_manager_position"),
                                  email = (t == null) ? "" : t.Field<string>("email"),
                                  web = (t == null) ? "" : t.Field<string>("web"),
                                  sns1 = (t == null) ? "" : t.Field<string>("sns1"),
                                  sns2 = (t == null) ? "" : t.Field<string>("sns2"),
                                  sns3 = (t == null) ? "" : t.Field<string>("sns3"),

                                  isManagement1 = (t == null) ? "FALSE" : t.Field<string>("isManagement1"),
                                  isManagement2 = (t == null) ? "FALSE" : t.Field<string>("isManagement2"),
                                  isManagement3 = (t == null) ? "FALSE" : t.Field<string>("isManagement3"),
                                  isManagement4 = (t == null) ? "FALSE" : t.Field<string>("isManagement4"),
                                  isHide = (t == null) ? "FALSE" : t.Field<string>("isHide"),
                                  createtime = (t == null) ? DateTime.Now.ToString("yyyy-MM-dd") : t.Field<string>("createtime"),

                                  ato_manager = (t == null) ? p.Field<string>("매출자") : t.Field<string>("ato_manager"),
                                  distribution = (t == null) ? "" : t.Field<string>("distribution"),
                                  handling_item = (t == null) ? "" : t.Field<string>("handling_item"),
                                  remark = p.Field<string>("참고사항"),
                                  remark2 = (t == null) ? "" : t.Field<string>("remark2"),
                                  isPotential1 = (t == null) ? "FALSE" : t.Field<string>("isPotential1"),
                                  isPotential2 = (t == null) ? "FALSE" : t.Field<string>("isPotential2"),
                                  isTrading = (t == null) ? "TRUE" : t.Field<string>("isTrading"),
                                  current_sale_date = p.Field<string>("최종매출일자"),
                                  current_sale_manager = p.Field<string>("매출자"),

                                  isNonHandled = (t == null) ? "" : t.Field<string>("isNonHandled"),
                                  isOutBusiness = (t == null) ? p.Field<string>("폐업유무") : t.Field<string>("isOutBusiness"),
                                  isNotSendFax = (t == null) ? "" : t.Field<string>("isNotSendFax"),
                                  sales_updatetime = (t == null) ? "" : t.Field<string>("sales_updatetime"),
                                  sales_edit_user = (t == null) ? "" : t.Field<string>("sales_edit_user"),
                                  sales_contents = (t == null) ? "" : t.Field<string>("sales_contents"),
                                  sales_log = (t == null) ? "" : t.Field<string>("sales_log"),
                                  sales_remark = (t == null) ? "" : t.Field<string>("sales_remark"),
                                  division = "SEAOVER",

                                  duplicate_common_count = 0,
                                  duplicate_myData_count = 0,
                                  duplicate_potential1_count = 0,
                                  duplicate_potential2_count = 0,
                                  duplicate_trading_count = 0,
                                  duplicate_nonHandled_count = 0,
                                  duplicate_notSendFax_count = 0,
                                  duplicate_outBusiness_count = 0,
                                  duplicate_result = "",

                                  isDelete = (t == null) ? "FALSE" : t.Field<string>("isDelete"),

                                  industry_type = (t == null) ? "" : t.Field<string>("industry_type"),
                                  industry_type_rank = 0

                              };
            DataTable seaoverDt = ConvertListToDatatable(seaoverTemp);
            if (seaoverDt != null && seaoverDt.Rows.Count > 0)
            {
                //그룹별 거래처
                var seaoverTemp2 = from p in seaoverDt.AsEnumerable()
                                   join t in groupDt.AsEnumerable()
                                   on p.Field<string>("seaover_company_code") equals t.Field<string>("company_id")
                                   into outer
                                   from t in outer.DefaultIfEmpty()
                                   select new
                                   {
                                       id = p.Field<Int32>("id"),
                                       group_name = p.Field<string>("group_name"),
                                       company = p.Field<string>("company"),
                                       seaover_company_code = p.Field<string>("seaover_company_code"),
                                       tel = p.Field<string>("tel"),
                                       fax = p.Field<string>("fax"),
                                       phone = p.Field<string>("phone"),
                                       other_phone = p.Field<string>("other_phone"),
                                       ceo = p.Field<string>("ceo"),
                                       registration_number = p.Field<string>("registration_number"),
                                       address = p.Field<string>("address"),

                                       origin = p.Field<string>("origin"),
                                       company_manager = p.Field<string>("company_manager"),
                                       company_manager_position = p.Field<string>("company_manager_position"),
                                       email = p.Field<string>("email"),
                                       web = p.Field<string>("web"),
                                       sns1 = p.Field<string>("sns1"),
                                       sns2 = p.Field<string>("sns2"),
                                       sns3 = p.Field<string>("sns3"),

                                       isManagement1 = p.Field<string>("isManagement1"),
                                       isManagement2 = p.Field<string>("isManagement2"),
                                       isManagement3 = p.Field<string>("isManagement3"),
                                       isManagement4 = p.Field<string>("isManagement4"),
                                       isHide = p.Field<string>("isHide"),
                                       createtime = p.Field<string>("createtime"),

                                       ato_manager = p.Field<string>("ato_manager"),
                                       distribution = p.Field<string>("distribution"),
                                       handling_item = p.Field<string>("handling_item"),
                                       remark = p.Field<string>("remark"),
                                       remark2 = p.Field<string>("remark2"),
                                       isPotential1 = p.Field<string>("isPotential1"),
                                       isPotential2 = p.Field<string>("isPotential2"),
                                       isTrading = p.Field<string>("isTrading"),
                                       current_sale_date = p.Field<string>("current_sale_date"),
                                       current_sale_manager = p.Field<string>("current_sale_manager"),

                                       isNonHandled = p.Field<string>("isNonHandled"),
                                       isOutBusiness = p.Field<string>("isOutBusiness"),
                                       isNotSendFax = p.Field<string>("isNotSendFax"),
                                       sales_updatetime = p.Field<string>("sales_updatetime"),
                                       sales_edit_user = p.Field<string>("sales_edit_user"),
                                       sales_contents = p.Field<string>("sales_contents"),
                                       sales_log = p.Field<string>("sales_log"),
                                       sales_remark = p.Field<string>("sales_remark"),
                                       division = p.Field<string>("division"),

                                       duplicate_common_count = p.Field<Int32>("duplicate_common_count"),
                                       duplicate_myData_count = p.Field<Int32>("duplicate_myData_count"),
                                       duplicate_potential1_count = p.Field<Int32>("duplicate_potential1_count"),
                                       duplicate_potential2_count = p.Field<Int32>("duplicate_potential2_count"),
                                       duplicate_trading_count = p.Field<Int32>("duplicate_trading_count"),
                                       duplicate_nonHandled_count = p.Field<Int32>("duplicate_nonHandled_count"),
                                       duplicate_notSendFax_count = p.Field<Int32>("duplicate_notSendFax_count"),
                                       duplicate_outBusiness_count = p.Field<Int32>("duplicate_outBusiness_count"),
                                       duplicate_result = p.Field<string>("duplicate_result"),

                                       isDelete = p.Field<string>("isDelete"),

                                       industry_type = p.Field<string>("industry_type"),
                                       industry_type_rank = 0,

                                       main_id = (t == null) ? 0 : t.Field<Int32>("main_id"),
                                       sub_id = (t == null) ? 0 : t.Field<Int32>("sub_id")
                                   };
                seaoverDt = ConvertListToDatatable(seaoverTemp2);

                for (int i = 0; i < seaoverDt.Rows.Count; i++)
                {
                    //삭제거래처
                    bool isDelete;
                    if (!bool.TryParse(seaoverDt.Rows[i]["isDelete"].ToString(), out isDelete))
                        isDelete = false;
                    if (!isDelete)
                    {
                        // ** 매출 || 영업중 먼저인 내역이 우선 **

                        //최근 매출정보
                        DateTime current_sale_date;
                        if (!DateTime.TryParse(seaoverDt.Rows[i]["current_sale_date"].ToString(), out current_sale_date))
                            current_sale_date = new DateTime(1900, 1, 1);

                        //최근 영업정보
                        DateTime sales_updatetime;
                        if (!DateTime.TryParse(seaoverDt.Rows[i]["sales_updatetime"].ToString(), out sales_updatetime))
                            sales_updatetime = new DateTime(1900, 1, 1);

                        //2023-09-13
                        //시간까지 체크하니 테스트가 안됨 그날 매출달면 그냥 바로 거래중으로 적용
                        current_sale_date = new DateTime(current_sale_date.Year, current_sale_date.Month, current_sale_date.Day);
                        sales_updatetime = new DateTime(sales_updatetime.Year, sales_updatetime.Month, sales_updatetime.Day);

                        string sale_edit_user;
                        string sale_date;
                        string sale_contents;
                        string sale_remark;
                        string ato_manager;
                        if (current_sale_date >= sales_updatetime)
                        {
                            seaoverDt.Rows[i]["isTrading"] = "TRUE";
                            ato_manager = seaoverDt.Rows[i]["current_sale_manager"].ToString();
                            sale_edit_user = seaoverDt.Rows[i]["current_sale_manager"].ToString();
                            sale_contents = "씨오버 매출";
                            sale_date = current_sale_date.ToString("yyyy-MM-dd HH:mm:ss");
                            sale_remark = "";
                        }
                        else
                        {
                            ato_manager = seaoverDt.Rows[i]["ato_manager"].ToString();
                            sale_edit_user = seaoverDt.Rows[i]["sales_edit_user"].ToString();
                            sale_contents = seaoverDt.Rows[i]["sales_contents"].ToString();
                            *//*if (sale_contents == "씨오버 매출")
                                sale_contents = seaoverDt1.Rows[i]["sales_log"].ToString();*//*
                            sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                            sale_remark = seaoverDt.Rows[i]["sales_remark"].ToString();
                        }
                        //최신화
                        seaoverDt.Rows[i]["ato_manager"] = ato_manager;
                        seaoverDt.Rows[i]["sales_updatetime"] = sale_date;
                        seaoverDt.Rows[i]["sales_edit_user"] = sale_edit_user;
                        seaoverDt.Rows[i]["sales_contents"] = sale_contents;
                        seaoverDt.Rows[i]["sales_remark"] = sale_remark;
                        //거래처코드가 없는데 거래중은 자동삭제
                        bool trading;
                        if (!bool.TryParse(seaoverDt.Rows[i]["isTrading"].ToString(), out trading))
                            trading = false;
                        if (string.IsNullOrEmpty(seaoverDt.Rows[i]["seaover_company_code"].ToString()) && trading)
                            seaoverDt.Rows[i]["isDelete"] = "FALSE";
                    }
                }
                seaoverDt.AcceptChanges();

                //대표거래처 최종수정일 최신화
                for (int i = 0; i < seaoverDt.Rows.Count; i++)
                {
                    //대표거래처 최종수정일 최신화
                    int main_id;
                    if (!int.TryParse(seaoverDt.Rows[i]["main_id"].ToString(), out main_id))
                        main_id = 0;
                    int sub_id;
                    if (!int.TryParse(seaoverDt.Rows[i]["sub_id"].ToString(), out sub_id))
                        sub_id = 0;
                    if (main_id > 0 && sub_id == 0)
                    {
                        //최근 영업정보
                        DateTime sales_updatetime;
                        if (!DateTime.TryParse(seaoverDt.Rows[i]["sales_updatetime"].ToString(), out sales_updatetime))
                            sales_updatetime = new DateTime(1900, 1, 1);

                        string ato_manager = seaoverDt.Rows[i]["ato_manager"].ToString();
                        string sale_edit_user = seaoverDt.Rows[i]["sales_edit_user"].ToString();
                        string sale_contents = seaoverDt.Rows[i]["sales_contents"].ToString();
                        *//*if (sale_contents == "씨오버 매출")
                            sale_contents = seaoverDt1.Rows[i]["sales_log"].ToString();*//*
                        string sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                        string sale_remark = seaoverDt.Rows[i]["sales_remark"].ToString();

                        DataRow[] subDr = seaoverDt.Select($"main_id = {main_id} AND isDelete = 'FALSE'");
                        for (int j = 0; j < subDr.Length; j++)
                        {
                            DateTime tempUpdatetime;
                            if (DateTime.TryParse(subDr[j]["sales_updatetime"].ToString(), out tempUpdatetime))
                            {
                                if (sales_updatetime < tempUpdatetime)
                                {
                                    sales_updatetime = tempUpdatetime;
                                    ato_manager = subDr[j]["ato_manager"].ToString();
                                    sale_edit_user = subDr[j]["sales_edit_user"].ToString();
                                    sale_contents = subDr[j]["sales_contents"].ToString();
                                    *//*if (sale_contents == "씨오버 매출")
                                        sale_contents = seaoverDt1.Rows[i]["sales_log"].ToString();*//*
                                    sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                                    sale_remark = subDr[j]["sales_remark"].ToString();
                                }
                            }
                        }
                        //최신화
                        seaoverDt.Rows[i]["ato_manager"] = ato_manager;
                        seaoverDt.Rows[i]["sales_updatetime"] = sale_date;
                        seaoverDt.Rows[i]["sales_edit_user"] = sale_edit_user;
                        seaoverDt.Rows[i]["sales_contents"] = sale_contents;
                        seaoverDt.Rows[i]["sales_remark"] = sale_remark;

                    }
                }

            }
            seaoverDt.AcceptChanges();
            atoDt.Merge(seaoverDt);
            atoDt.Columns.Add("table_index", typeof(Int32));
            atoDt.AcceptChanges();
            for (int i = 0; i < atoDt.Rows.Count; i++)
                atoDt.Rows[i]["table_index"] = i;

            //초기화
            mainDt = new DataTable();
            DataRow[] mainDr = atoDt.Select("ato_manager = '' AND isNonHandled = 'FALSE' AND isOutBusiness = 'FALSE' AND isDelete = 'FALSE' AND isHide = 'FALSE'");
            if (mainDr.Length > 0)
                mainDt = mainDr.CopyToDataTable();

        }*/
        public static DataTable ConvertListToDatatable(IEnumerable<dynamic> v)
        {
            var firstRecord = v.FirstOrDefault();
            if (firstRecord == null)
                return null;

            PropertyInfo[] infos = firstRecord.GetType().GetProperties();

            DataTable table = new DataTable();
            foreach (var info in infos)
            {
                Type propType = info.PropertyType;

                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    table.Columns.Add(info.Name, Nullable.GetUnderlyingType(propType));
                else
                    table.Columns.Add(info.Name, info.PropertyType);
            }

            DataRow row;

            foreach (var record in v)
            {
                row = table.NewRow();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    row[i] = infos[i].GetValue(record) != null ? infos[i].GetValue(record) : DBNull.Value;
                }
                table.Rows.Add(row);
            }
            table.AcceptChanges();
            return table;
        }
        Libs.Tools.Common common = new Libs.Tools.Common();
        private void GetEmployee()
        {
            dgvEmplyee.Rows.Clear();
            //영업일
            int work_days;
            common.GetWorkDay(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
                            , new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1)
                            , out work_days);
            //유저 데이터
            DataTable employeeDt = usersRepository.GetUsers("", txtDepartment.Text, txtTeam.Text, txtEmplyeeName.Text, "승인", "");
            if (employeeDt.Rows.Count > 0)
            {
                this.dgvEmplyee.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEmplyee_CellValueChanged);

                int errIdx;
                try
                {
                    for (int i = 0; i < employeeDt.Rows.Count; i++)
                    {
                        errIdx = i;
                        int n = dgvEmplyee.Rows.Add();
                        dgvEmplyee.Rows[n].Cells["user_id"].Value = employeeDt.Rows[i]["user_id"].ToString();
                        dgvEmplyee.Rows[n].Cells["user_name"].Value = employeeDt.Rows[i]["user_name"].ToString();
                        dgvEmplyee.Rows[n].Cells["department"].Value = employeeDt.Rows[i]["department"].ToString();
                        dgvEmplyee.Rows[n].Cells["team"].Value = employeeDt.Rows[i]["team"].ToString();

                        double used_days = Convert.ToDouble(employeeDt.Rows[i]["used_days"].ToString());
                        int daily_work_goals_amount = Convert.ToInt32(employeeDt.Rows[i]["daily_work_goals_amount"].ToString());
                        int daily_sales_cnt = Convert.ToInt32(employeeDt.Rows[i]["daily_sales_cnt"].ToString());
                        int monthly_sales_cnt = Convert.ToInt32(employeeDt.Rows[i]["monthly_sales_cnt"].ToString());

                        dgvEmplyee.Rows[n].Cells["used_vacation_count"].Value = used_days;
                        dgvEmplyee.Rows[n].Cells["daily_work_goals_amount"].Value = daily_work_goals_amount;
                        dgvEmplyee.Rows[n].Cells["daily_sales_cnt"].Value = daily_sales_cnt;
                        dgvEmplyee.Rows[n].Cells["monthly_sales_cnt"].Value = monthly_sales_cnt;

                        //일 목표량 
                        double daily_success_rate = 0;
                        if (daily_work_goals_amount > 0)
                        {
                            daily_success_rate = (double)daily_sales_cnt / (double)daily_work_goals_amount;
                            if (double.IsNaN(daily_success_rate))
                                daily_success_rate = 0;
                            else if (double.IsInfinity(daily_success_rate))
                                daily_success_rate = 1;
                            else if (daily_success_rate > 1)
                                daily_success_rate = 1;
                            daily_success_rate *= 100;
                        }
                        dgvEmplyee.Rows[n].Cells["daily_success_rate"].Value = (int)daily_success_rate;
                        //월목표량
                        double monthly_success_rate = 0;
                        if (daily_work_goals_amount * (work_days - used_days) > 0)
                        {
                            monthly_success_rate = (double)monthly_sales_cnt / (double)(daily_work_goals_amount * (work_days - used_days));
                            if (double.IsNaN(monthly_success_rate))
                                monthly_success_rate = 0;
                            else if (double.IsInfinity(monthly_success_rate))
                                monthly_success_rate = 1;
                            else if (monthly_success_rate > 1)
                                monthly_success_rate = 1;
                            monthly_success_rate *= 100;
                        }
                        dgvEmplyee.Rows[n].Cells["monthly_success_rate"].Value = (int)monthly_success_rate;
                    }
                }
                catch
                {
                    messagebox.Show(this, "11");
                }
                this.dgvEmplyee.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEmplyee_CellValueChanged);
            }

            GetCompanyCount();
        }
        private void GetCompanyCount()
        {
            //매출처 정보
            DataTable companyDt = salesPartnerRepository.GetSalesPartner("", "", false, "", "", "", " ", true, false, false, false, false, false, false, false, "");

            //씨오버 거래처 
            DataTable seaoverDt = salesRepository.GetSaleCompany("", false, "", "", "", "", "", "", true, true);

            //아토 전산에만 저장된 거래처정보
            DataRow[] atoCompanyDt = companyDt.Select("seaover_company_code = '' AND isDelete = 'FALSE'");
            DataTable noSeaoverDt = new DataTable();
            if (atoCompanyDt.Length > 0)
                noSeaoverDt = atoCompanyDt.CopyToDataTable();

            //씨오버 거래정보 + 아토전산 거래처
            var seaoverTemp = from p in seaoverDt.AsEnumerable()
                              join t in companyDt.AsEnumerable()
                              on p.Field<string>("거래처코드") equals t.Field<string>("seaover_company_code")
                              into outer
                              from t in outer.DefaultIfEmpty()
                              select new
                              {
                                  id = (t == null) ? 0 : t.Field<Int32>("id"),
                                  group_name = (t == null) ? "" : t.Field<string>("group_name"),
                                  company = p.Field<string>("거래처명"),
                                  seaover_company_code = p.Field<string>("거래처코드"),
                                  tel = p.Field<string>("전화번호"),
                                  fax = p.Field<string>("팩스번호"),
                                  phone = p.Field<string>("휴대폰"),
                                  ceo = p.Field<string>("대표자명"),
                                  registration_number = p.Field<string>("사업자번호"),
                                  address = (t == null) ? "" : t.Field<string>("address"),

                                  origin = (t == null) ? "" : t.Field<string>("origin"),
                                  company_manager = (t == null) ? "" : t.Field<string>("company_manager"),
                                  company_manager_position = (t == null) ? "" : t.Field<string>("company_manager_position"),
                                  email = (t == null) ? "" : t.Field<string>("email"),
                                  web = (t == null) ? "" : t.Field<string>("web"),
                                  sns1 = (t == null) ? "" : t.Field<string>("sns1"),
                                  sns2 = (t == null) ? "" : t.Field<string>("sns2"),
                                  sns3 = (t == null) ? "" : t.Field<string>("sns3"),

                                  isManagement1 = (t == null) ? "false" : t.Field<string>("isManagement1"),
                                  isManagement2 = (t == null) ? "false" : t.Field<string>("isManagement2"),
                                  isManagement3 = (t == null) ? "false" : t.Field<string>("isManagement3"),
                                  isManagement4 = (t == null) ? "false" : t.Field<string>("isManagement4"),
                                  isHide = (t == null) ? "false" : t.Field<string>("isHide"),
                                  createtime = (t == null) ? DateTime.Now.ToString("yyyy-MM-dd") : t.Field<string>("createtime"),

                                  ato_manager = (t == null) ? p.Field<string>("매출자") : t.Field<string>("ato_manager"),
                                  distribution = (t == null) ? "" : t.Field<string>("distribution"),
                                  handling_item = (t == null) ? "" : t.Field<string>("handling_item"),
                                  remark = p.Field<string>("참고사항"),
                                  remark2 = (t == null) ? "" : t.Field<string>("remark2"),
                                  isPotential1 = (t == null) ? "false" : t.Field<string>("isPotential1"),
                                  isPotential2 = (t == null) ? "false" : t.Field<string>("isPotential2"),
                                  isTrading = (t == null) ? "true" : t.Field<string>("isTrading"),
                                  current_sale_date = p.Field<string>("최종매출일자"),
                                  current_sale_manager = p.Field<string>("매출자"),

                                  isNonHandled = (t == null) ? "false" : t.Field<string>("isNonHandled"),
                                  isOutBusiness = (t == null) ? p.Field<string>("폐업유무") : t.Field<string>("isOutBusiness"),
                                  isNotSendFax = (t == null) ? "false" : t.Field<string>("isNotSendFax"),
                                  sales_updatetime = (t == null) ? "" : t.Field<string>("sales_updatetime"),
                                  sales_edit_user = (t == null) ? "" : t.Field<string>("sales_edit_user"),
                                  sales_contents = (t == null) ? "" : t.Field<string>("sales_contents"),
                                  sales_log = (t == null) ? "" : t.Field<string>("sales_log"),
                                  sales_remark = (t == null) ? "" : t.Field<string>("sales_remark"),

                                  isDelete = (t == null) ? "FALSE" : t.Field<string>("isDelete")
                              };
            DataTable seaoverDt1 = ConvertListToDatatable(seaoverTemp);
            for (int i = 0; i < seaoverDt1.Rows.Count; i++)
            {
                //최근 매출정보
                DateTime current_sale_date;
                if (!DateTime.TryParse(seaoverDt1.Rows[i]["current_sale_date"].ToString(), out current_sale_date))
                    current_sale_date = new DateTime(1900, 1, 1);

                //최근 영업정보
                DateTime sales_updatetime;
                if (!DateTime.TryParse(seaoverDt1.Rows[i]["sales_updatetime"].ToString(), out sales_updatetime))
                    sales_updatetime = new DateTime(1900, 1, 1);

                string sale_edit_user;
                string sale_date;
                string sale_contents;
                string sale_remark;
                string ato_manager;

                if (current_sale_date >= sales_updatetime)
                {
                    seaoverDt1.Rows[i]["isTrading"] = "true";
                    ato_manager = seaoverDt1.Rows[i]["current_sale_manager"].ToString();
                    sale_edit_user = seaoverDt1.Rows[i]["current_sale_manager"].ToString();
                    sale_contents = "씨오버 매출";
                    sale_date = current_sale_date.ToString("yyyy-MM-dd HH:mm:ss");
                    sale_remark = "";
                }
                else
                {
                    ato_manager = seaoverDt1.Rows[i]["ato_manager"].ToString();
                    sale_edit_user = seaoverDt1.Rows[i]["sales_edit_user"].ToString();
                    sale_contents = seaoverDt1.Rows[i]["sales_contents"].ToString();
                    if (sale_contents == "씨오버 매출")
                        sale_contents = seaoverDt1.Rows[i]["sales_log"].ToString();
                    sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                    sale_remark = seaoverDt1.Rows[i]["sales_remark"].ToString();
                }

                seaoverDt1.Rows[i]["sales_edit_user"] = sale_edit_user;
                //seaoverDt1.Rows[i]["sales_date"] = sale_date;
                //seaoverDt1.Rows[i]["sales_contents"] = sale_contents;
                //seaoverDt1.Rows[i]["sales_remark"] = sale_remark;
                seaoverDt1.Rows[i]["ato_manager"] = ato_manager;
            }
            seaoverDt1.AcceptChanges();


            //출력
            string whr;
            DataRow[] noSeaoverDr, seaoverDr;
            for (int i = 0; i < dgvEmplyee.Rows.Count; i++)
            {
                //무작위(아토)=======================================================================================================
                int random_count = 0;
                if (noSeaoverDt != null && noSeaoverDt.Rows.Count > 0)
                {
                    whr = "isPotential1 = 'false' AND isPotential2 = 'false' "
                        + "  AND isNonHandled = 'false' AND isOutBusiness = 'false' AND isNotSendFax = 'false' "
                        + $" AND isTrading = 'false' AND ato_manager = '{dgvEmplyee.Rows[i].Cells["user_name"].Value.ToString()}'";
                    noSeaoverDr = noSeaoverDt.Select(whr);
                    random_count += noSeaoverDr.Length;
                }

                //무작위(씨오버)
                if (seaoverDt1 != null && seaoverDt1.Rows.Count > 0)
                {
                    whr = "isDelete = 'false'                                       "
                         + "  AND isPotential1 = 'false' AND isPotential2 = 'false' "
                         + "  AND isNonHandled = 'false' AND isOutBusiness = 'false' AND isNotSendFax = 'false' "
                         + $" AND isTrading = 'false' AND ato_manager = '{dgvEmplyee.Rows[i].Cells["user_name"].Value.ToString()}'";

                    seaoverDr = seaoverDt1.Select(whr);
                    random_count += seaoverDr.Length;
                }
                dgvEmplyee.Rows[i].Cells["random_count"].Value = random_count.ToString("#,##0");

                //잠재1(아토)=======================================================================================================
                int potential1_count = 0;
                if (noSeaoverDt != null && noSeaoverDt.Rows.Count > 0)
                {
                    whr = "isDelete = 'false' AND seaover_company_code = '' "
                        + "  AND isPotential1 = 'true' AND isPotential2 = 'false' "
                        + "  AND isNonHandled = 'false' AND isOutBusiness = 'false' AND isNotSendFax = 'false' "
                        + $" AND isTrading = 'false' AND ato_manager = '{dgvEmplyee.Rows[i].Cells["user_name"].Value.ToString()}'";
                    noSeaoverDr = noSeaoverDt.Select(whr);
                    potential1_count += noSeaoverDr.Length;
                }

                //잠재1(씨오버)
                if (seaoverDt1 != null && seaoverDt1.Rows.Count > 0)
                {
                    whr = "isDelete = 'false'  "
                         + "  AND isPotential1 = 'true' AND isPotential2 = 'false' "
                         + "  AND isNonHandled = 'false' AND isOutBusiness = 'false' AND isNotSendFax = 'false' "
                         + $" AND isTrading = 'false' AND ato_manager = '{dgvEmplyee.Rows[i].Cells["user_name"].Value.ToString()}'";

                    seaoverDr = seaoverDt1.Select(whr);
                    potential1_count += seaoverDr.Length;
                }
                dgvEmplyee.Rows[i].Cells["potential1_count"].Value = potential1_count.ToString("#,##0");

                //잠재2(아토)=======================================================================================================
                int potential2_count = 0;
                if (noSeaoverDt != null && noSeaoverDt.Rows.Count > 0)
                {
                    whr = "isDelete = 'false' AND seaover_company_code = '' "
                        + "  AND isPotential1 = 'false' AND isPotential2 = 'true' "
                        + "  AND isNonHandled = 'false' AND isOutBusiness = 'false' AND isNotSendFax = 'false' "
                        + $" AND isTrading = 'false' AND ato_manager = '{dgvEmplyee.Rows[i].Cells["user_name"].Value.ToString()}'";
                    noSeaoverDr = noSeaoverDt.Select(whr);
                    potential2_count += noSeaoverDr.Length;
                }

                //잠재2(씨오버)
                if (seaoverDt1 != null && seaoverDt1.Rows.Count > 0)
                {
                    whr = "isDelete = 'false' "
                         + "  AND isPotential1 = 'false' AND isPotential2 = 'true' "
                         + "  AND isNonHandled = 'false' AND isOutBusiness = 'false' AND isNotSendFax = 'false' "
                         + $" AND isTrading = 'false' AND ato_manager = '{dgvEmplyee.Rows[i].Cells["user_name"].Value.ToString()}'";

                    seaoverDr = seaoverDt1.Select(whr);
                    potential2_count += seaoverDr.Length;
                }
                dgvEmplyee.Rows[i].Cells["potential2_count"].Value = potential2_count.ToString("#,##0");

                //거래중(씨오버)=======================================================================================================
                int trading_count = 0;
                if (seaoverDt1 != null && seaoverDt1.Rows.Count > 0)
                {
                    whr = "isDelete = 'false'  "
                         + "  AND isNonHandled = 'false' AND isOutBusiness = 'false' AND isNotSendFax = 'false' "
                         + $" AND isTrading = 'true' AND ato_manager = '{dgvEmplyee.Rows[i].Cells["user_name"].Value.ToString()}'";

                    seaoverDr = seaoverDt1.Select(whr);
                    trading_count += seaoverDr.Length;
                }
                dgvEmplyee.Rows[i].Cells["trading_count"].Value = trading_count.ToString("#,##0");
            }

        }
        #endregion

        #region Button
        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetEmployee();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSttdateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtSttdate.Text = tmpDate.ToString("yyyy-MM-dd");
                if (currentRow != null)
                {
                    GetSalesDashboard();
                    VacationSuccessRate(currentRow);
                }
            }
        }

        private void btnEnddateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtEnddate.Text = tmpDate.ToString("yyyy-MM-dd");
                if (currentRow != null)
                {
                    GetSalesDashboard();
                    VacationSuccessRate(currentRow);
                }
            }
        }
        #endregion

        #region Key event
        private void txtSttdate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (currentRow != null)
                {
                    GetSalesDashboard();
                    VacationSuccessRate(currentRow);
                }
            }
        }
        private void txtDepartment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetEmployee();
        }

        private void SalesManagerDashboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { 
                switch(e.KeyCode) 
                {
                    case Keys.Q:
                        btnSearching.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.M:
                        txtDepartment.Focus();
                        break;
                    case Keys.N:
                        txtDepartment.Text = string.Empty;
                        txtTeam.Text = string.Empty;
                        txtEmplyeeName.Text = string.Empty;
                        txtDepartment.Focus();
                        break;
                }
            }
        }
        #endregion

        #region Datagridview event

        private void dgvEmplyee_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                currentRow = dgvEmplyee.Rows[e.RowIndex];
                GetSalesDashboard();
            }
        }
        private void dgvSales_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 & e.ColumnIndex >= 0)
            {
                if (dgvSales.Columns[e.ColumnIndex].Name == "sales_day_of_week")
                {
                    if (dgvSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && (dgvSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "토"
                        || dgvSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "일"))
                        dgvSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
                    else
                        dgvSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                }
            }
        }
        private void dgvEmplyee_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvEmplyee.Columns[e.ColumnIndex].Name == "btnSelect")
                {
                    currentRow = dgvEmplyee.Rows[e.RowIndex];
                    GetSalesDashboard();
                    VacationSuccessRate(currentRow);
                }
            }
        }
        #endregion

        private void txtDailyTargetAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (int.TryParse(txtDailyTargetAmount.Text, out int daily_target_amont))
                {
                    if (messagebox.Show(this, "일 목표량을 등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        StringBuilder sql = annualRepository.UpdateDailyTarget(um.user_id, daily_target_amont);
                        List<StringBuilder> sqlList = new List<StringBuilder>();
                        sqlList.Add(sql);
                        if (commonRepository.UpdateTran(sqlList) == -1)
                            messagebox.Show(this, "등록 중 에러가 발생하였습니다!");
                    }
                    if(currentRow != null)
                        VacationSuccessRate(currentRow);
                }
                else
                {
                    txtDailyTargetAmount.Focus();
                    messagebox.Show(this, "목표량 입력값이 숫자 형식이 아닙니다!");
                }
            }
        }

        private void dgvEmplyee_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvEmplyee.Columns[e.ColumnIndex].Name == "daily_work_goals_amount")
                {
                    if (dgvEmplyee.Rows[e.RowIndex].Cells["daily_work_goals_amount"].Value != null && int.TryParse(dgvEmplyee.Rows[e.RowIndex].Cells["daily_work_goals_amount"].Value.ToString(), out int daily_target_amont))
                    {
                        if (messagebox.Show(this, "일 목표량을 등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            StringBuilder sql = annualRepository.UpdateDailyTarget(um.user_id, daily_target_amont);
                            List<StringBuilder> sqlList = new List<StringBuilder>();
                            sqlList.Add(sql);
                            if (commonRepository.UpdateTran(sqlList) == -1)
                                messagebox.Show(this, "등록 중 에러가 발생하였습니다!");
                        }
                        VacationSuccessRate(dgvEmplyee.Rows[e.RowIndex]);
                    }
                    else
                    {
                        dgvEmplyee.CurrentCell = dgvEmplyee.Rows[e.RowIndex].Cells["daily_work_goals_amount"];
                        messagebox.Show(this, "목표량 입력값이 숫자 형식이 아닙니다!");
                    }
                }
            }
        }
    }
}
