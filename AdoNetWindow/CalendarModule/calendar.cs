using AdoNetWindow.API;
using AdoNetWindow.CalendarModule.VacationManager;
using AdoNetWindow.Common;
using AdoNetWindow.Config;
using AdoNetWindow.DashboardForSales;
using AdoNetWindow.DashboardForSales.MultiDashboard;
using AdoNetWindow.LoginForm;
using AdoNetWindow.Model;
using AdoNetWindow.Pending;
using AdoNetWindow.Product;
using AdoNetWindow.RecoveryPrincipal;
using AdoNetWindow.SaleManagement;
using AdoNetWindow.SaleManagement.DailyBusiness;
using AdoNetWindow.SaleManagement.SalesDashboard2;
using AdoNetWindow.SaleManagement.SalesManagerModule;
using AdoNetWindow.SEAOVER._2Line;
using AdoNetWindow.SEAOVER.PriceComparison;
using AdoNetWindow.SEAOVER.PriceComparison.AlmostOutofStock;
using AdoNetWindow.SEAOVER.ProductCostComparison;
using AdoNetWindow.SEAOVER.SimpleHandlingFormManager;
using Libs.Tools;
using Repositories;
using Repositories.Calender;
using Repositories.Company;
using Repositories.Config;
using Repositories.SEAOVER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ubiety.Dns.Core;

namespace AdoNetWindow.CalendarModule
{
    public partial class calendar : ApplicationRoot
    {
        LogManager log;
        ConfigModel cm = new ConfigModel();
        UsersModel um = new UsersModel();
        public int year, month, days;
        double customRate;
        string userId;
        DataGridView dgvGuarantee;

        IHideRepository hideRepository = new HideRepository();
        ICommonRepository commonRepository = new CommonRepository();
        IFavoriteMenuRepository favoriteMenuRepository = new FavoriteMenuRepository();
        IStockRepository stockRepository = new StockRepository();
        IUsersRepository usersRepository = new UsersRepository();
        ILoanRepository loanRepository = new LoanRepository();
        IMemoRepository memoRepository = new MemoRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        IConfigRepository configRepository = new ConfigRepository();
        IinspectionRepository inspectionRepository  = new InspectionRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        IPriceComparisonRepository priceComparisonRepository = new PriceComparisonRepository();
        IAnnualRepository annualRepository = new AnnualRepository();
        ICompanyAlarmRepository companyAlarmRepository = new CompanyAlarmRepository();
        IAuthorityRepository authorityRepository = new AuthorityRepository();   
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.ftpCommon ftManager = new Libs.ftpCommon();
        Libs.MessageBox messageBox = new Libs.MessageBox();

        //Find data
        List<string> findDiv = new List<string>();
        List<DateTime> findDate = new List<DateTime>();
        List<int> findId = new List<int>();
        int findIdx = -1;

        public calendar(string user_id)
        {
            InitializeComponent();
            userId = user_id;

        }
        //종료이벤트
        Libs.ftpCommon ftp = new Libs.ftpCommon();
        private void calendar_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (messageBox.Show(this, "프로그램을 종료 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //NAS 연결종료
                DirectoryInfo di = new DirectoryInfo("O:");
                //DirectoryInfo.Exists로 폴더 존재유무 확인
                if (di.Exists)
                    ftp.Cancelserver("O:");
                //Log
                log = new LogManager(null, "_Login");
                log.WriteLine("[Close Processing]-----");
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void calendar_Load(object sender, EventArgs e)
        {
            //Double buffer
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.KeyPreview = true;
            출력설정ToolStripMenuItem.DropDown.AutoClose = false;

            //기준년월
            DateTime now = DateTime.Now;
            month = now.Month;
            year = now.Year;

            //로그인 유저정보 확인
            um = usersRepository.GetUserInfo(userId);
            if (um != null)
                stmUser.Text = um.user_name + "님 반갑습니다.";
             else
            {
                messageBox.Show(this,"로그인 정보를 찾을 수 없습니다.");
                this.Hide();
                Logins login = new Logins();
                login.Show();
                Program.ac.MainForm = login;
                this.Close();
                foreach (Form openForm in Application.OpenForms)
                {
                    if (openForm.Name != "Login")
                        openForm.Dispose();
                }
                return;
            }
            //Seaover 사번이 없는경우 수정
            if (um.seaover_id == null || string.IsNullOrEmpty(um.seaover_id))
            {
                messageBox.Show(this,"내정보에서 SEAOVER 사번을 입력해주세요.");
                Config.EditMyInfo emi = new Config.EditMyInfo(um, this);
                um = emi.UpdateSeaoverId();
            }

            //로그인 유저 권한설정======================================================
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
                foreach (ToolStripItem tsItem in msMenu.Items)
                {
                    if (tsItem is ToolStripMenuItem)
                    {
                        ToolStripMenuItem mainMenu = (ToolStripMenuItem)tsItem;
                        string group_name = mainMenu.Text;
                        foreach (ToolStripItem subItem in mainMenu.DropDownItems)
                        {
                            if (subItem is ToolStripMenuItem)
                            {
                                foreach (DataRow ddr in authorityDt.Rows)
                                {
                                    if (group_name == ddr["group_name"].ToString() && subItem.Text == ddr["form_name"].ToString())
                                    {
                                        if (bool.TryParse(ddr["is_visible"].ToString(), out bool is_visible))
                                            subItem.Visible = is_visible;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //알람======================================================================================================================
            if (um.department.Contains("관리부") || um.department.Contains("전산") || um.department.Contains("무역"))
            {
                //통관미처리품목
                ConvertToPending ctp = new ConvertToPending(um);
                ctp.CalendarOpenAlarm();
                //씨오버미확인품목
                ConvertProductToSeaover cpts = new ConvertProductToSeaover(um);
                cpts.CalendarOpenAlarm();
                //계약ETD임박품목
                AlmostOutOfStockManager aosm = new AlmostOutOfStockManager(this, um);
                aosm.CalendarOpenAlarm();
                //ETD, ETA누락 선적내역
                MissingShipmentManager msm = new MissingShipmentManager(this, um); 
                msm.CalendarOpenAlarm();
                //팬딩 입항전/후 일정
                ArrivalSchedule arrivalSchedule = new ArrivalSchedule(um);
                arrivalSchedule.Show();

            }
            else if (um.department.Contains("관리부") || um.department.Contains("영업") || um.department.Contains("경리"))
            {
                CompanyInsuranceManager cim = new CompanyInsuranceManager(um, this);
                cim.CalendarOpenAlarm();
            }

            //Checked 설정내역======================================================================================================================
            cm = configRepository.GetConfigChecked(userId);
            if (cm != null)
            {
                if (cm.shipment_checked)
                {
                    tsmShipment.CheckState = CheckState.Checked;
                    cbShipment.Checked = true;
                }
                if (cm.payment_checked)
                {
                    tsmPayment.CheckState = CheckState.Checked;
                    cbPayment.Checked = true;
                }
                if (cm.memo_checked)
                {
                    tsmUnknown.CheckState = CheckState.Checked;
                    cbMemoPayment.Checked = true;
                }
                if (cm.salememo_checked)
                {
                    tsmSaleMemo.CheckState = CheckState.Checked;
                    cbSaleMemo.Checked = true;
                }
                if (cm.arrive_checked)
                {
                    tsmArrive.CheckState = CheckState.Checked;
                    cbArrive.Checked = true;
                }
                if (cm.inspection_checked)
                {
                    tsmInspection.CheckState = CheckState.Checked;
                    cbInspection.Checked = true;
                }
            }
            //즐겨찾기 가져오기======================================================================================================================
            GetFavoriteMenu();
            //Data가져오기======================================================================================================================
            displayDays(year, month);
            displayWidget();


            //미사용연차 통지확인======================================================================================================================
            if (!annualRepository.GetAgreement(um.user_id, DateTime.Now.Year, DateTime.Now.Month))
            {
                VacationAgreementManager vam = new VacationAgreementManager(um);
                vam.Owner = this;
                vam.Show();
            }
        }

        #region Method
        public MenuStrip GetMenu()
        {
            return msMenu;
        }
        //프로시져 호출
        private void bgwProcedure_DoWork(object sender, DoWorkEventArgs e)
        {
            CallProcedure();
        }
        //버전확인
        public void VersionCheck()
        {
            string newest_version = common.GetCurrentVersion("http://atotrading.iptime.org/AdoNetWindow.application");
            string current_version = common.GetAppVer();
            if (newest_version == current_version)
            {
                lbCurrentVersion.Text = "(*최신버전*)현재버전 : " + current_version;
            }
            else
            {
                lbCurrentVersion.Text = "(*구버전 업데이트 권장*)현재버전 : " + current_version;
                lbCurrentVersion.ForeColor = Color.Red;
                messageBox.Show(this, "구버전입니다. 프로그램을 재시작하여 최신버전으로 업데이트해주시기 바랍니다." + "\n *현재버전 : " + current_version, " ");
                this.Activate();
            }
        }

        //즐겨찾기 단축키
        private void OpenMenuAutoKey(string form_name, int idx)
        {
            if (pnFavoriteMenu.Controls.Count > idx - 1)
            {
                Button btn = (Button)pnFavoriteMenu.Controls[idx - 1];
                btn.PerformClick();
            }
        }
        //즐겨찾기메뉴 불러오기
        public void GetFavoriteMenu()
        {
            pnFavoriteMenu.Controls.Clear();
            //GetData
            List<FavoriteMenuModel> model = favoriteMenuRepository.GetFavoriteMenu(um.user_id);
            if (model.Count > 0)
            {
                for (int i = 0; i < model.Count; i++)
                {
                    Button bt = new Button();
                    bt.Margin = new System.Windows.Forms.Padding(0);
                    bt.Padding = new System.Windows.Forms.Padding(1);
                    //글자수에 따른 크기설정
                    bt.Text = model[i].form_name;
                    if (bt.Text.Length <= 4)
                        bt.Width = 75;
                    else if (bt.Text.Length <= 5)
                        bt.Width = 85;
                    else if (bt.Text.Length <= 7)
                        bt.Width = 100;
                    else if (bt.Text.Length <= 8)
                        bt.Width = 120;
                    else
                    {
                        bt.Width = model[i].form_name.Length * 12;
                        if (bt.Width < 75)
                               bt.Width = 75;
                    }
                    //Click event

                    //사용가능한 메뉴 설정
                    foreach (ToolStripItem tsItem in msMenu.Items)
                    {
                        if (tsItem is ToolStripMenuItem)
                        {
                            ToolStripMenuItem mainMenu = (ToolStripMenuItem)tsItem;
                            string group_name = mainMenu.Text;
                            foreach (ToolStripItem subItem in mainMenu.DropDownItems)
                            {
                                if (subItem is ToolStripMenuItem && subItem.Text == bt.Text)
                                {
                                    bt.Click += (sender, e) =>
                                    {
                                        subItem.PerformClick();
                                    };
                                }
                            }
                        }
                    }

                    /*switch (bt.Text)
                    {
                        //설정 TAB
                        case "국가별 배송기간":
                            bt.Click += new System.EventHandler(this.FavoirteMenue_CountyConfig_Click);
                            break;
                        case "나라별 공휴일":
                            bt.Click += new System.EventHandler(this.나라별공휴일ToolStripMenuItem_Click);
                            break;
                        case "내정보 수정":
                            bt.Click += new System.EventHandler(this.FavoirteMenue_Setting_Click);
                            break;
                        case "관리자 설정":
                            bt.Click += new System.EventHandler(this.관리자설정ToolStripMenuItem_Click);
                            break;
                        case "FAX":
                            bt.Click += new System.EventHandler(this.fAXToolStripMenuItem_Click_1);
                            break;
                        //공공데이터
                        case "수출입현황":
                            bt.Click += new System.EventHandler(this.FavoirteMenue_ImportApi_Click);
                            break;
                        case "HACCP 업체정보":
                            bt.Click += new System.EventHandler(this.hACCP업체정보ToolStripMenuItem_Click);
                            break;
                        case "수출업소대장":
                            bt.Click += new System.EventHandler(this.수출업소대장ToolStripMenuItem_Click);
                            break;
                        case "식품제조가공업정보":
                            bt.Click += new System.EventHandler(this.식품제조가공업정보ToolStripMenuItem_Click);
                            break;
                        case "수입식품 허가정보":
                            bt.Click += new System.EventHandler(this.수입식품허가정보ToolStripMenuItem_Click);
                            break;
                        //검품리스트
                        case "검품리스트":
                            bt.Click += new System.EventHandler(this.FavoirteMenue_Insepection_Click);
                            break;
                        //기준정보
                        case "품목 관리":
                            bt.Click += new System.EventHandler(this.FavoirteMenue_ProductManager_Click);
                            break;
                        case "거래처 관리":
                            bt.Click += new System.EventHandler(this.FavoirteMenue_CompanyManager_Click);
                            break;
                        case "조업/계약시기":
                            bt.Click += new System.EventHandler(this.FavoriteMenu_Operating_period_Click);
                            break;
                        //영업거래처 관리
                        case "원금회수율 관리":
                            bt.Click += new System.EventHandler(this.원금회수율ToolStripMenuItem_Click);
                            break;
                        case "영업거래처 관리":
                            bt.Click += new System.EventHandler(this.FavoirteMenue_SalesPartner_Click);
                            break;
                        case "영업전화 대시보드":
                            bt.Click += new System.EventHandler(this.영업전화대시보드ToolStripMenuItem_Click);
                            break;
                        //수입관리
                        case "거래처별 매입단가 일괄등록":
                            bt.Click += new System.EventHandler(this.FavoirteMenue_MultiInsertPurchasePrice_Click);
                            break;
                        case "거래처별 매입단가 조회":
                            bt.Click += new System.EventHandler(this.FavoirteMenue_PurchasePriceManager_Click);
                            break;
                        case "매입단가 그래프":
                            bt.Click += new System.EventHandler(this.FavoirteMenue_GraphManager_Click);
                            break;
                        case "원가계산":
                            bt.Click += new System.EventHandler(this.FavoirteMenue_CostAccounting_Click);
                            break;
                        case "원가 및 재고 대시보드":
                            bt.Click += new System.EventHandler(this.대시보드ToolStripMenuItem_Click);
                            break;
                        case "해외제조업소 및 수입업체 수출입":
                            bt.Click += new System.EventHandler(this.해외제조업소및수입업체수출입ToolStripMenuItem_Click);
                            break;
                        *//*case "다중 원가계산":
                            bt.Click += new System.EventHandler(this.다중원가계산ToolStripMenuItem_Click);
                            break;*//*
                        //팬딩관리
                        case "입항 일정":
                            bt.Click += new System.EventHandler(this.입항일정ToolStripMenuItem_Click);
                            break;
                        case "팬딩 조회":
                            bt.Click += new System.EventHandler(this.FavoirteMenue_PendingManager_Click);
                            break;
                        case "팬딩 조회2":
                            bt.Click += new System.EventHandler(this.팬딩조회2ToolStripMenuItem_Click);
                            break;
                        case "팬딩 등록":
                            bt.Click += new System.EventHandler(this.FavoirteMenue_InsertPending_Click);                            
                            break;
                        //SEAOVER
                        case "품목단가표":
                            bt.Click += new System.EventHandler(this.FavoirteMenue_SimpleHandlingFormManager_Click);
                            break;
                        case "취급품목서":
                            bt.Click += new System.EventHandler(this.FavoirteMenue_HandlingProduct_Click);
                            break;
                        case "취급품목서(초밥류)":
                            bt.Click += new System.EventHandler(this.취급품목서초밥류ToolStripMenuItem_Click);
                            break;
                        case "업체별시세현황":
                            bt.Click += new System.EventHandler(this.FavoirteMenue_PriceComparison_Click);
                            break;
                        case "업체별시세현황2":
                            bt.Click += new System.EventHandler(this.업체별시세현황21ToolStripMenuItem_Click);
                            break;
                        case "품목매출한도":
                            bt.Click += new System.EventHandler(this.품명별매출한도ToolStripMenuItem_Click);
                            break;
                        case "품명별 매출관리 대시보드":
                            bt.Click += new System.EventHandler(this.품명별매출관리대시보드ToolStripMenuItem_Click);
                            break;
                        *//*case "그룹 관리":
                            bt.Click += new System.EventHandler(this.FavoriteMenu_AddGroup_Click);
                            break;*//*
                    }*/
                    pnFavoriteMenu.Controls.Add(bt);
                }
            }
        }
        //SEAOVER 프로시져 호출하기
        private void CallProcedure()
        {
            string sttdate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
            string enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string seaover_id = um.seaover_id;
            //업체별시세현황 스토어프로시져 호출
            if (seaoverRepository.CallStoredProcedure(seaover_id, sttdate, enddate) == 0)
            {
                messageBox.Show(this,"호출 내용이 없음");
                return;
            }
            sttdate = DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd");
            enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            //품명별매출현황 스토어프로시져 호출
            if (priceComparisonRepository.CallStoredProcedure(seaover_id, sttdate, enddate) == 0)
            {
                messageBox.Show(this,"호출 내용이 없음");
                return;
            }
        }
        //유저정보 업데이트
        public void GetUserInfo()
        {
            um = usersRepository.GetUserInfo(um.user_id);
        }

        //메모
        public void displayMemo(int sYear, int sMonth)
        {
            MemoPanel.Controls.Clear();

            //신용장 한도 조회
            List<LoanInfo> list = new List<LoanInfo>();
            list = loanRepository.GetCurrentLoan();
            if (list.Count > 0)
            {
                double usanceTotal = 0;
                double atsightTotal = 0;
                double usanceUsed = 0;
                double atsightUsed = 0;
                string div = "";
                for (int i = 0; i < list.Count; i++)
                {
                    LoanInfo model = list[i];
                    //첫 타이틀
                    if (div != model.division & i == 0)
                    {
                        UserControlCreditTitle ucTitle = new UserControlCreditTitle(model.division);
                        MemoPanel.Controls.Add(ucTitle);
                        div = model.division;

                        //은행별 사용내역 출력
                        UserControlCredit ucCredit = new UserControlCredit(model);
                        MemoPanel.Controls.Add(ucCredit);
                        //총합 계산
                        usanceTotal += model.usance_loan_limit;
                        atsightTotal += model.atsight_loan_limit;
                        usanceUsed += model.usance_loan_used;
                        atsightUsed += model.atsight_loan_used;
                    }
                    else if (div != model.division)
                    {
                        UserControlCredit ucTotalCredit = new UserControlCredit(usanceTotal, usanceUsed, atsightTotal, atsightUsed);
                        MemoPanel.Controls.Add(ucTotalCredit);

                        //초기화
                        usanceTotal = 0;
                        atsightTotal = 0;
                        usanceUsed = 0;
                        atsightUsed = 0;

                        UserControlCreditTitle ucTitle = new UserControlCreditTitle(model.division);
                        MemoPanel.Controls.Add(ucTitle);
                        div = model.division;

                        //은행별 사용내역 출력
                        UserControlCredit ucCredit = new UserControlCredit(model);
                        MemoPanel.Controls.Add(ucCredit);
                        //총합 계산
                        usanceTotal += model.usance_loan_limit;
                        atsightTotal += model.atsight_loan_limit;
                        usanceUsed += model.usance_loan_used;
                        atsightUsed += model.atsight_loan_used;
                    }
                    else
                    {
                        //은행별 사용내역 출력
                        UserControlCredit ucCredit = new UserControlCredit(model);
                        MemoPanel.Controls.Add(ucCredit);
                        //총합 계산
                        usanceTotal += model.usance_loan_limit;
                        atsightTotal += model.atsight_loan_limit;
                        usanceUsed += model.usance_loan_used;
                        atsightUsed += model.atsight_loan_used;  
                    }
                }
                UserControlCredit ucLastTotalCredit = new UserControlCredit(usanceTotal, usanceUsed, atsightTotal, atsightUsed);
                MemoPanel.Controls.Add(ucLastTotalCredit);
            }
        }

        private DataTable CreateDatatable()
        { 
            DataTable dt = new DataTable("find_data");

            DataColumn col1 = new DataColumn();
            col1.DataType = typeof(int);
            col1.ColumnName = "id";
            dt.Columns.Add(col1);

            DataColumn col2 = new DataColumn();
            col2.DataType = typeof(DateTime);
            col2.ColumnName = "find_date";
            dt.Columns.Add(col2);

            DataColumn col3 = new DataColumn();
            col3.DataType = typeof(string);
            col3.ColumnName = "division";
            dt.Columns.Add(col3);

            return dt;
        }

        public void RefreshFindIdx()
        {
            findIdx = -1;
        }

        //내역찾기
        public void Find_pending(string ato_no = "", string bl_no = "", string lc_no = "")
        {
            //환율가져오기
            /*customRate = common.GetExchangeRateKEBBank("USD");
            lbCustomRate.Text = "환율 : " + customRate.ToString("#,##0.00$");*/
            //clear container
            daycontainer.Controls.Clear();
            //내역찾기============================================================
            cbInspection.Checked = false;
            cbArrive.Checked = false;
            DateTime sttdate = new DateTime(1900, 1, 1);
            DateTime enddate = new DateTime(2100, 12, 31);
            //string findDate = null;

            findId = new List<int>();
            findDiv = new List<string>();
            findDate = new List<DateTime>();
            DataTable findDt = CreateDatatable();
            
            //결제 체크박스 
            List<CustomsModel> cm = new List<CustomsModel>();
            List<MemoModel> mm = new List<MemoModel>();
            if (this.cbPayment.Checked)
            {
                cm = customsRepository.GetCustomMonth(sttdate, enddate, ato_no, bl_no, lc_no, "", "", "", "");
                if (cm.Count > 0)
                {
                    cm = SetPayment(cm);
                    for (int i = 0; i < cm.Count; i++)
                    {
                        DataRow dr = findDt.NewRow();
                        dr["id"] = cm[i].id;
                        dr["find_date"] = Convert.ToDateTime(cm[i].payment_date);
                        dr["division"] = "custom";
                        findDt.Rows.Add(dr);
                    }                    
                }
                mm = memoRepository.GetTradeMemo(sttdate, enddate, 0, true, "", ato_no);
                if (mm.Count > 0)
                {
                    for (int i = 0; i < mm.Count; i++)
                    {
                        DataRow dr = findDt.NewRow();
                        dr["id"] = mm[i].id;
                        dr["find_date"] = new DateTime(mm[i].syear, mm[i].smonth, mm[i].sday);
                        dr["division"] = "memo";
                        findDt.Rows.Add(dr);
                    }
                }
            }
            //미확인 결제 체크박스 
            List<CustomsModel> nm = new List<CustomsModel>();
            if (this.cbMemoPayment.Checked)
            {
                nm = customsRepository.GetCustomUnknown(sttdate, enddate, ato_no, bl_no, lc_no, "", "", "", "");
                if (nm.Count > 0)
                {
                    for (int i = 0; i < nm.Count; i++)
                    {
                        DataRow dr = findDt.NewRow();
                        dr["id"] = nm[i].id;
                        dr["find_date"] = Convert.ToDateTime(nm[i].payment_date);
                        dr["division"] = "unkouwn";
                        findDt.Rows.Add(dr);
                    }
                }
            }
            //선적 체크박스 
            List<CustomsModel> sm = new List<CustomsModel>();
            if (cm.Count == 0 && nm.Count == 0 && this.cbShipment.Checked)
            {
                sm = customsRepository.GetShipmentMonth(sttdate, enddate, ato_no, bl_no, lc_no, "", "", "", "");
                if (sm.Count > 0)
                {
                    for (int i = 0; i < sm.Count; i++)
                    {
                        DataRow dr = findDt.NewRow();
                        dr["id"] = sm[i].id;
                        dr["find_date"] = Convert.ToDateTime(sm[i].payment_date);
                        dr["division"] = "shipment";
                        findDt.Rows.Add(dr);
                    }
                }
            }
            //출력================================================
            if (findDt.Rows.Count == 0)
            {
                messageBox.Show(this,"검색한 내역의 결과값이 없습니다.");
                return;
            }
            else
            {
                //Sort
                DataView dv = new DataView(findDt);
                dv.Sort = "find_date DESC";
                findDt = dv.ToTable();
                findDt.EndInit();
                for (int i = 0; i < findDt.Rows.Count; i++)
                {
                    findDiv.Add(findDt.Rows[i]["division"].ToString());
                    findDate.Add(Convert.ToDateTime(findDt.Rows[i]["find_date"].ToString()));
                    findId.Add(Convert.ToInt32(findDt.Rows[i]["id"].ToString()));
                }
                //Output
                findIdx++;
                if (findId.Count <= findIdx)
                {
                    messageBox.Show(this,"찾을 내역이 없습니다.");
                    findIdx = -1;
                    return;
                }
                displayDays(findDate[findIdx].Year, findDate[findIdx].Month, findDate[findIdx].Day, ato_no, bl_no, lc_no, "", "", "", "", findDiv, findDate, findId, findIdx);
            }
        }

        //달력일정
        public void displayDays(int sYear, int sMonth, int accent_date = 0
            , string ato_no = "", string bl_no = "", string lc_no = ""
            , string product = "", string origin = "", string sizes = "", string manager = ""
            , List<string> findDIv = null, List<DateTime> findDate = null, List<int> findId = null, int findIdx = -1)
        {
            //환율가져오기
            customRate = common.GetExchangeRateKEBBank("USD");
            lbCustomRate.Text = "환율 : " + customRate.ToString("#,##0.00$");
            //clear container
            daycontainer.Controls.Clear();

            string monthname = DateTimeFormatInfo.CurrentInfo.GetMonthName(sMonth);
            //Title
            LBDATE.Text = sYear + "년 " + monthname;

            //Lets get the first day of the month
            DateTime startofthemonth = new DateTime(sYear, sMonth, 1);

            //get the count of days of the month
            int days = DateTime.DaysInMonth(sYear, sMonth);

            //convert the startofthemonth to integer
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 1;

            //Holiday List
            int[] no;
            string[] name;
            common.getRedDay(sYear, sMonth, out no, out name);

            DateTime sttdate = new DateTime(sYear, sMonth, 1, 0,0,0);
            DateTime sttdate2 = new DateTime(sYear, sMonth, 1, 0, 0, 0).AddDays(-7);
            DateTime enddate = new DateTime(sYear, sMonth, 1,0,0,0).AddMonths(1).AddDays(-1);
            int dayofnextmonth = 0;
            //마지막주의 금요일까지
            while (!(enddate.DayOfWeek == DayOfWeek.Saturday || enddate.DayOfWeek == DayOfWeek.Sunday))
            {
                enddate = enddate.AddDays(1);
                dayofnextmonth++;      //다음달 오버되는 일짜
            }
            //선적 체크박스 
            List<CustomsModel> sm = new List<CustomsModel>();
            if (this.cbShipment.Checked)
            {
                sm = customsRepository.GetShipmentMonth(sttdate, enddate, ato_no, bl_no, lc_no, product, origin, sizes, manager);
            }

            //결제 체크박스 
            List<CustomsModel> cm = new List<CustomsModel>();  
            if (this.cbPayment.Checked)
            {
                cm = customsRepository.GetCustomMonth(sttdate2, enddate, ato_no, bl_no, lc_no, product, origin, sizes, manager);
                cm = SetPayment(cm);
            }
            //미확인 결제 체크박스 
            List<CustomsModel> nm = new List<CustomsModel>();  
            if (this.cbMemoPayment.Checked)
            {
                nm = customsRepository.GetCustomUnknown(sttdate2, enddate, ato_no, bl_no, lc_no, product, origin, sizes, manager);
                nm = SetPayment(nm);
            }
            //기타결제 체크박스 (무역부)
            List<MemoModel> mm = new List<MemoModel>();
            if (this.cbPayment.Checked
                && string.IsNullOrEmpty(bl_no) && string.IsNullOrEmpty(lc_no)
                && string.IsNullOrEmpty(product) && string.IsNullOrEmpty(origin) && string.IsNullOrEmpty(sizes) && string.IsNullOrEmpty(manager))
            {
                mm = memoRepository.GetTradeMemo(sttdate2, enddate, 0, true, manager, ato_no);
            }
            
            //영업부 메모
            List<MemoModel> smm = new List<MemoModel>();
            DataTable alarmDt = new DataTable();
            if (this.cbSaleMemo.Checked)
            {
                smm = memoRepository.GetSaleMemo(sttdate2, enddate, 0, true);
                alarmDt = companyAlarmRepository.GetCompanyAlarmForCalendar(um.user_name);
                alarmDt = SetAlarm(alarmDt, year, month);
            }
            //통관예정
            List<ArriveModel> am = new List<ArriveModel>();
            if (this.cbArrive.Checked)
            {
                am = customsRepository.GetArrive(sttdate2, enddate, product, origin, sizes, manager);
                am = SetArrive(am, year, month);
            }
            //검품일장
            List<InspectionModel> im = new List<InspectionModel>();
            if (this.cbInspection.Checked)
            {
                im = inspectionRepository.GetInspectionSchedule(sttdate2, enddate, origin, product, sizes, manager);
                im = SetInspection(im);
            }
            //엮을 품목
            DataTable gDt = null;
            if (this.cbGuarantee.Checked)
            {
                gDt = customsRepository.GetGuarantee(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"), ato_no, "", "");
                gDt = SetGuarantee(gDt, year, month);
            }

            //==========================================================================
            //first lets create a blank usercontrol
            for (int i = 1; i < dayoftheweek; i++)
            {                
                DateTime dt = new DateTime(sYear, sMonth, 1).AddDays(-dayoftheweek + i);
                UserControlBlank ucblack = new UserControlBlank(dt);
                daycontainer.Controls.Add(ucblack);
            }
            //==========================================================================
            //now lets create usercontrol for days
            for (int i = 1; i <= days; i++)
            {
                UserControlDays ucdays = new UserControlDays(this);
                ucdays.days(um, this, i, sYear, sMonth, no, name, cm, mm, sm, nm, smm, alarmDt, am, im, gDt, findDIv, findDate, findId, findIdx);
                daycontainer.Controls.Add(ucdays);
            }
            //==========================================================================
            //day of same week and over month
            sttdate = new DateTime(sYear, sMonth, 1).AddMonths(1);
            if (dayofnextmonth > 0)
            {
                //Holiday List
                common.getRedDay(sttdate.Year, sttdate.Month, out no, out name);
                for (int i = 1; i <= dayofnextmonth; i++)
                {
                    UserControlDays ucdays = new UserControlDays(this);
                    ucdays.days(um, this, i, sttdate.Year, sttdate.Month, no, name, cm, mm, sm, nm, smm, alarmDt, am, im, gDt, findDIv, findDate, findId, findIdx, true);
                    daycontainer.Controls.Add(ucdays);
                }
            }
            
            //총 결제금액
            TotalPaymentPrice(cm, mm, sm, nm, smm, am, im, gDt);
            //버전체크
            //VersionCheck();
        }

        private void lbWidgetTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnHelper.Visible = false;
            string title = lbWidgetTitle.Text;
            //문제내역
            switch (title)
            {
                case "미결제 또는 확인필수 내역":
                    ProblemData();
                    lbWidget.Text = "1";
                    break;
                case "담보가능 재고":
                    GuaranteeStock();
                    lbWidget.Text = "2";
                    break;
                case "서류 미도착분":
                    CheckNoPaper();
                    lbWidget.Text = "3";
                    break;
                case "아토재고 대시보드":
                    StockDashboard();
                    lbWidget.Text = "4";
                    break;
                case "선결제 재고":
                    GetPrepaymentStock();
                    lbWidget.Text = "5";
                    break;
                
            }
        }

        public void displayWidget()
        {
            btnHelper.Visible = false;
            int id = Convert.ToInt16(lbWidget.Text);
            //문제내역
            switch (id)
            {
                case 1:
                    ProblemData();
                    break;
                case 2:
                    GuaranteeStock();
                    break;
                case 3:
                    CheckNoPaper();
                    break;
                case 4:
                    StockDashboard();
                    break;
            }
            
        }

        public void StockDashboard()
        {
            btnHelper.Visible = true;
            //Title
            lbWidgetTitle.Text = "아토재고 대시보드";
            //Panel change
            Color col = Color.FromArgb(47, 117, 181);
            pnTotalAmount.Dock = DockStyle.Top;
            lbTitle.Font = new Font("나눔고딕", 20, FontStyle.Bold);
            lbTitle.ForeColor = col;
            lbTitle.Location = new Point(5, 0);
            lbTotalPrice.Font = new Font("나눔고딕", 20, FontStyle.Bold);
            lbTotalPrice.ForeColor = col;
            lbTotalPrice.Height = 40;
            lbTotalPrice.Location = new Point(-2, 0);

            MemoPanel.Dock = DockStyle.None;
            MemoPanel.Visible = false;

            stockPanel.Dock = DockStyle.Fill;
            stockPanel.Visible = true;
            //Table Setting
            dgvGuarantee = new DataGridView();
            dgvGuarantee.AllowUserToDeleteRows = false;
            dgvGuarantee.AllowUserToAddRows = false;
            dgvGuarantee.RowHeadersVisible = false;
            dgvGuarantee.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvGuarantee.Font = new Font("나눔고딕", 9, FontStyle.Bold);
            dgvGuarantee.Width = MemoPanel.Width - 20;
            dgvGuarantee.Height = 500;
            //Columns
            dgvGuarantee.ColumnCount = 3;

            dgvGuarantee.Columns[0].Name = "id";
            dgvGuarantee.Columns[0].HeaderText = "id";
            dgvGuarantee.Columns[0].Width = 10;
            dgvGuarantee.Columns[0].Visible = false;

            dgvGuarantee.Columns[1].Name = "title";
            dgvGuarantee.Columns[1].HeaderText = "구분";
            dgvGuarantee.Columns[1].Width = 150;

            dgvGuarantee.Columns[2].Name = "total_amount";
            dgvGuarantee.Columns[2].HeaderText = "금액";
            dgvGuarantee.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvGuarantee.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvGuarantee.ColumnHeadersDefaultCellStyle.BackColor = Color.Beige;
            dgvGuarantee.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            stockPanel.Controls.Add(dgvGuarantee);
            dgvGuarantee.Dock = DockStyle.Fill;
            dgvGuarantee.BringToFront();

            //event handler
            dgvGuarantee.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvGuarantee_MouseUp);
            dgvGuarantee.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGuarantee_CellMouseClick);

            //Data
            CallStockProcedure(true);
            DataTable model = stockRepository.GetStockDashboard();

            if (model.Rows.Count > 0)
            {
                double total_amount = 0;
                int n;
                for (int i = 0; i < model.Rows.Count; i++)
                {                    
                    n = dgvGuarantee.Rows.Add();
                    dgvGuarantee.Rows[n].Cells["id"].Value = model.Rows[i]["id"].ToString();
                    dgvGuarantee.Rows[n].Cells["title"].Value = model.Rows[i]["구분"].ToString();
                    dgvGuarantee.Rows[n].Cells["total_amount"].Value = Convert.ToDouble(model.Rows[i]["재고금액"].ToString()).ToString("#,##0");
                    //총 금액
                    switch(Convert.ToInt16(model.Rows[i]["id"].ToString()))
                    {
                        case 1:
                            total_amount += Convert.ToDouble(model.Rows[i]["재고금액"].ToString());
                            break;
                        case 2:
                            total_amount -= Convert.ToDouble(model.Rows[i]["재고금액"].ToString());
                            break;
                        case 3:
                            total_amount += Convert.ToDouble(model.Rows[i]["재고금액"].ToString());
                            break;
                        case 4:
                            dgvGuarantee.Rows[i].DefaultCellStyle.BackColor = Color.Beige;
                            total_amount -= Convert.ToDouble(model.Rows[i]["재고금액"].ToString());
                            break;
                        case 5:
                            dgvGuarantee.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
                            total_amount -= Convert.ToDouble(model.Rows[i]["재고금액"].ToString());
                            break;
                        case 6:
                            dgvGuarantee.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
                            total_amount -= Convert.ToDouble(model.Rows[i]["재고금액"].ToString());
                            break;
                    }
                }
                //선T/T재고
                double amount = 0;
                DataTable ttModel = customsRepository.GetPrepayment();
                if (ttModel.Rows.Count > 0)
                {
                    for (int j = 0; j < ttModel.Rows.Count; j++)
                    {
                        amount += (Convert.ToDouble(ttModel.Rows[j]["total_price"].ToString()) * customRate);
                    }
                }
                n = dgvGuarantee.Rows.Add();
                dgvGuarantee.Rows[n].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
                dgvGuarantee.Rows[n].Cells["title"].Value = "선T/T재고";
                dgvGuarantee.Rows[n].Cells["total_amount"].Value = amount.ToString("#,##0");

                //총 금액
                total_amount += amount;
                lbTotalPrice.Text = total_amount.ToString("#,##0");

                //선결제 재고 상세
                for (int i = 0; i < ttModel.Rows.Count; i++)
                {
                    n = dgvGuarantee.Rows.Add();
                    dgvGuarantee.Rows[n].DefaultCellStyle.Font = new Font("나눔고딕", 7, FontStyle.Italic);
                    dgvGuarantee.Rows[n].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
                    dgvGuarantee.Rows[n].Cells["title"].Value = "     " + ttModel.Rows[i]["ato_no"].ToString();
                    dgvGuarantee.Rows[n].Cells["total_amount"].Value = (Convert.ToDouble(ttModel.Rows[i]["total_price"].ToString()) * customRate).ToString("#,##0");
                }
            }
        }
        public void GetPrepaymentStock()
        {
            //Title
            lbWidgetTitle.Text = "선결제 재고";
            //Panel change
            MemoPanel.Dock = DockStyle.None;
            MemoPanel.Visible = false;

            stockPanel.Dock = DockStyle.Fill;
            stockPanel.Visible = true;
            //Table Setting
            dgvGuarantee = new DataGridView();
            dgvGuarantee.AllowUserToDeleteRows = false;
            dgvGuarantee.AllowUserToAddRows = false;
            dgvGuarantee.RowHeadersVisible = false;
            dgvGuarantee.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvGuarantee.Font = new Font("나눔고딕", 7, FontStyle.Regular);
            dgvGuarantee.Width = MemoPanel.Width - 20;
            dgvGuarantee.Height = 500;
            //Columns
            dgvGuarantee.ColumnCount = 4;

            dgvGuarantee.Columns[0].Name = "ato_no";
            dgvGuarantee.Columns[0].HeaderText = "Ato no.";
            dgvGuarantee.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvGuarantee.Columns[1].Name = "product";
            dgvGuarantee.Columns[1].HeaderText = "품목";
            dgvGuarantee.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvGuarantee.Columns[2].Name = "price_usd";
            dgvGuarantee.Columns[2].HeaderText = "USD";
            dgvGuarantee.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvGuarantee.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGuarantee.Columns[3].Name = "price_krw";
            dgvGuarantee.Columns[3].HeaderText = "KRW";
            dgvGuarantee.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvGuarantee.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGuarantee.ColumnHeadersDefaultCellStyle.BackColor = Color.Beige;
            dgvGuarantee.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGuarantee.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvGuarantee_MouseUp);
            dgvGuarantee.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGuarantee_CellMouseClick);

            stockPanel.Controls.Add(dgvGuarantee);
            dgvGuarantee.Dock = DockStyle.Fill;
            dgvGuarantee.BringToFront();
            //Data
            DataTable model = customsRepository.GetPrepayment();
            if (model.Rows.Count > 0)
            {
                double total_amount = 0;
                //환율
                customRate = common.GetExchangeRateKEBBank("USD");
                for (int i = 0; i < model.Rows.Count; i++)
                {
                    int n = dgvGuarantee.Rows.Add();
                    dgvGuarantee.Rows[n].Cells["ato_no"].Value = model.Rows[i]["ato_no"].ToString();
                    dgvGuarantee.Rows[n].Cells["product"].Value = model.Rows[i]["product"].ToString();
                    dgvGuarantee.Rows[n].Cells["price_usd"].Value = Convert.ToDouble(model.Rows[i]["total_price"].ToString()).ToString("#,##0.00");
                    dgvGuarantee.Rows[n].Cells["price_krw"].Value = (Convert.ToDouble(model.Rows[i]["total_price"].ToString()) * customRate).ToString("#,##0");
                    total_amount += Convert.ToDouble(model.Rows[i]["total_price"].ToString()) * customRate;
                }
                lbTotalPrice.Text = total_amount.ToString("#,##0");
            }
        }
        public void CheckNoPaper()
        {
            lbWidgetTitle.Text = "서류 미도착분";

            //Panel change
            MemoPanel.Dock = DockStyle.Fill;
            MemoPanel.Visible = true;
            MemoPanel.Controls.Clear();

            stockPanel.Dock = DockStyle.None;
            stockPanel.Visible = false;

            DataTable chkDt = customsRepository.CheckHOCO("2022-09-01", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            if (chkDt.Rows.Count > 0)
            {
                for (int i = 0; i < chkDt.Rows.Count; i++)
                {
                    UserControlNoPaper ucNoPaper = new UserControlNoPaper(this, um, chkDt.Rows[i]["id"].ToString(), chkDt.Rows[i]);
                    ucNoPaper.Width = 200;
                    ucNoPaper.Height = 20;
                    MemoPanel.Controls.Add(ucNoPaper);
                }
            }
        }

        public void ProblemData()
        {
            lbWidgetTitle.Text = "미결제 또는 확인필수 내역";

            //Panel change
            MemoPanel.Dock = DockStyle.Fill;
            MemoPanel.Visible = true;

            stockPanel.Dock = DockStyle.None;
            stockPanel.Visible = false;

            DateTime nowDt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            //List<CustomsModel> cm = customsRepository.GetCustomUnknown(new DateTime(1900,1,1), DateTime.Now.AddDays(-1),"","","","","","","");
            List<CustomsModel> cm = customsRepository.GetCustomUnknown2(nowDt, "","","","","");
            List<MemoModel> mm = memoRepository.GetTradeMemo2(nowDt);
            bool isOutput;

            MemoPanel.Controls.Clear();

            

            int cnt = 0;
            //pending(결제)
            if (cm != null)
            {
                cm = SetPayment(cm);
                for (int i = 0; i < cm.Count(); i++)
                {
                    isOutput = true;
                    DateTime payDate;
                    if (DateTime.TryParse(cm[i].payment_date, out payDate))
                    {
                        if (payDate >= nowDt)
                        {
                            isOutput = false;
                        }
                    }
                    //출력==============================================================================
                    if (isOutput
                        && cm[i].ato_no.Length > 0
                        && cm[i].ato_no.Substring(0, 2) != "ad" && cm[i].ato_no.Substring(0, 2) != "AD"
                        && cm[i].ato_no.Substring(0, 2) != "dw" && cm[i].ato_no.Substring(0, 2) != "DW"
                        && cm[i].ato_no.Substring(0, 2) != "jd" && cm[i].ato_no.Substring(0, 2) != "JD"
                        && cm[i].ato_no.Substring(0, 2) != "dl" && cm[i].ato_no.Substring(0, 2) != "DL"
                        && cm[i].ato_no.Substring(0, 2) != "hs" && cm[i].ato_no.Substring(0, 2) != "HS"
                        && cm[i].ato_no.Substring(0, 2) != "od" && cm[i].ato_no.Substring(0, 2) != "OD"
                        )
                    {
                        //title
                        if (cnt == 0)
                        {
                            Label payLabel = new Label();
                            payLabel.Width = 560;
                            payLabel.Height = 30;
                            payLabel.TextAlign = ContentAlignment.BottomLeft;
                            payLabel.Text = "결제 확인내역";
                            payLabel.Font = new Font("바탕", 15, FontStyle.Bold);
                            MemoPanel.Controls.Add(payLabel);
                            cnt++;
                        }
                        //data
                        if (cm[i].payment_date == null || cm[i].payment_date == "")
                        {
                            cm[i].payment_date = "[ETD 미기입]";
                        }
                        string str = cm[i].payment_date + "    $" + cm[i].total_amount.ToString("#,##0.00") + "  " + cm[i].ato_no + "  " + cm[i].payment_bank;
                        //구분추가
                        if (cm[i].division != null && cm[i].division != "")
                        {
                            str += "[" + cm[i].division.Trim() + "]";
                        }
                        str += " " + cm[i].manager;
                        if (cm[i].accuracy != "100")
                        {
                            str += "(" + cm[i].accuracy + "%)";
                        }
                        //정확도 수정
                        switch (cm[i].accuracy)
                        {
                            case "50":
                                cm[i].payment_date_status = "";
                                break;
                            case "60":
                                cm[i].payment_date_status = "";
                                break;
                            case "70":
                                cm[i].payment_date_status = "";
                                break;
                            case "80":
                                cm[i].payment_date_status = "미확정";
                                break;
                        }
                        UserControlPending ucPending = ucPending = new UserControlPending(year, month, this, um, 0, cm[i].payment_date);
                        ucPending.pending(str, cm[i].payment_date_status, cm[i].id, cm[i].usance_type, cm[i].product, cm[i].accuracy);
                        ucPending.Width = 500;
                        ucPending.Height = 20;
                        MemoPanel.Controls.Add(ucPending);
                    }
                }
            }
            //memo 내용(기타결제, 무역부)
            if (mm != null)
            {
                for (int i = 0; i < mm.Count(); i++)
                {
                    DateTime payDt = new DateTime(mm[i].syear, mm[i].smonth, mm[i].sday);
                    if (payDt < nowDt && mm[i].pay_status != "결제완료")
                    {
                        //title
                        if (cnt == 0)
                        {
                            Label payLabel = new Label();
                            payLabel.Width = 560;
                            payLabel.Height = 30;
                            payLabel.TextAlign = ContentAlignment.BottomLeft;
                            payLabel.Text = "결제 확인내역";
                            payLabel.Font = new Font("바탕", 15, FontStyle.Bold);
                            MemoPanel.Controls.Add(payLabel);
                            cnt++;
                        }

                        string str = mm[i].contents;
                        string temp = "";
                        if (mm[i].pay_amount > 0)
                        {
                            if (mm[i].currency == "USD")
                            {
                                temp = "$" + string.Format("{0:#,##0.00}", mm[i].pay_amount) + " " + mm[i].pay_bank;
                            }
                            else
                            {
                                temp = @"\" + string.Format("{0:#,##0}", mm[i].pay_amount) + " " + mm[i].pay_bank;
                            }
                        }
                        temp = temp.Trim();
                        if (temp != "")
                        {
                            str = payDt.ToString("yyyy-MM-dd") + "    (" + temp + ")" + str + " " + mm[i].manager;
                        }

                        UserControlDayMemo ucMemo = new UserControlDayMemo(mm[i], days, this, um);
                        ucMemo.Memo(str);
                        ucMemo.Width = 500;
                        ucMemo.Height = 20;
                        MemoPanel.Controls.Add(ucMemo);
                    }
                }
            }

            cnt = 0;
            DataTable pendingDt = customsRepository.GetProblemPending();
            if (pendingDt.Rows.Count > 0)
            {
                for (int i = 0; i < pendingDt.Rows.Count; i++)
                {
                    //title
                    if (cnt == 0)
                    {
                        Label pendingLabel = new Label();
                        pendingLabel.Width = 200;
                        pendingLabel.Height = 30;
                        pendingLabel.TextAlign = ContentAlignment.BottomLeft;
                        pendingLabel.Text = "통관 확인내역";
                        pendingLabel.Font = new Font("바탕", 15, FontStyle.Bold);
                        pendingLabel.ForeColor = Color.BlueViolet;
                        MemoPanel.Controls.Add(pendingLabel);
                        cnt++;
                    }
                    string str;
                    DateTime payDt;
                    if (pendingDt.Rows[i]["pending_date"] == null || !DateTime.TryParse(pendingDt.Rows[i]["pending_date"].ToString(), out payDt))
                        pendingDt.Rows[i]["pending_date"] = "입력값이 없음";

                    str = pendingDt.Rows[i]["ato_no"].ToString() + " " + pendingDt.Rows[i]["pending_date"].ToString() + " " + pendingDt.Rows[i]["manager"].ToString();

                    UserControlPending ucPending = new UserControlPending(year, month, this, um, 0, pendingDt.Rows[i]["pending_date"].ToString());

                    int id = Convert.ToInt32(pendingDt.Rows[i]["id"].ToString());

                    ucPending.pending(str, id, pendingDt.Rows[i]["product"].ToString(), Convert.ToInt32(pendingDt.Rows[i]["score"].ToString()));
                    ucPending.Width = 500;
                    ucPending.Height = 20;
                    MemoPanel.Controls.Add(ucPending);
                }
            }
        }

        

        public void GuaranteeStock()
        {
            //Title
            lbWidgetTitle.Text = "담보가능 재고";
            //Panel change
            MemoPanel.Dock = DockStyle.None;
            MemoPanel.Visible = false;

            stockPanel.Dock = DockStyle.Fill;
            stockPanel.Visible = true;
            //Call procedure 
            CallStockProcedure();
            //담보재고 씨오버 테이블
            DataTable gDt = stockRepository.GetGuarnteeStock();
            //담보재고 아토시스템 테이블
            DataTable aDt = customsRepository.GetCollateralAvailableProduct();

            var joinResult = (from p in gDt.AsEnumerable()
                              join t in aDt.AsEnumerable()
                              on p.Field<string>("AtoNo") equals t.Field<string>("ato_no")
                              select new
                              {
                                  매입처 = p.Field<string>("매입처"),
                                  AtoNo = p.Field<string>("AtoNo"),
                                  재고금액 = p.Field<string>("재고금액"),
                                  재고수 = p.Field<string>("재고수"),
                                  입고일자 = p.Field<string>("입고일자"),
                                  품명 = p.Field<string>("품명")
                              }).ToList();
            DataTable productDt = common.ConvertListToDatatable(joinResult);
            if (productDt != null)
                productDt.AcceptChanges();
            gDt = productDt;
            //제외 테이블
            DataTable hDt = hideRepository.GetHideTable(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), "","","","","", "담보가능재고");

            dgvGuarantee = new DataGridView();
            dgvGuarantee.AllowUserToDeleteRows = false;
            dgvGuarantee.AllowUserToAddRows = false;
            dgvGuarantee.RowHeadersVisible = false;
            dgvGuarantee.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvGuarantee.Font = new Font("나눔고딕", 7, FontStyle.Regular);
            dgvGuarantee.Width = MemoPanel.Width - 20;
            dgvGuarantee.Height = 500;
            //Columns
            dgvGuarantee.ColumnCount = 4;

            dgvGuarantee.Columns[0].Name = "ato_no";
            dgvGuarantee.Columns[0].HeaderText = "Ato no.";
            dgvGuarantee.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvGuarantee.Columns[1].Name = "amount";
            dgvGuarantee.Columns[1].HeaderText = "금액";
            dgvGuarantee.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvGuarantee.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            /*dgv.Columns[2].Name = "product";
            dgv.Columns[2].HeaderText = "품명 | 원산지 | 규격 | 단위";
            dgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;*/

            dgvGuarantee.Columns[2].Name = "qty";
            dgvGuarantee.Columns[2].HeaderText = "재고";
            dgvGuarantee.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvGuarantee.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGuarantee.Columns[3].Name = "in_date";
            dgvGuarantee.Columns[3].HeaderText = "입고일자";
            dgvGuarantee.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvGuarantee.ColumnHeadersDefaultCellStyle.BackColor = Color.Beige;
            dgvGuarantee.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGuarantee.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvGuarantee_MouseUp);
            dgvGuarantee.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGuarantee_CellMouseClick);

            dgvGuarantee.AllowUserToResizeRows = false;

            stockPanel.Controls.Add(dgvGuarantee);
            dgvGuarantee.Dock = DockStyle.Fill;
            dgvGuarantee.BringToFront();
            //Data
            if (gDt.Rows.Count > 0)
            {
                double total_amount = 0;
                int n;
                for (int i = 0; i < gDt.Rows.Count; i++)
                {
                    //ato_no
                    string ato_no = gDt.Rows[i]["매입처"].ToString().Substring(gDt.Rows[i]["매입처"].ToString().IndexOf('-') + 1, 5);
                    int tmpInt;
                    if (!int.TryParse(ato_no.Substring(ato_no.Length - 1, 1), out tmpInt))
                    {
                        ato_no = ato_no.Substring(0, 4);
                        //간혹 3자시 ato_no도 존재
                        if (!int.TryParse(ato_no.Substring(ato_no.Length - 1, 1), out tmpInt))
                        {
                            ato_no = ato_no.Substring(0, 3);
                        }
                    }
                    //제외품목 확인
                    bool isHide = false;
                    DataRow[] row = null;
                    if (hDt.Rows.Count > 0)
                    {
                        string whr = "category = '" + ato_no + "'";
                        row = hDt.Select(whr);
                        if (row.Length > 0)
                        {
                            isHide = true;
                        }
                    }
                    //출력
                    if (!isHide)
                    {
                        if (Convert.ToDouble(gDt.Rows[i]["재고수"].ToString()) > 0
                            && ato_no != "BG03"
                            && Convert.ToDouble(gDt.Rows[i]["재고금액"].ToString()) >= 10000000)
                        {
                            n = dgvGuarantee.Rows.Add();
                            //dgvGuarantee.Rows[n].Cells["ato_no"].Value = ato_no;
                            dgvGuarantee.Rows[n].Cells["ato_no"].Value = gDt.Rows[i]["AtoNo"].ToString();
                            dgvGuarantee.Rows[n].Cells["amount"].Value = Convert.ToDouble(gDt.Rows[i]["재고금액"].ToString()).ToString("#,##0");
                            dgvGuarantee.Rows[n].Cells["qty"].Value = Convert.ToDouble(gDt.Rows[i]["재고수"].ToString()).ToString("#,##0");
                            dgvGuarantee.Rows[n].Cells["in_date"].Value = Convert.ToDateTime(gDt.Rows[i]["입고일자"].ToString()).ToString("yyyy-MM-dd");

                            string product = gDt.Rows[i]["품명"].ToString().Replace(",", "\r\n");

                            dgvGuarantee.Rows[n].Cells[0].ToolTipText = product;
                            dgvGuarantee.Rows[n].Cells[1].ToolTipText = product;
                            dgvGuarantee.Rows[n].Cells[2].ToolTipText = product;
                            dgvGuarantee.Rows[n].Cells[3].ToolTipText = product;

                            total_amount += Convert.ToDouble(gDt.Rows[i]["재고금액"].ToString());
                        }
                    }        
                }
                //총 재고금액
                lbTotalPrice.Text = total_amount.ToString("#,##0");
            }
        }

        private void dgvGuarantee_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    //Selection
                    dgvGuarantee.ClearSelection();
                    DataGridViewRow selectRow = this.dgvGuarantee.Rows[e.RowIndex];
                    selectRow.Selected = !selectRow.Selected;
                }
            }
            catch 
            {
            }
        }
        //우클릭 메뉴 Create
        private void dgvGuarantee_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvGuarantee.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    ContextMenuStrip m = new ContextMenuStrip();

                    if (lbWidget.Text == "2" && (um.department.Contains("관리부") || um.department.Contains("전산") || um.department.Contains("경리") || um.department.Contains("무역")))
                    {
                        m.Items.Add("3일 뒤에 확인");
                        m.Items.Add("5일 뒤에 확인");
                        m.Items.Add("영원히 숨기기");
                        m.Items.Add("숨긴재고 확인");
                    }
                    else if (lbWidget.Text == "4")
                    {
                        if (row >= 3 && row <= 6)
                            m.Items.Add("상세내역");
                    }
                    else
                    {
                        m.Items.Add("팬딩내역");
                    }

                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(dgvGuarantee, e.Location);
                }
            }
            catch
            {}
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (dgvGuarantee.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow row = dgvGuarantee.SelectedRows[0];
                    if (row.Index < 0)
                    {
                        return;
                    }
                    List<StringBuilder> sqlList = new List<StringBuilder>();
                    StringBuilder sql = new StringBuilder();
                    HideModel hm = new HideModel();
                    switch (e.ClickedItem.Text)
                    {
                        case "3일 뒤에 확인":
                            hm.id = commonRepository.GetNextId("t_hide", "id");
                            hm.division = "담보가능재고";
                            hm.category = dgvGuarantee.SelectedRows[0].Cells["ato_no"].Value.ToString();
                            hm.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                            hm.edit_user = um.user_name;
                            hm.until_date = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd");

                            sql = hideRepository.InsertHide(hm);
                            sqlList.Add(sql);
                            if (hideRepository.UpdateTrans(sqlList) == -1)
                            {
                                messageBox.Show(this,"등록중 에러가 발생하였습니다.");
                            }
                            else
                            {
                                GuaranteeStock();
                            }
                            break;
                        case "5일 뒤에 확인":
                            hm.id = commonRepository.GetNextId("t_hide", "id");
                            hm.division = "담보가능재고";
                            hm.category = dgvGuarantee.SelectedRows[0].Cells["ato_no"].Value.ToString();
                            hm.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                            hm.edit_user = um.user_name;
                            hm.until_date = DateTime.Now.AddDays(5).ToString("yyyy-MM-dd");

                            sql = hideRepository.InsertHide(hm);
                            sqlList.Add(sql);
                            if (hideRepository.UpdateTrans(sqlList) == -1)
                            {
                                messageBox.Show(this,"등록중 에러가 발생하였습니다.");
                            }
                            else
                            {
                                GuaranteeStock();
                            }
                            break;
                        case "영원히 숨기기":
                            hm.id = commonRepository.GetNextId("t_hide", "id");
                            hm.division = "담보가능재고";
                            hm.category = dgvGuarantee.SelectedRows[0].Cells["ato_no"].Value.ToString();
                            hm.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                            hm.edit_user = um.user_name;
                            hm.until_date = DateTime.Now.AddDays(10000).ToString("yyyy-MM-dd");

                            sql = hideRepository.InsertHide(hm);
                            sqlList.Add(sql);
                            if (hideRepository.UpdateTrans(sqlList) == -1)
                            {
                                messageBox.Show(this,"등록중 에러가 발생하였습니다.");
                            }
                            else
                            {
                                GuaranteeStock();
                            }
                            break;
                        case "숨긴재고 확인":
                            CalendarModule.HideManager hManager = new HideManager(um);
                            //hManager.ShowDialog();
                            hManager.Show();
                            hManager.TopMost = true;
                            GuaranteeStock();
                            break;
                        case "팬딩내역":
                            Pending.UnPendingManager upm = new UnPendingManager(this, um, dgvGuarantee.SelectedRows[0].Cells["ato_no"].Value.ToString(), null);
                            upm.Show();
                            break;
                        case "상세내역":

                            int id = Convert.ToInt32(row.Cells["id"].Value.ToString());

                            StockDashboardDetail sdd = new StockDashboardDetail(id, this, um);
                            sdd.StartPosition = FormStartPosition.CenterParent;
                            sdd.TopMost = true;
                            sdd.Show();
                            break;
                    }
                }
                catch
                { }
            }
        }

        private void CallStockProcedure(bool isDash = false)
        {
            //품명별재고현황 스토어프로시져 호출
            try
            {
                string eDate = DateTime.Now.ToString("yyyy-MM-dd");
                string user_id = um.seaover_id;
                if (stockRepository.CallStoredProcedureSTOCK(user_id, eDate, isDash) == 0)
                {
                    messageBox.Show(this,"호출 내용이 없음");
                    return;
                }
            }
            catch (Exception e)
            {
                messageBox.Show(this,e.Message);
                return;
            }
        }

        private void TotalPaymentPrice(List<CustomsModel> cm, List<MemoModel> mm, List<CustomsModel> sm, List<CustomsModel> nm, List<MemoModel> smm
                                    , List<ArriveModel> am, List<InspectionModel> im, DataTable gDt)
        {
            //초기화
            lbTotalUsd.Text = "$0";
            lbTotalKrw.Text = @"\0";
            lbTotalConfirmUsd.Text = "$0";
            lbTotalConfirmKrw.Text = @"\0";
            lbBalanceUsd.Text = "$0";
            lbBalanceKrw.Text = @"\0";

            //재계산
            double krw = 0;
            double usd = 0;
            double cKrw = 0;
            double cUsd = 0;

            //결제내역
            if (cm != null)
            {
                for (int i = 0; i < cm.Count(); i++)
                {
                    DateTime tempDt = new DateTime();
                    tempDt = Convert.ToDateTime(cm[i].payment_date);
                    if (cm[i].ato_no.Length > 0)
                    { 
                        if (tempDt.Year == year && tempDt.Month == month
                            && cm[i].ato_no.Substring(0, 2) != "ad" && cm[i].ato_no.Substring(0, 2) != "AD"
                            && cm[i].ato_no.Substring(0, 2) != "dw" && cm[i].ato_no.Substring(0, 2) != "DW"
                            && cm[i].ato_no.Substring(0, 2) != "jd" && cm[i].ato_no.Substring(0, 2) != "JD")
                        {
                            usd = usd + Convert.ToDouble(cm[i].total_amount);

                            if (cm[i].payment_date_status == "결제완료")
                            {
                                cUsd = cUsd + Convert.ToDouble(cm[i].total_amount);
                            }
                        }
                    }
                }
            }
            //기타결제 내역(무역)
            if (mm != null)
            {
                for (int i = 0; i < mm.Count(); i++)
                {
                    DateTime tempDt = new DateTime(mm[i].syear, mm[i].smonth, mm[i].sday);

                    if (tempDt.Year == year && tempDt.Month == month)
                    { 
                        if (mm[i].currency == "KRW")
                        {
                            krw = krw + Convert.ToDouble(mm[i].pay_amount);
                        }
                        else if (mm[i].currency == "USD")
                        {
                            usd = usd + Convert.ToDouble(mm[i].pay_amount);
                        }

                        if (mm[i].pay_status == "결제완료")
                        {
                            if (mm[i].currency == "KRW")
                            {
                                cKrw = cKrw + Convert.ToInt32(mm[i].pay_amount);
                            }
                            else if (mm[i].currency == "USD")
                            {
                                cUsd = cUsd + Convert.ToDouble(mm[i].pay_amount);
                            }
                        }
                    }
                }
            }
            //미확인 내역
            if (nm != null)
            {
                for (int i = 0; i < nm.Count(); i++)
                {
                    DateTime tempDt = new DateTime();

                    if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) > Convert.ToDateTime(nm[i].payment_date))
                    {
                        tempDt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        tempDt = tempDt.AddDays(3);
                    }
                    else
                    {
                        tempDt = Convert.ToDateTime(nm[i].payment_date);
                    }


                    if (tempDt.Year == year && tempDt.Month == month)
                    {
                        if (nm[i].ato_no.Substring(0, 2) != "ad" && nm[i].ato_no.Substring(0, 2) != "AD"
                            && nm[i].ato_no.Substring(0, 2) != "dw" && nm[i].ato_no.Substring(0, 2) != "DW"
                            && nm[i].ato_no.Substring(0, 2) != "jd" && nm[i].ato_no.Substring(0, 2) != "JD")
                        {
                            usd = usd + Convert.ToDouble(nm[i].total_amount);
                        }
                    }

                }
            }

            //엮을품목 일정
            if (gDt != null)
            {
                for (int i = 0; i < gDt.Rows.Count; i++)
                {
                    DateTime tempDt = new DateTime();
                    if (DateTime.TryParse(gDt.Rows[i]["guarantee_date"].ToString(), out tempDt))
                    {
                        string atoNo = gDt.Rows[i]["ato_no"].ToString();
                        if (tempDt.Year == year && tempDt.Month == month && tempDt >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                        {
                            if (atoNo.Substring(0, 2) != "ad" && atoNo.Substring(0, 2) != "AD"
                                && atoNo.Substring(0, 2) != "dw" && atoNo.Substring(0, 2) != "DW"
                                && atoNo.Substring(0, 2) != "jd" && atoNo.Substring(0, 2) != "JD")
                            {
                                krw = krw + Convert.ToDouble(gDt.Rows[i]["total_amount"].ToString()) * 0.7;
                            }
                        }
                    }
                }
            }


            // TOTAL 결제금액 출력
            if (usd > 0)
                lbTotalUsd.Text = "$" + usd.ToString("#,##0.00");
            if (krw > 0)
                lbTotalKrw.Text = @"\" + krw.ToString("#,##0");
            if (cUsd > 0)
                lbTotalConfirmUsd.Text = "$" + cUsd.ToString("#,##0.00");
            if (cKrw > 0)
                lbTotalConfirmKrw.Text = @"\" + cKrw.ToString("#,##0");
            if (usd > 0 || cUsd > 0)
                lbBalanceUsd.Text = "$" + (usd - cUsd).ToString("#,##0.00");
            if (krw > 0 || cKrw > 0)
                lbBalanceKrw.Text = @"\" + (krw - cKrw).ToString("#,##0");
        }

        private List<CustomsModel> SetPayment(List<CustomsModel> model)
        {
            int[] no1, no2;
            string[] name1, name2;
            DateTime tempDt;
            DateTime tempDt1 = new DateTime(year, month, 1);
            DateTime tempDt2 = new DateTime(year, month, 1).AddDays(-1);
            common.getRedDay(tempDt1.Year, tempDt1.Month, out no1, out name1);
            common.getRedDay(tempDt2.Year, tempDt2.Month, out no2, out name2);
            //주말일경우 날짜 수정
            for (int i = 0; i < model.Count(); i++)
            {
                if (DateTime.TryParse(model[i].payment_date, out tempDt))
                { 
                retry:
                    //주말일 경우 수정
                    if (tempDt.DayOfWeek == DayOfWeek.Saturday)
                    {
                        tempDt = tempDt.AddDays(2);
                    }
                    else if (tempDt.DayOfWeek == DayOfWeek.Sunday)
                    {
                        tempDt = tempDt.AddDays(1);
                    }
                    model[i].payment_date = tempDt.ToString("yyyy-MM-dd");

                    //법정공휴일일 경우
                    if (tempDt.Month == tempDt1.Month)
                    {
                        foreach (int n in no1)
                        {
                            if (n == tempDt.Day)
                            {
                                tempDt = tempDt.AddDays(1);
                                goto retry;
                            }
                        }
                    }
                    else
                    {
                        foreach (int n in no2)
                        {
                            if (n == tempDt.Day)
                            {
                                tempDt = tempDt.AddDays(1);
                                goto retry;
                            }
                        }
                    }
                }
            }
            return model;
        }
        private DataTable SetGuarantee(DataTable model, int year, int month)
        {
            int overDays;
            if (rbInsection1.Checked)
            {
                overDays = 4;
            }
            else
            {
                overDays = 7;
            }

            int[] no1, no2;
            string[] name1, name2;
            DateTime tempDt;
            DateTime tempDt1 = new DateTime(year, month, 1);
            DateTime tempDt2 = new DateTime(year, month, 1).AddDays(-1);
            common.getRedDay(tempDt1.Year, tempDt1.Month, out no1, out name1);
            common.getRedDay(tempDt2.Year, tempDt2.Month, out no2, out name2);
            //주말일경우 날짜 수정
            for (int i = 0; i < model.Rows.Count; i++)
            {
                double total_amount = Convert.ToDouble(model.Rows[i]["total_amount"].ToString()) * customRate;
                model.Rows[i]["total_amount"] = total_amount;


                if (DateTime.TryParse(model.Rows[i]["eta"].ToString(), out tempDt))
                {
                    common.GetPlusDay(tempDt, overDays, out tempDt);
                retry:
                    //주말일 경우 수정
                    if (tempDt.DayOfWeek == DayOfWeek.Saturday)
                    {
                        tempDt = tempDt.AddDays(2);
                    }
                    else if (tempDt.DayOfWeek == DayOfWeek.Sunday)
                    {
                        tempDt = tempDt.AddDays(1);
                    }

                    //법정공휴일일 경우
                    if (tempDt.Month == tempDt1.Month)
                    {
                        foreach (int n in no1)
                        {
                            if (n == tempDt.Day)
                            {
                                tempDt = tempDt.AddDays(1);
                                goto retry;
                            }
                        }
                    }
                    else
                    {
                        foreach (int n in no2)
                        {
                            if (n == tempDt.Day)
                            {
                                tempDt = tempDt.AddDays(1);
                                goto retry;
                            }
                        }
                    }
                    model.Rows[i]["guarantee_date"] = tempDt.ToString("yyyy-MM-dd");
                }
                else
                {
                    model.Rows[i]["guarantee_date"] = null;
                }
            }
            return model;
        }

        private DataTable SetAlarm(DataTable dt, int year, int month)
        {
            int[] no1, no2;
            string[] name1, name2;
            DateTime tempDt;
            DateTime tempDt1 = new DateTime(year, month, 1);
            DateTime tempDt2 = new DateTime(year, month, 1).AddDays(-1);
            common.getRedDay(tempDt1.Year, tempDt1.Month, out no1, out name1);
            common.getRedDay(tempDt2.Year, tempDt2.Month, out no2, out name2);
            //주말일경우 날짜 수정
            dt.Columns["alarm_date"].ReadOnly = false;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tempDt = Convert.ToDateTime(dt.Rows[i]["alarm_date"].ToString());
            retry:
                //주말일 경우 수정
                if (tempDt.DayOfWeek == DayOfWeek.Saturday)
                {
                    tempDt = tempDt.AddDays(2);
                }
                else if (tempDt.DayOfWeek == DayOfWeek.Sunday)
                {
                    tempDt = tempDt.AddDays(1);
                }
                dt.Rows[i]["alarm_date"] = tempDt.ToString("yyyy-MM-dd");

                //법정공휴일일 경우
                if (tempDt.Month == tempDt1.Month)
                {
                    foreach (int n in no1)
                    {
                        if (n == tempDt.Day)
                        {
                            tempDt = tempDt.AddDays(1);
                            goto retry;
                        }
                    }
                }
                else
                {
                    foreach (int n in no2)
                    {
                        if (n == tempDt.Day)
                        {
                            tempDt = tempDt.AddDays(1);
                            goto retry;
                        }
                    }
                }
            }
            dt.AcceptChanges();
            return dt;
        }

        private List<ArriveModel> SetArrive(List<ArriveModel> model, int year, int month)
        {
            int[] no1, no2;
            string[] name1, name2;
            DateTime tempDt;
            DateTime tempDt1 = new DateTime(year, month, 1);
            DateTime tempDt2 = new DateTime(year, month, 1).AddDays(-1);
            common.getRedDay(tempDt1.Year, tempDt1.Month, out no1, out name1);
            common.getRedDay(tempDt2.Year, tempDt2.Month, out no2, out name2);
            //주말일경우 날짜 수정
            for (int i = 0; i < model.Count(); i++)
            {
                tempDt = Convert.ToDateTime(model[i].pending_date);
            retry:
                //주말일 경우 수정
                if (tempDt.DayOfWeek == DayOfWeek.Saturday)
                {
                    tempDt = tempDt.AddDays(2);
                }
                else if (tempDt.DayOfWeek == DayOfWeek.Sunday)
                {
                    tempDt = tempDt.AddDays(1);
                }
                model[i].pending_date = tempDt.ToString("yyyy-MM-dd");

                //법정공휴일일 경우
                if (tempDt.Month == tempDt1.Month)
                {
                    foreach (int n in no1)
                    {
                        if (n == tempDt.Day)
                        {
                            tempDt = tempDt.AddDays(1);
                            goto retry;
                        }
                    }
                }
                else
                {
                    foreach (int n in no2)
                    {
                        if (n == tempDt.Day)
                        {
                            tempDt = tempDt.AddDays(1);
                            goto retry;
                        }
                    }
                }
            }
            return model;
        }

        private List<InspectionModel> SetInspection(List<InspectionModel> model)
        {
            int[] no1, no2;
            string[] name1, name2;
            DateTime tempDt;
            DateTime tempDt1 = new DateTime(year, month, 1);
            DateTime tempDt2 = new DateTime(year, month, 1).AddDays(-1);
            common.getRedDay(tempDt1.Year, tempDt1.Month, out no1, out name1);
            common.getRedDay(tempDt2.Year, tempDt2.Month, out no2, out name2);
            //주말일경우 날짜 수정
            for (int i = 0; i < model.Count(); i++)
            {
                tempDt = Convert.ToDateTime(model[i].warehousing_date);
            retry:
                //주말일 경우 수정
                if (tempDt.DayOfWeek == DayOfWeek.Saturday)
                {
                    tempDt = tempDt.AddDays(2);
                }
                else if (tempDt.DayOfWeek == DayOfWeek.Sunday)
                {
                    tempDt = tempDt.AddDays(1);
                }
                model[i].warehousing_date = tempDt.ToString("yyyy-MM-dd");

                //법정공휴일일 경우
                if (tempDt.Month == tempDt1.Month)
                {
                    foreach (int n in no1)
                    {
                        if (n == tempDt.Day)
                        {
                            tempDt = tempDt.AddDays(1);
                            goto retry;
                        }
                    }
                }
                else
                {
                    foreach (int n in no2)
                    {
                        if (n == tempDt.Day)
                        {
                            tempDt = tempDt.AddDays(1);
                            goto retry;
                        }
                    }
                }

            }
            return model;
        }
        #endregion

        #region 달력이동 버튼
        private void btnWidgetRight_Click(object sender, EventArgs e)
        {
            int idx = lbWidgetTitle.Items.Count;
            int id = Convert.ToInt16(lbWidget.Text);
            if (id + 1 <= idx)
            {
                lbWidget.Text = (id + 1).ToString();
            }
            else
            {
                lbWidget.Text = "1";
            }
            displayWidget();
        }

        private void btnWidgetLeft_Click(object sender, EventArgs e)
        {
            int idx = lbWidgetTitle.Items.Count;
            int id = Convert.ToInt16(lbWidget.Text);
            if (id - 1 > 0)
            {
                lbWidget.Text = (id - 1).ToString();
            }
            else
            {
                lbWidget.Text = idx.ToString();
            }
            displayWidget();
        }
        private void btnPre_Click(object sender, EventArgs e)
        {
            month--;
            if (month < 1)
            {
                year--;
                month = 12;
            }
            displayDays(year, month, 0, "", "", "", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtManager.Text);
            //displayMemo(year, month);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            month++;
            if (month > 12)
            {
                year++;
                month = 1;
            }
            displayDays(year, month, 0, "", "", "", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtManager.Text);
            //displayMemo(year, month);
        }
        #endregion

        #region Key event, 새로고침 // F1 ~ F8
        private void txtManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                displayDays(year, month, 0, "", "", "", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtManager.Text);
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Libs.Tools.Common common = new Libs.Tools.Common();
            Control tb = common.FindFocusedControl(this);
            {
                switch (keyData)
                {
                    case Keys.Left:
                        tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
                        return true;
                    case Keys.Right:
                        tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                        return true;
                    case Keys.Up:
                        tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
                        return true;
                    case Keys.Down:
                        tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                        return true;
                }

                return base.ProcessCmdKey(ref msg, keyData);
            }
        }
        private void calendar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        Bitmap bt = common.ScreenCaptureForm(new Point(this.Left, this.Top), this.Size);
                        Clipboard.SetImage(bt);
                        break;
                    case Keys.F12:
                        {
                            MiniGame.ListManager lm = new MiniGame.ListManager();
                            lm.Show();
                        }
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    //찾기
                    case Keys.F:
                        {
                            //초기화
                            findIdx = -1;
                            //찾기
                            FindData fd = new FindData(um, this);
                            //fd.ShowDialog();
                            fd.Show();
                            fd.TopMost = true;
                            break;
                        }
                }
            }
            else if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        {
                            displayDays(year, month, 0, "", "", "", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtManager.Text);
                            displayWidget();
                            break;
                        }
                    case Keys.M:
                        {
                            txtProduct.Focus();
                            break;
                        }
                    case Keys.N:
                        {
                            txtProduct.Text = String.Empty;
                            txtOrigin.Text = String.Empty;
                            txtSizes.Text = String.Empty;
                            txtManager.Text = String.Empty;
                            txtProduct.Focus();
                            break;
                        }
                    case Keys.D1:
                        OpenMenuAutoKey(pnFavoriteMenu.Controls[0].Name, 1);
                        break;
                    case Keys.D2:
                        OpenMenuAutoKey(pnFavoriteMenu.Controls[0].Name, 2);
                        break;
                    case Keys.D3:
                        OpenMenuAutoKey(pnFavoriteMenu.Controls[0].Name, 3);
                        break;
                    case Keys.D4:
                        OpenMenuAutoKey(pnFavoriteMenu.Controls[0].Name, 4);
                        break;
                    case Keys.D5:
                        OpenMenuAutoKey(pnFavoriteMenu.Controls[0].Name, 5);
                        break;
                    case Keys.D6:
                        OpenMenuAutoKey(pnFavoriteMenu.Controls[0].Name, 6);
                        break;
                    case Keys.D7:
                        OpenMenuAutoKey(pnFavoriteMenu.Controls[0].Name, 7);
                        break;
                    case Keys.D8:
                        OpenMenuAutoKey(pnFavoriteMenu.Controls[0].Name, 8);
                        break;
                    case Keys.D9:
                        OpenMenuAutoKey(pnFavoriteMenu.Controls[0].Name, 9);
                        break;
                }
            }
            else
            {
                if (e.KeyData == Keys.F8)
                {
                    if (!this.cbGuarantee.Checked)
                    {
                        cbGuarantee.Checked = false;
                        cbPayment.Checked = false;
                        cbMemoPayment.Checked = false;
                        cbSaleMemo.Checked = false;
                        cbInspection.Checked = false;
                        cbArrive.Checked = false;
                    }
                    this.cbGuarantee.Checked = !this.cbGuarantee.Checked;
                    displayDays(year, month);
                }
                else if (e.KeyData == Keys.F1
                    || e.KeyData == Keys.F2
                    || e.KeyData == Keys.F3
                    || e.KeyData == Keys.F4
                    || e.KeyData == Keys.F5
                    || e.KeyData == Keys.F6
                    || e.KeyData == Keys.F7
                    || e.KeyData == Keys.F9
                    || e.KeyData == Keys.F10
                    || e.KeyData == Keys.F11
                    || e.KeyData == Keys.F12
                    )
                {
                    this.cbGuarantee.Checked = false;

                    //선적 콤보박스 클릭
                    if (e.KeyData == Keys.F1)
                    {
                        this.cbShipment.Checked = !this.cbShipment.Checked;
                    }
                    //결제 콤보박스 클릭
                    else if (e.KeyData == Keys.F2)
                    {
                        this.cbPayment.Checked = !this.cbPayment.Checked;
                    }
                    else if (e.KeyData == Keys.F3)
                    {
                        this.cbMemoPayment.Checked = !this.cbMemoPayment.Checked;
                    }
                    //새로고침
                    else if (e.KeyData == Keys.F5)
                    {

                    }
                    //영업메모 클릭
                    else if (e.KeyData == Keys.F4)
                    {
                        this.cbSaleMemo.Checked = !this.cbSaleMemo.Checked;
                    }
                    //입고내역 클릭
                    else if (e.KeyData == Keys.F6)
                    {
                        this.cbArrive.Checked = !this.cbArrive.Checked;
                    }
                    //검품내역 클릭
                    else if (e.KeyData == Keys.F7)
                    {
                        this.cbInspection.Checked = !this.cbInspection.Checked;
                    }
                    //엮을 품목 일정
                    else if (e.KeyData == Keys.F8)
                    {
                        this.cbGuarantee.Checked = !cbGuarantee.Checked;
                    }

                    displayDays(year, month, 0, "", "", "", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtManager.Text);
                    displayWidget();
                }
            }
        }

        #endregion

        #region 조회 체크박스
        private void cbShipment_Click(object sender, EventArgs e)
        {
            displayDays(year, month, 0, "", "", "", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtManager.Text);
        }
        private void cbPayment_Click(object sender, EventArgs e)
        {
            displayDays(year, month, 0, "", "", "", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtManager.Text);
        }

        private void cbMemo_Click(object sender, EventArgs e)
        {
            displayDays(year, month, 0, "", "", "", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtManager.Text);
        }

        private void cbUnknwon_Click(object sender, EventArgs e)
        {
            displayDays(year, month, 0, "", "", "", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtManager.Text);
        }
        private void cbSaleMemo_Click(object sender, EventArgs e)
        {
            displayDays(year, month, 0, "", "", "", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtManager.Text);
        }
        private void cbArrive_Click(object sender, EventArgs e)
        {
            displayDays(year, month, 0, "", "", "", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtManager.Text);
        }
        private void cbInspection_Click(object sender, EventArgs e)
        {
            displayDays(year, month);
        }
        private void rbInsection2_Click(object sender, EventArgs e)
        {
            displayDays(year, month);
        }

        private void cbMemoPayment_Click(object sender, EventArgs e)
        {
            displayDays(year, month, 0, "", "", "", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtManager.Text);
        }
        private void cbGuarantee_CheckedChanged(object sender, EventArgs e)
        {
            if (cbGuarantee.Checked)
            {
                cbShipment.Checked = false;
                cbPayment.Checked = false;
                cbMemoPayment.Checked = false;
                cbSaleMemo.Checked = false;
                cbInspection.Checked = false;
                cbArrive.Checked = false;
                rbInsection1.Visible = true;
                rbInsection2.Visible = true;

                lbWidget.Text = "2";
                displayWidget();
            }
            else
            {
                rbInsection1.Visible = false;
                rbInsection2.Visible = false;
            }

            displayDays(year, month);
        }
        private void btnAddMemo_Click(object sender, EventArgs e)
        {
            UsersMemo uMemo = new UsersMemo(year, month, 0, this, false, null, um);
            uMemo.StartPosition = FormStartPosition.CenterParent;
            //uMemo.ShowDialog();
            uMemo.Show();
            uMemo.TopMost = true;
            //displayMemo(year, month);
        }
        private void LBDATE_Click(object sender, EventArgs e)
        {
            /*Common.Calendar cal = new Common.Calendar();
            string sDate = cal.GetDate(true);*/
            Common.YearAndMonthSelect yms = new Common.YearAndMonthSelect();
            string sDate = yms.GetYearAndMonth();

            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                year = tmpDate.Year;
                month = tmpDate.Month;
                displayDays(year, month, 0, "", "", "", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtManager.Text);
                //displayMemo(year, month);
            }
        }
        private void btnLoan_Click(object sender, EventArgs e)
        {
            LoanManager.LoanManger lm = new LoanManager.LoanManger();
            //lm.ShowDialog();
            lm.Show();

            lm.TopMost = true;
        }
        private void btnSetting_Click(object sender, EventArgs e)
        {
            Config.FavoriteMenuSettingManager fmsm = new Config.FavoriteMenuSettingManager(this, um);
            fmsm.Show();
        }
        #endregion

        #region tsm click event
        private void 관리자설정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminConfigManager acm = new AdminConfigManager(um, this);
            acm.ShowDialog();
        }
        //선적
        private void tsmShipment_Click(object sender, EventArgs e)
        {
            int results = -1;
            //Checked 설정내역
            cm = configRepository.GetConfigChecked(userId);
            if (cm != null)
            {
                if (!cm.shipment_checked)
                {
                    results = configRepository.updateConfig(userId, "shipment_checked", "TRUE");
                    tsmShipment.CheckState = CheckState.Checked;
                }
                else
                {
                    results = configRepository.updateConfig(userId, "shipment_checked", "FALSE");
                    tsmShipment.CheckState = CheckState.Unchecked;
                }
            }
            else
            {
                results = configRepository.InsertConfig(userId, "shipment_checked", "TRUE");
                tsmShipment.CheckState = CheckState.Checked;
            }
            //예외처리
            if (results == -1)
            {
                messageBox.Show(this,"설정 중 에러가 발생하였습니다.");
            }
        }
        //선적
        private void tsmPayment_Click(object sender, EventArgs e)
        {          
            int results = -1;
            //Checked 설정내역
            cm = configRepository.GetConfigChecked(userId);
            if (cm != null)
            {
                if (!cm.payment_checked)
                {
                    results = configRepository.updateConfig(userId, "payment_checked", "TRUE");
                    tsmPayment.CheckState = CheckState.Checked;
                }
                else
                {
                    results = configRepository.updateConfig(userId, "payment_checked", "FALSE");
                    tsmPayment.CheckState = CheckState.Unchecked;
                }
            }
            else
            {
                results = configRepository.InsertConfig(userId, "payment_checked", "TRUE");
                tsmPayment.CheckState = CheckState.Checked;
            }
            //예외처리
            if (results == -1)
            {
                messageBox.Show(this,"설정 중 에러가 발생하였습니다.");
            }
        }
        //영업 메모
        private void tsmSaleMemo_Click(object sender, EventArgs e)
        {
            int results = -1;
            //Checked 설정내역
            cm = configRepository.GetConfigChecked(userId);
            if (cm != null)
            {
                if (!cm.salememo_checked)
                {
                    results = configRepository.updateConfig(userId, "salememo_checked", "TRUE");
                    tsmSaleMemo.CheckState = CheckState.Checked;
                }
                else
                {
                    results = configRepository.updateConfig(userId, "salememo_checked", "FALSE");
                    tsmSaleMemo.CheckState = CheckState.Unchecked;
                }
            }
            else
            {
                results = configRepository.InsertConfig(userId, "salememo_checked", "TRUE");
                tsmSaleMemo.CheckState = CheckState.Checked;
            }
            //예외처리
            if (results == -1)
            {
                messageBox.Show(this,"설정 중 에러가 발생하였습니다.");
            }
        }

        //미확인결제
        private void tsmUnknown_Click(object sender, EventArgs e)
        {
            int results = -1;
            //Checked 설정내역
            cm = configRepository.GetConfigChecked(userId);
            if (cm != null)
            {
                if (!cm.memo_checked)
                {
                    results = configRepository.updateConfig(userId, "memo_checked", "TRUE");
                    tsmUnknown.CheckState = CheckState.Checked;
                }
                else
                {
                    results = configRepository.updateConfig(userId, "memo_checked", "FALSE");
                    tsmUnknown.CheckState = CheckState.Unchecked;
                }
            }
            else
            {
                results = configRepository.InsertConfig(userId, "memo_checked", "TRUE");
                tsmUnknown.CheckState = CheckState.Checked;
            }
            //예외처리
            if (results == -1)
            {
                messageBox.Show(this,"설정 중 에러가 발생하였습니다.");
            }
        }
        //입고일정 설정
        private void tsmArrive_Click(object sender, EventArgs e)
        {
            int results = -1;
            //Checked 설정내역
            cm = configRepository.GetConfigChecked(userId);
            if (cm != null)
            {
                if (!cm.arrive_checked)
                {
                    results = configRepository.updateConfig(userId, "arrive_checked", "TRUE");
                    tsmArrive.CheckState = CheckState.Checked;
                }
                else
                {
                    results = configRepository.updateConfig(userId, "arrive_checked", "FALSE");
                    tsmArrive.CheckState = CheckState.Unchecked;
                }
            }
            else
            {
                results = configRepository.InsertConfig(userId, "arrive_checked", "TRUE");
                tsmArrive.CheckState = CheckState.Checked;
            }
            //예외처리
            if (results == -1)
            {
                messageBox.Show(this,"설정 중 에러가 발생하였습니다.");
            }
        }
        //검품일정 설정
        private void tsmInspection_Click(object sender, EventArgs e)
        {
            int results = -1;
            //Checked 설정내역
            cm = configRepository.GetConfigChecked(userId); 
            if (cm != null)
            {
                if (!cm.inspection_checked)
                {
                    results = configRepository.updateConfig(userId, "inspection_checked", "TRUE");
                    tsmInspection.CheckState = CheckState.Checked;
                }
                else
                {
                    results = configRepository.updateConfig(userId, "inspection_checked", "FALSE");
                    tsmInspection.CheckState = CheckState.Unchecked;
                }
            }
            else
            {
                results = configRepository.InsertConfig(userId, "inspection_checked", "TRUE");
                tsmInspection.CheckState = CheckState.Checked;
            }
            //예외처리
            if (results == -1)
            {
                messageBox.Show(this,"설정 중 에러가 발생하였습니다.");
            }
        }



        #endregion

        #region 상단메뉴 버튼

        private void 매입단가그래프ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                //if (frm.Name == "RecoveryPrincipalManager")
                if (frm.Name == "GraphManager.cs")
                {
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                    isFormActive = true;
                }
            }
            //새로열기
            if (!isFormActive)
            {
                PurchaseManager.GraphManager gm = new PurchaseManager.GraphManager(um);
                gm.Show();
            }
        }
        private void 간이품목서ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "SimpleHandlingFormManager")
                {
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                    isFormActive = true;
                }
            }
            //새로열기
            if (!isFormActive)
            {
                //Seaover 사번이 없는경우 수정
                if (um.seaover_id == null || string.IsNullOrEmpty(um.seaover_id))
                {
                    messageBox.Show(this,"내정보에서 SEAOVER 사번을 입력해주세요.");
                    Config.EditMyInfo emi = new Config.EditMyInfo(um, this);
                    um = emi.UpdateSeaoverId();
                }
                //Seaover 사번이 있을 경우만 
                if (um.seaover_id != null && !string.IsNullOrEmpty(um.seaover_id))
                {
                    SimpleHandlingFormManager shfm = new SimpleHandlingFormManager(um);
                    shfm.Show();
                }
            }
        }
        //계약내역 리스트
        private void 미통관내역ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "UnconfirmedPending")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            if (!isFormActive)
            {
                UnconfirmedPending up = new UnconfirmedPending(userId, this);
                up.Show();
                up.WindowState = FormWindowState.Maximized;
            }
        }

        private void 팬딩조회2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "PendingList2")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            if (!isFormActive)
            {
                PendingList2 pl2 = new PendingList2(um);
                pl2.Show();
            }
        }
        //로그아웃 메뉴
        private void 로그아웃ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (messageBox.Show(this, "로그아웃 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Hide();
                Logins login = new Logins();
                login.Show();
                Program.ac.MainForm = login;
                this.Dispose();
                try
                {
                    foreach (Form openForm in Application.OpenForms)
                    {
                        if (openForm.Name != "Logins")
                        {
                            openForm.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
        }
        //시작프로그램 등록
        private void 활성화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (messageBox.Show(this, "시작프로그램으로 등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Libs.Tools.Common common = new Libs.Tools.Common();
                common.AddStartupProgram("AtoSystem", Application.ExecutablePath);
            }
        }
        //시작프로그램 해제
        private void 비활성화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (messageBox.Show(this, "시작프로그램을 해제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Libs.Tools.Common common = new Libs.Tools.Common();
                common.RemoveStartupProgram("AtoSystem");
            }
        }
        private void 검품리스트ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Arrive.InspectionList ispl = new Arrive.InspectionList(this, um);
            //ispl.ShowDialog();
            ispl.Show();
        }
        private void 두줄품목서ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "_2LineForm")
                {
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                    isFormActive = true;
                }
            }
            //새로열기
            if (!isFormActive)
            {
                //Seaover 사번이 없는경우 수정
                if (um.seaover_id == null || string.IsNullOrEmpty(um.seaover_id))
                {
                    messageBox.Show(this,"내정보에서 SEAOVER 사번을 입력해주세요.");
                    Config.EditMyInfo emi = new Config.EditMyInfo(um, this);
                    um = emi.UpdateSeaoverId();
                }
                //Seaover 사번이 있을 경우만 
                if (um.seaover_id != null && !string.IsNullOrEmpty(um.seaover_id))
                {
                    _2LineForm pm = new _2LineForm(14, um);
                    pm.Show();
                    pm.WindowState = FormWindowState.Maximized;
                }
            }
        }
        private void 사용자관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginForm.UsersManager um = new UsersManager();
            um.StartPosition = FormStartPosition.CenterParent;
            //um.ShowDialog();
            um.Show();
            um.TopMost = true;
        }
        private void 내정보수정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config.EditMyInfo ei = new Config.EditMyInfo(um, this);
            //ei.ShowDialog();
            ei.Show();
            ei.TopMost = true;
        }
        private void 업체별시세현황ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //이미 열려있는 폼이 있는지 확인
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "PriceComparison")
                {
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                    isFormActive = true;
                }
            }
            //새로열기
            if (!isFormActive)
            {
                try
                {
                    //Seaover 사번이 없는경우 수정
                    if (um.seaover_id == null || string.IsNullOrEmpty(um.seaover_id))
                    {
                        messageBox.Show(this,"내정보에서 SEAOVER 사번을 입력해주세요.");
                        Config.EditMyInfo emi = new Config.EditMyInfo(um, this);
                        um = emi.UpdateSeaoverId();
                    }
                    //Seaover 사번이 있을 경우만 
                    if (um.seaover_id != null && !string.IsNullOrEmpty(um.seaover_id))
                    {
                        SEAOVER.PriceComparison.PriceComparison pc = new SEAOVER.PriceComparison.PriceComparison(um);
                        pc.Show();
                        pc.WindowState = FormWindowState.Maximized;
                    }
                }
                catch (Exception ee)
                {
                }
            }
        }
        private void 팬딩등록ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "UnpendingAddManager")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            if (!isFormActive)
            {
                UnpendingAddManager uam = new UnpendingAddManager(um);
                uam.Show();
            }
        }
        private void 품목별매입단가ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "PurchaseUnitManager")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            if (!isFormActive)
            {
                PurchaseManager.PurchaseUnitManager pum = new PurchaseManager.PurchaseUnitManager(um);
                pum.Show();
            }

        }
        private void 통관미처리품목ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConvertToPending ctp = new ConvertToPending(um);
            ctp.CalendarOpenAlarm(false);
        }

        private void 씨오버미확인품목ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConvertProductToSeaover cpts = new ConvertProductToSeaover(um);
            cpts.CalendarOpenAlarm(false);
        }

        private void 계약ETD임박품목ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlmostOutOfStockManager aosm = new AlmostOutOfStockManager(this, um);
            aosm.Show();
        }
        private void 원가계산ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PurchaseManager.CostAccounting ca = new PurchaseManager.CostAccounting(um);
            ca.Show();
        }
        private void 거래처별매입단가일괄등록ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "PurchaseUnitPriceInfo")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            if (!isFormActive)
            {
                PurchaseManager.PurchaseUnitPriceInfo pup = new PurchaseManager.PurchaseUnitPriceInfo(um);
                pup.Show();
            }
        }
        private void 거래처관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "CompanyManager")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            if (!isFormActive)
            {
                Company.CompanyManager cm = new Company.CompanyManager(um);
                cm.Show();
            }
        }
        //수입수출리스트 열기
        private void 수출입현황ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "ImportExport")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            if (!isFormActive)
            {
                ImportExport ie = new ImportExport();
                ie.StartPosition = FormStartPosition.CenterParent;
                ie.Show();
            }
        }
        private void 수입식품허가정보ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "ImportProductPermitInfo")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            if (!isFormActive)
            {
                ImportProductPermitInfo hc = new ImportProductPermitInfo();
                hc.StartPosition = FormStartPosition.CenterParent;
                hc.Show();
            }
        }
        private void hACCP업체정보ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "HACCPCompany")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            if (!isFormActive)
            {
                HACCPCompany hc = new HACCPCompany();
                hc.StartPosition = FormStartPosition.CenterParent;
                hc.Show();
            }
        }

        private void 식품제조가공업정보ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "ProcessingCompany")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            if (!isFormActive)
            {
                ProcessingCompany hc = new ProcessingCompany();
                hc.StartPosition = FormStartPosition.CenterParent;
                hc.Show();
            }
        }
        private void 수출업소대장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "IncomeExportCompany")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            if (!isFormActive)
            {
                IncomeExportCompany iec = new IncomeExportCompany();
                iec.StartPosition = FormStartPosition.CenterParent;
                iec.Show();
            }
        }

        //국가별 배송기간 설정열기
        private void 국가별배송기간ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "CountryConfig")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            if (!isFormActive)
            {
                Config.CountryConfig country = new Config.CountryConfig(um);
                country.Show();
            }
        }
        private void 원금회수율ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "SalesPartnerManager")
                {
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                    isFormActive = true;
                }
            }
            //새로열기
            if (!isFormActive)
            {
                RecoveryPrincipal.SalesPartnerManager rpm = new RecoveryPrincipal.SalesPartnerManager(um);
                rpm.Show();
            }
        }
        private void 조업시기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "ContractRecommendationManager2")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            if (!isFormActive)
            {
                Product.ContractRecommendationManager2 pm = new Product.ContractRecommendationManager2(um);
                pm.StartPosition = FormStartPosition.CenterParent;
                pm.Show();
            }
        }
        private void 품목정보ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "ProductManager")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            if (!isFormActive)
            {
                Product.ProductManager pm = new Product.ProductManager(um);
                pm.StartPosition = FormStartPosition.CenterParent;
                pm.Show();
            }
        }

        private void 그룹관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "GroupManager")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            if (!isFormActive)
            {
                Config.GroupManager ag = new Config.GroupManager(um);
                ag.StartPosition = FormStartPosition.CenterParent;
                ag.Show();
            }
        }
        #endregion

        #region 즐겨찾기 기능

        private void lbTotalPrice_MouseHover(object sender, EventArgs e)
        {
            if (lbWidget.Text == "4")
            {
                string txt = "총재고-은행담보재고+은행담보재고아토분-대행재고-유산스-미결제재고+선t/t재고";
                ToolTip tt = new ToolTip();
                tt.ToolTipTitle = txt;
                tt.IsBalloon = true;
                tt.SetToolTip(this.lbTotalPrice, " ");
            }
        }

        private void 품명별매출한도ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                //if (frm.Name == "RecoveryPrincipalManager")
                if (frm.Name == "ProductSalesManager")
                {
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                    isFormActive = true;
                }
            }
            //새로열기
            if (!isFormActive)
            {
                ProductSalesManager.ProductSalesManager psm = new ProductSalesManager.ProductSalesManager(um);
                psm.Show();
            }
        }

        private void tsmExit_Click(object sender, EventArgs e)
        {
            출력설정ToolStripMenuItem.DropDown.Close();
        }

        private void 대시보드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "DetailDashboard")
                {
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                    isFormActive = true;
                }
            }
            //새로열기
            if (!isFormActive)
            {
                DetailDashboard dd = new DetailDashboard(um);
                dd.Show();
            }
        }

        private void 입항일정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "ArrivalSchedule")
                {
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                    isFormActive = true;
                }
            }
            //새로열기
            if (!isFormActive)
            {
                ArrivalSchedule arrivalSchedule = new ArrivalSchedule(um);
                arrivalSchedule.Show();
            }
        }

        private void 해외제조업소및수입업체수출입ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OverseaManufacturingBusiness.OverseaManufacturingBusiness omfb = new OverseaManufacturingBusiness.OverseaManufacturingBusiness(um);
            omfb.Show();
            /*FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                //if (frm.Name == "RecoveryPrincipalManager")
                if (frm.Name == "OverseaManufacturingBusiness")
                {
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                    isFormActive = true;
                }
            }
            //새로열기
            if (!isFormActive)
            {
                OverseaManufacturingBusiness.OverseaManufacturingBusiness omfb = new OverseaManufacturingBusiness.OverseaManufacturingBusiness(um);
                omfb.Show();
            }*/
        }

        private void 취급품목서초밥류ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                //if (frm.Name == "RecoveryPrincipalManager")
                if (frm.Name == "OneLineForm")
                {
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                    isFormActive = true;
                }
            }
            //새로열기
            if (!isFormActive)
            {
                SEAOVER.OneLine.OneLineForm ol = new SEAOVER.OneLine.OneLineForm(um);
                ol.Show();
            }
        }

        private void 품명별매출관리대시보드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                //if (frm.Name == "RecoveryPrincipalManager")
                if (frm.Name == "DetailDashBoardForSales")
                {
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                    isFormActive = true;
                }
            }
            //새로열기
            if (!isFormActive)
            {
                DetailDashBoardForSales ddfs = new DetailDashBoardForSales(um);
                ddfs.Show();
            }
        }

        private void 거래처관리ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            
            foreach (Form frm in fc)
            {
                //iterate through
                //if (frm.Name == "RecoveryPrincipalManager")
                if (frm.Name == "SalesManager")
                {
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                    isFormActive = true;
                }
            }
            //새로열기
            if (!isFormActive)
            {
                SaleManagement.SalesManager rpm = new SaleManagement.SalesManager(um);
                rpm.Show();
                rpm.GetDatatable();
            }
        }

        private void 나라별공휴일ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HolidaysByCountry hbc = new HolidaysByCountry();
            hbc.Show();
        }

        private void 영업일보ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;

            foreach (Form frm in fc)
            {
                //iterate through
                //if (frm.Name == "RecoveryPrincipalManager")
                if (frm.Name == "DailyBusiness")
                {
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                    isFormActive = true;
                }
            }
            //새로열기
            if (!isFormActive)
            {
                DailyBusiness db = new DailyBusiness(um);
                db.Show();
            }
        }

        private void 매출내역대시보드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SalesDashboard sd = new SalesDashboard(um);
            sd.Show();
        }

        private void 수입예정관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IncomeProduct ip = new IncomeProduct(um);
            ip.Show();
        }

        private void 연차관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VacationDashboard vd = new VacationDashboard(um);
            vd.Owner = this;
            vd.Show();
        }

        private void 해외제조업소및수입업체매트릭스ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OverseaManufacturingBusiness.MetricsDashboard md = new OverseaManufacturingBusiness.MetricsDashboard(um);
            md.Show();
        }

        private void 원금회수율ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SalesPartnerManager2 spm = new SalesPartnerManager2(um);
            spm.Owner = this;
            spm.Show();
            spm.RefreshData();
        }

        private void btnInsurance_Click(object sender, EventArgs e)
        {
            CompanyInsuranceManager cim = new CompanyInsuranceManager(um, this);
            cim.ShowDialog();
        }

        private void 영업대시보드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            salesDashboard2 sd2 = new salesDashboard2(um);
            sd2.Show();
        }

        private void 다중대시보드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MultiDashBoard md = new MultiDashBoard(um);
            md.Show();
            md.WindowState = FormWindowState.Maximized;
        }

        private void 영업전화대시보드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                //if (frm.Name == "RecoveryPrincipalManager")
                if (frm.Name == "TodaySalesDashboard")
                {
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                    isFormActive = true;
                }
            }
            //새로열기
            if (!isFormActive)
            {
                SaleManagement.TodaySalesDashboard rpm = new SaleManagement.TodaySalesDashboard(um);
                rpm.Show();
            }
        }
        #endregion

    }
}

