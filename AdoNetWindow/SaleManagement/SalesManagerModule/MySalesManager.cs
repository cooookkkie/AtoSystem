using AdoNetWindow.Model;
using AdoNetWindow.SaleManagement.SalesManagerModule;
using Microsoft.VisualBasic.ApplicationServices;
using Repositories;
using Repositories.Calender;
using Repositories.Config;
using Repositories.SalesPartner;
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
    public partial class MySalesManager : Form
    {
        ISalesPartnerRepository salesPartnerRepository = new SalesPartnerRepository();
        ICommonRepository commonRepository = new CommonRepository();
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        IAnnualRepository annualRepository = new AnnualRepository();
        IUsersRepository usersRepository = new UsersRepository();   
        UsersModel um; 
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public MySalesManager(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
        }
        private void MySalesManager_Load(object sender, EventArgs e)
        {
            txtSttdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtManager.Text = um.user_name;
            lbTargetUser.Text = um.user_name + "님의 진행률";
            //로그인 유저정보 확인
            um = usersRepository.GetUserInfo(um.user_id);
            //일 목표량
            txtDailyTargetAmount.Text = um.daily_work_goals_amount.ToString("#,##0");
            //목표 년월
            DateTime sttMonth = new DateTime(2023, 01, 01);
            DateTime endMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
            for (DateTime tempMonth = sttMonth; tempMonth <= endMonth; tempMonth = tempMonth.AddMonths(1))
                cbTargetMonth.Items.Add(tempMonth.ToString("yyyy-MM"));

            cbTargetMonth.Text = DateTime.Now.ToString("yyyy-MM");

            //권한별 조회
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                //유저별 권한가져오기
                DataRow[] dr = authorityDt.Select("id = 1");
                if (dr.Length > 0)
                    authorityDt = dr.CopyToDataTable();
                //없으면 부서별 권한가져오기
                else
                {
                    dr = authorityDt.Select("id = 2");
                    authorityDt = dr.CopyToDataTable();
                }
                authorityDt.AcceptChanges();

                //사용가능한 메뉴 설정
                txtManager.Text = um.user_name;
                foreach (DataRow ddr in authorityDt.Rows)
                {
                    if ("영업거래처 관리" == ddr["group_name"].ToString() && "거래처 관리" == ddr["form_name"].ToString())
                    {
                        if (bool.TryParse(ddr["is_admin"].ToString(), out bool is_admin) && !is_admin)
                        {
                            lbManager.Visible = false;
                            txtManager.Visible = false;
                        }
                        break;
                    }
                }
            }
            btnSearching.PerformClick();
        }

        #region Button
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
            }
        }
        private void btnSearching_Click(object sender, EventArgs e)
        {
            dgvSales.Rows.Clear();
            DataTable saleDt = salesPartnerRepository.GetSaleInfo(txtSttdate.Text, txtEnddate.Text, "", txtCompany.Text, cbExactly.Checked, txtManager.Text);
            if (saleDt.Rows.Count > 0)
            {
                this.dgvSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSales_CellValueChanged);
                for (int i = 0; i < saleDt.Rows.Count; i++)
                {
                    int n = dgvSales.Rows.Add();
                    DataGridViewRow row = dgvSales.Rows[n];

                    row.Cells["company_id"].Value = saleDt.Rows[i]["company_id"].ToString();
                    row.Cells["sub_id"].Value = saleDt.Rows[i]["sub_id"].ToString();
                    row.Cells["company"].Value = saleDt.Rows[i]["company"].ToString();

                    DateTime dt;
                    if (DateTime.TryParse(saleDt.Rows[i]["updatetime"].ToString(), out dt))
                    {
                        row.Cells["updatetime_detail"].Value = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        row.Cells["updatetime"].Value = dt.ToString("yyyy-MM-dd");
                        //오후, 오전
                        int hour = dt.Hour;
                        if (hour < 13)
                        {
                            row.Cells["am"].Value = true;
                            row.Cells["pm"].Value = false;
                        }
                        else
                        {
                            row.Cells["am"].Value = false;
                            row.Cells["pm"].Value = true;
                        }
                    }

                    bool is_sales;
                    if (!bool.TryParse(saleDt.Rows[i]["is_sales"].ToString(), out is_sales))
                        is_sales = false;
                    row.Cells["is_sales"].Value = is_sales;

                    row.Cells["contents"].Value = saleDt.Rows[i]["contents"].ToString();

                    row.Cells["from_ato_manager"].Value = saleDt.Rows[i]["from_ato_manager"].ToString();
                    row.Cells["from_category"].Value = saleDt.Rows[i]["from_category"].ToString();
                    row.Cells["arrow"].Value = "→";
                    row.Cells["to_ato_manager"].Value = saleDt.Rows[i]["to_ato_manager"].ToString();
                    row.Cells["to_category"].Value = saleDt.Rows[i]["to_category"].ToString();
                    row.Cells["sale_remark"].Value = saleDt.Rows[i]["remark"].ToString();
                    row.Cells["edit_user"].Value = saleDt.Rows[i]["edit_user"].ToString();                    
                }
                this.dgvSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSales_CellValueChanged);
            }
            //총계
            int am, pm, potential1, potential2;
            SetCount(out am, out pm, out potential1, out potential2);
            txtTotal.Text = (am + pm).ToString("#,##0");
            txtAm.Text = am.ToString("#,##0");
            txtPm.Text = pm.ToString("#,##0");
            txtPotential1.Text = potential1.ToString("#,##0");
            txtPotential2.Text = potential2.ToString("#,##0");

            //일 / 월목표
            if (DateTime.TryParse(cbTargetMonth.Text + "-01", out DateTime sttDt))
            {
                int workDays = 0;
                DataTable vacationDt = annualRepository.GetAnnual(um.user_id, "", sttDt, sttDt.AddMonths(1).AddDays(-1));
                common.GetUserWorkDay(sttDt, sttDt.AddMonths(1).AddDays(-1), vacationDt, out workDays);
                //휴무를 뺀 영업일
                txtWorkDays.Text = workDays.ToString();

                //진행률 계산
                VacationSuccessRate(sttDt);
            }
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            dgvSales.EndEdit();
            if (dgvSales.Rows.Count > 0)
            {
                List<StringBuilder> sqlList = new List<StringBuilder>();
                for (int i = 0; i < dgvSales.Rows.Count; i++)
                {
                    bool isChecked = Convert.ToBoolean(dgvSales.Rows[i].Cells["chk"].Value);
                    if (isChecked)
                    {
                        CompanySalesModel model = new CompanySalesModel();
                        int company_id = 0;
                        if (dgvSales.Rows[i].Cells["company_id"].Value == null || !int.TryParse(dgvSales.Rows[i].Cells["company_id"].Value.ToString(), out company_id))
                        {
                            messageBox.Show(this,dgvSales.Rows[i].Cells["company"].Value.ToString() + " 거래처의 정보를 찾을 수 없습니다.");
                            this.Activate();
                            return;
                        }
                        model.company_id = company_id;

                        int sub_id = 0;
                        if (dgvSales.Rows[i].Cells["sub_id"].Value == null || !int.TryParse(dgvSales.Rows[i].Cells["sub_id"].Value.ToString(), out sub_id))
                        {
                            messageBox.Show(this,dgvSales.Rows[i].Cells["company"].Value.ToString() + " 거래처의 정보를 찾을 수 없습니다.");
                            this.Activate();
                            return;
                        }
                        model.sub_id = sub_id;  

                        DateTime updatetime;
                        if (dgvSales.Rows[i].Cells["updatetime_detail"].Value != null && DateTime.TryParse(dgvSales.Rows[i].Cells["updatetime_detail"].Value.ToString(), out updatetime))
                        {
                            if (Convert.ToBoolean(dgvSales.Rows[i].Cells["am"].Value))
                                model.updatetime = updatetime.ToString("yyyy-MM-dd") + " 12:00:00";
                            else
                                model.updatetime = updatetime.ToString("yyyy-MM-dd") + " 13:00:00";
                        }

                        bool is_sales;
                        if (dgvSales.Rows[i].Cells["is_sales"].Value == null || !bool.TryParse(dgvSales.Rows[i].Cells["is_sales"].Value.ToString(), out is_sales))
                            is_sales = false;
                        model.is_sales = is_sales;

                        if (dgvSales.Rows[i].Cells["edit_user"].Value == null)
                            dgvSales.Rows[i].Cells["edit_user"].Value = "";
                        model.edit_user = dgvSales.Rows[i].Cells["edit_user"].Value.ToString();

                        if (dgvSales.Rows[i].Cells["sale_remark"].Value == null)
                            dgvSales.Rows[i].Cells["sale_remark"].Value = "";
                        model.remark = dgvSales.Rows[i].Cells["sale_remark"].Value.ToString();

                        StringBuilder sql = salesPartnerRepository.UpdateSalesInfo(model);
                        sqlList.Add(sql);
                    }
                }
                if (sqlList.Count > 0)
                {
                    if (commonRepository.UpdateTran(sqlList) == -1)
                    {
                        messageBox.Show(this,"수정중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                    else
                        btnSearching.PerformClick();
                }
            }
        }
        #endregion

        #region Method
        public DataGridView GetDgv()
        {
            dgvSales.EndEdit();
            return dgvSales;
        }
        public void GetCompanySalesDetail(string company, int id)
        {
            txtSttdate.Text = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
            if (id == 0)
                return;
            txtCompany.Text = company;  
            dgvSales.Rows.Clear();
            DataTable saleDt = salesPartnerRepository.GetSaleInfo("", "", "", "", false, "", id.ToString());
            if (saleDt.Rows.Count > 0)
            {
                this.dgvSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSales_CellValueChanged);
                for (int i = 0; i < saleDt.Rows.Count; i++)
                {
                    int n = dgvSales.Rows.Add();
                    DataGridViewRow row = dgvSales.Rows[n];

                    row.Cells["company_id"].Value = saleDt.Rows[i]["company_id"].ToString();
                    row.Cells["sub_id"].Value = saleDt.Rows[i]["sub_id"].ToString();
                    row.Cells["company"].Value = saleDt.Rows[i]["company"].ToString();

                    DateTime dt;
                    if (DateTime.TryParse(saleDt.Rows[i]["updatetime"].ToString(), out dt))
                    {
                        row.Cells["updatetime_detail"].Value = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        row.Cells["updatetime"].Value = dt.ToString("yyyy-MM-dd");
                        //오후, 오전
                        int hour = dt.Hour;
                        if (hour < 13)
                        {
                            row.Cells["am"].Value = true;
                            row.Cells["pm"].Value = false;
                        }
                        else
                        {
                            row.Cells["am"].Value = false;
                            row.Cells["pm"].Value = true;
                        }
                    }

                    bool is_sales;
                    if (!bool.TryParse(saleDt.Rows[i]["is_sales"].ToString(), out is_sales))
                        is_sales = false;
                    row.Cells["is_sales"].Value = is_sales;

                    row.Cells["contents"].Value = saleDt.Rows[i]["contents"].ToString();

                    row.Cells["from_ato_manager"].Value = saleDt.Rows[i]["from_ato_manager"].ToString();
                    row.Cells["from_category"].Value = saleDt.Rows[i]["from_category"].ToString();
                    row.Cells["arrow"].Value = "→";
                    row.Cells["to_ato_manager"].Value = saleDt.Rows[i]["to_ato_manager"].ToString();
                    row.Cells["to_category"].Value = saleDt.Rows[i]["to_category"].ToString();
                    row.Cells["sale_remark"].Value = saleDt.Rows[i]["remark"].ToString();
                    row.Cells["edit_user"].Value = saleDt.Rows[i]["edit_user"].ToString();

                }
                this.dgvSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSales_CellValueChanged);
            }
            //총계
            int am, pm, potential1, potential2;
            SetCount(out am, out pm, out potential1, out potential2);
            txtTotal.Text = (am + pm).ToString("#,##0");
            txtAm.Text = am.ToString("#,##0");
            txtPm.Text = pm.ToString("#,##0");
            txtPotential1.Text = potential1.ToString("#,##0");
            txtPotential2.Text = potential2.ToString("#,##0");
        }
        List<string> companyList = new List<string>();
        private void SetCount(out int am, out int pm, out int potential1, out int potential2)
        {
            //초기화
            dgvSales.EndEdit();
            companyList = new List<string>();
            txtTotal.Text = "0";
            txtAm.Text = "0";
            txtPm.Text = "0";
            txtPotential1.Text = "0";
            txtPotential2.Text = "0";
            am = 0;
            pm = 0;
            potential1 = 0;
            potential2 = 0;
            //출력
            if (dgvSales.Rows.Count > 0)
            {
                for (int i = 0; i < dgvSales.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvSales.Rows[i];

                    int company_id = Convert.ToInt32(row.Cells["company_id"].Value.ToString());
                    DateTime updatetime = Convert.ToDateTime(row.Cells["updatetime"].Value.ToString());
                    string company_code = updatetime.ToString("yyyy-MM-dd") + "^" + company_id.ToString();
                    bool is_sales = Convert.ToBoolean(row.Cells["is_sales"].Value);
                    if (!companyList.Contains(company_code) && is_sales)
                    {
                        if (Convert.ToBoolean(row.Cells["am"].Value))
                            am++;
                        else
                            pm++;

                        if (row.Cells["to_category"].Value != null && row.Cells["to_category"].Value.ToString().Equals("잠재1"))
                            potential1++;
                        else if (row.Cells["to_category"].Value != null && row.Cells["to_category"].Value.ToString().Equals("잠재2"))
                            potential2++;

                        companyList.Add(company_code);
                    }
                }
            }
        }
        #endregion

        #region Datagridview event
        private void dgvSales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvSales.EndEdit();
            if (e.RowIndex >= 0)
            {
                bool isChecked;
                if (dgvSales.Columns[e.ColumnIndex].Name == "am")
                {
                    isChecked = Convert.ToBoolean(dgvSales.Rows[e.RowIndex].Cells["am"].Value);
                    dgvSales.Rows[e.RowIndex].Cells["am"].Value = isChecked;
                    dgvSales.Rows[e.RowIndex].Cells["pm"].Value = !isChecked;
                }
                else if (dgvSales.Columns[e.ColumnIndex].Name == "pm")
                {
                    isChecked = Convert.ToBoolean(dgvSales.Rows[e.RowIndex].Cells["pm"].Value);
                    dgvSales.Rows[e.RowIndex].Cells["pm"].Value = isChecked;
                    dgvSales.Rows[e.RowIndex].Cells["am"].Value = !isChecked;
                }

                //총계
                int am, pm, potential1, potential2;
                SetCount(out am, out pm, out potential1, out potential2);
                txtTotal.Text = (am + pm).ToString("#,##0");
                txtAm.Text = am.ToString("#,##0");
                txtPm.Text = pm.ToString("#,##0");
                txtPotential1.Text = potential1.ToString("#,##0");
                txtPotential2.Text = potential2.ToString("#,##0");

            }
        }

        private void dgvSales_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dgvSales.Columns[e.ColumnIndex].Name != "chk")
                    dgvSales.Rows[e.RowIndex].Cells["chk"].Value = true;
                else
                {
                    dgvSales.EndEdit();
                    if (Convert.ToBoolean(dgvSales.Rows[e.RowIndex].Cells["chk"].Value))
                        dgvSales.Rows[e.RowIndex].Cells["chk"].Style.BackColor = Color.Red;
                    else
                        dgvSales.Rows[e.RowIndex].Cells["chk"].Style.BackColor = Color.White;
                }
            }
        }

        #endregion

        #region Key event
        private void txtSttdate_KeyDown(object sender, KeyEventArgs e)
        {
            Control tb = (Control)sender;
            if (e.KeyCode == Keys.Enter)
            {
                tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                //날짜 완성
                Control tbb = (Control)sender;
                tbb.Text = common.strDatetime(tbb.Text);
            }
        }
        private void MySalesManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearching.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.A:
                        btnInsert.PerformClick();
                        break;
                    case Keys.M:
                        txtCompany.Focus();
                        break;
                    case Keys.N:
                        txtCompany.Text = String.Empty;
                        txtCompany.Focus();
                        break;
                }
            }
        }


        #endregion

        private void cbTargetMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DateTime.TryParse(cbTargetMonth.Text + "-01", out DateTime sttDt))
            {
                int workDays = 0;
                DataTable vacationDt = annualRepository.GetAnnual(um.user_id, "", sttDt, sttDt.AddMonths(1).AddDays(-1));
                common.GetUserWorkDay(sttDt, sttDt.AddMonths(1).AddDays(-1), vacationDt, out workDays);
                //휴무를 뺀 영업일
                txtWorkDays.Text = workDays.ToString();

                //진행률 계산
                VacationSuccessRate(sttDt);
            }
        }

        private void VacationSuccessRate(DateTime standard_date)
        {
            //일 목표량
            txtDailyTargetAmount.Text = usersRepository.GetDailyTarget(txtManager.Text).ToString();
            //영업일
            int work_days;
            if (!int.TryParse(txtWorkDays.Text, out work_days))
                work_days = 0;

            //일 목표량
            int daily_target_amount;
            if (!int.TryParse(txtDailyTargetAmount.Text, out daily_target_amount))
                daily_target_amount = 0;

            //월 목표량
            int monthly_target_amount = daily_target_amount * work_days;
            txtMonthTargetAmount.Text = monthly_target_amount.ToString("#,##0");

            //일일 목표달성률
            DataTable dailyDt = salesPartnerRepository.GetUserSaleDashboard(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), txtManager.Text);
            int daily_success_amount = 0;
            if (dailyDt != null && dailyDt.Rows.Count > 0)
                daily_success_amount = Convert.ToInt32(dailyDt.Rows[0]["am_cnt"].ToString()) + Convert.ToInt32(dailyDt.Rows[0]["pm_cnt"].ToString());

            txtDailySuccessRate.Text = daily_success_amount.ToString("#,##0") + " / " + daily_target_amount.ToString("#,##0");
            pbDailySuccessRate.Maximum = daily_target_amount;
            if(daily_success_amount > daily_target_amount)
                daily_success_amount = daily_target_amount;
            pbDailySuccessRate.Value = daily_success_amount;

            double daily_rate = 0;
            if(daily_target_amount > 0)
                daily_rate = (double)daily_success_amount / (double)daily_target_amount;
            if (double.IsNaN(daily_rate))
                daily_rate = 0;
            else if (double.IsInfinity(daily_rate))
                daily_rate = 1;

            lbDailySuccessRate.Text = (daily_rate * 100).ToString("#,##0") + "%";

            //월별 목표량
            DateTime sttdate = new DateTime(standard_date.Year, standard_date.Month, 1);
            DateTime enddate = new DateTime(standard_date.Year, standard_date.Month, 1).AddMonths(1).AddDays(-1);
            DataTable monthlyDt = salesPartnerRepository.GetUserSaleDashboard(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"), txtManager.Text);
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

        private void txtDailyTargetAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (int.TryParse(txtDailyTargetAmount.Text, out int daily_target_amont))
                {
                    if (messageBox.Show(this, "일 목표량을 등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        StringBuilder sql = annualRepository.UpdateDailyTarget(um.user_id, daily_target_amont);
                        List<StringBuilder> sqlList = new List<StringBuilder>();
                        sqlList.Add(sql);
                        if (commonRepository.UpdateTran(sqlList) == -1)
                            messageBox.Show(this, "등록 중 에러가 발생하였습니다!");
                    }

                    if (DateTime.TryParse(cbTargetMonth.Text + "-01", out DateTime sttDt))
                        VacationSuccessRate(sttDt);
                }
                else
                {
                    txtDailyTargetAmount.Focus();
                    messageBox.Show(this, "목표량 입력값이 숫자 형식이 아닙니다!");
                }
            }
        }
    }
}
