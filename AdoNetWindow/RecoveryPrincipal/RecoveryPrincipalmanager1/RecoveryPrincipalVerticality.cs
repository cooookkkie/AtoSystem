using AdoNetWindow.Model;
using Repositories;
using Repositories.Company;
using Repositories.Config;
using Repositories.RecoveryPrincipal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.RecoveryPrincipal
{
    public partial class RecoveryPrincipalVerticality : Form
    {
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        ICompanyRecoveryRepository companyRecoveryRepository = new CompanyRecoveryRepository();
        ICommonRepository commonRepository = new CommonRepository();
        ICompanyRepository companyRepository = new CompanyRepository(); 
        Libs.Tools.Common common = new Libs.Tools.Common();
        IUsersRepository usersRepository = new UsersRepository();
        IRecoveryPrincipalRepository recoveryPrincipalRepository = new RecoveryPrincipalRepository();
        IRecoveryPrincipalPriceRepository recoveryPrincipalPriceRepository = new RecoveryPrincipalPriceRepository();
        UsersModel um;

        public RecoveryPrincipalVerticality(UsersModel uModel, string company, string manager, DateTime sdate)
        {
            InitializeComponent();
            um = uModel;
            cbCompany.Text = company;
            cbManager.Text = manager;

            cbSttYear.Text = sdate.AddYears(-1).Year.ToString();
            cbSttMonth.Text = sdate.AddYears(-1).Month.ToString();

            cbEndYear.Text = sdate.Year.ToString();
            cbEndMonth.Text = sdate.Month.ToString();
            SetDgvStyleSetting();
            GetCompanyInfo();
        }

        private void RecoveryPrincipalVerticality_Load(object sender, EventArgs e)
        {
            cbManager.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbManager.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbCompany.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbCompany.AutoCompleteSource = AutoCompleteSource.ListItems;
            //기준년월
            for (int i = DateTime.Now.Year - 10; i <= DateTime.Now.Year; i++)
            {
                cbSttYear.Items.Add(i);
                cbEndYear.Items.Add(i);
            }
            //거래처
            /*DataTable companyDt = recoveryPrincipalPriceRepository.GetCompanyList("");
            if (companyDt.Rows.Count > 0)
            {
                for (int i = 0; i < companyDt.Rows.Count; i++)
                {
                    cbCompany.Items.Add(companyDt.Rows[i]["거래처명"].ToString());
                }
            }
            //담당자
            cbManager.Items.Add("전체");
            DataTable managerDt = usersRepository.GetTeamMember("");
            if (managerDt.Rows.Count > 0)
            {
                for (int i = 0; i < managerDt.Rows.Count; i++)
                {
                    cbManager.Items.Add(managerDt.Rows[i]["user_name"].ToString());
                }
            }*/

            GetData();
        }

        #region Method
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        private void InsertRecoveryPrincipal()
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

            if (string.IsNullOrEmpty(cbCompany.Text))
            {
                MessageBox.Show(this, "거래처를 선택해주세요.");
                this.Activate();
            }

            //Seaover 거래처 내역
            DataTable companyDt = seaoverRepository.SelectData("업체별월별매출현황", "*", "", $"거래처명='{cbCompany.Text}'", "사용자 DESC");
            if (companyDt.Rows.Count == 0)
            {
                MessageBox.Show(this,"거래처 정보를 찾을 수 없습니다.");
                this.Activate();
                return;
            }

            //Msg
            if (MessageBox.Show(this, "등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;


            //원금회수율 데이터
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
            //거래처 기본정보 저장, 수정
            CompanyModel cm = new CompanyModel();
            DataTable atoCompanyDt = companyRepository.GetCompanyRecovery("", "", false, "", companyDt.Rows[0]["거래처코드"].ToString());
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
                cm.ato_manager = companyDt.Rows[0]["매출자"].ToString(); ;
                cm.isManagement1 = cbIsManagement.Checked;
                cm.isManagement2 = cbIsManagement2.Checked;
                cm.isManagement3 = cbIsManagement3.Checked;
                cm.isManagement4 = cbIsManagement4.Checked;
                cm.isTrading = true;
                cm.isHide = false;
                //cm.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                cm.createtime = DateTime.Now.ToString("yyyy-MM-dd");
                cm.edit_user = um.user_name;
                cm.seaover_company_code = companyDt.Rows[0]["거래처코드"].ToString();

                //목표개월수
                rm.target_recovery_month = 18;

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
                cm.isManagement1 = cbIsManagement.Checked;
                cm.isManagement2 = cbIsManagement2.Checked;
                cm.isManagement3 = cbIsManagement3.Checked;
                cm.isManagement4 = cbIsManagement4.Checked;
                cm.isHide = Convert.ToBoolean(atoCompanyDt.Rows[0]["isHide"].ToString());
                //cm.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                cm.createtime = atoCompanyDt.Rows[0]["createtime"].ToString();
                cm.edit_user = um.user_name;
                cm.seaover_company_code = companyDt.Rows[0]["거래처코드"].ToString();
                cm.id = Convert.ToInt32(atoCompanyDt.Rows[0]["id"].ToString());

                //목표개월수
                rm.target_recovery_month = Convert.ToInt32(atoCompanyDt.Rows[0]["target_recovery_month"].ToString());

                //Update
                sql = companyRepository.UpdateCompany(cm);
                sqlList.Add(sql);
            }

            rm.remark = txtRemark.Text;
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
        private void GetCompanyInfo()
        {
            if (!string.IsNullOrEmpty(cbCompany.Text))
            {
                //Seaover 거래처 내역
                DataTable companyDt = seaoverRepository.SelectData("업체별월별매출현황", "*", "", $"거래처명='{cbCompany.Text}'", "사용자 DESC");
                if (companyDt.Rows.Count == 0)
                {
                    MessageBox.Show(this,"거래처 정보를 찾을 수 없습니다.");
                    this.Activate();
                    return;
                }
                else
                {
                    //Ato 전산에 등록된 거래처 내역
                    DataTable atoCompanyDt = companyRepository.GetCompanyRecovery("", "", true, "", companyDt.Rows[0]["거래처코드"].ToString());
                    if (atoCompanyDt.Rows.Count > 0)
                    {
                        txtSalesProfit.Text = atoCompanyDt.Rows[0]["last_year_profit"].ToString();
                        txtNetWorkingCapital.Text = atoCompanyDt.Rows[0]["net_operating_capital_rate"].ToString();
                        txtWorkingCapital.Text = atoCompanyDt.Rows[0]["operating_capital_rate"].ToString();
                        txtAtoCapital.Text = atoCompanyDt.Rows[0]["ato_capital_rate"].ToString();
                        txtEquityCapital.Text = atoCompanyDt.Rows[0]["equity_capital_rate"].ToString();
                        txtRemark.Text = atoCompanyDt.Rows[0]["recovery_remark"].ToString(); ;

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

                        cbIsManagement.Checked = isManagement1;
                        cbIsManagement2.Checked = isManagement2;
                        cbIsManagement3.Checked = isManagement3;
                        cbIsManagement4.Checked = isManagement4;

                    }
                    else
                    {
                        txtSalesProfit.Text = "5.5";
                        txtNetWorkingCapital.Text = "13.64";
                        txtWorkingCapital.Text = "10.45";
                        txtAtoCapital.Text = "0.8";
                        txtEquityCapital.Text = "21.47";
                        txtRemark.Text = String.Empty;
                    }
                }
            }
        }
        private void SetDgvStyleSetting()
        {
            dgvPrinciipal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPrinciipal.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            dgvPrinciipal.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(84, 130, 53);
            dgvPrinciipal.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgvPrinciipal.Columns["receivable_account"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            dgvPrinciipal.Columns["average_receivable_account"].DefaultCellStyle.BackColor = Color.FromArgb(153, 204, 255);

            dgvPrinciipal.Columns["net_working_capital"].Visible = false;
            dgvPrinciipal.Columns["working_capital"].Visible = false;
            dgvPrinciipal.Columns["ato_capital"].Visible = false;
            dgvPrinciipal.Columns["equity_capital"].Visible = false;
        }


        private void GetData()
        {
            dgvPrinciipal.Rows.Clear();
            //검색년도 유효성 검사
            int sttYear, sttMonth, endYear, endMonth;
            if (!int.TryParse(cbSttYear.Text, out sttYear) || !int.TryParse(cbSttMonth.Text, out sttMonth) 
                || !int.TryParse(cbEndYear.Text, out endYear) || !int.TryParse(cbEndMonth.Text, out endMonth))
            {
                MessageBox.Show(this,"검색년월을 설정해주세요.");
                this.Activate();
                return;
            }
            DateTime sttdate, enddate;
            try 
            {
                sttdate = new DateTime(sttYear, sttMonth, 1);
                enddate = new DateTime(endYear, endMonth, 1);
            }
            catch
            {
                MessageBox.Show(this,"검색년월을 날짜 형식에서 벗어납니다. 다시 설정해주세요.");
                this.Activate();
                return;
            }
            //거래처 정보가져오기
            string company = cbCompany.Text;
            //매출금액, 미수잔액
            DataTable cpDt = recoveryPrincipalRepository.GetRecoerPrincipal(enddate.ToString("yyyyMM"), cbCompany.Text);
            if (cpDt != null && cpDt.Rows.Count > 0)
            {
                //회전율 및 재무재표 내역
                double net_working_capital;
                if (!double.TryParse(txtNetWorkingCapital.Text, out net_working_capital))
                    net_working_capital = 0;
                double working_capital;
                if (!double.TryParse(txtWorkingCapital.Text, out working_capital))
                    working_capital = 0;
                double ato_capital;
                if (!double.TryParse(txtAtoCapital.Text, out ato_capital))
                    ato_capital = 0.8;
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

                //순회하면서 출력
                double cumulative_sales_price = 0;
                double cumulative_balance_price = 0;
                double cumulative_month_margin = 0;
                double cumulative_net_working_capital = 0;
                double cumulative_working_capital = 0;
                double cumulative_ato_capital = 0;
                double cumulative_equity_capital = 0;
                double average_balance = 0;
                double average_cnt = 0;
                double average_margin_rate = 0;
                double busniess_months = 0;
                int n;
                DataGridViewRow row;
                for (int i = 0; i < cpDt.Rows.Count; i++)
                {
                    DateTime sdate = Convert.ToDateTime(cpDt.Rows[i]["기준년월"].ToString());
                    //거래기간
                    if (Convert.ToDouble(cpDt.Rows[i]["매출금액"].ToString()) > 0 || Convert.ToDouble(cpDt.Rows[i]["미수잔액"].ToString()) > 0)
                        busniess_months++;
                    //기간내 데이터 출력
                    if (sttdate <= sdate)
                    {
                        //전월내역 출력
                        if (dgvPrinciipal.Rows.Count == 0)
                        {
                            n = dgvPrinciipal.Rows.Add();
                            row = dgvPrinciipal.Rows[n];
                            //row.DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);

                            row.Cells["year"].Value = "전월";
                            //평잔
                            double average_receivable_account = average_balance / average_cnt;
                            if (double.IsNaN(average_receivable_account))
                                average_receivable_account = 0;
                            row.Cells["average_receivable_account"].Value = average_receivable_account.ToString("#,##0");
                            //기회비용
                            if (rbNetWorkingCapital.Checked)
                                row.Cells["cumulative_capital_amount"].Value = cumulative_net_working_capital.ToString("#,##0");
                            else if (rbWorkingCapital.Checked)
                                row.Cells["cumulative_capital_amount"].Value = cumulative_working_capital.ToString("#,##0");
                            else if (rbAtoCapital.Checked)
                                row.Cells["cumulative_capital_amount"].Value = cumulative_ato_capital.ToString("#,##0");
                            else if (rbEquityCapital.Checked)
                                row.Cells["cumulative_capital_amount"].Value = cumulative_equity_capital.ToString("#,##0");
                            //누적매출, 마진
                            row.Cells["cumulative_sales_price"].Value = cumulative_sales_price.ToString("#,##0");
                            row.Cells["cumulative_month_margin"].Value = cumulative_month_margin.ToString("#,##0");
                            //총 미수잔액 
                            row.Cells["receivable_account"].Value = cumulative_balance_price.ToString("#,##0");
                            //평균마진율
                            average_margin_rate = cumulative_month_margin / cumulative_sales_price * 100;
                            if (double.IsNaN(average_margin_rate))
                                average_margin_rate = 0;
                            row.Cells["average_margin_rate"].Value = average_margin_rate.ToString("#,##0.00");
                        }

                        //검색년월 안의 내역
                        n = dgvPrinciipal.Rows.Add();
                        row = dgvPrinciipal.Rows[n];
                        row.DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Regular);

                        row.Cells["year"].Value = sdate.Year.ToString();
                        row.Cells["month"].Value = sdate.Month.ToString();
                        row.Cells["sales_price"].Value = Convert.ToDouble(cpDt.Rows[i]["매출금액"].ToString()).ToString("#,##0");
                        row.Cells["receivable_account"].Value = Convert.ToDouble(cpDt.Rows[i]["미수잔액"].ToString()).ToString("#,##0");
                        row.Cells["average_receivable_account"].Value = Convert.ToDouble(cpDt.Rows[i]["월평균잔액"].ToString()).ToString("#,##0");
                        row.Cells["month_margin"].Value = Convert.ToDouble(cpDt.Rows[i]["손익금액"].ToString()).ToString("#,##0");
                        //row.Cells["month_margin"].Style.Font = new Font("중고딕", 9, FontStyle.Bold);

                        

                        //기회비용
                        double net_working_amount = Convert.ToDouble(cpDt.Rows[i]["월평균잔액"].ToString()) * net_working_capital * sales_profit / 12;
                        cumulative_net_working_capital += net_working_amount;
                        double working_amount = Convert.ToDouble(cpDt.Rows[i]["월평균잔액"].ToString()) * working_capital * sales_profit / 12;
                        cumulative_working_capital += working_amount;
                        double ato_amount = Convert.ToDouble(cpDt.Rows[i]["월평균잔액"].ToString()) * ato_capital * 3;
                        cumulative_ato_capital += ato_amount;
                        double equity_amount = Convert.ToDouble(cpDt.Rows[i]["월평균잔액"].ToString()) * equity_capital * sales_profit / 12;
                        cumulative_equity_capital += equity_amount;
                        row.Cells["net_working_capital"].Value = cumulative_net_working_capital.ToString("#,##0");
                        row.Cells["net_working_capital"].Style.Font = new Font("중고딕", 7, FontStyle.Regular);
                        row.Cells["working_capital"].Value = cumulative_working_capital.ToString("#,##0");
                        row.Cells["working_capital"].Style.Font = new Font("중고딕", 7, FontStyle.Regular);
                        row.Cells["ato_capital"].Value = cumulative_ato_capital.ToString("#,##0");
                        row.Cells["ato_capital"].Style.Font = new Font("중고딕", 7, FontStyle.Regular);
                        row.Cells["equity_capital"].Value = cumulative_equity_capital.ToString("#,##0");
                        row.Cells["equity_capital"].Style.Font = new Font("중고딕", 7, FontStyle.Regular);


                        //손익분기잔액1
                        double principal_recovery_amount = Convert.ToDouble(cpDt.Rows[i]["미수잔액"].ToString()) - cumulative_month_margin;
                        row.Cells["principal_recovery_amount1"].Value = principal_recovery_amount.ToString("#,##0");
                        //row.Cells["principal_recovery_amount1"].Style.Font = new Font("중고딕", 9, FontStyle.Bold);
                        //원금회수기간
                        double principal_recovery_month1 = (principal_recovery_amount / Convert.ToDouble(cpDt.Rows[i]["손익금액"].ToString()));
                        row.Cells["principal_recovery_month1"].Value = principal_recovery_month1.ToString("#,##0.00");
                        //row.Cells["principal_recovery_month1"].Style.Font = new Font("중고딕", 9, FontStyle.Bold);

                        if (double.IsNaN(principal_recovery_month1))  //Nan
                            principal_recovery_month1 = 0;
                        else if (double.IsInfinity(principal_recovery_month1))  //Infinity
                            principal_recovery_month1 = 9999;
                        else if (principal_recovery_month1 > 9999)  //Max
                            principal_recovery_month1 = 9999;

                        //업체등급
                        int grade;
                        string grade_remark;
                        GetGrade(principal_recovery_month1, out grade, out grade_remark);

                        row.Cells["grade1"].Value = grade;
                        //row.Cells["grade1"].Style.Font = new Font("중고딕", 7, FontStyle.Bold);
                        //row.Cells["grade1"].ToolTipText = grade_remark;

                        //손익분기잔액2
                        double month_capital_amount = 0;
                        if (rbNetWorkingCapital.Checked)
                        {
                            month_capital_amount = net_working_amount;
                            principal_recovery_amount += cumulative_net_working_capital;

                            //기회비용 발생/누적금액
                            row.Cells["capital_amount"].Value = net_working_amount.ToString("#,##0");
                            row.Cells["cumulative_capital_amount"].Value = cumulative_net_working_capital.ToString("#,##0");
                            row.Cells["capital_amount"].ToolTipText = "(순영업자본 회전율 x 평균미수금 x 작년 영업이익) / 12개월";
                            row.Cells["cumulative_capital_amount"].ToolTipText = "(순영업자본 회전율 x 평균미수금 x 작년 영업이익) / 12개월";
                        }
                        else if (rbWorkingCapital.Checked)
                        {
                            month_capital_amount = working_amount;
                            principal_recovery_amount += cumulative_working_capital;

                            //기회비용 발생/누적금액
                            row.Cells["capital_amount"].Value = working_amount.ToString("#,##0");
                            row.Cells["cumulative_capital_amount"].Value = cumulative_working_capital.ToString("#,##0");
                            row.Cells["capital_amount"].ToolTipText = "(영업자본 회전율 x 평균미수금 x 작년 영업이익) / 12개월";
                            row.Cells["cumulative_capital_amount"].ToolTipText = "(영업자본 회전율 x 평균미수금 x 작년 영업이익) / 12개월";
                        }
                        else if (rbAtoCapital.Checked)
                        {
                            month_capital_amount = ato_amount;
                            principal_recovery_amount += cumulative_ato_capital;

                            //기회비용 발생/누적금액
                            row.Cells["capital_amount"].Value = ato_amount.ToString("#,##0");
                            row.Cells["cumulative_capital_amount"].Value = cumulative_ato_capital.ToString("#,##0");
                            row.Cells["capital_amount"].ToolTipText = "(평균미수금 x 판매가능기준) * 3";
                            row.Cells["cumulative_capital_amount"].ToolTipText = "(평균미수금 x 판매가능기준) * 3";
                        }
                        else if (rbEquityCapital.Checked)
                        {
                            month_capital_amount = equity_amount;
                            principal_recovery_amount += cumulative_equity_capital;

                            //기회비용 발생/누적금액
                            row.Cells["capital_amount"].Value = equity_amount.ToString("#,##0");
                            row.Cells["cumulative_capital_amount"].Value = cumulative_equity_capital.ToString("#,##0");
                            row.Cells["capital_amount"].ToolTipText = "(자기자본 회전율 x 평균미수금 x 작년 영업이익) / 12개월";
                            row.Cells["cumulative_capital_amount"].ToolTipText = "(자기자본 회전율 x 평균미수금 x 작년 영업이익) / 12개월";
                        }
                        

                        //원금회수기간
                        row.Cells["principal_recovery_amount2"].Value = principal_recovery_amount.ToString("#,##0");
                        //row.Cells["principal_recovery_amount2"].Style.Font = new Font("중고딕", 9, FontStyle.Bold);


                        //원금회수기간2
                        double month_margin = Convert.ToDouble(cpDt.Rows[i]["손익금액"].ToString());
                        double principal_recovery_month2;
                        if (month_margin < 0)
                            principal_recovery_month2 = principal_recovery_amount / 0;
                        else
                        {
                            double real_month_margin = month_margin - month_capital_amount;

                            if (real_month_margin <= 0)
                                principal_recovery_month2 = principal_recovery_amount / 0;
                            else
                                principal_recovery_month2 = principal_recovery_amount / real_month_margin;  //손익분기잔액1 / (한달마진 - 당월발생 기회비용)
                        }

                        if (double.IsNaN(principal_recovery_month2))  //Nan
                            principal_recovery_month2 = 0;
                        else if (double.IsInfinity(principal_recovery_month2))  //Infinity
                            principal_recovery_month2 = 9999;
                        else if (principal_recovery_month2 > 9999)  //Max
                            principal_recovery_month2 = 9999;

                        row.Cells["principal_recovery_month2"].Value = principal_recovery_month2.ToString("#,##0.00");
                        //row.Cells["principal_recovery_month2"].Style.Font = new Font("중고딕", 9, FontStyle.Bold);

                        //업체등급
                        int grade2;
                        string grade_remark2;
                        GetGrade(principal_recovery_month2, out grade2, out grade_remark2);

                        row.Cells["grade2"].Value = grade2;
                        //row.Cells["grade2"].Style.Font = new Font("중고딕", 7, FontStyle.Bold);
                        //row.Cells["grade2"].ToolTipText = grade_remark2;
                        //dr보고유무.Cells[i].Value = grade_remark;


                        //누적매출, 마진
                        cumulative_sales_price += Convert.ToDouble(cpDt.Rows[i]["매출금액"].ToString());
                        cumulative_month_margin += Convert.ToDouble(cpDt.Rows[i]["손익금액"].ToString());
                        row.Cells["cumulative_sales_price"].Value = cumulative_sales_price.ToString("#,##0");
                        row.Cells["cumulative_month_margin"].Value = cumulative_month_margin.ToString("#,##0");

                        //총 미수잔액
                        cumulative_balance_price += Convert.ToDouble(cpDt.Rows[i]["미수잔액"].ToString());

                        //평균마진율
                        average_margin_rate = Convert.ToDouble(cpDt.Rows[i]["손익금액"].ToString()) / Convert.ToDouble(cpDt.Rows[i]["매출금액"].ToString()) * 100;
                        if (double.IsNaN(average_margin_rate))
                            average_margin_rate = 0;
                        row.Cells["average_margin_rate"].Value = average_margin_rate.ToString("#,##0.00");

                        //매출채권회전수
                        double salse_month_count_current_month = Convert.ToDouble(row.Cells["sales_price"].Value.ToString()) / Convert.ToDouble(row.Cells["receivable_account"].Value.ToString());
                        double salse_month_count = cumulative_sales_price / cumulative_balance_price;
                        row.Cells["accounts_receivable_count"].Value = salse_month_count.ToString("#,##0.00") + " (" + salse_month_count_current_month.ToString("#,##0.00") + ")";

                        //매출채권회일수
                        double sales_day_coun_current_montht = 30 / salse_month_count_current_month;
                        double sales_day_count = 30 / salse_month_count;
                        row.Cells["accounts_receivable_days"].Value = sales_day_count.ToString("#,##0") + " (" + sales_day_coun_current_montht.ToString("#,##0") + ")";
                        //매출회전일수 / 마진율
                        if (average_margin_rate < 0)
                            average_margin_rate = 0;
                        double margin_div = Math.Round(sales_day_coun_current_montht) / Math.Round(average_margin_rate, 2);
                        if(double.IsNaN(margin_div))
                            margin_div = 0;
                        row.Cells["accounts_receivable_count_division"].Value = margin_div.ToString("#,##0.00");

                        //ToolTip
                        row.Cells["principal_recovery_amount1"].ToolTipText = "미수금 - 전월 누적마진금액";
                        row.Cells["principal_recovery_amount2"].ToolTipText = "미수금 - 전월 누적마진금액 + 기회비용 누적금액";
                        string grade_tooltip = "90이상 유지 (원금회수기간 6달 이하)"
                                    + "\n 80이상 유지(관리)  (원금회수기간 11달 이하)"
                                    + "\n 70이상 수시관리↑  (원금회수기간 16달 이하)"
                                    + "\n 60이상 수시관리(조심)  (원금회수기간 21달 이하)"
                                    + "\n 50이상 보고  (원금회수기간 26달 이하)"
                                    + "\n 50미만 보고필 (원금회수기간 26달 초과) ";
                        row.Cells["grade1"].ToolTipText = grade_tooltip;
                        row.Cells["grade2"].ToolTipText = grade_tooltip;
                    }
                    //전월 데이터 누적
                    else
                    {
                        //기회비용
                        cumulative_net_working_capital += Convert.ToDouble(cpDt.Rows[i]["월평균잔액"].ToString()) * net_working_capital * sales_profit / 12;
                        cumulative_working_capital += Convert.ToDouble(cpDt.Rows[i]["월평균잔액"].ToString()) * working_capital * sales_profit / 12;
                        cumulative_ato_capital += Convert.ToDouble(cpDt.Rows[i]["월평균잔액"].ToString()) * ato_capital * 3;
                        cumulative_equity_capital += Convert.ToDouble(cpDt.Rows[i]["월평균잔액"].ToString()) * equity_capital * sales_profit / 12;
                        //누적매출, 마진
                        cumulative_sales_price += Convert.ToDouble(cpDt.Rows[i]["매출금액"].ToString());
                        cumulative_balance_price += Convert.ToDouble(cpDt.Rows[i]["미수잔액"].ToString());
                        cumulative_month_margin += Convert.ToDouble(cpDt.Rows[i]["손익금액"].ToString());

                        average_balance += Convert.ToDouble(cpDt.Rows[i]["월평균잔액"].ToString());
                        average_cnt++;
                    }
                }
                txtBusinessTerms.Text = busniess_months.ToString("#,##0");
            }  
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

        #region Radio, Button
        private void btnInsert_Click(object sender, EventArgs e)
        {
            InsertRecoveryPrincipal();
        }
        private void RecoveryPrincipalVerticality_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        GetData();
                        break;
                    case Keys.A:
                        btnInsert.PerformClick();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
        }
        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetData();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void rbNetWorkingCapital_CheckedChanged(object sender, EventArgs e)
        {
            GetData();
        }
        private void btnManagment_Click(object sender, EventArgs e)
        {
            cbIsManagement.Checked = !cbIsManagement.Checked;
            if (cbIsManagement.Checked)
            {
                cbIsManagement2.Checked = false;
                cbIsManagement3.Checked = false;
                cbIsManagement4.Checked = false;
            }
        }

        private void cbIsManagement_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsManagement.Checked)
                btnManagment.BackgroundImage = Properties.Resources.Star_icon1;
            else
                btnManagment.BackgroundImage = Properties.Resources.Star_empty_icon;
        }
        private void btnManagment2_Click(object sender, EventArgs e)
        {
            cbIsManagement2.Checked = !cbIsManagement2.Checked;
            if (cbIsManagement2.Checked)
            {
                cbIsManagement.Checked = false;
                cbIsManagement3.Checked = false;
                cbIsManagement4.Checked = false;
            }
        }

        private void btnManagment3_Click(object sender, EventArgs e)
        {
            cbIsManagement3.Checked = !cbIsManagement3.Checked;
            if (cbIsManagement3.Checked)
            {
                cbIsManagement.Checked = false;
                cbIsManagement2.Checked = false;
                cbIsManagement4.Checked = false;
            }
        }

        private void btnManagment4_Click(object sender, EventArgs e)
        {
            cbIsManagement4.Checked = !cbIsManagement4.Checked;
            if (cbIsManagement4.Checked)
            {
                cbIsManagement.Checked = false;
                cbIsManagement2.Checked = false;
                cbIsManagement3.Checked = false;
            }
        }

        private void cbIsManagement2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsManagement2.Checked)
                btnManagment2.BackgroundImage = Properties.Resources.Star_icon2;
            else
                btnManagment2.BackgroundImage = Properties.Resources.Star_empty_icon;
        }

        private void cbIsManagement3_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsManagement3.Checked)
                btnManagment3.BackgroundImage = Properties.Resources.Star_icon3;
            else
                btnManagment3.BackgroundImage = Properties.Resources.Star_empty_icon;
        }

        private void cbIsManagement4_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsManagement4.Checked)
                btnManagment4.BackgroundImage = Properties.Resources.Star_icon4;
            else
                btnManagment4.BackgroundImage = Properties.Resources.Star_empty_icon;
        }
        #endregion

        #region Datagridview event
        private void dgvPrinciipal_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            int a = int.Parse(e.CellValue1.ToString()), b = int.Parse(e.CellValue2.ToString());
            e.SortResult = a.CompareTo(b);
            e.Handled = true;
        }
        private void dgvPrinciipal_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvPrinciipal.Columns[e.ColumnIndex].Name != "grade")
                {
                    double d;
                    if (dgvPrinciipal.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && double.TryParse(dgvPrinciipal.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out d))
                    {
                        if (d < 0)
                            dgvPrinciipal.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
                        else
                            dgvPrinciipal.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                    }
                }
                else if (dgvPrinciipal.Columns[e.ColumnIndex].Name == "grade")
                {
                    double grade;
                    if (dgvPrinciipal.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !double.TryParse(dgvPrinciipal.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out grade))
                    {
                        grade = 0;
                    }
                    if (grade >= 80)
                    {
                        dgvPrinciipal.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Blue;
                    }
                    else if (grade < 50)
                    {
                        dgvPrinciipal.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
                    }
                }
            }
        }
        #endregion

        #region Key event
        private void txtSalesProfit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox tb = (TextBox)sender;
                tb.Text = common.GetN2(tb.Text);
                GetData();
            }
        }

        private void txtSalesProfit_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == (Char)45 || e.KeyChar == (Char)46 || e.KeyChar == (Char)47))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
            //이전값이 0일경우 삭제 후 입력
            Control con = (Control)sender;
            if (con.Text == "0")
            {
                con.Text = "";
            }
        }


        #endregion

        
    }
}
