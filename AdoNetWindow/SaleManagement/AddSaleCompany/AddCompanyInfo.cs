using AdoNetWindow.Common.MultiDatesCalendar;
using AdoNetWindow.Model;
using AdoNetWindow.SaleManagement.AddSaleCompany;
using Repositories;
using Repositories.Company;
using Repositories.Config;
using Repositories.SalesPartner;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement
{
    public partial class AddCompanyInfo : Form
    {
        ICommonRepository commonRepository = new CommonRepository();
        ICompanyRepository companyRepository = new CompanyRepository();
        ICompanyAlarmRepository companyAlarmRepository = new CompanyAlarmRepository();
        ISalesPartnerRepository salesPartnerRepository = new SalesPartnerRepository();
        ICompanyFinanceRepository companyFinanceRepository = new CompanyFinanceRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        UsersModel um;
        DataGridViewRow row = null;
        SalesManager sm = null;
        string alarm_complete_date;
        public AddCompanyInfo(UsersModel um, SalesManager sm, DataGridViewRow row)
        {
            InitializeComponent();
            this.um = um;
            this.row = row;
            this.sm = sm;

            sm.SetCellValueEvent(false);
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.Value == null)
                    cell.Value = string.Empty;
            }
            sm.SetCellValueEvent();

            lbCompanyId.Text = row.Cells["id"].Value.ToString().Trim();
            txtGroupName.Text = row.Cells["group_name"].Value.ToString().Trim();
            txtCompany.Text = row.Cells["company"].Value.ToString().Trim();
            txtRegistrationNumber.Text = row.Cells["registration_number"].Value.ToString().Trim();
            txtCeo.Text = row.Cells["ceo"].Value.ToString().Trim();
            txtTel.Text = row.Cells["tel"].Value.ToString().Trim();
            txtFax.Text = row.Cells["fax"].Value.ToString().Trim();
            txtPhone.Text = row.Cells["phone"].Value.ToString().Trim();
            txtOtherPhone.Text = row.Cells["other_phone"].Value.ToString().Trim();
            txtCompanyManager.Text = row.Cells["company_manager"].Value.ToString().Trim();
            txtCompanyManagerPosition.Text = row.Cells["company_manager_position"].Value.ToString().Trim();
            txtAddress.Text = row.Cells["address"].Value.ToString().Trim();
            txtEmail.Text = row.Cells["email"].Value.ToString().Trim();
            txtWeb.Text = row.Cells["web"].Value.ToString().Trim();
            txtRemark.Text = row.Cells["remark"].Value.ToString().Trim();
            txtAtoManager.Text = row.Cells["ato_manager"].Value.ToString().Trim();
            txtSeaoverCompanyCode.Text = row.Cells["seaover_company_code"].Value.ToString().Trim();
            txtRemark4.Text = row.Cells["remark4"].Value.ToString().Trim();
            txtRemark5.Text = row.Cells["remark5"].Value.ToString().Trim();
            txtRemark6.Text = row.Cells["remark6"].Value.ToString().Trim();

            txtIndustryType.Text = row.Cells["industry_type"].Value.ToString().Trim();
            txtIndustryType2.Text = row.Cells["industry_type2"].Value.ToString().Trim();

            bool isNotSendFax;
            if (row.Cells["isNotSendFax"].Value == null || !bool.TryParse(row.Cells["isNotSendFax"].Value.ToString(), out isNotSendFax))
                isNotSendFax = false;
            cbSendFax.Checked = isNotSendFax;

            //알람
            /*txtAlarmMonth.Text = row.Cells["alarm_month"].Value.ToString();
            if(!string.IsNullOrEmpty(row.Cells["alarm_week"].Value.ToString().Trim()))
            {
                string[] weeks = row.Cells["alarm_week"].Value.ToString().Split('_');
                foreach(string week in weeks)
                {
                    if (!string.IsNullOrEmpty(week.Trim()))
                    {
                        switch (week.Trim())
                        {
                            case "1":
                                cbMonday.Checked = true;
                                break;
                            case "2":
                                cbTuseday.Checked = true;
                                break;
                            case "3":
                                cbWednesday.Checked = true;
                                break;
                            case "4":
                                cbThursday.Checked = true;
                                break;
                            case "5":
                                cbFriday.Checked = true;
                                break;

                        }
                    }
                }
            }*/
            alarm_complete_date = row.Cells["alarm_complete_date"].Value.ToString();
            //체크리스트
            SetDgvComments(row.Cells["sales_comment"].Value.ToString()
                        , row.Cells["distribution"].Value.ToString()
                        , row.Cells["handling_item"].Value.ToString()
                        , row.Cells["payment_date"].Value.ToString());
            //알람리스트
            GetAlarm();
        }

        public AddCompanyInfo(UsersModel um, string company_id)
        {
            InitializeComponent();
            this.um = um;

            lbCompanyId.Text = company_id;

            //DataTable companyDt = commonRepository.SelectAsOne("t_company", " * ", "id" , company_id);
            DataTable companyDt = companyRepository.GetCompanyAsOne("", "", company_id);
            if (companyDt != null && companyDt.Rows.Count > 0)
            {
                txtCompany.Text = companyDt.Rows[0]["name"].ToString();
                txtRegistrationNumber.Text = companyDt.Rows[0]["registration_number"].ToString();
                txtCeo.Text = companyDt.Rows[0]["ceo"].ToString();
                txtTel.Text = companyDt.Rows[0]["tel"].ToString();
                txtFax.Text = companyDt.Rows[0]["fax"].ToString();
                txtPhone.Text = companyDt.Rows[0]["phone"].ToString();
                txtCompanyManager.Text = companyDt.Rows[0]["company_manager"].ToString();
                txtCompanyManagerPosition.Text = companyDt.Rows[0]["company_manager_position"].ToString();
                txtAddress.Text = companyDt.Rows[0]["address"].ToString();
                txtEmail.Text = companyDt.Rows[0]["email"].ToString();
                txtWeb.Text = companyDt.Rows[0]["web"].ToString();
                txtRemark.Text = companyDt.Rows[0]["remark"].ToString();
                txtAtoManager.Text = companyDt.Rows[0]["ato_manager"].ToString();
                txtSeaoverCompanyCode.Text = companyDt.Rows[0]["seaover_company_code"].ToString();
                txtRemark4.Text = companyDt.Rows[0]["remark4"].ToString();

                bool isNotSendFax;
                if (!bool.TryParse(companyDt.Rows[0]["isNotSendFax"].ToString(), out isNotSendFax))
                    isNotSendFax = false;
                cbSendFax.Checked = isNotSendFax;

                //알람
                alarm_complete_date = companyDt.Rows[0]["alarm_complete_date"].ToString();
            }
            //체크리스트
            SetDgvComments(companyDt.Rows[0]["sales_comment"].ToString()
                        , companyDt.Rows[0]["distribution"].ToString()
                        , companyDt.Rows[0]["handling_item"].ToString()
                        , companyDt.Rows[0]["payment_date"].ToString());
            //알람리스트
            GetAlarm();
        }

        private void AddCompanyInfo_Load(object sender, EventArgs e)
        {
            for (int i = 2016; i <= DateTime.Now.Year + 2; i++)
                cbYear.Items.Add(i.ToString()); 

            cbYear.Text = DateTime.Now.Year.ToString();
            cbMonth.Text = DateTime.Now.Month.ToString();
            //GetCalendar(Convert.ToInt16(cbYear.Text), Convert.ToInt16(cbMonth.Text));
            //재무제표
            GetCompanyFinance();
        }

        #region Method
        public int GetIndustryTypeRank(string industry_type)
        {
            int industry_type_rank;
            switch (industry_type)
            {
                case "수산동물 가공 및 저장 처리업":
                    industry_type_rank = 2;
                    break;
                case "기타 수산동물 가공 및 저장 처리업":
                    industry_type_rank = 2;
                    break;
                case "수산동물 냉동품 제조업":
                    industry_type_rank = 2;
                    break;
                case "수산동물 훈제, 조리 및 유사 조제식품 제조업":
                    industry_type_rank = 2;
                    break;
                case "신선, 냉동 및 기타 수산물 도매업":
                    industry_type_rank = 2;
                    break;
                case "수산물 가공식품 도매업":
                    industry_type_rank = 2;
                    break;
                case "신선, 냉동 및 기타 수산물 소매업":
                    industry_type_rank = 3;
                    break;
                case "기타 식사용 가공처리 조리식품 제조업":
                    industry_type_rank = 4;
                    break;
                case "기타 신선식품 및 단순 가공식품 도매업":
                    industry_type_rank = 4;
                    break;
                case "도시락류 제조업":
                    industry_type_rank = 4;
                    break;
                case "수산동물 건조 및 염장품 제조업":
                    industry_type_rank = 5;
                    break;
                case "건어물 및 젓갈류 도매업":
                    industry_type_rank = 6;
                    break;
                case "어로 어업":
                    industry_type_rank = 6;
                    break;
                case "건어물 및 젓갈류 소매업":
                    industry_type_rank = 7;
                    break;
                case "수산식물 가공 및 저장 처리업":
                    industry_type_rank = 8;
                    break;
                case "내수면 양식 어업":
                    industry_type_rank = 9;
                    break;
                case "내수면 어업":
                    industry_type_rank = 9;
                    break;
                case "수산물 부화 및 수산종자 생산업":
                    industry_type_rank = 9;
                    break;
                case "양식 어업":
                    industry_type_rank = 9;
                    break;
                case "어업 관련 서비스업":
                    industry_type_rank = 9;
                    break;
                case "연근해 어업":
                    industry_type_rank = 9;
                    break;
                case "원양 어업":
                    industry_type_rank = 9;
                    break;
                case "해수면 양식 어업":
                    industry_type_rank = 9;
                    break;
                case "낚시 및 수렵용구 제조업":
                    industry_type_rank = 10;
                    break;
                case "낚시장 운영업":
                    industry_type_rank = 10;
                    break;
                case "어망 및 기따 끈 가공품 제조업":
                    industry_type_rank = 10;
                    break;

                default:
                    industry_type_rank = 99;
                    break;
            }
            return industry_type_rank;
        }
        public void AddCompanyFinance(CompanyFinanceModel model)
        {
            foreach (DataGridViewRow row in dgvFinance.Rows)
            {
                if (row.Cells["finance_year"].Value.ToString().Equals(model.year.ToString()))
                {
                    dgvFinance.Rows.Remove(row);
                    break;
                }
            }
            int n = dgvFinance.Rows.Add();
            dgvFinance.Rows[n].Cells["finance_year"].Value = model.year.ToString();
            dgvFinance.Rows[n].Cells["assets_amount"].Value = (model.capital_amount + model.debt_amount).ToString("#,##0");
            dgvFinance.Rows[n].Cells["capital_amount"].Value = model.capital_amount.ToString("#,##0");
            dgvFinance.Rows[n].Cells["debt_amount"].Value = model.debt_amount.ToString("#,##0");
            dgvFinance.Rows[n].Cells["sales_amount"].Value = model.sales_amount.ToString("#,##0");
            dgvFinance.Rows[n].Cells["net_income_amount"].Value = model.net_income_amount.ToString("#,##0");
            dgvFinance.Rows[n].Cells["finance_edit_user"].Value = um.user_name;
            dgvFinance.Rows[n].Cells["finance_updatetime"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            dgvFinance.Sort(dgvFinance.Columns["finance_year"], System.ComponentModel.ListSortDirection.Descending);
        }

        public void DeleteCompanyFinance(CompanyFinanceModel model)
        {
            foreach (DataGridViewRow row in dgvFinance.Rows)
            {
                if (row.Cells["finance_year"].Value.ToString().Equals(model.year.ToString()))
                {
                    dgvFinance.Rows.Remove(row);
                    break;
                }
            }
            dgvFinance.Sort(dgvFinance.Columns["finance_year"], System.ComponentModel.ListSortDirection.Descending);
        }

        public void GetCompanyFinance()
        { 
            dgvFinance.Rows.Clear();
            DataTable financeDt = companyFinanceRepository.GetCompanyFinance(lbCompanyId.Text);
            if (financeDt.Rows.Count > 0)
            {
                for (int i = 0; i < financeDt.Rows.Count; i++)
                {
                    int n = dgvFinance.Rows.Add();
                    dgvFinance.Rows[n].Cells["finance_year"].Value = financeDt.Rows[i]["year"].ToString();

                    double capital_amount;
                    if (!double.TryParse(financeDt.Rows[i]["capital_amount"].ToString(), out capital_amount))
                        capital_amount = 0;

                    double debt_amount;
                    if (!double.TryParse(financeDt.Rows[i]["debt_amount"].ToString(), out debt_amount))
                        debt_amount = 0;

                    double sales_amount;
                    if (!double.TryParse(financeDt.Rows[i]["sales_amount"].ToString(), out sales_amount))
                        sales_amount = 0;

                    double net_income_amount;
                    if (!double.TryParse(financeDt.Rows[i]["net_income_amount"].ToString(), out net_income_amount))
                        net_income_amount = 0;

                    dgvFinance.Rows[n].Cells["assets_amount"].Value = (capital_amount + debt_amount).ToString("#,##0");
                    dgvFinance.Rows[n].Cells["capital_amount"].Value = capital_amount.ToString("#,##0");
                    dgvFinance.Rows[n].Cells["debt_amount"].Value = debt_amount.ToString("#,##0");
                    dgvFinance.Rows[n].Cells["sales_amount"].Value = sales_amount.ToString("#,##0");
                    dgvFinance.Rows[n].Cells["net_income_amount"].Value = net_income_amount.ToString("#,##0");
                    dgvFinance.Rows[n].Cells["finance_edit_user"].Value = financeDt.Rows[i]["edit_user"].ToString();
                    if (DateTime.TryParse(financeDt.Rows[i]["updatetime"].ToString(), out DateTime updatetime))
                        dgvFinance.Rows[n].Cells["finance_updatetime"].Value = updatetime.ToString("yyyy-MM-dd HH:mm:ss");

                }
            }
        }



        private void GetCalendar(int year, int month)
        {
            //Lets get the first day of the month
            DateTime startofthemonth = new DateTime(year, month, 1);

            //get the count of days of the month
            int days = DateTime.DaysInMonth(year, month);

            //convert the startofthemonth to integer
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 1;

            //Holiday List
            int[] no;
            string[] name;
            common.getRedDay(year, month, out no, out name);

            DateTime sttdate = new DateTime(year, month, 1, 0, 0, 0);
            DateTime sttdate2 = new DateTime(year, month, 1, 0, 0, 0).AddDays(-7);
            DateTime enddate = new DateTime(year, month, 1, 0, 0, 0).AddMonths(1).AddDays(-1);
            int dayofnextmonth = 0;
            //마지막주의 금요일까지
            while (!(enddate.DayOfWeek == DayOfWeek.Saturday || enddate.DayOfWeek == DayOfWeek.Sunday))
            {
                enddate = enddate.AddDays(1);
                dayofnextmonth++;      //다음달 오버되는 일짜
            }
            pnDays.Controls.Clear();
            //==========================================================================
            //first lets create a blank usercontrol
            for (int i = 1; i < dayoftheweek; i++)
            {
                UnitEmptyDays ucblack = new UnitEmptyDays();
                pnDays.Controls.Add(ucblack);
            }
            //==========================================================================
            //now lets create usercontrol for days
            for (int i = 1; i <= days; i++)
            {
                DateTime dt = new DateTime(year, month, i);
                UnitDays ucdays = new UnitDays(this, dt);
                pnDays.Controls.Add(ucdays);
            }
        }

        private void GetAlarm()
        {
            int id;
            if (!Int32.TryParse(lbCompanyId.Text, out id))
            {
                messageBox.Show(this,"거래처 정보를 찾을 수  없습니다.");
                this.Activate();
                return;
            }

            dgvAlarm.Rows.Clear();

            DateTime complete_date;
            if (!DateTime.TryParse(alarm_complete_date, out complete_date))
                complete_date = new DateTime(1900, 1, 1);

            DataTable alarmDt = companyAlarmRepository.GetCompanyAlarm(id, "", complete_date.ToString("yyyy-MM-dd"));
            if (alarmDt.Rows.Count > 0)
            {
                alarmDt.Columns["complete_date"].ReadOnly = false;
                alarmDt.Columns["real_alarm_date"].ReadOnly = false;
                foreach (DataRow dr in alarmDt.Rows)
                {
                    dr["complete_date"] = complete_date.ToString("yyyy-MM-dd");

                    if (dr["category"].ToString() == "특정일자" && DateTime.TryParse(dr["alarm_date"].ToString(), out DateTime alarm_date))
                    {
                        //공휴일일 경우 최근영업일로 변경
                        alarm_date = common.SetCurrentWorkDate(alarm_date);
                        dr["real_alarm_date"] = alarm_date.ToString("yyyy-MM-dd");
                    }
                    else if (dr["category"].ToString() == "주알람" && int.TryParse(dr["alarm_date"].ToString(), out int alarm_week))
                    {
                        alarm_date = DateTime.Now.AddDays(alarm_week - (int)DateTime.Now.DayOfWeek);
                        alarm_date = common.SetCurrentWorkDate(alarm_date);
                        //주알람인데 지났다면 강제 다음주
                        if (alarm_date < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                        {
                            alarm_date = DateTime.Now.AddDays(7).AddDays(alarm_week - (int)DateTime.Now.DayOfWeek);
                            alarm_date = common.SetCurrentWorkDate(alarm_date);
                        }
                        dr["real_alarm_date"] = alarm_date.ToString("yyyy-MM-dd");
                    }
                    else if (dr["category"].ToString() == "월알람" && int.TryParse(dr["alarm_date"].ToString(), out int alarm_days))
                    {
                        alarm_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, alarm_days);
                        alarm_date = common.SetCurrentWorkDate(alarm_date);
                        //월알람인데 지났다면 강제 다음월
                        if (alarm_date < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                        {
                            alarm_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, alarm_days).AddMonths(1);
                            alarm_date = common.SetCurrentWorkDate(alarm_date);
                        }
                        dr["real_alarm_date"] = alarm_date.ToString("yyyy-MM-dd");
                    }

                    DateTime real_alarm_date;
                    if (DateTime.TryParse(dr["real_alarm_date"].ToString(), out real_alarm_date)
                        && Convert.ToDateTime(real_alarm_date.ToString("yyyy-MM-dd")) > Convert.ToDateTime(complete_date.ToString("yyyy-MM-dd")))
                        dr["isComplete"] = "0";
                    else
                        dr["isComplete"] = "1";
                }
                alarmDt.AcceptChanges();
                DataView dv = new DataView(alarmDt);
                dv.Sort = "isComplete ASC, real_alarm_date ASC ";
                alarmDt = dv.ToTable();

                for (int i = 0; i < alarmDt.Rows.Count; i++)
                {
                    if (alarmDt.Rows[i]["category"].ToString() == "특정일자" && DateTime.TryParse(alarmDt.Rows[i]["alarm_date"].ToString(), out DateTime alarm_date))
                    {
                        //공휴일일 경우 최근영업일로 변경
                        alarm_date = common.SetCurrentWorkDate(alarm_date);
                        //알람이 안지난
                        if (alarm_date >= complete_date)
                        {
                            int n = dgvAlarm.Rows.Add();
                            DataGridViewComboBoxCell cCell = new DataGridViewComboBoxCell();
                            cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            cCell.Items.Add("발주");
                            cCell.Items.Add("결제");
                            cCell.Items.Add("신규");
                            cCell.Items.Add("기타");
                            dgvAlarm.Rows[n].Cells["alarm_division"] = cCell;
                            dgvAlarm.Rows[n].Cells["alarm_division"].Value = alarmDt.Rows[i]["division"].ToString();
                            dgvAlarm.Rows[n].Cells["alarm_category"].Value = alarmDt.Rows[i]["category"].ToString();
                            dgvAlarm.Rows[n].Cells["alarm_date"].Value = alarmDt.Rows[i]["alarm_date"].ToString();
                            dgvAlarm.Rows[n].Cells["real_alarm_date"].Value = alarmDt.Rows[i]["alarm_date"].ToString();
                            dgvAlarm.Rows[n].Cells["alarm_remark"].Value = alarmDt.Rows[i]["alarm_remark"].ToString();
                            dgvAlarm.Rows[n].Cells["edit_user"].Value = alarmDt.Rows[i]["edit_user"].ToString();
                            if (DateTime.TryParse(alarmDt.Rows[i]["updatetime"].ToString(), out DateTime updatetime))
                                dgvAlarm.Rows[n].Cells["updatetime"].Value = updatetime.ToString("yyyy-MM-dd");

                            if (alarm_date == complete_date)
                                dgvAlarm.Rows[n].DefaultCellStyle.ForeColor = Color.LightGray;

                        }
                    }
                    else if (alarmDt.Rows[i]["category"].ToString() == "주알람" && int.TryParse(alarmDt.Rows[i]["alarm_date"].ToString(), out int alarm_week))
                    {
                        alarm_date = DateTime.Now.AddDays(alarm_week - (int)DateTime.Now.DayOfWeek);
                        alarm_date = common.SetCurrentWorkDate(alarm_date);
                        //주알람인데 지났다면 강제 다음주
                        if (alarm_date < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                        {
                            alarm_date = DateTime.Now.AddDays(7).AddDays(alarm_week - (int)DateTime.Now.DayOfWeek);
                            alarm_date = common.SetCurrentWorkDate(alarm_date);
                        }
                        //알람이 안지난
                        if (alarm_date >= complete_date)
                        {
                            int n = dgvAlarm.Rows.Add();
                            DataGridViewComboBoxCell cCell = new DataGridViewComboBoxCell();
                            cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            cCell.Items.Add("발주");
                            cCell.Items.Add("결제");
                            cCell.Items.Add("신규");
                            cCell.Items.Add("기타");
                            dgvAlarm.Rows[n].Cells["alarm_division"] = cCell;
                            dgvAlarm.Rows[n].Cells["alarm_division"].Value = alarmDt.Rows[i]["division"].ToString();
                            dgvAlarm.Rows[n].Cells["alarm_category"].Value = alarmDt.Rows[i]["category"].ToString();
                            switch (alarmDt.Rows[i]["alarm_date"].ToString())
                            {
                                case "1":
                                    dgvAlarm.Rows[n].Cells["alarm_date"].Value = "월";
                                    break;
                                case "2":
                                    dgvAlarm.Rows[n].Cells["alarm_date"].Value = "화";
                                    break;
                                case "3":
                                    dgvAlarm.Rows[n].Cells["alarm_date"].Value = "수";
                                    break;
                                case "4":
                                    dgvAlarm.Rows[n].Cells["alarm_date"].Value = "목";
                                    break;
                                case "5":
                                    dgvAlarm.Rows[n].Cells["alarm_date"].Value = "금";
                                    break;
                                default:
                                    dgvAlarm.Rows[n].Cells["alarm_date"].Value = alarmDt.Rows[i]["alarm_date"].ToString();
                                    break;
                            }
                            dgvAlarm.Rows[n].Cells["real_alarm_date"].Value = alarm_date.ToString("yyyy-MM-dd");

                            dgvAlarm.Rows[n].Cells["alarm_remark"].Value = alarmDt.Rows[i]["alarm_remark"].ToString();
                            dgvAlarm.Rows[n].Cells["edit_user"].Value = alarmDt.Rows[i]["edit_user"].ToString();
                            if (DateTime.TryParse(alarmDt.Rows[i]["updatetime"].ToString(), out DateTime updatetime))
                                dgvAlarm.Rows[n].Cells["updatetime"].Value = updatetime.ToString("yyyy-MM-dd");

                            if (alarm_date == complete_date)
                                dgvAlarm.Rows[n].DefaultCellStyle.ForeColor = Color.LightGray;

                        }
                    }
                    else if (alarmDt.Rows[i]["category"].ToString() == "월알람" && int.TryParse(alarmDt.Rows[i]["alarm_date"].ToString(), out int alarm_days))
                    {
                        alarm_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, alarm_days);
                        alarm_date = common.SetCurrentWorkDate(alarm_date);
                        //월알람인데 지났다면 강제 다음월
                        if (alarm_date < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                        {
                            alarm_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, alarm_days).AddMonths(1);
                            alarm_date = common.SetCurrentWorkDate(alarm_date);
                        }
                        //알람이 안지난
                        if (alarm_date >= complete_date)
                        {
                            int n = dgvAlarm.Rows.Add();
                            DataGridViewComboBoxCell cCell = new DataGridViewComboBoxCell();
                            cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            cCell.Items.Add("발주");
                            cCell.Items.Add("결제");
                            cCell.Items.Add("신규");
                            cCell.Items.Add("기타");
                            dgvAlarm.Rows[n].Cells["alarm_division"] = cCell;
                            dgvAlarm.Rows[n].Cells["alarm_division"].Value = alarmDt.Rows[i]["division"].ToString();
                            dgvAlarm.Rows[n].Cells["alarm_category"].Value = alarmDt.Rows[i]["category"].ToString();
                            dgvAlarm.Rows[n].Cells["alarm_date"].Value = alarmDt.Rows[i]["alarm_date"].ToString();
                            dgvAlarm.Rows[n].Cells["real_alarm_date"].Value = alarm_date.ToString("yyyy-MM-dd");
                            dgvAlarm.Rows[n].Cells["alarm_remark"].Value = alarmDt.Rows[i]["alarm_remark"].ToString();
                            dgvAlarm.Rows[n].Cells["edit_user"].Value = alarmDt.Rows[i]["edit_user"].ToString();
                            if (DateTime.TryParse(alarmDt.Rows[i]["updatetime"].ToString(), out DateTime updatetime))
                                dgvAlarm.Rows[n].Cells["updatetime"].Value = updatetime.ToString("yyyy-MM-dd");

                            if (alarm_date == complete_date)
                                dgvAlarm.Rows[n].DefaultCellStyle.ForeColor = Color.LightGray;

                        }
                    }
                }
            }
        }


        DataGridViewRow DistributionRow = null;
        DataGridViewRow PayDateRow = null;
        DataGridViewRow HandlingItemRow = null;
        
        private void SetDgvComments(string sales_comment, string distribution, string handling_item, string payment_date)
        {
            foreach (DataGridViewColumn column in dgvComments.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            int n = dgvComments.Rows.Add();
            dgvComments.Rows[n].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
            dgvComments.Rows[n].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvComments.Rows[n].Cells["division"].Value = "1.친근감 ";
            /*dgvComments.Rows[n].Cells["comment"].Value = "친근감";*/

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "안녕하세요. 사장님. 사장님 부산에서 워낙 유명하시던데 사장님과 꼭 거래해보고 싶어서 연락드렸습니다.";
            dgvComments.Rows[n].Visible = false;

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "직원도 많고 수산물의 대기업이라 불리시던데 저도 사장님이랑 꼭 거래해보고 싶습니다.";
            dgvComments.Rows[n].Visible = false;

            //유통
            n = dgvComments.Rows.Add();
            DistributionRow = dgvComments.Rows[n];
            dgvComments.Rows[n].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
            dgvComments.Rows[n].DefaultCellStyle.BackColor = Color.Beige;
            dgvComments.Rows[n].Cells["division"].Value = "2.유통";
            dgvComments.Rows[n].Cells["comment"].Value = distribution;
            dgvComments.Rows[n].Cells["comment"].Style.Font = new Font("굴림", 9, FontStyle.Regular);
            dgvComments.Rows[n].Cells["comment"].Style.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvComments.Rows[n].Height = 100;

            /*n = dgvComments.Rows.Add();
            dgvComments.Rows[n].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
            dgvComments.Rows[n].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvComments.Rows[n].Cells["division"].Value = "2 . ";
            dgvComments.Rows[n].Cells["comment"].Value = "유통구조";*/

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "사장님 보통 어디에 납품하세요?";
            dgvComments.Rows[n].Visible = false;

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "사장님 중식, 일식, 급식, 식자재, 프랜차이즈 등 어떤 곳에 주로 납품하세요?";
            dgvComments.Rows[n].Visible = false;

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "급식 : 백새우살, 홍새우살, 절단낙지, 가자미, 칵테일새우 등 벌크 위주 제품";
            dgvComments.Rows[n].Visible = false;

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "중식 : 새우류, 연체류 모든 제품(새우살, PDTO, 오징어채, 무라편채, 무라귀채 등)";
            dgvComments.Rows[n].Visible = false;

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "일식 : 초밥류 재료, 노바시 새우, 튀김용 새우";
            dgvComments.Rows[n].Visible = false;

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "뷔페,  결혼식 : 초밥류, 과일, 야채류 등";
            dgvComments.Rows[n].Visible = false;

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "프랜차이즈 : 어떤 프랜차이즈인지 물어보고 사용하는 제품 꼭 물어볼 것(프랜차이즈는 제품이랑 규격이 정해져 있기 때문)";
            dgvComments.Rows[n].Visible = false;

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
            dgvComments.Rows[n].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvComments.Rows[n].Cells["division"].Value = "3 .자신감 ";
            //dgvComments.Rows[n].Cells["comment"].Value = "자신감";

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "얼마 받고 계세요? 천원 더 싸게 맞춰드릴게요.";
            dgvComments.Rows[n].Visible = false;

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "저희 물건 품질 좋은데 5박스만 샘플 써주세요~ 창고 안맞으면 실어다드릴게요.";
            dgvComments.Rows[n].Visible = false;

            
            //취급품목
            n = dgvComments.Rows.Add();
            HandlingItemRow = dgvComments.Rows[n];
            dgvComments.Rows[n].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
            dgvComments.Rows[n].DefaultCellStyle.BackColor = Color.Beige;
            dgvComments.Rows[n].Cells["division"].Value = "4.발주주기\n 소진기간 물어보기(취급품목)";
            dgvComments.Rows[n].Cells["comment"].Value = handling_item;
            dgvComments.Rows[n].Cells["comment"].Style.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvComments.Rows[n].Cells["comment"].Style.Font = new Font("굴림", 9, FontStyle.Regular);
            dgvComments.Rows[n].Height = 100;

            /*n = dgvComments.Rows.Add();
            dgvComments.Rows[n].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
            dgvComments.Rows[n].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvComments.Rows[n].Cells["division"].Value = "4 . ";
            dgvComments.Rows[n].Cells["comment"].Value = "발주주기, 소진기간 물어보기";*/

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "사장님~ 최근에 물건 올리셨나요?";
            dgvComments.Rows[n].Visible = false;

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "또 언제 발주 예정이세요?";
            dgvComments.Rows[n].Visible = false;

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "정해진 발주 날짜, 요일이 있을까요?";
            dgvComments.Rows[n].Visible = false;

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "발주 전날 오후에 제가 전화드리겠습니다.";
            dgvComments.Rows[n].Visible = false;

            //결제일자
            n = dgvComments.Rows.Add();
            PayDateRow = dgvComments.Rows[n];
            dgvComments.Rows[n].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
            dgvComments.Rows[n].Cells["comment"].Style.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvComments.Rows[n].DefaultCellStyle.BackColor = Color.Beige;
            dgvComments.Rows[n].Cells["division"].Value = "5.결제일자";
            dgvComments.Rows[n].Cells["comment"].Value = payment_date;
            dgvComments.Rows[n].Cells["comment"].Style.Font = new Font("굴림", 9, FontStyle.Regular);
            dgvComments.Rows[n].Height = 100;

            /*n = dgvComments.Rows.Add();
            dgvComments.Rows[n].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
            dgvComments.Rows[n].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvComments.Rows[n].Cells["division"].Value = "5 . ";
            dgvComments.Rows[n].Cells["comment"].Value = "결제조건";*/

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "사장님 결제는 어떻게 진행될까요?";
            dgvComments.Rows[n].Visible = false;

            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "저희는 보통 3일에서 5일 결제인데, 괜찮으신가요?";
            dgvComments.Rows[n].Visible = false;

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "사장님 ~ 다른 업체랑은 거래할때 결제는 어떻게 진행하나요?";
            dgvComments.Rows[n].Visible = false;

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "다른곳에서 깔아주고 있다고 하면 '저희도 최대한 맞춰볼게요~ 우선은 윗선에 보고하고 말씀드리겠습니다.'";
            dgvComments.Rows[n].Visible = false;

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
            dgvComments.Rows[n].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvComments.Rows[n].Cells["division"].Value = "6 .마무리";
            dgvComments.Rows[n].Cells["comment"].Value = "공감대 형성 & 자기어필(통화종료 전에)";

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "사장님 목소리가 너무 좋으시네요.";
            dgvComments.Rows[n].Visible = false;

            n = dgvComments.Rows.Add();
            dgvComments.Rows[n].Cells["division"].Value = " - ";
            dgvComments.Rows[n].Cells["division"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvComments.Rows[n].Cells["comment"].Value = "사장님 혹시라도 다른 곳에서 못구하는 제품 있으면 저한테 알려주세요. 다른 사람들이 못구하는거 저는 구해드릴수 있습니다.";
            dgvComments.Rows[n].Visible = false;


            foreach (DataGridViewRow row in dgvComments.Rows)
            {
                if (row.Cells["division"].Value != null && row.Cells["division"].Value.ToString().Contains('.'))
                {
                    DataGridViewCheckBoxCell cell = new DataGridViewCheckBoxCell();
                    cell.Value = false;
                    row.Cells["chk"] = cell;
                    row.Cells["chk"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }

            if (!string.IsNullOrEmpty(sales_comment))
            {
                string[] comments = sales_comment.Split('_');
                if (comments.Length > 0)
                {
                    foreach (string cmt in comments)
                    {
                        if (!string.IsNullOrEmpty(cmt))
                        {
                            int rowindex = Convert.ToInt32(cmt.Trim());
                            dgvComments.Rows[rowindex].Cells["chk"].Value = true;
                        }
                    }
                }
            }
        }

        private bool isVailidation()
        {
            if (!rbUpdateInfo.Checked && !rbCommonData.Checked && !rbMyData.Checked && !rbPotential1.Checked && !rbPotential2.Checked && !rbNonHandled.Checked)
            {
                messageBox.Show(this,"카테고리를 선택해주세요!");
                this.Activate();
                return false;
            }
            else if (!rbUpdateInfo.Checked && !rbNonHandled.Checked && string.IsNullOrEmpty(txtAtoManager.Text))
            {
                messageBox.Show(this,"아토담당자를 입력해주세요!");
                this.Activate();
                return false;
            }
            else if (string.IsNullOrEmpty(txtCompany.Text.Trim()))
            {
                messageBox.Show(this,"거래처를 입력해주세요!");
                this.Activate();
                return false;
            }
            return true;
        }
        #endregion

        #region Button
        private void btnToday_Click(object sender, EventArgs e)
        {
            AddAlarmEvent(DateTime.Now);
        }

        private void btnTomorrow_Click(object sender, EventArgs e)
        {
            AddAlarmEvent(DateTime.Now.AddDays(1));
        }

        private void btnAddFinance_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "영업거래처 관리", "거래처 관리", "is_add"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            int company_id;
            if (!int.TryParse(lbCompanyId.Text, out company_id) || company_id <= 0)
            {
                messageBox.Show(this, "거래처를 먼저 등록한 후에 재무제표를 추가해주세요!");
                return;
            }
            try
            {
                FinanceManager fm = new FinanceManager(this, um, company_id.ToString());
                fm.Owner = this;
                fm.ShowDialog();
            }
            catch
            { }
        }
        private void btnPreMonth_Click(object sender, EventArgs e)
        {
            int year;
            if (!int.TryParse(cbYear.Text, out year))
                year = DateTime.Now.Year;

            int month;
            if (!int.TryParse(cbMonth.Text, out month))
                month = DateTime.Now.Month;

            DateTime standardDt = new DateTime(year, month, 1).AddMonths(-1);

            cbYear.Text = standardDt.Year.ToString();
            cbMonth.Text = standardDt.Month.ToString();

            GetCalendar(standardDt.Year, standardDt.Month);

        }

        private void btnAfterMonth_Click(object sender, EventArgs e)
        {
            int year;
            if (!int.TryParse(cbYear.Text, out year))
                year = DateTime.Now.Year;

            int month;
            if (!int.TryParse(cbMonth.Text, out month))
                month = DateTime.Now.Month;

            DateTime standardDt = new DateTime(year, month, 1).AddMonths(1);

            cbYear.Text = standardDt.Year.ToString();
            cbMonth.Text = standardDt.Month.ToString();

            GetCalendar(standardDt.Year, standardDt.Month);
        }

        private bool ExistAlarm(string category, string date)
        {
            foreach (DataGridViewRow row in dgvAlarm.Rows)
            {
                if (row.Cells["alarm_category"].Value.ToString() == category
                    && row.Cells["alarm_date"].Value.ToString() == date)
                    return true;
            }
            return false;
        }

        IAuthorityRepository authorityRepository = new AuthorityRepository();

        private void btnAddCompany_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "영업거래처 관리", "거래처 관리", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            if (!isVailidation())
                return;

            if (messageBox.Show(this,"거래처를 등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            //공용DATA가 체크됬을 경우
            if (rbCommonData.Checked)
            {
                if (messageBox.Show(this, "공용DATA로 이동시키면 영업사원들에게 무작위로 분배됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }



            //알람일 경우 처리할건지 확인
            bool isComplete = true;
            DateTime complete_date;
            if (!DateTime.TryParse(alarm_complete_date, out complete_date))
                complete_date = new DateTime(1900, 1, 1);

            //설정 알람순회
            foreach (DataGridViewRow row in dgvAlarm.Rows)
            {
                if (row.Cells["alarm_category"].Value.ToString().Equals("주알람"))
                {
                    int weeks = 0;
                    switch (row.Cells["alarm_date"].Value.ToString())
                    {
                        case "월":
                            weeks = 1;
                            break;
                        case "화":
                            weeks = 2;
                            break;
                        case "수":
                            weeks = 3;
                            break;
                        case "목":
                            weeks = 4;
                            break;
                        case "금":
                            weeks = 5;
                            break;
                    }

                    DateTime alarm_week_date = DateTime.Now.AddDays(weeks - (int)DateTime.Now.DayOfWeek);
                    row.Cells["real_alarm_date"].Value = alarm_week_date.ToString("yyyy-MM-dd");
                    alarm_week_date = common.SetCurrentWorkDate(alarm_week_date);
                    if (alarm_week_date.DayOfWeek == DateTime.Now.DayOfWeek
                        && Convert.ToDateTime(alarm_week_date.ToString("yyyy-MM-dd")) > Convert.ToDateTime(complete_date.ToString("yyyy-MM-dd")))
                        isComplete = false;
                }
                else if (row.Cells["alarm_category"].Value.ToString().Equals("월알람") && row.Cells["alarm_date"].Value != null && int.TryParse(row.Cells["alarm_date"].Value.ToString(), out int month))
                {
                    DateTime monthDt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, month);
                    row.Cells["real_alarm_date"].Value = monthDt.ToString("yyyy-MM-dd");
                    //공휴일일 경우 최근영업일로 변경
                    monthDt = common.SetCurrentWorkDate(monthDt);
                    if (Convert.ToDateTime(monthDt.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))
                        && Convert.ToDateTime(complete_date.ToString("yyyy-MM-dd")) < Convert.ToDateTime(monthDt.ToString("yyyy-MM-dd")))
                    {
                        isComplete = false;
                        break;
                    }
                }
                else if (row.Cells["alarm_category"].Value.ToString().Equals("특정일자") && row.Cells["alarm_date"].Value != null && DateTime.TryParse(row.Cells["alarm_date"].Value.ToString(), out DateTime alarmDt))
                {
                    row.Cells["real_alarm_date"].Value = alarmDt.ToString("yyyy-MM-dd");
                    //공휴일일 경우 최근영업일로 변경
                    alarmDt = common.SetCurrentWorkDate(alarmDt);
                    if (Convert.ToDateTime(alarmDt.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))
                        && Convert.ToDateTime(complete_date.ToString("yyyy-MM-dd")) < Convert.ToDateTime(alarmDt.ToString("yyyy-MM-dd")))
                    {
                        isComplete = false;
                        break;
                    }
                }
            }

            //완료되지 않은 알람일 경우
            bool isAlarmComplete = false;
            if (!isComplete)
            {
                if (messageBox.Show(this, "오늘 알람 설정 되어있는 거래처입니다. 알람완료 처리하시겠습니가?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    alarm_complete_date = DateTime.Now.ToString("yyyy-MM-dd");
                    isAlarmComplete = true;
                }
                this.Activate();
            }

            //기존ID가 없을경우 신규ID
            CompanyModel cModel = null;
            CompanyModel model = new CompanyModel();
            int id;
            if (!Int32.TryParse(lbCompanyId.Text, out id))
                id = 0;
            if (id == 0)
                id = commonRepository.GetNextId("t_company", "id");
            else
                cModel = companyRepository.GetCompanyAsOne2("", "", id.ToString());

            //잠재1, 2 휴대폰, 팩스, 이메일 유효성검사
            if ((rbUpdateInfo.Checked && cModel != null && (cModel.isPotential1 || cModel.isPotential2))
                || rbPotential1.Checked || rbPotential2.Checked)
            {
                if (string.IsNullOrEmpty(txtPhone.Text.Trim()) && string.IsNullOrEmpty(txtFax.Text.Trim()) && string.IsNullOrEmpty(txtEmail.Text.Trim()))
                {
                    messageBox.Show(this,"잠재1, 2 경우 팩스 또는 휴대폰, 이메일 중 하나는 필수값입니다!");
                    this.Activate();
                    return;
                }
            }

            model.id = id;
            model.division = "매출처";
            model.registration_number = txtRegistrationNumber.Text;
            model.group_name = txtGroupName.Text;
            model.name = txtCompany.Text;
            model.address = txtAddress.Text;
            model.ceo = txtCeo.Text;
            model.tel = txtTel.Text;
            model.fax = txtFax.Text;
            model.phone = txtPhone.Text;
            model.other_phone = txtOtherPhone.Text;
            model.company_manager = txtCompanyManager.Text;
            model.company_manager_position = txtCompanyManagerPosition.Text;
            model.email = txtEmail.Text;
            model.remark = txtRemark.Text;
            model.remark4 = txtRemark4.Text;
            model.remark5 = txtRemark5.Text;
            model.remark6 = txtRemark6.Text;
            model.web = txtWeb.Text;
            model.ato_manager = txtAtoManager.Text;

            if (!DateTime.TryParse(model.createtime, out DateTime dt) || dt.Year == 0000)
                model.createtime = DateTime.Now.ToString("yyyy-MM-dd");

            model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            model.edit_user = um.user_name;
            

            if (DistributionRow.Cells["comment"].Value == null)
                DistributionRow.Cells["comment"].Value = string.Empty;
            model.distribution = DistributionRow.Cells["comment"].Value.ToString();

            if (HandlingItemRow.Cells["comment"].Value == null)
                HandlingItemRow.Cells["comment"].Value = string.Empty;
            model.handling_item = HandlingItemRow.Cells["comment"].Value.ToString();

            if (PayDateRow.Cells["comment"].Value == null)
                PayDateRow.Cells["comment"].Value = string.Empty;
            model.payment_date = PayDateRow.Cells["comment"].Value.ToString();

            model.isNotSendFax = cbSendFax.Checked;
            model.seaover_company_code = txtSeaoverCompanyCode.Text;

            model.industry_type = txtIndustryType.Text;
            model.industry_type2 = txtIndustryType2.Text;

            //알람 완료일자
            model.alarm_complete_date = alarm_complete_date;

            //추가정보
            if (cModel != null)
            {
                model.group_name = cModel.group_name;
                model.origin = cModel.origin;
                model.sns1 = cModel.sns1;
                model.sns2 = cModel.sns2;
                model.sns3 = cModel.sns3;
                model.seaover_company_code = cModel.seaover_company_code;
                model.isManagement1 = cModel.isManagement1;
                model.isManagement2 = cModel.isManagement2;
                model.isManagement3 = cModel.isManagement3;
                model.isManagement4 = cModel.isManagement4;
                model.isHide = cModel.isHide;
                model.isDelete = cModel.isDelete;
                model.createtime = cModel.createtime;
                model.remark2 = cModel.remark2;
                model.remark3 = cModel.remark3;
                model.isTrading = cModel.isTrading;
            }
            else
            {
                model.group_name = "";
                model.origin = "국내";
                model.isManagement1 = false;
                model.isManagement2 = false;
                model.isManagement3 = false;
                model.isManagement4 = false;
                model.isHide = false;
                model.isDelete = false;
                model.createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (!string.IsNullOrEmpty(model.seaover_company_code))
                    model.isTrading = true;
            }
            //카테고리
            if (rbCommonData.Checked)
            {
                model.ato_manager = "";
                model.isPotential1 = false;
                model.isPotential2 = false;
                model.isNonHandled = false;
                //model.isNotSendFax = false;
                model.isOutBusiness = false;
                model.isTrading = false;
                    
            }
            else if (rbMyData.Checked)
            {
                model.ato_manager = txtAtoManager.Text;
                model.isPotential1 = false;
                model.isPotential2 = false;
                model.isNonHandled = false;
                //model.isNotSendFax = false;
                model.isOutBusiness = false;
                model.isTrading = false;
            }
            else if (rbPotential1.Checked)
            {
                model.ato_manager = txtAtoManager.Text;
                model.isPotential1 = true;
                model.isPotential2 = false;
                model.isNonHandled = false;
                //model.isNotSendFax = false;
                model.isOutBusiness = false;
                model.isTrading = false;
            }
            else if (rbPotential2.Checked)
            {
                model.ato_manager = txtAtoManager.Text;
                model.isPotential1 = false;
                model.isPotential2 = true;
                model.isNonHandled = false;
                //model.isNotSendFax = false;
                model.isOutBusiness = false;
                model.isTrading = false;
            }
            else if (rbNonHandled.Checked)
            {
                model.ato_manager = txtAtoManager.Text;
                model.isPotential1 = false;
                model.isPotential2 = true;
                model.isNonHandled = true;
                //model.isNotSendFax = false;
                model.isOutBusiness = false;
                model.isTrading = false;
            }
            else
            {
                if (cModel != null)
                {
                    model.ato_manager = cModel.ato_manager;
                    model.isPotential1 = cModel.isPotential1;
                    model.isPotential2 = cModel.isPotential2;
                    model.isNonHandled = cModel.isNonHandled;
                    model.isOutBusiness = cModel.isOutBusiness;
                }
            }

            //영업맨트
            string sales_comments = "";
            dgvComments.EndEdit();
            foreach (DataGridViewRow row in dgvComments.Rows)
            {
                bool isChecked;
                if (row.Cells["chk"].Value == null || !bool.TryParse(row.Cells["chk"].Value.ToString(), out isChecked))
                    isChecked = false;
                if(isChecked)
                    sales_comments += "_" + row.Index.ToString();
            }
            model.sales_comment = sales_comments;

            //거래처 정보 재등록
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = companyRepository.RealDeleteCompany(model.id);
            sqlList.Add(sql);
            sql = companyRepository.InsertCompany(model);
            sqlList.Add(sql);

            //거래처 영업내용============================================================================
            CompanySalesModel sModel = new CompanySalesModel();
            sModel.company_id = id;
            sModel.sub_id = commonRepository.GetNextId("t_company_sales", "sub_id", "company_id", id.ToString());
            sModel.is_sales = true;
            sModel.contents = "거래처 정보수정";
            sModel.edit_user = um.user_name;
            sModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string pre_ato_manager = row.Cells["ato_manager"].Value.ToString();
            string ato_manager = txtAtoManager.Text;
            string pre_category = row.Cells["category"].Value.ToString();
            string category = "";
            if (rbCommonData.Checked)
                category = "공용DATA";
            else if (rbMyData.Checked)
                category = "내DATA";
            else if (rbPotential1.Checked)
                category = "잠재1";
            else if (rbPotential2.Checked)
                category = "잠재2";
            else if (rbNonHandled.Checked)
                category = "취급X";
            else
                category = row.Cells["category"].Value.ToString();

            sModel.from_ato_manager = pre_ato_manager;
            sModel.to_ato_manager = ato_manager;
            sModel.from_category = pre_category;
            sModel.to_category = category;

            //거래처 영업로그
            sql = salesPartnerRepository.InsertPartnerSales(sModel);
            sqlList.Add(sql);

            //거래처 알람정보
            sql = companyAlarmRepository.DeleteAlarm(id.ToString());
            sqlList.Add(sql);

            string alarm_txt = "";
            foreach (DataGridViewRow row in dgvAlarm.Rows)
            {
                DateTime alarm_date;
                if (row.Cells["alarm_division"].Value == null || string.IsNullOrEmpty(row.Cells["alarm_division"].Value.ToString()))
                {
                    messageBox.Show(this,"알람일정의 구분이 선택되지 않았습니다!");
                    this.Activate();
                    return;
                }
                else if (row.Cells["alarm_category"].Value == null || string.IsNullOrEmpty(row.Cells["alarm_category"].Value.ToString()))
                {
                    messageBox.Show(this,"알람일정의 카테고리가 입력되지 않았습니다!");
                    this.Activate();
                    return;
                }

                CompanyAlarmModel alarmModel = new CompanyAlarmModel();
                alarmModel.company_id = id;
                alarmModel.division = row.Cells["alarm_division"].Value.ToString();
                alarmModel.category = row.Cells["alarm_category"].Value.ToString();
                if (row.Cells["alarm_category"].Value.ToString() == "주알람")
                {
                    switch (row.Cells["alarm_date"].Value.ToString())
                    {
                        case "월":
                            alarmModel.alarm_date = "1";
                            break;
                        case "화":
                            alarmModel.alarm_date = "2";
                            break;
                        case "수":
                            alarmModel.alarm_date = "3";
                            break;
                        case "목":
                            alarmModel.alarm_date = "4";
                            break;
                        case "금":
                            alarmModel.alarm_date = "5";
                            break;
                    }
                }
                else
                    alarmModel.alarm_date = row.Cells["alarm_date"].Value.ToString();

                if (row.Cells["alarm_remark"].Value == null)
                    row.Cells["alarm_remark"].Value = string.Empty;
                alarmModel.alarm_remark = row.Cells["alarm_remark"].Value.ToString();
                alarmModel.edit_user = row.Cells["edit_user"].Value.ToString();
                alarmModel.updatetime = row.Cells["updatetime"].Value.ToString();
                alarmModel.alarm_complete = false;
                sql = companyAlarmRepository.InsertAlarm(alarmModel);
                sqlList.Add(sql);

                if (row.Cells["real_alarm_date"].Value != null && DateTime.TryParse(row.Cells["real_alarm_date"].Value.ToString(), out DateTime real_alarm_date))
                    alarm_txt += $" {alarmModel.alarm_date}_{alarmModel.category}_{alarmModel.division}_FALSE";
            }
            alarm_txt = alarm_txt.Trim().Replace(" ", ",");
            //재무제표===================================================================================
            sql = companyFinanceRepository.DeleteCompanyFinanc(sModel.company_id.ToString());
            sqlList.Add(sql);

            foreach (DataGridViewRow row in dgvFinance.Rows)
            {
                CompanyFinanceModel fModel = new CompanyFinanceModel();
                fModel.company_id = sModel.company_id;

                if (row.Cells["finance_year"].Value != null && int.TryParse(row.Cells["finance_year"].Value.ToString(), out int year))
                    fModel.year = year;

                if (row.Cells["capital_amount"].Value != null && double.TryParse(row.Cells["capital_amount"].Value.ToString(), out double capital_amount))
                    fModel.capital_amount = capital_amount;

                if (row.Cells["debt_amount"].Value != null && double.TryParse(row.Cells["debt_amount"].Value.ToString(), out double debt_amount))
                    fModel.debt_amount = debt_amount;

                if (row.Cells["sales_amount"].Value != null && double.TryParse(row.Cells["sales_amount"].Value.ToString(), out double sales_amount))
                    fModel.sales_amount = sales_amount;

                if (row.Cells["net_income_amount"].Value != null && double.TryParse(row.Cells["net_income_amount"].Value.ToString(), out double net_income_amount))
                    fModel.net_income_amount = net_income_amount;

                fModel.edit_user = row.Cells["finance_edit_user"].Value.ToString();

                if (row.Cells["finance_updatetime"].Value != null && DateTime.TryParse(row.Cells["finance_updatetime"].Value.ToString(), out DateTime finance_updatetime))
                    fModel.updatetime = finance_updatetime.ToString("yyyy-MM-dd HH:mm:ss");

                sql = companyFinanceRepository.InsertCompanyFinanc(fModel);
                sqlList.Add(sql);
            }
            //===========================================================================================
            //Execute
            if (commonRepository.UpdateTran(sqlList) == -1)
            {
                messageBox.Show(this,"등록중 에러가 발생했습니다.");
                this.Activate();
            }
            else
            {
                //AtoDt 변경
                if (sm != null)
                {
                    int atoRowIndex = Convert.ToInt32(row.Cells["table_index"].Value.ToString());
                    sm.UpdateAtoDt(atoRowIndex, "id", id.ToString());
                    sm.UpdateAtoDt(atoRowIndex, "registration_number", txtRegistrationNumber.Text);
                    sm.UpdateAtoDt(atoRowIndex, "group_name", txtGroupName.Text);
                    sm.UpdateAtoDt(atoRowIndex, "company", txtCompany.Text);
                    sm.UpdateAtoDt(atoRowIndex, "address", txtAddress.Text);
                    sm.UpdateAtoDt(atoRowIndex, "ceo", txtCeo.Text);
                    sm.UpdateAtoDt(atoRowIndex, "tel", txtTel.Text);
                    sm.UpdateAtoDt(atoRowIndex, "fax", txtFax.Text);
                    sm.UpdateAtoDt(atoRowIndex, "phone", txtPhone.Text);
                    sm.UpdateAtoDt(atoRowIndex, "other_phone", txtOtherPhone.Text);
                    sm.UpdateAtoDt(atoRowIndex, "company_manager", txtCompanyManager.Text);
                    sm.UpdateAtoDt(atoRowIndex, "company_manager_position", txtCompanyManagerPosition.Text);
                    sm.UpdateAtoDt(atoRowIndex, "email", txtEmail.Text);
                    sm.UpdateAtoDt(atoRowIndex, "remark", txtRemark.Text);
                    sm.UpdateAtoDt(atoRowIndex, "web", txtWeb.Text);
                    
                    sm.UpdateAtoDt(atoRowIndex, "sales_updatetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    sm.UpdateAtoDt(atoRowIndex, "sales_edit_user", um.user_name);
                    sm.UpdateAtoDt(atoRowIndex, "sales_contents", "거래처 정보수정");
                    if (DistributionRow.Cells["comment"].Value == null)
                        DistributionRow.Cells["comment"].Value = string.Empty;
                    sm.UpdateAtoDt(atoRowIndex, "distribution", DistributionRow.Cells["comment"].Value.ToString());
                    if (HandlingItemRow.Cells["comment"].Value == null)
                        HandlingItemRow.Cells["comment"].Value = string.Empty;
                    sm.UpdateAtoDt(atoRowIndex, "handling_item", HandlingItemRow.Cells["comment"].Value.ToString());
                    if (PayDateRow.Cells["comment"].Value == null)
                        PayDateRow.Cells["comment"].Value = string.Empty;
                    sm.UpdateAtoDt(atoRowIndex, "payment_date", PayDateRow.Cells["comment"].Value.ToString());
                    sm.UpdateAtoDt(atoRowIndex, "isPotential1", model.isPotential1);
                    sm.UpdateAtoDt(atoRowIndex, "isPotential2", model.isPotential2);
                    sm.UpdateAtoDt(atoRowIndex, "isNonHandled", model.isNonHandled);
                    sm.UpdateAtoDt(atoRowIndex, "isOutBusiness", model.isOutBusiness);
                    sm.UpdateAtoDt(atoRowIndex, "isNotSendFax", model.isNotSendFax);
                    sm.UpdateAtoDt(atoRowIndex, "isTrading", model.isTrading);
                    sm.UpdateAtoDt(atoRowIndex, "sales_comment", model.sales_comment);
                    sm.UpdateAtoDt(atoRowIndex, "alarm_date", alarm_txt);
                    /*sm.UpdateAtoDt(atoRowIndex, "alarm_month", model.alarm_month.ToString());
                    sm.UpdateAtoDt(atoRowIndex, "alarm_week", model.alarm_week.ToString());*/
                    sm.UpdateAtoDt(atoRowIndex, "alarm_complete_date", model.alarm_complete_date.ToString());
                    sm.UpdateAtoDt(atoRowIndex, "remark4", model.remark4.ToString());
                    sm.UpdateAtoDt(atoRowIndex, "remark5", model.remark5.ToString());
                    sm.UpdateAtoDt(atoRowIndex, "remark6", model.remark6.ToString());

                    sm.UpdateAtoDt(atoRowIndex, "category", category);
                    sm.UpdateAtoDt(atoRowIndex, "pre_ato_manager", ato_manager);
                    sm.UpdateAtoDt(atoRowIndex, "ato_manager", ato_manager);

                    sm.UpdateAtoDt(atoRowIndex, "industry_type", model.industry_type);
                    sm.UpdateAtoDt(atoRowIndex, "industry_type2", model.industry_type2);
                    sm.UpdateAtoDt(atoRowIndex, "ato_manager", ato_manager);



                    //DatagridviewRow 변경
                    sm.SetCellValueEvent(false);
                    row.Cells["id"].Value = id.ToString();
                    row.Cells["registration_number"].Value = txtRegistrationNumber.Text;
                    row.Cells["group_name"].Value = txtGroupName.Text;
                    row.Cells["company"].Value = txtCompany.Text;
                    row.Cells["address"].Value = txtAddress.Text;
                    row.Cells["ceo"].Value = txtCeo.Text;
                    row.Cells["tel"].Value = txtTel.Text;
                    row.Cells["fax"].Value = txtFax.Text;
                    row.Cells["phone"].Value = txtPhone.Text;
                    row.Cells["other_phone"].Value = txtOtherPhone.Text;
                    row.Cells["company_manager"].Value = txtCompanyManager.Text;
                    row.Cells["company_manager_position"].Value = txtCompanyManagerPosition.Text;
                    row.Cells["email"].Value = txtEmail.Text;
                    row.Cells["remark"].Value = txtRemark.Text;
                    row.Cells["web"].Value = txtWeb.Text;
                    row.Cells["pre_ato_manager"].Value = ato_manager;
                    row.Cells["ato_manager"].Value = ato_manager;
                    row.Cells["category"].Value = category;
                    row.Cells["sales_updatetime"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    row.Cells["sales_edit_user"].Value = um.user_name;
                    row.Cells["sales_contents"].Value = "거래처 정보수정";
                    row.Cells["distribution"].Value = DistributionRow.Cells["comment"].Value.ToString();
                    row.Cells["handling_item"].Value = HandlingItemRow.Cells["comment"].Value.ToString();
                    row.Cells["payment_date"].Value = PayDateRow.Cells["comment"].Value.ToString();
                    row.Cells["isPotential1"].Value = model.isPotential1;
                    row.Cells["isPotential2"].Value = model.isPotential2;
                    row.Cells["isNonHandled"].Value = model.isNonHandled;
                    row.Cells["isOutBusiness"].Value = model.isOutBusiness;
                    row.Cells["isNotSendFax"].Value = model.isNotSendFax;
                    row.Cells["isTrading"].Value = model.isTrading;
                    row.Cells["sales_comment"].Value = model.sales_comment;
                    row.Cells["alarm_complete_date"].Value = model.alarm_complete_date;
                    row.Cells["remark4"].Value = model.remark4;
                    row.Cells["remark5"].Value = model.remark5;
                    row.Cells["remark6"].Value = model.remark6;

                    row.Cells["industry_type"].Value = model.industry_type;
                    row.Cells["industry_type2"].Value = model.industry_type2;

                    //카테고리를 변경했을 경우 삭제여부
                    if (!rbUpdateInfo.Checked)
                    {
                        category = "";
                        if (rbCommonData.Checked)
                            category = "tabCommonData";
                        else if (rbMyData.Checked)
                            category = "tabRandomData";
                        else if (rbPotential1.Checked)
                            category = "tabCompany";
                        else if (rbPotential2.Checked)
                            category = "tabCompany2";
                        else if (rbNonHandled.Checked)
                            category = "tabNotTreatment";

                        sm.CheckCategory(category, cbSendFax.Checked, row.Index);
                    }
                    //알람 완료처리면 회색처리
                    if (isAlarmComplete && sm.isAlarmSheet())
                        row.DefaultCellStyle.ForeColor = Color.LightGray;

                    sm.SetCellValueEvent();
                }
                messageBox.Show(this,"등록완료");
                this.Activate();

                if (row.Index == -1)
                    this.Dispose();
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnPurchaseMargin_Click(object sender, EventArgs e)
        {
            int mainformx = this.Location.X;
            int mainformy = this.Location.Y; 
            int mainformwidth = this.Size.Width; 
            int mainformheight = this.Size.Height;

            AddCompanyInfoKeyHelper acik = new AddCompanyInfoKeyHelper();
            int childformwidth = acik.Size.Width; 
            int childformheight = acik.Size.Height;
            acik.Show();
            acik.Location = new Point(mainformx + (mainformwidth / 2) - (childformwidth / 2), mainformy + (mainformheight / 2) - (childformheight / 2));
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "영업거래처 관리", "거래처 관리", "is_delete"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            if (messageBox.Show(this, "거래처를 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            CompanyModel model = new CompanyModel();
            //기존ID가 없을경우 신규ID
            CompanyModel cModel = null;
            int id;
            if (!Int32.TryParse(lbCompanyId.Text, out id))
                id = 0;
            if (id == 0)
            {
                messageBox.Show(this,"거래처 정보를 찾을 수 없습니다.");
                this.Activate();
                return;
            }
            int step = 0;
            try
            {
                //거래처 정보 삭제
                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql = companyRepository.RealDeleteCompany(model.id);
                sqlList.Add(sql);
                //거래처 영업내용============================================================================
                CompanySalesModel sModel = new CompanySalesModel();
                sModel.company_id = id;
                sModel.sub_id = commonRepository.GetNextId("t_company_sales", "sub_id", "company_id", id.ToString());
                sModel.is_sales = false;
                sModel.contents = "거래처 정보삭제";
                sModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sModel.edit_user = um.user_name;

                sModel.from_ato_manager = txtAtoManager.Text;
                sModel.to_ato_manager = txtAtoManager.Text;
                sModel.from_category = row.Cells["category"].Value.ToString();
                sModel.to_category = "삭제거래처";

                sql = salesPartnerRepository.InsertPartnerSales(sModel);
                sqlList.Add(sql);
                step = 1;
                //===========================================================================================
                //Execute
                if (commonRepository.UpdateTran(sqlList) == -1)
                {
                    messageBox.Show(this, "등록중 에러가 발생했습니다.");
                    this.Activate();
                }
                else
                {
                    step = 2;
                    if (sm != null)
                    {
                        int atoRowIndex = Convert.ToInt32(row.Cells["table_index"].Value.ToString());
                        sm.UpdateAtoDt(atoRowIndex, "sales_updatetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:Ss"));
                        sm.UpdateAtoDt(atoRowIndex, "sales_edit_user", um.user_name);
                        sm.UpdateAtoDt(atoRowIndex, "sales_contents", "거래처 삭제");
                        sm.UpdateAtoDt(atoRowIndex, "category", "삭제거래처");
                        sm.UpdateAtoDt(atoRowIndex, "isDelete", true);

                        //카테고리를 변경했을 경우 삭제여부
                        sm.CheckCategory("tabNotTreatment", cbSendFax.Checked, row.Index);
                        step = 3;
                        this.Dispose();
                    }
                    else
                    {
                        step = 4;
                        messageBox.Show(this, "삭제완료");
                        this.Activate();
                    }   
                }
            }
            catch (Exception ex)
            {
                messageBox.Show(this, ex.Message + "\n");
            }
        }
        #endregion

        #region Key event
        private void AddCompanyInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        btnAddCompany.PerformClick();
                        break;
                    case Keys.D:
                        btnDelete.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.M:
                        if (dgvComments.Focused)
                            txtTel.Focus();
                        else
                            dgvComments.Focus();
                        break;
                }
            }
            else 
            {
                switch (e.KeyCode)
                {   
                    case Keys.F1:
                        rbUpdateInfo.Checked = true;
                        break;
                    case Keys.F3:
                        rbCommonData.Checked = true;
                        break;
                    case Keys.F4:
                        rbMyData.Checked = true;
                        break;
                    case Keys.F5:
                        rbPotential1.Checked = true;
                        break;
                    case Keys.F6:
                        rbPotential2.Checked = true;
                        break;
                    case Keys.F7:
                        rbNonHandled.Checked = true;
                        break;
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Control tb = common.FindFocusedControl(this);
            //Tab시작한 컬럼 기억해놓기
            if (keyData == (Keys.Control | Keys.Enter))
            {
                if (dgvComments.CurrentCell != null && dgvComments.Focused)
                {
                    DataGridViewRow row = dgvComments.Rows[dgvComments.CurrentCell.RowIndex];

                    if (row.Cells["division"].Value != null && row.Cells["division"].Value.ToString().Contains('.'))
                    {
                        bool isChecked;
                        if (row.Cells["chk"].Value == null || !bool.TryParse(row.Cells["chk"].Value.ToString(), out isChecked))
                            isChecked = false;
                        isChecked = !isChecked;
                        row.Cells["chk"].Value = isChecked;

                        return true;
                    }
                    else
                        return base.ProcessCmdKey(ref msg, keyData);
                }
                else
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
            }
        #endregion

        #region Datagridview event
        private void dgvComments_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvComments.Rows[e.RowIndex].Cells["division"].Value != null && dgvComments.Rows[e.RowIndex].Cells["division"].Value.ToString().Contains('.'))
                {
                    bool isVisible = !dgvComments.Rows[e.RowIndex + 1].Visible;
                    int rowindex = e.RowIndex + 1;
                    if (dgvComments.Rows[e.RowIndex + 1].Cells["division"].Value == null || !dgvComments.Rows[rowindex].Cells["division"].Value.ToString().Contains('-'))
                    {
                        isVisible = !dgvComments.Rows[e.RowIndex + 2].Visible;
                        rowindex = e.RowIndex + 2;
                    }

                    do
                    {
                        dgvComments.Rows[rowindex].Visible = isVisible;
                        rowindex++;
                    }
                    while (rowindex <= dgvComments.Rows.Count - 1 && dgvComments.Rows[rowindex].Cells["division"].Value != null && dgvComments.Rows[rowindex].Cells["division"].Value.ToString().Contains('-'));

                }
                else
                { 
                    
                }
            }
        }

        #endregion

        #region Alarm 관련
        private void dgvAlarm_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvAlarm.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                dgvAlarm.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void cbAlarmMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbAlarmMonth.Text.Trim()))
            {
                
                foreach (DataGridViewRow row in dgvAlarm.Rows)
                {
                    if (row.Cells["alarm_category"].Value.ToString().Equals("월알람") && row.Cells["alarm_date"].Value.ToString().Equals(cbAlarmMonth.Text.Trim()))
                    {
                        messageBox.Show(this, "이미 선택된 날짜입니다.");
                        return;
                    }
                }

                int n = dgvAlarm.Rows.Add();
                DataGridViewComboBoxCell cCell = new DataGridViewComboBoxCell();
                cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                cCell.Items.Add("발주");
                cCell.Items.Add("결제");
                cCell.Items.Add("신규");
                cCell.Items.Add("기타");
                dgvAlarm.Rows[n].Cells["alarm_division"] = cCell;
                dgvAlarm.Rows[n].Cells["alarm_division"].Value = cbAlarmDivision.Text;
                dgvAlarm.Rows[n].Cells["alarm_category"].Value = "월알람";
                dgvAlarm.Rows[n].Cells["alarm_date"].Value = cbAlarmMonth.Text.Trim();
                dgvAlarm.Rows[n].Cells["edit_user"].Value = um.user_name;
                dgvAlarm.Rows[n].Cells["updatetime"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        private void cbMonday_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            string week = "";
            switch (cb.Name)
            {
                case "cbMonday":
                    week = "월";
                    break;
                case "cbTuseday":
                    week = "화";
                    break;
                case "cbWednesday":
                    week = "수";
                    break;
                case "cbThursday":
                    week = "목";
                    break;
                case "cbFriday":
                    week = "금";
                    break;
            }

            //주알람 추가
            if (cb.Checked)
            {
                if (!string.IsNullOrEmpty(week) && !ExistAlarm("주알람", week))
                {
                    int n = dgvAlarm.Rows.Add();
                    DataGridViewComboBoxCell cCell = new DataGridViewComboBoxCell();
                    cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    cCell.Items.Add("발주");
                    cCell.Items.Add("결제");
                    cCell.Items.Add("신규");
                    cCell.Items.Add("기타");
                    dgvAlarm.Rows[n].Cells["alarm_division"] = cCell;
                    dgvAlarm.Rows[n].Cells["alarm_division"].Value = cbAlarmDivision.Text;
                    dgvAlarm.Rows[n].Cells["alarm_category"].Value = "주알람";
                    dgvAlarm.Rows[n].Cells["alarm_date"].Value = week;
                    dgvAlarm.Rows[n].Cells["edit_user"].Value = um.user_name;
                    dgvAlarm.Rows[n].Cells["updatetime"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            //주알람 해제
            else
            {
                foreach (DataGridViewRow row in dgvAlarm.Rows)
                {
                    if (row.Cells["alarm_category"].Value.ToString().Equals("주알람") && row.Cells["alarm_date"].Value.ToString().Equals(week))
                    {
                        dgvAlarm.Rows.Remove(row);
                        return;
                    }
                }
            }
        }

        public void AddAlarmEvent(DateTime dt)
        {
            if (!ExistAlarm("특정일자", dt.ToString("yyyy-MM-dd")))
            {
                int n = dgvAlarm.Rows.Add();
                DataGridViewComboBoxCell cCell = new DataGridViewComboBoxCell();
                cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                cCell.Items.Add("발주");
                cCell.Items.Add("결제");
                cCell.Items.Add("신규");
                cCell.Items.Add("기타");
                dgvAlarm.Rows[n].Cells["alarm_division"] = cCell;
                dgvAlarm.Rows[n].Cells["alarm_division"].Value = cbAlarmDivision.Text;
                dgvAlarm.Rows[n].Cells["alarm_category"].Value = "특정일자";
                dgvAlarm.Rows[n].Cells["alarm_date"].Value = dt.ToString("yyyy-MM-dd");
                dgvAlarm.Rows[n].Cells["edit_user"].Value = um.user_name;
                dgvAlarm.Rows[n].Cells["updatetime"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        private void cbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            int year;
            if (!int.TryParse(cbYear.Text, out year))
                year = DateTime.Now.Year;

            int month;
            if (!int.TryParse(cbMonth.Text, out month))
                month = DateTime.Now.Month;

            DateTime standardDt = new DateTime(year, month, 1);

            cbYear.Text = standardDt.Year.ToString();
            cbMonth.Text = standardDt.Month.ToString();

            GetCalendar(standardDt.Year, standardDt.Month);
        }

        #endregion

        #region 재무제표
        private void dgvFinance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dgvFinance.Columns[e.ColumnIndex].Name == "btnFinanceUpdate")
                {
                    int company_id;
                    if (!int.TryParse(lbCompanyId.Text, out company_id) || company_id <= 0)
                    {
                        messageBox.Show(this, "등록되지 않은 거래처입니다.");
                        return;
                    }

                    DataGridViewRow row = dgvFinance.Rows[e.RowIndex];
                    try
                    {
                        FinanceManager fm = new FinanceManager(this, um
                            , row.Cells["finance_year"].Value.ToString()
                            , row.Cells["capital_amount"].Value.ToString()
                            , row.Cells["debt_amount"].Value.ToString()
                            , row.Cells["sales_amount"].Value.ToString()
                            , row.Cells["net_income_amount"].Value.ToString()
                            , company_id.ToString());
                        fm.Owner = this;
                        fm.ShowDialog();
                    }
                    catch
                    { }
                }
            }
        }
        #endregion

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtIndustryType_TextChanged(object sender, EventArgs e)
        {
            txtIndustryTypeRank.Text = GetIndustryTypeRank(txtIndustryType.Text).ToString();
        }
    }
}
