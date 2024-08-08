using AdoNetWindow.Model;
using Microsoft.Office.Interop.Excel;
using Repositories;
using Repositories.Company;
using Repositories.Config;
using Repositories.RecoveryPrincipal;
using Repositories.SEAOVER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Fabric.Query;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;

namespace AdoNetWindow.RecoveryPrincipal.RecoveryPrincipalmanager2
{
    public partial class RecoveryPrincipalManager : Form
    {
        ICompanyRecoveryRepository companyRecoveryRepository = new CompanyRecoveryRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        ICommonRepository commonRepository = new CommonRepository();
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        ISeaoverCompanyRepository seaoverCompanyRepository = new SeaoverCompanyRepository();
        ICompanyRepository companyRepository = new CompanyRepository();
        IRecoveryPrincipalPriceRepository recoveryPrincipalPriceRepository = new RecoveryPrincipalPriceRepository();
        DataTable recoveryDt = null;
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();

        DataGridViewRow drCurrentSalesAmount = new DataGridViewRow();
        DataGridViewRow drTotalSalesAmount = new DataGridViewRow();

        DataGridViewRow drDiv0 = new DataGridViewRow();

        DataGridViewRow drCurrentMarginAmount = new DataGridViewRow();
        DataGridViewRow drMarginRate = new DataGridViewRow();
        DataGridViewRow drTotalMarginAmount = new DataGridViewRow();
        DataGridViewRow drTotalMarginRate = new DataGridViewRow();

        DataGridViewRow drDiv1 = new DataGridViewRow();
        DataGridViewRow drCurrentBalanceAmount = new DataGridViewRow();
        DataGridViewRow drCurrentAccountReceivableRound = new DataGridViewRow();
        DataGridViewRow drCurrentAccountReceivableRoundDays = new DataGridViewRow();
        DataGridViewRow drCurrentRoundDaysDivisionMargin = new DataGridViewRow();

        DataGridViewRow drDiv2 = new DataGridViewRow();
        DataGridViewRow drAverageBalanceAmount = new DataGridViewRow();
        DataGridViewRow drAverageAccountReceivableRound = new DataGridViewRow();
        DataGridViewRow drAverageAccountReceivableRoundDays = new DataGridViewRow();
        DataGridViewRow drAverageRoundDaysDivisionMargin = new DataGridViewRow();

        DataGridViewRow drDiv3 = new DataGridViewRow();
        DataGridViewRow drMaximumBalanceAmount = new DataGridViewRow();
        DataGridViewRow drMaximumAccountReceivableRound = new DataGridViewRow();
        DataGridViewRow drMaximumAccountReceivableRoundDays = new DataGridViewRow();
        DataGridViewRow drMaximumRoundDaysDivisionMargin = new DataGridViewRow();


        DataGridViewRow drDiv4 = new DataGridViewRow();
        DataGridViewRow drRecoveryAmount = new DataGridViewRow();
        DataGridViewRow drRecoveryMonth = new DataGridViewRow();


        DataGridViewRow drDiv5 = new DataGridViewRow();
        DataGridViewRow drTarget1 = new DataGridViewRow();
        DataGridViewRow drTarget1_1 = new DataGridViewRow();
        DataGridViewRow drTarget1_2 = new DataGridViewRow();
        DataGridViewRow drTarget1_3 = new DataGridViewRow();
        DataGridViewRow drTarget2 = new DataGridViewRow();
        DataGridViewRow drTarget2_1 = new DataGridViewRow();
        DataGridViewRow drTarget2_2 = new DataGridViewRow();

        UsersModel um;

        private System.Windows.Threading.DispatcherTimer timer;
        bool processingFlag = false;

        int target_month = 18;
        public RecoveryPrincipalManager(UsersModel um, string company_code, string company)
        {
            InitializeComponent();
            this.um = um;
            txtStandardDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            lbCompanyCode.Text = company_code;
            txtCompany.Text = company;

            DataTable companyInfoDt = companyRepository.GetCompanyRecovery("", "", false, "", company_code);
            if (companyInfoDt.Rows.Count > 0)
            {
                bool isManagement1 = false, isManagement2 = false, isManagement3 = false, isManagement4 = false;
                foreach(DataRow dr in companyInfoDt.Rows) 
                {
                    if (bool.TryParse(dr["isManagement1"].ToString(), out bool isTemp1) && isTemp1)
                        isManagement1 = isTemp1;

                    if (bool.TryParse(dr["isManagement2"].ToString(), out bool isTemp2) && isTemp2)
                        isManagement2 = isTemp2;

                    if (bool.TryParse(dr["isManagement3"].ToString(), out bool isTemp3) && isTemp3)
                        isManagement3 = isTemp3;

                    if (bool.TryParse(dr["isManagement4"].ToString(), out bool isTemp4) && isTemp4)
                        isManagement4 = isTemp4;
                    if (!int.TryParse(dr["target_recovery_month"].ToString(), out target_month))
                        target_month = 0;
                    if (target_month <= 0)
                        target_month = 18;
                }
                cbIsManagement.Checked = isManagement1;
                cbIsManagement2.Checked = isManagement2;
                cbIsManagement3.Checked = isManagement3;
                cbIsManagement4.Checked = isManagement4;
            }
        }

        private void RecoveryPrincipalManager_Load(object sender, EventArgs e)
        {
            // BackgroundWorker 이벤트 핸들러 등록
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            timer_start();
            //거래개월수
            txtBusinessTerms.Text = seaoverCompanyRepository.GetBusinessMonths(lbCompanyCode.Text).ToString("#,##0");
        }

        #region Key event
        private void RecoveryPrincipalManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { 
                switch(e.KeyCode) 
                {
                    case Keys.Q:
                        btnSearching.PerformClick();
                        break;
                    case Keys.A:
                        btnInsert.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
        }
        private void txtStandardDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                System.Windows.Forms.TextBox tbb = (System.Windows.Forms.TextBox)sender;
                tbb.Text = common.strDatetime(tbb.Text);
                if (tbb.Name == "txtStandardDate")
                {
                    DateTime dt;
                    if (DateTime.TryParse(tbb.Text, out dt))
                        tbb.Text = dt.ToString("yyyy-MM-dd");
                }
            }
        }
        #endregion

        #region Button, Checkbox
        private void btnStandardDate_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtStandardDate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }
        private void btnManagment_Click(object sender, EventArgs e)
        {
            cbIsManagement.Checked = !cbIsManagement.Checked;
        }

        private void btnManagment2_Click(object sender, EventArgs e)
        {
            cbIsManagement2.Checked = !cbIsManagement2.Checked;
        }

        private void btnManagment3_Click(object sender, EventArgs e)
        {
            cbIsManagement3.Checked = !cbIsManagement3.Checked;
        }

        private void btnManagment4_Click(object sender, EventArgs e)
        {
            cbIsManagement4.Checked = !cbIsManagement4.Checked;
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            InsertRecoveryPrincipal();
        }
        private void cbIsManagement_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsManagement.Checked)
            {
                cbIsManagement2.Checked = false;
                cbIsManagement3.Checked = false;
                cbIsManagement4.Checked = false;


                btnManagment.BackgroundImage = Properties.Resources.Star_icon1;
            }
            else
                btnManagment.BackgroundImage = Properties.Resources.Star_empty_icon;
        }

        private void cbIsManagement2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsManagement2.Checked)
            {
                cbIsManagement.Checked = false;
                cbIsManagement3.Checked = false;
                cbIsManagement4.Checked = false;
                btnManagment2.BackgroundImage = Properties.Resources.Star_icon2;
            }
            else
                btnManagment2.BackgroundImage = Properties.Resources.Star_empty_icon;
        }

        private void cbIsManagement3_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsManagement3.Checked)
            {
                cbIsManagement.Checked = false;
                cbIsManagement2.Checked = false;
                cbIsManagement4.Checked = false;
                btnManagment3.BackgroundImage = Properties.Resources.Star_icon3;
            }
            else
                btnManagment3.BackgroundImage = Properties.Resources.Star_empty_icon;
        }

        private void cbIsManagement4_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsManagement4.Checked)
            {
                cbIsManagement.Checked = false;
                cbIsManagement2.Checked = false;
                cbIsManagement3.Checked = false;
                btnManagment4.BackgroundImage = Properties.Resources.Star_icon4;
            }
            else
                btnManagment4.BackgroundImage = Properties.Resources.Star_empty_icon;
        }
        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Method
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

            dgvRecovery.EndEdit();

            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = new StringBuilder();

            //Seaover 거래처 내역
            string company_code = lbCompanyCode.Text.Split(',')[0].Trim();
            DataTable companyDt = seaoverRepository.SelectData("업체별월별매출현황", "*", "", $"거래처코드='{company_code}'", "사용자 DESC");
            if (companyDt.Rows.Count == 0)
            {
                MessageBox.Show(this, "거래처 정보를 찾을 수 없습니다.");
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
            int target_recovery_month;
            if (drTarget1.Cells[1].Value == null || !int.TryParse(drTarget1.Cells[1].Value.ToString(), out target_recovery_month))
                target_recovery_month = 0;
            rm.target_recovery_month = target_recovery_month;

            //rm.remark = txtRemark.Text;
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
        private void SetDgvStyle()
        {
            dgvRecovery.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(84, 130, 53);
            dgvRecovery.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRecovery.DefaultCellStyle.Font = new System.Drawing.Font("중고딕", 9, FontStyle.Regular);

            drCurrentSalesAmount.DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            drCurrentSalesAmount.DefaultCellStyle.Font = new System.Drawing.Font("중고딕", 9, FontStyle.Bold);

            drCurrentBalanceAmount.DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            drCurrentBalanceAmount.DefaultCellStyle.Font = new System.Drawing.Font("중고딕", 9, FontStyle.Bold);

            drAverageBalanceAmount.DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            drAverageBalanceAmount.DefaultCellStyle.Font = new System.Drawing.Font("중고딕", 9, FontStyle.Bold);

            drMaximumBalanceAmount.DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            drMaximumBalanceAmount.DefaultCellStyle.Font = new System.Drawing.Font("중고딕", 9, FontStyle.Bold);

            drRecoveryAmount.DefaultCellStyle.BackColor = Color.FromArgb(233, 237, 247);
            drRecoveryAmount.DefaultCellStyle.ForeColor = Color.Blue;
            drRecoveryAmount.DefaultCellStyle.Font = new System.Drawing.Font("중고딕", 9, FontStyle.Bold);
            drRecoveryMonth.DefaultCellStyle.Font = new System.Drawing.Font("중고딕", 9, FontStyle.Bold);

            drTarget1.DefaultCellStyle.Font = new System.Drawing.Font("중고딕", 9, FontStyle.Bold);
            drTarget2.DefaultCellStyle.Font = new System.Drawing.Font("중고딕", 9, FontStyle.Bold);
        }
        private void CalculateReceivableRound(int rowIdx, int columnIdx, double balance_amount)
        {
            double sales_amount;
            if (drCurrentSalesAmount.Cells[columnIdx].Value == null || !double.TryParse(drCurrentSalesAmount.Cells[columnIdx].Value.ToString(), out sales_amount))
                sales_amount = 0;

            double receivalbe_round = sales_amount / balance_amount;
            if (!double.IsNaN(receivalbe_round))
                dgvRecovery.Rows[rowIdx + 1].Cells[columnIdx].Value = receivalbe_round;
        }
        private void CalculateReceivableRoundDays(int rowIdx, int columnIdx)
        {
            double receivalbe_round;
            if (dgvRecovery.Rows[rowIdx].Cells[columnIdx].Value == null || !double.TryParse(dgvRecovery.Rows[rowIdx].Cells[columnIdx].Value.ToString(), out receivalbe_round))
                receivalbe_round = 0;

            double receivalbe_round_days = rbOneMonth.Checked ? 30 / receivalbe_round : rbTwoMonths.Checked ? 60 / receivalbe_round : 90 / receivalbe_round;
            if (!double.IsNaN(receivalbe_round_days))
                dgvRecovery.Rows[rowIdx + 1].Cells[columnIdx].Value = receivalbe_round_days;
        }
        private void CalculateReceivableRoundDaysDivisionMarginRate(int rowIdx, int columnIdx)
        {
            double margin_rate;
            if (drMarginRate.Cells[columnIdx].Value == null || !double.TryParse(drMarginRate.Cells[columnIdx].Value.ToString(), out margin_rate))
                margin_rate = 0;

            double receivalbe_round_days;
            if (dgvRecovery.Rows[rowIdx].Cells[columnIdx].Value == null || !double.TryParse(dgvRecovery.Rows[rowIdx].Cells[columnIdx].Value.ToString(), out receivalbe_round_days))
                receivalbe_round_days = 0;

            double receivalbe_round_days_division_margin = receivalbe_round_days / margin_rate;
            if (!double.IsNaN(receivalbe_round_days_division_margin))
                dgvRecovery.Rows[rowIdx + 1].Cells[columnIdx].Value = receivalbe_round_days_division_margin;
        }
        public void GetData()
        {
            dgvRecovery.Rows.Clear();
            DgvInit();
            backgroundWorker1.RunWorkerAsync();
        }


        #region 로딩 효과
        private void timer_start()
        {
            this.timer = new System.Windows.Threading.DispatcherTimer();
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            this.timer.Tick += timer_Tick;
            this.timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (processingFlag)
            {
                //모든 컨트롤 사용 비활성화
                foreach (Control c in this.Controls)
                {
                    c.Enabled = false;
                }
                pnGlass.Visible = true;
            }
            else
            {
                //모든 컨트롤 사용 활성화
                foreach (Control c in this.Controls)
                {
                    c.Enabled = true;
                }
                pnGlass.Visible = false;
            }
        }
        #endregion
        private void DgvInit()
        {
            dgvRecovery.Rows.Clear();

            int n = dgvRecovery.Rows.Add();
            drCurrentSalesAmount = dgvRecovery.Rows[n];
            drCurrentSalesAmount.Cells["division"].Value = "당월 매출금액";

            n = dgvRecovery.Rows.Add();
            drTotalSalesAmount = dgvRecovery.Rows[n];
            drTotalSalesAmount.Cells["division"].Value = "누적 매출금액";

            n = dgvRecovery.Rows.Add();
            drDiv0 = dgvRecovery.Rows[n];
            drDiv0.DefaultCellStyle.BackColor = Color.LightGray;

            n = dgvRecovery.Rows.Add();
            drCurrentMarginAmount = dgvRecovery.Rows[n];
            drCurrentMarginAmount.Cells["division"].Value = "당월 마진금액";

            n = dgvRecovery.Rows.Add();
            drMarginRate = dgvRecovery.Rows[n];
            drMarginRate.Cells["division"].Value = "마진율(%)";

            n = dgvRecovery.Rows.Add();
            drTotalMarginAmount = dgvRecovery.Rows[n];
            drTotalMarginAmount.Cells["division"].Value = "누적 마진금액";

            n = dgvRecovery.Rows.Add();
            drTotalMarginRate = dgvRecovery.Rows[n];
            drTotalMarginRate.Cells["division"].Value = "마진율(%)";

            n = dgvRecovery.Rows.Add();
            drDiv1 = dgvRecovery.Rows[n];
            drDiv1.DefaultCellStyle.BackColor = Color.LightGray;

            n = dgvRecovery.Rows.Add();
            drCurrentBalanceAmount = dgvRecovery.Rows[n];
            drCurrentBalanceAmount.Cells["division"].Value = "미수잔액(당월)";

            n = dgvRecovery.Rows.Add();
            drCurrentAccountReceivableRound = dgvRecovery.Rows[n];
            drCurrentAccountReceivableRound.Cells["division"].Value = "매출채권회전수";

            n = dgvRecovery.Rows.Add();
            drCurrentAccountReceivableRoundDays = dgvRecovery.Rows[n];
            drCurrentAccountReceivableRoundDays.Cells["division"].Value = "매출채권회전일수";

            n = dgvRecovery.Rows.Add();
            drCurrentRoundDaysDivisionMargin = dgvRecovery.Rows[n];
            drCurrentRoundDaysDivisionMargin.Cells["division"].Value = "채권일수/마진율";

            n = dgvRecovery.Rows.Add();
            drDiv2 = dgvRecovery.Rows[n];
            drDiv2.DefaultCellStyle.BackColor = Color.LightGray;

            n = dgvRecovery.Rows.Add();
            drAverageBalanceAmount = dgvRecovery.Rows[n];
            drAverageBalanceAmount.Cells["division"].Value = "미수잔액(평균)";

            n = dgvRecovery.Rows.Add();
            drAverageAccountReceivableRound = dgvRecovery.Rows[n];
            drAverageAccountReceivableRound.Cells["division"].Value = "매출채권회전수";

            n = dgvRecovery.Rows.Add();
            drAverageAccountReceivableRoundDays = dgvRecovery.Rows[n];
            drAverageAccountReceivableRoundDays.Cells["division"].Value = "매출채권회전일수";

            n = dgvRecovery.Rows.Add();
            drAverageRoundDaysDivisionMargin = dgvRecovery.Rows[n];
            drAverageRoundDaysDivisionMargin.Cells["division"].Value = "채권일수/마진율";

            n = dgvRecovery.Rows.Add();
            drDiv3 = dgvRecovery.Rows[n];
            drDiv3.DefaultCellStyle.BackColor = Color.LightGray;

            n = dgvRecovery.Rows.Add();
            drMaximumBalanceAmount = dgvRecovery.Rows[n];
            drMaximumBalanceAmount.Cells["division"].Value = "미수잔액(최고)";

            n = dgvRecovery.Rows.Add();
            drMaximumAccountReceivableRound = dgvRecovery.Rows[n];
            drMaximumAccountReceivableRound.Cells["division"].Value = "매출채권회전수";

            n = dgvRecovery.Rows.Add();
            drMaximumAccountReceivableRoundDays = dgvRecovery.Rows[n];
            drMaximumAccountReceivableRoundDays.Cells["division"].Value = "매출채권회전일수";

            n = dgvRecovery.Rows.Add();
            drMaximumRoundDaysDivisionMargin = dgvRecovery.Rows[n];
            drMaximumRoundDaysDivisionMargin.Cells["division"].Value = "채권일수/마진율";

            n = dgvRecovery.Rows.Add();
            drDiv4 = dgvRecovery.Rows[n];
            drDiv4.DefaultCellStyle.BackColor = Color.LightGray;

            n = dgvRecovery.Rows.Add();
            drRecoveryAmount = dgvRecovery.Rows[n];
            drRecoveryAmount.Cells["division"].Value = "손익분기잔액";

            n = dgvRecovery.Rows.Add();
            drRecoveryMonth = dgvRecovery.Rows[n];
            drRecoveryMonth.Cells["division"].Value = "윈금회수기간";

            //목표개월 시뮬레이션
            n = dgvRecovery.Rows.Add();
            drDiv5 = dgvRecovery.Rows[n];
            drDiv5.DefaultCellStyle.BackColor = Color.LightGray;
            drDiv5.Cells["division"].Value = "목표달성내용";
            drDiv5.Cells["pre_month_12"].Value = "목표개월수";

            n = dgvRecovery.Rows.Add();
            drTarget1 = dgvRecovery.Rows[n];
            drTarget1.DefaultCellStyle.BackColor = Color.Beige;
            drTarget1.Cells["division"].Value = "1.줄여야할 미수잔액";
            //drTarget1.Cells["pre_month_12"].Value = target_month.ToString();

            n = dgvRecovery.Rows.Add();
            drTarget1_1 = dgvRecovery.Rows[n];
            drTarget1_1.Cells["division"].Value = "1-1.줄여야할 총 금액";
            

            n = dgvRecovery.Rows.Add();
            drTarget1_2 = dgvRecovery.Rows[n];
            drTarget1_2.Cells["division"].Value = "1-2.한달동안 수금액";

            n = dgvRecovery.Rows.Add();
            drTarget1_3 = dgvRecovery.Rows[n];
            drTarget1_3.Cells["division"].Value = "1-2.유지해야할 외상미수잔액";

            n = dgvRecovery.Rows.Add();
            drTarget2 = dgvRecovery.Rows[n];
            drTarget2.DefaultCellStyle.BackColor = Color.Beige;
            drTarget2.Cells["division"].Value = "2.미수잔액이 같을때 마진";

            n = dgvRecovery.Rows.Add();
            drTarget2_1 = dgvRecovery.Rows[n];
            drTarget2_1.Cells["division"].Value = "2-1.높여야할 한달이익금액";

            n = dgvRecovery.Rows.Add();
            drTarget2_2 = dgvRecovery.Rows[n];
            drTarget2_2.Cells["division"].Value = "2-2.올려야할 한달 마진율";


            foreach (DataGridViewColumn col in dgvRecovery.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            SetDgvStyle();

        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            processingFlag = true;
            recoveryDt = GetDataTable(); // 데이터베이스에서 데이터 가져오기 예시 메소드
            if (recoveryDt != null && recoveryDt.Rows.Count > 0)
            {
                DateTime standardDate;
                if (!DateTime.TryParse(txtStandardDate.Text, out standardDate))
                    return;

                //Datatable 생성
                DataTable recoveryTempDt = new DataTable();
                recoveryTempDt.Columns.Add("standardYM", typeof(string));
                recoveryTempDt.Columns.Add("sales_amount", typeof(double));
                recoveryTempDt.Columns.Add("pre_total_sales_amount", typeof(double));
                recoveryTempDt.Columns.Add("balance_amount", typeof(double));
                recoveryTempDt.Columns.Add("average_balance_amount", typeof(double));
                recoveryTempDt.Columns.Add("maximum_balance_amount", typeof(double));
                recoveryTempDt.Columns.Add("margin_amount", typeof(double));
                recoveryTempDt.Columns.Add("pre_total_margin_amount", typeof(double));

                //전년전월 데이터
                double pre_total_sales_amcount = Convert.ToDouble(recoveryDt.Rows[0]["전월매출금액"].ToString());
                double pre_total_margin_amount = Convert.ToDouble(recoveryDt.Rows[0]["전월마진금액"].ToString());

                DateTime tempDt = standardDate.AddYears(-1).AddMonths(1);
                DateTime endTempDt = new DateTime(tempDt.Year, tempDt.Month, standardDate.Day);
                DateTime sttTempDt = rbOneMonth.Checked ? endTempDt.AddMonths(-1).AddDays(1) : rbTwoMonths.Checked? endTempDt.AddMonths(-2).AddDays(1) : endTempDt.AddMonths(-3).AddDays(1);
                string whr = $"날짜 < '{sttTempDt.ToString("yyyy-MM-dd")}'";
                DataRow[] drList = recoveryDt.Select(whr);
                if (drList.Length > 0)
                {
                    foreach (DataRow ddr in drList)
                    {
                        double temp_sales_amount;
                        if (!double.TryParse(ddr["매출"].ToString(), out temp_sales_amount))
                            temp_sales_amount = 0;
                        pre_total_sales_amcount += temp_sales_amount;

                        double temp_margin_amount;
                        if (!double.TryParse(ddr["손익"].ToString(), out temp_margin_amount))
                            temp_margin_amount = 0;
                        pre_total_margin_amount += temp_margin_amount;
                    }
                }

                DataRow dr = recoveryTempDt.NewRow();
                dr["standardYM"] = standardDate.AddYears(-1).ToString("yyyyMM");
                dr["pre_total_sales_amount"] = pre_total_sales_amcount;
                dr["pre_total_margin_amount"] = pre_total_margin_amount;
                recoveryTempDt.Rows.Add(dr);

                //1년간 데이터 
                do
                {
                    int tmpDay;
                    if (new DateTime(tempDt.Year, tempDt.Month, 1).AddMonths(1).AddDays(-1).Day < standardDate.Day)
                        tmpDay = new DateTime(tempDt.Year, tempDt.Month, 1).AddMonths(1).AddDays(-1).Day;
                    else
                        tmpDay = standardDate.Day;

                    endTempDt = new DateTime(tempDt.Year, tempDt.Month, tmpDay);
                    sttTempDt = rbOneMonth.Checked ? endTempDt.AddMonths(-1).AddDays(1) : rbTwoMonths.Checked ? endTempDt.AddMonths(-2).AddDays(1) : endTempDt.AddMonths(-3).AddDays(1);


                    //당월 매출, 마진, 평균미수
                    double total_sales_amount = 0, total_margin_amount = 0, maximum_balance_amount = 0;
                    whr = $"날짜 >= '{sttTempDt.ToString("yyyy-MM-dd")}' AND 날짜 <= '{endTempDt.ToString("yyyy-MM-dd")}'";
                    drList = recoveryDt.Select(whr);
                    if (drList.Length > 0)
                    {
                        foreach (DataRow ddr in drList)
                        {
                            double temp_sales_amount;
                            if (!double.TryParse(ddr["매출"].ToString(), out temp_sales_amount))
                                temp_sales_amount = 0;
                            total_sales_amount += temp_sales_amount;

                            double temp_margin_amount;
                            if (!double.TryParse(ddr["손익"].ToString(), out temp_margin_amount))
                                temp_margin_amount = 0;
                            total_margin_amount += temp_margin_amount;

                            double temp_balance_amount;
                            if (!double.TryParse(ddr["잔액"].ToString(), out temp_balance_amount))
                                temp_balance_amount = 0;
                            if (maximum_balance_amount < temp_balance_amount)
                                maximum_balance_amount = temp_balance_amount;
                        }
                    }

                    //누적마진, 누적매출
                    double total_accumulate_margin_amount = 0;
                    double total_accumulate_sales_amcount = 0;
                    whr = $"날짜 < '{sttTempDt.ToString("yyyy-MM-dd")}'";
                    drList = recoveryDt.Select(whr);
                    if (drList.Length > 0)
                    {
                        foreach (DataRow ddr in drList)
                        {
                            double temp_sales_amount;
                            if (!double.TryParse(ddr["매출"].ToString(), out temp_sales_amount))
                                temp_sales_amount = 0;
                            total_accumulate_sales_amcount += temp_sales_amount;

                            double temp_margin_amount;
                            if (!double.TryParse(ddr["손익"].ToString(), out temp_margin_amount))
                                temp_margin_amount = 0;
                            total_accumulate_margin_amount += temp_margin_amount;
                        }
                    }

                    //평균미수
                    endTempDt = new DateTime(tempDt.Year, tempDt.Month, tmpDay);
                    sttTempDt = rbOneMonth.Checked ? endTempDt.AddMonths(-1).AddDays(1) : rbTwoMonths.Checked ? endTempDt.AddMonths(-2).AddDays(1) : endTempDt.AddMonths(-3).AddDays(1);
                    double total_average_balance_amount = 0;
                    whr = $"날짜 >= '{sttTempDt.ToString("yyyy-MM-dd")}' AND 날짜 <= '{endTempDt.ToString("yyyy-MM-dd")}'";
                    drList = recoveryDt.Select(whr);
                    if (drList.Length > 0)
                    {
                        foreach (DataRow ddr in drList)
                        {
                            double temp_balance_amount;
                            if (!double.TryParse(ddr["잔액"].ToString(), out temp_balance_amount))
                                temp_balance_amount = 0;
                            total_average_balance_amount += temp_balance_amount;
                        }
                        //평균
                        total_average_balance_amount /= drList.Length;
                    }

                    //당월 마지막일 기준 미수
                    double balance_amount = 0;
                    whr = $"날짜 = '{new DateTime(tempDt.Year, tempDt.Month, tmpDay).ToString("yyyy-MM-dd")}'";
                    drList = recoveryDt.Select(whr);
                    if (drList.Length > 0)
                        balance_amount = Convert.ToDouble(drList[0]["잔액"].ToString());


                    //전년전월 데이터
                    dr = recoveryTempDt.NewRow();
                    dr["standardYM"] = tempDt.ToString("yyyyMM");
                    dr["sales_amount"] = total_sales_amount;
                    dr["pre_total_sales_amount"] = Convert.ToDouble(recoveryDt.Rows[0]["전월매출금액"].ToString()) + total_accumulate_sales_amcount;
                    dr["balance_amount"] = balance_amount;
                    dr["average_balance_amount"] = total_average_balance_amount;
                    dr["maximum_balance_amount"] = maximum_balance_amount;
                    dr["margin_amount"] = total_margin_amount;
                    dr["pre_total_margin_amount"] = Convert.ToDouble(recoveryDt.Rows[0]["전월마진금액"].ToString()) + total_accumulate_margin_amount;
                    recoveryTempDt.Rows.Add(dr);

                    tempDt = tempDt.AddMonths(1);
                } while (Convert.ToDateTime(tempDt.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(standardDate.ToString("yyyy-MM-dd")));


                recoveryDt = recoveryTempDt;
                recoveryDt.AcceptChanges();
            }
            else
                recoveryDt = null;
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetHeaderText();
            drTarget1.Cells[1].Value = target_month;
            if (recoveryDt != null && recoveryDt.Rows.Count > 0)
            {
                DateTime standardDate;
                if (!DateTime.TryParse(txtStandardDate.Text, out standardDate))
                    return;

                this.dgvRecovery.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRecovery_CellValueChanged);
                DateTime tempDt = standardDate.AddYears(-1);
                for (int i = 0; i < recoveryDt.Rows.Count; i++)
                {
                    string col_name = "pre_month_" + (12 - i);

                    double sales_amount;
                    if (!double.TryParse(recoveryDt.Rows[i]["sales_amount"].ToString(), out sales_amount))
                        sales_amount = 0;
                    double pre_total_sales_amount;
                    if (!double.TryParse(recoveryDt.Rows[i]["pre_total_sales_amount"].ToString(), out pre_total_sales_amount))
                        pre_total_sales_amount = 0;

                    double balance_amount;
                    if (!double.TryParse(recoveryDt.Rows[i]["balance_amount"].ToString(), out balance_amount))
                        balance_amount = 0;
                    double average_balance_amount;
                    if (!double.TryParse(recoveryDt.Rows[i]["average_balance_amount"].ToString(), out average_balance_amount))
                        average_balance_amount = 0;
                    double maximum_balance_amount;
                    if (!double.TryParse(recoveryDt.Rows[i]["maximum_balance_amount"].ToString(), out maximum_balance_amount))
                        maximum_balance_amount = 0;

                    double margin_amount;
                    if (!double.TryParse(recoveryDt.Rows[i]["margin_amount"].ToString(), out margin_amount))
                        margin_amount = 0;

                    double pre_total_margin_amount;
                    if (!double.TryParse(recoveryDt.Rows[i]["pre_total_margin_amount"].ToString(), out pre_total_margin_amount))
                        pre_total_margin_amount = 0;

                    drCurrentSalesAmount.Cells[col_name].Value = sales_amount;
                    drTotalSalesAmount.Cells[col_name].Value = (pre_total_sales_amount + sales_amount);
                    drCurrentMarginAmount.Cells[col_name].Value = margin_amount;
                    drTotalMarginAmount.Cells[col_name].Value = (pre_total_margin_amount + margin_amount);
                    drCurrentBalanceAmount.Cells[col_name].Value = balance_amount;
                    drAverageBalanceAmount.Cells[col_name].Value = average_balance_amount;
                    drMaximumBalanceAmount.Cells[col_name].Value = maximum_balance_amount;
                    

                    if (sales_amount > 0)
                    {
                        /*drMarginRate.Cells[col_name].Value = margin_amount / sales_amount;
                        drAccountReceivableRound.Cells[col_name].Value = sales_amount / balance_amount;
                        drAccountReceivableRound.Cells[col_name].Value = 30 / sales_amount / balance_amount;

                        drRoundDaysDivisionMargin.Cells[col_name].Value = (30 / sales_amount / balance_amount) / (margin_amount / sales_amount * 100);*/
                    }

                }
                Calculate();
                this.dgvRecovery.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRecovery_CellValueChanged);
            }
            processingFlag = false;
        }

        private void Calculate()
        {
            if (dgvRecovery.Rows.Count > 0)
            { 
                for(int i = 2; i < dgvRecovery.ColumnCount; i++) 
                {
                    double sales_amount;
                    if (drCurrentSalesAmount.Cells[i].Value == null || !double.TryParse(drCurrentSalesAmount.Cells[i].Value.ToString(), out sales_amount))
                        sales_amount = 0;

                    double total_sales_amount;
                    if (drTotalSalesAmount.Cells[i].Value == null || !double.TryParse(drTotalSalesAmount.Cells[i].Value.ToString(), out total_sales_amount))
                        total_sales_amount = 0;

                    double margin_amount;
                    if (drCurrentMarginAmount.Cells[i].Value == null || !double.TryParse(drCurrentMarginAmount.Cells[i].Value.ToString(), out margin_amount))
                        margin_amount = 0;

                    double total_margin_amount;
                    if (drTotalMarginAmount.Cells[i].Value == null || !double.TryParse(drTotalMarginAmount.Cells[i].Value.ToString(), out total_margin_amount))
                        total_margin_amount = 0;

                    //마진율 계산
                    double margin_rate = margin_amount / sales_amount * 100;
                    double total_margin_rate = total_margin_amount / total_sales_amount * 100;
                    drMarginRate.Cells[i].Value = margin_rate;
                    drTotalMarginRate.Cells[i].Value = total_margin_rate;


                    //미수잔액(당월)
                    double current_balance_amount;
                    if (drCurrentBalanceAmount.Cells[i].Value == null || !double.TryParse(drCurrentBalanceAmount.Cells[i].Value.ToString(), out current_balance_amount))
                        current_balance_amount = 0;

                    CalculateReceivableRound(8, i, current_balance_amount);
                    CalculateReceivableRoundDays(9, i);
                    CalculateReceivableRoundDaysDivisionMarginRate(10, i);

                    //미수잔액(평균)
                    double average_balance_amount;
                    if (drAverageBalanceAmount.Cells[i].Value == null || !double.TryParse(drAverageBalanceAmount.Cells[i].Value.ToString(), out average_balance_amount))
                        average_balance_amount = 0;

                    CalculateReceivableRound(13, i, average_balance_amount);
                    CalculateReceivableRoundDays(14, i);
                    CalculateReceivableRoundDaysDivisionMarginRate(15, i);

                    //미수잔액(최고)
                    double maximum_balance_amount;
                    if (drMaximumBalanceAmount.Cells[i].Value == null || !double.TryParse(drMaximumBalanceAmount.Cells[i].Value.ToString(), out maximum_balance_amount))
                        maximum_balance_amount = 0;

                    CalculateReceivableRound(18, i, maximum_balance_amount);
                    CalculateReceivableRoundDays(19, i);
                    CalculateReceivableRoundDaysDivisionMarginRate(20, i);

                    //손익분기잔액, 원금회수기간 계산
                    double recovery_amount = current_balance_amount - (total_margin_amount - margin_amount);
                    drRecoveryAmount.Cells[i].Value = recovery_amount;
                    double recovery_months = rbOneMonth.Checked ? recovery_amount / margin_amount : rbTwoMonths.Checked ? recovery_amount / margin_amount * 2 : recovery_amount / margin_amount * 3;
                    drRecoveryMonth.Cells[i].Value = recovery_months;

                    //목표
                    CalculateTarget(i);
                }
            }
        }


        private DataTable GetDataTable() 
        {
            DateTime standardDate;
            if (!DateTime.TryParse(txtStandardDate.Text, out standardDate))
            {
                messageBox.Show(this, "기준일자를 다시 확인해주세요.");
                return null;
            }
            //DATA 불러오기
            int salesTerms = rbOneMonth.Checked ? 1 : rbThreeMonths.Checked ? 2 : 3;
            DataTable resultDt = recoveryPrincipalPriceRepository.GetRecoveryDetail(standardDate, salesTerms, lbCompanyCode.Text, "");          
            return resultDt;
        }
        private void SetHeaderText()
        {
            DateTime standardDate;
            if (!DateTime.TryParse(txtStandardDate.Text, out standardDate))
            {
                messageBox.Show(this, "기준일자를 다시 확인해주세요.");
                return;
            }
            //당월
            dgvRecovery.Columns["pre_month_0"].HeaderText = standardDate.ToString("MM월");
            //전월
            for(int i = 0; i <= 12; i++) 
            {
                if (standardDate.Year > standardDate.AddMonths(-i).Year)
                    dgvRecovery.Columns["pre_month_" + i].HeaderText = standardDate.AddMonths(-i).ToString("yy/MM월");
                else
                    dgvRecovery.Columns["pre_month_" + i].HeaderText = standardDate.AddMonths(-i).ToString("MM월");

                dgvRecovery.Columns["pre_month_" + i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvRecovery.Columns["pre_month_" + i].DefaultCellStyle.Format = "#,##0";

                drCurrentAccountReceivableRound.Cells["pre_month_" + i].Style.Format = "#,##0.00";
                drAverageAccountReceivableRound.Cells["pre_month_" + i].Style.Format = "#,##0.00";
                drMaximumAccountReceivableRound.Cells["pre_month_" + i].Style.Format = "#,##0.00";

                drCurrentRoundDaysDivisionMargin.Cells["pre_month_" + i].Style.Format = "#,##0.00";
                drAverageRoundDaysDivisionMargin.Cells["pre_month_" + i].Style.Format = "#,##0.00";
                drMaximumRoundDaysDivisionMargin.Cells["pre_month_" + i].Style.Format = "#,##0.00";

                drMarginRate.Cells["pre_month_" + i].Style.Format = "#,##0.00";
                drTotalMarginRate.Cells["pre_month_" + i].Style.Format = "#,##0.00";
            }
        }

        #endregion

        #region Datagridview event
        private void dgvRecovery_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string col_name = dgvRecovery.Columns[e.ColumnIndex].Name;
                if (col_name.Contains("pre_month") && e.ColumnIndex > 1)
                {
                    dgvRecovery.EndEdit();
                    //매출단가, 마진금액 수정
                    if (drCurrentSalesAmount.Index == e.RowIndex || drCurrentMarginAmount.Index == e.RowIndex)
                    {
                        double sales_amount;
                        if (drCurrentSalesAmount.Cells[e.ColumnIndex].Value == null || !double.TryParse(drCurrentSalesAmount.Cells[e.ColumnIndex].Value.ToString(), out sales_amount))
                            sales_amount = 0;

                        double margin_amount;
                        if (drCurrentMarginAmount.Cells[e.ColumnIndex].Value == null || !double.TryParse(drCurrentMarginAmount.Cells[e.ColumnIndex].Value.ToString(), out margin_amount))
                            margin_amount = 0;

                        double margin_rate;
                        if (sales_amount > 0 && margin_amount > 0)
                            margin_rate = margin_amount / sales_amount * 100;
                        else
                            margin_rate = 0;
                        if(!double.IsNaN(margin_rate))
                            drMarginRate.Cells[e.ColumnIndex].Value = margin_rate * 100;

                        //손익분기잔액, 원금회수기간 계산
                        double current_balance_amount;
                        if (drCurrentBalanceAmount.Cells[e.ColumnIndex].Value == null || !double.TryParse(drCurrentBalanceAmount.Cells[e.ColumnIndex].Value.ToString(), out current_balance_amount))
                            current_balance_amount = 0;

                        CalculateReceivableRound(e.RowIndex, e.ColumnIndex, current_balance_amount);

                        double total_margin_amount;
                        if (drTotalMarginAmount.Cells[e.ColumnIndex].Value == null || !double.TryParse(drTotalMarginAmount.Cells[e.ColumnIndex].Value.ToString(), out total_margin_amount))
                            total_margin_amount = 0;
                        double current_margin_amount;
                        if (drCurrentMarginAmount.Cells[e.ColumnIndex].Value == null || !double.TryParse(drCurrentMarginAmount.Cells[e.ColumnIndex].Value.ToString(), out current_margin_amount))
                            current_margin_amount = 0;
                        double recovery_amount = current_balance_amount - (total_margin_amount - current_margin_amount);
                        if (!double.IsNaN(recovery_amount))
                            drRecoveryAmount.Cells[e.ColumnIndex].Value = recovery_amount;
                        double recovery_months = rbOneMonth.Checked ? recovery_amount / current_margin_amount : rbTwoMonths.Checked ? recovery_amount / current_margin_amount * 2 : recovery_amount / current_margin_amount * 3;
                        if (!double.IsNaN(recovery_months))
                            drRecoveryMonth.Cells[e.ColumnIndex].Value = recovery_months;
                        //목표
                        CalculateTarget(e.ColumnIndex);

                    }
                    //누적매출, 누적마진 
                    else if (drTotalSalesAmount.Index == e.RowIndex || drTotalMarginAmount.Index == e.RowIndex)
                    {
                        double total_sales_amount;
                        if (drTotalSalesAmount.Cells[e.ColumnIndex].Value == null || !double.TryParse(drTotalSalesAmount.Cells[e.ColumnIndex].Value.ToString(), out total_sales_amount))
                            total_sales_amount = 0;

                        double total_margin_amount;
                        if (drTotalMarginAmount.Cells[e.ColumnIndex].Value == null || !double.TryParse(drTotalMarginAmount.Cells[e.ColumnIndex].Value.ToString(), out total_margin_amount))
                            total_margin_amount = 0;

                        double margin_rate;
                        if (total_sales_amount > 0 && total_margin_amount > 0)
                            margin_rate = total_margin_amount / total_sales_amount;
                        else
                            margin_rate = 0;

                        if (!double.IsNaN(margin_rate))
                            drTotalMarginRate.Cells[e.ColumnIndex].Value = margin_rate * 100;
                        //목표
                        CalculateTarget(e.ColumnIndex);
                    }
                    //마진율 수정
                    else if (drMarginRate.Index == e.RowIndex)
                    {
                        CalculateReceivableRoundDaysDivisionMarginRate(10, e.ColumnIndex);
                        CalculateReceivableRoundDaysDivisionMarginRate(15, e.ColumnIndex);
                        CalculateReceivableRoundDaysDivisionMarginRate(20, e.ColumnIndex);
                    }
                    //현재 미수잔액
                    else if (drCurrentMarginAmount.Index == e.RowIndex || drCurrentBalanceAmount.Index == e.RowIndex)
                    {
                        double current_balance_amount;
                        if (drCurrentBalanceAmount.Cells[e.ColumnIndex].Value == null || !double.TryParse(drCurrentBalanceAmount.Cells[e.ColumnIndex].Value.ToString(), out current_balance_amount))
                            current_balance_amount = 0;

                        CalculateReceivableRound(e.RowIndex, e.ColumnIndex, current_balance_amount);


                        //손익분기잔액, 원금회수기간 계산
                        double total_margin_amount;
                        if (drTotalMarginAmount.Cells[e.ColumnIndex].Value == null || !double.TryParse(drTotalMarginAmount.Cells[e.ColumnIndex].Value.ToString(), out total_margin_amount))
                            total_margin_amount = 0;
                        double current_margin_amount;
                        if (drCurrentMarginAmount.Cells[e.ColumnIndex].Value == null || !double.TryParse(drCurrentMarginAmount.Cells[e.ColumnIndex].Value.ToString(), out current_margin_amount))
                            current_margin_amount = 0;
                        double recovery_amount = current_balance_amount - (total_margin_amount - current_margin_amount);
                        if (!double.IsNaN(recovery_amount))
                            drRecoveryAmount.Cells[e.ColumnIndex].Value = recovery_amount;
                        double recovery_months = rbOneMonth.Checked ? recovery_amount / current_margin_amount : rbTwoMonths.Checked ? recovery_amount / current_margin_amount * 2 : recovery_amount / current_margin_amount * 3;
                        if (!double.IsNaN(recovery_months))
                            drRecoveryMonth.Cells[e.ColumnIndex].Value = recovery_months;

                        //목표
                        CalculateTarget(e.ColumnIndex);
                    }
                    else if (drCurrentAccountReceivableRound.Index == e.RowIndex)
                    {
                        CalculateReceivableRoundDays(e.RowIndex, e.ColumnIndex);
                    }
                    else if (drCurrentAccountReceivableRoundDays.Index == e.RowIndex)
                    {
                        CalculateReceivableRoundDaysDivisionMarginRate(e.RowIndex, e.ColumnIndex);
                    }

                    //평균 미수잔액
                    else if (drAverageBalanceAmount.Index == e.RowIndex)
                    {
                        double average_balance_amount;
                        if (drAverageBalanceAmount.Cells[e.ColumnIndex].Value == null || !double.TryParse(drAverageBalanceAmount.Cells[e.ColumnIndex].Value.ToString(), out average_balance_amount))
                            average_balance_amount = 0;

                        CalculateReceivableRound(e.RowIndex, e.ColumnIndex, average_balance_amount);
                    }
                    else if (drAverageAccountReceivableRound.Index == e.RowIndex)
                    {
                        CalculateReceivableRoundDays(e.RowIndex, e.ColumnIndex);
                    }
                    else if (drAverageAccountReceivableRoundDays.Index == e.RowIndex)
                    {
                        CalculateReceivableRoundDaysDivisionMarginRate(e.RowIndex, e.ColumnIndex);
                    }

                    //최고 미수잔액
                    else if (drMaximumBalanceAmount.Index == e.RowIndex)
                    {
                        double maximum_balance_amount;
                        if (drMaximumBalanceAmount.Cells[e.ColumnIndex].Value == null || !double.TryParse(drMaximumBalanceAmount.Cells[e.ColumnIndex].Value.ToString(), out maximum_balance_amount))
                            maximum_balance_amount = 0;

                        CalculateReceivableRound(e.RowIndex, e.ColumnIndex, maximum_balance_amount);
                    }
                    else if (drMaximumAccountReceivableRound.Index == e.RowIndex)
                    {
                        CalculateReceivableRoundDays(e.RowIndex, e.ColumnIndex);
                    }
                    else if (drMaximumAccountReceivableRoundDays.Index == e.RowIndex)
                    {
                        CalculateReceivableRoundDaysDivisionMarginRate(e.RowIndex, e.ColumnIndex);
                    }


                    //원금회수기간
                    else if (drRecoveryMonth.Index == e.RowIndex && e.ColumnIndex > 1)
                    {
                        CalculateTarget(e.ColumnIndex);
                    }
                }
                else if (col_name.Contains("pre_month") && e.ColumnIndex == 1 && drTarget1.Index == e.RowIndex)
                {
                    for (int i = 2; i < dgvRecovery.ColumnCount; i++)
                    {
                        CalculateTarget(i);
                    }
                }       
            }
        }

        private void CalculateTarget(int col)
        {
            double target_months;
            if (drTarget1.Cells[1].Value == null || !double.TryParse(drTarget1.Cells[1].Value.ToString(), out target_months))
                target_months = 0;

            double recovery_months;
            if (drRecoveryMonth.Cells[col].Value == null || !double.TryParse(drRecoveryMonth.Cells[col].Value.ToString(), out recovery_months))
                recovery_months = 0;

            double month_margin_amount;
            if (drCurrentMarginAmount.Cells[col].Value == null || !double.TryParse(drCurrentMarginAmount.Cells[col].Value.ToString(), out month_margin_amount))
                month_margin_amount = 0;

            double total_month_margin_amount;
            if (drTotalMarginAmount.Cells[col].Value == null || !double.TryParse(drTotalMarginAmount.Cells[col].Value.ToString(), out total_month_margin_amount))
                total_month_margin_amount = 0;

            double current_balance_amount;
            if (drCurrentBalanceAmount.Cells[col].Value == null || !double.TryParse(drCurrentBalanceAmount.Cells[col].Value.ToString(), out current_balance_amount))
                current_balance_amount = 0;

            double sales_amount;
            if (drCurrentSalesAmount.Cells[col].Value == null || !double.TryParse(drCurrentSalesAmount.Cells[col].Value.ToString(), out sales_amount))
                sales_amount = 0;

            if (sales_amount > 0)
            {
                if (rbTwoMonths.Checked)
                {
                    month_margin_amount /= 2;
                    sales_amount /= 2;
                }
                else if (rbThreeMonths.Checked)
                {
                    month_margin_amount /= 3;
                    sales_amount /= 3;
                }

                drTarget1_1.Cells[col].Value = (recovery_months - target_months) * month_margin_amount;
                drTarget1_2.Cells[col].Value = ((recovery_months - target_months) * month_margin_amount) / target_months;
                drTarget1_3.Cells[col].Value = current_balance_amount - ((recovery_months - target_months) * month_margin_amount);

                drTarget2_1.Cells[col].Value = (total_month_margin_amount - month_margin_amount) / target_months;
                drTarget2_2.Cells[col].Value = (((total_month_margin_amount - month_margin_amount) / target_months) / sales_amount * 100).ToString("#,##0.0") + "%";
            }
        }


        #endregion

        private void dgvRecovery_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 1)
            {
                if (drCurrentRoundDaysDivisionMargin.Index == e.RowIndex
                    || drAverageRoundDaysDivisionMargin.Index == e.RowIndex
                    || drMaximumRoundDaysDivisionMargin.Index == e.RowIndex)
                {

                    double divVal;
                    if (dgvRecovery.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !double.TryParse(dgvRecovery.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out divVal))
                        divVal = 0;

                    if (divVal > 7)
                        dgvRecovery.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
                    else
                        dgvRecovery.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;

                }
                else if (drCurrentMarginAmount.Index == e.RowIndex || drMarginRate.Index == e.RowIndex
                    || drTotalMarginAmount.Index == e.RowIndex || drTotalMarginRate.Index == e.RowIndex
                    || drMarginRate.Index == e.RowIndex || drTotalMarginRate.Index == e.RowIndex)
                {
                    double divVal;
                    if (dgvRecovery.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !double.TryParse(dgvRecovery.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out divVal))
                        divVal = 0;

                    if (divVal < 0)
                        dgvRecovery.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
                    else
                        dgvRecovery.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                }
            }
        }

        
    }
}
