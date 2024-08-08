using AdoNetWindow.Model;
using Repositories;
using Repositories.Company;
using Repositories.Config;
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

namespace AdoNetWindow.RecoveryPrincipal
{
    public partial class RecoveryPrincipalManager : Form
    {
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        ICompanyRepository companyRepository = new CompanyRepository();
        ICompanyRecoveryRepository companyRecoveryRepository = new CompanyRecoveryRepository();
        ICommonRepository commonRepository = new CommonRepository();
        IRecoveryPrincipalRepository recoveryPrincipalRepository = new RecoveryPrincipalRepository();
        IRecoveryPrincipalPriceRepository recoveryPrincipalPriceRepository = new RecoveryPrincipalPriceRepository();
        ISalesPartnerRepository salesPartnerRepository = new SalesPartnerRepository();
        IUsersRepository usersRepository = new UsersRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        DataTable managerDt;
        UsersModel um;
        bool isLoadFlag = true;

        DataTable companyDt;

        //DatagridviewRow 
        DataGridViewRow dr당월매출;
        DataGridViewRow dr누적매출;
        DataGridViewRow dr미수금잔액;
        DataGridViewRow dr평균미수금;

        DataGridViewRow dr매출채권회전수;
        DataGridViewRow dr매출채권회전일수;
        DataGridViewRow dr평균마진율;

        
        DataGridViewRow dr손익분기잔액1;
        DataGridViewRow dr전월대비미수절감액;
        DataGridViewRow dr전월대비미수절감율;
        DataGridViewRow dr한달마진;
        DataGridViewRow dr누적마진;

        DataGridViewRow dr원금회수기간1;
        DataGridViewRow dr회전수나누기1;
        DataGridViewRow dr업체등급1;
        DataGridViewRow dr보고유무1;


        DataGridViewRow dr순영업자본;
        DataGridViewRow dr영업자본;
        DataGridViewRow dr아토영업;
        DataGridViewRow dr자기자본;

        DataGridViewRow dr기회비용발생금액;
        DataGridViewRow dr기회비용누적금액;


        DataGridViewRow dr손익분기잔액2;
        DataGridViewRow dr원금회수기간2;
        DataGridViewRow dr업체등급2;
        DataGridViewRow dr보고유무2;
        DataGridViewRow dr보고제출유무;

        DataGridViewRow dr1;
        DataGridViewRow dr1_1;
        DataGridViewRow dr1_2;
        DataGridViewRow dr1_3;
        DataGridViewRow dr2;
        DataGridViewRow dr2_1;
        DataGridViewRow dr2_2;

        DataGridViewRow drEmpty1;
        DataGridViewRow drEmpty2;
        DataGridViewRow drEmpty3;
        DataGridViewRow drEmpty4;

        public RecoveryPrincipalManager(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
        }
        public RecoveryPrincipalManager(UsersModel uModel, string company, string manager, DateTime sdate)
        {
            InitializeComponent();
            um = uModel;
            isLoadFlag = false;
            cbYear.Text = sdate.ToString("yyyy");
            cbMonth.Text = sdate.ToString("MM");
            //Datagridview 초기화
            dgvSales.Init(false);
            //담당자
            cbManager.Items.Add("전체");
            managerDt = usersRepository.GetTeamMember("");
            if (managerDt.Rows.Count > 0)
            {
                for (int i = 0; i < managerDt.Rows.Count; i++)
                {
                    cbManager.Items.Add(managerDt.Rows[i]["user_name"].ToString());
                }
            }
            //선택한 거래처정보
            cbManager.Text = manager;
            cbCompany.Text = company;
        }

        public RecoveryPrincipalManager(UsersModel uModel, string manager, string company, int years, int month = 0)
        {
            InitializeComponent();
            um = uModel;
            cbYear.Text = years.ToString();
            if (month == 0)
                month = DateTime.Now.Month;            
            cbMonth.Text = month.ToString();
            cbManager.Text = manager;
            cbCompany.Text = company;
            DgvInitialize();
            dgvSales.Init(false);
            //담당자
            cbManager.Items.Add("전체");
            managerDt = usersRepository.GetTeamMember("");
            if (managerDt.Rows.Count > 0)
            {
                for (int i = 0; i < managerDt.Rows.Count; i++)
                {
                    cbManager.Items.Add(managerDt.Rows[i]["user_name"].ToString());
                }
            }
            
            isLoadFlag = false;
        }
        private void RecoveryPrincipalManager_Load(object sender, EventArgs e)
        {           
            dgvSales.Init(false);
            //기준년월
            for (int i = DateTime.Now.Year - 10; i <= DateTime.Now.Year; i++)
            {
                cbYear.Items.Add(i);
            }
            //담당자
            cbManager.Items.Add("전체");
            managerDt = usersRepository.GetTeamMember("");
            if (managerDt.Rows.Count > 0)
            {
                for (int i = 0; i < managerDt.Rows.Count; i++)
                {
                    cbManager.Items.Add(managerDt.Rows[i]["user_name"].ToString());
                }
            }
            //Row추가
            DgvInitialize();
            //거래처 원금회수율 데이터
            GetCompanyInfo();
        }
        #region Method
        private void DgvInitialize()
        {
            this.dgvSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSales_CellValueChanged);

            dgvSales.Rows.Clear();

            int n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "당월 매출금액";    //0
            dr당월매출 = dgvSales.Rows[n];

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "누적 매출금액";    //1
            dr누적매출 = dgvSales.Rows[n];

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "미수금잔액";    //2
            dr미수금잔액= dgvSales.Rows[n];

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "평균 미수금잔액";    //3
            dr평균미수금 = dgvSales.Rows[n];

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "매출채권회전수(매출/외상)";    //4
            dr매출채권회전수 = dgvSales.Rows[n];
            for (int i = 0; i <= 13; i++)
                dr매출채권회전수.Cells[i].ToolTipText = "당월매출 / 미수금잔액";

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "매출채권회전일수";    //5
            dr매출채권회전일수 = dgvSales.Rows[n];
            for (int i = 0; i <= 13; i++)
                dr매출채권회전일수.Cells[i].ToolTipText = "30 / 매출채권회전수";

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "마진율(%)";    //6
            dr평균마진율 = dgvSales.Rows[n];

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "";                //7
            drEmpty1 = dgvSales.Rows[n];

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "손익분기잔액";    //8
            dr손익분기잔액1 = dgvSales.Rows[n];
            for (int i = 0; i <= 13; i++)
                dr손익분기잔액1.Cells[i].ToolTipText = "미수금 - 전월 누적마진금액";
            

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "   전월대비 미수절감액";    //9
            dr전월대비미수절감액 = dgvSales.Rows[n];

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "한달마진";    //10
            dr한달마진 = dgvSales.Rows[n];

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "누적마진";    //11
            dr누적마진 = dgvSales.Rows[n];

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "원금회수기간(미수금만)";    //12
            dr원금회수기간1 = dgvSales.Rows[n];
            for (int i = 0; i <= 13; i++)
                dr원금회수기간1.Cells[i].ToolTipText = "(미수금 - 전월 누적마진금액) / 당월 마진금액";

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "회전일수 / 마진율 (%)";    //13
            dr회전수나누기1 = dgvSales.Rows[n];
            for (int i = 0; i <= 13; i++)
                dr회전수나누기1.Cells[i].ToolTipText = "매출채권회전일수 30일, 마진율 약 4.25% 경우 7(마지노선)\n * 7이하 : 양호, 7초과 : 관리대상";

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "업체 등급";    //14
            dr업체등급1 = dgvSales.Rows[n];

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "보고유무";    //15
            dr보고유무1 = dgvSales.Rows[n];


            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "";    //16
            drEmpty2 = dgvSales.Rows[n];
            drEmpty2.Visible = false;

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "순영업자본 기회비용";    //17
            dr순영업자본 = dgvSales.Rows[n];
            dr순영업자본.Visible = false;

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "영업자본 기회비용";    //18
            dr영업자본 = dgvSales.Rows[n];
            dr영업자본.Visible = false;

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "판매가능 기회비용";    //19
            dr아토영업 = dgvSales.Rows[n];
            dr아토영업.Visible = false;

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "자기자본 기회비용";    //20
            dr자기자본 = dgvSales.Rows[n];
            dr자기자본.Visible = false;

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "기회비용 발생금액";    //21
            dr기회비용발생금액 = dgvSales.Rows[n];
            dr기회비용발생금액.Visible = false;

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "기회비용 누적금액";    //22
            dr기회비용누적금액 = dgvSales.Rows[n];
            dr기회비용누적금액.Visible = false;

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "손익분기잔액(+기회비용)";    //23
            dr손익분기잔액2 = dgvSales.Rows[n];
            dr손익분기잔액2.Visible = false;

            for (int i = 0; i <= 13; i++)
                dr손익분기잔액2.Cells[i].ToolTipText = "미수금 - 전월 누적마진금액 + 기회비용 누적금액";

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "원금회수기간(기회비용추가)";    //24
            dr원금회수기간2 = dgvSales.Rows[n];
            dr원금회수기간2.Visible = false;

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "업체 등급";    //25
            dr업체등급2 = dgvSales.Rows[n];
            dr업체등급2.Visible = false;

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "보고유무";    //26
            dr보고유무2 = dgvSales.Rows[n];
            dr보고유무2.Visible = false;

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "";               //27
            drEmpty3 = dgvSales.Rows[n];
            drEmpty3.Visible = false;

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "목표달성내용";   //28
            drEmpty4 = dgvSales.Rows[n];

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "1.줄여야할 미수잔액";   //29
            dr1 = dgvSales.Rows[n];
            dr1.Cells[1].Value = "목표개월수";

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "1-1.줄여야할 총 금액";  //30
            dr1_1 = dgvSales.Rows[n];
            for (int i = 0; i <= 13; i++)
                dr1_1.Cells[i].ToolTipText = "(원금회수기간 - 목표개월) * 한달마진";


            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "1-2.한달동안 수금액";   //31
            dr1_2 = dgvSales.Rows[n];
            for (int i = 0; i <= 13; i++)
                dr1_2.Cells[i].ToolTipText = "((원금회수기간 - 목표개월) * 한달마진) / 목표개월";

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "1-3.유지해야할 외상미수잔액";  //32
            dr1_3 = dgvSales.Rows[n];
            for (int i = 0; i <= 13; i++)
                dr1_3.Cells[i].ToolTipText = "미수금 - ((원금회수기간 - 목표개월) * 한달마진)";

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "2.미수잔액이 같을때 마진";     //33
            dr2 = dgvSales.Rows[n];
            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "2-1.높여야할 한달이익금액";    //34
            dr2_1 = dgvSales.Rows[n];
            for (int i = 0; i <= 13; i++)
                dr2_1.Cells[i].ToolTipText = "(미수금 - 전달 누적마진) / 목표개월";

            n = dgvSales.Rows.Add();
            dgvSales.Rows[n].Cells["division"].Value = "2-2.올려야할 한달 마진률";     //35 
            dr2_2 = dgvSales.Rows[n];
            for (int i = 0; i <= 13; i++)
                dr2_2.Cells[i].ToolTipText = "((미수금 - 전달 누적마진) / 목표개월) / 매출금액";
            //Style Setting
            SetDgvHeaderStyle();
            this.dgvSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSales_CellValueChanged);
        }

        private void SetDgvHeaderStyle()
        {
            DataGridView dgv = dgvSales;
            //dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(84, 130, 53);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Regular);

            dr당월매출.DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            dr당월매출.DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);
            dr미수금잔액.DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            dr미수금잔액.DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);
            //dr평균미수금.DefaultCellStyle.BackColor = Color.FromArgb(153, 204, 255);
            dr한달마진.DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);

            dr손익분기잔액1.DefaultCellStyle.BackColor = Color.FromArgb(233, 237, 247);
            dr손익분기잔액1.DefaultCellStyle.ForeColor = Color.Blue;
            dr손익분기잔액1.DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);
            dr원금회수기간1.DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);
            dr업체등급1.DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);

            dr손익분기잔액2.DefaultCellStyle.BackColor = Color.FromArgb(233, 237, 247);
            dr손익분기잔액2.DefaultCellStyle.ForeColor = Color.Blue;
            dr손익분기잔액2.DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);
            dr원금회수기간2.DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);
            dr업체등급2.DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);

            dr1.DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);
            dr2.DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);
            dr1.DefaultCellStyle.BackColor = Color.Beige;
            dr1.DefaultCellStyle.ForeColor = Color.Beige;
            dr1.Cells[0].Style.ForeColor = Color.Black;
            dr1.Cells[1].Style.ForeColor = Color.Black;

            dr2.DefaultCellStyle.BackColor = Color.Beige;
            dr2.DefaultCellStyle.ForeColor = Color.Beige;
            dr2.Cells[0].Style.ForeColor = Color.Black;

            dr순영업자본.Visible = false;
            dr영업자본.Visible = false;
            dr아토영업.Visible = false;
            dr자기자본.Visible = false;

            dr업체등급1.Visible = false;
            dr보고유무1.Visible = false;


            drEmpty1.DefaultCellStyle.BackColor = Color.Gray;
            drEmpty2.DefaultCellStyle.BackColor = Color.Gray;
            drEmpty3.DefaultCellStyle.BackColor = Color.Gray;
            drEmpty4.DefaultCellStyle.BackColor = Color.Gray;

            dr업체등급2.Visible = false;
            dr보고유무2.Visible = false;
        }
        #region 기존 Method
        private void GetCompanyInfo()
        {
            if (!string.IsNullOrEmpty(cbCompany.Text))
            {
                //Seaover 거래처 내역
                companyDt = seaoverRepository.SelectData("업체별월별매출현황", "*", "", $"거래처명='{cbCompany.Text}'", "사용자 DESC");
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
                        txtRemark.Text = atoCompanyDt.Rows[0]["recovery_remark"].ToString();
                        dr1_1.Cells[1].Value = atoCompanyDt.Rows[0]["target_recovery_month"].ToString();

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
                        dr1_1.Cells[1].Value = "18";
                        txtRemark.Text = String.Empty;
                    }
                }
                //데이터 불러오기
                GetData();
            }
        }
        #endregion

        #region 일별 평잔 계산해서 최근기준으로 보는 Method
        private void GetCompanyInfo2()
        {
            if (!string.IsNullOrEmpty(cbCompany.Text))
            {
                //Seaover 거래처 내역
                companyDt = seaoverRepository.SelectData("업체별월별매출현황", "*", "", $"거래처명='{cbCompany.Text}'", "사용자 DESC");
                if (companyDt.Rows.Count == 0)
                {
                    MessageBox.Show(this, "거래처 정보를 찾을 수 없습니다.");
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
                        txtRemark.Text = atoCompanyDt.Rows[0]["recovery_remark"].ToString();
                        dr1_1.Cells[1].Value = atoCompanyDt.Rows[0]["target_recovery_month"].ToString();

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
                        dr1_1.Cells[1].Value = "18";
                        txtRemark.Text = String.Empty;
                    }
                }
                //데이터 불러오기
                GetData();
            }
        }
        #endregion


        private void GetData()
        {
            this.dgvSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSales_CellValueChanged);
            int year;
            if (!int.TryParse(cbYear.Text, out year))
            {
                MessageBox.Show(this,"년도를 설정해주세요.");
                this.Activate();

                return;
            }
            int month;
            if (!int.TryParse(cbMonth.Text, out month))
            {
                MessageBox.Show(this,"월을 설정해주세요.");
                this.Activate();

                return;
            }
            //기준년월
            DateTime sYearMonth = new DateTime(year, month, 1);
            
            //날짜수정
            for (int i = 0; i < 12; i++)
            {
                DateTime tempDt = sYearMonth.AddMonths(-i);
                if (tempDt.Year == DateTime.Now.Year)
                    dgvSales.Columns[13 - i].HeaderText = tempDt.ToString("MM") + "월";
                else
                    dgvSales.Columns[13 - i].HeaderText = tempDt.ToString("yy") + "년 / " + tempDt.ToString("MM") + "월";
            }

            //목표개월수
            double target_recovery_months;
            if (dr1_1.Cells[1].Value == null || !double.TryParse(dr1_1.Cells[1].Value.ToString(), out target_recovery_months))
                target_recovery_months = 0;

            //거래처 정보가져오기
            string company = cbCompany.Text;
            //초기화
            DgvInitialize();
            dr1_1.Cells[1].Value = target_recovery_months.ToString();
            //매출금액, 미수잔액
            DateTime preYearMonth = sYearMonth.AddMonths(-11).AddDays(-1);
            DataTable cpDt = recoveryPrincipalRepository.GetRecoerPrincipal(sYearMonth.ToString("yyyyMM"), cbCompany.Text);
            if(cpDt != null && cpDt.Rows.Count > 0)
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

                double pre_sales_price = 0;
                double pre_balance_price = 0;
                double pre_margin_price = 0;
                double margin_rate = 0;
                double pre_months = 0;
                double pre_average_balance = 0;
                double principal_revoery_amount = 0;

                double pre_div = 0;

                double net_working_capital_opportunity_cost = 0;
                double working_capital_opportunity_cost = 0;
                double ato_capital_opportunity_cost = 0;
                double equity_capital_opportunity_cost = 0;
                double busniess_months = 0;
                for (int i = 0; i < cpDt.Rows.Count; i++) 
                {
                    DateTime tmpYearMonth = Convert.ToDateTime(cpDt.Rows[i]["기준년도"].ToString());
                    //거래기간
                    if (Convert.ToDouble(cpDt.Rows[i]["매출금액"].ToString()) > 0 || Convert.ToDouble(cpDt.Rows[i]["미수잔액"].ToString()) > 0)
                        busniess_months++;
                    //전년도
                    //if (sYearMonth.Year > tmpYearMonth.Year)
                    if (preYearMonth >= tmpYearMonth)
                    {
                        //전년도 매출금액
                        pre_sales_price += Convert.ToDouble(cpDt.Rows[i]["매출금액"].ToString());
                        //전년도 총 미수금액
                        pre_balance_price += Convert.ToDouble(cpDt.Rows[i]["미수잔액"].ToString());
                        //전년도 마진차액
                        principal_revoery_amount = Convert.ToDouble(cpDt.Rows[i]["미수잔액"].ToString()) - pre_margin_price;
                        //전년도 누적마진
                        pre_margin_price += Convert.ToDouble(cpDt.Rows[i]["손익금액"].ToString());
                        //전년도 월평균잔액, 평균을 내기위한 월수
                        pre_average_balance += Convert.ToDouble(cpDt.Rows[i]["월평균잔액"].ToString());
                        pre_months++;
                        //기회비용
                        net_working_capital_opportunity_cost += net_working_capital * Convert.ToDouble(cpDt.Rows[i]["월평균잔액"].ToString()) * sales_profit / 12;
                        working_capital_opportunity_cost += working_capital * Convert.ToDouble(cpDt.Rows[i]["월평균잔액"].ToString()) * sales_profit / 12;
                        ato_capital_opportunity_cost += ato_capital * Convert.ToDouble(cpDt.Rows[i]["월평균잔액"].ToString()) * 3;
                        equity_capital_opportunity_cost += equity_capital * Convert.ToDouble(cpDt.Rows[i]["월평균잔액"].ToString()) * sales_profit / 12;
                    }
                    //당해년도
                    else
                    {
                        int col_idx = 13 - ((sYearMonth.Year - tmpYearMonth.Year) * 12 + (sYearMonth.Month - tmpYearMonth.Month));
                        /*if(sYearMonth.Year == tmpYearMonth.Year)
                            dgvSales.Columns[col_idx].HeaderText = tmpYearMonth.ToString("MM") + "월";
                        else
                            dgvSales.Columns[col_idx].HeaderText = tmpYearMonth.ToString("yy") + "/" + tmpYearMonth.ToString("MM") + "월";*/
                        dr당월매출.Cells[col_idx].Value = Convert.ToDouble(cpDt.Rows[i]["매출금액"].ToString()).ToString("#,##0");
                        dr미수금잔액.Cells[col_idx].Value = Convert.ToDouble(cpDt.Rows[i]["미수잔액"].ToString()).ToString("#,##0");
                        dr평균미수금.Cells[col_idx].Value = Convert.ToDouble(cpDt.Rows[i]["월평균잔액"].ToString()).ToString("#,##0");
                        dr한달마진.Cells[col_idx].Value = Convert.ToDouble(cpDt.Rows[i]["손익금액"].ToString()).ToString("#,##0");
                        margin_rate = Convert.ToDouble(cpDt.Rows[i]["손익금액"].ToString()) / Convert.ToDouble(cpDt.Rows[i]["매출금액"].ToString()) * 100;
                        if (double.IsNaN(margin_rate))
                            margin_rate = 0;
                        dr평균마진율.Cells[col_idx].Value = margin_rate;
                    }
                }

                //거래기간
                txtBusinessTerms.Text = busniess_months.ToString("#,##0");

                //누적매출
                dr미수금잔액.Cells[1].Value = pre_balance_price.ToString("#,##0");

                //기회비용
                dr순영업자본.Cells[1].Value = net_working_capital_opportunity_cost.ToString("#,##0");
                dr영업자본.Cells[1].Value = working_capital_opportunity_cost.ToString("#,##0");
                dr아토영업.Cells[1].Value = ato_capital_opportunity_cost.ToString("#,##0");
                dr자기자본.Cells[1].Value = equity_capital_opportunity_cost.ToString("#,##0");

                //전년도 마진차액
                dr손익분기잔액1.Cells[1].Value = principal_revoery_amount.ToString("#,##0");
                //전년도 월평균잔액 평균
                double pre_balance = (pre_average_balance / pre_months);
                if (double.IsNaN(pre_balance))
                    pre_balance = 0;
                dr평균미수금.Cells[1].Value = pre_balance.ToString("#,##0");
                //전년도 누적매출
                dr누적매출.Cells[1].Value = pre_sales_price.ToString("#,##0");
                //전년도 누적매출
                dr누적마진.Cells[1].Value = pre_margin_price.ToString("#,##0");
                //전년도 마진율
                margin_rate = (pre_margin_price / pre_sales_price * 100);
                if(double.IsNaN(margin_rate))
                    margin_rate = 0;
                dr평균마진율.Cells[1].Value = margin_rate.ToString("#,##0.00");
                //누적순수마진
                if (rbNetWorkingCapital.Checked)
                    dr손익분기잔액2.Cells[1].Value = (principal_revoery_amount + net_working_capital_opportunity_cost).ToString("#,##0");
                else if (rbWorkingCapital.Checked)
                    dr손익분기잔액2.Cells[1].Value = (principal_revoery_amount + working_capital_opportunity_cost).ToString("#,##0");
                else if (rbAtoCapital.Checked)
                    dr손익분기잔액2.Cells[1].Value = (principal_revoery_amount + ato_capital_opportunity_cost).ToString("#,##0");
                else if (rbEquityCapital.Checked)
                    dr손익분기잔액2.Cells[1].Value = (principal_revoery_amount + equity_capital_opportunity_cost).ToString("#,##0");
            }
            //회수율 계산
            Calculate();
            this.dgvSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSales_CellValueChanged);
        }

        private void Calculate()
        {
            dgvSales.EndEdit();
            try
            {
                string grade_tooltip = "90이상 유지 (원금회수기간 6달 이하)"
                                        + "\n 80이상 유지(관리)  (원금회수기간 11달 이하)"
                                        + "\n 70이상 수시관리↑  (원금회수기간 16달 이하)"
                                        + "\n 60이상 수시관리(조심)  (원금회수기간 21달 이하)"
                                        + "\n 50이상 보고  (원금회수기간 26달 이하)"
                                        + "\n 50미만 보고필 (원금회수기간 26달 초과) ";
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
                //목표 회수개월
                double target_recovery_month;
                if (dr1_1.Cells[1].Value == null || !double.TryParse(dr1_1.Cells[1].Value.ToString(), out target_recovery_month))
                    target_recovery_month = 0;
                /*if (!double.TryParse(txtTargetMonth.Text, out target_recovery_month))
                    target_recovery_month = 0;*/

                //전년도 매출금액
                double pre_sales_price;
                if (dr누적매출.Cells[1].Value == null || !double.TryParse(dr누적매출.Cells[1].Value.ToString(), out pre_sales_price))
                    pre_sales_price = 0;
                //전년도 총 미수금액
                double pre_balance_price;
                if (dr미수금잔액.Cells[1].Value == null || !double.TryParse(dr미수금잔액.Cells[1].Value.ToString(), out pre_balance_price))
                    pre_balance_price = 0;
                //전년도 미수절감액
                double pre_increased_accumulate_margin = 0;
                if (dr손익분기잔액1.Cells[1].Value == null || !double.TryParse(dr손익분기잔액1.Cells[1].Value.ToString(), out pre_increased_accumulate_margin))
                    pre_increased_accumulate_margin = 0;

                double cumulative_sales = pre_sales_price;   //누적매출금액
                double cumulative_margin;                    //누적마진금액
                if (dr누적마진.Cells[1].Value == null || !double.TryParse(dr누적마진.Cells[1].Value.ToString(), out cumulative_margin))
                    cumulative_margin = 0;

                //전달기회비용
                double pre_net_working_capital_amount;
                if (dr순영업자본.Cells[1].Value == null || !double.TryParse(dr순영업자본.Cells[1].Value.ToString(), out pre_net_working_capital_amount))
                    pre_net_working_capital_amount = 0;

                double pre_working_capital_amount;
                if (dr영업자본.Cells[1].Value == null || !double.TryParse(dr영업자본.Cells[1].Value.ToString(), out pre_working_capital_amount))
                    pre_working_capital_amount = 0;

                double pre_ato_capital_amount;
                if (dr아토영업.Cells[1].Value == null || !double.TryParse(dr아토영업.Cells[1].Value.ToString(), out pre_ato_capital_amount))
                    pre_ato_capital_amount = 0;

                double pre_equity_capital_amount;
                if (dr자기자본.Cells[1].Value == null || !double.TryParse(dr자기자본.Cells[1].Value.ToString(), out pre_equity_capital_amount))
                    pre_equity_capital_amount = 0;

                //월별 순회 계산
                for (int i = 2; i < 14; i++)
                {
                    if (dr당월매출.Cells[i].Value != null && dr미수금잔액.Cells[i].Value != null)
                    {
                        //당월 매출금액
                        double sales_price;
                        if (dr당월매출.Cells[i].Value == null || !double.TryParse(dr당월매출.Cells[i].Value.ToString(), out sales_price))
                            sales_price = 0;

                        //미수금잔액
                        double balance_price;
                        if (dr미수금잔액.Cells[i].Value == null || !double.TryParse(dr미수금잔액.Cells[i].Value.ToString(), out balance_price))
                            balance_price = 0;
                        //누적 총 미수금액
                        pre_balance_price += balance_price;
                        //누적 매출금액
                        cumulative_sales += sales_price;
                        dr누적매출.Cells[i].Value = cumulative_sales.ToString("#,##0");
                        //누적마진
                        double target_margin;
                        if (dr누적마진.Cells[i].Value == null || !double.TryParse(dr누적마진.Cells[i].Value.ToString(), out target_margin))
                            target_margin = 0;

                        //매출채권회전수
                        double salse_month_count_current_month = sales_price / balance_price;
                        double salse_month_count = cumulative_sales / pre_balance_price;
                        dr매출채권회전수.Cells[i].Value = salse_month_count.ToString("#,##0.00") + " (" + salse_month_count_current_month.ToString("#,##0.00") + ")";

                        //매출채권회일수
                        int days = DateTime.DaysInMonth(Convert.ToInt16(cbYear.Text), i - 1);     //일수
                        double sales_day_count_current_month = 30 / salse_month_count_current_month;
                        double sales_day_count = 30 / salse_month_count;
                        dr매출채권회전일수.Cells[i].Value = sales_day_count.ToString("#,##0") + " (" + sales_day_count_current_month.ToString("#,##0") + ")";

                        //전월 누적매출
                        double pre_month_price = Convert.ToDouble(dr누적매출.Cells[i - 1].Value);
                        //전월 누적마진
                        double pre_margin_price = Convert.ToDouble(dr누적마진.Cells[i - 1].Value);


                        //한달마진
                        double month_margin;
                        if (dr한달마진.Cells[i].Value == null || !double.TryParse(dr한달마진.Cells[i].Value.ToString(), out month_margin))
                            month_margin = 0;

                        //미수금잔액 - 전달 누적마진
                        double increased_accumulate_margin = balance_price - cumulative_margin;
                        dr손익분기잔액1.Cells[i].Value = increased_accumulate_margin.ToString("#,##0");
                        //누적마진
                        cumulative_margin += month_margin;
                        dr누적마진.Cells[i].Value = cumulative_margin.ToString("#,##0");
                        //평균마진율
                        double avg = month_margin / sales_price * 100;
                        if (double.IsNaN(avg))
                            avg = 0;
                        string str_avg = avg.ToString("#,##0.00");

                        dr평균마진율.Cells[i].Value = str_avg;
                        if (dr평균마진율.Cells[i].Value == null)
                            dr평균마진율.Cells[i].Value = 0; 
                        double average_margin = Convert.ToDouble(dr평균마진율.Cells[i].Value.ToString());
                        //dr평균마진율.Cells[i].Value = average_margin.ToString("#,##0.00");
                        //손익금액1 절감액
                        dr전월대비미수절감액.Cells[i].Value = (pre_increased_accumulate_margin - increased_accumulate_margin).ToString("#,##0");
                        //전월 누적마진
                        pre_increased_accumulate_margin = increased_accumulate_margin;

                        //원금회수기간1
                        double recovery_month;
                        if (increased_accumulate_margin < 0)
                            recovery_month = 0;
                        else if (month_margin < 0)
                            recovery_month = increased_accumulate_margin / 0;
                        else
                            recovery_month = increased_accumulate_margin / month_margin;
                        if (double.IsNaN(recovery_month))
                            recovery_month = 0;

                        if (double.IsNaN(recovery_month))  //Nan
                            recovery_month = 0;
                        else if (double.IsInfinity(recovery_month))  //Infinity
                            recovery_month = 9999;
                        else if (recovery_month > 9999)  //Max
                            recovery_month = 9999;

                        dr원금회수기간1.Cells[i].Value = recovery_month.ToString("#,##0.0");

                        //회전일수 / 마진율
                        if (average_margin < 0)
                            average_margin = 0;
                        double rate_current_month = Math.Round(sales_day_count_current_month) / average_margin;
                        double total_average_margin_rate = cumulative_margin / cumulative_sales * 100;
                        double rate = Math.Round(sales_day_count) / Math.Round(total_average_margin_rate, 2);

                        dr평균마진율.Cells[0].Value = "마진율(%)      *평균마진 : " + total_average_margin_rate.ToString("#,##0.0") + "%";

                        dr회전수나누기1.Cells[i].Value = rate.ToString("#,##0.00") + " (" + rate_current_month.ToString("#,##0.00") + ")";
                        //dr회전수나누기1.Cells[i].Value = rate_current_month.ToString("#,##0.00");
                        if (rate_current_month >= 8)
                            dr회전수나누기1.Cells[i].Style.ForeColor = Color.Red;
                        else if (rate_current_month >= 7)
                            dr회전수나누기1.Cells[i].Style.ForeColor = Color.Orange;
                        else
                            dr회전수나누기1.Cells[i].Style.ForeColor = Color.Black;

                        //업체등급1
                        int grade;
                        string grade_remark;
                        GetGrade(recovery_month, out grade, out grade_remark);


                        

                        if (double.IsNaN(rate_current_month))
                        { 
                            
                        }
                        else
                        {
                            double div_grade = 100 - (40 / 7 * rate_current_month);
                            if (rate_current_month < 0)
                                div_grade = 0;

                            if (div_grade < 0)
                                div_grade = 0;
                            //Remark
                            if (div_grade >= 90)
                            {
                                //grade_eng.Value = "A+";
                                grade_remark = "유지";
                            }
                            else if (div_grade >= 80)
                            {
                                //grade_eng.Value = "A";
                                grade_remark = "유지(관리)";
                            }
                            else if (div_grade >= 70)
                            {
                                //grade_eng.Value = "A-";
                                grade_remark = "수시관리↑";
                            }
                            else if (div_grade >= 60)
                            {
                                //grade_eng.Value = "B";
                                grade_remark = "수시관리(조심)";
                            }
                            else if (div_grade >= 50)
                            {
                                //grade_eng.Value = "C";
                                grade_remark = "보고";
                            }
                            else if (div_grade >= 40)
                            {
                                //grade_eng.Value = "C-";
                                grade_remark = "보고필";
                            }
                            else
                            {
                                //grade_eng.Value = "F";
                                grade_remark = "보고필";
                            }



                            dr업체등급1.Cells[i].Value = Math.Ceiling(div_grade);
                            dr보고유무1.Cells[i].Value = grade_remark;
                            if (div_grade >= 80)
                            {
                                dr업체등급1.Cells[i].Style.ForeColor = Color.Blue;
                                dr보고유무1.Cells[i].Style.ForeColor = Color.Blue;
                            }
                            else if (div_grade < 50)
                            {
                                dr업체등급1.Cells[i].Style.ForeColor = Color.Red;
                                dr보고유무1.Cells[i].Style.ForeColor = Color.Red;
                            }
                            else
                            {
                                dr업체등급1.Cells[i].Style.ForeColor = Color.Black;
                                dr보고유무1.Cells[i].Style.ForeColor = Color.Black;
                            }    
                        }
                        


                        //월 평균잔액
                        double average_balance_price;
                        if (dr평균미수금.Cells[i].Value == null || !double.TryParse(dr평균미수금.Cells[i].Value.ToString(), out average_balance_price))
                            average_balance_price = 0;

                        //기회비용
                        double net_working_capital_opportunity_cost = net_working_capital * average_balance_price * sales_profit / 12;
                        dr순영업자본.Cells[i].Value = net_working_capital_opportunity_cost.ToString("#,##0");
                        pre_net_working_capital_amount += net_working_capital_opportunity_cost;

                        double working_capital_opportunity_cost = working_capital * average_balance_price * sales_profit / 12;
                        dr영업자본.Cells[i].Value = working_capital_opportunity_cost.ToString("#,##0");
                        pre_working_capital_amount += working_capital_opportunity_cost;

                        double ato_capital_opportunity_cost = ato_capital * average_balance_price * 3;
                        dr아토영업.Cells[i].Value = ato_capital_opportunity_cost.ToString("#,##0");
                        pre_ato_capital_amount += ato_capital_opportunity_cost;

                        double equity_capital_opportunity_cost = equity_capital * average_balance_price * sales_profit / 12;
                        dr자기자본.Cells[i].Value = equity_capital_opportunity_cost.ToString("#,##0");
                        pre_equity_capital_amount += equity_capital_opportunity_cost;



                        //누적 순수마진
                        double real_margin = 0;
                        double month_capital_amount = 0;
                        if (rbNetWorkingCapital.Checked)
                        {
                            month_capital_amount = net_working_capital_opportunity_cost;
                            real_margin = increased_accumulate_margin + pre_net_working_capital_amount;
                            dr기회비용발생금액.Cells[i].Value = net_working_capital_opportunity_cost.ToString("#,##0");
                            dr기회비용누적금액.Cells[i].Value = pre_net_working_capital_amount.ToString("#,##0");

                            dr기회비용발생금액.Cells[i].ToolTipText = "(순영업자본 회전율 x 평균미수금 x 작년 영업이익) / 12개월";
                            dr기회비용누적금액.Cells[i].ToolTipText = "(순영업자본 회전율 x 평균미수금 x 작년 영업이익) / 12개월";
                        }
                        else if (rbWorkingCapital.Checked)
                        {
                            month_capital_amount = working_capital_opportunity_cost;
                            real_margin = increased_accumulate_margin + pre_working_capital_amount;
                            dr기회비용발생금액.Cells[i].Value = working_capital_opportunity_cost.ToString("#,##0");
                            dr기회비용누적금액.Cells[i].Value = pre_working_capital_amount.ToString("#,##0");

                            dr기회비용발생금액.Cells[i].ToolTipText = "(영업자본 회전율 x 평균미수금 x 작년 영업이익) / 12개월";
                            dr기회비용누적금액.Cells[i].ToolTipText = "(영업자본 회전율 x 평균미수금 x 작년 영업이익) / 12개월";
                        }
                        else if (rbAtoCapital.Checked)
                        {
                            month_capital_amount = ato_capital_opportunity_cost;
                            real_margin = increased_accumulate_margin + pre_ato_capital_amount;
                            dr기회비용발생금액.Cells[i].Value = ato_capital_opportunity_cost.ToString("#,##0");
                            dr기회비용누적금액.Cells[i].Value = pre_ato_capital_amount.ToString("#,##0");

                            dr기회비용발생금액.Cells[i].ToolTipText = "(평균미수금 x 판매가능기준) * 3";
                            dr기회비용누적금액.Cells[i].ToolTipText = "(평균미수금 x 판매가능기준) * 3";
                        }
                        else
                        {
                            month_capital_amount = equity_capital_opportunity_cost;
                            real_margin = increased_accumulate_margin + pre_equity_capital_amount;
                            dr기회비용발생금액.Cells[i].Value = equity_capital_opportunity_cost.ToString("#,##0");
                            dr기회비용누적금액.Cells[i].Value = pre_equity_capital_amount.ToString("#,##0");

                            dr기회비용발생금액.Cells[i].ToolTipText = "(자기자본 회전율 x 평균미수금 x 작년 영업이익) / 12개월";
                            dr기회비용누적금액.Cells[i].ToolTipText = "(자기자본 회전율 x 평균미수금 x 작년 영업이익) / 12개월";
                        }
                        dr손익분기잔액2.Cells[i].Value = real_margin.ToString("#,##0");

                        //원금회수기간2
                        double recovery_month2;
                        if (month_margin < 0)
                            recovery_month2 = real_margin / 0;
                        else
                        {
                            double real_month_margin = month_margin - month_capital_amount;

                            if (real_month_margin <= 0)
                                recovery_month2 = real_margin / 0;
                            else
                                recovery_month2 = real_margin / real_month_margin;  //손익분기잔액1 / (한달마진 - 당월발생 기회비용)
                        }

                        if (double.IsNaN(recovery_month2))  //Nan
                            recovery_month2 = 0;
                        else if (double.IsInfinity(recovery_month2))  //Infinity
                            recovery_month2 = 9999;
                        else if (recovery_month2 > 9999)  //Max
                            recovery_month2 = 9999;

                        dr원금회수기간2.Cells[i].Value = recovery_month2.ToString("#,##0.0");

                        //업체등급2
                        int grade2;
                        string grade_remark2;
                        GetGrade(recovery_month2, out grade2, out grade_remark2);
                        dr업체등급2.Cells[i].Value = grade2;
                        dr보고유무2.Cells[i].Value = grade_remark2;
                        if (grade2 >= 80)
                        {
                            dr업체등급2.Cells[i].Style.ForeColor = Color.Blue;
                            dr보고유무2.Cells[i].Style.ForeColor = Color.Blue;
                        }
                        else if (grade2 < 50)
                        {
                            dr업체등급2.Cells[i].Style.ForeColor = Color.Red;
                            dr보고유무2.Cells[i].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            dr업체등급2.Cells[i].Style.ForeColor = Color.Black;
                            dr보고유무2.Cells[i].Style.ForeColor = Color.Black;
                        }

                        //ToolTip
                        dr업체등급1.Cells[i].ToolTipText = grade_tooltip;
                        
                        dr업체등급2.Cells[i].ToolTipText = grade_tooltip;


                        //목표개월까지 줄여야하는 총 금액
                        double target_minus_total_price;
                        if (cb기회비용.Checked)
                            target_minus_total_price = (recovery_month2 - target_recovery_month) * month_margin;
                        else
                            target_minus_total_price = (recovery_month - target_recovery_month) * month_margin;
                        if (double.IsNaN(target_minus_total_price))
                            target_minus_total_price = 0;

                        dr1_1.Cells[i].Value = target_minus_total_price.ToString("#,##0");
                        dr1_2.Cells[i].Value = (target_minus_total_price / target_recovery_month).ToString("#,##0");
                        dr1_3.Cells[i].Value = (balance_price - target_minus_total_price).ToString("#,##0");

                        if (cb기회비용.Checked)
                            dr2_1.Cells[i].Value = (real_margin / target_recovery_month).ToString("#,##0");
                        else
                            dr2_1.Cells[i].Value = (increased_accumulate_margin / target_recovery_month).ToString("#,##0");

                        dr2_2.Cells[i].Value = ((Convert.ToDouble(dr2_1.Cells[i].Value) / sales_price) * 100).ToString("#,##0.0") + "%";

                        if (Convert.ToDouble(dr1_1.Cells[i].Value) < 0)
                            dr1_1.Cells[i].Style.ForeColor = Color.Red;
                        else
                            dr1_1.Cells[i].Style.ForeColor = Color.Black;

                        if (Convert.ToDouble(dr1_2.Cells[i].Value) < 0)
                            dr1_2.Cells[i].Style.ForeColor = Color.Red;
                        else
                            dr1_2.Cells[i].Style.ForeColor = Color.Black;

                        if (Convert.ToDouble(dr1_3.Cells[i].Value) < 0)
                            dr1_3.Cells[i].Style.ForeColor = Color.Red;
                        else
                            dr1_3.Cells[i].Style.ForeColor = Color.Black;

                        if (Convert.ToDouble(dr2_1.Cells[i].Value) < 0)
                            dr2_1.Cells[i].Style.ForeColor = Color.Red;
                        else
                            dr2_1.Cells[i].Style.ForeColor = Color.Black;

                        if (Convert.ToDouble(dr2_2.Cells[i].Value.ToString().Replace("%", "")) < 0)
                            dr2_2.Cells[i].Style.ForeColor = Color.Red;
                        else
                            dr2_2.Cells[i].Style.ForeColor = Color.Black;
                    }
                }
            }
            catch 
            { }
        }
        private void CapitalCalculate()
        {
            this.dgvSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSales_CellValueChanged);
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
            //목표 회수개월
            double target_recovery_month;
            if (dr1_1.Cells[1].Value == null || !double.TryParse(dr1_1.Cells[1].Value.ToString(), out target_recovery_month))
                target_recovery_month = 0;
            /*if (!double.TryParse(txtTargetMonth.Text, out target_recovery_month))
                target_recovery_month = 0;*/

            //전년도 매출금액
            double pre_sales_price;
            if (dr누적매출.Cells[1].Value == null || !double.TryParse(dr누적매출.Cells[1].Value.ToString(), out pre_sales_price))
                pre_sales_price = 0;

            //전년도 미수절감액
            double pre_increased_accumulate_margin = 0;
            if (dr손익분기잔액1.Cells[1].Value == null || !double.TryParse(dr손익분기잔액1.Cells[1].Value.ToString(), out pre_increased_accumulate_margin))
                pre_increased_accumulate_margin = 0;


            double cumulative_sales = pre_sales_price;                                         //누적매출금액
            double cumulative_margin;                                                          //누적마진금액
            if (dr누적마진.Cells[1].Value == null || !double.TryParse(dr누적마진.Cells[1].Value.ToString(), out cumulative_margin))
                cumulative_margin = 0;

            //전달기회비용
            double pre_net_working_capital_amount;
            if (dr순영업자본.Cells[1].Value == null || !double.TryParse(dr순영업자본.Cells[1].Value.ToString(), out pre_net_working_capital_amount))
                pre_net_working_capital_amount = 0;

            double pre_working_capital_amount;
            if (dr영업자본.Cells[1].Value == null || !double.TryParse(dr영업자본.Cells[1].Value.ToString(), out pre_working_capital_amount))
                pre_working_capital_amount = 0;

            double pre_ato_capital_amount;
            if (dr아토영업.Cells[1].Value == null || !double.TryParse(dr아토영업.Cells[1].Value.ToString(), out pre_ato_capital_amount))
                pre_ato_capital_amount = 0;

            double pre_equity_capital_amount;
            if (dr자기자본.Cells[1].Value == null || !double.TryParse(dr자기자본.Cells[1].Value.ToString(), out pre_equity_capital_amount))
                pre_equity_capital_amount = 0;

            //월별 순회 계산
            for (int i = 2; i < 14; i++)
            {
                if (dr당월매출.Cells[i].Value != null && dr미수금잔액.Cells[i].Value != null)
                {

                    //당월 매출금액
                    double sales_price;
                    if (dr당월매출.Cells[i].Value == null || !double.TryParse(dr당월매출.Cells[i].Value.ToString(), out sales_price))
                        sales_price = 0;

                    //미수금잔액
                    double balance_price;
                    if (dr미수금잔액.Cells[i].Value == null || !double.TryParse(dr미수금잔액.Cells[i].Value.ToString(), out balance_price))
                        balance_price = 0;

                    //누적마진
                    double target_margin;
                    if (dr누적마진.Cells[i].Value == null || !double.TryParse(dr누적마진.Cells[i].Value.ToString(), out target_margin))
                        target_margin = 0;

                    //누적 매출금액
                    cumulative_sales += sales_price;
                    dr누적매출.Cells[i].Value = cumulative_sales.ToString("#,##0");

                    //전월 누적매출
                    double pre_month_price = Convert.ToDouble(dr누적매출.Cells[i - 1].Value);
                    //전월 누적마진
                    double pre_margin_price = Convert.ToDouble(dr누적마진.Cells[i - 1].Value);

                    //한달마진
                    double month_margin;
                    if (dr한달마진.Cells[i].Value == null || !double.TryParse(dr한달마진.Cells[i].Value.ToString(), out month_margin))
                        month_margin = 0;

                    //미수금잔액 - 전달 누적마진
                    double increased_accumulate_margin = balance_price - cumulative_margin;
                    dr손익분기잔액1.Cells[i].Value = increased_accumulate_margin.ToString("#,##0");
                    dr전월대비미수절감액.Cells[i].Value = (pre_increased_accumulate_margin - increased_accumulate_margin).ToString("#,##0");

                    //dr전월대비미수절감율.Cells[i].Value = ((increased_accumulate_margin - pre_increased_accumulate_margin) / pre_increased_accumulate_margin * 100).ToString("#,##0.00");

                    pre_increased_accumulate_margin = increased_accumulate_margin;

                    //누적마진
                    cumulative_margin += month_margin;
                    dr누적마진.Cells[i].Value = cumulative_margin.ToString("#,##0");

                    //평균마진율
                    /*double average_margin = cumulative_margin / cumulative_sales * 100;
                    dr평균마진율.Cells[i].Value = average_margin.ToString("#,##0.00");*/

                    //월 평균잔액
                    double average_balance_price;
                    if (dr평균미수금.Cells[i].Value == null || !double.TryParse(dr평균미수금.Cells[i].Value.ToString(), out average_balance_price))
                        average_balance_price = 0;

                    //기회비용
                    double net_working_capital_opportunity_cost;
                    if (dr순영업자본.Cells[i].Value == null || !double.TryParse(dr순영업자본.Cells[i].Value.ToString(), out net_working_capital_opportunity_cost))
                        net_working_capital_opportunity_cost = 0;
                    dr순영업자본.Cells[i].Value = net_working_capital_opportunity_cost.ToString("#,##0");
                    pre_net_working_capital_amount += net_working_capital_opportunity_cost;

                    double working_capital_opportunity_cost;
                    if (dr영업자본.Cells[i].Value == null || !double.TryParse(dr영업자본.Cells[i].Value.ToString(), out working_capital_opportunity_cost))
                        working_capital_opportunity_cost = 0;
                    dr영업자본.Cells[i].Value = working_capital_opportunity_cost.ToString("#,##0");
                    pre_working_capital_amount += working_capital_opportunity_cost;

                    double ato_capital_opportunity_cost;
                    if (dr아토영업.Cells[i].Value == null || !double.TryParse(dr아토영업.Cells[i].Value.ToString(), out ato_capital_opportunity_cost))
                        ato_capital_opportunity_cost = 0;
                    dr아토영업.Cells[i].Value = ato_capital_opportunity_cost.ToString("#,##0");
                    pre_ato_capital_amount += ato_capital_opportunity_cost;

                    double equity_capital_opportunity_cost;
                    if (dr자기자본.Cells[i].Value == null || !double.TryParse(dr자기자본.Cells[i].Value.ToString(), out equity_capital_opportunity_cost))
                        equity_capital_opportunity_cost = 0;
                    dr자기자본.Cells[i].Value = equity_capital_opportunity_cost.ToString("#,##0");
                    pre_equity_capital_amount += equity_capital_opportunity_cost;

                    //매출채권회전수
                    double salse_month_count = sales_price / balance_price;
                    dr매출채권회전수.Cells[i].Value = salse_month_count.ToString("#,##0.00");

                    //매출채권회일수
                    int days = DateTime.DaysInMonth(Convert.ToInt16(cbYear.Text), i - 1);     //일수
                    double sales_day_count = 30 / salse_month_count;
                    dr매출채권회전일수.Cells[i].Value = sales_day_count.ToString("#,##0.0");

                    //누적 순수마진
                    double real_margin = 0;
                    if (rbNetWorkingCapital.Checked)
                        real_margin = increased_accumulate_margin + pre_net_working_capital_amount;
                    else if (rbWorkingCapital.Checked)
                        real_margin = increased_accumulate_margin + pre_working_capital_amount;
                    else if (rbAtoCapital.Checked)
                        real_margin = increased_accumulate_margin + pre_ato_capital_amount;
                    else
                        real_margin = increased_accumulate_margin + pre_equity_capital_amount;

                    dr손익분기잔액2.Cells[i].Value = real_margin.ToString("#,##0");

                    //원금회수기간
                    double recovery_month;
                    if (month_margin < 0)
                        recovery_month = real_margin / 0;
                    else
                        recovery_month = real_margin / month_margin;
                    if (double.IsNaN(recovery_month))
                        recovery_month = 0;
                    dr원금회수기간2.Cells[i].Value = recovery_month.ToString("#,##0.0");

                    //업체등급
                    int grade;
                    string grade_remark;
                    GetGrade(recovery_month, out grade, out grade_remark);
                    dr업체등급2.Cells[i].Value = grade;
                    dr보고유무2.Cells[i].Value = grade_remark;

                    //목표개월까지 줄여야하는 총 금액
                    double target_minus_total_price = (recovery_month - target_recovery_month) * month_margin;
                    dr1_1.Cells[i].Value = target_minus_total_price.ToString("#,##0");
                    dr1_2.Cells[i].Value = (target_minus_total_price / target_recovery_month).ToString("#,##0");
                    dr1_3.Cells[i].Value = (balance_price - target_minus_total_price).ToString("#,##0");

                    dr2_1.Cells[i].Value = (increased_accumulate_margin / target_recovery_month).ToString("#,##0");
                    dr2_2.Cells[i].Value = (((increased_accumulate_margin / target_recovery_month) / sales_price) * 100).ToString("#,##0.0");
                }
            }
            this.dgvSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSales_CellValueChanged);
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

            dgvSales.EndEdit();

            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = new StringBuilder();

            if (string.IsNullOrEmpty(cbCompany.Text))
            {
                MessageBox.Show(this, "거래처를 선택해주세요.");
                this.Activate();
            }


            //Seaover 거래처 내역
            companyDt = seaoverRepository.SelectData("업체별월별매출현황", "*", "", $"거래처명='{cbCompany.Text}'", "사용자 DESC");
            if (companyDt.Rows.Count == 0)
            {
                MessageBox.Show(this,"거래처 정보를 찾을 수 없습니다.");
                this.Activate();
                return;
            }

            //Msg
            if (MessageBox.Show(this, "등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;


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
                cm.createtime= DateTime.Now.ToString("yyyy-MM-dd");
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
                cm.isManagement1 = cbIsManagement.Checked;
                cm.isManagement2 = cbIsManagement2.Checked;
                cm.isManagement3 = cbIsManagement3.Checked;
                cm.isManagement4 = cbIsManagement4.Checked;
                cm.isTrading = true;
                cm.isHide = Convert.ToBoolean(atoCompanyDt.Rows[0]["isHide"].ToString());
                //cm.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                cm.createtime = atoCompanyDt.Rows[0]["createtime"].ToString();
                cm.edit_user = um.user_name;
                cm.seaover_company_code = companyDt.Rows[0]["거래처코드"].ToString();
                cm.id = Convert.ToInt32(atoCompanyDt.Rows[0]["id"].ToString());
                //Update
                sql = companyRepository.UpdateCompany(cm);
                sqlList.Add(sql);
            }

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
            if (dr1_1.Cells[1].Value == null || !int.TryParse(dr1_1.Cells[1].Value.ToString(), out target_recovery_month))
                target_recovery_month = 0;
            rm.target_recovery_month = target_recovery_month;

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


        #endregion

        #region Datagridview event
        private void dgvSales_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 1)
            {
                this.dgvSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSales_CellValueChanged);
                //숫자표현
                double d;
                if (dgvSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !double.TryParse(dgvSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out d))
                    d = 0;
                dgvSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = d.ToString("#,##0");
                //기회비용
                if (e.RowIndex == dr순영업자본.Index || e.RowIndex == dr영업자본.Index || e.RowIndex == dr아토영업.Index || e.RowIndex == dr자기자본.Index)
                {
                    CapitalCalculate();
                }
                //마진율
                else if (e.RowIndex == 1 && e.ColumnIndex == 1)
                {
                    double margin;
                    if (dr누적마진.Cells[1].Value == null || !double.TryParse(dr누적마진.Cells[1].Value.ToString(), out margin))
                        margin = 0;
                    dr평균마진율.Cells[1].Value = d * margin / 100;
                    Calculate();
                }
                //나머지
                else
                {
                    Calculate();
                }
                this.dgvSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSales_CellValueChanged);
            }
        }
        #endregion

        #region CheckBox, Radion, Button
        private void cb기회비용_CheckedChanged(object sender, EventArgs e)
        {
            Calculate();
        }
        private void rbNetWorkingCapital_CheckedChanged(object sender, EventArgs e)
        {
            GetData();
        }
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            Calculate();
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            InsertRecoveryPrincipal();
        }
        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetCompanyInfo();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbCompany.Text))
                GetCompanyInfo();
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

        #region Key event
        private void txtNetWorkingCapital_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox tb = (TextBox)sender;
                tb.Text = common.GetN2(tb.Text);

                GetData();
            }
                
        }
        private void cbYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == (Char)46 || e.KeyChar == (Char)45 || e.KeyChar == (Char)65294))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }
        private void cbManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataRow[] tmpRow = managerDt.Select($"user_name LIKE '%{cbManager.Text}%'");
                if (tmpRow.Length > 0)
                {
                    cbManager.Text = tmpRow[0]["user_name"].ToString();
                }
            }
        }
        private void RecoveryPrincipalManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { 
                switch(e.KeyCode)
                {
                    case Keys.Q:
                        GetCompanyInfo();
                        break;
                    case Keys.W:
                        Calculate();
                        break;
                    case Keys.A:
                        InsertRecoveryPrincipal();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
        }

        private void txtTargetMargin_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == (Char)45 || e.KeyChar == (Char)46 || e.KeyChar == (Char)47))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
            //이전값이 0일경우 삭제 후 입력
            Control con = (Control)sender;
        }
        private void txtTargetMargin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Calculate();
            }
        }

        private void cbCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //거래처
                if (!cbCompany.DroppedDown)
                { 
                    DataTable companyDt = recoveryPrincipalPriceRepository.GetCompanyList(cbCompany.Text);
                    if (companyDt.Rows.Count > 0)
                    {
                        for (int i = 0; i < companyDt.Rows.Count; i++)
                        {
                            cbCompany.Items.Add(companyDt.Rows[i]["거래처명"].ToString());
                        }
                    }
                    cbCompany.DroppedDown = true;
                }
            }
        }

        #endregion

    }
}


