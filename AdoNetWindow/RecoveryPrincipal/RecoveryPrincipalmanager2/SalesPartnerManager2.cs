using Repositories.RecoveryPrincipal;
using Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using AdoNetWindow.Model;
using Repositories.Company;
using Repositories.SalesPartner;
using AdoNetWindow.RecoveryPrincipal.RecoveryPrincipalmanager2;
using System.Reflection;
using Microsoft.WindowsAPICodePack.ApplicationServices;
using System.Security.RightsManagement;
using Google.Type;
using Color = System.Drawing.Color;
using DateTime = System.DateTime;
using MySqlX.XDevAPI.Relational;
using Repositories.SEAOVER;
using AdoNetWindow.Common;
using Repositories.Config;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using DataTable = System.Data.DataTable;
using Font = System.Drawing.Font;
using Excel = Microsoft.Office.Interop.Excel;

namespace AdoNetWindow.RecoveryPrincipal
{
    public partial class SalesPartnerManager2 : Form
    {
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        ISeaoverCompanyRepository seaoverCompanyRepository = new SeaoverCompanyRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        ICompanyRecoveryRepository companyRecoveryRepository = new CompanyRecoveryRepository();
        ICommonRepository commonRepository = new CommonRepository();
        ICompanyRepository companyRepository = new CompanyRepository();
        ISalesPartnerRepository salesPartnerRepository = new SalesPartnerRepository();
        IRecoveryPrincipalPriceRepository recoveryPrincipalPriceRepository = new RecoveryPrincipalPriceRepository();
        ICompanyGroupRepository companyGroupRepository = new CompanyGroupRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        UsersModel um;


        private System.Windows.Threading.DispatcherTimer timer;
        bool processingFlag = false;
        DataTable companyDt = null;
        static Microsoft.Office.Interop.Excel.Application excelApp = null;
        static Microsoft.Office.Interop.Excel.Workbook workBook = null;
        static Microsoft.Office.Interop.Excel.Worksheet workSheet = null;

        public SalesPartnerManager2(UsersModel uModel)
        {
            InitializeComponent();
            this.um = uModel;
        }

        private void SalesPartnerManager2_Load(object sender, EventArgs e)
        {
            txtManager.Text = um.user_name;
            //업체별시세현황 스토어프로시져 호출
            recoveryPrincipalPriceRepository.SetSeaoverId(um.seaover_id);

            dgvCompany.DoubleBuffered(true);
            SetHeaderStyleSetting();

            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            timer_start();
        }

        #region Backgroudwork event
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            processingFlag = true;
            companyDt = RefreshDatatable();
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GetPartner();
            processingFlag = false;
        }

        #endregion

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

        #region Button, Check box

        private void cbCurrentMaximum_CheckedChanged(object sender, EventArgs e)
        {
            dgvCompany.Columns["div5"].Visible = cbCurrentMaximum.Checked;
            dgvCompany.Columns["maximum_balance_amount"].Visible = cbCurrentMaximum.Checked;
            dgvCompany.Columns["rotation_maximum"].Visible = cbCurrentMaximum.Checked;
            dgvCompany.Columns["rotation_maximum_days"].Visible = cbCurrentMaximum.Checked;
            dgvCompany.Columns["rotation_maximum_division_margin"].Visible = cbCurrentMaximum.Checked;
        }

        private void cbPreOneMonthtMaximum_CheckedChanged(object sender, EventArgs e)
        {
            dgvCompany.Columns["div6"].Visible = cbPreOneMonthtMaximum.Checked;
            dgvCompany.Columns["pre1_maximum_balance_amount"].Visible = cbPreOneMonthtMaximum.Checked;
            dgvCompany.Columns["pre1_rotation_maximum"].Visible = cbPreOneMonthtMaximum.Checked;
            dgvCompany.Columns["pre1_rotation_maximum_days"].Visible = cbPreOneMonthtMaximum.Checked;
            dgvCompany.Columns["pre1_rotation_maximum_division_margin"].Visible = cbPreOneMonthtMaximum.Checked;
        }

        private void cbPreTowMonthtsMaximum_CheckedChanged(object sender, EventArgs e)
        {
            dgvCompany.Columns["div7"].Visible = cbPreTowMonthtsMaximum.Checked;
            dgvCompany.Columns["pre2_maximum_balance_amount"].Visible = cbPreTowMonthtsMaximum.Checked;
            dgvCompany.Columns["pre2_rotation_maximum"].Visible = cbPreTowMonthtsMaximum.Checked;
            dgvCompany.Columns["pre2_rotation_maximum_days"].Visible = cbPreTowMonthtsMaximum.Checked;
            dgvCompany.Columns["pre2_rotation_maximum_division_margin"].Visible = cbPreTowMonthtsMaximum.Checked;
        }
        private void btnAdvancedFilter_Click(object sender, EventArgs e)
        {
            pnAdvancedFilter.Visible = !pnAdvancedFilter.Visible;
        }
        private void btnAdvanceFilterClose_Click(object sender, EventArgs e)
        {
            pnAdvancedFilter.Visible = false;
        }
        private void btnRefreshDatatable_Click(object sender, EventArgs e)
        {
            RefreshData();
        }
        public void RefreshData()
        {
            backgroundWorker1.RunWorkerAsync();
        }
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

            if (dgvCompany.Columns.Count > 0)
            {
                List<string> col_nme = new List<string>();
                for (int i = 0; i < dgvCompany.Columns.Count; i++)
                {
                    if (dgvCompany.Columns[i].Visible)
                        col_nme.Add(dgvCompany.Columns[i].Name);
                }
                GetExeclColumn(col_nme);
            }
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

            //row data
            int rowIndex = 2;
            try
            {
                excelApp = new Excel.Application();                                                 //엑셀 어플리케이션 생성
                workBook = excelApp.Workbooks.Add();                                                //워크북 추가
                workSheet = workBook.Worksheets.get_Item(1) as Excel.Worksheet;                     //엑셀 첫번째 워크시트 가져오기
                Microsoft.Office.Interop.Excel.Worksheet wk = workSheet;

                setAutomatic(excelApp, false);

                if (dgvCompany.Rows.Count > 0)
                {
                    //int row = newDt.Rows.Count + 1;
                    int row = 1;
                    foreach (DataGridViewRow dr in dgvCompany.Rows)
                    {
                        if (dr.Visible)
                            row++;
                    }
                    int column = 1;
                    foreach (DataGridViewColumn col in dgvCompany.Columns)
                    {
                        if (col.Visible)
                            column++;
                    }
                    object[,] data = new object[row, column];

                    //Data
                    Excel.Range rng = workSheet.get_Range("A1", "AC" + row + 2);
                    object[,] only_data = (object[,])rng.get_Value();

                    data = only_data;

                    //Header
                    for (int i = 0; i < col_name.Count; i++)
                    {
                        //wk.Cells[1, i + 1].value = dgvProduct.Columns[col_name[i]].HeaderText;

                        data[1, i + 1] = dgvCompany.Columns[col_name[i]].HeaderText;
                    }
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        if (dgvCompany.Rows[i].Visible)
                        {
                            for (int j = 0; j < col_name.Count; j++)
                            {
                                if (dgvCompany.Rows[i].Cells[col_name[j]].Value == null)
                                    dgvCompany.Rows[i].Cells[col_name[j]].Value = "";
                                only_data[rowIndex, j + 1] = dgvCompany.Rows[i].Cells[col_name[j]].Value.ToString();
                            }
                            rowIndex++;
                        }
                    }

                    rng.Value = data;
                }
                //Title
                int col_cnt = col_name.Count;
                Excel.Range rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[1, col_cnt]];
                rg1.Font.Size = 11;
                rg1.Font.Bold = true;
                //Border Line Style
                rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[1, col_cnt]];
                rg1.RowHeight = 18;
                rg1.ColumnWidth = 15;
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
                messageBox.Show(this, ex.Message.ToString() + "\n 생성 중 에러가 발생하였습니다.");
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

        #region Method

        public void SetGroupCompany()
        {
            messageBox.Show(this, "대표거래처 설정이 완료되었습니다. '데이터 최신화' 버튼을 통해 최신화 해주시기 바랍니다.");
        }

        private void SetHeaderStyleSetting()
        {
            DataGridView dgv = dgvCompany;
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.Columns["company"].ReadOnly = true;


            dgv.Columns["balance_amount"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            dgv.Columns["balance_amount"].DefaultCellStyle.Font = new System.Drawing.Font("중고딕", 9, FontStyle.Bold);

            dgv.Columns["average_balance_amount"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            dgv.Columns["average_balance_amount"].DefaultCellStyle.Font = new System.Drawing.Font("중고딕", 9, FontStyle.Bold);

            dgv.Columns["maximum_balance_amount"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            dgv.Columns["maximum_balance_amount"].DefaultCellStyle.Font = new System.Drawing.Font("중고딕", 9, FontStyle.Bold);

            dgv.Columns["pre1_maximum_balance_amount"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            dgv.Columns["pre1_maximum_balance_amount"].DefaultCellStyle.Font = new System.Drawing.Font("중고딕", 9, FontStyle.Bold);

            dgv.Columns["pre2_maximum_balance_amount"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            dgv.Columns["pre2_maximum_balance_amount"].DefaultCellStyle.Font = new System.Drawing.Font("중고딕", 9, FontStyle.Bold);


            dgv.Columns["break_even_balance_amount"].DefaultCellStyle.BackColor = Color.FromArgb(233, 237, 247);
            dgv.Columns["break_even_balance_amount"].DefaultCellStyle.ForeColor = Color.Blue;
            dgv.Columns["break_even_balance_amount"].DefaultCellStyle.Font = new System.Drawing.Font("중고딕", 9, FontStyle.Bold);
            dgv.Columns["principal_payback_period"].DefaultCellStyle.Font = new System.Drawing.Font("중고딕", 9, FontStyle.Bold);


            ((DataGridViewImageColumn)dgv.Columns["img"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
            dgv.Columns["img"].DefaultCellStyle.NullValue = null;
        }
        private void GetPartner()
        {
            if (companyDt == null)
                return;
            //초기화
            //this.dgvCompany.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            dgvCompany.Rows.Clear();
            //평균미수 반영기간
            DateTime sttdate, enddate;
            if (rbOneMonth.Checked)
            {
                sttdate = DateTime.Now.AddMonths(-1).AddDays(-1);
                enddate = DateTime.Now.AddDays(-1);
            }
            else if (rbTwoMonths.Checked)
            {
                sttdate = DateTime.Now.AddMonths(-2).AddDays(-1);
                enddate = DateTime.Now.AddDays(-1);
            }
            else
            {
                sttdate = DateTime.Now.AddMonths(-3).AddDays(-1);
                enddate = DateTime.Now.AddDays(-1);
            }

            //Get Data 
            //DataTable companyDt = recoveryPrincipalPriceRepository.GetRecoveryPrincipalTemp(sttdate, enddate, txtCompany.Text.Trim(), cbExistBalance.Checked, cbExcluedLitigation.Checked);
            //companyDt = RefreshDatatable();
            if (companyDt.Rows.Count > 0)
            {
                DataTable resultDt = companyDt.Copy();
                //거래처검색
                string whr = "";
                if (!string.IsNullOrEmpty(txtCompany.Text.Trim()))
                {
                    string[] companys = txtCompany.Text.Trim().Split(' ');
                    string temp = "";
                    foreach (string company in companys)
                    {
                        if (!string.IsNullOrEmpty(company))
                        {
                            if (string.IsNullOrEmpty(temp))
                                temp = $"company_code LIKE '%{company}%'";
                            else
                                temp += $" OR company_code LIKE '%{company}%'";
                        }
                    }
                    whr += "(" + temp + ")";
                }
                //담당자
                if (!string.IsNullOrEmpty(txtManager.Text.Trim()))
                {
                    string[] managers = txtManager.Text.Trim().Split(' ');
                    string temp = "";
                    foreach (string manager in managers)
                    {
                        if (!string.IsNullOrEmpty(manager))
                        {
                            if (string.IsNullOrEmpty(temp))
                                temp = $"담당자 LIKE '%{manager}%'";
                            else
                                temp += $" OR 담당자 LIKE '%{manager}%'";
                        }
                    }
                    whr += "(" + temp + ")";
                }

                //소송여부
                if (rbLawsuitFalse.Checked)
                {
                    if (string.IsNullOrEmpty(whr))
                        whr = "거래처명 NOT LIKE '%소송%' AND 거래처명 NOT LIKE '%(s)%' AND 거래처명 NOT LIKE '%(S)%'";
                    else
                        whr += " AND 거래처명 NOT LIKE '%소송%' AND 거래처명 NOT LIKE '%(s)%' AND 거래처명 NOT LIKE '%(S)%'";
                }
                if (rbLawsuitTrue.Checked)
                {
                    if (string.IsNullOrEmpty(whr))
                        whr = "(거래처명 LIKE '%소송%' OR 거래처명 LIKE '%(s)%' OR 거래처명 LIKE '%(S)%')";
                    else
                        whr += " AND (거래처명 LIKE '%소송%' OR 거래처명 LIKE '%(s)%' OR 거래처명 LIKE '%(S)%')";
                }
                //미수여부
                if (rbExistBalance.Checked)
                {
                    if (string.IsNullOrEmpty(whr))
                        whr = "현재미수잔액 > 0";
                    else
                        whr += " AND 현재미수잔액 > 0";
                }
                //원금회수율
                if (rbOutRecoveryOver.Checked)
                {
                    if (string.IsNullOrEmpty(whr))
                        whr = "손익분기잔액 <= 0";
                    else
                        whr += " AND 손익분기잔액 <= 0";
                }
                else if (rbOutRecoveryNotOver.Checked)
                {
                    if (string.IsNullOrEmpty(whr))
                        whr = "손익분기잔액 > 0";
                    else
                        whr += " AND 손익분기잔액 > 0";
                }

                //Select
                DataRow[] resultDr = companyDt.Select(whr);
                if (resultDr.Length == 0)
                    return;
                else
                    resultDt = resultDr.CopyToDataTable();
                //Other Info
                DataTable otherInfoDt = companyRepository.GetCompanyRecovery("", txtCompany.Text.Trim(), false, "");
                //어제 데이터
                DataTable yesterdayDt = companyRecoveryRepository.GetCompanyRecoveryTemp(DateTime.Now.AddDays(-1));
                for (int i = 0; i < resultDt.Rows.Count; i++)
                {
                    bool isOutput = true;
                    //매출금액
                    double sales_amount = Convert.ToDouble(resultDt.Rows[i]["당월매출금액"].ToString());
                    double pre1_sales_amount = Convert.ToDouble(resultDt.Rows[i]["전월매출금액"].ToString());
                    double pre2_sales_amount = Convert.ToDouble(resultDt.Rows[i]["전전월매출금액"].ToString());
                    double pre3_sales_amount = Convert.ToDouble(resultDt.Rows[i]["전전전월매출금액"].ToString());
                    double pre_accumulate_sales_amount = Convert.ToDouble(resultDt.Rows[i]["누적매출금액"].ToString());
                    double accumulate_sales_amount = pre_accumulate_sales_amount + sales_amount;
                    double limit_balance_amount = Convert.ToDouble(resultDt.Rows[i]["매출한도금액"].ToString());

                    //미수금액
                    double balance_amount = Convert.ToDouble(resultDt.Rows[i]["현재미수잔액"].ToString());
                    double average_balance_amount = Convert.ToDouble(resultDt.Rows[i]["평균미수"].ToString());
                    double maximum_balance_amount = Convert.ToDouble(resultDt.Rows[i]["당월최고미수"].ToString());
                    double pre1_maximum_balance_amount = Convert.ToDouble(resultDt.Rows[i]["전월최고미수"].ToString());
                    double pre2_maximum_balance_amount = Convert.ToDouble(resultDt.Rows[i]["전전월최고미수"].ToString());

                    //마진금액
                    double margin_amount = Convert.ToDouble(resultDt.Rows[i]["당월마진금액"].ToString());
                    double pre_accumulate_margin_amount = Convert.ToDouble(resultDt.Rows[i]["누적마진금액"].ToString());
                    double accumulate_margin_amount = pre_accumulate_margin_amount + margin_amount;

                    //아토 기회비용
                    bool isManage = false;
                    bool isManage2 = false;
                    bool isManage3 = false;
                    bool isManage4 = false;
                    bool isHide = false;
                    bool isInsert = false;
                    whr = "seaover_company_code = '" + resultDt.Rows[i]["거래처코드"].ToString() + "'"
                                + " AND seaover_company_code <> ''";
                    DataRow[] dr = otherInfoDt.Select(whr);
                    if (dr.Length > 0)
                    {
                        isManage = Convert.ToBoolean(dr[0]["isManagement1"].ToString());
                        isManage2 = Convert.ToBoolean(dr[0]["isManagement2"].ToString());
                        isManage3 = Convert.ToBoolean(dr[0]["isManagement3"].ToString());
                        isManage4 = Convert.ToBoolean(dr[0]["isManagement4"].ToString());
                        isHide = Convert.ToBoolean(dr[0]["isHide"].ToString());
                        isInsert = true;
                    }

                    //탭별검색
                    if (tcMain.SelectedTab.Name == "tabCompany")
                    {
                        if (isOutput && isHide)
                            isOutput = false;
                    }
                    else if (tcMain.SelectedTab.Name == "tabHide")
                        if (isOutput && !isHide)
                            isOutput = false;

                    if (isOutput)
                    {
                        //손익분기잔액1
                        double margin_price1 = balance_amount - pre_accumulate_margin_amount;
                        //원금회수기간1
                        double principal_recovery_month1 = 0;
                        if (margin_price1 > 0)
                        {
                            if (rbOneMonth.Checked)
                                principal_recovery_month1 = margin_price1 / margin_amount;
                            else if (rbTwoMonths.Checked)
                                principal_recovery_month1 = margin_price1 / margin_amount * 2;
                            else
                                principal_recovery_month1 = margin_price1 / margin_amount * 3;
                        }
                        if (double.IsNaN(principal_recovery_month1))  //Nan
                            principal_recovery_month1 = 0;
                        else if (double.IsInfinity(principal_recovery_month1))  //Infinity
                            principal_recovery_month1 = 9999;
                        else if (principal_recovery_month1 > 9999)  //Max
                            principal_recovery_month1 = 9999;

                        //매출채권회전수
                        double rotation_current = sales_amount / balance_amount;                  //당월
                        double rotation_average = sales_amount / average_balance_amount;          //평균
                        double rotation_maximum = sales_amount / maximum_balance_amount;          //당월최고
                        double pre1_rotation_maximum = sales_amount / pre1_maximum_balance_amount;          //전월최고
                        double pre2_rotation_maximum = sales_amount / pre2_maximum_balance_amount;          //전전월최고
                                                                                                            //매출채권회전일수(당월)
                        double sales_terms = rbOneMonth.Checked ? 30.0 : rbTwoMonths.Checked ? 60.0 : 90.0;
                        double rotation_current_days = sales_terms / rotation_current;
                        double rotation_average_days = sales_terms / rotation_average;
                        double rotation_maximum_days = sales_terms / rotation_maximum;
                        double pre1_rotation_maximum_days = sales_terms / pre1_rotation_maximum;
                        double pre2_rotation_maximum_days = sales_terms / pre2_rotation_maximum;

                        //마진(당월)
                        double month_margin = (margin_amount / sales_amount * 100);
                        if (double.IsNaN(month_margin))
                            month_margin = 0;
                        //마진(평균)
                        double month_margin_total_average = accumulate_margin_amount / accumulate_sales_amount * 100;
                        if (double.IsNaN(month_margin_total_average))
                            month_margin_total_average = 0;

                        //매출채권회전수/마진율(당월)
                        double rotation_current_days_division_margin = (rotation_current_days / month_margin);
                        double rotation_average_days_division_margin = (rotation_average_days / month_margin);
                        double rotation_maximum_days_division_margin = (rotation_maximum_days / month_margin);
                        double pre1_rotation_maximum_days_division_margin = (pre1_rotation_maximum_days / month_margin);
                        double pre2_rotation_maximum_days_division_margin = (pre2_rotation_maximum_days / month_margin);

                        //고급필터================================================================================================
                        //당월 회전일수/마진율
                        if (cbDivisionRotation.Checked)
                        {
                            double currentDivRotation = (double)nudSttDr.Value;
                            switch (cbDivisionRotationType.Text)
                            {
                                case "이상":
                                    if (rotation_current_days_division_margin < currentDivRotation)
                                        isOutput = false;
                                    break;
                                case "이하":
                                    if (rotation_current_days_division_margin > currentDivRotation)
                                        isOutput = false;
                                    break;
                            }
                        }
                        //당월 매출채권일수
                        if (cbRotation.Checked)
                        {
                            double currentRotation = (double)nudSttRotation.Value;
                            switch (cbRotationType.Text)
                            {
                                case "이상":
                                    if (rotation_current_days < currentRotation)
                                        isOutput = false;
                                    break;
                                case "이하":
                                    if (rotation_current_days > currentRotation)
                                        isOutput = false;
                                    break;
                            }
                        }



                        //평균 회전일수/마진율
                        if (cbDivisionRotationAverage.Checked)
                        {
                            double averageDivRotation = (double)nudSttDrAverage.Value;
                            switch (cbDivisionRotationAverageType.Text)
                            {
                                case "이상":
                                    if (rotation_average_days_division_margin < averageDivRotation)
                                        isOutput = false;
                                    break;
                                case "이하":
                                    if (rotation_average_days_division_margin > averageDivRotation)
                                        isOutput = false;
                                    break;
                            }
                        }
                        //평균 매출채권일수
                        if (cbRotationAverage.Checked)
                        {
                            double averageRotation = (double)nudSttRotationAverage.Value;
                            switch (cbRotationAverageType.Text)
                            {
                                case "이상":
                                    if (rotation_average_days < averageRotation)
                                        isOutput = false;
                                    break;
                                case "이하":
                                    if (rotation_average_days > averageRotation)
                                        isOutput = false;
                                    break;
                            }
                        }

                        //당월최고 회전일수/마진율
                        if (cbDivisionRotationMaximum.Checked)
                        {
                            double maximumDivRotation = (double)nudSttDrMaximum.Value;
                            switch (cbDivisionRotationMaximumType.Text)
                            {
                                case "이상":
                                    if (rotation_maximum_days_division_margin < maximumDivRotation)
                                        isOutput = false;
                                    break;
                                case "이하":
                                    if (rotation_maximum_days_division_margin > maximumDivRotation)
                                        isOutput = false;
                                    break;
                            }
                        }
                        //당월최고 매출채권일수
                        if (cbRotationMaximum.Checked)
                        {
                            double averageMaximum = (double)nudSttRotationMaximum.Value;
                            switch (cbRotationMaximumType.Text)
                            {
                                case "이상":
                                    if (rotation_maximum_days < averageMaximum)
                                        isOutput = false;
                                    break;
                                case "이하":
                                    if (rotation_maximum_days > averageMaximum)
                                        isOutput = false;
                                    break;
                            }
                        }


                        //출력====================================================================================================
                        if (isOutput)
                        {
                            int n = dgvCompany.Rows.Add();

                            dgvCompany.Rows[n].Cells["main_id"].Value = resultDt.Rows[i]["main_id"].ToString();
                            dgvCompany.Rows[n].Cells["sub_id"].Value = resultDt.Rows[i]["sub_id"].ToString();
                            dgvCompany.Rows[n].Cells["group_code"].Value = resultDt.Rows[i]["group_code"].ToString();
                            dgvCompany.Rows[n].Cells["company_code"].Value = resultDt.Rows[i]["company_code"].ToString();

                            dgvCompany.Rows[n].Cells["seaover_company_code"].Value = resultDt.Rows[i]["거래처코드"].ToString();
                            dgvCompany.Rows[n].Cells["company"].Value = resultDt.Rows[i]["거래처명"].ToString();
                            dgvCompany.Rows[n].Cells["limit_balance_amount"].Value = limit_balance_amount.ToString("#,##0");

                            //매출금액
                            dgvCompany.Rows[n].Cells["sales_amount"].Value = sales_amount.ToString("#,##0");
                            /*dgvCompany.Rows[n].Cells["pre1_sales_amount"].Value = pre1_sales_amount.ToString("#,##0");
                            dgvCompany.Rows[n].Cells["pre2_sales_amount"].Value = pre2_sales_amount.ToString("#,##0");
                            dgvCompany.Rows[n].Cells["pre3_sales_amount"].Value = pre3_sales_amount.ToString("#,##0");*/
                            dgvCompany.Rows[n].Cells["cumulative_sales_amount"].Value = accumulate_sales_amount.ToString("#,##0");
                            //미수금액
                            dgvCompany.Rows[n].Cells["balance_amount"].Value = balance_amount.ToString("#,##0");
                            dgvCompany.Rows[n].Cells["average_balance_amount"].Value = average_balance_amount.ToString("#,##0");
                            dgvCompany.Rows[n].Cells["maximum_balance_amount"].Value = maximum_balance_amount.ToString("#,##0");
                            dgvCompany.Rows[n].Cells["pre1_maximum_balance_amount"].Value = pre1_maximum_balance_amount.ToString("#,##0");
                            dgvCompany.Rows[n].Cells["pre2_maximum_balance_amount"].Value = pre2_maximum_balance_amount.ToString("#,##0");
                            //마진
                            dgvCompany.Rows[n].Cells["month_margin_amount"].Value = margin_amount.ToString("#,##0");
                            dgvCompany.Rows[n].Cells["cumulative_margin_amount"].Value = accumulate_margin_amount.ToString("#,##0");
                            dgvCompany.Rows[n].Cells["margin_rate"].Value = month_margin.ToString("#,##0.00");
                            dgvCompany.Rows[n].Cells["total_margin_rate"].Value = month_margin_total_average.ToString("#,##0.00");

                            //매출채권회전수
                            dgvCompany.Rows[n].Cells["rotation_current"].Value = rotation_current.ToString("#,##0.00");
                            dgvCompany.Rows[n].Cells["rotation_average"].Value = rotation_average.ToString("#,##0.00");
                            dgvCompany.Rows[n].Cells["rotation_maximum"].Value = rotation_maximum.ToString("#,##0.00");
                            dgvCompany.Rows[n].Cells["pre1_rotation_maximum"].Value = pre1_rotation_maximum.ToString("#,##0.00");
                            dgvCompany.Rows[n].Cells["pre2_rotation_maximum"].Value = pre2_rotation_maximum.ToString("#,##0.00");

                            //매출채권회전일수
                            dgvCompany.Rows[n].Cells["rotation_current_days"].Value = rotation_current_days.ToString("#,##0");
                            dgvCompany.Rows[n].Cells["rotation_average_days"].Value = rotation_average_days.ToString("#,##0");
                            dgvCompany.Rows[n].Cells["rotation_maximum_days"].Value = rotation_maximum_days.ToString("#,##0");
                            dgvCompany.Rows[n].Cells["pre1_rotation_maximum_days"].Value = pre1_rotation_maximum_days.ToString("#,##0");
                            dgvCompany.Rows[n].Cells["pre2_rotation_maximum_days"].Value = pre2_rotation_maximum_days.ToString("#,##0");

                            //손익분기잔액, 원금회수율
                            dgvCompany.Rows[n].Cells["break_even_balance_amount"].Value = margin_price1.ToString("#,##0");
                            dgvCompany.Rows[n].Cells["break_even_balance_amount"].ToolTipText = "미수금 - 전월 누적마진금액";
                            if (principal_recovery_month1 == 9999)
                                dgvCompany.Rows[n].Cells["principal_payback_period"].Value = "∞ 개월";
                            else
                                dgvCompany.Rows[n].Cells["principal_payback_period"].Value = principal_recovery_month1.ToString("#,##0") + "개월";

                            //회전일수 / 마진율
                            dgvCompany.Rows[n].Cells["rotation_current_division_margin"].Value = rotation_current_days_division_margin.ToString("#,##0.00");
                            dgvCompany.Rows[n].Cells["rotation_average_division_margin"].Value = rotation_average_days_division_margin.ToString("#,##0.00");
                            dgvCompany.Rows[n].Cells["rotation_maximum_division_margin"].Value = rotation_maximum_days_division_margin.ToString("#,##0.00");
                            dgvCompany.Rows[n].Cells["pre1_rotation_maximum_division_margin"].Value = pre1_rotation_maximum_days_division_margin.ToString("#,##0.00");
                            dgvCompany.Rows[n].Cells["pre2_rotation_maximum_division_margin"].Value = pre2_rotation_maximum_days_division_margin.ToString("#,##0.00");


                            if (!string.IsNullOrEmpty(companyDt.Rows[i]["최근매출일자"].ToString()))
                                dgvCompany.Rows[n].Cells["current_sales_updatetime"].Value = Convert.ToDateTime(resultDt.Rows[i]["최근매출일자"].ToString()).ToString("yyyy-MM-dd");
                            if (!string.IsNullOrEmpty(companyDt.Rows[i]["최근입금일자"].ToString()))
                                dgvCompany.Rows[n].Cells["current_payment_updatetime"].Value = Convert.ToDateTime(resultDt.Rows[i]["최근입금일자"].ToString()).ToString("yyyy-MM-dd");

                            dgvCompany.Rows[n].Cells["manager"].Value = resultDt.Rows[i]["담당자"].ToString();

                            //관심업체
                            dgvCompany.Rows[n].Cells["is_manage1"].Value = isManage.ToString();
                            dgvCompany.Rows[n].Cells["is_manage2"].Value = isManage2.ToString();
                            dgvCompany.Rows[n].Cells["is_manage3"].Value = isManage3.ToString();
                            dgvCompany.Rows[n].Cells["is_manage4"].Value = isManage4.ToString();
                            if (isManage)
                                dgvCompany.Rows[n].Cells["img"].Value = Properties.Resources.Star_icon1;
                            else if (isManage2)
                                dgvCompany.Rows[n].Cells["img"].Value = Properties.Resources.Star_icon2;
                            else if (isManage3)
                                dgvCompany.Rows[n].Cells["img"].Value = Properties.Resources.Star_icon3;
                            else if (isManage4)
                                dgvCompany.Rows[n].Cells["img"].Value = Properties.Resources.Star_icon4;
                            else
                                dgvCompany.Rows[n].Cells["img"].Value = null;

                            dgvCompany.Rows[n].Cells["table_index"].Value = resultDt.Rows[i]["table_index"].ToString();
                            dgvCompany.Rows[n].Cells["is_insert"].Value = isInsert;
                            dgvCompany.Rows[n].Cells["is_new"].Value = Convert.ToBoolean(resultDt.Rows[i]["is_new"].ToString());
                        }
                    }
                    //calculate();
                    
                }
                SetManagementImg();
            }
            //this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            //SetManagementImg();
        }

        private DataTable RefreshDatatable()
        {
            //평균미수 반영기간
            DateTime sttdate, enddate;
            if (rbOneMonth.Checked)
            {
                sttdate = DateTime.Now.AddMonths(-1);
                enddate = DateTime.Now;
            }
            else if (rbTwoMonths.Checked)
            {
                sttdate = DateTime.Now.AddMonths(-2);
                enddate = DateTime.Now;
            }
            else
            {
                sttdate = DateTime.Now.AddMonths(-3);
                enddate = DateTime.Now;
            }
            DataTable companyDt = recoveryPrincipalPriceRepository.GetRecoveryPrincipalTemp(sttdate, enddate, "", false, false);
            DataTable groupDt = companyGroupRepository.GetCompanyGroup2();
            DataTable resultDt = null;
            if (companyDt != null && groupDt != null)
            {
                // Left Outer Join
                var leftOuterJoin =
                    from p in companyDt.AsEnumerable()
                    join t in groupDt.AsEnumerable()
                    on p.Field<string>("거래처코드") equals t.Field<string>("group_code") into joined
                    from t in joined.DefaultIfEmpty()
                    select new
                    {
                        거래처코드 = p.Field<string>("거래처코드"),
                        거래처명 = p.Field<string>("거래처명"),
                        매출한도금액 = p.Field<double>("매출한도금액"),
                        현재미수잔액 = p.Field<double>("현재미수잔액"),
                        당월매출금액 = p.Field<double>("당월매출금액"),
                        전월매출금액 = p.Field<double>("전월매출금액"),
                        전전월매출금액 = p.Field<double>("전전월매출금액"),
                        전전전월매출금액 = p.Field<double>("전전전월매출금액"),
                        누적매출금액 = p.Field<double>("누적매출금액"),
                        당월마진금액 = p.Field<double>("당월마진금액"),
                        누적마진금액 = p.Field<double>("누적마진금액"),
                        당월최고미수 = p.Field<double>("당월최고미수"),
                        전월최고미수 = p.Field<double>("전월최고미수"),
                        전전월최고미수 = p.Field<double>("전전월최고미수"),
                        평균미수 = p.Field<double>("평균미수"),
                        당월입금금액 = p.Field<double>("당월입금금액"),
                        손익분기잔액 = p.Field<double>("손익분기잔액"),
                        최근매출일자 = p.Field<string>("최근매출일자"),
                        최근입금일자 = p.Field<string>("최근입금일자"),
                        담당자 = p.Field<string>("매출자"),
                        group_code = (t == null) ? p.Field<string>("거래처코드") : t.Field<string>("group_code"),
                        company_code = (t == null) ? p.Field<string>("거래처명") : t.Field<string>("company_code"),
                        main_id = (t == null) ? "0" : t.Field<string>("main_id"),
                        sub_id = (t == null) ? "0" : t.Field<string>("sub_id"),
                        table_index = 0
                    };

                // Right Outer Join
                var rightOuterJoin =
                    from t in groupDt.AsEnumerable()
                    join p in companyDt.AsEnumerable()
                    on t.Field<string>("group_code") equals p.Field<string>("거래처코드") into joined
                    from p in joined.DefaultIfEmpty()
                    where p == null // Left outer join에 대응하지 않는 row2를 선택
                    select new
                    {
                        거래처코드 = (p == null) ? t.Field<string>("company_id") : p.Field<string>("거래처코드"),
                        거래처명 = (p == null) ? t.Field<string>("company") : p.Field<string>("거래처명"),
                        매출한도금액 = (p == null) ? 0 : p.Field<double>("매출한도금액"),
                        현재미수잔액 = (p == null) ? 0 : p.Field<double>("현재미수잔액"),
                        당월매출금액 = (p == null) ? 0 : p.Field<double>("당월매출금액"),
                        전월매출금액 = (p == null) ? 0 : p.Field<double>("전월매출금액"),
                        전전월매출금액 = (p == null) ? 0 : p.Field<double>("전전월매출금액"),
                        전전전월매출금액 = (p == null) ? 0 : p.Field<double>("전전전월매출금액"),
                        누적매출금액 = (p == null) ? 0 : p.Field<double>("누적매출금액"),
                        당월마진금액 = (p == null) ? 0 : p.Field<double>("당월마진금액"),
                        누적마진금액 = (p == null) ? 0 : p.Field<double>("누적마진금액"),
                        당월최고미수 = (p == null) ? 0 : p.Field<double>("당월최고미수"),
                        전월최고미수 = (p == null) ? 0 : p.Field<double>("전월최고미수"),
                        전전월최고미수 = (p == null) ? 0 : p.Field<double>("전전월최고미수"),
                        평균미수 = (p == null) ? 0 : p.Field<double>("평균미수"),
                        당월입금금액 = (p == null) ? 0 : p.Field<double>("당월입금금액"),
                        손익분기잔액 = (p == null) ? 0 : p.Field<double>("손익분기잔액"),
                        최근매출일자 = (p == null) ? "" : p.Field<string>("최근매출일자"),
                        최근입금일자 = (p == null) ? "" : p.Field<string>("최근입금일자"),
                        담당자 = (p == null) ? "" : p.Field<string>("매출자"),
                        group_code = t.Field<string>("group_code"),
                        company_code = t.Field<string>("company_code"),
                        main_id = t.Field<string>("main_id"),
                        sub_id = t.Field<string>("sub_id"),
                        table_index = 0
                    };
                // Full Outer Join을 위한 Union
                var fullOuterJoinQuery = leftOuterJoin.Union(rightOuterJoin);
                resultDt = ConvertListToDatatable(fullOuterJoinQuery);
                if (resultDt != null)
                    resultDt.AcceptChanges();


                int table_index = 0;
                List<string> codeList = new List<string>();
                foreach (DataRow row in resultDt.Rows)
                {
                    int main_id = Convert.ToInt32(row["main_id"].ToString());
                    int sub_id = Convert.ToInt32(row["sub_id"].ToString());
                    string company_code = row["거래처코드"].ToString();
                    if (!codeList.Contains(company_code) && main_id > 0 && sub_id == 9999)
                    {
                        double sales_amount = 0, pre1_sales_amount = 0, pre2_sales_amount = 0, pre3_sales_amount = 0, total_sales_amount = 0
                            , margin_amount = 0, total_margin_amount = 0, limit_sales_amount = 0
                            , balance_amount = 0, average_balance_amount = 0, pre0_maximum_balance_amount = 0, pre1_maximum_balance_amount = 0, pre2_maximum_balance_amount = 0
                            , payemnt_amount = 0, break_even_balance = 0;
                        string manager = "";

                        DateTime max_sale_date = new DateTime(1900, 1, 1), max_payment_date = new DateTime(1900, 1, 1);
                        DataRow[] subList = resultDt.Select($"main_id = '{main_id}' AND sub_id < 9999");
                        if (subList.Length > 0)
                        {
                            foreach (DataRow subDr in subList)
                            {
                                sales_amount += Convert.ToDouble(subDr["당월매출금액"].ToString());
                                pre1_sales_amount += Convert.ToDouble(subDr["전월매출금액"].ToString());
                                pre2_sales_amount += Convert.ToDouble(subDr["전전월매출금액"].ToString());
                                pre3_sales_amount += Convert.ToDouble(subDr["전전전월매출금액"].ToString());

                                total_sales_amount += Convert.ToDouble(subDr["누적매출금액"].ToString());

                                margin_amount += Convert.ToDouble(subDr["당월마진금액"].ToString());
                                total_margin_amount += Convert.ToDouble(subDr["누적마진금액"].ToString());

                                limit_sales_amount += Convert.ToDouble(subDr["매출한도금액"].ToString());

                                balance_amount += Convert.ToDouble(subDr["현재미수잔액"].ToString());
                                average_balance_amount += Convert.ToDouble(subDr["평균미수"].ToString());

                                pre0_maximum_balance_amount += Convert.ToDouble(subDr["당월최고미수"].ToString());
                                pre1_maximum_balance_amount += Convert.ToDouble(subDr["전월최고미수"].ToString());
                                pre2_maximum_balance_amount += Convert.ToDouble(subDr["전전월최고미수"].ToString());

                                payemnt_amount += Convert.ToDouble(subDr["당월입금금액"].ToString());
                                break_even_balance += Convert.ToDouble(subDr["손익분기잔액"].ToString());

                                DateTime temp_sale_date;
                                if (!DateTime.TryParse(subDr["최근매출일자"].ToString(), out temp_sale_date))
                                    temp_sale_date = new DateTime(1900, 1, 1);
                                if (max_sale_date < temp_sale_date)
                                {
                                    max_sale_date = temp_sale_date;
                                    if(!string.IsNullOrEmpty(subDr["담당자"].ToString()))
                                        manager = subDr["담당자"].ToString();
                                }

                                DateTime temp_payment_date;
                                if (!DateTime.TryParse(subDr["최근입금일자"].ToString(), out temp_payment_date))
                                    temp_payment_date = new DateTime(1900, 1, 1);
                                if (max_payment_date < temp_payment_date)
                                    max_payment_date = temp_payment_date;
                            }
                        }


                        row["매출한도금액"] = limit_sales_amount;

                        row["당월매출금액"] = sales_amount;
                        row["전월매출금액"] = pre1_sales_amount;
                        row["전전월매출금액"] = pre2_sales_amount;
                        row["전전전월매출금액"] = pre3_sales_amount;
                        row["누적매출금액"] = total_sales_amount;

                        row["당월마진금액"] = margin_amount;
                        row["누적마진금액"] = total_margin_amount;

                        row["현재미수잔액"] = balance_amount;
                        row["평균미수"] = average_balance_amount;
                        row["당월최고미수"] = pre0_maximum_balance_amount;
                        row["전월최고미수"] = pre1_maximum_balance_amount;
                        row["전전월최고미수"] = pre2_maximum_balance_amount;

                        row["당월입금금액"] = payemnt_amount;
                        row["손익분기잔액"] = break_even_balance;

                        row["최근매출일자"] = max_sale_date.ToString("yyyy-MM-dd HH:mm:ss");
                        row["최근입금일자"] = max_payment_date.ToString("yyyy-MM-dd HH:mm:ss");

                        row["담당자"] = manager;
                    }
                    row["table_index"] = table_index++;
                }
            }


            DataTable yesterdayDt = companyRecoveryRepository.GetCompanyRecoveryTemp(DateTime.Now.AddDays(-1));
            if (resultDt != null && yesterdayDt != null)
            {
                // LINQ를 사용하여 어제 기준 새로나온 품목만 정의
                var query = from row1 in resultDt.AsEnumerable()
                            join row2 in yesterdayDt.AsEnumerable() on row1.Field<string>("group_code") equals row2.Field<string>("group_code") into gj
                            from subRow in gj.DefaultIfEmpty()
                            select new
                            {
                                is_new = subRow == null ? "TRUE" : "FALSE",
                                거래처명 = row1.Field<string>("거래처명"),
                                거래처코드 = row1.Field<string>("거래처코드"),
                                매출한도금액 = row1.Field<double>("매출한도금액"),
                                현재미수잔액 = row1.Field<double>("현재미수잔액"),
                                당월매출금액 = row1.Field<double>("당월매출금액"),
                                전월매출금액 = row1.Field<double>("전월매출금액"),
                                전전월매출금액 = row1.Field<double>("전전월매출금액"),
                                전전전월매출금액 = row1.Field<double>("전전전월매출금액"),
                                누적매출금액 = row1.Field<double>("누적매출금액"),
                                당월마진금액 = row1.Field<double>("당월마진금액"),
                                누적마진금액 = row1.Field<double>("누적마진금액"),
                                당월최고미수 = row1.Field<double>("당월최고미수"),
                                전월최고미수 = row1.Field<double>("전월최고미수"),
                                전전월최고미수 = row1.Field<double>("전전월최고미수"),
                                평균미수 = row1.Field<double>("평균미수"),
                                당월입금금액 = row1.Field<double>("당월입금금액"),
                                손익분기잔액 = row1.Field<double>("손익분기잔액"),
                                최근매출일자 = row1.Field<string>("최근매출일자"),
                                최근입금일자 = row1.Field<string>("최근입금일자"),
                                담당자 = row1.Field<string>("담당자"),
                                group_code = row1.Field<string>("group_code"),
                                company_code = row1.Field<string>("company_code"),
                                main_id = row1.Field<string>("main_id"),
                                sub_id = row1.Field<string>("sub_id"),
                                table_index = row1.Field<int>("table_index")
                            };
                if (query != null)
                    resultDt = ConvertListToDatatable(query);
            }

            return resultDt;
        }
        //List => Datatable
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
        private void SetManagementImg()
        {
            //this.dgvCompany.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow row = dgvCompany.Rows[i];

                int main_id;
                if (row.Cells["main_id"].Value == null || !int.TryParse(row.Cells["main_id"].Value.ToString(), out main_id))
                    main_id = 0;
                int sub_id;
                if (row.Cells["sub_id"].Value == null || !int.TryParse(row.Cells["sub_id"].Value.ToString(), out sub_id))
                    sub_id = 0;

                if (main_id > 0 && sub_id < 9999)
                    row.Visible = false;
                else if (main_id > 0 && sub_id == 9999)
                {
                    row.HeaderCell.Value = "+";
                    row.HeaderCell.Style.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                    row.HeaderCell.Style.ForeColor = Color.Red;
                    row.DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                }

                if (Convert.ToBoolean(row.Cells["is_new"].Value))
                {
                    double rotation_current_division_margin;
                    if (row.Cells["rotation_current_division_margin"].Value == null || !double.TryParse(row.Cells["rotation_current_division_margin"].Value.ToString(), out rotation_current_division_margin))
                        rotation_current_division_margin = 0;
                    double rotation_average_division_margin;
                    if (row.Cells["rotation_average_division_margin"].Value == null || !double.TryParse(row.Cells["rotation_average_division_margin"].Value.ToString(), out rotation_average_division_margin))
                        rotation_average_division_margin = 0;
                    double rotation_maximum_division_margin;
                    if (row.Cells["rotation_maximum_division_margin"].Value == null || !double.TryParse(row.Cells["rotation_maximum_division_margin"].Value.ToString(), out rotation_maximum_division_margin))
                        rotation_maximum_division_margin = 0;

                    if (rotation_current_division_margin > 7 || rotation_average_division_margin > 7 || rotation_maximum_division_margin > 7)
                    {
                        row.HeaderCell.Value = "NEW";
                        row.HeaderCell.Style.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                        row.HeaderCell.Style.ForeColor = Color.Red;
                    }
                }

            }
            //this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
        }
        #endregion

        #region Key event
        private void txtCompany_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    btnSearching.PerformClick();
                    break;
            }
        }
        private void SalesPartnerManager2_KeyDown(object sender, KeyEventArgs e)
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
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.E:
                        btnExcel.PerformClick();
                        break;
                }
            }
        }

        #endregion

        #region 우클릭 메뉴
        private void dgvCompany_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                RecoveryPrincipalmanager2.RecoveryPrincipalManager rpm = new RecoveryPrincipalmanager2.RecoveryPrincipalManager(um
                                                                            , dgvCompany.Rows[e.RowIndex].Cells["group_code"].Value.ToString()
                                                                            , dgvCompany.Rows[e.RowIndex].Cells["company_code"].Value.ToString());

                rpm.Owner = this;
                rpm.Show();
                rpm.GetData();
            }
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
                    m.Items.Add("대표거래처 설정");
                    m.Items.Add("대표거래처 취소");
                    if (tcMain.SelectedTab.Name == "tabCompany")
                        m.Items.Add("거래처 숨기기");
                    else if (tcMain.SelectedTab.Name == "tabHide")
                        m.Items.Add("거래처 숨기기 해제");
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

                switch (e.ClickedItem.Text)
                {
                    case "대표거래처 설정":
                        if (dgvCompany.SelectedRows.Count > 0)
                        {
                            List<DataGridViewRow> list = new List<DataGridViewRow>();
                            for (int i = 0; i < dgvCompany.Rows.Count; i++)
                            {
                                if (dgvCompany.Rows[i].Selected
                                    && dgvCompany.Rows[i].Cells["seaover_company_code"].Value != null
                                    && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["seaover_company_code"].Value.ToString().Trim()))
                                    list.Add(dgvCompany.Rows[i]);
                            }

                            if (list.Count > 0)
                            {
                                try
                                {
                                    SaleManagement.CompanyGroupManager cgm = new SaleManagement.CompanyGroupManager(um, this, list);
                                    cgm.ShowDialog();
                                }
                                catch
                                { }
                            }
                            else
                                messageBox.Show(this, "SEAOVER 거래처만 설정할 수 있습니다.");
                            this.Activate();
                        }
                        break;
                    case "대표거래처 취소":
                        {
                            if (messageBox.Show(this, "선택한 대표거래처 설정을 취소하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;

                            List<StringBuilder> sqlList = new List<StringBuilder>();
                            List<int> delete_main_id = new List<int>();
                            foreach (DataGridViewRow row in dgvCompany.SelectedRows)
                            {
                                int main_id = Convert.ToInt32(row.Cells["main_id"].Value);
                                if (main_id > 0)
                                {
                                    StringBuilder sql = companyGroupRepository.DeleteGroup(main_id);
                                    sqlList.Add(sql);

                                    delete_main_id.Add(main_id);
                                }
                            }

                            if (commonRepository.UpdateTran(sqlList) == -1)
                            {
                                messageBox.Show(this, "취소 중 에러가 발생하였습니다.");
                                this.Activate();
                            }
                            else
                            {

                                messageBox.Show(this, "대표업체 설정이 취소되었습니다. '데이터 최신화' 버튼을 통해 최신화 해주시기 바랍니다.");
                                this.Activate();
                            }
                        }
                        break;
                    case "거래처 숨기기":
                        {
                            if (messageBox.Show(this, "선택하신 거래처를 숨기시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;


                            int newId = commonRepository.GetNextId("t_company", "id");
                            List<StringBuilder> sqlList = new List<StringBuilder>();
                            foreach (DataGridViewRow selectRow in dgvCompany.SelectedRows)
                            {
                                if (selectRow.Cells["is_insert"].Value != null && bool.TryParse(selectRow.Cells["is_insert"].Value.ToString(), out bool is_insert) && is_insert)
                                {
                                    StringBuilder sql = commonRepository.UpdateData("t_company", "isHide = TRUE", $"seaover_company_code = '{selectRow.Cells["seaover_company_code"].Value.ToString()}'");
                                    sqlList.Add(sql);
                                }
                                else
                                {
                                    //거래처 기본정보 저장, 수정
                                    CompanyModel cm = new CompanyModel();
                                    DataTable atoCompanyDt = seaoverCompanyRepository.GetSeaoverCompanyInfo("", selectRow.Cells["seaover_company_code"].Value.ToString());
                                    if (atoCompanyDt.Rows.Count > 0)
                                    {
                                        cm.id = newId++;
                                        cm.division = "매출처";
                                        cm.registration_number = atoCompanyDt.Rows[0]["사업자번호"].ToString();
                                        cm.group_name = "";
                                        cm.name = atoCompanyDt.Rows[0]["거래처명"].ToString();
                                        cm.origin = "국내";
                                        cm.address = "";
                                        cm.ceo = atoCompanyDt.Rows[0]["대표자명"].ToString();
                                        cm.tel = atoCompanyDt.Rows[0]["전화번호"].ToString();
                                        cm.fax = atoCompanyDt.Rows[0]["팩스번호"].ToString();
                                        cm.phone = atoCompanyDt.Rows[0]["휴대폰"].ToString();
                                        cm.company_manager = "";
                                        cm.company_manager_position = "";
                                        cm.email = "";
                                        cm.sns1 = "";
                                        cm.sns2 = "";
                                        cm.sns3 = "";
                                        cm.web = "";
                                        cm.remark = atoCompanyDt.Rows[0]["참고사항"].ToString();
                                        cm.ato_manager = atoCompanyDt.Rows[0]["매출자"].ToString(); ;
                                        cm.isTrading = true;
                                        cm.isHide = true;
                                        cm.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                                        cm.createtime = DateTime.Now.ToString("yyyy-MM-dd");
                                        cm.edit_user = um.user_name;
                                        cm.seaover_company_code = atoCompanyDt.Rows[0]["거래처코드"].ToString();

                                        //Insert
                                        StringBuilder sql = companyRepository.InsertCompany(cm);
                                        sqlList.Add(sql);
                                    }
                                }
                            }

                            if (commonRepository.UpdateTran(sqlList) == -1)
                            {
                                messageBox.Show(this, "수정 중 에러가 발생하였습니다.");
                                this.Activate();
                            }
                            else
                            {
                                for (int i = dgvCompany.RowCount - 1; i >= 0; i--)
                                {
                                    if (dgvCompany.Rows[i].Selected)
                                        dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                }
                                this.Activate();
                            }

                        }
                        break;
                    case "거래처 숨기기 해제":
                        {
                            if (messageBox.Show(this, "선택하신 거래처를 숨기기 해제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;

                            List<StringBuilder> sqlList = new List<StringBuilder>();
                            foreach (DataGridViewRow selectRow in dgvCompany.SelectedRows)
                            {
                                StringBuilder sql = commonRepository.UpdateData("t_company", "isHide = FALSE", $"seaover_company_code = '{selectRow.Cells["seaover_company_code"].Value.ToString()}'");
                                sqlList.Add(sql);
                            }

                            if (commonRepository.UpdateTran(sqlList) == -1)
                            {
                                messageBox.Show(this, "수정 중 에러가 발생하였습니다.");
                                this.Activate();
                            }
                            else
                            {
                                for (int i = dgvCompany.RowCount - 1; i >= 0; i--)
                                {
                                    if (dgvCompany.Rows[i].Selected)
                                        dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                }
                                this.Activate();
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
                this.Activate();
            }

        }
        #endregion

        #region Datagridview event
        private void dgvCompany_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvCompany.Columns[e.ColumnIndex].Name.Equals("rotation_current_division_margin")
                    || dgvCompany.Columns[e.ColumnIndex].Name.Equals("rotation_average_division_margin")
                    || dgvCompany.Columns[e.ColumnIndex].Name.Equals("rotation_maximum_division_margin")
                    || dgvCompany.Columns[e.ColumnIndex].Name.Equals("pre1_rotation_maximum_division_margin")
                    || dgvCompany.Columns[e.ColumnIndex].Name.Equals("pre2_rotation_maximum_division_margin"))
                {
                    double tmpVal;
                    if (dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !double.TryParse(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out tmpVal))
                        tmpVal = 0;
                    if(tmpVal > 7)
                        dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
                    else
                        dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                }
            }
        }
        #endregion

        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            tcMain.SelectedTab.Controls.Add(this.dgvCompany);
            GetPartner();
        }

        private void dgvCompany_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = false;
            e.ThrowException = false;
        }
    }
}
