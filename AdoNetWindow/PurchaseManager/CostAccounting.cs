using AdoNetWindow.Common;
using AdoNetWindow.DashboardForSales;
using AdoNetWindow.Model;
using AdoNetWindow.Product;
using AdoNetWindow.SEAOVER;
using AdoNetWindow.SEAOVER.PriceComparison;
using AdoNetWindow.SEAOVER.ProductCostComparison;
using Repositories;
using Repositories.Config;
using Repositories.Group;
using Repositories.SEAOVER;
using Repositories.SEAOVER.Purchase;
using Repositories.SEAOVER.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace AdoNetWindow.PurchaseManager
{
    public partial class CostAccounting : Form
    {
        // Thread Process
        BackgroundWorker thdProcess = null;
        //Process value
        int maxValue;
        int preValue;
        int curValue;
        //Repository
        Libs.Tools.Common common = new Libs.Tools.Common();
        ICustomsRepository customsRepository = new CustomsRepository();
        IPurchaseRepository purchaseRepository = new PurchaseRepository();
        ICommonRepository commonRepository = new CommonRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        ICompanyRepository companyRepository = new CompanyRepository();
        IPurchasePriceRepository purchasePriceRepository = new PurchasePriceRepository();
        IProductOtherCostRepository productOtherCostRepository = new ProductOtherCostRepository();
        IPriceComparisonRepository priceComparisonRepository = new PriceComparisonRepository();
        IGroupRepository groupRepository = new GroupRepository();
        ISalesRepository salesRepository = new SalesRepository();
        IProductGroupRepository productGroupRepository = new ProductGroupRepository();

        List<DataGridViewRow> clipboardList = new List<DataGridViewRow>();

        UsersModel um;
        DataTable productDt = new DataTable();
        DataTable companyDt = new DataTable();
        DataTable stockSalesDt = new DataTable();

        int group_id;
        int workDays;
        bool isBookmarkMode = false;
        bool isContextstripmenu = false;
        //Excel
        static Microsoft.Office.Interop.Excel.Application excelApp = null;
        static Microsoft.Office.Interop.Excel.Workbook workBook = null;
        static Microsoft.Office.Interop.Excel.Worksheet workSheet = null;

        CostAccountingGroup cag = null;

        public CostAccounting(UsersModel uModel, CostAccountingGroup cagGroup = null, string product = "", string origin = "", string sizes = "", string company = "", bool isExactly = false)
        {
            InitializeComponent();
            um = uModel;
            cag = cagGroup;

            txtProduct.Text = product;
            txtOrigin.Text = origin;
            txtSizes.Text = sizes;
            txtCompany.Text = company;
            cbExactly.Checked = isExactly;

            //시작일~종료일
            txtSttdate.Text = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }


        private void CostAccounting_Load(object sender, EventArgs e)
        {
            //재고,매출테이블
            CallStockProcedure();
            //시작일~종료일
            txtSttdate.Text = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            string sttdate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
            string enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            //영업일수
            common.GetWorkDay(DateTime.Now.AddDays(-1).AddMonths(-6), DateTime.Now.AddDays(-1), out workDays);
            //환율가져오기
            double usdRate = common.GetExchangeRateKEBBank("USD");
            txtUsd.Text = usdRate.ToString("#,##0.00");
            double eurRate = common.GetExchangeRateKEBBank("EUR");
            txtEur.Text = eurRate.ToString("#,##0.00");
            //Get Trq
            double trq = commonRepository.GetTrq();
            txtTrq.Text = trq.ToString("#,##0");
            //Column header style setting
            SetColumn();
            //Seaover 사번이 없는경우 수정
            if (um.seaover_id == null || string.IsNullOrEmpty(um.seaover_id))
            {
                MessageBox.Show(this, "내정보에서 SEAOVER 사번을 입력해주세요.");
                Config.EditMyInfo emi = new Config.EditMyInfo(um);
                um = emi.UpdateSeaoverId();
            }
            if (seaoverRepository.CallStoredProcedure(um.seaover_id, sttdate, enddate) == 0)
            {
                MessageBox.Show(this, "호출 내용이 없음");
                this.Activate();
                return;
            }
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "원가계산", "is_excel"))
                    btnExcel.Visible = false;
                if (!common.CheckAuthority(authorityDt, "수입관리", "원가계산", "is_print"))
                    btnPreview.Visible = false;
            }

            //품목정보, 거래처정보
            productDt = seaoverRepository.GetOneColumn("품명", "품명", txtProduct.Text);
            companyDt = commonRepository.SelectAsOneLike("t_company", "name", "name", txtCompany.Text);
            txtProduct.Focus();
        }

        #region Method
        public List<DataGridViewRow> GetClipboard()
        {
            return clipboardList;
        }
        public void SetSelectTerm(string sdate, string edate)
        {
            //시작일~종료일
            txtSttdate.Text = sdate;
            txtEnddate.Text = edate;
        }
        private void SetProductPerCompany()
        {
            if (cag == null)
            {
                double trq;
                if (!double.TryParse(txtTrq.Text.Replace(",", ""), out trq))
                    trq = 0;
                cag = new CostAccountingGroup(um, trq);
            }

            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "CostAccountingGroup")
                    isFormActive = true;
            }


            if (!isFormActive)
            {
                double trq;
                if (!double.TryParse(txtTrq.Text.Replace(",", ""), out trq))
                    trq = 0;
                cag = new CostAccountingGroup(um, trq);
            }
                

            List<DataGridViewRow> productList = new List<DataGridViewRow>();
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                DataGridViewRow row = dgvProduct.Rows[i];
                bool isCheck = Convert.ToBoolean(row.Selected);
                if (isCheck)
                    productList.Add(row);
            }
            double exchange_rate = 0;
            if (rbUsd.Checked && !double.TryParse(txtUsd.Text, out exchange_rate))
                exchange_rate = 0;
            else if (rbEur.Checked && !double.TryParse(txtEur.Text, out exchange_rate))
                exchange_rate = 0;
            /*if (exchange_rate == 0)
                exchange_rate = common.GetExchangeRateKEBBank("USD");*/

            cag.SetProductPerCompany(productList, rbPurchaseprice.Checked, exchange_rate);
            cag.Show();   
        }
        private void Thread_Process(object sender, DoWorkEventArgs e)
        {
            Common.FormProcess process = new Common.FormProcess(maxValue);

            int mainFormX = this.Location.X;
            int mainFormY = this.Location.Y;
            int mainFormWidth = this.Size.Width;
            int mainFormHeight = this.Size.Height;

            int childFormWidth = process.Size.Width;
            int childFormHeight = process.Size.Height;

            process.StartPosition = FormStartPosition.CenterParent;
            process.Show();
            process.Location = new Point(mainFormX + (mainFormWidth / 2) - (childFormWidth / 2), mainFormY + (mainFormHeight / 2) - (childFormHeight / 2));

            for (int i = 0; i < 1000000000; i++)
            {
                Thread.Sleep(100);
                if (preValue != curValue)
                    process.SetProgress(curValue);  //Do Something
                else
                    break;
            }
            process.Close();
        }
        //매입단가 조회 -> 원가계산
        public void AddProduct(string sdate, string product, string origin, string sizes, string unit, string company)
        {
            
            DateTime enddate = Convert.ToDateTime(sdate);
            DateTime sttdate = enddate.AddDays(-15);
            txtEnddate.Text = enddate.ToString("yyyy-MM-dd");
            txtSttdate.Text = sttdate.ToString("yyyy-MM-dd");

            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            //Seaover 품목 단가정보
            /*DataTable sDt = seaoverRepository.GetPriceForCostAccount2(sttdate, enddate, product, origin, sizes, unit);*/
            DataTable sDt = seaoverRepository.GetPriceForCostAccount(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"), product, origin, sizes, unit);
            //Seaover 품목 매입단가정보
            DataTable ppDt = seaoverRepository.GetCostCalculateForPurchasePrice(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"), product, origin, sizes, unit);
            //Seaover 품목 매입단가 없을 경우
            DataTable npDt = seaoverRepository.GetCostCalculateForPurchasePrice("", "", product, origin, sizes, unit);
            //단가정보 
            DataTable pDt = purchasePriceRepository.GetCostAccounting2(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"), product, origin, sizes, unit, company);
            if (pDt.Rows.Count > 0)
            {
                //재고, 판매량
                stockSalesDt = priceComparisonRepository.GetCostAccountingProductInfo(product, origin, sizes);
                string whr;
                DataRow[] dtRow;
                for (int i = 0; i < pDt.Rows.Count; i++)
                {
                    int n = dgvProduct.Rows.Add();
                    dgvProduct.Rows[n].Cells["company"].Value = pDt.Rows[i]["cname"];
                    dgvProduct.Rows[n].Cells["product"].Value = pDt.Rows[i]["product"];
                    dgvProduct.Rows[n].Cells["origin"].Value = pDt.Rows[i]["origin"];
                    dgvProduct.Rows[n].Cells["sizes"].Value = pDt.Rows[i]["sizes"];
                    dgvProduct.Rows[n].Cells["box_weight"].Value = pDt.Rows[i]["unit"];
                    dgvProduct.Rows[n].Cells["cost_unit"].Value = pDt.Rows[i]["cost_unit"];
                    dgvProduct.Rows[n].Cells["origin_cost_unit"].Value = pDt.Rows[i]["cost_unit"];
                    dgvProduct.Rows[n].Cells["unit_price"].Value = pDt.Rows[i]["purchase_price"];
                    dgvProduct.Rows[n].Cells["updatetime"].Value = Convert.ToDateTime(pDt.Rows[i]["updatetime"]).ToString("yyyy-MM-dd");
                    if(rbUsd.Checked)
                        dgvProduct.Rows[n].Cells["exchange_rate"].Value = txtUsd.Text;
                    else
                        dgvProduct.Rows[n].Cells["exchange_rate"].Value = txtEur.Text;

                    dgvProduct.Rows[n].Cells["custom"].Value = pDt.Rows[i]["custom"];
                    dgvProduct.Rows[n].Cells["tax"].Value = pDt.Rows[i]["tax"];
                    dgvProduct.Rows[n].Cells["incidental_expense"].Value = pDt.Rows[i]["incidental_expense"];

                    bool weight_calculate = Convert.ToBoolean(pDt.Rows[i]["weight_calculate"].ToString());
                    bool tray_calculate = Convert.ToBoolean(pDt.Rows[i]["tray_calculate"].ToString());

                    dgvProduct.Rows[n].Cells["weight_calculate"].Value = weight_calculate;
                    dgvProduct.Rows[n].Cells["tray_calculate"].Value = !weight_calculate;

                    bool is_FOB = Convert.ToBoolean(pDt.Rows[i]["is_FOB"].ToString());
                    dgvProduct.Rows[n].Cells["is_CFR"].Value = !is_FOB;
                    dgvProduct.Rows[n].Cells["is_FOB"].Value = is_FOB;

                    /*if (weight_calculate == tray_calculate)
                        dgvProduct.Rows[n].Cells["weight_calculate"].Value = true;
                    else
                    {
                        dgvProduct.Rows[n].Cells["weight_calculate"].Value = weight_calculate;
                        dgvProduct.Rows[n].Cells["tray_calculate"].Value = tray_calculate;
                    }*/
                }
            }



            //대표품목 
            DataTable pgDt = productGroupRepository.GetProductGroup("", "", "", "", txtProduct.Text, txtOrigin.Text, txtSizes.Text, "");
            //매출단가, 매입단가
            DateTime sttDate;
            if (!DateTime.TryParse(txtSttdate.Text, out sttDate))
            {
                MessageBox.Show(this, "시작일 값이 올바르지 않은 날짜 형식입니다.");
                this.Activate();
                return;
            }
            DateTime endDate;
            if (!DateTime.TryParse(txtEnddate.Text, out endDate))
            {
                MessageBox.Show(this, "종료일 값이 올바르지 않은 날짜 형식입니다.");
                this.Activate();
                return;
            }
            //대표품목 
            sDt = seaoverRepository.GetPriceForCostAccount(sttDate.AddMonths(-3).ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
            //팬딩내역
            DataTable uppDt = customsRepository.GetUnpendingProduct2(txtProduct.Text, txtOrigin.Text, txtSizes.Text, "", false, false);
            //원가계산
            DataTable productDt = priceComparisonRepository.GetNotSalesCostProduct(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
            SetPrice(1, pgDt, sDt, uppDt, productDt);
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }

        //팬딩조회 -> 원가계산
        public void AddProduct2(List<string[]> list)
        {
            //영업일수
            common.GetWorkDay(DateTime.Now.AddDays(-1).AddMonths(-6), DateTime.Now.AddDays(-1), out workDays);
            //기간
            txtSttdate.Text = DateTime.Now.AddDays(-14).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            if (list != null && list.Count > 0)
            {
                //재고, 판매량
                CallStockProcedure();
                stockSalesDt = priceComparisonRepository.GetCostAccountingProductInfo("", "", "");
                //단가정보 
                DataTable pDt = productOtherCostRepository.GetProduct("", false,  "", "", "");
                //환율
                if (string.IsNullOrEmpty(txtUsd.Text) || !double.TryParse(txtUsd.Text, out double usd))
                    txtUsd.Text = common.GetExchangeRateKEBBank("USD").ToString();
                if (string.IsNullOrEmpty(txtEur.Text) || !double.TryParse(txtEur.Text, out double eur))
                    txtEur.Text = common.GetExchangeRateKEBBank("EUR").ToString();
                //데이터 출력
                for (int i = 0; i < list.Count; i++)
                {
                    int n = dgvProduct.Rows.Add();

                    dgvProduct.Rows[n].Cells["product"].Value = list[i][0];
                    dgvProduct.Rows[n].Cells["origin"].Value = list[i][1];
                    dgvProduct.Rows[n].Cells["sizes"].Value = list[i][2];
                    dgvProduct.Rows[n].Cells["box_weight"].Value = list[i][3];
                    dgvProduct.Rows[n].Cells["cost_unit"].Value = list[i][4];
                    dgvProduct.Rows[n].Cells["origin_cost_unit"].Value = list[i][4];

                    dgvProduct.Rows[n].Cells["tax"].Value = list[i][5];
                    dgvProduct.Rows[n].Cells["custom"].Value = list[i][6];
                    dgvProduct.Rows[n].Cells["incidental_expense"].Value = list[i][7];

                    dgvProduct.Rows[n].Cells["unit_price"].Value = list[i][9];
                    dgvProduct.Rows[n].Cells["assort"].Value = list[i][10];
                    dgvProduct.Rows[n].Cells["company"].Value = list[i][11];

                    bool weight_calculate = Convert.ToBoolean(list[i][12]);
                    bool tray_calculate = Convert.ToBoolean(list[i][13]);
                    if (weight_calculate == tray_calculate)
                        dgvProduct.Rows[n].Cells["weight_calculate"].Value = true;
                    else
                    {
                        dgvProduct.Rows[n].Cells["weight_calculate"].Value = weight_calculate;
                        dgvProduct.Rows[n].Cells["tray_calculate"].Value = tray_calculate;
                    }

                    if(rbUsd.Checked)
                        dgvProduct.Rows[n].Cells["exchange_rate"].Value = txtUsd.Text;
                    else
                        dgvProduct.Rows[n].Cells["exchange_rate"].Value = txtEur.Text;
                    dgvProduct.Rows[n].Cells["updatetime"].Value = DateTime.Now.ToString("yyyy-MM-dd");

                    //추가정보
                    string whr = "product = '" + list[i][0] + "'"
                            + " AND origin = '" + list[i][1] + "'"
                            + " AND sizes = '" + list[i][2] + "'"
                            + " AND unit = '" + list[i][3] + "'";
                    DataRow[] dtRow = pDt.Select(whr);
                    if (dtRow.Length > 0)
                    {
                        dgvProduct.Rows[n].Cells["custom"].Value = dtRow[0]["custom"];
                        dgvProduct.Rows[n].Cells["tax"].Value = dtRow[0]["tax"];
                        dgvProduct.Rows[n].Cells["incidental_expense"].Value = dtRow[0]["incidental_expense"];
                        weight_calculate = Convert.ToBoolean(dtRow[0]["weight_calculate"].ToString());
                        tray_calculate = Convert.ToBoolean(dtRow[0]["tray_calculate"].ToString());
                        if (weight_calculate == tray_calculate)
                            dgvProduct.Rows[n].Cells["weight_calculate"].Value = true;
                        else
                        {
                            dgvProduct.Rows[n].Cells["weight_calculate"].Value = weight_calculate;
                            dgvProduct.Rows[n].Cells["tray_calculate"].Value = tray_calculate;
                        }
                    }
                    else
                    {
                        dgvProduct.Rows[n].Cells["custom"].Value = 0;
                        dgvProduct.Rows[n].Cells["tax"].Value = 0;
                        dgvProduct.Rows[n].Cells["incidental_expense"].Value = 0;
                        dgvProduct.Rows[n].Cells["weight_calculate"].Value = true;
                        dgvProduct.Rows[n].Cells["tray_calculate"].Value = false;
                    }
                }
            }


            //대표품목 
            DataTable pgDt = productGroupRepository.GetProductGroup("", "", "", "", txtProduct.Text, txtOrigin.Text, txtSizes.Text, "");
            //매출단가, 매입단가
            DataTable sDt = seaoverRepository.GetPriceForCostAccount(DateTime.Now.AddDays(-14).AddMonths(-3).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
            //팬딩내역
            DataTable uppDt = customsRepository.GetUnpendingProduct2(txtProduct.Text, txtOrigin.Text, txtSizes.Text, "", false, false);
            //원가계산
            DataTable productDt = priceComparisonRepository.GetNotSalesCostProduct(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);

            SetPrice(1, pgDt, sDt, uppDt, productDt);
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }
        public void AddProduct3(List<string[]> list)
        {
            //영업일수
            common.GetWorkDay(DateTime.Now.AddDays(-1).AddMonths(-6), DateTime.Now.AddDays(-1), out workDays);
            //기간
            txtSttdate.Text = DateTime.Now.AddDays(-14).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            if (list != null && list.Count > 0)
            {
                //재고, 판매량
                CallStockProcedure();
                stockSalesDt = priceComparisonRepository.GetCostAccountingProductInfo("", "", "");
                //단가정보 
                DataTable pDt = productOtherCostRepository.GetProduct("", false, "", "", "");
                //환율
                if (string.IsNullOrEmpty(txtUsd.Text) || !double.TryParse(txtUsd.Text, out double usd))
                    txtUsd.Text = common.GetExchangeRateKEBBank("USD").ToString();
                if (string.IsNullOrEmpty(txtEur.Text) || !double.TryParse(txtEur.Text, out double eur))
                    txtEur.Text = common.GetExchangeRateKEBBank("EUR").ToString();
                //데이터 출력
                for (int i = 0; i < list.Count; i++)
                {
                    int n = dgvProduct.Rows.Add();

                    dgvProduct.Rows[n].Cells["product"].Value = list[i][0];
                    dgvProduct.Rows[n].Cells["origin"].Value = list[i][1];
                    dgvProduct.Rows[n].Cells["sizes"].Value = list[i][2];
                    dgvProduct.Rows[n].Cells["box_weight"].Value = list[i][3];
                    dgvProduct.Rows[n].Cells["cost_unit"].Value = list[i][4];
                    dgvProduct.Rows[n].Cells["origin_cost_unit"].Value = list[i][4];

                    dgvProduct.Rows[n].Cells["tax"].Value = list[i][5];
                    dgvProduct.Rows[n].Cells["custom"].Value = list[i][6];
                    dgvProduct.Rows[n].Cells["incidental_expense"].Value = list[i][7];

                    dgvProduct.Rows[n].Cells["unit_price"].Value = list[i][9];
                    //dgvProduct.Rows[n].Cells["company_id"].Value = list[i][15];
                    dgvProduct.Rows[n].Cells["company"].Value = list[i][11];
                    dgvProduct.Rows[n].Cells["updatetime"].Value = list[i][12];

                    bool weight_calculate = Convert.ToBoolean(list[i][13]);
                    bool tray_calculate = Convert.ToBoolean(list[i][14]);
                    if(rbUsd.Checked)
                        dgvProduct.Rows[n].Cells["exchange_rate"].Value = txtUsd.Text;
                    else
                        dgvProduct.Rows[n].Cells["exchange_rate"].Value = txtEur.Text;
                    //dgvProduct.Rows[n].Cells["updatetime"].Value = DateTime.Now.ToString("yyyy-MM-dd");

                    //추가정보
                    string whr = "product = '" + list[i][0] + "'"
                            + " AND origin = '" + list[i][1] + "'"
                            + " AND sizes = '" + list[i][2] + "'"
                            + " AND unit = '" + list[i][3] + "'";
                    DataRow[] dtRow = pDt.Select(whr);
                    if (dtRow.Length > 0)
                    {
                        dgvProduct.Rows[n].Cells["custom"].Value = dtRow[0]["custom"];
                        dgvProduct.Rows[n].Cells["tax"].Value = dtRow[0]["tax"];
                        dgvProduct.Rows[n].Cells["incidental_expense"].Value = dtRow[0]["incidental_expense"];
                        weight_calculate = Convert.ToBoolean(dtRow[0]["weight_calculate"].ToString());
                        tray_calculate = Convert.ToBoolean(dtRow[0]["tray_calculate"].ToString());
                        if (weight_calculate == tray_calculate)
                            dgvProduct.Rows[n].Cells["weight_calculate"].Value = true;
                        else
                        {
                            dgvProduct.Rows[n].Cells["weight_calculate"].Value = weight_calculate;
                            dgvProduct.Rows[n].Cells["tray_calculate"].Value = tray_calculate;
                        }
                    }
                    else
                    {
                        dgvProduct.Rows[n].Cells["custom"].Value = 0;
                        dgvProduct.Rows[n].Cells["tax"].Value = 0;
                        dgvProduct.Rows[n].Cells["incidental_expense"].Value = 0;
                        dgvProduct.Rows[n].Cells["weight_calculate"].Value = true;
                        dgvProduct.Rows[n].Cells["tray_calculate"].Value = false;
                    }
                }
            }

            //대표품목 
            DataTable pgDt = productGroupRepository.GetProductGroup("", "", "", "", txtProduct.Text, txtOrigin.Text, txtSizes.Text, "");
            //매출단가, 매입단가
            DateTime sttDate;
            if (!DateTime.TryParse(txtSttdate.Text, out sttDate))
            {
                MessageBox.Show(this, "시작일 값이 올바르지 않은 날짜 형식입니다.");
                this.Activate();
                return;
            }
            DateTime endDate;
            if (!DateTime.TryParse(txtEnddate.Text, out endDate))
            {
                MessageBox.Show(this, "종료일 값이 올바르지 않은 날짜 형식입니다.");
                this.Activate();
                return;
            }
            string sttdate = sttDate.AddMonths(-3).ToString("yyyy-MM-dd");
            string enddate = endDate.ToString("yyyy-MM-dd");
            DataTable sDt = seaoverRepository.GetPriceForCostAccount(sttdate, enddate, txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
            //팬딩내역
            DataTable uppDt = customsRepository.GetUnpendingProduct2(txtProduct.Text, txtOrigin.Text, txtSizes.Text, "", false, false);
            //원가계산
            DataTable productDt = priceComparisonRepository.GetNotSalesCostProduct(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);

            SetPrice(1, pgDt, sDt, uppDt, productDt);
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }

        private void CallStockProcedure()
        {
            //품명별재고현황 스토어프로시져 호출
            try
            {
                string sDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                string eDate = DateTime.Now.ToString("yyyy-MM-dd");
                string user_id = um.seaover_id;
                if (priceComparisonRepository.CallStoredProcedureSTOCK(user_id, eDate) == 0)
                {
                    MessageBox.Show(this, "호출 내용이 없음");
                    this.Activate();
                    return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(this,e.Message);
                this.Activate();
                return;
            }

        }

        //그냥 검색
        public void GetProduct()
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            dgvProduct.Rows.Clear();

            txtProduct.Text = txtProduct.Text.Trim();
            txtOrigin.Text = txtOrigin.Text.Trim();
            txtSizes.Text = txtSizes.Text.Trim();
            txtUnit.Text = txtUnit.Text.Trim();
            txtCompany.Text = txtCompany.Text.Trim();
            DateTime sttDate;
            if (!DateTime.TryParse(txtSttdate.Text, out sttDate))
            {
                MessageBox.Show(this, "시작일 값이 올바르지 않은 날짜 형식입니다.");
                this.Activate();
                return;
            }
            DateTime endDate;
            if (!DateTime.TryParse(txtEnddate.Text, out endDate))
            {
                MessageBox.Show(this, "종료일 값이 올바르지 않은 날짜 형식입니다.");
                this.Activate();
                return;
            }
            string sttdate = sttDate.AddMonths(-3).ToString("yyyy-MM-dd");
            string enddate = endDate.ToString("yyyy-MM-dd");
            //정렬방식
            int sortType = 1;
            switch (cbSortType.Text)
            {
                case "품명+원산지+규격":
                    sortType = 1;
                    break;
                case "거래처+품명+원산지+규격":
                    sortType = 2;
                    break;
                case "품명+원산지+규격+오퍼가":
                    sortType = 3;
                    break;
                case "오퍼가+품명+원산지+규격":
                    sortType = 4;
                    break;
                case "품명+원산지+규격+오퍼일자":
                    sortType = 5;
                    break;
                case "오퍼일자+품명+원산지+규격":
                    sortType = 6;
                    break;
            }
            
            //단가정보 
            DataTable pDt = purchasePriceRepository.GetCostAccounting(txtSttdate.Text, txtEnddate.Text
                                                                    , txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtCompany.Text, cbExactly.Checked
                                                                    , txtManager.Text, sortType, false, cbOnlyCurrent.Checked);
            if (pDt.Rows.Count > 0)
            {
                int sale_terms;
                switch (cbSaleTerms.Text)
                {
                    case "1개월":
                        sale_terms = 1;
                        break;
                    case "45일":
                        sale_terms = 45;
                        break;
                    case "2개월":
                        sale_terms = 2;
                        break;
                    case "3개월":
                        sale_terms = 3;
                        break;
                    case "6개월":
                        sale_terms = 6;
                        break;
                    case "12개월":
                        sale_terms = 12;
                        break;
                    case "18개월":
                        sale_terms = 18;
                        break;
                    default:
                        sale_terms = 6;
                        break;
                }
                DateTime sale_sttdate = DateTime.Now.AddMonths(-sale_terms);
                if(sale_terms == 45)
                    sale_sttdate = DateTime.Now.AddDays(-sale_terms);
                DateTime sale_enddate = DateTime.Now;
                //영업일수
                common.GetWorkDay(sale_sttdate, sale_enddate, out workDays);
                workDays--;
                //재고, 판매량
                CallStockProcedure();
                stockSalesDt = priceComparisonRepository.GetCostAccountingProductInfo(txtProduct.Text, txtOrigin.Text, txtSizes.Text, sale_terms);
                //데이터 출력
                for (int i = 0; i < pDt.Rows.Count; i++)
                {
                    int n = dgvProduct.Rows.Add();
                    dgvProduct.Rows[n].Cells["id"].Value = pDt.Rows[i]["id"];
                    dgvProduct.Rows[n].Cells["company"].Value = pDt.Rows[i]["cname"];
                    dgvProduct.Rows[n].Cells["company_id"].Value = pDt.Rows[i]["cid"];
                    dgvProduct.Rows[n].Cells["product"].Value = pDt.Rows[i]["product"];
                    dgvProduct.Rows[n].Cells["origin"].Value = pDt.Rows[i]["origin"];
                    dgvProduct.Rows[n].Cells["sizes"].Value = pDt.Rows[i]["sizes"];
                    dgvProduct.Rows[n].Cells["sizes2"].Value = pDt.Rows[i]["sizes2"];
                    dgvProduct.Rows[n].Cells["box_weight"].Value = pDt.Rows[i]["unit"];
                    dgvProduct.Rows[n].Cells["cost_unit"].Value = pDt.Rows[i]["cost_unit"];
                    dgvProduct.Rows[n].Cells["origin_cost_unit"].Value = pDt.Rows[i]["cost_unit"];
                    dgvProduct.Rows[n].Cells["unit_price"].Value = pDt.Rows[i]["purchase_price"];
                    dgvProduct.Rows[n].Cells["updatetime"].Value = Convert.ToDateTime(pDt.Rows[i]["updatetime"]).ToString("yyyy-MM-dd");
                    if(rbUsd.Checked)
                        dgvProduct.Rows[n].Cells["exchange_rate"].Value = txtUsd.Text;
                    else
                        dgvProduct.Rows[n].Cells["exchange_rate"].Value = txtEur.Text;

                    dgvProduct.Rows[n].Cells["custom"].Value = pDt.Rows[i]["custom"];
                    dgvProduct.Rows[n].Cells["tax"].Value = pDt.Rows[i]["tax"];
                    dgvProduct.Rows[n].Cells["incidental_expense"].Value = pDt.Rows[i]["incidental_expense"];

                    double fixed_tariff;
                    if (!double.TryParse(pDt.Rows[i]["fixed_tariff"].ToString(), out fixed_tariff))
                        fixed_tariff = 0;
                    dgvProduct.Rows[n].Cells["fixed_tariff"].Value = fixed_tariff.ToString("#,##0");

                    bool weight_calculate = Convert.ToBoolean(pDt.Rows[i]["weight_calculate"].ToString());
                    dgvProduct.Rows[n].Cells["weight_calculate"].Value = weight_calculate;
                    dgvProduct.Rows[n].Cells["tray_calculate"].Value = !weight_calculate;

                    bool is_FOB = Convert.ToBoolean(pDt.Rows[i]["is_FOB"].ToString());
                    dgvProduct.Rows[n].Cells["is_FOB"].Value = is_FOB;
                    dgvProduct.Rows[n].Cells["is_CFR"].Value = !is_FOB;
                }


                //대표품목 
                DataTable pgDt = productGroupRepository.GetProductGroup("", "", "", "", txtProduct.Text, txtOrigin.Text, txtSizes.Text, "");
                //매출단가, 매입단가
                sttdate = sttDate.AddMonths(-3).ToString("yyyy-MM-dd");
                enddate = endDate.ToString("yyyy-MM-dd");
                DataTable sDt = seaoverRepository.GetPriceForCostAccount(sttdate, enddate, txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
                //팬딩내역
                DataTable uppDt = customsRepository.GetUnpendingProduct2(txtProduct.Text, txtOrigin.Text, txtSizes.Text, "", false, false);
                //원가계산
                DataTable productDt = priceComparisonRepository.GetNotSalesCostProduct(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);

                SetPrice(sortType, pgDt, sDt, uppDt, productDt);
            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }

        private void SetPrice(int sortType = 1, DataTable pgDt = null, DataTable sDt = null, DataTable uppDt = null, DataTable productDt = null)
        {
            //데이터 출력    
            if (dgvProduct.Rows.Count > 0)
            {
                DateTime sttDate;
                if (!DateTime.TryParse(txtSttdate.Text, out sttDate))
                {
                    MessageBox.Show(this, "시작일 값이 올바르지 않은 날짜 형식입니다.");
                    this.Activate();
                    return;
                }
                DateTime endDate;
                if (!DateTime.TryParse(txtEnddate.Text, out endDate))
                {
                    MessageBox.Show(this, "종료일 값이 올바르지 않은 날짜 형식입니다.");
                    this.Activate();
                    return;
                }
                string whr;
                DataRow[] dtRow;
                //데이터출력=========================================================================
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];

                    //SEAOVER 품목
                    double sales_price = 0;
                    double purchase_price1 = 0;
                    double box_weight = Convert.ToDouble(row.Cells["box_weight"].Value.ToString());
                    double cost_unit;
                    if (!double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                        cost_unit = 0;
                    double sales_unit = Convert.ToDouble(row.Cells["box_weight"].Value.ToString());

                    bool weight_calculate;
                    if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                        weight_calculate = true;

                    double custom;
                    if (row.Cells["custom"].Value == null || !double.TryParse(row.Cells["custom"].Value.ToString(), out custom))
                        custom = 0;
                    custom /= 100;
                    double tax;
                    if (row.Cells["tax"].Value == null || !double.TryParse(row.Cells["tax"].Value.ToString(), out tax))
                        tax = 0;
                    tax /= 100;
                    double incidental_expense;
                    if (row.Cells["incidental_expense"].Value == null || !double.TryParse(row.Cells["incidental_expense"].Value.ToString(), out incidental_expense))
                        incidental_expense = 0;
                    incidental_expense /= 100;
                    double exchange_rate = 0;
                    if (rbUsd.Checked || !double.TryParse(txtUsd.Text, out exchange_rate))
                        exchange_rate = 0;
                    else if (rbEur.Checked || !double.TryParse(rbEur.Text, out exchange_rate))
                        exchange_rate = 0;
                    //대표품목=========================================================================================
                    string merge_product = "";
                    if (pgDt != null && pgDt.Rows.Count > 0)
                    {
                        whr = "item_product = '" + row.Cells["product"].Value.ToString() + "'"
                            + " AND item_origin = '" + row.Cells["origin"].Value.ToString() + "'"
                            + " AND item_sizes = '" + row.Cells["sizes"].Value.ToString() + "'"
                            + " AND item_seaover_unit = '" + row.Cells["box_weight"].Value.ToString() + "'";
                        DataRow[] pgDr = pgDt.Select(whr);
                        if (pgDr.Length > 0)
                        {
                            whr = "main_id = " + pgDr[0]["main_id"].ToString();
                            pgDr = pgDt.Select(whr);
                            foreach (DataRow dr in pgDr)
                            {
                                merge_product += "\n" + dr["item_product"].ToString()
                                                + "^" + dr["item_origin"].ToString()
                                                + "^" + dr["item_sizes"].ToString()
                                                + "^" + dr["item_unit"].ToString()
                                                + "^" + dr["item_price_unit"].ToString()
                                                + "^" + dr["item_unit_count"].ToString()
                                                + "^" + dr["item_seaover_unit"].ToString();
                            }

                            row.Cells["merge_product"].Value = merge_product.Trim();
                        }
                    }
                    //재고, 매출량========================================================================================
                    double stock = 0;
                    double sales_count = 0;
                    whr = "";
                    if (stockSalesDt.Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(merge_product))
                        {
                            whr = "품명 = '" + row.Cells["product"].Value.ToString() + "'"
                                    + " AND 원산지 = '" + row.Cells["origin"].Value.ToString() + "'"
                                    + " AND 규격 = '" + row.Cells["sizes"].Value.ToString() + "'"
                                    + " AND 단위 = '" + row.Cells["box_weight"].Value.ToString() + "'";
                        }
                        else
                        {
                            string[] sub_products = merge_product.Split('\n');
                            foreach (string sub_product in sub_products)
                            {
                                if (!string.IsNullOrEmpty(sub_product))
                                {
                                    string[] subs = sub_product.Split('^');
                                    whr += "\n (품명 = '" + subs[0] + "'"
                                            + " AND 원산지 = '" + subs[1] + "'"
                                            + " AND 규격 = '" + subs[2] + "'"
                                            + " AND 단위 = '" + subs[6] + "')";
                                }
                            }
                            whr = whr.Trim().Replace("\n", "OR");
                        }
                        dtRow = stockSalesDt.Select(whr);
                        if (dtRow.Length > 0)
                        {
                            foreach (DataRow dr in dtRow)
                            {
                                double temp_unit = Convert.ToDouble(dr["단위"].ToString());

                                stock += Convert.ToDouble(dr["재고수"].ToString()) * temp_unit / box_weight;
                                sales_count += Convert.ToDouble(dr["매출수"].ToString()) * temp_unit / box_weight;
                            }
                        }
                    }
                    row.Cells["stock_exclude_shipment"].Value = stock.ToString("#,##0");

                    double total_shipment_qty = 0;
                    double total_unpending_qty_before = 0;
                    double total_unpending_qty_after = 0;
                    double total_unpending_cost_price = 0;
                    DataRow[] uppRow = null;
                    if (uppDt.Rows.Count > 0)
                    {
                        whr = "product = '" + row.Cells["product"].Value.ToString() + "'"
                            + " AND origin = '" + row.Cells["origin"].Value.ToString() + "'"
                            + " AND sizes = '" + row.Cells["sizes"].Value.ToString() + "'"
                            + " AND box_weight = '" + row.Cells["box_weight"].Value.ToString() + "'";

                        uppRow = uppDt.Select(whr);
                        if (uppRow.Length > 0)
                        {
                            for (int j = 0; j < uppRow.Length; j++)
                            {
                                string bl_no = uppRow[j]["bl_no"].ToString();
                                string warehousing_date = uppRow[j]["warehousing_date"].ToString();
                                //초기화
                                double shipment_qty = 0;
                                double unpending_qty_before = 0;
                                double unpending_qty_after = 0;

                                //계약만 한 상태
                                if (string.IsNullOrEmpty(bl_no))
                                    shipment_qty = Convert.ToDouble(uppRow[j]["quantity_on_paper"]);
                                //배송중인 상태
                                else if (!string.IsNullOrEmpty(bl_no) && string.IsNullOrEmpty(warehousing_date))
                                    unpending_qty_before = Convert.ToDouble(uppRow[j]["quantity_on_paper"]);
                                //입항은 했지만 미통관인 상태
                                else if (!string.IsNullOrEmpty(bl_no) && !string.IsNullOrEmpty(warehousing_date))
                                    unpending_qty_after = Convert.ToDouble(uppRow[j]["quantity_on_paper"]);

                                //total
                                total_shipment_qty += shipment_qty;
                                total_unpending_qty_before += unpending_qty_before;
                                total_unpending_qty_after += unpending_qty_after;

                                double offer_price;
                                if (!double.TryParse(uppRow[j]["unit_price"].ToString(), out offer_price))
                                    offer_price = 0;

                                //동원 + 2.5% Or 2%
                                if (uppRow[j]["ato_no"].ToString().Contains("dw") || uppRow[j]["ato_no"].ToString().Contains("DW")
                                    || uppRow[j]["ato_no"].ToString().Contains("hs") || uppRow[j]["ato_no"].ToString().Contains("HS")
                                    || uppRow[j]["ato_no"].ToString().Contains("od") || uppRow[j]["ato_no"].ToString().Contains("OD")
                                    || uppRow[j]["ato_no"].ToString().Contains("ad") || uppRow[j]["ato_no"].ToString().Contains("AD"))
                                    offer_price = offer_price * 1.025;
                                else if (uppRow[j]["ato_no"].ToString().Contains("jd") || uppRow[j]["ato_no"].ToString().Contains("JD"))
                                    offer_price = offer_price * 1.02;

                                double shipment_cost_price;
                                if (weight_calculate)
                                    shipment_cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate * box_weight;
                                else
                                    shipment_cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate * cost_unit;

                                //연이자 추가
                                double interest = 0;
                                DateTime etd;
                                if (DateTime.TryParse(uppRow[j]["etd"].ToString(), out etd))
                                {
                                    TimeSpan ts = DateTime.Now - etd;
                                    int days = ts.Days;
                                    interest = shipment_cost_price * 0.08 / 365 * days;
                                    if (interest < 0)
                                        interest = 0;
                                }
                                shipment_cost_price += interest;

                                if(unpending_qty_after == 0)
                                    total_unpending_cost_price += shipment_cost_price * Convert.ToDouble(uppRow[j]["quantity_on_paper"].ToString());
                            }
                        }
                    }
                    //선적원가
                    double pending_cost_price = (total_unpending_cost_price / (total_shipment_qty + total_unpending_qty_before));
                    if (double.IsNaN(pending_cost_price))
                        pending_cost_price = 0;
                    row.Cells["pending_cost_price"].Value = pending_cost_price.ToString("#,##0");

                    double real_stock = stock;    //입고된재고

                    stock += total_shipment_qty;
                    stock += total_unpending_qty_before;
                    row.Cells["stock_add_shipment"].Value = stock.ToString("#,##0");

                    if (cbShipmentStock.Checked)
                        row.Cells["stock"].Value = row.Cells["stock_add_shipment"].Value;
                    else
                        row.Cells["stock"].Value = row.Cells["stock_exclude_shipment"].Value;
                    //매출량==========================================================================================
                    if (cbSaleTerms.Text == "1개월")
                        row.Cells["sales_count"].Value = sales_count.ToString("#,##0.00");
                    else
                        row.Cells["sales_count"].Value = (sales_count / workDays * 21).ToString("#,##0.00");
                    //매출단가========================================================================================
                    whr = "품명 = '" + row.Cells["product"].Value.ToString() + "'"
                            + " AND 원산지 = '" + row.Cells["origin"].Value.ToString() + "'"
                            + " AND 규격 = '" + row.Cells["sizes"].Value.ToString() + "'"
                            + " AND (SEAOVER단위 = '" + row.Cells["box_weight"].Value.ToString() + "' OR " + "계산단위 = '" + row.Cells["box_weight"].Value.ToString() + "')"
                            + " AND 매출단가 > 0";
                    dtRow = sDt.Select(whr);
                    if (dtRow.Length > 0)
                    {
                        if (dtRow[0]["가격단위"].ToString().Contains("팩"))
                        {
                            sales_unit = Convert.ToDouble(dtRow[0]["단위"]) * Convert.ToDouble(dtRow[0]["단위수량"]);
                            sales_price = Convert.ToDouble(dtRow[0]["매출단가"]) * Convert.ToDouble(dtRow[0]["단위수량"]);
                        }
                        else
                        {
                            sales_unit = Convert.ToDouble(dtRow[0]["단위"]);
                            sales_price = Convert.ToDouble(dtRow[0]["매출단가"]);
                        }
                    }
                    //없으면 전체 단위에서 가져오기
                    else
                    {
                        whr = "품명 = '" + row.Cells["product"].Value.ToString() + "'"
                            + " AND 원산지 = '" + row.Cells["origin"].Value.ToString() + "'"
                            + " AND 규격 = '" + row.Cells["sizes"].Value.ToString() + "'"
                            + " AND 매출단가 > 0";
                        dtRow = sDt.Select(whr);
                        if (dtRow.Length > 0)
                        {
                            if (dtRow[0]["가격단위"].ToString().Contains("팩"))
                            {
                                sales_unit = Convert.ToDouble(dtRow[0]["단위"]) * Convert.ToDouble(dtRow[0]["단위수량"]);
                                sales_price = Convert.ToDouble(dtRow[0]["매출단가"]) * Convert.ToDouble(dtRow[0]["단위수량"]);
                            }
                            else
                            {
                                sales_unit = Convert.ToDouble(dtRow[0]["단위"]);
                                sales_price = Convert.ToDouble(dtRow[0]["매출단가"]);
                            }
                        }
                    }
                    //원금
                    row.Cells["domestic_sales_price"].Value = sales_price.ToString("#,##0");
                    row.Cells["sales_unit"].Value = sales_unit;
                    
                    //매입단가========================================================================================
                    double purchase_unit = Convert.ToDouble(row.Cells["box_weight"].Value.ToString());

                    whr = "품명 = '" + row.Cells["product"].Value.ToString() + "'"
                            + " AND 원산지 = '" + row.Cells["origin"].Value.ToString() + "'"
                            + " AND 규격 = '" + row.Cells["sizes"].Value.ToString() + "'"
                            + " AND (SEAOVER단위 = '" + row.Cells["box_weight"].Value.ToString() + "' OR " + "계산단위 = '" + row.Cells["box_weight"].Value.ToString() + "')"
                            + " AND 매입단가 > 6";
                    dtRow = sDt.Select(whr);
                    if (dtRow.Length > 0)
                    {
                        if (dtRow[0]["가격단위"].ToString().Contains("팩"))
                        {
                            purchase_unit = Convert.ToDouble(dtRow[0]["단위"]) * Convert.ToDouble(dtRow[0]["단위수량"]);
                            purchase_price1 = Convert.ToDouble(dtRow[0]["매입단가"]) * Convert.ToDouble(dtRow[0]["단위수량"]);
                        }
                        else
                        {
                            purchase_unit = Convert.ToDouble(dtRow[0]["단위"]);
                            purchase_price1 = Convert.ToDouble(dtRow[0]["매입단가"]);
                        }                        
                    }
                    //없으면 전체 단위에서 가져오기
                    else
                    {
                        whr = "품명 = '" + row.Cells["product"].Value.ToString() + "'"
                            + " AND 원산지 = '" + row.Cells["origin"].Value.ToString() + "'"
                            + " AND 규격 = '" + row.Cells["sizes"].Value.ToString() + "'"
                            + " AND 매입단가 > 6";
                        dtRow = sDt.Select(whr);
                        if (dtRow.Length > 0)
                        {
                            if (dtRow[0]["가격단위"].ToString().Contains("팩"))
                            {
                                purchase_unit = Convert.ToDouble(dtRow[0]["단위"]) * Convert.ToDouble(dtRow[0]["단위수량"]);
                                purchase_price1 = Convert.ToDouble(dtRow[0]["매입단가"]) * Convert.ToDouble(dtRow[0]["단위수량"]);
                            }
                            else
                            {
                                purchase_unit = Convert.ToDouble(dtRow[0]["단위"]);
                                purchase_price1 = Convert.ToDouble(dtRow[0]["매입단가"]);
                            }
                        }
                    }
                    //없으면 매출단가로 변경(빨간색)
                    if (purchase_price1 == 0)
                    {
                        purchase_price1 = sales_price;
                        purchase_unit = sales_unit;
                        row.Cells["purchase_price1"].Style.ForeColor = Color.Red;
                    }
                    //원금
                    row.Cells["purchase_price"].Value = purchase_price1.ToString("#,##0");
                    row.Cells["purchase_unit"].Value = purchase_unit;

                    //S원가 계산
                    double seaover_cost_price;
                    ReplaceSalesCostToPendingCost(productDt, row, out seaover_cost_price);
                    row.Cells["seaover_cost_price"].Value = seaover_cost_price.ToString("#,##0");
                    //평균원가1
                    double average_cost_price1 = (total_unpending_cost_price + seaover_cost_price * real_stock) / (total_shipment_qty + total_unpending_qty_before + real_stock);
                    row.Cells["average_cost_price1"].Value = average_cost_price1.ToString("#,##0");
                }
            }
            else
            {
                MessageBox.Show(this, "검색내역이 없습니다.");
                this.Activate();
                return;
            }
            calculate();
            calculateWeight();
            calculateAssort();
        }


        private void ReplaceSalesCostToPendingCost(DataTable productDt, DataGridViewRow row, out double seaover_cost_price)
        {
            seaover_cost_price = 0;
            double exchange_rate = 0;
            if (rbUsd.Checked || !double.TryParse(txtUsd.Text, out exchange_rate))
                exchange_rate = 0;
            else if (rbEur.Checked || !double.TryParse(txtEur.Text, out exchange_rate))
                exchange_rate = 0;

            //최종S원가
            double final_average_cost = 0;
            double final_qty = 0;
            //ToolTip txt
            string tooltip = "";

            string[] sub_product;
            if (row.Cells["merge_product"].Value == null || row.Cells["merge_product"].Value.ToString() == "0" || string.IsNullOrEmpty(row.Cells["merge_product"].Value.ToString()))
            {
                sub_product = new string[1];
                sub_product[0] = row.Cells["product"].Value.ToString()
                            + "^" + row.Cells["origin"].Value.ToString()
                            + "^" + row.Cells["sizes"].Value.ToString()
                            + "^"
                            + "^"
                            + "^"
                            + "^" + row.Cells["box_weight"].Value.ToString();
            }
            else
                sub_product = row.Cells["merge_product"].Value.ToString().Trim().Split('\n');

            if (sub_product.Length > 0)
            {
                DataTable pendingDt = customsRepository.GetProductForNotSalesCost(txtProduct.Text, txtOrigin.Text, txtSizes.Text, "", productDt, sub_product);
                //매출원가 기본계산
                if (productDt.Rows.Count > 0)
                {
                    productDt.Columns["수량"].ReadOnly = false;
                    productDt.Columns["환율"].ReadOnly = false;
                    productDt.Columns["매출원가"].ReadOnly = false;
                    productDt.Columns["입고일자"].ReadOnly = false;
                    productDt.Columns["etd"].ReadOnly = false;
                    productDt.Columns["isPendingCalculate"].ReadOnly = false;

                    //데이터 조합
                    for (int i = 0; i < productDt.Rows.Count; i++)
                    {
                        if (pendingDt != null && pendingDt.Rows.Count > 0 && double.TryParse(productDt.Rows[i]["매출원가"].ToString(), out double sales_price) && sales_price == 0)
                        {
                            string whr = "ato_no = '" + productDt.Rows[i]["AtoNo"].ToString() + "'"
                                    + " AND product = '" + productDt.Rows[i]["품명"].ToString() + "'"
                                    + " AND origin = '" + productDt.Rows[i]["원산지"].ToString() + "'"
                                    + " AND sizes = '" + productDt.Rows[i]["규격"].ToString() + "'"
                                    + " AND unit = '" + productDt.Rows[i]["단위"].ToString() + "'";
                            DataRow[] dr = pendingDt.Select(whr);
                            if (dr.Length > 0)
                            {
                                double unit_price = Convert.ToDouble(dr[0]["unit_price"].ToString());
                                double box_weight = Convert.ToDouble(dr[0]["unit"].ToString());
                                double cost_unit = Convert.ToDouble(dr[0]["cost_unit"].ToString());
                                //단위맞추기
                                unit_price = unit_price * box_weight / Convert.ToDouble(row.Cells["box_weight"].Value.ToString());

                                bool isWeight = Convert.ToBoolean(dr[0]["weight_calculate"].ToString());
                                //트레이단가 -> 중량 단가
                                if (!isWeight)
                                    unit_price = (unit_price * cost_unit) / box_weight;
                                double custom = Convert.ToDouble(dr[0]["custom"].ToString()) / 100;
                                double tax = Convert.ToDouble(dr[0]["tax"].ToString()) / 100;
                                double incidental_expense = Convert.ToDouble(dr[0]["incidental_expense"].ToString()) / 100;
                                //대행일 경우 부대비용 3%
                                if (dr[0]["ato_no"].ToString().Contains("dw") || dr[0]["ato_no"].ToString().Contains("DW")
                                    || dr[0]["ato_no"].ToString().Contains("hs") || dr[0]["ato_no"].ToString().Contains("HS")
                                    || dr[0]["ato_no"].ToString().Contains("od") || dr[0]["ato_no"].ToString().Contains("OD")
                                    || dr[0]["ato_no"].ToString().Contains("ad") || dr[0]["ato_no"].ToString().Contains("AD")
                                    || dr[0]["ato_no"].ToString().Contains("jd") || dr[0]["ato_no"].ToString().Contains("JD"))
                                    incidental_expense = 0.03;
                                double qty = Convert.ToDouble(dr[0]["qty"].ToString());
                                double exchangeRate;
                                if (!double.TryParse(dr[0]["exchange_rate"].ToString(), out exchangeRate))
                                    exchangeRate = exchange_rate;

                                double sales_cost = unit_price * exchangeRate * (1 + custom + tax + incidental_expense);

                                //입고일자
                                string etd_txt = "";
                                if (DateTime.TryParse(dr[0]["etd2"].ToString(), out DateTime etd))
                                    etd_txt = etd.ToString("yyyy-MM-dd");

                                productDt.Rows[i]["etd"] = etd_txt;
                                productDt.Rows[i]["매출원가"] = sales_cost;
                                productDt.Rows[i]["환율"] = exchangeRate;
                                productDt.Rows[i]["isPendingCalculate"] = "TRUE";
                            }
                        }
                    }
                    productDt.AcceptChanges();

                    //데이터 출력
                    for (int k = 0; k < sub_product.Length; k++)
                    {
                        //평균
                        double total_average_cost = 0;
                        double total_qty = 0;
                        //씨오버단위
                        double unit;
                        if (!double.TryParse(row.Cells["box_weight"].Value.ToString(), out unit))
                            unit = 1;

                        //품목정보
                        string[] product = sub_product[k].Trim().Split('^');
                        string whr = "품명 = '" + product[0] + "'"
                                    + " AND 원산지 = '" + product[1] + "'"
                                    + " AND 규격 = '" + product[2] + "'"
                                    + " AND 단위 = '" + product[6] + "'";
                        DataRow[] dr = productDt.Select(whr);
                        if (dr.Length > 0)
                        {
                            for (int j = 0; j < dr.Length; j++)
                            {
                                double stock_unit;
                                if (!double.TryParse(dr[j]["단위"].ToString(), out stock_unit))
                                    stock_unit = 1;
                                double cost_unit;
                                if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                                    cost_unit = 0;
                                if (cost_unit == 0)
                                    cost_unit = 1;
                                double qty = Convert.ToDouble(dr[j]["수량"].ToString());
                                double sales_cost = Convert.ToDouble(dr[j]["매출원가"].ToString());


                                //계산된 매출원가
                                if (bool.TryParse(dr[j]["isPendingCalculate"].ToString(), out bool isPendingCalculate) && isPendingCalculate)
                                {
                                    sales_cost = sales_cost * unit / cost_unit;
                                    //동원시 추가
                                    if (dr[j]["AtoNo"].ToString().Contains("dw") || dr[j]["AtoNo"].ToString().Contains("DW")
                                        || dr[j]["AtoNo"].ToString().Contains("hs") || dr[j]["AtoNo"].ToString().Contains("HS")
                                        || dr[j]["AtoNo"].ToString().Contains("od") || dr[j]["AtoNo"].ToString().Contains("OD")
                                        || dr[j]["AtoNo"].ToString().Contains("ad") || dr[j]["AtoNo"].ToString().Contains("AD"))
                                        sales_cost = sales_cost * 1.025;
                                    else if (dr[j]["AtoNo"].ToString().Contains("jd") || dr[j]["AtoNo"].ToString().Contains("JD"))
                                        sales_cost = sales_cost * 1.02;


                                    //2023-10-30 냉장료 포함
                                    double refrigeration_fee = 0;
                                    DateTime in_date;
                                    string in_date_txt = "";
                                    if (DateTime.TryParse(dr[j]["입고일자"].ToString(), out in_date))
                                    {
                                        TimeSpan ts = DateTime.Now - in_date;
                                        int days = ts.Days;

                                        int r_fee_day;
                                        if (stock_unit <= 5)
                                            r_fee_day = 4;
                                        else if (stock_unit <= 10)
                                            r_fee_day = 7;
                                        else if (stock_unit <= 15)
                                            r_fee_day = 12;
                                        else if (stock_unit <= 20)
                                            r_fee_day = 13;
                                        else
                                            r_fee_day = 15;

                                        refrigeration_fee = r_fee_day * days;
                                        in_date_txt = in_date.ToString("yyyy-MM-dd");
                                    }


                                    //2023-10-25 대행건 연이자 8% 일발생
                                    double interest = 0;
                                    if (DateTime.TryParse(dr[j]["etd"].ToString(), out DateTime etd))
                                    {
                                        TimeSpan ts = DateTime.Now - etd;
                                        int days = ts.Days;
                                        interest = sales_cost * 0.08 / 365 * days;
                                    }
                                    //설명
                                    if (interest > 0 && refrigeration_fee > 0)
                                        tooltip += "\n AtoNo : " + dr[j]["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + $" + {interest.ToString("N2")}(이자) + {refrigeration_fee.ToString("#,##0")}(냉장료) = {(sales_cost + interest + refrigeration_fee).ToString("#,##0")} " + "  | 수량 :" + qty;
                                    else if (interest == 0 && refrigeration_fee > 0)
                                        tooltip += "\n AtoNo : " + dr[j]["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + $" + {refrigeration_fee.ToString("#,##0")}(냉장료) = {(sales_cost + refrigeration_fee).ToString("#,##0")} " + "  | 수량 :" + qty;
                                    else if (interest > 0 && refrigeration_fee == 0)
                                        tooltip += "\n AtoNo : " + dr[j]["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + $" + {interest.ToString("N2")}(이자) = {(sales_cost + interest).ToString("#,##0")} " + "  | 수량 :" + qty;
                                    else
                                        tooltip += "\n AtoNo : " + dr[j]["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + "  | 수량 :" + qty;

                                    sales_cost += interest + refrigeration_fee;
                                    //종합
                                    total_average_cost += sales_cost * qty;
                                    total_qty += qty;
                                }
                                //씨오버 매출원가
                                else
                                {
                                    sales_cost = sales_cost / stock_unit * unit;
                                    total_average_cost += sales_cost * qty;
                                    total_qty += qty;

                                    tooltip += "\n AtoNo : " + dr[j]["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + "  수량 :" + qty;
                                }
                            }
                        }

                        //평균원가
                        if (total_average_cost > 0 || total_qty > 0)
                        {
                            final_average_cost += total_average_cost;
                            final_qty += total_qty;
                        }
                    }

                }


                //평균원가
                if (final_average_cost > 0 || final_qty > 0)
                    seaover_cost_price = (final_average_cost / final_qty);
                dgvProduct.Rows[0].Cells["seaover_cost_price"].ToolTipText = "***** AtoNo 'AD,DW,OD,JD,HS' 경우 + 매출원가 2.5%, + 연이자 8% *****\n\n" + tooltip.Trim();
            }
        }


        private void calculateAssort()
        {
            dgvProduct.EndEdit();
            if (dgvProduct.Rows.Count > 0)
            {
                double total_assort_margin1 = 0;
                double total_assort_margin2 = 0;
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];
                    double assort;
                    if (row.Cells["assort"].Value == null || !double.TryParse(row.Cells["assort"].Value.ToString(), out assort))
                        assort = 0;

                    double cost_price;
                    if (row.Cells["cost_price"].Value == null || !double.TryParse(row.Cells["cost_price"].Value.ToString(), out cost_price))
                        cost_price = 0;

                    double domestic_sales_price1;
                    if (rbSalesPrice.Checked)
                    {
                        if (row.Cells["domestic_sales_price1"].Value == null || !double.TryParse(row.Cells["domestic_sales_price1"].Value.ToString(), out domestic_sales_price1))
                            domestic_sales_price1 = 0;
                    }
                    else
                    {
                        if (row.Cells["purchase_price1"].Value == null || !double.TryParse(row.Cells["purchase_price1"].Value.ToString(), out domestic_sales_price1))
                            domestic_sales_price1 = 0;
                    }

                    double trq;
                    if (row.Cells["trq"].Value == null || !double.TryParse(row.Cells["trq"].Value.ToString(), out trq))
                        trq = 0;

                    //마진1
                    double assort_margin1 = domestic_sales_price1 - cost_price;
                    row.Cells["assort_margin1"].Value = assort_margin1.ToString("#,##0");
                    total_assort_margin1 += (assort_margin1 * assort);
                    row.Cells["income_amount1"].Value = (assort_margin1 * assort).ToString("#,##0");
                    //마진2
                    double assort_margin2 = domestic_sales_price1 - trq;
                    //row.Cells["assort_margin2"].Value = assort_margin2.ToString("#,##0");
                    total_assort_margin2 += (assort_margin2 * assort);
                    row.Cells["income_amount2"].Value = (assort_margin2 * assort).ToString("#,##0");
                }

                if (double.IsNaN(total_assort_margin1))
                    total_assort_margin1 = 0;
                if (double.IsNaN(total_assort_margin2))
                    total_assort_margin2 = 0;

                txtTotalAssortMargin1.Text = total_assort_margin1.ToString("#,##0");
                txtTotalAssortMargin2.Text = total_assort_margin2.ToString("#,##0");
            }
        }

        private void calculate()
        {
            dgvProduct.EndEdit();
            if (dgvProduct.Rows.Count > 0)
            {
                //팬딩내역
                DataTable uppDt = customsRepository.GetUnpendingProduct2(txtProduct.Text, txtOrigin.Text, txtSizes.Text, "", false, false);

                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];
                    // Null Value change
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value == null || cell.Value.ToString() == "")
                            cell.Value = 0;
                    }
                    double purchase_price = Convert.ToDouble(row.Cells["unit_price"].Value);
                    //운임 추가단가
                    double freight_unit_price;
                    if (row.Cells["freight_unit_price"].Value == null || !double.TryParse(row.Cells["freight_unit_price"].Value.ToString(), out freight_unit_price))
                        freight_unit_price = 0;
                    bool is_CFR;
                    if (row.Cells["is_CFR"].Value == null || !bool.TryParse(row.Cells["is_CFR"].Value.ToString(), out is_CFR))
                        is_CFR = false;
                    //if(is_CFR)
                        purchase_price += freight_unit_price;

                    double custom = Convert.ToDouble(row.Cells["custom"].Value) / 100;
                    double tax = Convert.ToDouble(row.Cells["tax"].Value) / 100;
                    double fixed_tariff;
                    if (row.Cells["fixed_tariff"].Value == null || !double.TryParse(row.Cells["fixed_tariff"].Value.ToString(), out fixed_tariff))
                        fixed_tariff = 0;
                    fixed_tariff /= 1000;
                    double incidental_expense = Convert.ToDouble(row.Cells["incidental_expense"].Value) / 100;
                    double box_weight;
                    string txt = row.Cells["box_weight"].Value.ToString().Replace("벌크", "").Trim();
                    if (!double.TryParse(txt, out box_weight))
                        box_weight = 0;
                    double cost_unit;
                    if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                        cost_unit = 0;
                    //계산방식
                    double unit;
                    if (Convert.ToBoolean(row.Cells["tray_calculate"].Value))
                        unit = cost_unit;
                    else
                        unit = box_weight;
                    //판매가, 매입가 변환===================================================================================================
                    //판매가
                    double sales_unit;
                    if (row.Cells["sales_unit"].Value == null || !double.TryParse(row.Cells["sales_unit"].Value.ToString(), out sales_unit))
                        sales_unit = 1;
                    double domestic_sales_price;
                    if (row.Cells["domestic_sales_price"].Value == null || !double.TryParse(row.Cells["domestic_sales_price"].Value.ToString(), out domestic_sales_price))
                        domestic_sales_price = 0;

                    domestic_sales_price = domestic_sales_price * (box_weight / sales_unit);
                    if (double.IsNaN(domestic_sales_price))
                        domestic_sales_price = 0;
                    row.Cells["domestic_sales_price1"].Value = domestic_sales_price.ToString("#,##0");
                    //매입가
                    double purchase_unit;
                    if (row.Cells["purchase_unit"].Value == null || !double.TryParse(row.Cells["purchase_unit"].Value.ToString(), out purchase_unit))
                        purchase_unit = 1;
                    double purchase_origin_price;
                    if (row.Cells["purchase_price"].Value == null || !double.TryParse(row.Cells["purchase_price"].Value.ToString(), out purchase_origin_price))
                        purchase_origin_price = 0;
                    purchase_origin_price = purchase_origin_price * (box_weight / purchase_unit);
                    if (double.IsNaN(purchase_origin_price))
                        purchase_origin_price = 0;
                    row.Cells["purchase_price1"].Value = purchase_origin_price.ToString("#,##0");

                    //재고량
                    double stock;
                    if (cbShipmentStock.Checked)
                        stock = Convert.ToDouble(row.Cells["stock_add_shipment"].Value.ToString());
                    else
                        stock = Convert.ToDouble(row.Cells["stock_exclude_shipment"].Value.ToString());

                    row.Cells["stock_exclude_shipment"].Value = stock.ToString("#,##0");

                    //회전율
                    double sales_count = Convert.ToDouble(row.Cells["sales_count"].Value.ToString());
                    double sales_enable_days = stock / Math.Round(sales_count);
                    if (double.IsNaN(sales_enable_days))
                        sales_enable_days = 0;
                    //double month_around = sales_enable_days / 21;
                    double month_around = sales_enable_days;
                    if (double.IsNaN(month_around))
                        row.Cells["month_around"].Value = 0;
                    else if (double.IsInfinity(month_around))
                        row.Cells["month_around"].Value = "9999";
                    else
                        row.Cells["month_around"].Value = month_around.ToString("#,##0.00");

                    //환율
                    double exchange_rate = Convert.ToDouble(row.Cells["exchange_rate"].Value);
                    //원가계산
                    double cost_price = unit * purchase_price * exchange_rate;
                    //고지관세
                    if (fixed_tariff > 0)
                    {
                        cost_price += (fixed_tariff * unit * exchange_rate * custom);
                        if (tax > 0)
                            cost_price *= (tax + 1);
                        if (incidental_expense > 0)
                            cost_price *= (incidental_expense + 1);
                    }
                    //일반관세
                    else
                    {
                        if (custom > 0)
                            cost_price *= (custom + 1);
                        if (tax > 0)
                            cost_price *= (tax + 1);
                        if (incidental_expense > 0)
                            cost_price *= (incidental_expense + 1);
                    }
                   
                    double sales_price = Convert.ToDouble(row.Cells["domestic_sales_price1"].Value);
                    double purchase_price1 = Convert.ToDouble(row.Cells["purchase_price1"].Value);
                    double trq = Convert.ToDouble(txtTrq.Text.Replace(",", ""));

                    //원가
                    row.Cells["cost_price"].Value = cost_price.ToString("#,##0");
                    //단품판매가, 단품매입가   ===================================================================================================
                    double per_box_unit;
                    if (rbTray.Checked)
                        per_box_unit = cost_unit;
                    else
                        per_box_unit = box_weight;

                    row.Cells["per_box_cost_price"].Value = (cost_price / per_box_unit).ToString("#,##0");
                    row.Cells["per_box_purchase_price"].Value = (purchase_origin_price / per_box_unit).ToString("#,##0");
                    row.Cells["per_box_domestic_sales_price"].Value = (domestic_sales_price / per_box_unit).ToString("#,##0");
                    //TRQ
                    double trq_price = (purchase_price * unit * (1 + tax + incidental_expense) * exchange_rate) + (box_weight * trq);
                    row.Cells["trq"].Value = trq_price.ToString("#,##0");
                    row.Cells["trq_per_box"].Value = (trq_price / cost_unit).ToString("#,##0");

                    //원가 마진금액, 마진율
                    double margin_price = 0;
                    double margin_rate = 0;
                    if (rbSalesPrice.Checked)
                    {
                        margin_price = sales_price - cost_price;
                        margin_rate = ((sales_price - cost_price) / sales_price) * 100;
                    }
                    else
                    {
                        margin_price = purchase_price1 - cost_price;
                        margin_rate = ((purchase_price1 - cost_price) / purchase_price1) * 100;
                    }
                    if (double.IsNaN(margin_rate))
                        margin_rate = 0;
                    row.Cells["margin_rate"].Value = margin_rate.ToString("#,##0.00") + "%";

                    //TRQ 마진
                    double trq_margin = 0;
                    if (rbSalesPrice.Checked)
                        trq_margin = (((sales_price - Convert.ToDouble(row.Cells["trq"].Value.ToString().Replace(",", ""))) / sales_price) * 100);
                    else
                        trq_margin = (((purchase_price1 - Convert.ToDouble(row.Cells["trq"].Value.ToString().Replace(",", ""))) / purchase_price1) * 100);
                    row.Cells["trq_margin"].Value = trq_margin.ToString("#,##0.00") + "%";
                    double trq_cost_margin = trq_margin - margin_rate;
                    row.Cells["trq_cost_margin"].Value = trq_cost_margin.ToString("#,##0.00") + "%";

                    //평균원가2
                    double average_cost_price1;
                    if (row.Cells["average_cost_price1"].Value == null || !double.TryParse(row.Cells["average_cost_price1"].Value.ToString(), out average_cost_price1))
                        average_cost_price1 = 0;
                    double assort;
                    if (row.Cells["assort"].Value == null || !double.TryParse(row.Cells["assort"].Value.ToString(), out assort))
                        assort = 0;
                    double average_cost_price2 = (average_cost_price1 * stock + cost_price * assort) / (stock + assort);
                    row.Cells["average_cost_price2"].Value = average_cost_price2.ToString("#,##0");
                }
            }
        }

        private void calculateWeight()
        {
            dgvProduct.EndEdit();
            double total_weight = 0;
            double total_qty = 0;
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                DataGridViewRow row = dgvProduct.Rows[i];
                double qty;
                if (row.Cells["assort"].Value == null || !double.TryParse(row.Cells["assort"].Value.ToString(), out qty))
                    qty = 0;
                total_qty += qty;
                double assort;
                if (row.Cells["assort"].Value == null || !double.TryParse(row.Cells["assort"].Value.ToString(), out assort))
                    assort = 0;
                double box_weight;
                if (row.Cells["box_weight"].Value == null || !double.TryParse(row.Cells["box_weight"].Value.ToString(), out box_weight))
                    box_weight = 1;
                total_weight += assort * box_weight;
            }
            txtTotalWeghit.Text = total_weight.ToString("#,##0");
            txtTotalQty.Text = total_qty.ToString("#,##0");

            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            { 
                DataGridViewRow row = dgvProduct.Rows[i];
                double stock;
                if (row.Cells["stock"].Value == null || !double.TryParse(row.Cells["stock"].Value.ToString(), out stock))
                    stock = 0;
                double sales_count;
                if (row.Cells["sales_count"].Value == null || !double.TryParse(row.Cells["sales_count"].Value.ToString(), out sales_count))
                    sales_count = 0;
                double box_weight;
                if (row.Cells["box_weight"].Value == null || !double.TryParse(row.Cells["box_weight"].Value.ToString(), out box_weight))
                    box_weight = 1;

                //회전율
                double month_around = stock / sales_count;
                if (double.IsNaN(month_around))
                    row.Cells["month_around"].Value = 0;
                else if (double.IsInfinity(month_around))
                    row.Cells["month_around"].Value = "9999";
                else
                    row.Cells["month_around"].Value = month_around.ToString("#,##0.00");

                double assort;
                if (row.Cells["assort"].Value == null || !double.TryParse(row.Cells["assort"].Value.ToString(), out assort))
                    assort = 0;

                //비율
                row.Cells["assort_weight"].Value = (assort * box_weight).ToString("#,##0.00");
                row.Cells["weight_rate"].Value = ((assort * box_weight) / total_weight * 100).ToString("#,##0.00") + "%";
                

                //회전율
                double total_stock = stock + assort;
                double total_month_around = total_stock / sales_count;
                if (double.IsNaN(total_month_around))
                    row.Cells["assort_month_around"].Value = 0;
                else if (double.IsInfinity(total_month_around))
                    row.Cells["assort_month_around"].Value = "9999";
                else
                    row.Cells["assort_month_around"].Value = total_month_around.ToString("#,##0.00");
            }
        }
        private void SetColumn()
        {
            /*this.dgvProduct.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!*/
            this.dgvProduct.DeleteHighHeader();
            this.dgvProduct.Init(false);
            this.dgvProduct.DefaultCellStyle.Font = new Font("중고딕", 8, FontStyle.Regular);
            
            //Multi Header Setting
            this.dgvProduct.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            this.dgvProduct.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvProduct.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProduct.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToResizeColumns = true;
            this.dgvProduct.AllowUserToResizeRows = false;
            /*this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.ColumnHeadersHeight = 46;  // 2줄 기준*/

            //첫번째 헤더
            /*this.dgvProduct.AddHighHeader(0, 5, "품목정보", "LightSteelBlue");
            this.dgvProduct.AddHighHeader(6, 13, "수입원가 및 마진율", "DarkOrange");
            this.dgvProduct.AddHighHeader(14, 19, "국내시세 및 마진율", "Gainsboro");
            this.dgvProduct.AddHighHeader(20, 22, "TRQ", "LightGreen");*/

            //두번째 헤더
            this.dgvProduct.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            this.dgvProduct.Columns["product"].HeaderCell.Style.BackColor = Color.FromArgb(221, 235, 247); 
            this.dgvProduct.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["origin"].HeaderCell.Style.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["origin"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["sizes"].HeaderCell.Style.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["sizes"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["weight_calculate"].HeaderCell.Style.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["box_weight"].HeaderCell.Style.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["box_weight"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["tray_calculate"].HeaderCell.Style.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["cost_unit"].HeaderCell.Style.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["cost_unit"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);

            this.dgvProduct.Columns["month_around"].HeaderCell.Style.BackColor = Color.FromArgb(231, 235, 247);
            this.dgvProduct.Columns["month_around"].DefaultCellStyle.BackColor = Color.FromArgb(231, 235, 247);

            this.dgvProduct.Columns["month_around_add_shipment"].HeaderCell.Style.BackColor = Color.FromArgb(231, 235, 247);
            this.dgvProduct.Columns["month_around_add_shipment"].DefaultCellStyle.BackColor = Color.FromArgb(231, 235, 247);

            this.dgvProduct.Columns["stock"].HeaderCell.Style.BackColor = Color.FromArgb(231, 235, 247);
            this.dgvProduct.Columns["stock"].DefaultCellStyle.BackColor = Color.FromArgb(231, 235, 247);
            this.dgvProduct.Columns["sales_count"].HeaderCell.Style.BackColor = Color.FromArgb(231, 235, 247);
            this.dgvProduct.Columns["sales_count"].DefaultCellStyle.BackColor = Color.FromArgb(231, 235, 247);
            this.dgvProduct.Columns["month_around"].HeaderCell.Style.BackColor = Color.FromArgb(231, 235, 247);
            this.dgvProduct.Columns["month_around"].DefaultCellStyle.BackColor = Color.FromArgb(231, 235, 247);

            this.dgvProduct.Columns["updatetime"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["updatetime"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["company"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["company"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["unit_price"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["unit_price"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["freight_unit_price"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["freight_unit_price"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["custom"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["custom"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["tax"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["tax"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["incidental_expense"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["incidental_expense"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);

            this.dgvProduct.Columns["fixed_tariff"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["fixed_tariff"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);


            this.dgvProduct.Columns["exchange_rate"].HeaderCell.Style.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["exchange_rate"].DefaultCellStyle.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["cost_price"].HeaderCell.Style.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["cost_price"].DefaultCellStyle.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["margin_rate"].HeaderCell.Style.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["margin_rate"].DefaultCellStyle.BackColor = Color.FromArgb(216, 190, 190);

            this.dgvProduct.Columns["domestic_sales_price1"].HeaderCell.Style.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["domestic_sales_price1"].DefaultCellStyle.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["domestic_sales_price1"].DefaultCellStyle.Font = new Font("중고딕", 7, FontStyle.Bold);
            this.dgvProduct.Columns["purchase_price1"].HeaderCell.Style.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["purchase_price1"].DefaultCellStyle.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["purchase_price1"].DefaultCellStyle.Font = new Font("중고딕", 7, FontStyle.Bold);


            Color col4 = Color.FromArgb(255, 243, 243);
            this.dgvProduct.Columns["per_box_cost_price"].HeaderCell.Style.BackColor = col4;
            this.dgvProduct.Columns["per_box_cost_price"].DefaultCellStyle.BackColor = col4;

            this.dgvProduct.Columns["per_box_domestic_sales_price"].HeaderCell.Style.BackColor = col4;
            this.dgvProduct.Columns["per_box_domestic_sales_price"].DefaultCellStyle.BackColor = col4;

            this.dgvProduct.Columns["per_box_purchase_price"].HeaderCell.Style.BackColor = col4;
            this.dgvProduct.Columns["per_box_purchase_price"].DefaultCellStyle.BackColor = col4;

            /*this.dgvProduct.Columns["margin_price2"].HeaderCell.Style.BackColor = Color.FromArgb(255, 242, 204);
            this.dgvProduct.Columns["margin_price2"].DefaultCellStyle.BackColor = Color.FromArgb(255, 242, 204);
            this.dgvProduct.Columns["margin_rate2"].HeaderCell.Style.BackColor = Color.FromArgb(255, 242, 204);
            this.dgvProduct.Columns["margin_rate2"].DefaultCellStyle.BackColor = Color.FromArgb(255, 242, 204);*/

            /*this.dgvProduct.Columns["margin_price3"].HeaderCell.Style.BackColor = Color.FromArgb(255, 242, 204);
            this.dgvProduct.Columns["margin_price3"].DefaultCellStyle.BackColor = Color.FromArgb(255, 242, 204);
            this.dgvProduct.Columns["margin_rate3"].HeaderCell.Style.BackColor = Color.FromArgb(255, 242, 204);
            this.dgvProduct.Columns["margin_rate3"].DefaultCellStyle.BackColor = Color.FromArgb(255, 242, 204);*/

            this.dgvProduct.Columns["trq"].HeaderCell.Style.BackColor = Color.FromArgb(226, 239, 218);
            this.dgvProduct.Columns["trq"].DefaultCellStyle.BackColor = Color.FromArgb(226, 239, 218);

            this.dgvProduct.Columns["trq_per_box"].HeaderCell.Style.BackColor = Color.FromArgb(226, 239, 218);
            this.dgvProduct.Columns["trq_per_box"].DefaultCellStyle.BackColor = Color.FromArgb(226, 239, 218);


            //컬럼 넓이
            //this.dgvProduct.ColumnHeadersDefaultCellStyle.Padding = new Padding(0);
            this.dgvProduct.Columns["incidental_expense"].HeaderCell.Style.Font = new Font("중고딕", 7, FontStyle.Regular);
            this.dgvProduct.Columns["fixed_tariff"].HeaderCell.Style.Font = new Font("중고딕", 7, FontStyle.Regular);

            this.dgvProduct.Columns["cost_price"].DefaultCellStyle.Font = new Font("중고딕", 7, FontStyle.Bold);
            this.dgvProduct.Columns["unit_price"].DefaultCellStyle.Font = new Font("중고딕", 7, FontStyle.Bold);
            this.dgvProduct.Columns["trq"].DefaultCellStyle.Font = new Font("중고딕", 7, FontStyle.Bold);
            this.dgvProduct.Columns["trq_per_box"].DefaultCellStyle.Font = new Font("중고딕", 7, FontStyle.Bold);

            this.dgvProduct.Columns["trq_cost_margin"].DefaultCellStyle.Font = new Font("중고딕", 7, FontStyle.Bold);


            //Alingment
            dgvProduct.Columns["box_weight"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["cost_unit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            for (int i = 3; i < dgvProduct.ColumnCount; i++)
            {
                if(dgvProduct.Columns[i].Name != "company" && dgvProduct.Columns[i].Name != "updatetime")
                    dgvProduct.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            this.dgvProduct.ColumnHeadersDefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);

            //VIsible
            this.dgvProduct.Columns["sizes2"].Visible = false;
            this.dgvProduct.Columns["stock"].Visible = false;
            this.dgvProduct.Columns["sales_count"].Visible = false;
            this.dgvProduct.Columns["month_around"].Visible = false;
            this.dgvProduct.Columns["month_around_add_shipment"].Visible = false;
            //this.dgvProduct.Columns["assort"].Visible = false;
            this.dgvProduct.Columns["assort_weight"].Visible = false;
            this.dgvProduct.Columns["weight_rate"].Visible = false;
            this.dgvProduct.Columns["assort_month_around"].Visible = false;
            this.dgvProduct.Columns["tray_calculate"].Visible = false;
            this.dgvProduct.Columns["income_amount1"].Visible = false;
            this.dgvProduct.Columns["income_amount2"].Visible = false;

            this.dgvProduct.Columns["assort_margin1"].Visible = false;
            this.dgvProduct.Columns["sales_unit"].Visible = false;
            this.dgvProduct.Columns["purchase_unit"].Visible = false;
            dgvProduct.Columns["purchase_price"].Visible = false;
            dgvProduct.Columns["domestic_sales_price"].Visible = false;

            if (rbPurchaseprice.Checked)
            {
                dgvProduct.Columns["purchase_price1"].Visible = true;
                dgvProduct.Columns["domestic_sales_price1"].Visible = false;
            }
            else
            {
                dgvProduct.Columns["purchase_price1"].Visible = false;
                dgvProduct.Columns["domestic_sales_price1"].Visible = true;
            }
            
            dgvProduct.Columns["trq"].Visible = cbTrq.Checked;
            dgvProduct.Columns["trq_per_box"].Visible = cbTrq.Checked;
            dgvProduct.Columns["trq_margin"].Visible = cbTrq.Checked;
            dgvProduct.Columns["trq_cost_margin"].Visible = cbTrq.Checked;

            dgvProduct.Columns["per_box_cost_price"].Visible = cbStockSales.Checked;
            if (rbPurchaseprice.Checked)
            {
                dgvProduct.Columns["per_box_purchase_price"].Visible = cbStockSales.Checked;
                dgvProduct.Columns["per_box_domestic_sales_price"].Visible = false;
            }
            else
            {
                dgvProduct.Columns["per_box_purchase_price"].Visible = false;
                dgvProduct.Columns["per_box_domestic_sales_price"].Visible = cbStockSales.Checked;
            }
        }
        public void GetColumnSetting(List<string> col_name)
        {
            if (col_name.Count == 0)
                return;

            int col_cnt = col_name.Count;

            try
            {
                //Refresh
                for (int i = 0; i < dgvProduct.Columns.Count; i++)
                {
                    dgvProduct.Columns[i].Visible = false;
                }
                //Col_index
                List<int> col_idx = new List<int>();
                for (int i = 0; i < col_name.Count; i++)
                {
                    dgvProduct.Columns[col_name[i]].Visible = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message.ToString());
                this.Activate();
            }
        }
        #endregion



        #region Key Event
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Libs.Tools.Common common = new Libs.Tools.Common();
            Control tb = common.FindFocusedControl(this);
            /*if (tb.Name != null && Name != "dgvProduct")
            {
                switch (keyData)
                {
                    case Keys.Up:
                        if (tb.Name == "txtExchangeRate")
                        {
                            txtCompany.Focus();
                        }
                        else
                        {
                            tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
                        }
                        return true;
                    case Keys.Down:
                        if (tb.Name == "txtCompany")
                        {
                            txtExchangeRate.Focus();
                        }
                        else
                        {
                            tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                        }
                        return true;
                }
            }*/

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void txtCurrentDate_KeyDown_1(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    GetProduct();
                    break;
            }
        }
        
        private void CostAccounting_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        btnRegisteration.PerformClick();
                        break;
                    case Keys.Q:
                        GetProduct();
                        break;
                    case Keys.D:
                        //거래처별 보기
                        if (dgvProduct.SelectedRows.Count > 0 && cag == null)
                        {
                            FormCollection fc = Application.OpenForms;
                            foreach (Form frm in fc)
                            {
                                //iterate through
                                if (frm.Name == "CostAccountingGroup")
                                {
                                    cag = (CostAccountingGroup)frm;
                                    break;
                                }
                            }
                        }
                        //없으면 새로생성
                        if (cag == null)
                        {
                            double trq;
                            if (!double.TryParse(txtTrq.Text.Replace(",", ""), out trq))
                                trq = 0;
                            cag = new CostAccountingGroup(um, trq);
                        }
                            


                        //선택한 DgvRow List생성
                        List<DataGridViewRow> productList = new List<DataGridViewRow>();
                        for (int i = 0; i < dgvProduct.Rows.Count; i++)
                        {
                            DataGridViewRow row = dgvProduct.Rows[i];
                            bool isCheck = Convert.ToBoolean(row.Selected);
                            if (isCheck)
                                productList.Add(row);
                        }

                        //거래처별 보기로 데이터 넘기기
                        cag.SetProduct(productList);

                        //대시보드로 데이터 넘기기
                        if (dgvProduct.SelectedRows.Count > 0)
                        {
                            FormCollection fc = Application.OpenForms;
                            foreach (Form frm in fc)
                            {
                                //iterate through
                                if (frm.Name == "DetailDashboard")
                                {
                                    DetailDashboard dd = (DetailDashboard)frm;
                                    dd.InputCostAccountingData(productList);
                                }
                            }
                        }
                        clipboardList = productList;
                        break;

                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                    case Keys.M:
                        txtProduct.Focus();
                        break;
                    case Keys.N:
                        txtProduct.Text = "";
                        txtOrigin.Text = "";
                        txtSizes.Text = "";
                        txtUnit.Text = "";
                        txtCompany.Text = "";
                        txtManager.Text = "";
                        txtProduct.Focus();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.P:
                        btnPreview.PerformClick();
                        break;
                    case Keys.C:
                        //거래처별 보기
                        if (dgvProduct.SelectedRows.Count > 0 && cag == null)
                        {
                            FormCollection fc = Application.OpenForms;
                            foreach (Form frm in fc)
                            {
                                //iterate through
                                if (frm.Name == "CostAccountingGroup")
                                {
                                    cag = (CostAccountingGroup)frm;
                                    break;
                                }
                            }
                        }
                        //없으면 새로생성
                        if (cag == null)
                        {
                            double trq;
                            if (!double.TryParse(txtTrq.Text.Replace(",", ""), out trq))
                                trq = 0;
                            cag = new CostAccountingGroup(um, trq);
                        }
                        

                        //선택한 DgvRow List생성
                        List<DataGridViewRow> productList = new List<DataGridViewRow>();
                        for (int i = 0; i < dgvProduct.Rows.Count; i++)
                        {
                            DataGridViewRow row = dgvProduct.Rows[i];
                            bool isCheck = Convert.ToBoolean(row.Selected);
                            if (isCheck)
                                productList.Add(row);
                        }

                        //거래처별 보기로 데이터 넘기기
                        cag.SetProduct(productList);

                        //대시보드로 데이터 넘기기
                        if (dgvProduct.SelectedRows.Count > 0)
                        {
                            FormCollection fc = Application.OpenForms;
                            foreach (Form frm in fc)
                            {
                                //iterate through
                                if (frm.Name == "DetailDashboard")
                                {
                                    DetailDashboard dd = (DetailDashboard)frm;
                                    dd.InputCostAccountingData(productList);
                                }
                            }
                        }
                        clipboardList = productList;
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        cbStockSales.Checked = !cbStockSales.Checked;
                        break;
                    case Keys.F2:
                        cbAssort.Checked = !cbAssort.Checked;
                        break;
                    case Keys.F3:
                        cbTrq.Checked = !cbTrq.Checked;
                        break;
                    case Keys.F4:
                        ProductManager ps = new ProductManager(um, this);
                        ps.ShowDialog();
                        break;
                    case Keys.F5:
                        dgvProduct.Rows.Clear();
                        break;
                    case Keys.F6:
                        if(rbSalesPrice.Checked)
                            rbPurchaseprice.Checked = true;
                        else
                            rbSalesPrice.Checked = true;
                        break;
                    case Keys.F7:
                        if (cbPerBox.Checked && rbWeight.Checked)
                            rbTray.Checked = true;
                        else if (cbPerBox.Checked && rbTray.Checked)
                        {
                            rbWeight.Checked = true;
                            cbPerBox.Checked = !cbPerBox.Checked;
                        }
                        else
                        {
                            cbPerBox.Checked = !cbPerBox.Checked;
                        }
                        break;
                    case Keys.F8:
                        btnBookmark.PerformClick();
                        break;                        
                }
            }
        }
        private void txtTrq_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == (Char)46 || e.KeyChar == (Char)45))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }
        private void txtSttdate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                System.Windows.Forms.TextBox tbb = (System.Windows.Forms.TextBox)sender;
                tbb.Text = common.strDatetime(tbb.Text);
                if (tbb.Name != "txtSttdate" && tbb.Name != "txtEnddate")
                {
                    DateTime dt;
                    if (DateTime.TryParse(tbb.Text, out dt))
                    {
                        tbb.Text = dt.ToString("yyyy-MM-dd");
                    }
                    GetProduct();
                }
            }
        }

        private void txtSttdate_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Windows.Forms.TextBox tb = (System.Windows.Forms.TextBox)sender;
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(45) || e.KeyChar == Convert.ToChar(47)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }
        private void txtTrq_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Control tb = (Control)sender;
                if (tb.Name == "txtUsd" && rbUsd.Checked)
                {
                    foreach (DataGridViewRow row in dgvProduct.Rows)
                    {
                        row.Cells["exchange_rate"].Value = Convert.ToDouble(tb.Text).ToString("#,##0.00");
                    }
                }
                else if (tb.Name == "txtEur" && rbEur.Checked)
                {
                    foreach (DataGridViewRow row in dgvProduct.Rows)
                    {
                        row.Cells["exchange_rate"].Value = Convert.ToDouble(tb.Text).ToString("#,##0.00");
                    }
                }
                else if (tb.Name == "txtTrq")
                {
                    //수정
                    if (MessageBox.Show(this,"TRQ 기준금액을 변경하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        double trq_price;
                        if (!double.TryParse(txtTrq.Text.Replace(",", ""), out trq_price))
                            trq_price = 0;

                        if (commonRepository.UpdateTrq(trq_price) == -1)
                            MessageBox.Show(this,"등록중 에러가 발생하였습니다.");
                        else
                            MessageBox.Show(this,"완료");
                    }
                }
                calculate();
            }
        }

        #endregion

        #region CheckBox, Ridio, Button
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        private void btnRegisteration_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "원가계산", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            if (dgvProduct.Rows.Count == 0)
            {
                MessageBox.Show(this,"수정할 내역이 없습니다.");
                return;
            }
            if (MessageBox.Show(this,"오퍼내역을 수정하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;


            List<StringBuilder> sqlList = new List<StringBuilder>();
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                DataGridViewRow row = dgvProduct.Rows[i];

                PurchasePriceModel model = new PurchasePriceModel();
                model.id = Convert.ToInt32(row.Cells["id"].Value.ToString());
                model.product = row.Cells["product"].Value.ToString();
                model.origin = row.Cells["origin"].Value.ToString();
                model.sizes = row.Cells["sizes"].Value.ToString();
                model.unit = row.Cells["box_weight"].Value.ToString();
                model.cost_unit = row.Cells["cost_unit"].Value.ToString();

                double custom;
                if (row.Cells["custom"].Value == null || !double.TryParse(row.Cells["custom"].Value.ToString(), out custom))
                    custom = 0;
                custom /= 100;
                model.custom = custom;

                double tax;
                if (row.Cells["tax"].Value == null || !double.TryParse(row.Cells["tax"].Value.ToString(), out tax))
                    tax = 0;
                tax /= 100;
                model.tax = tax;

                double incidental_expense;
                if (row.Cells["incidental_expense"].Value == null || !double.TryParse(row.Cells["incidental_expense"].Value.ToString(), out incidental_expense))
                    incidental_expense = 0;
                incidental_expense /= 100;
                model.incidental_expense = incidental_expense;

                double fixed_tariff;
                if (row.Cells["fixed_tariff"].Value == null || !double.TryParse(row.Cells["fixed_tariff"].Value.ToString(), out fixed_tariff))
                    fixed_tariff = 0;
                model.fixed_tariff = fixed_tariff;

                model.company = row.Cells["company_id"].Value.ToString();

                bool weight_calculate;
                if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                    weight_calculate = false;
                model.weight_calculate = weight_calculate;

                bool is_FOB;
                if (row.Cells["is_FOB"].Value == null || !bool.TryParse(row.Cells["is_FOB"].Value.ToString(), out is_FOB))
                    is_FOB = false;
                model.is_FOB = is_FOB;

                double purchase_price;
                if (row.Cells["unit_price"].Value == null || !double.TryParse(row.Cells["unit_price"].Value.ToString(), out purchase_price))
                    purchase_price = 0;

                model.purchase_price = purchase_price;

                model.edit_user = um.user_name;
                model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");

                StringBuilder sql = purchasePriceRepository.UpdatePurchasePrice(model);
                sqlList.Add(sql);
            }
            //Execute
            if (commonRepository.UpdateTran(sqlList) == -1)
                MessageBox.Show(this,"수정중 에러가 발생하였습니다.");
            else
                MessageBox.Show(this,"수정완료");
            this.Activate();
        }
        private void cbSaleTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sale_terms;
            switch (cbSaleTerms.Text)
            {
                case "1개월":
                    sale_terms = 1;
                    break;
                case "45일":
                    sale_terms = 45;
                    break;
                case "2개월":
                    sale_terms = 2;
                    break;
                case "3개월":
                    sale_terms = 3;
                    break;
                case "6개월":
                    sale_terms = 6;
                    break;
                case "12개월":
                    sale_terms = 12;
                    break;
                case "18개월":
                    sale_terms = 18;
                    break;
                default:
                    sale_terms = 6;
                    break;
            }
            DateTime sale_sttdate = DateTime.Now.AddMonths(-sale_terms);
            if (sale_terms == 45)
                sale_sttdate = DateTime.Now.AddDays(-sale_terms);
            DateTime sale_enddate = DateTime.Now;
            //영업일수
            common.GetWorkDay(sale_sttdate, sale_enddate, out workDays);
            workDays--;

            if (dgvProduct.Rows.Count > 0)
            {
                string sub_product = "";
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    sub_product += "\n" + dgvProduct.Rows[i].Cells["product"].Value.ToString()
                                + "^" + dgvProduct.Rows[i].Cells["origin"].Value.ToString()
                                + "^" + dgvProduct.Rows[i].Cells["sizes"].Value.ToString()
                                + "^" + dgvProduct.Rows[i].Cells["box_weight"].Value.ToString();
                }
                DataTable salesDt = salesRepository.GetSalesQty(sale_terms, sub_product);
                if (salesDt.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    {
                        string whr = $"품명 = '{dgvProduct.Rows[i].Cells["product"].Value.ToString()}' "
                                    + $"AND 원산지 = '{dgvProduct.Rows[i].Cells["origin"].Value.ToString()}' "
                                    + $"AND 규격 = '{dgvProduct.Rows[i].Cells["sizes"].Value.ToString()}' "
                                    + $"AND 단위 = '{dgvProduct.Rows[i].Cells["box_weight"].Value.ToString()}' ";
                        DataRow[] dr = salesDt.Select(whr);
                        if (dr.Length > 0)
                        {
                            double stock;
                            if (dgvProduct.Rows[i].Cells["stock"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["stock"].Value.ToString(), out stock))
                                stock = 0;

                            double sales_count;
                            if (!double.TryParse(salesDt.Rows[0]["매출수"].ToString(), out sales_count))
                                sales_count = 0;

                            double avg_sales_count = sales_count / workDays * 21;

                            dgvProduct.Rows[i].Cells["sales_count"].Value = avg_sales_count.ToString("#,##0.00");
                        }
                    }
                }
            }
        }
        private void cbSortType_SelectedValueChanged(object sender, EventArgs e)
        {
            //Datatable

            DataTable dt = common.ConvertDgvToDataTable(dgvProduct);
            DataView dv = new DataView(dt);
            //정렬방식
            int sortType = 1;
            switch (cbSortType.Text)
            {
                case "품명+원산지+규격":
                    sortType = 1;
                    dv.Sort = "product, origin, sizes";
                    break;
                case "거래처+품명+원산지+규격":
                    sortType = 2;
                    dv.Sort = "company, product, origin, sizes";
                    break;
                case "품명+원산지+규격+오퍼가":
                    sortType = 3;
                    dv.Sort = "product, origin, sizes, unit_price";
                    break;
                case "오퍼가+품명+원산지+규격":
                    sortType = 4;
                    dv.Sort = "unit_price, product, origin, sizes";
                    break;
                case "품명+원산지+규격+오퍼일자":
                    sortType = 5;
                    dv.Sort = "product, origin, sizes, updatetime";
                    break;
                case "오퍼일자+품명+원산지+규격":
                    sortType = 6;
                    dv.Sort = "updatetime, product, origin, sizes";
                    break;
            }
            dt = dv.ToTable();
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            dgvProduct.Rows.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int n = dgvProduct.Rows.Add();
                for (int j = 0; j < dgvProduct.ColumnCount; j++)
                {
                    dgvProduct.Rows[n].Cells[j].Value = dt.Rows[i][j];
                    if (dgvProduct.Rows[n].Cells[j].Value == null)
                        dgvProduct.Rows[n].Cells[j].Value = 0;
                }
            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            //컬럼순서
            /*if (dgvProduct.Columns.Count > 0)
            {
                dgvProduct.Columns["product"].DisplayIndex = 0;
                dgvProduct.Columns["origin"].DisplayIndex = 1;
                dgvProduct.Columns["sizes"].DisplayIndex = 2;
                dgvProduct.Columns["company"].DisplayIndex = 11;
                dgvProduct.Columns["updatetime"].DisplayIndex = 12;
                dgvProduct.Columns["unit_price"].DisplayIndex = 13;
                switch (sortType)
                {
                    case 1:
                        dgvProduct.Columns["product"].DisplayIndex = 0;
                        dgvProduct.Columns["origin"].DisplayIndex = 1;
                        dgvProduct.Columns["sizes"].DisplayIndex = 2;
                        break;
                    case 2:
                        dgvProduct.Columns["company"].DisplayIndex = 0;
                        dgvProduct.Columns["product"].DisplayIndex = 1;
                        dgvProduct.Columns["origin"].DisplayIndex = 2;
                        dgvProduct.Columns["sizes"].DisplayIndex = 3;
                        break;
                    case 3:
                        dgvProduct.Columns["product"].DisplayIndex = 0;
                        dgvProduct.Columns["origin"].DisplayIndex = 1;
                        dgvProduct.Columns["sizes"].DisplayIndex = 2;
                        dgvProduct.Columns["unit_price"].DisplayIndex = 3;
                        break;
                    case 4:
                        dgvProduct.Columns["unit_price"].DisplayIndex = 0;
                        dgvProduct.Columns["product"].DisplayIndex = 1;
                        dgvProduct.Columns["origin"].DisplayIndex = 2;
                        dgvProduct.Columns["sizes"].DisplayIndex = 3;
                        break;
                    case 5:
                        dgvProduct.Columns["product"].DisplayIndex = 0;
                        dgvProduct.Columns["origin"].DisplayIndex = 1;
                        dgvProduct.Columns["sizes"].DisplayIndex = 2;
                        dgvProduct.Columns["updatetime"].DisplayIndex = 3;
                        break;
                    case 6:
                        dgvProduct.Columns["updatetime"].DisplayIndex = 0;
                        dgvProduct.Columns["product"].DisplayIndex = 1;
                        dgvProduct.Columns["origin"].DisplayIndex = 2;
                        dgvProduct.Columns["sizes"].DisplayIndex = 3;
                        break;
                }
            }*/
        }
        private void rbWeight_CheckedChanged(object sender, EventArgs e)
        {
            calculate();
            calculateAssort();
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "원가계산", "is_excel"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            //비밀번호 확인
            PasswordCheckManager pcm = new PasswordCheckManager(um.excel_password);
            if (!pcm.isPasswordCheck())
                return;

            if (dgvProduct.Columns.Count > 0)
            {
                List<string> col_txt = new List<string>();
                List<string> col_nme = new List<string>();

                for (int i = 0; i < dgvProduct.Columns.Count; i++)
                {
                    if (dgvProduct.Columns[i].Visible)
                    { 
                        col_txt.Add(dgvProduct.Columns[i].HeaderText);
                        col_nme.Add(dgvProduct.Columns[i].Name);
                    }
                }
                GetExeclColumn(col_nme);
            }
        }
        private void cbPerBox_CheckedChanged(object sender, EventArgs e)
        {
            dgvProduct.Columns["per_box_cost_price"].Visible = cbPerBox.Checked;
            if (rbPurchaseprice.Checked)
            {
                dgvProduct.Columns["per_box_purchase_price"].Visible = cbPerBox.Checked;
                dgvProduct.Columns["per_box_domestic_sales_price"].Visible = false;
            }
            else
            {
                dgvProduct.Columns["per_box_purchase_price"].Visible = false;
                dgvProduct.Columns["per_box_domestic_sales_price"].Visible = cbPerBox.Checked;
            }
            rbWeight.Visible = cbPerBox.Checked;
            rbTray.Visible = cbPerBox.Checked;
        }
        private void cbStockSales_CheckedChanged(object sender, EventArgs e)
        {
            dgvProduct.Columns["stock"].Visible = cbStockSales.Checked;
            dgvProduct.Columns["sales_count"].Visible = cbStockSales.Checked;
            dgvProduct.Columns["month_around"].Visible = cbStockSales.Checked;
            //dgvProduct.Columns["month_around_add_shipment"].Visible = cbStockSales.Checked;
        }

        private void cbAssort_CheckedChanged(object sender, EventArgs e)
        {
            dgvProduct.Columns["seaover_cost_price"].Visible = cbAssort.Checked;
            dgvProduct.Columns["pending_cost_price"].Visible = cbAssort.Checked;
            dgvProduct.Columns["average_cost_price1"].Visible = cbAssort.Checked;

            //dgvProduct.Columns["assort"].Visible = cbAssort.Checked;
            dgvProduct.Columns["assort_weight"].Visible = cbAssort.Checked;
            dgvProduct.Columns["weight_rate"].Visible = cbAssort.Checked;
            dgvProduct.Columns["assort_month_around"].Visible = cbAssort.Checked;

            dgvProduct.Columns["average_cost_price2"].Visible = cbAssort.Checked;
            dgvProduct.Columns["income_amount1"].Visible = cbAssort.Checked;
            dgvProduct.Columns["income_amount2"].Visible = cbAssort.Checked;
        }

        private void cbTrq_CheckedChanged(object sender, EventArgs e)
        {
            dgvProduct.Columns["trq"].Visible = cbTrq.Checked;
            dgvProduct.Columns["trq_per_box"].Visible = cbTrq.Checked;
            dgvProduct.Columns["trq_margin"].Visible = cbTrq.Checked;
            dgvProduct.Columns["trq_cost_margin"].Visible = cbTrq.Checked;
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            int n = dgvProduct.Rows.Add();
            dgvProduct.Rows[n].Cells["product"].Value = "임의품목";
            dgvProduct.Rows[n].Cells["company"].Value = "임의거래처";
            dgvProduct.Rows[n].Cells["unit"].Value = "0";
            if(rbUsd.Checked)
                dgvProduct.Rows[n].Cells["exchange_rate"].Value = txtUsd.Text;
            else
                dgvProduct.Rows[n].Cells["exchange_rate"].Value = txtEur.Text;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgvProduct.Rows.Clear();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            GetProduct();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            calculate();
        }
        private void btnCalendarCurrentDate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
            {
                txtSttdate.Text = sdate;
            }
        }
        private void btnCalendarEnddate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
            {
                txtEnddate.Text = sdate;
            }
        }

        private void btnBookmark_Click(object sender, EventArgs e)
        {
            if (isBookmarkMode)
            {
                txtGroupName.Text = String.Empty;
                dgvProduct.Rows.Clear();
                lbGroupId.Text = "NULL";
                btnBookmark.Text = "즐겨찾기 선택(F8)";
                btnBookmark.ForeColor = Color.Black;
                isBookmarkMode = false;
            }
            else
            {
                txtProduct.Text = "";
                txtOrigin.Text = "";
                txtSizes.Text = "";
                txtUnit.Text = "";
                txtManager.Text = "";
                txtCompany.Text = "";

                BookmarkManager bm = new BookmarkManager(um, this);
                bm.StartPosition = FormStartPosition.CenterParent;
                bm.ShowDialog();
            }
        }
        #endregion

        #region Etc event
        private void txtSttdate_Leave(object sender, EventArgs e)
        {
            Control tb = (Control)sender;
            tb.Text = common.strDatetime(tb.Text);
        }
        private void rbSalesPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPurchaseprice.Checked)
            {
                dgvProduct.Columns["purchase_price1"].Visible = true;
                dgvProduct.Columns["domestic_sales_price1"].Visible = false;
                if (cbPerBox.Checked)
                {
                    dgvProduct.Columns["per_box_purchase_price"].Visible = true;
                    dgvProduct.Columns["per_box_domestic_sales_price"].Visible = false;
                }
            }
            else
            {
                dgvProduct.Columns["purchase_price1"].Visible = false;
                dgvProduct.Columns["domestic_sales_price1"].Visible = true;
                if (cbPerBox.Checked)
                {
                    dgvProduct.Columns["per_box_purchase_price"].Visible = false;
                    dgvProduct.Columns["per_box_domestic_sales_price"].Visible = true;
                }
            }
               
            calculate();
            calculateAssort();
        }
        #endregion

        #region Datagridview event
        private void dgvProduct_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dgvProduct.SelectionMode != DataGridViewSelectionMode.FullRowSelect)
                {
                    dgvProduct.ClearSelection();
                    dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgvProduct.Rows[e.RowIndex].Selected = true;
                }
            }
        }

        private void dgvProduct_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DetailDashboard dd = null;
                FormCollection fc = Application.OpenForms;
                foreach (Form frm in fc)
                {
                    //iterate through
                    if (frm.Name == "DetailDashboard")
                    {
                        dd = (DetailDashboard)frm;
                        break;
                    }
                }

                List<DataGridViewRow> pList = new List<DataGridViewRow>();
                DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                pList.Add(row);

                //있을 경우만
                if (dd != null)
                {
                    clipboardList = pList;
                    dd.inputProduct();
                }
            }
        }
        private void dgvProduct_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            DataGridViewColumn col = dgvProduct.Columns[e.ColumnIndex];
            DataGridViewCell cell = dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex];
            double d;
            if (col.Name == "margin_rate" || col.Name == "trq_margin" || col.Name == "trq_cost_margin")
            {
                if (cell.Value != null)
                { 
                    string txt = cell.Value.ToString().Replace("%", "").Replace(",", "");
                    if (double.TryParse(txt, out d))
                    {
                        if (d < 0)
                            cell.Style.ForeColor = Color.Red;
                        else
                            cell.Style.ForeColor = Color.Black;
                    }
                }
            }
        }
        private void dgvProduct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 3)
            {
                this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                if (dgvProduct.Columns[e.ColumnIndex].Name == "trq")
                {
                    double sales_price = Convert.ToDouble(dgvProduct.Rows[e.RowIndex].Cells["domestic_sales_price1"].Value.ToString());
                    double purchase_price1 = Convert.ToDouble(dgvProduct.Rows[e.RowIndex].Cells["purchase_price1"].Value);

                    //Trq
                    double trq;
                    if (dgvProduct.Rows[e.RowIndex].Cells["trq"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["trq"].Value.ToString(), out trq))
                        trq = 0;
                    dgvProduct.Rows[e.RowIndex].Cells["trq"].Value = trq.ToString("#,##0");

                    double cost_unit;
                    if (dgvProduct.Rows[e.RowIndex].Cells["cost_unit"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["cost_unit"].Value.ToString(), out cost_unit))
                        cost_unit = 0;
                    dgvProduct.Rows[e.RowIndex].Cells["trq_per_box"].Value = (trq / cost_unit).ToString("#,##0");

                    //TRQ 마진
                    double trq_margin = 0;
                    if (rbSalesPrice.Checked)
                        trq_margin = (((sales_price - trq) / sales_price) * 100);
                    else
                        trq_margin = (((purchase_price1 - trq) / purchase_price1) * 100);
                    dgvProduct.Rows[e.RowIndex].Cells["trq_margin"].Value = trq_margin.ToString("#,##0.00") + "%";
                    //원가계산
                    double cost_price;
                    if (dgvProduct.Rows[e.RowIndex].Cells["cost_price"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["cost_price"].Value.ToString(), out cost_price))
                        cost_price = 0;
                    //마진율
                    double margin_rate = 0;
                    if (rbSalesPrice.Checked)
                        margin_rate = ((sales_price - cost_price) / sales_price) * 100;
                    else
                        margin_rate = ((purchase_price1 - cost_price) / sales_price) * 100;
                    double trq_cost_margin = (margin_rate - trq_margin);
                    dgvProduct.Rows[e.RowIndex].Cells["trq_cost_margin"].Value = trq_cost_margin.ToString("#,##0.00") + "%";
                    //손익금액(assort_margin2)
                    double assort_margin2 = sales_price - trq;
                    //dgvProduct.Rows[e.RowIndex].Cells["assort_margin2"].Value = assort_margin2.ToString("#,##0");
                    //전체 손익금액
                    calculateAssort();
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "box_weight"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "stock"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "sales_count"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "assort")
                {
                    double temp;
                    if (dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && double.TryParse(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out temp))
                        dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = temp;

                    //운임
                    bool is_CFR;
                    if (dgvProduct.Rows[e.RowIndex].Cells["is_CFR"].Value == null || !bool.TryParse(dgvProduct.Rows[e.RowIndex].Cells["is_CFR"].Value.ToString(), out is_CFR))
                        is_CFR = false;

                    if (is_CFR)
                    {
                        double freight_charge;
                        if (!double.TryParse(txtFreightCharge.Text, out freight_charge))
                            freight_charge = 0;

                        double assort;
                        if (dgvProduct.Rows[e.RowIndex].Cells["assort"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["assort"].Value.ToString(), out assort))
                            assort = 0;

                        double box_weight;
                        if (dgvProduct.Rows[e.RowIndex].Cells["box_weight"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["box_weight"].Value.ToString(), out box_weight))
                            box_weight = 0;

                        double freight_unit_price = freight_charge / (assort * box_weight);
                        if (double.IsNaN(freight_unit_price) || double.IsInfinity(freight_unit_price))
                            freight_unit_price = 0;
                        dgvProduct.Rows[e.RowIndex].Cells["freight_unit_price"].Value = freight_unit_price;
                    }

                    calculate();
                    calculateWeight();
                    calculateAssort();
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "purchase_price1")
                {
                    double d;
                    if (dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out d))
                        d = 0;

                    double sunit;
                    if (dgvProduct.Rows[e.RowIndex].Cells["purchase_unit"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["purchase_unit"].Value.ToString(), out sunit))
                        sunit = 1;

                    double unit;
                    if (dgvProduct.Rows[e.RowIndex].Cells["box_weight"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["box_weight"].Value.ToString(), out unit))
                        unit = 0;

                    dgvProduct.Rows[e.RowIndex].Cells["purchase_price"].Value = d / unit * sunit;
                    calculate();
                    calculateAssort();
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "domestic_sales_price1")
                {
                    double d;
                    if (dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out d))
                        d = 0;

                    double sunit;
                    if (dgvProduct.Rows[e.RowIndex].Cells["sales_unit"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["sales_unit"].Value.ToString(), out sunit))
                        sunit = 1;

                    double unit;
                    if (dgvProduct.Rows[e.RowIndex].Cells["box_weight"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["box_weight"].Value.ToString(), out unit))
                        unit = 0;

                    dgvProduct.Rows[e.RowIndex].Cells["domestic_sales_price"].Value = d / unit * sunit;
                    calculate();
                    calculateAssort();
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "per_box_cost_price")
                {
                    dgvProduct.EndEdit();

                    DataGridViewRow row = dgvProduct.Rows[e.RowIndex];

                    double per_box_cost_price;
                    if (row.Cells["per_box_cost_price"].Value == null || !double.TryParse(row.Cells["per_box_cost_price"].Value.ToString(), out per_box_cost_price))
                        per_box_cost_price = 0;
                    row.Cells["per_box_cost_price"].Value = per_box_cost_price.ToString("#,##0");

                    if (rbWeight.Checked)
                    {
                        double box_weight;
                        if (row.Cells["box_weight"].Value == null || !double.TryParse(row.Cells["box_weight"].Value.ToString(), out box_weight))
                            box_weight = 1;
                        row.Cells["cost_price"].Value = (per_box_cost_price * box_weight).ToString("#,##0");
                    }
                    else
                    {
                        double cost_unit;
                        if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                            cost_unit = 1;
                        row.Cells["cost_price"].Value = (per_box_cost_price * cost_unit).ToString("#,##0");
                    }
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "per_box_purchase_price")
                {
                    dgvProduct.EndEdit();

                    DataGridViewRow row = dgvProduct.Rows[e.RowIndex];

                    double per_box_purchase_price;
                    if (row.Cells["per_box_purchase_price"].Value == null || !double.TryParse(row.Cells["per_box_purchase_price"].Value.ToString(), out per_box_purchase_price))
                        per_box_purchase_price = 0;

                    row.Cells["per_box_purchase_price"].Value = per_box_purchase_price.ToString("#,##0");


                    double purchase_price;
                    if (rbWeight.Checked)
                    {
                        double box_weight;
                        if (row.Cells["box_weight"].Value == null || !double.TryParse(row.Cells["box_weight"].Value.ToString(), out box_weight))
                            box_weight = 1;
                        purchase_price = (per_box_purchase_price * box_weight);
                        row.Cells["purchase_price1"].Value = (per_box_purchase_price * box_weight).ToString("#,##0");
                    }
                    else
                    {
                        double cost_unit;
                        if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                            cost_unit = 1;
                        purchase_price = (per_box_purchase_price * cost_unit);
                        row.Cells["purchase_price1"].Value = (per_box_purchase_price * cost_unit).ToString("#,##0");
                    }


                    double cost_price;
                    if (row.Cells["cost_price"].Value == null || !double.TryParse(row.Cells["cost_price"].Value.ToString(), out cost_price))
                        cost_price = 0;

                    row.Cells["margin_rate"].Value = ((purchase_price - cost_price) / purchase_price * 100).ToString("#,##0.00") + "%";


                    //원가 마진금액, 마진율
                    double sales_price = Convert.ToDouble(row.Cells["domestic_sales_price1"].Value);
                    double purchase_price1 = Convert.ToDouble(row.Cells["purchase_price1"].Value);
                    double margin_price = 0;
                    double margin_rate = 0;
                    if (rbSalesPrice.Checked)
                    {
                        margin_price = sales_price - cost_price;
                        margin_rate = ((sales_price - cost_price) / sales_price) * 100;
                    }
                    else
                    {
                        margin_price = purchase_price1 - cost_price;
                        margin_rate = ((purchase_price1 - cost_price) / purchase_price1) * 100;
                    }
                    if (double.IsNaN(margin_rate))
                        margin_rate = 0;

                    //TRQ 마진
                    double trq_margin = 0;
                    if (rbSalesPrice.Checked)
                        trq_margin = (((sales_price - Convert.ToDouble(row.Cells["trq"].Value.ToString().Replace(",", ""))) / sales_price) * 100);
                    else
                        trq_margin = (((purchase_price1 - Convert.ToDouble(row.Cells["trq"].Value.ToString().Replace(",", ""))) / purchase_price1) * 100);
                    row.Cells["trq_margin"].Value = trq_margin.ToString("#,##0.00") + "%";
                    double trq_cost_margin = trq_margin - margin_rate;
                    row.Cells["trq_cost_margin"].Value = trq_cost_margin.ToString("#,##0.00") + "%";

                    //calculate();

                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "per_box_domestic_sales_price")
                {
                    dgvProduct.EndEdit();

                    DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                    double per_box_domestic_sales_price;
                    if (row.Cells["per_box_domestic_sales_price"].Value == null || !double.TryParse(row.Cells["per_box_domestic_sales_price"].Value.ToString(), out per_box_domestic_sales_price))
                        per_box_domestic_sales_price = 0;

                    row.Cells["per_box_domestic_sales_price"].Value = per_box_domestic_sales_price.ToString("#,##0");
                    double sales_price;
                    if (rbWeight.Checked)
                    {
                        double box_weight;
                        if (row.Cells["box_weight"].Value == null || !double.TryParse(row.Cells["box_weight"].Value.ToString(), out box_weight))
                            box_weight = 1;
                        sales_price = per_box_domestic_sales_price * box_weight;
                        row.Cells["domestic_sales_price1"].Value = sales_price.ToString("#,##0");

                    }
                    else
                    {
                        double cost_unit;
                        if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                            cost_unit = 1;

                        sales_price = per_box_domestic_sales_price * cost_unit;
                        row.Cells["domestic_sales_price1"].Value = sales_price.ToString("#,##0");
                    }

                    double cost_price;
                    if (row.Cells["cost_price"].Value == null || !double.TryParse(row.Cells["cost_price"].Value.ToString(), out cost_price))
                        cost_price = 0;

                    row.Cells["margin_rate"].Value = ((sales_price - cost_price) / sales_price * 100).ToString("#,##0.00") + "%";
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "cost_unit")
                {
                    this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                    dgvProduct.EndEdit();
                    double cost_unit;
                    if (dgvProduct.Rows[e.RowIndex].Cells["cost_unit"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["cost_unit"].Value.ToString(), out cost_unit))
                        cost_unit = 0;
                    double origin_cost_unit;
                    if (dgvProduct.Rows[e.RowIndex].Cells["origin_cost_unit"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["origin_cost_unit"].Value.ToString(), out origin_cost_unit))
                        origin_cost_unit = 0;

                    double box_weight;
                    if (dgvProduct.Rows[e.RowIndex].Cells["box_weight"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["box_weight"].Value.ToString(), out box_weight))
                        box_weight = 0;
                    box_weight *= cost_unit / origin_cost_unit;
                    dgvProduct.Rows[e.RowIndex].Cells["box_weight"].Value = box_weight.ToString();
                    dgvProduct.Rows[e.RowIndex].Cells["origin_cost_unit"].Value = cost_unit.ToString();
                    this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                }
                /*else if (dgvProduct.Columns[e.ColumnIndex].Name == "is_FOB")
                {
                    dgvProduct.EndEdit();
                    bool is_FOB;
                    if (dgvProduct.Rows[e.RowIndex].Cells["is_FOB"].Value == null || !bool.TryParse(dgvProduct.Rows[e.RowIndex].Cells["is_FOB"].Value.ToString(), out is_FOB))
                        is_FOB = false;

                    if (is_FOB)
                    {
                        if (MessageBox.Show(this, "[오퍼가]에 [+운임] 단가를 추가하시겠습니가?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            double unit_price;
                            if (dgvProduct.Rows[e.RowIndex].Cells["unit_price"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["unit_price"].Value.ToString(), out unit_price))
                                unit_price = 0;
                            double freight_unit_price;
                            if (dgvProduct.Rows[e.RowIndex].Cells["freight_unit_price"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["freight_unit_price"].Value.ToString(), out freight_unit_price))
                                freight_unit_price = 0;

                            dgvProduct.Rows[e.RowIndex].Cells["unit_price"].Value = unit_price + freight_unit_price;
                            dgvProduct.Rows[e.RowIndex].Cells["freight_unit_price"].Value = 0;
                            calculate();
                            calculateAssort();
                        }
                    }
                }*/
                else
                {
                    calculate();
                    calculateAssort();
                }

                this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            }
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvProduct.EndEdit();
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "weight_calculate")
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

                    dgvProduct.Rows[e.RowIndex].Cells["tray_calculate"].Value = !isChecked;
                    calculate();
                    calculateAssort();

                    if (isChecked)
                        dgvProduct.Columns[e.ColumnIndex].DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);
                    else
                        dgvProduct.Columns[e.ColumnIndex].DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Regular);
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "tray_calculate")
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    dgvProduct.Columns[e.ColumnIndex].DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);
                    dgvProduct.Rows[e.RowIndex].Cells["weight_calculate"].Value = !isChecked;
                    calculate();
                    calculateAssort();

                    if (isChecked)
                        dgvProduct.Columns[e.ColumnIndex].DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);
                    else
                        dgvProduct.Columns[e.ColumnIndex].DefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Regular);
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "is_CFR")
                {
                    bool is_CFR;
                    if (dgvProduct.Rows[e.RowIndex].Cells["is_CFR"].Value == null || !bool.TryParse(dgvProduct.Rows[e.RowIndex].Cells["is_CFR"].Value.ToString(), out is_CFR))
                        is_CFR = false;

                    dgvProduct.Rows[e.RowIndex].Cells["is_FOB"].Value = !is_CFR;
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "is_FOB")
                {
                    bool is_FOB;
                    if (dgvProduct.Rows[e.RowIndex].Cells["is_FOB"].Value == null || !bool.TryParse(dgvProduct.Rows[e.RowIndex].Cells["is_FOB"].Value.ToString(), out is_FOB))
                        is_FOB = false;

                    dgvProduct.Rows[e.RowIndex].Cells["is_CFR"].Value = !is_FOB;
                }
            }
        }

        private void dgvProduct_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "unit_price")
                {
                    DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                    double price;
                    if (e.ColumnIndex >= 5 && row.Cells[e.ColumnIndex].Value != null && double.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out price))
                    {
                        if (price > 0)
                        {
                            double custom;
                            if (row.Cells["custom"].Value == null || !double.TryParse(row.Cells["custom"].Value.ToString(), out custom))
                                custom = 0;
                            custom = custom / 100;
                            double tax;
                            if (row.Cells["tax"].Value == null || !double.TryParse(row.Cells["tax"].Value.ToString(), out tax))
                                tax = 0;
                            tax = tax / 100;
                            double incidental_expense;
                            if (row.Cells["incidental_expense"].Value == null || !double.TryParse(row.Cells["incidental_expense"].Value.ToString(), out incidental_expense))
                                incidental_expense = 0;
                            incidental_expense = incidental_expense / 100;
                            double cost_unit;
                            if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                                cost_unit = 0;
                            double unit;
                            if (row.Cells["box_weight"].Value == null || !double.TryParse(row.Cells["box_weight"].Value.ToString(), out unit))
                                unit = 0;
                            bool weight_calculate = Convert.ToBoolean(row.Cells["weight_calculate"].Value);
                            //트레이 단가
                            if (!weight_calculate)
                                unit = cost_unit;
                            //선택 오퍼가 순위
                            DateTime enddate = DateTime.Now;
                            DateTime sttdate = enddate.AddYears(-2);

                            DataTable rankDt = purchasePriceRepository.GetRankingPurchasePriceASOne(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                                , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["box_weight"].Value.ToString()
                                , "", price);
                            //매입내역
                            DataTable purchaseDt = purchaseRepository.GetPurchaseProduct(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                                , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["box_weight"].Value.ToString(), "");
                            if (purchaseDt.Rows.Count > 0)
                            {
                                //환율내역
                                DataTable exchangeRateDt = purchaseRepository.GetExchangeRate(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));
                                for (int i = 0; i < purchaseDt.Rows.Count; i++)
                                {
                                    DataRow rankRow = rankDt.NewRow();
                                    rankRow["updatetime"] = purchaseDt.Rows[i]["매입일자"].ToString() + "01";
                                    //KRW 매입단가
                                    double price_krw = Convert.ToDouble(purchaseDt.Rows[i]["단가2"].ToString());
                                    //환율
                                    DataRow[] dr = null;
                                    double now_exchange_rate = common.GetExchangeRateKEBBank("USD");
                                    double exchange_rate = 0;
                                    if (exchangeRateDt.Rows.Count > 0)
                                    {
                                        string whr = "base_date = '" + purchaseDt.Rows[i]["매입일자"].ToString() + "'";
                                        dr = exchangeRateDt.Select(whr);
                                        if (dr.Length > 0)
                                            exchange_rate = Convert.ToDouble(dr[0]["exchange_rate"].ToString());
                                        else if (purchaseDt.Rows[i]["매입일자"].ToString() == DateTime.Now.ToString("yyyyMM"))
                                            exchange_rate = now_exchange_rate;
                                    }
                                    //USD 매입단가
                                    double offer_price = price_krw / unit / (1 + custom + tax + incidental_expense) / exchange_rate;
                                    rankRow["purchase_price"] = offer_price;
                                    rankRow["division"] = -1;

                                    rankDt.Rows.Add(rankRow);
                                }
                                rankDt.AcceptChanges();
                            }
                            //단가로 정렬시키기
                            DataView dv = new DataView(rankDt);
                            dv.Sort = "purchase_price";
                            rankDt = dv.ToTable();
                            rankDt.AcceptChanges();
                            //순위매기기
                            string rank = "";
                            if (rankDt.Rows.Count > 0)
                            {
                                //순위
                                for (int i = 0; i < rankDt.Rows.Count; i++)
                                {
                                    if (rankDt.Rows[i]["division"].ToString() == "1")
                                    {
                                        double rate = ((((double)i + 1) / rankDt.Rows.Count) * 100);
                                        rank = "상위 " + rate.ToString("#,##0.00") + "%";
                                        break;
                                    }
                                }
                                //최대, 최소값
                                double MaxPrice = 0, MinPrice = 0;
                                if (!double.TryParse(rankDt.Rows[rankDt.Rows.Count - 1]["purchase_price"].ToString(), out MaxPrice))
                                    MaxPrice = 0;
                                if (!double.TryParse(rankDt.Rows[0]["purchase_price"].ToString(), out MinPrice))
                                    MinPrice = 0;

                                rank += "\n최소단가 : " + MinPrice.ToString("#,##0.00") + " ~ 최대단가 : " + MaxPrice.ToString("#,##0.00");
                            }
                            else
                            {
                                rank = "등록된 내역이 없습니다.";
                            }
                            row.Cells[e.ColumnIndex].ToolTipText = rank;
                        }
                    }
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "cost_price")
                {
                    DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                    double price;
                    if (e.ColumnIndex >= 5 && row.Cells[e.ColumnIndex].Value != null && double.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out price))
                    {
                        if (price > 0)
                        {
                            double custom;
                            if (row.Cells["custom"].Value == null || !double.TryParse(row.Cells["custom"].Value.ToString(), out custom))
                                custom = 0;
                            custom = custom / 100;
                            double tax;
                            if (row.Cells["tax"].Value == null || !double.TryParse(row.Cells["tax"].Value.ToString(), out tax))
                                tax = 0;
                            tax = tax / 100;
                            double incidental_expense;
                            if (row.Cells["incidental_expense"].Value == null || !double.TryParse(row.Cells["incidental_expense"].Value.ToString(), out incidental_expense))
                                incidental_expense = 0;
                            incidental_expense = incidental_expense / 100;
                            double cost_unit;
                            if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                                cost_unit = 0;
                            double unit;
                            if (row.Cells["box_weight"].Value == null || !double.TryParse(row.Cells["box_weight"].Value.ToString(), out unit))
                                unit = 0;
                            bool weight_calculate = Convert.ToBoolean(row.Cells["weight_calculate"].Value);
                            //트레이 단가
                            if (!weight_calculate)
                                unit = cost_unit;


                            //선택 오퍼가 순위
                            DateTime enddate = DateTime.Now;
                            DateTime sttdate = enddate.AddYears(-2);

                            DataTable rankDt = purchasePriceRepository.GetRankingPurchasePriceASOne(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                                , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["box_weight"].Value.ToString()
                                , "", price);
                            double now_exchange_rate = common.GetExchangeRateKEBBank("USD");
                            double exchange_rate = now_exchange_rate;
                            if (rankDt.Rows.Count > 0)
                            {
                                for (int i = 0; i < rankDt.Rows.Count; i++)
                                {
                                    double purchase_price_krw;
                                    if (rankDt.Rows[i]["division"].ToString() == "1")
                                        purchase_price_krw = Convert.ToDouble(rankDt.Rows[i]["purchase_price"].ToString());
                                    else
                                        purchase_price_krw = Convert.ToDouble(rankDt.Rows[i]["purchase_price"].ToString()) * (1 + custom + tax + incidental_expense) * exchange_rate * unit;
                                    rankDt.Rows[i]["purchase_price"] = purchase_price_krw;
                                }
                                rankDt.AcceptChanges();
                            }

                            //매입내역
                            DataTable purchaseDt = purchaseRepository.GetPurchaseProduct(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                            , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["box_weight"].Value.ToString(), "");
                            if (purchaseDt.Rows.Count > 0)
                            {
                                for (int i = 0; i < purchaseDt.Rows.Count; i++)
                                {
                                    DataRow rankRow = rankDt.NewRow();
                                    rankRow["updatetime"] = purchaseDt.Rows[i]["매입일자"].ToString() + "01";
                                    //KRW 매입단가
                                    double price_krw = Convert.ToDouble(purchaseDt.Rows[i]["단가2"].ToString());

                                    
                                    rankRow["purchase_price"] = price_krw;
                                    rankRow["division"] = -1;

                                    rankDt.Rows.Add(rankRow);
                                }
                                rankDt.AcceptChanges();
                            }
                            //단가로 정렬시키기
                            DataView dv = new DataView(rankDt);
                            dv.Sort = "purchase_price";
                            rankDt = dv.ToTable();
                            rankDt.AcceptChanges();
                            //순위매기기
                            string rank = "";
                            if (rankDt.Rows.Count > 0)
                            {
                                //순위
                                for (int i = 0; i < rankDt.Rows.Count; i++)
                                {
                                    if (rankDt.Rows[i]["division"].ToString() == "1")
                                    {
                                        double rate = ((((double)i + 1) / rankDt.Rows.Count) * 100);
                                        rank = "상위 " + rate.ToString("#,##0.00") + "%";
                                        break;
                                    }
                                }
                                //최대, 최소값
                                double MaxPrice = 0, MinPrice = 0;
                                if (!double.TryParse(rankDt.Rows[rankDt.Rows.Count - 1]["purchase_price"].ToString(), out MaxPrice))
                                    MaxPrice = 0;
                                if (!double.TryParse(rankDt.Rows[0]["purchase_price"].ToString(), out MinPrice))
                                    MinPrice = 0;

                                rank += "\n최소단가 : " + MinPrice.ToString("#,##0") + " ~ 최대단가 : " + MaxPrice.ToString("#,##0");
                            }
                            else
                            {
                                rank = "등록된 내역이 없습니다.";
                            }
                            row.Cells[e.ColumnIndex].ToolTipText = rank;
                        }
                    }
                }
            }
        }
        private void dgvProduct_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "purchase_price1")
                {
                    DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                    DataTable copDt = seaoverRepository.GetProductPriceInfo(row.Cells["product"].Value.ToString()
                                                                            , row.Cells["origin"].Value.ToString()
                                                                            , row.Cells["sizes"].Value.ToString()
                                                                            , "");
                    if (copDt.Rows.Count > 0)
                    {

                        double cost_unit;
                        if (row.Cells["origin_cost_unit"].Value == null || !double.TryParse(row.Cells["origin_cost_unit"].Value.ToString(), out cost_unit))
                            cost_unit = 1;

                        bool isWeight;
                        if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out isWeight))
                            isWeight = false;

                        double unit;
                        if (row.Cells["box_weight"].Value == null || !double.TryParse(row.Cells["box_weight"].Value.ToString(), out unit))
                            unit = 1;                      

                        //단위 변환
                        copDt.Columns["purchase_price"].ReadOnly = false;
                        for (int i = 0; i < copDt.Rows.Count; i++)
                        {
                            /*if(isWeight)
                                copDt.Rows[i]["purchase_price"] = Convert.ToDouble(copDt.Rows[i]["purchase_price"].ToString()) * (unit / Convert.ToDouble(copDt.Rows[i]["SEAOVER단위"].ToString()));
                            else
                                copDt.Rows[i]["purchase_price"] = Convert.ToDouble(copDt.Rows[i]["purchase_price"].ToString()) * (cost_unit / Convert.ToDouble(copDt.Rows[i]["단위수량"].ToString()));*/
                            copDt.Rows[i]["purchase_price"] = Convert.ToDouble(copDt.Rows[i]["purchase_price"].ToString()) * (unit / Convert.ToDouble(copDt.Rows[i]["SEAOVER단위"].ToString()));
                        }


                        string whr = "1 = 1";
                        DataRow[] copRow = copDt.Select(whr);
                        if (copRow.Length > 0)
                        {
                            //단가리스트 생성
                            OfferPriceList ofl = new OfferPriceList();
                            Point p = dgvProduct.PointToScreen(dgvProduct.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Location);
                            p = new Point(p.X - ofl.Size.Width - dgvProduct.ColumnHeadersHeight, p.Y - ofl.Size.Height + dgvProduct.Rows[e.RowIndex].Height);

                            ofl.StartPosition = FormStartPosition.Manual;
                            string[] price = ofl.CostAccountingManager(copRow, p);

                            if (price != null)
                            {
                                dgvProduct.Rows[e.RowIndex].Cells["purchase_price1"].Value = Convert.ToDouble(price[1]).ToString("#,##0.00");
                            }
                        }
                    }
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "per_box_purchase_price")
                {
                    DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                    DataTable copDt = seaoverRepository.GetProductPriceInfo(row.Cells["product"].Value.ToString()
                                                                            , row.Cells["origin"].Value.ToString()
                                                                            , row.Cells["sizes"].Value.ToString()
                                                                            , "");

                    if (copDt.Rows.Count > 0)
                    {
                        string whr = "1 = 1";
                        DataRow[] copRow = copDt.Select(whr);
                        if (copRow.Length > 0)
                        {
                            double cost_unit;
                            if (row.Cells["origin_cost_unit"].Value == null || !double.TryParse(row.Cells["origin_cost_unit"].Value.ToString(), out cost_unit))
                                cost_unit = 1;

                            bool isWeight;
                            if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out isWeight))
                                isWeight = false;

                            double unit;
                            if (row.Cells["box_weight"].Value == null || !double.TryParse(row.Cells["box_weight"].Value.ToString(), out unit))
                                unit = 1;

                            copDt.Columns["purchase_price"].ReadOnly = false;
                            for (int i = 0; i < copDt.Rows.Count; i++)
                            {
                                double purchase_price;
                                if (!double.TryParse(copDt.Rows[i]["purchase_price"].ToString(), out purchase_price))
                                    purchase_price = 0;
                                if(isWeight)
                                    copDt.Rows[i]["purchase_price"] = purchase_price / unit;
                                else
                                    copDt.Rows[i]["purchase_price"] = purchase_price / cost_unit;
                            }
                            copDt.AcceptChanges();


                            //단가리스트 생성
                            OfferPriceList ofl = new OfferPriceList();
                            Point p = dgvProduct.PointToScreen(dgvProduct.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Location);
                            p = new Point(p.X - ofl.Size.Width - dgvProduct.ColumnHeadersHeight, p.Y - ofl.Size.Height + dgvProduct.Rows[e.RowIndex].Height);

                            ofl.StartPosition = FormStartPosition.Manual;
                            string[] price = ofl.CostAccountingManager(copRow, p);

                            if (price != null)
                            {
                                dgvProduct.Rows[e.RowIndex].Cells["per_box_purchase_price"].Value = Convert.ToDouble(price[1]).ToString("#,##0.00");
                                if (row.Cells["box_weight"].Value == null || !double.TryParse(row.Cells["box_weight"].Value.ToString(), out unit))
                                    unit = 1;
                                //dgvProduct.Rows[e.RowIndex].Cells["purchase_price1"].Value = (Convert.ToDouble(price[1]) * unit).ToString("#,##0.00");
                            }
                        }
                    }
                }
                else
                {
                    DetailDashboard dd = null;
                    FormCollection fc = Application.OpenForms;
                    foreach (Form frm in fc)
                    {
                        //iterate through
                        if (frm.Name == "DetailDashboard")
                        {
                            dd = (DetailDashboard)frm;
                            break;
                        }
                    }

                    List<DataGridViewRow> pList = new List<DataGridViewRow>();
                    DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                    pList.Add(row);
                            
                    //있을 경우만
                    if (dd != null)
                    {
                        clipboardList = pList;
                        dd.inputProduct();
                    }
                }
            }
        }

        private void dgvProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.Button == MouseButtons.Right)
            {
                if (dgvProduct.SelectionMode != DataGridViewSelectionMode.FullRowSelect)
                {
                    dgvProduct.ClearSelection();
                    dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgvProduct.Rows[e.RowIndex].Selected = true;
                }
            }
        }
        
        #endregion

        #region 우클릭 메뉴, 매서드

        //우클릭 메뉴 Create
        private void dgvProduct_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvProduct.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    ContextMenuStrip m = new ContextMenuStrip();
                    //공통
                    if (dgvProduct.SelectionMode == DataGridViewSelectionMode.CellSelect && dgvProduct.SelectedCells.Count > 0)
                    {
                        m.Items.Add("역대단가 확인");
                    }
                    else if (dgvProduct.SelectionMode == DataGridViewSelectionMode.FullColumnSelect && dgvProduct.SelectedColumns.Count > 0)
                    {
                        m.Items.Add("열 숨기기");
                        if (dgvProduct.SelectedColumns.Count > 1)
                            m.Items.Add("열 펼치기");
                    }
                    else if (dgvProduct.SelectionMode == DataGridViewSelectionMode.FullRowSelect&& dgvProduct.SelectedRows.Count > 0)
                    {
                        m.Items.Add("수정(S)");
                        if (isBookmarkMode)
                            m.Items.Add("즐겨찾기 해제");
                        else
                            m.Items.Add("즐겨찾기 등록");

                        m.Items.Add("거래처별 비교");
                        m.Items.Add("상세 대시보드(D)");
                        m.Items.Add("상세 대시보드(영업)");
                        m.Items.Add("복사해서 추가하기");

                    }
                    
                    
                    if (m.Items.Count > 0)
                    {
                        isContextstripmenu = true;
                        //Event Method
                        m.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.cms_Closed);
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                        m.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cms_PreviewKeyDown);
                        //Create 
                        m.BackColor = Color.White;
                        m.Show(dgvProduct, e.Location);
                        
                    }
                    //Selection
                    /*PendingList.ClearSelection();
                    DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                    selectRow.Selected = !selectRow.Selected;*/
                }
            }
            catch {}
        }
        private void cms_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            isContextstripmenu = false;
        }
        private void cms_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                //우클릭 단축키
                case Keys.D:
                    if (dgvProduct.SelectedRows.Count > 0)
                    {

                        //선택한 DgvRow List생성
                        List<DataGridViewRow> pList = new List<DataGridViewRow>();
                        for (int i = 0; i < dgvProduct.Rows.Count; i++)
                        {
                            DataGridViewRow row = dgvProduct.Rows[i];
                            bool isCheck = Convert.ToBoolean(row.Selected);
                            if (isCheck)
                                pList.Add(row);
                        }

                        //대시보드로 데이터 넘기기
                        DetailDashboard dd = null;
                        if (dgvProduct.SelectedRows.Count > 0)
                        {
                            FormCollection fc = Application.OpenForms;
                            foreach (Form frm in fc)
                            {
                                //iterate through
                                if (frm.Name == "DetailDashboard")
                                {
                                    dd = (DetailDashboard)frm;
                                    dd.InputCostAccountingData(pList);

                                    dd.inputProduct();
                                    dd.BringToFront();
                                    clipboardList = pList;
                                    return;
                                }
                            }
                        }

                        if (dd == null)
                        {
                            dd = new DetailDashboard(um);
                            dd.InputCostAccountingData(pList);
                            dd.inputProduct();
                            dd.Show();
                        }

                        clipboardList = pList;
                    }
                    break;
                case Keys.S:
                    if (dgvProduct.SelectedRows.Count > 0)
                    {
                        DataGridViewRow row = dgvProduct.SelectedRows[0];
                        PurchaseUnitManager pum = new PurchaseUnitManager(um, row.Cells["updatetime"].Value.ToString(), row.Cells["updatetime"].Value.ToString()
                            , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["box_weight"].Value.ToString());
                        pum.Show();
                    }
                    break;
            }
        }

        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (dgvProduct.SelectedCells.Count == 0)
            {
                return;
            }
            try
            {
                if (dgvProduct.SelectedCells.Count < 0)
                {
                    return;
                }

                
                
                /*PendingInfo p;*/
                switch (e.ClickedItem.Text)
                {
                    case "수정(S)":
                        {
                            DataGridViewRow row = dgvProduct.SelectedRows[0];
                            PurchaseUnitManager pum = new PurchaseUnitManager(um, row.Cells["updatetime"].Value.ToString(), row.Cells["updatetime"].Value.ToString()
                                , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["box_weight"].Value.ToString());
                            pum.Show();
                        }
                        break;
                    case "상세 대시보드(D)":
                        {
                            if (dgvProduct.SelectedRows.Count > 0)
                            {
                                //선택한 DgvRow List생성
                                List<DataGridViewRow> pList = new List<DataGridViewRow>();
                                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                                {
                                    DataGridViewRow row = dgvProduct.Rows[i];
                                    bool isCheck = Convert.ToBoolean(row.Selected);
                                    if (isCheck)
                                        pList.Add(row);
                                }

                                //대시보드로 데이터 넘기기
                                DetailDashboard dd = null;
                                if (dgvProduct.SelectedRows.Count > 0)
                                {
                                    FormCollection fc = Application.OpenForms;
                                    foreach (Form frm in fc)
                                    {
                                        //iterate through
                                        if (frm.Name == "DetailDashboard")
                                        {
                                            dd = (DetailDashboard)frm;
                                            dd.InputCostAccountingData(pList);
                                            
                                            dd.inputProduct();
                                            dd.BringToFront();
                                            clipboardList = pList;
                                            return;
                                        }
                                    }
                                }

                                if (dd == null)
                                {
                                    dd = new DetailDashboard(um);
                                    dd.InputCostAccountingData(pList);
                                    dd.inputProduct();
                                    dd.Show();
                                }

                                clipboardList = pList;
                            }
                        }
                        break;
                    case "열 숨기기":
                        foreach (DataGridViewColumn col in dgvProduct.SelectedColumns)
                        {
                            col.Visible = false;
                        }
                        break;
                    case "열 펼치기":
                        int max_idx = dgvProduct.SelectedColumns[dgvProduct.SelectedColumns.Count - 1].Index;
                        int min_idx = dgvProduct.SelectedColumns[dgvProduct.SelectedColumns.Count - 1].Index;
                        foreach (DataGridViewColumn col in dgvProduct.SelectedColumns)
                        {
                            if(col.Index < min_idx)
                                min_idx = col.Index;
                            if (col.Index > max_idx)
                                max_idx = col.Index;
                        }

                        for (int i = min_idx; i <= max_idx; i++)
                        {
                            if (!(dgvProduct.Columns[i].Name == "stock"
                                || dgvProduct.Columns[i].Name == "sizes2"
                                || dgvProduct.Columns[i].Name == "sales_count"
                                || dgvProduct.Columns[i].Name == "month_around"
                                || dgvProduct.Columns[i].Name == "assort"
                                || dgvProduct.Columns[i].Name == "assort_weight"
                                || dgvProduct.Columns[i].Name == "weight_rate"
                                || dgvProduct.Columns[i].Name == "assort_month_around"
                                || dgvProduct.Columns[i].Name == "assort_margin1"
                                || dgvProduct.Columns[i].Name == "assort_margin1"
                                || dgvProduct.Columns[i].Name == "sales_unit"
                                || dgvProduct.Columns[i].Name == "purchase_unit"
                                || dgvProduct.Columns[i].Name == "purchase_price"
                                || dgvProduct.Columns[i].Name == "tray_calculate"
                                || dgvProduct.Columns[i].Name == "domestic_sales_price"))
                                dgvProduct.Columns[i].Visible = true;
                        }
                        break;
                    case "역대단가 확인":
                        List<string[]> productList = new List<string[]>();
                        foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                        {
                            if (dgvProduct.Columns[cell.ColumnIndex].Name == "unit_price")
                            {
                                string[] product = new string[6];
                                product[0] = dgvProduct.Rows[cell.RowIndex].Cells["product"].Value.ToString();
                                product[1] = dgvProduct.Rows[cell.RowIndex].Cells["origin"].Value.ToString();
                                product[2] = dgvProduct.Rows[cell.RowIndex].Cells["sizes"].Value.ToString();
                                product[3] = dgvProduct.Rows[cell.RowIndex].Cells["box_weight"].Value.ToString();

                                double price;
                                if (dgvProduct.Rows[cell.RowIndex].Cells["unit_price"].Value == null || !double.TryParse(dgvProduct.Rows[cell.RowIndex].Cells["unit_price"].Value.ToString(), out price))
                                {
                                    price = 0;
                                }
                                product[4] = price.ToString("#,##0.00");

                                productList.Add(product);
                            }
                        }
                        if (productList.Count > 0)
                        {
                            PurchaseManager.GraphManager gm = new GraphManager(um, productList);
                            gm.Show();
                        }
                        break;
                    case "즐겨찾기 등록":
                        if (dgvProduct.SelectedRows.Count > 0)
                        {
                            List<GroupModel> list = new List<GroupModel>();
                            for (int i = 0; i < dgvProduct.Rows.Count; i++)
                            {
                                DataGridViewRow dgvRow = dgvProduct.Rows[i];
                                bool isChecked = Convert.ToBoolean(dgvRow.Selected);
                                if (isChecked)
                                {
                                    GroupModel gModel = new GroupModel();
                                    gModel.product = dgvRow.Cells["product"].Value.ToString();
                                    gModel.origin = dgvRow.Cells["origin"].Value.ToString();
                                    gModel.sizes = dgvRow.Cells["sizes"].Value.ToString();
                                    gModel.unit = dgvRow.Cells["box_weight"].Value.ToString();
                                    gModel.cost_unit = dgvRow.Cells["cost_unit"].Value.ToString();
                                    double month_around = Convert.ToDouble(dgvRow.Cells["month_around"].Value.ToString());
                                    if (double.IsNaN(month_around))
                                        month_around = 0;
                                    else if (double.IsInfinity(month_around))
                                        month_around = 9999;
                                    gModel.month_around = Convert.ToDouble(dgvRow.Cells["month_around"].Value.ToString());

                                    gModel.offer_price = Convert.ToDouble(dgvRow.Cells["unit_price"].Value.ToString());
                                    gModel.offer_cost_price = Convert.ToDouble(dgvRow.Cells["cost_price"].Value.ToString());
                                    gModel.offer_company = dgvRow.Cells["company"].Value.ToString();
                                    gModel.offer_updatetime = dgvRow.Cells["updatetime"].Value.ToString();

                                    list.Add(gModel);
                                }
                            }

                            if (list.Count > 0)
                            {
                                SEAOVER.BookmarkManager bm = new BookmarkManager(um, this, list, 1);
                                bm.ShowDialog();
                            }
                        }
                        break;
                    case "즐겨찾기 해제":

                        int group_id;
                        if (!int.TryParse(lbGroupId.Text, out group_id))
                        {
                            MessageBox.Show(this, "즐겨찾기 정보를 찾을수 없습니다.");
                            this.Activate();
                            return;
                        }

                        if (dgvProduct.SelectedRows.Count > 0)
                        {
                            List<StringBuilder> sqlList = new List<StringBuilder>();
                            List<GroupModel> list = new List<GroupModel>();
                            for (int i = 0; i < dgvProduct.Rows.Count; i++)
                            {
                                DataGridViewRow dgvRow = dgvProduct.Rows[i];
                                bool isChecked = Convert.ToBoolean(dgvRow.Selected);
                                if (isChecked)
                                {
                                    GroupModel gModel = new GroupModel();
                                    gModel.id = group_id;
                                    gModel.product = dgvRow.Cells["product"].Value.ToString();
                                    gModel.origin = dgvRow.Cells["origin"].Value.ToString();
                                    gModel.sizes = dgvRow.Cells["sizes"].Value.ToString();
                                    gModel.unit = dgvRow.Cells["box_weight"].Value.ToString();
                                    gModel.cost_unit = dgvRow.Cells["cost_unit"].Value.ToString();
                                    double month_around = Convert.ToDouble(dgvRow.Cells["month_around"].Value.ToString());
                                    if (double.IsNaN(month_around))
                                        month_around = 0;
                                    else if (double.IsInfinity(month_around))
                                        month_around = 9999;
                                    gModel.month_around = Convert.ToDouble(dgvRow.Cells["month_around"].Value.ToString());

                                    gModel.offer_price = Convert.ToDouble(dgvRow.Cells["unit_price"].Value.ToString());
                                    gModel.offer_cost_price = Convert.ToDouble(dgvRow.Cells["cost_price"].Value.ToString());
                                    gModel.offer_company = dgvRow.Cells["company"].Value.ToString();
                                    gModel.offer_updatetime = dgvRow.Cells["updatetime"].Value.ToString();

                                    list.Add(gModel);

                                    StringBuilder sql = groupRepository.DeleteGroup2(gModel);
                                    sqlList.Add(sql);
                                }
                            }

                            if (commonRepository.UpdateTran(sqlList) == -1)
                            {
                                MessageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                            }
                            else
                            {
                                for (int i = dgvProduct.Rows.Count - 1; i >= 0; i--)
                                {
                                    DataGridViewRow dgvRow = dgvProduct.Rows[i];
                                    bool isChecked = Convert.ToBoolean(dgvRow.Selected);
                                    if (isChecked)
                                    {
                                        dgvProduct.Rows.Remove(dgvProduct.Rows[i]);
                                    }
                                }
                            }
                            
                        }
                        break;
                    case "거래처별 비교":
                        SetProductPerCompany();
                        break;
                    case "상세 대시보드(영업)":
                        if (dgvProduct.SelectedRows.Count > 0)
                        {
                            List<string[]> productInfoList = new List<string[]>();
                            for (int i = 0; i < dgvProduct.Rows.Count; i++)
                            {
                                DataGridViewRow dgvRow = dgvProduct.Rows[i];
                                bool isChecked = Convert.ToBoolean(dgvRow.Selected);
                                if (isChecked)
                                {
                                    


                                    string[] productInfo = new string[30];

                                    productInfo[0] = dgvRow.Cells["product"].Value.ToString();
                                    productInfo[1] = dgvRow.Cells["origin"].Value.ToString();
                                    productInfo[2] = dgvRow.Cells["sizes"].Value.ToString();
                                    productInfo[3] = dgvRow.Cells["box_weight"].Value.ToString();

                                    DataTable seaoverDt = seaoverRepository.GetProductInfo("2000-01-01", "2050-01-01", ""
                                                            , productInfo[0], productInfo[1], productInfo[2], productInfo[3]
                                                            , "", "", true);
                                    if (seaoverDt.Rows.Count > 0)
                                    {
                                        productInfo[5] = seaoverDt.Rows[0]["매입단가"].ToString();
                                        productInfo[15] = seaoverDt.Rows[0]["품목코드"].ToString();
                                        productInfo[16] = seaoverDt.Rows[0]["원산지코드"].ToString();
                                        productInfo[17] = seaoverDt.Rows[0]["규격코드"].ToString();
                                        productInfo[20] = seaoverDt.Rows[0]["가격단위"].ToString();
                                        productInfo[23] = seaoverDt.Rows[0]["담당자1"].ToString();
                                        productInfo[24] = seaoverDt.Rows[0]["담당자2"].ToString();
                                        productInfo[25] = seaoverDt.Rows[0]["매출단가"].ToString();
                                        productInfo[26] = seaoverDt.Rows[0]["대분류"].ToString();
                                        productInfo[29] = seaoverDt.Rows[0]["묶음수"].ToString();
                                    }

                                    DataTable otherCostDt = productOtherCostRepository.GetProductInfoAsOneExactly(productInfo[0], productInfo[1], productInfo[2], productInfo[3]);
                                    if (otherCostDt.Rows.Count > 0)
                                    {
                                        productInfo[9] = otherCostDt.Rows[0]["purchase_margin"].ToString();
                                        productInfo[10] = otherCostDt.Rows[0]["production_days"].ToString();    
                                        productInfo[14] = otherCostDt.Rows[0]["manager"].ToString();
                                        productInfo[27] = otherCostDt.Rows[0]["delivery_days"].ToString();
                                        productInfo[28] = "0";
                                        productInfo[21] = otherCostDt.Rows[0]["cost_unit"].ToString();
                                    }

                                    productInfo[6] = dgvRow.Cells["custom"].Value.ToString();
                                    productInfo[7] = dgvRow.Cells["tax"].Value.ToString();
                                    productInfo[8] = dgvRow.Cells["incidental_expense"].Value.ToString();
                                    

                                    productInfo[11] = dgvRow.Cells["unit_price"].Value.ToString();
                                    productInfo[12] = dgvRow.Cells["company"].Value.ToString();
                                    productInfo[13] = dgvRow.Cells["cost_price"].Value.ToString();
                                    
                                    productInfo[18] = dgvRow.Cells["updatetime"].Value.ToString();
                                    productInfo[19] = dgvRow.Cells["weight_calculate"].Value.ToString();

                                    
                                    productInfo[22] = dgvRow.Cells["box_weight"].Value.ToString();
                                    

                                    productInfoList.Add(productInfo);
                                }
                            }
                            int saleTerm;
                            switch (cbSaleTerms.Text)
                            {
                                case "1개월":
                                    saleTerm = 1;
                                    break;
                                case "45일":
                                    saleTerm = 45;
                                    break;
                                case "2개월":
                                    saleTerm = 2;
                                    break;
                                case "3개월":
                                    saleTerm = 3;
                                    break;
                                case "6개월":
                                    saleTerm = 6;
                                    break;
                                case "12개월":
                                    saleTerm = 12;
                                    break;
                                case "18개월":
                                    saleTerm = 18;
                                    break;
                                default:
                                    saleTerm = 6;
                                    break;
                            }

                            DetailDashBoardForSales dd = null;
                            FormCollection fc = Application.OpenForms;
                            foreach (Form frm in fc)
                            {
                                //iterate through
                                if (frm.Name == "DetailDashBoardForSales")
                                {
                                    if (MessageBox.Show(this, "새창으로 대시보드를 여시겠습니까?" + "\n Yes : 새창       No : 기존창", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                        dd = null;
                                    else
                                    {

                                        dd = (DetailDashBoardForSales)frm;
                                        dd.InputClipboardData(productInfoList);
                                        dd.Activate();
                                    }
                                }
                            }
                            //새로열기
                            if (dd == null)
                            {
                                try
                                {
                                    dd = new DetailDashBoardForSales(um, productInfoList, saleTerm);
                                    dd.Show();
                                }
                                catch
                                { }
                            }
                        }
                        break;
                    case "복사해서 추가하기":
                        DataGridViewRow selectedRow = dgvProduct.SelectedRows[0];

                        // 새로운 행을 생성하고, 선택된 행의 값을 복사합니다.
                        DataGridViewRow newRow = (DataGridViewRow)selectedRow.Clone();
                        for (int i = 0; i < selectedRow.Cells.Count; i++)
                        {
                            newRow.Cells[i].Value = selectedRow.Cells[i].Value;
                        }

                        // 새로운 행을 DataGridView에 추가합니다.
                        dgvProduct.Rows.Add(newRow);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message);
                this.Activate();
            }

        }

        public void SetGroup(int id, string group_name)
        {
            isBookmarkMode = true;
            txtGroupName.Text = group_name;
            txtGroupName.Enabled = true;
            group_id = id;
            lbGroupId.Text = group_id.ToString();
            btnBookmark.Text = "즐겨찾기 해제(F8)";
            btnBookmark.ForeColor = Color.Red;
            SetGroupData(id);
        }

        private void SetGroupData(int id)
        {
            dgvProduct.Rows.Clear();
            DataTable groupDt = groupRepository.GetGroup(id);
            List<string[]> productInfoList = new List<string[]>();

            for (int i = 0; i < groupDt.Rows.Count; i++)
            {
                string[] productInfo = new string[16];
                productInfo[0] = groupDt.Rows[i]["product"].ToString();
                productInfo[1] = groupDt.Rows[i]["origin"].ToString();
                productInfo[2] = groupDt.Rows[i]["sizes"].ToString();
                productInfo[3] = groupDt.Rows[i]["unit"].ToString();
                productInfo[4] = groupDt.Rows[i]["cost_unit"].ToString();

                productInfo[5] = groupDt.Rows[i]["tax"].ToString();
                productInfo[6] = groupDt.Rows[i]["custom"].ToString();
                productInfo[7] = groupDt.Rows[i]["incidental_expense"].ToString();

                productInfo[9] = groupDt.Rows[i]["offer_price"].ToString();                
                productInfo[10] = groupDt.Rows[i]["offer_cost_price"].ToString();          
                productInfo[11] = groupDt.Rows[i]["offer_company"].ToString();             
                productInfo[12] = groupDt.Rows[i]["offer_updatetime"].ToString();
                


                bool isWeight = Convert.ToBoolean(groupDt.Rows[i]["weight_calculate"].ToString());
                bool istray = true;
                if (isWeight == istray)
                    isWeight = true;
                    
                istray = !isWeight;

                productInfo[13] = isWeight.ToString();
                productInfo[14] = istray.ToString();
                //productInfo[15] = groupDt.Rows[i]["cid"].ToString();

                productInfoList.Add(productInfo);
            }
            AddProduct3(productInfoList);
        }

        #endregion

        #region Print 
        private void btnPreview_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "원가계산", "is_print"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            if (dgvProduct.Rows.Count == 0)
            {
                MessageBox.Show(this, "출력할 내역이 없습니다.");
                this.Activate();
                return;
            }

            cnt = 0;
            pageNo = 1;
            this.printDocument1 = new PrintDocument();
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument1_PrintPage);
            printDocument1.DefaultPageSettings.Landscape = true;

            int pages = common.GetPageCount(printDocument1);

            Common.PrintManager.PrintManager pm = new Common.PrintManager.PrintManager(this, printDocument1, pages);
            pm.ShowDialog();
        }

        public void InitVariable()
        {
            cnt = 0;
            pageNo = 1;
        }

        //양식만들기
        int cnt = 0;
        int pageNo = 1;
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            DataGridView dgv = this.dgvProduct;

            int dialogWidth = printDocument1.DefaultPageSettings.PaperSize.Height - 40;    //페이지 전체넓이 printPreivew.Width  (가로모드라 반대로)
            int dialogHeight = printDocument1.DefaultPageSettings.PaperSize.Width;    //페이지 전체넓이 printPreivew.Height  (가로모드라 반대로)

            StringFormat sf = new StringFormat();  //컬럼안에 있는 값들 가운데로 정렬하기 위해서.
            sf.Alignment = StringAlignment.Center;
            int width, width1;             //width는 시작점 위치, width1은 datagrid 1개의 컬럼 가로길이
            int startWidth = 10;                   //시작 x좌표
            int startHeight = 140;                 //시작 y좌표
            int avgHeight = dgv.Rows[0].Height - 5;    //컬럼 하나의 높이
            int i, j;                              //반복문용 변수
            int temp = 0;                          //row 개수 세어줄것, cnt의 역활

            //Title, Footer
            e.Graphics.DrawString("원가계산", new Font("Arial", 20, FontStyle.Bold), Brushes.Black, dialogWidth / 2 - 50, 40);
            e.Graphics.DrawString("인쇄일자 :  " + DateTime.Now.ToString("yyyy-MM-dd"), new Font("Arial", 11), Brushes.Black, dialogWidth - 200, 80);
            e.Graphics.DrawString("페이지번호 : " + pageNo, new Font("Arial", 11), Brushes.Black, dialogWidth - 200, 100);

            e.Graphics.DrawString("손익금액 : " + txtTotalAssortMargin1.Text, new Font("Arial", 11), Brushes.Black, 10, 80);
            e.Graphics.DrawString("손익금액(TRQ) : " + txtTotalAssortMargin2.Text, new Font("Arial", 11), Brushes.Black, 10, 100);

            

            //column 전체 넓이
            int column_width = 0;
            int column_count = 0;
            for (i = 0; i < dgv.ColumnCount; i++)
            {
                if (dgv.Columns[i].Visible == true)
                {
                    column_width += dgv.Columns[i].Width;
                    column_count += 1;
                }
            }
            int adjust_font_size = (int)(column_count / 10);
            //columnCount는 일정
            double width_rate;            //컬럼 넓이비율
            double pre_width_rate;        //이전 컬럼 넓이비율
            int pre_idx = 0;
            int tmp = 0;
            for (i = 0; i < dgv.ColumnCount; i++)
            {
                if (dgv.Columns[i].Visible == true)
                {                    
                    if (tmp == 0)
                    {
                        width_rate = ((double)dgv.Columns[i].Width / column_width) * 100;
                        //width1 = dgv.Columns[i].Width + tempWidth;
                        width = 0;
                        width1 = (int)(((double)dialogWidth / 100) * width_rate);
                    }
                    else if (tmp > 0 && i <= dgv.ColumnCount - 2)
                    {
                        width_rate = ((double)dgv.Columns[i].Width / column_width) * 100;
                        pre_width_rate = ((double)dgv.Columns[pre_idx].Width / column_width) * 100;

                        //width = dgv.Columns[i - 1].Width + tempWidth;
                        //width1 = dgv.Columns[i].Width + tempWidth;
                        width = (int)(((double)dialogWidth / 100) * pre_width_rate);
                        width1 = (int)(((double)dialogWidth / 100) * width_rate);
                    }
                    else
                    {
                        width_rate = ((double)dgv.Columns[i].Width / column_width) * 100;
                        pre_width_rate = ((double)dgv.Columns[pre_idx].Width / column_width) * 100;
                        //width = dgv.Columns[i - 1].Width + tempWidth;
                        //width1 = dgv.Columns[i].Width + tempWidth;
                        width = (int)(((double) dialogWidth / 100) * pre_width_rate);
                        width1 = (int)(((double) dialogWidth / 100) * width_rate);
                    }
                    
                    RectangleF drawRect = new RectangleF((float)(startWidth + width), (float)startHeight, (float)width1, avgHeight + 5);
                    e.Graphics.DrawRectangle(Pens.Black, (float)(startWidth + width), (float)startHeight - 4, (float)width1, avgHeight + 5);
                    // e.Graphics.FillRectangle(Brushes.LightGray, (float)(startWidth + width), (float)startHeight, (float)width, avgHeight);
                    e.Graphics.DrawString(dgv.Columns[i].HeaderText, new Font("Arial", 8 - adjust_font_size, FontStyle.Bold), Brushes.Black, drawRect, sf);

                    startWidth += width;
                    tmp += 1;
                    pre_idx = i;
                }
            }

            startHeight += avgHeight + 1;
            for (i = cnt; i < dgv.RowCount; i++)
            {
                tmp = 0;
                pre_idx = 0;
                startWidth = 10;  //다시 초기화
                for (j = 0; j < dgv.ColumnCount; j++)
                {
                    if (dgv.Columns[j].Visible == true && dgv.Rows[i].Cells[j].Visible == true)
                    { 
                        if (tmp == 0)
                        {
                            width_rate = ((double)dgv.Columns[j].Width / column_width) * 100;
                            //width = 0;
                            //width1 = dgv.Columns[j].Width + tempWidth;
                            width = 0;
                            width1 = (int)(((double)dialogWidth / 100) * width_rate);
                            
                        }
                        else if (tmp > 0 && j <= dgv.ColumnCount - 2)
                        {
                            width_rate = ((double)dgv.Columns[j].Width / column_width) * 100;
                            pre_width_rate = ((double)dgv.Columns[pre_idx].Width / column_width) * 100;

                            //width = dgv.Columns[j - 1].Width + tempWidth;
                            //width1 = dgv.Columns[j].Width + tempWidth;
                            width = (int)(((double)dialogWidth / 100) * pre_width_rate);
                            width1 = (int)(((double)dialogWidth / 100) * width_rate);
                        }
                        else
                        {
                            width_rate = ((double)dgv.Columns[j].Width / column_width) * 100;
                            pre_width_rate = ((double)dgv.Columns[pre_idx].Width / column_width) * 100;
                            //width = dgv.Columns[i - 1].Width + tempWidth;
                            //width1 = dgv.Columns[i].Width + tempWidth;
                            width = (int)(((double)dialogWidth / 100) * pre_width_rate);
                            width1 = (int)(((double)dialogWidth / 100) * width_rate);
                        }
                        RectangleF drawRect = new RectangleF((float)(startWidth + width), (float)startHeight + 2, (float)width1, avgHeight);
                        e.Graphics.DrawRectangle(Pens.Black, (float)(startWidth + width), (float)startHeight, (float)width1, avgHeight);
                        e.Graphics.DrawString(dgv.Rows[i].Cells[j].FormattedValue.ToString(), new Font("Arial", 8 - adjust_font_size, FontStyle.Bold), Brushes.Black, drawRect, sf);

                        startWidth += width;
                        tmp += 1;
                        pre_idx = j;
                    }
                }

                startHeight += avgHeight;
                temp++;
                cnt++;

                //한페이지당 35줄
                if (temp % 35 == 0)
                {
                    e.HasMorePages = true;
                    pageNo++;
                    return;
                }
            }
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
                    for (int j = 0; j < dgvProduct.Columns.Count; j++)
                    {
                        if (dgvProduct.Columns[j].Name == col_name[i])
                        {
                            col_idx.Add(j);
                            wk.Cells[1, i + 1].value = dgvProduct.Columns[j].HeaderText;
                            break;
                        }
                    }
                }
                //Data
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    for (int j = 0; j < col_idx.Count; j++)
                    {
                        wk.Cells[i + 2, j + 1] = dgvProduct.Rows[i].Cells[col_idx[j]].Value.ToString();
                    }
                }


                //Border Line Style
                rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[1, col_cnt]];
                rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                rg1.Borders.Weight = Excel.XlBorderWeight.xlThin;
                rg1.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);

                rg1 = wk.Range[wk.Cells[2, 1], wk.Cells[dgvProduct.Rows.Count + 1, col_cnt]];
                rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                //속도개선 ON
                setAutomatic(excelApp, true);
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message.ToString() + "\n 생성 중 에러가 발생하였습니다.");
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

        string isSortDirc = "DESC";
        private void dgvProduct_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;

            //새정렬
            if (isSortDirc == "ASC")
            {
                dgvProduct.Sort(
                    dgvProduct.Columns[e.ColumnIndex],
                    ListSortDirection.Ascending
                );
                isSortDirc = "DESC";
            }
            else
            {
                dgvProduct.Sort(
                    dgvProduct.Columns[e.ColumnIndex],
                    ListSortDirection.Descending
                );
                isSortDirc = "ASC";
            }
        }

        private void txtFreightCharge_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double freight_charge;
                if (!double.TryParse(txtFreightCharge.Text, out freight_charge))
                    freight_charge = 0;

                this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                if (dgvProduct.Rows.Count > 0)
                {
                    double total_weight = 0;
                    for (int i = 0; i < dgvProduct.RowCount; i++)
                    {
                        if (dgvProduct.Rows[i].Cells["is_fob"].Value == null || !bool.TryParse(dgvProduct.Rows[i].Cells["is_fob"].Value.ToString(), out bool is_fob))
                            is_fob = false;

                        if (is_fob)
                        {
                            double assort;
                            if (dgvProduct.Rows[i].Cells["assort"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["assort"].Value.ToString(), out assort))
                                assort = 0;

                            double box_weight;
                            if (dgvProduct.Rows[i].Cells["box_weight"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["box_weight"].Value.ToString(), out box_weight))
                                box_weight = 0;

                            total_weight += assort * box_weight;
                        }
                    }

                    for (int i = 0; i < dgvProduct.RowCount; i++)
                    {
                        if (dgvProduct.Rows[i].Cells["is_fob"].Value == null || !bool.TryParse(dgvProduct.Rows[i].Cells["is_fob"].Value.ToString(), out bool is_fob))
                            is_fob = false;
                        if(is_fob)
                            dgvProduct.Rows[i].Cells["freight_unit_price"].Value = freight_charge / total_weight;
                        else
                            dgvProduct.Rows[i].Cells["freight_unit_price"].Value = 0;
                    }
                }
                calculate();
                calculateAssort();
                this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            }
        }

        private void rbEur_CheckedChanged(object sender, EventArgs e)
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            double exchangeRage;
            if (rbUsd.Checked)
            {
                if (!double.TryParse(txtUsd.Text, out exchangeRage))
                    exchangeRage = 0;
                foreach (DataGridViewRow row in dgvProduct.Rows)
                    row.Cells["exchange_rate"].Value = exchangeRage.ToString("#,##0.00");
                calculate();
            }
            else
            {
                if (!double.TryParse(txtEur.Text, out exchangeRage))
                    exchangeRage = 0;
                foreach (DataGridViewRow row in dgvProduct.Rows)
                    row.Cells["exchange_rate"].Value = exchangeRage.ToString("#,##0.00");
                calculate();
            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }
    }
}


