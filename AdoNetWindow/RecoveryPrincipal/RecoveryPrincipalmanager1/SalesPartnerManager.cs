using AdoNetWindow.Model;
using Repositories;
using Repositories.RecoveryPrincipal;
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
using System.Reflection;
using Repositories.Company;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Repositories.Config;
using AdoNetWindow.Common;

namespace AdoNetWindow.RecoveryPrincipal
{
    public partial class SalesPartnerManager : Form
    {
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        ICompanyRecoveryRepository companyRecoveryRepository = new CompanyRecoveryRepository();
        ICommonRepository commonRepository = new CommonRepository();
        ICompanyRepository companyRepository = new CompanyRepository();
        ISalesPartnerRepository salesPartnerRepository = new SalesPartnerRepository();
        IRecoveryPrincipalPriceRepository recoveryPrincipalPriceRepository = new RecoveryPrincipalPriceRepository();
        UsersModel um;

        //Excel
        static Microsoft.Office.Interop.Excel.Application excelApp = null;
        static Microsoft.Office.Interop.Excel.Workbook workBook = null;
        static Microsoft.Office.Interop.Excel.Worksheet workSheet = null;

        public SalesPartnerManager(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
        }

        private void SalesPartnerManager_Load(object sender, EventArgs e)
        {
            for (int i = 2016; i <= DateTime.Now.Year; i++)
            { 
                cbYear.Items.Add(i.ToString());
            }

            cbYear.Text = DateTime.Now.AddMonths(-1).Year.ToString();
            cbMonth.Text = DateTime.Now.AddMonths(-1).Month.ToString();

            SetHeaderStyleSetting();

            //업체별시세현황 스토어프로시져 호출
            recoveryPrincipalPriceRepository.SetSeaoverId(um.seaover_id);

            dgvCompany.DoubleBuffered(true);
        }

        #region Method
        private void SetManagementImg()
        {
            this.dgvCompany.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                DataGridViewRow row = dgvCompany.Rows[i];

                bool isManage1;
                if (row.Cells["is_manage1"].Value == null || !bool.TryParse(row.Cells["is_manage1"].Value.ToString(), out isManage1))
                    isManage1 = false;
                bool isManage2;
                if (row.Cells["is_manage2"].Value == null || !bool.TryParse(row.Cells["is_manage2"].Value.ToString(), out isManage2))
                    isManage2 = false;
                bool isManage3;
                if (row.Cells["is_manage3"].Value == null || !bool.TryParse(row.Cells["is_manage3"].Value.ToString(), out isManage3))
                    isManage3 = false;
                bool isManage4;
                if (row.Cells["is_manage4"].Value == null || !bool.TryParse(row.Cells["is_manage4"].Value.ToString(), out isManage4))
                    isManage4 = false;

                if (isManage1)
                    row.Cells["img"].Value = Properties.Resources.Star_icon1;
                else if (isManage2)
                    row.Cells["img"].Value = Properties.Resources.Star_icon2;
                else if (isManage3)
                    row.Cells["img"].Value = Properties.Resources.Star_icon3;
                else if (isManage4)
                    row.Cells["img"].Value = Properties.Resources.Star_icon4;
                else
                    row.Cells["img"].Value = null;
            }
            this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
        }
        private void AtoCapitalRateInsert(DataGridViewRow row)
        {
            if (row.Cells["company"].Value == null || string.IsNullOrEmpty(row.Cells["company"].Value.ToString()))
            {
                MessageBox.Show(this, "거래처를 선택해주세요.");
                this.Activate();
                return;
            }
            //Seaover 거래처 내역
            DataTable companyDt = seaoverRepository.SelectData("업체별월별매출현황", "*", "", $"거래처명='{row.Cells["company"].Value.ToString()}'", "사용자 DESC");
            if (companyDt.Rows.Count == 0)
            {
                MessageBox.Show(this,"거래처 정보를 찾을 수 없습니다.");
                this.Activate();
                return;
            }
            //Ato 전산에 등록된 거래처 내역
            DataTable atoCompanyDt = companyRepository.GetCompanyRecovery("", "", false, "", companyDt.Rows[0]["거래처코드"].ToString());
            //Msg
            if (MessageBox.Show(this, "등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = new StringBuilder();

            //Recovery data matching
            CompanyRecoveryModel rm = new CompanyRecoveryModel();

            rm.company_code = companyDt.Rows[0]["거래처코드"].ToString();

            float last_sales_profit;
            if (!float.TryParse(txtSalesProfit.Text, out last_sales_profit))
                last_sales_profit = 0;
            rm.last_year_profit = last_sales_profit;

            float net_operating_capital_rate;
            if (!float.TryParse(txtNetWorkingCapital.Text, out net_operating_capital_rate))
                net_operating_capital_rate = 0;
            rm.net_operating_capital_rate = net_operating_capital_rate;

            float operating_capital_rate;
            if (!float.TryParse(txtWorkingCapital.Text, out operating_capital_rate))
                operating_capital_rate = 0;
            rm.operating_capital_rate = operating_capital_rate;

            float ato_capital_rate;
            if (!float.TryParse(txtAtoCapital.Text, out ato_capital_rate))
                ato_capital_rate = 0;
            rm.ato_capital_rate = ato_capital_rate;

            float equity_capital_rate;
            if (!float.TryParse(txtEquityCapital.Text, out equity_capital_rate))
                equity_capital_rate = 0;
            rm.equity_capital_rate = equity_capital_rate;

            int target_recovery_month;
            if (!int.TryParse(txtEquityCapital.Text, out target_recovery_month))
                target_recovery_month = 0;
            rm.target_recovery_month = target_recovery_month;

            if(atoCompanyDt.Rows.Count == 0)
                rm.remark = "";
            else
                rm.remark = atoCompanyDt.Rows[0]["recovery_remark"].ToString();
            rm.edit_user = um.user_name;
            rm.updatetime = DateTime.Now.ToString("yyyy-MM-dd");

            //Insert
            sql = companyRecoveryRepository.DeleteCompany(rm.company_code);
            sqlList.Add(sql);

            //Insert
            sql = companyRecoveryRepository.InsertCompany(rm);
            sqlList.Add(sql);

            //Execute
            int results = commonRepository.UpdateTran(sqlList);
            if (results == -1)
            {
                MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                this.Activate();
            }
        }
        private void InsertRecoveryPrincipal(DataGridViewRow row, int manage_type = 1)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "영업거래처 관리", "원금회수율 관리", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = new StringBuilder();

            //Seaover 거래처 내역
            DataTable companyDt = seaoverRepository.SelectData("업체별월별매출현황", "*", "", $"거래처명='{row.Cells["company"].Value.ToString()}'", "사용자 DESC");
            if (companyDt.Rows.Count == 0)
            {
                MessageBox.Show(this,"거래처 정보를 찾을 수 없습니다.");
                this.Activate();
                return;
            }
            //Ato 전산에 등록된 거래처 내역
            DataTable atoCompanyDt = companyRepository.GetCompanyRecovery("", "", false, "", companyDt.Rows[0]["거래처코드"].ToString());
            CompanyModel cm = new CompanyModel();
            if (atoCompanyDt.Rows.Count == 0)
            {
                cm.id = commonRepository.GetNextId("t_company", "id");
                cm.division = "매출처";
                cm.registration_number = companyDt.Rows[0]["사업자번호"].ToString();
                cm.group_name = "";
                cm.name = companyDt.Rows[0]["거래처명"].ToString();
                cm.origin = "국내";
                cm.address = "";
                cm.ceo = companyDt.Rows[0]["대표자명"].ToString();
                cm.tel = companyDt.Rows[0]["전화번호"].ToString();
                cm.fax = companyDt.Rows[0]["팩스번호"].ToString();
                cm.phone = companyDt.Rows[0]["휴대폰"].ToString();
                cm.company_manager = "";
                cm.company_manager_position = "";
                cm.email = "";
                cm.sns1 = "";
                cm.sns2 = "";
                cm.sns3 = "";
                cm.web = "";
                cm.isTrading = true;
                cm.remark = companyDt.Rows[0]["참고사항"].ToString();
                cm.ato_manager = companyDt.Rows[0]["매출자"].ToString();

                switch (manage_type)
                {
                    case 1:
                        cm.isManagement1 = true;
                        cm.isManagement2 = false;
                        cm.isManagement3 = false;
                        cm.isManagement4 = false;
                        break;
                    case 2:
                        cm.isManagement1 = false;
                        cm.isManagement2 = true;
                        cm.isManagement3 = false;
                        cm.isManagement4 = false;
                        break;
                    case 3:
                        cm.isManagement1 = false;
                        cm.isManagement2 = false;
                        cm.isManagement3 = true;
                        cm.isManagement4 = false;
                        break;
                    case 4:
                        cm.isManagement1 = false;
                        cm.isManagement2 = false;
                        cm.isManagement3 = false;
                        cm.isManagement4 = true;
                        break;

                }
                cm.isHide = false;
                cm.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                cm.createtime = DateTime.Now.ToString("yyyy-MM-dd");
                cm.edit_user = um.user_name;
                cm.seaover_company_code = companyDt.Rows[0]["거래처코드"].ToString();

                //Insert
                sql = companyRepository.InsertCompany(cm);
                sqlList.Add(sql);

            }
            else
            {
                cm.division = "매출처";
                cm.registration_number = atoCompanyDt.Rows[0]["registration_number"].ToString();
                cm.group_name = atoCompanyDt.Rows[0]["group_name"].ToString();
                cm.name = atoCompanyDt.Rows[0]["company"].ToString();
                cm.origin = atoCompanyDt.Rows[0]["origin"].ToString();
                cm.address = atoCompanyDt.Rows[0]["address"].ToString();
                cm.ceo = atoCompanyDt.Rows[0]["ceo"].ToString();
                cm.tel = atoCompanyDt.Rows[0]["tel"].ToString();
                cm.fax = atoCompanyDt.Rows[0]["fax"].ToString();
                cm.phone = atoCompanyDt.Rows[0]["phone"].ToString();
                cm.company_manager = atoCompanyDt.Rows[0]["company_manager"].ToString();
                cm.company_manager_position = atoCompanyDt.Rows[0]["company_manager_position"].ToString();
                cm.email = atoCompanyDt.Rows[0]["email"].ToString();
                cm.sns1 = atoCompanyDt.Rows[0]["sns1"].ToString();
                cm.sns2 = atoCompanyDt.Rows[0]["sns2"].ToString();
                cm.sns3 = atoCompanyDt.Rows[0]["sns3"].ToString();
                cm.web = atoCompanyDt.Rows[0]["web"].ToString();
                cm.remark = atoCompanyDt.Rows[0]["remark"].ToString();
                cm.ato_manager = atoCompanyDt.Rows[0]["ato_manager"].ToString();

                bool isManagement1;
                if (!bool.TryParse(atoCompanyDt.Rows[0]["isManagement1"].ToString(), out isManagement1))
                    isManagement1 = false;
                bool isManagement2;
                if (!bool.TryParse(atoCompanyDt.Rows[0]["isManagement2"].ToString(), out isManagement2))
                    isManagement2 = false;
                bool isManagement3;
                if (!bool.TryParse(atoCompanyDt.Rows[0]["isManagement3"].ToString(), out isManagement3))
                    isManagement3 = false;
                bool isManagement4;
                if (!bool.TryParse(atoCompanyDt.Rows[0]["isManagement4"].ToString(), out isManagement4))
                    isManagement4 = false;

                switch (manage_type)
                {
                    case 1:
                        cm.isManagement1 = !isManagement1;
                        cm.isManagement2 = false;
                        cm.isManagement3 = false;
                        cm.isManagement4 = false;
                        break;
                    case 2:
                        cm.isManagement1 = false;
                        cm.isManagement2 = !isManagement2;
                        cm.isManagement3 = false;
                        cm.isManagement4 = false;
                        break;
                    case 3:
                        cm.isManagement1 = false;
                        cm.isManagement2 = false;
                        cm.isManagement3 = !isManagement3;
                        cm.isManagement4 = false;
                        break;
                    case 4:
                        cm.isManagement1 = false;
                        cm.isManagement2 = false;
                        cm.isManagement3 = false;
                        cm.isManagement4 = !isManagement4;
                        break;

                }
                cm.isHide = Convert.ToBoolean(atoCompanyDt.Rows[0]["isHide"].ToString());
                cm.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                cm.createtime = atoCompanyDt.Rows[0]["createtime"].ToString();
                cm.edit_user = um.user_name;
                cm.seaover_company_code = companyDt.Rows[0]["거래처코드"].ToString();
                cm.id = Convert.ToInt32(atoCompanyDt.Rows[0]["id"].ToString());
                //Update
                sql = companyRepository.UpdateCompany(cm);
                sqlList.Add(sql);
            }

            row.Cells["is_manage1"].Value = cm.isManagement1;
            row.Cells["is_manage2"].Value = cm.isManagement2;
            row.Cells["is_manage3"].Value = cm.isManagement3;
            row.Cells["is_manage4"].Value = cm.isManagement4;

            //Execute
            int results = commonRepository.UpdateTran(sqlList);
            if (results == -1)
            {
                MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                this.Activate();
            }
        }
        private void InsertHideCompany(DataGridViewRow row)
        {

            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "영업거래처 관리", "원금회수율 관리", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = new StringBuilder();

            //Seaover 거래처 내역
            DataTable companyDt = seaoverRepository.SelectData("업체별월별매출현황", "*", "", $"거래처명='{row.Cells["company"].Value.ToString()}'", "사용자 DESC");
            if (companyDt.Rows.Count == 0)
            {
                MessageBox.Show(this,"거래처 정보를 찾을 수 없습니다.");
                this.Activate();
                return;
            }
            //Ato 전산에 등록된 거래처 내역
            DataTable atoCompanyDt = companyRepository.GetCompanyRecovery("", "", false, "", companyDt.Rows[0]["거래처코드"].ToString());
            CompanyModel cm = new CompanyModel();
            if (atoCompanyDt.Rows.Count == 0)
            {
                cm.id = commonRepository.GetNextId("t_company", "id");
                cm.division = "매출처";
                cm.registration_number = companyDt.Rows[0]["사업자번호"].ToString();
                cm.group_name = "";
                cm.name = companyDt.Rows[0]["거래처명"].ToString();
                cm.origin = "국내";
                cm.address = "";
                cm.ceo = companyDt.Rows[0]["대표자명"].ToString();
                cm.tel = companyDt.Rows[0]["전화번호"].ToString();
                cm.fax = companyDt.Rows[0]["팩스번호"].ToString();
                cm.phone = companyDt.Rows[0]["휴대폰"].ToString();
                cm.company_manager = "";
                cm.company_manager_position = "";
                cm.email = "";
                cm.sns1 = "";
                cm.sns2 = "";
                cm.sns3 = "";
                cm.web = "";
                cm.remark = companyDt.Rows[0]["참고사항"].ToString();
                cm.ato_manager = companyDt.Rows[0]["매출자"].ToString();
                cm.isManagement1 = false;
                cm.isManagement2 = false;
                cm.isManagement3 = false;
                cm.isManagement4 = false;
                cm.isTrading = true;
                cm.isHide = true;
                cm.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                cm.createtime = DateTime.Now.ToString("yyyy-MM-dd");
                cm.edit_user = um.user_name;
                cm.seaover_company_code = companyDt.Rows[0]["거래처코드"].ToString();

                //Insert
                sql = companyRepository.InsertCompany(cm);
                sqlList.Add(sql);

            }
            else
            {
                cm.division = "매출처";
                cm.registration_number = atoCompanyDt.Rows[0]["registration_number"].ToString();
                cm.group_name = atoCompanyDt.Rows[0]["group_name"].ToString();
                cm.name = atoCompanyDt.Rows[0]["company"].ToString();
                cm.origin = atoCompanyDt.Rows[0]["origin"].ToString();
                cm.address = atoCompanyDt.Rows[0]["address"].ToString();
                cm.ceo = atoCompanyDt.Rows[0]["ceo"].ToString();
                cm.tel = atoCompanyDt.Rows[0]["tel"].ToString();
                cm.fax = atoCompanyDt.Rows[0]["fax"].ToString();
                cm.phone = atoCompanyDt.Rows[0]["phone"].ToString();
                cm.company_manager = atoCompanyDt.Rows[0]["company_manager"].ToString();
                cm.company_manager_position = atoCompanyDt.Rows[0]["company_manager_position"].ToString();
                cm.email = atoCompanyDt.Rows[0]["email"].ToString();
                cm.sns1 = atoCompanyDt.Rows[0]["sns1"].ToString();
                cm.sns2 = atoCompanyDt.Rows[0]["sns2"].ToString();
                cm.sns3 = atoCompanyDt.Rows[0]["sns3"].ToString();
                cm.web = atoCompanyDt.Rows[0]["web"].ToString();
                cm.remark = atoCompanyDt.Rows[0]["remark"].ToString();
                cm.ato_manager = atoCompanyDt.Rows[0]["ato_manager"].ToString();

                bool isManagement1;
                if (!bool.TryParse(atoCompanyDt.Rows[0]["isManagement1"].ToString(), out isManagement1))
                    isManagement1 = false;
                bool isManagement2;
                if (!bool.TryParse(atoCompanyDt.Rows[0]["isManagement2"].ToString(), out isManagement2))
                    isManagement2 = false;
                bool isManagement3;
                if (!bool.TryParse(atoCompanyDt.Rows[0]["isManagement3"].ToString(), out isManagement3))
                    isManagement3 = false;
                bool isManagement4;
                if (!bool.TryParse(atoCompanyDt.Rows[0]["isManagement4"].ToString(), out isManagement4))
                    isManagement4 = false;

                cm.isManagement1 = isManagement1;
                cm.isManagement2 = isManagement1;
                cm.isManagement3 = isManagement1;
                cm.isManagement4 = isManagement1;
                bool isHide;
                if (!bool.TryParse(atoCompanyDt.Rows[0]["isHide"].ToString(), out isHide))
                    isHide = false;
                cm.isHide = !isHide;
                cm.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                cm.createtime = atoCompanyDt.Rows[0]["createtime"].ToString();
                cm.edit_user = um.user_name;
                cm.seaover_company_code = companyDt.Rows[0]["거래처코드"].ToString();
                cm.id = Convert.ToInt32(atoCompanyDt.Rows[0]["id"].ToString());
                //Update
                sql = companyRepository.UpdateCompany(cm);
                sqlList.Add(sql);
            }

            //Execute
            int results = commonRepository.UpdateTran(sqlList);
            if (results == -1)
            {
                MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                this.Activate();
            }
        }
        private void calculate()
        {
            this.dgvCompany.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            //회전율 및 재무재표 내역
            double net_working_capital;
            if (!double.TryParse(txtNetWorkingCapital.Text, out net_working_capital))
                net_working_capital = 0;
            double working_capital;
            if (!double.TryParse(txtWorkingCapital.Text, out working_capital))
                working_capital = 0;
            double ato_capital;
            if (!double.TryParse(txtAtoCapital.Text, out ato_capital))
                ato_capital = 0;
            else
                ato_capital = ato_capital / 100;
            double equity_capital;
            if (!double.TryParse(txtEquityCapital.Text, out equity_capital))
                equity_capital = 0;
            double sales_profit;
            if (!double.TryParse(txtSalesProfit.Text, out sales_profit))
                sales_profit = 0;
            else
                sales_profit = sales_profit / 100;

            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                DataGridViewRow row = dgvCompany.Rows[i];

                //회전수, 회전일수
                double balance_amount = Convert.ToDouble(row.Cells["current_balance"].Value.ToString());


                //한달, 누적마진
                double current_margin = Convert.ToDouble(row.Cells["month_margin"].Value.ToString());
                double pre_margin = Convert.ToDouble(row.Cells["pre_cumulative_margin"].Value.ToString());

                //평균잔액
                double average_balance_amount = Convert.ToDouble(row.Cells["average_balance"].Value.ToString());

                //손익분기잔액1
                double margin_price = balance_amount - pre_margin;
                //원금회수기간1
                double recovery_month = margin_price / current_margin;
                //업체등급
                int company_grade;
                string company_grade_remark;
                GetGrade(recovery_month, out company_grade, out company_grade_remark);

                if (company_grade < 0)
                    company_grade = 0;
                else if (company_grade > 100)
                    company_grade = 100;

                //누적 기회비용
                double accumulate_balance_amount = Convert.ToDouble(row.Cells["accumulate_balance_amount"].Value.ToString());
                double accumulate_average_balance_amount = Convert.ToDouble(row.Cells["accumulate_average_balance_amount"].Value.ToString());

                double accumulate_net_working_capital_opportunity_cost = net_working_capital * accumulate_balance_amount * sales_profit / 12;
                double accumulate_working_capital_opportunity_cost = working_capital * accumulate_balance_amount * sales_profit / 12;
                double accumulate_ato_capital_opportunity_cost = ato_capital * accumulate_average_balance_amount * 3;
                double accumulate_equity_capital_opportunity_cost = equity_capital * accumulate_balance_amount * sales_profit / 12;

                //발생 기회비용
                double net_working_capital_opportunity_cost = net_working_capital * balance_amount * sales_profit / 12;
                double working_capital_opportunity_cost = working_capital * balance_amount * sales_profit / 12;
                double ato_capital_opportunity_cost = ato_capital * average_balance_amount * 3;
                double equity_capital_opportunity_cost = equity_capital * balance_amount * sales_profit / 12;


                //손익분기잔액2
                double month_capital_amount = 0;
                double accumulate_month_capital_amount = 0;
                if (rbNetWorkingCapital.Checked)
                {
                    month_capital_amount = net_working_capital_opportunity_cost;
                    accumulate_month_capital_amount = accumulate_net_working_capital_opportunity_cost + net_working_capital_opportunity_cost;

                    margin_price += accumulate_net_working_capital_opportunity_cost + net_working_capital_opportunity_cost;
                }
                else if (rbWorkingCapital.Checked)
                {
                    month_capital_amount = working_capital_opportunity_cost;
                    accumulate_month_capital_amount = accumulate_working_capital_opportunity_cost + working_capital_opportunity_cost;

                    margin_price += accumulate_working_capital_opportunity_cost + working_capital_opportunity_cost;
                }
                else if (rbAtoCapital.Checked)
                {
                    month_capital_amount = ato_capital_opportunity_cost;
                    accumulate_month_capital_amount = accumulate_ato_capital_opportunity_cost + ato_capital_opportunity_cost;

                    margin_price += accumulate_ato_capital_opportunity_cost + ato_capital_opportunity_cost;
                }
                else if (rbEquityCapital.Checked)
                {
                    month_capital_amount = equity_capital_opportunity_cost;
                    accumulate_month_capital_amount = accumulate_equity_capital_opportunity_cost + equity_capital_opportunity_cost;

                    margin_price += accumulate_equity_capital_opportunity_cost + equity_capital_opportunity_cost;
                }
                row.Cells["capital_amount"].Value = month_capital_amount.ToString("#,##0");
                row.Cells["accumulate_capital_amount"].Value = accumulate_month_capital_amount.ToString("#,##0");



                //원금회수기간2
                if (margin_price < 0)
                    recovery_month = margin_price / 0;
                else
                {
                    double real_month_margin = current_margin - month_capital_amount;

                    if (real_month_margin <= 0)
                        recovery_month = margin_price / 0;
                    else
                        recovery_month = margin_price / real_month_margin;  //손익분기잔액1 / (한달마진 - 당월발생 기회비용)
                }

                if (double.IsNaN(recovery_month))  //Nan
                    recovery_month = 0;
                else if (double.IsInfinity(recovery_month))  //Infinity
                    recovery_month = 9999;
                else if (recovery_month > 9999)  //Max
                    recovery_month = 9999;
                //업체등급
                GetGrade(recovery_month, out company_grade, out company_grade_remark);

                if (company_grade < 0)
                    company_grade = 0;
                else if (company_grade > 100)
                    company_grade = 100;

                row.Cells["net_working_capital_cost"].Value = net_working_capital_opportunity_cost.ToString("#,##0");
                row.Cells["working_capital_cost"].Value = working_capital_opportunity_cost.ToString("#,##0");
                row.Cells["ato_capital_cost"].Value = ato_capital_opportunity_cost.ToString("#,##0");
                row.Cells["equity_capital_cost"].Value = equity_capital_opportunity_cost.ToString("#,##0");
                row.Cells["principal_payback_period2"].Value = recovery_month.ToString("#,##0.00");
                row.Cells["grade2"].Value = company_grade;
            }
            this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
        }
        private void SetHeaderStyleSetting()
        {
            DataGridView dgv = dgvCompany;
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgv.Columns["company"].ReadOnly = true;
            dgv.Columns["pre_cumulative_margin"].Visible = false;

            dgv.Columns["sales_amount_pre_1"].Visible = false;
            dgv.Columns["sales_amount_pre_1"].HeaderCell.Style.BackColor = Color.LightGray;
            dgv.Columns["sales_amount_pre_1"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["sales_amount_pre_2"].Visible = false;
            dgv.Columns["sales_amount_pre_2"].HeaderCell.Style.BackColor = Color.LightGray;
            dgv.Columns["sales_amount_pre_2"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["sales_amount_pre_3"].Visible = false;
            dgv.Columns["sales_amount_pre_3"].HeaderCell.Style.BackColor = Color.LightGray;
            dgv.Columns["sales_amount_pre_3"].HeaderCell.Style.ForeColor = Color.Black;

            dgv.Columns["pre1_balance"].Visible = false;
            dgv.Columns["pre1_balance"].HeaderCell.Style.BackColor = Color.LightGray;
            dgv.Columns["pre1_balance"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["pre2_balance"].Visible = false;
            dgv.Columns["pre2_balance"].HeaderCell.Style.BackColor = Color.LightGray;
            dgv.Columns["pre2_balance"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["pre3_balance"].Visible = false;
            dgv.Columns["pre3_balance"].HeaderCell.Style.BackColor = Color.LightGray;
            dgv.Columns["pre3_balance"].HeaderCell.Style.ForeColor = Color.Black;

            dgv.Columns["pre1_average_balance"].Visible = false;
            dgv.Columns["pre1_average_balance"].HeaderCell.Style.BackColor = Color.LightGray;
            dgv.Columns["pre1_average_balance"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["pre2_average_balance"].Visible = false;
            dgv.Columns["pre2_average_balance"].HeaderCell.Style.BackColor = Color.LightGray;
            dgv.Columns["pre2_average_balance"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["pre3_average_balance"].Visible = false;
            dgv.Columns["pre3_average_balance"].HeaderCell.Style.BackColor = Color.LightGray;
            dgv.Columns["pre3_average_balance"].HeaderCell.Style.ForeColor = Color.Black;

            dgv.Columns["capital_amount"].HeaderCell.Style.BackColor = Color.LightGray;
            dgv.Columns["capital_amount"].HeaderCell.Style.ForeColor = Color.Black;

            dgv.Columns["accumulate_capital_amount"].HeaderCell.Style.BackColor = Color.LightGray;
            dgv.Columns["accumulate_capital_amount"].HeaderCell.Style.ForeColor = Color.Black;


            dgv.Columns["month_margin"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
            //dgv.Columns["principal_payback_period2"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
            dgv.Columns["grade1"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
            dgv.Columns["grade2"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);

            ((DataGridViewImageColumn)dgv.Columns["img"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
            dgv.Columns["img"].DefaultCellStyle.NullValue = null;

            for (int i = 2; i < dgv.Columns.Count - 2; i++)
            {
                if(dgv.Columns[i].Name != "updatetime" && dgv.Columns[i].Name != "manager" && dgv.Columns[i].Name != "company")
                    dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }
        private void GetPartner()
        {
            int syear;
            if (!int.TryParse(cbYear.Text, out syear))
            {
                MessageBox.Show(this,"기준년월을 확해주세요.");
                this.Activate();
                return;
            }
            int smonth;
            if (!int.TryParse(cbMonth.Text, out smonth))
            {
                MessageBox.Show(this,"기준년월을 확해주세요.");
                this.Activate();
                return;
            }

            this.dgvCompany.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            dgvCompany.Rows.Clear();

            int division = 1;
            if (string.IsNullOrEmpty(cbDivision.Text.Trim()))
                division = 1;
            else if (cbDivision.Text == "미수잔액 있는")
                division = 2;

            //회전율 및 재무재표 내역
            double net_working_capital;
            if (!double.TryParse(txtNetWorkingCapital.Text, out net_working_capital))
                net_working_capital = 0;
            double working_capital;
            if (!double.TryParse(txtWorkingCapital.Text, out working_capital))
                working_capital = 0;
            double ato_capital;
            if (!double.TryParse(txtAtoCapital.Text, out ato_capital))
                ato_capital = 0;
            else
                ato_capital = ato_capital / 100;
            double equity_capital;
            if (!double.TryParse(txtEquityCapital.Text, out equity_capital))
                equity_capital = 0;
            double sales_profit;
            if (!double.TryParse(txtSalesProfit.Text, out sales_profit))
                sales_profit = 0;
            else
                sales_profit = sales_profit / 100;

            //Get Data 
            DateTime now_date = new DateTime(syear, smonth, 1).AddMonths(1).AddDays(-1);
            DataTable companyDt = recoveryPrincipalPriceRepository.GetCompanyList2(now_date, txtCompany.Text.Trim(), txtManager.Text.Trim()
                                                                                , net_working_capital, working_capital, ato_capital, equity_capital, sales_profit
                                                                                , division);
            if (companyDt.Rows.Count > 0)
            {
                DataTable otherInfoDt = companyRepository.GetCompanyRecovery("", txtCompany.Text.Trim(), false, "");
                for (int i = 0; i < companyDt.Rows.Count; i++)
                {
                    bool isOutput = true;

                    //매출금액
                    double sales_amount = Convert.ToDouble(companyDt.Rows[i]["당월매출"].ToString());
                    double pre_accumulate_sales_amount = Convert.ToDouble(companyDt.Rows[i]["전월누적매출"].ToString());
                    double accumulate_sales_amount = pre_accumulate_sales_amount + sales_amount;
                    double pre_month_sales_amount = Convert.ToDouble(companyDt.Rows[i]["매출금액1"].ToString());

                    List<double> pre_sales_amount_list = new List<double>();
                    double sales_amount_pre_1 = Convert.ToDouble(companyDt.Rows[i]["매출금액1"].ToString());
                    pre_sales_amount_list.Add(sales_amount_pre_1);
                    double sales_amount_pre_2 = Convert.ToDouble(companyDt.Rows[i]["매출금액2"].ToString());
                    pre_sales_amount_list.Add(sales_amount_pre_2);
                    double sales_amount_pre_3 = Convert.ToDouble(companyDt.Rows[i]["매출금액3"].ToString());
                    pre_sales_amount_list.Add(sales_amount_pre_3);
                    double sales_amount_pre_4 = Convert.ToDouble(companyDt.Rows[i]["매출금액4"].ToString());
                    pre_sales_amount_list.Add(sales_amount_pre_4);
                    double sales_amount_pre_5 = Convert.ToDouble(companyDt.Rows[i]["매출금액5"].ToString());
                    pre_sales_amount_list.Add(sales_amount_pre_5);
                    double sales_amount_pre_6 = Convert.ToDouble(companyDt.Rows[i]["매출금액6"].ToString());
                    pre_sales_amount_list.Add(sales_amount_pre_6);
                    double sales_amount_pre_7 = Convert.ToDouble(companyDt.Rows[i]["매출금액7"].ToString());
                    pre_sales_amount_list.Add(sales_amount_pre_7);
                    double sales_amount_pre_8 = Convert.ToDouble(companyDt.Rows[i]["매출금액8"].ToString());
                    pre_sales_amount_list.Add(sales_amount_pre_8);
                    double sales_amount_pre_9 = Convert.ToDouble(companyDt.Rows[i]["매출금액9"].ToString());
                    pre_sales_amount_list.Add(sales_amount_pre_9);
                    double sales_amount_pre_10 = Convert.ToDouble(companyDt.Rows[i]["매출금액10"].ToString());
                    pre_sales_amount_list.Add(sales_amount_pre_10);
                    double sales_amount_pre_11 = Convert.ToDouble(companyDt.Rows[i]["매출금액11"].ToString());
                    pre_sales_amount_list.Add(sales_amount_pre_11);
                    double sales_amount_pre_12 = Convert.ToDouble(companyDt.Rows[i]["매출금액12"].ToString());
                    pre_sales_amount_list.Add(sales_amount_pre_12);


                    //미수금액
                    double balance_amount = Convert.ToDouble(companyDt.Rows[i]["당월미수"].ToString());
                    double pre_accumulate_balance_amount = Convert.ToDouble(companyDt.Rows[i]["전월누적미수"].ToString());
                    double accumulate_balance_amount = pre_accumulate_balance_amount + balance_amount;
                    double pre_month_balance_amount = Convert.ToDouble(companyDt.Rows[i]["미수잔액1"].ToString());

                    List<double> pre_balance_list = new List<double>();
                    double balance_pre_1 = Convert.ToDouble(companyDt.Rows[i]["미수잔액1"].ToString());
                    pre_balance_list.Add(balance_pre_1);
                    double balance_pre_2 = Convert.ToDouble(companyDt.Rows[i]["미수잔액2"].ToString());
                    pre_balance_list.Add(balance_pre_2);
                    double balance_pre_3 = Convert.ToDouble(companyDt.Rows[i]["미수잔액3"].ToString());
                    pre_balance_list.Add(balance_pre_3);
                    double balance_pre_4 = Convert.ToDouble(companyDt.Rows[i]["미수잔액4"].ToString());
                    pre_balance_list.Add(balance_pre_4);
                    double balance_pre_5 = Convert.ToDouble(companyDt.Rows[i]["미수잔액5"].ToString());
                    pre_balance_list.Add(balance_pre_5);
                    double balance_pre_6 = Convert.ToDouble(companyDt.Rows[i]["미수잔액6"].ToString());
                    pre_balance_list.Add(balance_pre_6);
                    double balance_pre_7 = Convert.ToDouble(companyDt.Rows[i]["미수잔액7"].ToString());
                    pre_balance_list.Add(balance_pre_7);
                    double balance_pre_8 = Convert.ToDouble(companyDt.Rows[i]["미수잔액8"].ToString());
                    pre_balance_list.Add(balance_pre_8);
                    double balance_pre_9 = Convert.ToDouble(companyDt.Rows[i]["미수잔액9"].ToString());
                    pre_balance_list.Add(balance_pre_9);
                    double balance_pre_10 = Convert.ToDouble(companyDt.Rows[i]["미수잔액10"].ToString());
                    pre_balance_list.Add(balance_pre_10);
                    double balance_pre_11 = Convert.ToDouble(companyDt.Rows[i]["미수잔액11"].ToString());
                    pre_balance_list.Add(balance_pre_11);
                    double balance_pre_12 = Convert.ToDouble(companyDt.Rows[i]["미수잔액12"].ToString());
                    pre_balance_list.Add(balance_pre_12);

                    //월평균잔액
                    double avg_balance_amount = Convert.ToDouble(companyDt.Rows[i]["당월평잔"].ToString());
                    double pre_accumulate_avg_balance_amount = Convert.ToDouble(companyDt.Rows[i]["전월누적평잔"].ToString());
                    double accumulate_avg_balance_amount = pre_accumulate_avg_balance_amount + avg_balance_amount;
                    double pre_month_avg_balance_amount = Convert.ToDouble(companyDt.Rows[i]["월평균잔액1"].ToString());

                    List<double> pre_avg_balance_list = new List<double>();
                    double avg_balance_pre_1 = Convert.ToDouble(companyDt.Rows[i]["월평균잔액1"].ToString());
                    pre_avg_balance_list.Add(avg_balance_pre_1);
                    double avg_balance_pre_2 = Convert.ToDouble(companyDt.Rows[i]["월평균잔액2"].ToString());
                    pre_avg_balance_list.Add(avg_balance_pre_2);
                    double avg_balance_pre_3 = Convert.ToDouble(companyDt.Rows[i]["월평균잔액3"].ToString());
                    pre_avg_balance_list.Add(avg_balance_pre_3);
                    double avg_balance_pre_4 = Convert.ToDouble(companyDt.Rows[i]["월평균잔액4"].ToString());
                    pre_avg_balance_list.Add(avg_balance_pre_4);
                    double avg_balance_pre_5 = Convert.ToDouble(companyDt.Rows[i]["월평균잔액5"].ToString());
                    pre_avg_balance_list.Add(avg_balance_pre_5);
                    double avg_balance_pre_6 = Convert.ToDouble(companyDt.Rows[i]["월평균잔액6"].ToString());
                    pre_avg_balance_list.Add(avg_balance_pre_6);
                    double avg_balance_pre_7 = Convert.ToDouble(companyDt.Rows[i]["월평균잔액7"].ToString());
                    pre_avg_balance_list.Add(avg_balance_pre_7);
                    double avg_balance_pre_8 = Convert.ToDouble(companyDt.Rows[i]["월평균잔액8"].ToString());
                    pre_avg_balance_list.Add(avg_balance_pre_8);
                    double avg_balance_pre_9 = Convert.ToDouble(companyDt.Rows[i]["월평균잔액9"].ToString());
                    pre_avg_balance_list.Add(avg_balance_pre_9);
                    double avg_balance_pre_10 = Convert.ToDouble(companyDt.Rows[i]["월평균잔액10"].ToString());
                    pre_avg_balance_list.Add(avg_balance_pre_10);
                    double avg_balance_pre_11 = Convert.ToDouble(companyDt.Rows[i]["월평균잔액11"].ToString());
                    pre_avg_balance_list.Add(avg_balance_pre_11);
                    double avg_balance_pre_12 = Convert.ToDouble(companyDt.Rows[i]["월평균잔액12"].ToString());
                    pre_avg_balance_list.Add(avg_balance_pre_12);

                    //마진금액
                    double margin_amount = Convert.ToDouble(companyDt.Rows[i]["당월마진"].ToString());
                    double pre_accumulate_margin_amount = Convert.ToDouble(companyDt.Rows[i]["전월누적마진"].ToString());
                    double accumulate_margin_amount = pre_accumulate_margin_amount + margin_amount;
                    double pre_month_margin_amount = Convert.ToDouble(companyDt.Rows[i]["마진금액1"].ToString());

                    List<double> pre_margin_amount_list = new List<double>();
                    double margin_amount_pre_1 = Convert.ToDouble(companyDt.Rows[i]["마진금액1"].ToString());
                    pre_margin_amount_list.Add(margin_amount_pre_1);
                    double margin_amount_pre_2 = Convert.ToDouble(companyDt.Rows[i]["마진금액2"].ToString());
                    pre_margin_amount_list.Add(margin_amount_pre_2);
                    double margin_amount_pre_3 = Convert.ToDouble(companyDt.Rows[i]["마진금액3"].ToString());
                    pre_margin_amount_list.Add(margin_amount_pre_3);
                    double margin_amount_pre_4 = Convert.ToDouble(companyDt.Rows[i]["마진금액4"].ToString());
                    pre_margin_amount_list.Add(margin_amount_pre_4);
                    double margin_amount_pre_5 = Convert.ToDouble(companyDt.Rows[i]["마진금액5"].ToString());
                    pre_margin_amount_list.Add(margin_amount_pre_5);
                    double margin_amount_pre_6 = Convert.ToDouble(companyDt.Rows[i]["마진금액6"].ToString());
                    pre_margin_amount_list.Add(margin_amount_pre_6);
                    double margin_amount_pre_7 = Convert.ToDouble(companyDt.Rows[i]["마진금액7"].ToString());
                    pre_margin_amount_list.Add(margin_amount_pre_7);
                    double margin_amount_pre_8 = Convert.ToDouble(companyDt.Rows[i]["마진금액8"].ToString());
                    pre_margin_amount_list.Add(margin_amount_pre_8);
                    double margin_amount_pre_9 = Convert.ToDouble(companyDt.Rows[i]["마진금액9"].ToString());
                    pre_margin_amount_list.Add(margin_amount_pre_9);
                    double margin_amount_pre_10 = Convert.ToDouble(companyDt.Rows[i]["마진금액10"].ToString());
                    pre_margin_amount_list.Add(margin_amount_pre_10);
                    double margin_amount_pre_11 = Convert.ToDouble(companyDt.Rows[i]["마진금액11"].ToString());
                    pre_margin_amount_list.Add(margin_amount_pre_11);
                    double margin_amount_pre_12 = Convert.ToDouble(companyDt.Rows[i]["마진금액12"].ToString());
                    pre_margin_amount_list.Add(margin_amount_pre_12);


                    //회전수, 회전일수
                    double around_month_current_month = sales_amount / balance_amount;
                    double around_month = pre_accumulate_sales_amount / pre_accumulate_balance_amount;


                    //폐업유무
                    bool isOutOfBusiness = false;
                    if (companyDt.Rows[i]["폐업유무"].ToString() == "Y")
                        isOutOfBusiness = true;


                    //아토 기회비용
                    bool isManage = false;
                    bool isManage2 = false;
                    bool isManage3 = false;
                    bool isManage4 = false;
                    bool isHide = false;
                    string ato_capital_updatetime = "";
                    string whr = "seaover_company_code = '" + companyDt.Rows[i]["거래처코드"].ToString() + "'"
                                + " AND seaover_company_code <> ''";
                    DataRow[] dr = otherInfoDt.Select(whr);
                    if (dr.Length > 0)
                    {
                        if (!double.TryParse(dr[0]["ato_capital_rate"].ToString(), out ato_capital))
                        {
                            if (!double.TryParse(txtAtoCapital.Text, out ato_capital))
                                ato_capital = 0;
                        }
                        ato_capital = ato_capital / 100;
                        isManage = Convert.ToBoolean(dr[0]["isManagement1"].ToString());
                        isManage2 = Convert.ToBoolean(dr[0]["isManagement2"].ToString());
                        isManage3 = Convert.ToBoolean(dr[0]["isManagement3"].ToString());
                        isManage4 = Convert.ToBoolean(dr[0]["isManagement4"].ToString());
                        isHide = Convert.ToBoolean(dr[0]["isHide"].ToString());
                        DateTime dt;
                        if (DateTime.TryParse(dr[0]["recovery_updatetime"].ToString(), out dt))
                            ato_capital_updatetime = dt.ToString("yyyy-MM-dd");

                    }

                    //발생 기회비용
                    double net_working_capital_opportunity_cost = net_working_capital * avg_balance_amount * sales_profit / 12;
                    double working_capital_opportunity_cost = working_capital * avg_balance_amount * sales_profit / 12;
                    double ato_capital_opportunity_cost = ato_capital * avg_balance_amount * 3;
                    double equity_capital_opportunity_cost = equity_capital * avg_balance_amount * sales_profit / 12;

                    //누적 기회비용
                    double accumulate_net_working_capital_opportunity_cost = net_working_capital * pre_accumulate_avg_balance_amount * sales_profit / 12;
                    double accumulate_working_capital_opportunity_cost = working_capital * pre_accumulate_avg_balance_amount * sales_profit / 12;
                    double accumulate_ato_capital_opportunity_cost = ato_capital * pre_accumulate_avg_balance_amount * 3;
                    double accumulate_equity_capital_opportunity_cost = equity_capital * pre_accumulate_avg_balance_amount * sales_profit / 12;

                    //손익분기잔액1
                    double margin_price1 = balance_amount - pre_accumulate_margin_amount;
                    //원금회수기간1
                    double principal_recovery_month1 = 0;
                    if(margin_price1 > 0)
                        principal_recovery_month1 = margin_price1 / margin_amount;

                    if (double.IsNaN(principal_recovery_month1))  //Nan
                        principal_recovery_month1 = 0;
                    else if (double.IsInfinity(principal_recovery_month1))  //Infinity
                        principal_recovery_month1 = 9999;
                    else if (principal_recovery_month1 > 9999)  //Max
                        principal_recovery_month1 = 9999;

                    //업체등급1
                    int grade1;
                    string grade_remark1;
                    GetGrade(principal_recovery_month1, out grade1, out grade_remark1);
                    if (grade1 < 0)
                        grade1 = 0;
                    else if (grade1 > 100)
                        grade1 = 100;


                    //손익분기잔액2
                    double margin_price2 = 0;
                    double month_capital_amount = 0;
                    double accumulate_month_capital_amount = 0;
                    
                    if (rbNetWorkingCapital.Checked)
                    {
                        month_capital_amount = net_working_capital_opportunity_cost;
                        accumulate_month_capital_amount = accumulate_net_working_capital_opportunity_cost + net_working_capital_opportunity_cost;
                        margin_price2 = margin_price1 + accumulate_net_working_capital_opportunity_cost + net_working_capital_opportunity_cost;
                    }
                    else if (rbWorkingCapital.Checked)
                    {
                        month_capital_amount = working_capital_opportunity_cost;
                        accumulate_month_capital_amount = accumulate_working_capital_opportunity_cost + working_capital_opportunity_cost;
                        margin_price2 = margin_price1 + accumulate_working_capital_opportunity_cost + working_capital_opportunity_cost;
                    }
                    else if (rbAtoCapital.Checked)
                    {
                        month_capital_amount = ato_capital_opportunity_cost;
                        accumulate_month_capital_amount = accumulate_ato_capital_opportunity_cost + ato_capital_opportunity_cost;
                        margin_price2 = margin_price1 + accumulate_ato_capital_opportunity_cost + ato_capital_opportunity_cost;
                    }
                    else if (rbEquityCapital.Checked)
                    {
                        month_capital_amount = equity_capital_opportunity_cost;
                        accumulate_month_capital_amount = accumulate_equity_capital_opportunity_cost + equity_capital_opportunity_cost;
                        margin_price2 = margin_price1 + accumulate_equity_capital_opportunity_cost + equity_capital_opportunity_cost;
                    }

                    //원금회수기간2
                    double principal_recovery_month2 = 0;
                    if (margin_price2 < 0)
                        principal_recovery_month2 = margin_price2 / 0;
                    else
                    {
                        double real_month_margin = margin_amount - month_capital_amount;
                        if (real_month_margin <= 0)
                            principal_recovery_month2 = margin_price2 / 0;
                        else
                            principal_recovery_month2 = margin_price2 / real_month_margin;  //손익분기잔액1 / (한달마진 - 당월발생 기회비용)
                    }

                    if (double.IsNaN(principal_recovery_month2))  //Nan
                        principal_recovery_month2 = 0;
                    else if (double.IsInfinity(principal_recovery_month2))  //Infinity
                        principal_recovery_month2 = 9999;
                    else if (principal_recovery_month2 > 9999)  //Max
                        principal_recovery_month2 = 9999;
                    //업체등급2
                    int grade2;
                    string grade2_remark;
                    GetGrade(principal_recovery_month2, out grade2, out grade2_remark);
                    if (grade2 < 0)
                        grade2 = 0;
                    else if (grade2 > 100)
                        grade2 = 100;

                    

                    //매출채권회전수(당월)
                    double rotation_current_month = sales_amount / balance_amount;
                    //매출채권회전수(평균)
                    double rotation = accumulate_sales_amount / accumulate_balance_amount;
                    //매출채권회전일수(당월)
                    double rotation_days_current_month = Math.Round(30.0 / rotation_current_month);
                    //매출채권회전일수(평균)
                    double rotation_days = Math.Round(30.0 / rotation);

                    //마진(당월)
                    double month_margin = margin_amount / sales_amount * 100;
                    //마진(평균)
                    double month_margin_total_average = accumulate_margin_amount / accumulate_sales_amount * 100;

                    //매출채권회전수/마진율(당월)
                    if (month_margin < 0)
                        month_margin = 0;
                    double sales_division_margin = (rotation_days_current_month / month_margin);
                    if (double.IsNaN(sales_division_margin) || sales_division_margin < 0)
                        sales_division_margin = 100;

                    grade1 = (int)Math.Ceiling(100 - (40 / 7 * sales_division_margin));
                    if (grade1 < 0)
                        grade1 = 0;

                    //매출채권회전수/마진율(평균)
                    double sales_division_margin_average = (rotation_days / month_margin_total_average);
                    if (double.IsNaN(sales_division_margin_average) || sales_division_margin_average < 0)
                        sales_division_margin_average = 100;

                    //검색필터======================================================================================================================================

                    //숨김
                    if (rbHideTrue.Checked)
                        isOutput = isHide;
                    else if (rbHideFalse.Checked)
                        isOutput = !isHide;

                    //폐업
                    if (rbOutOfBusinessTrue.Checked)
                        isOutput = isOutOfBusiness;
                    else if (rbOutOfBusinessFalse.Checked)
                        isOutput = !isOutOfBusiness;

                    //원금회수 끝난 거래처
                    if (rbOutRecoveryAll.Checked)
                    {
                    }
                    else if (rbOutRecoveryOver.Checked)
                    {
                        if (principal_recovery_month1 > 0)
                            isOutput = false;
                    }
                    else if (rbOutRecoveryNotOver.Checked)
                    {
                        if (principal_recovery_month1 <= 0)
                            isOutput = false;
                    }

                    //소송중인 거래처
                    if (rbLawsuitAll.Checked)
                    {
                    }
                    else if (rbLawsuitTrue.Checked)
                    {
                        if (!companyDt.Rows[i]["거래처명"].ToString().Contains("소송"))
                            isOutput = false;
                    }
                    else if (rbLawsuitFalse.Checked)
                    {
                        if (companyDt.Rows[i]["거래처명"].ToString().Contains("소송"))
                            isOutput = false;
                    }

                    //관심업체만
                    if (isOutput && cbManagement.Checked && !isManage)
                        isOutput = false;
                    if (isOutput && cbManagement2.Checked && !isManage2)
                        isOutput = false;
                    if (isOutput && cbManagement3.Checked && !isManage3)
                        isOutput = false;
                    if (isOutput && cbManagement4.Checked && !isManage4)
                        isOutput = false;

                    //원금회수기간1
                    if (isOutput && cbRecoveryTerm.Checked)
                    {
                        int sttGrade = (int)nudSttRt.Value;
                        if (cbRecoveryTermType.Text == "이상")
                        {
                            if (principal_recovery_month1 < (double)sttGrade)
                                isOutput = false;
                        }
                        else
                        {
                            if (principal_recovery_month1 > (double)sttGrade)
                                isOutput = false;
                        }
                    }

                    //원금회수기간2
                    if (isOutput && cbRecoveryTerm.Checked)
                    {
                        int sttGrade = (int)nudSttRt2.Value;
                        if (cbRecoveryTerm2Type.Text == "이상")
                        {
                            if (principal_recovery_month2 < (double)sttGrade)
                                isOutput = false;
                        }
                        else
                        {
                            if (principal_recovery_month2 > (double)sttGrade)
                                isOutput = false;
                        }
                    }

                    //업체등급1
                    if (isOutput && cbGrade.Checked)
                    {
                        int sttGrade = (int)nudSttGrade.Value;
                        if (cbGradeType.Text == "이상")
                        {
                            if (grade1 < (double)sttGrade)
                                isOutput = false;
                        }
                        else
                        {
                            if (grade1 > (double)sttGrade)
                                isOutput = false;
                        }
                    }

                    //업체등급2
                    if (isOutput && cbGrade2.Checked)
                    {
                        int sttGrade = (int)nudSttGrade2.Value;
                        if (cbGrade2Type.Text == "이상")
                        {
                            if (grade2 < (double)sttGrade)
                                isOutput = false;
                        }
                        else
                        {
                            if (grade2 > (double)sttGrade)
                                isOutput = false;
                        }
                    }

                    //회전일수/마진율1
                    if (isOutput && cbDivisionRotation.Checked)
                    {
                        int sttGrade = (int)nudSttDr.Value;
                        if (cbDivisionRotationType.Text == "이상")
                        {
                            if (sales_division_margin < (double)sttGrade)
                                isOutput = false;
                        }
                        else
                        {
                            if (sales_division_margin > (double)sttGrade)
                                isOutput = false;
                        }
                    }

                    //회전일수1
                    if (isOutput && cbRotation.Checked)
                    {
                        int sttGrade = (int)nudSttRotation.Value;
                        if (cbRotationType.Text == "이상")
                        {
                            if (rotation_days_current_month < (double)sttGrade)
                                isOutput = false;
                        }
                        else
                        {
                            if (rotation_days_current_month > (double)sttGrade)
                                isOutput = false;
                        }
                    }

                    //회전일수/마진율2
                    if (isOutput && cbDivisionRotationAverage.Checked)
                    {
                        int sttGrade = (int)nudSttDr.Value;
                        if (cbDivisionRotationAverageType.Text == "이상")
                        {
                            if (sales_division_margin_average < (double)sttGrade)
                                isOutput = false;
                        }
                        else
                        {
                            if (sales_division_margin_average > (double)sttGrade)
                                isOutput = false;
                        }
                    }

                    //회전일수2
                    if (isOutput && cbRotationAverage.Checked)
                    {
                        int sttGrade = (int)nudSttRotationAverage.Value;
                        if (cbRotationAverageType.Text == "이상")
                        {
                            if (rotation_days < (double)sttGrade)
                                isOutput = false;
                        }
                        else
                        {
                            if (rotation_days > (double)sttGrade)
                                isOutput = false;
                        }
                    }

                    
                    //매출추이======================================================================================================================================
                    double base_rate;
                    if (!double.TryParse(txtBaseIncreaseRate.Text, out base_rate) && (cbIncreaseBalance.Checked || cbIncreaseSales.Checked || cbIncreaseMarginRate.Checked))
                    {
                        MessageBox.Show(this,"매출추이 기준값이 숫자형식이 아닙니다!");
                        this.Activate();
                        return;
                    }
                    //================================================================
                    //****************증감율을 하락율로만 구하는 이유*****************
                    //================================================================
                    //증율을 배수로 차이날 경우 몇백 몇천퍼센트로 늘어나고
                    //감율은 배수가 차이날 경우라도 100퍼센트 미만으로 측정되기 때문에
                    //감율로 비율을 통일해서 비교
                    //================================================================
                    //================================================================

                    //3개월간 미수증가
                    if (isOutput && cbIncreaseBalance.Checked)
                    {
                        isOutput = false;
                        int sales_month = (int)nudSalesMonth1.Value - 1;
                        double total_change_rate = 0;

                        double pre_balance;
                        double cur_balance;
                        double change_rate;
                        for (int j = sales_month; j >= 1; j--)
                        { 
                            pre_balance = pre_avg_balance_list[j];
                            cur_balance = pre_avg_balance_list[j - 1];

                            if (pre_balance > cur_balance)
                            {
                                if (pre_balance == 0 && cur_balance == 0)
                                    change_rate = 0;
                                else if (pre_balance == 0 && cur_balance > 0)
                                    change_rate = -100;
                                else if (pre_balance < 0 && cur_balance == 0)
                                    change_rate = 100;
                                else if (pre_balance > 0 && cur_balance == 0)
                                    change_rate = -100;
                                else
                                    change_rate = (cur_balance - pre_balance) / pre_balance * 100;
                                total_change_rate += change_rate;
                            }
                            else
                            {
                                if (pre_balance == 0 && cur_balance == 0)
                                    change_rate = 0;
                                else if (pre_balance > 0 && cur_balance == 0)
                                    change_rate = -100;
                                else if (pre_balance == 0 && cur_balance < 0)
                                    change_rate = 100;
                                else if (pre_balance == 0 && cur_balance > 0)
                                    change_rate = -100;
                                else
                                    change_rate = (pre_balance - cur_balance) / cur_balance * 100;
                                total_change_rate += -change_rate;
                            }
                        }

                        pre_balance = pre_avg_balance_list[0];
                        cur_balance = avg_balance_amount;
                        if (pre_balance > cur_balance)
                        {
                            if (pre_balance == 0 && cur_balance == 0)
                                change_rate = 0;
                            else if (pre_balance == 0 && cur_balance > 0)
                                change_rate = -100;
                            else if (pre_balance < 0 && cur_balance == 0)
                                change_rate = 100;
                            else if (pre_balance > 0 && cur_balance == 0)
                                change_rate = -100;
                            else
                                change_rate = ((cur_balance - pre_balance) / pre_balance * 100);
                        }
                        else
                        {
                            if (pre_balance == 0 && cur_balance == 0)
                                change_rate = 0;
                            else if (pre_balance > 0 && cur_balance == 0)
                                change_rate = -100;
                            else if (pre_balance == 0 && cur_balance < 0)
                                change_rate = 100;
                            else if (pre_balance == 0 && cur_balance > 0)
                                change_rate = -100;
                            else
                                change_rate = (pre_balance - cur_balance) / cur_balance * 100;
                            total_change_rate += -change_rate;
                        }
                        total_change_rate += change_rate;
                        total_change_rate /= (sales_month + 1);

                        if (cbIncreaseBalanceType1.Text == "증가")
                        {
                            if (total_change_rate > base_rate)
                                isOutput = true;
                        }
                        else
                        {
                            if (total_change_rate < base_rate)
                                isOutput = true;
                        }
                    }

                    //3개월간 매출증가
                    if (isOutput && cbIncreaseSales.Checked)
                    {
                        isOutput = false;
                        int sales_month = (int)nudSalesMonth2.Value - 1;
                        double total_change_rate = 0;

                        double pre_sales;
                        double cur_sales;
                        double change_rate;

                        int zero_cnt = 0;
                        for (int j = sales_month; j >= 1; j--)
                        {
                            pre_sales = pre_sales_amount_list[j];
                            cur_sales = pre_sales_amount_list[j - 1];

                            if (pre_sales > cur_sales)
                            {
                                if (pre_sales == 0 && cur_sales == 0)
                                    change_rate = 0;
                                else if (pre_sales == 0 && cur_sales > 0)
                                    change_rate = -100;
                                else if (pre_sales < 0 && cur_sales == 0)
                                    change_rate = 100;
                                else if (pre_sales > 0 && cur_sales == 0)
                                    change_rate = -100;
                                else
                                    change_rate = (cur_sales - pre_sales) / pre_sales * 100;
                                total_change_rate += change_rate;
                            }
                            else
                            {
                                if (pre_sales == 0 && cur_sales == 0)
                                    change_rate = 0;
                                else if (pre_sales > 0 && cur_sales == 0)
                                    change_rate = -100;
                                else if (pre_sales == 0 && cur_sales < 0)
                                    change_rate = 100;
                                else if (pre_sales == 0 && cur_sales > 0)
                                    change_rate = -100;
                                else
                                    change_rate = (pre_sales - cur_sales) / cur_sales * 100;
                                total_change_rate += -change_rate;
                            }
                        }

                        pre_sales = pre_sales_amount_list[0];
                        cur_sales = sales_amount;
                        if (pre_sales > cur_sales)
                        {
                            if (pre_sales == 0 && cur_sales == 0)
                                change_rate = 0;
                            else if (pre_sales == 0 && cur_sales > 0)
                                change_rate = -100;
                            else if (pre_sales < 0 && cur_sales == 0)
                                change_rate = 100;
                            else if (pre_sales > 0 && cur_sales == 0)
                                change_rate = -100;
                            else
                                change_rate = ((cur_sales - pre_sales) / pre_sales * 100);
                        }
                        else
                        {
                            if (pre_sales == 0 && cur_sales == 0)
                                change_rate = 0;
                            else if (pre_sales > 0 && cur_sales == 0)
                                change_rate = -100;
                            else if (pre_sales == 0 && cur_sales < 0)
                                change_rate = 100;
                            else if (pre_sales == 0 && cur_sales > 0)
                                change_rate = -100;
                            else
                                change_rate = (pre_sales - cur_sales) / cur_sales * 100;
                            total_change_rate += -change_rate;
                        }
                        total_change_rate += change_rate;
                        total_change_rate /= (sales_month + 1);

                        if (cbIncreaseBalanceType2.Text == "증가")
                        {
                            if (total_change_rate > base_rate)
                                isOutput = true;
                        }
                        else
                        {
                            if (total_change_rate < base_rate)
                                isOutput = true;
                        }
                    }

                    //3개월간 마진율 증가
                    if (isOutput && cbIncreaseMarginRate.Checked)
                    {
                        isOutput = false;
                        int sales_month = (int)nudSalesMonth3.Value - 1;
                        double total_change_rate = 0;

                        double pre_sales;
                        double cur_sales;
                        double change_rate;
                        for (int j = sales_month; j >= 1; j--)
                        {
                            pre_sales = pre_margin_amount_list[j] / pre_sales_amount_list[j] * 100;
                            cur_sales = pre_margin_amount_list[j - 1] / pre_sales_amount_list[j - 1] * 100;

                            if (pre_sales > cur_sales)
                            {
                                if (pre_sales == 0 && cur_sales == 0)
                                    change_rate = 0;
                                else if (pre_sales == 0 && cur_sales > 0)
                                    change_rate = -100;
                                else if (pre_sales < 0 && cur_sales == 0)
                                    change_rate = 100;
                                else if (pre_sales > 0 && cur_sales == 0)
                                    change_rate = -100;
                                else
                                    change_rate = (cur_sales - pre_sales) / pre_sales * 100;
                                total_change_rate += change_rate;
                            }
                            else
                            {
                                if (pre_sales == 0 && cur_sales == 0)
                                    change_rate = 0;
                                else if (pre_sales > 0 && cur_sales == 0)
                                    change_rate = -100;
                                else if (pre_sales == 0 && cur_sales < 0)
                                    change_rate = 100;
                                else if (pre_sales == 0 && cur_sales > 0)
                                    change_rate = -100;
                                else
                                    change_rate = (pre_sales - cur_sales) / cur_sales * 100;
                                total_change_rate += -change_rate;
                            }
                        }

                        pre_sales = pre_margin_amount_list[0] / pre_sales_amount_list[0] * 100;
                        if (double.IsInfinity(pre_sales))
                            pre_sales = 100;
                        else if (double.IsNaN(pre_sales))
                            pre_sales = 0;
                        cur_sales = margin_amount / sales_amount * 100;
                        if (double.IsInfinity(cur_sales))
                            cur_sales = 100;
                        else if (double.IsNaN(cur_sales))
                            cur_sales = 0;

                        if (pre_sales > cur_sales)
                        {
                            if (pre_sales == 0 && cur_sales == 0)
                                change_rate = 0;
                            else if (pre_sales == 0 && cur_sales > 0)
                                change_rate = -100;
                            else if (pre_sales < 0 && cur_sales == 0)
                                change_rate = 100;
                            else if (pre_sales > 0 && cur_sales == 0)
                                change_rate = -100;
                            else
                                change_rate = ((cur_sales - pre_sales) / pre_sales * 100);
                        }
                        else
                        {
                            if (pre_sales == 0 && cur_sales == 0)
                                change_rate = 0;
                            else if (pre_sales > 0 && cur_sales == 0)
                                change_rate = -100;
                            else if (pre_sales == 0 && cur_sales < 0)
                                change_rate = 100;
                            else if (pre_sales == 0 && cur_sales > 0)
                                change_rate = -100;
                            else
                                change_rate = (pre_sales - cur_sales) / cur_sales * 100;
                            total_change_rate += -change_rate;
                        }
                        total_change_rate += change_rate;
                        total_change_rate /= (sales_month + 1);

                        if (cbIncreaseBalanceType3.Text == "증가")
                        {
                            if (total_change_rate > base_rate)
                                isOutput = true;
                        }
                        else
                        {
                            if (total_change_rate < base_rate)
                                isOutput = true;
                        }
                    }

                    //출력=====================================================================
                    if (isOutput)
                    { 
                        int n = dgvCompany.Rows.Add();

                        dgvCompany.Rows[n].Cells["is_manage1"].Value = isManage.ToString();
                        dgvCompany.Rows[n].Cells["is_manage2"].Value = isManage2.ToString();
                        dgvCompany.Rows[n].Cells["is_manage3"].Value = isManage3.ToString();
                        dgvCompany.Rows[n].Cells["is_manage4"].Value = isManage4.ToString();


                        dgvCompany.Rows[n].Cells["seaover_company_code"].Value = companyDt.Rows[i]["거래처코드"].ToString();
                        dgvCompany.Rows[n].Cells["company"].Value = companyDt.Rows[i]["거래처명"].ToString();
                        dgvCompany.Rows[n].Cells["manager"].Value = companyDt.Rows[i]["매출자"].ToString();
                        if (!string.IsNullOrEmpty(companyDt.Rows[i]["최종매출일자"].ToString()))
                            dgvCompany.Rows[n].Cells["updatetime"].Value = Convert.ToDateTime(companyDt.Rows[i]["최종매출일자"].ToString()).ToString("yyyy-MM-dd");

                        dgvCompany.Rows[n].Cells["sales_amount"].Value = sales_amount.ToString("#,##0");
                        dgvCompany.Rows[n].Cells["sales_amount_pre_1"].Value = sales_amount_pre_1.ToString("#,##0");
                        dgvCompany.Rows[n].Cells["sales_amount_pre_2"].Value = sales_amount_pre_2.ToString("#,##0");
                        dgvCompany.Rows[n].Cells["sales_amount_pre_3"].Value = sales_amount_pre_3.ToString("#,##0");


                        dgvCompany.Rows[n].Cells["current_balance"].Value = balance_amount.ToString("#,##0");
                        dgvCompany.Rows[n].Cells["different_pre_balance"].Value = (balance_amount - balance_pre_1).ToString("#,##0");
                        dgvCompany.Rows[n].Cells["pre1_balance"].Value = balance_pre_1.ToString("#,##0");
                        dgvCompany.Rows[n].Cells["pre2_balance"].Value = balance_pre_2.ToString("#,##0");
                        dgvCompany.Rows[n].Cells["pre3_balance"].Value = balance_pre_3.ToString("#,##0");


                        dgvCompany.Rows[n].Cells["average_balance"].Value = avg_balance_amount.ToString("#,##0");
                        dgvCompany.Rows[n].Cells["pre1_average_balance"].Value = avg_balance_pre_1.ToString("#,##0");
                        dgvCompany.Rows[n].Cells["pre2_average_balance"].Value = avg_balance_pre_2.ToString("#,##0");
                        dgvCompany.Rows[n].Cells["pre3_average_balance"].Value = avg_balance_pre_3.ToString("#,##0");

                        dgvCompany.Rows[n].Cells["around_month"].Value = rotation.ToString("#,##0.00") + " (" + rotation_current_month.ToString("#,##0.00") + ")";
                        dgvCompany.Rows[n].Cells["around_day"].Value = rotation_days.ToString("#,##0") + " (" + rotation_days_current_month.ToString("#,##0") + ")";

                        dgvCompany.Rows[n].Cells["month_margin"].Value = margin_amount.ToString("#,##0");
                        dgvCompany.Rows[n].Cells["cumulative_margin"].Value = accumulate_margin_amount.ToString("#,##0");
                        dgvCompany.Rows[n].Cells["pre_cumulative_margin"].Value = pre_accumulate_margin_amount.ToString("#,##0");

                        dgvCompany.Rows[n].Cells["net_working_capital_cost"].Value = net_working_capital_opportunity_cost.ToString("#,##0");
                        dgvCompany.Rows[n].Cells["working_capital_cost"].Value = working_capital_opportunity_cost.ToString("#,##0");
                        dgvCompany.Rows[n].Cells["ato_capital_cost"].Value = ato_capital_opportunity_cost.ToString("#,##0");
                        dgvCompany.Rows[n].Cells["equity_capital_cost"].Value = equity_capital_opportunity_cost.ToString("#,##0");

                        dgvCompany.Rows[n].Cells["ato_capital_rate"].Value = (ato_capital * 100).ToString("#,##0.0");
                        if (!string.IsNullOrEmpty(ato_capital_updatetime))
                        {
                            dgvCompany.Rows[n].Cells["ato_capital_rate"].Style.ForeColor = Color.Black;
                            dgvCompany.Rows[n].Cells["ato_capital_rate"].ToolTipText = ato_capital_updatetime;
                        }
                        else
                            dgvCompany.Rows[n].Cells["ato_capital_rate"].Style.ForeColor = Color.Gray;

                        dgvCompany.Rows[n].Cells["accumulate_balance_amount"].Value = accumulate_balance_amount.ToString("#,##0");
                        dgvCompany.Rows[n].Cells["accumulate_average_balance_amount"].Value = accumulate_avg_balance_amount.ToString("#,##0");

                        dgvCompany.Rows[n].Cells["capital_amount"].Value = month_capital_amount.ToString("#,##0");
                        dgvCompany.Rows[n].Cells["accumulate_capital_amount"].Value = accumulate_month_capital_amount.ToString("#,##0");

                        //손익분기잔액, 원금회수율1
                        dgvCompany.Rows[n].Cells["break_even_balance1"].Value = margin_price1.ToString("#,##0");
                        dgvCompany.Rows[n].Cells["break_even_balance1"].ToolTipText = "미수금 - 전월 누적마진금액";
                        if(principal_recovery_month1 == 9999)
                            dgvCompany.Rows[n].Cells["principal_payback_period1"].Value =  "∞ 개월";
                        else 
                            dgvCompany.Rows[n].Cells["principal_payback_period1"].Value = principal_recovery_month1.ToString("#,##0") + "개월";

                        //손익분기잔액, 원금회수율2
                        dgvCompany.Rows[n].Cells["break_even_balance2"].Value = margin_price2.ToString("#,##0");
                        dgvCompany.Rows[n].Cells["break_even_balance2"].ToolTipText = "미수금 - 전월 누적마진금액 + 기회비용 누적금액";
                        if (principal_recovery_month2 == 9999)
                            dgvCompany.Rows[n].Cells["principal_payback_period2"].Value = "∞ 개월";
                        else
                            dgvCompany.Rows[n].Cells["principal_payback_period2"].Value = principal_recovery_month2.ToString("#,##0") + "개월";

                        dgvCompany.Rows[n].Cells["sales_division_margin"].Value = sales_division_margin.ToString("#,##0.0");

                        //업체등급 1,2 
                        string grade_tooltip = "90이상 유지 (원금회수기간 6달 이하)"
                                        + "\n 80이상 유지(관리)  (원금회수기간 11달 이하)"
                                        + "\n 70이상 수시관리↑  (원금회수기간 16달 이하)"
                                        + "\n 60이상 수시관리(조심)  (원금회수기간 21달 이하)"
                                        + "\n 50이상 보고  (원금회수기간 26달 이하)"
                                        + "\n 50미만 보고필 (원금회수기간 26달 초과) ";
                        dgvCompany.Rows[n].Cells["grade1"].ToolTipText = grade_tooltip;
                        dgvCompany.Rows[n].Cells["grade2"].ToolTipText = grade_tooltip;
                        dgvCompany.Rows[n].Cells["grade1"].Value = grade1;
                        dgvCompany.Rows[n].Cells["grade2"].Value = grade2;

                        if (grade1 >= 80)
                        {
                            dgvCompany.Rows[n].Cells["grade1"].Style.ForeColor = Color.Blue;
                            dgvCompany.Rows[n].Cells["grade1"].Style.ForeColor = Color.Blue;
                        }
                        else if (grade1 < 50)
                        {
                            dgvCompany.Rows[n].Cells["grade1"].Style.ForeColor = Color.Red;
                            dgvCompany.Rows[n].Cells["grade1"].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            dgvCompany.Rows[n].Cells["grade1"].Style.ForeColor = Color.Black;
                            dgvCompany.Rows[n].Cells["grade1"].Style.ForeColor = Color.Black;
                        }

                        if (grade2 >= 80)
                        {
                            dgvCompany.Rows[n].Cells["grade2"].Style.ForeColor = Color.Blue;
                            dgvCompany.Rows[n].Cells["grade2"].Style.ForeColor = Color.Blue;
                        }
                        else if (grade2 < 50)
                        {
                            dgvCompany.Rows[n].Cells["grade2"].Style.ForeColor = Color.Red;
                            dgvCompany.Rows[n].Cells["grade2"].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            dgvCompany.Rows[n].Cells["grade2"].Style.ForeColor = Color.Black;
                            dgvCompany.Rows[n].Cells["grade2"].Style.ForeColor = Color.Black;
                        }

                        //기회비용 Tooltip
                        if (rbNetWorkingCapital.Checked)
                        {
                            dgvCompany.Rows[n].Cells["capital_amount"].ToolTipText = "(순영업자본 회전율 x 평균미수금 x 작년 영업이익) / 12개월";
                            dgvCompany.Rows[n].Cells["accumulate_capital_amount"].ToolTipText = "(순영업자본 회전율 x 평균미수금 x 작년 영업이익) / 12개월";
                        }
                        else if (rbWorkingCapital.Checked)
                        {
                            dgvCompany.Rows[n].Cells["capital_amount"].ToolTipText = "(영업자본 회전율 x 평균미수금 x 작년 영업이익) / 12개월";
                            dgvCompany.Rows[n].Cells["accumulate_capital_amount"].ToolTipText = "(영업자본 회전율 x 평균미수금 x 작년 영업이익) / 12개월";
                        }
                        else if (rbAtoCapital.Checked)
                        {
                            dgvCompany.Rows[n].Cells["capital_amount"].ToolTipText = "(평균미수금 x 판매가능기준) * 3";
                            dgvCompany.Rows[n].Cells["accumulate_capital_amount"].ToolTipText = "(평균미수금 x 판매가능기준) * 3";
                        }
                        else
                        {
                            dgvCompany.Rows[n].Cells["capital_amount"].ToolTipText = "(자기자본 회전율 x 평균미수금 x 작년 영업이익) / 12개월";
                            dgvCompany.Rows[n].Cells["accumulate_capital_amount"].ToolTipText = "(자기자본 회전율 x 평균미수금 x 작년 영업이익) / 12개월";
                        }

                    }
                }
                //calculate();
            }            
            this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            SetManagementImg();
        }
        private void GetGrade(double recovery_month, out int grade, out string grade_remark)
        {
            if (recovery_month < 0)
                recovery_month = 0;
            else if (double.IsInfinity(recovery_month))
            {
                grade = 0;
                grade_remark = "마진X";
                return;
            }
            double rMonth = Math.Ceiling(recovery_month);
            if (rMonth == 0)
                grade = 100;
            else
                grade = 100 - (((int)rMonth - 1) * 2);

            //Remark
            if (grade >= 90)
            {
                //grade_eng.Value = "A+";
                grade_remark = "유지";
            }
            else if (grade >= 80)
            {
                //grade_eng.Value = "A";
                grade_remark = "유지(관리)";
            }
            else if (grade >= 70)
            {
                //grade_eng.Value = "A-";
                grade_remark = "수시관리↑";
            }
            else if (grade >= 60)
            {
                //grade_eng.Value = "B";
                grade_remark = "수시관리(조심)";
            }
            else if (grade >= 50)
            {
                //grade_eng.Value = "C";
                grade_remark = "보고";
            }
            else if (grade >= 40)
            {
                //grade_eng.Value = "C-";
                grade_remark = "보고필";
            }
            else
            {
                //grade_eng.Value = "F";
                grade_remark = "보고필";
            }
        }
        #endregion

        #region Key event
        private void txtNetWorkingCapital_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                calculate();
        }
        private void Numberic_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(46) || e.KeyChar == Convert.ToChar(45) || e.KeyChar == Convert.ToChar(47)))
                e.Handled = true;
        }
        private void SalesPartnerManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        GetPartner();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                    case Keys.M:
                        txtCompany.Focus();
                        break;
                    case Keys.N:
                        txtCompany.Text = String.Empty;
                        txtManager.Text = String.Empty;
                        txtCompany.Focus();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        cbSalesAmount.Checked = !cbSalesAmount.Checked;
                        break;
                    case Keys.F2:
                        cbBalance.Checked = !cbBalance.Checked;
                        break;
                    case Keys.F3:
                        cbAverageMargin.Checked = !cbAverageMargin.Checked;
                        break;
                }
            }
        }
        private void txtGrade_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetPartner();
        }
        private void dgvCompany_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvCompany.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    ContextMenuStrip m = new ContextMenuStrip();
                    m.Items.Add("원금회수율 관리(가로)");
                    m.Items.Add("원금회수율 관리(세로)");
                    ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
                    toolStripSeparator1.Name = "toolStripSeparator";
                    toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
                    m.Items.Add(toolStripSeparator1);
                    m.Items.Add("관심업체1 등록/해제");
                    m.Items.Add("관심업체2 등록/해제");
                    m.Items.Add("관심업체3 등록/해제");
                    m.Items.Add("관심업체4 등록/해제");
                    ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
                    toolStripSeparator2.Name = "toolStripSeparator";
                    toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
                    m.Items.Add(toolStripSeparator2);
                    m.Items.Add("판매가능기준 등록");
                    m.Items.Add("거래처 숨김 등록/해제");
                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(dgvCompany, e.Location);
                }
            }
            catch (Exception ex)
            {

            }
        }
        


        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (dgvCompany.SelectedRows.Count == 0)
            {
                return;
            }
            try
            {
                DataGridViewRow dr = dgvCompany.SelectedRows[0];
                if (dr.Index < 0)
                {
                    return;
                }

                int rowindex = Convert.ToInt32(dr.Cells[0].Value);
                /*PendingInfo p;*/

                int syear;
                if (!int.TryParse(cbYear.Text, out syear))
                {
                    MessageBox.Show(this,"기준년월을 확인해주세요.");
                    this.Activate();
                    return;
                }
                int smonth;
                if (!int.TryParse(cbMonth.Text, out smonth))
                {
                    MessageBox.Show(this,"기준년월을 확인해주세요.");
                    this.Activate();
                    return;
                }

                DateTime now_date = new DateTime(syear, smonth, 1).AddMonths(1).AddDays(-1);

                switch (e.ClickedItem.Text)
                {
                    case "원금회수율 관리(가로)":
                        RecoveryPrincipalManager rpm = new RecoveryPrincipalManager(um, dr.Cells["company"].Value.ToString()
                                                                                    , dr.Cells["manager"].Value.ToString()
                                                                                    , now_date);
                        rpm.Show();
                        break;
                    case "원금회수율 관리(세로)":
                        RecoveryPrincipalVerticality rpv = new RecoveryPrincipalVerticality(um, dr.Cells["company"].Value.ToString(), dr.Cells["manager"].Value.ToString(), now_date);
                        rpv.Show();
                        break;
                    case "관심업체1 등록/해제":
                        InsertRecoveryPrincipal(dr, 1);
                        SetManagementImg();
                        break;
                    case "관심업체2 등록/해제":
                        InsertRecoveryPrincipal(dr, 2);
                        SetManagementImg();
                        break;
                    case "관심업체3 등록/해제":
                        InsertRecoveryPrincipal(dr, 3);
                        SetManagementImg();
                        break;
                    case "관심업체4 등록/해제":
                        InsertRecoveryPrincipal(dr, 4);
                        SetManagementImg();
                        break;
                    case "판매가능기준 등록":
                        AtoCapitalRateInsert(dr);
                        break;
                    case "거래처 숨김 등록/해제":
                        InsertHideCompany(dr);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message);
                this.Activate();
            }

        }
        

        #endregion

        #region Datagridview event
        private void dgvCompany_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            string name = dgvCompany.CurrentCell.OwningColumn.Name;

            if (name == "target_margin_rate" || name == "target_recovery_month")
            {
                e.Control.KeyPress += new KeyPressEventHandler(Numberic_KeyPress);
            }
            else
            {
                e.Control.KeyPress -= new KeyPressEventHandler(Numberic_KeyPress);
            }
        }
        private void dgvCompany_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 1)
            {
                dgvCompany.Rows[e.RowIndex].Cells["chk"].Value = true;
            }
        }
        private void dgvCompany_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            this.dgvCompany.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DateTime dt;
                if (e.ColumnIndex == 0)
                {
                    bool isChecked = Convert.ToBoolean(dgvCompany.Rows[e.RowIndex].Cells[0].Value);
                    if (isChecked)
                    {
                        dgvCompany.Rows[e.RowIndex].HeaderCell.Style.BackColor = Color.Red;
                    }
                    else
                    {
                        dgvCompany.Rows[e.RowIndex].HeaderCell.Style.BackColor = Color.LightGray;
                    }
                }
                else if (dgvCompany.Columns[e.ColumnIndex].Name == "grade" && dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    //업체등급
                    double grade;
                    if (dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !double.TryParse(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out grade))
                        grade = 0;

                    if (grade >= 80)
                        dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Blue;
                    else if (grade < 50)
                        dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
                    else
                        dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                }
                else if (dgvCompany.Columns[e.ColumnIndex].Name == "updatetime"
                    && dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null 
                    && DateTime.TryParse(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out dt))
                {
                    if (dt <= DateTime.Now.AddDays(-15))
                        dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.DarkRed;
                    else if (dt <= DateTime.Now.AddMonths(-1))
                        dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
                }
            }
            this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
        }

        private void dgvCompany_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    dgvCompany.ClearSelection();
                    dgvCompany.Rows[e.RowIndex].Selected = true;
                }
            }
        }
        private void dgvCompany_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int syear;
                if (!int.TryParse(cbYear.Text, out syear))
                {
                    MessageBox.Show(this,"기준년월을 확인해주세요.");
                    this.Activate();
                    return;
                }
                int smonth;
                if (!int.TryParse(cbMonth.Text, out smonth))
                {
                    MessageBox.Show(this,"기준년월을 확인해주세요.");
                    this.Activate();
                    return;
                }

                DateTime now_date = new DateTime(syear, smonth, 1).AddMonths(1).AddDays(-1);
                RecoveryPrincipalManager rpm = new RecoveryPrincipalManager(um, dgvCompany.Rows[e.RowIndex].Cells["company"].Value.ToString()
                                                                                        , dgvCompany.Rows[e.RowIndex].Cells["manager"].Value.ToString()
                                                                                        , now_date);
                rpm.Show();
            }
        }
        #endregion

        #region Checkbox, Radio, Button
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        private void btnExcel_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "영업거래처 관리", "원금회수율 관리", "is_excel"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }
            //비밀번호 확인
            PasswordCheckManager pcm = new PasswordCheckManager(um.excel_password);
            if (!pcm.isPasswordCheck())
                return;


            List<string> colList = new List<string>();
            colList.Add("seaover_company_code");
            colList.Add("company");
            colList.Add("sales_amount");
            colList.Add("sales_amount_pre_1");
            colList.Add("sales_amount_pre_2");
            colList.Add("current_balance");
            colList.Add("different_pre_balance");
            colList.Add("pre1_balance");
            colList.Add("pre2_balance");
            colList.Add("pre3_balance");
            colList.Add("div3");
            colList.Add("average_balance");
            colList.Add("pre1_average_balance");
            colList.Add("pre2_average_balance");
            colList.Add("pre3_average_balance");
            colList.Add("div4");
            colList.Add("around_month");
            colList.Add("around_day");
            colList.Add("month_margin");
            colList.Add("cumulative_margin");
            colList.Add("pre_cumulative_margin");
            colList.Add("div1");
            colList.Add("break_even_balance1");
            colList.Add("principal_payback_period1");
            colList.Add("grade1");
            colList.Add("div2");
            colList.Add("net_working_capital_cost");
            colList.Add("working_capital_cost");
            colList.Add("ato_capital_cost");
            colList.Add("equity_capital_cost");
            colList.Add("ato_capital_rate");
            colList.Add("capital_amount");
            colList.Add("accumulate_capital_amount");
            colList.Add("break_even_balance2");
            colList.Add("principal_payback_period2");
            colList.Add("grade2");
            colList.Add("sales_division_margin");
            colList.Add("div5");
            colList.Add("updatetime");
            colList.Add("manager");
            colList.Add("accumulate_balance_amount");
            colList.Add("accumulate_average_balance_amount");
            GetExeclColumn(colList);
        }
        private void cbSalesAmount_CheckedChanged(object sender, EventArgs e)
        {
            dgvCompany.Columns["sales_amount_pre_1"].Visible = cbSalesAmount.Checked;
            dgvCompany.Columns["sales_amount_pre_2"].Visible = cbSalesAmount.Checked;
            dgvCompany.Columns["sales_amount_pre_3"].Visible = cbSalesAmount.Checked;
        }
        private void cbBalance_CheckedChanged(object sender, EventArgs e)
        {
            dgvCompany.Columns["pre1_balance"].Visible = cbBalance.Checked;
            dgvCompany.Columns["pre2_balance"].Visible = cbBalance.Checked;
            dgvCompany.Columns["pre3_balance"].Visible = cbBalance.Checked;
        }

        private void cbAverageMargin_CheckedChanged(object sender, EventArgs e)
        {
            dgvCompany.Columns["pre1_average_balance"].Visible = cbAverageMargin.Checked;
            dgvCompany.Columns["pre2_average_balance"].Visible = cbAverageMargin.Checked;
            dgvCompany.Columns["pre3_average_balance"].Visible = cbAverageMargin.Checked;
        }

        private void rbNetWorkingCapital_CheckedChanged(object sender, EventArgs e)
        {
            Font font = new Font("나눔고딕", 9, FontStyle.Regular);
            dgvCompany.Columns["net_working_capital_cost"].DefaultCellStyle.Font = font;
            dgvCompany.Columns["net_working_capital_cost"].HeaderCell.Style.Font = font;
            dgvCompany.Columns["working_capital_cost"].DefaultCellStyle.Font = font;
            dgvCompany.Columns["working_capital_cost"].HeaderCell.Style.Font = font;
            dgvCompany.Columns["ato_capital_cost"].DefaultCellStyle.Font = font;
            dgvCompany.Columns["ato_capital_cost"].HeaderCell.Style.Font = font;
            dgvCompany.Columns["equity_capital_cost"].DefaultCellStyle.Font = font;
            dgvCompany.Columns["equity_capital_cost"].HeaderCell.Style.Font = font;

            font = new Font("나눔고딕", 9, FontStyle.Bold);
            if (rbNetWorkingCapital.Checked)
            {
                dgvCompany.Columns["net_working_capital_cost"].DefaultCellStyle.Font = font;
                dgvCompany.Columns["net_working_capital_cost"].HeaderCell.Style.Font = font;
                dgvCompany.Columns["ato_capital_rate"].Visible = false;
            }
            else if (rbWorkingCapital.Checked)
            {
                dgvCompany.Columns["working_capital_cost"].DefaultCellStyle.Font = font;
                dgvCompany.Columns["working_capital_cost"].HeaderCell.Style.Font = font;
                dgvCompany.Columns["ato_capital_rate"].Visible = false;
            }
            else if (rbAtoCapital.Checked)
            {
                dgvCompany.Columns["ato_capital_cost"].DefaultCellStyle.Font = font;
                dgvCompany.Columns["ato_capital_cost"].HeaderCell.Style.Font = font;
                dgvCompany.Columns["ato_capital_rate"].Visible = true;
            }
            else if (rbEquityCapital.Checked)
            { 
                dgvCompany.Columns["equity_capital_cost"].DefaultCellStyle.Font = font;
                dgvCompany.Columns["equity_capital_cost"].HeaderCell.Style.Font = font;
                dgvCompany.Columns["ato_capital_rate"].Visible = false;
            }

            calculate();
        }
        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetPartner();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


        #endregion


        #region Excel download
        public void GetExeclColumn(List<string> col_name)
        {
            if (col_name.Count == 0)
                return;

            int col_cnt = col_name.Count;

            try
            {
                excelApp = new Excel.Application();                                                 //엑셀 어플리케이션 생성
                workBook = excelApp.Workbooks.Add();                                                //워크북 추가
                workSheet = workBook.Worksheets.get_Item(1) as Excel.Worksheet;                     //엑셀 첫번째 워크시트 가져오기
                Microsoft.Office.Interop.Excel.Worksheet wk = workSheet;

                setAutomatic(excelApp, false);
                //excelApp.Visible = true;
                Excel.Range rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[1, col_cnt]];
                rg1.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                rg1.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                rg1.RowHeight = 18;
                rg1.ColumnWidth = 15;

                //Title
                rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[1, col_cnt]];
                rg1.Font.Size = 11;
                rg1.Font.Bold = true;

                //Col_index
                List<int> col_idx = new List<int>();
                for (int i = 0; i < col_name.Count; i++)
                {
                    for (int j = 0; j < dgvCompany.Columns.Count; j++)
                    {
                        if (dgvCompany.Columns[j].Name == col_name[i])
                        {
                            col_idx.Add(j);
                            wk.Cells[1, i + 1].value = dgvCompany.Columns[j].HeaderText;
                            break;
                        }
                    }
                }
                //Data
                for (int i = 0; i < dgvCompany.Rows.Count; i++)
                {
                    for (int j = 0; j < col_idx.Count; j++)
                    {
                        if (dgvCompany.Rows[i].Cells[col_idx[j]].Value == null)
                            dgvCompany.Rows[i].Cells[col_idx[j]].Value = string.Empty;
                        if(j == 0)
                            wk.Cells[i + 2, j + 1] = "'" + dgvCompany.Rows[i].Cells[col_idx[j]].Value.ToString();
                        else 
                            wk.Cells[i + 2, j + 1] = dgvCompany.Rows[i].Cells[col_idx[j]].Value.ToString();
                    }
                }

                for (int i = 0; i < col_name.Count; i++)
                {
                    for (int j = 0; j < dgvCompany.Columns.Count; j++)
                    {
                        if (dgvCompany.Columns[j].Name == col_name[i])
                        {
                            if (col_name[i].Contains("div1") 
                                || col_name[i].Contains("div2")
                                || col_name[i].Contains("div3")
                                || col_name[i].Contains("div4")
                                || col_name[i].Contains("div5")
                                || col_name[i].Contains("div6"))
                            {
                                rg1 = wk.Range[wk.Cells[1, i + 1], wk.Cells[dgvCompany.Rows.Count + 1, i + 1]];
                                rg1.ColumnWidth = 1.5;
                                rg1.Interior.Color = Color.DarkGray;
                            }

                            break;
                        }
                    }
                }



                //Border Line Style
                rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[1, col_cnt]];
                rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                rg1.Borders.Weight = Excel.XlBorderWeight.xlThin;
                rg1.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);

                rg1 = wk.Range[wk.Cells[2, 1], wk.Cells[dgvCompany.Rows.Count + 1, col_cnt]];
                rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                //속도개선 ON
                setAutomatic(excelApp, true);
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message.ToString() + "\n 생성 중 에러가 발생하였습니다.");
                this.Activate();
                setAutomatic(excelApp, true);
                ReleaseObject(workSheet);
                ReleaseObject(workBook);
                ReleaseObject(excelApp);
            }
            finally
            {
                ReleaseObject(workSheet);
                ReleaseObject(workBook);
                ReleaseObject(excelApp);
            }
        }

        //Excel속도개선
        private void setAutomatic(Excel.Application excel, bool auto)
        {
            if (auto)
            {
                excel.DisplayAlerts = true;
                excel.Visible = true;
                excel.ScreenUpdating = true;
                excel.DisplayStatusBar = true;
                excel.Calculation = Excel.XlCalculation.xlCalculationAutomatic;
                excel.EnableEvents = true;
            }
            else
            {
                excel.DisplayAlerts = false;
                excel.Visible = false;
                excel.ScreenUpdating = false;
                excel.DisplayStatusBar = false;
                excel.Calculation = Excel.XlCalculation.xlCalculationManual;
                excel.EnableEvents = false;
            }
        }
        /// <summary>
        /// 엑셀 객체 해재 메소드
        /// </summary>
        /// <param name="obj"></param>
        static void ReleaseObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);   //엑셀객체 해제
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();  //가비지 수집
            }
        }
        #endregion

    }
    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }
}
