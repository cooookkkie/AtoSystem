using AdoNetWindow.Common;
using AdoNetWindow.Model;
using AdoNetWindow.SaleManagement.DuplicateCompany;
using AdoNetWindow.SaleManagement.SalesManagerModule;
using iTextSharp.text.pdf.parser;
using Libs;
using Libs.Tools;
using Microsoft.WindowsAPICodePack.ApplicationServices;
using MySqlX.XDevAPI.Common;
using MySqlX.XDevAPI.Relational;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Repositories;
using Repositories.Company;
using Repositories.Config;
using Repositories.SalesPartner;
using Repositories.SEAOVER.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using static NPOI.HSSF.Util.HSSFColor;

namespace AdoNetWindow.SaleManagement
{
    public partial class SalesManager : Form
    {
        Libs.Tools.Common common = new Libs.Tools.Common();
        UsersModel um;
        INotSendFaxRepository notSendFaxRepository = new NotSendFaxRepository();
        ICompanyAlarmRepository companyAlarmRepository = new CompanyAlarmRepository();
        ICompanyGroupRepository companyGroupRepository = new CompanyGroupRepository();
        ISalesPartnerRepository salesPartnerRepository = new SalesPartnerRepository();
        ICompanyRepository companyRepository = new CompanyRepository();
        ISalesRepository salesRepository = new SalesRepository();
        ICommonRepository commonRepository = new CommonRepository();
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        DataTable mainDt = new DataTable();
        DataTable subDt = new DataTable();
        DataTable Dt0 = new DataTable();
        DataTable Dt1 = new DataTable();
        DataTable Dt2 = new DataTable();
        DataTable Dt3 = new DataTable();
        DataTable Dt4 = new DataTable();
        DataTable Dt5 = new DataTable();
        DataTable Dt6 = new DataTable();

        private bool isCheckDuplicate = false;
        LoginCookie cookie;
        Dictionary<string, string> styleDic = new Dictionary<string, string>();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        DateTime dataUpdatetime;

        private System.Windows.Threading.DispatcherTimer timer;
        bool processingFlag = false;

        public SalesManager(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
        }

        private void SalesManager_Load(object sender, EventArgs e)
        {
            this.dgvInputCompany.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvCompany_DataError);
            //테이블 초기화
            mainDt = SetTable();
            Dt0 = SetTable();
            Dt1 = SetTable();
            Dt2 = SetTable();
            Dt3 = SetTable();
            Dt4 = SetTable();
            Dt5 = SetTable();
            Dt6 = SetTable();


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
                            btnAdminDashboard.Visible = false;
                            foreach (TabPage page in tcMain.TabPages)
                            {
                                if (page.Name == "tabCommonData")
                                    tcMain.TabPages.Remove(page);
                            }
                        }
                        else
                        {
                            lbManager.Visible = true;
                            txtManager.Visible = true;
                            cbLimitCopy.Visible = true;
                        }
                        break;
                    }
                }
            }

            //중복정보 listview
            lvDuplicate.View = View.Details;
            lvDuplicate.Columns.Add("담당자", 70);
            lvDuplicate.Columns.Add("상호", 150);
            lvDuplicate.Columns.Add("카테고리", 70);
            // 리스뷰를 Refresh하여 보여줌
            lvDuplicate.EndUpdate();
            //최근수정정보 listview
            lvUpdate.View = View.Details;
            lvUpdate.Columns.Add("수정일", 70);
            lvUpdate.Columns.Add("내용", 150);
            lvUpdate.Columns.Add("수정자", 70);
            // 리스뷰를 Refresh하여 보여줌
            lvUpdate.EndUpdate();

            //Datagridview 사용자 설정
            cookie = new LoginCookie(@"C:\Cookies\TEMP\SALESMANAGER\" + um.user_id, null, @"SETTING");
            string style_txt = cookie.GetTempFileString("", "");
            if (style_txt != null && !string.IsNullOrEmpty(style_txt))
            {
                string[] styles = style_txt.Split('\n');
                {
                    foreach (string style in styles)
                    {
                        if (!string.IsNullOrEmpty(style))
                        {
                            string[] s = style.Split('^');
                            if (s.Length == 2)
                                styleDic.Add(s[0], s[1]);   
                        }
                    }
                }
            }

            //테이블 최신화
            bgw.DoWork += bgw_DoWork;
            bgw.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;

            //팩스금지 페이지
            bgw2.DoWork += bgw2_DoWork;
            bgw2.RunWorkerCompleted += backgroundWorker2_RunWorkerCompleted;

            //timer_start();

            //중복이 있는지 확인
            /*MyDuplicateDataManager mdm = new MyDuplicateDataManager(um, this, atoDt);
            int duplicateCnt = mdm.GetDulicate();
            if (duplicateCnt > 0)
            {
                try
                {
                    mdm.ShowDialog();
                }
                catch
                { }
            }*/
            SetInputCompanyColumn();
            //복사금지
            dgvCompany.CopyEnabled(!cbLimitCopy.Checked);
            dgvCompany.DoInit();
        }

        #region BackgroudWork event
        private void bgw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundRefreshDatatable();
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GetData();
            isCheckDuplicate = false;
            btnRefresh.Text = "최신화 (" + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            timer_Stop();
        }

        string excel_path = "";
        DataTable excelDt = null;
        private void bgw2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (excel_path != null && !string.IsNullOrEmpty(excel_path))
            {
                excelDt = ExcelToDataTable(excel_path);
                if (excelDt != null && excelDt.Columns.Count > 0)
                    excelDt.Columns.Add("is_not_send_fax", typeof(bool));
            }
            else
                excelDt = null;
        }
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgvSearching.Rows.Clear();
            if (excelDt != null && excelDt.Columns.Count > 0)
            {
                for (int i = 0; i < excelDt.Columns.Count; i++)
                {
                    if (!excelDt.Columns[i].ColumnName.Equals("is_not_send_fax"))
                    {
                        int n = dgvSearching.Rows.Add();
                        dgvSearching.Rows[n].Cells["column_name"].Value = excelDt.Columns[i].ColumnName;
                    }
                }

                SearchingData();
            }
            txtNotSendFaxCount.Text = "0";
            timer_Stop();
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
        private void timer_Stop()
        {
            processingFlag = false;
            //모든 컨트롤 사용 활성화
            foreach (Control c in this.Controls)
            {
                c.Enabled = true;
            }
            pnGlass.Visible = false;

            this.timer.Stop();
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

        #region 거래처 테이블 최신화
        DataTable resultDt;
        DataTable atoDt, seaoverDt, onlySeaoverDt;

        private DataTable SetDataTable(IEnumerable<dynamic> result)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("id", typeof(Int32));
            dtResult.Columns.Add("group_name", typeof(string));
            dtResult.Columns.Add("company", typeof(string));
            dtResult.Columns.Add("seaover_company_code", typeof(string));
            dtResult.Columns.Add("tel", typeof(string));
            dtResult.Columns.Add("fax", typeof(string));
            dtResult.Columns.Add("phone", typeof(string));
            dtResult.Columns.Add("other_phone", typeof(string));
            dtResult.Columns.Add("ceo", typeof(string));
            dtResult.Columns.Add("registration_number", typeof(string));
            dtResult.Columns.Add("address", typeof(string));
            dtResult.Columns.Add("origin", typeof(string));
            dtResult.Columns.Add("company_manager", typeof(string));
            dtResult.Columns.Add("company_manager_position", typeof(string));
            dtResult.Columns.Add("email", typeof(string));
            dtResult.Columns.Add("web", typeof(string));
            dtResult.Columns.Add("sns1", typeof(string));
            dtResult.Columns.Add("sns2", typeof(string));
            dtResult.Columns.Add("sns3", typeof(string));

            dtResult.Columns.Add("isManagement1", typeof(string));
            dtResult.Columns.Add("isManagement2", typeof(string));
            dtResult.Columns.Add("isManagement3", typeof(string));
            dtResult.Columns.Add("isManagement4", typeof(string));

            dtResult.Columns.Add("isHide", typeof(string));
            dtResult.Columns.Add("createtime", typeof(string));
            dtResult.Columns.Add("sales_comment", typeof(string));

            dtResult.Columns.Add("ato_manager", typeof(string));
            dtResult.Columns.Add("distribution", typeof(string));
            dtResult.Columns.Add("handling_item", typeof(string));
            dtResult.Columns.Add("seaover_handling_item", typeof(string));
            dtResult.Columns.Add("payment_date", typeof(string));

            dtResult.Columns.Add("remark", typeof(string));
            dtResult.Columns.Add("remark2", typeof(string));
            dtResult.Columns.Add("remark4", typeof(string));
            dtResult.Columns.Add("remark5", typeof(string));
            dtResult.Columns.Add("remark6", typeof(string));
            dtResult.Columns.Add("isPotential1", typeof(string));
            dtResult.Columns.Add("isPotential2", typeof(string));
            dtResult.Columns.Add("isTrading", typeof(string));

            dtResult.Columns.Add("current_sale_date", typeof(string));
            dtResult.Columns.Add("current_sale_manager", typeof(string));

            dtResult.Columns.Add("isNonHandled", typeof(string));
            dtResult.Columns.Add("isOutBusiness", typeof(string));
            dtResult.Columns.Add("isNotSendFax", typeof(string));

            dtResult.Columns.Add("sales_updatetime", typeof(string));
            dtResult.Columns.Add("sales_edit_user", typeof(string));
            dtResult.Columns.Add("sales_contents", typeof(string));
            dtResult.Columns.Add("sales_log", typeof(string));
            dtResult.Columns.Add("sales_remark", typeof(string));
            dtResult.Columns.Add("pre_ato_manager", typeof(string));
            dtResult.Columns.Add("division", typeof(string));

            dtResult.Columns.Add("duplicate_common_count", typeof(int));
            dtResult.Columns.Add("duplicate_myData_count", typeof(int));
            dtResult.Columns.Add("duplicate_potential1_count", typeof(int));
            dtResult.Columns.Add("duplicate_potential2_count", typeof(int));
            dtResult.Columns.Add("duplicate_trading_count", typeof(int));
            dtResult.Columns.Add("duplicate_nonHandled_count", typeof(int));
            dtResult.Columns.Add("duplicate_notSendFax_count", typeof(int));
            dtResult.Columns.Add("duplicate_outBusiness_count", typeof(int));
            dtResult.Columns.Add("duplicate_result", typeof(string));

            dtResult.Columns.Add("isDelete", typeof(string));
            dtResult.Columns.Add("alarm_month", typeof(string));
            dtResult.Columns.Add("alarm_week", typeof(string));
            dtResult.Columns.Add("alarm_complete_date", typeof(string));
            dtResult.Columns.Add("alarm_date", typeof(string));
            dtResult.Columns.Add("industry_type", typeof(string));
            dtResult.Columns.Add("industry_type2", typeof(string));
            dtResult.Columns.Add("industry_type_rank", typeof(int));

            dtResult.Columns.Add("category", typeof(string));

            dtResult.Columns.Add("main_id", typeof(Int32));
            dtResult.Columns.Add("sub_id", typeof(Int32));

            // Add other columns here

            foreach (var item in result)
            {
                DataRow newRow = dtResult.NewRow();
                newRow["id"] = item.id;
                newRow["group_name"] = item.group_name;
                newRow["company"] = item.company;
                newRow["seaover_company_code"] = item.seaover_company_code;
                newRow["tel"] = item.tel;
                newRow["fax"] = item.fax;
                newRow["phone"] = item.phone;
                newRow["other_phone"] = item.other_phone;
                newRow["ceo"] = item.ceo;
                newRow["registration_number"] = item.registration_number;
                newRow["address"] = item.address;
                newRow["origin"] = item.origin;
                newRow["company_manager"] = item.company_manager;
                newRow["company_manager_position"] = item.company_manager_position;
                newRow["email"] = item.email;
                newRow["web"] = item.web;
                newRow["sns1"] = item.sns1;
                newRow["sns2"] = item.sns2;
                newRow["sns3"] = item.sns3;

                newRow["isManagement1"] = item.isManagement1;
                newRow["isManagement2"] = item.isManagement2;
                newRow["isManagement3"] = item.isManagement3;
                newRow["isManagement4"] = item.isManagement4;
                newRow["isHide"] = item.isHide;
                newRow["createtime"] = item.createtime;
                newRow["sales_comment"] = item.sales_comment;

                newRow["ato_manager"] = item.ato_manager;
                newRow["distribution"] = item.distribution;
                newRow["handling_item"] = item.handling_item;
                newRow["seaover_handling_item"] = item.seaover_handling_item;
                newRow["payment_date"] = item.payment_date;

                newRow["remark"] = item.remark;
                newRow["remark2"] = item.remark2;
                newRow["remark4"] = item.remark4;
                newRow["remark5"] = item.remark5;
                newRow["remark6"] = item.remark6;
                newRow["isPotential1"] = item.isPotential1;
                newRow["isPotential2"] = item.isPotential2;
                newRow["isTrading"] = item.isTrading;

                newRow["current_sale_date"] = item.current_sale_date;
                newRow["current_sale_manager"] = item.current_sale_manager;

                newRow["isNonHandled"] = item.isNonHandled;
                newRow["isOutBusiness"] = item.isOutBusiness;
                newRow["isNotSendFax"] = item.isNotSendFax;

                newRow["sales_updatetime"] = item.sales_updatetime;
                newRow["sales_edit_user"] = item.sales_edit_user;
                newRow["sales_contents"] = item.sales_contents;
                newRow["sales_log"] = item.sales_log;
                newRow["sales_remark"] = item.sales_remark;
                newRow["pre_ato_manager"] = item.pre_ato_manager;
                newRow["division"] = item.division;

                newRow["duplicate_common_count"] = item.duplicate_common_count;
                newRow["duplicate_myData_count"] = item.duplicate_myData_count;
                newRow["duplicate_potential1_count"] = item.duplicate_potential1_count;
                newRow["duplicate_potential2_count"] = item.duplicate_potential2_count;
                newRow["duplicate_trading_count"] = item.duplicate_trading_count;
                newRow["duplicate_nonHandled_count"] = item.duplicate_nonHandled_count;
                newRow["duplicate_notSendFax_count"] = item.duplicate_notSendFax_count;
                newRow["duplicate_outBusiness_count"] = item.duplicate_outBusiness_count;
                newRow["duplicate_result"] = item.duplicate_result;

                newRow["isDelete"] = item.isDelete;

                newRow["alarm_month"] = item.alarm_month;
                newRow["alarm_week"] = item.alarm_week;
                newRow["alarm_complete_date"] = item.alarm_complete_date;
                newRow["alarm_date"] = item.alarm_date;

                newRow["industry_type"] = item.industry_type;
                newRow["industry_type2"] = item.industry_type2;
                newRow["industry_type_rank"] = item.industry_type_rank;

                newRow["category"] = item.category;

                newRow["main_id"] = item.main_id;
                newRow["sub_id"] = item.sub_id;
                // Set other column values
                dtResult.Rows.Add(newRow);
            }

            return dtResult;
        }

        public void RefreshTable(bool isGetData = true)
        {
            isCheckDuplicate = false;
            //영업, 매출제한
            string saleTelDate;
            switch (cbCurrentSaleTelDate.Text)
            {
                case "15일 이상 지난":
                    saleTelDate = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
                    break;
                case "한달 이상 지난":
                    saleTelDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    break;
                case "45일 이상 지난":
                    saleTelDate = DateTime.Now.AddDays(-45).ToString("yyyy-MM-dd");
                    break;
                case "두달 이상 지난":
                    saleTelDate = DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd");
                    break;
                case "세달 이상 지난":
                    saleTelDate = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
                    break;
                case "여섯달 이상 지난":
                    saleTelDate = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
                    break;
                default:
                    saleTelDate = "";
                    break;
            }

            //데이터 출력====================================================================================
            //매출처 정보
            DataTable companyDt = salesPartnerRepository.GetSalesPartner2("", "", cbExactly.Checked, ""
                                            , "", "", " ", true, false, false, false, false, !cbNotOutOfBusiness.Checked, false, false, saleTelDate);

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
                              sales_comment = p.Field<string>("sales_comment"),

                              ato_manager = p.Field<string>("ato_manager"),
                              distribution = p.Field<string>("distribution"),
                              handling_item = p.Field<string>("handling_item"),
                              seaover_handling_item = "",
                              payment_date = p.Field<string>("payment_date"),

                              remark = p.Field<string>("remark"),
                              remark2 = p.Field<string>("remark2"),
                              remark4 = p.Field<string>("remark4"),
                              isPotential1 = p.Field<string>("isPotential1"),
                              isPotential2 = p.Field<string>("isPotential2"),
                              isTrading = p.Field<string>("isTrading"),

                              current_sale_date = "",
                              current_sale_manager = "",

                              isNonHandled = p.Field<string>("isNonHandled"),
                              isOutBusiness = p.Field<string>("isOutBusiness"),
                              isNotSendFax = p.Field<string>("isNotSendFax"),

                              sales_updatetime = p.Field<string>("sales_updatetime"),
                              sales_edit_user = p.Field<string>("sales_edit_user"),
                              sales_contents = (t == null) ? "" : t.Field<string>("contents"),
                              sales_log = (t == null) ? "" : t.Field<string>("log"),
                              sales_remark = (t == null) ? "" : t.Field<string>("remark"),
                              pre_ato_manager = (t == null) ? "" : t.Field<string>("from_ato_manager"),
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

                              alarm_month = p.Field<string>("alarm_month"),
                              alarm_week = p.Field<string>("alarm_week"),
                              alarm_complete_date = p.Field<string>("alarm_complete_date"),
                              alarm_date = p.Field<string>("alarm_date"),

                              industry_type = p.Field<string>("industry_type"),
                              industry_type_rank = 00,

                              category = "",

                              main_id = 0,
                              sub_id = 0
                          };
            //companyDt = ConvertListToDatatable(atoTemp);
            companyDt = SetDataTable(atoTemp);
            companyDt.AcceptChanges();



            //아토 전산에만 저장된 거래처정보
            DataRow[] atoCompanyDt = companyDt.Select("seaover_company_code = ''");
            atoDt = new DataTable();
            if (atoCompanyDt.Length > 0)
                atoDt = atoCompanyDt.CopyToDataTable();

            //씨오버 거래처 
            DataTable seaDt = salesRepository.GetSaleCompany("", cbExactly.Checked, "", "", "", "", "", "", cbNotOutOfBusiness.Checked, cbLitigation.Checked);
            onlySeaoverDt = seaDt;
            //씨오버취급품목
            DataTable handledItemDt = salesRepository.GetHandledItem("", "");
            seaDt.Columns.Add("seaover_handling_item", typeof(string));
            if (handledItemDt.Rows.Count > 0)
            {
                foreach (DataRow dr in seaDt.Rows)
                {
                    DataRow[] productDr = handledItemDt.Select($"매출처코드='{dr["거래처코드"].ToString()}'");
                    if (productDr.Length > 0)
                    {
                        string handled_item = "";
                        foreach (DataRow pDr in productDr)
                            handled_item += " " + pDr["품명"].ToString();
                        handled_item = handled_item.Trim().Replace(' ', ',');

                        dr["seaover_handling_item"] = handled_item;
                    }
                }
            }
            seaDt.AcceptChanges();
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
                                  address = p.Field<string>("사업장주소"),

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
                                  sales_comment = (t == null) ? "" : t.Field<string>("sales_comment"),

                                  ato_manager = (t == null) ? p.Field<string>("매출자") : t.Field<string>("ato_manager"),
                                  distribution = (t == null) ? "" : t.Field<string>("distribution"),
                                  handling_item = (t == null) ? "" : t.Field<string>("handling_item"),
                                  seaover_handling_item = p.Field<string>("seaover_handling_item"),
                                  payment_date = (t == null) ? "" : t.Field<string>("payment_date"),

                                  remark = p.Field<string>("참고사항"),
                                  remark2 = (t == null) ? "" : t.Field<string>("remark2"),
                                  remark4 = (t == null) ? "" : t.Field<string>("remark4"),
                                  isPotential1 = (t == null) ? "FALSE" : t.Field<string>("isPotential1"),
                                  isPotential2 = (t == null) ? "FALSE" : t.Field<string>("isPotential2"),
                                  isTrading = (t == null) ? "TRUE" : t.Field<string>("isTrading"),
                                  current_sale_date = p.Field<string>("최종매출일자"),
                                  current_sale_manager = p.Field<string>("매출자"),

                                  isNonHandled = (t == null) ? "" : t.Field<string>("isNonHandled"),
                                  isOutBusiness = (t == null) ? p.Field<string>("폐업유무") : t.Field<string>("isOutBusiness"),
                                  isNotSendFax = (t == null) ? "" : t.Field<string>("isNotSendFax"),
                                  sales_updatetime = (t == null) ? "" : t.Field<string>("sales_updatetime"),
                                  real_sales_updatetime = p.Field<string>("최종매출일자"),
                                  sales_edit_user = (t == null) ? "" : t.Field<string>("sales_edit_user"),
                                  sales_contents = (t == null) ? "" : t.Field<string>("sales_contents"),
                                  sales_log = (t == null) ? "" : t.Field<string>("sales_log"),
                                  sales_remark = (t == null) ? "" : t.Field<string>("sales_remark"),
                                  pre_ato_manager = (t == null) ? "" : t.Field<string>("pre_ato_manager"),
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

                                  alarm_month = (t == null) ? "0" : t.Field<string>("alarm_month"),
                                  alarm_week = (t == null) ? "" : t.Field<string>("alarm_week"),
                                  alarm_complete_date = (t == null) ? "" : t.Field<string>("alarm_complete_date"),
                                  alarm_date = (t == null) ? "" : t.Field<string>("alarm_date"),

                                  industry_type = (t == null) ? "" : t.Field<string>("industry_type"),
                                  industry_type_rank = 00,

                                  category = "",

                                  main_id = 0,
                                  sub_id = 0
                              };
            seaoverDt = SetDataTable(seaoverTemp);
            //seaoverDt = ConvertListToDatatable(seaoverTemp);
            //그룹별 거래처
            DataTable groupDt = companyGroupRepository.GetCompanyGroup();
            if (seaoverDt != null && seaoverDt.Rows.Count > 0 && groupDt != null && groupDt.Rows.Count > 0)
            {
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
                                       sales_comment = p.Field<string>("sales_comment"),

                                       ato_manager = p.Field<string>("ato_manager"),
                                       distribution = p.Field<string>("distribution"),
                                       handling_item = p.Field<string>("handling_item"),
                                       seaover_handling_item = p.Field<string>("seaover_handling_item"),
                                       payment_date = p.Field<string>("payment_date"),
                                       remark = p.Field<string>("remark"),
                                       remark2 = p.Field<string>("remark2"),
                                       remark4 = p.Field<string>("remark4"),
                                       isPotential1 = p.Field<string>("isPotential1"),
                                       isPotential2 = p.Field<string>("isPotential2"),
                                       isTrading = p.Field<string>("isTrading"),
                                       current_sale_date = p.Field<string>("current_sale_date"),
                                       current_sale_manager = p.Field<string>("current_sale_manager"),

                                       isNonHandled = p.Field<string>("isNonHandled"),
                                       isOutBusiness = p.Field<string>("isOutBusiness"),
                                       isNotSendFax = p.Field<string>("isNotSendFax"),
                                       sales_updatetime = p.Field<string>("sales_updatetime"),
                                       real_sales_updatetime = p.Field<string>("sales_updatetime"),
                                       sales_edit_user = p.Field<string>("sales_edit_user"),
                                       sales_contents = p.Field<string>("sales_contents"),
                                       sales_log = p.Field<string>("sales_log"),
                                       sales_remark = p.Field<string>("sales_remark"),
                                       pre_ato_manager = p.Field<string>("pre_ato_manager"),
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

                                       alarm_month = p.Field<string>("alarm_month"),
                                       alarm_week = p.Field<string>("alarm_week"),
                                       alarm_complete_date = p.Field<string>("alarm_complete_date"),
                                       alarm_date = p.Field<string>("alarm_date"),

                                       industry_type = p.Field<string>("industry_type"),
                                       industry_type_rank = 00,

                                       category = "",

                                       main_id = (t == null) ? 0 : t.Field<Int32>("main_id"),
                                       sub_id = (t == null) ? 0 : t.Field<Int32>("sub_id")
                                   };
                //seaoverDt = ConvertListToDatatable(seaoverTemp2);
                seaoverDt = SetDataTable(seaoverTemp2);
                foreach (DataRow dr in seaoverDt.Rows)
                {
                    //삭제거래처
                    bool isDelete;
                    if (!bool.TryParse(dr["isDelete"].ToString(), out isDelete))
                        isDelete = false;
                    if (!isDelete)
                    {
                        // ** 매출 || 영업중 먼저인 내역이 우선 **

                        //최근 매출정보
                        DateTime current_sale_date;
                        if (!DateTime.TryParse(dr["current_sale_date"].ToString(), out current_sale_date))
                            current_sale_date = new DateTime(1900, 1, 1);

                        //최근 영업정보
                        DateTime sales_updatetime;
                        if (!DateTime.TryParse(dr["sales_updatetime"].ToString(), out sales_updatetime))
                            sales_updatetime = new DateTime(1900, 1, 1);

                        //2023-09-13
                        //시간까지 체크하니 테스트가 안됨 그날 매출달면 그냥 바로 거래중으로 적용
                        current_sale_date = new DateTime(current_sale_date.Year, current_sale_date.Month, current_sale_date.Day);
                        sales_updatetime = new DateTime(sales_updatetime.Year, sales_updatetime.Month, sales_updatetime.Day);

                        string sale_edit_user;
                        string sale_date;
                        string sale_contents;
                        string sale_remark;
                        string sale_log;
                        string ato_manager;
                        if (current_sale_date >= sales_updatetime)
                        {
                            dr["isTrading"] = "TRUE";
                            ato_manager = dr["current_sale_manager"].ToString();
                            sale_edit_user = dr["current_sale_manager"].ToString();
                            sale_contents = "씨오버 매출";
                            sale_date = current_sale_date.ToString("yyyy-MM-dd HH:mm:ss");
                            sale_remark = "";
                            sale_log = "";
                        }
                        else
                        {
                            ato_manager = dr["ato_manager"].ToString();
                            sale_edit_user = dr["sales_edit_user"].ToString();
                            sale_contents = dr["sales_contents"].ToString();
                            /*if (sale_contents == "씨오버 매출")
                                sale_contents = seaoverDt1.Rows[i]["sales_log"].ToString();*/
                            sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                            sale_remark = dr["sales_remark"].ToString();
                            sale_log = dr["sales_log"].ToString();
                        }
                        //최신화
                        dr["ato_manager"] = ato_manager;
                        dr["pre_ato_manager"] = ato_manager;
                        dr["sales_updatetime"] = sale_date;
                        dr["sales_edit_user"] = sale_edit_user;
                        dr["sales_contents"] = sale_contents;
                        dr["sales_remark"] = sale_remark;
                        dr["sales_log"] = sale_log;
                        //거래처코드가 없는데 거래중은 자동삭제
                        bool trading;
                        if (!bool.TryParse(dr["isTrading"].ToString(), out trading))
                            trading = false;
                        if (string.IsNullOrEmpty(dr["seaover_company_code"].ToString()) && trading)
                            dr["isDelete"] = "FALSE";
                    }
                }
                seaoverDt.AcceptChanges();

                //대표거래처 최종수정일 최신화
                foreach (DataRow dr in seaoverDt.Rows)
                {
                    //대표거래처 최종수정일 최신화
                    int main_id;
                    if (!int.TryParse(dr["main_id"].ToString(), out main_id))
                        main_id = 0;
                    int sub_id;
                    if (!int.TryParse(dr["sub_id"].ToString(), out sub_id))
                        sub_id = 0;

                    /*if (dr["company"].ToString().Contains("알마푸드"))
                        sub_id = sub_id;*/

                    bool isOutBusiness;
                    if (!bool.TryParse(dr["isOutBusiness"].ToString(), out isOutBusiness))
                        isOutBusiness = false;

                    if (main_id > 0 && sub_id == 0)
                    {
                        //최근 영업정보
                        DateTime sales_updatetime;
                        if (!DateTime.TryParse(dr["sales_updatetime"].ToString(), out sales_updatetime))
                            sales_updatetime = new DateTime(1900, 1, 1);

                        //최근 매출정보
                        DateTime current_sales_date;
                        if (!DateTime.TryParse(dr["current_sale_date"].ToString(), out current_sales_date))
                            current_sales_date = new DateTime(1900, 1, 1);


                        string ato_manager = dr["ato_manager"].ToString();
                        string pre_ato_manager = dr["ato_manager"].ToString();
                        string sale_edit_user = dr["sales_edit_user"].ToString();
                        string sale_contents = dr["sales_contents"].ToString();
                        /*if (sale_contents == "씨오버 매출")
                            sale_contents = seaoverDt1.Rows[i]["sales_log"].ToString();*/
                        string sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                        string current_sale_date = current_sales_date.ToString("yyyy-MM-dd HH:mm:ss");
                        string sale_remark = dr["sales_remark"].ToString();
                        string sale_log = dr["sales_log"].ToString();

                        DataRow[] subDr = seaoverDt.Select($"main_id = {main_id} AND isDelete = 'FALSE'");
                        for (int j = 0; j < subDr.Length; j++)
                        {
                            if (!isOutBusiness)
                            {
                                DateTime tempUpdatetime;
                                if (DateTime.TryParse(subDr[j]["sales_updatetime"].ToString(), out tempUpdatetime))
                                {
                                    if (sales_updatetime < tempUpdatetime)
                                    {
                                        if (DateTime.TryParse(subDr[j]["current_sale_date"].ToString(), out current_sales_date))
                                            current_sale_date = current_sales_date.ToString("yyyy-MM-dd");

                                        sales_updatetime = tempUpdatetime;
                                        ato_manager = subDr[j]["ato_manager"].ToString();
                                        pre_ato_manager = subDr[j]["ato_manager"].ToString();
                                        sale_edit_user = subDr[j]["sales_edit_user"].ToString();
                                        sale_contents = subDr[j]["sales_contents"].ToString();
                                        /*if (sale_contents == "씨오버 매출")
                                            sale_contents = seaoverDt1.Rows[i]["sales_log"].ToString();*/
                                        sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                                        sale_remark = subDr[j]["sales_remark"].ToString();
                                        sale_log = subDr[j]["sales_log"].ToString();
                                    }
                                }
                            }
                            //대표거래처가 폐업처리 되었을 경우 그룹을 품
                            else
                            {
                                subDr[j]["main_id"] = "0";
                                subDr[j]["sub_id"] = "0";
                            }
                        }
                        //최신화
                        dr["ato_manager"] = ato_manager;
                        dr["pre_ato_manager"] = ato_manager;
                        dr["sales_updatetime"] = sale_date;
                        dr["current_sale_date"] = current_sale_date;
                        dr["sales_edit_user"] = sale_edit_user;
                        dr["sales_contents"] = sale_contents;
                        dr["sales_remark"] = sale_remark;
                        dr["sales_log"] = sale_log;
                    }
                }
            }
            seaoverDt.AcceptChanges();
            //마지막 table_index, industry_type_rank, category 정리
            atoDt.Merge(seaoverDt);
            atoDt.Columns.Add("table_index", typeof(Int32));
            for (int i = 0; i < atoDt.Rows.Count; i++)
            {
                atoDt.Rows[i]["table_index"] = i;
                atoDt.Rows[i]["industry_type_rank"] = GetIndustryTypeRank(atoDt.Rows[i]["industry_type"].ToString());

                bool isTrading;
                if (!bool.TryParse(atoDt.Rows[i]["isTrading"].ToString(), out isTrading))
                    isTrading = false;
                bool isPotential1, isPotential2;
                if (!bool.TryParse(atoDt.Rows[i]["isPotential1"].ToString(), out isPotential1))
                    isPotential1 = false;
                if (!bool.TryParse(atoDt.Rows[i]["isPotential2"].ToString(), out isPotential2))
                    isPotential2 = false;
                bool isNonHandled, isNotSendFax, isOutBusiness;
                if (!bool.TryParse(atoDt.Rows[i]["isNonHandled"].ToString(), out isNonHandled))
                    isNonHandled = false;
                if (!bool.TryParse(atoDt.Rows[i]["isNotSendFax"].ToString(), out isNotSendFax))
                    isNotSendFax = false;
                if (!bool.TryParse(atoDt.Rows[i]["isOutBusiness"].ToString(), out isOutBusiness))
                    isOutBusiness = false;

                //카테고리
                string category = "";
                if (isNonHandled || isOutBusiness)
                    category = "취급X";
                else if (isTrading)
                    category = "거래중";
                else if (isPotential1 && !string.IsNullOrEmpty(atoDt.Rows[i]["ato_manager"].ToString().Trim()))
                    category = "잠재1";
                else if (isPotential2 && !string.IsNullOrEmpty(atoDt.Rows[i]["ato_manager"].ToString().Trim()))
                    category = "잠재2";
                else if (!string.IsNullOrEmpty(atoDt.Rows[i]["ato_manager"].ToString().Trim()))
                    category = "내DATA";
                else
                    category = "공용DATA";
                atoDt.Rows[i]["category"] = category;
            }
            atoDt.AcceptChanges();
            //최신화
            btnRefresh.Text = "최신화(" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ")";
            if (isProgress)
                pb.Close();
            /*//상태 필터
            DataTable statusDt = atoDt.DefaultView.ToTable(false, new string[] { "remark4" });
            DataView statusDv = new DataView(statusDt);
            statusDv.Sort = "remark4";
            statusDt = statusDv.ToTable(true, new string[] { "remark4"});

            cbStatus.Items.Clear();
            cbStatus.Items.Add("전체");
            for (int i = 0; i < statusDt.Rows.Count; i++)
                cbStatus.Items.Add(statusDt.Rows[i]["remark4"].ToString());*/
            //데이터출력
            if (isGetData)
                GetData();

            dataUpdatetime = DateTime.Now;
        }

        public void GetDatatable() 
        {
            //영업, 매출제한
            switch (cbCurrentSaleTelDate.Text)
            {
                case "15일 이상 지난":
                    saleTelDate = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
                    break;
                case "한달 이상 지난":
                    saleTelDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    break;
                case "45일 이상 지난":
                    saleTelDate = DateTime.Now.AddDays(-45).ToString("yyyy-MM-dd");
                    break;
                case "두달 이상 지난":
                    saleTelDate = DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd");
                    break;
                case "세달 이상 지난":
                    saleTelDate = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
                    break;
                case "여섯달 이상 지난":
                    saleTelDate = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
                    break;
                default:
                    saleTelDate = "";
                    break;
            }
            timer_start();
            bgw.RunWorkerAsync();
            
        }
        string saleTelDate;
        private void BackgroundRefreshDatatable()
        {
            isCheckDuplicate = false;
            processingFlag = true;

            //매출처 정보
            DataTable companyDt = salesPartnerRepository.GetSalesPartner2("", "", cbExactly.Checked, ""
                                            , "", "", " ", true, false, false, false, false, !cbNotOutOfBusiness.Checked, false, false, saleTelDate);

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
                              sales_comment = p.Field<string>("sales_comment"),

                              ato_manager = p.Field<string>("ato_manager"),
                              distribution = p.Field<string>("distribution"),
                              handling_item = p.Field<string>("handling_item"),
                              seaover_handling_item = "",
                              payment_date = p.Field<string>("payment_date"),

                              remark = p.Field<string>("remark"),
                              remark2 = p.Field<string>("remark2"),
                              remark4 = p.Field<string>("remark4"),
                              remark5 = p.Field<string>("remark5"),
                              remark6 = p.Field<string>("remark6"),
                              isPotential1 = p.Field<string>("isPotential1"),
                              isPotential2 = p.Field<string>("isPotential2"),
                              isTrading = p.Field<string>("isTrading"),

                              current_sale_date = "",
                              current_sale_manager = "",

                              isNonHandled = p.Field<string>("isNonHandled"),
                              isOutBusiness = p.Field<string>("isOutBusiness"),
                              isNotSendFax = p.Field<string>("isNotSendFax"),

                              sales_updatetime = p.Field<string>("sales_updatetime"),
                              sales_edit_user = p.Field<string>("sales_edit_user"),
                              sales_contents = (t == null) ? "" : t.Field<string>("contents"),
                              sales_log = (t == null) ? "" : t.Field<string>("log"),
                              sales_remark = (t == null) ? "" : t.Field<string>("remark"),
                              pre_ato_manager = (t == null) ? "" : t.Field<string>("from_ato_manager"),
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

                              alarm_month = p.Field<string>("alarm_month"),
                              alarm_week = p.Field<string>("alarm_week"),
                              alarm_complete_date = p.Field<string>("alarm_complete_date"),
                              alarm_date = p.Field<string>("alarm_date"),

                              industry_type = p.Field<string>("industry_type"),
                              industry_type2 = p.Field<string>("industry_type2"),
                              industry_type_rank = 00,

                              category = "",

                              main_id = 0,
                              sub_id = 0
                          };
            //companyDt = ConvertListToDatatable(atoTemp);
            companyDt = SetDataTable(atoTemp);
            companyDt.AcceptChanges();



            //아토 전산에만 저장된 거래처정보
            DataRow[] atoCompanyDt = companyDt.Select("seaover_company_code = ''");
            atoDt = new DataTable();
            if (atoCompanyDt.Length > 0)
                atoDt = atoCompanyDt.CopyToDataTable();

            //씨오버 거래처 
            DataTable seaDt = salesRepository.GetSaleCompany("", cbExactly.Checked, "", "", "", "", "", "", cbNotOutOfBusiness.Checked, cbLitigation.Checked);
            onlySeaoverDt = seaDt;
            //씨오버취급품목
            DataTable handledItemDt = salesRepository.GetHandledItem("", "");
            seaDt.Columns.Add("seaover_handling_item", typeof(string));
            if (handledItemDt.Rows.Count > 0)
            {
                foreach (DataRow dr in seaDt.Rows)
                {
                    DataRow[] productDr = handledItemDt.Select($"매출처코드='{dr["거래처코드"].ToString()}'");
                    if (productDr.Length > 0)
                    {
                        string handled_item = "";
                        foreach (DataRow pDr in productDr)
                            handled_item += " " + pDr["품명"].ToString();
                        handled_item = handled_item.Trim().Replace(' ', ',');

                        dr["seaover_handling_item"] = handled_item;
                    }
                }
            }
            seaDt.AcceptChanges();
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
                                  address = p.Field<string>("사업장주소"),

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
                                  sales_comment = (t == null) ? "" : t.Field<string>("sales_comment"),

                                  ato_manager = (t == null) ? p.Field<string>("매출자") : t.Field<string>("ato_manager"),
                                  distribution = (t == null) ? "" : t.Field<string>("distribution"),
                                  handling_item = (t == null) ? "" : t.Field<string>("handling_item"),
                                  seaover_handling_item = p.Field<string>("seaover_handling_item"),
                                  payment_date = (t == null) ? "" : t.Field<string>("payment_date"),

                                  remark = p.Field<string>("참고사항"),
                                  remark2 = (t == null) ? "" : t.Field<string>("remark2"),
                                  remark4 = (t == null) ? "" : t.Field<string>("remark4"),
                                  remark5 = (t == null) ? "" : t.Field<string>("remark5"),
                                  remark6 = (t == null) ? "" : t.Field<string>("remark6"),
                                  isPotential1 = (t == null) ? "FALSE" : t.Field<string>("isPotential1"),
                                  isPotential2 = (t == null) ? "FALSE" : t.Field<string>("isPotential2"),
                                  isTrading = (t == null) ? "TRUE" : t.Field<string>("isTrading"),
                                  current_sale_date = p.Field<string>("최종매출일자"),
                                  current_sale_manager = p.Field<string>("매출자"),

                                  isNonHandled = (t == null) ? "" : t.Field<string>("isNonHandled"),
                                  isOutBusiness = (t == null) ? p.Field<string>("폐업유무") : t.Field<string>("isOutBusiness"),
                                  isNotSendFax = (t == null) ? "" : t.Field<string>("isNotSendFax"),
                                  sales_updatetime = (t == null) ? "" : t.Field<string>("sales_updatetime"),
                                  real_sales_updatetime = p.Field<string>("최종매출일자"),
                                  sales_edit_user = (t == null) ? "" : t.Field<string>("sales_edit_user"),
                                  sales_contents = (t == null) ? "" : t.Field<string>("sales_contents"),
                                  sales_log = (t == null) ? "" : t.Field<string>("sales_log"),
                                  sales_remark = (t == null) ? "" : t.Field<string>("sales_remark"),
                                  pre_ato_manager = (t == null) ? "" : t.Field<string>("pre_ato_manager"),
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

                                  alarm_month = (t == null) ? "0" : t.Field<string>("alarm_month"),
                                  alarm_week = (t == null) ? "" : t.Field<string>("alarm_week"),
                                  alarm_complete_date = (t == null) ? "" : t.Field<string>("alarm_complete_date"),
                                  alarm_date = (t == null) ? "" : t.Field<string>("alarm_date"),

                                  industry_type = (t == null) ? "" : t.Field<string>("industry_type"),
                                  industry_type2 = (t == null) ? "" : t.Field<string>("industry_type2"),
                                  industry_type_rank = 00,

                                  category = "",

                                  main_id = 0,
                                  sub_id = 0
                              };
            seaoverDt = SetDataTable(seaoverTemp);
            //seaoverDt = ConvertListToDatatable(seaoverTemp);
            //그룹별 거래처
            DataTable groupDt = companyGroupRepository.GetCompanyGroup();
            if (seaoverDt != null && seaoverDt.Rows.Count > 0 && groupDt != null && groupDt.Rows.Count > 0)
            {
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
                                       sales_comment = p.Field<string>("sales_comment"),

                                       ato_manager = p.Field<string>("ato_manager"),
                                       distribution = p.Field<string>("distribution"),
                                       handling_item = p.Field<string>("handling_item"),
                                       seaover_handling_item = p.Field<string>("seaover_handling_item"),
                                       payment_date = p.Field<string>("payment_date"),
                                       remark = p.Field<string>("remark"),
                                       remark2 = p.Field<string>("remark2"),
                                       remark4 = p.Field<string>("remark4"),
                                       remark5 = p.Field<string>("remark5"),
                                       remark6 = p.Field<string>("remark6"),
                                       isPotential1 = p.Field<string>("isPotential1"),
                                       isPotential2 = p.Field<string>("isPotential2"),
                                       isTrading = p.Field<string>("isTrading"),
                                       current_sale_date = p.Field<string>("current_sale_date"),
                                       current_sale_manager = p.Field<string>("current_sale_manager"),

                                       isNonHandled = p.Field<string>("isNonHandled"),
                                       isOutBusiness = p.Field<string>("isOutBusiness"),
                                       isNotSendFax = p.Field<string>("isNotSendFax"),
                                       sales_updatetime = p.Field<string>("sales_updatetime"),
                                       real_sales_updatetime = p.Field<string>("sales_updatetime"),
                                       sales_edit_user = p.Field<string>("sales_edit_user"),
                                       sales_contents = p.Field<string>("sales_contents"),
                                       sales_log = p.Field<string>("sales_log"),
                                       sales_remark = p.Field<string>("sales_remark"),
                                       pre_ato_manager = p.Field<string>("pre_ato_manager"),
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

                                       alarm_month = p.Field<string>("alarm_month"),
                                       alarm_week = p.Field<string>("alarm_week"),
                                       alarm_complete_date = p.Field<string>("alarm_complete_date"),
                                       alarm_date = p.Field<string>("alarm_date"),

                                       industry_type = p.Field<string>("industry_type"),
                                       industry_type2 = p.Field<string>("industry_type2"),
                                       industry_type_rank = 00,

                                       category = "",

                                       main_id = (t == null) ? 0 : t.Field<Int32>("main_id"),
                                       sub_id = (t == null) ? 0 : t.Field<Int32>("sub_id")
                                   };
                seaoverDt = SetDataTable(seaoverTemp2);
                foreach (DataRow dr in seaoverDt.Rows)
                {
                    //삭제거래처
                    bool isDelete;
                    if (!bool.TryParse(dr["isDelete"].ToString(), out isDelete))
                        isDelete = false;
                    if (!isDelete)
                    {
                        // ** 매출 || 영업중 먼저인 내역이 우선 **

                        //최근 매출정보
                        DateTime current_sale_date;
                        if (!DateTime.TryParse(dr["current_sale_date"].ToString(), out current_sale_date))
                            current_sale_date = new DateTime(1900, 1, 1);

                        //최근 영업정보
                        DateTime sales_updatetime;
                        if (!DateTime.TryParse(dr["sales_updatetime"].ToString(), out sales_updatetime))
                            sales_updatetime = new DateTime(1900, 1, 1);

                        //2023-09-13
                        //시간까지 체크하니 테스트가 안됨 그날 매출달면 그냥 바로 거래중으로 적용
                        current_sale_date = new DateTime(current_sale_date.Year, current_sale_date.Month, current_sale_date.Day);
                        sales_updatetime = new DateTime(sales_updatetime.Year, sales_updatetime.Month, sales_updatetime.Day);

                        string sale_edit_user;
                        string sale_date;
                        string sale_contents;
                        string sale_remark;
                        string sale_log;
                        string ato_manager;
                        if (current_sale_date >= sales_updatetime)
                        {
                            dr["isTrading"] = "TRUE";
                            ato_manager = dr["current_sale_manager"].ToString();
                            sale_edit_user = dr["current_sale_manager"].ToString();
                            sale_contents = "씨오버 매출";
                            sale_date = current_sale_date.ToString("yyyy-MM-dd HH:mm:ss");
                            sale_remark = "";
                            sale_log = "";
                        }
                        else
                        {
                            ato_manager = dr["ato_manager"].ToString();
                            sale_edit_user = dr["sales_edit_user"].ToString();
                            sale_contents = dr["sales_contents"].ToString();
                            /*if (sale_contents == "씨오버 매출")
                                sale_contents = seaoverDt1.Rows[i]["sales_log"].ToString();*/
                            sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                            sale_remark = dr["sales_remark"].ToString();
                            sale_log = dr["sales_log"].ToString();
                        }
                        //최신화
                        dr["ato_manager"] = ato_manager;
                        dr["pre_ato_manager"] = ato_manager;
                        dr["sales_updatetime"] = sale_date;
                        dr["sales_edit_user"] = sale_edit_user;
                        dr["sales_contents"] = sale_contents;
                        dr["sales_remark"] = sale_remark;
                        dr["sales_log"] = sale_log;
                        //거래처코드가 없는데 거래중은 자동삭제
                        bool trading;
                        if (!bool.TryParse(dr["isTrading"].ToString(), out trading))
                            trading = false;
                        if (string.IsNullOrEmpty(dr["seaover_company_code"].ToString()) && trading)
                            dr["isDelete"] = "FALSE";
                    }
                }
                seaoverDt.AcceptChanges();

                //대표거래처 최종수정일 최신화
                foreach (DataRow dr in seaoverDt.Rows)
                {
                    //대표거래처 최종수정일 최신화
                    int main_id;
                    if (!int.TryParse(dr["main_id"].ToString(), out main_id))
                        main_id = 0;
                    int sub_id;
                    if (!int.TryParse(dr["sub_id"].ToString(), out sub_id))
                        sub_id = 0;

                    /*if (dr["company"].ToString().Contains("알마푸드"))
                        sub_id = sub_id;*/

                    bool isOutBusiness;
                    if (!bool.TryParse(dr["isOutBusiness"].ToString(), out isOutBusiness))
                        isOutBusiness = false;

                    if (main_id > 0 && sub_id == 0)
                    {
                        //최근 영업정보
                        DateTime sales_updatetime;
                        if (!DateTime.TryParse(dr["sales_updatetime"].ToString(), out sales_updatetime))
                            sales_updatetime = new DateTime(1900, 1, 1);

                        //최근 매출정보
                        DateTime current_sales_date;
                        if (!DateTime.TryParse(dr["current_sale_date"].ToString(), out current_sales_date))
                            current_sales_date = new DateTime(1900, 1, 1);


                        string ato_manager = dr["ato_manager"].ToString();
                        string pre_ato_manager = dr["ato_manager"].ToString();
                        string sale_edit_user = dr["sales_edit_user"].ToString();
                        string sale_contents = dr["sales_contents"].ToString();
                        /*if (sale_contents == "씨오버 매출")
                            sale_contents = seaoverDt1.Rows[i]["sales_log"].ToString();*/
                        string sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                        string current_sale_date = current_sales_date.ToString("yyyy-MM-dd HH:mm:ss");
                        string sale_remark = dr["sales_remark"].ToString();
                        string sale_log = dr["sales_log"].ToString();

                        DataRow[] subDr = seaoverDt.Select($"main_id = {main_id} AND isDelete = 'FALSE'");
                        for (int j = 0; j < subDr.Length; j++)
                        {
                            if (!isOutBusiness)
                            {
                                DateTime tempUpdatetime;
                                if (DateTime.TryParse(subDr[j]["sales_updatetime"].ToString(), out tempUpdatetime))
                                {
                                    if (sales_updatetime < tempUpdatetime)
                                    {
                                        if (DateTime.TryParse(subDr[j]["current_sale_date"].ToString(), out current_sales_date))
                                            current_sale_date = current_sales_date.ToString("yyyy-MM-dd");

                                        sales_updatetime = tempUpdatetime;
                                        ato_manager = subDr[j]["ato_manager"].ToString();
                                        pre_ato_manager = subDr[j]["ato_manager"].ToString();
                                        sale_edit_user = subDr[j]["sales_edit_user"].ToString();
                                        sale_contents = subDr[j]["sales_contents"].ToString();
                                        /*if (sale_contents == "씨오버 매출")
                                            sale_contents = seaoverDt1.Rows[i]["sales_log"].ToString();*/
                                        sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                                        sale_remark = subDr[j]["sales_remark"].ToString();
                                        sale_log = subDr[j]["sales_log"].ToString();
                                    }
                                }
                            }
                            //대표거래처가 폐업처리 되었을 경우 그룹을 품
                            else
                            {
                                subDr[j]["main_id"] = "0";
                                subDr[j]["sub_id"] = "0";
                            }
                        }
                        //최신화
                        dr["ato_manager"] = ato_manager;
                        dr["pre_ato_manager"] = ato_manager;
                        dr["sales_updatetime"] = sale_date;
                        dr["current_sale_date"] = current_sale_date;
                        dr["sales_edit_user"] = sale_edit_user;
                        dr["sales_contents"] = sale_contents;
                        dr["sales_remark"] = sale_remark;
                        dr["sales_log"] = sale_log;
                    }
                }
            }
            seaoverDt.AcceptChanges();
            //마지막 table_index, industry_type_rank, category 정리
            atoDt.Merge(seaoverDt);
            atoDt.Columns.Add("table_index", typeof(Int32));
            for (int i = 0; i < atoDt.Rows.Count; i++)
            {
                atoDt.Rows[i]["table_index"] = i;
                atoDt.Rows[i]["industry_type_rank"] = GetIndustryTypeRank(atoDt.Rows[i]["industry_type"].ToString());

                bool isTrading;
                if (!bool.TryParse(atoDt.Rows[i]["isTrading"].ToString(), out isTrading))
                    isTrading = false;
                bool isPotential1, isPotential2;
                if (!bool.TryParse(atoDt.Rows[i]["isPotential1"].ToString(), out isPotential1))
                    isPotential1 = false;
                if (!bool.TryParse(atoDt.Rows[i]["isPotential2"].ToString(), out isPotential2))
                    isPotential2 = false;
                bool isNonHandled, isNotSendFax, isOutBusiness;
                if (!bool.TryParse(atoDt.Rows[i]["isNonHandled"].ToString(), out isNonHandled))
                    isNonHandled = false;
                if (!bool.TryParse(atoDt.Rows[i]["isNotSendFax"].ToString(), out isNotSendFax))
                    isNotSendFax = false;
                if (!bool.TryParse(atoDt.Rows[i]["isOutBusiness"].ToString(), out isOutBusiness))
                    isOutBusiness = false;

                //카테고리
                string category = "";
                if (isNonHandled || isOutBusiness)
                    category = "취급X";
                else if (isTrading)
                    category = "거래중";
                else if (isPotential1 && !string.IsNullOrEmpty(atoDt.Rows[i]["ato_manager"].ToString().Trim()))
                    category = "잠재1";
                else if (isPotential2 && !string.IsNullOrEmpty(atoDt.Rows[i]["ato_manager"].ToString().Trim()))
                    category = "잠재2";
                else if (!string.IsNullOrEmpty(atoDt.Rows[i]["ato_manager"].ToString().Trim()))
                    category = "내DATA";
                else
                    category = "공용DATA";
                atoDt.Rows[i]["category"] = category;
            }
            atoDt.AcceptChanges();
            //최신화 일자
            dataUpdatetime = DateTime.Now;
            processingFlag = false;
        }
        #endregion

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

        private void CompleteAlarm()
        {
            //잠재1 에서는 기능X
            
            if (tcMain.SelectedTab.Name != "tabAlarm")
                return;
            //선택한 내역 선택)
            if (dgvCompany.SelectedRows.Count == 0 && dgvCompany.SelectedCells.Count == 0)
            {
                messageBox.Show(this, "거래처를 먼저 선택해주세요!");
                this.Activate();
            }
            else if (dgvCompany.SelectedRows.Count == 0 && dgvCompany.SelectedCells.Count > 0)
            {
                int rowindex = dgvCompany.SelectedCells[0].RowIndex;
                dgvCompany.ClearSelection();
                dgvCompany.Rows[rowindex].Selected = true;
            }

            if (messageBox.Show(this, "알람 완료처리 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                this.Activate();
                return;
            }

            List<StringBuilder> sqlList = new List<StringBuilder>();
            dgvCompany.EndEdit();
            //알람 처리여부
            DateTime alarm_standard_date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            if (tcMain.SelectedTab.Name == "tabAlarm")
            {
                /*switch (cbAlarmDivision.Text)
                {
                    case "어제":
                        alarm_standard_date = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                        break;
                    case "내일":
                        alarm_standard_date = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                        break;
                }*/
            }

            foreach (DataGridViewRow row in dgvCompany.Rows)
            {
                if (row.Selected)
                {
                    //알람일 경우 처리할건지 확인
                    bool isAlarmComplete = false;
                    bool isComplete = true;
                    DateTime complete_date;
                    if (row.Cells["alarm_complete_date"].Value == null || !DateTime.TryParse(row.Cells["alarm_complete_date"].Value.ToString(), out complete_date))
                        complete_date = new DateTime(1900, 1, 1);


                    if (row.Cells["alarm_date"].Value != null && !string.IsNullOrEmpty(row.Cells["alarm_date"].Value.ToString()))
                    {
                        string[] alarms = row.Cells["alarm_date"].Value.ToString().Split('\n');
                        foreach (string alarm in alarms)
                        {
                            if (!string.IsNullOrEmpty(alarm.Trim()))
                            {
                                string[] alarm_detail = alarm.Split('_');
                                if (DateTime.TryParse(alarm_detail[0], out DateTime alarmDt)
                                    && Convert.ToDateTime(alarmDt.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(alarm_standard_date.ToString("yyyy-MM-dd"))
                                    && Convert.ToDateTime(alarmDt.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(complete_date.ToString("yyyy-MM-dd")))
                                {
                                    isComplete = false;
                                    break;
                                }
                            }
                        }
                    }
                    if (!isComplete)
                    {
                        StringBuilder sql = commonRepository.UpdateData("t_company", $"alarm_complete_date = '{alarm_standard_date}', updatetime = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', edit_user = '{um.user_name}'", $"id = {row.Cells["id"].Value.ToString()}");
                        sqlList.Add(sql);
                    }
                }
            }


            if (sqlList.Count > 0)
            {
                if (commonRepository.UpdateTran(sqlList) == -1)
                {
                    messageBox.Show(this,"등록중 에러가 발생했습니다.");
                    this.Activate();
                }
                else
                {
                    SetCellValueEvent(false);
                    foreach (DataGridViewRow row in dgvCompany.Rows)
                    {
                        if (row.Selected)
                        {
                            int table_index = Convert.ToInt32(row.Cells["table_index"].Value.ToString());
                            atoDt.Rows[table_index]["alarm_complete_date"] = alarm_standard_date;
                            //atoDt.Rows[table_index]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            //row.Cells["sales_updatetime"].Value = alarm_standard_date;
                            row.Cells["alarm_complete_date"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            row.Cells["chk"].Value = false;
                            row.DefaultCellStyle.ForeColor = Color.LightGray;
                        }
                    }
                    SetCellValueEvent(true);
                }
            }

        }
        public bool isAlarmSheet()
        {
            if (tcMain.SelectedTab.Name == "tabAlarm" && !rbAlarmIncomplete.Checked)
                return true;
            else
                return false;
        }
        private void CancelAlarm()
        {
            //잠재1 에서는 기능X

            if (tcMain.SelectedTab.Name != "tabAlarm")
                return;
            //선택한 내역 선택)
            if (dgvCompany.SelectedRows.Count == 0 && dgvCompany.SelectedCells.Count == 0)
            {
                messageBox.Show(this, "거래처를 먼저 선택해주세요!");
                this.Activate();
            }
            else if (dgvCompany.SelectedRows.Count == 0 && dgvCompany.SelectedCells.Count > 0)
            {
                int rowindex = dgvCompany.SelectedCells[0].RowIndex;
                dgvCompany.ClearSelection();
                dgvCompany.Rows[rowindex].Selected = true;
            }

            if (messageBox.Show(this, "알람 취소처리 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                this.Activate();
                return;
            }

            List<StringBuilder> sqlList = new List<StringBuilder>();
            dgvCompany.EndEdit();
            //알람 처리여부
            DateTime alarm_standard_date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            if (tcMain.SelectedTab.Name == "tabAlarm")
            {
                /*switch (cbAlarmDivision.Text)
                {
                    case "어제":
                        alarm_standard_date = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                        break;
                    case "내일":
                        alarm_standard_date = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                        break;
                }*/
            }

            foreach (DataGridViewRow row in dgvCompany.Rows)
            {
                if (row.Selected)
                {
                    //알람일 경우 처리할건지 확인
                    bool isAlarmComplete = false;
                    bool isComplete = true;
                    DateTime complete_date;
                    if (row.Cells["alarm_complete_date"].Value == null || !DateTime.TryParse(row.Cells["alarm_complete_date"].Value.ToString(), out complete_date))
                        complete_date = new DateTime(1900, 1, 1);


                    if (row.Cells["alarm_date"].Value != null && !string.IsNullOrEmpty(row.Cells["alarm_date"].Value.ToString()))
                    {
                        string[] alarms = row.Cells["alarm_date"].Value.ToString().Split('\n');
                        foreach (string alarm in alarms)
                        {
                            if (!string.IsNullOrEmpty(alarm.Trim()))
                            {
                                string[] alarm_detail = alarm.Split('_');
                                if (DateTime.TryParse(alarm_detail[0], out DateTime alarmDt)
                                    && Convert.ToDateTime(alarmDt.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(alarm_standard_date.ToString("yyyy-MM-dd"))
                                    && Convert.ToDateTime(alarmDt.ToString("yyyy-MM-dd")) > Convert.ToDateTime(complete_date.ToString("yyyy-MM-dd")))
                                {
                                    isComplete = false;
                                    break;
                                }
                            }
                        }
                    }
                    if (isComplete)
                    {
                        StringBuilder sql = commonRepository.UpdateData("t_company", $"alarm_complete_date = '{alarm_standard_date.AddDays(-1)}', updatetime = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', edit_user = '{um.user_name}'", $"id = {row.Cells["id"].Value.ToString()}");
                        sqlList.Add(sql);
                    }
                }
            }


            if (sqlList.Count > 0)
            {
                if (commonRepository.UpdateTran(sqlList) == -1)
                {
                    messageBox.Show(this, "등록중 에러가 발생했습니다.");
                    this.Activate();
                }
                else
                {
                    SetCellValueEvent(false);
                    foreach (DataGridViewRow row in dgvCompany.Rows)
                    {
                        if (row.Selected)
                        {
                            //알람일 경우 처리할건지 확인
                            bool isAlarmComplete = false;
                            bool isComplete = true;
                            DateTime complete_date;
                            if (row.Cells["alarm_complete_date"].Value == null || !DateTime.TryParse(row.Cells["alarm_complete_date"].Value.ToString(), out complete_date))
                                complete_date = new DateTime(1900, 1, 1);


                            if (row.Cells["alarm_date"].Value != null && !string.IsNullOrEmpty(row.Cells["alarm_date"].Value.ToString()))
                            {
                                string[] alarms = row.Cells["alarm_date"].Value.ToString().Split('\n');
                                foreach (string alarm in alarms)
                                {
                                    if (!string.IsNullOrEmpty(alarm.Trim()))
                                    {
                                        string[] alarm_detail = alarm.Split('_');
                                        if (DateTime.TryParse(alarm_detail[0], out DateTime alarmDt)
                                            && Convert.ToDateTime(alarmDt.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(alarm_standard_date.ToString("yyyy-MM-dd"))
                                            && Convert.ToDateTime(alarmDt.ToString("yyyy-MM-dd")) > Convert.ToDateTime(complete_date.ToString("yyyy-MM-dd")))
                                        {
                                            isComplete = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (isComplete)
                            {
                                int table_index = Convert.ToInt32(row.Cells["table_index"].Value.ToString());
                                atoDt.Rows[table_index]["alarm_complete_date"] = alarm_standard_date.AddDays(-1);
                                //atoDt.Rows[table_index]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                //row.Cells["sales_updatetime"].Value = alarm_standard_date.AddDays(-1);
                                row.Cells["alarm_complete_date"].Value = alarm_standard_date.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                                row.Cells["chk"].Value = false;
                                row.DefaultCellStyle.ForeColor = Color.Black;
                            }
                        }
                    }
                    SetCellValueEvent(true);

                    messageBox.Show(this, "취소완료");
                    this.Activate();
                }
            }
            else
            {
                messageBox.Show(this, "완료처리된 거래처를 찾을 수 없습니다.");
                this.Activate();
            }
        }
        private void SetInputCompanyColumn()
        {
            if (dgvCompany.Columns.Count > 0)
            {
                dgvInputCompany.Rows.Clear();
                dgvInputCompany.Columns.Clear();
                for (int i = 0; i < dgvCompany.Columns.Count; i++)
                {
                    if (dgvCompany.Columns[i].Name == "company"
                        || dgvCompany.Columns[i].Name == "seaover_company_code"
                        || dgvCompany.Columns[i].Name == "registration_number"
                        || dgvCompany.Columns[i].Name == "ceo"
                        || dgvCompany.Columns[i].Name == "tel"
                        || dgvCompany.Columns[i].Name == "fax"
                        || dgvCompany.Columns[i].Name == "phone"
                        || dgvCompany.Columns[i].Name == "other_phone"
                        || dgvCompany.Columns[i].Name == "email"
                        || dgvCompany.Columns[i].Name == "company_manager"
                        || dgvCompany.Columns[i].Name == "company_manager_position"
                        || dgvCompany.Columns[i].Name == "address"
                        || dgvCompany.Columns[i].Name == "web"
                        || dgvCompany.Columns[i].Name == "remark"
                        || dgvCompany.Columns[i].Name == "remark5"
                        || dgvCompany.Columns[i].Name == "remark6"
                        || dgvCompany.Columns[i].Name == "isNotSendFax"
                        || dgvCompany.Columns[i].Name == "distribution"
                        || dgvCompany.Columns[i].Name == "handling_item"
                        || dgvCompany.Columns[i].Name == "payment_date"
                        || dgvCompany.Columns[i].Name == "industry_type"
                        || dgvCompany.Columns[i].Name == "industry_type2")

                    {
                        string col_name = "input_" + dgvCompany.Columns[i].Name;
                        if (dgvCompany.Columns[i].Name == "isNotSendFax")
                        {
                            DataGridViewCheckBoxColumn ccCell = new DataGridViewCheckBoxColumn();
                            ccCell.ValueType = typeof(bool);
                            ccCell.Name = col_name;
                            ccCell.HeaderText = dgvCompany.Columns[i].HeaderText;
                            dgvInputCompany.Columns.Add(ccCell);
                        }
                        else
                            dgvInputCompany.Columns.Add(col_name, dgvCompany.Columns[i].HeaderText);
                        dgvInputCompany.Columns[col_name].HeaderCell.Style = dgvCompany.Columns[i].HeaderCell.Style;
                        dgvInputCompany.Columns[col_name].DefaultCellStyle = dgvCompany.Columns[i].DefaultCellStyle;
                        dgvInputCompany.Columns[col_name].Width = dgvCompany.Columns[i].Width;
                    }
                }

                if (dgvInputCompany.Columns.Count > 0)
                {
                    foreach (DataGridViewColumn col in dgvInputCompany.Columns)
                    {
                        if (col.Name == "input_seaover_company_code" || col.Name == "input_address" || col.Name == "input_ceo")
                            col.Visible = !cbSimpleInput.Checked;
                    }
                }


                DataGridViewCheckBoxColumn isIgnoreCell = new DataGridViewCheckBoxColumn();
                isIgnoreCell.ValueType = typeof(bool);
                isIgnoreCell.Name = "isIgnore";
                isIgnoreCell.HeaderText = "isIgnore";
                isIgnoreCell.Visible = false;
                dgvInputCompany.Columns.Add(isIgnoreCell);

                DataGridViewCheckBoxColumn isRecoveryCell = new DataGridViewCheckBoxColumn();
                isRecoveryCell.ValueType = typeof(bool);
                isRecoveryCell.Name = "isRecovery";
                isRecoveryCell.HeaderText = "isRecovery";
                isRecoveryCell.Visible = false;
                dgvInputCompany.Columns.Add(isRecoveryCell);

                DataGridViewCheckBoxColumn isDeleteCell = new DataGridViewCheckBoxColumn();
                isDeleteCell.ValueType = typeof(bool);
                isDeleteCell.Name = "isDelete";
                isDeleteCell.HeaderText = "isDelete";
                isDeleteCell.Visible = false;
                dgvInputCompany.Columns.Add(isDeleteCell);

                dgvInputCompany.Columns.Add("duplicate_table_index", "duplicate_table_index");
                dgvInputCompany.Columns["duplicate_table_index"].Visible = false;
            }
        }
        private void AddCompanyInfoRefresh()
        {
            dgvInputCompany.Rows.Clear();
        }
        private bool isVailidation()
        {
            dgvInputCompany.EndEdit();
            for (int i = dgvInputCompany.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow row = dgvInputCompany.Rows[i];
                if (row.Cells["input_company"].Value == null || string.IsNullOrEmpty(row.Cells["input_company"].Value.ToString().Trim()))
                    dgvInputCompany.Rows.Remove(row);
            }

            if (dgvInputCompany.Rows.Count == 0)
            {
                messageBox.Show(this,"등록할 거래처가 없습니다!");
                dgvInputCompany.Rows.Add();
                this.Activate();
                return false;
            }

            foreach(DataGridViewRow row in dgvInputCompany.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value == null)
                        cell.Value = string.Empty;
                }
                
                if (string.IsNullOrEmpty(row.Cells["input_registration_number"].Value.ToString().Trim()) 
                    && string.IsNullOrEmpty(row.Cells["input_tel"].Value.ToString().Trim()) 
                    && string.IsNullOrEmpty(row.Cells["input_fax"].Value.ToString().Trim()) 
                    && string.IsNullOrEmpty(row.Cells["input_phone"].Value.ToString().Trim())
                    && string.IsNullOrEmpty(row.Cells["input_other_phone"].Value.ToString().Trim())
                    && string.IsNullOrEmpty(row.Cells["input_address"].Value.ToString().Trim())
                    && string.IsNullOrEmpty(row.Cells["input_email"].Value.ToString().Trim()))
                {
                    messageBox.Show(this,"거래처명을 입력하고 사업자등록번호, TEL, FAX, PHONE, 기타연락처, 주소, Email 중 하나는 필수 입력값입니다!");
                    dgvInputCompany.ClearSelection();
                    row.Selected = true;
                    this.Activate();
                    return false;
                }

                row.Cells["input_tel"].Value = ValidationTelString(row.Cells["input_tel"].Value.ToString());
                row.Cells["input_fax"].Value = ValidationTelString(row.Cells["input_fax"].Value.ToString());
                row.Cells["input_phone"].Value = ValidationTelString(row.Cells["input_phone"].Value.ToString());
                row.Cells["input_other_phone"].Value = ValidationTelString(row.Cells["input_other_phone"].Value.ToString());
            }

            return true;
        }
        //최근수정내역
        private void GetCurrentUpdateInfo(int rowindex)
        {
            lvUpdate.Items.Clear();
            if (rowindex >= 0)
            {
                DataGridViewRow row = dgvCompany.Rows[rowindex];
                dgvUpdateList.Rows.Clear();
                DataTable updateDt = salesPartnerRepository.GetUpdateList(row.Cells["id"].Value.ToString());
                for (int i = 0; i < updateDt.Rows.Count; i++)
                {
                    //int n = dgvUpdateList.Rows.Add();
                    string[] arr = new string[3];
                    DateTime updatetime;
                    if (DateTime.TryParse(updateDt.Rows[i]["updatetime"].ToString(), out updatetime))
                        arr[0] = updatetime.ToString("yyyy-MM-dd");

                    arr[1] = updateDt.Rows[i]["contents"].ToString();
                    string log = updateDt.Rows[i]["log"].ToString();
                    if (log.Contains("|"))
                        arr[1] = log.Split('|')[1].Trim() + " → " + updateDt.Rows[i]["contents"].ToString();
                    else
                        arr[1] = updateDt.Rows[i]["contents"].ToString();

                    arr[2] = updateDt.Rows[i]["edit_user"].ToString();


                    ListViewItem itm = new ListViewItem(arr);
                    lvUpdate.Items.Add(itm);

                }
                lvUpdate.EndUpdate();
                lbCompanyName.Text = row.Cells["company"].Value.ToString();
                lbCompanyId.Text = row.Cells["id"].Value.ToString();
            }
        }
        //중복거래처
        private void CheckDuplicate(int rowindex)
        {
            lvDuplicate.Items.Clear();
            if (dgvCompany.Rows.Count > 0)
            {
                //유효성검사
                dgvCompany.EndEdit();
                DataGridViewRow row = dgvCompany.Rows[rowindex];
                int idx = Convert.ToInt32(row.Cells["table_index"].Value.ToString());
                if (duplicateDic.ContainsKey(idx))
                {
                    List<DataRow> list = duplicateDic[idx];
                    for (int i = 0; i < list.Count; i++)
                    {
                        string[] arr = new string[3];
                        arr[0] = list[i]["ato_manager"].ToString();
                        arr[1] = list[i]["company"].ToString();
                        arr[2] = list[i]["division"].ToString();
                        ListViewItem itm = new ListViewItem(arr);
                        lvDuplicate.Items.Add(itm);
                    }
                }
            }
            lvDuplicate.Update();
        }
        public void SetGroupCompany(int rowIndex, int main_id, int sub_id, string ato_manager, string sale_edit_user, string sale_contents, string sale_date, string sale_remark)
        {
            atoDt.Rows[rowIndex]["main_id"] = main_id;
            atoDt.Rows[rowIndex]["sub_id"] = sub_id;

            if (sub_id == 0)
            {
                atoDt.Rows[rowIndex]["ato_manager"] = ato_manager;
                atoDt.Rows[rowIndex]["sales_edit_user"] = sale_edit_user;
                atoDt.Rows[rowIndex]["sales_contents"] = sale_contents;
                atoDt.Rows[rowIndex]["sales_updatetime"] = sale_date;
                atoDt.Rows[rowIndex]["sales_remark"] = sale_remark;
            }

            atoDt.AcceptChanges();
        }
        public DataTable SetCompanyData(int rowIndex, bool isDelete)
        {
            atoDt.Rows[rowIndex]["isDelete"] = isDelete;

            atoDt.AcceptChanges();
            return atoDt;
        }

        public DataTable HideCompanyData(int rowIndex, bool isDelete)
        {
            atoDt.Rows[rowIndex]["isDelete"] = isDelete;
            atoDt.Rows[rowIndex]["isHide"] = isDelete;

            atoDt.AcceptChanges();
            return atoDt;
        }

        public void UpdateCompany(int updateType, bool isMsgShow = true)
        {
            switch (updateType)
            {
                case 1:

                    UpdateCompanyInfo(1);

                    SetCellValueEvent(false);
                    for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                    {
                        if (Convert.ToBoolean(dgvCompany.Rows[i].Cells["chk"].Value))
                            dgvCompany.Rows[i].Cells["chk"].Value = false;
                    }
                    SetCellValueEvent(true);

                    break;
                case 2:
                    if (messageBox.Show(this, "모든 담당자가 확인할 수 있는 공용DATA로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;

                    if (UpdateCompanyInfo(2))
                    {
                        for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                        {
                            if (Convert.ToBoolean(dgvCompany.Rows[i].Selected))
                                dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                        }
                    }
                    break;
                case 3:
                    if (messageBox.Show(this, um.user_name + "님만 확인할 수 있는 내DATA로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;

                    if (UpdateCompanyInfo(3))
                    {
                        for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                        {
                            if (Convert.ToBoolean(dgvCompany.Rows[i].Selected))
                                dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                        }
                    }
                    break;
                case 4:

                    if (messageBox.Show(this, "잠재1로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;

                    if (UpdateCompanyInfo(4))
                    {
                        for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                        {
                            if (Convert.ToBoolean(dgvCompany.Rows[i].Selected))
                                dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                        }
                    }
                    break;
                case 5:
                    if (messageBox.Show(this, "잠재2로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;
                    if (UpdateCompanyInfo(5))
                    {
                        for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                        {
                            if (Convert.ToBoolean(dgvCompany.Rows[i].Selected))
                                dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                        }
                    }
                    break;

                case 6:
                    if (messageBox.Show(this, "SEAOVER 거래처중에서만 [거래중(SEAOVER)] 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;
                    if (UpdateCompanyInfo(9))
                    {
                        for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                        {
                            if (Convert.ToBoolean(dgvCompany.Rows[i].Selected))
                                dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                        }
                    }
                    break;
                case 7:
                    if (messageBox.Show(this, "[영업금지 거래처] 취급X 거래처로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;
                    if (UpdateCompanyInfo(6))
                    {
                        for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                        {
                            if (Convert.ToBoolean(dgvCompany.Rows[i].Selected))
                                dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                        }
                    }
                    break;
                case 8:
                    if (messageBox.Show(this, "[영업금지 거래처] 팩스X 거래처로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;

                    if (UpdateCompanyInfo(7))
                    {
                        for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                        {
                            if (Convert.ToBoolean(dgvCompany.Rows[i].Selected))
                                dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                        }
                    }
                    break;
                case 9:
                    if (messageBox.Show(this, "[영업금지 거래처] 폐업 거래처로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;
                    if (UpdateCompanyInfo(8))
                    {
                        for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                        {
                            if (Convert.ToBoolean(dgvCompany.Rows[i].Selected))
                                dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                        }
                    }
                    break;
            }

            //GetData(isMsgShow, false);
        }
        public bool SearchingTxt(string find, string except)
        {
            if (!string.IsNullOrEmpty(find))
            {
                string[] find_txt = find.Split(',');
                string[] except_txt = except.Split(',');

                int rowidx = 0, coldx = 0;
                if (dgvCompany.SelectedCells.Count > 0)
                {
                    rowidx = dgvCompany.SelectedCells[0].RowIndex;
                    coldx = dgvCompany.SelectedCells[0].ColumnIndex;
                }

                if (dgvCompany.Rows.Count > 0)
                {
                    //현재행부터 끝까지
                    for (int i = rowidx; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewCell cell = null;
                        if (i == rowidx)
                        {
                            bool is_find = false;
                            for (int j = coldx; j < dgvCompany.ColumnCount; j++)
                            {
                                if (dgvCompany.Rows[i].Cells[j].Visible && dgvCompany.Rows[i].Cells[j] != dgvCompany.SelectedCells[0])
                                {
                                    cell = dgvCompany.Rows[i].Cells[j];
                                    if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                    {
                                        string val = cell.Value.ToString();
                                        //찾을 단어
                                        if (!string.IsNullOrEmpty(find))
                                        {
                                            for (int k = 0; k < find_txt.Length; k++)
                                            {
                                                //if (!string.IsNullOrEmpty(find_txt[k].Trim()) && val.Contains(find_txt[k].Trim()))
                                                if (!string.IsNullOrEmpty(find_txt[k].Trim()) && common.isContains(val, find_txt[k].Trim()))
                                                {
                                                    is_find = true;
                                                    break;
                                                }
                                            }
                                        }
                                        //제외 단어
                                        if (is_find && !string.IsNullOrEmpty(except))
                                        {
                                            for (int k = 0; k < except_txt.Length; k++)
                                            {
                                                //if (!string.IsNullOrEmpty(except_txt[k].Trim()) && val.Contains(except_txt[k].Trim()))
                                                if (!string.IsNullOrEmpty(except_txt[k].Trim()) && common.isContains(val, except_txt[k].Trim()))
                                                {
                                                    is_find = false;
                                                    break;
                                                }
                                            }
                                        }
                                        //찾기 성공!
                                        if (is_find)
                                        {
                                            dgvCompany.CurrentCell = cell;
                                            return true;
                                        }
                                    }
                                }
                            }
                            coldx = 0;
                        }
                        else
                        {
                            for (int j = 0; j < dgvCompany.ColumnCount; j++)
                            {
                                if (dgvCompany.Rows[i].Cells[j].Visible && dgvCompany.Rows[i].Cells[j] != dgvCompany.SelectedCells[0])
                                {
                                    cell = dgvCompany.Rows[i].Cells[j];
                                    if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                    {
                                        bool is_find = false;
                                        string val = cell.Value.ToString();
                                        //찾을 단어
                                        if (!string.IsNullOrEmpty(find))
                                        {
                                            for (int k = 0; k < find_txt.Length; k++)
                                            {
                                                //if (!string.IsNullOrEmpty(find_txt[k].Trim()) && val.Contains(find_txt[k].Trim()))
                                                if (!string.IsNullOrEmpty(find_txt[k].Trim()) && common.isContains(val, find_txt[k].Trim()))
                                                {
                                                    is_find = true;
                                                    break;
                                                }
                                            }
                                        }
                                        //제외 단어
                                        if (is_find && !string.IsNullOrEmpty(except))
                                        {
                                            for (int k = 0; k < except_txt.Length; k++)
                                            {
                                                //if (!string.IsNullOrEmpty(except_txt[k].Trim()) && val.Contains(except_txt[k].Trim()))
                                                if (!string.IsNullOrEmpty(except_txt[k].Trim()) && common.isContains(val, except_txt[k].Trim()))
                                                {
                                                    is_find = false;
                                                    break;
                                                }
                                            }
                                        }
                                        //찾기 성공!
                                        if (is_find)
                                        {
                                            dgvCompany.CurrentCell = cell;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //첨부터 현재행까지
                    for (int i = 0; i <= rowidx; i++)
                    {
                        DataGridViewCell cell = null;
                        if (i == rowidx)
                        {
                            for (int j = 0; j < dgvCompany.SelectedCells[0].ColumnIndex; j++)
                            {
                                if (dgvCompany.Rows[i].Cells[j].Visible && dgvCompany.Rows[i].Cells[j] != dgvCompany.SelectedCells[0])
                                {
                                    cell = dgvCompany.Rows[i].Cells[j];
                                    if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                    {
                                        bool is_find = false;
                                        string val = cell.Value.ToString();
                                        //찾을 단어
                                        if (!string.IsNullOrEmpty(find))
                                        {
                                            for (int k = 0; k < find_txt.Length; k++)
                                            {
                                                //if (!string.IsNullOrEmpty(find_txt[k].Trim()) && val.Contains(find_txt[k].Trim()))
                                                if (!string.IsNullOrEmpty(find_txt[k].Trim()) && common.isContains(val, find_txt[k].Trim()))
                                                {
                                                    is_find = true;
                                                    break;
                                                }
                                            }
                                        }
                                        //제외 단어
                                        if (is_find && !string.IsNullOrEmpty(except))
                                        {
                                            for (int k = 0; k < except_txt.Length; k++)
                                            {
                                                //if (!string.IsNullOrEmpty(except_txt[k].Trim()) && val.Contains(except_txt[k].Trim()))
                                                if (!string.IsNullOrEmpty(except_txt[k].Trim()) && common.isContains(val, except_txt[k].Trim()))
                                                {
                                                    is_find = false;
                                                    break;
                                                }
                                            }
                                        }
                                        //찾기 성공!
                                        if (is_find)
                                        {
                                            dgvCompany.CurrentCell = cell;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < dgvCompany.ColumnCount; j++)
                            {
                                if (dgvCompany.Rows[i].Cells[j].Visible && dgvCompany.Rows[i].Cells[j] != dgvCompany.SelectedCells[0])
                                {
                                    cell = dgvCompany.Rows[i].Cells[j];
                                    if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                    {
                                        bool is_find = false;
                                        string val = cell.Value.ToString();
                                        //찾을 단어
                                        if (!string.IsNullOrEmpty(find))
                                        {
                                            for (int k = 0; k < find_txt.Length; k++)
                                            {
                                                //if (!string.IsNullOrEmpty(find_txt[k].Trim()) && val.Contains(find_txt[k].Trim()))
                                                if (!string.IsNullOrEmpty(find_txt[k].Trim()) && common.isContains(val, find_txt[k].Trim()))
                                                {
                                                    is_find = true;
                                                    break;
                                                }
                                            }
                                        }
                                        //제외 단어
                                        if (is_find && !string.IsNullOrEmpty(except))
                                        {
                                            for (int k = 0; k < except_txt.Length; k++)
                                            {
                                                //if (!string.IsNullOrEmpty(except_txt[k].Trim()) && val.Contains(except_txt[k].Trim()))
                                                if (!string.IsNullOrEmpty(except_txt[k].Trim()) && common.isContains(val, except_txt[k].Trim()))
                                                {
                                                    is_find = false;
                                                    break;
                                                }
                                            }
                                        }
                                        //찾기 성공!
                                        if (is_find)
                                        {
                                            dgvCompany.CurrentCell = cell;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }


                        if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                        {
                            bool is_find = false;
                            string val = cell.Value.ToString();
                            //찾을 단어
                            if (!string.IsNullOrEmpty(find))
                            {
                                for (int k = 0; k < find_txt.Length; k++)
                                {
                                    //if (!string.IsNullOrEmpty(find_txt[k].Trim()) && val.Contains(find_txt[k].Trim()))
                                    if (!string.IsNullOrEmpty(find_txt[k].Trim()) && common.isContains(val, find_txt[k].Trim()))
                                    {
                                        is_find = true;
                                        break;
                                    }
                                }
                            }
                            //제외 단어
                            if (is_find && !string.IsNullOrEmpty(except))
                            {
                                for (int k = 0; k < except_txt.Length; k++)
                                {
                                    //if (!string.IsNullOrEmpty(except_txt[k].Trim()) && val.Contains(except_txt[k].Trim()))
                                    if (!string.IsNullOrEmpty(except_txt[k].Trim()) && common.isContains(val, except_txt[k].Trim()))
                                    {
                                        is_find = false;
                                        break;
                                    }
                                }
                            }
                            //찾기 성공!
                            if (is_find)
                            {
                                dgvCompany.CurrentCell = cell;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        private DataTable SetTable()
        {
            DataTable dt = new DataTable();

            DataColumn col01 = new DataColumn();
            col01.DataType = System.Type.GetType("System.Boolean");
            col01.AllowDBNull = false;
            col01.ColumnName = "chk";
            col01.Caption = "";
            col01.DefaultValue = false;
            dt.Columns.Add(col01);

            DataColumn col02 = new DataColumn();
            col02.DataType = System.Type.GetType("System.Double");
            col02.AllowDBNull = true;
            col02.ColumnName = "id";
            col02.Caption = "id";
            col02.DefaultValue = null;
            dt.Columns.Add(col02);

            DataColumn col0233 = new DataColumn();
            col0233.DataType = System.Type.GetType("System.String");
            col0233.AllowDBNull = true;
            col0233.ColumnName = "category";
            col0233.Caption = "카테고리";
            col0233.DefaultValue = "";
            dt.Columns.Add(col0233);

            DataColumn col03 = new DataColumn();
            col03.DataType = System.Type.GetType("System.String");
            col03.AllowDBNull = false;
            col03.ColumnName = "group_name";
            col03.Caption = "그룹";
            col03.DefaultValue = "";
            dt.Columns.Add(col03);

            DataColumn col04 = new DataColumn();
            col04.DataType = System.Type.GetType("System.String");
            col04.AllowDBNull = false;
            col04.ColumnName = "company";
            col04.Caption = "거래처";
            col04.DefaultValue = "";
            dt.Columns.Add(col04);

            DataColumn col21 = new DataColumn();
            col21.DataType = System.Type.GetType("System.String");
            col21.AllowDBNull = false;
            col21.ColumnName = "seaover_company_code";
            col21.Caption = "씨오버코드";
            col21.DefaultValue = "";
            dt.Columns.Add(col21);

            DataColumn col08 = new DataColumn();
            col08.DataType = System.Type.GetType("System.String");
            col08.AllowDBNull = false;
            col08.ColumnName = "registration_number";
            col08.Caption = "사업자번호";
            col08.DefaultValue = "";
            dt.Columns.Add(col08);

            DataColumn col081 = new DataColumn();
            col081.DataType = System.Type.GetType("System.String");
            col081.AllowDBNull = false;
            col081.ColumnName = "origin";
            col081.Caption = "국가";
            col081.DefaultValue = "";
            dt.Columns.Add(col081);

            DataColumn col090 = new DataColumn();
            col090.DataType = System.Type.GetType("System.String");
            col090.AllowDBNull = false;
            col090.ColumnName = "address";
            col090.Caption = "주소";
            col090.DefaultValue = "";
            dt.Columns.Add(col090);

            DataColumn col09 = new DataColumn();
            col09.DataType = System.Type.GetType("System.String");
            col09.AllowDBNull = false;
            col09.ColumnName = "ceo";
            col09.Caption = "대표자";
            col09.DefaultValue = "";
            dt.Columns.Add(col09);

            DataColumn col05 = new DataColumn();
            col05.DataType = System.Type.GetType("System.String");
            col05.AllowDBNull = false;
            col05.ColumnName = "tel";
            col05.Caption = "전화번호";
            col05.DefaultValue = "";
            dt.Columns.Add(col05);

            DataColumn col051 = new DataColumn();
            col051.DataType = System.Type.GetType("System.String");
            col051.AllowDBNull = false;
            col051.ColumnName = "pre_tel";
            col051.Caption = "전화번호";
            col051.DefaultValue = "";
            dt.Columns.Add(col051);

            DataColumn col06 = new DataColumn();
            col06.DataType = System.Type.GetType("System.String");
            col06.AllowDBNull = false;
            col06.ColumnName = "fax";
            col06.Caption = "팩스번호";
            col06.DefaultValue = "";
            dt.Columns.Add(col06);

            DataColumn col061 = new DataColumn();
            col061.DataType = System.Type.GetType("System.String");
            col061.AllowDBNull = false;
            col061.ColumnName = "pre_fax";
            col061.Caption = "팩스번호";
            col061.DefaultValue = "";
            dt.Columns.Add(col061);

            DataColumn col07 = new DataColumn();
            col07.DataType = System.Type.GetType("System.String");
            col07.AllowDBNull = false;
            col07.ColumnName = "phone";
            col07.Caption = "휴대폰";
            col07.DefaultValue = "";
            dt.Columns.Add(col07);

            DataColumn col071 = new DataColumn();
            col071.DataType = System.Type.GetType("System.String");
            col071.AllowDBNull = false;
            col071.ColumnName = "pre_phone";
            col071.Caption = "휴대폰";
            col071.DefaultValue = "";
            dt.Columns.Add(col071);

            DataColumn col07122 = new DataColumn();
            col07122.DataType = System.Type.GetType("System.String");
            col07122.AllowDBNull = false;
            col07122.ColumnName = "other_phone";
            col07122.Caption = "기타연락처";
            col07122.DefaultValue = "";
            dt.Columns.Add(col07122);

            DataColumn col07123 = new DataColumn();
            col07123.DataType = System.Type.GetType("System.String");
            col07123.AllowDBNull = false;
            col07123.ColumnName = "pre_other_phone";
            col07123.Caption = "기타연락처";
            col07123.DefaultValue = "";
            dt.Columns.Add(col07123);

            DataColumn col0711 = new DataColumn();
            col0711.DataType = System.Type.GetType("System.String");
            col0711.AllowDBNull = false;
            col0711.ColumnName = "company_manager";
            col0711.Caption = "업체 담당자";
            col0711.DefaultValue = "";
            dt.Columns.Add(col0711);

            DataColumn col072 = new DataColumn();
            col072.DataType = System.Type.GetType("System.String");
            col072.AllowDBNull = false;
            col072.ColumnName = "company_manager_position";
            col072.Caption = "업체 담당자 직책";
            col072.DefaultValue = "";
            dt.Columns.Add(col072);



            DataColumn col211 = new DataColumn();
            col211.DataType = System.Type.GetType("System.String");
            col211.AllowDBNull = false;
            col211.ColumnName = "email";
            col211.Caption = "Email";
            col211.DefaultValue = "";
            dt.Columns.Add(col211);



            DataColumn col191 = new DataColumn();
            col191.DataType = System.Type.GetType("System.String");
            col191.AllowDBNull = false;
            col191.ColumnName = "sns1";
            col191.Caption = "sns1";
            col191.DefaultValue = "";
            dt.Columns.Add(col191);

            DataColumn col192 = new DataColumn();
            col192.DataType = System.Type.GetType("System.String");
            col192.AllowDBNull = false;
            col192.ColumnName = "sns2";
            col192.Caption = "sns2";
            col192.DefaultValue = "";
            dt.Columns.Add(col192);

            DataColumn col193 = new DataColumn();
            col193.DataType = System.Type.GetType("System.String");
            col193.AllowDBNull = false;
            col193.ColumnName = "sns3";
            col193.Caption = "sns3";
            col193.DefaultValue = "";
            dt.Columns.Add(col193);

            DataColumn col194 = new DataColumn();
            col194.DataType = System.Type.GetType("System.String");
            col194.AllowDBNull = false;
            col194.ColumnName = "web";
            col194.Caption = "홈페이지";
            col194.DefaultValue = "";
            dt.Columns.Add(col194);

            DataColumn col195 = new DataColumn();
            col195.DataType = System.Type.GetType("System.Boolean");
            col195.AllowDBNull = false;
            col195.ColumnName = "isManagement1";
            col195.Caption = "isManagement1";
            col195.DefaultValue = false;
            dt.Columns.Add(col195);

            DataColumn col196 = new DataColumn();
            col196.DataType = System.Type.GetType("System.Boolean");
            col196.AllowDBNull = false;
            col196.ColumnName = "isManagement2";
            col196.Caption = "isManagement2";
            col196.DefaultValue = false;
            dt.Columns.Add(col196);

            DataColumn col197 = new DataColumn();
            col197.DataType = System.Type.GetType("System.Boolean");
            col197.AllowDBNull = false;
            col197.ColumnName = "isManagement3";
            col197.Caption = "isManagement3";
            col197.DefaultValue = false;
            dt.Columns.Add(col197);

            DataColumn col198 = new DataColumn();
            col198.DataType = System.Type.GetType("System.Boolean");
            col198.AllowDBNull = false;
            col198.ColumnName = "isManagement4";
            col198.Caption = "isManagement4";
            col198.DefaultValue = false;
            dt.Columns.Add(col198);

            DataColumn col199 = new DataColumn();
            col199.DataType = System.Type.GetType("System.Boolean");
            col199.AllowDBNull = false;
            col199.ColumnName = "isHide";
            col199.Caption = "isHide";
            col199.DefaultValue = false;
            dt.Columns.Add(col199);

            DataColumn col091 = new DataColumn();
            col091.DataType = System.Type.GetType("System.String");
            col091.AllowDBNull = false;
            col091.ColumnName = "distribution";
            col091.Caption = "유통";
            col091.DefaultValue = "";
            dt.Columns.Add(col091);

            DataColumn col092 = new DataColumn();
            col092.DataType = System.Type.GetType("System.String");
            col092.AllowDBNull = false;
            col092.ColumnName = "handling_item";
            col092.Caption = "취급품목1";
            col092.DefaultValue = "";
            dt.Columns.Add(col092);

            DataColumn col0923 = new DataColumn();
            col092.DataType = System.Type.GetType("System.String");
            col0923.AllowDBNull = false;
            col0923.ColumnName = "seaover_handling_item";
            col0923.Caption = "취급품목2";
            col0923.DefaultValue = "";
            dt.Columns.Add(col0923);

            DataColumn col0933 = new DataColumn();
            col0933.DataType = System.Type.GetType("System.String");
            col0933.AllowDBNull = false;
            col0933.ColumnName = "payment_date";
            col0933.Caption = "결제일자";
            col0933.DefaultValue = "";
            dt.Columns.Add(col0933);

            DataColumn col19 = new DataColumn();
            col19.DataType = System.Type.GetType("System.String");
            col19.AllowDBNull = false;
            col19.ColumnName = "remark";
            col19.Caption = "비고";
            col19.DefaultValue = "";
            dt.Columns.Add(col19);

            DataColumn col190 = new DataColumn();
            col190.DataType = System.Type.GetType("System.String");
            col190.AllowDBNull = false;
            col190.ColumnName = "remark2";
            col190.Caption = "비고2";
            col190.DefaultValue = "";
            dt.Columns.Add(col190);

            DataColumn col19012 = new DataColumn();
            col19012.DataType = System.Type.GetType("System.String");
            col19012.AllowDBNull = false;
            col19012.ColumnName = "remark5";
            col19012.Caption = "비고2";
            col19012.DefaultValue = "";
            dt.Columns.Add(col19012);

            DataColumn col19013 = new DataColumn();
            col19013.DataType = System.Type.GetType("System.String");
            col19013.AllowDBNull = false;
            col19013.ColumnName = "remark6";
            col19013.Caption = "비고3";
            col19013.DefaultValue = "";
            dt.Columns.Add(col19013);

            DataColumn col1901 = new DataColumn();
            col1901.DataType = System.Type.GetType("System.String");
            col1901.AllowDBNull = false;
            col1901.ColumnName = "remark4";
            col1901.Caption = "상태";
            col1901.DefaultValue = "";
            dt.Columns.Add(col1901);

            DataColumn col1902 = new DataColumn();
            col1902.DataType = System.Type.GetType("System.String");
            col1902.AllowDBNull = false;
            col1902.ColumnName = "industry_type";
            col1902.Caption = "업종";
            col1902.DefaultValue = "";
            dt.Columns.Add(col1902);

            DataColumn col19022 = new DataColumn();
            col19022.DataType = System.Type.GetType("System.String");
            col19022.AllowDBNull = false;
            col19022.ColumnName = "industry_type2";
            col19022.Caption = "업종2";
            col19022.DefaultValue = "";
            dt.Columns.Add(col19022);

            DataColumn col19021 = new DataColumn();
            col19021.DataType = System.Type.GetType("System.Int16");
            col19021.AllowDBNull = false;
            col19021.ColumnName = "industry_type_rank";
            col19021.Caption = "업종순위";
            col19021.DefaultValue = 9;
            dt.Columns.Add(col19021);

            DataColumn col093 = new DataColumn();
            col093.DataType = System.Type.GetType("System.String");
            col093.AllowDBNull = false;
            col093.ColumnName = "createtime";
            col093.Caption = "생성일";
            col093.DefaultValue = "";
            dt.Columns.Add(col093);

            DataColumn col27 = new DataColumn();
            col27.DataType = System.Type.GetType("System.String");
            col27.AllowDBNull = false;
            col27.ColumnName = "ato_manager";
            col27.Caption = "아토담당";
            col27.DefaultValue = "";
            dt.Columns.Add(col27);

            DataColumn col271 = new DataColumn();
            col271.DataType = System.Type.GetType("System.String");
            col271.AllowDBNull = false;
            col271.ColumnName = "pre_ato_manager";
            col271.Caption = "전아토담당";
            col271.DefaultValue = "";
            dt.Columns.Add(col271);

            DataColumn col2712 = new DataColumn();
            col2712.DataType = System.Type.GetType("System.String");
            col2712.AllowDBNull = false;
            col2712.ColumnName = "pre_category";
            col2712.Caption = "전카테고리";
            col2712.DefaultValue = "";
            dt.Columns.Add(col2712);

            DataColumn col101 = new DataColumn();
            col101.DataType = System.Type.GetType("System.String");
            col101.AllowDBNull = false;
            col101.ColumnName = "div0";
            col101.Caption = "";
            col101.DefaultValue = "";
            dt.Columns.Add(col101);

            DataColumn col102 = new DataColumn();
            col102.DataType = System.Type.GetType("System.Boolean");
            col102.AllowDBNull = false;
            col102.ColumnName = "isPotential1";
            col102.Caption = "잠재1";
            col102.DefaultValue = false;
            dt.Columns.Add(col102);

            DataColumn col103 = new DataColumn();
            col103.DataType = System.Type.GetType("System.Boolean");
            col103.AllowDBNull = false;
            col103.ColumnName = "isPotential2";
            col103.Caption = "잠재2";
            col103.DefaultValue = false;
            dt.Columns.Add(col103);

            DataColumn col1031 = new DataColumn();
            col1031.DataType = System.Type.GetType("System.Boolean");
            col1031.AllowDBNull = false;
            col1031.ColumnName = "isTrading";
            col1031.Caption = "거래중";
            col1031.DefaultValue = false;
            dt.Columns.Add(col1031);

            DataColumn col24 = new DataColumn();
            col24.DataType = System.Type.GetType("System.Boolean");
            col24.AllowDBNull = false;
            col24.ColumnName = "isNonHandled";
            col24.Caption = "취급X";
            col24.DefaultValue = false;
            dt.Columns.Add(col24);

            DataColumn col26 = new DataColumn();
            col26.DataType = System.Type.GetType("System.Boolean");
            col26.AllowDBNull = false;
            col26.ColumnName = "isNotSendFax";
            col26.Caption = "팩스X";
            col26.DefaultValue = false;
            dt.Columns.Add(col26);

            DataColumn col25 = new DataColumn();
            col25.DataType = System.Type.GetType("System.Boolean");
            col25.AllowDBNull = false;
            col25.ColumnName = "isOutBusiness";
            col25.Caption = "폐업";
            col25.DefaultValue = false;
            dt.Columns.Add(col25);



            DataColumn col10 = new DataColumn();
            col10.DataType = System.Type.GetType("System.String");
            col10.AllowDBNull = false;
            col10.ColumnName = "div1";
            col10.Caption = "";
            col10.DefaultValue = "";
            dt.Columns.Add(col10);

            DataColumn col18 = new DataColumn();
            col18.DataType = System.Type.GetType("System.String");
            col18.AllowDBNull = false;
            col18.ColumnName = "sales_updatetime";
            col18.Caption = "수정일";
            col18.DefaultValue = "";
            dt.Columns.Add(col18);

            DataColumn col188 = new DataColumn();
            col188.DataType = System.Type.GetType("System.String");
            col188.AllowDBNull = false;
            col188.ColumnName = "current_sale_date";
            col188.Caption = "매출일";
            col188.DefaultValue = "";
            dt.Columns.Add(col188);

            DataColumn col15 = new DataColumn();
            col15.DataType = System.Type.GetType("System.String");
            col15.AllowDBNull = false;
            col15.ColumnName = "sales_contents";
            col15.Caption = "최근영업내용";
            col15.DefaultValue = "";
            dt.Columns.Add(col15);

            DataColumn col152 = new DataColumn();
            col152.DataType = System.Type.GetType("System.String");
            col152.AllowDBNull = false;
            col152.ColumnName = "sales_remark";
            col152.Caption = "비고";
            col152.DefaultValue = "";
            dt.Columns.Add(col152);

            DataColumn col1522 = new DataColumn();
            col1522.DataType = System.Type.GetType("System.String");
            col1522.AllowDBNull = false;
            col1522.ColumnName = "sales_log";
            col1522.Caption = "log";
            col1522.DefaultValue = "";
            dt.Columns.Add(col1522);

            DataColumn col20 = new DataColumn();
            col20.DataType = System.Type.GetType("System.String");
            col20.AllowDBNull = false;
            col20.ColumnName = "sales_edit_user";
            col20.Caption = "수정자";
            col20.DefaultValue = "";
            dt.Columns.Add(col20);

            /*DataColumn col201 = new DataColumn();
            col201.DataType = System.Type.GetType("System.String");
            col201.AllowDBNull = false;
            col201.ColumnName = "pre_ato_manager";
            col201.Caption = "전담당자";
            col201.DefaultValue = "";
            dt.Columns.Add(col201);*/


            DataColumn col30 = new DataColumn();
            col30.DataType = System.Type.GetType("System.Int32");
            col30.AllowDBNull = false;
            col30.ColumnName = "table_div";
            col30.Caption = "table_div";
            col30.DefaultValue = 1;
            dt.Columns.Add(col30);

            DataColumn col31 = new DataColumn();
            col31.DataType = System.Type.GetType("System.Int64");
            col31.AllowDBNull = false;
            col31.ColumnName = "table_index";
            col31.Caption = "table_index";
            col31.DefaultValue = 1;
            dt.Columns.Add(col31);

            DataColumn col32 = new DataColumn();
            col32.DataType = System.Type.GetType("System.String");
            col32.AllowDBNull = false;
            col32.ColumnName = "duplicate_result";
            col32.Caption = "중복결과";
            col32.DefaultValue = "";
            dt.Columns.Add(col32);

            DataColumn col33 = new DataColumn();
            col33.DataType = System.Type.GetType("System.Int16");
            col33.AllowDBNull = false;
            col33.ColumnName = "duplicate_common_count";
            col33.Caption = "duplicate_common_count";
            col33.DefaultValue = 0;
            dt.Columns.Add(col33);

            DataColumn col34 = new DataColumn();
            col34.DataType = System.Type.GetType("System.Int16");
            col34.AllowDBNull = false;
            col34.ColumnName = "duplicate_myData_count";
            col34.Caption = "duplicate_myData_count";
            col34.DefaultValue = 0;
            dt.Columns.Add(col34);

            DataColumn col35 = new DataColumn();
            col35.DataType = System.Type.GetType("System.Int16");
            col35.AllowDBNull = false;
            col35.ColumnName = "duplicate_potential1_count";
            col35.Caption = "duplicate_potential1_count";
            col35.DefaultValue = 0;
            dt.Columns.Add(col35);

            DataColumn col36 = new DataColumn();
            col36.DataType = System.Type.GetType("System.Int16");
            col36.AllowDBNull = false;
            col36.ColumnName = "duplicate_potential2_count";
            col36.Caption = "duplicate_potential2_count";
            col36.DefaultValue = 0;
            dt.Columns.Add(col36);

            DataColumn col37 = new DataColumn();
            col37.DataType = System.Type.GetType("System.Int16");
            col37.AllowDBNull = false;
            col37.ColumnName = "duplicate_trading_count";
            col37.Caption = "duplicate_trading_count";
            col37.DefaultValue = 0;
            dt.Columns.Add(col37);


            DataColumn col39 = new DataColumn();
            col39.DataType = System.Type.GetType("System.Int16");
            col39.AllowDBNull = false;
            col39.ColumnName = "duplicate_nonHandled_count";
            col39.Caption = "duplicate_nonHandled_count";
            col39.DefaultValue = 0;
            dt.Columns.Add(col39);

            DataColumn col40 = new DataColumn();
            col40.DataType = System.Type.GetType("System.Int16");
            col40.AllowDBNull = false;
            col40.ColumnName = "duplicate_notSendFax_count";
            col40.Caption = "duplicate_notSendFax_count";
            col40.DefaultValue = 0;
            dt.Columns.Add(col40);

            DataColumn col41 = new DataColumn();
            col41.DataType = System.Type.GetType("System.Int16");
            col41.AllowDBNull = false;
            col41.ColumnName = "duplicate_outBusiness_count";
            col41.Caption = "duplicate_outBusiness_count";
            col41.DefaultValue = 0;
            dt.Columns.Add(col41);

            DataColumn col42 = new DataColumn();
            col42.DataType = System.Type.GetType("System.Int32");
            col42.AllowDBNull = false;
            col42.ColumnName = "main_id";
            col42.Caption = "main_id";
            col42.DefaultValue = 0;
            dt.Columns.Add(col42);

            DataColumn col43 = new DataColumn();
            col43.DataType = System.Type.GetType("System.Int32");
            col43.AllowDBNull = false;
            col43.ColumnName = "sub_id";
            col43.Caption = "sub_id";
            col43.DefaultValue = 0;
            dt.Columns.Add(col43);

            DataColumn col44 = new DataColumn();
            col44.DataType = System.Type.GetType("System.String");
            col44.AllowDBNull = false;
            col44.ColumnName = "sales_comment";
            col44.Caption = "sales_comment";
            col44.DefaultValue = "";
            dt.Columns.Add(col44);

            DataColumn col45 = new DataColumn();
            col45.DataType = System.Type.GetType("System.Int32");
            col45.AllowDBNull = false;
            col45.ColumnName = "alarm_month";
            col45.Caption = "alarm_month";
            col45.DefaultValue = 0;
            dt.Columns.Add(col45);

            DataColumn col46 = new DataColumn();
            col46.DataType = System.Type.GetType("System.String");
            col46.AllowDBNull = false;
            col46.ColumnName = "alarm_week";
            col46.Caption = "alarm_week";
            col46.DefaultValue = "";
            dt.Columns.Add(col46);

            DataColumn col47 = new DataColumn();
            col47.DataType = System.Type.GetType("System.String");
            col47.AllowDBNull = false;
            col47.ColumnName = "alarm_complete_date";
            col47.Caption = "alarm_complete_date";
            col47.DefaultValue = "";
            dt.Columns.Add(col47);

            DataColumn col477 = new DataColumn();
            col477.DataType = System.Type.GetType("System.String");
            col477.AllowDBNull = false;
            col477.ColumnName = "alarm_date";
            col477.Caption = "alarm_date";
            col477.DefaultValue = "";
            dt.Columns.Add(col477);

            /*DataColumn col23 = new DataColumn();
            col23.DataType = System.Type.GetType("System.String");
            col23.AllowDBNull = false;
            col23.ColumnName = "div2";
            col23.Caption = "";
            col23.DefaultValue = "";
            dt.Columns.Add(col23);

            */

            DataColumn col2821 = new DataColumn();
            col2821.DataType = System.Type.GetType("System.String");
            col2821.AllowDBNull = false;
            col2821.ColumnName = "alarm_division";
            col2821.Caption = "구분";
            col2821.DefaultValue = "";
            dt.Columns.Add(col2821);

            DataColumn col2823 = new DataColumn();
            col2823.DataType = System.Type.GetType("System.Boolean");
            col2823.AllowDBNull = false;
            col2823.ColumnName = "alarm_complete";
            col2823.Caption = "";
            col2823.DefaultValue = false;
            dt.Columns.Add(col2823);

            DataColumn col2825 = new DataColumn();
            col2825.DataType = System.Type.GetType("System.String");
            col2825.AllowDBNull = false;
            col2825.ColumnName = "btn_alarm_complete";
            col2825.Caption = "";
            col2825.DefaultValue = "";
            dt.Columns.Add(col2825);

            DataColumn col2827 = new DataColumn();
            col2827.DataType = System.Type.GetType("System.String");
            col2827.AllowDBNull = false;
            col2827.ColumnName = "btn_alarm_extension";
            col2827.Caption = "";
            col2827.DefaultValue = "";
            dt.Columns.Add(col2827);

            DataColumn col2826 = new DataColumn();
            col2826.DataType = System.Type.GetType("System.Int32");
            col2826.AllowDBNull = false;
            col2826.ColumnName = "alarm_division_int";
            col2826.Caption = "";
            col2826.DefaultValue = 999999;
            dt.Columns.Add(col2826);


            return dt;
        }
        private void SetHeaderStyle(bool isInit = false)
        {
            
            dgvCompany.EndEdit();
            txtTotalRecord.Text = dgvCompany.Rows.Count.ToString("#,##0");
            txtCurrentRecord.Text = "0";

            //Header style
            dgvCompany.ColumnHeadersDefaultCellStyle.Font = new Font("나눔고딕", 11, FontStyle.Bold);
            dgvCompany.DefaultCellStyle.Font = new Font("나눔고딕", 11, FontStyle.Regular);

            if (dgvCompany.Columns.Count > 0)
            {
                //기본세팅
                dgvCompany.Columns["chk"].Width = 30;
                dgvCompany.Columns["div0"].Width = 10;
                dgvCompany.Columns["div1"].Width = 10;
                dgvCompany.Columns["address"].Width = 50;
                dgvCompany.Columns["registration_number"].Width = 50;
                dgvCompany.Columns["distribution"].Width = 60;
                dgvCompany.Columns["handling_item"].Width = 200;
                dgvCompany.Columns["ceo"].Width = 60;
                dgvCompany.Columns["category"].Width = 60;
                dgvCompany.Columns["sales_edit_user"].Width = 60;
                dgvCompany.Columns["sales_updatetime"].Width = 80;
                dgvCompany.Columns["current_sale_date"].Width = 80;
                dgvCompany.Columns["sales_contents"].Width = 200;
                dgvCompany.Columns["isPotential1"].Width = 50;
                dgvCompany.Columns["ispotential2"].Width = 50;
                dgvCompany.Columns["isNonHandled"].Width = 50;
                dgvCompany.Columns["isNotSendFax"].Width = 50;
                dgvCompany.Columns["isOutBusiness"].Width = 50;
                dgvCompany.Columns["ato_manager"].Width = 50;
                dgvCompany.Columns["company"].Width = 150;
                dgvCompany.Columns["duplicate_result"].Width = 150;
                dgvCompany.Columns["sales_contents"].Width = 100;
                dgvCompany.Columns["alarm_division"].Width = 60;
                dgvCompany.Columns["btn_alarm_complete"].Width = 60;
                dgvCompany.Columns["btn_alarm_extension"].Width = 60;
                
                dgvCompany.Columns["remark4"].Width = 60;
                dgvCompany.Columns["remark5"].Width = 60;
                dgvCompany.Columns["remark6"].Width = 60;

                ((DataGridViewImageColumn)dgvCompany.Columns["image"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
                dgvCompany.Columns["image"].DefaultCellStyle.NullValue = null;
                //visible
                dgvCompany.Columns["id"].Visible = false;
                //dgvCompany.Columns["group_name"].Visible = false;
                dgvCompany.Columns["origin"].Visible = false;
                dgvCompany.Columns["company_manager"].Visible = false;
                dgvCompany.Columns["company_manager_position"].Visible = false;
                dgvCompany.Columns["pre_ato_manager"].Visible = false;
                dgvCompany.Columns["pre_category"].Visible = false;
                //dgvCompany.Columns["email"].Visible = false;
                dgvCompany.Columns["sns1"].Visible = false;
                dgvCompany.Columns["sns2"].Visible = false;
                dgvCompany.Columns["sns3"].Visible = false;
                dgvCompany.Columns["pre_tel"].Visible = false;
                dgvCompany.Columns["pre_fax"].Visible = false;
                dgvCompany.Columns["pre_phone"].Visible = false;
                dgvCompany.Columns["pre_other_phone"].Visible = false;
                dgvCompany.Columns["web"].Visible = false;
                dgvCompany.Columns["seaover_handling_item"].Visible = false;
                dgvCompany.Columns["remark2"].Visible = false;
                dgvCompany.Columns["isManagement1"].Visible = false;
                dgvCompany.Columns["isManagement2"].Visible = false;
                dgvCompany.Columns["isManagement3"].Visible = false;
                dgvCompany.Columns["isManagement4"].Visible = false;
                dgvCompany.Columns["isHide"].Visible = false;
                dgvCompany.Columns["isTrading"].Visible = false;
                dgvCompany.Columns["createtime"].Visible = false;
                dgvCompany.Columns["btn_alarm_extension"].Visible = false;
                dgvCompany.Columns["seaover_company_code"].Visible = cbSeaoverCode.Checked;
                //dgvCompany.Columns["seaover_company_code"].Visible = false;
                /*dgvCompany.Columns["sales_contents"].Visible = false;*/
                dgvCompany.Columns["sales_remark"].Visible = false;
                dgvCompany.Columns["sales_edit_user"].Visible = false;
                dgvCompany.Columns["sales_log"].Visible = false;
                dgvCompany.Columns["duplicate_result"].Visible = false;
                dgvCompany.Columns["payment_date"].Visible = false;
                //dgvCompany.Columns["duplicate_count"].Visible = false;
                dgvCompany.Columns["isPotential1"].Visible = false;
                dgvCompany.Columns["isPotential2"].Visible = false;
                dgvCompany.Columns["isNonHandled"].Visible = false;
                dgvCompany.Columns["duplicate_common_count"].Visible = false;
                dgvCompany.Columns["duplicate_myData_count"].Visible = false;
                dgvCompany.Columns["duplicate_potential1_count"].Visible = false;
                dgvCompany.Columns["duplicate_potential2_count"].Visible = false;
                dgvCompany.Columns["duplicate_trading_count"].Visible = false;
                dgvCompany.Columns["duplicate_nonHandled_count"].Visible = false;
                dgvCompany.Columns["duplicate_notSendFax_count"].Visible = false;
                dgvCompany.Columns["duplicate_outBusiness_count"].Visible = false;
                dgvCompany.Columns["alarm_month"].Visible = false;
                dgvCompany.Columns["alarm_week"].Visible = false;
                dgvCompany.Columns["alarm_date"].Visible = false;
                dgvCompany.Columns["alarm_complete_date"].Visible = false;
                dgvCompany.Columns["alarm_division_int"].Visible = false;
                dgvCompany.Columns["alarm_complete"].Visible = false;
                dgvCompany.Columns["main_id"].Visible = false;
                dgvCompany.Columns["sub_id"].Visible = false;
                dgvCompany.Columns["sales_comment"].Visible = false;
                dgvCompany.Columns["table_div"].Visible = false;
                dgvCompany.Columns["table_index"].Visible = false;

                //Header
                for (int i = 1; i < dgvCompany.Columns.Count; i++)
                {
                    dgvCompany.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
                    dgvCompany.Columns[i].HeaderText = mainDt.Columns[i - 1].Caption;
                }

                //사용자 설정값==============================================================================
                if (isInit)
                {
                    foreach (string key in styleDic.Keys)
                    {
                        if (!string.IsNullOrEmpty(key) && key != "sort" && dgvCompany.Columns.Contains(key))
                        {
                            int width = Convert.ToInt32(styleDic[key]);
                            if (width < 0)
                            {
                                dgvCompany.Columns[key].Width = -width;
                                dgvCompany.Columns[key].Visible = false;
                            }
                            else
                            {
                                dgvCompany.Columns[key].Width = width;
                                dgvCompany.Columns[key].Visible = true;
                            }
                        }
                        else
                        {
                            string sortType = styleDic[key];
                            cbSortType.Text = sortType;
                        }
                    }
                }
                //업종순위는 내DATA에서만 보임
                if (tcMain.SelectedTab.Name == "tabCommonData" || tcMain.SelectedTab.Name == "tabRamdomData")
                {
                    dgvCompany.Columns["industry_type_rank"].Visible = true;
                    dgvCompany.Columns["industry_type_rank"].Width = 46;
                }
                else
                    dgvCompany.Columns["industry_type_rank"].Visible = false;
                //폐업유무
                dgvCompany.Columns["isOutBusiness"].Visible = !cbNotOutOfBusiness.Checked;
                //알람탭에서만 활성화==============================================================================
                if (tcMain.SelectedTab.Name == "tabAlarm")
                {
                    dgvCompany.Columns["category"].Visible = true;
                    dgvCompany.Columns["alarm_division"].Visible = true;
                    dgvCompany.Columns["btn_alarm_complete"].Visible = true;
                    //dgvCompany.Columns["btn_alarm_extension"].Visible = true;

                    lbAlarmComplete.Visible = true;
                    lbAlarmOrder.Visible = true;
                    lbAlarmEtc.Visible = true;
                    lbAlarmNew.Visible = true;
                    lbAlarmPayment.Visible = true;

                    txtAlarmComplete.Visible = true;
                    txtAlarmOrder.Visible = true;
                    txtAlarmEtc.Visible = true;
                    txtAlarmNew.Visible = true;
                    txtAlarmPayment.Visible = true;

                }
                else
                {
                    if (tcMain.SelectedTab.Name == "tabAllMyCompany")
                    {
                        dgvCompany.Columns["category"].Visible = true;
                        dgvCompany.Columns["alarm_division"].Visible = false;
                        dgvCompany.Columns["btn_alarm_complete"].Visible = false;
                    }

                    dgvCompany.Columns["category"].Visible = false;
                    dgvCompany.Columns["alarm_division"].Visible = false;
                    dgvCompany.Columns["btn_alarm_complete"].Visible = false;
                    //dgvCompany.Columns["btn_alarm_extension"].Visible = false;

                    lbAlarmComplete.Visible = false;
                    lbAlarmOrder.Visible = false;
                    lbAlarmEtc.Visible = false;
                    lbAlarmNew.Visible = false;
                    lbAlarmPayment.Visible = false;

                    txtAlarmComplete.Visible = false;
                    txtAlarmOrder.Visible = false;
                    txtAlarmEtc.Visible = false;
                    txtAlarmNew.Visible = false;
                    txtAlarmPayment.Visible = false;
                }

                btnDistribution.Visible = false;
                if (tcMain.SelectedTab.Name == "tabCommonData")
                    btnDistribution.Visible = true;

                //씨오버코드
                dgvCompany.Columns["seaover_company_code"].Visible = cbSeaoverCode.Checked;

                //아토담당 변경
                if (um.department.Contains("전산부") || um.department.Contains("관리"))
                    dgvCompany.Columns["ato_manager"].ReadOnly = false;
                else
                    dgvCompany.Columns["ato_manager"].ReadOnly = true;

                //Seaover 표시
                for (int i = 0; i < dgvCompany.Rows.Count; i++)
                {
                    if (dgvCompany.Rows[i].Cells["sales_updatetime"].Value.ToString().Equals("2999-12-31"))
                        dgvCompany.Rows[i].Cells["sales_updatetime"].Value = string.Empty;
                    if (dgvCompany.Rows[i].Cells["current_sale_date"].Value.ToString().Equals("2999-12-31"))
                        dgvCompany.Rows[i].Cells["current_sale_date"].Value = string.Empty;

                    //알람 완료여부
                    if (tcMain.SelectedTab.Name == "tabAlarm")
                    {
                        if (dgvCompany.Rows[i].Cells["alarm_division"].Value != null)
                        {
                            if (dgvCompany.Rows[i].Cells["alarm_division"].Value.ToString().Contains("발주"))
                                dgvCompany.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(238, 235, 248);
                            else if (dgvCompany.Rows[i].Cells["alarm_division"].Value.ToString().Contains("신규"))
                                dgvCompany.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(226, 250, 234);
                            else if (dgvCompany.Rows[i].Cells["alarm_division"].Value.ToString().Contains("결제"))
                                dgvCompany.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 223);
                            else if (dgvCompany.Rows[i].Cells["alarm_division"].Value.ToString().Contains("기타"))
                                dgvCompany.Rows[i].DefaultCellStyle.BackColor = Color.White;
                            //dgvCompany.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(253, 230, 231);
                        }
                    }
                    else
                    {
                        if (i % 2 == 0)
                            dgvCompany.Rows[i].DefaultCellStyle.BackColor = Color.FloralWhite;
                    }

                    //씨오버거래처
                    if (dgvCompany.Rows[i].Cells["seaover_company_code"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["seaover_company_code"].Value.ToString()))
                    {
                        dgvCompany.Rows[i].HeaderCell.Value = "S";
                        dgvCompany.Rows[i].HeaderCell.Style.ForeColor = Color.Red;
                        dgvCompany.Rows[i].HeaderCell.Style.Font = new Font("나눔고딕", 11, FontStyle.Bold);
                    }

                    //이미지====================================================================================================================
                    if (dgvCompany.Rows[i].Cells["alarm_division_int"].Value != null
                        && int.TryParse(dgvCompany.Rows[i].Cells["alarm_division_int"].Value.ToString(), out int alarm_division_int)
                        && alarm_division_int >= 10 && alarm_division_int < 20)
                        dgvCompany.Rows[i].Cells["image"].Value = Properties.Resources.not_complete_icon;
                    else
                    {
                        string company = dgvCompany.Rows[i].Cells["company"].Value.ToString();
                        if (company.Contains("(선)") || company.Contains("tjs"))
                            dgvCompany.Rows[i].Cells["image"].Value = Properties.Resources.Money_icon;
                        else if (company.Contains("(악)") || company.Contains("dkr"))
                            dgvCompany.Rows[i].Cells["image"].Value = Properties.Resources.Devil_icon;
                        else
                            dgvCompany.Rows[i].Cells["image"].Value = Properties.Resources.empty;
                    }
                }
                //틀고정
                dgvCompany.Columns["image"].Frozen = true;
                dgvCompany.Columns["chk"].Frozen = true;
                dgvCompany.Columns["company"].Frozen = true;
            }
        }
        private bool UpdateCompanyInfo(int updateType = 1)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "영업거래처 관리", "거래처 관리", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return false;
                }
            }

            //=====================
            //=====================
            // 수정 타입
            // 1 : 거래처 수정
            // 2 : 공용DB 전환
            // 3 : 무작위DB 전환
            // 4 : 잠재1 전환
            // 5 : 잠재2 전환
            // 6 : 취급X 전환
            // 7 : 팩스X 전환
            // 8 : 폐업 전환
            //=====================
            //알람 처리여부
            DateTime alarm_standard_date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            /*if (tcMain.SelectedTab.Name == "tabAlarm")
            {
                switch (cbAlarmDivision.Text)
                {
                    case "어제":
                        alarm_standard_date = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                        break;
                    case "내일":
                        alarm_standard_date = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                        break;
                }
            }*/

            //알람일 경우 처리할건지 확인
            bool isAlarmComplete = false;
            for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
            {
                bool isSelect = false;
                if (updateType == 1)
                    isSelect = Convert.ToBoolean(dgvCompany.Rows[i].Cells["chk"].Value);
                else
                    isSelect = dgvCompany.Rows[i].Selected;

                if (isSelect)
                {
                    DataGridViewRow row = dgvCompany.Rows[i];

                    bool isComplete = true;
                    DateTime complete_date;
                    if (row.Cells["alarm_complete_date"].Value == null || !DateTime.TryParse(row.Cells["alarm_complete_date"].Value.ToString(), out complete_date))
                        complete_date = new DateTime(1900, 1, 1);

                    //특정알람
                    if (row.Cells["alarm_date"].Value != null && !string.IsNullOrEmpty(row.Cells["alarm_date"].Value.ToString()))
                    {
                        string[] alarms = row.Cells["alarm_date"].Value.ToString().Split('\n');
                        foreach (string alarm in alarms)
                        {
                            if (!string.IsNullOrEmpty(alarm.Trim()))
                            {
                                string[] alarm_detail = alarm.Split('_');
                                if (DateTime.TryParse(alarm_detail[0], out DateTime alarmDt)
                                    && alarmDt.ToString("yyyy-MM-dd") == alarm_standard_date.ToString("yyyy-MM-dd")
                                    && Convert.ToDateTime(alarmDt.ToString("yyyy-MM-dd")) > Convert.ToDateTime(complete_date.ToString("yyyy-MM-dd")))
                                {
                                    isComplete = false;
                                    break;
                                }
                            }
                        }
                    }
                    //월알람
                    if (row.Cells["alarm_month"].Value != null && !string.IsNullOrEmpty(row.Cells["alarm_month"].Value.ToString()))
                    {
                        int alarm_month = Convert.ToInt16(row.Cells["alarm_month"].Value.ToString());
                        if (alarm_month > 0)
                        {
                            DateTime alarm_month_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, alarm_month);
                            if (alarm_month_date.ToString("yyyy-MM-dd") == alarm_standard_date.ToString("yyyy-MM-dd")
                                && Convert.ToDateTime(alarm_month_date.ToString("yyyy-MM-dd")) > Convert.ToDateTime(alarm_standard_date.ToString("yyyy-MM-dd")))
                                isComplete = false;
                        }
                    }
                    //주알람
                    if (row.Cells["alarm_week"].Value != null && !string.IsNullOrEmpty(row.Cells["alarm_week"].Value.ToString()))
                    {
                        string[] weeks = row.Cells["alarm_week"].Value.ToString().Split('_');
                        foreach (string week in weeks)
                        {
                            DateTime week_date = DateTime.Now.AddDays((int)DateTime.Now.DayOfWeek - Convert.ToInt16(week));
                            if (week_date.ToString("yyyy-MM-dd") == alarm_standard_date.ToString("yyyy-MM-dd")
                                && Convert.ToDateTime(week_date.ToString("yyyy-MM-dd")) > Convert.ToDateTime(complete_date.ToString("yyyy-MM-dd")))
                            {
                                isComplete = false;
                                break;
                            }
                        }
                    }
                    //완료되지 않은 알람이면
                    if (!isComplete)
                    {
                        if (messageBox.Show(this, "알람완료 처리되지 않은 거래처가 존재합니다. 알람 완료처리 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            isAlarmComplete = true;
                        break;
                    }
                }
            }
            //====================================================================================================
            bool isSale = false;   //영업전화 반영여부
            switch (updateType)
            {
                //거래처 수정
                case 1:
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        for (int j = 0; j > dgvCompany.ColumnCount; j++)
                        {
                            if (dgvCompany.Rows[i].Cells[j].Value == null)
                                dgvCompany.Rows[i].Cells[j].Value = string.Empty;
                        }

                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = Convert.ToBoolean(row.Cells["chk"].Value);
                        if (isChecked)
                        {
                            int div = Convert.ToInt32(row.Cells["table_div"].Value);
                            int idx = Convert.ToInt32(row.Cells["table_index"].Value);

                            atoDt.Rows[idx]["sales_contents"] = "거래처 수정";
                            atoDt.Rows[idx]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            atoDt.Rows[idx]["sales_edit_user"] = um.user_name.ToString();

                            atoDt.Rows[idx]["pre_ato_manager"] = atoDt.Rows[idx]["ato_manager"].ToString();
                        }
                    }
                    break;
                //공용DB 전환
                case 2:
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = row.Selected;
                        if (isChecked)
                        {
                            int div = Convert.ToInt32(row.Cells["table_div"].Value);
                            int idx = Convert.ToInt32(row.Cells["table_index"].Value);

                            row.Cells["isTrading"].Value = false;
                            row.Cells["isNonHandled"].Value = false;
                            row.Cells["isNotSendFax"].Value = false;
                            row.Cells["isOutBusiness"].Value = false;
                            row.Cells["ato_manager"].Value = string.Empty;

                            atoDt.Rows[idx]["sales_contents"] = "공용DATA 전환";
                            atoDt.Rows[idx]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            atoDt.Rows[idx]["sales_edit_user"] = um.user_name.ToString();

                            atoDt.Rows[idx]["ato_manager"] = string.Empty;
                            atoDt.Rows[idx]["pre_ato_manager"] = atoDt.Rows[idx]["ato_manager"].ToString();
                        }
                    }
                    break;
                //내DATA 전환
                case 3:
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = row.Selected;
                        if (isChecked)
                        {
                            int div = Convert.ToInt32(row.Cells["table_div"].Value);
                            int idx = Convert.ToInt32(row.Cells["table_index"].Value);

                            if (row.Cells["ato_manager"].Value == null || string.IsNullOrEmpty(row.Cells["ato_manager"].Value.ToString().Trim()))
                            {
                                dgvCompany.FirstDisplayedScrollingRowIndex = row.Index;
                                row.Selected = true;
                                messageBox.Show(this, "아토담당이 입력되지 않았습니다.");
                                this.Activate();
                                return false;
                            }

                            row.Cells["isPotential1"].Value = false;
                            row.Cells["isPotential2"].Value = false;
                            row.Cells["isTrading"].Value = false;

                            row.Cells["isNonHandled"].Value = false;
                            row.Cells["isNotSendFax"].Value = false;
                            row.Cells["isOutBusiness"].Value = false;

                            atoDt.Rows[idx]["sales_contents"] = "내DATA 전환";
                            atoDt.Rows[idx]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            atoDt.Rows[idx]["sales_edit_user"] = um.user_name.ToString();

                            atoDt.Rows[idx]["pre_ato_manager"] = atoDt.Rows[idx]["ato_manager"].ToString();
                        }
                    }
                    break;
                //잠재1 전환
                case 4:
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = row.Selected;
                        if (isChecked)
                        {
                            //유효성검사

                            if (string.IsNullOrEmpty(row.Cells["fax"].Value.ToString().Trim()) && string.IsNullOrEmpty(row.Cells["phone"].Value.ToString().Trim()) && string.IsNullOrEmpty(row.Cells["email"].Value.ToString().Trim()))
                            {
                                messageBox.Show(this, "잠재거래처는 FAX 또는 휴대폰, 이메일 값이 필수입니다.");
                                this.Activate();
                                return false;
                            }

                            int div = Convert.ToInt32(row.Cells["table_div"].Value);
                            int idx = Convert.ToInt32(row.Cells["table_index"].Value);

                            if (row.Cells["ato_manager"].Value == null || string.IsNullOrEmpty(row.Cells["ato_manager"].Value.ToString().Trim()))
                            {
                                dgvCompany.FirstDisplayedScrollingRowIndex = row.Index;
                                row.Selected = true;
                                messageBox.Show(this, "아토담당이 입력되지 않았습니다.");
                                this.Activate();
                                return false;
                            }

                            row.Cells["isPotential1"].Value = true;
                            row.Cells["isPotential2"].Value = false;
                            row.Cells["isTrading"].Value = false;

                            row.Cells["isNonHandled"].Value = false;
                            row.Cells["isNotSendFax"].Value = false;
                            row.Cells["isOutBusiness"].Value = false;

                            atoDt.Rows[idx]["sales_contents"] = "잠재1 전환";
                            atoDt.Rows[idx]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            atoDt.Rows[idx]["sales_edit_user"] = um.user_name.ToString();

                            atoDt.Rows[idx]["pre_ato_manager"] = atoDt.Rows[idx]["ato_manager"].ToString();

                            isSale = true;
                        }
                    }
                    break;
                //잠재2 전환
                case 5:
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = row.Selected;
                        if (isChecked)
                        {
                            //유효성검사
                            if (string.IsNullOrEmpty(row.Cells["fax"].Value.ToString().Trim()) 
                                && string.IsNullOrEmpty(row.Cells["phone"].Value.ToString().Trim()) 
                                && string.IsNullOrEmpty(row.Cells["email"].Value.ToString().Trim()))
                            {
                                messageBox.Show(this,"잠재거래처는 FAX 또는 휴대폰, 이메일 값이 필수입니다.");
                                this.Activate();
                                return false;
                            }

                            int div = Convert.ToInt32(row.Cells["table_div"].Value);
                            int idx = Convert.ToInt32(row.Cells["table_index"].Value);

                            if (row.Cells["ato_manager"].Value == null || string.IsNullOrEmpty(row.Cells["ato_manager"].Value.ToString().Trim()))
                            {
                                dgvCompany.FirstDisplayedScrollingRowIndex = row.Index;
                                row.Selected = true;
                                messageBox.Show(this, "아토담당이 입력되지 않았습니다.");
                                this.Activate();
                                return false;
                            }
                            row.Cells["isPotential1"].Value = false;
                            row.Cells["isPotential2"].Value = true;
                            row.Cells["isTrading"].Value = false;

                            row.Cells["isNonHandled"].Value = false;
                            row.Cells["isNotSendFax"].Value = false;
                            row.Cells["isOutBusiness"].Value = false;

                            atoDt.Rows[idx]["sales_contents"] = "잠재2 전환";
                            atoDt.Rows[idx]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            atoDt.Rows[idx]["sales_edit_user"] = um.user_name.ToString();

                            atoDt.Rows[idx]["pre_ato_manager"] = atoDt.Rows[idx]["ato_manager"].ToString();

                            isSale = true;
                        }
                    }
                    break;
                //취급X 전환
                case 6:
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = row.Selected;
                        if (isChecked)
                        {
                            int div = Convert.ToInt32(row.Cells["table_div"].Value);
                            int idx = Convert.ToInt32(row.Cells["table_index"].Value);

                            row.Cells["pre_ato_manager"].Value = row.Cells["ato_manager"].Value;
                            row.Cells["isNonHandled"].Value = true;

                            atoDt.Rows[idx]["sales_contents"] = "취급X 전환";
                            atoDt.Rows[idx]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            atoDt.Rows[idx]["sales_edit_user"] = um.user_name.ToString();

                            atoDt.Rows[idx]["pre_ato_manager"] = atoDt.Rows[idx]["ato_manager"].ToString();

                            isSale = true;
                        }
                    }
                    break;
                //팩스X 전환
                case 7:
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = row.Selected;
                        if (isChecked)
                        {
                            int div = Convert.ToInt32(row.Cells["table_div"].Value);
                            int idx = Convert.ToInt32(row.Cells["table_index"].Value);

                            row.Cells["pre_ato_manager"].Value = row.Cells["ato_manager"].Value;
                            row.Cells["isNotSendFax"].Value = true;

                            atoDt.Rows[idx]["sales_contents"] = "팩스X 전환";
                            atoDt.Rows[idx]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            atoDt.Rows[idx]["sales_edit_user"] = um.user_name.ToString();

                            atoDt.Rows[idx]["pre_ato_manager"] = atoDt.Rows[idx]["ato_manager"].ToString();

                            isSale = true;
                        }
                    }
                    break;
                //폐업 전환
                case 8:
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = row.Selected;
                        if (isChecked)
                        {
                            int div = Convert.ToInt32(row.Cells["table_div"].Value);
                            int idx = Convert.ToInt32(row.Cells["table_index"].Value);

                            row.Cells["pre_ato_manager"].Value = row.Cells["ato_manager"].Value;
                            row.Cells["isOutBusiness"].Value = true;

                            atoDt.Rows[idx]["sales_contents"] = "폐업 전환";
                            atoDt.Rows[idx]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            atoDt.Rows[idx]["sales_edit_user"] = um.user_name.ToString();

                            atoDt.Rows[idx]["pre_ato_manager"] = atoDt.Rows[idx]["ato_manager"].ToString();

                            isSale = true;
                        }
                    }
                    break;
                //거래중 전환
                case 9:
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = row.Selected;
                        if (isChecked)
                        {
                            int div = Convert.ToInt32(row.Cells["table_div"].Value);
                            int idx = Convert.ToInt32(row.Cells["table_index"].Value);

                            if (row.Cells["ato_manager"].Value == null || string.IsNullOrEmpty(row.Cells["ato_manager"].Value.ToString().Trim()))
                            {
                                dgvCompany.FirstDisplayedScrollingRowIndex = row.Index;
                                row.Selected = true;
                                messageBox.Show(this, "아토담당이 입력되지 않았습니다.");
                                this.Activate();
                                return false;
                            }
                            row.Cells["isPotential1"].Value = false;
                            row.Cells["isPotential2"].Value = false;
                            row.Cells["isTrading"].Value = true;

                            row.Cells["isNonHandled"].Value = false;
                            row.Cells["isNotSendFax"].Value = false;
                            row.Cells["isOutBusiness"].Value = false;

                            atoDt.Rows[idx]["sales_contents"] = "거래중 전환";
                            atoDt.Rows[idx]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            atoDt.Rows[idx]["sales_edit_user"] = um.user_name.ToString();

                            atoDt.Rows[idx]["pre_ato_manager"] = atoDt.Rows[idx]["ato_manager"].ToString();
                        }
                    }
                    break;
            }

            

            atoDt.AcceptChanges();
            //신규ID
            int new_id = commonRepository.GetNextId("t_company", "id");
            //거래처 수정
            dgvCompany.EndEdit();
            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();
            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                DataGridViewRow row = dgvCompany.Rows[i];
                bool isChecked;
                if (updateType == 1)
                    isChecked = Convert.ToBoolean(row.Cells["chk"].Value);
                else
                    isChecked = dgvCompany.Rows[i].Selected;
                if (isChecked)
                {
                    CompanyModel model = new CompanyModel();
                    //기존ID가 없을경우 신규ID
                    int id;
                    if (!int.TryParse(row.Cells["id"].Value.ToString(), out id))
                        id = 0;
                    if (id == 0)
                    {
                        id = new_id;
                        new_id++;

                        model.createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                        model = companyRepository.GetCompanyAsOne2("", "", id.ToString());

                    if (model == null)
                        model = new CompanyModel();

                    model.id = id;
                    model.division = "매출처";
                    model.group_name = row.Cells["group_name"].Value.ToString();
                    model.name = row.Cells["company"].Value.ToString().Trim();
                    model.registration_number = row.Cells["registration_number"].Value.ToString().Trim();
                    model.ceo = row.Cells["ceo"].Value.ToString().Trim();
                    model.tel = row.Cells["tel"].Value.ToString().Trim();
                    model.fax = row.Cells["fax"].Value.ToString().Trim();
                    model.phone = row.Cells["phone"].Value.ToString().Trim();
                    model.other_phone = row.Cells["other_phone"].Value.ToString().Trim();
                    model.distribution = row.Cells["distribution"].Value.ToString().Trim();
                    model.handling_item = row.Cells["handling_item"].Value.ToString().Trim();
                    model.address = row.Cells["address"].Value.ToString().Trim();
                    model.origin = row.Cells["origin"].Value.ToString().Trim();
                    model.ato_manager = row.Cells["ato_manager"].Value.ToString().Trim();

                    model.email = row.Cells["email"].Value.ToString().Trim();
                    model.web = row.Cells["web"].Value.ToString().Trim();
                    model.sns1 = row.Cells["sns1"].Value.ToString().Trim();
                    model.sns2 = row.Cells["sns2"].Value.ToString().Trim();
                    model.sns3 = row.Cells["sns3"].Value.ToString().Trim();
                    model.company_manager = row.Cells["company_manager"].Value.ToString().Trim();
                    model.company_manager_position = row.Cells["company_manager_position"].Value.ToString().Trim();
                    model.seaover_company_code = row.Cells["seaover_company_code"].Value.ToString().Trim();

                    bool isHide;
                    if (row.Cells["isHide"].Value == null || !bool.TryParse(row.Cells["isHide"].Value.ToString(), out isHide))
                        isHide = false;
                    model.isHide = isHide;
                    model.isDelete = false;
                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    if (!DateTime.TryParse(model.createtime, out DateTime dt) || dt.Year == 0000)
                        model.createtime = DateTime.Now.ToString("yyyy-MM-dd");

                    model.createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    model.remark = row.Cells["remark"].Value.ToString().Trim();
                    model.remark2 = row.Cells["remark2"].Value.ToString().Trim();
                    model.remark4 = row.Cells["remark4"].Value.ToString().Trim();
                    model.remark5 = row.Cells["remark5"].Value.ToString().Trim();
                    model.remark6 = row.Cells["remark6"].Value.ToString().Trim();
                    model.isPotential1 = Convert.ToBoolean(row.Cells["isPotential1"].Value);
                    model.isPotential2 = Convert.ToBoolean(row.Cells["isPotential2"].Value);
                    model.isTrading = Convert.ToBoolean(row.Cells["isTrading"].Value);
                    model.isNonHandled = Convert.ToBoolean(row.Cells["isNonHandled"].Value);
                    model.isNotSendFax = Convert.ToBoolean(row.Cells["isNotSendFax"].Value);
                    model.isOutBusiness = Convert.ToBoolean(row.Cells["isOutBusiness"].Value);
                    model.industry_type = row.Cells["industry_type"].Value.ToString();
                    model.industry_type2 = row.Cells["industry_type2"].Value.ToString();
                    model.edit_user = um.user_name;

                    if (isAlarmComplete)
                        model.alarm_complete_date = DateTime.Now.ToString("yyyy-MM-dd");
                    else
                        model.alarm_complete_date = row.Cells["alarm_complete_date"].Value.ToString();

                    //테이블 데이터 최신화
                    int div = Convert.ToInt32(row.Cells["table_div"].Value);
                    int idx = Convert.ToInt32(row.Cells["table_index"].Value);

                    atoDt.Rows[idx]["id"] = id.ToString();
                    atoDt.Rows[idx]["company"] = row.Cells["company"].Value.ToString().Trim();
                    atoDt.Rows[idx]["seaover_company_code"] = row.Cells["seaover_company_code"].Value.ToString().Trim();
                    atoDt.Rows[idx]["registration_number"] = row.Cells["registration_number"].Value.ToString().Trim();
                    atoDt.Rows[idx]["address"] = row.Cells["address"].Value.ToString().Trim();
                    atoDt.Rows[idx]["ceo"] = row.Cells["ceo"].Value.ToString().Trim();
                    atoDt.Rows[idx]["tel"] = row.Cells["tel"].Value.ToString().Trim();
                    atoDt.Rows[idx]["fax"] = row.Cells["fax"].Value.ToString().Trim();
                    atoDt.Rows[idx]["phone"] = row.Cells["phone"].Value.ToString().Trim();
                    atoDt.Rows[idx]["other_phone"] = row.Cells["other_phone"].Value.ToString().Trim();
                    atoDt.Rows[idx]["distribution"] = row.Cells["distribution"].Value.ToString().Trim();
                    atoDt.Rows[idx]["handling_item"] = row.Cells["handling_item"].Value.ToString().Trim();
                    atoDt.Rows[idx]["ato_manager"] = row.Cells["ato_manager"].Value.ToString().Trim();

                    atoDt.Rows[idx]["isTrading"] = Convert.ToBoolean(row.Cells["isTrading"].Value);
                    atoDt.Rows[idx]["isPotential1"] = Convert.ToBoolean(row.Cells["isPotential1"].Value);
                    atoDt.Rows[idx]["isPotential2"] = Convert.ToBoolean(row.Cells["isPotential2"].Value);
                    atoDt.Rows[idx]["isNonHandled"] = Convert.ToBoolean(row.Cells["isNonHandled"].Value);
                    atoDt.Rows[idx]["isNotSendFax"] = Convert.ToBoolean(row.Cells["isNotSendFax"].Value);
                    atoDt.Rows[idx]["isOutBusiness"] = Convert.ToBoolean(row.Cells["isOutBusiness"].Value);
                    atoDt.Rows[idx]["remark"] = row.Cells["remark"].Value.ToString().Trim();
                    atoDt.Rows[idx]["remark4"] = row.Cells["remark4"].Value.ToString().Trim();
                    atoDt.Rows[idx]["remark5"] = row.Cells["remark5"].Value.ToString().Trim();
                    atoDt.Rows[idx]["remark6"] = row.Cells["remark6"].Value.ToString().Trim();

                    row.Cells["sales_updatetime"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    atoDt.Rows[idx]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    atoDt.Rows[idx]["alarm_complete_date"] = model.alarm_complete_date;
                    atoDt.Rows[idx]["industry_type"] = row.Cells["industry_type"].Value.ToString().Trim();
                    atoDt.Rows[idx]["industry_type2"] = row.Cells["industry_type2"].Value.ToString().Trim();

                    switch (updateType)
                    {
                        case 2:
                            model.isDelete = false;
                            atoDt.Rows[idx]["isDelete"] = "FALSE";
                            break;
                        case 3:
                            model.isDelete = false;
                            atoDt.Rows[idx]["isDelete"] = "FALSE";
                            break;
                        case 4:
                            model.isDelete = false;
                            atoDt.Rows[idx]["isDelete"] = "FALSE";
                            break;
                        case 5:
                            model.isDelete = false;
                            atoDt.Rows[idx]["isDelete"] = "FALSE";
                            break;
                        case 6:
                            model.isDelete = false;
                            atoDt.Rows[idx]["isDelete"] = "FALSE";
                            break;
                        case 9:
                            model.isDelete = false;
                            atoDt.Rows[idx]["isDelete"] = "FALSE";
                            break;
                    }


                    //거래처 삭제
                    sql = companyRepository.RealDeleteCompany(id);
                    sqlList.Add(sql);

                    //거래처 정보 재등록
                    sql = companyRepository.InsertCompany(model);
                    sqlList.Add(sql);

                    //거래처 영업내용============================================================================
                    string contents = atoDt.Rows[idx]["sales_contents"].ToString();
                    string log = row.Cells["pre_ato_manager"].Value.ToString() + " | " + tcMain.SelectedTab.Text.Replace("(F1)", "").Replace("(F2)", "")
                        .Replace("(F3)", "").Replace("(F4)", "").Replace("(F5)", "").Replace("(F6)", "").Replace("(F7)", "").Replace("(F8)", "").Replace("(F9)", "").Trim();
                    CompanySalesModel sModel = new CompanySalesModel();
                    sModel.company_id = id;
                    sModel.sub_id = commonRepository.GetNextId("t_company_sales", "sub_id", "company_id", id.ToString());
                    sModel.is_sales = true;
                    sModel.contents = contents;
                    sModel.log = log;
                    sModel.remark = row.Cells["sales_remark"].Value.ToString();
                    sModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    sModel.edit_user = um.user_name;

                    sModel.from_ato_manager = row.Cells["pre_ato_manager"].Value.ToString();
                    sModel.to_ato_manager = row.Cells["ato_manager"].Value.ToString();
                    sModel.from_category = row.Cells["pre_category"].Value.ToString();

                    switch (updateType)
                    {
                        //거래처 수정
                        case 1:
                            sModel.to_category = row.Cells["category"].Value.ToString();
                            break;
                        //공용DB 전환
                        case 2:
                            sModel.to_category = "공용DATA";
                            break;
                        //내DATA 전환
                        case 3:
                            sModel.to_category = "내DATA";
                            break;
                        //잠재1 전환
                        case 4:
                            sModel.to_category = "잠재1";
                            break;
                        //잠재2 전환
                        case 5:
                            sModel.to_category = "잠재2";
                            break;
                        //취급X 전환
                        case 6:
                            sModel.to_category = "취급X";
                            break;
                        //팩스X 전환
                        case 7:
                            sModel.to_category = row.Cells["category"].Value.ToString();
                            break;
                        //폐업 전환
                        case 8:
                            sModel.to_category = "취급X";
                            break;
                        //거래중 전환
                        case 9:
                            sModel.to_category = "거래중";
                            break;
                    }
                    sql = salesPartnerRepository.InsertPartnerSales(sModel);
                    sqlList.Add(sql);
                    //===========================================================================================
                    atoDt.Rows[idx]["category"] = sModel.to_category;
                }
            }
            //Execute
            if (sqlList.Count > 0)
            {
                if (commonRepository.UpdateTran(sqlList) == -1)
                {
                    messageBox.Show(this,"수정중 에러가 발생하였습니다.");
                    this.Activate();
                    return false;
                }
                else
                    return true;                   
            }
            return true;
        }
        public DataTable GetAtoDt()
        {
            atoDt.AcceptChanges();
            return atoDt;
        }
        public void SetAtoDt(DataTable dt)
        {
            atoDt = dt;
        }
        public DataTable GetSeaoverDt()
        {
            return seaoverDt;
        }

        private bool DeleteCompanyInfo(bool isDelete = true)
        {
            dgvCompany.EndEdit();
            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();
            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                DataGridViewRow row = dgvCompany.Rows[i];
                bool isChecked = row.Selected;
                if (isChecked)
                {
                    int id;
                    if (!int.TryParse(row.Cells["id"].Value.ToString(), out id))
                        id = 0;
                    //SEAOVER 거래처일 경우
                    if (id == 0 && !AddCompany(row.Cells["company"].Value.ToString(), out id))
                    {
                        messageBox.Show(this, "등록되지 않은 거래처입니다.( 혹은 SEAOVER 거래처입니다.)");
                        this.Activate();
                        return false;
                    }
                    //거래처 삭제
                    sql = companyRepository.DeleteCompany(id, isDelete);
                    sqlList.Add(sql);

                    //영업내역
                    CompanySalesModel sModel = new CompanySalesModel();
                    sModel.company_id = id;
                    sModel.sub_id = commonRepository.GetNextId("t_company_sales", "sub_id", "company_id", id.ToString());
                    sModel.is_sales = false;
                    if (!isDelete)
                        sModel.contents = "거래처 복구";
                    else
                        sModel.contents = "거래처 삭제";
                    sModel.log = "";
                    sModel.remark = row.Cells["sales_remark"].Value.ToString();
                    sModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    sModel.edit_user = um.user_name;

                    sql = salesPartnerRepository.InsertPartnerSales(sModel);
                    sqlList.Add(sql);

                }
            }
            //Execute
            if (sqlList.Count > 0)
            {
                if (commonRepository.UpdateTran(sqlList) == -1)
                {
                    messageBox.Show(this, "수정중 에러가 발생하였습니다.");
                    this.Activate();
                    return false;
                }
                else
                {

                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = row.Selected;
                        if (isChecked)
                        {
                            int div = Convert.ToInt32(row.Cells["table_div"].Value);
                            int idx = Convert.ToInt32(row.Cells["table_index"].Value);

                            if (div == 1)
                            {
                                atoDt.Rows[idx]["isDelete"] = isDelete.ToString();

                                if (!isDelete)
                                    atoDt.Rows[idx]["sales_contents"] = "거래처 복구";
                                else
                                    atoDt.Rows[idx]["sales_contents"] = "거래처 삭제";
                                atoDt.Rows[idx]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                atoDt.Rows[idx]["sales_edit_user"] = um.user_name.ToString();
                            }
                            else
                            {
                                seaoverDt.Rows[idx]["isDelete"] = isDelete.ToString();

                                if (!isDelete)
                                    seaoverDt.Rows[idx]["sales_contents"] = "거래처 복구";
                                else
                                    seaoverDt.Rows[idx]["sales_contents"] = "거래처 삭제";
                                seaoverDt.Rows[idx]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                seaoverDt.Rows[idx]["sales_edit_user"] = um.user_name.ToString();
                            }
                        }
                    }

                    atoDt.AcceptChanges();
                    seaoverDt.AcceptChanges();
                    return true;
                }
            }
            return true;
        }
        private bool RealDeleteCompanyInfo(bool isDelete = true)
        {
            dgvCompany.EndEdit();
            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();
            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                DataGridViewRow row = dgvCompany.Rows[i];
                bool isChecked = row.Selected;
                if (isChecked)
                {
                    int id;
                    if (!int.TryParse(row.Cells["id"].Value.ToString(), out id))
                        id = 0;
                    //SEAOVER 거래처일 경우
                    if (id == 0 && !AddCompany(row.Cells["company"].Value.ToString(), out id))
                    {
                        messageBox.Show(this, "등록되지 않은 거래처입니다.( 혹은 SEAOVER 거래처입니다.)");
                        this.Activate();
                        return false;
                    }
                    //거래처 삭제
                    sql = companyRepository.RealDeleteCompany(id);
                    sqlList.Add(sql);
                    //거래처 수정내역 삭제
                    sql = salesPartnerRepository.DeletePartnerSales(id);
                    sqlList.Add(sql);
                    //거래처 알람삭제
                    sql = companyAlarmRepository.DeleteAlarm(id.ToString());
                    sqlList.Add(sql);
                }
            }
            //Execute
            if (sqlList.Count > 0)
            {
                if (commonRepository.UpdateTran(sqlList) == -1)
                {
                    messageBox.Show(this, "수정중 에러가 발생하였습니다.");
                    this.Activate();
                    return false;
                }
                else
                {
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = row.Selected;
                        if (isChecked)
                        {
                            int idx = Convert.ToInt32(row.Cells["table_index"].Value);
                            atoDt.Rows.Remove(atoDt.Rows[idx]);
                        }
                    }
                    atoDt.AcceptChanges();
                    seaoverDt.AcceptChanges();
                    return true;
                }
            }
            return true;
        }
        private bool AddCompany(string company, out int company_id)
        {
            /*company_id = 0;
            //Seaover 거래처 내역
            DataTable companyDt = companyRepository.GetProductPriceInfo(company);
            if (companyDt.Rows.Count == 0)
            {
                MessageBox.Show(this,"거래처 정보를 찾을 수 없습니다.");
                return false;
            }
            //Ato 전산에 등록된 거래처 내역
            double ato_capital_rate = 0;
            string ato_capital_rate_updatetime = "";
            CompanyModel company_model = companyRepository.GetCompanyAsOne2(company, companyDt.Rows[0]["거래처코드"].ToString());
            if (company_model == null)
            {
                company_model = new CompanyModel();
                company_model.id = commonRepository.GetNextId("t_company", "id");
                company_model.division = "매출처";
                company_model.name = companyDt.Rows[0]["거래처명"].ToString();
                company_model.seaover_company_code = companyDt.Rows[0]["거래처코드"].ToString();
                company_model.origin = "국내";
                company_model.fax = companyDt.Rows[0]["팩스번호"].ToString();
                company_model.tel = companyDt.Rows[0]["전화번호"].ToString();
                company_model.kakao = companyDt.Rows[0]["휴대폰"].ToString();
                company_model.ceo = companyDt.Rows[0]["대표자명"].ToString();
                company_model.remark = companyDt.Rows[0]["참고사항"].ToString();
                company_model.createtime = DateTime.Now.ToString("yyyy-MM-dd");
                company_model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                company_model.edit_user = "";
                company_model.isManagement = false;
                company_model.isManagement2 = false;
                company_model.isManagement3 = false;
                company_model.isManagement4 = false;
                company_model.isHide = false;
                company_model.isPotential1 = false;
                company_model.isPotential2 = false;
                company_model.ato_capital_rate = ato_capital_rate;
                company_model.ato_capital_rate_updatetime = ato_capital_rate_updatetime;

                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql = companyRepository.InsertCompany(company_model);
                sqlList.Add(sql);

                //Execute
                int results = commonRepository.UpdateTran(sqlList);
                if (results == -1)
                {
                    MessageBox.Show(this,"등록중 에러가 발생하였습니다.");
                    return false;
                }
            }
            company_id = company_model.id;
            return true;*/
            company_id = 1;
            return false;
        }
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
        private void StyleSettingTxt()
        {
            //정렬
            if (styleDic.ContainsKey("sort"))
                styleDic["sort"] = cbSortType.Text;
            else
                styleDic.Add("sort", cbSortType.Text);
            //컬럼 스타일
            for (int i = 0; i < dgvCompany.ColumnCount; i++)
            {
                if (styleDic.ContainsKey(dgvCompany.Columns[i].Name))
                {
                    if (dgvCompany.Columns[i].Visible)
                        styleDic[dgvCompany.Columns[i].Name] = dgvCompany.Columns[i].Width.ToString();
                    else
                        styleDic[dgvCompany.Columns[i].Name] = (-dgvCompany.Columns[i].Width).ToString();
                }
                else
                {
                    if (dgvCompany.Columns[i].Visible)
                        styleDic.Add(dgvCompany.Columns[i].Name, dgvCompany.Columns[i].Width.ToString());
                    else
                        styleDic.Add(dgvCompany.Columns[i].Name, (-dgvCompany.Columns[i].Width).ToString());
                }
            }
        }
        private string WhrAtoManager(string ato_manager)
        {
            string whr = "";
            if (!string.IsNullOrEmpty(ato_manager))
            {
                string[] managers = ato_manager.Trim().Split(' ');
                if (managers.Length > 0)
                {
                    for (int i = 0; i < managers.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(managers[i].Trim()))
                        {
                            if (!string.IsNullOrEmpty(whr))
                                whr += $" OR ato_manager = '{managers[i].Trim()}'";
                            else
                                whr = $"ato_manager = '{managers[i].Trim()}'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(whr))
                whr = " AND (" + whr + ")";

            return whr;
        }

        public void GetData(bool isMsgShow = true, bool auto_update = true)
        {
            if (atoDt == null)
                return;
            this.dgvCompany.isPush = false;
            //이전 Datagridview 스타일 저장
            if (dgvCompany.Columns.Count > 1)
                StyleSettingTxt();

            //매출일자 
            string saleTelDate;
            switch (cbCurrentSaleTelDate.Text)
            {
                case "15일 이상 지난":
                    saleTelDate = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
                    break;
                case "한달 이상 지난":
                    saleTelDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    break;
                case "45일 이상 지난":
                    saleTelDate = DateTime.Now.AddDays(-45).ToString("yyyy-MM-dd");
                    break;
                case "두달 이상 지난":
                    saleTelDate = DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd");
                    break;
                case "세달 이상 지난":
                    saleTelDate = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
                    break;
                case "여섯달 이상 지난":
                    saleTelDate = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
                    break;
                default:
                    saleTelDate = "";
                    break;
            }
            //영업일자 
            string saleUpdateDate;
            switch (cbSaleUpdatetiime.Text)
            {
                case "15일 이상 지난":
                    saleUpdateDate = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
                    break;
                case "한달 이상 지난":
                    saleUpdateDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    break;
                case "45일 이상 지난":
                    saleUpdateDate = DateTime.Now.AddDays(-45).ToString("yyyy-MM-dd");
                    break;
                case "두달 이상 지난":
                    saleUpdateDate = DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd");
                    break;
                case "세달 이상 지난":
                    saleUpdateDate = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
                    break;
                case "여섯달 이상 지난":
                    saleUpdateDate = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
                    break;
                default:
                    saleUpdateDate = "";
                    break;
            }
            //출력 데이터수
            double limitCnt = -1;
            if (!double.TryParse(cbDataCount.Text, out limitCnt))
                limitCnt = -1;
            //초기화
            mainDt = SetTable();
            subDt = SetTable();

            dgvCompany.DataSource = null;
            atoDt.AcceptChanges();

            //알람기준            
            if (tcMain.SelectedTab.Name == "tabAlarm")
            {
                //lbAlarm.Visible = true;
                //cbAlarmDivision.Visible = true;

                lbAlarm.Visible = true;
                rbAlarmAll.Visible = true;
                rbAlarmComplete.Visible = true;
                rbAlarmIncomplete.Visible = true;

            }
            else
            {
                //lbAlarm.Visible = false;
                //cbAlarmDivision.Visible = false;

                lbAlarm.Visible = true;
                rbAlarmAll.Visible = false;
                rbAlarmComplete.Visible = false;
                rbAlarmIncomplete.Visible = false;
            }
            //알람기준일
            DateTime alarm_standard_date = DateTime.Now;

            DateTime alarm_standard_sttdate = DateTime.Now.AddDays(-1);
            DateTime alarm_standard_enddate = DateTime.Now;

            DataTable tempAtoDt = null;
            string whr;
            switch (tcMain.SelectedTab.Name.ToString())
            {
                case "tabCommonData":
                    {
                        whr = "isDelete = FALSE AND ato_manager = ''";
                        if (cbLitigation.Checked)
                            whr += $" AND company NOT LIKE '%소송%'  AND company NOT LIKE '%(s)%'  AND company NOT LIKE '(S)%'";

                        if (!string.IsNullOrEmpty(txtCompany.Text.Trim()))
                            whr += whereSql("company", txtCompany.Text.Trim(), cbExactly.Checked);
                        if (!string.IsNullOrEmpty(txtDistribution.Text.Trim()))
                            whr += whereSql("distribution", txtDistribution.Text.Trim(), false);
                        if (!string.IsNullOrEmpty(txtRemark4.Text.Trim()))
                            whr += whereSql("remark4", txtRemark4.Text.Trim(), false);
                        if (!string.IsNullOrEmpty(txtGroupName.Text.Trim()))
                            whr += whereSql("group_name", txtGroupName.Text.Trim(), false);

                        DataRow[] tempDr = atoDt.Select(whr);
                        if (tempDr.Length > 0)
                            tempAtoDt = tempDr.CopyToDataTable();
                        else
                            tempAtoDt = null;
                    }
                    break;
                case "tabAllMyCompany":
                    {
                        whr = $"isDelete = FALSE AND isNonHandled <> 'TRUE' AND isOutBusiness <> 'TRUE' {WhrAtoManager(txtManager.Text)}";
                        if (cbLitigation.Checked)
                            whr += $" AND company NOT LIKE '%소송%'  AND company NOT LIKE '%(s)%'  AND company NOT LIKE '(S)%'";

                        if (!string.IsNullOrEmpty(txtCompany.Text.Trim()))
                            whr += whereSql("company", txtCompany.Text.Trim(), cbExactly.Checked);
                        if (!string.IsNullOrEmpty(txtDistribution.Text.Trim()))
                            whr += whereSql("distribution", txtDistribution.Text.Trim(), false);
                        if (!string.IsNullOrEmpty(txtRemark4.Text.Trim()))
                            whr += whereSql("remark4", txtRemark4.Text.Trim(), false);
                        if (!string.IsNullOrEmpty(txtGroupName.Text.Trim()))
                            whr += whereSql("group_name", txtGroupName.Text.Trim(), false);

                        DataRow[] tempDr = atoDt.Select(whr);
                        if (tempDr.Length > 0)
                            tempAtoDt = tempDr.CopyToDataTable();
                        else
                            tempAtoDt = null;
                    }
                    break;
                case "tabRamdomData":
                    {
                        whr = $"isDelete = FALSE AND isPotential1 = FALSE AND isPotential2 = FALSE AND ato_manager <> '' AND isNonHandled <> 'TRUE' AND isOutBusiness <> 'TRUE' {WhrAtoManager(txtManager.Text)}";
                        if (cbLitigation.Checked)
                            whr += $" AND company NOT LIKE '%소송%'  AND company NOT LIKE '%(s)%'  AND company NOT LIKE '(S)%'";

                        if (!string.IsNullOrEmpty(txtCompany.Text.Trim()))
                            whr += whereSql("company", txtCompany.Text.Trim(), cbExactly.Checked);
                        if (!string.IsNullOrEmpty(txtDistribution.Text.Trim()))
                            whr += whereSql("distribution", txtDistribution.Text.Trim(), false);
                        if (!string.IsNullOrEmpty(txtRemark4.Text.Trim()))
                            whr += whereSql("remark4", txtRemark4.Text.Trim(), false);
                        if (!string.IsNullOrEmpty(txtGroupName.Text.Trim()))
                            whr += whereSql("group_name", txtGroupName.Text.Trim(), false);

                        DataRow[] tempDr = atoDt.Select(whr);
                        if (tempDr.Length > 0)
                            tempAtoDt = tempDr.CopyToDataTable();
                        else
                            tempAtoDt = null;
                    }
                    break;
                case "tabCompany":
                    {
                        whr = $"isDelete = FALSE AND isPotential1 = TRUE AND ato_manager <> '' AND isNonHandled <> 'TRUE' AND isOutBusiness <> 'TRUE' {WhrAtoManager(txtManager.Text)}";
                        if (cbLitigation.Checked)
                            whr += $" AND company NOT LIKE '%소송%'  AND company NOT LIKE '%(s)%'  AND company NOT LIKE '(S)%'";

                        if (!string.IsNullOrEmpty(txtCompany.Text.Trim()))
                            whr += whereSql("company", txtCompany.Text.Trim(), cbExactly.Checked);
                        if (!string.IsNullOrEmpty(txtDistribution.Text.Trim()))
                            whr += whereSql("distribution", txtDistribution.Text.Trim(), false);
                        if (!string.IsNullOrEmpty(txtRemark4.Text.Trim()))
                            whr += whereSql("remark4", txtRemark4.Text.Trim(), false);
                        if (!string.IsNullOrEmpty(txtGroupName.Text.Trim()))
                            whr += whereSql("group_name", txtGroupName.Text.Trim(), false);

                        DataRow[] tempDr = atoDt.Select(whr);
                        if (tempDr.Length > 0)
                            tempAtoDt = tempDr.CopyToDataTable();
                        else
                            tempAtoDt = null;
                    }
                    break;
                case "tabCompany2":
                    {
                        whr = $"isDelete = FALSE AND isPotential2 = TRUE AND ato_manager <> '' AND isNonHandled <> 'TRUE' AND isOutBusiness <> 'TRUE' {WhrAtoManager(txtManager.Text)}";
                        if (cbLitigation.Checked)
                            whr += $" AND company NOT LIKE '%소송%'  AND company NOT LIKE '%(s)%'  AND company NOT LIKE '(S)%'";

                        if (!string.IsNullOrEmpty(txtCompany.Text.Trim()))
                            whr += whereSql("company", txtCompany.Text.Trim(), cbExactly.Checked);
                        if (!string.IsNullOrEmpty(txtDistribution.Text.Trim()))
                            whr += whereSql("distribution", txtDistribution.Text.Trim(), false);
                        if (!string.IsNullOrEmpty(txtRemark4.Text.Trim()))
                            whr += whereSql("remark4", txtRemark4.Text.Trim(), false);
                        if (!string.IsNullOrEmpty(txtGroupName.Text.Trim()))
                            whr += whereSql("group_name", txtGroupName.Text.Trim(), false);


                        DataRow[] tempDr = atoDt.Select(whr);
                        if (tempDr.Length > 0)
                            tempAtoDt = tempDr.CopyToDataTable();
                        else
                            tempAtoDt = null;
                    }
                    break;
                case "tabNotTreatment":
                    {
                        whr = "isDelete = FALSE AND (isNonHandled = 'TRUE' OR isOutBusiness = 'TRUE')";
                        if (cbLitigation.Checked)
                            whr += $" AND company NOT LIKE '%소송%'  AND company NOT LIKE '%(s)%'  AND company NOT LIKE '(S)%'";

                        if (!string.IsNullOrEmpty(txtCompany.Text.Trim()))
                            whr += whereSql("company", txtCompany.Text.Trim(), cbExactly.Checked);
                        if (!string.IsNullOrEmpty(txtDistribution.Text.Trim()))
                            whr += whereSql("distribution", txtDistribution.Text.Trim(), false);
                        if (!string.IsNullOrEmpty(txtRemark4.Text.Trim()))
                            whr += whereSql("remark4", txtRemark4.Text.Trim(), false);
                        if (!string.IsNullOrEmpty(txtGroupName.Text.Trim()))
                            whr += whereSql("group_name", txtGroupName.Text.Trim(), false);


                        DataRow[] tempDr = atoDt.Select(whr);
                        if (tempDr.Length > 0)
                            tempAtoDt = tempDr.CopyToDataTable();
                        else
                            tempAtoDt = null;
                    }
                    break;
                case "tabSeaover":
                    {
                        whr = $"isDelete = FALSE AND isTrading = TRUE AND isNonHandled <> 'TRUE' AND isOutBusiness <> 'TRUE' {WhrAtoManager(txtManager.Text)}";
                        if (cbLitigation.Checked)
                            whr += $" AND company NOT LIKE '%소송%'  AND company NOT LIKE '%(s)%'  AND company NOT LIKE '(S)%'";

                        if (!string.IsNullOrEmpty(txtCompany.Text.Trim()))
                            whr += whereSql("company", txtCompany.Text.Trim(), cbExactly.Checked);
                        if (!string.IsNullOrEmpty(txtDistribution.Text.Trim()))
                            whr += whereSql("distribution", txtDistribution.Text.Trim(), false);
                        if (!string.IsNullOrEmpty(txtRemark4.Text.Trim()))
                            whr += whereSql("remark4", txtRemark4.Text.Trim(), false);
                        if (!string.IsNullOrEmpty(txtGroupName.Text.Trim()))
                            whr += whereSql("group_name", txtGroupName.Text.Trim(), false);


                        DataRow[] tempDr = atoDt.Select(whr);
                        if (tempDr.Length > 0)
                            tempAtoDt = tempDr.CopyToDataTable();
                        else
                            tempAtoDt = null;
                    }
                    break;
                case "tabAlarm":
                    {
                        whr = "alarm_date <> '' AND isNonHandled <> 'TRUE' AND isOutBusiness <> 'TRUE'";
                        if (cbLitigation.Checked)
                            whr += $" AND company NOT LIKE '%소송%'  AND company NOT LIKE '%(s)%'  AND company NOT LIKE '(S)%'";

                        if (!string.IsNullOrEmpty(txtCompany.Text.Trim()))
                            whr += whereSql("company", txtCompany.Text.Trim(), cbExactly.Checked);
                        if (!string.IsNullOrEmpty(txtDistribution.Text.Trim()))
                            whr += whereSql("distribution", txtDistribution.Text.Trim(), false);
                        if (!string.IsNullOrEmpty(txtRemark4.Text.Trim()))
                            whr += whereSql("remark4", txtRemark4.Text.Trim(), false);
                        if (!string.IsNullOrEmpty(txtGroupName.Text.Trim()))
                            whr += whereSql("group_name", txtGroupName.Text.Trim(), false);


                        DataRow[] tempDr = atoDt.Select(whr);
                        if (tempDr.Length > 0)
                            tempAtoDt = tempDr.CopyToDataTable();
                        else
                            tempAtoDt = null;
                    }
                    break;
                default:
                    break;
            }
            this.dgvCompany.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            this.dgvCompany.isPush = false;
            //저장된 매출처먼저 출력
            if (tempAtoDt != null && tempAtoDt.Rows.Count > 0)
            {
                int dataCnt = 0;
                string tmpRowindex = "";
                try
                {
                    foreach (DataRow tempAtoDr in tempAtoDt.Rows)
                    {
                        tmpRowindex = tempAtoDr["table_index"].ToString();
                        /*if (tempAtoDr["company"].ToString().Contains("피딜"))
                            tmpRowindex = tempAtoDr["table_index"].ToString();*/

                        //대표 거래처ID
                        int main_id;
                        if (!int.TryParse(tempAtoDr["main_id"].ToString(), out main_id))
                            main_id = 0;
                        //서브 거래처ID
                        int sub_id;
                        if (!int.TryParse(tempAtoDr["sub_id"].ToString(), out sub_id))
                            sub_id = 0;

                        //삭제거래처
                        bool isDelete;
                        if (!bool.TryParse(tempAtoDr["isDelete"].ToString(), out isDelete))
                            isDelete = false;

                        //삭제거래처
                        bool isHide;
                        if (!bool.TryParse(tempAtoDr["isHide"].ToString(), out isHide))
                            isHide = false;

                        //거래중
                        bool isTrading;
                        if (!bool.TryParse(tempAtoDr["isTrading"].ToString(), out isTrading))
                            isTrading = false;

                        //잠재1, 잠재2
                        bool isPotential1, isPotential2;
                        if (!bool.TryParse(tempAtoDr["isPotential1"].ToString(), out isPotential1))
                            isPotential1 = false;
                        if (!bool.TryParse(tempAtoDr["isPotential2"].ToString(), out isPotential2))
                            isPotential2 = false;

                        //영업금지 거래처
                        bool isNonHandled, isNotSendFax, isOutBusiness;
                        if (!bool.TryParse(tempAtoDr["isNonHandled"].ToString(), out isNonHandled))
                            isNonHandled = false;
                        if (!bool.TryParse(tempAtoDr["isNotSendFax"].ToString(), out isNotSendFax))
                            isNotSendFax = false;
                        if (!bool.TryParse(tempAtoDr["isOutBusiness"].ToString(), out isOutBusiness))
                            isOutBusiness = false;

                        int alarm_month;
                        if (!int.TryParse(tempAtoDr["alarm_month"].ToString(), out alarm_month))
                            alarm_month = 0;
                        string alarm_week = tempAtoDr["alarm_week"].ToString();
                        string alarm_date = tempAtoDr["alarm_date"].ToString();
                        string alarm_complete_date = "";
                        if (DateTime.TryParse(tempAtoDr["alarm_complete_date"].ToString(), out DateTime alarm_complete_dt))
                            alarm_complete_date = alarm_complete_dt.ToString("yyyy-MM-dd");

                        string alarm_date_txt = "";
                        string alarm_division = "";
                        int alarm_division_int = 0;
                        bool alarm_complete = false;
                        bool isOutput = false;
                        //서브 거래처는 무조건 추가를 하고 숨김처리함
                        if (main_id > 0 && sub_id > 0)
                            isOutput = true;
                        //대표 거래처거나 설정하지 않은 거래처는 검색조건을 거침
                        else
                        {
                            /*if (tmpRowindex == "100874")
                                tmpRowindex = tmpRowindex;*/

                            //탭별 기본 조건==========================================================================================================
                            switch (tcMain.SelectedTab.Name)
                            {
                                //알람
                                case "tabAlarm":
                                    //거래중
                                    isOutput = true;
                                    //삭제거래처
                                    if (isOutput && isDelete)
                                        isOutput = false;
                                    //숨김거래처
                                    if (isOutput && isHide)
                                        isOutput = false;
                                    //영업금지거래처
                                    if (isOutput && (isNonHandled || isOutBusiness))
                                        isOutput = false;
                                    //영업담당자
                                    if (isOutput && !string.IsNullOrEmpty(txtManager.Text.Trim()))
                                    {
                                        isOutput = false;
                                        if (!string.IsNullOrEmpty(tempAtoDr["ato_manager"].ToString().Trim()))
                                        {

                                            string[] managers = txtManager.Text.Split(' ');
                                            for (int j = 0; j < managers.Length; j++)
                                            {
                                                if (!string.IsNullOrEmpty(managers[j]) && tempAtoDr["ato_manager"].ToString().Contains(managers[j]))
                                                {
                                                    isOutput = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    //알람
                                    if (isOutput)
                                    {
                                        /*if (tempAtoDr["company"].ToString() == "(금강상사)잠수기중매인40/김현")
                                            isOutput = isOutput;*/

                                        isOutput = false;
                                        if (!string.IsNullOrEmpty(alarm_date))
                                        {
                                            string[] alarms = alarm_date.Split(',');
                                            foreach (string alarm in alarms)
                                            {
                                                if (!string.IsNullOrEmpty(alarm))
                                                {
                                                    string[] alarm_detail = alarm.Split('_');
                                                    DateTime alarmDt;
                                                    //주알람
                                                    if (alarm_detail[1] == "주알람" && int.TryParse(alarm_detail[0], out int week))
                                                    {
                                                        alarmDt = DateTime.Now.AddDays(week - (int)DateTime.Now.DayOfWeek);
                                                        isOutput = true;
                                                    }
                                                    //월알람
                                                    else if (alarm_detail[1] == "월알람" && int.TryParse(alarm_detail[0], out int days))
                                                    {
                                                        alarmDt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, days);
                                                        isOutput = true;
                                                    }
                                                    //특정일자 - 날짜아닐때
                                                    else if (!DateTime.TryParse(alarm_detail[0], out alarmDt))
                                                        isOutput = false;
                                                    //특정일자 - 날짜맞을때
                                                    else
                                                        isOutput = true;

                                                    //처리일이 오늘이면 출력
                                                    if (Convert.ToDateTime(alarm_complete_dt.ToString("yyyy-MM-dd")) == Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                                                        isOutput = true;

                                                    //출력여부
                                                    if (isOutput)
                                                    {
                                                        isOutput = false;
                                                        //공휴일일 경우 최근영업일로 변경
                                                        alarmDt = common.SetCurrentWorkDate(alarmDt);

                                                        //처리일이 오늘이면 출력
                                                        if (Convert.ToDateTime(alarm_complete_dt.ToString("yyyy-MM-dd")) == Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                                                            isOutput = true;
                                                        //처리일이 아닌데 처리일 보다 알람일이 더 가까우면 출력O
                                                        else if (Convert.ToDateTime(alarmDt.ToString("yyyy-MM-dd")) > Convert.ToDateTime(alarm_complete_dt.ToString("yyyy-MM-dd")))
                                                            isOutput = true;

                                                        //미처리된 옛날 알람일 경우 어제날짜로 강제변경
                                                        if (isOutput)
                                                        {
                                                            if (Convert.ToDateTime(alarmDt.ToString("yyyy-MM-dd")) < Convert.ToDateTime(alarm_standard_sttdate.ToString("yyyy-MM-dd")))
                                                            {
                                                                alarmDt = DateTime.Now.AddDays(-1);
                                                                alarm_division_int = 10;
                                                            }
                                                            //아직 알람일자 안옴
                                                            else if (Convert.ToDateTime(alarmDt.ToString("yyyy-MM-dd")) > Convert.ToDateTime(alarm_standard_enddate.ToString("yyyy-MM-dd")))
                                                                isOutput = false;
                                                            else
                                                                alarm_division_int = 20;

                                                            if (isOutput)
                                                            {
                                                                alarm_date_txt = alarmDt.ToString("yyyy-MM-dd");
                                                                if (!alarm_division.Contains(alarm_detail[2]))
                                                                    alarm_division += " " + alarm_detail[2];
                                                                alarm_complete = Convert.ToBoolean(alarm_detail[3]);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        //alarm_division_int  :  정렬기준
                                        //10 : 미처리
                                        //20 : 오늘 알람
                                        //90 : 완료 알람

                                        if (isOutput)
                                        {
                                            //전체, 대기, 완료
                                            bool isComplete = false;
                                            if (!string.IsNullOrEmpty(alarm_complete_date) && Convert.ToDateTime(alarm_date_txt) <= Convert.ToDateTime(alarm_complete_date))
                                                isComplete = true;

                                            //알람 정렬순서

                                            string[] alarm_divisions = alarm_division.Split(' ');
                                            foreach (string division in alarm_divisions)
                                            {
                                                switch (division)
                                                {
                                                    case "발주":
                                                        alarm_division_int += 1;
                                                        break;
                                                    case "결제":
                                                        alarm_division_int += 2;
                                                        break;
                                                    case "신규":
                                                        alarm_division_int += 3;
                                                        break;
                                                    case "기타":
                                                        alarm_division_int += 4;
                                                        break;
                                                }
                                            }
                                            //완료시 +10
                                            if (isComplete)
                                                alarm_division_int += 90;

                                            if (rbAlarmIncomplete.Checked && isComplete)
                                                isOutput = false;
                                            else if (rbAlarmComplete.Checked && !isComplete)
                                                isOutput = false;
                                        }
                                    }


                                    break;
                                //공용DATA
                                case "tabCommonData":
                                    //담당자 거래처
                                    if (string.IsNullOrEmpty(tempAtoDr["ato_manager"].ToString().Trim()))
                                        isOutput = true;
                                    //거래중
                                    if (isOutput && isTrading)
                                        isOutput = false;
                                    //삭제거래처
                                    if (isOutput && isDelete)
                                        isOutput = false;
                                    //숨김거래처
                                    if (isOutput && isHide)
                                        isOutput = false;
                                    //영업금지거래처
                                    if (isOutput && (isNonHandled || isOutBusiness))
                                        isOutput = false;
                                    break;
                                //내 전체 거래처
                                case "tabAllMyCompany":
                                    //담당자 거래처
                                    if (!string.IsNullOrEmpty(tempAtoDr["ato_manager"].ToString().Trim()))
                                    {
                                        if (!string.IsNullOrEmpty(txtManager.Text.Trim()))
                                        {
                                            string[] managers = txtManager.Text.Split(' ');
                                            for (int j = 0; j < managers.Length; j++)
                                            {
                                                if (!string.IsNullOrEmpty(managers[j]) && tempAtoDr["ato_manager"].ToString().Contains(managers[j]))
                                                {
                                                    isOutput = true;
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                            isOutput = true;
                                    }
                                    //삭제거래처
                                    if (isOutput && isDelete)
                                        isOutput = false;
                                    //숨김거래처
                                    if (isOutput && isHide)
                                        isOutput = false;
                                    //영업금지거래처
                                    if (isOutput && (isNonHandled || isOutBusiness))
                                        isOutput = false;

                                    break;
                                //내DATA
                                case "tabRamdomData":
                                    //담당자 거래처
                                    if (!string.IsNullOrEmpty(tempAtoDr["ato_manager"].ToString().Trim()))
                                    {
                                        if (!string.IsNullOrEmpty(txtManager.Text.Trim()))
                                        {
                                            string[] managers = txtManager.Text.Split(' ');
                                            for (int j = 0; j < managers.Length; j++)
                                            {
                                                if (!string.IsNullOrEmpty(managers[j]) && tempAtoDr["ato_manager"].ToString().Contains(managers[j]))
                                                {
                                                    isOutput = true;
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                            isOutput = true;
                                    }
                                    //잠재1, 2 
                                    if (isOutput && (isPotential1 || isPotential2))
                                        isOutput = false;
                                    //거래중
                                    if (isOutput && isTrading)
                                        isOutput = false;
                                    //삭제거래처
                                    if (isOutput && isDelete)
                                        isOutput = false;
                                    //숨김거래처
                                    if (isOutput && isHide)
                                        isOutput = false;
                                    //영업금지거래처
                                    if (isOutput && (isNonHandled || isOutBusiness))
                                        isOutput = false;
                                    break;
                                //잠재1
                                case "tabCompany":
                                    {
                                        //담당자 거래처
                                        if (!string.IsNullOrEmpty(tempAtoDr["ato_manager"].ToString().Trim()))
                                        {
                                            if (!string.IsNullOrEmpty(txtManager.Text.Trim()))
                                            {
                                                string[] managers = txtManager.Text.Split(' ');
                                                for (int j = 0; j < managers.Length; j++)
                                                {
                                                    if (!string.IsNullOrEmpty(managers[j]) && tempAtoDr["ato_manager"].ToString().Contains(managers[j]))
                                                    {
                                                        isOutput = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            else
                                                isOutput = true;
                                        }
                                        //잠재1, 잠재2
                                        if (!bool.TryParse(tempAtoDr["isPotential1"].ToString(), out isPotential1))
                                            isPotential1 = false;
                                        if (!bool.TryParse(tempAtoDr["isPotential2"].ToString(), out isPotential2))
                                            isPotential2 = false;
                                        if (isOutput && !isPotential1)
                                            isOutput = false;
                                        //거래중
                                        if (isOutput && isTrading)
                                            isOutput = false;
                                        //삭제거래처
                                        if (isOutput && isDelete)
                                            isOutput = false;
                                        //숨김거래처
                                        if (isOutput && isHide)
                                            isOutput = false;
                                        //영업금지거래처
                                        if (isOutput && (isNonHandled || isOutBusiness))
                                            isOutput = false;
                                    }
                                    break;
                                //잠재2
                                case "tabCompany2":
                                    {
                                        //담당자 거래처
                                        if (!string.IsNullOrEmpty(tempAtoDr["ato_manager"].ToString().Trim()))
                                        {
                                            if (!string.IsNullOrEmpty(txtManager.Text.Trim()))
                                            {
                                                string[] managers = txtManager.Text.Split(' ');
                                                for (int j = 0; j < managers.Length; j++)
                                                {
                                                    if (!string.IsNullOrEmpty(managers[j]) && tempAtoDr["ato_manager"].ToString().Contains(managers[j]))
                                                    {
                                                        isOutput = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            else
                                                isOutput = true;
                                        }
                                        //잠재1, 잠재2
                                        if (!bool.TryParse(tempAtoDr["isPotential1"].ToString(), out isPotential1))
                                            isPotential1 = false;
                                        if (!bool.TryParse(tempAtoDr["isPotential2"].ToString(), out isPotential2))
                                            isPotential2 = false;
                                        if (isOutput && !isPotential2)
                                            isOutput = false;
                                        //거래중
                                        if (isOutput && isTrading)
                                            isOutput = false;
                                        //삭제거래처
                                        if (isOutput && isDelete)
                                            isOutput = false;
                                        //숨김거래처
                                        if (isOutput && isHide)
                                            isOutput = false;
                                        //영업금지거래처
                                        if (isOutput && (isNonHandled || isOutBusiness))
                                            isOutput = false;
                                    }
                                    break;
                                //SEAOVER
                                case "tabSeaover":
                                    //담당자 거래처
                                    if (!string.IsNullOrEmpty(tempAtoDr["ato_manager"].ToString().Trim()))
                                    {
                                        if (!string.IsNullOrEmpty(txtManager.Text.Trim()))
                                        {
                                            string[] managers = txtManager.Text.Split(' ');
                                            for (int j = 0; j < managers.Length; j++)
                                            {
                                                if (!string.IsNullOrEmpty(managers[j]) && tempAtoDr["ato_manager"].ToString().Contains(managers[j]))
                                                {
                                                    isOutput = true;
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                            isOutput = true;
                                    }
                                    //구분
                                    /*if (isOutput && tempAtoDr["division"].ToString() != "SEAOVER")
                                        isOutput = false;*/
                                    //거래중
                                    if (isOutput && !isTrading)
                                        isOutput = false;
                                    //삭제거래처
                                    if (isOutput && isDelete)
                                        isOutput = false;
                                    //숨김거래처
                                    if (isOutput && isHide)
                                        isOutput = false;
                                    //영업금지거래처
                                    if (isOutput && (isNonHandled || isOutBusiness))
                                        isOutput = false;
                                    break;
                                //영업금지거래처
                                case "tabNotTreatment":
                                    //영업금지거래처
                                    if (isNonHandled || isOutBusiness)
                                        isOutput = true;
                                    //삭제거래처
                                    if (isDelete)
                                        isOutput = false;
                                    //숨김거래처
                                    if (isOutput && isHide)
                                        isOutput = false;

                                    //구분
                                    if (isOutput)
                                    {
                                        if (cbNoneHandled.Text.Contains("취급X") && cbNoneHandled.Text.Contains("팩스X") && cbNoneHandled.Text.Contains("폐업"))
                                        {
                                            if (!(isNonHandled && isOutBusiness))
                                                isOutput = false;
                                        }
                                        else if (cbNoneHandled.Text.Contains("취급X") && cbNoneHandled.Text.Contains("팩스X"))
                                        {
                                            if (!(isNonHandled && !isOutBusiness))
                                                isOutput = false;
                                        }
                                        else if (cbNoneHandled.Text.Contains("취급X") && cbNoneHandled.Text.Contains("폐업"))
                                        {
                                            if (!(isNonHandled && isOutBusiness))
                                                isOutput = false;
                                        }
                                        else if (cbNoneHandled.Text.Contains("팩스X") && cbNoneHandled.Text.Contains("폐업"))
                                        {
                                            if (!(!isNonHandled && isOutBusiness))
                                                isOutput = false;
                                        }
                                        else if (cbNoneHandled.Text.Contains("취급X"))
                                        {
                                            if (!(isNonHandled && !isOutBusiness))
                                                isOutput = false;
                                        }
                                        else if (cbNoneHandled.Text.Contains("팩스X"))
                                        {
                                            if (!(!isNonHandled && !isOutBusiness))
                                                isOutput = false;
                                        }
                                        else if (cbNoneHandled.Text.Contains("폐업"))
                                        {
                                            if (!(!isNonHandled && isOutBusiness))
                                                isOutput = false;
                                        }
                                    }

                                    break;
                                //팩스금지거래처
                                case "tabNotSendFax":
                                    //영업금지거래처
                                    if (isNotSendFax)
                                        isOutput = true;
                                    //삭제거래처
                                    if (isDelete)
                                        isOutput = false;

                                    //구분
                                    if (isOutput)
                                    {
                                        //담당자 거래처
                                        if (!string.IsNullOrEmpty(txtManager.Text.Trim()))
                                        {
                                            if (!string.IsNullOrEmpty(tempAtoDr["ato_manager"].ToString().Trim()))
                                            {
                                                isOutput = false;
                                                string[] managers = txtManager.Text.Split(' ');
                                                for (int j = 0; j < managers.Length; j++)
                                                {
                                                    if (!string.IsNullOrEmpty(managers[j]) && tempAtoDr["ato_manager"].ToString().Contains(managers[j]))
                                                    {
                                                        isOutput = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            else
                                                isOutput = false;
                                        }
                                        if (isOutput)
                                        {
                                            if (cbNoneHandled.Text.Contains("취급X") && cbNoneHandled.Text.Contains("팩스X") && cbNoneHandled.Text.Contains("폐업"))
                                            {
                                                if (!(isNonHandled && isOutBusiness))
                                                    isOutput = false;
                                            }
                                            else if (cbNoneHandled.Text.Contains("취급X") && cbNoneHandled.Text.Contains("팩스X"))
                                            {
                                                if (!(isNonHandled && !isOutBusiness))
                                                    isOutput = false;
                                            }
                                            else if (cbNoneHandled.Text.Contains("취급X") && cbNoneHandled.Text.Contains("폐업"))
                                            {
                                                if (!(isNonHandled && isOutBusiness))
                                                    isOutput = false;
                                            }
                                            else if (cbNoneHandled.Text.Contains("팩스X") && cbNoneHandled.Text.Contains("폐업"))
                                            {
                                                if (!(!isNonHandled && isOutBusiness))
                                                    isOutput = false;
                                            }
                                            else if (cbNoneHandled.Text.Contains("취급X"))
                                            {
                                                if (!(isNonHandled && !isOutBusiness))
                                                    isOutput = false;
                                            }
                                            else if (cbNoneHandled.Text.Contains("팩스X"))
                                            {
                                                if (!(!isNonHandled && !isOutBusiness))
                                                    isOutput = false;
                                            }
                                            else if (cbNoneHandled.Text.Contains("폐업"))
                                            {
                                                if (!(!isNonHandled && isOutBusiness))
                                                    isOutput = false;
                                            }
                                        }
                                    }

                                    break;
                                //삭제거래처
                                case "tabDeleteCompany":
                                    //삭제거래처
                                    if (isDelete)
                                        isOutput = true;
                                    break;
                            }

                            //소송거래처 제외=========================================================================================================
                            /*if (isOutput && cbLitigation.Checked
                                && (tempAtoDr["company"].ToString().Contains("(S)") || tempAtoDr["company"].ToString().Contains("소송")))
                                isOutput = false;*/
                            //검색조건================================================================================================================
                            if (isOutput)
                            {
                                //최근매출내역
                                DateTime limitDate, current_sales_date;
                                if (DateTime.TryParse(saleTelDate, out limitDate))
                                {
                                    isOutput = false;
                                    if (DateTime.TryParse(tempAtoDr["current_sale_date"].ToString(), out current_sales_date) && limitDate >= current_sales_date)
                                        isOutput = true;
                                }

                                //최근영업내역
                                DateTime limitDate2, sale_updatetime;
                                if (DateTime.TryParse(saleUpdateDate, out limitDate2))
                                {
                                    isOutput = false;
                                    if (DateTime.TryParse(tempAtoDr["sales_updatetime"].ToString(), out sale_updatetime) && limitDate2 >= sale_updatetime)
                                        isOutput = true;
                                }
                                //취급품목 검색
                                if (isOutput && !string.IsNullOrEmpty(txtHandlingItem.Text.Trim()))
                                {
                                    isOutput = false;
                                    string[] splitTxt = txtHandlingItem.Text.Trim().Split(' ');
                                    for (int j = 0; j < splitTxt.Length; j++)
                                    {
                                        if (!string.IsNullOrEmpty(splitTxt[j]) && (common.isContains(tempAtoDr["handling_item"].ToString().Replace("-", "").Replace(" ", ""), splitTxt[j])))
                                        {
                                            isOutput = true;
                                            break;
                                        }
                                    }

                                    if (!isOutput)
                                    {
                                        for (int j = 0; j < splitTxt.Length; j++)
                                        {
                                            if (!string.IsNullOrEmpty(splitTxt[j]) && (common.isContains(tempAtoDr["seaover_handling_item"].ToString().Replace("-", "").Replace(" ", ""), splitTxt[j])))
                                            {
                                                isOutput = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //출력
                        if (isOutput)
                        {
                            /*if (tempAtoDr["company"].ToString().Contains("피딜"))
                                tempAtoDr["company"] = tempAtoDr["company"].ToString();*/
                           dataCnt++;

                            DataRow row;
                            if (sub_id == 0)
                                row = mainDt.NewRow();
                            else
                                row = subDt.NewRow();

                            row["id"] = tempAtoDr["id"].ToString();
                            row["group_name"] = tempAtoDr["group_name"].ToString();
                            row["category"] = tempAtoDr["category"].ToString();
                            row["pre_category"] = tempAtoDr["category"].ToString();

                            row["company"] = tempAtoDr["company"].ToString();
                            if (cbTelHipen.Checked)
                            {
                                row["tel"] = SetTelTxt(tempAtoDr["tel"].ToString());
                                row["fax"] = SetTelTxt(tempAtoDr["fax"].ToString());
                                row["phone"] = SetTelTxt(tempAtoDr["phone"].ToString());
                                row["other_phone"] = SetTelTxt(tempAtoDr["other_phone"].ToString());
                            }
                            else
                            {
                                row["tel"] = tempAtoDr["tel"].ToString();
                                row["fax"] = tempAtoDr["fax"].ToString();
                                row["phone"] = tempAtoDr["phone"].ToString();
                                row["other_phone"] = tempAtoDr["other_phone"].ToString();
                            }

                            row["pre_tel"] = tempAtoDr["tel"].ToString();
                            row["pre_fax"] = tempAtoDr["fax"].ToString();
                            row["pre_phone"] = tempAtoDr["phone"].ToString();
                            row["pre_other_phone"] = tempAtoDr["other_phone"].ToString();

                            row["registration_number"] = tempAtoDr["registration_number"].ToString();


                            row["ceo"] = tempAtoDr["ceo"].ToString();
                            row["address"] = tempAtoDr["address"].ToString();
                            row["remark"] = tempAtoDr["remark"].ToString().Replace(@"\r\n", "\n");
                            row["distribution"] = tempAtoDr["distribution"].ToString().Replace(@"\r\n", "\n");
                            row["handling_item"] = tempAtoDr["handling_item"].ToString().Replace(@"\r\n", "\n");
                            row["seaover_handling_item"] = tempAtoDr["seaover_handling_item"].ToString().Replace(@"\r\n", "\n");
                            row["payment_date"] = tempAtoDr["payment_date"].ToString().Replace(@"\r\n", "\n");
                            row["seaover_company_code"] = tempAtoDr["seaover_company_code"].ToString();

                            row["origin"] = tempAtoDr["origin"].ToString();
                            row["remark2"] = tempAtoDr["remark2"].ToString();
                            row["remark4"] = tempAtoDr["remark4"].ToString().Replace(@"\r\n", "\n");
                            row["remark5"] = tempAtoDr["remark5"].ToString();
                            row["remark6"] = tempAtoDr["remark6"].ToString();

                            row["company_manager"] = tempAtoDr["company_manager"].ToString();
                            row["company_manager_position"] = tempAtoDr["company_manager_position"].ToString();
                            row["email"] = tempAtoDr["email"].ToString();
                            row["web"] = tempAtoDr["web"].ToString();
                            row["sns1"] = tempAtoDr["sns1"].ToString();
                            row["sns2"] = tempAtoDr["sns2"].ToString();
                            row["sns3"] = tempAtoDr["sns3"].ToString();
                            bool isManagement1, isManagement2, isManagement3, isManagement4;
                            if (!bool.TryParse(tempAtoDr["isManagement1"].ToString(), out isManagement1))
                                isManagement1 = false;
                            if (!bool.TryParse(tempAtoDr["isManagement2"].ToString(), out isManagement2))
                                isManagement2 = false;
                            if (!bool.TryParse(tempAtoDr["isManagement3"].ToString(), out isManagement3))
                                isManagement3 = false;
                            if (!bool.TryParse(tempAtoDr["isManagement4"].ToString(), out isManagement4))
                                isManagement4 = false;
                            row["isManagement1"] = isManagement1;
                            row["isManagement2"] = isManagement2;
                            row["isManagement3"] = isManagement3;
                            row["isManagement4"] = isManagement4;
                            if (!bool.TryParse(tempAtoDr["isHide"].ToString(), out isHide))
                                isHide = false;
                            row["isHide"] = isHide;

                            row["isTrading"] = isTrading;
                            row["createtime"] = tempAtoDr["createtime"].ToString();
                            row["sales_comment"] = tempAtoDr["sales_comment"].ToString();
                            row["ato_manager"] = tempAtoDr["ato_manager"].ToString();
                            row["pre_ato_manager"] = tempAtoDr["ato_manager"].ToString();
                            //잠재1, 잠재2
                            row["isPotential1"] = isPotential1;
                            row["isPotential2"] = isPotential2;
                            //영업금지 거래처
                            row["isNonHandled"] = isNonHandled;
                            row["isNotSendFax"] = isNotSendFax;
                            row["isOutBusiness"] = isOutBusiness;

                            //최근 영업정보
                            DateTime updatetime;
                            if (DateTime.TryParse(tempAtoDr["sales_updatetime"].ToString(), out updatetime))
                                row["sales_updatetime"] = updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                            else
                                row["sales_updatetime"] = new DateTime(2999, 12, 31).ToString("yyyy-MM-dd");

                            //최근 매출정보
                            DateTime current_sales_date;
                            if (DateTime.TryParse(tempAtoDr["current_sale_date"].ToString(), out current_sales_date))
                                row["current_sale_date"] = current_sales_date.ToString("yyyy-MM-dd HH:mm:ss");
                            else
                                row["current_sale_date"] = new DateTime(2999, 12, 31).ToString("yyyy-MM-dd");

                            //최근 영업내용                            
                            row["sales_contents"] = tempAtoDr["sales_contents"].ToString();
                            row["sales_remark"] = tempAtoDr["sales_remark"].ToString();
                            row["sales_log"] = tempAtoDr["sales_log"].ToString();
                            row["sales_edit_user"] = tempAtoDr["sales_edit_user"].ToString();
                            row["table_div"] = 1;
                            row["table_index"] = tempAtoDr["table_index"].ToString();

                            //알람
                            row["alarm_month"] = alarm_month;
                            row["alarm_week"] = alarm_week;
                            row["alarm_complete_date"] = tempAtoDr["alarm_complete_date"].ToString();

                            //최근 매출정보
                            row["alarm_division"] = alarm_division.Trim().Replace(' ', '/');
                            row["alarm_division_int"] = alarm_division_int;
                            row["alarm_complete"] = alarm_complete;
                            row["alarm_date"] = alarm_date_txt;
                            //업종
                            row["industry_type"] = tempAtoDr["industry_type"].ToString();
                            row["industry_type2"] = tempAtoDr["industry_type2"].ToString();

                            int industry_type_rank;
                            if (!int.TryParse(tempAtoDr["industry_type_rank"].ToString(), out industry_type_rank))
                                industry_type_rank = 99;
                            row["industry_type_rank"] = industry_type_rank;

                            //대표품목,서브품목
                            row["main_id"] = main_id;
                            row["sub_id"] = sub_id;

                            if (sub_id == 0)
                                mainDt.Rows.Add(row);
                            else
                                subDt.Rows.Add(row);
                        }

                        //취급X 또는 삭제거래처는 1000개씩 출력
                        if (limitCnt > 0 && dataCnt >= limitCnt)
                            break;
                    }
                }
                catch
                {
                    messageBox.Show(this, "에러발생 Row index :  " + tmpRowindex + " 확인바랍니다.");
                }
            }            
            DataView dv = new DataView(mainDt);
            if (tcMain.SelectedTab.Name == "tabAlarm")
                dv.Sort = "alarm_division_int ASC, " + GetOrderByTxt(cbSortType.Text);
            else if (tcMain.SelectedTab.Name == "tabNotTreatment")
                dv.Sort = "sales_updatetime DESC," + GetOrderByTxt(cbSortType.Text);
            else if (tcMain.SelectedTab.Name == "tabAlarm")
                dv.Sort = "alarm_division, alarm_complete," + GetOrderByTxt(cbSortType.Text);
            else
                dv.Sort = GetOrderByTxt(cbSortType.Text);

            mainDt = dv.ToTable();

            //취급X는 너무 많아서 1000개만 출력
            /*if (tcMain.SelectedTab.Name == "tabNotTreatment")
            {
                for (int i = mainDt.Rows.Count - 1; i >= 1000; i--)
                    mainDt.Rows.RemoveAt(i);
            }*/
            dgvCompany.DataSource = mainDt;
            //스타일
            SetHeaderStyle();
            //대표,서브 거래처
            SetGroupCompany();
            this.dgvCompany.isPush = true;
            this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            //팩스X 정렬가능하게
            dgvCompany.Columns["isNotSendFax"].SortMode = DataGridViewColumnSortMode.Automatic;
            //중복
            isCheckDuplicate = false;
            //Redo, Undo 최신화
            this.dgvCompany.isPush = true;
            dgvCompany.SetStackIdx(tcMain.SelectedIndex);
        }


        private string whereSql(string col, string val, bool isExactly)
        {
            string tmp = "";
            if (isExactly)
                tmp = $" AND {col} LIKE '%{val}%'";
            else
            {
                string[] valTxt = val.Trim().Split(' ');
                for (int j = 0; j < valTxt.Length; j++)
                {
                    if (!string.IsNullOrEmpty(valTxt[j].Trim()))
                    {
                        if(string.IsNullOrEmpty(tmp))
                            tmp = $" {col} LIKE '%{valTxt[j].Trim()}%'";
                        else
                            tmp += $" OR {col} LIKE '%{valTxt[j].Trim()}%'";
                    }
                    
                }
                tmp = " AND (" + tmp + ")";
            }
            return tmp;
        }


        private string SetTelHipen(string tel)
        {
            if (!tel.Contains('-'))
            {
                if (tel.Length == 8 && (tel.Substring(0, 2) == "15" || tel.Substring(0, 2) == "16" || tel.Substring(0, 2) == "18"))
                    tel = tel.Substring(0, 4) + "-" + tel.Substring(4, 4);
                else if (tel.Length == 9 && tel.Substring(0, 2) == "02")
                    tel = tel.Substring(0, 2) + "-" + tel.Substring(2, 3) + "-" + tel.Substring(5, 4);
                else if (tel.Length == 9 && tel.Substring(0, 1) != "0")
                {
                    tel = "0" + tel;
                    tel = tel.Substring(0, 3) + "-" + tel.Substring(3, 3) + "-" + tel.Substring(6, 4);
                }
                else if (tel.Length == 10 && tel.Substring(0, 2) == "02")
                    tel = tel.Substring(0, 2) + "-" + tel.Substring(2, 3) + "-" + tel.Substring(5, 4);
                else if (tel.Length == 10)
                {
                    if (tel.Substring(0, 2) == "70")
                    {
                        tel = "0" + tel;
                        tel = tel.Substring(0, 3) + "-" + tel.Substring(3, 4) + "-" + tel.Substring(7, 4);
                    }
                    else if (tel.Substring(0, 1) != "0")
                    {
                        tel = "0" + tel;
                        tel = tel.Substring(0, 3) + "-" + tel.Substring(3, 4) + "-" + tel.Substring(7, 4);
                    }
                    else
                        tel = tel.Substring(0, 3) + "-" + tel.Substring(3, 3) + "-" + tel.Substring(6, 4);
                }
                else if (tel.Length == 11 && tel.Substring(0, 3) == "507")
                {
                    tel = "0" + tel;
                    tel = tel.Substring(0, 4) + "-" + tel.Substring(4, 4) + "-" + tel.Substring(8, 4);
                }
                else if (tel.Length == 11)
                    tel = tel.Substring(0, 3) + "-" + tel.Substring(3, 4) + "-" + tel.Substring(7, 4);
                else if (tel.Length == 12 && (tel.Substring(0, 4) == "0507" || tel.Substring(0, 4) == "0303"))
                    tel = tel.Substring(0, 4) + "-" + tel.Substring(4, 4) + "-" + tel.Substring(8, 4);
            }

            return tel;
        }
        private string SetTelTxt(string tel)
        {
            if (!string.IsNullOrEmpty(tel) && !tel.Contains('-'))
            {
                tel = tel.Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "");
                if (tel.Length > 1 && (tel.Substring(0, 1) == "," || tel.Substring(0, 1) == "/"))
                    tel = tel.Substring(1, tel.Length - 1);

                if (tel.Contains(','))
                {
                    string[] tels = tel.Split(',');
                    for (int j = 0; j < tels.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(tels[j].Trim()))
                            tels[j] = SetTelHipen(tels[j].Trim());
                    }

                    tel = "";
                    for (int j = 0; j < tels.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(tels[j].Trim()))
                        {
                            tel += " " + tels[j].Trim();
                        }
                    }
                    tel = tel.Trim().Replace(" ", ",");
                }
                else if (tel.Contains('/'))
                {
                    string[] tels = tel.Split('/');
                    for (int j = 0; j < tels.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(tels[j].Trim()))
                            tels[j] = SetTelHipen(tels[j].Trim());
                    }

                    tel = "";
                    for (int j = 0; j < tels.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(tels[j].Trim()))
                        {
                            tel += " " + tels[j].Trim();
                        }
                    }
                    tel = tel.Trim().Replace(" ", "/");
                }
                else if (tel.Contains('~'))
                {
                    string[] tels = tel.Split('~');
                    for (int j = 0; j < tels.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(tels[j].Trim()))
                            tels[j] = SetTelHipen(tels[j].Trim());
                    }

                    tel = "";
                    for (int j = 0; j < tels.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(tels[j].Trim()))
                        {
                            tel += " " + tels[j].Trim();
                        }
                    }
                    tel = tel.Trim().Replace(" ", "~");
                }
                else
                    tel = SetTelHipen(tel);

            }
            else
                tel = SetTelHipen(tel);

            return tel;
        }

        private void SetGroupCompany()
        {
            if (dgvCompany.Rows.Count > 0)
            {
                //알람기준
                DateTime alarm_standard_date = DateTime.Now;
                /*switch (cbAlarmDivision.Text)
                {
                    case "오늘":
                        alarm_standard_date = DateTime.Now;
                        break;
                    case "어제":
                        alarm_standard_date = DateTime.Now.AddDays(-1);
                        break;
                    case "내일":
                        alarm_standard_date = DateTime.Now.AddDays(1);
                        break;
                }*/
                //알람수
                int orderCnt = 0, paymentCnt = 0, newCnt = 0, etcCnt = 0, weekCnt = 0, monthCnt = 0, completeCnt = 0;

                for (int i = 0; i < dgvCompany.Rows.Count; i++)
                {
                    DataGridViewButtonCell cell = new DataGridViewButtonCell();
                    cell.Value = "완료";
                    dgvCompany.Rows[i].Cells["btn_alarm_complete"] = cell;
                    dgvCompany.Rows[i].Cells["btn_alarm_complete"].Value = "완료";
                    dgvCompany.Rows[i].Cells["btn_alarm_complete"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    /*DataGridViewButtonCell eCell = new DataGridViewButtonCell();
                    eCell.Value = "연장";
                    dgvCompany.Rows[i].Cells["btn_alarm_extension"] = eCell;
                    dgvCompany.Rows[i].Cells["btn_alarm_extension"].Value = "연장";
                    dgvCompany.Rows[i].Cells["btn_alarm_extension"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;*/

                    int main_id;
                    if (dgvCompany.Rows[i].Cells["main_id"].Value == null || !int.TryParse(dgvCompany.Rows[i].Cells["main_id"].Value.ToString(), out main_id))
                        main_id = 0;
                    int sub_id;
                    if (dgvCompany.Rows[i].Cells["sub_id"].Value == null || !int.TryParse(dgvCompany.Rows[i].Cells["sub_id"].Value.ToString(), out sub_id))
                        sub_id = 0;

                    //Main거래처
                    if (main_id > 0 && sub_id == 0)
                    {
                        dgvCompany.Rows[i].Cells["company"].Style.Font = new Font("굴림", 11, FontStyle.Bold);
                        dgvCompany.Rows[i].DefaultCellStyle.BackColor = Color.Beige;
                    }
                    //Sub거래처
                    else if (main_id > 0 && sub_id > 0)
                    {
                        dgvCompany.Rows[i].Cells["company"].Style.Font = new Font("굴림", 11, FontStyle.Regular);
                        dgvCompany.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                    }

                    //알람 완료처리
                    if (tcMain.SelectedTab.Name == "tabAlarm")
                    {
                        string alarm_division = dgvCompany.Rows[i].Cells["alarm_division"].Value.ToString();
                        if (alarm_division.Contains("발주"))
                            orderCnt++;
                        if (alarm_division.Contains("결제"))
                            orderCnt++;
                        if (alarm_division.Contains("신규"))
                            orderCnt++;
                        if (alarm_division.Contains("기타"))
                            orderCnt++;

                        DateTime alarm_complete_date;
                        if (dgvCompany.Rows[i].Cells["alarm_complete_date"].Value != null && DateTime.TryParse(dgvCompany.Rows[i].Cells["alarm_complete_date"].Value.ToString(), out alarm_complete_date))
                        {
                            if (dgvCompany.Rows[i].Cells["alarm_date"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["alarm_date"].Value.ToString()))
                            {
                                string alarm_date = dgvCompany.Rows[i].Cells["alarm_date"].Value.ToString();
                                string[] alarms = alarm_date.Split(',');
                                foreach (string alarm in alarms)
                                {
                                    if (!string.IsNullOrEmpty(alarm))
                                    {
                                        string[] alarm_detail = alarm.Split('_');

                                        DateTime alarmDt;
                                        if (DateTime.TryParse(alarm_detail[0], out alarmDt))
                                        {
                                            //공휴일일 경우 최근영업일로 변경
                                            alarmDt = common.SetCurrentWorkDate(alarmDt);
                                            if (Convert.ToDateTime(alarmDt.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(alarm_standard_date.ToString("yyyy-MM-dd"))
                                                && Convert.ToDateTime(alarmDt.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(alarm_complete_date.ToString("yyyy-MM-dd")))
                                            {
                                                if(rbAlarmComplete.Checked)
                                                    dgvCompany.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                                                else
                                                    dgvCompany.Rows[i].DefaultCellStyle.ForeColor = Color.LightGray;
                                                completeCnt++;
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                txtAlarmOrder.Text = orderCnt.ToString("#,##0");
                txtAlarmPayment.Text = paymentCnt.ToString("#,##0");
                txtAlarmNew.Text = newCnt.ToString("#,##0");
                txtAlarmEtc.Text = etcCnt.ToString("#,##0");
                txtAlarmComplete.Text = completeCnt.ToString("#,##0");
            }
        }

        private string GetOrderByTxt(string sortType)
        {
            if (string.IsNullOrEmpty(sortType.Trim()))
                return null;

            string[] sort = sortType.Split('+');
            string result = "";
            if (dgvCompany.Columns.Count > 0)
            {
                for (int i = 0; i < sort.Length; i++)
                {
                    if (!string.IsNullOrEmpty(sort[i].Trim()))
                    {
                        if (sort[i].Trim() == "지역")
                            sort[i] = "주소";
                        else if (sort[i].Trim() == "업태")
                            sort[i] = "업태순위";

                        for (int j = 0; j < mainDt.Columns.Count; j++)
                        {
                            if (sort[i] == mainDt.Columns[j].Caption)
                            {
                                if (string.IsNullOrEmpty(result))
                                    result = mainDt.Columns[j].ColumnName;
                                else
                                    result += "," + mainDt.Columns[j].ColumnName;

                                break;
                            }
                        }
                    }
                }

                return result;
            }
            else
                return null;
        }

        private string SetSecurityTxt(string txt, int visibleLength = 1)
        {
            //securityType 1: 반가리기
            //securityType 2 부터는 보여지는 글자수

            if (!string.IsNullOrEmpty(txt) && txt.Length > 2)
            {
                if (visibleLength == 1)
                {
                    string resultTxt = txt.Substring(0, txt.Length / 2);

                    for (int i = txt.Length; i < txt.Length; i++)
                        resultTxt += "*";
                    return resultTxt;
                }
                else
                {
                    if (visibleLength < txt.Length)
                        visibleLength = txt.Length;


                    string resultTxt = txt.Substring(0, visibleLength - 1);

                    for (int i = visibleLength - 1; i < txt.Length; i++)
                        resultTxt += "*";
                    return resultTxt;

                }
            }
            else
                return txt;

        }

        #endregion

        #region Button, ComboBox
        private void btnCheckNotSendFax_Click(object sender, EventArgs e)
        {
            if (excelDt != null && excelDt.Rows.Count > 0)
            {
                string fax_column_name = "";
                foreach (DataGridViewRow row in dgvSearching.Rows)
                {
                    bool isChecked;
                    if (row.Cells["chk"].Value == null || !bool.TryParse(row.Cells["chk"].Value.ToString(), out isChecked))
                        isChecked = false;

                    if (isChecked)
                    {
                        fax_column_name = row.Cells["column_name"].Value.ToString();
                        break;
                    }
                }
                //fax컬럼
                if (string.IsNullOrEmpty(fax_column_name))
                {
                    messageBox.Show(this, "비교할 팩스컬럼을 선택해주세요!");
                    return;
                }

                //입력데이터 -> Datatable
                DataTable inputDt = common.ConvertDgvToDataTable(dgvData);
                inputDt = SetFaxDataDt(inputDt, fax_column_name);
                //매출처 정보
                DataTable NotSendFaxDt = notSendFaxRepository.GetNotSendFax("");
                NotSendFaxDt = SetFaxDataDt(NotSendFaxDt, "fax");
                // 데이터 중복추출, PLINQ 사용
                DataTable duplicateDt = null;
                DataRow[] tempDr = inputDt.Select($"fax <> ''");
                DataRow[] tempDr2 = NotSendFaxDt.Select("fax <> ''");
                if (tempDr.Length > 0 && tempDr2.Length > 0)
                {
                    DataTable rnDt = tempDr.CopyToDataTable();
                    DataTable rnDt2 = tempDr2.CopyToDataTable();
                    var duplicateVar = from p in rnDt.AsEnumerable()
                                       join t in rnDt2.AsEnumerable()
                                       on p.Field<string>("fax") equals t.Field<string>("fax")
                                       into outer
                                       from t in outer.DefaultIfEmpty()
                                       select new
                                       {
                                           table_index = p.Field<string>("table_index"),
                                           fax = p.Field<string>("fax"),
                                           is_not_send_fax = (t == null) ? "FALSE" : "TRUE"
                                       };
                    duplicateDt = ConvertListToDatatable(duplicateVar);
                    if (duplicateDt != null)
                        duplicateDt.AcceptChanges();
                }
                //중복삭제
                int not_send_fax_count = 0;
                if (duplicateDt != null && duplicateDt.Rows.Count > 0)
                {
                    foreach (DataRow duplicateDr in duplicateDt.Rows)
                    {
                        int table_index;
                        if (!int.TryParse(duplicateDr["table_index"].ToString(), out table_index))
                            table_index = -1;

                        bool is_not_send_fax;
                        if (!bool.TryParse(duplicateDr["is_not_send_fax"].ToString(), out is_not_send_fax))
                            is_not_send_fax = false;

                        if (table_index >= 0)
                        {
                            dgvData.Rows[table_index].Cells["is_not_send_fax"].Value = is_not_send_fax;
                            if(is_not_send_fax)
                                not_send_fax_count++;
                        }
                    }
                    dgvData.Update();
                    txtNotSendFaxCount.Text = not_send_fax_count.ToString("#,##0");
                }
                messageBox.Show(this, "완료");
            }
            else
                messageBox.Show(this, "비교할 데이터가 없습니다!");
        }
        private void btnInsertNotSendFax_Click(object sender, EventArgs e)
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

            InsertNotSendFax insf = new InsertNotSendFax(um, this);
            insf.ShowDialog();
        }
        private void btnSelectExcelFile_Click(object sender, EventArgs e)
        {
            excel_path = ChooseExcelFile();
            if (excel_path != null && !string.IsNullOrEmpty(excel_path))
            {
                timer_start();
                bgw2.RunWorkerAsync();
            }
        }
        private void btnColumnSetting_Click(object sender, EventArgs e)
        {
            ColumnSetting cs = new ColumnSetting(this.dgvCompany);
            cs.Owner = this;
            DataTable columnDt = cs.GetColumnSetting();
            if (columnDt != null && columnDt.Rows.Count > 0)
            {
                foreach (DataRow dr in columnDt.Rows)
                {
                    dgvCompany.Columns[dr["column_name"].ToString()].Visible = Convert.ToBoolean(dr["isVisible"].ToString());
                    dgvCompany.Columns[dr["column_name"].ToString()].Width = Convert.ToInt16(dr["column_width"].ToString());
                }
            }
        }
        private void btnAdminDashboard_Click(object sender, EventArgs e)
        {
            //데이터 최신화 ( 1시간 이상 지났을 경우)
            TimeSpan ts = DateTime.Now - dataUpdatetime;
            if(ts.Minutes > 60)
                RefreshTable(false);

            //대시보드 
            SalesManagerDashboard smd = new SalesManagerDashboard(um, mainDt);
            smd.Owner = this;
            smd.Show();
        }
        private void cbTelHipen_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTelHipen.Checked)
            {
                SetCellValueEvent(false);
                foreach (DataGridViewRow row in dgvCompany.Rows)
                {
                    row.Cells["tel"].Value = SetTelTxt(row.Cells["pre_tel"].Value.ToString());
                    row.Cells["fax"].Value = SetTelTxt(row.Cells["pre_fax"].Value.ToString());
                    row.Cells["phone"].Value = SetTelTxt(row.Cells["pre_phone"].Value.ToString());
                    row.Cells["other_phone"].Value = SetTelTxt(row.Cells["pre_other_phone"].Value.ToString());
                }
                SetCellValueEvent(true);
            }
            else
            {
                SetCellValueEvent(false);
                foreach (DataGridViewRow row in dgvCompany.Rows)
                {
                    row.Cells["tel"].Value = row.Cells["pre_tel"].Value.ToString();
                    row.Cells["fax"].Value = row.Cells["pre_fax"].Value.ToString();
                    row.Cells["phone"].Value = row.Cells["pre_phone"].Value.ToString();
                    row.Cells["other_phone"].Value = row.Cells["pre_other_phone"].Value.ToString();
                }
                SetCellValueEvent(true);
            }
        }
        private void cbSimpleInput_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvInputCompany.Columns.Count > 0)
            {
                foreach (DataGridViewColumn col in dgvInputCompany.Columns)
                {
                    if (col.Name == "input_seaover_company_code" || col.Name == "input_address" || col.Name == "input_ceo")
                        col.Visible = !cbSimpleInput.Checked;
                }
            }
        }
        private void btnConvertToCommonData_Click(object sender, EventArgs e)
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

            if (tcMain.SelectedTab.Name == "tabCommonData")
                return;
            //데이터 유효성검사
            if (dgvCompany.SelectedCells.Count == 0 && dgvCompany.SelectedRows.Count == 0)
            {
                messageBox.Show(this,"전환할 거래처를 선택해주세요.");
                this.Activate();
                return;
            }
            //Msg
            if (messageBox.Show(this,"선택한 내역을 모든 담당자가 확인할 수 있는 공용DATA로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            //수정
            if (UpdateCompanyInfo(2))
            {
                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                {
                    if (dgvCompany.Rows[i].Selected)
                        dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                }
            }
        }
        private void btnPurchaseMargin_Click(object sender, EventArgs e)
        {
            int mainformx = this.Location.X;
            int mainformy = this.Location.Y;
            int mainformwidth = this.Size.Width;
            int mainformheight = this.Size.Height;

            KeyHelper acik = new KeyHelper();
            int childformwidth = acik.Size.Width;
            int childformheight = acik.Size.Height;
            acik.Show();
            acik.Location = new Point(mainformx + (mainformwidth / 2) - (childformwidth / 2), mainformy + (mainformheight / 2) - (childformheight / 2));
        }
        private void btnMultiUpdate_Click(object sender, EventArgs e)
        {
            if (messageBox.Show(this,$"체크된 내역을 일괄수정하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dgvCompany.EndEdit();
                UpdateCompany(1);
            }
        }
        private void btnCheckSeaover_Click(object sender, EventArgs e)
        {
            CheckSeaoverCompany();
        }
        private void btnRemoveDuplicate_Click(object sender, EventArgs e)
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

            try
            {
                MyDuplicateDataManager mdm = new MyDuplicateDataManager(um, this, atoDt);
                int n = mdm.GetDulicate();
                mdm.ShowDialog();
            }
            catch (Exception ex)
            {

            }

        }
        private void btnCompanySalesDetail_Click(object sender, EventArgs e)
        {
            int company_id;
            if (!int.TryParse(lbCompanyId.Text, out company_id))
            {
                messageBox.Show(this, "거래처를 선택해주세요.");
                this.Activate();
                return;
            }

            MySalesManager smm = new MySalesManager(um);
            smm.GetCompanySalesDetail(lbCompanyName.Text, company_id);
            smm.Show();
        }

        private void btnCheckRetire_Click(object sender, EventArgs e)
        {
            GetApiAsync();
        }

        private void btnMyDailyBusiness_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "영업거래처 관리", "영업일보", "is_visible"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

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
                DailyBusiness.DailyBusiness db = new DailyBusiness.DailyBusiness(um);
                db.Show();
            }
        }
        Common.ProgressBar pb = null;
        bool isProgress = false;
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            timer_start();
            bgw.RunWorkerAsync();

            this.Activate();
        }

        


        private void btnDetailInfo_Click(object sender, EventArgs e)
        {
            if (dgvCompany.SelectedCells.Count == 0 && dgvCompany.SelectedRows.Count == 0)
                return;
            else if (dgvCompany.SelectedCells.Count > 0 && dgvCompany.SelectedRows.Count == 0)
            {
                int rowindex = dgvCompany.SelectedCells[0].RowIndex;
                dgvCompany.ClearSelection();
                dgvCompany.Rows[rowindex].Selected = true;
            }

            //최근거래처 수정내역
            GetCurrentUpdateInfo(dgvCompany.SelectedRows[0].Index);
            //중복거래처
            SetCellValueEvent(false);
            CheckDuplicateAsOne(dgvCompany.SelectedRows[0].Index);
            SetCellValueEvent(true);
            pnDetail.Visible = true;
            this.Update();
        }

        private void btnDistribution_Click(object sender, EventArgs e)
        {
            DataDistribution dd = new DataDistribution(um, this);
            dd.Show();
            //dd.Start();
        }
        private void cbNotOutOfBusiness_CheckedChanged(object sender, EventArgs e)
        {
            dgvCompany.Columns["isOutBusiness"].Visible = !cbNotOutOfBusiness.Checked;
        }
        private void btnMySales_Click(object sender, EventArgs e)
        {
            try
            {
                MySalesManager msm = new MySalesManager(um);
                msm.Show();
            }
            catch
            { }
        }
        private void btnAddCompany_Click(object sender, EventArgs e)
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

            try
            {
                DuplicateCompany.DuplicateCompany dc = new DuplicateCompany.DuplicateCompany(um, this, atoDt, true);
                dc.Show();
            }
            catch
            { }
        }
        public void SetCellValueEvent(bool isFlag = true)
        {
            if(isFlag)
                this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            else
                this.dgvCompany.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
        }
        private void btnUpdate_Click(object sender, EventArgs e)
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

            try
            {
                DataGridViewRow selectRow = null;
                if (dgvCompany.SelectedRows.Count > 0)
                    selectRow = dgvCompany.SelectedRows[0];
                else if (dgvCompany.SelectedCells.Count > 0)
                    selectRow = dgvCompany.Rows[dgvCompany.SelectedCells[0].RowIndex];

                if (selectRow != null)
                {
                    AddCompanyInfo aci = new AddCompanyInfo(um, this, selectRow);
                    aci.ShowDialog();
                }
            }
            catch
            { }
        }
        private void btnSearching_Click(object sender, EventArgs e)
        {
            if (!tcMain.SelectedTab.Name.Equals("tabNotSendFax"))
            {
                //수정내용 확인
                if (dgvCompany.Rows.Count > 0)
                {
                    dgvCompany.EndEdit();
                    foreach (DataGridViewRow row in dgvCompany.Rows)
                    {
                        bool isChecked;
                        if (row.Cells["chk"].Value == null || !bool.TryParse(row.Cells["chk"].Value.ToString(), out isChecked))
                            isChecked = false;

                        if (isChecked)
                        {
                            if (messageBox.Show(this, "수정된 거래처 내역이 있습니다. 등록 후 조회하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                UpdateCompany(1, false);
                            this.Activate();
                            break;
                        }

                    }
                }

                GetData();
            }
            else
            {
                SearchingData();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnInsert_Click(object sender, EventArgs e)
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

            if (!pnAddCompany.Visible)
            {
                SetInputCompanyColumn();
                pnAddCompany.Visible = true;
                dgvInputCompany.Focus();
            }
            else
            {
                dgvInputCompany.AllowUserToAddRows = false;
                //유효성검사
                if (!isVailidation())
                {
                    dgvInputCompany.AllowUserToAddRows = true;
                    return;
                }
                //중복검사
                //foreach (DataGridViewRow row in dgvInputCompany.Rows)
                for (int i = dgvInputCompany.Rows.Count - 1; i >= 0; i--)
                {
                    DataGridViewRow row = dgvInputCompany.Rows[i];
                    CompanyModel model = new CompanyModel();
                    model.name = row.Cells["input_company"].Value.ToString();
                    model.registration_number = row.Cells["input_registration_number"].Value.ToString();
                    model.tel = row.Cells["input_tel"].Value.ToString();
                    model.fax = row.Cells["input_fax"].Value.ToString();
                    model.phone = row.Cells["input_phone"].Value.ToString();
                    model.other_phone = row.Cells["input_other_phone"].Value.ToString();
                    //중복검사
                    int notSendFax;
                    
                    if (!CheckDuplicateData(model, row, out notSendFax))
                    {
                        dgvInputCompany.ClearSelection();
                        row.Selected = true;


                        if (messageBox.Show(this, row.Cells["input_company"].Value.ToString() + " 거래처를 삭제후 진행하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            dgvInputCompany.Rows.Remove(row);
                        else
                        {
                            dgvInputCompany.AllowUserToAddRows = true;
                            return;
                        }
                    }
                    //팩스X 기록이 있으면 따라 팩스X처리
                    if (notSendFax > 0)
                        row.Cells["input_isNotSendFax"].Value = true;
                }
                //취급X 포함시
                List<DataGridViewRow> nonehandledList = new List<DataGridViewRow>();
                foreach (DataGridViewRow row in dgvInputCompany.Rows)
                {
                    if (row.Cells["duplicate_table_index"].Value != null && !string.IsNullOrEmpty(row.Cells["duplicate_table_index"].Value.ToString()))
                        nonehandledList.Add(row);
                }
                if (nonehandledList.Count > 0)
                {
                    NotHandlingCompanyList nhcl = new NotHandlingCompanyList(nonehandledList, this);
                    DataTable resultDt = nhcl.GetNoneHandledList();
                    foreach (DataRow dr in resultDt.Rows)
                    {
                        int rowindex = Convert.ToInt32(dr["rowindex"].ToString());

                        bool isIgnore;
                        if (!bool.TryParse(dr["isIgnore"].ToString(), out isIgnore))
                            isIgnore = false;

                        bool isRecovery;
                        if (!bool.TryParse(dr["isRecovery"].ToString(), out isRecovery))
                            isRecovery = false;

                        bool isDelete;
                        if (!bool.TryParse(dr["isDelete"].ToString(), out isDelete))
                            isDelete = false;

                        dgvInputCompany.Rows[rowindex].Cells["isIgnore"].Value = isIgnore;
                        dgvInputCompany.Rows[rowindex].Cells["isRecovery"].Value = isRecovery;
                        dgvInputCompany.Rows[rowindex].Cells["isDelete"].Value = isDelete;
                    }
                }

                //등록
                if (dgvInputCompany.Rows.Count == 0)
                {
                    dgvInputCompany.AllowUserToAddRows = true;
                    this.Activate();
                    return;
                }

                //등록할 거래처수
                int inputCnt = 0;
                foreach (DataGridViewRow row in dgvInputCompany.Rows)
                {
                    bool isDelete;
                    if (row.Cells["isDelete"].Value == null || !bool.TryParse(row.Cells["isDelete"].Value.ToString(), out isDelete))
                        isDelete = false;

                    if (!isDelete)
                        inputCnt++;
                }

                if (messageBox.Show(this, inputCnt + "개 거래처를 등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    dgvInputCompany.AllowUserToAddRows = true;
                    this.Activate();
                    return;
                }

                StringBuilder sql = new StringBuilder();
                List<StringBuilder> sqlList = new List<StringBuilder>();
                List<CompanyModel> modelList = new List<CompanyModel>();
                List<string> deleteList = new List<string>();
                //기존ID가 없을경우 신규ID
                int id = commonRepository.GetNextId("t_company", "id");
                foreach (DataGridViewRow row in dgvInputCompany.Rows)
                {
                    CompanyModel model = new CompanyModel();
                    model.id = id;
                    model.division = "매출처";
                    model.group_name = "";
                    model.name = row.Cells["input_company"].Value.ToString();
                    model.registration_number = row.Cells["input_registration_number"].Value.ToString();
                    model.ceo = row.Cells["input_ceo"].Value.ToString();
                    model.tel = row.Cells["input_tel"].Value.ToString();
                    model.fax = row.Cells["input_fax"].Value.ToString();
                    model.phone = row.Cells["input_phone"].Value.ToString();
                    model.other_phone = row.Cells["input_other_phone"].Value.ToString();
                    model.company_manager = row.Cells["input_company_manager"].Value.ToString();
                    model.company_manager_position = row.Cells["input_company_manager_position"].Value.ToString();
                    model.distribution = row.Cells["input_distribution"].Value.ToString();
                    model.handling_item = row.Cells["input_handling_item"].Value.ToString();
                    model.address = row.Cells["input_address"].Value.ToString();
                    model.origin = "국내";
                    model.email = row.Cells["input_email"].Value.ToString();
                    model.web = row.Cells["input_web"].Value.ToString();

                    model.remark2 = row.Cells["input_remark2"].Value.ToString();
                    model.remark3 = row.Cells["input_remark3"].Value.ToString();
                    model.remark4 = row.Cells["input_remark4"].Value.ToString();
                    model.remark5 = row.Cells["input_remark5"].Value.ToString();
                    model.remark6 = row.Cells["input_remark6"].Value.ToString();


                    bool input_isNotSendFax;
                    if (row.Cells["input_isNotSendFax"].Value == null || !bool.TryParse(row.Cells["input_isNotSendFax"].Value.ToString(), out input_isNotSendFax))
                        input_isNotSendFax = false;

                    model.isNotSendFax = input_isNotSendFax;
                    model.isHide = false;
                    model.isDelete = false;
                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    model.createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    model.industry_type = row.Cells["input_industry_type"].Value.ToString();
                    model.industry_type2 = row.Cells["input_industry_type2"].Value.ToString();

                    model.edit_user = um.user_name;

                    switch (tcMain.SelectedTab.Name)
                    {
                        //알람  -> 내Data와 동일
                        case "tabAlarm":
                            model.category = "내DATA";
                            model.ato_manager = um.user_name;
                            model.isPotential1 = false;
                            model.isPotential2 = false;
                            model.isNonHandled = false;
                            //model.isNotSendFax = false;
                            model.isOutBusiness = false;
                            break;
                        //공용DATA
                        case "tabCommonData":
                            model.category = "공용DATA";
                            model.ato_manager = "";
                            model.isPotential1 = false;
                            model.isPotential2 = false;
                            model.isNonHandled = false;
                            //model.isNotSendFax = false;
                            model.isOutBusiness = false;
                            break;
                        //전체  -> 내Data와 동일
                        case "tabAllMyCompany":
                            model.category = "내DATA";
                            model.ato_manager = um.user_name;
                            model.isPotential1 = false;
                            model.isPotential2 = false;
                            model.isNonHandled = false;
                            //model.isNotSendFax = false;
                            model.isOutBusiness = false;
                            break;
                        //내DATA
                        case "tabRamdomData":
                            model.category = "내DATA";
                            model.ato_manager = um.user_name;
                            model.isPotential1 = false;
                            model.isPotential2 = false;
                            model.isNonHandled = false;
                            //model.isNotSendFax = false;
                            model.isOutBusiness = false;
                            break;
                        //잠재1
                        case "tabCompany":
                            model.category = "잠재1";
                            model.ato_manager = um.user_name;
                            model.isPotential1 = true;
                            model.isPotential2 = false;
                            model.isNonHandled = false;
                            //model.isNotSendFax = false;
                            model.isOutBusiness = false;
                            break;
                        //잠재2
                        case "tabCompany2":
                            model.category = "잠재2";
                            model.ato_manager = um.user_name;
                            model.isPotential1 = false;
                            model.isPotential2 = true;
                            model.isNonHandled = false;
                            //model.isNotSendFax = false;
                            model.isOutBusiness = false;
                            break;
                        //SEAOVER
                        case "tabSeaover":
                            model.category = "거래중";
                            model.ato_manager = um.user_name;
                            model.isPotential1 = false;
                            model.isPotential2 = false;
                            model.isNonHandled = false;
                            model.isTrading = true;
                            //model.isNotSendFax = false;
                            model.isOutBusiness = false;
                            break;
                        //영업금지거래처
                        case "tabNotTreatment":
                            model.category = "취급X";
                            model.ato_manager = um.user_name;
                            model.isPotential1 = false;
                            model.isPotential2 = false;
                            model.isNonHandled = true;
                            //model.isNotSendFax = false;
                            model.isOutBusiness = false;
                            break;
                        case "tabNotSendFax":
                            model.ato_manager = um.user_name;
                            model.isNotSendFax = true;
                            model.isHide = true;
                            break;
                        //삭제거래처
                        case "tabDeleteCompany":
                            model.category = "거래처 삭제";
                            model.ato_manager = um.user_name;
                            model.isPotential1 = false;
                            model.isPotential2 = false;
                            model.isNonHandled = true;
                            //model.isNotSendFax = false;
                            model.isDelete = true;
                            model.isOutBusiness = false;
                            break;
                    }
                    //등록하지 않는 거래처
                    bool isDelete;
                    if (row.Cells["isDelete"].Value == null || !bool.TryParse(row.Cells["isDelete"].Value.ToString(), out isDelete))
                        isDelete = false;

                    //취급X 거래처 복구 경우
                    bool isRecovery;
                    if (row.Cells["isRecovery"].Value == null || !bool.TryParse(row.Cells["isRecovery"].Value.ToString(), out isRecovery))
                        isRecovery = false;
                    if (nonehandledList.Count > 0 && isRecovery)
                    {
                        modelList.Add(model);

                        string[] duplicate_ids = row.Cells["duplicate_table_index"].Value.ToString().Split(' ');
                        if (duplicate_ids.Length > 0)
                        {
                            foreach (string duplicate_id in duplicate_ids)
                            {
                                if (!string.IsNullOrEmpty(duplicate_id) && Int32.TryParse(duplicate_id, out Int32 table_index)
                                    && bool.TryParse(atoDt.Rows[table_index]["isNonHandled"].ToString(), out bool isNonHandled) && isNonHandled)
                                {
                                    //데이터 통합
                                    if (string.IsNullOrEmpty(model.ceo))
                                        model.ceo = atoDt.Rows[table_index]["ceo"].ToString();
                                    if (string.IsNullOrEmpty(model.registration_number))
                                        model.registration_number = atoDt.Rows[table_index]["registration_number"].ToString();
                                    if (string.IsNullOrEmpty(model.tel))
                                        model.tel = atoDt.Rows[table_index]["tel"].ToString();
                                    if (string.IsNullOrEmpty(model.fax))
                                        model.fax = atoDt.Rows[table_index]["fax"].ToString();
                                    if (string.IsNullOrEmpty(model.phone))
                                        model.phone = atoDt.Rows[table_index]["phone"].ToString();
                                    if (string.IsNullOrEmpty(model.other_phone))
                                        model.phone = atoDt.Rows[table_index]["other_phone"].ToString();
                                    if (string.IsNullOrEmpty(model.email))
                                        model.email = atoDt.Rows[table_index]["email"].ToString();
                                    if (string.IsNullOrEmpty(model.address))
                                        model.address = atoDt.Rows[table_index]["address"].ToString();
                                    if (string.IsNullOrEmpty(model.handling_item))
                                        model.handling_item = atoDt.Rows[table_index]["handling_item"].ToString();
                                    if (string.IsNullOrEmpty(model.distribution))
                                        model.distribution = atoDt.Rows[table_index]["distribution"].ToString();
                                    if (string.IsNullOrEmpty(model.payment_date))
                                        model.payment_date = atoDt.Rows[table_index]["payment_date"].ToString();
                                    if (string.IsNullOrEmpty(model.remark))
                                        model.remark = atoDt.Rows[table_index]["remark"].ToString();
                                    if (string.IsNullOrEmpty(model.industry_type))
                                        model.industry_type = atoDt.Rows[table_index]["industry_type"].ToString();

                                    //취급X 거래처 -> 삭제거래처
                                    deleteList.Add(table_index.ToString());

                                    //실제 서버삭제
                                    sql = companyRepository.DeleteCompany(Convert.ToInt32(atoDt.Rows[table_index]["id"].ToString()), true);
                                    sqlList.Add(sql);
                                }
                            }
                        }
                    }
                    else if (!isDelete)
                    {
                        modelList.Add(model);

                        sql = companyRepository.InsertCompany(model);
                        sqlList.Add(sql);

                        //거래처 영업내용============================================================================
                        string contents = "거래처 등록";
                        CompanySalesModel sModel = new CompanySalesModel();
                        sModel.company_id = id;
                        sModel.sub_id = commonRepository.GetNextId("t_company_sales", "sub_id", "company_id", id.ToString());
                        sModel.is_sales = false;
                        sModel.contents = contents;
                        sModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        sModel.edit_user = um.user_name;

                        sModel.pre_ato_manager = model.ato_manager;
                        sModel.from_ato_manager = model.ato_manager;
                        sModel.from_category = "";
                        sModel.to_category = model.category;

                        sql = salesPartnerRepository.InsertPartnerSales(sModel);
                        sqlList.Add(sql);
                        id++;
                    }
                }
                //===========================================================================================
                if (sqlList.Count > 0 && commonRepository.UpdateTran(sqlList) == -1)
                {
                    messageBox.Show(this,"등록중 에러가 발생했습니다.");
                    this.Activate();
                }
                else
                {
                    //신규거래처
                    int table_index = atoDt.Rows.Count;
                    foreach (CompanyModel model in modelList)
                    {
                        DataRow newDr = atoDt.NewRow();

                        newDr["id"] = model.id;
                        newDr["company"] = model.name;
                        newDr["tel"] = model.tel;
                        newDr["fax"] = model.fax;
                        newDr["phone"] = model.phone;
                        newDr["other_phone"] = model.other_phone;

                        /*newDr["pre_tel"] = model.tel;
                        newDr["pre_fax"] = model.fax;
                        newDr["pre_phone"] = model.phone;
                        newDr["pre_other_phone"] = model.other_phone;*/

                        newDr["email"] = model.email;

                        newDr["ceo"] = model.ceo;
                        newDr["registration_number"] = model.registration_number;
                        newDr["address"] = model.address;
                        newDr["origin"] = model.origin;
                        newDr["isHide"] = "FALSE";
                        newDr["createtime"] = model.createtime;
                        newDr["ato_manager"] = model.ato_manager;
                        newDr["pre_ato_manager"] = model.ato_manager;
                        newDr["category"] = model.category;
                        newDr["distribution"] = model.distribution;
                        newDr["handling_item"] = model.handling_item;
                        newDr["remark"] = model.remark;
                        newDr["isPotential1"] = model.isPotential1.ToString();
                        newDr["isPotential2"] = model.isPotential2.ToString();
                        newDr["isTrading"] = model.isTrading.ToString();
                        newDr["sales_updatetime"] = model.updatetime;
                        newDr["isNonHandled"] = model.isNonHandled.ToString();
                        newDr["isOutBusiness"] = model.isOutBusiness.ToString();
                        newDr["isNotSendFax"] = model.isNotSendFax.ToString();
                        newDr["division"] = "ATO";
                        newDr["isDelete"] = model.isDelete.ToString();
                        newDr["industry_type"] = model.industry_type.ToString();
                        newDr["isHide"] = model.isHide.ToString();
                        //newDr["table_div"] = "1";
                        newDr["table_index"] = table_index++;

                        newDr["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        newDr["sales_contents"] = "거래처 등록";

                        atoDt.Rows.Add(newDr);
                    }
                    //삭제거래처
                    foreach (string index in deleteList)
                    {
                        atoDt.Rows[Convert.ToInt32(index)]["isDelete"] = "TRUE";
                        atoDt.Rows[Convert.ToInt32(index)]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    messageBox.Show(this,"등록완료");
                    AddCompanyInfoRefresh();
                    dgvInputCompany.AllowUserToAddRows = true;
                    this.Activate();
                }
            }
        }
        private void cbSeaoverCode_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvCompany.Columns.Count > 0)
                dgvCompany.Columns["seaover_company_code"].Visible = cbSeaoverCode.Checked;
        }
        #endregion

        #region Key event
        private void txtNotSendFax_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetNotSendFaxData();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Shift))
            {
                if (dgvCompany.SelectedCells.Count > 0)
                {
                    List<int> rowIndexList = new List<int>();
                    foreach (DataGridViewCell cell in dgvCompany.SelectedCells)
                    {
                        if (!rowIndexList.Contains(cell.RowIndex))
                            rowIndexList.Add(cell.RowIndex);
                    }

                    dgvCompany.ClearSelection();
                    for (int i = 0; i < rowIndexList.Count; i++)
                        dgvCompany.Rows[rowIndexList[i]].Selected = true;
                }
                return true;

            }
            else if (keyData == (Keys.Control | Keys.C))
            {
                if (cbLimitCopy.Checked)
                {
                    if(dgvCompany.Focused && (dgvCompany.CurrentCell.OwningColumn.Name == "email" || dgvCompany.CurrentCell.OwningColumn.Name == "fax"))
                        return base.ProcessCmdKey(ref msg, keyData);
                    else
                        Clipboard.Clear();
                    return true;
                }
                else
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }
        private void txtGroupName_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetData();
            else if (e.KeyCode == Keys.Down)
                dgvCompany.Focus();

        }
        private void SalesManager_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearching.PerformClick();
                        break;
                    case Keys.R:
                        CompleteAlarm();
                        break;
                    case Keys.W:
                        btnDetailInfo.PerformClick();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                    case Keys.S:
                        btnUpdate.PerformClick();
                        break;
                    case Keys.D:
                        btnMultiUpdate.PerformClick();
                        break;
                    case Keys.A:
                        btnInsert.PerformClick();
                        break;
                    case Keys.M:
                        txtCompany.Focus();
                        break;
                    case Keys.N:
                        //txtGroupName.Text = String.Empty;
                        txtCompany.Text = String.Empty;
                        txtDistribution.Text = String.Empty;
                        txtHandlingItem.Text = String.Empty;
                        txtRemark4.Text = String.Empty;
                        txtCompany.Focus();
                        break;
                    case Keys.D1:
                        //잠재1 에서는 기능X
                        if (tcMain.SelectedTab.Name == "tabCompany")
                            return;
                        //선택한 내역 선택)
                        if (dgvCompany.SelectedRows.Count == 0 && dgvCompany.SelectedCells.Count == 0)
                        {
                            messageBox.Show(this, "거래처를 먼저 선택해주세요!");
                            this.Activate();
                        }
                        else if (dgvCompany.SelectedRows.Count == 0 && dgvCompany.SelectedCells.Count > 0)
                        {
                            int rowindex = dgvCompany.SelectedCells[0].RowIndex;
                            dgvCompany.ClearSelection();
                            dgvCompany.Rows[rowindex].Selected = true;
                        }

                        if (messageBox.Show(this, "잠재1로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                            return;

                        if (UpdateCompanyInfo(4))
                        {
                            for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                            {
                                if (dgvCompany.Rows[i].Selected)
                                    dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                            }
                        }
                        break;
                    case Keys.D2:
                        //잠재2 에서는 기능X
                        if (tcMain.SelectedTab.Name == "tabCompany2")
                            return;
                        //선택한 내역 선택
                        if (dgvCompany.SelectedRows.Count == 0 && dgvCompany.SelectedCells.Count == 0)
                        {
                            messageBox.Show(this, "거래처를 먼저 선택해주세요!");
                            this.Activate();
                        }
                        else if (dgvCompany.SelectedRows.Count == 0 && dgvCompany.SelectedCells.Count > 0)
                        {
                            int rowindex = dgvCompany.SelectedCells[0].RowIndex;    
                            dgvCompany.ClearSelection();
                            dgvCompany.Rows[rowindex].Selected = true;
                        }

                        if (messageBox.Show(this,"잠재2로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                            return;

                        if (UpdateCompanyInfo(5))
                        {
                            for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                            {
                                if (dgvCompany.Rows[i].Selected)
                                    dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                            }
                        }
                        break;
                    case Keys.D3:
                        //선택한 내역 선택
                        if (dgvCompany.SelectedRows.Count == 0 && dgvCompany.SelectedCells.Count == 0)
                        {
                            messageBox.Show(this, "거래처를 먼저 선택해주세요!");
                            this.Activate();
                        }
                        else if (dgvCompany.SelectedRows.Count == 0 && dgvCompany.SelectedCells.Count > 0)
                        {
                            int rowindex = dgvCompany.SelectedCells[0].RowIndex;
                            dgvCompany.ClearSelection();
                            dgvCompany.Rows[rowindex].Selected = true;
                        }

                        if (messageBox.Show(this, "[영업금지 거래처] 취급X 거래처로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                            return;
                        if (UpdateCompanyInfo(6))
                        {
                            for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                            {
                                if (Convert.ToBoolean(dgvCompany.Rows[i].Selected))
                                    dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                            }
                        }
                        break;

                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.F:
                        try
                        {
                            TxtFindManger tfm = new TxtFindManger(um, this);
                            tfm.Show();
                        }
                        catch
                        { }
                        break;
                    case Keys.O:
                        btnMyDailyBusiness.PerformClick();
                        break;
                    case Keys.D1:
                        btnMyDailyBusiness.PerformClick();
                        break;
                    case Keys.Z:
                        //dgvCompany.Undo();
                        break;
                    case Keys.Y:
                        //dgvCompany.Redo();
                        break;
                    case Keys.D:
                        string tempData = "";
                        int pre_col_idx = -1;
                        for (int i = 0; i < dgvCompany.ColumnCount; i++)
                        {
                            for (int j = 0; j < dgvCompany.Rows.Count; j++)
                            {
                                if (dgvCompany.Rows[j].Cells[i].Selected)
                                {
                                    if (dgvCompany.Rows[j].Cells[i].Value == null)
                                        dgvCompany.Rows[j].Cells[i].Value = string.Empty;

                                    if (pre_col_idx != i)
                                    {
                                        tempData = dgvCompany.Rows[j].Cells[i].Value.ToString();
                                        pre_col_idx = i;
                                    }
                                    dgvCompany.Rows[j].Cells[i].Value = tempData;
                                }
                            }
                        }
                        break;
                }
            }
            else
            {
                try
                {
                    switch (e.KeyCode)
                    {
                        case Keys.F1:
                            tcMain.SelectTab("tabAlarm");
                            break;
                        case Keys.F3:
                            tcMain.SelectTab("tabCommonData");
                            break;
                        case Keys.F4:
                            tcMain.SelectTab("tabAllMyCompany");
                            break;
                        case Keys.F5:
                            tcMain.SelectTab("tabRamdomData");
                            break;
                        case Keys.F6:
                            tcMain.SelectTab("tabCompany");
                            break;
                        case Keys.F7:
                            tcMain.SelectTab("tabCompany2");
                            break;
                        case Keys.F8:
                            tcMain.SelectTab("tabSeaover");
                            break;
                        case Keys.F9:
                            tcMain.SelectTab("tabNotTreatment");
                            break;
                        case Keys.F10:
                            tcMain.SelectTab("tabNotSendFax");
                            break;
                        case Keys.F11:
                            tcMain.SelectTab("tabDeleteCompany");
                            break;

                        case Keys.Escape:
                            if (pnAddCompany.Visible)
                                pnAddCompany.Visible = false;
                            if (pnDetail.Visible)
                                pnDetail.Visible = false;
                            break;
                    }
                }
                catch
                { }
            }
        }
        #endregion

        #region 우클릭 메뉴
        ContextMenuStrip m;
        private void dgvCompany_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    if (dgvCompany.SelectedRows.Count == 0)
                        return;
                    //한영변환
                    ChangeIME(false);

                    hitTestInfo = dgvCompany.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    m = new ContextMenuStrip();

                    if (tcMain.SelectedTab.Name != "tabNotSendFax")
                    {
                        m.Items.Add("거래처 정보수정(S)");
                        if (tcMain.SelectedTab.Name == "tabDeleteCompany")
                        {
                            m.Items.Add("거래처 복구");
                            if (um.department == "관리부" || um.department == "전산부")
                                m.Items.Add("거래처 영구삭제");
                        }
                        else
                            m.Items.Add("거래처 삭제");

                        m.Items.Add("담당자 변경");
                        ToolStripSeparator toolStripSeparator0 = new ToolStripSeparator();
                        toolStripSeparator0.Name = "toolStripSeparator";
                        toolStripSeparator0.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator0);
                        m.Items.Add("SEAOVER 거래처 복구");
                        ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
                        toolStripSeparator1.Name = "toolStripSeparator";
                        toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator1);

                        /*if (tcMain.SelectedTab.Name != "tabCommonData")
                            m.Items.Add("공용DATA 전환");*/

                        if (tcMain.SelectedTab.Name != "tabRamdomData")
                            m.Items.Add("내DATA 전환");

                        if (tcMain.SelectedTab.Name != "tabCompany")
                            m.Items.Add("잠재1 전환 (1)");

                        if (tcMain.SelectedTab.Name != "tabCompany2")
                            m.Items.Add("잠재2 전환 (2)");

                        /*if (tcMain.SelectedTab.Name != "tabSeaover")
                            m.Items.Add("거래중 전환");*/


                        ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
                        toolStripSeparator2.Name = "toolStripSeparator";
                        toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator2);
                        m.Items.Add("취급X 전환 (3)");
                        //m.Items.Add("팩스X 전환");
                        m.Items.Add("폐업 전환");
                    }

                    if (tcMain.SelectedTab.Name == "tabSeaover")
                    {
                        ToolStripSeparator toolStripSeparator3 = new ToolStripSeparator();
                        toolStripSeparator3.Name = "toolStripSeparator";
                        toolStripSeparator3.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator3);
                        m.Items.Add("취급품목 관리");
                    }
                    if (tcMain.SelectedTab.Name == "tabCommonData")
                    {
                        ToolStripSeparator toolStripSeparator4 = new ToolStripSeparator();
                        toolStripSeparator4.Name = "toolStripSeparator3";
                        toolStripSeparator4.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator4);
                        m.Items.Add("거래처 분배");
                    }

                    if (Convert.ToInt32(dgvCompany.Rows[row].Cells["main_id"].Value) > 0)
                    {
                        ToolStripSeparator toolStripSeparator4 = new ToolStripSeparator();
                        toolStripSeparator4.Name = "toolStripSeparator3";
                        toolStripSeparator4.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator4);
                        m.Items.Add("대표 거래처 취소");
                    }
                    else if (dgvCompany.SelectedRows.Count > 1)
                    {
                        ToolStripSeparator toolStripSeparator4 = new ToolStripSeparator();
                        toolStripSeparator4.Name = "toolStripSeparator3";
                        toolStripSeparator4.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator4);
                        m.Items.Add("대표 거래처 설정");
                    }

                    if (tcMain.SelectedTab.Name == "tabAlarm")
                    {
                        ToolStripSeparator toolStripSeparator5 = new ToolStripSeparator();
                        toolStripSeparator5.Name = "toolStripSeparator3";
                        toolStripSeparator5.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator5);
                        m.Items.Add("알람 완료처리(R)");
                        m.Items.Add("알람 취소처리");
                        m.Items.Add("알람 내역삭제");

                    }
                    else
                    {
                        ToolStripSeparator toolStripSeparator5 = new ToolStripSeparator();
                        toolStripSeparator5.Name = "toolStripSeparator3";
                        toolStripSeparator5.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator5);
                        m.Items.Add("영업알림 설정");
                    }
                    





                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    m.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cms_PreviewKeyDown);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(dgvCompany, e.Location);
                    //Selection
                    /*PendingList.ClearSelection();
                    DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                    selectRow.Selected = !selectRow.Selected;*/
                }
            }
            catch (Exception ex)
            {
                messageBox.Show(this, ex.Message);
                this.Activate();
            }
        }
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (dgvCompany.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow dr = dgvCompany.SelectedRows[0];
                    if (dr.Index < 0)
                        return;
                    int rowindex = dr.Index;

                    //함수호출
                    switch (e.ClickedItem.Text)
                    {
                        case "거래처 정보수정(S)":
                            DataGridViewRow selectRow = null;
                            if (dgvCompany.SelectedRows.Count > 0)
                                selectRow = dgvCompany.SelectedRows[0];
                            else if (dgvCompany.SelectedCells.Count > 0)
                                selectRow = dgvCompany.Rows[dgvCompany.SelectedCells[0].RowIndex];
                            if (selectRow != null)
                            {
                                AddCompanyInfo aci = new AddCompanyInfo(um, this, selectRow);
                                aci.ShowDialog();
                            }
                            break;
                        case "공용DATA 전환":

                            if (messageBox.Show(this, "모든 담당자가 확인할 수 있는 공용DATA로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;

                            if (UpdateCompanyInfo(2))
                            {
                                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (Convert.ToBoolean(dgvCompany.Rows[i].Selected))
                                        dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                }
                            }
                            break;
                        case "내DATA 전환":
                            if (messageBox.Show(this, um.user_name + "님만 확인할 수 있는 내DATA로 전환됩니다. 계속하시겠습니까?", "YesOrNo" , MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;

                            if (UpdateCompanyInfo(3))
                            {
                                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (Convert.ToBoolean(dgvCompany.Rows[i].Selected))
                                        dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                }
                            }
                            break;
                        case "잠재1 전환 (1)":

                            if (messageBox.Show(this, "잠재1로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;

                            if (UpdateCompanyInfo(4))
                            {
                                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (Convert.ToBoolean(dgvCompany.Rows[i].Selected))
                                        dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                }
                            }
                            break;
                        case "잠재2 전환 (2)":
                            if (messageBox.Show(this, "잠재2로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;
                            if (UpdateCompanyInfo(5))
                            {
                                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (Convert.ToBoolean(dgvCompany.Rows[i].Selected))
                                        dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                }
                            }
                            break;
                        case "거래중 전환":
                            if (messageBox.Show(this, "SEAOVER 거래처중에서만 거래중(SEAOVER) 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;
                            if (UpdateCompanyInfo(9))
                            {
                                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (Convert.ToBoolean(dgvCompany.Rows[i].Selected))
                                        dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                }
                            }
                            break;
                        case "취급X 전환 (3)":
                            if (messageBox.Show(this, "[영업금지 거래처] 취급X 거래처로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;
                            if (UpdateCompanyInfo(6))
                            {
                                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (Convert.ToBoolean(dgvCompany.Rows[i].Selected))
                                        dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                }
                            }
                            break;
                        case "팩스X 전환":
                            if (messageBox.Show(this, "[영업금지 거래처] 팩스X 거래처로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;
                            if (UpdateCompanyInfo(7))
                            {
                                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (Convert.ToBoolean(dgvCompany.Rows[i].Selected))
                                        dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                }
                            }
                            break;
                        case "폐업 전환":
                            if (messageBox.Show(this, "[영업금지 거래처] 폐업 거래처로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;
                            if (UpdateCompanyInfo(8))
                            {
                                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (Convert.ToBoolean(dgvCompany.Rows[i].Selected))
                                        dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                }
                            }
                            break;
                        case "취급품목 관리":
                            {
                                try
                                {
                                    SaleProductBySeaover spb = new SaleProductBySeaover(um, dr.Cells["seaover_company_code"].Value.ToString(), dr.Cells["company"].Value.ToString());
                                    spb.Show();
                                }
                                catch
                                { }
                            }
                            break;
                        case "거래처 삭제":
                            {
                                if (messageBox.Show(this, "거래처 정보를 삭제하시겠습니까?\n * '삭제거래처' 탭에서 확인할 수 있습니다.", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                    return;
                                if (DeleteCompanyInfo())
                                {
                                    for (int i = dgvCompany.RowCount - 1; i >= 0; i--)
                                    {
                                        DataGridViewRow row = dgvCompany.Rows[i];
                                        if (Convert.ToBoolean(row.Selected))
                                            dgvCompany.Rows.Remove(row);

                                    }
                                }
                            }
                            break;
                        case "거래처 복구":
                            {
                                if (messageBox.Show(this, "거래처 정보를 복구하시겠습니까?\n * 삭제 이전 위치로 다시 되돌아갑니다.", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                    return;
                                if (DeleteCompanyInfo(false))
                                {
                                    for (int i = dgvCompany.RowCount - 1; i >= 0; i--)
                                    {
                                        DataGridViewRow row = dgvCompany.Rows[i];
                                        if (Convert.ToBoolean(row.Selected))
                                            dgvCompany.Rows.Remove(row);

                                    }
                                }
                            }
                            break;
                        case "거래처 분배":
                            {
                                if (dgvCompany.SelectedRows.Count > 0)
                                {
                                    if (messageBox.Show(this, "선택한 거래처를 분배하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                        return;
                                    DataTable companyDt = common.ConvertSelectListToDatatable(dgvCompany);
                                    if (companyDt != null && companyDt.Rows.Count > 0)
                                    {
                                        DataDistribution dd = new DataDistribution(um, companyDt, this);
                                        try
                                        {
                                            dd.ShowDialog();
                                            //dd.Start();

                                            GetData(false, false);
                                        }
                                        catch
                                        { }
                                    }
                                }
                                else
                                    messageBox.Show(this, "거래처를 먼저 선택해주세요.");
                                this.Activate();
                            }
                            break;
                        case "대표 거래처 설정":
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
                                        CompanyGroupManager cgm = new CompanyGroupManager(um, this, list);
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
                        case "대표 거래처 취소":
                            {
                                if (messageBox.Show(this, "선택한 거래처의 그룹설정을 취소하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
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
                                    for (int i = 0; i < mainDt.Rows.Count; i++)
                                    {
                                        int main_id = Convert.ToInt32(mainDt.Rows[i]["main_id"].ToString());
                                        for (int j = 0; j < delete_main_id.Count; j++)
                                        {
                                            if (main_id == delete_main_id[j])
                                            {
                                                //기존 
                                                int rowIndex = Convert.ToInt32(mainDt.Rows[i]["table_index"].ToString());
                                                atoDt.Rows[rowIndex]["main_id"] = 0;
                                                atoDt.Rows[rowIndex]["sub_id"] = 0;
                                            }
                                        }
                                    }

                                    for (int i = 0; i < subDt.Rows.Count; i++)
                                    {
                                        int main_id = Convert.ToInt32(subDt.Rows[i]["main_id"].ToString());
                                        for (int j = 0; j < delete_main_id.Count; j++)
                                        {
                                            if (main_id == delete_main_id[j])
                                            {
                                                //기존 
                                                int rowIndex = Convert.ToInt32(subDt.Rows[i]["table_index"].ToString());
                                                atoDt.Rows[rowIndex]["main_id"] = 0;
                                                atoDt.Rows[rowIndex]["sub_id"] = 0;
                                            }
                                        }
                                    }
                                    atoDt.AcceptChanges();
                                    GetData(false, false);
                                    messageBox.Show(this, "취소 완료");
                                    this.Activate();
                                }
                            }
                            break;
                        case "영업알림 설정":
                            if (dgvCompany.SelectedRows.Count > 0)
                            {
                                AlarmSettingManager asm = new AlarmSettingManager(um, this);
                                asm.ShowDialog();
                            }
                            break;
                        case "알람 완료처리(R)":
                            CompleteAlarm();
                            break;
                        case "알람 취소처리":
                            CancelAlarm();
                            break;
                        case "거래처 영구삭제":
                            {
                                if (messageBox.Show(this, "거래처 정보를 영구적으로 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                    return;
                                if (RealDeleteCompanyInfo())
                                {
                                    for (int i = dgvCompany.RowCount - 1; i >= 0; i--)
                                    {
                                        DataGridViewRow row = dgvCompany.Rows[i];
                                        if (row.Selected)
                                            dgvCompany.Rows.Remove(row);

                                    }
                                }
                            }
                            break;
                        case "SEAOVER 거래처 복구":
                            {
                                if (messageBox.Show(this, "SEAOVER 거래처일 경우만 'SEAOVER (F7)' 탭으로 이동합니다.", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                    return;
                                List<StringBuilder> sqlList = new List<StringBuilder>();
                                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (dgvCompany.Rows[i].Selected && dgvCompany.Rows[i].Cells["seaover_company_code"].Value != null 
                                        && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["seaover_company_code"].Value.ToString()))
                                    {
                                        StringBuilder sql = commonRepository.UpdateData("t_company"
                                            , $"isTrading = TRUE, isDelete = FALSE, updatetime = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', edit_user = '{um.user_name}'"
                                            , $"id = {dgvCompany.Rows[i].Cells["id"].Value.ToString()}");
                                        sqlList.Add(sql);
                                    }
                                }
                                if (sqlList.Count > 0)
                                {
                                    if (commonRepository.UpdateTran(sqlList) == -1)
                                    {
                                        messageBox.Show(this, "수정중 에러가 발생하였습니다.");
                                        this.Activate();
                                    }
                                    else
                                    {
                                        for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                        {
                                            if (dgvCompany.Rows[i].Selected 
                                                && dgvCompany.Rows[i].Cells["seaover_company_code"].Value != null 
                                                && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["seaover_company_code"].Value.ToString()))
                                            {
                                                int table_index = Convert.ToInt32(dgvCompany.Rows[i].Cells["table_index"].Value.ToString());
                                                atoDt.Rows[table_index]["isTrading"] = "TRUE";
                                                atoDt.Rows[table_index]["isDelete"] = "FALSE";
                                                atoDt.Rows[table_index]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                                dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                            }
                                        }
                                    } 
                                }
                            }
                            break;
                        case "담당자 변경":

                            List<DataGridViewRow> rowList = new List<DataGridViewRow>();
                            foreach (DataGridViewRow row in dgvCompany.SelectedRows)
                                rowList.Add(row);

                            if (rowList.Count > 0)
                            {
                                ManagerChanger mc = new ManagerChanger(um, this, rowList);
                                mc.Owner = this;
                                mc.ShowDialog();
                            }

                            break;
                        case "알람 내역삭제":

                            {

                                if (messageBox.Show(this, "선택한 거래처의 알람내역을 전부 삭제하시겠습니까?.", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                    return;

                                List<StringBuilder> sqlList = new List<StringBuilder>();
                                foreach (DataGridViewRow row in dgvCompany.SelectedRows)
                                {
                                    //거래처 알람정보
                                    StringBuilder sql = companyAlarmRepository.DeleteAlarm(row.Cells["id"].Value.ToString().ToString());
                                    sqlList.Add(sql);
                                }

                                if (commonRepository.UpdateTran(sqlList) == -1)
                                    messageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                                else
                                {
                                    for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                    {
                                        if (dgvCompany.Rows[i].Selected)
                                        {
                                            int table_index = Convert.ToInt32(dgvCompany.Rows[i].Cells["table_index"].Value.ToString());
                                            atoDt.Rows[table_index]["alarm_date"] = "";
                                            atoDt.Rows[table_index]["alarm_complete_date"] = "";
                                            atoDt.Rows[table_index]["alarm_week"] = "";
                                            atoDt.Rows[table_index]["alarm_month"] = "";

                                            dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                        }
                                    }
                                }
                            }

                            break;
                        default:
                            break;
                            
                    }
                }
                catch
                { }
            }
        }
        private void cms_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                //우클릭 단축키
                case Keys.S:
                    if (dgvCompany.SelectedRows.Count > 0)
                        m.Items[0].PerformClick();
                    break;
                case Keys.D1:
                    //잠재1 에서는 기능X
                    if (tcMain.SelectedTab.Name == "tabCompany")
                        return;
                    //선택한 내역 선택)
                    if (dgvCompany.SelectedRows.Count == 0 && dgvCompany.SelectedCells.Count == 0)
                    {
                        messageBox.Show(this, "거래처를 먼저 선택해주세요!");
                        this.Activate();
                    }
                    else if (dgvCompany.SelectedRows.Count == 0 && dgvCompany.SelectedCells.Count > 0)
                    {
                        int rowindex = dgvCompany.SelectedCells[0].RowIndex;
                        dgvCompany.ClearSelection();
                        dgvCompany.Rows[rowindex].Selected = true;
                    }

                    if (messageBox.Show(this,"잠재1로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;

                    if (UpdateCompanyInfo(4))
                    {
                        for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                        {
                            if (dgvCompany.Rows[i].Selected)
                                dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                        }
                    }
                    break;
                case Keys.D2:
                    //잠재1 에서는 기능X
                    if (tcMain.SelectedTab.Name == "tabCompany")
                        return;
                    //선택한 내역 선택)
                    if (dgvCompany.SelectedRows.Count == 0 && dgvCompany.SelectedCells.Count == 0)
                    {
                        messageBox.Show(this, "거래처를 먼저 선택해주세요!");
                        this.Activate();
                    }
                    else if (dgvCompany.SelectedRows.Count == 0 && dgvCompany.SelectedCells.Count > 0)
                    {
                        int rowindex = dgvCompany.SelectedCells[0].RowIndex;
                        dgvCompany.ClearSelection();
                        dgvCompany.Rows[rowindex].Selected = true;
                    }

                    if (messageBox.Show(this,"잠재2로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;

                    if (UpdateCompanyInfo(5))
                    {
                        for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                        {
                            if (dgvCompany.Rows[i].Selected)
                                dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                        }
                    }
                    break;
                case Keys.D3:
                    if (messageBox.Show(this, "[영업금지 거래처] 취급X 거래처로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;
                    if (UpdateCompanyInfo(6))
                    {
                        for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                        {
                            if (Convert.ToBoolean(dgvCompany.Rows[i].Selected))
                                dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                        }
                    }
                    break;
                case Keys.R:
                    CompleteAlarm();
                    break;
            }
        }
        private void dgvCompany_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && dgvCompany.SelectedRows.Count <= 1)
                {
                    dgvCompany.ClearSelection();
                    dgvCompany.Rows[e.RowIndex].Selected = true;
                }
            }
        }
        #endregion

        #region Datagrieveiw, tabControl event
        private void dgvNotSendFax_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
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


                string fax = dgvNotSendFax.Rows[e.RowIndex].Cells["fax_number"].Value.ToString();
                if (messageBox.Show(this, "[" + fax + "]을 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    List<StringBuilder> sqlList = new List<StringBuilder>();
                    StringBuilder sql = notSendFaxRepository.DeleteCompany(fax);
                    sqlList.Add(sql);

                    if (commonRepository.UpdateTran(sqlList) == -1)
                        messageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                    else
                        dgvCompany.Rows.Remove(dgvCompany.Rows[e.RowIndex]);
                }
            }
        }
        private void dgvCompany_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //MessageBox.Show(this,"입력값이 잘못되었습니다. 다시 확인해주세요.");
            e.Cancel = false;
            e.ThrowException = false;
        }
        private void dgvCompany_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int main_id;
                if (dgvCompany.Rows[e.RowIndex].Cells["main_id"].Value == null || !int.TryParse(dgvCompany.Rows[e.RowIndex].Cells["main_id"].Value.ToString(), out main_id))
                    main_id = 0;
                int sub_id;
                if (dgvCompany.Rows[e.RowIndex].Cells["sub_id"].Value == null || !int.TryParse(dgvCompany.Rows[e.RowIndex].Cells["sub_id"].Value.ToString(), out sub_id))
                    sub_id = 0;

                //이전 출력 서브거래처 삭제
                bool isExist = false;
                for (int i = mainDt.Rows.Count - 1; i >= 0; i--)
                {
                    int sub_main_id;
                    if (!int.TryParse(mainDt.Rows[i]["main_id"].ToString(), out sub_main_id))
                        sub_main_id = 0;
                    int sub_sub_id;
                    if (!int.TryParse(mainDt.Rows[i]["sub_id"].ToString(), out sub_sub_id))
                        sub_sub_id = 0;

                    if (main_id == sub_main_id && sub_sub_id == 1)
                    {
                        mainDt.Rows.Remove(mainDt.Rows[i]);
                        isExist = true;
                    }
                }

                //대표 거래처일 경우 만
                if (main_id > 0 && sub_id == 0 && !isExist)
                {
                    for (int i = subDt.Rows.Count - 1; i >= 0; i--)
                    {
                        int sub_main_id;
                        if (!int.TryParse(subDt.Rows[i]["main_id"].ToString(), out sub_main_id))
                            sub_main_id = 0;

                        if (main_id == sub_main_id)
                        {
                            DataRow subDr = mainDt.NewRow();

                            for (int j = 0; j < subDt.Columns.Count; j++)
                                subDr[j] = subDt.Rows[i][j];

                            mainDt.Rows.InsertAt(subDr, e.RowIndex + 1);
                        }
                    }
                    SetGroupCompany();
                }
            }
        }
        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            //수정내용 확인
            if (dgvCompany.Rows.Count > 0)
            {
                dgvCompany.EndEdit();
                foreach (DataGridViewRow row in dgvCompany.Rows)
                {
                    bool isChecked;
                    if (row.Cells["chk"].Value == null || !bool.TryParse(row.Cells["chk"].Value.ToString(), out isChecked))
                        isChecked = false;

                    if (isChecked)
                    {
                        if (messageBox.Show(this,"수정된 거래처 내역이 있습니다. 등록 후 조회하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            UpdateCompany(1, false);
                        this.Activate();
                        break;
                    }

                }
            }

            if (!tcMain.SelectedTab.Name.Equals("tabNotSendFax"))
            {
                tcMain.SelectedTab.Controls.Add(this.dgvCompany);
                tcMain.SelectedTab.Controls.Add(this.pnAddCompany);
                tcMain.SelectedTab.Controls.Add(this.pnRecord);

                dgvCompany.DataSource = mainDt;
                GetData(false, false);
                dgvCompany.SetStackIdx(tcMain.SelectedIndex);

                switch (tcMain.SelectedTab.Name)
                {
                    case "tabRamdomData":
                        CheckSeaoverCompany();
                        break;
                    case "tabCompany":
                        CheckSeaoverCompany();
                        break;
                    case "tabCompany2":
                        CheckSeaoverCompany();
                        break;
                }
            }
            else
                GetNotSendFaxData();
            this.Update();
        }

        private void dgvCompany_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 1)
            {
                this.dgvCompany.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
                dgvCompany.Rows[e.RowIndex].Cells["chk"].Value = true;
                dgvCompany.EndEdit();
                this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);

                if (dgvCompany.Columns[e.ColumnIndex].Name == "tel"
                    || dgvCompany.Columns[e.ColumnIndex].Name == "fax"
                    || dgvCompany.Columns[e.ColumnIndex].Name == "phone"
                    || dgvCompany.Columns[e.ColumnIndex].Name == "other_phone")
                    dgvCompany.Rows[e.RowIndex].Cells["pre_" + dgvCompany.Columns[e.ColumnIndex].Name].Value = dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            }
        }
        private void dgvCompany_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 1)
            {
                //dgvCompany.Rows[e.RowIndex].Cells["chk"].Value = true;
                dgvCompany.EndEdit();
                if (dgvCompany.Columns[e.ColumnIndex].Name == "status_absence")
                {
                    bool isCheck = Convert.ToBoolean(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

                    dgvCompany.Rows[e.RowIndex].Cells["status_sales"].Value = !isCheck;
                    dgvCompany.Rows[e.RowIndex].Cells["chk"].Value = true;
                }
                else if (dgvCompany.Columns[e.ColumnIndex].Name == "status_sales")
                {
                    bool isCheck = Convert.ToBoolean(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

                    dgvCompany.Rows[e.RowIndex].Cells["status_absence"].Value = !isCheck;
                    dgvCompany.Rows[e.RowIndex].Cells["chk"].Value = true;
                }
                else if (dgvCompany.Columns[e.ColumnIndex].Name == "btn_alarm_complete")
                {
                    //알람 처리여부
                    DateTime alarm_standard_date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                    DateTime alarm_standard_sttdate = DateTime.Now.AddDays(-1);
                    DateTime alarm_standard_enddate = DateTime.Now;

                    //알람일 경우 처리할건지 확인
                    bool isAlarmComplete = false;
                    DataGridViewRow row = dgvCompany.Rows[e.RowIndex];
                    bool isComplete = true;
                    DateTime complete_date;
                    if (row.Cells["alarm_complete_date"].Value == null || !DateTime.TryParse(row.Cells["alarm_complete_date"].Value.ToString(), out complete_date))
                        complete_date = new DateTime(1900, 1, 1);


                    if (row.Cells["alarm_date"].Value != null && !string.IsNullOrEmpty(row.Cells["alarm_date"].Value.ToString()))
                    {
                        string[] alarms = row.Cells["alarm_date"].Value.ToString().Split('\n');
                        foreach (string alarm in alarms)
                        {
                            if (!string.IsNullOrEmpty(alarm.Trim()))
                            {
                                string[] alarm_detail = alarm.Split('_');
                                if (DateTime.TryParse(alarm_detail[0], out DateTime alarmDt)
                                    && Convert.ToDateTime(alarmDt.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(alarm_standard_sttdate.ToString("yyyy-MM-dd"))
                                    && Convert.ToDateTime(alarmDt.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(alarm_standard_enddate.ToString("yyyy-MM-dd"))
                                    && Convert.ToDateTime(alarmDt.ToString("yyyy-MM-dd")) > Convert.ToDateTime(complete_date.ToString("yyyy-MM-dd")))
                                {
                                    isComplete = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (!isComplete)
                    {
                        if (messageBox.Show(this, "알람 완료처리 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        {
                            this.Activate();
                            return;
                        }

                        StringBuilder sql = commonRepository.UpdateData("t_company", $"alarm_complete_date = '{alarm_standard_date.ToString("yyyy-MM-dd")}'" +
                                                                                  $", updatetime = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                                                                                  $", updatetime = '{um.user_name}'"
                                                                    , $"id = {row.Cells["id"].Value.ToString()}");
                        List<StringBuilder> sqlList = new List<StringBuilder>();
                        sqlList.Add(sql);

                        if (commonRepository.UpdateTran(sqlList) == -1)
                        {
                            messageBox.Show(this, "등록중 에러가 발생했습니다.");
                            this.Activate();
                        }
                        else
                        {
                            int table_index = Convert.ToInt32(row.Cells["table_index"].Value.ToString());
                            atoDt.Rows[table_index]["alarm_complete_date"] = alarm_standard_date;
                            atoDt.Rows[table_index]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            row.DefaultCellStyle.ForeColor = Color.LightGray;
                        }
                        dgvCompany.EndEdit();
                    }
                    else
                    {
                        messageBox.Show(this, "알람 처리가 이미 완료된 거래처이거나 처리할 수 없는 거래처입니다.");
                        return;
                    }
                }   
            }
        }
        private void dgvCompany_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewColumn col = dgvCompany.Columns[e.ColumnIndex];
                //체크여부
                if (col.Name == "chk")
                {
                    bool isChecked = Convert.ToBoolean(dgvCompany.Rows[e.RowIndex].Cells["chk"].Value);

                    if (isChecked)
                        dgvCompany.Rows[e.RowIndex].Cells["chk"].Style.BackColor = Color.Red;
                    else
                        dgvCompany.Rows[e.RowIndex].Cells["chk"].Style.BackColor = Color.White;
                }
                else if (col.Name.Contains("div"))
                {
                    dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Gray;
                }
                else if (col.Name == "current_sale_date" || col.Name == "current_sale_date")
                {
                    DateTime dt;
                    if (dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && DateTime.TryParse(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out dt))
                    {
                        TimeSpan dateDiff = DateTime.Now - dt;
                        double diffMonth = dateDiff.Days / 30;

                        if (diffMonth > 3)
                        {
                            dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Blue;
                            dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font("나눔고딕", 11, FontStyle.Bold);
                        }
                        else if (diffMonth > 2)
                        {
                            dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.DeepSkyBlue;
                            dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font("나눔고딕", 11, FontStyle.Bold);
                        }
                        else if (diffMonth > 1)
                        {
                            dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.LightBlue;
                            dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font("나눔고딕", 11, FontStyle.Bold);
                        }
                        else
                        {
                            dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                            dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font("나눔고딕", 11, FontStyle.Regular);
                        }
                    }
                }
                else if (col.Name == "industry_type_rank" 
                    && (dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null 
                    || dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "99"))
                {
                    dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.White;
                }
            }
        }
        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStatus.Text != string.Empty && cbStatus.Text != "전체")
            {
                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                        dgvCompany.Rows[i].Visible = true;

                CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dgvCompany.DataSource];
                currencyManager1.SuspendBinding();
                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                {

                    if (dgvCompany.Rows[i].Cells["remark4"].Value == null || !dgvCompany.Rows[i].Cells["remark4"].Value.ToString().Contains(cbStatus.Text))
                        dgvCompany.Rows[i].Visible = false;
                }
                currencyManager1.ResumeBinding();

            }
        }
        private void dgvCompany_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCompany.SelectedCells.Count > 0)
                txtCurrentRecord.Text = (dgvCompany.SelectedCells[0].RowIndex + 1).ToString("#,##0");


            txtSelectionCount.Text = dgvCompany.SelectedRows.Count.ToString("#,##0");
        }
        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvData.Columns[e.ColumnIndex].Name.Equals("is_not_send_fax"))
                {
                    bool isChecked;
                    if (dgvData.Rows[e.RowIndex].Cells["is_not_send_fax"].Value == null || !bool.TryParse(dgvData.Rows[e.RowIndex].Cells["is_not_send_fax"].Value.ToString(), out isChecked))
                        isChecked = false;

                    if (isChecked)
                        dgvData.Rows[e.RowIndex].Cells["is_not_send_fax"].Style.BackColor = Color.Red;
                    else
                        dgvData.Rows[e.RowIndex].Cells["is_not_send_fax"].Style.BackColor = Color.White;

                }
            }
        }
        #endregion

        #region 중복거래처 조회
        private string whereTxt(string tel)
        {
            tel = ValidationTelString(tel);
            string where_txt = "";

            if (!string.IsNullOrEmpty(tel))
            {
                if (tel.Contains(','))
                {
                    if (tel.Substring(0, 1) == ",")
                        tel = tel.Substring(1, tel.Length - 1);

                    string[] tels = tel.Split(',');
                    if (tels.Length == 1)
                        where_txt = $"tel = '{tel}'";
                    else
                    {
                        for (int j = 0; j < tels.Length; j++)
                        {
                            if (!string.IsNullOrEmpty(tels[j].Trim()))
                            {
                                string rsTel = Regex.Replace(tels[j].Trim(), @"[^0-9]", "").ToString();                                   
                                if (!string.IsNullOrEmpty(rsTel))
                                {
                                    if (j > 0 && tels[0].Length > rsTel.Length)
                                        rsTel = tels[0].Substring(0, tels[0].Length - rsTel.Length) + rsTel;

                                    if (string.IsNullOrEmpty(where_txt))
                                        where_txt += $"tel = '{rsTel}'";
                                    else
                                        where_txt += $" OR tel = '{rsTel}'";
                                }
                            }
                        }
                        where_txt = "(" + where_txt + ")";
                    }
                }
                else if (tel.Contains('/'))
                {
                    if (tel.Substring(0, 1) == "/")
                        tel = tel.Substring(1, tel.Length - 1);

                    string[] tels = tel.Split('/');
                    if (tels.Length == 1)
                        where_txt = $"tel = '{tel}'";
                    else
                    {
                        for (int j = 0; j < tels.Length; j++)
                        {
                            if (!string.IsNullOrEmpty(tels[j].Trim()))
                            {
                                string rsTel = Regex.Replace(tels[j].Trim(), @"[^0-9]", "").ToString();
                                if (!string.IsNullOrEmpty(rsTel))
                                {
                                    if (j > 0 && tels[0].Length > rsTel.Length)
                                        rsTel = tels[0].Substring(0, tels[0].Length - rsTel.Length) + rsTel;

                                    if (string.IsNullOrEmpty(where_txt))
                                        where_txt += $"tel = '{rsTel}'";
                                    else
                                        where_txt += $" OR tel = '{rsTel}'";
                                }
                            }
                        }
                        where_txt = "(" + where_txt + ")";
                    }
                }
                else if (tel.Contains('~'))
                {
                    string[] tels = tel.Split('~');
                    string tel1 = Regex.Replace(tels[0].Trim(), @"[^0-9]", "").ToString();
                    string tel2 = Regex.Replace(tels[1].Trim(), @"[^0-9]", "").ToString();
                    if (tel1.Length > 0 && tel2.Length > 0 && tel1.Length > tel2.Length)
                    {
                        int sttNum = Convert.ToInt16(tel1.Substring(tel1.Length - 1, 1));
                        int endNum = Convert.ToInt16(tel2);

                        for (int j = sttNum; j <= endNum; j++)
                        {
                            if (!string.IsNullOrEmpty(tels[0].Trim()))
                            {
                                string rsTel = tels[0].Substring(0, tels[0].Length - 1) + j.ToString();
                                if (string.IsNullOrEmpty(where_txt))
                                    where_txt += $"tel = '{rsTel}'";
                                else
                                    where_txt += $" OR tel = '{rsTel}'";
                            }
                        }
                        where_txt = "(" + where_txt + ")";
                    }
                    else
                        where_txt = $"tel = '{tel}'";
                }
                else
                    where_txt = $"tel = '{tel}'";
                where_txt += " AND tel <> ''";
            }
            else
                where_txt += " tel = '' AND tel <> ''";

            return where_txt;

        }
        Dictionary<int, List<DataRow>> duplicateDic = new Dictionary<int, List<DataRow>>();
        private bool CheckDuplicateData(CompanyModel model, DataGridViewRow row, out int isNotSendFax)
        {
            DataRow[] noDeleteDr = atoDt.Copy().Select("isDelete = 'FALSE'");
            DataTable tempDt = noDeleteDr.CopyToDataTable();
            tempDt = SetSeaoverDatatable(tempDt);           //Ato전산에만 있는 거래처
            //InputDt(입력한 데이터)와 비교해 중복데이터만 걸러내기=======================================
            //INPUTDT JOIN NOSEAOVERDT 
            DataRow[] seaover1 = tempDt.Select(whereTxt(model.tel));
            DataTable seaoverDt1 = new DataTable();
            if (seaover1.Length > 0)
                seaoverDt1 = seaover1.CopyToDataTable();

            DataRow[] seaover2 = tempDt.Select(whereTxt(model.fax));
            DataTable seaoverDt2 = new DataTable();
            if (seaover2.Length > 0)
                seaoverDt2 = seaover2.CopyToDataTable();

            DataRow[] seaover3 = tempDt.Select(whereTxt(model.phone));
            DataTable seaoverDt3 = new DataTable();
            if (seaover3.Length > 0)
                seaoverDt3 = seaover3.CopyToDataTable();

            DataRow[] seaover4 = tempDt.Select(whereTxt(model.other_phone));
            DataTable seaoverDt4 = new DataTable();
            if (seaover4.Length > 0)
                seaoverDt4 = seaover4.CopyToDataTable();

            DataRow[] seaover5 = tempDt.Select($"registration_number = '{model.registration_number}'  AND registration_number <> ''");
            DataTable seaoverDt5 = new DataTable();
            if (seaover5.Length > 0)
                seaoverDt5 = seaover5.CopyToDataTable();

            //최종Datatable
            DataTable duplicateDt = new DataTable();
            duplicateDt.Columns.Add("company_id", typeof(string));
            duplicateDt.Columns.Add("company", typeof(string));
            duplicateDt.Columns.Add("seaover_company_code", typeof(string));
            duplicateDt.Columns.Add("current_sale_date", typeof(string));
            duplicateDt.Columns.Add("ato_manager", typeof(string));
            duplicateDt.Columns.Add("division", typeof(string));
            duplicateDt.Columns.Add("idx", typeof(string));
            duplicateDt.Columns.Add("isNonHandled", typeof(string));
            duplicateDt.Columns.Add("isNotSendFax", typeof(string));
            duplicateDt.Columns.Add("table_index", typeof(string));

            //중복확인
            List<DataRow> list = new List<DataRow>();
            if (seaoverDt1 != null && seaoverDt1.Rows.Count > 0)
            {
                for (int j = 0; j < seaoverDt1.Rows.Count; j++)
                {
                    DataRow newDr = duplicateDt.NewRow();
                    newDr["company_id"] = seaoverDt1.Rows[j]["id"].ToString();
                    newDr["company"] = seaoverDt1.Rows[j]["company"].ToString();
                    newDr["seaover_company_code"] = seaoverDt1.Rows[j]["seaover_company_code"].ToString();
                    newDr["ato_manager"] = seaoverDt1.Rows[j]["ato_manager"].ToString();
                    newDr["division"] = seaoverDt1.Rows[j]["is_duplicate"].ToString();
                    newDr["current_sale_date"] = seaoverDt1.Rows[j]["current_sale_date"].ToString();
                    newDr["isNotSendFax"] = seaoverDt1.Rows[j]["isNotSendFax"].ToString();
                    newDr["isNonHandled"] = seaoverDt1.Rows[j]["isNonHandled"].ToString();
                    newDr["idx"] = seaoverDt1.Rows[j]["idx"].ToString();
                    newDr["table_index"] = seaoverDt1.Rows[j]["table_index"].ToString();

                    bool isExist = false;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (seaoverDt1.Rows[j]["idx"].ToString() == list[i]["idx"].ToString())
                            isExist = true;
                    }


                    if (!isExist)
                        list.Add(newDr);
                }
            }

            if (seaoverDt2 != null && seaoverDt2.Rows.Count > 0)
            {
                for (int j = 0; j < seaoverDt2.Rows.Count; j++)
                {
                    DataRow newDr = duplicateDt.NewRow();
                    newDr["company_id"] = seaoverDt2.Rows[j]["id"].ToString();
                    newDr["company"] = seaoverDt2.Rows[j]["company"].ToString();
                    newDr["seaover_company_code"] = seaoverDt2.Rows[j]["seaover_company_code"].ToString();
                    newDr["ato_manager"] = seaoverDt2.Rows[j]["ato_manager"].ToString();
                    newDr["division"] = seaoverDt2.Rows[j]["is_duplicate"].ToString();
                    newDr["current_sale_date"] = seaoverDt2.Rows[j]["current_sale_date"].ToString();
                    newDr["isNotSendFax"] = seaoverDt2.Rows[j]["isNotSendFax"].ToString();
                    newDr["isNonHandled"] = seaoverDt2.Rows[j]["isNonHandled"].ToString();
                    newDr["idx"] = seaoverDt2.Rows[j]["idx"].ToString();
                    newDr["table_index"] = seaoverDt2.Rows[j]["table_index"].ToString();

                    bool isExist = false;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (seaoverDt2.Rows[j]["idx"].ToString() == list[i]["idx"].ToString())
                            isExist = true;
                    }


                    if (!isExist)
                        list.Add(newDr);
                }
            }

            if (seaoverDt3 != null && seaoverDt3.Rows.Count > 0)
            {
                for (int j = 0; j < seaoverDt3.Rows.Count; j++)
                {
                    DataRow newDr = duplicateDt.NewRow();
                    newDr["company_id"] = seaoverDt3.Rows[j]["id"].ToString();
                    newDr["company"] = seaoverDt3.Rows[j]["company"].ToString();
                    newDr["seaover_company_code"] = seaoverDt3.Rows[j]["seaover_company_code"].ToString();
                    newDr["ato_manager"] = seaoverDt3.Rows[j]["ato_manager"].ToString();
                    newDr["division"] = seaoverDt3.Rows[j]["is_duplicate"].ToString();
                    newDr["current_sale_date"] = seaoverDt3.Rows[j]["current_sale_date"].ToString();
                    newDr["isNotSendFax"] = seaoverDt3.Rows[j]["isNotSendFax"].ToString();
                    newDr["isNonHandled"] = seaoverDt3.Rows[j]["isNonHandled"].ToString();
                    newDr["idx"] = seaoverDt3.Rows[j]["idx"].ToString();
                    newDr["table_index"] = seaoverDt3.Rows[j]["table_index"].ToString();

                    bool isExist = false;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (seaoverDt3.Rows[j]["idx"].ToString() == list[i]["idx"].ToString())
                            isExist = true;
                    }


                    if (!isExist)
                        list.Add(newDr);
                }
            }

            if (seaoverDt4 != null && seaoverDt4.Rows.Count > 0)
            {
                for (int j = 0; j < seaoverDt4.Rows.Count; j++)
                {
                    DataRow newDr = duplicateDt.NewRow();
                    newDr["company_id"] = seaoverDt4.Rows[j]["id"].ToString();
                    newDr["company"] = seaoverDt4.Rows[j]["company"].ToString();
                    newDr["seaover_company_code"] = seaoverDt4.Rows[j]["seaover_company_code"].ToString();
                    newDr["ato_manager"] = seaoverDt4.Rows[j]["ato_manager"].ToString();
                    newDr["division"] = seaoverDt4.Rows[j]["is_duplicate"].ToString();
                    newDr["current_sale_date"] = seaoverDt4.Rows[j]["current_sale_date"].ToString();
                    newDr["isNotSendFax"] = seaoverDt4.Rows[j]["isNotSendFax"].ToString();
                    newDr["isNonHandled"] = seaoverDt4.Rows[j]["isNonHandled"].ToString();
                    newDr["idx"] = seaoverDt4.Rows[j]["idx"].ToString();
                    newDr["table_index"] = seaoverDt4.Rows[j]["table_index"].ToString();

                    bool isExist = false;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (seaoverDt4.Rows[j]["idx"].ToString() == list[i]["idx"].ToString())
                            isExist = true;
                    }


                    if (!isExist)
                        list.Add(newDr);
                }
            }

            //사업자등록번호 중복
            if (seaoverDt5 != null && seaoverDt5.Rows.Count > 0 && !string.IsNullOrEmpty(model.registration_number))
            {
                for (int j = 0; j < seaoverDt5.Rows.Count; j++)
                {
                    DataRow newDr = duplicateDt.NewRow();
                    newDr["company_id"] = seaoverDt5.Rows[j]["id"].ToString();
                    newDr["company"] = seaoverDt5.Rows[j]["company"].ToString();
                    newDr["seaover_company_code"] = seaoverDt5.Rows[j]["seaover_company_code"].ToString();
                    newDr["ato_manager"] = seaoverDt5.Rows[j]["ato_manager"].ToString();
                    newDr["division"] = seaoverDt5.Rows[j]["is_duplicate"].ToString();
                    newDr["current_sale_date"] = seaoverDt5.Rows[j]["current_sale_date"].ToString();
                    newDr["isNotSendFax"] = seaoverDt5.Rows[j]["isNotSendFax"].ToString();
                    newDr["isNonHandled"] = seaoverDt5.Rows[j]["isNonHandled"].ToString();
                    newDr["idx"] = seaoverDt5.Rows[j]["idx"].ToString();
                    newDr["table_index"] = seaoverDt5.Rows[j]["table_index"].ToString();

                    bool isExist = false;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (seaoverDt5.Rows[j]["idx"].ToString() == list[i]["idx"].ToString())
                            isExist = true;
                    }


                    if (!isExist)
                        list.Add(newDr);

                }
            }

            //중복결과 출력
            int common = 0, myData = 0, potential1 = 0, potential2 = 0, trading = 0, noneHnadled = 0, notSendFax = 0, outOfBusiness = 0;
            string duplicate_id = "";
            for (int i = 0; i < list.Count; i++)
            {
                switch (list[i]["division"].ToString())
                {
                    case "공용DATA":
                        common++;
                        break;
                    case "내DATA":
                        myData++;
                        break;
                    case "잠재1":
                        potential1++;
                        break;
                    case "잠재2":
                        potential2++;
                        break;
                    case "거래중(SEAOVER)":
                        trading++;
                        break;
                    case "취급X":
                        noneHnadled++;
                        duplicate_id += " " + list[i]["table_index"].ToString();
                        break;
                    case "팩스X":
                        notSendFax++;
                        break;
                    case "폐업":
                        outOfBusiness++;
                        break;
                }
            }
            row.Cells["duplicate_table_index"].Value = duplicate_id;


            isNotSendFax = notSendFax;

            if (common + myData + potential1 + potential2 + trading >= 3)
            {
                messageBox.Show(this, model.name + "은(는) 등록된 중복거래처가 3개 이상 존재합니다.");
                this.Activate();
                return false;
            }
            else if (trading > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i]["division"].ToString() == "거래중(SEAOVER)" && !string.IsNullOrEmpty(list[i]["seaover_company_code"].ToString()))
                    {
                        DateTime current_sale_date;
                        if (DateTime.TryParse(list[i]["current_sale_date"].ToString(), out current_sale_date) && current_sale_date >= DateTime.Now.AddMonths(-8))
                        {
                            messageBox.Show(this, model.name + "은(는) 이미 거래중인 씨오버거래처가 존재합니다!");
                            this.Activate();
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        private void CheckDuplicateAsOne(int idx)
        {
            if (dgvCompany.Rows.Count > 0)
            {
                duplicateDic = new Dictionary<int, List<DataRow>>();
                DataRow[] noDeleteDr = atoDt.Copy().Select("isDelete = 'FALSE'");
                DataTable tempDt = noDeleteDr.CopyToDataTable();
                tempDt = SetSeaoverDatatable(tempDt);           //Ato전산에만 있는 거래처

                DataGridViewRow row = dgvCompany.Rows[idx];
                //InputDt(입력한 데이터)와 비교해 중복데이터만 걸러내기=======================================

                //INPUTDT JOIN NOSEAOVERDT 
                DataRow[] seaover1 = tempDt.Select(whereTxt(row.Cells["tel"].Value.ToString()));
                DataTable seaoverDt1 = new DataTable();
                if (seaover1.Length > 0)
                    seaoverDt1 = seaover1.CopyToDataTable();

                DataRow[] seaover2 = tempDt.Select(whereTxt(row.Cells["fax"].Value.ToString()));
                DataTable seaoverDt2 = new DataTable();
                if (seaover2.Length > 0)
                    seaoverDt2 = seaover2.CopyToDataTable();

                DataRow[] seaover3 = tempDt.Select(whereTxt(row.Cells["phone"].Value.ToString()));
                DataTable seaoverDt3 = new DataTable();
                if (seaover3.Length > 0)
                    seaoverDt3 = seaover3.CopyToDataTable();

                DataRow[] seaover4 = tempDt.Select(whereTxt(row.Cells["other_phone"].Value.ToString()));
                DataTable seaoverDt4 = new DataTable();
                if (seaover4.Length > 0)
                    seaoverDt4 = seaover4.CopyToDataTable();

                DataRow[] seaover5 = tempDt.Select($"registration_number = '{row.Cells["registration_number"].Value.ToString()}'  AND registration_number <> ''");
                DataTable seaoverDt5 = new DataTable();
                if (seaover5.Length > 0)
                    seaoverDt5 = seaover5.CopyToDataTable();

                //초기화
                if (duplicateDic.ContainsKey(idx))
                    duplicateDic[idx] = new List<DataRow>();

                DataTable duplicateDt = new DataTable();
                duplicateDt.Columns.Add("company_id", typeof(string));
                duplicateDt.Columns.Add("company", typeof(string));
                duplicateDt.Columns.Add("seaover_company_code", typeof(string));
                duplicateDt.Columns.Add("ato_manager", typeof(string));
                duplicateDt.Columns.Add("division", typeof(string));
                duplicateDt.Columns.Add("idx", typeof(string));

                //atoDt index
                int table_index = Convert.ToInt32(row.Cells["table_index"].Value.ToString());

                if (seaoverDt1 != null && seaoverDt1.Rows.Count > 0)
                {
                    for (int j = 0; j < seaoverDt1.Rows.Count; j++)
                    {
                        DataRow newDr = duplicateDt.NewRow();
                        newDr["company_id"] = seaoverDt1.Rows[j]["id"].ToString();
                        newDr["company"] = seaoverDt1.Rows[j]["company"].ToString();
                        newDr["seaover_company_code"] = seaoverDt1.Rows[j]["seaover_company_code"].ToString();
                        newDr["ato_manager"] = seaoverDt1.Rows[j]["ato_manager"].ToString();
                        newDr["division"] = seaoverDt1.Rows[j]["is_duplicate"].ToString();
                        newDr["idx"] = seaoverDt1.Rows[j]["idx"].ToString();


                        if (duplicateDic.ContainsKey(table_index))
                        {
                            List<DataRow> list = duplicateDic[table_index];

                            bool isExist = false;
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (seaoverDt1.Rows[j]["idx"].ToString() == list[i]["idx"].ToString())
                                    isExist = true;
                            }

                            if (!isExist)
                            {
                                list.Add(newDr);
                                duplicateDic[table_index] = list;
                            }
                        }
                        else
                        {
                            List<DataRow> list = new List<DataRow>();
                            list.Add(newDr);
                            duplicateDic[table_index] = list;
                        }
                    }
                }

                if (seaoverDt2 != null && seaoverDt2.Rows.Count > 0)
                {
                    for (int j = 0; j < seaoverDt2.Rows.Count; j++)
                    {
                        DataRow newDr = duplicateDt.NewRow();
                        newDr["company_id"] = seaoverDt2.Rows[j]["id"].ToString();
                        newDr["company"] = seaoverDt2.Rows[j]["company"].ToString();
                        newDr["seaover_company_code"] = seaoverDt2.Rows[j]["seaover_company_code"].ToString();
                        newDr["ato_manager"] = seaoverDt2.Rows[j]["ato_manager"].ToString();
                        newDr["division"] = seaoverDt2.Rows[j]["is_duplicate"].ToString();
                        newDr["idx"] = seaoverDt2.Rows[j]["idx"].ToString();

                        if (duplicateDic.ContainsKey(table_index))
                        {
                            List<DataRow> list = duplicateDic[table_index];

                            bool isExist = false;
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (seaoverDt2.Rows[j]["idx"].ToString() == list[i]["idx"].ToString())
                                    isExist = true;
                            }


                            if (!isExist)
                            {
                                list.Add(newDr);
                                duplicateDic[table_index] = list;
                            }
                        }
                        else
                        {
                            List<DataRow> list = new List<DataRow>();
                            list.Add(newDr);
                            duplicateDic[table_index] = list;
                        }
                    }
                }

                if (seaoverDt3 != null && seaoverDt3.Rows.Count > 0)
                {
                    for (int j = 0; j < seaoverDt3.Rows.Count; j++)
                    {
                        DataRow newDr = duplicateDt.NewRow();
                        newDr["company_id"] = seaoverDt3.Rows[j]["id"].ToString();
                        newDr["company"] = seaoverDt3.Rows[j]["company"].ToString();
                        newDr["seaover_company_code"] = seaoverDt3.Rows[j]["seaover_company_code"].ToString();
                        newDr["ato_manager"] = seaoverDt3.Rows[j]["ato_manager"].ToString();
                        newDr["division"] = seaoverDt3.Rows[j]["is_duplicate"].ToString();
                        newDr["idx"] = seaoverDt3.Rows[j]["idx"].ToString();

                        if (duplicateDic.ContainsKey(table_index))
                        {
                            List<DataRow> list = duplicateDic[table_index];

                            bool isExist = false;
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (seaoverDt3.Rows[j]["idx"].ToString() == list[i]["idx"].ToString())
                                    isExist = true;
                            }


                            if (!isExist)
                            {
                                list.Add(newDr);
                                duplicateDic[table_index] = list;
                            }
                        }
                        else
                        {
                            List<DataRow> list = new List<DataRow>();
                            list.Add(newDr);
                            duplicateDic[table_index] = list;
                        }
                    }
                }

                if (seaoverDt4 != null && seaoverDt4.Rows.Count > 0)
                {
                    for (int j = 0; j < seaoverDt4.Rows.Count; j++)
                    {
                        DataRow newDr = duplicateDt.NewRow();
                        newDr["company_id"] = seaoverDt4.Rows[j]["id"].ToString();
                        newDr["company"] = seaoverDt4.Rows[j]["company"].ToString();
                        newDr["seaover_company_code"] = seaoverDt4.Rows[j]["seaover_company_code"].ToString();
                        newDr["ato_manager"] = seaoverDt4.Rows[j]["ato_manager"].ToString();
                        newDr["division"] = seaoverDt4.Rows[j]["is_duplicate"].ToString();
                        newDr["idx"] = seaoverDt4.Rows[j]["idx"].ToString();

                        if (duplicateDic.ContainsKey(table_index))
                        {
                            List<DataRow> list = duplicateDic[table_index];

                            bool isExist = false;
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (seaoverDt4.Rows[j]["idx"].ToString() == list[i]["idx"].ToString())
                                    isExist = true;
                            }


                            if (!isExist)
                            {
                                list.Add(newDr);
                                duplicateDic[table_index] = list;
                            }
                        }
                        else
                        {
                            List<DataRow> list = new List<DataRow>();
                            list.Add(newDr);
                            duplicateDic[table_index] = list;
                        }
                    }
                }

                //사업자등록번호 중복
                if (seaoverDt5 != null && seaoverDt5.Rows.Count > 0 && !string.IsNullOrEmpty(row.Cells["registration_number"].Value.ToString()))
                {
                    for (int j = 0; j < seaoverDt5.Rows.Count; j++)
                    {
                        DataRow newDr = duplicateDt.NewRow();
                        newDr["company_id"] = seaoverDt5.Rows[j]["id"].ToString();
                        newDr["company"] = seaoverDt5.Rows[j]["company"].ToString();
                        newDr["seaover_company_code"] = seaoverDt5.Rows[j]["seaover_company_code"].ToString();
                        newDr["ato_manager"] = seaoverDt5.Rows[j]["ato_manager"].ToString();
                        newDr["division"] = seaoverDt5.Rows[j]["is_duplicate"].ToString();
                        newDr["idx"] = seaoverDt5.Rows[j]["idx"].ToString();

                        if (duplicateDic.ContainsKey(table_index))
                        {
                            List<DataRow> list = duplicateDic[table_index];

                            bool isExist = false;
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (seaoverDt5.Rows[j]["idx"].ToString() == list[i]["idx"].ToString())
                                    isExist = true;
                            }


                            if (!isExist)
                            {
                                list.Add(newDr);
                                duplicateDic[table_index] = list;
                            }
                        }
                        else
                        {
                            List<DataRow> list = new List<DataRow>();
                            list.Add(newDr);
                            duplicateDic[table_index] = list;
                        }
                    }
                }

                //중복결과 출력
                if (duplicateDic.ContainsKey(table_index))
                {
                    int common = 0, myData = 0, potential1 = 0, potential2 = 0, trading = 0, noneHnadled = 0, notSendFax = 0, outOfBusiness = 0;
                    List<DataRow> list = duplicateDic[table_index];

                    for (int i = 0; i < list.Count; i++)
                    {
                        switch (list[i]["division"].ToString())
                        {
                            case "공용DATA":
                                common++;
                                break;
                            case "내DATA":
                                myData++;
                                break;
                            case "잠재1":
                                potential1++;
                                break;
                            case "잠재2":
                                potential2++;
                                break;
                            case "거래중(SEAOVER)":
                                trading++;
                                break;
                            case "취급X":
                                noneHnadled++;
                                break;
                            case "팩스X":
                                notSendFax++;
                                break;
                            case "폐업":
                                outOfBusiness++;
                                break;
                        }
                    }

                    string duplicate_result = "";
                    if (common > 0)
                    {
                        duplicate_result += " 공용DATA : " + common.ToString("#,##0");
                        atoDt.Rows[table_index]["duplicate_common_count"] = common;
                        row.Cells["duplicate_common_count"].Value = common;
                    }
                    if (myData > 0)
                    {
                        duplicate_result += " 내DATA : " + myData.ToString("#,##0");
                        atoDt.Rows[table_index]["duplicate_myData_count"] = myData;
                        row.Cells["duplicate_myData_count"].Value = myData;
                    }
                    if (potential1 > 0)
                    {
                        duplicate_result += " 잠재1 : " + potential1.ToString("#,##0");
                        atoDt.Rows[table_index]["duplicate_potential1_count"] = potential1;
                        row.Cells["duplicate_potential1_count"].Value = potential1;
                    }
                    if (potential2 > 0)
                    {
                        duplicate_result += " 잠재2 : " + potential2.ToString("#,##0");
                        atoDt.Rows[table_index]["duplicate_potential2_count"] = potential2;
                        row.Cells["duplicate_potential2_count"].Value = potential2;
                    }
                    if (trading > 0)
                    {
                        duplicate_result += " 거래중 : " + trading.ToString("#,##0");
                        atoDt.Rows[table_index]["duplicate_trading_count"] = trading;
                        row.Cells["duplicate_trading_count"].Value = trading;
                    }
                    if (noneHnadled > 0)
                    {
                        duplicate_result += " 취급X : " + noneHnadled.ToString("#,##0");
                        atoDt.Rows[table_index]["duplicate_nonHandled_count"] = noneHnadled;
                        row.Cells["duplicate_nonHandled_count"].Value = noneHnadled;
                    }
                    if (notSendFax > 0)
                    {
                        duplicate_result += " 팩스X : " + notSendFax.ToString("#,##0");
                        atoDt.Rows[table_index]["duplicate_notSendFax_count"] = notSendFax;
                        row.Cells["duplicate_notSendFax_count"].Value = notSendFax;
                    }
                    if (outOfBusiness > 0)
                    {
                        duplicate_result += " 폐업 : " + outOfBusiness.ToString("#,##0");
                        atoDt.Rows[table_index]["duplicate_outBusiness_count"] = outOfBusiness;
                        row.Cells["duplicate_outBusiness_count"].Value = outOfBusiness;
                    }

                    atoDt.Rows[table_index]["duplicate_result"] = duplicate_result.Trim();
                    row.Cells["duplicate_result"].Value = duplicate_result.Trim();
                }
            }
            atoDt.AcceptChanges();
            CheckDuplicate(idx);
        }

        private void CheckSeaoverCompany()
        {
            List<string[]> updateList = new List<string[]>();
            if (dgvCompany.Rows.Count > 0)
            {
                dgvCompany.ClearSelection();

                string division = "";
                switch (tcMain.SelectedTab.Name)
                {
                    case "tabAllMyCompany":
                        division = "내DATA";
                        break;
                    case "tabCommonData":
                        division = "공용DATA";
                        break;
                    case "tabRamdomData":
                        division = "내DATA";
                        break;
                    case "tabCompany":
                        division = "잠재1";
                        break;
                    case "tabCompany2":
                        division = "잠재2";
                        break;
                    default:
                        messageBox.Show(this, "현재 탭에서는 사용할 수 없습니다.");
                        this.Activate();
                        return;
                }

                //입력데이터 -> Datatable
                DataTable inputDt = common.ConvertDgvToDataTable(dgvCompany);
                inputDt.Columns.Add("rowIndex", typeof(Int32));
                for (int i = 0; i < inputDt.Rows.Count; i++)
                    inputDt.Rows[i]["rowindex"] = i;
                //inputDt.AcceptChanges();
                DataRow[] inputDr = inputDt.Select("seaover_company_code = ''");
                if (inputDr.Length == 0)
                    return;
                else
                    inputDt = inputDr.CopyToDataTable();
                inputDt = SetAtoDatatable(inputDt);

                //Ato전산에 있는 씨오버 거래처
                duplicateDic = new Dictionary<int, List<DataRow>>();
                DataRow[] seaoverDr = atoDt.Copy().Select("seaover_company_code <> '' AND isTrading = 'TRUE'");
                DataTable tempDt = seaoverDr.CopyToDataTable();
                tempDt = SetSeaoverDatatable(tempDt);

                // 데이터 중복추출, PLINQ 사용
                DataTable duplicateDt = null;
                DataRow[] tempDr = inputDt.Select("tel <> ''");
                DataRow[] tempDr2 = tempDt.Select("tel <> ''");
                if (tempDr.Length > 0 && tempDr2.Length > 0)
                {
                    DataTable rnDt = tempDr.CopyToDataTable();
                    DataTable rnDt2 = tempDr2.CopyToDataTable();

                    var duplicateVar1 = from p in rnDt.AsEnumerable()
                                        join t in rnDt2.AsEnumerable()
                                        on p.Field<string>("tel") equals t.Field<string>("tel")
                                        into outer
                                        from t in outer.DefaultIfEmpty()
                                        select new
                                        {
                                            input_table_index = p.Field<string>("table_index"),
                                            idx = (t == null) ? "" : t.Field<string>("table_index"),
                                            tel = p.Field<string>("tel"),
                                            seaover_company_code = (t == null) ? "" : t.Field<string>("seaover_company_code")
                                        };
                    DataTable duplicateDt1 = ConvertListToDatatable(duplicateVar1);
                    if (duplicateDt1 != null)
                    {
                        duplicateDt1.AcceptChanges();
                        duplicateDt = duplicateDt1;
                    }
                }

                tempDr = inputDt.Select("registration_number <> ''");
                tempDr2 = tempDt.Select("registration_number <> ''");
                if (tempDr.Length > 0 && tempDr2.Length > 0)
                {
                    DataTable rnDt = tempDr.CopyToDataTable();
                    DataTable rnDt2 = tempDr2.CopyToDataTable();
                    var duplicateVar2 = from p in rnDt.AsEnumerable()
                                        join t in rnDt2.AsEnumerable()
                                        on p.Field<string>("registration_number") equals t.Field<string>("registration_number")
                                        into outer
                                        from t in outer.DefaultIfEmpty()
                                        select new
                                        {
                                            input_table_index = p.Field<string>("table_index"),
                                            idx = (t == null) ? "" : t.Field<string>("table_index"),
                                            tel = p.Field<string>("registration_number"),
                                            seaover_company_code = (t == null) ? "" : t.Field<string>("seaover_company_code")
                                        };
                    DataTable duplicateDt2 = ConvertListToDatatable(duplicateVar2);
                    if (duplicateDt2 != null)
                    {
                        duplicateDt2.AcceptChanges();

                        if (duplicateDt != null)
                        {
                            duplicateDt.Merge(duplicateDt2);
                            duplicateDt.AcceptChanges();
                        }
                    }
                }
                //중복삭제
                if (duplicateDt != null)
                {
                    duplicateDt = duplicateDt.DefaultView.ToTable(true, new string[] { "input_table_index", "tel", "idx", "seaover_company_code" });
                    DataRow[] resultDr = duplicateDt.Select("idx <> '' AND tel <> '' AND seaover_company_code <> ''");
                    if (resultDr.Length > 0)
                    {
                        DataTable resultDt = resultDr.CopyToDataTable();

                        //유효성검사
                        //for (int k = 0; k < dgvCompany.Rows.Count; k++)
                        foreach (DataGridViewRow row in dgvCompany.Rows)
                        {
                            //atoDt index
                            int table_index = Convert.ToInt32(row.Cells["table_index"].Value.ToString());
                            resultDr = resultDt.Select($"input_table_index = '{table_index}'");
                            if (resultDr.Length > 0)
                            {
                                //최종Datatable
                                DataTable resultDuplicateDt = new DataTable();
                                resultDuplicateDt.Columns.Add("company_id", typeof(string));
                                resultDuplicateDt.Columns.Add("company", typeof(string));
                                resultDuplicateDt.Columns.Add("seaover_company_code", typeof(string));
                                resultDuplicateDt.Columns.Add("ato_manager", typeof(string));
                                resultDuplicateDt.Columns.Add("division", typeof(string));
                                resultDuplicateDt.Columns.Add("current_sales_date", typeof(string));
                                resultDuplicateDt.Columns.Add("idx", typeof(string));

                                foreach (DataRow dr in resultDr)
                                {
                                    DataRow atoDr = atoDt.Rows[Convert.ToInt32(dr["idx"].ToString())];
                                    DataRow newDr = resultDuplicateDt.NewRow();
                                    newDr["company_id"] = atoDr["id"].ToString();
                                    newDr["company"] = atoDr["company"].ToString();
                                    newDr["seaover_company_code"] = atoDr["seaover_company_code"].ToString();
                                    newDr["ato_manager"] = atoDr["ato_manager"].ToString();
                                    //newDr["division"] = atoDr["is_duplicate"].ToString();
                                    newDr["current_sales_date"] = atoDr["current_sale_date"].ToString();
                                    newDr["idx"] = atoDr["table_index"].ToString();

                                    if (duplicateDic.ContainsKey(table_index))
                                    {
                                        List<DataRow> list = duplicateDic[table_index];

                                        bool isExist = false;
                                        for (int i = 0; i < list.Count; i++)
                                        {
                                            if (dr["idx"].ToString() == list[i]["idx"].ToString())
                                            {
                                                isExist = true;
                                                break;
                                            }
                                        }
                                        if (!isExist)
                                        {
                                            list.Add(newDr);
                                            duplicateDic[table_index] = list;
                                        }
                                    }
                                    else
                                    {
                                        List<DataRow> list = new List<DataRow>();
                                        list.Add(newDr);
                                        duplicateDic[table_index] = list;
                                    }
                                }
                                //중복결과 출력
                                if (duplicateDic.ContainsKey(table_index))
                                {
                                    List<DataRow> list = duplicateDic[table_index];
                                    foreach (DataRow dr in list)
                                    {
                                        DateTime current_sales_date;
                                        if (!string.IsNullOrEmpty(dr["seaover_company_code"].ToString())
                                            && DateTime.TryParse(dr["current_sales_date"].ToString(), out current_sales_date) && current_sales_date >= DateTime.Now.AddMonths(-8))
                                        {
                                            row.Cells["seaover_company_code"].Value = dr["seaover_company_code"].ToString();
                                            row.Selected = true;

                                            string[] companyInfo = new string[9];
                                            companyInfo[0] = row.Cells["id"].Value.ToString();
                                            companyInfo[1] = division;
                                            companyInfo[2] = row.Cells["company"].Value.ToString();
                                            companyInfo[3] = row.Cells["ato_manager"].Value.ToString();
                                            companyInfo[4] = row.Cells["table_index"].Value.ToString();
                                            companyInfo[5] = dr["company"].ToString();
                                            companyInfo[6] = dr["seaover_company_code"].ToString();
                                            companyInfo[7] = dr["ato_manager"].ToString();
                                            companyInfo[8] = current_sales_date.ToString("yyyy-MM-dd");
                                            updateList.Add(companyInfo);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            //데이터 정리끝 출력
            isCheckDuplicate = true;
            if (updateList.Count > 0)
            {
                try
                {
                    NewSeaoverCompanyUpdateManager nscum = new NewSeaoverCompanyUpdateManager(um, this, updateList);
                    nscum.ShowDialog();

                    //씨오버코드
                    foreach (DataGridViewRow row in dgvCompany.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells["chk"].Value))
                        {
                            if (messageBox.Show(this, $"거래처중 씨오버거래처로 보이는 내역에 씨오버코드를 매칭시키겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                UpdateCompany(1);
                            this.Activate();
                            break;
                        }
                    }
                    
                }
                catch
                { }
            }
        }
        private string ValidationTelString(string tel)
        {
            if (tel == null)
                tel = "";
            tel = tel.Replace("/", "");

            if (!string.IsNullOrEmpty(tel))
            {
                tel = tel.Replace(" ", "");
                tel = tel.Replace("-", "");
                tel = tel.Replace("(", "");
                tel = tel.Replace(")", "");

                if (tel.Length == 8 && tel.Substring(0, 2) != "15" && tel.Substring(0, 2) != "16" && tel.Substring(0, 2) != "18" && tel.Substring(0, 1) != "0")
                    tel = "010" + tel;
                else if (tel.Length > 2 && tel.Substring(0, 2) != "15" && tel.Substring(0, 2) != "16" && tel.Substring(0, 2) != "18" && tel.Substring(0, 1) != "0")
                    tel = "0" + tel;

            }
            return tel;
        }


        #region 중복 Datatable 만들기
        private DataTable SetAtoDatatable(DataTable seaoverDt)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("company", typeof(string));
            dt.Columns.Add("sales_updatetime", typeof(string));
            dt.Columns.Add("current_sale_date", typeof(string));
            dt.Columns.Add("seaover_company_code", typeof(string));
            dt.Columns.Add("tel", typeof(string));
            dt.Columns.Add("registration_number", typeof(string));
            dt.Columns.Add("ato_manager", typeof(string));
            dt.Columns.Add("isNonHandled", typeof(string));
            dt.Columns.Add("isOutBusiness", typeof(string));
            dt.Columns.Add("isNotSendFax", typeof(string));
            dt.Columns.Add("isPotential1", typeof(string));
            dt.Columns.Add("isPotential2", typeof(string));
            dt.Columns.Add("isTrading", typeof(string));
            dt.Columns.Add("is_duplicate", typeof(string));
            dt.Columns.Add("table_index", typeof(string));
            if (seaoverDt != null && seaoverDt.Rows.Count > 0)
            {
                int idx = 0;
                foreach (DataRow row in seaoverDt.Rows)
                {
                    idx++;
                    bool isAdd = false;
                    string tel = ValidationTelString(row["tel"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""));
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('/'))
                        {
                            if (tel.Substring(0, 1) == "/")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split('/');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');
                            string tel1 = Regex.Replace(tels[0].Trim(), @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1].Trim(), @"[^0-9]", "").ToString();
                            if (tel1.Length >= 10 && tel2.Length > 0)
                            {
                                int sttNum = Convert.ToInt16(tel1.Substring(tel1.Length - 1, 1));
                                int endNum = Convert.ToInt16(tel2);

                                for (int j = sttNum; j <= endNum; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[0].Trim()))
                                    {
                                        string rsTel = tels[0].Substring(0, tels[0].Length - 1) + j.ToString();
                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = GetDatarow(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = GetDatarow(dt, row, tel, idx);
                            dt.Rows.Add(dr);
                        }
                        isAdd = true;
                    }

                    tel = ValidationTelString(row["fax"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""));
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('/'))
                        {
                            if (tel.Substring(0, 1) == "/")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split('/');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');
                            string tel1 = Regex.Replace(tels[0].Trim(), @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1].Trim(), @"[^0-9]", "").ToString();
                            if (tel1.Length >= 10 && tel2.Length > 0)
                            {
                                int sttNum = Convert.ToInt16(tel1.Substring(tel1.Length - 1, 1));
                                int endNum = Convert.ToInt16(tel2);

                                for (int j = sttNum; j <= endNum; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[0].Trim()))
                                    {
                                        string rsTel = tels[0].Substring(0, tels[0].Length - 1) + j.ToString();
                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = GetDatarow(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = GetDatarow(dt, row, tel, idx);
                            dt.Rows.Add(dr);
                        }
                        isAdd = true;
                    }

                    tel = ValidationTelString(row["phone"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""));
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('/'))
                        {
                            if (tel.Substring(0, 1) == "/")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split('/');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');

                            string tel1 = Regex.Replace(tels[0].Trim(), @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1].Trim(), @"[^0-9]", "").ToString();
                            if (tel1.Length >= 10 && tel2.Length > 0)
                            {
                                int sttNum = Convert.ToInt16(tel1.Substring(tel1.Length - 1, 1));
                                int endNum = Convert.ToInt16(tel2);

                                for (int j = sttNum; j <= endNum; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[0].Trim()))
                                    {
                                        string rsTel = tels[0].Substring(0, tels[0].Length - 1) + j.ToString();
                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = GetDatarow(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = GetDatarow(dt, row, tel, idx);
                            dt.Rows.Add(dr);
                        }
                        isAdd = true;
                    }

                    tel = ValidationTelString(row["other_phone"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""));
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('/'))
                        {
                            if (tel.Substring(0, 1) == "/")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split('/');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');

                            string tel1 = Regex.Replace(tels[0].Trim(), @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1].Trim(), @"[^0-9]", "").ToString();
                            if (tel1.Length >= 10 && tel2.Length > 0)
                            {
                                int sttNum = Convert.ToInt16(tel1.Substring(tel1.Length - 1, 1));
                                int endNum = Convert.ToInt16(tel2);

                                for (int j = sttNum; j <= endNum; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[0].Trim()))
                                    {
                                        string rsTel = tels[0].Substring(0, tels[0].Length - 1) + j.ToString();
                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = GetDatarow(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = GetDatarow(dt, row, tel, idx);
                            dt.Rows.Add(dr);
                        }
                        isAdd = true;
                    }

                    string registration_number = row["registration_number"].ToString().Trim().Replace("-", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(registration_number) && !isAdd)
                    {
                        DataRow dr = GetDatarow(dt, row, "", idx);
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }
        private DataTable SetSeaoverDatatable(DataTable seaoverDt)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("idx", typeof(string));
            dt.Columns.Add("company", typeof(string));
            dt.Columns.Add("sales_updatetime", typeof(string));
            dt.Columns.Add("current_sale_date", typeof(string));
            dt.Columns.Add("seaover_company_code", typeof(string));
            dt.Columns.Add("tel", typeof(string));
            dt.Columns.Add("registration_number", typeof(string));
            dt.Columns.Add("ato_manager", typeof(string));
            dt.Columns.Add("isNonHandled", typeof(string));
            dt.Columns.Add("isOutBusiness", typeof(string));
            dt.Columns.Add("isNotSendFax", typeof(string));
            dt.Columns.Add("isPotential1", typeof(string));
            dt.Columns.Add("isPotential2", typeof(string));
            dt.Columns.Add("isTrading", typeof(string));
            dt.Columns.Add("is_duplicate", typeof(string));
            dt.Columns.Add("table_index", typeof(string));
            if (seaoverDt != null && seaoverDt.Rows.Count > 0)
            {
                int idx = 0;
                foreach (DataRow row in seaoverDt.Rows)
                {
                    idx++;
                    bool isAdd = false;
                    string tel = row["tel"].ToString().Trim().Replace("-", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('/'))
                        {
                            if (tel.Substring(0, 1) == "/")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split('/');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');
                            string tel1 = Regex.Replace(tels[0].Trim(), @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1].Trim(), @"[^0-9]", "").ToString();
                            if (tel1.Length >= 10 && tel2.Length == 1)
                            {
                                if (Int32.TryParse(tel1.Substring(tel1.Length - 1, 1), out int sttNum) && Int32.TryParse(tel2, out int endNum))
                                {
                                    for (int j = sttNum; j <= endNum; j++)
                                    {
                                        if (!string.IsNullOrEmpty(tels[0].Trim()))
                                        {
                                            string rsTel = tels[0].Substring(0, tels[0].Length - 1) + j.ToString();
                                            DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                            dt.Rows.Add(dr);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = GetDatarow2(dt, row, tel, idx);
                            dt.Rows.Add(dr);
                        }
                        isAdd = true;
                    }

                    tel = row["fax"].ToString().Trim().Replace("-", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('/'))
                        {
                            if (tel.Substring(0, 1) == "/")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split('/');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');
                            string tel1 = Regex.Replace(tels[0].Trim(), @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1].Trim(), @"[^0-9]", "").ToString();
                            if (tel1.Length >= 10 && tel2.Length == 1)
                            {

                                if (Int32.TryParse(tel1.Substring(tel1.Length - 1, 1), out int sttNum) && Int32.TryParse(tel2, out int endNum))
                                {
                                    for (int j = sttNum; j <= endNum; j++)
                                    {
                                        if (!string.IsNullOrEmpty(tels[0].Trim()))
                                        {
                                            string rsTel = tels[0].Substring(0, tels[0].Length - 1) + j.ToString();
                                            DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                            dt.Rows.Add(dr);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = GetDatarow2(dt, row, tel, idx);
                            dt.Rows.Add(dr);
                        }
                        isAdd = true;
                    }

                    tel = row["phone"].ToString().Trim().Replace("-", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('/'))
                        {
                            if (tel.Substring(0, 1) == "/")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split('/');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');

                            string tel1 = Regex.Replace(tels[0].Trim(), @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1].Trim(), @"[^0-9]", "").ToString();
                            if (tel1.Length >= 10 && tel2.Length == 1)
                            {
                                if (Int32.TryParse(tel1.Substring(tel1.Length - 1, 1), out int sttNum) && Int32.TryParse(tel2, out int endNum))
                                {
                                    for (int j = sttNum; j <= endNum; j++)
                                    {
                                        if (!string.IsNullOrEmpty(tels[0].Trim()))
                                        {
                                            string rsTel = tels[0].Substring(0, tels[0].Length - 1) + j.ToString();
                                            DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                            dt.Rows.Add(dr);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = GetDatarow2(dt, row, tel, idx);
                            dt.Rows.Add(dr);
                        }
                        isAdd = true;
                    }

                    tel = row["other_phone"].ToString().Trim().Replace("-", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('/'))
                        {
                            if (tel.Substring(0, 1) == "/")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split('/');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');

                            string tel1 = Regex.Replace(tels[0].Trim(), @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1].Trim(), @"[^0-9]", "").ToString();
                            if (tel1.Length >= 10 && tel2.Length > 0)
                            {
                                int sttNum = Convert.ToInt16(tel1.Substring(tel1.Length - 1, 1));
                                int endNum = Convert.ToInt16(tel2);

                                for (int j = sttNum; j <= endNum; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[0].Trim()))
                                    {
                                        string rsTel = tels[0].Substring(0, tels[0].Length - 1) + j.ToString();
                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = GetDatarow2(dt, row, tel, idx);
                            dt.Rows.Add(dr);
                        }
                        isAdd = true;
                    }


                    string registration_number = row["registration_number"].ToString().Trim().Replace("-", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(registration_number) && !isAdd)
                    {
                        DataRow dr = GetDatarow2(dt, row, "", idx);
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }
        private DataRow GetDatarow(DataTable dt, DataRow row, string tel, int idx)
        {
            DataRow dr = dt.NewRow();
            dr["id"] = row["id"].ToString();
            //dr["idx"] = idx.ToString();
            dr["company"] = row["company"].ToString();
            dr["sales_updatetime"] = row["sales_updatetime"].ToString();
            dr["current_sale_date"] = row["current_sale_date"].ToString();
            dr["seaover_company_code"] = row["seaover_company_code"].ToString();
            dr["tel"] = tel;
            dr["registration_number"] = row["registration_number"].ToString();
            dr["ato_manager"] = row["ato_manager"].ToString();
            dr["isNonHandled"] = row["isNonHandled"].ToString();
            dr["isOutBusiness"] = row["isOutBusiness"].ToString();
            dr["isNotSendFax"] = row["isNotSendFax"].ToString();
            dr["isTrading"] = row["isTrading"].ToString();
            dr["isPotential1"] = row["isPotential1"].ToString();
            dr["isPotential2"] = row["isPotential2"].ToString();
            dr["table_index"] = row["table_index"].ToString();

            //카테고리
            bool isNonHandled;
            if (!bool.TryParse(row["isNonHandled"].ToString(), out isNonHandled))
                isNonHandled = false;
            bool isOutBusiness;
            if (!bool.TryParse(row["isOutBusiness"].ToString(), out isOutBusiness))
                isOutBusiness = false;
            bool isNotSendFax;
            if (!bool.TryParse(row["isNotSendFax"].ToString(), out isNotSendFax))
                isNotSendFax = false;
            bool isTrading;
            if (!bool.TryParse(row["isTrading"].ToString(), out isTrading))
                isTrading = false;
            bool isPotential1;
            if (!bool.TryParse(row["isPotential1"].ToString(), out isPotential1))
                isPotential1 = false;
            bool isPotential2;
            if (!bool.TryParse(row["isPotential2"].ToString(), out isPotential2))
                isPotential2 = false;

            string is_duplicate;
            if (isNonHandled)
                is_duplicate = "취급X";
            else if (isOutBusiness)
                is_duplicate = "폐업";
            else if (isTrading)
                is_duplicate = "거래중(SEAOVER)";
            else if (string.IsNullOrEmpty(row["ato_manager"].ToString()))
                is_duplicate = "공용DATA";
            else if (isPotential1)
                is_duplicate = "잠재1";
            else if (isPotential2)
                is_duplicate = "잠재2";
            else
                is_duplicate = "내DATA";

            dr["is_duplicate"] = is_duplicate;

            return dr;
        }
        private DataRow GetDatarow2(DataTable dt, DataRow row, string tel, int idx)
        {
            DataRow dr = dt.NewRow();
            dr["id"] = row["id"].ToString();
            dr["idx"] = idx.ToString();
            dr["company"] = row["company"].ToString();
            dr["sales_updatetime"] = row["sales_updatetime"].ToString();
            dr["current_sale_date"] = row["current_sale_date"].ToString();
            dr["seaover_company_code"] = row["seaover_company_code"].ToString();
            dr["tel"] = tel;
            dr["registration_number"] = row["registration_number"].ToString();
            dr["ato_manager"] = row["ato_manager"].ToString();
            dr["isNonHandled"] = row["isNonHandled"].ToString();
            dr["isOutBusiness"] = row["isOutBusiness"].ToString();
            dr["isNotSendFax"] = row["isNotSendFax"].ToString();
            dr["isTrading"] = row["isTrading"].ToString();
            dr["isPotential1"] = row["isPotential1"].ToString();
            dr["isPotential2"] = row["isPotential2"].ToString();
            dr["table_index"] = row["table_index"].ToString();

            //카테고리
            bool isNonHandled;
            if (!bool.TryParse(row["isNonHandled"].ToString(), out isNonHandled))
                isNonHandled = false;
            bool isOutBusiness;
            if (!bool.TryParse(row["isOutBusiness"].ToString(), out isOutBusiness))
                isOutBusiness = false;
            bool isNotSendFax;
            if (!bool.TryParse(row["isNotSendFax"].ToString(), out isNotSendFax))
                isNotSendFax = false;
            bool isTrading;
            if (!bool.TryParse(row["isTrading"].ToString(), out isTrading))
                isTrading = false;
            bool isHide;
            if (!bool.TryParse(row["isHide"].ToString(), out isHide))
                isHide = false;
            bool isPotential1;
            if (!bool.TryParse(row["isPotential1"].ToString(), out isPotential1))
                isPotential1 = false;
            bool isPotential2;
            if (!bool.TryParse(row["isPotential2"].ToString(), out isPotential2))
                isPotential2 = false;

            string is_duplicate;
            if(isHide && isNotSendFax)
                is_duplicate = "팩스X";
            else if (isNonHandled)
                is_duplicate = "취급X";
            else if (isOutBusiness)
                is_duplicate = "폐업";
            else if (isTrading)
                is_duplicate = "거래중(SEAOVER)";
            else if (string.IsNullOrEmpty(row["ato_manager"].ToString()))
                is_duplicate = "공용DATA";
            else if (isPotential1)
                is_duplicate = "잠재1";
            else if (isPotential2)
                is_duplicate = "잠재2";
            else
                is_duplicate = "내DATA";

            dr["is_duplicate"] = is_duplicate;

            return dr;
        }
        #endregion


        #endregion

        #region 폐업확인
        private void dgvDuplicate_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            dgvDuplicate.Rows[e.RowIndex].ErrorText = "Concisely describe the error and how to fix it";
            e.Cancel = true;
        }

        private void dgvCompany_Sorted(object sender, EventArgs e)
        {
            //Seaover 표시
            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                if (i % 2 == 0)
                    dgvCompany.Rows[i].DefaultCellStyle.BackColor = Color.FloralWhite;

                if (dgvCompany.Rows[i].Cells["seaover_company_code"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["seaover_company_code"].Value.ToString()))
                {
                    dgvCompany.Rows[i].HeaderCell.Value = "S";
                    dgvCompany.Rows[i].HeaderCell.Style.ForeColor = Color.Red;
                    dgvCompany.Rows[i].HeaderCell.Style.Font = new Font("나눔고딕", 11, FontStyle.Bold);
                }
            }
            SetHeaderStyle();
        }



        private async Task GetApiAsync()
        {
            if (dgvCompany.Rows.Count == 0)
                return;

            var httpClient = new HttpClient();
            var serviceKey = "%2BSPXqSxPP9bwz%2Fgv5Dc7LCFMneAJ%2Fj%2FINtS%2ByapPN4QfMH7W81e%2Fli4cYtVZCItRwp4kFNM7wRRFPCWYabjJig%3D%3D";
            var serviceUrl = $"http://api.odcloud.kr/api/nts-businessman/v1/status?serviceKey={serviceKey}&returnType=XML";

            cbNotOutOfBusiness.Checked = false;

            ProcessBar pb = new ProcessBar(dgvCompany.Rows.Count);
            pb.Show();

            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                if (dgvCompany.Rows[i].Cells["registration_number"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["registration_number"].Value.ToString()))
                {
                    string registration_number = dgvCompany.Rows[i].Cells["registration_number"].Value.ToString().Replace("-", "").Replace(" ", "");
                    var business = new Dictionary<string, string[]>
                    {
                        ["b_no"] = new string[] { registration_number },
                    };
                    var jsonData = JsonConvert.SerializeObject(business);

                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var result = await httpClient.PostAsync(serviceUrl, content);
                    string resultXml = await result.Content.ReadAsStringAsync();
                    MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(resultXml));
                    XmlDocument xmlFile = new XmlDocument();
                    xmlFile.Load(ms);
                    XmlNodeList xmlList = xmlFile.GetElementsByTagName("BStt");
                    foreach (XmlNode item in xmlList)
                    {
                        if (item.InnerText == "폐업자")
                            dgvCompany.Rows[i].Cells["isOutBusiness"].Value = true;
                    }

                    pb.AddProcessing();
                }
            }

            pb.Close();
        }
        #endregion

        #region 복붙방지
        private void dgvCompany_MouseLeave(object sender, EventArgs e)
        {
            /*if (cbLimitCopy.Checked)
                Clipboard.Clear();*/
        }

        #endregion

        #region AtoDt Update
        public void SettingAlarm(int month, string week, DataGridView dgvAlarm)
        {
            int new_id = commonRepository.GetNextId("t_company", "id");
            foreach (DataGridViewRow row in dgvCompany.Rows)
            {
                if (Convert.ToBoolean(row.Selected))
                {
                    List<StringBuilder> sqlList = new List<StringBuilder>();
                    StringBuilder sql = new StringBuilder();

                    string alarm_txt = "";
                    string address = "";
                    
                    int company_id;
                    if (row.Cells["id"].Value == null || !Int32.TryParse(row.Cells["id"].Value.ToString(), out company_id))
                        company_id = 0;

                    //등록된 정보가 없을 경우
                    if (company_id == 0)
                    {
                        //기존ID가 없을경우 신규ID
                        CompanyModel model = new CompanyModel();
                        DataTable seaoverDt = salesRepository.GetSaleCompany("", false, "", "", "", "", "", "", false, false, row.Cells["seaover_company_code"].Value.ToString());
                        if (seaoverDt == null || seaoverDt.Rows.Count == 0)
                        {
                            messageBox.Show(this,"거래처 정보를 찾을 수 없습니다.");
                            this.Activate();
                            return;
                        }

                        company_id = new_id++;

                        model.id = company_id;
                        model.division = "매출처";
                        model.registration_number = seaoverDt.Rows[0]["사업자번호"].ToString();
                        model.name = seaoverDt.Rows[0]["거래처명"].ToString();
                        model.address = seaoverDt.Rows[0]["사업장주소"].ToString();
                        address = model.address;
                        model.ceo = seaoverDt.Rows[0]["대표자명"].ToString();
                        model.tel = seaoverDt.Rows[0]["전화번호"].ToString();
                        model.fax = seaoverDt.Rows[0]["팩스번호"].ToString();
                        model.phone = seaoverDt.Rows[0]["휴대폰"].ToString();
                        model.other_phone = seaoverDt.Rows[0]["기타연락처"].ToString();
                        model.company_manager = "";
                        model.company_manager_position = "";
                        model.email = "";
                        model.remark = seaoverDt.Rows[0]["참고사항"].ToString();
                        model.web = "";
                        model.ato_manager = seaoverDt.Rows[0]["매출자"].ToString();
                        model.updatetime = "1900-01-01 00:00:00";
                        model.edit_user = um.user_name;
                        model.isNotSendFax = false;
                        model.seaover_company_code = seaoverDt.Rows[0]["거래처코드"].ToString();
                        //영업알람
                        model.alarm_month = month;
                        model.alarm_week = week;

                        //추가정보
                        model.group_name = "";
                        model.origin = "국내";
                        model.isManagement1 = false;
                        model.isManagement2 = false;
                        model.isManagement3 = false;
                        model.isManagement4 = false;
                        model.isHide = false;
                        model.isDelete = false;
                        model.isTrading = true;
                        model.createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        //거래처 정보 등록
                        sql = companyRepository.InsertCompany(model);
                        sqlList.Add(sql);

                        //거래처 영업내용============================================================================
                        CompanySalesModel sModel = new CompanySalesModel();
                        sModel.company_id = company_id;
                        sModel.sub_id = 1;
                        sModel.is_sales = false;
                        sModel.contents = "거래처 등록";
                        sModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        sModel.pre_ato_manager = "";
                        sModel.edit_user = um.user_name;
                        sql = salesPartnerRepository.InsertPartnerSales(sModel);
                        sqlList.Add(sql);
                    }

                    //특정일자
                    if (dgvAlarm.Rows.Count > 0)
                    {
                        sql = companyAlarmRepository.DeleteAlarm(company_id.ToString());
                        sqlList.Add(sql);
                        foreach (DataGridViewRow alarmRow in dgvAlarm.Rows)
                        {
                            CompanyAlarmModel alarmModel = new CompanyAlarmModel();
                            alarmModel.company_id = company_id;
                            alarmModel.division = alarmRow.Cells["alarm_division"].Value.ToString();
                            alarmModel.category = alarmRow.Cells["alarm_category"].Value.ToString();
                            if (alarmModel.category == "주알람")
                            {
                                switch (alarmRow.Cells["alarm_date"].Value.ToString())
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
                                alarmModel.alarm_date = alarmRow.Cells["alarm_date"].Value.ToString();

                            if (alarmRow.Cells["alarm_remark"].Value == null)
                                alarmRow.Cells["alarm_remark"].Value = string.Empty;
                            alarmModel.alarm_remark = alarmRow.Cells["alarm_remark"].Value.ToString();
                            alarmModel.edit_user = alarmRow.Cells["edit_user"].Value.ToString();
                            alarmModel.updatetime = alarmRow.Cells["updatetime"].Value.ToString();
                            alarmModel.alarm_complete = false;

                            sql = companyAlarmRepository.InsertAlarm(alarmModel);
                            sqlList.Add(sql);

                            //if (row.Cells["real_alarm_date"].Value != null && DateTime.TryParse(row.Cells["real_alarm_date"].Value.ToString(), out DateTime real_alarm_date))
                                alarm_txt += $" {alarmModel.alarm_date}_{alarmModel.category}_{alarmModel.division}_FALSE";
                        }
                        alarm_txt = alarm_txt.Trim().Replace("\n", ",");
                    }
                    //===========================================================================================
                    //Execute
                    if (commonRepository.UpdateTran(sqlList) == -1)
                    {
                        messageBox.Show(this,"등록중 에러가 발생했습니다.");
                        this.Activate();
                        return;
                    }
                    else
                    {
                        int table_index = Convert.ToInt32(row.Cells["table_index"].Value.ToString());
                        atoDt.Rows[table_index]["id"] = company_id;
                        atoDt.Rows[table_index]["alarm_date"] = alarm_txt;
                        atoDt.Rows[table_index]["alarm_week"] = week;
                        atoDt.Rows[table_index]["alarm_month"] = month;
                        this.dgvCompany.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
                        row.Cells["id"].Value = company_id;
                        this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
                    }
                }
            }
        }

        public void CheckCategory(string category, bool isNotSendFax, int rowindex)
        {
            if (!string.IsNullOrEmpty(category))
            {
                if (tcMain.SelectedTab.Name == "tabAllMyCompany" && (category == "tabNonehandled" || category == "tabCommonData"))
                {
                    dgvCompany.Rows.Remove(dgvCompany.Rows[rowindex]);
                    dgvCompany.Update();
                }
                else if (tcMain.SelectedTab.Name == "tabNotSendFax" && !isNotSendFax)
                {
                    dgvCompany.Rows.Remove(dgvCompany.Rows[rowindex]);
                    dgvCompany.Update();
                }
                else if (tcMain.SelectedTab.Name != category)
                {
                    dgvCompany.Rows.Remove(dgvCompany.Rows[rowindex]);
                    dgvCompany.Update();
                }
            }
        }
        public void UpdateAtoDt(int rowIndex, string col_name, string col_val)
        {
            if (atoDt != null & atoDt.Rows.Count > 0)
                atoDt.Rows[rowIndex][col_name] = col_val;
            atoDt.AcceptChanges();
        }
        public void UpdateAtoDt(int rowIndex, string col_name, bool col_val)
        {
            if (atoDt != null & atoDt.Rows.Count > 0)
            {
                atoDt.Rows[rowIndex][col_name] = col_val;
                atoDt.AcceptChanges();
            }
        }

        public void UpdateAtoDt(int rowIndex, string col_name, int col_val)
        {
            if (atoDt != null & atoDt.Rows.Count > 0)
            {
                atoDt.Rows[rowIndex][col_name] = col_val;
                atoDt.AcceptChanges();
            }
        }

        public void UpdateAtoDt(List<CompanyModel> list)
        {
            foreach (CompanyModel model in list)
            {
                string[] table_indexs = model.table_index.Split(' ');
                if (table_indexs.Length > 0)
                {
                    foreach (string table_index in table_indexs)
                    {
                        if (int.TryParse(table_index, out int rowindex))
                        {
                            atoDt.Rows[rowindex]["isNotSendFax"] = model.isNotSendFax;
                            //atoDt.Rows[rowindex]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                }
            }
        }
        public void DeleteAtoDt(List<CompanyModel> list)
        {
            foreach (CompanyModel model in list)
            {
                string[] table_indexs = model.table_index.Split(' ');
                if (table_indexs.Length > 0)
                {
                    foreach (string table_index in table_indexs)
                    {
                        if (int.TryParse(table_index, out int rowindex))
                        {
                            atoDt.Rows[rowindex]["isDelete"] = true;
                            atoDt.Rows[rowindex]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                }
            }
        }


        public void InsertAtoDt(List<CompanyModel> list)
        {
            int table_index = atoDt.Rows.Count;


            foreach (CompanyModel model in list)
            { 
                DataRow newDr = atoDt.NewRow();
                newDr["id"] = model.id;
                newDr["company"] = model.name;
                newDr["group_name"] = model.group_name;
                newDr["registration_number"] = model.registration_number;
                newDr["seaover_company_code"] = model.seaover_company_code;
                newDr["origin"] = model.origin;
                newDr["address"] = model.address;
                newDr["ceo"] = model.ceo;

                newDr["tel"] = model.tel;
                newDr["fax"] = model.fax;
                newDr["phone"] = model.phone;
                newDr["other_phone"] = model.other_phone;

                newDr["company_manager"] = model.company_manager;
                newDr["company_manager_position"] = model.company_manager_position;

                newDr["email"] = model.email;
                newDr["remark"] = model.remark;
                newDr["web"] = model.web;

                newDr["distribution"] = model.distribution;
                newDr["handling_item"] = model.handling_item;

                newDr["isHide"] = model.isHide;
                newDr["isDelete"] = model.isDelete;

                newDr["createtime"] = model.createtime;
                //newDr["updatetime"] = model.updatetime;

                newDr["ato_manager"] = model.ato_manager;

                newDr["isPotential1"] = model.isPotential1;
                newDr["isPotential2"] = model.isPotential2;
                newDr["isTrading"] = model.isTrading;
                newDr["isNonHandled"] = model.isNonHandled;
                newDr["isNotSendFax"] = model.isNotSendFax;
                newDr["isOutBusiness"] = model.isOutBusiness;

                newDr["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                newDr["sales_contents"] = "거래처 등록";
                newDr["sales_edit_user"] = um.user_name;
                newDr["industry_type"] = model.industry_type;

                newDr["table_index"] = table_index++;

                atoDt.Rows.Add(newDr);
            }
        }
        #endregion

        #region imm32.dll :: Get_IME_Mode IME가져오기
        [DllImport("imm32.dll")]
        public static extern IntPtr ImmGetContext(IntPtr hWnd);
        [DllImport("imm32.dll")]
        public static extern Boolean ImmSetConversionStatus(IntPtr hIMC, Int32 fdwConversion, Int32 fdwSentence);
        [DllImport("imm32.dll")]
        private static extern IntPtr ImmGetDefaultIMEWnd(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr IParam);

        public const int IME_CMODE_ALPHANUMERIC = 0x0;   //영문
        public const int IME_CMODE_NATIVE = 0x1;         //한글
        #endregion

        #region user32.dll :: keybd_event CapsLock 변경
        [DllImport("user32.dll")]
        public static extern short GetKeyState(int keyCode);
        [DllImport("user32.dll")]
        public static extern void keybd_event(uint vk, uint scan, uint flags, uint extraInfo);

        private void cbLimitCopy_CheckedChanged(object sender, EventArgs e)
        {
            dgvCompany.CopyEnabled(!cbLimitCopy.Checked);
        }
        private void rbAlarmAll_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rbAlarmAll.Checked)
                GetData();
            else if(rbAlarmComplete.Checked)
                GetData();
            else if (rbAlarmIncomplete.Checked)
                GetData();
        }

        
        private void SalesManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dgvCompany.Columns.Count > 1)
            {
                cookie = new LoginCookie(@"C:\Cookies\TEMP\SALESMANAGER\" + um.user_id, "SETTING", null);
                StyleSettingTxt();
                cookie.SaveSalesManagerSetting(styleDic);
            }
        }

        
        #endregion

        #region User_Fn
        /// <summary>
        /// [한/영]전환 true=한글, false=영어
        /// </summary>
        /// <param name="b_toggle"></param>
        private void ChangeIME(bool b_toggle)
        {
            IntPtr hwnd = ImmGetContext(this.Handle);  //C# WindowForm만 적용됨.
            // [한/영]전환 b_toggle : true=한글, false=영어
            Int32 dwConversion = (b_toggle == true ? IME_CMODE_NATIVE : IME_CMODE_ALPHANUMERIC);
            ImmSetConversionStatus(hwnd, dwConversion, 0);
        }

        


        /// <summary>
        /// [Caps Lock]전환 true=ON, false=OFF
        /// </summary>
        /// <param name="b_toggle"></param>
        private void ChangeCAP(bool b_toggle)
        {
            if ((GetKeyState((int)Keys.CapsLock) & 0xffff) == 0 && b_toggle == true)  // if = 소문자 -> 대문자
            {
                keybd_event((byte)Keys.CapsLock, 0, 0, 0);
                keybd_event((byte)Keys.CapsLock, 0, 2, 0);
            }
            else if ((GetKeyState((int)Keys.CapsLock) & 0xffff) != 0 && b_toggle == false)  // if = 대문자 -> 소문자
            {
                keybd_event((byte)Keys.CapsLock, 0, 0, 0);
                keybd_event((byte)Keys.CapsLock, 0, 2, 0);
            }
        }
        #endregion

        #region 팩스 금지거래처 등록 Method

        static Dictionary<string, int> duplicateColumnDic = new Dictionary<string, int>();
        public static DataTable ExcelToDataTable(string filePath)
        {
            duplicateColumnDic = new Dictionary<string, int>();
            DataTable dt = new DataTable();
            IWorkbook workbook;

            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // 파일 확장자에 따라 적절한 Workbook 생성
                if (filePath.EndsWith(".xls"))
                {
                    workbook = new HSSFWorkbook(stream); // .xls 파일용
                }
                else if (filePath.EndsWith(".xlsx"))
                {
                    workbook = new XSSFWorkbook(stream); // .xlsx 파일용
                }
                else
                {
                    throw new Exception("Invalid file extension");
                }

                ISheet sheet = workbook.GetSheetAt(0); // 첫 번째 시트 가져오기
                IRow headerRow = sheet.GetRow(0); // 첫 번째 행을 헤더로 사용
                int cellCount = headerRow.LastCellNum;

                // 헤더를 기준으로 DataTable의 컬럼 생성

                for (int j = 0; j < cellCount; j++)
                {
                    string col_name;

                    if(headerRow.GetCell(j) == null || string.IsNullOrEmpty(headerRow.GetCell(j).ToString().Trim()))
                        col_name = "NULL";
                    else
                        col_name = headerRow.GetCell(j).ToString();


                    if (!dt.Columns.Contains(col_name))
                        dt.Columns.Add(col_name);
                    else
                    {
                        int duplicate_cnt;
                        if (duplicateColumnDic.ContainsKey(col_name))
                        {
                            duplicate_cnt = duplicateColumnDic[col_name];
                            dt.Columns.Add(col_name + "_" + ++duplicate_cnt);
                        }
                        else
                        {
                            duplicate_cnt = 0;
                            duplicateColumnDic.Add(col_name, duplicate_cnt);
                            dt.Columns.Add(col_name + "_" + duplicate_cnt);
                            duplicate_cnt++;
                        }
                    }
                }

                // 데이터 로우를 DataTable에 추가
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue; // 빈 행은 스킵
                    DataRow dataRow = dt.NewRow();

                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = row.GetCell(j).ToString();
                    }

                    dt.Rows.Add(dataRow);
                }
            }

            return dt;
        }
        public static string ChooseExcelFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // 필터 설정 (Excel 파일만 표시)
                openFileDialog.Filter = "Excel Files|*.xls*;*.csv;";
                openFileDialog.Title = "Select an Excel File";

                // 대화 상자를 표시하고 결과 확인
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 선택된 파일의 경로 반환
                    return openFileDialog.FileName;
                }
                else
                {
                    // 취소되면 빈 문자열 반환
                    return string.Empty;
                }
            }
        }
        private void SearchingData()
        {
            dgvData.DataSource = null;
            if (this.excelDt != null && this.excelDt.Rows.Count > 0)
            {
                string whr = "";
                for (int i = 0; i < dgvSearching.Rows.Count; i++)
                {
                    if (dgvSearching.Rows[i].Cells["search_text"].Value != null
                        && !string.IsNullOrEmpty(dgvSearching.Rows[i].Cells["search_text"].Value.ToString()))
                    {
                        string column_name = dgvSearching.Rows[i].Cells["column_name"].Value.ToString();
                        string search_text = dgvSearching.Rows[i].Cells["search_text"].Value.ToString();
                        if (search_text.Contains('^'))
                        {
                            string temp = "";
                            string[] andTxt = search_text.Split('^');
                            foreach (string txt in andTxt)
                            {
                                if (!string.IsNullOrEmpty(txt.Trim()))
                                {
                                    if (string.IsNullOrEmpty(temp))
                                        temp = column_name + $" LIKE '%{txt}%'";
                                    else
                                        temp += " AND " + column_name + $" LIKE '%{txt}%'";
                                }
                            }

                            if (!string.IsNullOrEmpty(temp))
                            {
                                temp = "(" + temp + ")";
                                if (string.IsNullOrEmpty(whr))
                                    whr = temp;
                                else
                                    whr += " AND " + temp;
                            }
                        }
                        else
                        {
                            string temp = "";
                            string[] orTxt = search_text.Split(' ');
                            foreach (string txt in orTxt)
                            {
                                if (!string.IsNullOrEmpty(txt.Trim()))
                                {
                                    if (string.IsNullOrEmpty(temp))
                                        temp = column_name + $" LIKE '%{txt}%'";
                                    else
                                        temp += " OR " + column_name + $" LIKE '%{txt}%'";
                                }
                            }

                            if (!string.IsNullOrEmpty(temp))
                            {
                                temp = "(" + temp + ")";
                                if (string.IsNullOrEmpty(whr))
                                    whr = temp;
                                else
                                    whr += " AND " + temp;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(whr))
                {
                    DataRow[] dr = this.excelDt.Select(whr);
                    if (dr.Length > 0)
                    {
                        DataTable searchDt = dr.CopyToDataTable();
                        dgvData.DataSource = searchDt;
                    }
                }
                else
                    dgvData.DataSource = this.excelDt;

                dgvData.Columns["is_not_send_fax"].HeaderText = "금지";
                dgvData.Columns["is_not_send_fax"].Width = 40;

                foreach (DataGridViewColumn col in dgvData.Columns)
                    col.SortMode = DataGridViewColumnSortMode.Automatic;

                txtTotalCount.Text = dgvData.Rows.Count.ToString("#,##0");
            }
        }
        public void GetNotSendFaxData()
        {
            dgvNotSendFax.Rows.Clear();
            DataTable notSendFaxDt = notSendFaxRepository.GetNotSendFax(txtNotSendFax.Text);
            if(notSendFaxDt != null && notSendFaxDt.Rows.Count > 0)
            {
                int limit_count;
                if (!int.TryParse(cbDataCount.Text, out limit_count))
                    limit_count = notSendFaxDt.Rows.Count - 1;


                for (int i = 0; i <= limit_count; i++)
                {
                    int n = dgvNotSendFax.Rows.Add();
                    dgvNotSendFax.Rows[n].Cells["fax_number"].Value = notSendFaxDt.Rows[i]["fax"].ToString();
                }
            }
        }

        #region 데이터 중복검사

        private DataTable SetFaxDataDt(DataTable faxDt, string fax_column)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("table_index", typeof(string));
            dt.Columns.Add("fax", typeof(string));
            if (faxDt != null && faxDt.Rows.Count > 0)
            {
                for (int i = 0; i < faxDt.Rows.Count; i++)
                {
                    DataRow row = faxDt.Rows[i];
                    string fax = ValidationTelString(row[fax_column].ToString().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", ""));
                    if (!string.IsNullOrEmpty(fax))
                    {
                        if (fax.Contains(','))
                        {
                            if (fax.Substring(0, 1) == ",")
                                fax = fax.Substring(1, fax.Length - 1);

                            string[] tels = fax.Split(',');
                            if (tels.Length == 1)
                            {
                                DataRow dr = SetFaxDataRow(dt, fax, i);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

                                        DataRow dr = SetFaxDataRow(dt, rsTel, i);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (fax.Contains('/'))
                        {
                            if (fax.Substring(0, 1) == "/")
                                fax = fax.Substring(1, fax.Length - 1);

                            string[] tels = fax.Split('/');
                            if (tels.Length == 1)
                            {
                                DataRow dr = SetFaxDataRow(dt, fax, i);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

                                        DataRow dr = SetFaxDataRow(dt, rsTel, i);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (fax.Contains('~'))
                        {
                            string[] tels = fax.Split('~');
                            string tel1 = Regex.Replace(tels[0].Trim(), @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1].Trim(), @"[^0-9]", "").ToString();
                            if (tel1.Length > 0 && tel2.Length > 0 && tel1.Length > tel2.Length)
                            {
                                int sttNum = Convert.ToInt16(tel1.Substring(tel1.Length - 1, 1));
                                int endNum = Convert.ToInt16(tel2);

                                for (int j = sttNum; j <= endNum; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[0].Trim()))
                                    {
                                        string rsTel = tels[0].Substring(0, tels[0].Length - 1) + j.ToString();
                                        DataRow dr = SetFaxDataRow(dt, rsTel, i);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = SetFaxDataRow(dt, fax, i);
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = SetFaxDataRow(dt, fax, i);
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            return dt;
        }
        private DataRow SetFaxDataRow(DataTable dt, string tel, int table_index)
        {
            DataRow dr = dt.NewRow();
            dr["fax"] = tel;
            dr["table_index"] = table_index.ToString();
            return dr;
        }

        #endregion

        #region Excel 저장하기
        object[,] values = null;

        private void btnSaveToExcel_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "영업거래처 관리", "거래처 관리", "is_excel"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            if (messageBox.Show(this, "데이터중 팩스 금지거래처를 제외한 데이터만 Excel파일로 만들겠습니다.", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                values = GetValues();
                CreateDatatoExcel(values);
            }
        }

        private void dgvData_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvData.CurrentCell != null)
                txtCurrentIndex.Text = (dgvData.CurrentCell.RowIndex + 1).ToString("#,##0");
            else if (dgvData.CurrentRow != null)
                txtCurrentIndex.Text = (dgvData.CurrentRow.Index + 1).ToString("#,##0");
        }

        private object[,] GetValues()
        {
            // 데이터 배열 크기 결정
            int cols = dgvData.Columns.Count;
            int rows = 1;

            foreach (DataGridViewRow dr in dgvData.Rows)
            {
                bool is_not_send_fax;
                if (dr.Cells["is_not_send_fax"].Value == null || !bool.TryParse(dr.Cells["is_not_send_fax"].Value.ToString(), out is_not_send_fax))
                    is_not_send_fax = false;
                if (!is_not_send_fax)
                    rows++;
            }

            // 데이터 배열 생성
            object[,] values = new object[rows, cols];


            int colIndex = 0;
            // First row for column names
            for (int i = 0; i < cols; i++)
            {
                values[0, colIndex++] = dgvData.Columns[i].Name;
            }

            try
            {
                // Data rows
                int idx = 1;
                for (int i = 0; i < dgvData.Rows.Count; i++)
                {
                    DataGridViewRow dr = dgvData.Rows[i];
                    bool is_not_send_fax;
                    if (dr.Cells["is_not_send_fax"].Value == null || !bool.TryParse(dr.Cells["is_not_send_fax"].Value.ToString(), out is_not_send_fax))
                        is_not_send_fax = false;
                    if (!is_not_send_fax)
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            values[idx, j] = dgvData.Rows[i].Cells[j].Value;
                        }
                        idx++;
                    }
                }
            }
            catch (Exception ex)
            {
                messageBox.Show(this, ex.Message);
            }

            return values;
        }
        private void CreateDatatoExcel(object[,] val)
        {
            if (val != null)
            {
                int values_col = dgvData.Columns.Count;

                int values_row = 1;

                foreach (DataGridViewRow dr in dgvData.Rows)
                {
                    bool is_not_send_fax;
                    if (dr.Cells["is_not_send_fax"].Value == null || !bool.TryParse(dr.Cells["is_not_send_fax"].Value.ToString(), out is_not_send_fax))
                        is_not_send_fax = false;
                    if (!is_not_send_fax)
                        values_row++;
                }


                // Excel 애플리케이션 및 워크북 초기화
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = excelApp.Workbooks.Add();
                Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Sheets[1];

                // Excel 파일에 데이터 쓰기
                Microsoft.Office.Interop.Excel.Range startCell = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, 1]; // 데이터 시작 위치 설정
                Microsoft.Office.Interop.Excel.Range endCell = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[values_row, values_col]; // 데이터 끝 위치 설정
                Microsoft.Office.Interop.Excel.Range writeRange = worksheet.Range[startCell, endCell];

                writeRange.Value2 = val; // 한 번에 모든 데이터 쓰기

                // Excel 애플리케이션을 보이게 설정
                excelApp.Visible = true;

                // 사용자가 Excel을 제어할 수 있게 설정
                excelApp.UserControl = true;
            }
        }
        #endregion

        #endregion
    }
}
