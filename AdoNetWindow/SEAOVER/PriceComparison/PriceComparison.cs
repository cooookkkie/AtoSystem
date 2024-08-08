using AdoNetWindow.Common;
using AdoNetWindow.DashboardForSales;
using AdoNetWindow.DashboardForSales.MultiDashboard;
using AdoNetWindow.Model;
using AdoNetWindow.Pending;
using AdoNetWindow.PurchaseManager;
using AdoNetWindow.SEAOVER.ProductCostComparison;
using Libs;
using Libs.MultiHeaderGrid;
using Libs.Tools;
using MySqlX.XDevAPI.Relational;
using Repositories;
using Repositories.Config;
using Repositories.Group;
using Repositories.SaleProduct;
using Repositories.SEAOVER;
using Repositories.SEAOVER.Purchase;
using Repositories.SEAOVER.Sales;
using ScottPlot.Drawing.Colormaps;
using ScottPlot.Palettes;
using ScottPlot.SnapLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Application = System.Windows.Forms.Application;
using Excel = Microsoft.Office.Interop.Excel;
using FontStyle = System.Drawing.FontStyle;
using Point = System.Drawing.Point;

namespace AdoNetWindow.SEAOVER.PriceComparison
{
    delegate void myDelegate(int i, int max);
    public partial class PriceComparison : Form
    {
        // Thread Process
        BackgroundWorker thdProcess = null;
        //Process value
        int maxValue;
        int preValue;
        int curValue;
        // Thread Process Not Rate
        bool processingFlag = false;
        private System.Windows.Threading.DispatcherTimer timer;
        //Repository
        IPurchaseRepository purchaseRepository = new PurchaseRepository();
        IProductExcludedSalesRepository productExcludedSalesRepository = new ProductExcludedSalesRepository();
        IGroupRepository groupRepository = new GroupRepository();
        IProductGroupRepository productGroupRepository = new ProductGroupRepository();
        ICommonRepository commonRepository = new CommonRepository();
        IHideRepository hideRepository = new HideRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        IPriceComparisonRepository priceComparisonRepository = new PriceComparisonRepository();
        IConfigRepository configRepository = new ConfigRepository();
        IPurchasePriceRepository purchasePriceRepository = new PurchasePriceRepository();
        ISalesRepository salesRepository = new SalesRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        string isSortDirc = "DESC";
        private Image btnPlus = Properties.Resources.Plus2_btn;
        double exchange_rate;
        Libs.MessageBox msgBox = new Libs.MessageBox();

        int workDays;
        DataTable deliDt = new DataTable();
        DataTable uppDt = null;
        DataTable shpDt = null;
        DataTable copDt = null;
        DataTable hdDt = null;
        DataTable purchaseDt = null;
        DataTable offerDt = null;
        DataTable exchangeRateDt = null;
        DataTable tmDt = null;
        UsersModel um;
        DataTable newDt;

        bool isBookmarkMode = false;
        int group_id;

        LoginCookie cookie;
        Dictionary<string, string> styleDic = new Dictionary<string, string>();

        static Microsoft.Office.Interop.Excel.Application excelApp = null;
        static Microsoft.Office.Interop.Excel.Workbook workBook = null;
        static Microsoft.Office.Interop.Excel.Worksheet workSheet = null;

        public PriceComparison(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
        }

        private void PriceComparison_Load(object sender, EventArgs e)
        {
            //고급검색이 없어진 상태
            pnSetting.Height = 85;

            //Datasource로 사용할 Datatable 컬럼세팅
            SetTable();

            this.KeyPreview = true;
            //Double Buffer
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            Init_DataGridView();

            dtpEnddate.Value = DateTime.Now.AddDays(1);
            dtpSttdate.Value = DateTime.Now.AddMonths(-6);
            txtEnddate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            txtSttdate.Text = DateTime.Now.AddDays(-20).ToString("yyyy-MM-dd");

            txtStockDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            //Datagridview 사용자 설정
            cookie = new LoginCookie(@"C:\Cookies\TEMP\PRICECOMPARISON\" + um.user_id, null, @"SETTING");
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

            //DateTimePick Textbox Lenght Limit
            txtdtpSttdateYear.MaxLength = 4;
            txtdtpSttdateMonth.MaxLength = 2;
            txtdtpSttdateDay.MaxLength = 2;
            txtdtpEnddateYear.MaxLength = 4;
            txtdtpEnddateMonth.MaxLength = 2;
            txtdtpEnddateDay.MaxLength = 2;
            //Loading
            timer_start();
            priceComparisonRepository.SetSeaoverId(um.seaover_id);
            //권한별 설정
            if (um.department.Contains("영업"))
                txtDivision.Text = "10 20 30";
            //Call Procedure
            //품명별판매현황 
            CallSalesProcedure2();
            //업체별시세현황 스토어프로시져 호출
            CallProductProcedure();
            //품명별재고현황 스토어프로시져 호출
            CallStockProcedure();
            //나라별 배송기간
            shpDt = configRepository.GetCountry("");
            //환율
            exchange_rate = common.GetExchangeRateKEBBank("USD");
            txtExchangeRate.Text = exchange_rate.ToString("#,##0.00");
            //Get Trq
            double trq = commonRepository.GetTrq();
            txtTrq.Text = trq.ToString("#,##0");
            //최근 오퍼내역
            copDt = purchasePriceRepository.GetCurrentPurchasePrice("", "", "", "");
            //국가별 배송기간
            deliDt = configRepository.GetCountryDelivery(txtOrigin.Text);
            //무역부
            if (um.department == "무역부")
                cbExhaustedDate.Checked = true;
        }

        #region 로딩 효과 Timer
        private void bgwProcedure_DoWork(object sender, DoWorkEventArgs e)
        {
            CallProcedure();
        }
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

        #region 회의 컨트롤러 Method
        public void SetConference(ConferenceController con)
        {
            if (dgvProduct.Rows.Count > 0)
            {
                //초기화
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    dgvProduct.Rows[i].Visible = true;

                //마진상세
                cbMarginMinPrice.Checked = true;

                //단가비교
                DataTable normalPriceDt = Libs.Tools.Common.DataGridView_To_Datatable(con.dgvNormalPriceAdvance);

                //일반시세 기간
                DateTime sttdate = DateTime.Now, enddate = DateTime.Now;
                if (con.rbNormalTerms15Days.Checked)
                    sttdate = enddate.AddDays(-15);
                else if (con.rbNormalTerms1Months.Checked)
                    sttdate = enddate.AddMonths(-1);
                else if (con.rbNormalTerms45Days.Checked)
                    sttdate = enddate.AddDays(-45);
                else if (con.rbNormalTerms2Months.Checked)
                    sttdate = enddate.AddMonths(-2);
                else if (con.rbNormalTerms15Days.Checked)
                    sttdate = enddate.AddDays(-15);
                else if (con.rbAllTerms.Checked)
                    sttdate = enddate.AddDays(-100);
                //일반시세
                DataTable normalPirceListDt = priceComparisonRepository.GetNormalPriceList(sttdate, enddate, 100, txtProduct.Text, txtOrigin.Text, txtSizes.Text);


                CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dgvProduct.DataSource];
                currencyManager1.SuspendBinding();
                int temp = -1;
                try
                {
                    for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    {
                        temp = i;
                        DataGridViewRow row = dgvProduct.Rows[i];
                        if (row.Cells["main_id"].Value.ToString() == "0" || row.Cells["sub_id"].Value.ToString() == "9999")
                        {
                            bool isOutput = true;

                            //재고검색===========================================================================================================
                            double stock = 0;
                            if (row.Cells["real_stock"].Value == null || !double.TryParse(row.Cells["real_stock"].Value.ToString(), out stock))
                                stock = 0;
                            //전체
                            if (con.rbAllStock.Checked)
                                isOutput = true;
                            //재고있는
                            else if (con.rbInStock.Checked && stock <= 0)
                                isOutput = false;
                            //재고없는
                            else if (con.rbOutOfStock.Checked && stock > 0)
                                isOutput = false;


                            //회전율검색===========================================================================================================
                            if (isOutput)
                            {
                                //선적미포함 회전율
                                if (con.cbRoundStock.Checked)
                                {
                                    //회전율
                                    double month_around = 0;
                                    if (row.Cells["month_around"].Value == null || !double.TryParse(row.Cells["month_around"].Value.ToString(), out month_around))
                                        month_around = 0;
                                    //검색범위
                                    double sttRound, endRound;
                                    if (!double.TryParse(con.nudSttRound.Value.ToString(), out sttRound))
                                        sttRound = 0;
                                    if (!double.TryParse(con.nudEndRound.Value.ToString(), out endRound))
                                        endRound = 0;
                                    //확인
                                    if (!(month_around >= sttRound && month_around <= endRound))
                                        isOutput = false;
                                }
                                //선적포함 회전율
                                else if (con.cbRoundInShipmentStock.Checked)
                                {
                                    //회전율
                                    double month_around = 0;
                                    if (row.Cells["month_around_in_shipment"].Value == null || !double.TryParse(row.Cells["month_around_in_shipment"].Value.ToString(), out month_around))
                                        month_around = 0;
                                    //검색범위
                                    double sttRound, endRound;
                                    if (!double.TryParse(con.nudInShipmentSttRound.Value.ToString(), out sttRound))
                                        sttRound = 0;
                                    if (!double.TryParse(con.nudInShipmentEndRound.Value.ToString(), out endRound))
                                        endRound = 0;
                                    //확인
                                    if (!(month_around >= sttRound && month_around <= endRound))
                                        isOutput = false;
                                }
                                //선적포함(etd반영) 회전율
                                else if (con.cbRoundInShipmentStock2.Checked)
                                {
                                    //회전율
                                    double month_around = 0;
                                    if (row.Cells["month_around_in_shipment2"].Value == null || !double.TryParse(row.Cells["month_around_in_shipment2"].Value.ToString(), out month_around))
                                        month_around = 0;
                                    //검색범위
                                    double sttRound, endRound;
                                    if (!double.TryParse(con.nudInShipmentSttRound2.Value.ToString(), out sttRound))
                                        sttRound = 0;
                                    if (!double.TryParse(con.nudInShipmentEndRound2.Value.ToString(), out endRound))
                                        endRound = 0;
                                    //확인
                                    if (!(month_around >= sttRound && month_around <= endRound))
                                        isOutput = false;
                                }
                            }

                            //단가비교검색===========================================================================================================
                            if (isOutput)
                            {
                                //매출단가
                                double sales_price = 0;
                                if (row.Cells["sales_price"].Value == null || !double.TryParse(row.Cells["sales_price"].Value.ToString(), out sales_price))
                                    sales_price = 0;
                                //최저단가
                                double purchase_price = 0;
                                if (row.Cells["purchase_price"].Value == null || !double.TryParse(row.Cells["purchase_price"].Value.ToString(), out purchase_price))
                                    purchase_price = 0;
                                //일반시세
                                double average_purchase_price = 0;
                                if (row.Cells["average_purchase_price"].Value == null || !double.TryParse(row.Cells["average_purchase_price"].Value.ToString(), out average_purchase_price))
                                    average_purchase_price = 0;
                                //선적원가
                                double shipment_cost_price = 0;
                                if (row.Cells["pending_cost_price"].Value == null || !double.TryParse(row.Cells["pending_cost_price"].Value.ToString(), out shipment_cost_price))
                                    shipment_cost_price = 0;
                                //S원가
                                double sales_cost_price = 0;
                                if (row.Cells["sales_cost_price"].Value == null || !double.TryParse(row.Cells["sales_cost_price"].Value.ToString(), out sales_cost_price))
                                    sales_cost_price = 0;
                                //평균원가1
                                double average_sales_cost_price1 = 0;
                                if (row.Cells["average_sales_cost_price1"].Value == null || !double.TryParse(row.Cells["average_sales_cost_price1"].Value.ToString(), out average_sales_cost_price1))
                                    average_sales_cost_price1 = sales_cost_price;
                                //오퍼원가
                                double offer_cost_price = 0;
                                if (row.Cells["offer_cost_price"].Value == null || !double.TryParse(row.Cells["offer_cost_price"].Value.ToString(), out offer_cost_price))
                                    offer_cost_price = 0;

                                //매출원가 - 최저단가
                                if (con.cbMinPrice.Checked && isOutput)
                                {
                                    if (con.cbMinPriceType.Text == "<=" && sales_price > purchase_price)
                                        isOutput = false;
                                    else if (con.cbMinPriceType.Text == "<" && sales_price >= purchase_price)
                                        isOutput = false;
                                    else if (con.cbMinPriceType.Text == ">=" && sales_price < purchase_price)
                                        isOutput = false;
                                    else if (con.cbMinPriceType.Text == ">" && sales_price <= purchase_price)
                                        isOutput = false;

                                    //최저단가 0 -> 우리 재고밖에 없는 품목, 최저가 찾을땐 추가
                                    if (con.cbMinPriceType.Text.Contains("<") && purchase_price == 0)
                                        isOutput = true;

                                }
                                //매출단가 - 일반시세
                                if (con.cbNormalPrice.Checked && isOutput)
                                {
                                    if (con.cbNormalPriceType.Text == "<=" && sales_price > average_purchase_price)
                                        isOutput = false;
                                    else if (con.cbNormalPriceType.Text == "<" && sales_price >= average_purchase_price)
                                        isOutput = false;
                                    else if (con.cbNormalPriceType.Text == ">=" && sales_price < average_purchase_price)
                                        isOutput = false;
                                    else if (con.cbNormalPriceType.Text == ">" && sales_price <= average_purchase_price)
                                        isOutput = false;

                                    //최저단가 0 -> 우리 재고밖에 없는 품목, 최저가 찾을땐 추가
                                    if (con.cbNormalPriceType.Text.Contains("<") && average_purchase_price == 0)
                                        isOutput = true;
                                }
                                //매출단가 - 선적원가
                                if (con.cbShipmentCost.Checked && isOutput)
                                {
                                    if (con.cbShipmentCostType.Text == "<=" && sales_price > shipment_cost_price)
                                        isOutput = false;
                                    else if (con.cbShipmentCostType.Text == "<" && sales_price >= shipment_cost_price)
                                        isOutput = false;
                                    else if (con.cbShipmentCostType.Text == ">=" && sales_price < shipment_cost_price)
                                        isOutput = false;
                                    else if (con.cbShipmentCostType.Text == ">" && sales_price <= shipment_cost_price)
                                        isOutput = false;

                                    //최저단가 0 -> 우리 재고밖에 없는 품목, 최저가 찾을땐 추가
                                    if (con.cbShipmentCostType.Text.Contains("<") && shipment_cost_price == 0)
                                        isOutput = true;
                                }
                                //매출단가 - 오퍼원가
                                if (con.cbOfferCost.Checked && isOutput)
                                {
                                    if (con.cbOfferCostType.Text == "<=" && sales_price > offer_cost_price)
                                        isOutput = false;
                                    else if (con.cbOfferCostType.Text == "<" && sales_price >= offer_cost_price)
                                        isOutput = false;
                                    else if (con.cbOfferCostType.Text == ">=" && sales_price < offer_cost_price)
                                        isOutput = false;
                                    else if (con.cbOfferCostType.Text == ">" && sales_price <= offer_cost_price)
                                        isOutput = false;

                                    //최저단가 0 -> 우리 재고밖에 없는 품목, 최저가 찾을땐 추가
                                    if (con.cbOfferCostType.Text.Contains("<") && offer_cost_price == 0)
                                        isOutput = true;
                                }
                                //매출단가 - 오퍼원가
                                if (con.cbOfferCost.Checked && isOutput)
                                {
                                    if (con.cbOfferCostType.Text == "<=" && sales_price > offer_cost_price)
                                        isOutput = false;
                                    else if (con.cbOfferCostType.Text == "<" && sales_price >= offer_cost_price)
                                        isOutput = false;
                                    else if (con.cbOfferCostType.Text == ">=" && sales_price < offer_cost_price)
                                        isOutput = false;
                                    else if (con.cbOfferCostType.Text == ">" && sales_price <= offer_cost_price)
                                        isOutput = false;
                                }
                                //매출단가 - 최저단가
                                if (con.cbMinPriceMarginRate.Checked && isOutput)
                                {
                                    double margin = ((sales_price - purchase_price) / sales_price) * 100;

                                    double sttMargin, endMargin;
                                    if (!double.TryParse(con.txtMinPriceSttMargin.Text, out sttMargin))
                                        sttMargin = 0;
                                    if (!double.TryParse(con.txtMinPriceEndMargin.Text, out endMargin))
                                        endMargin = 0;

                                    if (!(margin >= sttMargin && margin <= endMargin))
                                        isOutput = false;
                                }
                                //매출단가 - 일반시세
                                if (con.cbNormalPriceMarginRate.Checked && isOutput)
                                {
                                    double margin = ((sales_price - average_purchase_price) / sales_price) * 100;

                                    double sttMargin, endMargin;
                                    if (!double.TryParse(con.txtNormalPriceSttMargin.Text, out sttMargin))
                                        sttMargin = 0;
                                    if (!double.TryParse(con.txtNormalPriceEndMargin.Text, out endMargin))
                                        endMargin = 0;

                                    if (!(margin >= sttMargin && margin <= endMargin))
                                        isOutput = false;
                                }
                                //매출단가 - S원가
                                if (con.cbSeaoverCostMarginRate.Checked && isOutput)
                                {
                                    double margin = ((sales_price - sales_cost_price) / sales_price) * 100;

                                    double sttMargin, endMargin;
                                    if (!double.TryParse(con.txtSeaoverCostSttMargin.Text, out sttMargin))
                                        sttMargin = 0;
                                    if (!double.TryParse(con.txtSeaoverCostEndMargin.Text, out endMargin))
                                        endMargin = 0;

                                    if (!(margin >= sttMargin && margin <= endMargin))
                                        isOutput = false;
                                }
                                //매출단가 - 평균원가1
                                if (con.cbAverageCostMarginRate.Checked && isOutput)
                                {
                                    double margin = ((sales_price - average_sales_cost_price1) / sales_price) * 100;

                                    double sttMargin, endMargin;
                                    if (!double.TryParse(con.txtAverageCostSttMargin.Text, out sttMargin))
                                        sttMargin = 0;
                                    if (!double.TryParse(con.txtAverageCostEndMargin.Text, out endMargin))
                                        endMargin = 0;

                                    if (!(margin >= sttMargin && margin <= endMargin))
                                        isOutput = false;
                                }
                                //S원가 - 평균원가1
                                if (con.cbCostMarginRate.Checked && isOutput)
                                {
                                    double margin = ((sales_cost_price - average_sales_cost_price1) / sales_cost_price) * 100;

                                    double sttMargin, endMargin;
                                    if (!double.TryParse(con.txtCostSttMargin.Text, out sttMargin))
                                        sttMargin = 0;

                                    switch (con.CbCostMarginDivision.Text)
                                    {
                                        case "인상":
                                            if (margin < sttMargin)
                                                isOutput = false;
                                            break;
                                        case "인하":
                                            if (margin > sttMargin)
                                                isOutput = false;
                                            break;
                                    }
                                }
                            }

                            //단가비교
                            if (con.cbNormalPriceAdvance.Checked && isOutput)
                            {
                                //일반시세 등수
                                dgvProduct.Columns["average_purchase_price_rank"].Visible = true;

                                if (dgvProduct.Columns.Count > 0)
                                    dgvProduct.Columns["sales_price_updatetime"].Visible = true;

                                //단가제외기간
                                DateTime excluedSttdate = DateTime.Now.AddDays(10), excluedEnddate = DateTime.Now.AddDays(10);
                                if (con.cbExcludeSaleTerms.Checked)
                                {
                                    if (DateTime.TryParse(con.txtExcluedSttdate.Text, out excluedSttdate))
                                        excluedSttdate = DateTime.Now.AddDays(10);
                                    if (DateTime.TryParse(con.txtExcluedEnddate.Text, out excluedEnddate))
                                        excluedEnddate = DateTime.Now.AddDays(10);
                                }

                                //일반시세
                                if (isOutput)
                                {
                                    //단가수정일 제외기간
                                    DateTime sales_price_updatetime;
                                    if (DateTime.TryParse(row.Cells["sales_price_updatetime"].Value.ToString(), out sales_price_updatetime))
                                    {
                                        if (excluedSttdate <= sales_price_updatetime && excluedEnddate >= sales_price_updatetime)
                                            isOutput = false;
                                    }
                                    //조건에 부합할때
                                    if (isOutput)
                                    {
                                        double sales_price = 0;
                                        if (row.Cells["sales_price"].Value == null || !double.TryParse(row.Cells["sales_price"].Value.ToString(), out sales_price))
                                            sales_price = 0;

                                        string whr;
                                        if (row.Cells["price_unit"].Value.ToString().Contains("묶") || row.Cells["price_unit"].Value.ToString().Contains("팩"))
                                        {
                                            whr = $"품명 = '{row.Cells["product"].Value.ToString()}'  "
                                                + $" AND 원산지 = '{row.Cells["origin"].Value.ToString()}'  "
                                                + $" AND 규격 = '{row.Cells["sizes"].Value.ToString()}'  "
                                                + $" AND 단위 = '{row.Cells["unit"].Value.ToString()}'  ";
                                        }
                                        else
                                        {
                                            whr = $"품명 = '{row.Cells["product"].Value.ToString()}'  "
                                                    + $" AND 원산지 = '{row.Cells["origin"].Value.ToString()}'  "
                                                    + $" AND 규격 = '{row.Cells["sizes"].Value.ToString()}'  "
                                                    + $" AND 단위 = '{row.Cells["seaover_unit"].Value.ToString()}'  ";
                                        }

                                        //업체수
                                        DataRow[] dr = normalPirceListDt.Select(whr);


                                        string month_round_txt = "";
                                        if (con.dgvNormalPriceAdvance.Rows[con.dgvNormalPriceAdvance.Rows.Count - 1].Cells["remark"].Value != null)
                                            month_round_txt = Regex.Replace(con.dgvNormalPriceAdvance.Rows[con.dgvNormalPriceAdvance.Rows.Count - 1].Cells["remark"].Value.ToString().Trim(), @"[^0-9]", "").ToString();
                                        int limit_month_around;
                                        if (!int.TryParse(month_round_txt, out limit_month_around))
                                            limit_month_around = 3;

                                        //업체수
                                        int rowIdx;
                                        if (dr.Length >= 9)
                                            rowIdx = 0;
                                        else
                                            rowIdx = 9 - dr.Length;

                                        //거래처수 1개일 땐
                                        if (rowIdx == 9)
                                        {
                                            double month_around;
                                            if (row.Cells["month_around"].Value == null || !double.TryParse(row.Cells["month_around"].Value.ToString(), out month_around))
                                                month_around = 0;

                                            if (month_around < limit_month_around)
                                                isOutput = false;

                                            row.Cells["average_purchase_price_rank"].Value = "1 / 1";
                                        }
                                        else
                                        {
                                            
                                            
                                            //업체별 시세 + 매출단가
                                            DataTable priceDt = dr.CopyToDataTable();
                                            DataRow newDr = priceDt.NewRow();
                                            newDr["매입처"] = "매출단가";
                                            newDr["일반시세"] = sales_price.ToString("#,##0");
                                            priceDt.Rows.Add(newDr);
                                            priceDt.AcceptChanges();
                                            DataView dv = new DataView(priceDt);
                                            dv.Sort = "일반시세 ASC";
                                            priceDt = dv.ToTable();

                                            //출력대상이면 
                                            if (isOutput)
                                            {
                                                int rank = 1;
                                                int currentRank = 1;
                                                int tmpRank = 1;
                                                double currentPrice = 0;
                                                string tooltipTxt = "";
                                                for (int j = 0; j < priceDt.Rows.Count; j++)
                                                {
                                                    double normal_price = 0;
                                                    if (!double.TryParse(priceDt.Rows[j]["일반시세"].ToString(), out normal_price))
                                                        normal_price = 0;

                                                    string normal_price_updatetime_txt = "";
                                                    DateTime normal_price_updatetime;
                                                    if (DateTime.TryParse(priceDt.Rows[j]["매입일자"].ToString(), out normal_price_updatetime))
                                                        normal_price_updatetime_txt = normal_price_updatetime.ToString("yyyy-MM-dd");

                                                    //현재등수
                                                    if (currentPrice < normal_price && !string.IsNullOrEmpty(tooltipTxt))
                                                    {
                                                        rank += tmpRank;
                                                        tmpRank = 1;
                                                    }
                                                    else if (currentPrice == normal_price)
                                                        tmpRank++;

                                                    currentPrice = normal_price;

                                                    //Tooltip
                                                    if (string.IsNullOrEmpty(normal_price_updatetime_txt))
                                                        tooltipTxt += $"\n{rank}. {priceDt.Rows[j]["매입처"].ToString()}" + " : " + normal_price.ToString("#,##0");
                                                    else
                                                        tooltipTxt += $"\n{rank}. {priceDt.Rows[j]["매입처"].ToString()} ({normal_price_updatetime_txt})" + " : " + normal_price.ToString("#,##0");

                                                    if (priceDt.Rows[j]["매입처"].ToString().Equals("매출단가"))
                                                    {
                                                        currentRank = rank;
                                                        tooltipTxt += "    <===";
                                                    }

                                                }
                                                row.Cells["average_purchase_price_rank"].ToolTipText = tooltipTxt.Trim();
                                                row.Cells["average_purchase_price_rank"].Value = currentRank + " / " + priceDt.Rows.Count.ToString();

                                                //업체별 등수 제한
                                                if (con.rbRank.Checked)
                                                {
                                                    //업체 제한등수
                                                    int limit_rank = Convert.ToInt16(normalPriceDt.Rows[rowIdx][1].ToString());

                                                    if (con.cbNormalPriceAdvanceType.Text == "초과" && currentRank <= limit_rank)
                                                        isOutput = false;
                                                    else if (con.cbNormalPriceAdvanceType.Text == "미만" && currentRank >= limit_rank)
                                                        isOutput = false;
                                                }
                                                //업체별 비율 제한
                                                else if (con.rbRate.Checked)
                                                {
                                                    //업체 제한비율
                                                    double limit_rate = -1;
                                                    if (double.TryParse(normalPriceDt.Rows[rowIdx][2].ToString().Replace("%", ""), out limit_rate) && limit_rate > 0)
                                                    { 
                                                        double current_rate = (double)currentRank / (double)priceDt.Rows.Count * 100;
                                                        if (con.cbNormalPriceAdvanceType.Text == "초과" && current_rate <= limit_rate)
                                                            isOutput = false;
                                                        else if (con.cbNormalPriceAdvanceType.Text == "미만" && current_rate >= limit_rate)
                                                            isOutput = false;
                                                    }
                                                    else
                                                    {
                                                        month_round_txt = "";
                                                        if (!string.IsNullOrEmpty(normalPriceDt.Rows[rowIdx]["remark"].ToString()))
                                                            month_round_txt = Regex.Replace(normalPriceDt.Rows[rowIdx]["remark"].ToString().Trim(), @"[^0-9]", "").ToString();
                                                        if (!int.TryParse(month_round_txt, out limit_month_around))
                                                            limit_month_around = 3;

                                                        double month_around;
                                                        if (row.Cells["month_around"].Value == null || !double.TryParse(row.Cells["month_around"].Value.ToString(), out month_around))
                                                            month_around = 0;

                                                        if (month_around < limit_month_around)
                                                            isOutput = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //출력여부
                            row.Visible = isOutput;
                        }
                        else
                            row.Visible = false;


                    }
                }
                catch
                {
                    temp = temp;
                }
                currencyManager1.ResumeBinding();
            }
        }
        #endregion

        #region Method
        private void HideSubProduct()
        {
            //서브품목 숨기기
            if (dgvProduct.RowCount > 0)
            {
                CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dgvProduct.DataSource];
                currencyManager1.SuspendBinding();
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    int main_id;
                    if (dgvProduct.Rows[i].Cells["main_id"].Value == null || !int.TryParse(dgvProduct.Rows[i].Cells["main_id"].Value.ToString(), out main_id))
                        main_id = 0;
                    int sub_id;
                    if (dgvProduct.Rows[i].Cells["sub_id"].Value == null || !int.TryParse(dgvProduct.Rows[i].Cells["sub_id"].Value.ToString(), out sub_id))
                        sub_id = 0;

                    if (dgvProduct.Rows[i].Cells["sales_cost_price_tooltip"].Value != null)
                    {
                        dgvProduct.Rows[i].Cells["sales_cost_price"].ToolTipText = dgvProduct.Rows[i].Cells["sales_cost_price_tooltip"].Value.ToString();
                        if (dgvProduct.Rows[i].Cells["sales_cost_price_tooltip"].Value.ToString().Contains("확인필요"))
                            dgvProduct.Rows[i].Cells["sales_cost_price"].Style.ForeColor = Color.Red;
                    }

                    if (main_id > 0 && sub_id < 9999)
                        dgvProduct.Rows[i].Visible = false;
                }
                currencyManager1.ResumeBinding();
            }
        }
        private void AllRowCalculateCostPrice()
        {
            if (dgvProduct.Rows.Count > 0)
            {
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];
                    //오퍼단가 계산
                    double box_weight = Convert.ToDouble(row.Cells["seaover_unit"].Value.ToString());
                    double cost_unit = Convert.ToDouble(row.Cells["cost_unit"].Value.ToString());
                    double unit = box_weight;
                    if (cost_unit > 0)
                        unit = cost_unit;
                    double offer_price = Convert.ToDouble(row.Cells["offer_price"].Value.ToString());

                    double tax;
                    if (!double.TryParse(row.Cells["tax"].Value.ToString(), out tax))
                        tax = 0;
                    tax = tax / 100;
                    double custom;
                    if (!double.TryParse(row.Cells["custom"].Value.ToString(), out custom))
                        custom = 0;
                    custom = custom / 100;
                    double incidental_expense;
                    if (!double.TryParse(row.Cells["incidental_expense"].Value.ToString(), out incidental_expense))
                        incidental_expense = 0;
                    incidental_expense = incidental_expense / 100;

                    double fixed_tariff;
                    if (!double.TryParse(row.Cells["fixed_tariff"].Value.ToString(), out fixed_tariff))
                        fixed_tariff = 0;
                    fixed_tariff /= 1000;

                    double dongwon = 0;
                    if (cbDongwon.Checked)
                        dongwon = 0.035;

                    double unit_count = Convert.ToDouble(row.Cells["unit_count"].Value.ToString());
                    //원가계산
                    if (rbCostprice.Checked)
                    {
                        double cost_price;

                        if (fixed_tariff > 0)
                        {
                            cost_price = unit * offer_price * exchange_rate;
                            cost_price += fixed_tariff * unit * exchange_rate * custom;

                            cost_price *= (tax + 1);
                            cost_price *= (incidental_expense + 1);
                            cost_price *= (dongwon + 1);
                        }
                        else
                        {
                            cost_price = unit * offer_price * exchange_rate;
                            cost_price *= (custom + 1);

                            cost_price *= (tax + 1);
                            cost_price *= (incidental_expense + 1);
                            cost_price *= (dongwon + 1);
                        }
                        if (unit_count > 0)
                            cost_price /= unit_count;
                        row.Cells["offer_cost_price"].Value = cost_price;
                    }
                    //TRQ원가계산
                    else if (rbTrq.Checked)
                    {
                        double trq = Convert.ToDouble(txtTrq.Text.Replace(",", ""));
                        double trq_price;
                        if (offer_price > 0)
                            trq_price = (offer_price * unit * (1 + tax + incidental_expense) * exchange_rate) + (box_weight * trq);
                        else
                            trq_price = 0;
                        if (unit_count > 0)
                            trq_price /= unit_count;
                        row.Cells["offer_cost_price"].Value = trq_price;
                    }


                    //평균원가
                    double seaover_stock = Convert.ToDouble(row.Cells["seaover_pending"].Value.ToString()) + Convert.ToDouble(row.Cells["seaover_unpending"].Value.ToString());
                    double shipment_stock = Convert.ToDouble(row.Cells["shipment_qty"].Value.ToString()) + Convert.ToDouble(row.Cells["unpending_qty_before"].Value.ToString());
                    double order_qty = Convert.ToDouble(row.Cells["order_qty"].Value.ToString());

                    double total_average_cost_price = 0;
                    double total_real_stock = 0;

                    double sales_cost_price = Convert.ToDouble(row.Cells["sales_cost_price"].Value.ToString());
                    double shipment_cost_price = Convert.ToDouble(row.Cells["pending_cost_price"].Value.ToString());
                    double offer_cost_price = Convert.ToDouble(row.Cells["offer_cost_price"].Value.ToString());

                    if (sales_cost_price > 0 && seaover_stock > 0)
                    {
                        total_average_cost_price += sales_cost_price * seaover_stock;
                        total_real_stock += seaover_stock;
                    }
                    if (shipment_cost_price > 0 && shipment_stock > 0)
                    {
                        total_average_cost_price += shipment_cost_price * shipment_stock;
                        total_real_stock += shipment_stock;
                    }
                    //평균원가1
                    double average_sales_cost_price1 = total_average_cost_price / total_real_stock;
                    if (double.IsNaN(average_sales_cost_price1))
                        average_sales_cost_price1 = 0;
                    row.Cells["average_sales_cost_price1"].Value = average_sales_cost_price1;
                    //평균원2
                    if (offer_cost_price > 0 && order_qty != 0)
                    {
                        total_average_cost_price += offer_cost_price * order_qty;
                        total_real_stock += order_qty;
                    }
                    double average_sales_cost_price2 = total_average_cost_price / total_real_stock;
                    if (double.IsNaN(average_sales_cost_price2))
                        average_sales_cost_price2 = 0;
                    row.Cells["average_sales_cost_price1"].Value = average_sales_cost_price2;
                }
            }
        }
        private void StyleSettingTxt()
        {
            dgvProduct.EndEdit();
            //정렬
            if (styleDic.ContainsKey("sort"))
                styleDic["sort"] = cbSortType.Text;
            else
                styleDic.Add("sort", cbSortType.Text);
            //컬럼 스타일
            for (int i = 0; i < dgvProduct.ColumnCount; i++)
            {
                if (styleDic.ContainsKey(dgvProduct.Columns[i].Name))
                {
                    if (dgvProduct.Columns[i].Visible)
                        styleDic[dgvProduct.Columns[i].Name] = dgvProduct.Columns[i].Width.ToString();
                    else
                        styleDic[dgvProduct.Columns[i].Name] = (-dgvProduct.Columns[i].Width).ToString();
                }
                else
                {
                    if (dgvProduct.Columns[i].Visible)
                        styleDic.Add(dgvProduct.Columns[i].Name, dgvProduct.Columns[i].Width.ToString());
                    else
                        styleDic.Add(dgvProduct.Columns[i].Name, (-dgvProduct.Columns[i].Width).ToString());
                }
            }
        }

        private void ChangeForeColor()
        {
            if (dgvProduct.SelectedCells.Count > 0)
            {
                Color color = new Color();
                ColorDialog cd = new ColorDialog();
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    color = cd.Color;
                    btnFontColor.BackColor = color;
                    foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                        cell.Style.ForeColor = color;
                }
            }
        }
        private void ChangeBackColor()
        {
            if (dgvProduct.SelectedCells.Count > 0)
            {
                Color color = new Color();
                ColorDialog cd = new ColorDialog();
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    color = cd.Color;
                    btnColor.BackColor = color;
                    foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                        cell.Style.BackColor = color;
                }
            }
        }
        private void CalculateAverageSalesCostPriceMargin(DataGridViewRow row, bool isSPOCalculate = true)
        {
            //평균원가1,2 마진율
            double average_sales_cost_price1;
            if (row.Cells["average_sales_cost_price1"].Value == null || !double.TryParse(row.Cells["average_sales_cost_price1"].Value.ToString(), out average_sales_cost_price1))
                average_sales_cost_price1 = 0;
            double average_sales_cost_price2;
            if (row.Cells["average_sales_cost_price2"].Value == null || !double.TryParse(row.Cells["average_sales_cost_price2"].Value.ToString(), out average_sales_cost_price2))
                average_sales_cost_price2 = 0;
            //비교단가
            double temp_price;
            if (rbSalesPriceMargin.Checked)
            {
                if (row.Cells["sales_price"].Value == null || !double.TryParse(row.Cells["sales_price"].Value.ToString(), out temp_price))
                    temp_price = 0;
            }
            else if (rbNormalPriceMargin.Checked)
            {
                if (row.Cells["average_purchase_price"].Value == null || !double.TryParse(row.Cells["average_purchase_price"].Value.ToString(), out temp_price))
                    temp_price = 0;
            }
            else
            {
                if (row.Cells["purchase_price"].Value == null || !double.TryParse(row.Cells["purchase_price"].Value.ToString(), out temp_price))
                    temp_price = 0;
            }
            double average_sales_cost_price1_margin, average_sales_cost_price2_margin;
            average_sales_cost_price1_margin = (1 - (average_sales_cost_price1 / temp_price)) * 100;
            if (double.IsNaN(average_sales_cost_price1_margin))
                average_sales_cost_price1_margin = 0;
            average_sales_cost_price2_margin = (1 - (average_sales_cost_price2 / temp_price)) * 100;
            if (double.IsNaN(average_sales_cost_price2_margin))
                average_sales_cost_price2_margin = 0;

            row.Cells["average_sales_cost_price1_margin"].Value = average_sales_cost_price1_margin;
            row.Cells["average_sales_cost_price2_margin"].Value = average_sales_cost_price2_margin;

            //오퍼원가 마진
            double offer_cost_price;
            if (row.Cells["offer_cost_price"].Value == null || !double.TryParse(row.Cells["offer_cost_price"].Value.ToString(), out offer_cost_price))
                offer_cost_price = 0;
            double offer_cost_price_margin = (temp_price - offer_cost_price) / offer_cost_price * 100;
            row.Cells["offer_cost_price_margin"].Value = offer_cost_price_margin;

            if(isSPOCalculate)
                CalculateCostPriceMarginAmount();
        }

        bool isSelectCalculate;
        private void CalculateCostPriceMarginAmount(bool isSelect = false)
        {
            isSelectCalculate = isSelect;
            double total_offer_qty = 0;
            double total_margin_amount0 = 0;
            double total_margin_amount1 = 0;
            double total_margin_amount2 = 0;
            double total_margin_amount3 = 0;
            if (!isSelect)
            {
                foreach (DataGridViewRow row in dgvProduct.Rows)
                {
                    if (row.Visible)
                    {
                        //재고수
                        double shipment_qty;
                        if (row.Cells["shipment_qty"].Value == null || !double.TryParse(row.Cells["shipment_qty"].Value.ToString(), out shipment_qty))
                            shipment_qty = 0;
                        double unpending_qty_before;
                        if (row.Cells["unpending_qty_before"].Value == null || !double.TryParse(row.Cells["unpending_qty_before"].Value.ToString(), out unpending_qty_before))
                            unpending_qty_before = 0;
                        double seaover_unpending;
                        if (row.Cells["seaover_unpending"].Value == null || !double.TryParse(row.Cells["seaover_unpending"].Value.ToString(), out seaover_unpending))
                            seaover_unpending = 0;
                        double seaover_pending;
                        if (row.Cells["seaover_pending"].Value == null || !double.TryParse(row.Cells["seaover_pending"].Value.ToString(), out seaover_pending))
                            seaover_pending = 0;
                        double reserved_stock;
                        if (row.Cells["reserved_stock"].Value == null || !double.TryParse(row.Cells["reserved_stock"].Value.ToString(), out reserved_stock))
                            reserved_stock = 0;

                        //S원가
                        double sales_cost_price;
                        if (row.Cells["sales_cost_price"].Value == null || !double.TryParse(row.Cells["sales_cost_price"].Value.ToString(), out sales_cost_price))
                            sales_cost_price = 0;
                        //선적원가
                        double pending_cost_price;
                        if (row.Cells["pending_cost_price"].Value == null || !double.TryParse(row.Cells["pending_cost_price"].Value.ToString(), out pending_cost_price))
                            pending_cost_price = 0;
                        //평균원가1
                        double average_sales_cost_price1;
                        if (row.Cells["average_sales_cost_price1"].Value == null || !double.TryParse(row.Cells["average_sales_cost_price1"].Value.ToString(), out average_sales_cost_price1))
                            average_sales_cost_price1 = 0;
                        if (double.IsNaN(average_sales_cost_price1))
                            average_sales_cost_price1 = 0;
                        else if(double.IsInfinity(average_sales_cost_price1))
                            average_sales_cost_price1 = 0;
                        //평균원가2
                        double average_sales_cost_price2;
                        if (row.Cells["average_sales_cost_price2"].Value == null || !double.TryParse(row.Cells["average_sales_cost_price2"].Value.ToString(), out average_sales_cost_price2))
                            average_sales_cost_price2 = 0;
                        if (double.IsNaN(average_sales_cost_price2))
                            average_sales_cost_price2 = 0;
                        else if (double.IsInfinity(average_sales_cost_price2))
                            average_sales_cost_price2 = 0;
                        //오더수량
                        double order_qty;
                        if (row.Cells["order_qty"].Value == null || !double.TryParse(row.Cells["order_qty"].Value.ToString(), out order_qty))
                            order_qty = 0;
                        total_offer_qty += order_qty;
                        //오펴원가
                        double offer_cost_price;
                        if (row.Cells["offer_cost_price"].Value == null || !double.TryParse(row.Cells["offer_cost_price"].Value.ToString(), out offer_cost_price))
                            offer_cost_price = 0;

                        //비교단가
                        double temp_price;
                        if (rbSalesPriceMargin.Checked)
                        {
                            if (!double.TryParse(row.Cells["sales_price"].Value.ToString(), out temp_price))
                                temp_price = 0;
                        }
                        else if (rbNormalPriceMargin.Checked)
                        {
                            if (!double.TryParse(row.Cells["average_purchase_price"].Value.ToString(), out temp_price))
                                temp_price = 0;
                        }
                        else
                        {
                            if (!double.TryParse(row.Cells["purchase_price"].Value.ToString(), out temp_price))
                                temp_price = 0;
                        }


                        //O손익
                        if (temp_price > 0 && offer_cost_price > 0 && order_qty != 0)
                            total_margin_amount0 += (temp_price * order_qty) - (offer_cost_price * order_qty);
                        //손익1 (재고만)
                        if (temp_price > 0 && sales_cost_price > 0)
                            total_margin_amount1 += (temp_price - sales_cost_price) * (seaover_unpending + seaover_pending - reserved_stock);
                        //손익2 (재고 + 선적)
                        if (temp_price > 0 && average_sales_cost_price1 > 0)
                            total_margin_amount2 += (temp_price - average_sales_cost_price1) * (shipment_qty + unpending_qty_before + seaover_unpending + seaover_pending - reserved_stock);
                        //SPO 손익
                        if (temp_price > 0 && average_sales_cost_price2 > 0)
                            total_margin_amount3 += (temp_price - average_sales_cost_price2) * (shipment_qty + unpending_qty_before + seaover_unpending + seaover_pending + order_qty - reserved_stock);
                    }
                }
            }
            else
            {
                foreach (DataGridViewRow row in dgvProduct.Rows)
                {
                    if (row.Selected && row.Visible)
                    {
                        //재고수
                        double shipment_qty;
                        if (row.Cells["shipment_qty"].Value == null || !double.TryParse(row.Cells["shipment_qty"].Value.ToString(), out shipment_qty))
                            shipment_qty = 0;
                        double unpending_qty_before;
                        if (row.Cells["unpending_qty_before"].Value == null || !double.TryParse(row.Cells["unpending_qty_before"].Value.ToString(), out unpending_qty_before))
                            unpending_qty_before = 0;
                        double seaover_unpending;
                        if (row.Cells["seaover_unpending"].Value == null || !double.TryParse(row.Cells["seaover_unpending"].Value.ToString(), out seaover_unpending))
                            seaover_unpending = 0;
                        double seaover_pending;
                        if (row.Cells["seaover_pending"].Value == null || !double.TryParse(row.Cells["seaover_pending"].Value.ToString(), out seaover_pending))
                            seaover_pending = 0;
                        double reserved_stock;
                        if (row.Cells["reserved_stock"].Value == null || !double.TryParse(row.Cells["reserved_stock"].Value.ToString(), out reserved_stock))
                            reserved_stock = 0;

                        //S원가
                        double sales_cost_price;
                        if (row.Cells["sales_cost_price"].Value == null || !double.TryParse(row.Cells["sales_cost_price"].Value.ToString(), out sales_cost_price))
                            sales_cost_price = 0;
                        //선적원가
                        double pending_cost_price;
                        if (row.Cells["pending_cost_price"].Value == null || !double.TryParse(row.Cells["pending_cost_price"].Value.ToString(), out pending_cost_price))
                            pending_cost_price = 0;
                        //평균원가1
                        double average_sales_cost_price1;
                        if (row.Cells["average_sales_cost_price1"].Value == null || !double.TryParse(row.Cells["average_sales_cost_price1"].Value.ToString(), out average_sales_cost_price1))
                            average_sales_cost_price1 = 0;
                        //평균원가2
                        double average_sales_cost_price2;
                        if (row.Cells["average_sales_cost_price2"].Value == null || !double.TryParse(row.Cells["average_sales_cost_price2"].Value.ToString(), out average_sales_cost_price2))
                            average_sales_cost_price2 = 0;
                        //오더수량
                        double order_qty;
                        if (row.Cells["order_qty"].Value == null || !double.TryParse(row.Cells["order_qty"].Value.ToString(), out order_qty))
                            order_qty = 0;
                        total_offer_qty += order_qty;
                        //오펴원가
                        double offer_cost_price;
                        if (row.Cells["offer_cost_price"].Value == null || !double.TryParse(row.Cells["offer_cost_price"].Value.ToString(), out offer_cost_price))
                            offer_cost_price = 0;

                        //비교단가
                        double temp_price;
                        if (rbSalesPriceMargin.Checked)
                        {
                            if (row.Cells["sales_price"].Value == null || !double.TryParse(row.Cells["sales_price"].Value.ToString(), out temp_price))
                                temp_price = 0;
                        }
                        else if (rbNormalPriceMargin.Checked)
                        {
                            if (row.Cells["average_purchase_price"].Value == null || !double.TryParse(row.Cells["average_purchase_price"].Value.ToString(), out temp_price))
                                temp_price = 0;
                        }
                        else
                        {
                            if (row.Cells["purchase_price"].Value == null || !double.TryParse(row.Cells["purchase_price"].Value.ToString(), out temp_price))
                                temp_price = 0;
                        }


                        //O손익
                        if (temp_price > 0 && offer_cost_price > 0 && order_qty != 0)
                            total_margin_amount0 += (temp_price * order_qty) - (offer_cost_price * order_qty);
                        //손익1 (재고만)
                        if (temp_price > 0 && sales_cost_price > 0)
                            total_margin_amount1 += (temp_price - sales_cost_price) * (seaover_unpending + seaover_pending - reserved_stock);
                        //손익2 (재고 + 선적)
                        if (temp_price > 0 && average_sales_cost_price1 > 0)
                            total_margin_amount2 += (temp_price - average_sales_cost_price1) * (shipment_qty + unpending_qty_before + seaover_unpending + seaover_pending - reserved_stock);
                        //SPO 손익
                        if (temp_price > 0 && average_sales_cost_price2 > 0)
                            total_margin_amount3 += (temp_price - average_sales_cost_price2) * (shipment_qty + unpending_qty_before + seaover_unpending + seaover_pending + order_qty - reserved_stock);
                    }
                }
            }
            txtTotalOrderQty.Text = total_offer_qty.ToString("#,##0");
            txtTotalOrderMarginAmount0.Text = total_margin_amount0.ToString("#,##0");
            txtTotalOrderMarginAmount1.Text = total_margin_amount1.ToString("#,##0");
            txtTotalOrderMarginAmount2.Text = total_margin_amount2.ToString("#,##0");
            txtTotalOrderMarginAmount3.Text = total_margin_amount3.ToString("#,##0");
        }

        public void CHangeCheckBox(int chkId)
        {
            switch (chkId)
            {
                case 1:
                    cbReservationDetails.Checked = !cbReservationDetails.Checked;
                    break;
                case 2:
                    cbSalesCount.Checked = !cbSalesCount.Checked;
                    break;
                case 3:
                    cbMarginMinPrice.Checked = !cbMarginMinPrice.Checked;
                    break;
                case 4:
                    cbCostPrice.Checked = !cbCostPrice.Checked;
                    break;
                case 5:
                    cbOffer.Checked = !cbOffer.Checked;
                    break;
                case 6:
                    cbOffer.Checked = !cbOffer.Checked;
                    break;
            }
        }
        private void changeExchangeRate()
        {
            if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
            {
                msgBox.Show(this, "환율 값을 확인해주세요.");
                this.Activate();
                return;
            }
            else
                txtExchangeRate.Text = exchange_rate.ToString("#,##0.00");
            //반영
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                DataGridViewRow row = dgvProduct.Rows[i];
                CalculateOfferCostUnit(row);
                CalculateAverageCostUnit(row);
            }
        }
        //씨오버원가 재계산
        /*private void ReplaceSalesCostToPendingCost()
        {
            DataTable productDt = priceComparisonRepository.GetNotSalesCostProduct(txtProduct.Text, txtOrigin.Text, txtSizes.Text, "");
            DataTable productDt2 = priceComparisonRepository.GetNotSalesCostProduct2(txtProduct.Text, txtOrigin.Text, txtSizes.Text, "");
            DataTable pendingDt = customsRepository.GetProductForNotSalesCost(txtProduct.Text, txtOrigin.Text, txtSizes.Text, "", productDt);
            if (productDt.Rows.Count > 0 || productDt2.Rows.Count > 0 || pendingDt.Rows.Count > 0)
            {
                productDt.Columns["수량"].ReadOnly = false;
                productDt.Columns["환율"].ReadOnly = false;
                productDt.Columns["매출원가"].ReadOnly = false;
                productDt.Columns["오퍼가"].ReadOnly = false;
                //데이터 조합
                for (int i = 0; i < productDt.Rows.Count; i++)
                {

                    string whr = "ato_no = '" + productDt.Rows[i]["AtoNo"].ToString() + "'"
                                + " AND product = '" + productDt.Rows[i]["품명"].ToString() + "'"
                                + " AND origin = '" + productDt.Rows[i]["원산지"].ToString() + "'"
                                + " AND sizes = '" + productDt.Rows[i]["규격"].ToString() + "'";
                    DataRow[] dr = pendingDt.Select(whr);
                    if (dr.Length > 0)
                    {
                        for (int j = 0; j < dr.Length; j++)
                        {
                            double unit_price = Convert.ToDouble(dr[j]["unit_price"].ToString());
                            double box_weight = Convert.ToDouble(dr[j]["unit"].ToString());
                            double cost_unit = Convert.ToDouble(dr[j]["cost_unit"].ToString());
                            double trq_amount = Convert.ToDouble(dr[j]["trq_amount"].ToString());
                            bool isWeight = Convert.ToBoolean(dr[j]["weight_calculate"].ToString());
                            //트레이단가 -> 중량 단가
                            if (!isWeight)
                                unit_price = (unit_price * cost_unit) / box_weight;


                            double custom = Convert.ToDouble(dr[j]["custom"].ToString()) / 100;
                            double tax = Convert.ToDouble(dr[j]["tax"].ToString()) / 100;
                            double incidental_expense = Convert.ToDouble(dr[j]["incidental_expense"].ToString()) / 100;
                            double qty = Convert.ToDouble(dr[j]["qty"].ToString());
                            double exchangeRate;
                            if (!double.TryParse(dr[j]["exchange_rate"].ToString(), out exchangeRate))
                                exchangeRate = exchange_rate;
                            //double sales_cost = unit_price * exchangeRate * (1 + custom + tax + incidental_expense);

                            double sales_cost;
                            //일반
                            if (trq_amount == 0)
                                sales_cost = unit_price * exchangeRate * (1 + custom + tax + incidental_expense);
                            //trq
                            else
                                sales_cost = (unit_price * exchangeRate * (1 + tax + incidental_expense)) + (trq_amount);


                            //동원시 추가
                            if (dr[j]["ato_no"].ToString().Contains("dw") || dr[j]["ato_no"].ToString().Contains("DW")
                                || dr[j]["ato_no"].ToString().Contains("hs") || dr[j]["ato_no"].ToString().Contains("HS")
                                || dr[j]["ato_no"].ToString().Contains("od") || dr[j]["ato_no"].ToString().Contains("OD")
                                || dr[j]["ato_no"].ToString().Contains("ad") || dr[j]["ato_no"].ToString().Contains("AD")
                                || dr[j]["ato_no"].ToString().Contains("jd") || dr[j]["ato_no"].ToString().Contains("JD"))
                                sales_cost = sales_cost * 1.035;

                            productDt.Rows[i]["매출원가"] = sales_cost;
                            productDt.Rows[i]["환율"] = exchangeRate;
                            productDt.Rows[i]["오퍼가"] = unit_price;
                            break;
                        }
                    }
                }
                //출력
                if (productDt.Rows.Count > 0 || productDt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    {
                        string price_unit = dgvProduct.Rows[i].Cells["price_unit"].Value.ToString();

                        //ToolTip txt
                        string tooltip = "";
                        //씨오버단위
                        double unit;
                        if (!double.TryParse(dgvProduct.Rows[i].Cells["seaover_unit"].Value.ToString(), out unit))
                            unit = 1;
                        //팩일땐 단위를 사용
                        if (price_unit.Contains("팩"))
                        {
                            if (!double.TryParse(dgvProduct.Rows[i].Cells["unit"].Value.ToString(), out unit))
                                unit = 1;
                        }


                        //단위수량
                        double unit_count;
                        if (!double.TryParse(dgvProduct.Rows[i].Cells["unit_count"].Value.ToString(), out unit_count))
                            unit_count = 1;

                        double total_average_cost = 0;
                        double total_qty = 0;

                        string whr = "품명 = '" + dgvProduct.Rows[i].Cells["product"].Value.ToString() + "'"
                                        + " AND 원산지 = '" + dgvProduct.Rows[i].Cells["origin"].Value.ToString() + "'"
                                        + " AND 규격 = '" + dgvProduct.Rows[i].Cells["sizes"].Value.ToString() + "'"
                                        + " AND 매출원가 > 0";
                        DataRow[] dr = productDt.Select(whr);
                        if (dr.Length > 0)
                        {
                            for (int j = 0; j < dr.Length; j++)
                            {
                                double stock_unit;
                                if (!double.TryParse(dr[j]["단위"].ToString(), out stock_unit))
                                    stock_unit = 1;
                                double qty = Convert.ToDouble(dr[j]["수량"].ToString());
                                double sales_cost = Convert.ToDouble(dr[j]["매출원가"].ToString());
                                sales_cost = sales_cost * unit;
                                
                                //묶음일때만 단위수량으로 나눔
                                if (price_unit == "묶")
                                    sales_cost = sales_cost / unit_count;

                                total_average_cost += sales_cost * qty;
                                total_qty += qty;

                                tooltip += "\n AtoNo :" + dr[j]["AtoNo"].ToString() + "    매출원가 :" + sales_cost.ToString("#,##0") + " (" + dr[j]["오퍼가"].ToString() + " | " + dr[j]["환율"].ToString() + ")  수량 :" + qty;
                            }
                        }

                        whr = "품명 = '" + dgvProduct.Rows[i].Cells["product"].Value.ToString() + "'"
                                        + " AND 원산지 = '" + dgvProduct.Rows[i].Cells["origin"].Value.ToString() + "'"
                                        + " AND 규격 = '" + dgvProduct.Rows[i].Cells["sizes"].Value.ToString() + "'";
                        dr = productDt2.Select(whr);
                        if (dr.Length > 0)
                        {
                            for (int j = 0; j < dr.Length; j++)
                            {
                                double stock_unit;
                                if (!double.TryParse(dr[j]["단위"].ToString(), out stock_unit))
                                    stock_unit = 1;
                                double qty = Convert.ToDouble(dr[j]["수량"].ToString()) / stock_unit * unit;
                                double sales_cost = Convert.ToDouble(dr[j]["매출원가"].ToString());
                                sales_cost = sales_cost / stock_unit * unit;
                                //묶음일때만 단위수량으로 나눔
                                if (price_unit == "묶")
                                    sales_cost = sales_cost / unit_count;

                                total_average_cost += sales_cost * qty;
                                total_qty += qty;

                                tooltip += "\n AtoNo : " + dr[j]["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + "  수량 :" + qty;
                            }
                        }
                        //평균원가
                        if (total_average_cost > 0 && total_qty > 0)
                        {

                            //S원가
                            double seaover_cost_price = (total_average_cost / total_qty);
                            dgvProduct.Rows[i].Cells["sales_cost_price"].Value = seaover_cost_price;
                            dgvProduct.Rows[i].Cells["sales_cost_price"].ToolTipText = " " + tooltip.Trim();

                            if (!double.TryParse(dgvProduct.Rows[i].Cells["seaover_unit"].Value.ToString(), out unit))
                                unit = 1;


                            //선적수
                            double shipment_qty;
                            if (!double.TryParse(dgvProduct.Rows[i].Cells["shipment_qty"].Value.ToString(), out shipment_qty))
                                shipment_qty = 0;
                            //배송중
                            double unpending_qty_before;
                            if (!double.TryParse(dgvProduct.Rows[i].Cells["unpending_qty_before"].Value.ToString(), out unpending_qty_before))
                                unpending_qty_before = 0;
                            //선적원가
                            double pending_cost_price;
                            if (!double.TryParse(dgvProduct.Rows[i].Cells["pending_cost_price"].Value.ToString(), out pending_cost_price))
                                pending_cost_price = 0;

                            //미통관
                            double seaover_unpending;
                            if (!double.TryParse(dgvProduct.Rows[i].Cells["seaover_unpending"].Value.ToString(), out seaover_unpending))
                                seaover_unpending = 0;
                            //통관
                            double seaover_pending;
                            if (!double.TryParse(dgvProduct.Rows[i].Cells["seaover_pending"].Value.ToString(), out seaover_pending))
                                seaover_pending = 0;

                            //평균원가1
                            dgvProduct.Rows[i].Cells["average_sales_cost_price1"].Value = (seaover_cost_price * (seaover_unpending + seaover_pending) + pending_cost_price * (shipment_qty + unpending_qty_before)) / (seaover_unpending + seaover_pending + shipment_qty + unpending_qty_before);


                        }
                    }
                }
            }
        }*/

        private void GetCostToPendingCostTable2(out DataTable dt)
        {
            DataTable productDt = priceComparisonRepository.GetNotSalesCostProduct(txtProduct.Text, txtOrigin.Text, txtSizes.Text, "");
            DataTable pendingDt = customsRepository.GetProductForNotSalesCost(txtProduct.Text, txtOrigin.Text, txtSizes.Text, "", productDt);
            if (productDt.Rows.Count > 0)
            {
                //매출원가 있는 데이터 조합
                productDt.Columns["etd"].ReadOnly = false;
                productDt.Columns["수량"].ReadOnly = false;
                productDt.Columns["매출원가"].ReadOnly = false;
                productDt.Columns["환율"].ReadOnly = false;
                productDt.Columns["오퍼가"].ReadOnly = false;
                productDt.Columns["trq"].ReadOnly = false;
                productDt.Columns["custom_id"].ReadOnly = false;
                productDt.Columns["sub_id"].ReadOnly = false;
                productDt.Columns["etd"].ReadOnly = false;
                productDt.Columns["isPendingCalculate"].ReadOnly = false;


                for (int i = 0; i < productDt.Rows.Count; i++)
                {
                    double sales_price;
                    if (!double.TryParse(productDt.Rows[i]["매출원가"].ToString(), out sales_price))
                        sales_price = 0;
                    if (pendingDt != null && pendingDt.Rows.Count > 0 &&  sales_price == 0)
                    {
                        string whr = "ato_no = '" + productDt.Rows[i]["AtoNo"].ToString() + "'"
                                + " AND product = '" + productDt.Rows[i]["품명"].ToString() + "'"
                                + " AND origin = '" + productDt.Rows[i]["원산지"].ToString() + "'"
                                + " AND sizes = '" + productDt.Rows[i]["규격"].ToString() + "'";
                        DataRow[] dr = pendingDt.Select(whr);
                        if (dr.Length > 0)
                        {
                            //ETD
                            string etd_txt = "";
                            if (DateTime.TryParse(dr[0]["etd2"].ToString(), out DateTime etd))
                                etd_txt = etd.ToString("yyyy-MM-dd");
                            productDt.Rows[i]["etd"] = etd_txt;

                            //단가정보
                            double unit_price = Convert.ToDouble(dr[0]["unit_price"].ToString());
                            double box_weight = Convert.ToDouble(dr[0]["unit"].ToString());
                            double cost_unit = Convert.ToDouble(dr[0]["cost_unit"].ToString());
                            bool isWeight = Convert.ToBoolean(dr[0]["weight_calculate"].ToString());
                            double trq_amount = Convert.ToDouble(dr[0]["trq_amount"].ToString());
                            //트레이단가 -> 중량 단가
                            if (!isWeight && cost_unit == 0)
                                isWeight = true;
                            if (!isWeight)
                                unit_price = (unit_price * cost_unit) / box_weight;

                            //관세,과세,부대비용
                            double custom = Convert.ToDouble(dr[0]["custom"].ToString()) / 100;
                            double tax = Convert.ToDouble(dr[0]["tax"].ToString()) / 100;
                            double incidental_expense = Convert.ToDouble(dr[0]["incidental_expense"].ToString()) / 100;

                            //2023-11-16 대행일 경우 부대비용 강제 3%
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
                            double sales_cost;
                            //일반
                            if (trq_amount == 0)
                                sales_cost = unit_price * exchangeRate * (1 + custom + tax + incidental_expense);
                            //trq
                            else
                                sales_cost = (unit_price * exchangeRate * (1 + tax + incidental_expense)) + (trq_amount);

                            //최신화
                            productDt.Rows[i]["매출원가"] = sales_cost;
                            productDt.Rows[i]["환율"] = exchangeRate;
                            productDt.Rows[i]["오퍼가"] = unit_price;
                            productDt.Rows[i]["etd"] = etd_txt;
                            productDt.Rows[i]["isPendingCalculate"] = "TRUE";
                        }
                    }
                }
            }
            dt = productDt;
        }

        private void GetCostToPendingCostTable(out DataTable dt1, out DataTable dt2)
        {
            DataTable productDt = priceComparisonRepository.GetNotSalesCostProduct(txtProduct.Text, txtOrigin.Text, txtSizes.Text, "");
            DataTable productDt2 = priceComparisonRepository.GetNotSalesCostProduct2(txtProduct.Text, txtOrigin.Text, txtSizes.Text, "");
            DataTable pendingDt = customsRepository.GetProductForNotSalesCost(txtProduct.Text, txtOrigin.Text, txtSizes.Text, "", productDt);
            if (productDt.Rows.Count > 0 || productDt2.Rows.Count > 0 || pendingDt.Rows.Count > 0)
            {
                //매출원가 있는 데이터 조합
                productDt2.Columns["etd"].ReadOnly = false;
                for (int i = 0; i < productDt2.Rows.Count; i++)
                {
                    string whr = "ato_no = '" + productDt2.Rows[i]["AtoNo"].ToString() + "'"
                                + " AND product = '" + productDt2.Rows[i]["품명"].ToString() + "'"
                                + " AND origin = '" + productDt2.Rows[i]["원산지"].ToString() + "'"
                                + " AND sizes = '" + productDt2.Rows[i]["규격"].ToString() + "'";
                    DataRow[] dr = pendingDt.Select(whr);
                    if (dr.Length > 0)
                    {
                        for (int j = 0; j < dr.Length; j++)
                        {
                            DateTime etd;
                            string etd_txt = "";
                            if (DateTime.TryParse(dr[j]["etd2"].ToString(), out etd))
                                etd_txt = etd.ToString("yyyy-MM-dd");
                            productDt2.Rows[i]["etd"] = etd_txt;

                            break;
                        }
                    }
                }


                //매출원가 0데이터 조합
                productDt.Columns["수량"].ReadOnly = false;
                productDt.Columns["매출원가"].ReadOnly = false;
                productDt.Columns["환율"].ReadOnly = false;
                productDt.Columns["오퍼가"].ReadOnly = false;
                productDt.Columns["trq"].ReadOnly = false;
                productDt.Columns["custom_id"].ReadOnly = false;
                productDt.Columns["sub_id"].ReadOnly = false;
                productDt.Columns["etd"].ReadOnly = false;

                for (int i = 0; i < productDt.Rows.Count; i++)
                {
                    string whr = "ato_no = '" + productDt.Rows[i]["AtoNo"].ToString() + "'"
                                + " AND product = '" + productDt.Rows[i]["품명"].ToString() + "'"
                                + " AND origin = '" + productDt.Rows[i]["원산지"].ToString() + "'"
                                + " AND sizes = '" + productDt.Rows[i]["규격"].ToString() + "'";
                    DataRow[] dr = pendingDt.Select(whr);
                    if (dr.Length > 0)
                    {
                        for (int j = 0; j < dr.Length; j++)
                        {
                            double unit_price = Convert.ToDouble(dr[j]["unit_price"].ToString());
                            double box_weight = Convert.ToDouble(dr[j]["unit"].ToString());
                            double cost_unit = Convert.ToDouble(dr[j]["cost_unit"].ToString());
                            bool isWeight = Convert.ToBoolean(dr[j]["weight_calculate"].ToString());
                            double trq_amount = Convert.ToDouble(dr[j]["trq_amount"].ToString());
                            //트레이단가 -> 중량 단가
                            if (!isWeight && cost_unit == 0)
                                isWeight = true;

                            if (!isWeight)
                                unit_price = (unit_price * cost_unit) / box_weight;


                            double custom = Convert.ToDouble(dr[j]["custom"].ToString()) / 100;
                            double tax = Convert.ToDouble(dr[j]["tax"].ToString()) / 100;
                            double incidental_expense = Convert.ToDouble(dr[j]["incidental_expense"].ToString()) / 100;

                            //2023-11-16 대행일 경우 부대비용 강제 3%
                            if (dr[j]["ato_no"].ToString().Contains("dw") || dr[j]["ato_no"].ToString().Contains("DW")
                                || dr[j]["ato_no"].ToString().Contains("hs") || dr[j]["ato_no"].ToString().Contains("HS")
                                || dr[j]["ato_no"].ToString().Contains("od") || dr[j]["ato_no"].ToString().Contains("OD")
                                || dr[j]["ato_no"].ToString().Contains("ad") || dr[j]["ato_no"].ToString().Contains("AD")
                                || dr[j]["ato_no"].ToString().Contains("jd") || dr[j]["ato_no"].ToString().Contains("JD"))
                                incidental_expense = 0.03;


                            double qty = Convert.ToDouble(dr[j]["qty"].ToString());
                            double exchangeRate;
                            if (!double.TryParse(dr[j]["exchange_rate"].ToString(), out exchangeRate))
                                exchangeRate = exchange_rate;
                            double sales_cost;
                            //일반
                            if (trq_amount == 0)
                                sales_cost = unit_price * exchangeRate * (1 + custom + tax + incidental_expense);
                            //trq
                            else
                                sales_cost = (unit_price * exchangeRate * (1 + tax + incidental_expense)) + (trq_amount);



                            DateTime etd;
                            string etd_txt = "";
                            if (DateTime.TryParse(dr[j]["etd2"].ToString(), out etd))
                                etd_txt = etd.ToString("yyyy-MM-dd");

                            productDt.Rows[i]["매출원가"] = sales_cost;
                            productDt.Rows[i]["환율"] = exchangeRate;
                            productDt.Rows[i]["오퍼가"] = unit_price;
                            productDt.Rows[i]["etd"] = etd_txt;

                            break;
                        }
                    }
                }
            }
            dt1 = productDt;
            dt2 = productDt2;
        }
        private void SetTable()
        {
            newDt = new DataTable();

            DataColumn col01 = new DataColumn();
            col01.DataType = System.Type.GetType("System.Double");
            col01.AllowDBNull = false;
            col01.ColumnName = "main_id";
            col01.Caption = "ID";
            col01.DefaultValue = 0;
            newDt.Columns.Add(col01);

            DataColumn col02 = new DataColumn();
            col02.DataType = System.Type.GetType("System.Double");
            col02.AllowDBNull = false;
            col02.ColumnName = "sub_id";
            col02.Caption = "SUB ID";
            col02.DefaultValue = 0;
            newDt.Columns.Add(col02);

            DataColumn col1 = new DataColumn();
            col1.DataType = System.Type.GetType("System.String");
            col1.AllowDBNull = false;
            col1.ColumnName = "category_code";
            col1.Caption = "대분류코드";
            col1.DefaultValue = "";
            newDt.Columns.Add(col1);

            DataColumn col2 = new DataColumn();
            col2.DataType = System.Type.GetType("System.String");
            col2.AllowDBNull = false;
            col2.ColumnName = "category";
            col2.Caption = "대분류";
            col2.DefaultValue = "";
            newDt.Columns.Add(col2);

            DataColumn col3 = new DataColumn();
            col3.DataType = System.Type.GetType("System.String");
            col3.AllowDBNull = false;
            col3.ColumnName = "category2";
            col3.Caption = "대분류2";
            col3.DefaultValue = "";
            newDt.Columns.Add(col3);

            DataColumn col4 = new DataColumn();
            col4.DataType = System.Type.GetType("System.String");
            col4.AllowDBNull = false;
            col4.ColumnName = "category3";
            col4.Caption = "대분류3";
            col4.DefaultValue = "";
            newDt.Columns.Add(col4);

            DataColumn col5 = new DataColumn();
            col5.DataType = System.Type.GetType("System.String");
            col5.AllowDBNull = false;
            col5.ColumnName = "product_code";
            col5.Caption = "품명코드";
            col5.DefaultValue = "";
            newDt.Columns.Add(col5);

            DataColumn col6 = new DataColumn();
            col6.DataType = System.Type.GetType("System.String");
            col6.AllowDBNull = false;
            col6.ColumnName = "product";
            col6.Caption = "품명";
            col6.DefaultValue = "";
            newDt.Columns.Add(col6);

            DataColumn col7 = new DataColumn();
            col7.DataType = System.Type.GetType("System.String");
            col7.AllowDBNull = false;
            col7.ColumnName = "origin_code";
            col7.Caption = "원산지코드";
            col7.DefaultValue = "";
            newDt.Columns.Add(col7);

            DataColumn col8 = new DataColumn();
            col8.DataType = System.Type.GetType("System.String");
            col8.AllowDBNull = false;
            col8.ColumnName = "origin";
            col8.Caption = "원산지";
            col8.DefaultValue = "";
            newDt.Columns.Add(col8);

            DataColumn col9 = new DataColumn();
            col9.DataType = System.Type.GetType("System.String");
            col9.AllowDBNull = false;
            col9.ColumnName = "sizes_code";
            col9.Caption = "규격코드";
            col9.DefaultValue = "";
            newDt.Columns.Add(col9);

            DataColumn col10 = new DataColumn();
            col10.DataType = System.Type.GetType("System.String");
            col10.AllowDBNull = false;
            col10.ColumnName = "sizes";
            col10.Caption = "규격";
            col10.DefaultValue = "";
            newDt.Columns.Add(col10);

            DataColumn col11 = new DataColumn();
            col11.DataType = System.Type.GetType("System.String");
            col11.AllowDBNull = false;
            col11.ColumnName = "sizes2";
            col11.Caption = "규격2";
            col11.DefaultValue = "";
            newDt.Columns.Add(col11);

            DataColumn col12 = new DataColumn();
            col12.DataType = System.Type.GetType("System.String");
            col12.AllowDBNull = false;
            col12.ColumnName = "sizes3";
            col12.Caption = "규격3";
            col12.DefaultValue = "";
            newDt.Columns.Add(col12);

            DataColumn col13 = new DataColumn();
            col13.DataType = System.Type.GetType("System.String");
            col13.AllowDBNull = false;
            col13.ColumnName = "sizes4";
            col13.Caption = "규격4";
            col13.DefaultValue = "";
            newDt.Columns.Add(col13);

            DataColumn col14 = new DataColumn();
            col14.DataType = System.Type.GetType("System.String");
            col14.AllowDBNull = false;
            col14.ColumnName = "unit";
            col14.Caption = "단위";
            col14.DefaultValue = "";
            newDt.Columns.Add(col14);

            DataColumn col15 = new DataColumn();
            col15.DataType = System.Type.GetType("System.String");
            col15.AllowDBNull = false;
            col15.ColumnName = "price_unit";
            col15.Caption = "가격단위";
            col15.DefaultValue = "";
            newDt.Columns.Add(col15);

            DataColumn col16 = new DataColumn();
            col16.DataType = System.Type.GetType("System.String");
            col16.AllowDBNull = false;
            col16.ColumnName = "unit_count";
            col16.Caption = "단위수량";
            col16.DefaultValue = "";
            newDt.Columns.Add(col16);

            DataColumn col162 = new DataColumn();
            col162.DataType = System.Type.GetType("System.Double");
            col162.AllowDBNull = false;
            col162.ColumnName = "bundle_count";
            col162.Caption = "묶음수";
            col162.DefaultValue = 0;
            newDt.Columns.Add(col162);

            DataColumn col17 = new DataColumn();
            col17.DataType = System.Type.GetType("System.String");
            col17.AllowDBNull = false;
            col17.ColumnName = "seaover_unit";
            col17.Caption = "S단위";
            col17.DefaultValue = "";
            newDt.Columns.Add(col17);

            DataColumn col170 = new DataColumn();
            col170.DataType = System.Type.GetType("System.String");
            col170.AllowDBNull = false;
            col170.ColumnName = "box_weight";
            col170.Caption = "중량";
            col170.DefaultValue = "";
            newDt.Columns.Add(col170);

            DataColumn col171 = new DataColumn();
            col171.DataType = System.Type.GetType("System.Double");
            col171.AllowDBNull = false;
            col171.ColumnName = "cost_unit";
            col171.Caption = "트레이";
            col171.DefaultValue = 0;
            newDt.Columns.Add(col171);

            //재고량===========================================
            DataColumn col30 = new DataColumn();
            col30.DataType = System.Type.GetType("System.Double");
            col30.AllowDBNull = false;
            col30.ColumnName = "real_stock";
            col30.Caption = "실재고";
            col30.DefaultValue = 0;
            newDt.Columns.Add(col30);

            DataColumn col3001 = new DataColumn();
            col3001.DataType = System.Type.GetType("System.Double");
            col3001.AllowDBNull = false;
            col3001.ColumnName = "seaover_unpending";
            col3001.Caption = "미통관";
            col3001.DefaultValue = 0;
            newDt.Columns.Add(col3001);

            DataColumn col3002 = new DataColumn();
            col3002.DataType = System.Type.GetType("System.Double");
            col3002.AllowDBNull = false;
            col3002.ColumnName = "seaover_pending";
            col3002.Caption = "통관";
            col3002.DefaultValue = 0;
            newDt.Columns.Add(col3002);

            DataColumn col25 = new DataColumn();
            col25.DataType = System.Type.GetType("System.Double");
            col25.AllowDBNull = false;
            col25.ColumnName = "reserved_stock";
            col25.Caption = "예약수";
            col25.DefaultValue = 0;
            newDt.Columns.Add(col25);

            DataColumn col26 = new DataColumn();
            col26.DataType = System.Type.GetType("System.String");
            col26.AllowDBNull = false;
            col26.ColumnName = "reserved_stock_details";
            col26.Caption = "예약상세";
            col26.DefaultValue = "";
            newDt.Columns.Add(col26);

            DataColumn col301 = new DataColumn();
            col301.DataType = System.Type.GetType("System.Double");
            col301.AllowDBNull = false;
            col301.ColumnName = "shipment_stock";
            col301.Caption = "선적재고";
            col301.DefaultValue = 0;
            newDt.Columns.Add(col301);

            DataColumn col27 = new DataColumn();
            col27.DataType = System.Type.GetType("System.Double");
            col27.AllowDBNull = false;
            col27.ColumnName = "shipment_qty";
            col27.Caption = "선적전";
            col27.DefaultValue = 0;
            newDt.Columns.Add(col27);

            DataColumn col28 = new DataColumn();
            col28.DataType = System.Type.GetType("System.Double");
            col28.AllowDBNull = false;
            col28.ColumnName = "unpending_qty_before";
            col28.Caption = "배송중";
            col28.DefaultValue = 0;
            newDt.Columns.Add(col28);

            DataColumn col29 = new DataColumn();
            col29.DataType = System.Type.GetType("System.Double");
            col29.AllowDBNull = false;
            col29.ColumnName = "unpending_qty_after";
            col29.Caption = "미통관";
            col29.DefaultValue = 0;
            newDt.Columns.Add(col29);

            DataColumn col302 = new DataColumn();
            col302.DataType = System.Type.GetType("System.Double");
            col302.AllowDBNull = false;
            col302.ColumnName = "total_stock";
            col302.Caption = "총 재고";
            col302.DefaultValue = 0;
            newDt.Columns.Add(col302);
            //판매량===========================================
            DataColumn col32 = new DataColumn();
            col32.DataType = System.Type.GetType("System.Double");
            col32.AllowDBNull = false;
            col32.ColumnName = "average_month_sales_count";
            col32.Caption = "月판매";
            col32.DefaultValue = 0;
            newDt.Columns.Add(col32);

            DataColumn col33 = new DataColumn();
            col33.DataType = System.Type.GetType("System.Double");
            col33.AllowDBNull = false;
            col33.ColumnName = "average_day_sales_count";
            col33.Caption = "日판매";
            col33.DefaultValue = 0;
            newDt.Columns.Add(col33);

            DataColumn col34 = new DataColumn();
            col34.DataType = System.Type.GetType("System.Double");
            col34.AllowDBNull = false;
            col34.ColumnName = "average_day_sales_count_double";
            col34.Caption = "일판매";
            col34.DefaultValue = 0;
            newDt.Columns.Add(col34);

            DataColumn col31 = new DataColumn();
            col31.DataType = System.Type.GetType("System.Double");
            col31.AllowDBNull = false;
            col31.ColumnName = "sales_count";
            col31.Caption = "총판매량";
            col31.DefaultValue = 0;
            newDt.Columns.Add(col31);


            DataColumn col311 = new DataColumn();
            col311.DataType = System.Type.GetType("System.Double");
            col311.AllowDBNull = false;
            col311.ColumnName = "excluded_qty";
            col311.Caption = "제외수량";
            col311.DefaultValue = 0;
            newDt.Columns.Add(col311);

            DataColumn col18 = new DataColumn();
            col18.DataType = System.Type.GetType("System.Double");
            col18.AllowDBNull = false;
            col18.ColumnName = "sales_price";
            col18.Caption = "매출단가";
            col18.DefaultValue = 0;
            newDt.Columns.Add(col18);

            DataColumn col222 = new DataColumn();
            col222.DataType = System.Type.GetType("System.String");
            col222.AllowDBNull = false;
            col222.ColumnName = "average_purchase_price_rank";
            col222.Caption = "등수";
            col222.DefaultValue = "";
            newDt.Columns.Add(col222);

            DataColumn col1811 = new DataColumn();
            col1811.DataType = System.Type.GetType("System.String");
            col1811.AllowDBNull = false;
            col1811.ColumnName = "sales_price_updatetime";
            col1811.Caption = "매출단가수정일자";
            col1811.DefaultValue = "";
            newDt.Columns.Add(col1811);

            DataColumn col19 = new DataColumn();
            col19.DataType = System.Type.GetType("System.String");
            col19.AllowDBNull = false;
            col19.ColumnName = "margin";
            col19.Caption = "마진";
            col19.DefaultValue = "0";
            newDt.Columns.Add(col19);

            DataColumn col191 = new DataColumn();
            col191.DataType = System.Type.GetType("System.Double");
            col191.AllowDBNull = false;
            col191.ColumnName = "margin_double";
            col191.Caption = "마진(매출-최저)";
            col191.DefaultValue = "0";
            newDt.Columns.Add(col191);

            DataColumn col21 = new DataColumn();
            col21.DataType = System.Type.GetType("System.Double");
            col21.AllowDBNull = false;
            col21.ColumnName = "purchase_price";
            col21.Caption = "최저단가";
            col21.DefaultValue = 0;
            newDt.Columns.Add(col21);

            DataColumn col22 = new DataColumn();
            col22.DataType = System.Type.GetType("System.Double");
            col22.AllowDBNull = false;
            col22.ColumnName = "average_purchase_price";
            col22.Caption = "일반시세";
            col22.DefaultValue = 0;
            newDt.Columns.Add(col22);

            DataColumn col57 = new DataColumn();
            col57.DataType = System.Type.GetType("System.Double");
            col57.AllowDBNull = false;
            col57.ColumnName = "normal_price_rate";
            col57.Caption = "상위";
            col57.DefaultValue = 0;
            newDt.Columns.Add(col57);

            DataColumn col577 = new DataColumn();
            col577.DataType = System.Type.GetType("System.String");
            col577.AllowDBNull = false;
            col577.ColumnName = "normal_price_rate_txt";
            col577.Caption = "상위";
            col577.DefaultValue = "";
            newDt.Columns.Add(col577);

            DataColumn col201 = new DataColumn();
            col201.DataType = System.Type.GetType("System.Double");
            col201.AllowDBNull = false;
            col201.ColumnName = "sales_cost_price";
            col201.Caption = "S원가";
            col201.DefaultValue = 0;
            newDt.Columns.Add(col201);

            DataColumn col201222 = new DataColumn();
            col201222.DataType = System.Type.GetType("System.String");
            col201222.AllowDBNull = false;
            col201222.ColumnName = "sales_cost_price_tooltip";
            col201222.Caption = "sales_cost_price_tooltip";
            col201222.DefaultValue = "";
            newDt.Columns.Add(col201222);

            DataColumn col2011 = new DataColumn();
            col2011.DataType = System.Type.GetType("System.String");
            col2011.AllowDBNull = false;
            col2011.ColumnName = "sales_cost_price_detail";
            col2011.Caption = "S원가상세";
            col2011.DefaultValue = 0;
            newDt.Columns.Add(col2011);

            DataColumn col202 = new DataColumn();
            col202.DataType = System.Type.GetType("System.Double");
            col202.AllowDBNull = false;
            col202.ColumnName = "pending_cost_price";
            col202.Caption = "선적원가";
            col202.DefaultValue = 0;
            newDt.Columns.Add(col202);

            DataColumn col2022 = new DataColumn();
            col2022.DataType = System.Type.GetType("System.Double");
            col2022.AllowDBNull = false;
            col2022.ColumnName = "average_sales_cost_price1";
            col2022.Caption = "평균원가1";
            col2022.DefaultValue = 0;
            newDt.Columns.Add(col2022);

            DataColumn col2023 = new DataColumn();
            col2023.DataType = System.Type.GetType("System.Double");
            col2023.AllowDBNull = false;
            col2023.ColumnName = "average_sales_cost_price1_margin";
            col2023.Caption = "마진(평균원가1)";
            col2023.DefaultValue = 0;
            newDt.Columns.Add(col2023);

            //3******************
            DataColumn col2033 = new DataColumn();
            col2033.DataType = System.Type.GetType("System.String");
            col2033.AllowDBNull = false;
            col2033.ColumnName = "offer_updatetime";
            col2033.Caption = "오퍼일자";
            col2033.DefaultValue = 0;
            newDt.Columns.Add(col2033);

            DataColumn col2031 = new DataColumn();
            col2031.DataType = System.Type.GetType("System.Double");
            col2031.AllowDBNull = false;
            col2031.ColumnName = "offer_price";
            col2031.Caption = "오퍼가($)";
            col2031.DefaultValue = 0;
            newDt.Columns.Add(col2031);


            DataColumn col2034 = new DataColumn();
            col2034.DataType = System.Type.GetType("System.String");
            col2034.AllowDBNull = false;
            col2034.ColumnName = "offer_company";
            col2034.Caption = "거래처";
            col2034.DefaultValue = "";
            newDt.Columns.Add(col2034);



            DataColumn col172 = new DataColumn();
            col172.DataType = System.Type.GetType("System.Double");
            col172.AllowDBNull = false;
            col172.ColumnName = "custom";
            col172.Caption = "관세";
            col172.DefaultValue = 0;
            newDt.Columns.Add(col172);

            DataColumn col173 = new DataColumn();
            col173.DataType = System.Type.GetType("System.Double");
            col173.AllowDBNull = false;
            col173.ColumnName = "tax";
            col173.Caption = "과세";
            col173.DefaultValue = 0;
            newDt.Columns.Add(col173);

            DataColumn col174 = new DataColumn();
            col174.DataType = System.Type.GetType("System.Double");
            col174.AllowDBNull = false;
            col174.ColumnName = "incidental_expense";
            col174.Caption = "부대비용";
            col174.DefaultValue = 0;
            newDt.Columns.Add(col174);

            DataColumn col1744 = new DataColumn();
            col1744.DataType = System.Type.GetType("System.Double");
            col1744.AllowDBNull = false;
            col1744.ColumnName = "fixed_tariff";
            col1744.Caption = "고지가";
            col1744.DefaultValue = 0;
            newDt.Columns.Add(col1744);


            DataColumn col203 = new DataColumn();
            col203.DataType = System.Type.GetType("System.Double");
            col203.AllowDBNull = false;
            col203.ColumnName = "offer_cost_price";
            col203.Caption = "원가계산";
            col203.DefaultValue = 0;
            newDt.Columns.Add(col203);

            DataColumn col20351 = new DataColumn();
            col20351.DataType = System.Type.GetType("System.Double");
            col20351.AllowDBNull = false;
            col20351.ColumnName = "offer_cost_price_margin";
            col20351.Caption = "마진(%)";
            col20351.DefaultValue = 0;
            newDt.Columns.Add(col20351);

            DataColumn col2035 = new DataColumn();
            col2035.DataType = System.Type.GetType("System.Double");
            col2035.AllowDBNull = false;
            col2035.ColumnName = "order_qty";
            col2035.Caption = "오더수량";
            col2035.DefaultValue = 0;
            newDt.Columns.Add(col2035);

            DataColumn col203512 = new DataColumn();
            col203512.DataType = System.Type.GetType("System.String");
            col203512.AllowDBNull = false;
            col203512.ColumnName = "order_etd";
            col203512.Caption = "ETD";
            col203512.DefaultValue = "";
            newDt.Columns.Add(col203512);

            DataColumn col2036 = new DataColumn();
            col2036.DataType = System.Type.GetType("System.Double");
            col2036.AllowDBNull = false;
            col2036.ColumnName = "total_real_stock";
            col2036.Caption = "수량합계";
            col2036.DefaultValue = 0;
            newDt.Columns.Add(col2036);

            DataColumn col204 = new DataColumn();
            col204.DataType = System.Type.GetType("System.Double");
            col204.AllowDBNull = false;
            col204.ColumnName = "average_sales_cost_price2";
            col204.Caption = "평균원가2";
            col204.DefaultValue = 0;
            newDt.Columns.Add(col204);

            DataColumn col2024 = new DataColumn();
            col2024.DataType = System.Type.GetType("System.Double");
            col2024.AllowDBNull = false;
            col2024.ColumnName = "average_sales_cost_price2_margin";
            col2024.Caption = "마진(오퍼원가)";
            col2024.DefaultValue = 0;
            newDt.Columns.Add(col2024);

            /*DataColumn col23 = new DataColumn();
            col23.DataType = System.Type.GetType("System.String");
            col23.AllowDBNull = false;
            col23.ColumnName = "average_purchase_price_details";
            col23.Caption = "일반시세상세";
            col23.DefaultValue = "";
            newDt.Columns.Add(col23);*/



            DataColumn col35 = new DataColumn();
            col35.DataType = System.Type.GetType("System.String");
            col35.AllowDBNull = false;
            col35.ColumnName = "up";
            col35.Caption = "";
            col35.DefaultValue = "";
            newDt.Columns.Add(col35);

            DataColumn col36 = new DataColumn();
            col36.DataType = System.Type.GetType("System.String");
            col36.AllowDBNull = false;
            col36.ColumnName = "down";
            col36.Caption = "";
            col36.DefaultValue = "";
            newDt.Columns.Add(col36);

            DataColumn col37 = new DataColumn();
            col37.DataType = System.Type.GetType("System.Double");
            col37.AllowDBNull = false;
            col37.ColumnName = "up_lv";
            col37.Caption = "up_lv";
            col37.DefaultValue = 0;
            newDt.Columns.Add(col37);

            DataColumn col38 = new DataColumn();
            col38.DataType = System.Type.GetType("System.Double");
            col38.AllowDBNull = false;
            col38.ColumnName = "down_lv";
            col38.Caption = "down_lv";
            col38.DefaultValue = 0;
            newDt.Columns.Add(col38);

            DataColumn col58 = new DataColumn();
            col58.DataType = System.Type.GetType("System.Double");
            col58.AllowDBNull = false;
            col58.ColumnName = "base_around_month";
            col58.Caption = "기준회전율";
            col58.DefaultValue = 0;
            newDt.Columns.Add(col58);

            DataColumn col388 = new DataColumn();
            col388.DataType = System.Type.GetType("System.Double");
            col388.AllowDBNull = false;
            col388.ColumnName = "month_around";
            col388.Caption = "회전율(선적미포함)";
            col388.DefaultValue = 0;
            newDt.Columns.Add(col388);

            DataColumn col38811 = new DataColumn();
            col38811.DataType = System.Type.GetType("System.Double");
            col38811.AllowDBNull = false;
            col38811.ColumnName = "month_around_in_shipment";
            col38811.Caption = "회전율(선적포함)";
            col38811.DefaultValue = 0;
            newDt.Columns.Add(col38811);

            DataColumn col38812 = new DataColumn();
            col38812.DataType = System.Type.GetType("System.Double");
            col38812.AllowDBNull = false;
            col38812.ColumnName = "month_around_in_shipment2";
            col38812.Caption = "회전율(선적일반영)";
            col38812.DefaultValue = 0;
            newDt.Columns.Add(col38812);

            DataColumn col3881 = new DataColumn();
            col3881.DataType = System.Type.GetType("System.String");
            col3881.AllowDBNull = false;
            col3881.ColumnName = "month_around_1";
            col3881.Caption = "회전율(1개월)";
            col3881.DefaultValue = "0";
            newDt.Columns.Add(col3881);

            DataColumn col3882 = new DataColumn();
            col3881.DataType = System.Type.GetType("System.String");
            col3882.AllowDBNull = false;
            col3882.ColumnName = "month_around_45";
            col3882.Caption = "회전율(45일)";
            col3882.DefaultValue = "0";
            newDt.Columns.Add(col3882);

            DataColumn col3883 = new DataColumn();
            col3883.DataType = System.Type.GetType("System.String");
            col3883.AllowDBNull = false;
            col3883.ColumnName = "month_around_2";
            col3883.Caption = "회전율(2개월)";
            col3883.DefaultValue = "0";
            newDt.Columns.Add(col3883);

            DataColumn col3884 = new DataColumn();
            col3884.DataType = System.Type.GetType("System.String");
            col3884.AllowDBNull = false;
            col3884.ColumnName = "month_around_3";
            col3884.Caption = "회전율(3개월)";
            col3884.DefaultValue = "0";
            newDt.Columns.Add(col3884);

            DataColumn col3885 = new DataColumn();
            col3885.DataType = System.Type.GetType("System.String");
            col3885.AllowDBNull = false;
            col3885.ColumnName = "month_around_6";
            col3885.Caption = "회전율(6개월)";
            col3885.DefaultValue = "0";
            newDt.Columns.Add(col3885);

            DataColumn col3886 = new DataColumn();
            col3886.DataType = System.Type.GetType("System.String");
            col3886.AllowDBNull = false;
            col3886.ColumnName = "month_around_12";
            col3886.Caption = "회전율(12개월)";
            col3886.DefaultValue = "0";
            newDt.Columns.Add(col3886);

            DataColumn col3887 = new DataColumn();
            col3887.DataType = System.Type.GetType("System.String");
            col3887.AllowDBNull = false;
            col3887.ColumnName = "month_around_18";
            col3887.Caption = "회전율(18개월)";
            col3887.DefaultValue = "0";
            newDt.Columns.Add(col3887);


            DataColumn col39 = new DataColumn();
            col39.DataType = System.Type.GetType("System.Double");
            col39.AllowDBNull = false;
            col39.ColumnName = "enable_sales_days";
            col39.Caption = "판매가능일";
            col39.DefaultValue = 0;
            newDt.Columns.Add(col39);

            DataColumn col3888 = new DataColumn();
            col3888.DataType = System.Type.GetType("System.Double");
            col3888.AllowDBNull = false;
            col3888.ColumnName = "month_around2";
            col3888.Caption = "회전율";
            col3888.DefaultValue = 0;
            newDt.Columns.Add(col3888);

            DataColumn col399 = new DataColumn();
            col399.DataType = System.Type.GetType("System.Double");
            col399.AllowDBNull = false;
            col399.ColumnName = "enable_sales_days2";
            col399.Caption = "판매가능일";
            col399.DefaultValue = 0;
            newDt.Columns.Add(col399);

            DataColumn col3889 = new DataColumn();
            col3889.DataType = System.Type.GetType("System.String");
            col3889.AllowDBNull = false;
            col3889.ColumnName = "createdatetime";
            col3889.Caption = "등록일자";
            col3889.DefaultValue = "";
            newDt.Columns.Add(col3889);

            DataColumn col40 = new DataColumn();
            col40.DataType = System.Type.GetType("System.String");
            col40.AllowDBNull = false;
            col40.ColumnName = "exhausted_date";
            col40.Caption = "쇼트일자";
            col40.DefaultValue = "";
            newDt.Columns.Add(col40);

            DataColumn col4000 = new DataColumn();
            col4000.DataType = System.Type.GetType("System.Double");
            col4000.AllowDBNull = false;
            col4000.ColumnName = "exhausted_count";
            col4000.Caption = "쇼트수량";
            col4000.DefaultValue = 0;
            newDt.Columns.Add(col4000);


            DataColumn col40000 = new DataColumn();
            col40000.DataType = System.Type.GetType("System.Double");
            col40000.AllowDBNull = false;
            col40000.ColumnName = "delivery_days";
            col40000.Caption = "배송기간";
            col40000.DefaultValue = "0";
            newDt.Columns.Add(col40000);

            DataColumn col400 = new DataColumn();
            col400.DataType = System.Type.GetType("System.String");
            col400.AllowDBNull = false;
            col400.ColumnName = "contract_date";
            col400.Caption = "추천계약일";
            col400.DefaultValue = "";
            newDt.Columns.Add(col400);

            DataColumn col51 = new DataColumn();
            col51.DataType = System.Type.GetType("System.Double");
            col51.AllowDBNull = false;
            col51.ColumnName = "until_days1";
            col51.Caption = "남은일";
            col51.DefaultValue = 0;
            newDt.Columns.Add(col51);

            DataColumn col50 = new DataColumn();
            col50.DataType = System.Type.GetType("System.String");
            col50.AllowDBNull = false;
            col50.ColumnName = "min_etd";
            col50.Caption = "최소ETD";
            col50.DefaultValue = "";
            newDt.Columns.Add(col50);

            DataColumn col52 = new DataColumn();
            col52.DataType = System.Type.GetType("System.Double");
            col52.AllowDBNull = false;
            col52.ColumnName = "until_days2";
            col52.Caption = "남은일";
            col52.DefaultValue = 0;
            newDt.Columns.Add(col52);

            DataColumn col41 = new DataColumn();
            col41.DataType = System.Type.GetType("System.String");
            col41.AllowDBNull = false;
            col41.ColumnName = "hide_mode";
            col41.Caption = "hide_mode";
            col41.DefaultValue = "";
            newDt.Columns.Add(col41);

            DataColumn col42 = new DataColumn();
            col42.DataType = System.Type.GetType("System.String");
            col42.AllowDBNull = false;
            col42.ColumnName = "hide_details";
            col42.Caption = "hide_details";
            col42.DefaultValue = "";
            newDt.Columns.Add(col42);

            DataColumn col43 = new DataColumn();
            col43.DataType = System.Type.GetType("System.String");
            col43.AllowDBNull = false;
            col43.ColumnName = "hide_until_date";
            col43.Caption = "제외일자";
            col43.DefaultValue = "";
            newDt.Columns.Add(col43);

            DataColumn col44 = new DataColumn();
            col44.DataType = System.Type.GetType("System.String");
            col44.AllowDBNull = false;
            col44.ColumnName = "division";
            col44.Caption = "구분";
            col44.DefaultValue = "";
            newDt.Columns.Add(col44);

            DataColumn col45 = new DataColumn();
            col45.DataType = System.Type.GetType("System.String");
            col45.AllowDBNull = false;
            col45.ColumnName = "manager1";
            col45.Caption = "담당1";
            col45.DefaultValue = "";
            newDt.Columns.Add(col45);

            DataColumn col46 = new DataColumn();
            col46.DataType = System.Type.GetType("System.String");
            col46.AllowDBNull = false;
            col46.ColumnName = "manager2";
            col46.Caption = "담당2";
            col46.DefaultValue = "";
            newDt.Columns.Add(col46);

            DataColumn col47 = new DataColumn();
            col47.DataType = System.Type.GetType("System.String");
            col47.AllowDBNull = false;
            col47.ColumnName = "manager3";
            col47.Caption = "무역담당";
            col47.DefaultValue = "";
            newDt.Columns.Add(col47);

            DataColumn col48 = new DataColumn();
            col48.DataType = System.Type.GetType("System.Boolean");
            col48.AllowDBNull = false;
            col48.ColumnName = "weight_calculate";
            col48.Caption = "중량";
            col48.DefaultValue = false;
            newDt.Columns.Add(col48);

            DataColumn col49 = new DataColumn();
            col49.DataType = System.Type.GetType("System.Boolean");
            col49.AllowDBNull = false;
            col49.ColumnName = "tray_calculate";
            col49.Caption = "트레이";
            col49.DefaultValue = false;
            newDt.Columns.Add(col49);

            DataColumn col55 = new DataColumn();
            col55.DataType = System.Type.GetType("System.Double");
            col55.AllowDBNull = false;
            col55.ColumnName = "production_days";
            col55.Caption = "생산일";
            col55.DefaultValue = 0;
            newDt.Columns.Add(col55);

            DataColumn col56 = new DataColumn();
            col56.DataType = System.Type.GetType("System.Double");
            col56.AllowDBNull = false;
            col56.ColumnName = "purchase_margin";
            col56.Caption = "수입마진";
            col56.DefaultValue = 0;
            newDt.Columns.Add(col56);

            DataColumn col59 = new DataColumn();
            col59.DataType = System.Type.GetType("System.String");
            col59.AllowDBNull = false;
            col59.ColumnName = "merge_product";
            col59.Caption = "병합품목";
            col59.DefaultValue = "";
            newDt.Columns.Add(col59);

            DataColumn col60 = new DataColumn();
            col60.DataType = System.Type.GetType("System.String");
            col60.AllowDBNull = false;
            col60.ColumnName = "sort_hide";
            col60.Caption = "sort_hide";
            col60.DefaultValue = "";
            newDt.Columns.Add(col60);

            DataColumn col61 = new DataColumn();
            col61.DataType = System.Type.GetType("System.DateTime");
            col61.AllowDBNull = false;
            col61.ColumnName = "e_date";
            col61.Caption = "e_date";
            col61.DefaultValue = "1900-01-01";
            newDt.Columns.Add(col61);


            DataColumn col62 = new DataColumn();
            col62.DataType = System.Type.GetType("System.DateTime");
            col62.AllowDBNull = false;
            col62.ColumnName = "c_date";
            col62.Caption = "c_date";
            col62.DefaultValue = "1900-01-01";
            newDt.Columns.Add(col62);

            DataColumn col63 = new DataColumn();
            col63.DataType = System.Type.GetType("System.Double");
            col63.AllowDBNull = false;
            col63.ColumnName = "sizes3_double";
            col63.Caption = "sizes3_double";
            col63.DefaultValue = 0;
            newDt.Columns.Add(col63);
            

        }

        private void GetData()
        {
            cbAllTerm.Checked = false;
            if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
            {
                msgBox.Show(this, "환율값을 확인해주세요.");
                this.Activate();
                return;
            }

            

            //데이터 출력
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            DataTable groupDt = null;
            if (isBookmarkMode)
                groupDt = groupRepository.GetGroup(group_id);

            //실행시간
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();

            GetProduct3(groupDt);

            //사용자설정 저장
            if (dgvProduct.ColumnCount > 0)
            {
                StyleSettingTxt();
                cookie.SaveSalesManagerSetting(styleDic);
            }

            stopwatch1.Stop();
            //msgBox .Show($"{(stopwatch1.ElapsedMilliseconds / 1000).ToString()}");

            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
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

        private void GetProduct(DataTable groupDt = null)
        {
            //업체별시세현황
            CallProductProcedure();
            //재고별시세현황
            CallStockProcedure();
            // 전체 컬럼의 Sorting 기능 차단 
            if (dgvProduct.Columns.Count > 0)
            {
                foreach (DataGridViewColumn item in dgvProduct.Columns)
                    item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //Reset
            /*dgvProduct.Rows.Clear();
            dgvProduct.Columns.Clear();
            dgvProduct.Refresh();*/
            newDt.Rows.Clear();

            //매출기간
            int sales_term = 6;
            switch (cbSaleTerm.Text)
            {
                case "1개월":
                    sales_term = 1;
                    break;
                case "45일":
                    sales_term = 45;
                    break;
                case "2개월":
                    sales_term = 2;
                    break;
                case "3개월":
                    sales_term = 3;
                    break;
                case "6개월":
                    sales_term = 6;
                    break;
                case "12개월":
                    sales_term = 12;
                    break;
                case "18개월":
                    sales_term = 18;
                    break;
                default:
                    sales_term = 6;
                    break;
            }
            //영업일 수
            DateTime sttdate, enddate;
            if (!DateTime.TryParse(txtStockDate.Text, out enddate))
            {
                msgBox.Show(this, "기준일자를 다시 확인해주세요!");
                this.Activate();
                return;
            }
            //매출기간
            if (sales_term == 45)
                sttdate = enddate.AddDays(-sales_term);
            else
                sttdate = enddate.AddMonths(-sales_term);

            //영업일수
            common.GetWorkDay(sttdate, enddate, out workDays);
            //오늘 매출은 반영안되니 기준일자가 오늘이면 영업일 -1
            if (enddate.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                workDays--;

            //일반시세 기간
            DateTime stt_date, end_date;
            if (!DateTime.TryParse(txtSttdate.Text, out stt_date) || !DateTime.TryParse(txtEnddate.Text, out end_date))
            {
                msgBox.Show(this, "일반시세 기간을 확인해주세요.");
                this.Activate();
                return;
            }

            //검색항목
            string div = txtDivision.Text.Trim();
            string category = txtCategory.Text.Trim();
            string product = txtProduct.Text.Trim();
            string origin = txtOrigin.Text.Trim();
            string sizes = txtSizes.Text.Trim();
            string unit = txtUnit.Text.Trim();
            string manager1 = txtManager1.Text.Trim();
            string manager2 = txtManager2.Text.Trim();

            //고급검색===========================================================
            //매입단가 검색
            int sttPrice = 0;
            int endPrice = 0;
            try
            {
                sttPrice = Convert.ToInt32(txtSttPrice.Text.Replace(",", ""));
                endPrice = Convert.ToInt32(txtEndPrice.Text.Replace(",", ""));
            }
            catch
            { }

            //매출단가 비교
            int priceType = 0;
            if (cbSalesPrice.Checked)
            {
                if (cbPriceTypeDropdown.Text == "매출단가 <= 최저단가")
                { priceType = 1; }
                else if (cbPriceTypeDropdown.Text == "매출단가 < 일반시세")
                { priceType = 2; }
                else if (cbPriceTypeDropdown.Text == "매출단가 >= 일반시세")
                { priceType = 3; }
            }
            //매입단가, 매출단가 검색
            bool isPurchasePrice = false;
            bool isSalesPrice = false;
            if (pnAdvancedSearch.Visible)
            {
                isPurchasePrice = cbPurchasePrice.Checked;
                isSalesPrice = cbSalesPrice.Checked;
            }

            //정렬기준
            int sortType = 1;
            switch (cbSortType.Text)
            {
                case "대분류+품명+원산지+규격":
                    sortType = 1;
                    break;
                case "품명+원산지+규격":
                    sortType = 2;
                    break;
                case "원산지+품명+규격":
                    sortType = 3;
                    break;
            }
            //=======================================================================
            //Get data
            DataTable productDt;
            productDt = priceComparisonRepository.GetPriceComparisonDataTable(enddate, Convert.ToInt16(cbCp.Text), category, product, origin, sizes, unit, manager1, manager2, div
                                                                , stt_date, end_date
                                                                , pnAdvancedSearch.Visible
                                                                , isPurchasePrice, sttPrice, endPrice
                                                                , isSalesPrice, priceType, sales_term, sortType, null);

            if (groupDt != null)
            {
                var joinResult = (from p in productDt.AsEnumerable()
                                  join t in groupDt.AsEnumerable()
                                  on p.Field<string>("그룹코드") equals t.Field<string>("group_code")
                                  select new
                                  {
                                      대분류 = p.Field<string>("대분류"),
                                      대분류1 = p.Field<string>("대분류1"),
                                      대분류2 = p.Field<string>("대분류2"),
                                      대분류3 = p.Field<string>("대분류3"),
                                      품명코드 = p.Field<string>("품명코드"),
                                      원산지코드 = p.Field<string>("원산지코드"),
                                      규격코드 = p.Field<string>("규격코드"),
                                      품명 = p.Field<string>("품명"),
                                      규격 = p.Field<string>("규격"),
                                      규격2 = p.Field<string>("규격2"),
                                      규격3 = p.Field<double>("규격3"),
                                      규격4 = p.Field<string>("규격4"),
                                      원산지 = p.Field<string>("원산지"),
                                      단위 = p.Field<string>("단위").Trim(),
                                      가격단위 = p.Field<string>("가격단위"),
                                      단위수량 = p.Field<int>("단위수량"),
                                      묶음수 = p.Field<int>("묶음수"),
                                      SEAOVER단위 = p.Field<double>("SEAOVER단위"),
                                      매출단가 = p.Field<double>("매출단가"),
                                      단가수정일 = p.Field<string>("단가수정일"),
                                      최저단가 = p.Field<double>("최저단가"),
                                      구분 = p.Field<string>("구분"),
                                      계산단위 = p.Field<double>("계산단위"),
                                      일반시세 = p.Field<double>("일반시세"),
                                      일반시세상세 = p.Field<string>("일반시세상세"),
                                      통관재고 = p.Field<double>("통관재고"),
                                      미통관재고 = p.Field<double>("미통관재고"),
                                      예약수 = p.Field<double>("예약수"),
                                      예약상세 = p.Field<string>("예약상세"),
                                      매출수 = p.Field<double>("매출수"),
                                      매출금액 = p.Field<double>("매출금액"),
                                      매출원가 = p.Field<double>("매출원가")
                                  }).ToList();
                productDt = ConvertListToDatatable(joinResult);
                if (productDt != null)
                    productDt.AcceptChanges();
            }

            if (productDt != null && productDt.Rows.Count > 0)
            {
                //대표품목
                DataTable pgDt = productGroupRepository.GetProductGroup(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
                //팬딩내역
                uppDt = customsRepository.GetUnpendingProduct2(product, origin, sizes, unit, false, false);
                //제외품목
                hdDt = hideRepository.GetHideTable(enddate.ToString("yyyy-MM-dd"), "", "", "", "", "", "업체별시세현황");
                //담당자1, 담당자2 전용 품목 DATA
                DataTable mngDt = seaoverRepository.GetProductTapble(category, product, origin, sizes, unit, manager1, manager2, div);
                //무역담당자
                tmDt = purchasePriceRepository.GetTradeManaer(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtTradeManager.Text.Trim());
                //미통관 원가
                DataTable upDt = customsRepository.GetPendingProductByAtono("", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
                //제외매출
                DataTable eDt = productExcludedSalesRepository.GetExcludedSalesAsOne(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"), product, origin, sizes, unit, "", "", "");
                //일반시세 역대현황
                if (cbNormalPrice.Checked)
                    purchaseDt = purchaseRepository.GetPurchaseProductList(DateTime.Now.AddYears(-4).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), product, origin, sizes, unit);

                //최종 데이터테이블 만들기
                OutputProductData2(productDt, mngDt, tmDt, pgDt, upDt, eDt, sttPrice, endPrice, groupDt);
            }

            SetCalculate();
        }
        private void GetProduct2(DataTable groupDt = null)
        {
            //업체별시세현황
            CallProductProcedure();
            //재고별시세현황
            CallStockProcedure();
            // 전체 컬럼의 Sorting 기능 차단 
            if (dgvProduct.Columns.Count > 0)
            {
                foreach (DataGridViewColumn item in dgvProduct.Columns)
                    item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //Reset
            newDt.Rows.Clear();

            //매출기간
            int sales_term = 6;
            switch (cbSaleTerm.Text)
            {
                case "1개월":
                    sales_term = 1;
                    break;
                case "45일":
                    sales_term = 45;
                    break;
                case "2개월":
                    sales_term = 2;
                    break;
                case "3개월":
                    sales_term = 3;
                    break;
                case "6개월":
                    sales_term = 6;
                    break;
                case "12개월":
                    sales_term = 12;
                    break;
                case "18개월":
                    sales_term = 18;
                    break;
                default:
                    sales_term = 6;
                    break;
            }
            //영업일 수
            DateTime sttdate, enddate;
            if (!DateTime.TryParse(txtStockDate.Text, out enddate))
            {
                msgBox.Show(this, "기준일자를 다시 확인해주세요!");
                this.Activate();
                return;
            }
            //매출기간
            if (sales_term == 45)
                sttdate = enddate.AddDays(-sales_term);
            else
                sttdate = enddate.AddMonths(-sales_term);

            //영업일수
            common.GetWorkDay(sttdate, enddate, out workDays);
            //오늘 매출은 반영안되니 기준일자가 오늘이면 영업일 -1
            if (enddate.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                workDays--;

            //일반시세 기간
            DateTime stt_date, end_date;
            if (!DateTime.TryParse(txtSttdate.Text, out stt_date) || !DateTime.TryParse(txtEnddate.Text, out end_date))
            {
                msgBox.Show(this, "일반시세 기간을 확인해주세요.");
                this.Activate();
                return;
            }

            //검색항목
            string div = txtDivision.Text.Trim();
            string category = txtCategory.Text.Trim();
            string product = txtProduct.Text.Trim();
            string origin = txtOrigin.Text.Trim();
            string sizes = txtSizes.Text.Trim();
            string unit = txtUnit.Text.Trim();
            string manager1 = txtManager1.Text.Trim();
            string manager2 = txtManager2.Text.Trim();

            //고급검색===========================================================
            //매입단가 검색
            int sttPrice = 0;
            int endPrice = 0;
            try
            {
                sttPrice = Convert.ToInt32(txtSttPrice.Text.Replace(",", ""));
                endPrice = Convert.ToInt32(txtEndPrice.Text.Replace(",", ""));
            }
            catch
            { }

            //매출단가 비교
            int priceType = 0;
            if (cbSalesPrice.Checked)
            {
                if (cbPriceTypeDropdown.Text == "매출단가 <= 최저단가")
                { priceType = 1; }
                else if (cbPriceTypeDropdown.Text == "매출단가 < 일반시세")
                { priceType = 2; }
                else if (cbPriceTypeDropdown.Text == "매출단가 >= 일반시세")
                { priceType = 3; }
            }
            //매입단가, 매출단가 검색
            bool isPurchasePrice = false;
            bool isSalesPrice = false;
            if (pnAdvancedSearch.Visible)
            {
                isPurchasePrice = cbPurchasePrice.Checked;
                isSalesPrice = cbSalesPrice.Checked;
            }

            //정렬기준
            int sortType = 1;
            switch (cbSortType.Text)
            {
                case "대분류+품명+원산지+규격":
                    sortType = 1;
                    break;
                case "품명+원산지+규격":
                    sortType = 2;
                    break;
                case "원산지+품명+규격":
                    sortType = 3;
                    break;
            }
            //Get data=======================================================================
            DataTable productDt = priceComparisonRepository.GetPriceComparisonDataTable(enddate, Convert.ToInt16(cbCp.Text), "", "", "", "", "", "", "", ""
                                                                , stt_date, end_date
                                                                , pnAdvancedSearch.Visible
                                                                , isPurchasePrice, sttPrice, endPrice
                                                                , isSalesPrice, priceType, sales_term, sortType, null);
            if (productDt != null && productDt.Rows.Count > 0)
            {
                //즐겨찾기일 경우
                if (groupDt != null)
                {
                    var joinResult = (from p in productDt.AsEnumerable()
                                      join t in groupDt.AsEnumerable()
                                      on p.Field<string>("그룹코드") equals t.Field<string>("group_code")
                                      select new
                                      {
                                          대분류 = p.Field<string>("대분류"),
                                          대분류1 = p.Field<string>("대분류1"),
                                          대분류2 = p.Field<string>("대분류2"),
                                          대분류3 = p.Field<string>("대분류3"),
                                          품명코드 = p.Field<string>("품명코드"),
                                          원산지코드 = p.Field<string>("원산지코드"),
                                          규격코드 = p.Field<string>("규격코드"),
                                          품명 = p.Field<string>("품명"),
                                          규격 = p.Field<string>("규격"),
                                          규격2 = p.Field<string>("규격2"),
                                          규격3 = p.Field<double>("규격3"),
                                          규격4 = p.Field<string>("규격4"),
                                          원산지 = p.Field<string>("원산지"),
                                          단위 = p.Field<string>("단위").Trim(),
                                          가격단위 = p.Field<string>("가격단위"),
                                          단위수량 = p.Field<int>("단위수량"),
                                          묶음수 = p.Field<int>("묶음수"),
                                          SEAOVER단위 = p.Field<double>("SEAOVER단위"),
                                          매출단가 = p.Field<double>("매출단가"),
                                          단가수정일 = p.Field<string>("단가수정일"),
                                          최저단가 = p.Field<double>("최저단가"),
                                          구분 = p.Field<string>("구분"),
                                          계산단위 = p.Field<double>("계산단위"),
                                          일반시세 = p.Field<double>("일반시세"),
                                          일반시세상세 = p.Field<string>("일반시세상세"),
                                          통관재고 = p.Field<double>("통관재고"),
                                          미통관재고 = p.Field<double>("미통관재고"),
                                          예약수 = p.Field<double>("예약수"),
                                          예약상세 = p.Field<string>("예약상세"),
                                          매출수 = p.Field<double>("매출수"),
                                          매출금액 = p.Field<double>("매출금액"),
                                          매출원가 = p.Field<double>("매출원가"),
                                          그룹코드 = p.Field<string>("그룹코드"),
                                          main_id = p.Field<string>("main_id"),
                                          sub_id = p.Field<string>("sub_id")
                                      }).ToList();
                    productDt = ConvertListToDatatable(joinResult);
                    if (productDt != null)
                        productDt.AcceptChanges();
                }

                //대표품목
                DataTable pgDt = productGroupRepository.GetProductGroup2("", "", "", "");
                if (pgDt != null && pgDt.Rows.Count > 0)
                {
                    var joinResult = from p in productDt.AsEnumerable()
                                     join t in pgDt.AsEnumerable()
                                     on p.Field<string>("그룹코드") equals t.Field<string>("group_code")
                                     into outer
                                     from t in outer.DefaultIfEmpty()
                                     select new
                                     {
                                         대분류 = p.Field<string>("대분류"),
                                         대분류1 = p.Field<string>("대분류1"),
                                         대분류2 = p.Field<string>("대분류2"),
                                         대분류3 = p.Field<string>("대분류3"),
                                         품명코드 = p.Field<string>("품명코드"),
                                         원산지코드 = p.Field<string>("원산지코드"),
                                         규격코드 = p.Field<string>("규격코드"),
                                         품명 = p.Field<string>("품명"),
                                         규격 = p.Field<string>("규격"),
                                         규격2 = p.Field<string>("규격2"),
                                         규격3 = p.Field<double>("규격3"),
                                         규격4 = p.Field<string>("규격4"),
                                         원산지 = p.Field<string>("원산지"),
                                         단위 = p.Field<string>("단위").Trim(),
                                         가격단위 = p.Field<string>("가격단위"),
                                         단위수량 = p.Field<int>("단위수량"),
                                         묶음수 = p.Field<int>("묶음수"),
                                         SEAOVER단위 = p.Field<double>("SEAOVER단위"),
                                         매출단가 = p.Field<double>("매출단가"),
                                         단가수정일 = p.Field<string>("단가수정일"),
                                         최저단가 = p.Field<double>("최저단가"),
                                         구분 = p.Field<string>("구분"),
                                         계산단위 = p.Field<double>("계산단위"),
                                         일반시세 = p.Field<double>("일반시세"),
                                         일반시세상세 = p.Field<string>("일반시세상세"),
                                         통관재고 = p.Field<double>("통관재고"),
                                         미통관재고 = p.Field<double>("미통관재고"),
                                         예약수 = p.Field<double>("예약수"),
                                         예약상세 = p.Field<string>("예약상세"),
                                         매출수 = p.Field<double>("매출수"),
                                         매출금액 = p.Field<double>("매출금액"),
                                         매출원가 = p.Field<double>("매출원가"),
                                         main_id = (t == null) ? "000000" : t.Field<string>("main_id"),
                                         sub_id = (t == null) ? "000000" : t.Field<string>("sub_id")
                                     };
                    productDt = ConvertListToDatatable(joinResult);
                    if (productDt != null)
                        productDt.AcceptChanges();
                }

                //팬딩내역
                uppDt = customsRepository.GetUnpendingProduct2("", "", "", "", false, false);
                //제외품목
                hdDt = hideRepository.GetHideTable(enddate.ToString("yyyy-MM-dd"), "", "", "", "", "", "업체별시세현황");
                //담당자1, 담당자2 전용 품목 DATA
                DataTable mngDt = seaoverRepository.GetProductTapble(category, product, origin, sizes, unit, manager1, manager2, div);
                //무역담당자
                tmDt = purchasePriceRepository.GetTradeManaer(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtTradeManager.Text.Trim());
                //미통관 원가
                DataTable upDt = customsRepository.GetPendingProductByAtono("", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
                //제외매출
                DataTable eDt = productExcludedSalesRepository.GetExcludedSalesAsOne(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"), product, origin, sizes, unit, "", "", "");
                //일반시세 역대현황
                if (cbNormalPrice.Checked)
                    purchaseDt = purchaseRepository.GetPurchaseProductList(DateTime.Now.AddYears(-4).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), product, origin, sizes, unit);

                //최종 데이터테이블 만들기
                OutputProductData2(productDt, mngDt, tmDt, pgDt, upDt, eDt, sttPrice, endPrice, groupDt);
            }

            SetCalculate();
        }
        private void GetProduct3(DataTable groupDt = null)
        {
            //업체별시세현황
            CallProductProcedure();
            //재고별시세현황
            CallStockProcedure();
            // 전체 컬럼의 Sorting 기능 차단 
            if (dgvProduct.Columns.Count > 0)
            {
                foreach (DataGridViewColumn item in dgvProduct.Columns)
                    item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //Reset
            newDt.Rows.Clear();
            //틀고정
            if (dgvProduct.ColumnCount > 0)
            {
                dgvProduct.Columns["category"].Frozen = false;
                dgvProduct.Columns["product"].Frozen = false;
                dgvProduct.Columns["origin"].Frozen = false;
                dgvProduct.Columns["sizes"].Frozen = false;
                dgvProduct.Columns["unit"].Frozen = false;
            }

            //매출기간
            int sales_term = 6;
            switch (cbSaleTerm.Text)
            {
                case "1개월":
                    sales_term = 1;
                    break;
                case "45일":
                    sales_term = 45;
                    break;
                case "2개월":
                    sales_term = 2;
                    break;
                case "3개월":
                    sales_term = 3;
                    break;
                case "6개월":
                    sales_term = 6;
                    break;
                case "12개월":
                    sales_term = 12;
                    break;
                case "18개월":
                    sales_term = 18;
                    break;
                default:
                    sales_term = 6;
                    break;
            }
            //영업일 수
            DateTime sttdate, enddate;
            if (!DateTime.TryParse(txtStockDate.Text, out enddate))
            {
                msgBox.Show(this, "기준일자를 다시 확인해주세요!");
                this.Activate();
                return;
            }
            //매출기간
            if (sales_term == 45)
                sttdate = enddate.AddDays(-sales_term);
            else
                sttdate = enddate.AddMonths(-sales_term);

            //영업일수
            common.GetWorkDay(sttdate, enddate, out workDays);
            //오늘 매출은 반영안되니 기준일자가 오늘이면 영업일 -1
            if (enddate.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                workDays--;

            //일반시세 기간
            DateTime stt_date, end_date;
            if (!DateTime.TryParse(txtSttdate.Text, out stt_date) || !DateTime.TryParse(txtEnddate.Text, out end_date))
            {
                msgBox.Show(this, "일반시세 기간을 확인해주세요.");
                this.Activate();
                return;
            }

            //검색항목
            string div = txtDivision.Text.Trim();
            string category = txtCategory.Text.Trim();
            string product = txtProduct.Text.Trim();
            string origin = txtOrigin.Text.Trim();
            string sizes = txtSizes.Text.Trim();
            string unit = txtUnit.Text.Trim();
            string manager1 = txtManager1.Text.Trim();
            string manager2 = txtManager2.Text.Trim();

            //고급검색===========================================================
            //매입단가 검색
            int sttPrice = 0;
            int endPrice = 0;
            try
            {
                sttPrice = Convert.ToInt32(txtSttPrice.Text.Replace(",", ""));
                endPrice = Convert.ToInt32(txtEndPrice.Text.Replace(",", ""));
            }
            catch
            { }

            //매출단가 비교
            int priceType = 0;
            if (cbSalesPrice.Checked)
            {
                if (cbPriceTypeDropdown.Text == "매출단가 <= 최저단가")
                { priceType = 1; }
                else if (cbPriceTypeDropdown.Text == "매출단가 < 일반시세")
                { priceType = 2; }
                else if (cbPriceTypeDropdown.Text == "매출단가 >= 일반시세")
                { priceType = 3; }
            }
            //매입단가, 매출단가 검색
            bool isPurchasePrice = false;
            bool isSalesPrice = false;
            if (pnAdvancedSearch.Visible)
            {
                isPurchasePrice = cbPurchasePrice.Checked;
                isSalesPrice = cbSalesPrice.Checked;
            }

            //정렬기준
            int sortType = 1;
            switch (cbSortType.Text)
            {
                case "대분류+품명+원산지+규격":
                    sortType = 1;
                    break;
                case "품명+원산지+규격":
                    sortType = 2;
                    break;
                case "원산지+품명+규격":
                    sortType = 3;
                    break;
            }
            //Get data=======================================================================
            DataTable productDt = priceComparisonRepository.GetPriceComparisonDataTable(enddate, Convert.ToInt16(cbCp.Text), "", "", "", "", "", "", "", ""
                                                                , stt_date, end_date
                                                                , pnAdvancedSearch.Visible
                                                                , isPurchasePrice, sttPrice, endPrice
                                                                , isSalesPrice, priceType, sales_term, sortType, null, cbAllStock.Checked);
            if (productDt != null && productDt.Rows.Count > 0)
            {
                //대표품목
                DataTable pgDt = productGroupRepository.GetProductGroup3("", "", "", "");
                if (pgDt != null && pgDt.Rows.Count > 0)
                {
                    var joinResult = from p in productDt.AsEnumerable()
                                     join t in pgDt.AsEnumerable()
                                     on p.Field<string>("그룹코드") equals t.Field<string>("group_code")
                                     into outer
                                     from t in outer.DefaultIfEmpty()
                                     select new
                                     {
                                         그룹코드 = (t == null) ? p.Field<string>("그룹코드") : t.Field<string>("merge_code"),
                                         품목코드 = p.Field<string>("그룹코드"),
                                         대분류 = p.Field<string>("대분류"),
                                         대분류1 = p.Field<string>("대분류1"),
                                         대분류2 = p.Field<string>("대분류2"),  
                                         대분류3 = p.Field<string>("대분류3"),
                                         품명코드 = p.Field<string>("품명코드"),
                                         원산지코드 = p.Field<string>("원산지코드"),
                                         규격코드 = p.Field<string>("규격코드"),
                                         품명 = p.Field<string>("품명"),
                                         규격 = p.Field<string>("규격"),
                                         규격2 = p.Field<string>("규격2"),
                                         규격3 = p.Field<double>("규격3"),
                                         규격4 = p.Field<string>("규격4"),
                                         원산지 = p.Field<string>("원산지"),
                                         단위 = p.Field<string>("단위").Trim(),
                                         가격단위 = p.Field<string>("가격단위"),
                                         단위수량 = p.Field<int>("단위수량"),
                                         묶음수 = p.Field<int>("묶음수"),
                                         SEAOVER단위 = p.Field<double>("SEAOVER단위"),
                                         매출단가 = p.Field<double>("매출단가"),
                                         단가수정일 = p.Field<string>("단가수정일"),
                                         최저단가 = p.Field<double>("최저단가"),
                                         구분 = p.Field<string>("구분"),
                                         계산단위 = p.Field<double>("계산단위"),
                                         일반시세 = p.Field<double>("일반시세"),
                                         일반시세상세 = p.Field<string>("일반시세상세"),
                                         통관재고 = p.Field<double>("통관재고"),
                                         미통관재고 = p.Field<double>("미통관재고"),
                                         예약수 = p.Field<double>("예약수"),
                                         예약상세 = p.Field<string>("예약상세"),
                                         매출수 = p.Field<double>("매출수"),
                                         매출금액 = p.Field<double>("매출금액"),
                                         매출원가 = p.Field<double>("매출원가"),
                                         main_id = (t == null) ? "000000" : t.Field<string>("main_id"),
                                         sub_id = (t == null) ? "000000" : t.Field<string>("sub_id")
                                     };
                    productDt = ConvertListToDatatable(joinResult);
                    if (productDt != null)
                        productDt.AcceptChanges();
                }

                //즐겨찾기일 경우
                if (groupDt != null && productDt != null && productDt.Rows.Count > 0)
                {
                    //서브품목
                    DataTable subDt = null;
                    DataRow[] subDr = productDt.Select("main_id > 0 AND sub_id < 9999");
                    if (subDr.Length > 0)
                        subDt = subDr.CopyToDataTable();
                    //메인품목
                    DataTable mainDt = null;
                    DataRow[] mainDr = productDt.Select("main_id = 0 OR (main_id > 0 AND sub_id = 9999)");
                    if (mainDr.Length > 0)
                        mainDt = mainDr.CopyToDataTable();

                    //즐겨찾기
                    var joinResult = (from p in mainDt.AsEnumerable()
                                      join t in groupDt.AsEnumerable()
                                      on p.Field<string>("품목코드") equals t.Field<string>("group_code")
                                      select new
                                      {
                                          그룹코드 = p.Field<string>("그룹코드"),
                                          대분류 = p.Field<string>("대분류"),
                                          대분류1 = p.Field<string>("대분류1"),
                                          대분류2 = p.Field<string>("대분류2"),
                                          대분류3 = p.Field<string>("대분류3"),
                                          품명코드 = p.Field<string>("품명코드"),
                                          원산지코드 = p.Field<string>("원산지코드"),
                                          규격코드 = p.Field<string>("규격코드"),
                                          품명 = p.Field<string>("품명"),
                                          규격 = p.Field<string>("규격"),
                                          규격2 = p.Field<string>("규격2"),
                                          규격3 = p.Field<double>("규격3"),
                                          규격4 = p.Field<string>("규격4"),
                                          원산지 = p.Field<string>("원산지"),
                                          단위 = p.Field<string>("단위").Trim(),
                                          가격단위 = p.Field<string>("가격단위"),
                                          단위수량 = p.Field<int>("단위수량"),
                                          묶음수 = p.Field<int>("묶음수"),
                                          SEAOVER단위 = p.Field<double>("SEAOVER단위"),
                                          매출단가 = p.Field<double>("매출단가"),
                                          단가수정일 = p.Field<string>("단가수정일"),
                                          최저단가 = p.Field<double>("최저단가"),
                                          구분 = p.Field<string>("구분"),
                                          계산단위 = p.Field<double>("계산단위"),
                                          일반시세 = p.Field<double>("일반시세"),
                                          일반시세상세 = p.Field<string>("일반시세상세"),
                                          통관재고 = p.Field<double>("통관재고"),
                                          미통관재고 = p.Field<double>("미통관재고"),
                                          예약수 = p.Field<double>("예약수"),
                                          예약상세 = p.Field<string>("예약상세"),
                                          매출수 = p.Field<double>("매출수"),
                                          매출금액 = p.Field<double>("매출금액"),
                                          매출원가 = p.Field<double>("매출원가"),
                                          main_id = p.Field<string>("main_id"),
                                          sub_id = p.Field<string>("sub_id")
                                      }).ToList();
                    productDt = ConvertListToDatatable(joinResult);
                    if (productDt != null)
                    {
                        productDt.Merge(subDt);
                        productDt.AcceptChanges();
                    }
                }

                //팬딩내역
                uppDt = customsRepository.GetUnpendingProduct2("", "", "", "", false, false);
                //제외품목
                hdDt = hideRepository.GetHideTable(enddate.ToString("yyyy-MM-dd"), "", "", "", "", "", "업체별시세현황");
                //담당자1, 담당자2 전용 품목 DATA
                DataTable mngDt = seaoverRepository.GetProductTapble(category, product, origin, sizes, unit, manager1, manager2, div);
                //무역담당자
                tmDt = purchasePriceRepository.GetTradeManaer(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtTradeManager.Text.Trim());
                //미통관 원가
                DataTable upDt = customsRepository.GetPendingProductByAtono("", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
                //제외매출
                DataTable eDt = productExcludedSalesRepository.GetExcludedSalesAsOne(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"), product, origin, sizes, unit, "", "", "");
                //일반시세 역대현황
                if (cbNormalPrice.Checked)
                    purchaseDt = purchaseRepository.GetPurchaseProductList(DateTime.Now.AddYears(-4).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), product, origin, sizes, unit);

                //최종 데이터테이블 만들기
                OutputProductData4(productDt, mngDt, tmDt, pgDt, upDt, eDt, sttPrice, endPrice, groupDt);
            }

            SetCalculate();
        }

        private void SetCalculate()
        {
            int total_index = 0;
            int current_index = 0;

            CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dgvProduct.DataSource];
            currencyManager1.SuspendBinding();
            foreach (DataGridViewRow row in dgvProduct.Rows)
            {
                //평균원가 Tooltip
                row.Cells["average_sales_cost_price1"].ToolTipText = "선적원가+S원가 평균";
                row.Cells["average_sales_cost_price2"].ToolTipText = "평균원가+오퍼원가 평균";
                //일반시세 상위 Tooltip
                row.Cells["normal_price_rate"].ToolTipText = row.Cells["normal_price_rate_txt"].Value.ToString();
                //S원가상세
                row.Cells["sales_cost_price"].ToolTipText = row.Cells["sales_cost_price_tooltip"].Value.ToString();
                if (row.Cells["sales_cost_price_tooltip"].Value.ToString().Contains("확인필요"))
                    row.Cells["sales_cost_price"].Style.ForeColor = Color.Red;

                //소진일자
                if (cbExhaustedDate.Checked)
                    SetData(row);
                //Updown 표시
                UpDownColor(row);
                //서브품목 가리기
                int main_id;
                if (row.Cells["main_id"].Value == null || !int.TryParse(row.Cells["main_id"].Value.ToString(), out main_id))
                    main_id = -1;
                int sub_id;
                if (row.Cells["sub_id"].Value == null || !int.TryParse(row.Cells["sub_id"].Value.ToString(), out sub_id))
                    sub_id = 0;
                //대표, 서브품목
                if (main_id > 0 && sub_id < 9999)
                    row.Visible = false;
                else
                    total_index++;

                if (dgvProduct.CurrentCell != null && dgvProduct.CurrentCell.RowIndex == row.Index)
                    current_index = total_index;

                double real_stock;
                if (row.Cells["real_stock"].Value == null || !double.TryParse(row.Cells["real_stock"].Value.ToString(), out real_stock))
                    real_stock = 0;
                double shipment_stock;
                if (row.Cells["shipment_stock"].Value == null || !double.TryParse(row.Cells["shipment_stock"].Value.ToString(), out shipment_stock))
                    shipment_stock = 0;
                double sales_count;
                if (row.Cells["sales_count"].Value == null || !double.TryParse(row.Cells["sales_count"].Value.ToString(), out sales_count))
                    sales_count = 0;
                if ((real_stock == 0 && shipment_stock == 0) || sales_count == 0)
                {
                    row.Cells["month_around"].Style.ForeColor = Color.Black;
                    row.Cells["month_around_in_shipment"].Style.ForeColor = Color.Black;
                    row.Cells["month_around_in_shipment2"].Style.ForeColor = Color.Black;
                }

            }
            currencyManager1.ResumeBinding();
            //레코드
            txtTotalIndex.Text = total_index.ToString("#,##0");
            txtCurrentIndex.Text = current_index.ToString("#,##0");

            CalculateCostPriceMarginAmount();
        }
        private bool TxtContain(string mainTxt, string containTxt)
        {
            bool isOutput = false;
            if (!string.IsNullOrEmpty(containTxt))
            {
                string[] temps = containTxt.Trim().Split(' ');
                if (temps.Length > 0)
                {
                    foreach (string temp in temps)
                    {
                        if (!string.IsNullOrEmpty(temp))
                        {
                            bool containsIgnoreCase = mainTxt.IndexOf(temp, StringComparison.OrdinalIgnoreCase) >= 0;
                            if (containsIgnoreCase)
                                return true;
                        }
                    }
                }
                return false;
            }
            else
                isOutput = true;
            return isOutput;
        }

        private void OutputProductData2(DataTable productDt, DataTable mngDt, DataTable tmDt, DataTable pgDt, DataTable upDt, DataTable eDt, int sttPrice, int endPrice, DataTable groupDt = null)
        {
            //대표품목 출력여부 Dic
            List<string> mergeMainList = new List<string>();
            //설정일자
            DateTime endDate = endDate = dtpEnddate.Value; ;
            //기타
            DataRow main_row;
            string whr;
            //검색항목
            string category = txtCategory.Text.Trim();
            string product = txtProduct.Text.Trim();
            string origin = txtOrigin.Text.Trim();
            string sizes = txtSizes.Text.Trim();
            string unit = txtUnit.Text.Trim();
            string manager1 = txtManager1.Text.Trim();
            string manager2 = txtManager2.Text.Trim();
            string manager3 = txtTradeManager.Text.Trim();
            string division = txtDivision.Text.Trim();
            //newDt setting
            for (int i = 0; i < productDt.Rows.Count; i++)
            {
                bool isOutput = true;
                DataRow row = productDt.Rows[i];
                //단위
                string price_unit = row["가격단위"].ToString();
                string unit_count = row["단위수량"].ToString();
                string bundle_count = row["묶음수"].ToString();
                if (string.IsNullOrEmpty(unit_count))
                    unit_count = "0";
                string seaover_unit = row["SEAOVER단위"].ToString();
                if (string.IsNullOrEmpty(seaover_unit))
                    seaover_unit = "0";
                //main_id, sub_id
                int main_id = Convert.ToInt32(row["main_id"].ToString());
                int sub_id = Convert.ToInt32(row["sub_id"].ToString());
                //담당자1,2===========================================================================================
                string sales_manager1 = "";
                string sales_manager2 = "";
                if (isOutput && mngDt.Rows.Count > 0)
                {
                    whr = "대분류1 = '" + row["대분류"].ToString() + "'"
                        + " AND 품명 = '" + row["품명"].ToString() + "'"
                        + " AND 원산지 = '" + row["원산지"].ToString() + "'"
                        + " AND 규격 = '" + row["규격"].ToString() + "'"
                        + " AND 단위 = '" + row["단위"].ToString() + "'"
                        + " AND 가격단위 = '" + row["가격단위"].ToString() + "'"
                        + " AND 단위수량 = " + unit_count
                        + " AND SEAOVER단위 = '" + row["SEAOVER단위"].ToString() + "'";

                    DataRow[] dtRow = null;
                    dtRow = mngDt.Select(whr);

                    if (dtRow.Length > 0)
                    {
                        sales_manager1 = dtRow[0]["담당자1"].ToString();
                        sales_manager2 = dtRow[0]["담당자2"].ToString();
                    }
                }
                //무역담당자
                whr = "product = '" + row["품명"].ToString() + "'"
                                            + " AND origin = '" + row["원산지"].ToString() + "'"
                                            + " AND sizes = '" + row["규격"].ToString() + "'"
                                            + " AND (unit = '" + row["단위"].ToString() + "'"
                                            + " OR unit = '" + row["SEAOVER단위"].ToString() + "')";
                DataRow[] tmRow = tmDt.Select(whr);
                string trade_manager3 = "";
                if (tmRow.Length > 0)
                {
                    for (int j = 0; j < tmRow.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(tmRow[j]["manager"].ToString().Trim()))
                        {
                            trade_manager3 = tmRow[j]["manager"].ToString().Trim();
                            break;
                        }
                    }
                }

                //출력
                //if (isOutput)
                {
                    //재고수
                    double seaover_unpending = Convert.ToDouble(row["미통관재고"].ToString());
                    double seaover_pending = Convert.ToDouble(row["통관재고"].ToString());
                    //예약수
                    double reserved_stock = Convert.ToDouble(row["예약수"].ToString());
                    double real_stock = seaover_unpending + seaover_pending - reserved_stock;
                    
                    // 가용재고 = 재고수 - 예약수
                    double sales_cost_price = Convert.ToDouble(row["매출원가"].ToString());         //매출원가
                    double sales_price = Convert.ToDouble(row["매출단가"].ToString());              //매출단가
                    double purchase_price = Convert.ToDouble(row["최저단가"].ToString());           //매입단가
                    double average_purchase_price = Convert.ToDouble(row["일반시세"].ToString());   //일반시세
                    //국가별 배송기간====================================================================================
                    int delivery_day = 15;
                    whr = "country_name = '" + row["원산지"].ToString() + "'";
                    DataRow[] deliRow = deliDt.Select(whr);
                    if (deliRow.Length > 0)
                        delivery_day = Convert.ToInt32(deliRow[0]["delivery_days"].ToString());
                    //무역담당자, 트레이=================================================================================
                    double cost_unit = 0;
                    double tax = 0;
                    double custom = 0;
                    double incidental_expense = 0;
                    double fixed_tariff = 0;
                    double offer_price = 0;
                    string offer_updatetime = "";
                    string offer_company = "";
                    double production_days = 20;
                    double purchase_margin = 5;
                    double base_around_month = 3;
                    bool weight_calculate = true;
                    bool tray_calculate = false;
                    
                    if (!string.IsNullOrEmpty(txtTradeManager.Text) && tmDt.Rows.Count == 0)
                        isOutput = false;
                    else if (tmDt.Rows.Count > 0)
                    {
                        //무역담당자

                        //범준과장님 요청(2023-05-28) 특이케이스로 단위랑 씨오버단위 둘중 하나만 맞으면 가져오기
                        //자숙골뱅이 40/60 <- 단위1, 단위수량 10, seaover단위 10  <---------- 이게 kg라 아토 품목단위랑 그냥단위 비교해져서 안가져나와짐 
                        //그래서 일단 seaover단위랑 단위 둘중에 하나라도 맞으면 가져오게함 또 분명 나오지않아도 되는거 나올거라고 수정해야될 순간이 옴 그때 참고!!
                        /*if (row["품명"].ToString().Equals("참소라살(피뿔고동살)") && row["원산지"].ToString().Equals("멕시코"))
                        {
                            whr = "product = '" + row["품명"].ToString() + "'"
                                              + " AND origin = '" + row["원산지"].ToString() + "'"
                                              + " AND sizes = '" + row["규격"].ToString() + "'"
                                              + " AND (unit = '" + row["단위"].ToString() + "'"
                                              + " OR unit = '" + row["SEAOVER단위"].ToString() + "')";

                        }
                        else if (row["가격단위"].ToString().Contains("묶") || row["가격단위"].ToString().Contains("팩"))
                        {
                            whr = "product = '" + row["품명"].ToString() + "'"
                                            + " AND origin = '" + row["원산지"].ToString() + "'"
                                            + " AND sizes = '" + row["규격"].ToString() + "'"
                                            + " AND (unit = '" + row["단위"].ToString() + "'"
                                            + " OR unit = '" + row["SEAOVER단위"].ToString() + "')";
                        }
                        else
                        {
                            whr = "product = '" + row["품명"].ToString() + "'"
                                            + " AND origin = '" + row["원산지"].ToString() + "'"
                                            + " AND sizes = '" + row["규격"].ToString() + "'"
                                            + " AND unit = '" + row["단위"].ToString() + "'";
                        }*/

                        
                        //기타 부대비용
                        if (row["가격단위"].ToString().Contains("팩"))
                        {
                            whr = "product = '" + row["품명"].ToString() + "'"
                                        + " AND origin = '" + row["원산지"].ToString() + "'"
                                        + " AND sizes = '" + row["규격"].ToString() + "'"
                                        + " AND (unit = '" + row["단위"].ToString() + "'"
                                        + " OR unit2 =  '" + row["SEAOVER단위"].ToString() + "'"
                                        + " OR unit2 =  '" + row["단위"].ToString() + "')";
                        }
                        else
                        {
                            whr = "product = '" + row["품명"].ToString() + "'"
                                    + " AND origin = '" + row["원산지"].ToString() + "'"
                                    + " AND sizes = '" + row["규격"].ToString() + "'"
                                    + " AND (unit = '" + row["단위"].ToString() + "'"
                                    + " OR unit2 = '" + row["SEAOVER단위"].ToString() + "')";
                        }
                        tmRow = tmDt.Select(whr);
                        if (tmRow.Length > 0)
                        {
                            //관세
                            if (!double.TryParse(tmRow[0]["custom"].ToString(), out custom))
                                custom = 0;
                            custom = custom / 100;
                            //과세
                            if (!double.TryParse(tmRow[0]["tax"].ToString(), out tax))
                                tax = 0;
                            tax = tax / 100;
                            //부대비용
                            if (!double.TryParse(tmRow[0]["incidental_expense"].ToString(), out incidental_expense))
                                incidental_expense = 0;
                            incidental_expense = incidental_expense / 100;
                            //고지가
                            if (!double.TryParse(tmRow[0]["fixed_tariff"].ToString(), out fixed_tariff))
                                fixed_tariff = 0;
                            fixed_tariff /= 1000;
                            //생산일
                            if (!double.TryParse(tmRow[0]["production_days"].ToString(), out production_days))
                                production_days = 0;
                            //수입마진
                            if (!double.TryParse(tmRow[0]["purchase_margin"].ToString(), out purchase_margin))
                                purchase_margin = 0;

                            //계산방식
                            weight_calculate = Convert.ToBoolean(tmRow[0]["weight_calculate"].ToString());
                            tray_calculate = Convert.ToBoolean(tmRow[0]["tray_calculate"].ToString());

                            //트레이
                            if (!double.TryParse(tmRow[0]["cost_unit"].ToString(), out cost_unit))
                                cost_unit = 0;
                            //기준회전율
                            base_around_month = Convert.ToDouble(tmRow[0]["base_around_month"].ToString());

                            if (string.IsNullOrEmpty(trade_manager3) && !string.IsNullOrEmpty(txtTradeManager.Text))
                                isOutput = false;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(txtTradeManager.Text))
                                isOutput = false;
                        }

                    }
                    //최근 오퍼내역===========================================================================
                    DataRow[] copRow = null;
                    if (isOutput && copDt != null && copDt.Rows.Count > 0)
                    {
                        if (row["가격단위"].ToString().Contains("팩"))
                        {
                            whr = "product = '" + row["품명"].ToString() + "'"
                                        + " AND origin = '" + row["원산지"].ToString() + "'"
                                        + " AND sizes = '" + row["규격"].ToString() + "'"
                                        + " AND (unit = '" + row["단위"].ToString() + "'"
                                        + " OR unit2 = '" + row["단위"].ToString() + "')";
                        }
                        else
                        {
                            whr = "product = '" + row["품명"].ToString() + "'"
                                    + " AND origin = '" + row["원산지"].ToString() + "'"
                                    + " AND sizes = '" + row["규격"].ToString() + "'"
                                    + " AND (unit = '" + row["단위"].ToString() + "'"
                                    + " OR unit2 = '" + row["SEAOVER단위"].ToString() + "')";
                        }
                        copRow = copDt.Select(whr);
                        if (copRow.Length > 0)
                        {
                            offer_price = Convert.ToDouble(copRow[0]["purchase_price"].ToString());
                            cost_unit = Convert.ToDouble(copRow[0]["cost_unit"].ToString());
                            //원가 등록일
                            offer_updatetime = Convert.ToDateTime(copRow[0]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                            //거래처
                            offer_company = copRow[0]["company"].ToString();

                            if (tmRow == null || tmRow.Length == 0)
                            {
                                weight_calculate = Convert.ToBoolean(copRow[0]["weight_calculate"].ToString());
                                tray_calculate = Convert.ToBoolean(copRow[0]["tray_calculate"].ToString());
                            }

                            /*//관세
                            if (!double.TryParse(copRow[0]["custom"].ToString(), out custom))
                                custom = 0;
                            custom = custom / 100;
                            //과세
                            if (!double.TryParse(copRow[0]["tax"].ToString(), out tax))
                                tax = 0;
                            tax = tax / 100;
                            //부대비용
                            if (!double.TryParse(copRow[0]["incidental_expense"].ToString(), out incidental_expense))
                                incidental_expense = 0;
                            incidental_expense = incidental_expense / 100;
                            //계산방식
                            weight_calculate = Convert.ToBoolean(copRow[0]["weight_calculate"].ToString());
                            tray_calculate = Convert.ToBoolean(copRow[0]["tray_calculate"].ToString());*/
                        }
                    }
                    //팬딩내역====================================================================================
                    double shipment_qty = 0;
                    double unpending_qty_before = 0;
                    double unpending_qty_after = 0;

                    double total_shipment_qty = 0;
                    double total_unpending_qty_before = 0;
                    double total_unpending_qty_after = 0;

                    double shipment_cost_price = 0;
                    DataRow[] uppRow = null;
                    if (isOutput && uppDt.Rows.Count > 0)
                    {
                        whr = "product = '" + row["품명"].ToString() + "'"
                            + " AND origin = '" + row["원산지"].ToString() + "'"
                            + " AND sizes = '" + row["규격"].ToString() + "'"
                            + " AND box_weight = '" + row["SEAOVER단위"].ToString() + "'";
                        uppRow = uppDt.Select(whr);
                        if (uppRow.Length > 0)
                        {
                            double total_cost_price = 0;
                            double total_qty = 0;
                            for (int j = 0; j < uppRow.Length; j++)
                            {
                                string bl_no = uppRow[j]["bl_no"].ToString();
                                string warehousing_date = uppRow[j]["warehousing_date"].ToString();
                                //초기화
                                shipment_qty = 0;
                                unpending_qty_before = 0;
                                unpending_qty_after = 0;

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

                                //선적원가
                                double cost_price;
                                if (!double.TryParse(uppRow[j]["cost_price"].ToString(), out cost_price))
                                    cost_price = 0;
                                if (string.IsNullOrEmpty(warehousing_date) || cost_price > 0)
                                {
                                    double box_weight;
                                    if (uppRow[j]["box_weight"] == null || !double.TryParse(uppRow[j]["box_weight"].ToString(), out box_weight))
                                        box_weight = 1;
                                    double tray;
                                    if (uppRow[j]["cost_unit"] == null || !double.TryParse(uppRow[j]["cost_unit"].ToString(), out tray))
                                        tray = 1;
                                    //단위 <-> 트레이
                                    bool isWeight = Convert.ToBoolean(uppRow[j]["weight_calculate"].ToString());
                                    if (!isWeight)
                                        box_weight = tray;
                                    //원가계산
                                    double unit_price;
                                    if (!double.TryParse(uppRow[j]["unit_price"].ToString(), out unit_price))
                                        unit_price = 0;


                                    if (unit_price > 0)
                                    {
                                        // shipment_cost_price = (unit_price * exchange_rate) * (1 + custom + tax + incidental_expense) * box_weight;
                                        shipment_cost_price = unit_price * exchange_rate * box_weight;

                                        if (custom > 0)
                                            shipment_cost_price *= (custom + 1);
                                        if (tax > 0)
                                            shipment_cost_price *= (tax + 1);
                                        if (incidental_expense > 0)
                                            shipment_cost_price *= (incidental_expense + 1);

                                    }
                                    else
                                        shipment_cost_price = cost_price;

                                    //동원 + 2% or 2.5%
                                    if (uppRow[j]["ato_no"].ToString().Contains("dw") || uppRow[j]["ato_no"].ToString().Contains("DW")
                                        || uppRow[j]["ato_no"].ToString().Contains("hs") || uppRow[j]["ato_no"].ToString().Contains("HS")
                                        || uppRow[j]["ato_no"].ToString().Contains("od") || uppRow[j]["ato_no"].ToString().Contains("OD")
                                        || uppRow[j]["ato_no"].ToString().Contains("ad") || uppRow[j]["ato_no"].ToString().Contains("AD"))
                                        shipment_cost_price = shipment_cost_price * 1.025;
                                    else if (uppRow[j]["ato_no"].ToString().Contains("jd") || uppRow[j]["ato_no"].ToString().Contains("JD"))
                                        shipment_cost_price = shipment_cost_price * 1.02;

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

                                    //평균
                                    if (unpending_qty_after == 0)
                                    {
                                        total_cost_price += shipment_cost_price * (shipment_qty + unpending_qty_before);
                                        total_qty += shipment_qty + unpending_qty_before;
                                    }
                                }
                            }
                            shipment_cost_price = total_cost_price / total_qty;
                            if (double.IsNaN(shipment_cost_price))
                                shipment_cost_price = 0;
                        }
                    }
                    //즐겨찾기 선택일 경우=========================================================================================================
                    double enable_days2 = 0;
                    double month_around2 = 0;
                    string createdatetime = "";
                    if (groupDt != null && groupDt.Rows.Count > 0)
                    {
                        whr = "product = '" + row["품명"].ToString() + "'"
                            + " AND origin = '" + row["원산지"].ToString() + "'"
                            + " AND sizes = '" + row["규격"].ToString() + "'"
                            + " AND unit = '" + row["SEAOVER단위"].ToString() + "'";
                        DataRow[] grRow = groupDt.Select(whr);
                        if (grRow.Length > 0)
                        {
                            if (grRow[0]["enable_days"] == null || !double.TryParse(grRow[0]["enable_days"].ToString(), out enable_days2))
                                enable_days2 = 0;

                            /*if (grRow[0]["month_around"] == null || !double.TryParse(grRow[0]["month_around"].ToString(), out month_around2))
                                month_around2 = 0;*/

                            DateTime tmp = DateTime.Now;
                            if (grRow[0]["createdatetime"] == null || !DateTime.TryParse(grRow[0]["createdatetime"].ToString(), out tmp))
                                createdatetime = "";
                            else
                                createdatetime = tmp.ToString("yyyy-MM-dd");

                            //날짜에 맞게 판매가능일, 회전율 변환
                            if (!string.IsNullOrEmpty(createdatetime))
                            {
                                int wDays;
                                common.GetWorkDay(tmp, Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), out wDays);
                                wDays--;

                                enable_days2 -= wDays;
                                month_around2 = enable_days2 / 21;
                            }
                            if (enable_days2 >= 9999)
                                enable_days2 = 9999;
                            if (month_around2 >= 9999)
                                month_around2 = 9999;
                        }
                    }
                    //매출제외 수량===========================================================================
                    double excluded_qty = 0;
                    DataRow[] eRow = null;
                    if (isOutput && eDt != null && eDt.Rows.Count > 0)
                    {
                        whr = "product = '" + row["품명"].ToString() + "'"
                                + " AND origin = '" + row["원산지"].ToString() + "'"
                                + " AND sizes = '" + row["규격"].ToString() + "'"
                                + " AND unit = '" + row["단위"].ToString() + "'"
                                + " AND price_unit = '" + row["가격단위"].ToString() + "'"
                                + " AND unit_count = '" + row["단위수량"].ToString() + "'"
                                + " AND seaover_unit = '" + row["SEAOVER단위"].ToString() + "'";

                        eRow = eDt.Select(whr);
                        if (eRow.Length > 0)
                            excluded_qty = Convert.ToDouble(eRow[0]["sale_qty"].ToString());
                    }


                    //판매량
                    double salesCnt = Convert.ToDouble(row["매출수"].ToString());
                    double real_salesCnt = salesCnt - excluded_qty;
                    if (real_salesCnt < 0)
                        real_salesCnt = 0;
                    // 평균 판매수(일, 월)
                    double avg_day_sales = real_salesCnt / workDays;
                    double avg_month_sales = avg_day_sales * 21;
                    //제외품목=================================================================
                    DataRow[] hdRow = null;
                    bool isHide = false;
                    if (isOutput && hdDt.Rows.Count > 0)
                    {
                        whr = "category = '" + row["대분류1"].ToString() + "'"
                             + " AND product = '" + row["품명"].ToString() + "'"
                             + " AND origin = '" + row["원산지"].ToString() + "'"
                             + " AND sizes = '" + row["규격"].ToString() + "'"
                             + " AND seaover_unit = '" + row["SEAOVER단위"].ToString() + "'";
                        hdRow = hdDt.Select(whr);

                        if (hdRow.Length > 0)
                            isHide = true;
                    }


                    //일반시세 역대현황=====================================================
                    double normal_price_rate = 100;
                    double normal_price = Convert.ToDouble(row["일반시세"].ToString());
                    double maxPrice = normal_price, minPrice = 0;
                    int upCnt = 0, downCnt = 0;
                    if (normal_price > 0 && cbNormalPrice.Checked && purchaseDt.Rows.Count > 0)
                    {
                        whr = "품명 = '" + row["품명"].ToString() + "'"
                            + " AND 원산지 = '" + row["원산지"].ToString() + "'"
                            + " AND 규격 = '" + row["규격"].ToString() + "'"
                            + " AND 단위 = '" + row["SEAOVER단위"].ToString() + "'"
                            + " AND 단가 > 0";

                        DataRow[] dtRow = null;
                        dtRow = purchaseDt.Select(whr);
                        if (dtRow.Length > 0)
                        {
                            for (int j = 0; j < dtRow.Length; j++)
                            {
                                double temp_price = Convert.ToDouble(dtRow[j]["단가"].ToString());
                                if (normal_price > temp_price)
                                {
                                    minPrice = temp_price;
                                    upCnt++;
                                }
                                else
                                {
                                    maxPrice = temp_price;
                                    downCnt++;
                                }
                            }
                            normal_price_rate = (double)upCnt / ((double)upCnt + (double)downCnt) * 100;
                        }
                    }
                    //검색항목================================================================================================
                    //서브 품목은 어차피 가릴거라 제외
                    if (main_id > 0 && sub_id > 0)
                        isOutput = true;
                    else if (sub_id == 0)
                    {
                        if (isOutput)
                            isOutput = TxtContain(row["대분류1"].ToString(), category);
                        if (isOutput)
                            isOutput = TxtContain(row["품명"].ToString(), product);
                        if (isOutput)
                            isOutput = TxtContain(row["원산지"].ToString(), origin);
                        if (isOutput)
                            isOutput = TxtContain(row["규격"].ToString(), sizes);
                        if (isOutput)
                            isOutput = TxtContain(row["단위"].ToString(), unit);
                        if (isOutput)
                            isOutput = TxtContain(sales_manager1, manager1);
                        if (isOutput)
                            isOutput = TxtContain(sales_manager2, manager2);
                        if (isOutput)
                            isOutput = TxtContain(row["구분"].ToString(), division);
                        if (isOutput)
                            isOutput = TxtContain(trade_manager3, manager3);
                    }

                    //데이터 매칭================================================================================================
                    if (isOutput)
                    {
                        DataRow newRow = newDt.NewRow();
                        //Row 추가
                        newRow["category_code"] = row["대분류"].ToString();
                        newRow["category"] = row["대분류1"].ToString();
                        newRow["category2"] = row["대분류2"].ToString();
                        newRow["category3"] = row["대분류3"].ToString();

                        newRow["product_code"] = row["품명코드"].ToString();
                        newRow["origin_code"] = row["원산지코드"].ToString();
                        newRow["sizes_code"] = row["규격코드"].ToString();

                        newRow["product"] = row["품명"].ToString();
                        newRow["origin"] = row["원산지"].ToString();
                        newRow["sizes"] = row["규격"].ToString();
                        newRow["sizes2"] = row["규격2"].ToString();
                        newRow["sizes3"] = row["규격3"].ToString();
                        newRow["sizes4"] = row["규격4"].ToString();
                        newRow["unit"] = row["단위"].ToString().Trim();

                        newRow["price_unit"] = price_unit;
                        newRow["unit_count"] = unit_count;
                        newRow["bundle_count"] = bundle_count;
                        newRow["seaover_unit"] = seaover_unit;

                        newRow["delivery_days"] = delivery_day;

                        newRow["cost_unit"] = cost_unit;
                        newRow["custom"] = custom * 100;
                        newRow["tax"] = tax * 100;
                        newRow["incidental_expense"] = incidental_expense * 100;
                        newRow["fixed_tariff"] = fixed_tariff * 1000;

                        //즐겨찾기
                        newRow["enable_sales_days2"] = enable_days2;
                        newRow["month_around2"] = month_around2;
                        newRow["createdatetime"] = createdatetime;

                        //예약상세
                        //dgv.Rows[n].Cells["reserved_stock_detail"].Value = row["예약상세"].ToString();
                        newRow["reserved_stock_details"] = "";
                        newRow["reserved_stock"] = reserved_stock.ToString("#,##0");

                        newRow["seaover_unpending"] = seaover_unpending.ToString("#,##0");
                        newRow["seaover_pending"] = seaover_pending.ToString("#,##0");

                        newRow["shipment_qty"] = total_shipment_qty.ToString("#,##0");
                        newRow["unpending_qty_before"] = total_unpending_qty_before.ToString("#,##0");
                        newRow["unpending_qty_after"] = total_unpending_qty_after.ToString("#,##0");

                        newRow["real_stock"] = (seaover_unpending + seaover_pending).ToString("#,##0");
                        newRow["shipment_stock"] = (seaover_unpending + seaover_pending + total_shipment_qty + total_unpending_qty_before).ToString("#,##0");

                        newRow["purchase_price"] = purchase_price.ToString("#,##0");
                        newRow["sales_price"] = sales_price.ToString("#,##0");
                        DateTime sales_price_updatetime;
                        if (DateTime.TryParse(row["단가수정일"].ToString(), out sales_price_updatetime))
                            newRow["sales_price_updatetime"] = sales_price_updatetime.ToString("yyyy-MM-dd");

                        //원가 / 단위수량
                        double unitCount;
                        if (!double.TryParse(unit_count, out unitCount))
                            unitCount = 1;
                        if (unitCount <= 0)
                            unitCount = 1;
                        sales_cost_price = sales_cost_price / unitCount;
                        shipment_cost_price = shipment_cost_price / unitCount;
                        //씨오버 매출원가
                        newRow["sales_cost_price"] = sales_cost_price;
                        //선적 원가
                        newRow["pending_cost_price"] = shipment_cost_price;

                        //최근 오퍼가 매출원가
                        newRow["offer_price"] = offer_price;
                        newRow["offer_updatetime"] = offer_updatetime;
                        newRow["offer_company"] = offer_company;
                        newRow["weight_calculate"] = weight_calculate;
                        newRow["tray_calculate"] = tray_calculate;

                        newRow["sales_count"] = salesCnt.ToString("#,##0");
                        newRow["excluded_qty"] = excluded_qty.ToString("#,##0");
                        newRow["average_day_sales_count"] = avg_day_sales.ToString("#,##0");
                        newRow["average_day_sales_count_double"] = avg_day_sales.ToString("#,##0.00");
                        newRow["average_month_sales_count"] = avg_month_sales.ToString("#,##0");
                        /*newRow["enable_sales_days"] = enable_days.ToString("#,##0");
                        newRow["month_around"] = month_around;*/

                        newRow["division"] = row["구분"].ToString();
                        newRow["manager1"] = sales_manager1;
                        newRow["manager2"] = sales_manager2;
                        newRow["manager3"] = trade_manager3;

                        newRow["production_days"] = production_days;
                        newRow["purchase_margin"] = purchase_margin;
                        newRow["base_around_month"] = base_around_month;

                        //일반시세
                        newRow["average_purchase_price"] = Convert.ToDouble(row["일반시세"].ToString()).ToString("#,##0");

                        //일반시세 역대현황
                        newRow["normal_price_rate"] = normal_price_rate.ToString("#,##0.0");
                        newRow["normal_price_rate_txt"] = (upCnt + downCnt).ToString("#,##0") + "개 중 " + upCnt.ToString("#,##0") + "위"
                            + "\n" + minPrice.ToString("#,##0") + " ~ " + maxPrice.ToString("#,##0") + " (상위" + normal_price_rate.ToString("#,##0.0") + "%)";
                        //제외품목================================================================================================
                        if (hdRow != null && hdRow.Length > 0)
                        {
                            newRow["hide_mode"] = hdRow[0]["hide_mode"];
                            newRow["hide_details"] = hdRow[0]["hide_details"];
                            DateTime tmpDt;
                            if (DateTime.TryParse(hdRow[0]["until_date"].ToString(), out tmpDt))
                            {
                                if (endDate <= tmpDt)
                                    newRow["hide_until_date"] = tmpDt.ToString("yyyy-MM-dd");
                            }
                        }
                        //병합처리================================================================================================
                        newRow["main_id"] = main_id;
                        newRow["sub_id"] = sub_id;
                        //메인품목은 한번더 등록
                        if (main_id > 0 && sub_id == 0)
                        {
                            string main_code = row["품명"].ToString()
                                + "^" + row["원산지"].ToString()
                                + "^" + row["규격"].ToString()
                                + "^" + row["단위"].ToString()
                                + "^" + row["가격단위"].ToString()
                                + "^" + row["단위수량"].ToString()
                                + "^" + row["seaover단위"].ToString();
                            //대표행 추가
                            if (!mergeMainList.Contains(main_code))
                            {
                                main_row = newDt.NewRow();

                                main_row["main_id"] = main_id;
                                main_row["sub_id"] = 9999;

                                main_row["category_code"] = newRow["category_code"].ToString();
                                main_row["category"] = newRow["category"].ToString();
                                main_row["category2"] = newRow["category2"].ToString();
                                main_row["category3"] = newRow["category3"].ToString();
                                main_row["product_code"] = newRow["product_code"].ToString();
                                main_row["origin_code"] = newRow["origin_code"].ToString();
                                main_row["sizes_code"] = newRow["sizes_code"].ToString();
                                main_row["product"] = row["품명"].ToString();
                                main_row["origin"] = row["원산지"].ToString();
                                main_row["sizes"] = row["규격"].ToString();
                                main_row["sizes2"] = newRow["sizes2"].ToString();
                                main_row["sizes3"] = newRow["sizes3"].ToString();
                                main_row["sizes4"] = newRow["sizes4"].ToString();

                                main_row["delivery_days"] = newRow["delivery_days"].ToString();

                                main_row["unit"] = row["단위"].ToString().Trim();
                                main_row["price_unit"] = row["가격단위"].ToString();
                                main_row["unit_count"] = row["단위수량"].ToString();
                                main_row["bundle_count"] = newRow["bundle_count"].ToString();
                                main_row["seaover_unit"] = row["seaover단위"].ToString();
                                main_row["cost_unit"] = newRow["cost_unit"].ToString();
                                main_row["weight_calculate"] = newRow["weight_calculate"].ToString();
                                main_row["tray_calculate"] = newRow["tray_calculate"].ToString();


                                main_row["tax"] = newRow["tax"].ToString();
                                main_row["custom"] = newRow["custom"].ToString();
                                main_row["incidental_expense"] = newRow["incidental_expense"].ToString();
                                main_row["fixed_tariff"] = newRow["fixed_tariff"].ToString();

                                main_row["offer_price"] = newRow["offer_price"].ToString();
                                main_row["offer_updatetime"] = newRow["offer_updatetime"].ToString();
                                main_row["offer_company"] = newRow["offer_company"].ToString();

                                main_row["sales_price"] = newRow["sales_price"].ToString();
                                main_row["sales_price_updatetime"] = newRow["sales_price_updatetime"].ToString();
                                main_row["purchase_price"] = newRow["purchase_price"].ToString();
                                main_row["average_purchase_price"] = newRow["average_purchase_price"].ToString();
                                main_row["margin"] = newRow["margin"].ToString();
                                main_row["margin_double"] = newRow["margin_double"].ToString();

                                main_row["sales_cost_price"] = newRow["sales_cost_price"].ToString();
                                main_row["pending_cost_price"] = newRow["pending_cost_price"].ToString();
                                main_row["offer_cost_price"] = newRow["offer_cost_price"].ToString();
                                main_row["average_sales_cost_price1"] = newRow["average_sales_cost_price1"].ToString();
                                main_row["average_sales_cost_price2"] = newRow["average_sales_cost_price2"].ToString();

                                main_row["division"] = newRow["division"].ToString();
                                main_row["manager1"] = newRow["manager1"].ToString();
                                main_row["manager2"] = newRow["manager2"].ToString();
                                main_row["manager3"] = newRow["manager3"].ToString();

                                main_row["production_days"] = newRow["production_days"].ToString();
                                main_row["purchase_margin"] = newRow["purchase_margin"].ToString();
                                main_row["base_around_month"] = newRow["base_around_month"].ToString();

                                //서브품목리스트
                                string merge_product = "";
                                whr = "main_id = '" + main_id + "'";
                                DataRow[] subRow = pgDt.Select(whr);
                                if (subRow.Length > 0)
                                {
                                    for (int j = 0; j < subRow.Length; j++)
                                    {
                                        merge_product += "\n" + subRow[j]["product"].ToString()
                                                        + "^" + subRow[j]["origin"].ToString()
                                                        + "^" + subRow[j]["sizes"].ToString()
                                                        + "^" + subRow[j]["unit"].ToString()
                                                        + "^" + subRow[j]["price_unit"].ToString()
                                                        + "^" + subRow[j]["unit_count"].ToString()
                                                        + "^" + subRow[j]["seaover_unit"].ToString();
                                    }
                                }
                                main_row["merge_product"] = merge_product;
                                //메인품목 등록
                                newDt.Rows.Add(main_row);
                                mergeMainList.Add(main_code);
                            }
                        }
                        newDt.Rows.Add(newRow);
                    }
                }
            }
            //데이터 출력
            dgvProduct.DataSource = MergeProduct2(newDt);
            //Header, Cell style
            SetHeaderStyle();

            dgvProduct.EndEdit();
            dgvProduct.Refresh();
        }


        private void OutputProductData3(DataTable productDt, DataTable mngDt, DataTable tmDt, DataTable pgDt, DataTable upDt, DataTable eDt, int sttPrice, int endPrice, DataTable groupDt = null)
        {
            if (productDt != null && productDt.Rows.Count > 0)
            {
                string whr;
                //대표품목 출력여부 Dic
                List<string> mergeMainList = new List<string>();
                DataTable resultDt = new DataTable();
                //검색항목
                string category = txtCategory.Text.Trim();
                string product = txtProduct.Text.Trim();
                string origin = txtOrigin.Text.Trim();
                string sizes = txtSizes.Text.Trim();
                string unit = txtUnit.Text.Trim();
                string manager1 = txtManager1.Text.Trim();
                string manager2 = txtManager2.Text.Trim();
                string manager3 = txtTradeManager.Text.Trim();
                string division = txtDivision.Text.Trim();

                if (string.IsNullOrEmpty(category)
                    && string.IsNullOrEmpty(product)
                    && string.IsNullOrEmpty(origin)
                    && string.IsNullOrEmpty(sizes)
                    && string.IsNullOrEmpty(unit)
                    && string.IsNullOrEmpty(division))
                {
                    whr = "1 = 1";
                }
                else
                {
                    whr = "(sub_id < 9999 AND sub_id > 0) OR ( 1=1 ";
                    if (!string.IsNullOrEmpty(category))
                        whr += $"\n {whereSql("대분류1", category)}";
                    if (!string.IsNullOrEmpty(product))
                        whr += $"\n {whereSql("품명", product)}";
                    if (!string.IsNullOrEmpty(origin))
                        whr += $"\n {whereSql("원산지", origin)}";
                    if (!string.IsNullOrEmpty(sizes))
                        whr += $"\n {whereSql("규격", sizes)}";
                    if (!string.IsNullOrEmpty(unit))
                        whr += $"\n {whereSql("단위", unit)}";
                    if (!string.IsNullOrEmpty(division))
                        whr += $"\n {whereSql("구분", division)}";
                    whr += $"\n )                                        ";
                }    

                DataRow[] resultDr = productDt.Select(whr);
                if (resultDr.Length > 0)
                    productDt = resultDr.CopyToDataTable();

                if (productDt != null && productDt.Rows.Count > 0)
                {
                    foreach (DataRow row in productDt.Rows)
                    {
                        // 1.담당자1,2
                        string sales_manager1 = "";
                        string sales_manager2 = "";
                        if (mngDt.Rows.Count > 0)
                        {
                            whr = "대분류1 = '" + row["대분류"].ToString() + "'"
                                + " AND 품명 = '" + row["품명"].ToString() + "'"
                                + " AND 원산지 = '" + row["원산지"].ToString() + "'"
                                + " AND 규격 = '" + row["규격"].ToString() + "'"
                                + " AND 단위 = '" + row["단위"].ToString() + "'"
                                + " AND 가격단위 = '" + row["가격단위"].ToString() + "'"
                                + " AND 단위수량 = " + row["단위수량"].ToString()
                                + " AND SEAOVER단위 = '" + row["SEAOVER단위"].ToString() + "'";

                            DataRow[] dtRow = null;
                            dtRow = mngDt.Select(whr);
                            if (dtRow.Length > 0)
                            {
                                sales_manager1 = dtRow[0]["담당자1"].ToString();
                                sales_manager2 = dtRow[0]["담당자2"].ToString();
                            }
                        }
                        // 2.무역담당자
                        whr = "product = '" + row["품명"].ToString() + "'"
                                                    + " AND origin = '" + row["원산지"].ToString() + "'"
                                                    + " AND sizes = '" + row["규격"].ToString() + "'"
                                                    + " AND (unit = '" + row["단위"].ToString() + "'"
                                                    + " OR unit = '" + row["SEAOVER단위"].ToString() + "')";
                        DataRow[] tmRow = tmDt.Select(whr);
                        string trade_manager3 = "";
                        if (tmRow.Length > 0)
                        {
                            for (int j = 0; j < tmRow.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tmRow[j]["manager"].ToString().Trim()))
                                {
                                    trade_manager3 = tmRow[j]["manager"].ToString().Trim();
                                    break;
                                }
                            }
                        }

                        // 3.검색항목 필터
                        int main_id = Convert.ToInt32(row["main_id"].ToString());
                        int sub_id = Convert.ToInt32(row["sub_id"].ToString());

                        bool isOutput = true;
                        if (main_id == 0 || (main_id > 0 && sub_id == 9999))
                        {
                            if (isOutput)
                                isOutput = TxtContain(row["대분류1"].ToString(), category);
                            if (isOutput)
                                isOutput = TxtContain(row["품명"].ToString(), product);
                            if (isOutput)
                                isOutput = TxtContain(row["원산지"].ToString(), origin);
                            if (isOutput)
                                isOutput = TxtContain(row["규격"].ToString(), sizes);
                            if (isOutput)
                                isOutput = TxtContain(row["단위"].ToString(), unit);
                            if (isOutput)
                                isOutput = TxtContain(sales_manager1, manager1);
                            if (isOutput)
                                isOutput = TxtContain(sales_manager2, manager2);
                            if (isOutput)
                                isOutput = TxtContain(row["구분"].ToString(), division);
                            if (isOutput)
                                isOutput = TxtContain(trade_manager3, manager3);
                        }

                        if (isOutput)
                        {
                            //품목정보
                            string price_unit = row["가격단위"].ToString();
                            string unit_count = row["단위수량"].ToString();
                            string bundle_count = row["묶음수"].ToString();
                            if (string.IsNullOrEmpty(unit_count))
                                unit_count = "0";
                            string seaover_unit = row["SEAOVER단위"].ToString();
                            if (string.IsNullOrEmpty(seaover_unit))
                                seaover_unit = "0";
                            //재고수
                            double seaover_unpending = Convert.ToDouble(row["미통관재고"].ToString());
                            double seaover_pending = Convert.ToDouble(row["통관재고"].ToString());
                            //예약수
                            double reserved_stock = Convert.ToDouble(row["예약수"].ToString());
                            double real_stock = seaover_unpending + seaover_pending - reserved_stock;

                            // 가용재고 = 재고수 - 예약수
                            double sales_cost_price = Convert.ToDouble(row["매출원가"].ToString());         //매출원가
                            double sales_price = Convert.ToDouble(row["매출단가"].ToString());              //매출단가
                            double purchase_price = Convert.ToDouble(row["최저단가"].ToString());           //매입단가
                            double average_purchase_price = Convert.ToDouble(row["일반시세"].ToString());   //일반시세

                            // 4.기타 부대비용
                            double cost_unit = 0;
                            double tax = 0;
                            double custom = 0;
                            double incidental_expense = 0;
                            double fixed_tariff = 0;
                            double offer_price = 0;
                            string offer_updatetime = "";
                            string offer_company = "";
                            double production_days = 20;
                            double purchase_margin = 5;
                            double base_around_month = 3;
                            bool weight_calculate = true;
                            bool tray_calculate = false;
                            if (row["가격단위"].ToString().Contains("팩"))
                            {
                                whr = "product = '" + row["품명"].ToString() + "'"
                                            + " AND origin = '" + row["원산지"].ToString() + "'"
                                            + " AND sizes = '" + row["규격"].ToString() + "'"
                                            + " AND (unit = '" + row["단위"].ToString() + "'"
                                            + " OR unit2 =  '" + row["SEAOVER단위"].ToString() + "'"
                                            + " OR unit2 =  '" + row["단위"].ToString() + "')";
                            }
                            else
                            {
                                whr = "product = '" + row["품명"].ToString() + "'"
                                        + " AND origin = '" + row["원산지"].ToString() + "'"
                                        + " AND sizes = '" + row["규격"].ToString() + "'"
                                        + " AND (unit = '" + row["단위"].ToString() + "'"
                                        + " OR unit2 = '" + row["SEAOVER단위"].ToString() + "')";
                            }
                            tmRow = tmDt.Select(whr);
                            if (tmRow.Length > 0)
                            {
                                //관세
                                if (!double.TryParse(tmRow[0]["custom"].ToString(), out custom))
                                    custom = 0;
                                custom = custom / 100;
                                //과세
                                if (!double.TryParse(tmRow[0]["tax"].ToString(), out tax))
                                    tax = 0;
                                tax = tax / 100;
                                //부대비용
                                if (!double.TryParse(tmRow[0]["incidental_expense"].ToString(), out incidental_expense))
                                    incidental_expense = 0;
                                incidental_expense = incidental_expense / 100;
                                //고지가
                                if (!double.TryParse(tmRow[0]["fixed_tariff"].ToString(), out fixed_tariff))
                                    fixed_tariff = 0;
                                fixed_tariff /= 1000;
                                //생산일
                                if (!double.TryParse(tmRow[0]["production_days"].ToString(), out production_days))
                                    production_days = 0;
                                //수입마진
                                if (!double.TryParse(tmRow[0]["purchase_margin"].ToString(), out purchase_margin))
                                    purchase_margin = 0;

                                //계산방식
                                weight_calculate = Convert.ToBoolean(tmRow[0]["weight_calculate"].ToString());
                                tray_calculate = Convert.ToBoolean(tmRow[0]["tray_calculate"].ToString());

                                //트레이
                                if (!double.TryParse(tmRow[0]["cost_unit"].ToString(), out cost_unit))
                                    cost_unit = 0;
                                //기준회전율
                                base_around_month = Convert.ToDouble(tmRow[0]["base_around_month"].ToString());
                            }

                            // 5.국가별 배송기간
                            int delivery_day = 15;
                            whr = "country_name = '" + row["원산지"].ToString() + "'";
                            DataRow[] deliRow = deliDt.Select(whr);
                            if (deliRow.Length > 0)
                                delivery_day = Convert.ToInt32(deliRow[0]["delivery_days"].ToString());

                            // 6.최근 오퍼내역
                            DataRow[] copRow = null;
                            if (copDt != null && copDt.Rows.Count > 0)
                            {
                                if (row["가격단위"].ToString().Contains("팩"))
                                {
                                    whr = "product = '" + row["품명"].ToString() + "'"
                                                + " AND origin = '" + row["원산지"].ToString() + "'"
                                                + " AND sizes = '" + row["규격"].ToString() + "'"
                                                + " AND (unit = '" + row["단위"].ToString() + "'"
                                                + " OR unit2 = '" + row["단위"].ToString() + "')";
                                }
                                else
                                {
                                    whr = "product = '" + row["품명"].ToString() + "'"
                                            + " AND origin = '" + row["원산지"].ToString() + "'"
                                            + " AND sizes = '" + row["규격"].ToString() + "'"
                                            + " AND (unit = '" + row["단위"].ToString() + "'"
                                            + " OR unit2 = '" + row["SEAOVER단위"].ToString() + "')";
                                }
                                copRow = copDt.Select(whr);
                                if (copRow.Length > 0)
                                {
                                    offer_price = Convert.ToDouble(copRow[0]["purchase_price"].ToString());
                                    cost_unit = Convert.ToDouble(copRow[0]["cost_unit"].ToString());
                                    //원가 등록일
                                    offer_updatetime = Convert.ToDateTime(copRow[0]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                                    //거래처
                                    offer_company = copRow[0]["company"].ToString();

                                    if (tmRow == null || tmRow.Length == 0)
                                    {
                                        weight_calculate = Convert.ToBoolean(copRow[0]["weight_calculate"].ToString());
                                        tray_calculate = Convert.ToBoolean(copRow[0]["tray_calculate"].ToString());
                                    }
                                }
                            }

                            // 7.팬딩내역
                            double shipment_qty = 0;
                            double unpending_qty_before = 0;
                            double unpending_qty_after = 0;

                            double total_shipment_qty = 0;
                            double total_unpending_qty_before = 0;
                            double total_unpending_qty_after = 0;

                            double shipment_cost_price = 0;
                            DataRow[] uppRow = null;
                            if (uppDt.Rows.Count > 0)
                            {
                                whr = "product = '" + row["품명"].ToString() + "'"
                                    + " AND origin = '" + row["원산지"].ToString() + "'"
                                    + " AND sizes = '" + row["규격"].ToString() + "'"
                                    + " AND box_weight = '" + row["SEAOVER단위"].ToString() + "'";
                                uppRow = uppDt.Select(whr);
                                if (uppRow.Length > 0)
                                {
                                    double total_cost_price = 0;
                                    double total_qty = 0;
                                    for (int j = 0; j < uppRow.Length; j++)
                                    {
                                        string bl_no = uppRow[j]["bl_no"].ToString();
                                        string warehousing_date = uppRow[j]["warehousing_date"].ToString();
                                        //초기화
                                        shipment_qty = 0;
                                        unpending_qty_before = 0;
                                        unpending_qty_after = 0;

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

                                        //선적원가
                                        double cost_price;
                                        if (!double.TryParse(uppRow[j]["cost_price"].ToString(), out cost_price))
                                            cost_price = 0;
                                        if (string.IsNullOrEmpty(warehousing_date) || cost_price > 0)
                                        {
                                            double box_weight;
                                            if (uppRow[j]["box_weight"] == null || !double.TryParse(uppRow[j]["box_weight"].ToString(), out box_weight))
                                                box_weight = 1;
                                            double tray;
                                            if (uppRow[j]["cost_unit"] == null || !double.TryParse(uppRow[j]["cost_unit"].ToString(), out tray))
                                                tray = 1;
                                            //단위 <-> 트레이
                                            bool isWeight = Convert.ToBoolean(uppRow[j]["weight_calculate"].ToString());
                                            if (!isWeight)
                                                box_weight = tray;
                                            //원가계산
                                            double unit_price;
                                            if (!double.TryParse(uppRow[j]["unit_price"].ToString(), out unit_price))
                                                unit_price = 0;


                                            if (unit_price > 0)
                                            {
                                                // shipment_cost_price = (unit_price * exchange_rate) * (1 + custom + tax + incidental_expense) * box_weight;
                                                shipment_cost_price = unit_price * exchange_rate * box_weight;

                                                if (custom > 0)
                                                    shipment_cost_price *= (custom + 1);
                                                if (tax > 0)
                                                    shipment_cost_price *= (tax + 1);
                                                if (incidental_expense > 0)
                                                    shipment_cost_price *= (incidental_expense + 1);

                                            }
                                            else
                                                shipment_cost_price = cost_price;

                                            //동원 + 2% or 2.5%
                                            if (uppRow[j]["ato_no"].ToString().Contains("dw") || uppRow[j]["ato_no"].ToString().Contains("DW")
                                                || uppRow[j]["ato_no"].ToString().Contains("hs") || uppRow[j]["ato_no"].ToString().Contains("HS")
                                                || uppRow[j]["ato_no"].ToString().Contains("od") || uppRow[j]["ato_no"].ToString().Contains("OD")
                                                || uppRow[j]["ato_no"].ToString().Contains("ad") || uppRow[j]["ato_no"].ToString().Contains("AD"))
                                                shipment_cost_price = shipment_cost_price * 1.025;
                                            else if (uppRow[j]["ato_no"].ToString().Contains("jd") || uppRow[j]["ato_no"].ToString().Contains("JD"))
                                                shipment_cost_price = shipment_cost_price * 1.02;

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

                                            //평균
                                            if (unpending_qty_after == 0)
                                            {
                                                total_cost_price += shipment_cost_price * (shipment_qty + unpending_qty_before);
                                                total_qty += shipment_qty + unpending_qty_before;
                                            }
                                        }
                                    }
                                    shipment_cost_price = total_cost_price / total_qty;
                                    if (double.IsNaN(shipment_cost_price))
                                        shipment_cost_price = 0;
                                }
                            }

                            // 8.즐겨찾기 선택일 경우
                            double enable_days2 = 0;
                            double month_around2 = 0;
                            string createdatetime = "";
                            if (groupDt != null && groupDt.Rows.Count > 0)
                            {
                                whr = "product = '" + row["품명"].ToString() + "'"
                                    + " AND origin = '" + row["원산지"].ToString() + "'"
                                    + " AND sizes = '" + row["규격"].ToString() + "'"
                                    + " AND unit = '" + row["SEAOVER단위"].ToString() + "'";
                                DataRow[] grRow = groupDt.Select(whr);
                                if (grRow.Length > 0)
                                {
                                    if (grRow[0]["enable_days"] == null || !double.TryParse(grRow[0]["enable_days"].ToString(), out enable_days2))
                                        enable_days2 = 0;

                                    DateTime tmp = DateTime.Now;
                                    if (grRow[0]["createdatetime"] == null || !DateTime.TryParse(grRow[0]["createdatetime"].ToString(), out tmp))
                                        createdatetime = "";
                                    else
                                        createdatetime = tmp.ToString("yyyy-MM-dd");

                                    //날짜에 맞게 판매가능일, 회전율 변환
                                    if (!string.IsNullOrEmpty(createdatetime))
                                    {
                                        int wDays;
                                        common.GetWorkDay(tmp, Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), out wDays);
                                        wDays--;

                                        enable_days2 -= wDays;
                                        month_around2 = enable_days2 / 21;
                                    }
                                    if (enable_days2 >= 9999)
                                        enable_days2 = 9999;
                                    if (month_around2 >= 9999)
                                        month_around2 = 9999;
                                }
                            }

                            // 9.매출제외 / 판매량 // 월 평균 판매량
                            //매출제외
                            double excluded_qty = 0;
                            DataRow[] eRow = null;
                            if (eDt != null && eDt.Rows.Count > 0)
                            {
                                whr = "product = '" + row["품명"].ToString() + "'"
                                        + " AND origin = '" + row["원산지"].ToString() + "'"
                                        + " AND sizes = '" + row["규격"].ToString() + "'"
                                        + " AND unit = '" + row["단위"].ToString() + "'"
                                        + " AND price_unit = '" + row["가격단위"].ToString() + "'"
                                        + " AND unit_count = '" + row["단위수량"].ToString() + "'"
                                        + " AND seaover_unit = '" + row["SEAOVER단위"].ToString() + "'";

                                eRow = eDt.Select(whr);
                                if (eRow.Length > 0)
                                    excluded_qty = Convert.ToDouble(eRow[0]["sale_qty"].ToString());
                            }
                            //판매량
                            double salesCnt = Convert.ToDouble(row["매출수"].ToString());
                            double real_salesCnt = salesCnt - excluded_qty;
                            if (real_salesCnt < 0)
                                real_salesCnt = 0;
                            // 평균 판매수(일, 월)
                            double avg_day_sales = real_salesCnt / workDays;
                            double avg_month_sales = avg_day_sales * 21;

                            // 10.제외품목                        
                            /*bool isHide = false;
                            if (hdDt.Rows.Count > 0)
                            {
                                whr = "category = '" + row["대분류1"].ToString() + "'"
                                     + " AND product = '" + row["품명"].ToString() + "'"
                                     + " AND origin = '" + row["원산지"].ToString() + "'"
                                     + " AND sizes = '" + row["규격"].ToString() + "'"
                                     + " AND seaover_unit = '" + row["SEAOVER단위"].ToString() + "'";
                                hdRow = hdDt.Select(whr);

                                if (hdRow.Length > 0)
                                    isHide = true;
                            }*/

                            // 11.일반시세 역대현황
                            double normal_price_rate = 100;
                            double normal_price = Convert.ToDouble(row["일반시세"].ToString());
                            double maxPrice = normal_price, minPrice = 0;
                            int upCnt = 0, downCnt = 0;
                            if (normal_price > 0 && cbNormalPrice.Checked && purchaseDt.Rows.Count > 0)
                            {
                                whr = "품명 = '" + row["품명"].ToString() + "'"
                                    + " AND 원산지 = '" + row["원산지"].ToString() + "'"
                                    + " AND 규격 = '" + row["규격"].ToString() + "'"
                                    + " AND 단위 = '" + row["SEAOVER단위"].ToString() + "'"
                                    + " AND 단가 > 0";

                                DataRow[] dtRow = null;
                                dtRow = purchaseDt.Select(whr);
                                if (dtRow.Length > 0)
                                {
                                    for (int j = 0; j < dtRow.Length; j++)
                                    {
                                        double temp_price = Convert.ToDouble(dtRow[j]["단가"].ToString());
                                        if (normal_price > temp_price)
                                        {
                                            minPrice = temp_price;
                                            upCnt++;
                                        }
                                        else
                                        {
                                            maxPrice = temp_price;
                                            downCnt++;
                                        }
                                    }
                                    normal_price_rate = (double)upCnt / ((double)upCnt + (double)downCnt) * 100;
                                }
                            }


                            //데이터 매칭================================================================================================
                            DataRow newRow = newDt.NewRow();
                            //Row 추가
                            newRow["category_code"] = row["대분류"].ToString();
                            newRow["category"] = row["대분류1"].ToString();
                            newRow["category2"] = row["대분류2"].ToString();
                            newRow["category3"] = row["대분류3"].ToString();

                            newRow["product_code"] = row["품명코드"].ToString();
                            newRow["origin_code"] = row["원산지코드"].ToString();
                            newRow["sizes_code"] = row["규격코드"].ToString();

                            newRow["product"] = row["품명"].ToString();
                            newRow["origin"] = row["원산지"].ToString();
                            newRow["sizes"] = row["규격"].ToString();
                            newRow["sizes2"] = row["규격2"].ToString();
                            newRow["sizes3"] = row["규격3"].ToString();
                            newRow["sizes4"] = row["규격4"].ToString();
                            newRow["unit"] = row["단위"].ToString().Trim();

                            newRow["price_unit"] = price_unit;
                            newRow["unit_count"] = unit_count;
                            newRow["bundle_count"] = bundle_count;
                            newRow["seaover_unit"] = seaover_unit;

                            newRow["delivery_days"] = delivery_day;

                            newRow["cost_unit"] = cost_unit;
                            newRow["custom"] = custom * 100;
                            newRow["tax"] = tax * 100;
                            newRow["incidental_expense"] = incidental_expense * 100;
                            newRow["fixed_tariff"] = fixed_tariff * 1000;

                            //즐겨찾기
                            newRow["enable_sales_days2"] = enable_days2;
                            newRow["month_around2"] = month_around2;
                            newRow["createdatetime"] = createdatetime;

                            //예약상세
                            //dgv.Rows[n].Cells["reserved_stock_detail"].Value = row["예약상세"].ToString();
                            newRow["reserved_stock_details"] = "";
                            newRow["reserved_stock"] = reserved_stock.ToString("#,##0");

                            newRow["seaover_unpending"] = seaover_unpending.ToString("#,##0");
                            newRow["seaover_pending"] = seaover_pending.ToString("#,##0");

                            newRow["shipment_qty"] = total_shipment_qty.ToString("#,##0");
                            newRow["unpending_qty_before"] = total_unpending_qty_before.ToString("#,##0");
                            newRow["unpending_qty_after"] = total_unpending_qty_after.ToString("#,##0");

                            newRow["real_stock"] = (seaover_unpending + seaover_pending).ToString("#,##0");
                            newRow["shipment_stock"] = (seaover_unpending + seaover_pending + total_shipment_qty + total_unpending_qty_before).ToString("#,##0");

                            newRow["purchase_price"] = purchase_price.ToString("#,##0");
                            newRow["sales_price"] = sales_price.ToString("#,##0");
                            DateTime sales_price_updatetime;
                            if (DateTime.TryParse(row["단가수정일"].ToString(), out sales_price_updatetime))
                                newRow["sales_price_updatetime"] = sales_price_updatetime.ToString("yyyy-MM-dd");

                            //원가 / 단위수량
                            double unitCount;
                            if (!double.TryParse(unit_count, out unitCount))
                                unitCount = 1;
                            if (unitCount <= 0)
                                unitCount = 1;
                            sales_cost_price = sales_cost_price / unitCount;
                            shipment_cost_price = shipment_cost_price / unitCount;
                            //씨오버 매출원가
                            newRow["sales_cost_price"] = sales_cost_price;
                            //선적 원가
                            newRow["pending_cost_price"] = shipment_cost_price;

                            //최근 오퍼가 매출원가
                            newRow["offer_price"] = offer_price;
                            newRow["offer_updatetime"] = offer_updatetime;
                            newRow["offer_company"] = offer_company;
                            newRow["weight_calculate"] = weight_calculate;
                            newRow["tray_calculate"] = tray_calculate;

                            newRow["sales_count"] = salesCnt.ToString("#,##0");
                            newRow["excluded_qty"] = excluded_qty.ToString("#,##0");
                            newRow["average_day_sales_count"] = avg_day_sales.ToString("#,##0");
                            newRow["average_day_sales_count_double"] = avg_day_sales.ToString("#,##0.00");
                            newRow["average_month_sales_count"] = avg_month_sales.ToString("#,##0");
                            /*newRow["enable_sales_days"] = enable_days.ToString("#,##0");
                            newRow["month_around"] = month_around;*/

                            newRow["division"] = row["구분"].ToString();
                            newRow["manager1"] = sales_manager1;
                            newRow["manager2"] = sales_manager2;
                            newRow["manager3"] = trade_manager3;

                            newRow["production_days"] = production_days;
                            newRow["purchase_margin"] = purchase_margin;
                            newRow["base_around_month"] = base_around_month;

                            //일반시세
                            newRow["average_purchase_price"] = Convert.ToDouble(row["일반시세"].ToString()).ToString("#,##0");

                            //일반시세 역대현황
                            newRow["normal_price_rate"] = normal_price_rate.ToString("#,##0.0");
                            newRow["normal_price_rate_txt"] = (upCnt + downCnt).ToString("#,##0") + "개 중 " + upCnt.ToString("#,##0") + "위"
                                + "\n" + minPrice.ToString("#,##0") + " ~ " + maxPrice.ToString("#,##0") + " (상위" + normal_price_rate.ToString("#,##0.0") + "%)";
                            //제외품목================================================================================================
                            DataRow[] hdRow = null;
                            if (hdRow != null && hdRow.Length > 0)
                            {
                                //설정일자
                                DateTime endDate = endDate = dtpEnddate.Value; ;
                                newRow["hide_mode"] = hdRow[0]["hide_mode"];
                                newRow["hide_details"] = hdRow[0]["hide_details"];
                                DateTime tmpDt;
                                if (DateTime.TryParse(hdRow[0]["until_date"].ToString(), out tmpDt))
                                {
                                    if (endDate <= tmpDt)
                                        newRow["hide_until_date"] = tmpDt.ToString("yyyy-MM-dd");
                                }
                            }
                            newRow["main_id"] = main_id;
                            newRow["sub_id"] = sub_id;
                            newDt.Rows.Add(newRow);
                        }
                    }
                }
            }
            //데이터 출력
            dgvProduct.DataSource = MergeProduct2(newDt);
            //Header, Cell style
            SetHeaderStyle();

            dgvProduct.EndEdit();
            dgvProduct.Refresh();
        }


        private void GetSeaoverCostPrice(DataTable productDt, DataRow row, out double cost_price, out string cost_price_tooltip)
        {
            string whr;
            //품목정보
            double unit_count;
            if (!double.TryParse(row["단위수량"].ToString(), out unit_count))
                unit_count = 1;
            double unit;
            if (!double.TryParse(row["단위"].ToString(), out unit))
                unit = 1;
            double seaover_unit;
            if (!double.TryParse(row["SEAOVER단위"].ToString(), out seaover_unit))
                seaover_unit = 1;

            int main_id;
            if (!int.TryParse(row["main_id"].ToString(), out main_id))
                main_id = 0;
            int sub_id;
            if (!int.TryParse(row["sub_id"].ToString(), out sub_id))
                sub_id = 0;
            //SEAOVER 원가계산===============================================================================================
            //병합품목
            string[] sub_product;
            if (sub_id == 9999 && !string.IsNullOrEmpty(row["그룹코드"].ToString()))
                sub_product = row["그룹코드"].ToString().Trim().Split('$');
            else
            {
                sub_product = new string[1];
                sub_product[0] = row["품명"].ToString()
                            + "^" + row["원산지"].ToString()
                            + "^" + row["규격"].ToString()
                            + "^" + row["단위"].ToString()
                            + "^" + row["가격단위"].ToString()
                            + "^" + row["단위수량"].ToString()
                            + "^" + row["SEAOVER단위"].ToString();
            }
            string tooltip = "";
            double total_average_cost = 0;
            double total_qty = 0;

            whr = "";
            foreach (string sub in sub_product)
            {
                string[] products = sub.Trim().Split('^');
                if (sub.Length >= 6)
                { 
                    string tmpWhr = "품명 = '" + products[0] + "'"
                                + " AND 원산지 = '" + products[1] + "'"
                                + " AND 규격 = '" + products[2] + "'"
                                + " AND 단위 = '" + products[6] + "'";

                    if (string.IsNullOrEmpty(whr))
                        whr = "(" + tmpWhr + ")";
                    else
                        whr += " \n OR (" + tmpWhr + ")";
                }
            }

            if (!string.IsNullOrEmpty(whr))
            {
                DataRow[] dr = productDt.Select(whr);
                if (dr.Length > 0)
                {
                    for (int k = 0; k < dr.Length; k++)
                    {
                        //재고단위
                        double stock_unit;
                        if (!double.TryParse(dr[k]["단위"].ToString(), out stock_unit))
                            stock_unit = 1;
                        //수량
                        double qty = Convert.ToDouble(dr[k]["수량"].ToString());
                        //매출원가
                        double sales_cost = Convert.ToDouble(dr[k]["매출원가"].ToString());
                        if (sales_cost == 0)
                            sales_cost = Convert.ToDouble(dr[k]["매입금액"].ToString());

                        //계산된 원가
                        if (bool.TryParse(dr[k]["isPendingCalculate"].ToString(), out bool isPendingCalculate) && isPendingCalculate)
                        {
                            //동원시 추가
                            if (dr[k]["AtoNo"].ToString().Contains("dw") || dr[k]["AtoNo"].ToString().Contains("DW")
                                || dr[k]["AtoNo"].ToString().Contains("hs") || dr[k]["AtoNo"].ToString().Contains("HS")
                                || dr[k]["AtoNo"].ToString().Contains("od") || dr[k]["AtoNo"].ToString().Contains("OD")
                                || dr[k]["AtoNo"].ToString().Contains("ad") || dr[k]["AtoNo"].ToString().Contains("AD"))
                                sales_cost = sales_cost * 1.025;
                            else if (dr[k]["AtoNo"].ToString().Contains("jd") || dr[k]["AtoNo"].ToString().Contains("JD"))
                                sales_cost = sales_cost * 1.02;

                            //매출단가 * 단위
                            sales_cost = sales_cost * unit;

                            //냉장료
                            double refrigeration_fee = 0;
                            DateTime in_date;
                            if (DateTime.TryParse(dr[k]["입고일자"].ToString(), out in_date))
                            {
                                TimeSpan ts = DateTime.Now - in_date;
                                int days = ts.Days;
                                refrigeration_fee = stock_unit / unit_count * days;
                            }

                            //연이자 추가
                            double interest = 0;
                            DateTime etd;
                            if (DateTime.TryParse(dr[k]["etd"].ToString(), out etd))
                            {
                                TimeSpan ts = DateTime.Now - etd;
                                int days = ts.Days;
                                interest = sales_cost * 0.08 / 365 * days;
                                if (interest < 0)
                                    interest = 0;
                            }
                            //설명
                            if (qty > 0)
                            {
                                if (interest > 0 && refrigeration_fee > 0)
                                    tooltip += "\n AtoNo : " + dr[k]["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + $" + {interest.ToString("N2")}(이자) + {refrigeration_fee.ToString("#,##0")}(냉장료) = {(sales_cost + interest + refrigeration_fee).ToString("#,##0")} " + "  | 수량 :" + qty;
                                else if (interest == 0 && refrigeration_fee > 0)
                                    tooltip += "\n AtoNo : " + dr[k]["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + $" + {refrigeration_fee.ToString("#,##0")}(냉장료) = {(sales_cost + refrigeration_fee).ToString("#,##0")} " + "  | 수량 :" + qty;
                                else if (interest > 0 && refrigeration_fee == 0)
                                    tooltip += "\n AtoNo : " + dr[k]["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + $" + {interest.ToString("N2")}(이자) = {(sales_cost + interest).ToString("#,##0")} " + "  | 수량 :" + qty;
                                else
                                    tooltip += "\n AtoNo : " + dr[k]["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + "  | 수량 :" + qty;

                                //2024-01-25 매출원가가 0원으로 잡힌것들은 표시를 해야함
                                if (sales_cost == 0 && qty > 0)
                                    tooltip += "   ***확인필요***";
                            }
                            //매출원가+이자+냉장료
                            sales_cost += interest + refrigeration_fee;
                        }
                        //SEAOVER 원가
                        else
                        {
                            //현재 단위로 맞춤
                            sales_cost = sales_cost / stock_unit * unit;
                            //설명
                            tooltip += "\n AtoNo : " + dr[k]["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + "  | 수량 :" + qty;

                            //2024-01-25 매출원가가 0원으로 잡힌것들은 표시를 해야함
                            if (sales_cost == 0 && qty > 0)
                                tooltip += "   ***확인필요***";
                        }

                        //대표품목 s원가
                        total_average_cost += sales_cost * qty;
                        total_qty += qty;
                    }
                }
            }

            cost_price = total_average_cost / total_qty;
            if (double.IsNaN(cost_price))
                cost_price = 0;
            cost_price_tooltip = tooltip;
        }
        private void GetPendingCostPrice(DataRow row, double custom, double tax, double incidental_expense
            , out double total_shipment_qty, out double total_unpending_qty_before, out double total_unpending_qty_after, out double shipment_cost_price)
        {
            string whr;
            total_shipment_qty = 0;
            total_unpending_qty_before = 0;
            total_unpending_qty_after = 0;

            shipment_cost_price = 0;
            DataRow[] uppRow = null;
            if (uppDt.Rows.Count > 0)
            {
                int main_id;
                if (!int.TryParse(row["main_id"].ToString(), out main_id))
                    main_id = 0;
                int sub_id;
                if (!int.TryParse(row["sub_id"].ToString(), out sub_id))
                    sub_id = 0;
                double unit_count;
                if (!double.TryParse(row["단위수량"].ToString(), out unit_count))
                    unit_count = 0;
                double bundle_count;
                if (!double.TryParse(row["묶음수"].ToString(), out bundle_count))
                    bundle_count = 0;

                if(bundle_count > unit_count)
                    unit_count = bundle_count;

                if (unit_count == 0)
                    unit_count = 1;


                if (main_id == 0 && sub_id == 0)
                    sub_id = 0;

                //병합품목
                string[] sub_product;
                if (sub_id == 9999 && !string.IsNullOrEmpty(row["그룹코드"].ToString()))
                    sub_product = row["그룹코드"].ToString().Trim().Split('$');
                else
                {
                    sub_product = new string[1];
                    sub_product[0] = row["품명"].ToString()
                                + "^" + row["원산지"].ToString()
                                + "^" + row["규격"].ToString()
                                + "^" + row["단위"].ToString()
                                + "^" + row["가격단위"].ToString()
                                + "^" + row["단위수량"].ToString()
                                + "^" + row["SEAOVER단위"].ToString();
                }

                foreach (string sub in sub_product)
                {
                    string[] product_info = sub.Split('^');
                    whr = "product = '" + product_info[0] + "'"
                        + " AND origin = '" + product_info[1] + "'"
                        + " AND sizes = '" + product_info[2] + "'"
                        + " AND box_weight = '" + product_info[6] + "'";
                    uppRow = uppDt.Select(whr);
                    if (uppRow.Length > 0)
                    {
                        double total_cost_price = 0;
                        double total_qty = 0;
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

                            //선적원가
                            double cost_price;
                            if (!double.TryParse(uppRow[j]["cost_price"].ToString(), out cost_price))
                                cost_price = 0;
                            if (string.IsNullOrEmpty(warehousing_date) || cost_price > 0)
                            {
                                double box_weight;
                                if (uppRow[j]["box_weight"] == null || !double.TryParse(uppRow[j]["box_weight"].ToString(), out box_weight))
                                    box_weight = 1;
                                double tray;
                                if (uppRow[j]["cost_unit"] == null || !double.TryParse(uppRow[j]["cost_unit"].ToString(), out tray))
                                    tray = 1;
                                //단위 <-> 트레이
                                bool isWeight = Convert.ToBoolean(uppRow[j]["weight_calculate"].ToString());
                                if (!isWeight)
                                    box_weight = tray;
                                //원가계산
                                double unit_price;
                                if (!double.TryParse(uppRow[j]["unit_price"].ToString(), out unit_price))
                                    unit_price = 0;


                                if (unit_price > 0)
                                {
                                    // shipment_cost_price = (unit_price * exchange_rate) * (1 + custom + tax + incidental_expense) * box_weight;
                                    shipment_cost_price = unit_price * exchange_rate * box_weight / unit_count;

                                    if (custom > 0)
                                        shipment_cost_price *= (custom + 1);
                                    if (tax > 0)
                                        shipment_cost_price *= (tax + 1);
                                    if (incidental_expense > 0)
                                        shipment_cost_price *= (incidental_expense + 1);

                                }
                                else
                                    shipment_cost_price = cost_price;

                                //동원 + 2% or 2.5%
                                if (uppRow[j]["ato_no"].ToString().Contains("dw") || uppRow[j]["ato_no"].ToString().Contains("DW")
                                    || uppRow[j]["ato_no"].ToString().Contains("hs") || uppRow[j]["ato_no"].ToString().Contains("HS")
                                    || uppRow[j]["ato_no"].ToString().Contains("od") || uppRow[j]["ato_no"].ToString().Contains("OD")
                                    || uppRow[j]["ato_no"].ToString().Contains("ad") || uppRow[j]["ato_no"].ToString().Contains("AD"))
                                    shipment_cost_price = shipment_cost_price * 1.025;
                                else if (uppRow[j]["ato_no"].ToString().Contains("jd") || uppRow[j]["ato_no"].ToString().Contains("JD"))
                                    shipment_cost_price = shipment_cost_price * 1.02;

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

                                //평균
                                if (unpending_qty_after == 0)
                                {
                                    total_cost_price += shipment_cost_price * (shipment_qty + unpending_qty_before);
                                    total_qty += shipment_qty + unpending_qty_before;
                                }
                            }
                        }
                        shipment_cost_price = total_cost_price / total_qty;
                        if (double.IsNaN(shipment_cost_price))
                            shipment_cost_price = 0;
                    }
                }
            }
        }

        private void OutputProductData4(DataTable productDt, DataTable mngDt, DataTable tmDt, DataTable pgDt, DataTable upDt, DataTable eDt, int sttPrice, int endPrice, DataTable groupDt = null)
        {
            if (productDt != null && productDt.Rows.Count > 0)
            {
                string whr;
                //대표품목 출력여부 Dic
                List<string> mergeMainList = new List<string>();
                DataTable resultDt = new DataTable();
                //검색항목
                string category = txtCategory.Text.Trim();
                string product = txtProduct.Text.Trim();
                string origin = txtOrigin.Text.Trim();
                string sizes = txtSizes.Text.Trim();
                string unit = txtUnit.Text.Trim();
                string manager1 = txtManager1.Text.Trim();
                string manager2 = txtManager2.Text.Trim();
                string manager3 = txtTradeManager.Text.Trim();
                string division = txtDivision.Text.Trim();

                if (string.IsNullOrEmpty(category)
                    && string.IsNullOrEmpty(product)
                    && string.IsNullOrEmpty(origin)
                    && string.IsNullOrEmpty(sizes)
                    && string.IsNullOrEmpty(unit)
                    && string.IsNullOrEmpty(division))
                    whr = "1 = 1";
                else
                {
                    whr = "(sub_id < 9999 AND sub_id > 0) OR ( 1=1 ";
                    if (!string.IsNullOrEmpty(category))
                        whr += $"\n {whereSql("대분류1", category)}";
                    if (!string.IsNullOrEmpty(product))
                        whr += $"\n {whereSql("품명", product)}";
                    if (!string.IsNullOrEmpty(origin))
                        whr += $"\n {whereSql("원산지", origin)}";
                    if (!string.IsNullOrEmpty(sizes))
                        whr += $"\n {whereSql("규격", sizes)}";
                    if (!string.IsNullOrEmpty(unit))
                        whr += $"\n {whereSql("단위", unit)}";
                    if (!string.IsNullOrEmpty(division))
                        whr += $"\n {whereSql("구분", division)}";
                    whr += $"\n )                                        ";
                }

                DataRow[] resultDr = productDt.Select(whr);
                if (resultDr.Length > 0)
                    productDt = resultDr.CopyToDataTable();
                else
                    return;

                //추가 정보 붙히기
                if (productDt != null && productDt.Rows.Count > 0)
                {
                    DataTable productDt2 = null;
                    GetCostToPendingCostTable2(out productDt2);

                    foreach (DataRow row in productDt.Rows)
                    {
                        // 1.담당자1,2
                        string sales_manager1 = "";
                        string sales_manager2 = "";
                        if (mngDt.Rows.Count > 0)
                        {
                            whr = "대분류1 = '" + row["대분류"].ToString() + "'"
                                + " AND 품명 = '" + row["품명"].ToString() + "'"
                                + " AND 원산지 = '" + row["원산지"].ToString() + "'"
                                + " AND 규격 = '" + row["규격"].ToString() + "'"
                                + " AND 단위 = '" + row["단위"].ToString() + "'"
                                + " AND 가격단위 = '" + row["가격단위"].ToString() + "'"
                                + " AND 단위수량 = " + row["단위수량"].ToString()
                                + " AND SEAOVER단위 = '" + row["SEAOVER단위"].ToString() + "'";

                            DataRow[] dtRow = null;
                            dtRow = mngDt.Select(whr);
                            if (dtRow.Length > 0)
                            {
                                sales_manager1 = dtRow[0]["담당자1"].ToString();
                                sales_manager2 = dtRow[0]["담당자2"].ToString();
                            }
                        }
                        // 2.무역담당자
                        whr = "product = '" + row["품명"].ToString() + "'"
                                            + " AND origin = '" + row["원산지"].ToString() + "'"
                                            + " AND sizes = '" + row["규격"].ToString() + "'"
                                            + " AND (unit = '" + row["단위"].ToString() + "'"
                                            + " OR unit = '" + row["SEAOVER단위"].ToString() + "')";
                        DataRow[] tmRow = tmDt.Select(whr);
                        string trade_manager3 = "";
                        if (tmRow.Length > 0)
                        {
                            for (int j = 0; j < tmRow.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tmRow[j]["manager"].ToString().Trim()))
                                {
                                    trade_manager3 = tmRow[j]["manager"].ToString().Trim();
                                    break;
                                }
                            }
                        }

                        // 2.검색항목 필터
                        int main_id = Convert.ToInt32(row["main_id"].ToString());
                        int sub_id = Convert.ToInt32(row["sub_id"].ToString());

                        bool isOutput = true;
                        if (main_id == 0 || (main_id > 0 && sub_id == 9999))
                        {
                            if (isOutput)
                                isOutput = TxtContain(sales_manager1, manager1);
                            if (isOutput)
                                isOutput = TxtContain(sales_manager2, manager2);
                            if (isOutput)
                                isOutput = TxtContain(trade_manager3, manager3);
                        }
                        //실제 데이터 출력======================================================================================
                        if (isOutput)
                        {
                            //품목정보
                            string price_unit = row["가격단위"].ToString();
                            string unit_count = row["단위수량"].ToString();
                            string bundle_count = row["묶음수"].ToString();
                            if (string.IsNullOrEmpty(unit_count))
                                unit_count = "0";
                            string seaover_unit = row["SEAOVER단위"].ToString();
                            if (string.IsNullOrEmpty(seaover_unit))
                                seaover_unit = "0";
                            //재고수
                            double seaover_unpending = Convert.ToDouble(row["미통관재고"].ToString());
                            double seaover_pending = Convert.ToDouble(row["통관재고"].ToString());
                            //예약수
                            double reserved_stock = Convert.ToDouble(row["예약수"].ToString());
                            double real_stock = seaover_unpending + seaover_pending - reserved_stock;

                            // 가용재고 = 재고수 - 예약수
                            double sales_cost_price = Convert.ToDouble(row["매출원가"].ToString());         //매출원가
                            double sales_price = Convert.ToDouble(row["매출단가"].ToString());              //매출단가
                            double purchase_price = Convert.ToDouble(row["최저단가"].ToString());           //매입단가
                            double average_purchase_price = Convert.ToDouble(row["일반시세"].ToString());   //일반시세

                            // 3. S원가계산
                            double seaover_cost_price;
                            string seaover_tooltip;
                            GetSeaoverCostPrice(productDt2, row, out seaover_cost_price, out seaover_tooltip);

                            if (main_id > 0 && sub_id == 9999)
                                sub_id = 9999;

                            // 4.기타 부대비용
                            double cost_unit = 0;
                            double tax = 0;
                            double custom = 0;
                            double incidental_expense = 0;
                            double fixed_tariff = 0;
                            double offer_price = 0;
                            string offer_updatetime = "";
                            string offer_company = "";
                            double production_days = 20;
                            double purchase_margin = 5;
                            double base_around_month = 3;
                            bool weight_calculate = true;
                            bool tray_calculate = false;
                            if (row["가격단위"].ToString().Contains("팩"))
                            {
                                whr = "product = '" + row["품명"].ToString() + "'"
                                            + " AND origin = '" + row["원산지"].ToString() + "'"
                                            + " AND sizes = '" + row["규격"].ToString() + "'"
                                            + " AND (unit = '" + row["단위"].ToString() + "'"
                                            + " OR unit2 =  '" + row["SEAOVER단위"].ToString() + "'"
                                            + " OR unit2 =  '" + row["단위"].ToString() + "')";
                            }
                            else
                            {
                                whr = "product = '" + row["품명"].ToString() + "'"
                                        + " AND origin = '" + row["원산지"].ToString() + "'"
                                        + " AND sizes = '" + row["규격"].ToString() + "'"
                                        + " AND (unit = '" + row["단위"].ToString() + "'"
                                        + " OR unit2 = '" + row["SEAOVER단위"].ToString() + "')";
                            }
                            tmRow = tmDt.Select(whr);
                            if (tmRow.Length > 0)
                            {
                                //관세
                                if (!double.TryParse(tmRow[0]["custom"].ToString(), out custom))
                                    custom = 0;
                                custom = custom / 100;
                                //과세
                                if (!double.TryParse(tmRow[0]["tax"].ToString(), out tax))
                                    tax = 0;
                                tax = tax / 100;
                                //부대비용
                                if (!double.TryParse(tmRow[0]["incidental_expense"].ToString(), out incidental_expense))
                                    incidental_expense = 0;
                                incidental_expense = incidental_expense / 100;
                                //고지가
                                if (!double.TryParse(tmRow[0]["fixed_tariff"].ToString(), out fixed_tariff))
                                    fixed_tariff = 0;
                                fixed_tariff /= 1000;
                                //생산일
                                if (!double.TryParse(tmRow[0]["production_days"].ToString(), out production_days))
                                    production_days = 0;
                                //수입마진
                                if (!double.TryParse(tmRow[0]["purchase_margin"].ToString(), out purchase_margin))
                                    purchase_margin = 0;

                                //계산방식
                                weight_calculate = Convert.ToBoolean(tmRow[0]["weight_calculate"].ToString());
                                tray_calculate = Convert.ToBoolean(tmRow[0]["tray_calculate"].ToString());

                                //트레이
                                if (!double.TryParse(tmRow[0]["cost_unit"].ToString(), out cost_unit))
                                    cost_unit = 0;
                                //기준회전율
                                base_around_month = Convert.ToDouble(tmRow[0]["base_around_month"].ToString());
                            }


                            //선적원가
                            double total_shipment_qty, total_unpending_qty_before, total_unpending_qty_after, shipment_cost_price;
                            GetPendingCostPrice(row, custom, tax, incidental_expense
                                , out total_shipment_qty, out total_unpending_qty_before, out total_unpending_qty_after, out shipment_cost_price);

                            // 5.국가별 배송기간
                            int delivery_day = 15;
                            whr = "country_name = '" + row["원산지"].ToString() + "'";
                            DataRow[] deliRow = deliDt.Select(whr);
                            if (deliRow.Length > 0)
                                delivery_day = Convert.ToInt32(deliRow[0]["delivery_days"].ToString());

                            // 6.최근 오퍼내역
                            DataRow[] copRow = null;
                            if (copDt != null && copDt.Rows.Count > 0)
                            {
                                if (row["가격단위"].ToString().Contains("팩"))
                                {
                                    whr = "product = '" + row["품명"].ToString() + "'"
                                                + " AND origin = '" + row["원산지"].ToString() + "'"
                                                + " AND sizes = '" + row["규격"].ToString() + "'"
                                                + " AND (unit = '" + row["단위"].ToString() + "'"
                                                + " OR unit2 = '" + row["단위"].ToString() + "')";
                                }
                                else
                                {
                                    whr = "product = '" + row["품명"].ToString() + "'"
                                            + " AND origin = '" + row["원산지"].ToString() + "'"
                                            + " AND sizes = '" + row["규격"].ToString() + "'"
                                            + " AND (unit = '" + row["단위"].ToString() + "'"
                                            + " OR unit2 = '" + row["SEAOVER단위"].ToString() + "')";
                                }
                                copRow = copDt.Select(whr);
                                if (copRow.Length > 0)
                                {
                                    offer_price = Convert.ToDouble(copRow[0]["purchase_price"].ToString());
                                    cost_unit = Convert.ToDouble(copRow[0]["cost_unit"].ToString());
                                    //원가 등록일
                                    offer_updatetime = Convert.ToDateTime(copRow[0]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                                    //거래처
                                    offer_company = copRow[0]["company"].ToString();

                                    if (tmRow == null || tmRow.Length == 0)
                                    {
                                        weight_calculate = Convert.ToBoolean(copRow[0]["weight_calculate"].ToString());
                                        tray_calculate = Convert.ToBoolean(copRow[0]["tray_calculate"].ToString());
                                    }
                                }
                            }

                            

                            // 8.즐겨찾기 선택일 경우
                            double enable_days2 = 0;
                            double month_around2 = 0;
                            string createdatetime = "";
                            if (groupDt != null && groupDt.Rows.Count > 0)
                            {
                                whr = "product = '" + row["품명"].ToString() + "'"
                                    + " AND origin = '" + row["원산지"].ToString() + "'"
                                    + " AND sizes = '" + row["규격"].ToString() + "'"
                                    + " AND unit = '" + row["SEAOVER단위"].ToString() + "'";
                                DataRow[] grRow = groupDt.Select(whr);
                                if (grRow.Length > 0)
                                {
                                    if (grRow[0]["enable_days"] == null || !double.TryParse(grRow[0]["enable_days"].ToString(), out enable_days2))
                                        enable_days2 = 0;

                                    DateTime tmp = DateTime.Now;
                                    if (grRow[0]["createdatetime"] == null || !DateTime.TryParse(grRow[0]["createdatetime"].ToString(), out tmp))
                                        createdatetime = "";
                                    else
                                        createdatetime = tmp.ToString("yyyy-MM-dd");

                                    //날짜에 맞게 판매가능일, 회전율 변환
                                    if (!string.IsNullOrEmpty(createdatetime))
                                    {
                                        int wDays;
                                        common.GetWorkDay(tmp, Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), out wDays);
                                        wDays--;

                                        enable_days2 -= wDays;
                                        month_around2 = enable_days2 / 21;
                                    }
                                    if (enable_days2 >= 9999)
                                        enable_days2 = 9999;
                                    if (month_around2 >= 9999)
                                        month_around2 = 9999;
                                }
                            }

                            // 9.매출제외 / 판매량 // 월 평균 판매량
                            //매출제외
                            double excluded_qty = 0;
                            DataRow[] eRow = null;
                            if (eDt != null && eDt.Rows.Count > 0)
                            {
                                whr = "product = '" + row["품명"].ToString() + "'"
                                        + " AND origin = '" + row["원산지"].ToString() + "'"
                                        + " AND sizes = '" + row["규격"].ToString() + "'"
                                        + " AND unit = '" + row["단위"].ToString() + "'"
                                        + " AND price_unit = '" + row["가격단위"].ToString() + "'"
                                        + " AND unit_count = '" + row["단위수량"].ToString() + "'"
                                        + " AND seaover_unit = '" + row["SEAOVER단위"].ToString() + "'";

                                eRow = eDt.Select(whr);
                                if (eRow.Length > 0)
                                    excluded_qty = Convert.ToDouble(eRow[0]["sale_qty"].ToString());
                            }
                            //판매량
                            double salesCnt = Convert.ToDouble(row["매출수"].ToString());
                            double real_salesCnt = salesCnt - excluded_qty;
                            if (real_salesCnt < 0)
                                real_salesCnt = 0;
                            // 평균 판매수(일, 월)
                            double avg_day_sales = real_salesCnt / workDays;
                            double avg_month_sales = avg_day_sales * 21;

                            // 11.일반시세 역대현황
                            double normal_price_rate = 100;
                            double normal_price = Convert.ToDouble(row["일반시세"].ToString());
                            double maxPrice = normal_price, minPrice = 0;
                            int upCnt = 0, downCnt = 0;
                            if (normal_price > 0 && cbNormalPrice.Checked && purchaseDt.Rows.Count > 0)
                            {
                                whr = "품명 = '" + row["품명"].ToString() + "'"
                                    + " AND 원산지 = '" + row["원산지"].ToString() + "'"
                                    + " AND 규격 = '" + row["규격"].ToString() + "'"
                                    + " AND 단위 = '" + row["SEAOVER단위"].ToString() + "'"
                                    + " AND 단가 > 0";

                                DataRow[] dtRow = null;
                                dtRow = purchaseDt.Select(whr);
                                if (dtRow.Length > 0)
                                {
                                    for (int j = 0; j < dtRow.Length; j++)
                                    {
                                        double temp_price = Convert.ToDouble(dtRow[j]["단가"].ToString());
                                        if (normal_price > temp_price)
                                        {
                                            minPrice = temp_price;
                                            upCnt++;
                                        }
                                        else
                                        {
                                            maxPrice = temp_price;
                                            downCnt++;
                                        }
                                    }
                                    normal_price_rate = (double)upCnt / ((double)upCnt + (double)downCnt) * 100;
                                }
                            }


                            //데이터 매칭================================================================================================
                            DataRow newRow = newDt.NewRow();
                            //Row 추가
                            newRow["category_code"] = row["대분류"].ToString();
                            newRow["category"] = row["대분류1"].ToString();
                            newRow["category2"] = row["대분류2"].ToString();
                            newRow["category3"] = row["대분류3"].ToString();

                            newRow["product_code"] = row["품명코드"].ToString();
                            newRow["origin_code"] = row["원산지코드"].ToString();
                            newRow["sizes_code"] = row["규격코드"].ToString();

                            newRow["product"] = row["품명"].ToString();
                            newRow["origin"] = row["원산지"].ToString();
                            newRow["sizes"] = row["규격"].ToString();
                            newRow["sizes2"] = row["규격2"].ToString();
                            newRow["sizes3"] = row["규격3"].ToString();
                            newRow["sizes4"] = row["규격4"].ToString();
                            newRow["unit"] = row["단위"].ToString().Trim();

                            newRow["price_unit"] = price_unit;
                            newRow["unit_count"] = unit_count;
                            newRow["bundle_count"] = bundle_count;
                            newRow["seaover_unit"] = seaover_unit;

                            newRow["delivery_days"] = delivery_day;

                            newRow["cost_unit"] = cost_unit;
                            newRow["custom"] = custom * 100;
                            newRow["tax"] = tax * 100;
                            newRow["incidental_expense"] = incidental_expense * 100;
                            newRow["fixed_tariff"] = fixed_tariff * 1000;

                            //즐겨찾기
                            newRow["enable_sales_days2"] = enable_days2;
                            newRow["month_around2"] = month_around2;
                            newRow["createdatetime"] = createdatetime;

                            //예약상세
                            //dgv.Rows[n].Cells["reserved_stock_detail"].Value = row["예약상세"].ToString();
                            newRow["reserved_stock_details"] = "";
                            newRow["reserved_stock"] = reserved_stock.ToString("#,##0");

                            newRow["seaover_unpending"] = seaover_unpending.ToString("#,##0");
                            newRow["seaover_pending"] = seaover_pending.ToString("#,##0");

                            newRow["shipment_qty"] = total_shipment_qty.ToString("#,##0");
                            newRow["unpending_qty_before"] = total_unpending_qty_before.ToString("#,##0");
                            newRow["unpending_qty_after"] = total_unpending_qty_after.ToString("#,##0");

                            newRow["real_stock"] = (seaover_unpending + seaover_pending).ToString("#,##0");
                            newRow["shipment_stock"] = (seaover_unpending + seaover_pending + total_shipment_qty + total_unpending_qty_before).ToString("#,##0");
                            //선적 원가
                            newRow["pending_cost_price"] = shipment_cost_price;
                            //매입, 매출단가
                            newRow["purchase_price"] = purchase_price.ToString("#,##0");
                            newRow["sales_price"] = sales_price.ToString("#,##0");
                            DateTime sales_price_updatetime;
                            if (DateTime.TryParse(row["단가수정일"].ToString(), out sales_price_updatetime))
                                newRow["sales_price_updatetime"] = sales_price_updatetime.ToString("yyyy-MM-dd");

                            //원가 / 단위수량
                            double unitCount;
                            if (!double.TryParse(unit_count, out unitCount))
                                unitCount = 1;
                            if (unitCount <= 0)
                                unitCount = 1;
                            sales_cost_price = sales_cost_price / unitCount;
                            shipment_cost_price = shipment_cost_price / unitCount;
                            //씨오버 매출원가
                            newRow["sales_cost_price"] = seaover_cost_price;
                            newRow["sales_cost_price_tooltip"] = seaover_tooltip;
                            

                            //최근 오퍼가 매출원가
                            newRow["offer_price"] = offer_price;
                            newRow["offer_updatetime"] = offer_updatetime;
                            newRow["offer_company"] = offer_company;
                            newRow["weight_calculate"] = weight_calculate;
                            newRow["tray_calculate"] = tray_calculate;

                            newRow["sales_count"] = salesCnt.ToString("#,##0");
                            newRow["excluded_qty"] = excluded_qty.ToString("#,##0");
                            newRow["average_day_sales_count"] = avg_day_sales.ToString("#,##0");
                            newRow["average_day_sales_count_double"] = avg_day_sales.ToString("#,##0.00");
                            newRow["average_month_sales_count"] = avg_month_sales.ToString("#,##0");
                            /*newRow["enable_sales_days"] = enable_days.ToString("#,##0");
                            newRow["month_around"] = month_around;*/

                            newRow["division"] = row["구분"].ToString();
                            newRow["manager1"] = sales_manager1;
                            newRow["manager2"] = sales_manager2;
                            newRow["manager3"] = trade_manager3;

                            newRow["production_days"] = production_days;
                            newRow["purchase_margin"] = purchase_margin;
                            newRow["base_around_month"] = base_around_month;

                            //일반시세
                            newRow["average_purchase_price"] = Convert.ToDouble(row["일반시세"].ToString()).ToString("#,##0");

                            //일반시세 역대현황
                            newRow["normal_price_rate"] = normal_price_rate.ToString("#,##0.0");
                            newRow["normal_price_rate_txt"] = (upCnt + downCnt).ToString("#,##0") + "개 중 " + upCnt.ToString("#,##0") + "위"
                                + "\n" + minPrice.ToString("#,##0") + " ~ " + maxPrice.ToString("#,##0") + " (상위" + normal_price_rate.ToString("#,##0.0") + "%)";
                            //제외품목================================================================================================
                            DataRow[] hdRow = null;
                            if (hdRow != null && hdRow.Length > 0)
                            {
                                //설정일자
                                DateTime endDate = endDate = dtpEnddate.Value; ;
                                newRow["hide_mode"] = hdRow[0]["hide_mode"];
                                newRow["hide_details"] = hdRow[0]["hide_details"];
                                DateTime tmpDt;
                                if (DateTime.TryParse(hdRow[0]["until_date"].ToString(), out tmpDt))
                                {
                                    if (endDate <= tmpDt)
                                        newRow["hide_until_date"] = tmpDt.ToString("yyyy-MM-dd");
                                }
                            }
                            newRow["main_id"] = main_id;
                            newRow["sub_id"] = sub_id;
                            newRow["merge_product"] = row["그룹코드"].ToString().Replace("$","\n");
                            newDt.Rows.Add(newRow);
                        }
                    }
                }
            }
            //데이터 출력
            dgvProduct.DataSource = MergeProduct3(newDt);
            //Header, Cell style
            //처음 호출에만 저장된 컬럼설정 들고오기
            if(GetDataCnt == 0)
                SetHeaderStyle();
            else
                SetHeaderStyle(false);
            GetDataCnt++;
            dgvProduct.EndEdit();
            dgvProduct.Refresh();
        }
        int GetDataCnt = 0;

        private string whereSql(string whrColumn, string whrValue)
        {
            string[] tempStr = null;
            string tempWhr = "";
            string whrStr = "";
            if (!string.IsNullOrEmpty(whrValue.Trim()))
            {
                tempStr = whrValue.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i]))
                        {
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                tempWhr = $"\n	   {whrColumn} LIKE '%{tempStr[i]}%' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR {whrColumn} LIKE '%{tempStr[i]}%' ";
                            }
                        }
                    }
                    whrStr = $"\n	 AND ( {tempWhr} )";
                }
                else
                {
                    whrStr = $"\n	  AND {whrColumn} LIKE '%{whrValue}%'";
                }
            }
            return whrStr;
        }


        private void HeaderTextChange()
        {
            //HeaderText 변경
            for (int i = 0; i < dgvProduct.Columns.Count; i++)
            {
                dgvProduct.Columns[i].HeaderText = newDt.Columns[i].Caption;
            }
        }
        private void SetPriceUpDown(DataGridViewRow row, double avg_day_sales, out int up_lv, out int down_lv)
        {
            dgvProduct.EndEdit();
            up_lv = 0;
            down_lv = 0;

            bool isHide = false;
            bool isDownHide = false;
            bool isUpHide = false;

            double real_stock;
            if (row.Cells["real_stock"].Value == null || !double.TryParse(row.Cells["real_stock"].Value.ToString(), out real_stock))
                real_stock = 0;

            DateTime untilDate;
            if (row.Cells["hide_until_date"].Value != null && DateTime.TryParse(row.Cells["hide_until_date"].Value.ToString(), out untilDate))
            {
                if (Convert.ToDateTime(untilDate.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    //숨기기 아닌 품목
                    if (row.Cells["hide_mode"].Value == null || string.IsNullOrEmpty(row.Cells["hide_mode"].Value.ToString()))
                    {
                        isHide = false;
                    }
                    //All 제외
                    else if (row.Cells["hide_mode"].Value.ToString() == "all")
                    {
                        isHide = true;
                    }
                    //재고수 제한 제외
                    else if (row.Cells["hide_mode"].Value.ToString() == "stock")
                    {
                        string[] details = row.Cells["hide_details"].Value.ToString().Split('|');
                        foreach (string d in details)
                        {
                            if (!string.IsNullOrEmpty(d.Trim()))
                            {
                                string[] stock = d.Trim().Split('_');
                                if (stock.Length >= 1)
                                {
                                    double hide_qty = Convert.ToDouble(stock[1]);
                                    if (stock[0] == "up")
                                    {
                                        if (real_stock > hide_qty)
                                            isUpHide = true;
                                    }
                                    else if (stock[0] == "down")
                                    {
                                        if (real_stock < hide_qty)
                                            isDownHide = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //무조건제외가 아니라면
            if (!isHide)
            {
                //판매가능일수
                double enable_days = 0;
                if (avg_day_sales > 0 & real_stock > 0)
                    enable_days = (real_stock / avg_day_sales);
                //일반시세
                double avg_price;
                if (row.Cells["average_purchase_price"].Value == null || !double.TryParse(row.Cells["average_purchase_price"].Value.ToString(), out avg_price))
                    avg_price = 0;
                //매출단가
                double sales_price;
                if (row.Cells["sales_price"].Value == null || !double.TryParse(row.Cells["sales_price"].Value.ToString(), out sales_price))
                    sales_price = 0;
                //회전율
                double month_round;
                if (enable_days > 0)
                    month_round = enable_days / 21;
                else
                    month_round = 10;

                //기준회전율
                double base_month_round;
                if (row.Cells["base_around_month"].Value == null || !double.TryParse(row.Cells["base_around_month"].Value.ToString(), out base_month_round))
                    base_month_round = 0;
                base_month_round /= 3;


                //실제재고가 0 이상 일경우
                if (real_stock > 0)
                {
                    //일반시세가 0원일 경우
                    if (avg_price == 0)
                    {
                        if (month_round >= (base_month_round * 5))
                        {
                            if (!isDownHide)
                            {
                                up_lv = -1;
                                down_lv = -6;
                                return;
                            }
                        }
                        else if (month_round >= (base_month_round * 4))
                        {
                            if (!isDownHide)
                            {
                                up_lv = -2;
                                down_lv = -5;
                                return;
                            }
                        }
                        else if (month_round >= (base_month_round * 3))
                        {
                            if (!isDownHide)
                            {
                                up_lv = -3;
                                down_lv = -4;
                                return;
                            }

                        }
                        else if (month_round < (base_month_round * 3))
                        {
                            if (!isUpHide)
                            {
                                up_lv = -4;
                                down_lv = -3;
                                return;
                            }
                        }
                        else if (month_round < (base_month_round * 2))
                        {
                            if (!isUpHide)
                            {
                                up_lv = -5;
                                down_lv = -2;
                                return;
                            }
                        }
                        else if (month_round < 1)
                        {
                            if (!isUpHide)
                            {
                                up_lv = -6;
                                down_lv = -1;
                                return;
                            }
                        }
                    }
                    //일반시세가 0원 이상일 경웅
                    else if (avg_price > 10)
                    {
                        if (month_round >= (base_month_round * 5))
                        {
                            if (sales_price >= avg_price && !isDownHide)
                            {
                                up_lv = 1;
                                down_lv = 6;
                                return;
                            }
                        }
                        else if (month_round >= (base_month_round * 4))
                        {
                            if (sales_price >= avg_price && !isDownHide)
                            {
                                up_lv = 2;
                                down_lv = 5;
                                return;
                            }
                        }
                        else if (month_round >= (base_month_round * 3))
                        {
                            if (sales_price >= avg_price && !isDownHide)
                            {
                                up_lv = 3;
                                down_lv = 4;
                                return;
                            }
                        }
                        else if (month_round < (base_month_round * 3) && month_round >= (base_month_round * 2))
                        {
                            if (sales_price <= avg_price && !isUpHide)
                            {
                                up_lv = 4;
                                down_lv = 3;
                                return;
                            }
                        }
                        else if (month_round < (base_month_round * 2) && month_round >= (base_month_round * 1))
                        {
                            if (sales_price <= avg_price && !isUpHide)
                            {
                                up_lv = 5;
                                down_lv = 2;
                                return;
                            }
                        }
                        else if (month_round < base_month_round)
                        {
                            if (sales_price <= avg_price && !isUpHide)
                            {
                                up_lv = 6;
                                down_lv = 1;
                                return;
                            }
                        }
                    }
                }
            }
        }
        private void SetPriceUpDown(DataRow row, double avg_day_sales, out int up_lv, out int down_lv)
        {
            up_lv = 0;
            down_lv = 0;

            bool isHide = false;
            bool isDownHide = false;
            bool isUpHide = false;

            double real_stock;
            if (row["real_stock"] == null || !double.TryParse(row["real_stock"].ToString(), out real_stock))
            {
                real_stock = 0;
            }

            DateTime untilDate;
            if (row["hide_until_date"] != null && DateTime.TryParse(row["hide_until_date"].ToString(), out untilDate))
            {
                if (Convert.ToDateTime(untilDate.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    //숨기기 아닌 품목
                    if (row["hide_mode"] == null || string.IsNullOrEmpty(row["hide_mode"].ToString()))
                    {
                        isHide = false;
                    }
                    //All 제외
                    else if (row["hide_mode"].ToString() == "all")
                    {
                        isHide = true;
                    }
                    //재고수 제한 제외
                    else if (row["hide_mode"].ToString() == "stock")
                    {
                        string[] details = row["hide_details"].ToString().Split('|');
                        foreach (string d in details)
                        {
                            if (!string.IsNullOrEmpty(d.Trim()))
                            {
                                string[] stock = d.Trim().Split('_');
                                if (stock.Length >= 1)
                                {
                                    double hide_qty = Convert.ToDouble(stock[1]);
                                    if (stock[0] == "up")
                                    {
                                        if (real_stock > hide_qty)
                                        {
                                            isUpHide = true;
                                        }

                                    }
                                    else if (stock[0] == "down")
                                    {
                                        if (real_stock < hide_qty)
                                        {
                                            isDownHide = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //무조건제외가 아니라면
            if (!isHide)
            {
                //판매가능일수
                double enable_days = 0;
                if (avg_day_sales > 0 & real_stock > 0)
                    enable_days = (real_stock / avg_day_sales);
                //일반시세
                double avg_price;
                if (row["average_purchase_price"] == null || !double.TryParse(row["average_purchase_price"].ToString(), out avg_price))
                    avg_price = 0;
                //매출단가
                double sales_price;
                if (row["sales_price"] == null || !double.TryParse(row["sales_price"].ToString(), out sales_price))
                    sales_price = 0;
                //회전율
                double month_round;
                if (enable_days > 0)
                    month_round = enable_days / 21;
                else
                    month_round = 10;
                //기준회전율
                double base_month_round;
                if (row["base_around_month"] == null || !double.TryParse(row["base_around_month"].ToString(), out base_month_round))
                    base_month_round = 0;
                base_month_round /= 3;


                //실제재고가 0 이상 일경우
                if (real_stock > 0)
                {
                    //일반시세가 0원일 경우
                    if (avg_price == 0)
                    {
                        if (month_round >= (base_month_round * 5))
                        {
                            if (!isDownHide)
                            {
                                up_lv = -1;
                                down_lv = -6;
                                return;
                            }
                        }
                        else if (month_round >= (base_month_round * 4))
                        {
                            if (!isDownHide)
                            {
                                up_lv = -2;
                                down_lv = -5;
                                return;
                            }
                        }
                        else if (month_round >= (base_month_round * 3))
                        {
                            if (!isDownHide)
                            {
                                up_lv = -3;
                                down_lv = -4;
                                return;
                            }

                        }
                        else if (month_round < (base_month_round * 3) && month_round >= (base_month_round * 2))
                        {
                            if (!isUpHide)
                            {
                                up_lv = -4;
                                down_lv = -3;
                                return;
                            }
                        }
                        else if (month_round < (base_month_round * 2) && month_round >= (base_month_round * 1))
                        {
                            if (!isUpHide)
                            {
                                up_lv = -5;
                                down_lv = -2;
                                return;
                            }
                        }
                        else if (month_round < base_month_round)
                        {
                            if (!isUpHide)
                            {
                                up_lv = -6;
                                down_lv = -1;
                                return;
                            }
                        }
                    }
                    //일반시세가 0원 이상일 경웅
                    else if (avg_price > 10)
                    {
                        if (month_round >= (base_month_round * 5))
                        {
                            if (sales_price >= avg_price && !isDownHide)
                            {
                                up_lv = 1;
                                down_lv = 6;
                                return;
                            }
                        }
                        else if (month_round >= (base_month_round * 4))
                        {
                            if (sales_price >= avg_price && !isDownHide)
                            {
                                up_lv = 2;
                                down_lv = 5;
                                return;
                            }
                        }
                        else if (month_round >= (base_month_round * 3))
                        {
                            if (sales_price >= avg_price && !isDownHide)
                            {
                                up_lv = 3;
                                down_lv = 4;
                                return;
                            }
                        }
                        else if (month_round < (base_month_round * 3) && month_round >= (base_month_round * 2))
                        {
                            if (sales_price <= avg_price && !isUpHide)
                            {
                                up_lv = 4;
                                down_lv = 3;
                                return;
                            }
                        }
                        else if (month_round < (base_month_round * 2) && month_round >= (base_month_round * 1))
                        {
                            if (sales_price <= avg_price && !isUpHide)
                            {
                                up_lv = 5;
                                down_lv = 2;
                                return;
                            }
                        }
                        else if (month_round < base_month_round)
                        {
                            if (sales_price <= avg_price && !isUpHide)
                            {
                                up_lv = 6;
                                down_lv = 1;
                                return;
                            }
                        }
                    }
                }
            }
        }

        //Datagridview Header style 
        private void SetHeaderStyle(bool isInit = false)
        {
            DataGridView dgv = dgvProduct;
            if (dgv.Columns.Count > 0)
            {
                HeaderTextChange();
                //this.dgvProduct.AllowUserToAddRows = false;
                dgv.Columns["main_id"].Visible = false;
                dgv.Columns["sub_id"].Visible = false;
                dgv.Columns["category_code"].Visible = false;
                dgv.Columns["category2"].Visible = false;
                dgv.Columns["category3"].Visible = false;
                dgv.Columns["product_code"].Visible = false;
                dgv.Columns["origin_code"].Visible = false;
                dgv.Columns["sizes_code"].Visible = false;
                dgv.Columns["sizes2"].Visible = false;
                dgv.Columns["sizes3"].Visible = false;
                dgv.Columns["sizes4"].Visible = false;
                dgv.Columns["box_weight"].Visible = false;
                dgv.Columns["delivery_days"].Visible = false;
                dgv.Columns["sales_cost_price_detail"].Visible = false;
                dgv.Columns["reserved_stock_details"].Visible = false;
                dgv.Columns["unpending_qty_after"].Visible = false;
                dgv.Columns["margin_double"].Visible = false;
                dgv.Columns["average_day_sales_count_double"].Visible = false;
                dgv.Columns["merge_product"].Visible = false;
                //dgv.Columns["exhausted_date"].Visible = false;
                dgv.Columns["hide_mode"].Visible = false;
                dgv.Columns["hide_details"].Visible = false;
                dgv.Columns["up_lv"].Visible = false;
                dgv.Columns["down_lv"].Visible = false;
                //dgv.Columns["sales_count"].Visible = false;
                dgv.Columns["weight_calculate"].Visible = false;
                dgv.Columns["tray_calculate"].Visible = false;
                dgv.Columns["production_days"].Visible = false;
                dgv.Columns["purchase_margin"].Visible = false;
                dgv.Columns["normal_price_rate_txt"].Visible = false;
                dgv.Columns["sales_cost_price_tooltip"].Visible = false;
                dgv.Columns["average_purchase_price_rank"].Visible = false;

                dgv.Columns["month_around_1"].Visible = false;
                dgv.Columns["month_around_45"].Visible = false;
                dgv.Columns["month_around_2"].Visible = false;
                dgv.Columns["month_around_3"].Visible = false;
                dgv.Columns["month_around_6"].Visible = false;
                dgv.Columns["month_around_12"].Visible = false;
                dgv.Columns["month_around_18"].Visible = false;

                dgv.Columns["sort_hide"].Visible = false;
                dgv.Columns["sales_price_updatetime"].Visible = false;

                dgv.Columns["e_date"].Visible = false;
                dgv.Columns["c_date"].Visible = false;
                dgv.Columns["sizes3_double"].Visible = false;

                //마진, 최저단가 
                if (!cbMarginMinPrice.Checked)
                {
                    dgv.Columns["margin"].Visible = false;
                    dgv.Columns["purchase_price"].Visible = false;
                }
                //재고내역 상세
                if (!cbReservationDetails.Checked)
                {
                    dgv.Columns["shipment_qty"].Visible = false;
                    dgv.Columns["unpending_qty_before"].Visible = false;
                    dgv.Columns["seaover_unpending"].Visible = false;
                    dgv.Columns["seaover_pending"].Visible = false;
                    dgv.Columns["reserved_stock"].Visible = false;

                    //dgv.Columns["month_around_in_shipment"].Visible = false;
                }
                //판매내역 상세
                if (!cbSalesCount.Checked)
                {
                    dgv.Columns["sales_count"].Visible = false;
                    dgv.Columns["excluded_qty"].Visible = false;
                    dgv.Columns["average_day_sales_count"].Visible = false;
                }
                //원가내역 상세
                if (!cbCostPrice.Checked)
                {
                    dgv.Columns["pending_cost_price"].Visible = false;
                    dgv.Columns["average_sales_cost_price1"].Visible = false;
                    dgv.Columns["average_sales_cost_price1_margin"].Visible = false;
                }
                //오퍼내역 상세
                if (!cbOffer.Checked)
                {
                    dgv.Columns["tax"].Visible = false;
                    dgv.Columns["custom"].Visible = false;
                    dgv.Columns["incidental_expense"].Visible = false;
                    dgv.Columns["fixed_tariff"].Visible = false;
                    dgv.Columns["offer_price"].Visible = false;
                    dgv.Columns["offer_updatetime"].Visible = false;
                    dgv.Columns["offer_company"].Visible = false;
                    dgv.Columns["total_real_stock"].Visible = false;
                    dgv.Columns["order_qty"].Visible = false;
                    dgv.Columns["order_etd"].Visible = false;
                    dgv.Columns["offer_cost_price"].Visible = false;
                    dgv.Columns["average_sales_cost_price2"].Visible = false;
                    dgv.Columns["average_sales_cost_price2_margin"].Visible = false;
                }
                //일반시세 역대현황
                dgv.Columns["normal_price_rate"].Visible = cbNormalPrice.Checked;
                //즐겨찾기 모드
                dgvProduct.Columns["enable_sales_days2"].Visible = isBookmarkMode;
                dgvProduct.Columns["month_around2"].Visible = isBookmarkMode;
                dgvProduct.Columns["createdatetime"].Visible = isBookmarkMode;

                //헤더 디자인
                dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
                dgv.RowHeadersDefaultCellStyle.BackColor = Color.SeaGreen;

                //기본(회색)
                Color darkBlue = Color.FromArgb(43, 94, 170);    //남색
                dgv.ColumnHeadersDefaultCellStyle.BackColor = darkBlue;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
                dgv.Columns["unit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["unit_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["bundle_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["seaover_unit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["cost_unit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["seaover_unit"].HeaderCell.Style.Font = new Font("나눔고딕", 7, FontStyle.Regular);
                //재고내역========================================================================================
                Color lightBlue = Color.FromArgb(51, 102, 255);  //진파랑            
                dgv.Columns["shipment_qty"].HeaderCell.Style.BackColor = Color.LightGray;
                dgv.Columns["shipment_qty"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["shipment_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["shipment_qty"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["unpending_qty_before"].HeaderCell.Style.BackColor = Color.LightGray;
                dgv.Columns["unpending_qty_before"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["unpending_qty_before"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["unpending_qty_before"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["seaover_unpending"].HeaderCell.Style.BackColor = Color.LightGray;
                dgv.Columns["seaover_unpending"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["seaover_unpending"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["seaover_unpending"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["seaover_pending"].HeaderCell.Style.BackColor = Color.LightGray;
                dgv.Columns["seaover_pending"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["seaover_pending"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["seaover_pending"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["reserved_stock"].HeaderCell.Style.BackColor = Color.LightGray;
                dgv.Columns["reserved_stock"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["reserved_stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["reserved_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["real_stock"].HeaderCell.Style.BackColor = Color.DarkGray;
                dgv.Columns["real_stock"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["real_stock"].DefaultCellStyle.Font = new Font("나눔고딕", 8, System.Drawing.FontStyle.Bold);
                dgv.Columns["real_stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["real_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["shipment_stock"].HeaderCell.Style.BackColor = Color.DarkGray;
                dgv.Columns["shipment_stock"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["shipment_stock"].DefaultCellStyle.Font = new Font("나눔고딕", 8, System.Drawing.FontStyle.Bold);
                dgv.Columns["shipment_stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["shipment_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["total_stock"].HeaderCell.Style.BackColor = Color.DarkGray;
                dgv.Columns["total_stock"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["total_stock"].DefaultCellStyle.Font = new Font("나눔고딕", 8, System.Drawing.FontStyle.Bold);
                dgv.Columns["total_stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["total_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                //판매내역========================================================================================
                Color lightRosybrown = Color.FromArgb(216, 190, 190);
                dgv.Columns["sales_count"].HeaderCell.Style.BackColor = lightRosybrown;
                dgv.Columns["sales_count"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["sales_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["sales_count"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["average_day_sales_count"].HeaderCell.Style.BackColor = lightRosybrown;
                dgv.Columns["average_day_sales_count"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["average_day_sales_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["average_day_sales_count"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["excluded_qty"].HeaderCell.Style.BackColor = lightRosybrown;
                dgv.Columns["excluded_qty"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["excluded_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["excluded_qty"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["average_month_sales_count"].HeaderCell.Style.BackColor = Color.RosyBrown;
                dgv.Columns["average_month_sales_count"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["average_month_sales_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["average_month_sales_count"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                //매출단가 및 최저, 마진 등 ======================================================================================
                Color lightGreen = Color.SeaGreen;
                //dgv.Columns["month_around"].DefaultCellStyle.BackColor = Color.FromArgb(242, 255, 202);

                dgv.Columns["sales_price"].HeaderCell.Style.BackColor = lightGreen;
                dgv.Columns["sales_price"].HeaderCell.Style.ForeColor = Color.Yellow;
                dgv.Columns["sales_price"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["sales_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["sales_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["purchase_price"].HeaderCell.Style.BackColor = lightGreen;
                dgv.Columns["purchase_price"].HeaderCell.Style.ForeColor = Color.Yellow;
                dgv.Columns["purchase_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["purchase_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["average_purchase_price"].HeaderCell.Style.BackColor = lightGreen;
                dgv.Columns["average_purchase_price"].HeaderCell.Style.ForeColor = Color.White;
                dgv.Columns["average_purchase_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["average_purchase_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["average_purchase_price_rank"].HeaderCell.Style.BackColor = lightGreen;
                dgv.Columns["average_purchase_price_rank"].HeaderCell.Style.ForeColor = Color.White;

                dgv.Columns["normal_price_rate"].HeaderCell.Style.BackColor = lightGreen;
                dgv.Columns["normal_price_rate"].HeaderCell.Style.ForeColor = Color.White;
                dgv.Columns["normal_price_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["normal_price_rate"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마

                dgv.Columns["sales_cost_price"].HeaderCell.Style.BackColor = lightGreen;
                dgv.Columns["sales_cost_price"].HeaderCell.Style.ForeColor = Color.White;
                dgv.Columns["sales_cost_price"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["sales_cost_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["sales_cost_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["margin"].HeaderCell.Style.BackColor = lightGreen;
                dgv.Columns["margin"].HeaderCell.Style.ForeColor = Color.White;
                dgv.Columns["margin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["margin"].DefaultCellStyle.Format = "#,###,##0.##"; // 소수점 2자리까지 있으


                //씨오버원가, 선적원가 평균원가1======================================================================
                Color violet = Color.FromArgb(125, 135, 255);
                dgv.Columns["sales_cost_price"].HeaderCell.Style.BackColor = violet;
                dgv.Columns["sales_cost_price"].HeaderCell.Style.ForeColor = Color.White;
                dgv.Columns["sales_cost_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["sales_cost_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["pending_cost_price"].HeaderCell.Style.BackColor = violet;
                dgv.Columns["pending_cost_price"].HeaderCell.Style.ForeColor = Color.White;
                dgv.Columns["pending_cost_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["pending_cost_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["average_sales_cost_price1"].HeaderCell.Style.BackColor = violet;
                dgv.Columns["average_sales_cost_price1"].HeaderCell.Style.ForeColor = Color.White;
                dgv.Columns["average_sales_cost_price1"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["average_sales_cost_price1"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                dgv.Columns["average_sales_cost_price1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["average_sales_cost_price1"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["average_sales_cost_price1_margin"].HeaderCell.Style.BackColor = violet;
                dgv.Columns["average_sales_cost_price1_margin"].HeaderCell.Style.ForeColor = Color.White;
                dgv.Columns["average_sales_cost_price1_margin"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["average_sales_cost_price1_margin"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                dgv.Columns["average_sales_cost_price1_margin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["average_sales_cost_price1_margin"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마


                dgv.Columns["average_sales_cost_price2"].HeaderCell.Style.BackColor = violet;
                dgv.Columns["average_sales_cost_price2"].HeaderCell.Style.ForeColor = Color.White;
                dgv.Columns["average_sales_cost_price2"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["average_sales_cost_price2"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                dgv.Columns["average_sales_cost_price2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["average_sales_cost_price2"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["average_sales_cost_price2_margin"].HeaderCell.Style.BackColor = violet;
                dgv.Columns["average_sales_cost_price2_margin"].HeaderCell.Style.ForeColor = Color.White;
                dgv.Columns["average_sales_cost_price2_margin"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["average_sales_cost_price2_margin"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                dgv.Columns["average_sales_cost_price2_margin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["average_sales_cost_price2_margin"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마

                //오퍼원가 관련, 평균원가2======================================================================
                Color lightViolet = Color.FromArgb(205, 209, 255);
                dgv.Columns["offer_cost_price"].HeaderCell.Style.BackColor = lightViolet;
                dgv.Columns["offer_cost_price"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["offer_cost_price"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["offer_cost_price"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                dgv.Columns["offer_cost_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["offer_cost_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["offer_cost_price_margin"].HeaderCell.Style.BackColor = lightViolet;
                dgv.Columns["offer_cost_price_margin"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["offer_cost_price_margin"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                dgv.Columns["offer_cost_price_margin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["offer_cost_price_margin"].DefaultCellStyle.Format = "#,##0.00"; // 콤마

                dgv.Columns["incidental_expense"].HeaderCell.Style.BackColor = lightViolet;
                dgv.Columns["incidental_expense"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["incidental_expense"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["tax"].HeaderCell.Style.BackColor = lightViolet;
                dgv.Columns["tax"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["tax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["custom"].HeaderCell.Style.BackColor = lightViolet;
                dgv.Columns["custom"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["custom"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dgv.Columns["fixed_tariff"].HeaderCell.Style.BackColor = lightViolet;
                dgv.Columns["fixed_tariff"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["fixed_tariff"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


                dgv.Columns["offer_updatetime"].HeaderCell.Style.BackColor = lightViolet;
                dgv.Columns["offer_updatetime"].HeaderCell.Style.ForeColor = Color.Black;

                dgv.Columns["offer_price"].HeaderCell.Style.BackColor = lightViolet;
                dgv.Columns["offer_price"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["offer_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["offer_price"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마

                dgv.Columns["offer_company"].HeaderCell.Style.BackColor = lightViolet;
                dgv.Columns["offer_company"].HeaderCell.Style.ForeColor = Color.Black;

                dgv.Columns["order_qty"].HeaderCell.Style.BackColor = lightViolet;
                dgv.Columns["order_qty"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["order_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["order_qty"].DefaultCellStyle.BackColor = Color.FromArgb(239, 239, 255);
                dgv.Columns["order_qty"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["order_etd"].HeaderCell.Style.BackColor = lightViolet;
                dgv.Columns["order_etd"].HeaderCell.Style.ForeColor = Color.Black;

                dgv.Columns["total_real_stock"].HeaderCell.Style.BackColor = lightViolet;
                dgv.Columns["total_real_stock"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["total_real_stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["total_real_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마           

                dgv.Columns["exhausted_count"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
                dgv.Columns["exhausted_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dgv.Columns["exhausted_date"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
                dgv.Columns["contract_date"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);

                dgv.Columns["min_etd"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
                dgv.Columns["until_days1"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
                dgv.Columns["until_days1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["until_days2"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
                dgv.Columns["until_days2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dgv.Columns["average_purchase_price"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["average_purchase_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["average_purchase_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["month_around"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["month_around"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
                dgv.Columns["month_around"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["month_around"].DefaultCellStyle.ForeColor = Color.Blue;
                dgv.Columns["month_around"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["month_around"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마

                dgv.Columns["month_around_in_shipment"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["month_around_in_shipment"].HeaderCell.Style.BackColor = Color.FromArgb(51, 122, 255);
                dgv.Columns["month_around_in_shipment"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["month_around_in_shipment"].DefaultCellStyle.ForeColor = Color.Blue;
                dgv.Columns["month_around_in_shipment"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["month_around_in_shipment"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마

                dgv.Columns["month_around_in_shipment2"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["month_around_in_shipment2"].HeaderCell.Style.BackColor = Color.FromArgb(51, 122, 255);
                dgv.Columns["month_around_in_shipment2"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["month_around_in_shipment2"].DefaultCellStyle.ForeColor = Color.Blue;
                dgv.Columns["month_around_in_shipment2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["month_around_in_shipment2"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마

                dgv.Columns["month_around_1"].HeaderCell.Style.BackColor = Color.LightCoral;
                dgv.Columns["month_around_1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["month_around_1"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마

                dgv.Columns["month_around_45"].HeaderCell.Style.BackColor = Color.LightCoral;
                dgv.Columns["month_around_45"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["month_around_45"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마

                dgv.Columns["month_around_2"].HeaderCell.Style.BackColor = Color.LightCoral;
                dgv.Columns["month_around_2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["month_around_2"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마

                dgv.Columns["month_around_3"].HeaderCell.Style.BackColor = Color.LightCoral;
                dgv.Columns["month_around_3"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["month_around_3"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마

                dgv.Columns["month_around_6"].HeaderCell.Style.BackColor = Color.LightCoral;
                dgv.Columns["month_around_6"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["month_around_6"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마

                dgv.Columns["month_around_12"].HeaderCell.Style.BackColor = Color.LightCoral;
                dgv.Columns["month_around_12"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["month_around_12"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마

                dgv.Columns["month_around_18"].HeaderCell.Style.BackColor = Color.LightCoral;
                dgv.Columns["month_around_18"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["month_around_18"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마


                dgv.Columns["enable_sales_days"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
                dgv.Columns["enable_sales_days"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
                dgv.Columns["enable_sales_days"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["enable_sales_days"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["month_around_1"].DefaultCellStyle.Font = new Font("DejaVu Sans Mono", 9, FontStyle.Bold);
                dgv.Columns["month_around_1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgv.Columns["month_around_45"].DefaultCellStyle.Font = new Font("DejaVu Sans Mono", 9, FontStyle.Bold);
                dgv.Columns["month_around_45"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgv.Columns["month_around_2"].DefaultCellStyle.Font = new Font("DejaVu Sans Mono", 9, FontStyle.Bold);
                dgv.Columns["month_around_2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgv.Columns["month_around_3"].DefaultCellStyle.Font = new Font("DejaVu Sans Mono", 9, FontStyle.Bold);
                dgv.Columns["month_around_3"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgv.Columns["month_around_6"].DefaultCellStyle.Font = new Font("DejaVu Sans Mono", 9, FontStyle.Bold);
                dgv.Columns["month_around_6"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgv.Columns["month_around_12"].DefaultCellStyle.Font = new Font("DejaVu Sans Mono", 9, FontStyle.Bold);
                dgv.Columns["month_around_12"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgv.Columns["month_around_18"].DefaultCellStyle.Font = new Font("DejaVu Sans Mono", 9, FontStyle.Bold);
                dgv.Columns["month_around_18"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;


                dgv.Columns["up"].HeaderCell.Style.BackColor = Color.Red;
                dgv.Columns["up"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                dgv.Columns["down"].HeaderCell.Style.BackColor = Color.Blue;
                dgv.Columns["down"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);

                dgv.Columns["exhausted_count"].DefaultCellStyle.Format = "#,##0";

                //즐겨찾기 항목==============================================================================================
                dgv.Columns["month_around2"].HeaderCell.Style.BackColor = Color.FromArgb(249, 200, 173);
                dgv.Columns["month_around2"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["month_around2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["month_around2"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마
                dgv.Columns["enable_sales_days2"].HeaderCell.Style.BackColor = Color.FromArgb(249, 200, 173);
                dgv.Columns["enable_sales_days2"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["enable_sales_days2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["enable_sales_days2"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
                dgv.Columns["createdatetime"].HeaderCell.Style.BackColor = Color.FromArgb(249, 200, 173);
                dgv.Columns["createdatetime"].HeaderCell.Style.ForeColor = Color.Black;


                // 보여지는 컬럼 넓이조정
                dgv.Columns["category"].Width = 50;
                dgv.Columns["product"].Width = 180;
                dgv.Columns["origin"].Width = 60;
                dgv.Columns["sizes"].Width = 100;
                dgv.Columns["unit"].Width = 40;
                dgv.Columns["price_unit"].Width = 40;
                dgv.Columns["unit_count"].Width = 40;
                dgv.Columns["bundle_count"].Width = 40;
                dgv.Columns["seaover_unit"].Width = 45;
                dgv.Columns["cost_unit"].Width = 40;
                dgv.Columns["sales_price"].Width = 60;
                dgv.Columns["margin"].Width = 50;
                dgv.Columns["purchase_price"].Width = 60;
                dgv.Columns["sales_cost_price"].Width = 60;
                dgv.Columns["pending_cost_price"].Width = 60;
                dgv.Columns["offer_cost_price"].Width = 60;
                dgv.Columns["offer_cost_price_margin"].Width = 50;
                dgv.Columns["average_sales_cost_price1"].Width = 65;
                dgv.Columns["average_sales_cost_price2"].Width = 65;
                dgv.Columns["average_sales_cost_price1_margin"].Width = 50;
                dgv.Columns["average_sales_cost_price2_margin"].Width = 50;
                dgv.Columns["average_purchase_price"].Width = 60;
                dgv.Columns["average_purchase_price_rank"].Width = 60;
                dgv.Columns["normal_price_rate"].Width = 60;

                dgv.Columns["seaover_unpending"].Width = 50;
                dgv.Columns["seaover_pending"].Width = 50;
                dgv.Columns["reserved_stock"].Width = 50;
                dgv.Columns["reserved_stock_details"].Width = 200;

                dgv.Columns["offer_updatetime"].Width = 70;
                dgv.Columns["offer_price"].Width = 65;
                dgv.Columns["offer_company"].Width = 60;
                dgv.Columns["order_qty"].Width = 60;
                dgv.Columns["order_etd"].Width = 70;
                dgv.Columns["total_real_stock"].Width = 60;

                dgv.Columns["custom"].Width = 50;
                dgv.Columns["tax"].Width = 50;
                dgv.Columns["incidental_expense"].Width = 50;
                dgv.Columns["fixed_tariff"].Width = 50;

                dgv.Columns["shipment_qty"].Width = 50;
                dgv.Columns["unpending_qty_after"].Width = 50;
                dgv.Columns["unpending_qty_before"].Width = 50;
                dgv.Columns["real_stock"].Width = 50;
                dgv.Columns["shipment_stock"].Width = 50;
                dgv.Columns["total_stock"].Width = 50;

                dgv.Columns["sales_count"].Width = 60;
                dgv.Columns["excluded_qty"].Width = 60;
                dgv.Columns["average_day_sales_count"].Width = 60;
                dgv.Columns["average_month_sales_count"].Width = 60;
                dgv.Columns["month_around"].Width = 60;
                dgv.Columns["month_around_in_shipment"].Width = 60;
                dgv.Columns["month_around_in_shipment2"].Width = 60;
                dgv.Columns["enable_sales_days"].Width = 70;

                dgv.Columns["up"].Width = 20;
                dgv.Columns["down"].Width = 20;
                dgv.Columns["base_around_month"].Width = 40;

                dgv.Columns["month_around2"].Width = 60;
                dgv.Columns["enable_sales_days2"].Width = 70;
                dgv.Columns["createdatetime"].Width = 70;

                dgv.Columns["exhausted_date"].Width = 75;
                dgv.Columns["exhausted_count"].Width = 60;
                dgv.Columns["contract_date"].Width = 75;
                dgv.Columns["until_days1"].Width = 50;
                dgv.Columns["min_etd"].Width = 75;
                dgv.Columns["until_days2"].Width = 50;

                dgv.Columns["hide_until_date"].Width = 75;

                dgv.Columns["division"].Width = 40;
                dgv.Columns["manager1"].Width = 50;
                dgv.Columns["manager2"].Width = 50;
                dgv.Columns["manager3"].Width = 50;
                //틀고정
                dgv.Columns["category"].Frozen = true;
                dgv.Columns["product"].Frozen = true;
                dgv.Columns["origin"].Frozen = true;
                dgv.Columns["sizes"].Frozen = true;
                dgv.Columns["unit"].Frozen = true;


                //사용자 설정값==============================================================================
                if (!isInit)
                {
                    foreach (string key in styleDic.Keys)
                    {
                        if (!string.IsNullOrEmpty(key) && key != "sort")
                        {
                            int width = Convert.ToInt32(styleDic[key]);
                            if (width < 0)
                            {
                                dgvProduct.Columns[key].Width = -width;
                                dgvProduct.Columns[key].Visible = false;
                            }
                            else
                            {
                                dgvProduct.Columns[key].Width = width;
                                dgvProduct.Columns[key].Visible = true;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Method2
        private void ToHandlingForm()
        {
            dgvProduct.EndEdit();
            //Check 여부
            int cnt = 0;
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                bool isChecked = Convert.ToBoolean(dgvProduct.Rows[i].Selected);
                if (isChecked)
                    cnt += 1;
            }
            //유효성검사
            if (cnt == 0)
            {
                msgBox.Show(this, "선택한 품목이 없습니다.");
                this.Activate();
                return;
            }
            else
            {
                DataTable productDt = new DataTable("Prodcut");

                DataColumn col1 = new DataColumn("category");
                col1.DataType = System.Type.GetType("System.String");
                col1.Namespace = "category";
                productDt.Columns.Add(col1);

                DataColumn col2 = new DataColumn("category_code");
                col2.DataType = System.Type.GetType("System.String");
                col2.Namespace = "category_code";
                productDt.Columns.Add(col2);

                DataColumn col3 = new DataColumn("category1");
                col3.DataType = System.Type.GetType("System.String");
                col3.Namespace = "category1";
                productDt.Columns.Add(col3);

                DataColumn col4 = new DataColumn("category2");
                col4.DataType = System.Type.GetType("System.String");
                col4.Namespace = "category2";
                productDt.Columns.Add(col4);

                DataColumn col5 = new DataColumn("category3");
                col5.DataType = System.Type.GetType("System.String");
                col5.Namespace = "category3";
                productDt.Columns.Add(col5);

                DataColumn col6 = new DataColumn("product");
                col6.DataType = System.Type.GetType("System.String");
                col6.Namespace = "product";
                productDt.Columns.Add(col6);

                DataColumn col7 = new DataColumn("product_code");
                col7.DataType = System.Type.GetType("System.String");
                col7.Namespace = "product_code";
                productDt.Columns.Add(col7);

                DataColumn col8 = new DataColumn("origin");
                col8.DataType = System.Type.GetType("System.String");
                col8.Namespace = "origin";
                productDt.Columns.Add(col8);

                DataColumn col9 = new DataColumn("origin_code");
                col9.DataType = System.Type.GetType("System.String");
                col9.Namespace = "origin_code";
                productDt.Columns.Add(col9);

                DataColumn col10 = new DataColumn("sizes");
                col10.DataType = System.Type.GetType("System.String");
                col10.Namespace = "sizes";
                productDt.Columns.Add(col10);

                DataColumn col11 = new DataColumn("sizes_code");
                col11.DataType = System.Type.GetType("System.String");
                col11.Namespace = "sizes_code";
                productDt.Columns.Add(col11);

                DataColumn col12 = new DataColumn("sales_price");
                col12.DataType = System.Type.GetType("System.String");
                col12.Namespace = "sales_price";
                productDt.Columns.Add(col12);

                DataColumn col13 = new DataColumn("purchase_price");
                col13.DataType = System.Type.GetType("System.String");
                col13.Namespace = "purchase_price";
                productDt.Columns.Add(col13);

                DataColumn col14 = new DataColumn("price_unit");
                col14.DataType = System.Type.GetType("System.String");
                col14.Namespace = "price_unit";
                productDt.Columns.Add(col14);

                DataColumn col15 = new DataColumn("unit");
                col15.DataType = System.Type.GetType("System.String");
                col15.Namespace = "unit";
                productDt.Columns.Add(col15);

                DataColumn col16 = new DataColumn("unit_count");
                col16.DataType = System.Type.GetType("System.String");
                col16.Namespace = "unit_count";
                productDt.Columns.Add(col16);

                DataColumn col17 = new DataColumn("seaover_unit");
                col17.DataType = System.Type.GetType("System.String");
                col17.Namespace = "seaover_unit";
                productDt.Columns.Add(col17);

                DataColumn col18 = new DataColumn("division");
                col18.DataType = System.Type.GetType("System.String");
                col18.Namespace = "division";
                productDt.Columns.Add(col18);

                DataColumn col19 = new DataColumn("manager1");
                col19.DataType = System.Type.GetType("System.String");
                col19.Namespace = "manager1";
                productDt.Columns.Add(col19);

                DataColumn col20 = new DataColumn("weight");
                col20.DataType = System.Type.GetType("System.String");
                col20.Namespace = "weight";
                productDt.Columns.Add(col20);
                //Datatable <= data
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[i].Selected);
                    if (isChecked && dgvProduct.Rows[i].Visible)
                    {
                        DataRow dr = productDt.NewRow();
                        dr["category_code"] = dgvProduct.Rows[i].Cells["category_code"].Value;
                        dr["category"] = dgvProduct.Rows[i].Cells["category"].Value;
                        dr["category1"] = dgvProduct.Rows[i].Cells["category_code"].Value;
                        dr["category2"] = dgvProduct.Rows[i].Cells["category2"].Value;
                        dr["category3"] = dgvProduct.Rows[i].Cells["category3"].Value;
                        dr["product"] = dgvProduct.Rows[i].Cells["product"].Value;
                        dr["product_code"] = dgvProduct.Rows[i].Cells["product_code"].Value;
                        dr["origin"] = dgvProduct.Rows[i].Cells["origin"].Value;
                        dr["origin_code"] = dgvProduct.Rows[i].Cells["origin_code"].Value;
                        dr["sizes"] = dgvProduct.Rows[i].Cells["sizes"].Value;
                        dr["sizes_code"] = dgvProduct.Rows[i].Cells["sizes_code"].Value;
                        dr["sales_price"] = dgvProduct.Rows[i].Cells["sales_price"].Value;
                        dr["purchase_price"] = dgvProduct.Rows[i].Cells["purchase_price"].Value;
                        dr["price_unit"] = dgvProduct.Rows[i].Cells["price_unit"].Value;
                        dr["unit"] = dgvProduct.Rows[i].Cells["unit"].Value;
                        dr["unit_count"] = dgvProduct.Rows[i].Cells["unit_count"].Value;
                        dr["seaover_unit"] = dgvProduct.Rows[i].Cells["seaover_unit"].Value;
                        dr["division"] = dgvProduct.Rows[i].Cells["division"].Value;
                        dr["manager1"] = dgvProduct.Rows[i].Cells["manager1"].Value;
                        //대분류
                        if (dr["category2"].ToString() == "Aa") dr["category"] = "라운드새우류";
                        else if (dr["category2"].ToString() == "Ab") dr["category"] = "새우살류";
                        else if (dr["category2"].ToString() == "Ba") dr["category"] = "쭈꾸미류";
                        else if (dr["category2"].ToString() == "Bb") dr["category"] = "낙지류";
                        else if (dr["category2"].ToString() == "Bc") dr["category"] = "갑오징어류";
                        else if (dr["category2"].ToString() == "Bd") dr["category"] = "오징어류";
                        else if (dr["category2"].ToString() == "Be") dr["category"] = "문어류";
                        else if (dr["category2"].ToString() == "C") dr["category"] = "갑각류";
                        else if (dr["category2"].ToString() == "Da") dr["category"] = "어패류";
                        else if (dr["category2"].ToString() == "Db") dr["category"] = "살류";
                        else if (dr["category2"].ToString() == "Dc") dr["category"] = "해물류";
                        else if (dr["category2"].ToString() == "Ea") dr["category"] = "초밥/일식류";
                        else if (dr["category2"].ToString() == "Eb") dr["category"] = "기타가공품(튀김, 가금류)";
                        else if (dr["category2"].ToString() == "Ec") dr["category"] = "기타가공품(야채, 과일)";
                        else if (dr["category2"].ToString() == "F") dr["category"] = "선어류";

                        //중량
                        DataGridViewRow row = dgvProduct.Rows[i];
                        if (row.Cells["price_unit"].Value.ToString() == "팩")
                        {
                            if (Convert.ToDouble(row.Cells["unit"].Value) < 1)
                            {
                                dr["weight"] = dr["seaover_unit"] + "kg \n (" + (Convert.ToDouble(dr["unit"]) * 1000).ToString() + "g x " + dr["unit_count"] + "p)";
                            }
                            else
                            {
                                dr["weight"] = dr["seaover_unit"] + "kg \n (" + dr["unit"] + "k x " + dr["unit_count"] + "p)";
                            }
                        }
                        //kg
                        else if (row.Cells["price_unit"].Value.ToString() == "kg" || row.Cells["price_unit"].Value.ToString() == "Kg" || row.Cells["price_unit"].Value.ToString() == "KG")
                        {

                            if (Convert.ToDouble(row.Cells["unit"].Value) < 1)
                            {
                                dr["weight"] = dr["seaover_unit"] + "kg \n (" + (Convert.ToDouble(dr["unit"]) * 1000).ToString() + "g x " + dr["unit_count"] + "p)";
                            }
                            else
                            {
                                dr["weight"] = dr["seaover_unit"] + "kg \n (" + dr["unit"] + "k x " + dr["unit_count"] + "p)";
                            }
                        }
                        //묶음
                        else if (row.Cells["price_unit"].Value.ToString() == "묶" || row.Cells["price_unit"].Value.ToString() == "묶음")
                        {
                            dr["weight"] = dr["unit"] + "kg";
                        }
                        //나머지
                        else
                        {
                            dr["weight"] = dr["seaover_unit"] + "kg";
                        }

                        productDt.Rows.Add(dr);
                    }
                }
                //Open
                try
                {
                    SEAOVER._2Line._2LineForm form = new _2Line._2LineForm(14, um, productDt);
                    form.Show();
                    form.WindowState = FormWindowState.Maximized;
                }
                catch
                { }

                //form.BringToFront();
            }
        }
        private void OpenExhaustedManager(int rowIndex)
        {
            DataGridViewRow row = dgvProduct.Rows[rowIndex];

            ShortManagerModel smModel = new ShortManagerModel();
            smModel.category = row.Cells["category"].Value.ToString();
            smModel.product = row.Cells["product"].Value.ToString();
            smModel.origin = row.Cells["origin"].Value.ToString();
            smModel.sizes = row.Cells["sizes"].Value.ToString();
            smModel.unit = row.Cells["seaover_unit"].Value.ToString();

            //매출기간
            int salesMonth = 6;
            switch (cbSaleTerm.Text)
            {
                case "1개월":
                    salesMonth = 1;
                    break;
                case "45일":
                    salesMonth = 45;
                    break;
                case "2개월":
                    salesMonth = 2;
                    break;
                case "3개월":
                    salesMonth = 3;
                    break;
                case "6개월":
                    salesMonth = 6;
                    break;
                case "12개월":
                    salesMonth = 12;
                    break;
                case "18개월":
                    salesMonth = 18;
                    break;
                default:
                    salesMonth = 6;
                    break;
            }
            DateTime stockDate;
            if (!DateTime.TryParse(txtStockDate.Text, out stockDate))
                stockDate = DateTime.Now;

            //영업일 수
            int workDays = 0;
            string sDate = stockDate.AddMonths(-salesMonth).ToString("yyyy-MM-dd");
            if (salesMonth == 45)
                sDate = DateTime.Now.AddDays(-salesMonth).ToString("yyyy-MM-dd");
            string eDate = stockDate.ToString("yyyy-MM-dd");
            common.GetWorkDay(Convert.ToDateTime(sDate), Convert.ToDateTime(eDate), out workDays);

            if (eDate == DateTime.Now.ToString("yyyy-MM-dd"))
                workDays--;

            //판매량
            double salesCnt = Convert.ToDouble(row.Cells["sales_count"].Value.ToString().Replace(",", ""));
            double avg_day_sales = Convert.ToDouble(row.Cells["average_day_sales_count_double"].Value.ToString());
            double avg_month_sales = Convert.ToDouble(row.Cells["average_month_sales_count"].Value.ToString());

            //model 
            smModel.avg_sales_day = avg_day_sales;
            smModel.avg_sales_month = avg_month_sales;
            smModel.real_stock = Convert.ToDouble(row.Cells["seaover_unpending"].Value.ToString()) + Convert.ToDouble(row.Cells["seaover_pending"].Value.ToString()) - Convert.ToDouble(row.Cells["reserved_stock"].Value.ToString());           //최초 소진일자 계산
            //smModel.real_stock = Convert.ToDouble(row.Cells["seaover_unpending"].Value.ToString()) + Convert.ToDouble(row.Cells["seaover_pending"].Value.ToString());           //최초 소진일자 계산
            DateTime exhausted_date;
            double exhausted_cnt;
            common.GetExhausedDateDayd(DateTime.Now, smModel.real_stock, smModel.avg_sales_day, 0, null, out exhausted_date, out exhausted_cnt);

            smModel.exhaust_date = exhausted_date.ToString("yyyy-MM-dd");
            DateTime enddate = stockDate;
            smModel.enddate = enddate.ToString("yyyy-MM-dd");

            int production_days;
            if (row.Cells["production_days"].Value == null || !int.TryParse(row.Cells["production_days"].Value.ToString(), out production_days))
                production_days = 20;

            StockManager sm = new StockManager(smModel, production_days);
            int mainFormX = this.Location.X;
            int mainFormY = this.Location.Y;
            int mainFormWidth = this.Size.Width;
            int mainFormHeight = this.Size.Height;

            int childFormWidth = sm.Size.Width;
            int childFormHeight = sm.Size.Height;
            try
            {
                sm.StartPosition = FormStartPosition.CenterParent;
                sm.Show();
                sm.Location = new Point(mainFormX + (mainFormWidth / 2) - (childFormWidth / 2), mainFormY + (mainFormHeight / 2) - (childFormHeight / 2));
            }
            catch
            { }
        }
        //일괄제외설정
        private void BatchHide()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "업체별시세관리", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            //유효성검사
            int cnt = 0;
            foreach (DataGridViewRow row in dgvProduct.Rows)
            {
                bool isChecked = Convert.ToBoolean(row.Selected);
                if (isChecked)
                {
                    cnt++;
                }
            }
            if (cnt == 0)
            {
                msgBox.Show(this, "선택한 내역이 없습니다.");
                this.Activate();
                return;
            }
            //설정값
            HideAllProductManager hpm = new HideAllProductManager();
            hpm.StartPosition = FormStartPosition.CenterParent;
            HideModel settingModel;
            bool isApply = hpm.SetHIdeProduct(out settingModel);
            //설정했을 경우만
            if (isApply)
            {
                List<StringBuilder> sqlList = new List<StringBuilder>();
                int id = commonRepository.GetNextId("t_hide", "id");

                foreach (DataGridViewRow row in dgvProduct.Rows)
                {
                    bool isChecked = Convert.ToBoolean(row.Selected);
                    if (isChecked)
                    {
                        HideModel model = new HideModel();
                        model.id = id;
                        model.division = "업체별시세현황";
                        model.category = row.Cells["category"].Value.ToString();
                        model.product = row.Cells["product"].Value.ToString();
                        model.origin = row.Cells["origin"].Value.ToString();
                        model.sizes = row.Cells["sizes"].Value.ToString();
                        model.seaover_unit = row.Cells["seaover_unit"].Value.ToString();
                        model.hide_mode = settingModel.hide_mode;
                        model.hide_details = settingModel.hide_details;
                        model.until_date = settingModel.until_date;
                        model.remark = settingModel.remark;
                        model.edit_user = um.user_name;
                        model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");

                        id++;

                        StringBuilder sql = hideRepository.InsertHide(model);
                        sqlList.Add(sql);
                    }
                }
                //Execute
                if (sqlList.Count > 0)
                {
                    int results = hideRepository.UpdateTrans(sqlList);
                    if (results == -1)
                    {
                        msgBox.Show(this, "등록중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                    else
                    {
                        msgBox.Show(this, "등록완료");
                        this.Activate();
                    }
                }
            }
        }
        //숫자콤마
        private string GetN2(string A)
        {
            double B = Convert.ToDouble(A);
            string result;
            if (B == (int)B)
                result = B.ToString("#,##0");
            else
                result = B.ToString("N2");

            return result;
        }
        //DatagridView Double Buffered Setting
        private void Init_DataGridView()
        {
            dgvProduct.DoubleBuffered(true);
        }
        private void CallSalesProcedure2()
        {
            //품명별매출현황 스토어프로시져 호출
            try
            {
                string user_id = um.seaover_id;
                priceComparisonRepository.SetSeaoverId(user_id);
            }
            catch (Exception e)
            {
                msgBox.Show(this, e.Message);
                this.Activate();
                return;
            }
        }
        private void CallProcedure()
        {
            try
            {
                timer_start();
                processingFlag = true;
                //업체별시세현황 스토어프로시져 호출
                CallProductProcedure();
                //품명별재고현황 스토어프로시져 호출
                CallStockProcedure();
                //품명별매출현황 스토어프로시져 호출
                CallSalesProcedure();
                dgvProduct.Rows.Clear();
                processingFlag = false;
            }
            catch
            {
                msgBox.Show(this, "최신화 중 출동이 일어났습니다. 잠시후 다시 시도해주시기 바랍니다.");
                this.Activate();
            }
        }
        private void CallProductProcedure()
        {
            //업체별시세현황 스토어프로시져 호출
            try
            {
                string sDate = Convert.ToDateTime(txtdtpSttdateYear.Text + "-" + txtdtpSttdateMonth.Text + "-" + txtdtpSttdateDay.Text).AddYears(-2).ToString("yyyy-MM-dd");
                string eDate = Convert.ToDateTime(txtdtpEnddateYear.Text + "-" + txtdtpEnddateMonth.Text + "-" + txtdtpEnddateDay.Text).ToString("yyyy-MM-dd");
                string user_id = um.seaover_id;
                ////업체별시세현황 스토어프로시져 호출
                if (seaoverRepository.CallStoredProcedure(user_id, sDate, eDate) == 0)
                {
                    msgBox.Show(this, "호출 내용이 없음");
                    this.Activate();
                    processingFlag = false;
                    return;
                }
            }
            catch (Exception e)
            {
                msgBox.Show(this, e.Message);
                this.Activate();
                processingFlag = false;
                return;
            }

        }
        private void CallStockProcedure()
        {
            //품명별재고현황 스토어프로시져 호출
            try
            {
                DateTime enddate;
                if (!DateTime.TryParse(txtStockDate.Text, out enddate))
                    enddate = DateTime.Now;

                string sDate = enddate.AddYears(-2).ToString("yyyy-MM-dd");
                string eDate = enddate.ToString("yyyy-MM-dd");
                string user_id = um.seaover_id;
                if (priceComparisonRepository.CallStoredProcedureSTOCK(user_id, eDate) == 0)
                {
                    msgBox.Show(this, "호출 내용이 없음");
                    this.Activate();
                    processingFlag = false;

                    return;
                }
            }
            catch (Exception e)
            {
                msgBox.Show(this, e.Message);
                this.Activate();
                processingFlag = false;
                return;
            }

        }
        private void CallSalesProcedure()
        {
            //품명별매출현황 스토어프로시져 호출
            try
            {
                string sDate = Convert.ToDateTime(txtdtpSttdateYear.Text + "-" + txtdtpSttdateMonth.Text + "-" + txtdtpSttdateDay.Text).ToString("yyyy-MM-dd");
                string eDate = Convert.ToDateTime(txtdtpEnddateYear.Text + "-" + txtdtpEnddateMonth.Text + "-" + txtdtpEnddateDay.Text).ToString("yyyy-MM-dd");
                string user_id = um.seaover_id;
                if (priceComparisonRepository.CallStoredProcedure(user_id, sDate, eDate) == 0)
                {
                    msgBox.Show(this, "호출 내용이 없음");
                    this.Activate();
                    processingFlag = false;
                    return;
                }
                else
                {
                    dgvProduct.Rows.Clear();
                }
            }
            catch (Exception e)
            {
                msgBox.Show(this, e.Message);
                this.Activate();
                processingFlag = false;
                return;
            }
        }

        #endregion

        #region comboBox, checkbox check
        private void rbCostprice_CheckedChanged(object sender, EventArgs e)
        {
            AllRowCalculateCostPrice();
        }
        private void cbDongwon_CheckedChanged(object sender, EventArgs e)
        {
            AllRowCalculateCostPrice();
        }

        private void rbDirectReflectionShipmentQty_CheckedChanged(object sender, EventArgs e)
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            if (rbNotReflectionShipmentQty.Checked)
            {
                if (dgvProduct.Rows.Count > 0)
                {
                    //날짜계산
                    if (cbExhaustedDate.Checked)
                    {
                        CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dgvProduct.DataSource];
                        currencyManager1.SuspendBinding();
                        foreach (DataGridViewRow row in dgvProduct.Rows)
                        {
                            if (Convert.ToInt32(row.Cells["main_id"].Value) == 0 || (Convert.ToInt32(row.Cells["main_id"].Value) > 0 && row.Cells["sub_id"].Value.ToString() == "9999"))
                            {
                                double order_qty;
                                if (row.Cells["order_qty"].Value == null || !double.TryParse(row.Cells["order_qty"].Value.ToString(), out order_qty))
                                    order_qty = 0;
                                SetData(row, order_qty);
                            }
                            else
                                row.Visible = false;
                        }
                        currencyManager1.ResumeBinding();
                    }
                    //전체회전율
                    if (cbAllTerm.Checked)
                        GetAllQtySumaryTerm();
                }
            }
            else if (rbDirectReflectionShipmentQty.Checked)
            {
                //날짜계산
                if (cbExhaustedDate.Checked)
                {
                    CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dgvProduct.DataSource];
                    currencyManager1.SuspendBinding();
                    foreach (DataGridViewRow row in dgvProduct.Rows)
                    {
                        if (Convert.ToInt32(row.Cells["main_id"].Value) == 0 || (Convert.ToInt32(row.Cells["main_id"].Value) > 0 && row.Cells["sub_id"].Value.ToString() == "9999"))
                        {
                            double order_qty;
                            if (row.Cells["order_qty"].Value == null || !double.TryParse(row.Cells["order_qty"].Value.ToString(), out order_qty))
                                order_qty = 0;
                            SetData(row, order_qty);
                        }
                        else
                            row.Visible = false;
                    }
                    currencyManager1.ResumeBinding();
                }
                //전체회전율
                if (cbAllTerm.Checked)
                    GetAllQtySumaryTerm();
            }
            else if (rbDetailReflectionShipmentQty.Checked)
            {
                //날짜계산
                if (cbExhaustedDate.Checked)
                {
                    CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dgvProduct.DataSource];
                    currencyManager1.SuspendBinding();
                    foreach (DataGridViewRow row in dgvProduct.Rows)
                    {
                        if (Convert.ToInt32(row.Cells["main_id"].Value) == 0 || (Convert.ToInt32(row.Cells["main_id"].Value) > 0 && row.Cells["sub_id"].Value.ToString() == "9999"))
                        {
                            double order_qty;
                            if (row.Cells["order_qty"].Value == null || !double.TryParse(row.Cells["order_qty"].Value.ToString(), out order_qty))
                                order_qty = 0;
                            SetData(row, order_qty);
                        }
                        else
                            row.Visible = false;
                    }
                    currencyManager1.ResumeBinding();
                }
                //전체회전율
                if (cbAllTerm.Checked)
                    GetAllQtySumaryTerm();
            }

            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }
        private void cbShipmentRate_CheckedChanged(object sender, EventArgs e)
        {
            if (cbExhaustedDate.Checked && dgvProduct.Rows.Count > 0)
            {
                if (msgBox.Show(this, "현재 출력된 품목의 추천계약일, 소진일자를 다시 계산할까요?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                    foreach (DataGridViewRow row in dgvProduct.Rows)
                    {
                        double order_qty;
                        if (row.Cells["order_qty"].Value == null || !double.TryParse(row.Cells["order_qty"].Value.ToString(), out order_qty))
                            order_qty = 0;

                        SetData(row, order_qty);
                    }
                    this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                }
            }
        }

        private void cbTerm_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = cbTerm.SelectedIndex;
            if (idx > -1)
            {
                int m = Convert.ToInt16(cbTerm.Items[idx].ToString().Replace("개월", "").Trim());
                dtpSttdate.Value = dtpSttdate.Value.AddMonths(-m);
            }
        }
        private void cbOffer_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvProduct.Columns.Count > 0)
            {
                dgvProduct.Columns["tax"].Visible = cbOffer.Checked;
                dgvProduct.Columns["custom"].Visible = cbOffer.Checked;
                dgvProduct.Columns["incidental_expense"].Visible = cbOffer.Checked;
                dgvProduct.Columns["fixed_tariff"].Visible = cbOffer.Checked;
                dgvProduct.Columns["offer_price"].Visible = cbOffer.Checked;
                dgvProduct.Columns["offer_updatetime"].Visible = cbOffer.Checked;
                dgvProduct.Columns["offer_company"].Visible = cbOffer.Checked;
                dgvProduct.Columns["total_real_stock"].Visible = cbOffer.Checked;
                dgvProduct.Columns["order_qty"].Visible = cbOffer.Checked;
                dgvProduct.Columns["order_etd"].Visible = cbOffer.Checked;
                dgvProduct.Columns["average_sales_cost_price2"].Visible = cbOffer.Checked;
                dgvProduct.Columns["average_sales_cost_price2_margin"].Visible = cbOffer.Checked;
                dgvProduct.Columns["offer_cost_price"].Visible = cbOffer.Checked;
                dgvProduct.Columns["offer_cost_price_margin"].Visible = cbOffer.Checked;
                SetDgvMergeProductVisible();
            }
            //txtTotalOrderQty.Visible = cbOffer.Checked;
        }

        private void cbCostPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvProduct.Columns.Count > 0)
            {
                dgvProduct.Columns["pending_cost_price"].Visible = cbCostPrice.Checked;
                dgvProduct.Columns["average_sales_cost_price1"].Visible = cbCostPrice.Checked;
                dgvProduct.Columns["average_sales_cost_price1_margin"].Visible = cbCostPrice.Checked;
                SetDgvMergeProductVisible();
            }
        }
        private void cbReservationDetails_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvProduct.Columns.Count > 0)
            {
                dgvProduct.Columns["seaover_unpending"].Visible = cbReservationDetails.Checked;
                dgvProduct.Columns["seaover_pending"].Visible = cbReservationDetails.Checked;
                dgvProduct.Columns["reserved_stock"].Visible = cbReservationDetails.Checked;

                dgvProduct.Columns["shipment_qty"].Visible = cbReservationDetails.Checked;
                dgvProduct.Columns["unpending_qty_before"].Visible = cbReservationDetails.Checked;
                //dgvProduct.Columns["reserved_stock_details"].Visible = cbReservationDetails.Checked;

                /*dgvProduct.Columns["month_around_in_shipment"].Visible = cbReservationDetails.Checked;
                dgvProduct.Columns["month_around_out_shipment"].Visible = cbReservationDetails.Checked;*/
                        SetDgvMergeProductVisible();
            }
        }

        private void SetDgvMergeProductVisible()
        {
            //이미 열려있는 폼이 있는지 확인
            FormCollection fc = Application.OpenForms;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "MergeProduct")
                {
                    MergeProduct mp = (MergeProduct)frm;
                    mp.SetDgvMergeProductVisible(dgvProduct);

                    break;
                }
            }
        }

        private void cbMarginMinPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvProduct.Columns.Count > 0)
            {
                dgvProduct.Columns["margin"].Visible = cbMarginMinPrice.Checked;
                dgvProduct.Columns["purchase_price"].Visible = cbMarginMinPrice.Checked;
                SetDgvMergeProductVisible();
            }
        }

        private void cbSalesCount_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvProduct.Columns.Count > 0)
            {
                dgvProduct.Columns["sales_count"].Visible = cbSalesCount.Checked;
                dgvProduct.Columns["average_day_sales_count"].Visible = cbSalesCount.Checked;
                dgvProduct.Columns["excluded_qty"].Visible = cbSalesCount.Checked;
                SetDgvMergeProductVisible();
            }
        }
        #endregion

        #region Key Event
        private void txtTrq_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (msgBox.Show(this, "TRQ 기준금액을 변경하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    double trq_price;
                    if (!double.TryParse(txtTrq.Text.Replace(",", ""), out trq_price))
                        trq_price = 0;

                    if (commonRepository.UpdateTrq(trq_price) == -1)
                        msgBox.Show(this, "등록중 에러가 발생하였습니다.");
                    else
                        msgBox.Show(this, "완료");

                    this.Activate();
                }
            }
        }
        private void txtExchangeRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                changeExchangeRate();
            }
        }
        private void txtSttdate_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(45) || e.KeyChar == Convert.ToChar(47)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }
        private void txtSttdate_KeyDown(object sender, KeyEventArgs e)
        {
            Control tb = (Control)sender;
            if (e.KeyCode == Keys.Enter)
            {
                //날짜 완성
                Control tbb = (Control)sender;
                tbb.Text = common.strDatetime(tbb.Text);
            }
        }
        private void dgvProduct_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control && e.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        common.GetDgvSelectCellsCapture(dgvProduct);
                        break;
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Libs.Tools.Common common = new Libs.Tools.Common();
            Control tb = common.FindFocusedControl(this);
            if (tb != null && tb.Name != null && tb.Name != "dgvProduct" && tb.Name.Length >= 3 && tb.Name.Substring(0, 3) == "txt")
            {
                switch (keyData)
                {
                    case Keys.Up:
                        tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
                        return true;
                    case Keys.Down:
                        tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                        return true;

                    case Keys.Left:
                        {
                            if (string.IsNullOrEmpty(tb.Text))
                                tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
                            else
                            {
                                TextBox txtBox = (TextBox)tb;
                                int line = txtBox.GetLineFromCharIndex(txtBox.SelectionStart);
                                int column = txtBox.SelectionStart - txtBox.GetFirstCharIndexFromLine(line);
                                if (column == 0)
                                    tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
                                else
                                    return base.ProcessCmdKey(ref msg, keyData);
                            }
                        }
                        return true;
                    case Keys.Right:
                        {
                            if (string.IsNullOrEmpty(tb.Text))
                                tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                            else
                            {
                                TextBox txtBox = (TextBox)tb;
                                int line = txtBox.GetLineFromCharIndex(txtBox.SelectionStart);
                                int column = txtBox.SelectionStart - txtBox.GetFirstCharIndexFromLine(line);
                                if (column == txtBox.Text.Length)
                                    tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                                else
                                    return base.ProcessCmdKey(ref msg, keyData);
                            }
                        }
                        return true;
                }
            }
            else if (dgvProduct.Focused && dgvProduct.SelectedRows.Count == 0 && dgvProduct.SelectedCells.Count > 0 && keyData == Keys.Delete)
            {
                int min_row = 99999, max_row = 0;
                foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                {
                    if (min_row >= cell.RowIndex)
                        min_row = cell.RowIndex;
                    if (max_row <= cell.RowIndex)
                        max_row = cell.RowIndex;
                }

                for (int i = min_row; i <= max_row; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];
                    bool isExistSelectCell = false;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Selected)
                        {
                            cell.Value = "0";
                            isExistSelectCell = true;
                        }
                    }
                    if (isExistSelectCell)
                    {
                        double order_qty;
                        if (row.Cells["order_qty"].Value == null || !double.TryParse(row.Cells["order_qty"].Value.ToString(), out order_qty))
                            order_qty = 0;
                        SetData(row, order_qty);
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void txtSttPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    txtSttPrice.Text = GetN2(txtSttPrice.Text);
                    txtSttPrice.SelectionStart = txtSttPrice.TextLength; //** 캐럿을 맨 뒤로 보낸다...
                    txtSttPrice.SelectionLength = 0;
                }
                catch { }
            }

        }

        private void txtSttPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Back))
            {
            }
            else if (e.KeyChar == Convert.ToChar(Keys.Delete))
            {
            }
            else if (e.KeyChar == Convert.ToChar(46))
            {
            }
            else if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtEndPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    txtEndPrice.Text = GetN2(txtEndPrice.Text);
                    txtEndPrice.SelectionStart = txtEndPrice.TextLength; //** 캐럿을 맨 뒤로 보낸다...
                    txtEndPrice.SelectionLength = 0;
                }
                catch { }
            }
        }

        private void txtEndPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Back))
            {
            }
            else if (e.KeyChar == Convert.ToChar(Keys.Delete))
            {
            }
            else if (e.KeyChar == Convert.ToChar(46))
            {
            }
            else if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void PriceComparison_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        {
                            GetData();
                            break;
                        }
                    case Keys.D:
                        {
                            if (dgvProduct.SelectedRows.Count > 0)
                            {
                                List<string[]> productInfoList = new List<string[]>();
                                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                                {
                                    DataGridViewRow dgvRow = dgvProduct.Rows[i];
                                    bool isChecked = Convert.ToBoolean(dgvRow.Selected);
                                    if (isChecked)
                                    {
                                        string[] productInfo = new string[18];

                                        productInfo[0] = dgvRow.Cells["product"].Value.ToString();
                                        productInfo[1] = dgvRow.Cells["origin"].Value.ToString();
                                        productInfo[2] = dgvRow.Cells["sizes"].Value.ToString();
                                        productInfo[3] = dgvRow.Cells["seaover_unit"].Value.ToString();
                                        productInfo[4] = dgvRow.Cells["cost_unit"].Value.ToString();
                                        productInfo[5] = dgvRow.Cells["purchase_price"].Value.ToString();

                                        productInfo[6] = dgvRow.Cells["custom"].Value.ToString();
                                        productInfo[7] = dgvRow.Cells["tax"].Value.ToString();
                                        productInfo[8] = dgvRow.Cells["incidental_expense"].Value.ToString();
                                        productInfo[9] = dgvRow.Cells["purchase_margin"].Value.ToString();
                                        productInfo[10] = dgvRow.Cells["production_days"].Value.ToString();

                                        productInfo[11] = dgvRow.Cells["offer_price"].Value.ToString();
                                        productInfo[12] = dgvRow.Cells["offer_company"].Value.ToString();
                                        productInfo[13] = dgvRow.Cells["offer_cost_price"].Value.ToString();
                                        productInfo[14] = dgvRow.Cells["manager3"].Value.ToString();

                                        productInfo[15] = dgvRow.Cells["product_code"].Value.ToString();
                                        productInfo[16] = dgvRow.Cells["origin_code"].Value.ToString();
                                        productInfo[17] = dgvRow.Cells["sizes_code"].Value.ToString();



                                        productInfoList.Add(productInfo);
                                    }
                                }
                                try
                                {
                                    DetailDashboard dd = new DetailDashboard(um, productInfoList);
                                    dd.Show();
                                }
                                catch
                                { }
                            }
                            break;
                        }
                    case Keys.N:
                        {
                            txtCategory.Text = "";
                            txtProduct.Text = "";
                            txtOrigin.Text = "";
                            txtSizes.Text = "";
                            txtUnit.Text = "";
                            txtManager1.Text = "";
                            txtManager2.Text = "";
                            txtTradeManager.Text = "";
                            //txtDivision.Text = "";
                            txtProduct.Focus();
                            break;
                        }
                    case Keys.M:
                        {
                            txtProduct.Focus();
                            break;
                        }
                    case Keys.X:
                        {
                            this.Dispose();
                            break;
                        }
                    case Keys.W:
                        {
                            ToHandlingForm();
                            break;
                        }
                    case Keys.H:
                        {
                            BatchHide();
                            break;
                        }
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
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:

                        //이미 열려있는 폼이 있는지 확인
                        FormCollection fc = Application.OpenForms;
                        foreach (Form frm in fc)
                        {
                            //iterate through
                            if (frm.Name == "MergeProduct")
                                frm.Dispose();

                            break;
                        }

                        break;
                    case Keys.F1:
                        {
                            bool isCheck = !cbReservationDetails.Checked;
                            cbReservationDetails.Checked = isCheck;
                            break;
                        }
                    case Keys.F2:
                        {
                            bool isCheck = !cbSalesCount.Checked;
                            cbSalesCount.Checked = isCheck;
                            break;
                        }
                    case Keys.F3:
                        {
                            bool isCheck = !cbMarginMinPrice.Checked;
                            cbMarginMinPrice.Checked = isCheck;
                            break;
                        }
                    case Keys.F4:
                        {
                            bool isCheck = !cbCostPrice.Checked;
                            cbCostPrice.Checked = isCheck;
                            break;
                        }
                    case Keys.F5:
                        {
                            break;
                        }
                    case Keys.F6:
                        {
                            bool isCheck = !cbOffer.Checked;
                            cbOffer.Checked = isCheck;
                            break;
                        }
                    case Keys.F7:
                        {
                            bool isCheck = !cbExhaustedDate.Checked;
                            cbExhaustedDate.Checked = isCheck;
                            break;
                        }
                    case Keys.F8:
                        {
                            btnBookmark.PerformClick();
                            break;
                        }
                    case Keys.F9:
                        {
                            cbAllTerm.Checked = !cbAllTerm.Checked;
                            break;
                        }
                }
            }
        }
        private void txtCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetData();
            }
        }

        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetData();
            }
        }

        private void txtSizes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetData();
            }
        }

        private void txtUnit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetData();
            }
        }

        private void txtOrigin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetData();
            }
        }

        private void txtManager1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetData();
            }
        }

        private void txtManager2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetData();
            }
        }
        private void txtDivision_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetData();
            }
        }
        #endregion

        #region Button, Checkbox event
        private void txtTotalOrderMarginAmount0_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MarginDetails md = new MarginDetails(um, this.dgvProduct);
            md.Owner = this;
            md.Show();
        }
        private void cbShipment_CheckedChanged(object sender, EventArgs e)
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            GetAllQtySumaryTerm();
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            if (dgvProduct.SelectedCells.Count > 0)
            {
                Color color = btnColor.BackColor;
                foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                    cell.Style.BackColor = color;
            }
        }

        private void btnFontColor_Click(object sender, EventArgs e)
        {
            if (dgvProduct.SelectedCells.Count > 0)
            {
                Color color = btnFontColor.BackColor;
                foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                    cell.Style.ForeColor = color;
            }
        }
        private void btnColorStting_Click(object sender, EventArgs e)
        {
            ChangeBackColor();
        }

        private void btnFontColorSetting_Click(object sender, EventArgs e)
        {
            ChangeForeColor();
        }
        private void cbExhaustedDate_CheckedChanged(object sender, EventArgs e)
        {
            if (cbExhaustedDate.Checked && dgvProduct.Rows.Count > 0)
            {
                if (msgBox.Show(this, "현재 출력된 품목의 추천계약일, 소진일자를 다시 계산할까요?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                    foreach (DataGridViewRow row in dgvProduct.Rows)
                    {
                        if (row.Visible)
                        {
                            double order_qty;
                            if (row.Cells["order_qty"].Value == null || !double.TryParse(row.Cells["order_qty"].Value.ToString(), out order_qty))
                                order_qty = 0;

                            SetData(row, order_qty);
                        }
                    }
                    this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                    //서브 품목 숨기기
                    HideSubProduct();
                }
            }
        }
        private void cbAllTerm_CheckedChanged(object sender, EventArgs e)
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            if (cbAllTerm.Checked)
                GetAllQtySumaryTerm();
            else
            {
                if (dgvProduct.ColumnCount > 0)
                {
                    dgvProduct.Columns["month_around_1"].Visible = false;
                    dgvProduct.Columns["month_around_45"].Visible = false;
                    dgvProduct.Columns["month_around_2"].Visible = false;
                    dgvProduct.Columns["month_around_3"].Visible = false;
                    dgvProduct.Columns["month_around_6"].Visible = false;
                    dgvProduct.Columns["month_around_12"].Visible = false;
                    dgvProduct.Columns["month_around_18"].Visible = false;
                }
            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }
        private void btnStockDate_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtStockDate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }
        private void cbSortType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvProduct.ColumnCount > 0 && dgvProduct.RowCount > 0)
            {
                StyleSettingTxt();
                cookie.SaveSalesManagerSetting(styleDic);

                dgvProduct.ClearSelection();
                dgvProduct.SelectionMode = DataGridViewSelectionMode.CellSelect;

                DataTable tempDt = newDt.Copy();

                for (int i = 0; i < tempDt.Rows.Count; i++)
                {
                    DateTime exhausted_date;
                    if (tempDt.Rows[i]["exhausted_date"].ToString() == "1년이상 판매가능")
                        exhausted_date = new DateTime(2999, 12, 31);
                    else if (!DateTime.TryParse(tempDt.Rows[i]["exhausted_date"].ToString(), out exhausted_date))
                        exhausted_date = new DateTime(2999, 12, 31);

                    tempDt.Rows[i]["e_date"] = exhausted_date;

                    DateTime contract_date;
                    if (tempDt.Rows[i]["contract_date"].ToString() == "1년이상 판매가능")
                        contract_date = new DateTime(2999, 12, 31);
                    else if (!DateTime.TryParse(tempDt.Rows[i]["contract_date"].ToString(), out contract_date))
                        contract_date = new DateTime(2999, 12, 31);

                    tempDt.Rows[i]["c_date"] = contract_date;

                    double sizes3;
                    if (!double.TryParse(tempDt.Rows[i]["sizes3"].ToString(), out sizes3))
                        sizes3 = 0;

                    tempDt.Rows[i]["sizes3_double"] = sizes3;
                }
                tempDt.AcceptChanges();

                DataView dv = new DataView(tempDt);

                string sortTxt;
                switch (cbSortType.Text)
                {
                    case "대분류+품명+원산지+규격":
                        sortTxt = "category, product, origin, sizes3_double, sizes2, sizes";
                        break;
                    case "품명+원산지+규격":
                        sortTxt = "product, origin, sizes3_double, sizes2, sizes";
                        break;
                    case "원산지+품명+규격":
                        sortTxt = "origin, product, sizes3_double, sizes2, sizes";
                        break;
                    case "쇼트일자+품명+원산지+규격":
                        sortTxt = "e_date, product, origin, sizes3_double, sizes2, sizes";
                        break;
                    case "계약일자+품명+원산지+규격":
                        sortTxt = "c_date, product, origin, sizes3_double, sizes2, sizes";
                        break;
                    case "판매가능일+품명+원산지+규격":
                        sortTxt = "enable_sales_days DESC, product, origin, sizes3_double, sizes2, sizes";
                        break;
                    default:
                        sortTxt = "category, product, origin, sizes3_double, sizes2, sizes";
                        break;

                }
                dv.Sort = sortTxt;
                newDt = dv.ToTable();
                newDt.AcceptChanges();
                dgvProduct.DataSource = null;
                dgvProduct.DataSource = newDt;

                //서브품목 숨기기
                HideSubProduct();

                
                SetHeaderStyle();
                HeaderTextChange();
                CalculateCostPriceMarginAmount();
                //dgvProduct.DataSource = newDt;
            }
        }
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        private void btnExcel_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "업체별시세관리", "is_excel"))
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
                List<string> col_nme = new List<string>();
                for (int i = 0; i < dgvProduct.Columns.Count; i++)
                {
                    if (dgvProduct.Columns[i].Visible)
                        col_nme.Add(dgvProduct.Columns[i].Name);
                }
                GetExeclColumn(col_nme);
            }
        }
        private void btnBookmark_Click(object sender, EventArgs e)
        {
            if (isBookmarkMode)
            {
                txtGroupName.Text = String.Empty;
                lbGroupId.Text = String.Empty;
                txtGroupName.Enabled = false;
                isBookmarkMode = false;
                newDt.Rows.Clear();
                btnBookmark.Text = "즐겨찾기 선택(F8)";
                btnBookmark.ForeColor = Color.Black;

                if (dgvProduct.ColumnCount > 0)
                {
                    dgvProduct.Columns["enable_sales_days2"].Visible = false;
                    dgvProduct.Columns["month_around2"].Visible = false;
                    dgvProduct.Columns["createdatetime"].Visible = false;
                }
            }
            else
            {
                txtCategory.Text = "";
                txtProduct.Text = "";
                txtOrigin.Text = "";
                txtSizes.Text = "";
                txtUnit.Text = "";
                txtManager1.Text = "";
                txtManager2.Text = "";
                txtTradeManager.Text = "";

                BookmarkManager bm = new BookmarkManager(um, this);
                bm.StartPosition = FormStartPosition.CenterParent;
                bm.ShowDialog();

                if (dgvProduct.ColumnCount > 0)
                {
                    dgvProduct.Columns["enable_sales_days2"].Visible = true;
                    dgvProduct.Columns["month_around2"].Visible = true;
                    dgvProduct.Columns["createdatetime"].Visible = true;
                }
            }

        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                GetData();
            }
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
        private void btnAdvancedSearch_Click(object sender, EventArgs e)
        {
            /*pnAdvancedSearch.Visible = true;
            pnSetting.Height = 148;
            this.Update();*/
            try
            {
                ConferenceController cc = new ConferenceController(um, this);
                cc.Owner = this;
                cc.Show();
            }
            catch
            { }
        }

        private void btnAdvancedSearchExit_Click(object sender, EventArgs e)
        {
            pnAdvancedSearch.Visible = false;
            pnSetting.Height = 85;
        }
        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetData();
        }
        private void btnHandlingItem_Click(object sender, EventArgs e)
        {
            ToHandlingForm();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnHide_Click(object sender, EventArgs e)
        {
            BatchHide();
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            bgwProcedure.RunWorkerAsync();
            //CallProcedure();
        }
        #endregion

        #region Datagridview Event
        private void dgvProduct_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            CalculateCostPriceMarginAmount();
        }
        private void rbSalesPriceMargin_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSalesPriceMargin.Checked || rbNormalPriceMargin.Checked || rbPurchasePrice.Checked)
            {
                this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                foreach (DataGridViewRow row in dgvProduct.Rows)
                    CalculateAverageSalesCostPriceMargin(row, false);
                CalculateCostPriceMarginAmount();
                this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            }
        }
        private void dgvProduct_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "product")
                {
                    DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                    int main_id;
                    if (row.Cells["main_id"].Value == null || !int.TryParse(row.Cells["main_id"].Value.ToString(), out main_id))
                        main_id = -1;
                    int sub_id;
                    if (row.Cells["sub_id"].Value == null || !int.TryParse(row.Cells["sub_id"].Value.ToString(), out sub_id))
                        sub_id = -1;

                    if (main_id > 0 && sub_id == 9999)
                    {
                        row.HeaderCell.Value = "+";
                        row.HeaderCell.Style.Font = new Font("나눔고딕", 10, FontStyle.Bold);
                        row.HeaderCell.Style.ForeColor = Color.Yellow;
                        row.Cells["product"].Style.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                        row.Cells["product"].Style.BackColor = Color.DarkOrange;
                    }
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "month_around_in_shipment")
                {
                    if (dgvProduct.Rows[e.RowIndex].Cells["month_around"].Value.ToString() != dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                        dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
                    else
                        dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "sales_cost_price")
                {
                    if (dgvProduct.Rows[e.RowIndex].Cells["sales_cost_price_tooltip"].Value != null && dgvProduct.Rows[e.RowIndex].Cells["sales_cost_price_tooltip"].Value.ToString().Equals("확인필요"))
                        dgvProduct.Rows[e.RowIndex].Cells["sales_cost_price"].Style.ForeColor = Color.Red;
                }
            }
        }
        private void dgvProduct_SelectionChanged(object sender, EventArgs e)
        {
            dgvProduct.EndEdit();
            if (dgvProduct.SelectedCells.Count > 0)
            {
                int selectRowIndex = 0;
                DataGridViewCell selectCell = dgvProduct.SelectedCells[0];
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];
                    if (row.Visible)
                    {
                        selectRowIndex++;
                        if (row.Cells[selectCell.ColumnIndex] == selectCell)
                        {
                            txtCurrentIndex.Text = selectRowIndex.ToString("#,##0");
                            break;
                        }
                    }
                }
            }


            double total_sum = 0, total_avg = 0, real_cnt = 0;
            if (dgvProduct.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                {
                    if (cell.Value != null && double.TryParse(cell.Value.ToString(), out double val))
                    {
                        total_sum += val;
                        real_cnt++;
                    }
                }
                total_avg = total_sum / real_cnt;
            }
            lbSelectionSummary.Text = $"평균 : {total_avg.ToString("#,##0.00")}   개수 : {real_cnt.ToString("#,##0")}   합계 : {total_sum.ToString("#,##0.00")}";
        }

        private void dgvProduct_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvProduct.Rows[e.RowIndex].Selected = true;
            }
        }
        private void dgvProduct_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "offer_price"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "offer_company"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "offer_updatetime")
                {

                    DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                    if (copDt.Rows.Count > 0)
                    {
                        string whr;
                        if (row.Cells["price_unit"].Value.ToString().Contains("팩"))
                        {
                            whr = "product = '" + row.Cells["product"].Value.ToString() + "'"
                                    + " AND origin = '" + row.Cells["origin"].Value.ToString() + "'"
                                    + " AND sizes = '" + row.Cells["sizes"].Value.ToString() + "'"
                                    + " AND (unit2 = '" + row.Cells["unit"].Value.ToString() + "'"
                                    + " OR unit2 = '" + row.Cells["seaover_unit"].Value.ToString() + "')";
                        }
                        else
                        {
                            whr = "product = '" + row.Cells["product"].Value.ToString() + "'"
                                    + " AND origin = '" + row.Cells["origin"].Value.ToString() + "'"
                                    + " AND sizes = '" + row.Cells["sizes"].Value.ToString() + "'"
                                    + " AND (unit = '" + row.Cells["unit"].Value.ToString() + "'"
                                    + " OR unit2 = '" + row.Cells["seaover_unit"].Value.ToString() + "')";
                        }
                        DataRow[] copRow = copDt.Select(whr);
                        if (copRow.Length > 0)
                        {
                            //단가리스트 생성
                            OfferPriceList ofl = new OfferPriceList();
                            Point p = dgvProduct.PointToScreen(dgvProduct.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Location);
                            p = new Point(p.X - ofl.Size.Width - dgvProduct.ColumnHeadersHeight, p.Y - ofl.Size.Height + dgvProduct.Rows[e.RowIndex].Height);

                            ofl.StartPosition = FormStartPosition.Manual;
                            string[] price = ofl.Manager(copRow, p);

                            if (price != null)
                            {
                                dgvProduct.Rows[e.RowIndex].Cells["offer_updatetime"].Value = price[0].ToString();
                                dgvProduct.Rows[e.RowIndex].Cells["offer_price"].Value = Convert.ToDouble(price[1]).ToString("#,##0.00");
                                dgvProduct.Rows[e.RowIndex].Cells["offer_company"].Value = price[2];
                                dgvProduct.Rows[e.RowIndex].Cells["fixed_tariff"].Value = price[3];
                            }
                        }
                    }
                }
            }
        }
        private void dgvProduct_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
        private void dgvProduct_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dgvProduct.Rows[e.RowIndex].HeaderCell.Value != null && !string.IsNullOrEmpty(dgvProduct.Rows[e.RowIndex].HeaderCell.Value.ToString()))
                {
                    if (dgvProduct.Rows[e.RowIndex].HeaderCell.Value.ToString() == "+")
                    {
                        //서브품목
                        List<DataRow> subRowList = new List<DataRow>();
                        string main_id = dgvProduct.Rows[e.RowIndex].Cells["main_id"].Value.ToString();
                        for (int i = newDt.Rows.Count - 1; i >= 0; i--)
                        {
                            if (newDt.Rows[i]["main_id"].ToString() == main_id
                                && newDt.Rows[i]["sub_id"].ToString() != "9999")
                            {
                                DataRow subRow = newDt.NewRow();
                                subRow.ItemArray = newDt.Rows[i].ItemArray;
                                subRowList.Add(subRow);
                            }
                        }

                        MergeProduct mpl = new MergeProduct(um, this, uppDt, dgvProduct, subRowList, exchange_rate);

                        Point p = dgvProduct.PointToScreen(dgvProduct.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Location);
                        p = new Point(p.X + dgvProduct.RowHeadersWidth, p.Y + dgvProduct.Rows[e.RowIndex].Height);


                        mpl.SetLocation(p);
                        mpl.Show();


                        /*List<DataRow> subRowList = new List<DataRow>();

                        dgvProduct.Rows[e.RowIndex].HeaderCell.Value = "-";
                        string main_id = dgvProduct.Rows[e.RowIndex].Cells["main_id"].Value.ToString();
                        for (int i = newDt.Rows.Count - 1; i >= 0; i--)
                        {
                            if (newDt.Rows[i]["main_id"].ToString() == main_id
                                && newDt.Rows[i]["sub_id"].ToString() != "9999")
                            {

                                DataRow subRow = newDt.NewRow();
                                subRow.ItemArray = newDt.Rows[i].ItemArray;
                                subRowList.Add(subRow);
                                newDt.Rows.RemoveAt(i);
                            }
                        }

                        //메인품목 밑으로 출력
                        if (subRowList.Count > 0)
                        {
                            int rowindex = 0;
                            for (int i = newDt.Rows.Count - 1; i >= 0; i--)
                            {
                                if (newDt.Rows[i]["main_id"].ToString() == main_id && newDt.Rows[i]["sub_id"].ToString() == "9999")
                                {
                                    for (int j = 0; j < subRowList.Count; j++)
                                    {
                                        newDt.Rows.InsertAt(subRowList[j], i + 1);
                                    }
                                    break;
                                }

                            }
                            
                        }
                        SetMainSubProduct();*/
                    }
                    else
                    {
                        dgvProduct.Rows[e.RowIndex].HeaderCell.Value = "+";
                        for (int i = e.RowIndex + 1; i < dgvProduct.Rows.Count; i++)
                        {
                            if (dgvProduct.Rows[i].Cells["main_id"].Value.ToString() == dgvProduct.Rows[e.RowIndex].Cells["main_id"].Value.ToString())
                            {
                                try
                                {
                                    dgvProduct.Rows[i].Visible = false;
                                }
                                catch
                                { }
                            }
                        }
                    }
                }
            }
        }
        private void dgvProduct_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                //일반시세
                if (dgvProduct.Columns[e.ColumnIndex].Name == "average_purchase_price"
                    && Convert.ToDouble(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) > 0)
                {
                    DateTime sttDate = Convert.ToDateTime(txtSttdate.Text);
                    DateTime endDate = Convert.ToDateTime(txtEnddate.Text);

                    string detail = priceComparisonRepository.GetAveragePurchasePriceDetail(dgvProduct.Rows[e.RowIndex].Cells["product"].Value.ToString()
                                                                                            , dgvProduct.Rows[e.RowIndex].Cells["origin"].Value.ToString()
                                                                                            , dgvProduct.Rows[e.RowIndex].Cells["sizes"].Value.ToString()
                                                                                            , dgvProduct.Rows[e.RowIndex].Cells["unit"].Value.ToString()
                                                                                            , dgvProduct.Rows[e.RowIndex].Cells["price_unit"].Value.ToString()
                                                                                            , dgvProduct.Rows[e.RowIndex].Cells["unit_count"].Value.ToString()
                                                                                            , dgvProduct.Rows[e.RowIndex].Cells["seaover_unit"].Value.ToString()
                                                                                            , sttDate
                                                                                            , endDate
                                                                                            , Convert.ToInt16(cbCp.Text));
                    if (!string.IsNullOrEmpty(detail))
                    {
                        detail = detail.Replace("|", "\n");
                        dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = detail;
                    }
                }
                //예약상세
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "reserved_stock"
                        && Convert.ToDouble(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) > 0)
                {
                    DateTime sttDate = Convert.ToDateTime(txtSttdate.Text);
                    DateTime endDate = Convert.ToDateTime(txtEnddate.Text);

                    if (dgvProduct.Rows[e.RowIndex].Cells["sub_id"].Value.ToString() != "9999")
                    {
                        DataTable detailDt = priceComparisonRepository.GetReservedDetail(dgvProduct.Rows[e.RowIndex].Cells["product"].Value.ToString()
                                                                                    , dgvProduct.Rows[e.RowIndex].Cells["origin"].Value.ToString()
                                                                                    , dgvProduct.Rows[e.RowIndex].Cells["sizes"].Value.ToString()
                                                                                    , dgvProduct.Rows[e.RowIndex].Cells["seaover_unit"].Value.ToString());
                        if (detailDt.Rows.Count > 0)
                        {
                            string detail = detailDt.Rows[0]["예약상세"].ToString();
                            dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = detail;
                        }
                    }
                    else
                    {
                        string details = "";
                        double seaover_unit = Convert.ToDouble(dgvProduct.Rows[e.RowIndex].Cells["seaover_unit"].Value.ToString());
                        DataTable detailDt = priceComparisonRepository.GetReservedDetail(dgvProduct.Rows[e.RowIndex].Cells["product"].Value.ToString()
                                                                                    , dgvProduct.Rows[e.RowIndex].Cells["origin"].Value.ToString()
                                                                                    , dgvProduct.Rows[e.RowIndex].Cells["sizes"].Value.ToString()
                                                                                    , "");
                        if (detailDt.Rows.Count > 0)
                        {
                            for (int i = 0; i < detailDt.Rows.Count; i++)
                            {
                                double unit = Convert.ToDouble(detailDt.Rows[i]["단위"].ToString());
                                string detail = detailDt.Rows[i]["예약상세"].ToString();
                                string[] stock = detail.Split(' ');

                                for (int j = 0; j < stock.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(stock[j].Trim()))
                                    {
                                        double qty = 0;
                                        string[] txtQty = stock[j].Split('/');
                                        if (txtQty.Length > 0)
                                            qty = Convert.ToDouble(txtQty[0]);

                                        qty = qty * unit / seaover_unit;


                                        details += qty.ToString() + "/" + txtQty[1] + " ";
                                    }
                                }
                            }
                            dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = details;
                        }
                    }
                }
                //오퍼원가
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "offer_cost_price"
                        && Convert.ToDouble(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) > 0)
                {
                    DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
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
                    if (row.Cells["seaover_unit"].Value == null || !double.TryParse(row.Cells["seaover_unit"].Value.ToString(), out unit))
                        unit = 0;
                    double unit_count;
                    if (row.Cells["unit_count"].Value == null || !double.TryParse(row.Cells["unit_count"].Value.ToString(), out unit_count))
                        unit_count = 0;
                    double purchase_margin;
                    if (row.Cells["purchase_margin"].Value == null || !double.TryParse(row.Cells["purchase_margin"].Value.ToString(), out purchase_margin))
                        purchase_margin = 0;
                    purchase_margin = purchase_margin / 100;
                    bool weight_calculate = Convert.ToBoolean(row.Cells["weight_calculate"].Value);
                    //트레이 단가
                    if ((!weight_calculate && cost_unit > 0))
                        unit = cost_unit;

                    //선택 오퍼가 순위
                    double now_exchange_rate = common.GetExchangeRateKEBBank("USD");
                    double price = Convert.ToDouble(row.Cells["offer_cost_price"].Value);
                    DateTime enddate = DateTime.Now;
                    DateTime sttdate = enddate.AddYears(-2);

                    DataTable rankDt = purchasePriceRepository.GetRankingPurchasePriceASOne(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                        , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["seaover_unit"].Value.ToString()
                        , "", price);

                    double exchange_rate = now_exchange_rate;
                    if (rankDt.Rows.Count > 0)
                    {
                        for (int i = 0; i < rankDt.Rows.Count; i++)
                        {
                            double purchase_price_krw;
                            if (rankDt.Rows[i]["division"].ToString() == "1")
                                purchase_price_krw = Convert.ToDouble(rankDt.Rows[i]["purchase_price"].ToString());
                            else
                            {
                                purchase_price_krw = Convert.ToDouble(rankDt.Rows[i]["purchase_price"].ToString());
                                purchase_price_krw = purchase_price_krw * unit * ((1 + custom + tax + incidental_expense));
                                purchase_price_krw = purchase_price_krw * exchange_rate / unit_count;
                            }
                            rankDt.Rows[i]["purchase_price"] = purchase_price_krw;
                        }
                        rankDt.AcceptChanges();
                    }

                    //매입내역
                    DataTable purchaseDt = purchaseRepository.GetPurchaseProduct(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                    , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString(), "");
                    if (purchaseDt.Rows.Count > 0)
                    {
                        for (int i = 0; i < purchaseDt.Rows.Count; i++)
                        {
                            DataRow rankRow = rankDt.NewRow();
                            rankRow["updatetime"] = purchaseDt.Rows[i]["매입일자"].ToString() + "01";

                            if (!weight_calculate && cost_unit > 0)
                                unit = Convert.ToDouble(row.Cells["unit"].Value.ToString());
                            //KRW 국내매입
                            double in_purcahse = Convert.ToDouble(purchaseDt.Rows[i]["단가2"].ToString()) * (1 - purchase_margin);
                            //KRW 수입매입
                            double out_purcahse = Convert.ToDouble(purchaseDt.Rows[i]["단가3"].ToString());
                            //매입단가 평균
                            double price_krw;
                            if (in_purcahse > 0 && out_purcahse > 0)
                                price_krw = (in_purcahse + out_purcahse) / 2;
                            else if (in_purcahse > 0 && out_purcahse == 0)
                                price_krw = in_purcahse;
                            else
                                price_krw = out_purcahse;
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
                //최근 오퍼내역상세(USD)
                else if ((dgvProduct.Columns[e.ColumnIndex].Name == "offer_price"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "offer_updatetime"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "offer_company")
                    && dgvProduct.Rows[e.RowIndex].Cells["offer_price"].Value != null
                    && Convert.ToDouble(dgvProduct.Rows[e.RowIndex].Cells["offer_price"].Value) > 0)
                {

                    DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                    //선택 오퍼가 순위
                    double price = Convert.ToDouble(dgvProduct.Rows[e.RowIndex].Cells["offer_price"].Value);
                    DateTime enddate = DateTime.Now;
                    DateTime sttdate = enddate.AddYears(-2);

                    DataTable rankDt = purchasePriceRepository.GetRankingPurchasePriceASOne(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                        , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString()
                        , "", price);
                    //매입내역
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
                    if (row.Cells["seaover_unit"].Value == null || !double.TryParse(row.Cells["seaover_unit"].Value.ToString(), out unit))
                        unit = 0;
                    double unit_count;
                    if (row.Cells["unit_count"].Value == null || !double.TryParse(row.Cells["unit_count"].Value.ToString(), out unit_count))
                        unit_count = 0;
                    double purchase_margin;
                    if (row.Cells["purchase_margin"].Value == null || !double.TryParse(row.Cells["purchase_margin"].Value.ToString(), out purchase_margin))
                        purchase_margin = 0;
                    purchase_margin = purchase_margin / 100;
                    bool weight_calculate = Convert.ToBoolean(row.Cells["weight_calculate"].Value);
                    //트레이 단가
                    if ((!weight_calculate && cost_unit > 0))
                        unit = cost_unit;

                    DataTable purchaseDt = purchaseRepository.GetPurchaseProduct(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                        , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["seaover_unit"].Value.ToString(), "");
                    if (purchaseDt.Rows.Count > 0)
                    {
                        double now_exchange_rate = common.GetExchangeRateKEBBank("USD");
                        //환율내역
                        DataTable exchangeRateDt = purchaseRepository.GetExchangeRate(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));
                        for (int i = 0; i < purchaseDt.Rows.Count; i++)
                        {
                            DataRow rankRow = rankDt.NewRow();
                            rankRow["updatetime"] = purchaseDt.Rows[i]["매입일자"].ToString() + "01";
                            //KRW 국내매입
                            double in_purcahse = Convert.ToDouble(purchaseDt.Rows[i]["단가2"].ToString()) * (1 - purchase_margin);
                            //KRW 수입매입
                            double out_purcahse = Convert.ToDouble(purchaseDt.Rows[i]["단가3"].ToString());
                            //매입단가 평균
                            double price_krw;
                            if (in_purcahse > 0 && out_purcahse > 0)
                                price_krw = (in_purcahse + out_purcahse) / 2;
                            else if (in_purcahse > 0 && out_purcahse == 0)
                                price_krw = in_purcahse;
                            else
                                price_krw = out_purcahse;
                            //환율
                            DataRow[] dr = null;

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
                            double offer_price = price_krw * unit_count / exchange_rate / (1 + custom + tax + incidental_expense) / unit;
                            if (row.Cells["price_unit"].Value.ToString() == "팩")
                                offer_price = offer_price / unit_count;
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
                //선적원가
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "pending_cost_price")
                    {
                        DataGridViewRow row = dgvProduct.Rows[e.RowIndex];

                        double custom;
                        if (row.Cells["custom"].Value == null || !double.TryParse(row.Cells["custom"].Value.ToString(), out custom))
                            custom = 0;
                        else
                            custom = custom / 100;
                        double tax;
                        if (row.Cells["tax"].Value == null || !double.TryParse(row.Cells["tax"].Value.ToString(), out tax))
                            tax = 0;
                        else
                            tax = tax / 100;
                        double incidental_expense;
                        if (row.Cells["incidental_expense"].Value == null || !double.TryParse(row.Cells["incidental_expense"].Value.ToString(), out incidental_expense))
                            incidental_expense = 0;
                        else
                            incidental_expense = incidental_expense / 100;

                        string tooltipTxt = "";

                        double shipment_qty = 0;
                        double unpending_qty_before = 0;
                        double unpending_qty_after = 0;
                        double shipment_cost_price = 0;
                        DataRow[] uppRow = null;
                        if (uppDt.Rows.Count > 0)
                        {
                            string whr = "product = '" + row.Cells["product"].Value.ToString() + "'"
                                + " AND origin = '" + row.Cells["origin"].Value.ToString() + "'"
                                + " AND sizes = '" + row.Cells["sizes"].Value.ToString() + "'"
                                + " AND box_weight = '" + row.Cells["seaover_unit"].Value.ToString() + "'";
                            uppRow = uppDt.Select(whr);
                            if (uppRow.Length > 0)
                            {
                                double total_cost_price = 0;
                                double total_qty = 0;
                                for (int j = 0; j < uppRow.Length; j++)
                                {
                                    shipment_qty = 0;
                                    unpending_qty_before = 0;
                                    unpending_qty_after = 0;


                                    string bl_no = uppRow[j]["bl_no"].ToString();
                                    string warehousing_date = uppRow[j]["warehousing_date"].ToString();
                                    //계약만 한 상태
                                    if (string.IsNullOrEmpty(bl_no))
                                        shipment_qty = Convert.ToDouble(uppRow[j]["quantity_on_paper"]);
                                    //배송중인 상태
                                    else if (!string.IsNullOrEmpty(bl_no) && string.IsNullOrEmpty(warehousing_date))
                                        unpending_qty_before = Convert.ToDouble(uppRow[j]["quantity_on_paper"]);
                                    //입항은 했지만 미통관인 상태
                                    else if (!string.IsNullOrEmpty(bl_no) && !string.IsNullOrEmpty(warehousing_date))
                                        unpending_qty_after = Convert.ToDouble(uppRow[j]["quantity_on_paper"]);
                                    //선적원가
                                    if (string.IsNullOrEmpty(warehousing_date))
                                    {
                                        double box_weight;
                                        if (uppRow[j]["box_weight"] == null || !double.TryParse(uppRow[j]["box_weight"].ToString(), out box_weight))
                                            box_weight = 1;
                                        double tray;
                                        if (uppRow[j]["cost_unit"] == null || !double.TryParse(uppRow[j]["cost_unit"].ToString(), out tray))
                                            tray = 1;
                                        //단위 <-> 트레이
                                        bool isWeight = Convert.ToBoolean(uppRow[j]["weight_calculate"].ToString());
                                        if (!isWeight)
                                            box_weight = tray;
                                        double unit_count;
                                        if (row.Cells["unit_count"].Value == null || !double.TryParse(row.Cells["unit_count"].Value.ToString(), out unit_count))
                                            unit_count = 1;
                                        //원가계산
                                        shipment_cost_price = (Convert.ToDouble(uppRow[j]["unit_price"].ToString()) * exchange_rate) * (1 + custom + tax + incidental_expense) * (box_weight / unit_count);
                                        //동원 + 2.5% Or 2%
                                        if (uppRow[j]["ato_no"].ToString().Contains("dw") || uppRow[j]["ato_no"].ToString().Contains("DW")
                                            || uppRow[j]["ato_no"].ToString().Contains("hs") || uppRow[j]["ato_no"].ToString().Contains("HS")
                                            || uppRow[j]["ato_no"].ToString().Contains("od") || uppRow[j]["ato_no"].ToString().Contains("OD")
                                            || uppRow[j]["ato_no"].ToString().Contains("ad") || uppRow[j]["ato_no"].ToString().Contains("AD"))
                                            shipment_cost_price = shipment_cost_price * 1.025;
                                        else if (uppRow[j]["ato_no"].ToString().Contains("jd") || uppRow[j]["ato_no"].ToString().Contains("JD"))
                                            shipment_cost_price = shipment_cost_price * 1.02;

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

                                        //ToolTip
                                        tooltipTxt += "\n" + uppRow[j]["ato_no"].ToString() + "  " + shipment_cost_price.ToString("#,##0") + "(" + exchange_rate.ToString("#,##0.00") + ")  + 이자(" + interest.ToString("#,##0") + ")     수량 : " + (shipment_qty + unpending_qty_before).ToString("#,##0");
                                    }
                                }
                                row.Cells["pending_cost_price"].ToolTipText = tooltipTxt.Trim();
                            }
                        }
                    }
            }
        }

        private void GetAllQtySumaryTerm()
        {
            if (!cbAllTerm.Checked || dgvProduct.Columns.Count == 0)
                return;

            dgvProduct.EndEdit();
              
            dgvProduct.Columns["month_around_1"].Visible = true;
            dgvProduct.Columns["month_around_45"].Visible = true;
            dgvProduct.Columns["month_around_2"].Visible = true;
            dgvProduct.Columns["month_around_3"].Visible = true;
            dgvProduct.Columns["month_around_6"].Visible = true;
            dgvProduct.Columns["month_around_12"].Visible = true;
            dgvProduct.Columns["month_around_18"].Visible = true;

            DateTime eDate;
            if (!DateTime.TryParse(txtStockDate.Text, out eDate))
                eDate = DateTime.Now;
            //매출기간
            int saleTerm;
            switch (cbSaleTerm.Text)
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
            DateTime sDate = eDate.AddMonths(-saleTerm);
            if (saleTerm == 45)
                sDate = eDate.AddDays(-saleTerm);

            //매출내역
            DataTable qtyDt = priceComparisonRepository.GetSumaryProductAllTerms(eDate, txtProduct.Text, txtOrigin.Text, txtSizes.Text);
            if (qtyDt.Rows.Count > 0)
            {

                //제외매출
                DataTable eDt = productExcludedSalesRepository.GetExcludedSales(eDate.AddMonths(-18).ToString("yyyy-MM-dd"), eDate.ToString("yyyy-MM-dd")
                                                                            , txtProduct.Text, txtOrigin.Text, txtSizes.Text, "", "", "", "");
                if (eDt.Rows.Count > 0)
                {
                    eDt.Columns.Add("sale_date_dt", typeof(DateTime));
                    for (int i = 0; i < eDt.Rows.Count; i++)
                    {
                        DateTime sale_date;
                        if (DateTime.TryParse(eDt.Rows[i]["sale_date"].ToString(), out sale_date))
                            eDt.Rows[i]["sale_date_dt"] = sale_date;
                    }
                    eDt.AcceptChanges();
                }
                //영업일
                int work_days_1 = 0;
                common.GetWorkDay(eDate.AddMonths(-1), eDate, out work_days_1);
                int work_days_45 = 0;
                common.GetWorkDay(eDate.AddDays(-45), eDate, out work_days_45);
                int work_days_2 = 0;
                common.GetWorkDay(eDate.AddMonths(-2), eDate, out work_days_2);
                int work_days_3 = 0;
                common.GetWorkDay(eDate.AddMonths(-3), eDate, out work_days_3);
                int work_days_6 = 0;
                common.GetWorkDay(eDate.AddMonths(-6), eDate, out work_days_6);
                int work_days_12 = 0;
                common.GetWorkDay(eDate.AddMonths(-12), eDate, out work_days_12);
                int work_days_18 = 0;
                common.GetWorkDay(eDate.AddMonths(-18), eDate, out work_days_18);
                //오늘하루 제외
                if (eDate.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    work_days_1--;
                    work_days_45--;
                    work_days_2--;
                    work_days_3--;
                    work_days_6--;
                    work_days_12--;
                    work_days_18--;
                }
                int tmep;
                try
                {
                    //this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                    for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    {
                        tmep = i;
                        /*if(tmep == 33)
                            tmep = i;*/
                        DataGridViewRow row = dgvProduct.Rows[i];

                        if (row.Visible)
                        {
                            //병합여부
                            string[] sub_product;
                            if (row.Cells["merge_product"].Value == null || string.IsNullOrEmpty(row.Cells["merge_product"].Value.ToString()))
                            {
                                sub_product = new string[1];
                                sub_product[0] = row.Cells["product"].Value.ToString()
                                            + "^" + row.Cells["origin"].Value.ToString()
                                            + "^" + row.Cells["sizes"].Value.ToString()
                                            + "^" + row.Cells["unit"].Value.ToString()
                                            + "^" + row.Cells["price_unit"].Value.ToString()
                                            + "^" + row.Cells["unit_count"].Value.ToString()
                                            + "^" + row.Cells["seaover_unit"].Value.ToString();
                            }
                            else
                                sub_product = row.Cells["merge_product"].Value.ToString().Trim().Split('\n');

                            //대표품목
                            double main_unit;
                            if (row.Cells["seaover_unit"].Value == null || !double.TryParse(row.Cells["seaover_unit"].Value.ToString(), out main_unit))
                                main_unit = 0;
                            //실재고
                            double stock;
                            if (row.Cells["real_stock"].Value == null | !double.TryParse(row.Cells["real_stock"].Value.ToString(), out stock))
                                stock = 0;
                            //선적재고
                            double shipment_stock;
                            if (row.Cells["shipment_stock"].Value == null | !double.TryParse(row.Cells["shipment_stock"].Value.ToString(), out shipment_stock))
                                shipment_stock = 0;
                            //오더수량
                            double order_qty;
                            if (row.Cells["order_qty"].Value == null | !double.TryParse(row.Cells["order_qty"].Value.ToString(), out order_qty))
                                order_qty = 0;
                            //stock += order_qty;

                            string whr;
                            //병함품목 순회
                            double sale_qty1 = 0;
                            double sale_qty45 = 0;
                            double sale_qty2 = 0;
                            double sale_qty3 = 0;
                            double sale_qty6 = 0;
                            double sale_qty12 = 0;
                            double sale_qty18 = 0;
                            for (int j = 0; j < sub_product.Length; j++)
                            {
                                string[] products = sub_product[j].Trim().Split('^');
                                if (products.Length > 5)
                                {
                                    whr = $"품명 = '{products[0]}'"
                                            + $" AND 원산지 = '{products[1]}'"
                                            + $" AND 규격 = '{products[2]}'"
                                            + $" AND 단위 = '{products[6]}'";

                                    DataRow[] dr = qtyDt.Select(whr);
                                    if (dr.Length > 0)
                                    {
                                        double unit;
                                        if (!double.TryParse(dr[0]["단위"].ToString(), out unit))
                                            unit = 0;

                                        double sale_qty;
                                        //1개월
                                        if (!double.TryParse(dr[0]["매출수1"].ToString(), out sale_qty))
                                            sale_qty = 0;
                                        sale_qty *= unit;
                                        sale_qty1 += sale_qty;

                                        //제외매출
                                        if (eDt.Rows.Count > 0)
                                        {
                                            whr = $"product = '{products[0]}'"
                                            + $" AND origin = '{products[1]}'"
                                            + $" AND sizes = '{products[2]}'"
                                            + $" AND unit = '{products[3]}'"
                                            + $" AND price_unit = '{products[4]}'"
                                            + $" AND unit_count = '{products[5]}'"
                                            + $" AND seaover_unit = '{products[6]}'"
                                            + $" AND sale_date_dt >= #{eDate.AddMonths(-1).ToString("yyyy-MM-dd")}#"
                                            + $" AND sale_date_dt <= #{eDate.ToString("yyyy-MM-dd")}#";
                                            DataRow[] eDr = eDt.Select(whr);
                                            if (eDr.Length > 0)
                                            {
                                                for (int k = 0; k < eDr.Length; k++)
                                                {
                                                    double exclude_qty;
                                                    if (!double.TryParse(eDr[k]["sale_qty"].ToString(), out exclude_qty))
                                                        exclude_qty = 0;

                                                    double seaover_unit;
                                                    if (!double.TryParse(eDr[k]["seaover_unit"].ToString(), out seaover_unit))
                                                        seaover_unit = 1;

                                                    sale_qty1 -= exclude_qty * seaover_unit;
                                                }
                                            }
                                        }


                                        //45일
                                        if (!double.TryParse(dr[0]["매출수45"].ToString(), out sale_qty))
                                            sale_qty = 0;
                                        sale_qty *= unit;
                                        sale_qty45 += sale_qty;

                                        //제외매출
                                        if (eDt.Rows.Count > 0)
                                        {
                                            whr = $"product = '{products[0]}'"
                                            + $" AND origin = '{products[1]}'"
                                            + $" AND sizes = '{products[2]}'"
                                            + $" AND unit = '{products[3]}'"
                                            + $" AND price_unit = '{products[4]}'"
                                            + $" AND unit_count = '{products[5]}'"
                                            + $" AND seaover_unit = '{products[6]}'"
                                            + $" AND sale_date_dt >= #{eDate.AddDays(-45).ToString("yyyy-MM-dd")}#"
                                            + $" AND sale_date_dt <= #{eDate.ToString("yyyy-MM-dd")}#";
                                            DataRow[] eDr = eDt.Select(whr);
                                            if (eDr.Length > 0)
                                            {
                                                for (int k = 0; k < eDr.Length; k++)
                                                {
                                                    double exclude_qty;
                                                    if (!double.TryParse(eDr[k]["sale_qty"].ToString(), out exclude_qty))
                                                        exclude_qty = 0;
                                                    double seaover_unit;
                                                    if (!double.TryParse(eDr[k]["seaover_unit"].ToString(), out seaover_unit))
                                                        seaover_unit = 1;

                                                    sale_qty45 -= exclude_qty * seaover_unit;
                                                }
                                            }
                                        }


                                        //2개월
                                        if (!double.TryParse(dr[0]["매출수2"].ToString(), out sale_qty))
                                            sale_qty = 0;
                                        sale_qty *= unit;
                                        sale_qty2 += sale_qty;
                                        //제외매출
                                        if (eDt.Rows.Count > 0)
                                        {
                                            whr = $"product = '{products[0]}'"
                                            + $" AND origin = '{products[1]}'"
                                            + $" AND sizes = '{products[2]}'"
                                            + $" AND unit = '{products[3]}'"
                                            + $" AND price_unit = '{products[4]}'"
                                            + $" AND unit_count = '{products[5]}'"
                                            + $" AND seaover_unit = '{products[6]}'"
                                            + $" AND sale_date_dt >= #{eDate.AddMonths(-2).ToString("yyyy-MM-dd")}#"
                                            + $" AND sale_date_dt <= #{eDate.ToString("yyyy-MM-dd")}#";
                                            DataRow[] eDr = eDt.Select(whr);
                                            if (eDr.Length > 0)
                                            {
                                                for (int k = 0; k < eDr.Length; k++)
                                                {
                                                    double exclude_qty;
                                                    if (!double.TryParse(eDr[k]["sale_qty"].ToString(), out exclude_qty))
                                                        exclude_qty = 0;
                                                    double seaover_unit;
                                                    if (!double.TryParse(eDr[k]["seaover_unit"].ToString(), out seaover_unit))
                                                        seaover_unit = 1;

                                                    sale_qty2 -= exclude_qty * seaover_unit;
                                                }
                                            }
                                        }


                                        //3개월
                                        if (!double.TryParse(dr[0]["매출수3"].ToString(), out sale_qty))
                                            sale_qty = 0;
                                        sale_qty *= unit;
                                        sale_qty3 += sale_qty;
                                        //제외매출
                                        if (eDt.Rows.Count > 0)
                                        {
                                            whr = $"product = '{products[0]}'"
                                            + $" AND origin = '{products[1]}'"
                                            + $" AND sizes = '{products[2]}'"
                                            + $" AND unit = '{products[3]}'"
                                            + $" AND price_unit = '{products[4]}'"
                                            + $" AND unit_count = '{products[5]}'"
                                            + $" AND seaover_unit = '{products[6]}'"
                                            + $" AND sale_date_dt >= #{eDate.AddMonths(-3).ToString("yyyy-MM-dd")}#"
                                            + $" AND sale_date_dt <= #{eDate.ToString("yyyy-MM-dd")}#";
                                            DataRow[] eDr = eDt.Select(whr);
                                            if (eDr.Length > 0)
                                            {
                                                for (int k = 0; k < eDr.Length; k++)
                                                {
                                                    double exclude_qty;
                                                    if (!double.TryParse(eDr[k]["sale_qty"].ToString(), out exclude_qty))
                                                        exclude_qty = 0;
                                                    double seaover_unit;
                                                    if (!double.TryParse(eDr[k]["seaover_unit"].ToString(), out seaover_unit))
                                                        seaover_unit = 1;

                                                    sale_qty3 -= exclude_qty * seaover_unit;
                                                }
                                            }
                                        }

                                        //6개월
                                        if (!double.TryParse(dr[0]["매출수6"].ToString(), out sale_qty))
                                            sale_qty = 0;
                                        sale_qty *= unit;
                                        sale_qty6 += sale_qty;
                                        //제외매출
                                        if (eDt.Rows.Count > 0)
                                        {
                                            whr = $"product = '{products[0]}'"
                                            + $" AND origin = '{products[1]}'"
                                            + $" AND sizes = '{products[2]}'"
                                            + $" AND unit = '{products[3]}'"
                                            + $" AND price_unit = '{products[4]}'"
                                            + $" AND unit_count = '{products[5]}'"
                                            + $" AND seaover_unit = '{products[6]}'"
                                            + $" AND sale_date_dt >= #{eDate.AddMonths(-6).ToString("yyyy-MM-dd")}#"
                                            + $" AND sale_date_dt <= #{eDate.ToString("yyyy-MM-dd")}#";
                                            DataRow[] eDr = eDt.Select(whr);
                                            if (eDr.Length > 0)
                                            {
                                                for (int k = 0; k < eDr.Length; k++)
                                                {
                                                    double exclude_qty;
                                                    if (!double.TryParse(eDr[k]["sale_qty"].ToString(), out exclude_qty))
                                                        exclude_qty = 0;
                                                    double seaover_unit;
                                                    if (!double.TryParse(eDr[k]["seaover_unit"].ToString(), out seaover_unit))
                                                        seaover_unit = 1;

                                                    sale_qty6 -= exclude_qty * seaover_unit;
                                                }
                                            }
                                        }

                                        //12개월
                                        if (!double.TryParse(dr[0]["매출수12"].ToString(), out sale_qty))
                                            sale_qty = 0;
                                        sale_qty *= unit;
                                        sale_qty12 += sale_qty;
                                        //제외매출
                                        if (eDt.Rows.Count > 0)
                                        {
                                            whr = $"product = '{products[0]}'"
                                            + $" AND origin = '{products[1]}'"
                                            + $" AND sizes = '{products[2]}'"
                                            + $" AND unit = '{products[3]}'"
                                            + $" AND price_unit = '{products[4]}'"
                                            + $" AND unit_count = '{products[5]}'"
                                            + $" AND seaover_unit = '{products[6]}'"
                                            + $" AND sale_date_dt >= #{eDate.AddMonths(-12).ToString("yyyy-MM-dd")}#"
                                            + $" AND sale_date_dt <= #{eDate.ToString("yyyy-MM-dd")}#";
                                            DataRow[] eDr = eDt.Select(whr);
                                            if (eDr.Length > 0)
                                            {
                                                for (int k = 0; k < eDr.Length; k++)
                                                {
                                                    double exclude_qty;
                                                    if (!double.TryParse(eDr[k]["sale_qty"].ToString(), out exclude_qty))
                                                        exclude_qty = 0;
                                                    double seaover_unit;
                                                    if (!double.TryParse(eDr[k]["seaover_unit"].ToString(), out seaover_unit))
                                                        seaover_unit = 1;

                                                    sale_qty12 -= exclude_qty * seaover_unit;
                                                }
                                            }
                                        }
                                        //18개월
                                        if (!double.TryParse(dr[0]["매출수18"].ToString(), out sale_qty))
                                            sale_qty = 0;
                                        sale_qty *= unit;
                                        sale_qty18 += sale_qty;
                                        //제외매출
                                        if (eDt.Rows.Count > 0)
                                        {
                                            whr = $"product = '{products[0]}'"
                                                + $" AND origin = '{products[1]}'"
                                                + $" AND sizes = '{products[2]}'"
                                                + $" AND unit = '{products[3]}'"
                                                + $" AND price_unit = '{products[4]}'"
                                                + $" AND unit_count = '{products[5]}'"
                                                + $" AND seaover_unit = '{products[6]}'"
                                                + $" AND sale_date_dt >= #{eDate.AddMonths(-18).ToString("yyyy-MM-dd")}#"
                                                + $" AND sale_date_dt <= #{eDate.ToString("yyyy-MM-dd")}#";
                                            DataRow[] eDr = eDt.Select(whr);
                                            if (eDr.Length > 0)
                                            {
                                                for (int k = 0; k < eDr.Length; k++)
                                                {
                                                    double exclude_qty;
                                                    if (!double.TryParse(eDr[k]["sale_qty"].ToString(), out exclude_qty))
                                                        exclude_qty = 0;
                                                    double seaover_unit;
                                                    if (!double.TryParse(eDr[k]["seaover_unit"].ToString(), out seaover_unit))
                                                        seaover_unit = 1;

                                                    sale_qty18 -= exclude_qty * seaover_unit;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //매출량 단위변환
                            sale_qty1 /= main_unit;
                            sale_qty45 /= main_unit;
                            sale_qty2 /= main_unit;
                            sale_qty3 /= main_unit;
                            sale_qty6 /= main_unit;
                            sale_qty12 /= main_unit;
                            sale_qty18 /= main_unit;
                            //월별계산=======================================================================================
                            if (row.Cells["sub_id"].Value.ToString() != "9999")
                            {
                                whr = "product = '" + row.Cells["product"].Value.ToString() + "'"
                                    + " AND origin = '" + row.Cells["origin"].Value.ToString() + "'"
                                    + " AND sizes = '" + row.Cells["sizes"].Value.ToString() + "'"
                                    + " AND box_weight = '" + row.Cells["seaover_unit"].Value.ToString() + "'";
                            }
                            else
                            {
                                whr = "product = '" + row.Cells["product"].Value.ToString() + "'"
                                    + " AND origin = '" + row.Cells["origin"].Value.ToString() + "'"
                                    + " AND sizes = '" + row.Cells["sizes"].Value.ToString() + "'"
                                    + " AND box_weight = '" + row.Cells["unit"].Value.ToString() + "'";
                            }

                            //팬딩/오퍼 ETD반영
                            if (rbDetailReflectionShipmentQty.Checked)
                            {
                                DateTime exhausted_date;
                                double exhausted_cnt;
                                DateTime contract_date, min_etd;
                                int until_days, until_days2;
                                double month_around_in_shipment;

                                //1개월==================================================================================
                                double sale_day_qty = sale_qty1 / 21;
                                //팬딩내역
                                DataTable copyUppDt = uppDt.Copy();
                                DataRow[] uppRow = copyUppDt.Select(whr);
                                //대표품목이면서 일치 내역이 없으면 다시 seaover단위로 검색
                                if (row.Cells["sub_id"].Value.ToString() == "9999" && uppRow != null && uppRow.Length == 0)
                                {
                                    whr = "";
                                    sub_product = row.Cells["merge_product"].Value.ToString().Trim().Split('\n');
                                    if (sub_product.Length > 0)
                                    {
                                        if (sub_product.Length == 1)
                                        {
                                            string[] products = sub_product[0].Split('^');
                                            whr = "product = '" + products[0] + "'"
                                            + " AND origin = '" + products[1] + "'"
                                            + " AND sizes = '" + products[2] + "'"
                                            + " AND box_weight = '" + products[6] + "'";
                                        }
                                        else
                                        {

                                            for (int j = 0; j < sub_product.Length; j++)
                                            {
                                                if (j == 0)
                                                    whr = "(";

                                                string[] products = sub_product[j].Split('^');
                                                whr += "product = '" + products[0] + "'"
                                                + " AND origin = '" + products[1] + "'"
                                                + " AND sizes = '" + products[2] + "'"
                                                + " AND box_weight = '" + products[6] + "'";
                                                if (j < sub_product.Length - 1)
                                                    whr += ") OR (";
                                                else
                                                    whr += ") ";
                                            }
                                        }
                                        uppRow = copyUppDt.Select(whr);
                                    }
                                }
                                GetMonthRound(row, uppRow, order_qty, sale_day_qty, out exhausted_date, out exhausted_cnt, out contract_date, out until_days, out min_etd, out until_days2, out month_around_in_shipment);

                                string month_around_txt = month_around_in_shipment.ToString("#,##0.00");
                                if (month_around_txt.Length > 3 && month_around_txt.Substring(month_around_txt.Length - 3, 3) == ".00")
                                    month_around_txt = month_around_txt.Substring(0, month_around_txt.Length - 3);

                                string sale_qty_txt = ("(" + sale_qty1.ToString("#,##0") + ")").PadLeft(10, ' ');
                                row.Cells["month_around_1"].Value = month_around_txt + sale_qty_txt;
                                newDt.Rows[row.Index]["month_around_1"] = month_around_txt + sale_qty_txt;

                                //45일==================================================================================
                                sale_day_qty = sale_qty45 / work_days_45;

                                //팬딩내역
                                copyUppDt = uppDt.Copy();
                                uppRow = copyUppDt.Select(whr);
                                //대표품목이면서 일치 내역이 없으면 다시 seaover단위로 검색
                                if (row.Cells["sub_id"].Value.ToString() == "9999" && uppRow != null && uppRow.Length == 0)
                                {
                                    whr = "";
                                    sub_product = row.Cells["merge_product"].Value.ToString().Trim().Split('\n');
                                    if (sub_product.Length > 0)
                                    {
                                        if (sub_product.Length == 1)
                                        {
                                            string[] products = sub_product[0].Split('^');
                                            whr = "product = '" + products[0] + "'"
                                            + " AND origin = '" + products[1] + "'"
                                            + " AND sizes = '" + products[2] + "'"
                                            + " AND box_weight = '" + products[6] + "'";
                                        }
                                        else
                                        {

                                            for (int j = 0; j < sub_product.Length; j++)
                                            {
                                                if (j == 0)
                                                    whr = "(";

                                                string[] products = sub_product[j].Split('^');
                                                whr += "product = '" + products[0] + "'"
                                                + " AND origin = '" + products[1] + "'"
                                                + " AND sizes = '" + products[2] + "'"
                                                + " AND box_weight = '" + products[6] + "'";
                                                if (j < sub_product.Length - 1)
                                                    whr += ") OR (";
                                                else
                                                    whr += ") ";
                                            }
                                        }
                                        uppRow = copyUppDt.Select(whr);
                                    }
                                }
                                GetMonthRound(row, uppRow, order_qty, sale_day_qty, out exhausted_date, out exhausted_cnt, out contract_date, out until_days, out min_etd, out until_days2, out month_around_in_shipment);

                                month_around_txt = month_around_in_shipment.ToString("#,##0.00");
                                if (month_around_txt.Length > 3 && month_around_txt.Substring(month_around_txt.Length - 3, 3) == ".00")
                                    month_around_txt = month_around_txt.Substring(0, month_around_txt.Length - 3);
                                sale_qty_txt = ("(" + Math.Round((sale_day_qty) * 21).ToString("#,##0") + ")").PadLeft(10, ' ');
                                row.Cells["month_around_45"].Value = month_around_txt + sale_qty_txt;
                                newDt.Rows[row.Index]["month_around_45"] = month_around_txt + sale_qty_txt;
                                //2개월==================================================================================
                                sale_day_qty = sale_qty2 / work_days_2;
                                //팬딩내역
                                copyUppDt = uppDt.Copy();
                                uppRow = copyUppDt.Select(whr);
                                //대표품목이면서 일치 내역이 없으면 다시 seaover단위로 검색
                                if (row.Cells["sub_id"].Value.ToString() == "9999" && uppRow != null && uppRow.Length == 0)
                                {
                                    whr = "";
                                    sub_product = row.Cells["merge_product"].Value.ToString().Trim().Split('\n');
                                    if (sub_product.Length > 0)
                                    {
                                        if (sub_product.Length == 1)
                                        {
                                            string[] products = sub_product[0].Split('^');
                                            whr = "product = '" + products[0] + "'"
                                            + " AND origin = '" + products[1] + "'"
                                            + " AND sizes = '" + products[2] + "'"
                                            + " AND box_weight = '" + products[6] + "'";
                                        }
                                        else
                                        {

                                            for (int j = 0; j < sub_product.Length; j++)
                                            {
                                                if (j == 0)
                                                    whr = "(";

                                                string[] products = sub_product[j].Split('^');
                                                whr += "product = '" + products[0] + "'"
                                                + " AND origin = '" + products[1] + "'"
                                                + " AND sizes = '" + products[2] + "'"
                                                + " AND box_weight = '" + products[6] + "'";
                                                if (j < sub_product.Length - 1)
                                                    whr += ") OR (";
                                                else
                                                    whr += ") ";
                                            }
                                        }
                                        uppRow = copyUppDt.Select(whr);
                                    }
                                }
                                GetMonthRound(row, uppRow, order_qty, sale_day_qty, out exhausted_date, out exhausted_cnt, out contract_date, out until_days, out min_etd, out until_days2, out month_around_in_shipment);

                                month_around_txt = month_around_in_shipment.ToString("#,##0.00");
                                if (month_around_txt.Length > 3 && month_around_txt.Substring(month_around_txt.Length - 3, 3) == ".00")
                                    month_around_txt = month_around_txt.Substring(0, month_around_txt.Length - 3);
                                sale_qty_txt = ("(" + Math.Round((sale_day_qty) * 21).ToString("#,##0") + ")").PadLeft(10, ' ');
                                row.Cells["month_around_2"].Value = month_around_txt + sale_qty_txt;
                                newDt.Rows[row.Index]["month_around_2"] = month_around_txt + sale_qty_txt;
                                //3개월==================================================================================
                                sale_day_qty = sale_qty3 / work_days_3;
                                //팬딩내역
                                copyUppDt = uppDt.Copy();
                                uppRow = copyUppDt.Select(whr);
                                //대표품목이면서 일치 내역이 없으면 다시 seaover단위로 검색
                                if (row.Cells["sub_id"].Value.ToString() == "9999" && uppRow != null && uppRow.Length == 0)
                                {
                                    whr = "";
                                    sub_product = row.Cells["merge_product"].Value.ToString().Trim().Split('\n');
                                    if (sub_product.Length > 0)
                                    {
                                        if (sub_product.Length == 1)
                                        {
                                            string[] products = sub_product[0].Split('^');
                                            whr = "product = '" + products[0] + "'"
                                            + " AND origin = '" + products[1] + "'"
                                            + " AND sizes = '" + products[2] + "'"
                                            + " AND box_weight = '" + products[6] + "'";
                                        }
                                        else
                                        {

                                            for (int j = 0; j < sub_product.Length; j++)
                                            {
                                                if (j == 0)
                                                    whr = "(";

                                                string[] products = sub_product[j].Split('^');
                                                whr += "product = '" + products[0] + "'"
                                                + " AND origin = '" + products[1] + "'"
                                                + " AND sizes = '" + products[2] + "'"
                                                + " AND box_weight = '" + products[6] + "'";
                                                if (j < sub_product.Length - 1)
                                                    whr += ") OR (";
                                                else
                                                    whr += ") ";
                                            }
                                        }
                                        uppRow = copyUppDt.Select(whr);
                                    }
                                }
                                GetMonthRound(row, uppRow, order_qty, sale_day_qty, out exhausted_date, out exhausted_cnt, out contract_date, out until_days, out min_etd, out until_days2, out month_around_in_shipment);

                                month_around_txt = month_around_in_shipment.ToString("#,##0.00");
                                if (month_around_txt.Length > 3 && month_around_txt.Substring(month_around_txt.Length - 3, 3) == ".00")
                                    month_around_txt = month_around_txt.Substring(0, month_around_txt.Length - 3);
                                sale_qty_txt = ("(" + Math.Round((sale_day_qty) * 21).ToString("#,##0") + ")").PadLeft(10, ' ');
                                row.Cells["month_around_3"].Value = month_around_txt + sale_qty_txt;
                                newDt.Rows[row.Index]["month_around_3"] = month_around_txt + sale_qty_txt;
                                //6개월==================================================================================
                                sale_day_qty = sale_qty6 / work_days_6;
                                //팬딩내역
                                copyUppDt = uppDt.Copy();
                                uppRow = copyUppDt.Select(whr);
                                //대표품목이면서 일치 내역이 없으면 다시 seaover단위로 검색
                                if (row.Cells["sub_id"].Value.ToString() == "9999" && uppRow != null && uppRow.Length == 0)
                                {
                                    whr = "";
                                    sub_product = row.Cells["merge_product"].Value.ToString().Trim().Split('\n');
                                    if (sub_product.Length > 0)
                                    {
                                        if (sub_product.Length == 1)
                                        {
                                            string[] products = sub_product[0].Split('^');
                                            whr = "product = '" + products[0] + "'"
                                            + " AND origin = '" + products[1] + "'"
                                            + " AND sizes = '" + products[2] + "'"
                                            + " AND box_weight = '" + products[6] + "'";
                                        }
                                        else
                                        {

                                            for (int j = 0; j < sub_product.Length; j++)
                                            {
                                                if (j == 0)
                                                    whr = "(";

                                                string[] products = sub_product[j].Split('^');
                                                whr += "product = '" + products[0] + "'"
                                                + " AND origin = '" + products[1] + "'"
                                                + " AND sizes = '" + products[2] + "'"
                                                + " AND box_weight = '" + products[6] + "'";
                                                if (j < sub_product.Length - 1)
                                                    whr += ") OR (";
                                                else
                                                    whr += ") ";
                                            }
                                        }
                                        uppRow = copyUppDt.Select(whr);
                                    }
                                }
                                GetMonthRound(row, uppRow, order_qty, sale_day_qty, out exhausted_date, out exhausted_cnt, out contract_date, out until_days, out min_etd, out until_days2, out month_around_in_shipment);

                                month_around_txt = month_around_in_shipment.ToString("#,##0.00");
                                if (month_around_txt.Length > 3 && month_around_txt.Substring(month_around_txt.Length - 3, 3) == ".00")
                                    month_around_txt = month_around_txt.Substring(0, month_around_txt.Length - 3);
                                sale_qty_txt = ("(" + Math.Round((sale_day_qty) * 21).ToString("#,##0") + ")").PadLeft(10, ' ');
                                row.Cells["month_around_6"].Value = month_around_txt + sale_qty_txt;
                                newDt.Rows[row.Index]["month_around_6"] = month_around_txt + sale_qty_txt;
                                //12개월==================================================================================
                                sale_day_qty = sale_qty12 / work_days_12;
                                //팬딩내역
                                copyUppDt = uppDt.Copy();
                                uppRow = copyUppDt.Select(whr);
                                //대표품목이면서 일치 내역이 없으면 다시 seaover단위로 검색
                                if (row.Cells["sub_id"].Value.ToString() == "9999" && uppRow != null && uppRow.Length == 0)
                                {
                                    whr = "";
                                    sub_product = row.Cells["merge_product"].Value.ToString().Trim().Split('\n');
                                    if (sub_product.Length > 0)
                                    {
                                        if (sub_product.Length == 1)
                                        {
                                            string[] products = sub_product[0].Split('^');
                                            whr = "product = '" + products[0] + "'"
                                            + " AND origin = '" + products[1] + "'"
                                            + " AND sizes = '" + products[2] + "'"
                                            + " AND box_weight = '" + products[6] + "'";
                                        }
                                        else
                                        {

                                            for (int j = 0; j < sub_product.Length; j++)
                                            {
                                                if (j == 0)
                                                    whr = "(";

                                                string[] products = sub_product[j].Split('^');
                                                whr += "product = '" + products[0] + "'"
                                                + " AND origin = '" + products[1] + "'"
                                                + " AND sizes = '" + products[2] + "'"
                                                + " AND box_weight = '" + products[6] + "'";
                                                if (j < sub_product.Length - 1)
                                                    whr += ") OR (";
                                                else
                                                    whr += ") ";
                                            }
                                        }
                                        uppRow = copyUppDt.Select(whr);
                                    }
                                }
                                GetMonthRound(row, uppRow, order_qty, sale_day_qty, out exhausted_date, out exhausted_cnt, out contract_date, out until_days, out min_etd, out until_days2, out month_around_in_shipment);

                                month_around_txt = month_around_in_shipment.ToString("#,##0.00");
                                if (month_around_txt.Length > 3 && month_around_txt.Substring(month_around_txt.Length - 3, 3) == ".00")
                                    month_around_txt = month_around_txt.Substring(0, month_around_txt.Length - 3);
                                sale_qty_txt = ("(" + Math.Round((sale_day_qty) * 21).ToString("#,##0") + ")").PadLeft(10, ' ');
                                row.Cells["month_around_12"].Value = month_around_txt + sale_qty_txt;
                                newDt.Rows[row.Index]["month_around_12"] = month_around_txt + sale_qty_txt;
                                //18개월==================================================================================
                                sale_day_qty = sale_qty18 / work_days_18;
                                //팬딩내역
                                copyUppDt = uppDt.Copy();
                                uppRow = copyUppDt.Select(whr);
                                //대표품목이면서 일치 내역이 없으면 다시 seaover단위로 검색
                                if (row.Cells["sub_id"].Value.ToString() == "9999" && uppRow != null && uppRow.Length == 0)
                                {
                                    whr = "";
                                    sub_product = row.Cells["merge_product"].Value.ToString().Trim().Split('\n');
                                    if (sub_product.Length > 0)
                                    {
                                        if (sub_product.Length == 1)
                                        {
                                            string[] products = sub_product[0].Split('^');
                                            whr = "product = '" + products[0] + "'"
                                            + " AND origin = '" + products[1] + "'"
                                            + " AND sizes = '" + products[2] + "'"
                                            + " AND box_weight = '" + products[6] + "'";
                                        }
                                        else
                                        {

                                            for (int j = 0; j < sub_product.Length; j++)
                                            {
                                                if (j == 0)
                                                    whr = "(";

                                                string[] products = sub_product[j].Split('^');
                                                whr += "product = '" + products[0] + "'"
                                                + " AND origin = '" + products[1] + "'"
                                                + " AND sizes = '" + products[2] + "'"
                                                + " AND box_weight = '" + products[6] + "'";
                                                if (j < sub_product.Length - 1)
                                                    whr += ") OR (";
                                                else
                                                    whr += ") ";
                                            }
                                        }
                                        uppRow = copyUppDt.Select(whr);
                                    }
                                }
                                GetMonthRound(row, uppRow, order_qty, sale_day_qty, out exhausted_date, out exhausted_cnt, out contract_date, out until_days, out min_etd, out until_days2, out month_around_in_shipment);

                                month_around_txt = month_around_in_shipment.ToString("#,##0.00");
                                if (month_around_txt.Length > 3 && month_around_txt.Substring(month_around_txt.Length - 3, 3) == ".00")
                                    month_around_txt = month_around_txt.Substring(0, month_around_txt.Length - 3);
                                sale_qty_txt = ("(" + Math.Round((sale_day_qty) * 21).ToString("#,##0") + ")").PadLeft(10, ' ');
                                row.Cells["month_around_18"].Value = month_around_txt + sale_qty_txt;
                                newDt.Rows[row.Index]["month_around_18"] = month_around_txt + sale_qty_txt;
                            }
                            //전체재고로 생각하고 회전율 계산
                            else
                            {
                                //오퍼수량
                                stock += order_qty;
                                //재고에 선적재고 포함/미포함
                                if (rbDirectReflectionShipmentQty.Checked)
                                    stock += shipment_stock;

                                //1개월
                                double month_around = stock / sale_qty1;
                                if (double.IsInfinity(month_around))
                                    month_around = 9999;
                                else if (double.IsNaN(month_around))
                                    month_around = 0;

                                string month_around_txt = month_around.ToString("#,##0.00");
                                if (month_around_txt.Substring(month_around_txt.Length - 3, 3) == ".00")
                                    month_around_txt = month_around_txt.Substring(0, month_around_txt.Length - 3);
                                string sale_qty_txt = ("(" + sale_qty1.ToString("#,##0") + ")").PadLeft(10, ' ');
                                row.Cells["month_around_1"].Value = month_around_txt + sale_qty_txt;
                                newDt.Rows[row.Index]["month_around_1"] = month_around_txt + sale_qty_txt;
                                //45일
                                month_around = stock / (sale_qty45 / work_days_45) / 21;
                                if (double.IsInfinity(month_around))
                                    month_around = 9999;
                                else if (double.IsNaN(month_around))
                                    month_around = 0;

                                month_around_txt = month_around.ToString("#,##0.00");
                                if (month_around_txt.Substring(month_around_txt.Length - 3, 3) == ".00")
                                    month_around_txt = month_around_txt.Substring(0, month_around_txt.Length - 3);
                                sale_qty_txt = ("(" + Math.Round((sale_qty45 / work_days_45) * 21).ToString("#,##0") + ")").PadLeft(10, ' ');
                                row.Cells["month_around_45"].Value = month_around_txt + sale_qty_txt;
                                newDt.Rows[row.Index]["month_around_45"] = month_around_txt + sale_qty_txt;
                                //2개월
                                /*double avg_day_sales = sale_qty2 / work_days_2;
                                double enable_sales_days = stock / avg_day_sales;
                                month_around = enable_sales_days / 21;*/
                                month_around = stock / (sale_qty2 / work_days_2) / 21;
                                if (double.IsInfinity(month_around))
                                    month_around = 9999;
                                else if (double.IsNaN(month_around))
                                    month_around = 0;
                                month_around_txt = month_around.ToString("#,##0.00");
                                if (month_around_txt.Substring(month_around_txt.Length - 3, 3) == ".00")
                                    month_around_txt = month_around_txt.Substring(0, month_around_txt.Length - 3);
                                sale_qty_txt = ("(" + Math.Round((sale_qty2 / work_days_2) * 21).ToString("#,##0") + ")").PadLeft(10, ' ');
                                row.Cells["month_around_2"].Value = month_around_txt + sale_qty_txt;
                                newDt.Rows[row.Index]["month_around_2"] = month_around_txt + sale_qty_txt;
                                //3개월
                                month_around = stock / (sale_qty3 / work_days_3) / 21;
                                if (double.IsInfinity(month_around))
                                    month_around = 9999;
                                else if (double.IsNaN(month_around))
                                    month_around = 0;
                                month_around_txt = month_around.ToString("#,##0.00");
                                if (month_around_txt.Substring(month_around_txt.Length - 3, 3) == ".00")
                                    month_around_txt = month_around_txt.Substring(0, month_around_txt.Length - 3);
                                sale_qty_txt = ("(" + Math.Round((sale_qty3 / work_days_3) * 21).ToString("#,##0") + ")").PadLeft(10, ' ');
                                row.Cells["month_around_3"].Value = month_around_txt + sale_qty_txt;
                                newDt.Rows[row.Index]["month_around_3"] = month_around_txt + sale_qty_txt;
                                //6개월
                                month_around = stock / (sale_qty6 / work_days_6) / 21;
                                if (double.IsInfinity(month_around))
                                    month_around = 9999;
                                else if (double.IsNaN(month_around))
                                    month_around = 0;
                                month_around_txt = month_around.ToString("#,##0.00");
                                if (month_around_txt.Substring(month_around_txt.Length - 3, 3) == ".00")
                                    month_around_txt = month_around_txt.Substring(0, month_around_txt.Length - 3);
                                sale_qty_txt = ("(" + Math.Round((sale_qty6 / work_days_6) * 21).ToString("#,##0") + ")").PadLeft(10, ' ');
                                row.Cells["month_around_6"].Value = month_around_txt + sale_qty_txt;
                                newDt.Rows[row.Index]["month_around_6"] = month_around_txt + sale_qty_txt;
                                //12개월
                                month_around = stock / (sale_qty12 / work_days_12) / 21;
                                if (double.IsInfinity(month_around))
                                    month_around = 9999;
                                else if (double.IsNaN(month_around))
                                    month_around = 0;
                                month_around_txt = month_around.ToString("#,##0.00");
                                if (month_around_txt.Substring(month_around_txt.Length - 3, 3) == ".00")
                                    month_around_txt = month_around_txt.Substring(0, month_around_txt.Length - 3);
                                sale_qty_txt = ("(" + Math.Round((sale_qty12 / work_days_12) * 21).ToString("#,##0") + ")").PadLeft(10, ' ');
                                row.Cells["month_around_12"].Value = month_around_txt + sale_qty_txt;
                                newDt.Rows[row.Index]["month_around_12"] = month_around_txt + sale_qty_txt;
                                //18개월
                                month_around = stock / (sale_qty18 / work_days_18) / 21;
                                if (double.IsInfinity(month_around))
                                    month_around = 9999;
                                else if (double.IsNaN(month_around))
                                    month_around = 0;
                                month_around_txt = month_around.ToString("#,##0.00");
                                if (month_around_txt.Substring(month_around_txt.Length - 3, 3) == ".00")
                                    month_around_txt = month_around_txt.Substring(0, month_around_txt.Length - 3);
                                sale_qty_txt = ("(" + Math.Round((sale_qty18 / work_days_18) * 21).ToString("#,##0") + ")").PadLeft(10, ' ');
                                row.Cells["month_around_18"].Value = month_around_txt + sale_qty_txt;
                                newDt.Rows[row.Index]["month_around_18"] = month_around_txt + sale_qty_txt;
                            }
                        }
                    }
                    //서브품목 숨기기
                    HideSubProduct();
                }
                catch
                {
                    work_days_1 = work_days_1;
                }
                //this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            }
        }

        private void dgvProduct_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;

            //정렬준비
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
                dgvProduct.Rows[i].Cells["sort_hide"].Value = dgvProduct.Rows[i].Visible.ToString();
            //dgvProduct.EndEdit();

            //새정렬
            if (dgvProduct.Columns[e.ColumnIndex].Name == "up")
            {
                if (isSortDirc == "ASC")
                {
                    dgvProduct.Sort(
                        dgvProduct.Columns["up_lv"],
                        ListSortDirection.Ascending
                    );
                }
                else
                {
                    dgvProduct.Sort(
                        dgvProduct.Columns["up_lv"],
                        ListSortDirection.Descending
                    );
                }
            }
            else if (dgvProduct.Columns[e.ColumnIndex].Name == "down")
            {
                if (isSortDirc == "ASC")
                {
                    dgvProduct.Sort(
                        dgvProduct.Columns["down_lv"],
                        ListSortDirection.Ascending
                    );
                }
                else
                {
                    dgvProduct.Sort(
                        dgvProduct.Columns["down_lv"],
                        ListSortDirection.Descending
                    );
                }
            }
            else if (dgvProduct.Columns[e.ColumnIndex].Name == "margin")
            {
                if (isSortDirc == "ASC")
                {
                    dgvProduct.Sort(
                        dgvProduct.Columns["margin_double"],
                        ListSortDirection.Ascending
                    );
                }
                else
                {
                    dgvProduct.Sort(
                        dgvProduct.Columns["margin_double"],
                        ListSortDirection.Descending
                    );
                }
            }
            else
            {
                if (isSortDirc == "ASC")
                {
                    dgvProduct.Sort(
                        dgvProduct.Columns[e.ColumnIndex],
                        ListSortDirection.Ascending
                    );
                }
                else
                {
                    dgvProduct.Sort(
                        dgvProduct.Columns[e.ColumnIndex],
                        ListSortDirection.Descending
                    );
                }

            }
            //다시 변경
            if (isSortDirc == "ASC")
                isSortDirc = "DESC";
            else
                isSortDirc = "ASC";
            //서브품목 숨기기
            for (int i = dgvProduct.Rows.Count - 1; i >= 0; i--)
            {
                //숨김품목
                bool isHide;
                if (dgvProduct.Rows[i].Cells["sort_hide"].Value == null || !bool.TryParse(dgvProduct.Rows[i].Cells["sort_hide"].Value.ToString(), out isHide))
                    isHide = false;
                if (!isHide)
                {
                    try
                    {
                        dgvProduct.Rows[i].Visible = false;
                    }
                    catch
                    {
                        for (int j = 0; j < dgvProduct.Rows.Count; j++)
                        {
                            if (j != i && dgvProduct.Rows[j].Cells["product"].Visible)
                            {
                                dgvProduct.CurrentCell = dgvProduct.Rows[j].Cells["product"];
                                break;
                            }
                        }
                        dgvProduct.Rows[i].Visible = false;
                    }
                }


                //대표품목
                int main_id;
                if (dgvProduct.Rows[i].Cells["main_id"].Value == null || !int.TryParse(dgvProduct.Rows[i].Cells["main_id"].Value.ToString(), out main_id))
                    main_id = 0;

                int sub_id;
                if (dgvProduct.Rows[i].Cells["sub_id"].Value == null || !int.TryParse(dgvProduct.Rows[i].Cells["sub_id"].Value.ToString(), out sub_id))
                    sub_id = 0;
                if (main_id > 0 && sub_id != 9999)
                {
                    try
                    {
                        dgvProduct.Rows[i].Visible = false;
                    }
                    catch
                    {
                        for (int j = 0; j < dgvProduct.Rows.Count; j++)
                        {
                            if (j != i && dgvProduct.Rows[j].Cells["product"].Visible)
                            {
                                dgvProduct.CurrentCell = dgvProduct.Rows[j].Cells["product"];
                                break;
                            }
                        }
                        dgvProduct.Rows[i].Visible = false;
                    }
                }
                /*else if (main_id > 0 && sub_id == 9999)
                {
                    dgvProduct.Rows[i].HeaderCell.Value = "+";
                    dgvProduct.Rows[i].HeaderCell.Style.Font = new Font("나눔고딕", 10, FontStyle.Bold);
                    dgvProduct.Rows[i].HeaderCell.Style.ForeColor = Color.Yellow;
                    dgvProduct.Rows[i].Cells["product"].Style.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                    dgvProduct.Rows[i].Cells["product"].Style.BackColor = Color.DarkOrange;
                }*/
            }
            //Updown 컬러
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                dgvProduct.Rows[i].Cells["sales_cost_price"].ToolTipText = dgvProduct.Rows[i].Cells["sales_cost_price_tooltip"].Value.ToString();
                UpDownColor(dgvProduct.Rows[i]);
            }

            CalculateCostPriceMarginAmount();
        }

        #endregion

        #region DatatimePicker Textbox
        private void dtpSttdate_ValueChanged(object sender, EventArgs e)
        {
            txtdtpSttdateYear.Text = dtpSttdate.Value.Year.ToString();
            txtdtpSttdateMonth.Text = dtpSttdate.Value.Month.ToString();
            txtdtpSttdateDay.Text = dtpSttdate.Value.Day.ToString();
        }
        private void dtpEnddate_ValueChanged(object sender, EventArgs e)
        {
            txtdtpEnddateYear.Text = dtpEnddate.Value.Year.ToString();
            txtdtpEnddateMonth.Text = dtpEnddate.Value.Month.ToString();
            txtdtpEnddateDay.Text = dtpEnddate.Value.Day.ToString();
        }

        private void txtdtpSttdateYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            //삭제후 재입력
            if (txtdtpSttdateYear.Text.Length == 4)
            {
                txtdtpSttdateYear.Text = "";
            }
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }

        private void txtdtpSttdateMonth_KeyPress(object sender, KeyPressEventArgs e)
        {
            //삭제후 재입력
            if (txtdtpSttdateMonth.Text.Length == 2)
            {
                txtdtpSttdateMonth.Text = "";
            }
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }

        private void txtdtpSttdateDay_KeyPress(object sender, KeyPressEventArgs e)
        {
            //삭제후 재입력
            if (txtdtpSttdateDay.Text.Length == 2)
            {
                txtdtpSttdateDay.Text = "";
            }
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }
        private void txtdtpEnddateYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            //삭제후 재입력
            if (txtdtpEnddateYear.Text.Length == 4)
            {
                txtdtpSttdateYear.Text = "";
            }
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }
        private void txtdtpEnddateMonth_KeyPress(object sender, KeyPressEventArgs e)
        {
            //삭제후 재입력
            if (txtdtpEnddateMonth.Text.Length == 2)
            {
                txtdtpEnddateMonth.Text = "";
            }
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }

        private void txtdtpEnddateDay_KeyPress(object sender, KeyPressEventArgs e)
        {
            //삭제후 재입력
            if (txtdtpEnddateDay.Text.Length == 2)
            {
                txtdtpEnddateDay.Text = "";
            }
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }

        #endregion

        #region 우클릭 메뉴
        private void dgvProduct_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                foreach (DataGridViewColumn col in dgvProduct.Columns)
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;

                dgvProduct.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
                dgvProduct.Columns[e.ColumnIndex].Selected = true;

                HideSubProduct();

            }
        }
        private void dgvProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 & e.ColumnIndex >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    if (dgvProduct.SelectedRows.Count == 1)
                    {
                        dgvProduct.ClearSelection();
                        dgvProduct.Rows[e.RowIndex].Selected = true;
                    }
                    else
                    {
                        dgvProduct.Rows[e.RowIndex].Selected = true;
                    }
                }
                else
                {
                    dgvProduct.SelectionMode = DataGridViewSelectionMode.CellSelect;
                    dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                }
            }
        }
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

                    if (dgvProduct.SelectedRows.Count > 0)
                    {
                        m.Items.Add("상세 대시보드(D)");
                        m.Items.Add("상세 대시보드(영업)");
                        m.Items.Add("상세 대시보드(다중)");
                        ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
                        toolStripSeparator2.Name = "toolStripSeparator";
                        toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator2);
                        /*if (dgvProduct.Rows[row].Cells["main_id"].Value.ToString() == "0" 
                            && um.auth_level >= 50)*/
                        if (dgvProduct.Rows[row].Cells["main_id"].Value.ToString() == "0")
                        {
                            m.Items.Add("대표품목 설정");
                            ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
                            toolStripSeparator.Name = "toolStripSeparator";
                            toolStripSeparator.Size = new System.Drawing.Size(119, 6);
                            m.Items.Add(toolStripSeparator);
                        }
                        else if (dgvProduct.Rows[row].Cells["main_id"].Value.ToString() != "0")
                        {
                            m.Items.Add("대표품목 취소");
                            ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
                            toolStripSeparator.Name = "toolStripSeparator";
                            toolStripSeparator.Size = new System.Drawing.Size(119, 6);
                            m.Items.Add(toolStripSeparator);
                        }

                        double group_id;
                        if (double.TryParse(lbGroupId.Text, out group_id))
                        {
                            m.Items.Add("즐겨찾기 이동");
                            m.Items.Add("즐겨찾기 복사");
                            m.Items.Add("즐겨찾기 해제");
                            m.Items.Add("즐겨찾기 복제");
                            ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
                            toolStripSeparator.Name = "toolStripSeparator";
                            toolStripSeparator.Size = new System.Drawing.Size(119, 6);
                            m.Items.Add(toolStripSeparator);
                        }
                        else
                        {
                            m.Items.Add("즐겨찾기 등록");
                            ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
                            toolStripSeparator.Name = "toolStripSeparator";
                            toolStripSeparator.Size = new System.Drawing.Size(119, 6);
                            m.Items.Add(toolStripSeparator);
                        }

                    }
                    if (dgvProduct.SelectedRows.Count == 1)
                    {

                        m.Items.Add("팬딩내역 확인");
                        m.Items.Add("팬딩내역2 확인");
                        m.Items.Add("오퍼내역 확인");
                        m.Items.Add("역대단가 확인");
                        ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
                        toolStripSeparator1.Name = "toolStripSeparator";
                        toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator1);
                        m.Items.Add("제외품목");
                        m.Items.Add("통관일정");
                        ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
                        toolStripSeparator2.Name = "toolStripSeparator";
                        toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator2);
                        m.Items.Add("제외매출 등록");
                        m.Items.Add("제외매출 조회");
                    }
                    else if (dgvProduct.SelectedRows.Count > 1)
                        m.Items.Add("SPO 손익계산");                        
                    else if (dgvProduct.SelectedColumns.Count > 0)
                    {
                        m.Items.Add("컬럼 설정 초기화");
                        ToolStripSeparator toolStripSeparator3 = new ToolStripSeparator();
                        toolStripSeparator3.Name = "toolStripSeparator";
                        toolStripSeparator3.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator3);

                        m.Items.Add("열 숨기기");
                        if (dgvProduct.SelectedColumns.Count > 1)
                            m.Items.Add("열 펼치기");
                    }
                    else if (dgvProduct.SelectedRows.Count > 1)
                        m.Items.Add("일괄제외");

                    /*ToolStripSeparator toolStripSeparator3 = new ToolStripSeparator();
                    toolStripSeparator3.Name = "toolStripSeparator";
                    toolStripSeparator3.Size = new System.Drawing.Size(119, 6);
                    m.Items.Add(toolStripSeparator3);
                    m.Items.Add("글꼴 색상");
                    m.Items.Add("배경 색상");*/

                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    m.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cms_PreviewKeyDown);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(dgvProduct, e.Location);
                    //Selection
                    /*PendingList.ClearSelection();
                    DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                    selectRow.Selected = !selectRow.Selected;*/
                }
            }
            catch
            {
            }
        }

        

        private void cms_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                //우클릭 단축키
                case Keys.D:
                    if (dgvProduct.SelectedRows.Count > 0)
                    {
                        List<string[]> productInfoList = new List<string[]>();
                        for (int i = 0; i < dgvProduct.Rows.Count; i++)
                        {
                            DataGridViewRow dgvRow = dgvProduct.Rows[i];
                            bool isChecked = Convert.ToBoolean(dgvRow.Selected);
                            if (isChecked)
                            {
                                string[] productInfo = new string[15];

                                productInfo[0] = dgvRow.Cells["product"].Value.ToString();
                                productInfo[1] = dgvRow.Cells["origin"].Value.ToString();
                                productInfo[2] = dgvRow.Cells["sizes"].Value.ToString();
                                productInfo[3] = dgvRow.Cells["seaover_unit"].Value.ToString();
                                productInfo[4] = dgvRow.Cells["cost_unit"].Value.ToString();
                                productInfo[5] = dgvRow.Cells["purchase_price"].Value.ToString();

                                productInfo[6] = dgvRow.Cells["custom"].Value.ToString();
                                productInfo[7] = dgvRow.Cells["tax"].Value.ToString();
                                productInfo[8] = dgvRow.Cells["incidental_expense"].Value.ToString();
                                productInfo[9] = dgvRow.Cells["purchase_margin"].Value.ToString();
                                productInfo[10] = dgvRow.Cells["production_days"].Value.ToString();

                                productInfo[11] = dgvRow.Cells["offer_price"].Value.ToString();
                                productInfo[12] = dgvRow.Cells["offer_company"].Value.ToString();
                                productInfo[13] = dgvRow.Cells["offer_cost_price"].Value.ToString();
                                productInfo[14] = dgvRow.Cells["manager3"].Value.ToString();



                                productInfoList.Add(productInfo);
                            }
                        }
                        try
                        {
                            DetailDashboard dd = new DetailDashboard(um, productInfoList);
                            dd.Show();
                        }
                        catch
                        { }
                    }
                    break;
            }
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            if (dgvProduct.SelectedRows.Count > 0)
            {
                row = dgvProduct.SelectedRows[0];
            }

            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();

            switch (e.ClickedItem.Text)
            {
                case "제외품목":
                    row = dgvProduct.SelectedRows[0];
                    string category = row.Cells["category"].Value.ToString();
                    string product = row.Cells["product"].Value.ToString();
                    string sizes = row.Cells["sizes"].Value.ToString();
                    string origin = row.Cells["origin"].Value.ToString();
                    string seaover_unit = row.Cells["seaover_unit"].Value.ToString();
                    HideProductManagement hpm = new HideProductManagement(category, product, origin, sizes, seaover_unit, um);
                    hpm.StartPosition = FormStartPosition.Manual;
                    Point p = MousePosition;
                    int x = p.X - hpm.Size.Width - dgvProduct.ColumnHeadersHeight;
                    int y = p.Y - hpm.Size.Height + row.Height;
                    if (y < 12)
                    {
                        p = new Point(p.X - hpm.Size.Width - dgvProduct.ColumnHeadersHeight, 12);
                    }
                    else
                    {
                        p = new Point(p.X - hpm.Size.Width - dgvProduct.ColumnHeadersHeight, p.Y - hpm.Size.Height + row.Height);
                    }
                    bool isUpdate = hpm.SetHide(p);

                    if (isUpdate)
                    {
                        DataTable dt = hideRepository.GetHideTable(DateTime.Now.ToString("yyyy-MM-dd"), category, product, origin, sizes, seaover_unit);
                        if (dt.Rows.Count > 0)
                        {
                            row.Cells["hide_mode"].Value = dt.Rows[0]["hide_mode"].ToString();
                            row.Cells["hide_details"].Value = dt.Rows[0]["hide_details"].ToString();
                            row.Cells["hide_until_date"].Value = Convert.ToDateTime(dt.Rows[0]["until_date"].ToString()).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            row.Cells["hide_mode"].Value = "";
                            row.Cells["hide_details"].Value = "";
                            row.Cells["hide_until_date"].Value = "";
                        }
                    }

                    break;
                case "통관일정":
                    OpenExhaustedManager(row.Index);
                    break;
                case "대표품목 설정":
                    SetMainProduct(row);
                    break;
                case "일괄제외":
                    btnHide.PerformClick();
                    break;
                case "대표품목 취소":
                    {
                        //권한확인
                        DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                        if (authorityDt != null && authorityDt.Rows.Count > 0)
                        {
                            if (!common.CheckAuthority(authorityDt, "씨오버", "업체별시세관리", "is_update"))
                            {
                                messageBox.Show(this, "권한이 없습니다!");
                                return;
                            }
                        }

                        //설정데이터 삭제
                        ProductGroupModel model = new ProductGroupModel();
                        model.main_id = Convert.ToInt32(row.Cells["main_id"].Value.ToString());
                        model.product = row.Cells["product"].Value.ToString();
                        model.origin = row.Cells["origin"].Value.ToString();
                        model.sizes = row.Cells["sizes"].Value.ToString();
                        model.unit = row.Cells["unit"].Value.ToString();
                        model.price_unit = row.Cells["price_unit"].Value.ToString();
                        model.unit_count = row.Cells["unit_count"].Value.ToString();
                        model.seaover_unit = row.Cells["seaover_unit"].Value.ToString();

                        sql = productGroupRepository.DeleteProductGroup(model);
                        sqlList.Add(sql);

                        if (productGroupRepository.UpdateTrans(sqlList) == -1)
                        {
                            msgBox.Show(this, "삭제실패!");
                            this.Activate();
                            return;
                        }
                        //숨김처리 펼치기
                        CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dgvProduct.DataSource];
                        currencyManager1.SuspendBinding();
                        for (int i = 0; i < dgvProduct.Rows.Count; i++)
                        {
                            int main_id;
                            if (dgvProduct.Rows[i].Cells["main_id"].Value == null || !int.TryParse(dgvProduct.Rows[i].Cells["main_id"].Value.ToString(), out main_id))
                                main_id = 0;
                            int sub_id;
                            if (dgvProduct.Rows[i].Cells["sub_id"].Value == null || !int.TryParse(dgvProduct.Rows[i].Cells["sub_id"].Value.ToString(), out sub_id))
                                sub_id = 0;

                            //다시 품목 숨김,보이기
                            if (main_id == model.main_id && sub_id < 9999)
                            {
                                dgvProduct.Rows[i].Cells["main_id"].Value = 0;
                                dgvProduct.Rows[i].Cells["sub_id"].Value = 0;
                                dgvProduct.Rows[i].Visible = true;
                            }
                            else if (main_id == model.main_id && sub_id == 9999)
                                dgvProduct.Rows[i].Visible = false;
                        }
                        currencyManager1.ResumeBinding();
                        //datagridview data 수정
                        /*newDt.Rows.Remove(newDt.Rows[row.Index]);
                        dgvProduct.DataSource = newDt;*/
                    }
                    break;
                case "즐겨찾기 등록":
                    {
                        //권한확인
                        DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                        if (authorityDt != null && authorityDt.Rows.Count > 0)
                        {
                            if (!common.CheckAuthority(authorityDt, "씨오버", "업체별시세관리", "is_update"))
                            {
                                messageBox.Show(this, "권한이 없습니다!");
                                return;
                            }
                        }
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
                                    gModel.category = dgvRow.Cells["category"].Value.ToString();
                                    gModel.product = dgvRow.Cells["product"].Value.ToString();
                                    gModel.product_code = dgvRow.Cells["product_code"].Value.ToString();
                                    gModel.origin = dgvRow.Cells["origin"].Value.ToString();
                                    gModel.origin_code = dgvRow.Cells["origin_code"].Value.ToString();
                                    gModel.sizes = dgvRow.Cells["sizes"].Value.ToString();
                                    gModel.sizes_code = dgvRow.Cells["sizes_code"].Value.ToString();
                                    gModel.unit = dgvRow.Cells["unit"].Value.ToString();
                                    gModel.cost_unit = dgvRow.Cells["cost_unit"].Value.ToString();
                                    gModel.month_around = Convert.ToDouble(dgvRow.Cells["month_around"].Value.ToString());
                                    gModel.enable_days = Convert.ToDouble(dgvRow.Cells["enable_sales_days"].Value.ToString());
                                    gModel.price_unit = dgvRow.Cells["price_unit"].Value.ToString();
                                    gModel.unit_count = dgvRow.Cells["unit_count"].Value.ToString();
                                    gModel.seaover_unit = dgvRow.Cells["seaover_unit"].Value.ToString();
                                    gModel.division = dgvRow.Cells["division"].Value.ToString();

                                    list.Add(gModel);
                                }
                            }

                            if (list.Count > 0)
                            {
                                try
                                {
                                    SEAOVER.BookmarkManager bm = new BookmarkManager(um, this, list, 1);
                                    bm.ShowDialog();
                                }
                                catch
                                { }
                            }
                        }
                    }

                    break;
                case "즐겨찾기 복사":
                    {
                        //권한확인
                        DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                        if (authorityDt != null && authorityDt.Rows.Count > 0)
                        {
                            if (!common.CheckAuthority(authorityDt, "씨오버", "업체별시세관리", "is_update"))
                            {
                                messageBox.Show(this, "권한이 없습니다!");
                                return;
                            }
                        }
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
                                    gModel.category = dgvRow.Cells["category"].Value.ToString();
                                    gModel.product = dgvRow.Cells["product"].Value.ToString();
                                    gModel.product_code = dgvRow.Cells["product_code"].Value.ToString();
                                    gModel.origin = dgvRow.Cells["origin"].Value.ToString();
                                    gModel.origin_code = dgvRow.Cells["origin_code"].Value.ToString();
                                    gModel.sizes = dgvRow.Cells["sizes"].Value.ToString();
                                    gModel.sizes_code = dgvRow.Cells["sizes_code"].Value.ToString();
                                    gModel.unit = dgvRow.Cells["unit"].Value.ToString();
                                    gModel.price_unit = dgvRow.Cells["price_unit"].Value.ToString();
                                    gModel.unit_count = dgvRow.Cells["unit_count"].Value.ToString();
                                    gModel.seaover_unit = dgvRow.Cells["seaover_unit"].Value.ToString();
                                    gModel.division = dgvRow.Cells["division"].Value.ToString();
                                    gModel.month_around = Convert.ToDouble(dgvRow.Cells["month_around"].Value.ToString());
                                    gModel.enable_days = Convert.ToDouble(dgvRow.Cells["enable_sales_days"].Value.ToString());

                                    list.Add(gModel);
                                }
                            }

                            if (list.Count > 0)
                            {
                                SEAOVER.BookmarkManager bm = new BookmarkManager(um, this, list, 2);
                                bm.ShowDialog();
                            }
                        }
                    }
                    break;
                case "즐겨찾기 이동":

                    {
                        //권한확인
                        DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                        if (authorityDt != null && authorityDt.Rows.Count > 0)
                        {
                            if (!common.CheckAuthority(authorityDt, "씨오버", "업체별시세관리", "is_update"))
                            {
                                messageBox.Show(this, "권한이 없습니다!");
                                return;
                            }
                        }
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
                                    gModel.id = Convert.ToInt32(lbGroupId.Text);
                                    gModel.category = dgvRow.Cells["category"].Value.ToString();
                                    gModel.product = dgvRow.Cells["product"].Value.ToString();
                                    gModel.product_code = dgvRow.Cells["product_code"].Value.ToString();
                                    gModel.origin = dgvRow.Cells["origin"].Value.ToString();
                                    gModel.origin_code = dgvRow.Cells["origin_code"].Value.ToString();
                                    gModel.sizes = dgvRow.Cells["sizes"].Value.ToString();
                                    gModel.sizes_code = dgvRow.Cells["sizes_code"].Value.ToString();
                                    gModel.unit = dgvRow.Cells["unit"].Value.ToString();
                                    gModel.price_unit = dgvRow.Cells["price_unit"].Value.ToString();
                                    gModel.unit_count = dgvRow.Cells["unit_count"].Value.ToString();
                                    gModel.seaover_unit = dgvRow.Cells["seaover_unit"].Value.ToString();
                                    gModel.division = dgvRow.Cells["division"].Value.ToString();
                                    gModel.month_around = Convert.ToDouble(dgvRow.Cells["month_around"].Value.ToString());
                                    gModel.enable_days = Convert.ToDouble(dgvRow.Cells["enable_sales_days"].Value.ToString());

                                    list.Add(gModel);
                                }
                            }

                            if (list.Count > 0)
                            {
                                SEAOVER.BookmarkManager bm = new BookmarkManager(um, this, list, 3);
                                bm.ShowDialog();
                            }
                        }
                    }

                    break;

                case "즐겨찾기 해제":

                    {
                        //권한확인
                        DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                        if (authorityDt != null && authorityDt.Rows.Count > 0)
                        {
                            if (!common.CheckAuthority(authorityDt, "씨오버", "업체별시세관리", "is_delete"))
                            {
                                messageBox.Show(this, "권한이 없습니다!");
                                return;
                            }
                        }
                        if (dgvProduct.SelectedRows.Count > 0)
                        {
                            for (int i = dgvProduct.Rows.Count - 1; i >= 0; i--)
                            {
                                DataGridViewRow dgvRow = dgvProduct.Rows[i];
                                bool isChecked = Convert.ToBoolean(dgvRow.Selected);
                                if (isChecked)
                                {
                                    GroupModel gm = new GroupModel();
                                    gm.id = Convert.ToInt32(lbGroupId.Text);
                                    gm.division = "업체별시세현황";
                                    gm.category = dgvRow.Cells["category"].Value.ToString();
                                    gm.product = dgvRow.Cells["product"].Value.ToString();
                                    gm.origin = dgvRow.Cells["origin"].Value.ToString();
                                    gm.sizes = dgvRow.Cells["sizes"].Value.ToString();
                                    gm.unit = dgvRow.Cells["unit"].Value.ToString();
                                    gm.price_unit = dgvRow.Cells["price_unit"].Value.ToString();
                                    gm.unit_count = dgvRow.Cells["unit_count"].Value.ToString();
                                    gm.seaover_unit = dgvRow.Cells["seaover_unit"].Value.ToString();
                                    gm.division = dgvRow.Cells["division"].Value.ToString();
                                    gm.month_around = Convert.ToDouble(dgvRow.Cells["month_around"].Value.ToString());
                                    gm.enable_days = Convert.ToDouble(dgvRow.Cells["enable_sales_days"].Value.ToString());

                                    sql = groupRepository.DeleteGroup(gm);
                                    sqlList.Add(sql);

                                    dgvProduct.Rows.Remove(dgvRow);
                                }
                            }

                            if (sqlList.Count > 0)
                            {
                                if (groupRepository.UpdateTrans(sqlList) == -1)
                                {
                                    msgBox.Show(this, "삭제중 에러가 발생하였습니다.");
                                    this.Activate();
                                    return;
                                }
                            }
                        }
                    }

                    break;
                case "팬딩내역 확인":
                    {
                        //권한확인
                        DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                        if (authorityDt != null && authorityDt.Rows.Count > 0)
                        {
                            if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 수정", "is_visible"))
                            {
                                messageBox.Show(this, "권한이 없습니다!");
                                return;
                            }
                        }

                        if (dgvProduct.SelectedRows.Count > 0)
                        {
                            try
                            {
                                UnconfirmedPending ufp = new UnconfirmedPending(um.user_id
                                                                        , row.Cells["product"].Value.ToString()
                                                                        , row.Cells["origin"].Value.ToString()
                                                                        , row.Cells["sizes"].Value.ToString()
                                                                        , row.Cells["seaover_unit"].Value.ToString());
                                ufp.Show();
                            }
                            catch
                            { }
                        }
                        break;
                    }
                case "팬딩내역2 확인":
                    {
                        if (dgvProduct.SelectedRows.Count > 0)
                        {
                            //권한확인
                            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                            if (authorityDt != null && authorityDt.Rows.Count > 0)
                            {
                                if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 조회2", "is_visible"))
                                {
                                    messageBox.Show(this, "권한이 없습니다!");
                                    return;
                                }
                            }

                            try
                            {
                                PendingList2 ufp = new PendingList2(um
                                                                , row.Cells["product"].Value.ToString()
                                                                , row.Cells["origin"].Value.ToString()
                                                                , row.Cells["sizes"].Value.ToString()
                                                                , row.Cells["seaover_unit"].Value.ToString());
                                ufp.Show();
                            }
                            catch
                            { }
                        }
                        break;
                    }
                case "오퍼내역 확인":
                    {
                        //권한확인
                        DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                        if (authorityDt != null && authorityDt.Rows.Count > 0)
                        {
                            if (!common.CheckAuthority(authorityDt, "수입관리", "거래처별 매입단가 조회", "is_visible"))
                            {
                                messageBox.Show(this, "권한이 없습니다!");
                                return;
                            }
                        }
                        if (dgvProduct.SelectedRows.Count > 0)
                        {
                            try
                            {
                                PurchaseUnitManager pum = new PurchaseUnitManager(um
                                                                            , row.Cells["offer_updatetime"].Value.ToString()
                                                                            , row.Cells["product"].Value.ToString()
                                                                            , row.Cells["origin"].Value.ToString()
                                                                            , row.Cells["sizes"].Value.ToString()
                                                                            , row.Cells["seaover_unit"].Value.ToString());
                                pum.Show();
                            }
                            catch
                            { }
                        }
                        break;
                    }
                case "역대단가 확인":
                    {
                        //권한확인
                        DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                        if (authorityDt != null && authorityDt.Rows.Count > 0)
                        {
                            if (!common.CheckAuthority(authorityDt, "수입관리", "매입단가 그래프", "is_visible"))
                            {
                                messageBox.Show(this, "권한이 없습니다!");
                                return;
                            }
                        }

                        List<string[]> productList = new List<string[]>();
                        if (dgvProduct.SelectedRows.Count > 0)
                        {
                            row = dgvProduct.SelectedRows[0];

                            string[] item = new string[6];
                            item[0] = row.Cells["product"].Value.ToString();
                            item[1] = row.Cells["origin"].Value.ToString();
                            item[2] = row.Cells["sizes"].Value.ToString();
                            item[3] = row.Cells["seaover_unit"].Value.ToString();
                            item[4] = row.Cells["cost_unit"].Value.ToString();

                            double price;
                            if (row.Cells["offer_price"].Value == null || !double.TryParse(row.Cells["offer_price"].Value.ToString(), out price))
                            {
                                price = 0;
                            }
                            item[5] = price.ToString("#,##0.00");

                            productList.Add(item);
                        }
                        if (productList.Count > 0)
                        {
                            try
                            {
                                PurchaseManager.GraphManager gm = new GraphManager(um, productList);
                                gm.Show();
                            }
                            catch
                            { }
                        }
                    }
                    break;
                case "컬럼 설정 초기화":
                    SetHeaderStyle(true);
                    StyleSettingTxt();
                    break;
                case "열 숨기기":
                    foreach (DataGridViewColumn col in dgvProduct.SelectedColumns)
                        col.Visible = false;
                    break;
                case "열 펼치기":
                    int max_idx = dgvProduct.SelectedColumns[dgvProduct.SelectedColumns.Count - 1].Index;
                    int min_idx = dgvProduct.SelectedColumns[dgvProduct.SelectedColumns.Count - 1].Index;
                    foreach (DataGridViewColumn col in dgvProduct.SelectedColumns)
                    {
                        if (col.Index < min_idx)
                            min_idx = col.Index;
                        if (col.Index > max_idx)
                            max_idx = col.Index;
                    }

                    for (int i = min_idx; i <= max_idx; i++)
                    {
                        if (!(dgvProduct.Columns[i].Name == "main_id"
                            || dgvProduct.Columns[i].Name == "sub_id"
                            || dgvProduct.Columns[i].Name == "category_code"
                            || dgvProduct.Columns[i].Name == "category2"
                            || dgvProduct.Columns[i].Name == "category3"
                            || dgvProduct.Columns[i].Name == "product_code"
                            || dgvProduct.Columns[i].Name == "origin_code"
                            || dgvProduct.Columns[i].Name == "sizes_code"
                            || dgvProduct.Columns[i].Name == "sizes2"
                            || dgvProduct.Columns[i].Name == "sizes3"
                            || dgvProduct.Columns[i].Name == "sizes4"
                            || dgvProduct.Columns[i].Name == "box_weight"
                            || dgvProduct.Columns[i].Name == "reserved_stock_details"
                            || dgvProduct.Columns[i].Name == "average_day_sales_count_double"
                            || dgvProduct.Columns[i].Name == "margin_double"
                            || dgvProduct.Columns[i].Name == "normal_price_rate_txt"
                            || dgvProduct.Columns[i].Name == "sales_cost_price_tooltip"
                            || dgvProduct.Columns[i].Name == "sales_cost_price_detail"
                            || dgvProduct.Columns[i].Name == "up_lv"
                            || dgvProduct.Columns[i].Name == "down_lv"
                            || dgvProduct.Columns[i].Name == "hide_mode"
                            || dgvProduct.Columns[i].Name == "hide_details"
                            || dgvProduct.Columns[i].Name == "weight_calculate"
                            || dgvProduct.Columns[i].Name == "tray_calculate"
                            || dgvProduct.Columns[i].Name == "production_days"
                            || dgvProduct.Columns[i].Name == "delivery_days"
                            || dgvProduct.Columns[i].Name == "purchase_margin"
                            || dgvProduct.Columns[i].Name == "merge_product"
                            || dgvProduct.Columns[i].Name == "sort_hide"))
                            dgvProduct.Columns[i].Visible = true;
                    }
                    break;
                case "제외매출 등록":
                    {
                        ProductExcludedSalesModel pModel = new ProductExcludedSalesModel();
                        pModel.id = commonRepository.GetNextId("t_product_excluded_sales", "id");
                        pModel.product = row.Cells["product"].Value.ToString();
                        pModel.origin = row.Cells["origin"].Value.ToString();
                        pModel.sizes = row.Cells["sizes"].Value.ToString();
                        pModel.unit = row.Cells["unit"].Value.ToString();
                        pModel.price_unit = row.Cells["price_unit"].Value.ToString();
                        pModel.unit_count = row.Cells["unit_count"].Value.ToString();
                        pModel.seaover_unit = row.Cells["seaover_unit"].Value.ToString();
                        pModel.sale_qty = 0;

                        AddExcludedSales aes = new AddExcludedSales(um, this, pModel);
                        aes.ShowDialog();
                    }
                    break;
                case "제외매출 조회":
                    {
                        int saleTerm;
                        switch (cbSaleTerm.Text)
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
                        DateTime eDate;
                        if (!DateTime.TryParse(txtStockDate.Text, out eDate))
                            eDate = DateTime.Now;

                        string sttdate = eDate.AddMonths(-saleTerm).ToString("yyyy-MM-dd");
                        if (saleTerm == 45)
                            eDate.AddDays(-saleTerm).ToString("yyyy-MM-dd");
                        string enddate = eDate.ToString("yyyy-MM-dd");

                        ProductExcludedSalesModel pModel = new ProductExcludedSalesModel();
                        pModel.id = commonRepository.GetNextId("t_product_excluded_sales", "id");
                        pModel.product = row.Cells["product"].Value.ToString();
                        pModel.origin = row.Cells["origin"].Value.ToString();
                        pModel.sizes = row.Cells["sizes"].Value.ToString();
                        pModel.unit = row.Cells["unit"].Value.ToString();
                        pModel.price_unit = row.Cells["price_unit"].Value.ToString();
                        pModel.unit_count = row.Cells["unit_count"].Value.ToString();
                        pModel.seaover_unit = row.Cells["seaover_unit"].Value.ToString();
                        pModel.sale_qty = 0;
                        try
                        {
                            ExcludedSales es = new ExcludedSales(um, this, pModel, sttdate, enddate);
                            es.Show();
                        }
                        catch
                        { }
                    }
                    break;
                case "상세 대시보드(D)":
                    {

                        //권한확인
                        DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                        if (authorityDt != null && authorityDt.Rows.Count > 0)
                        {
                            if (!common.CheckAuthority(authorityDt, "수입관리", "원가 및 재고 대시보드", "is_visible"))
                            {
                                messageBox.Show(this, "권한이 없습니다!");
                                return;
                            }
                        }

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
                                    productInfo[3] = dgvRow.Cells["seaover_unit"].Value.ToString();
                                    productInfo[4] = dgvRow.Cells["cost_unit"].Value.ToString();
                                    productInfo[5] = dgvRow.Cells["purchase_price"].Value.ToString();

                                    productInfo[6] = dgvRow.Cells["custom"].Value.ToString();
                                    productInfo[7] = dgvRow.Cells["tax"].Value.ToString();
                                    productInfo[8] = dgvRow.Cells["incidental_expense"].Value.ToString();
                                    productInfo[9] = dgvRow.Cells["purchase_margin"].Value.ToString();
                                    productInfo[10] = dgvRow.Cells["production_days"].Value.ToString();

                                    productInfo[11] = dgvRow.Cells["offer_price"].Value.ToString();
                                    productInfo[12] = dgvRow.Cells["offer_company"].Value.ToString();
                                    productInfo[13] = dgvRow.Cells["offer_cost_price"].Value.ToString();
                                    productInfo[14] = dgvRow.Cells["manager3"].Value.ToString();

                                    productInfo[15] = dgvRow.Cells["product_code"].Value.ToString();
                                    productInfo[16] = dgvRow.Cells["origin_code"].Value.ToString();
                                    productInfo[17] = dgvRow.Cells["sizes_code"].Value.ToString();

                                    productInfo[18] = dgvRow.Cells["offer_updatetime"].Value.ToString();
                                    productInfo[19] = dgvRow.Cells["weight_calculate"].Value.ToString();

                                    productInfo[20] = dgvRow.Cells["price_unit"].Value.ToString();
                                    productInfo[21] = dgvRow.Cells["unit_count"].Value.ToString();
                                    productInfo[22] = dgvRow.Cells["seaover_unit"].Value.ToString();
                                    productInfo[23] = dgvRow.Cells["manager1"].Value.ToString();
                                    productInfo[24] = dgvRow.Cells["manager2"].Value.ToString();
                                    productInfo[25] = dgvRow.Cells["sales_price"].Value.ToString();
                                    productInfo[26] = dgvRow.Cells["division"].Value.ToString();
                                    productInfo[27] = dgvRow.Cells["delivery_days"].Value.ToString();

                                    productInfo[28] = dgvRow.Cells["average_purchase_price"].Value.ToString();
                                    productInfo[29] = dgvRow.Cells["bundle_count"].Value.ToString();


                                    productInfoList.Add(productInfo);
                                }
                            }
                            //무역부
                            if (um.auth_level >= 50)
                            {
                                DetailDashboard dd = null;
                                FormCollection fc = Application.OpenForms;
                                foreach (Form frm in fc)
                                {
                                    //iterate through
                                    if (frm.Name == "DetailDashboard")
                                    {

                                        if (msgBox.Show(this, "새창으로 대시보드를 여시겠습니까?" + "\n Yes : 새창       No : 기존창", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                            dd = null;
                                        else
                                        {
                                            dd = (DetailDashboard)frm;
                                            dd.InputClipboardData(productInfoList);
                                            dd.Activate();
                                        }
                                    }
                                    break;
                                }
                                //새로열기
                                if (dd == null)
                                {
                                    try
                                    {
                                        dd = new DetailDashboard(um, productInfoList);
                                        dd.Show();
                                    }
                                    catch
                                    { }
                                }
                            }
                            //영업부
                            else
                            {
                                int saleTerm;
                                switch (cbSaleTerm.Text)
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
                                        if (msgBox.Show(this, "새창으로 대시보드를 여시겠습니까?" + "\n Yes : 새창       No : 기존창", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                            dd = null;
                                        else
                                        {
                                            dd = (DetailDashBoardForSales)frm;
                                            dd.InputClipboardData(productInfoList);
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
                                    catch { }
                                }
                            }
                        }
                    }
                    break;
                case "상세 대시보드(영업)":
                    {
                        //권한확인
                        DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                        if (authorityDt != null && authorityDt.Rows.Count > 0)
                        {
                            if (!common.CheckAuthority(authorityDt, "씨오버", "품명별 매출관리 대시보드", "is_visible"))
                            {
                                messageBox.Show(this, "권한이 없습니다!");
                                return;
                            }
                        }

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
                                    productInfo[3] = dgvRow.Cells["unit"].Value.ToString();
                                    productInfo[4] = dgvRow.Cells["cost_unit"].Value.ToString();
                                    productInfo[5] = dgvRow.Cells["purchase_price"].Value.ToString();

                                    productInfo[6] = dgvRow.Cells["custom"].Value.ToString();
                                    productInfo[7] = dgvRow.Cells["tax"].Value.ToString();
                                    productInfo[8] = dgvRow.Cells["incidental_expense"].Value.ToString();
                                    productInfo[9] = dgvRow.Cells["purchase_margin"].Value.ToString();
                                    productInfo[10] = dgvRow.Cells["production_days"].Value.ToString();

                                    productInfo[11] = dgvRow.Cells["offer_price"].Value.ToString();
                                    productInfo[12] = dgvRow.Cells["offer_company"].Value.ToString();
                                    productInfo[13] = dgvRow.Cells["offer_cost_price"].Value.ToString();
                                    productInfo[14] = dgvRow.Cells["manager3"].Value.ToString();

                                    productInfo[15] = dgvRow.Cells["product_code"].Value.ToString();
                                    productInfo[16] = dgvRow.Cells["origin_code"].Value.ToString();
                                    productInfo[17] = dgvRow.Cells["sizes_code"].Value.ToString();

                                    productInfo[18] = dgvRow.Cells["offer_updatetime"].Value.ToString();
                                    productInfo[19] = dgvRow.Cells["weight_calculate"].Value.ToString();

                                    productInfo[20] = dgvRow.Cells["price_unit"].Value.ToString();
                                    productInfo[21] = dgvRow.Cells["unit_count"].Value.ToString();
                                    productInfo[22] = dgvRow.Cells["seaover_unit"].Value.ToString();
                                    productInfo[23] = dgvRow.Cells["manager1"].Value.ToString();
                                    productInfo[24] = dgvRow.Cells["manager2"].Value.ToString();
                                    productInfo[25] = dgvRow.Cells["sales_price"].Value.ToString();
                                    productInfo[26] = dgvRow.Cells["division"].Value.ToString();
                                    productInfo[27] = dgvRow.Cells["delivery_days"].Value.ToString();

                                    productInfo[28] = dgvRow.Cells["average_purchase_price"].Value.ToString();
                                    productInfo[29] = dgvRow.Cells["bundle_count"].Value.ToString();

                                    productInfoList.Add(productInfo);
                                }
                            }
                            int saleTerm;
                            switch (cbSaleTerm.Text)
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
                                    if (msgBox.Show(this, "새창으로 대시보드를 여시겠습니까?" + "\n Yes : 새창       No : 기존창", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                        dd = null;
                                    else
                                    {

                                        dd = (DetailDashBoardForSales)frm;
                                        dd.InputClipboardData(productInfoList);
                                        dd.Activate();
                                    }

                                    break;
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
                    }
                    break;
                case "상세 대시보드(다중)":
                    {
                        //권한확인
                        /*DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                        if (authorityDt != null && authorityDt.Rows.Count > 0)
                        {
                            if (!common.CheckAuthority(authorityDt, "씨오버", "품명별 매출관리 대시보드", "is_visible"))
                            {
                                messageBox.Show(this, "권한이 없습니다!");
                                return;
                            }
                        }*/

                        List<DataGridViewRow> productList = new List<DataGridViewRow>();
                        foreach (DataGridViewRow productRow in dgvProduct.SelectedRows)
                            productList.Add(productRow);



                        MultiDashBoard dd = null;
                        FormCollection fc = Application.OpenForms;
                        foreach (Form frm in fc)
                        {
                            //iterate through
                            if (frm.Name == "MultiDashBoard")
                            {
                                if (msgBox.Show(this, "새창으로 대시보드를 여시겠습니까?" + "\n Yes : 새창       No : 기존창", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    dd = null;
                                else
                                {

                                    dd = (MultiDashBoard)frm;
                                    dd.AddProduct(productList);
                                    dd.Activate();
                                }

                                break;
                            }
                        }
                        //새로열기
                        if (dd == null)
                        {
                            try
                            {
                                dd = new MultiDashBoard(um, productList);
                                dd.Show();
                                dd.WindowState = FormWindowState.Maximized;
                            }
                            catch
                            { }
                        }
                    }
                    break;

                case "즐겨찾기 복제":

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
                                gModel.id = Convert.ToInt32(lbGroupId.Text);
                                gModel.category = dgvRow.Cells["category"].Value.ToString();
                                gModel.product = dgvRow.Cells["product"].Value.ToString();
                                gModel.origin = dgvRow.Cells["origin"].Value.ToString();
                                gModel.sizes = dgvRow.Cells["sizes"].Value.ToString();
                                gModel.unit = dgvRow.Cells["unit"].Value.ToString();
                                gModel.price_unit = dgvRow.Cells["price_unit"].Value.ToString();
                                gModel.unit_count = dgvRow.Cells["unit_count"].Value.ToString();
                                gModel.seaover_unit = dgvRow.Cells["seaover_unit"].Value.ToString();
                                gModel.division = dgvRow.Cells["division"].Value.ToString();
                                gModel.month_around = Convert.ToDouble(dgvRow.Cells["month_around"].Value.ToString());
                                gModel.enable_days = Convert.ToDouble(dgvRow.Cells["enable_sales_days"].Value.ToString());

                                list.Add(gModel);
                            }
                        }

                        if (list.Count > 0)
                        {
                            DataTable groupDt = groupRepository.GetGroup(group_id);

                            SEAOVER.BookmarkManager bm = new BookmarkManager(um, this, list, 3);
                            bm.AddBookMark(groupDt.Rows[0]["group_name"].ToString(), groupDt.Rows[0]["form_name"].ToString());
                        }
                    }

                    break;
                case "SPO 손익계산":
                    CalculateCostPriceMarginAmount(true); 
                    break;
                case "글꼴 색상":
                    ChangeForeColor();
                    break;
                case "배경 색상":
                    ChangeBackColor();
                    break;

            }
        }

        public void SetExcludedSales()
        {
            if (dgvProduct.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvProduct.SelectedRows[0];

                int saleTerm;
                switch (cbSaleTerm.Text)
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
                DateTime eDate;
                if (!DateTime.TryParse(txtStockDate.Text, out eDate))
                    eDate = DateTime.Now;

                string sttdate = eDate.AddMonths(-saleTerm).ToString("yyyy-MM-dd");
                if (saleTerm == 45)
                    eDate.AddDays(-saleTerm).ToString("yyyy-MM-dd");
                string enddate = eDate.ToString("yyyy-MM-dd");
                DataTable productDt = productExcludedSalesRepository.GetExcludedSalesAsOne(sttdate, enddate
                    , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString()
                    , row.Cells["price_unit"].Value.ToString(), row.Cells["unit_count"].Value.ToString(), row.Cells["seaover_unit"].Value.ToString());
                double sale_qty = 0;
                if (productDt.Rows.Count > 0)
                    sale_qty = Convert.ToDouble(productDt.Rows[0]["sale_qty"].ToString());

                //제외수량
                row.Cells["excluded_qty"].Value = sale_qty;
                double sales_count;
                //판매수량
                if (row.Cells["sales_count"].Value == null || !double.TryParse(row.Cells["sales_count"].Value.ToString(), out sales_count))
                    sales_count = 0;
                //실제 반영수량
                double real_sales_count = sales_count - sale_qty;
                if (real_sales_count < 0)
                    real_sales_count = 0;
                //일매출
                double day_sales_count = real_sales_count / workDays;
                //월매출
                double month_sales_count = day_sales_count * 21;

                row.Cells["average_month_sales_count"].Value = month_sales_count.ToString("#,##0");
                row.Cells["average_day_sales_count"].Value = day_sales_count.ToString("#,##0");
                row.Cells["average_day_sales_count_double"].Value = day_sales_count;
            }

        }


        #endregion

        #region 대표품목 설정(병합)
        private void SetMainProduct(DataGridViewRow row)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "업체별시세관리", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            List<DataGridViewRow> list = new List<DataGridViewRow>();
            foreach (DataGridViewRow r in dgvProduct.SelectedRows)
                list.Add(r);


            if (list.Count > 0)
            {
                MergeManager mm = new MergeManager(um, list, this);
                mm.Owner = this;
                mm.Show();
            }
        }

        public void SetMergeProduct(DataGridView dgv, int main_id)
        {
            DataTable copyDt = newDt.Copy();
            copyDt.Rows.Clear();

            //메인품목 찾기
            DataRow main_row = copyDt.NewRow();
            for (int i = 0; i < newDt.Rows.Count; i++)
            {
                //대표품목 찾아서 먼저 출력
                for (int j = 0; j < dgv.Rows.Count; j++)
                {
                    if (newDt.Rows[i]["product"].ToString() == dgv.Rows[j].Cells["product"].Value.ToString()
                        && newDt.Rows[i]["origin"].ToString() == dgv.Rows[j].Cells["origin"].Value.ToString()
                        && newDt.Rows[i]["sizes"].ToString() == dgv.Rows[j].Cells["sizes"].Value.ToString()
                        && newDt.Rows[i]["unit"].ToString() == dgv.Rows[j].Cells["unit"].Value.ToString()
                        && newDt.Rows[i]["price_unit"].ToString() == dgv.Rows[j].Cells["price_unit"].Value.ToString()
                        && newDt.Rows[i]["unit_count"].ToString() == dgv.Rows[j].Cells["unit_count"].Value.ToString()
                        && newDt.Rows[i]["seaover_unit"].ToString() == dgv.Rows[j].Cells["seaover_unit"].Value.ToString()
                        && Convert.ToBoolean(dgv.Rows[j].Cells["chk"].Value))
                    {
                        main_row.ItemArray = newDt.Rows[i].ItemArray;
                        break;
                    }
                }
            }

            if (main_row == null)
                return;
            else
            {
                string merge_product = "";
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    merge_product += "\n" + dgv.Rows[i].Cells["product"].Value.ToString()
                                + "^" + dgv.Rows[i].Cells["origin"].Value.ToString()
                                + "^" + dgv.Rows[i].Cells["sizes"].Value.ToString()
                                + "^" + dgv.Rows[i].Cells["unit"].Value.ToString()
                                + "^" + dgv.Rows[i].Cells["price_unit"].Value.ToString()
                                + "^" + dgv.Rows[i].Cells["unit_count"].Value.ToString()
                                + "^" + dgv.Rows[i].Cells["seaover_unit"].Value.ToString();
                }


                main_row["main_id"] = main_id;
                main_row["sub_id"] = "9999";
                main_row["merge_product"] = merge_product.Trim();
            }

            bool isMainOutput = false;
            for (int i = 0; i < newDt.Rows.Count; i++)
            {
                DataRow dr = copyDt.NewRow();
                dr.ItemArray = newDt.Rows[i].ItemArray;

                //설정품목인지 아닌지 확인
                if (!isMainOutput)
                {
                    for (int j = 0; j < dgv.Rows.Count; j++)
                    {
                        if (dr["product"].ToString() == dgv.Rows[j].Cells["product"].Value.ToString()
                             && dr["origin"].ToString() == dgv.Rows[j].Cells["origin"].Value.ToString()
                             && dr["sizes"].ToString() == dgv.Rows[j].Cells["sizes"].Value.ToString()
                             && dr["unit"].ToString() == dgv.Rows[j].Cells["unit"].Value.ToString()
                             && dr["price_unit"].ToString() == dgv.Rows[j].Cells["price_unit"].Value.ToString()
                             && dr["unit_count"].ToString() == dgv.Rows[j].Cells["unit_count"].Value.ToString()
                             && dr["seaover_unit"].ToString() == dgv.Rows[j].Cells["seaover_unit"].Value.ToString())
                        {
                            isMainOutput = true;
                            break;
                        }
                    }

                    if (isMainOutput)
                    {
                        copyDt.Rows.Add(main_row);

                        for (int j = 0; j < dgv.Rows.Count; j++)
                        {
                            if (dr["product"].ToString() == dgv.Rows[j].Cells["product"].Value.ToString()
                                 && dr["origin"].ToString() == dgv.Rows[j].Cells["origin"].Value.ToString()
                                 && dr["sizes"].ToString() == dgv.Rows[j].Cells["sizes"].Value.ToString()
                                 && dr["unit"].ToString() == dgv.Rows[j].Cells["unit"].Value.ToString()
                                 && dr["price_unit"].ToString() == dgv.Rows[j].Cells["price_unit"].Value.ToString()
                                 && dr["unit_count"].ToString() == dgv.Rows[j].Cells["unit_count"].Value.ToString()
                                 && dr["seaover_unit"].ToString() == dgv.Rows[j].Cells["seaover_unit"].Value.ToString()
                                 && Convert.ToBoolean(dgv.Rows[j].Cells["chk"].Value))
                            {
                                dr["main_id"] = main_id;
                                dr["sub_id"] = "0";
                                break;
                            }
                            else if (dr["product"].ToString() == dgv.Rows[j].Cells["product"].Value.ToString()
                                 && dr["origin"].ToString() == dgv.Rows[j].Cells["origin"].Value.ToString()
                                 && dr["sizes"].ToString() == dgv.Rows[j].Cells["sizes"].Value.ToString()
                                 && dr["unit"].ToString() == dgv.Rows[j].Cells["unit"].Value.ToString()
                                 && dr["price_unit"].ToString() == dgv.Rows[j].Cells["price_unit"].Value.ToString()
                                 && dr["unit_count"].ToString() == dgv.Rows[j].Cells["unit_count"].Value.ToString()
                                 && dr["seaover_unit"].ToString() == dgv.Rows[j].Cells["seaover_unit"].Value.ToString())
                            {
                                dr["main_id"] = main_id;
                                dr["sub_id"] = "1";
                                break;
                            }
                        }

                        copyDt.Rows.Add(dr);
                    }
                    else
                        copyDt.Rows.Add(dr);

                }
                else
                {
                    for (int j = 0; j < dgv.Rows.Count; j++)
                    {
                        if (dr["product"].ToString() == dgv.Rows[j].Cells["product"].Value.ToString()
                             && dr["origin"].ToString() == dgv.Rows[j].Cells["origin"].Value.ToString()
                             && dr["sizes"].ToString() == dgv.Rows[j].Cells["sizes"].Value.ToString()
                             && dr["unit"].ToString() == dgv.Rows[j].Cells["unit"].Value.ToString()
                             && dr["price_unit"].ToString() == dgv.Rows[j].Cells["price_unit"].Value.ToString()
                             && dr["unit_count"].ToString() == dgv.Rows[j].Cells["unit_count"].Value.ToString()
                             && dr["seaover_unit"].ToString() == dgv.Rows[j].Cells["seaover_unit"].Value.ToString()
                             && Convert.ToBoolean(dgv.Rows[j].Cells["chk"].Value))
                        {
                            dr["main_id"] = main_id;
                            dr["sub_id"] = "0";
                            break;
                        }
                        else if (dr["product"].ToString() == dgv.Rows[j].Cells["product"].Value.ToString()
                             && dr["origin"].ToString() == dgv.Rows[j].Cells["origin"].Value.ToString()
                             && dr["sizes"].ToString() == dgv.Rows[j].Cells["sizes"].Value.ToString()
                             && dr["unit"].ToString() == dgv.Rows[j].Cells["unit"].Value.ToString()
                             && dr["price_unit"].ToString() == dgv.Rows[j].Cells["price_unit"].Value.ToString()
                             && dr["unit_count"].ToString() == dgv.Rows[j].Cells["unit_count"].Value.ToString()
                             && dr["seaover_unit"].ToString() == dgv.Rows[j].Cells["seaover_unit"].Value.ToString())
                        {
                            dr["main_id"] = main_id;
                            dr["sub_id"] = "1";
                            break;
                        }
                    }
                    copyDt.Rows.Add(dr);
                }


            }
            copyDt.AcceptChanges();
            newDt = copyDt;
            dgvProduct.DataSource = null;
            dgvProduct.DataSource = MergeProduct2(newDt);
            //Header, Cell style
            SetHeaderStyle();
            //HeaderText 변경
            HeaderTextChange();
            //서브품목 숨기기
            HideSubProduct();

        }
        private void SetData(DataGridViewRow row, double offer_qty = 0, double avg_sales_day = 0, double stock = 0, double shipment_stock = 0)
        {
            dgvProduct.EndEdit();
            //order etd
            if (row.Cells["order_etd"].Value != null)
                row.Cells["order_etd"].Value = common.strDatetime(row.Cells["order_etd"].Value.ToString());

            if(stock == 0)
                stock = Convert.ToDouble(row.Cells["seaover_unpending"].Value.ToString()) + Convert.ToDouble(row.Cells["seaover_pending"].Value.ToString()) - Convert.ToDouble(row.Cells["reserved_stock"].Value.ToString());
            if(shipment_stock == 0)
                shipment_stock = Convert.ToDouble(row.Cells["shipment_qty"].Value.ToString()) + Convert.ToDouble(row.Cells["unpending_qty_before"].Value.ToString());
            //stock += offer_qty;
            int delivery_days = Convert.ToInt16(row.Cells["delivery_days"].Value.ToString());
            if (delivery_days == 0)
                delivery_days = 15;
            //파라미터 매출량이 없으면 출력된 매출량
            if(avg_sales_day == 0)
                avg_sales_day = Convert.ToDouble(row.Cells["average_day_sales_count_double"].Value.ToString());
            //생산일
            int production_days;
            if (row.Cells["production_days"].Value == null || !int.TryParse(row.Cells["production_days"].Value.ToString(), out production_days))
                production_days = 20;
            //선적 or 배송내역
            DataTable copyDt = uppDt.Copy();
            DataRow[] uppRow = null;

            string whr = "";

            //대표품목이면서 일치 내역이 없으면 다시 seaover단위로 검색
            if (copyDt != null && copyDt.Rows.Count > 0)
            {
                if (Convert.ToInt32(row.Cells["main_id"].Value.ToString()) == 0 || row.Cells["merge_product"].Value == null || string.IsNullOrEmpty(row.Cells["merge_product"].Value.ToString()))
                {
                    whr = "product = '" + row.Cells["product"].Value.ToString() + "'"
                        + " AND origin = '" + row.Cells["origin"].Value.ToString() + "'"
                        + " AND sizes = '" + row.Cells["sizes"].Value.ToString() + "'"
                        + " AND box_weight = '" + row.Cells["seaover_unit"].Value.ToString() + "'";
                }
                else
                {
                    string[] sub_product = row.Cells["merge_product"].Value.ToString().Trim().Split('\n');
                    if (sub_product.Length > 0)
                    {
                        if (sub_product.Length == 1)
                        {
                            string[] products = sub_product[0].Split('^');
                            whr = "product = '" + products[0] + "'"
                            + " AND origin = '" + products[1] + "'"
                            + " AND sizes = '" + products[2] + "'"
                            + " AND box_weight = '" + products[6] + "'";
                        }
                        else
                        {

                            for (int i = 0; i < sub_product.Length; i++)
                            {
                                if (i == 0)
                                    whr = "(";

                                string[] products = sub_product[i].Split('^');
                                whr += "product = '" + products[0] + "'"
                                + " AND origin = '" + products[1] + "'"
                                + " AND sizes = '" + products[2] + "'"
                                + " AND box_weight = '" + products[6] + "'";
                                if (i < sub_product.Length - 1)
                                    whr += ") OR (";
                                else
                                    whr += ") ";
                            }
                        }
                    }
                }
            }
            DataTable copyUppDt = uppDt.Copy();
            uppRow = copyUppDt.Select(whr);

            TimeSpan ts;
            int sDay;
            //소진일자
            DateTime exhaust_date = DateTime.Now;
            //if ((!cbShipment.Checked || uppRow == null || uppRow.Length == 0) && stock == 0 && avg_sales_day == 0)
            if ((uppRow == null || uppRow.Length == 0) && stock == 0 && avg_sales_day == 0)
            {
                //소진일자
                row.Cells["exhausted_date"].Value = exhaust_date.ToString("yyyy-MM-dd");
                newDt.Rows[row.Index]["exhausted_date"] = exhaust_date.ToString("yyyy-MM-dd");

                //추천계약일
                DateTime contract_date = exhaust_date.AddDays(-(delivery_days + production_days + 5));
                row.Cells["contract_date"].Value = contract_date.ToString("yyyy-MM-dd");
                newDt.Rows[row.Index]["contract_date"] = contract_date.ToString("yyyy-MM-dd");
                //남은일
                ts = contract_date - DateTime.Now;
                int until_days = ts.Days;
                row.Cells["until_days1"].Value = until_days.ToString("#,##0");
                newDt.Rows[row.Index]["until_days1"] = until_days.ToString("#,##0");

                //최소ETD
                DateTime min_etd = exhaust_date.AddDays(-(delivery_days + 5));
                row.Cells["min_etd"].Value = min_etd.ToString("yyyy-MM-dd");
                newDt.Rows[row.Index]["min_etd"] = min_etd.ToString("yyyy-MM-dd");
                //남은일
                ts = min_etd - DateTime.Now;
                until_days = ts.Days;
                row.Cells["until_days2"].Value = until_days.ToString("#,##0");
                newDt.Rows[row.Index]["until_days2"] = until_days.ToString("#,##0");
                //2024-01-16===============================================================================================
                //2024-01-10 오더수량을 dr List추가
                if (offer_qty > 0)
                {
                    DateTime exhausted_date;
                    double exhausted_cnt;

                    //선적 or 오퍼 ETD까지 반영해서 계산하기
                    if (rbDetailReflectionShipmentQty.Checked)
                    {
                        DataRow offerDr = uppDt.NewRow();
                        DateTime offer_warehouse_date = exhaust_date.AddDays(delivery_days + production_days + 5);
                        DateTime offer_etd = offer_warehouse_date.AddDays(-(delivery_days + 5));
                        row.Cells["order_qty"].ToolTipText = "선적 : " + offer_etd.ToString("yyyy-MM-dd");
                        offerDr["warehousing_date"] = offer_warehouse_date.ToString("yyyy-MM-dd");
                        offerDr["quantity_on_paper"] = offer_qty;

                        //Data output
                        List<DataRow> dr = new List<DataRow>();  //선적내역
                        dr.Add(offerDr);

                        //소진일자 계산
                        common.GetExhausedDateDayd(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), stock, avg_sales_day, 0, dr, out exhausted_date, out exhausted_cnt, true);
                    }
                    //선적 or 오퍼 바로 실재고에 추가해서 계산하기
                    else if (rbDirectReflectionShipmentQty.Checked)
                    {
                        //소진일자 계산
                        common.GetExhausedDateDayd(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), stock + shipment_stock, avg_sales_day, 0, null, out exhausted_date, out exhausted_cnt, true);
                    }
                    //실재고에서 바로 계산하기
                    else 
                    {
                        //소진일자 계산
                        common.GetExhausedDateDayd(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), stock, avg_sales_day, 0, null, out exhausted_date, out exhausted_cnt, true);
                    }

                    //출력=============================================================================================================================================================================
                    if (exhausted_date.Year == 3000)
                    {
                        row.Cells["exhausted_date"].Value = "1년이상 판매가능";
                        row.Cells["exhausted_count"].Value = exhausted_cnt;
                        row.Cells["month_around_in_shipment2"].Value = "9999";

                        newDt.Rows[row.Index]["exhausted_date"] = "1년이상 판매가능";
                        newDt.Rows[row.Index]["exhausted_count"] = exhausted_cnt;
                        newDt.Rows[row.Index]["month_around_in_shipment2"] = "9999";

                    }
                    else
                    {
                        row.Cells["exhausted_date"].Value = exhausted_date.ToString("yyyy-MM-dd");
                        newDt.Rows[row.Index]["exhausted_date"] = exhaust_date.ToString("yyyy-MM-dd");
                        row.Cells["exhausted_count"].Value = exhausted_cnt;
                        newDt.Rows[row.Index]["exhausted_count"] = exhausted_cnt;
                        int enabled_days;
                        common.GetWorkDay(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), exhausted_date, out enabled_days);
                        double month_around_in_shipment2 = (double)enabled_days / 21;
                        row.Cells["month_around_in_shipment2"].Value = month_around_in_shipment2.ToString("#,##0.00");
                        newDt.Rows[row.Index]["month_around_in_shipment2"] = month_around_in_shipment2.ToString("#,##0.00");

                        //추천계약일
                        contract_date = exhausted_date.AddDays(-(delivery_days + production_days + 5));
                        row.Cells["contract_date"].Value = contract_date.ToString("yyyy-MM-dd");
                        newDt.Rows[row.Index]["contract_date"] = contract_date.ToString("yyyy-MM-dd");
                        ts = contract_date - DateTime.Now;
                        until_days = ts.Days;
                        row.Cells["until_days1"].Value = until_days.ToString("#,##0");
                        newDt.Rows[row.Index]["until_days1"] = until_days.ToString("#,##0");
                        //최소etd
                        min_etd = exhausted_date.AddDays(-(delivery_days + 5));
                        row.Cells["min_etd"].Value = min_etd.ToString("yyyy-MM-dd");
                        newDt.Rows[row.Index]["min_etd"] = min_etd.ToString("yyyy-MM-dd");
                        ts = min_etd - DateTime.Now;
                        until_days = ts.Days;
                        row.Cells["until_days2"].Value = until_days.ToString("#,##0");
                        newDt.Rows[row.Index]["until_days1"] = until_days.ToString("#,##0");

                    }
                }
            }
            else
            {
                if (stock / avg_sales_day > 365)
                {
                    row.Cells["exhausted_date"].Value = "1년이상 판매가능";
                    row.Cells["contract_date"].Value = "";
                    row.Cells["month_around_in_shipment2"].Value = "9999";

                    newDt.Rows[row.Index]["exhausted_date"] = "1년이상 판매가능";
                    newDt.Rows[row.Index]["contract_date"] = "";
                    newDt.Rows[row.Index]["month_around_in_shipment2"] = "9999";
                }
                else
                {
                    //Data output
                    List<DataRow> dr = new List<DataRow>();  //선적내역
                    if (uppRow != null && uppRow.Length > 0)
                    {
                        for (int j = 0; j < uppRow.Length; j++)
                        {
                            //계약만 한 상태 OR 배송중인 상태
                            string bl_no = uppRow[j]["bl_no"].ToString();
                            string warehousing_date = uppRow[j]["warehousing_date"].ToString();
                            double cost_price;
                            if (!double.TryParse(uppRow[j]["cost_price"].ToString(), out cost_price))
                                cost_price = 0;
                            if (string.IsNullOrEmpty(bl_no) || (!string.IsNullOrEmpty(bl_no) && string.IsNullOrEmpty(warehousing_date)) || cost_price > 0)
                            {
                                //대표품목일 경우 수량 변환
                                if (row.Cells["sub_id"].Value.ToString() == "9999")
                                {
                                    double seaover_unit = Convert.ToDouble(row.Cells["seaover_unit"].Value.ToString());
                                    uppRow[j]["quantity_on_paper"] = Convert.ToDouble(uppRow[j]["quantity_on_paper"].ToString()) * Convert.ToDouble(uppRow[j]["box_weight"].ToString()) / seaover_unit;
                                }

                                DateTime etd, eta, warehouse_date;
                                if (DateTime.TryParse(uppRow[j]["eta"].ToString(), out eta))
                                {
                                    int pending_day = 5;
                                    //common.GetPlusDay(eta, pending_day, out warehouse_date);
                                    warehouse_date = eta.AddDays(pending_day);
                                    if (warehouse_date <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                                        warehouse_date = DateTime.Now.AddDays(5);
                                    uppRow[j]["warehousing_date"] = warehouse_date.ToString("yyyy-MM-dd");
                                    dr.Add(uppRow[j]);
                                }
                                else if (DateTime.TryParse(uppRow[j]["etd"].ToString(), out etd))
                                {
                                    int pending_day = delivery_days + 5;
                                    //common.GetPlusDay(etd, pending_day, out warehouse_date);
                                    warehouse_date = etd.AddDays(pending_day);
                                    if (warehouse_date <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                                        warehouse_date = DateTime.Now.AddDays(5);
                                    uppRow[j]["warehousing_date"] = warehouse_date.ToString("yyyy-MM-dd");
                                    dr.Add(uppRow[j]);
                                }
                                else if (cost_price > 0)
                                    dr.Add(uppRow[j]);
                                else
                                    warehouse_date = DateTime.Now.AddDays(5);
                            }
                        }
                    }

                    List<DataRow> inPendingList = dr;
                    List<DataRow> outPendingList = new List<DataRow>();


                    //2024-01-10 오더수량을 dr List추가
                    if (offer_qty != 0)
                    {
                        DataRow offerDr = uppDt.NewRow();

                        if (row.Cells["order_etd"].Value == null || !DateTime.TryParse(row.Cells["order_etd"].Value.ToString(), out DateTime etd))
                        {
                            DateTime offer_warehouse_date = exhaust_date.AddDays(delivery_days + production_days + 5);
                            offerDr["warehousing_date"] = offer_warehouse_date.ToString("yyyy-MM-dd");
                            DateTime offer_etd = exhaust_date.AddDays(production_days);
                            row.Cells["order_etd"].Value = offer_etd.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            DateTime offer_warehouse_date = etd.AddDays(delivery_days + 5);
                            offerDr["warehousing_date"] = offer_warehouse_date.ToString("yyyy-MM-dd");
                        }
                        offerDr["quantity_on_paper"] = offer_qty;
                        inPendingList.Add(offerDr);
                        outPendingList.Add(offerDr);
                    }

                    //선적포함 소진일자 계산
                    DateTime exhausted_date;
                    double exhausted_cnt;
                    int enabled_days;

                    {
                        //선적포함 체크, ETD날짜 반영 체크
                        if (rbDetailReflectionShipmentQty.Checked)
                            common.GetExhausedDateDayd(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), stock, avg_sales_day, 0, inPendingList, out exhausted_date, out exhausted_cnt, true);
                        //선적포함 체크, ETD날짜 반영 미체크
                        else if (rbDirectReflectionShipmentQty.Checked)
                            common.GetExhausedDateDayd(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), stock + shipment_stock + offer_qty, avg_sales_day, 0, null, out exhausted_date, out exhausted_cnt, true);
                        //실재고에서 바로 계산
                        else 
                            common.GetExhausedDateDayd(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), stock + offer_qty, avg_sales_day, 0, null, out exhausted_date, out exhausted_cnt, true);

                        //남은일 계산
                        if (exhausted_date.Year == 3000)
                        {
                            row.Cells["month_around_in_shipment2"].Value = "9999";
                            row.Cells["exhausted_date"].Value = "1년이상 판매가능";
                            row.Cells["exhausted_count"].Value = exhausted_cnt;

                            newDt.Rows[row.Index]["month_around_in_shipment2"] = "9999";
                            newDt.Rows[row.Index]["exhausted_date"] = "1년이상 판매가능";
                            newDt.Rows[row.Index]["exhausted_count"] = exhausted_cnt;
                        }
                        else
                        {
                            //판매가능일2
                            common.GetWorkDay(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), exhausted_date, out enabled_days);
                            double month_around_in_shipment2 = (double)enabled_days / 21;
                            row.Cells["month_around_in_shipment2"].Value = month_around_in_shipment2.ToString("#,##0.00");
                            row.Cells["enable_sales_days"].Value = enabled_days;
                            row.Cells["exhausted_date"].Value = exhausted_date.ToString("yyyy-MM-dd");
                            row.Cells["exhausted_count"].Value = exhausted_cnt;

                            newDt.Rows[row.Index]["month_around_in_shipment2"] = month_around_in_shipment2.ToString("#,##0.00");
                            newDt.Rows[row.Index]["enable_sales_days"] = enabled_days;
                            newDt.Rows[row.Index]["exhausted_date"] = exhausted_date.ToString("yyyy-MM-dd");
                            newDt.Rows[row.Index]["exhausted_count"] = exhausted_cnt;


                            //추천계약일
                            DateTime contract_date = exhausted_date.AddDays(-(delivery_days + production_days + 5));
                            row.Cells["contract_date"].Value = contract_date.ToString("yyyy-MM-dd");
                            newDt.Rows[row.Index]["contract_date"] = contract_date.ToString("yyyy-MM-dd");
                            ts = contract_date - DateTime.Now;
                            int until_days = ts.Days;
                            row.Cells["until_days1"].Value = until_days.ToString("#,##0");
                            newDt.Rows[row.Index]["until_days1"] = until_days.ToString("#,##0");
                            //최소etd
                            DateTime min_etd = exhausted_date.AddDays(-(delivery_days + 5));
                            row.Cells["min_etd"].Value = min_etd.ToString("yyyy-MM-dd");
                            newDt.Rows[row.Index]["min_etd"] = min_etd.ToString("yyyy-MM-dd");
                            ts = min_etd - DateTime.Now;
                            until_days = ts.Days;
                            row.Cells["until_days2"].Value = until_days.ToString("#,##0");
                            newDt.Rows[row.Index]["until_days2"] = until_days.ToString("#,##0");
                        }
                    }
                }
            }
        }

        private void GetMonthRound(DataGridViewRow row, DataRow[] uppRow, double offer_qty, double avg_sales_day
                                    , out DateTime exhausted_date, out double exhausted_cnt
                                    , out DateTime contract_date, out int until_days
                                    , out DateTime min_etd, out int until_days2
                                    , out double month_around_in_shipment2)
        {
            double stock = Convert.ToDouble(row.Cells["seaover_unpending"].Value.ToString()) + Convert.ToDouble(row.Cells["seaover_pending"].Value.ToString()) - Convert.ToDouble(row.Cells["reserved_stock"].Value.ToString());
            double shipment_stock = Convert.ToDouble(row.Cells["shipment_qty"].Value.ToString()) + Convert.ToDouble(row.Cells["unpending_qty_before"].Value.ToString());
            //stock += offer_qty;
            int delivery_days = Convert.ToInt16(row.Cells["delivery_days"].Value.ToString());
            if (delivery_days == 0)
                delivery_days = 15;
            //파라미터 매출량이 없으면 출력된 매출량
            if (avg_sales_day == 0)
                avg_sales_day = Convert.ToDouble(row.Cells["average_day_sales_count_double"].Value.ToString());
            //생산일
            int production_days;
            if (row.Cells["production_days"].Value == null || !int.TryParse(row.Cells["production_days"].Value.ToString(), out production_days))
                production_days = 20;


            //소진일자
            exhausted_date = DateTime.Now;
            exhausted_cnt = 0;

            //추천계약일
            contract_date = exhausted_date.AddDays(-(delivery_days + production_days + 5));
            //row.Cells["contract_date"].Value = contract_date.ToString("yyyy-MM-dd");
            TimeSpan ts = contract_date - DateTime.Now;
            until_days = ts.Days;
            //최소etd
            min_etd = exhausted_date.AddDays(-(delivery_days + 5));
            //row.Cells["min_etd"].Value = min_etd.ToString("yyyy-MM-dd");
            ts = min_etd - DateTime.Now;
            until_days2 = ts.Days;

            //선적일 포함 회전율
            month_around_in_shipment2 = 0;

            //ETD날짜 반영
            if (rbDetailReflectionShipmentQty.Checked)
            {
                //팬딩없고, 재고, 판매량 전부없음
                if ((uppRow == null || uppRow.Length == 0) && stock == 0 && avg_sales_day == 0)
                {
                    //2024-01-10 오더수량을 dr List추가
                    if (offer_qty > 0)
                    {
                        DataRow offerDr = uppDt.NewRow();

                        DateTime offer_warehouse_date = exhausted_date.AddDays(delivery_days + production_days + 5);      //입고일자
                        DateTime offer_etd = offer_warehouse_date.AddDays(-(delivery_days + 5));                          //ETD

                        //Data output
                        List<DataRow> dr = new List<DataRow>();  //선적내역
                        dr.Add(offerDr);

                        //소진일자 계산
                        common.GetExhausedDateDayd(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), stock, avg_sales_day, 0, dr, out exhausted_date, out exhausted_cnt, true);

                        //출력
                        if (exhausted_date.Year == 1900)
                            month_around_in_shipment2 = 9999;
                        else if (exhausted_date.Year > 1900)
                        {
                            int enabled_days;
                            common.GetWorkDay(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), exhausted_date, out enabled_days);
                            month_around_in_shipment2 = (double)enabled_days / 21;

                            //추천계약일
                            contract_date = exhausted_date.AddDays(-(delivery_days + production_days + 5));
                            ts = contract_date - DateTime.Now;
                            until_days = ts.Days;
                            //최소etd
                            min_etd = exhausted_date.AddDays(-(delivery_days + 5));
                            ts = min_etd - DateTime.Now;
                            until_days2 = ts.Days;
                        }
                    }
                }
                else
                {
                    if (stock / avg_sales_day > 365)
                        month_around_in_shipment2 = 9999;
                    else
                    {
                        //Data output
                        List<DataRow> dr = new List<DataRow>();  //선적내역
                        if (uppRow != null && uppRow.Length > 0)
                        {
                            for (int j = 0; j < uppRow.Length; j++)
                            {
                                //계약만 한 상태 OR 배송중인 상태
                                string bl_no = uppRow[j]["bl_no"].ToString();
                                string warehousing_date = uppRow[j]["warehousing_date"].ToString();
                                double cost_price;
                                if (!double.TryParse(uppRow[j]["cost_price"].ToString(), out cost_price))
                                    cost_price = 0;
                                if (string.IsNullOrEmpty(bl_no) || (!string.IsNullOrEmpty(bl_no) && string.IsNullOrEmpty(warehousing_date)) || cost_price > 0)
                                {
                                    //대표품목일 경우 수량 변환
                                    if (row.Cells["sub_id"].Value.ToString() == "9999")
                                    {
                                        double seaover_unit = Convert.ToDouble(row.Cells["seaover_unit"].Value.ToString());
                                        uppRow[j]["quantity_on_paper"] = Convert.ToDouble(uppRow[j]["quantity_on_paper"].ToString()) * Convert.ToDouble(uppRow[j]["box_weight"].ToString()) / seaover_unit;
                                    }


                                    DateTime etd, eta, warehouse_date;
                                    if (DateTime.TryParse(uppRow[j]["eta"].ToString(), out eta))
                                    {
                                        int pending_day = 5;
                                        //common.GetPlusDay(eta, pending_day, out warehouse_date);
                                        warehouse_date = eta.AddDays(pending_day);
                                        if (warehouse_date <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                                            warehouse_date = DateTime.Now.AddDays(5);
                                        uppRow[j]["warehousing_date"] = warehouse_date.ToString("yyyy-MM-dd");
                                        dr.Add(uppRow[j]);
                                    }
                                    else if (DateTime.TryParse(uppRow[j]["etd"].ToString(), out etd))
                                    {
                                        int pending_day = delivery_days + 5;
                                        //common.GetPlusDay(etd, pending_day, out warehouse_date);
                                        warehouse_date = etd.AddDays(pending_day);
                                        if (warehouse_date <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                                            warehouse_date = DateTime.Now.AddDays(5);
                                        uppRow[j]["warehousing_date"] = warehouse_date.ToString("yyyy-MM-dd");
                                        dr.Add(uppRow[j]);
                                    }
                                    else if (cost_price > 0)
                                        dr.Add(uppRow[j]);
                                    else
                                        warehouse_date = DateTime.Now.AddDays(5);
                                }
                            }
                        }

                        List<DataRow> inPendingList = dr;
                        List<DataRow> outPendingList = new List<DataRow>();


                        //2024-01-10 오더수량을 dr List추가
                        if (offer_qty > 0)
                        {
                            DataRow offerDr = uppDt.NewRow();

                            if (row.Cells["order_etd"].Value == null || !DateTime.TryParse(row.Cells["order_etd"].Value.ToString(), out DateTime etd))
                            {
                                DateTime offer_warehouse_date = exhausted_date.AddDays(delivery_days + production_days + 5);
                                DateTime offer_etd = exhausted_date.AddDays(production_days);
                                offerDr["warehousing_date"] = offer_warehouse_date.ToString("yyyy-MM-dd");
                                offerDr["quantity_on_paper"] = offer_qty;
                            }
                            else
                            {
                                DateTime offer_warehouse_date = etd.AddDays(delivery_days + 5);
                                offerDr["warehousing_date"] = offer_warehouse_date.ToString("yyyy-MM-dd");
                                offerDr["quantity_on_paper"] = offer_qty;
                            }
                            inPendingList.Add(offerDr);
                            outPendingList.Add(offerDr);
                        }

                        //2024-01-12 선적체크안해도 회전율(선적일반영)은 계산하게
                        //선적포함 소진일자 계산
                        common.GetExhausedDateDayd(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), stock, avg_sales_day, 0, inPendingList, out exhausted_date, out exhausted_cnt, true);

                        if (exhausted_date.Year == 1900)
                            month_around_in_shipment2 = 9999;
                        else
                        {
                            int enabled_days;
                            common.GetWorkDay(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), exhausted_date, out enabled_days);
                            month_around_in_shipment2 = (double)enabled_days / 21;

                            //추천계약일
                            contract_date = exhausted_date.AddDays(-(delivery_days + production_days + 5));
                            ts = contract_date - DateTime.Now;
                            until_days = ts.Days;
                            //최소etd
                            min_etd = exhausted_date.AddDays(-(delivery_days + 5));
                            ts = min_etd - DateTime.Now;
                            until_days2 = ts.Days;
                        }


                        //선전미포함 소진일자 계산
                        common.GetExhausedDateDayd(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), stock, avg_sales_day, 0, outPendingList, out exhausted_date, out exhausted_cnt, true);
                        if (exhausted_date.Year == 1900)
                            month_around_in_shipment2 = 9999;
                        else
                        {
                            //추천계약일
                            contract_date = exhausted_date.AddDays(-(delivery_days + production_days + 5));
                            ts = contract_date - DateTime.Now;
                            until_days = ts.Days;
                            //최소etd
                            min_etd = exhausted_date.AddDays(-(delivery_days + 5));
                            ts = min_etd - DateTime.Now;
                            until_days2 = ts.Days;
                        }
                    }
                }
            }
            //ETD날짜 반영X
            else
            {
                //선적포함
                if (rbDirectReflectionShipmentQty.Checked)
                    stock += shipment_stock + offer_qty;
                //선적미포함
                else
                    stock += offer_qty;
                double enable_sales_day = stock / avg_sales_day;
                month_around_in_shipment2 = enable_sales_day / 21;
            }
        }


        private void CalculateMonthAround(DataRow row)
        {
            row.AcceptChanges();
            //회전율 및 소트일자 계산
            double real_stock = Convert.ToDouble(row["seaover_unpending"].ToString()) + Convert.ToDouble(row["seaover_pending"].ToString()) - Convert.ToDouble(row["reserved_stock"].ToString());
            double shipment_stock = Convert.ToDouble(row["shipment_qty"].ToString()) + Convert.ToDouble(row["unpending_qty_before"].ToString());
            row["real_stock"] = real_stock;
            row["shipment_stock"] = shipment_stock;
            row["total_stock"] = real_stock + shipment_stock;
            row["total_real_stock"] = real_stock + shipment_stock;
            //평균원가1=======================================================================================================
            double seaover_cost_price;
            if (!double.TryParse(row["sales_cost_price"].ToString(), out seaover_cost_price))
                seaover_cost_price = 0;

            double pending_cost_price;
            if (!double.TryParse(row["pending_cost_price"].ToString(), out pending_cost_price))
                pending_cost_price = 0;


            double average_total_cost_price = 0, average_total_qty = 0;
            if (real_stock > 0 && seaover_cost_price > 0)
            {
                average_total_cost_price += seaover_cost_price * real_stock;
                average_total_qty += real_stock;
            }
            if (shipment_stock > 0 && pending_cost_price > 0)
            {
                average_total_cost_price += pending_cost_price * shipment_stock;
                average_total_qty += shipment_stock;
            }

            double average_cost_price = average_total_cost_price / average_total_qty;
            if (double.IsNaN(average_cost_price))
                average_cost_price = 0;
            else if (double.IsInfinity(average_cost_price))
                average_cost_price = 0;
            if (average_cost_price == 0 && seaover_cost_price > 0)
                average_cost_price = seaover_cost_price;
            else if (average_cost_price == 0 && pending_cost_price > 0)
                average_cost_price = pending_cost_price;


            row["average_sales_cost_price1"] = average_cost_price;

            

            double temp_price = 0;
            if (rbSalesPriceMargin.Checked)
            {
                if (!double.TryParse(row["sales_price"].ToString(), out temp_price))
                    temp_price = 0;
            }
            else if (rbNormalPriceMargin.Checked)
            {
                if (!double.TryParse(row["normal_price"].ToString(), out temp_price))
                    temp_price = 0;
            }
            else if (rbPurchasePrice.Checked)
            {
                if (!double.TryParse(row["purchase_price"].ToString(), out temp_price))
                    temp_price = 0;
            }

            //매출단가 마진
            double sales_price;
            if (!double.TryParse(row["sales_price"].ToString(), out sales_price))
                sales_price = 0;
            double purchase_price;
            if (!double.TryParse(row["purchase_price"].ToString(), out purchase_price))
                purchase_price = 0;
            double margin = (sales_price - purchase_price) / sales_price;
            if (double.IsNaN(margin))
                margin = 0;
            else if (double.IsInfinity(margin))
                margin = 0;
            row["margin"] = (margin * 100).ToString("#,##0.00");

            //평균원가1 마진
            if (temp_price > 0 && average_cost_price > 0)
            {
                double average_cost_margin = (temp_price - average_cost_price) / temp_price * 100;
                if (double.IsNaN(average_cost_margin))
                    average_cost_margin = 0;
                row["average_sales_cost_price1_margin"] = average_cost_margin;
            }
            else
                row["average_sales_cost_price1_margin"] = 0;

            //평균원가2
            double offer_cost_price = Convert.ToDouble(row["offer_cost_price"].ToString());
            double offer_order_qty = Convert.ToDouble(row["order_qty"].ToString());
            double total_cost_amount = (seaover_cost_price * real_stock) + (pending_cost_price * shipment_stock) + (offer_cost_price * offer_order_qty);
            double total_cost_qty = real_stock + shipment_stock + offer_order_qty;

            double average_cost_price2 = total_cost_amount / total_cost_qty;
            if (double.IsNaN(average_cost_price2))
                average_cost_price2 = 0;
            else if (double.IsInfinity(average_cost_price2))
                average_cost_price2 = 0;

            if (average_cost_price2 == 0 && seaover_cost_price > 0)
                average_cost_price2 = seaover_cost_price;
            else if (average_cost_price2 == 0 && pending_cost_price > 0)
                average_cost_price2 = pending_cost_price;
            else if (average_cost_price2 == 0 && offer_cost_price > 0)
                average_cost_price2 = offer_cost_price;

            row["average_sales_cost_price2"] = average_cost_price2;
            //평균원가2 마진
            if (temp_price > 0 && average_cost_price2 > 0)
            {
                double average_cost_margin = (temp_price - average_cost_price2) / temp_price * 100;
                if (double.IsNaN(average_cost_margin))
                    average_cost_margin = 0;
                row["average_sales_cost_price2_margin"] = average_cost_margin;
            }
            else
                row["average_sales_cost_price2_margin"] = 0;

            //일판매량, 월판매량=======================================================================================================
            double sales_count = Convert.ToDouble(row["sales_count"].ToString()) - Convert.ToDouble(row["excluded_qty"].ToString());
            double avg_day_sales = sales_count / workDays;            //일판매량
            double average_month_sales_count = avg_day_sales * 21;    //월평균판매량
            //매출기간
            switch (cbSaleTerm.Text)
            {
                case "1개월":
                    average_month_sales_count = sales_count;          //매출기간 1달이면 실제 매출량으로 변경
                    break;
            }

            row["average_month_sales_count"] = average_month_sales_count;
            row["average_day_sales_count_double"] = avg_day_sales;
            row["average_day_sales_count"] = avg_day_sales.ToString("#,##0");
            //회전율====================================================================================================================
            double month_around, month_around_in_shipment, enable_sales_days;            
            //선적미포함 회전율
            if (real_stock == 0 && sales_count == 0)
                month_around = 0;
            else if (real_stock > 0 && sales_count == 0)
                month_around = 9999;
            else if (real_stock == 0 && sales_count > 0)
                month_around = 0;
            else
            {
                enable_sales_days = (real_stock / avg_day_sales);
                if (double.IsNaN(enable_sales_days))
                    enable_sales_days = 0;

                //선적미포함 회전율
                month_around = enable_sales_days / 21;
                if (rbNotReflectionShipmentQty.Checked)
                    row["enable_sales_days"] = enable_sales_days;
            }
            row["month_around"] = month_around;
            //선적포함 회전율
            if (real_stock + shipment_stock == 0 && sales_count == 0)
                month_around_in_shipment = 0;
            else if (real_stock + shipment_stock > 0 && sales_count == 0)
                month_around_in_shipment = 9999;
            else if (real_stock + shipment_stock == 0 && sales_count > 0)
                month_around_in_shipment = 0;
            else
            {
                enable_sales_days = ((real_stock + shipment_stock) / avg_day_sales);
                if (double.IsNaN(enable_sales_days))
                    enable_sales_days = 0;
                month_around_in_shipment = enable_sales_days / 21;
            }
            row["month_around_in_shipment"] = month_around_in_shipment;
            //제외품목================================================================================================
            if (hdDt != null && hdDt.Rows.Count > 0)
            {
                string whr = "category = '" + row["category"].ToString() + "'"
                         + " AND product = '" + row["product"].ToString() + "'"
                         + " AND origin = '" + row["origin"].ToString() + "'"
                         + " AND sizes = '" + row["sizes"].ToString() + "'"
                         + " AND seaover_unit = '" + row["seaover_unit"].ToString() + "'";
                DataRow[] dtRow = hdDt.Select(whr);
                if (dtRow.Length > 0)
                {
                    row["hide_mode"] = dtRow[0]["hide_mode"];
                    row["hide_details"] = dtRow[0]["hide_details"];

                    DateTime tmpDt;
                    if (DateTime.TryParse(dtRow[0]["until_date"].ToString(), out tmpDt))
                    {
                        if (DateTime.Now <= tmpDt)
                        {
                            row["hide_until_date"] = tmpDt.ToString("yyyy-MM-dd");
                        }
                    }
                }
            }
            //업다운표시==============================================================================================
            int up_lv = 0;
            int down_lv = 0;
            SetPriceUpDown(row, avg_day_sales, out up_lv, out down_lv);
            row["up_lv"] = up_lv;
            row["down_lv"] = down_lv;
        }



        private DataTable MergeProduct2(DataTable resultDt)
        {
            if (resultDt.Rows.Count > 0)
            {
                //SEAOVER 선적, 재고내역
                DataTable productDt;
                DataTable productDt2;
                GetCostToPendingCostTable2(out productDt);

                DataRow main_row = null;

                double unit_count = 0;
                double final_average_cost = 0;
                double final_qty = 0;
                double final_pending_cost = 0;
                double final_pending_qty = 0;
                double average_purchase_price = 0;

                Dictionary<int, DataRow> mainRowDic = new Dictionary<int, DataRow>();
                Dictionary<int, ProductTotalCountModel> mergeProductTotalCountDic = new Dictionary<int, ProductTotalCountModel>();

                for (int i = resultDt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = resultDt.Rows[i];

                    double sales_price;
                    if (!double.TryParse(row["sales_price"].ToString(), out sales_price))
                        sales_price = 0;

                    double seaover_cost_price;
                    double seaover_unpending;
                    double seaover_pending;
                    double reserved_stock;
                    double shipment_qty;
                    double unpending_qty_before;
                    double unpending_qty_after;

                    //단위
                    double unit;
                    if (!double.TryParse(row["unit"].ToString(), out unit))
                        unit = 1;

                    //씨오버단위
                    double seaover_unit;
                    if (!double.TryParse(row["seaover_unit"].ToString(), out seaover_unit))
                        seaover_unit = 0;

                    //재고량                        
                    if (!double.TryParse(row["seaover_unpending"].ToString(), out seaover_unpending))
                        seaover_unpending = 0;
                    seaover_unpending = seaover_unpending * seaover_unit;

                    if (!double.TryParse(row["seaover_pending"].ToString(), out seaover_pending))
                        seaover_pending = 0;
                    seaover_pending = seaover_pending * seaover_unit;

                    //예약수
                    if (!double.TryParse(row["reserved_stock"].ToString(), out reserved_stock))
                        reserved_stock = 0;
                    reserved_stock = reserved_stock * seaover_unit;

                    //선적
                    if (!double.TryParse(row["shipment_qty"].ToString(), out shipment_qty))
                        shipment_qty = 0;
                    shipment_qty = shipment_qty * seaover_unit;

                    //배송
                    if (!double.TryParse(row["unpending_qty_before"].ToString(), out unpending_qty_before))
                        unpending_qty_before = 0;
                    unpending_qty_before = unpending_qty_before * seaover_unit;

                    //미통관
                    if (!double.TryParse(row["unpending_qty_after"].ToString(), out unpending_qty_after))
                        unpending_qty_after = 0;
                    unpending_qty_after = unpending_qty_after * seaover_unit;

                    //판매량
                    double sales_cnt;
                    if (!double.TryParse(row["sales_count"].ToString(), out sales_cnt))
                        sales_cnt = 0;
                    sales_cnt = sales_cnt * seaover_unit;

                    //매출제외
                    double excluded_qty;
                    if (!double.TryParse(row["excluded_qty"].ToString(), out excluded_qty))
                        excluded_qty = 0;
                    excluded_qty = excluded_qty * seaover_unit;

                    //메인. 서브품목
                    int sub_id;
                    if (!int.TryParse(row["sub_id"].ToString(), out sub_id))
                        sub_id = 0;
                    int main_id;
                    if (!int.TryParse(row["main_id"].ToString(), out main_id))
                        main_id = 0;

                    double real_stock = 0;
                    double shipment_stock = 0;

                    if (main_id == 320)
                    {
                        main_id = 320;
                    }

                    //선적원가
                    double pending_cost_price;
                    if (!double.TryParse(row["pending_cost_price"].ToString(), out pending_cost_price))
                        pending_cost_price = 0;

                    //서브품목
                    if (main_id > 0 && sub_id < 9999)
                    {
                        real_stock = Convert.ToDouble(row["seaover_unpending"].ToString()) + Convert.ToDouble(row["seaover_pending"].ToString()) - Convert.ToDouble(row["reserved_stock"].ToString());
                        shipment_stock = Convert.ToDouble(row["seaover_unpending"].ToString()) + Convert.ToDouble(row["seaover_pending"].ToString()) - Convert.ToDouble(row["reserved_stock"].ToString());

                        row["real_stock"] = real_stock;
                        row["shipment_stock"] = shipment_stock;
                        row["total_stock"] = real_stock + shipment_stock;

                        //Sum
                        ProductTotalCountModel totalModel;
                        if (!mergeProductTotalCountDic.ContainsKey(main_id))
                        {
                            totalModel = new ProductTotalCountModel();
                            totalModel.main_row_index = -1;
                            totalModel.total_seaover_unpending += seaover_unpending;
                            totalModel.total_seaover_pending += seaover_pending;
                            totalModel.total_reserved_stock += reserved_stock;
                            totalModel.total_shipment_qty += shipment_qty;
                            totalModel.total_unpending_qty_before += unpending_qty_before;
                            totalModel.total_unpending_qty_after += unpending_qty_after;
                            totalModel.total_sales_count += sales_cnt;
                            totalModel.total_excluded_qty += excluded_qty;

                            totalModel.total_pending_cost += pending_cost_price *  (shipment_qty + unpending_qty_before);


                            mergeProductTotalCountDic.Add(main_id, totalModel);
                        }
                        else
                        {
                            totalModel = mergeProductTotalCountDic[main_id];
                            totalModel.total_seaover_unpending += seaover_unpending;
                            totalModel.total_seaover_pending += seaover_pending;
                            totalModel.total_reserved_stock += reserved_stock;
                            totalModel.total_shipment_qty += shipment_qty;
                            totalModel.total_unpending_qty_before += unpending_qty_before;
                            totalModel.total_unpending_qty_after += unpending_qty_after;
                            totalModel.total_sales_count += sales_cnt;
                            totalModel.total_excluded_qty += excluded_qty;

                            totalModel.total_pending_cost += pending_cost_price * (shipment_qty + unpending_qty_before);

                            mergeProductTotalCountDic[main_id] = totalModel;
                        }
                        //메인품목에 다시 적용
                        if (totalModel.main_row_index >= 0)
                        {
                            main_row = resultDt.Rows[totalModel.main_row_index];
                            //씨오버단위
                            double main_seaover_unit;
                            if (!double.TryParse(main_row["seaover_unit"].ToString(), out main_seaover_unit))
                                main_seaover_unit = 1;

                            main_row["seaover_unpending"] = (totalModel.total_seaover_unpending / main_seaover_unit);
                            main_row["seaover_pending"] = (totalModel.total_seaover_pending / main_seaover_unit);
                            main_row["reserved_stock"] = (totalModel.total_reserved_stock / main_seaover_unit);
                            main_row["shipment_qty"] = (totalModel.total_shipment_qty / main_seaover_unit);
                            main_row["unpending_qty_before"] = (totalModel.total_unpending_qty_before / main_seaover_unit);
                            main_row["unpending_qty_after"] = (totalModel.total_unpending_qty_after / main_seaover_unit);
                            main_row["sales_count"] = (totalModel.total_sales_count / main_seaover_unit);
                            main_row["excluded_qty"] = (totalModel.total_excluded_qty / main_seaover_unit);
                            //row.Cells["total_real_stock"].Value = row.Cells["real_stock"].Value;

                            real_stock = 0;
                            real_stock += (totalModel.total_seaover_unpending / main_seaover_unit);
                            real_stock += (totalModel.total_seaover_pending / main_seaover_unit);
                            real_stock -= (totalModel.total_reserved_stock / main_seaover_unit);

                            shipment_stock = 0;
                            shipment_stock += (totalModel.total_shipment_qty / main_seaover_unit);
                            shipment_stock += (totalModel.total_unpending_qty_before / main_seaover_unit);

                            main_row["real_stock"] = real_stock;
                            main_row["shipment_stock"] = shipment_stock;
                            main_row["total_stock"] = real_stock + shipment_stock;
                            main_row["total_real_stock"] = real_stock + shipment_stock;


                            main_row["pending_cost_price"] = totalModel.total_pending_cost / (totalModel.total_shipment_qty + totalModel.total_unpending_qty_before);

                            //판매량, 회전율 등 계산
                            CalculateMonthAround(main_row);
                        }

                        //대표품목 묶지 않은 품목
                        if (sub_id == 0)
                        {
                            if (!mainRowDic.ContainsKey(main_id))
                                mainRowDic.Add(main_id, row);
                            main_row = row;
                        }
                    }
                    //대표품목
                    else if (main_id > 0 && sub_id == 9999)
                    {
                        ProductTotalCountModel totalModel;
                        if (!mergeProductTotalCountDic.ContainsKey(main_id))
                        {
                            totalModel = new ProductTotalCountModel();
                            totalModel.main_row_index = i;
                            /*totalModel.total_seaover_unpending += seaover_unpending;
                            totalModel.total_seaover_pending += seaover_pending;
                            totalModel.total_reserved_stock += reserved_stock;
                            totalModel.total_shipment_qty += shipment_qty;
                            totalModel.total_unpending_qty_before += unpending_qty_before;
                            totalModel.total_unpending_qty_after += unpending_qty_after;
                            totalModel.total_sales_count += sales_cnt;
                            totalModel.total_excluded_qty += excluded_qty;*/

                            mergeProductTotalCountDic.Add(main_id, totalModel);
                        }
                        else
                        {
                            totalModel = mergeProductTotalCountDic[main_id];
                            totalModel.main_row_index = i;
                        }
                        //메인품목 총합변환값 출력
                        row["seaover_unpending"] = (totalModel.total_seaover_unpending / seaover_unit);
                        row["seaover_pending"] = (totalModel.total_seaover_pending / seaover_unit);
                        row["reserved_stock"] = (totalModel.total_reserved_stock / seaover_unit);
                        row["shipment_qty"] = (totalModel.total_shipment_qty / seaover_unit);
                        row["unpending_qty_before"] = (totalModel.total_unpending_qty_before / seaover_unit);
                        row["unpending_qty_after"] = (totalModel.total_unpending_qty_after / seaover_unit);
                        row["sales_count"] = (totalModel.total_sales_count / seaover_unit);
                        row["excluded_qty"] = (totalModel.total_excluded_qty / seaover_unit);
                        //row.Cells["total_real_stock"].Value = row.Cells["real_stock"].Value;

                        //재고현황
                        real_stock += (totalModel.total_seaover_unpending / seaover_unit);
                        real_stock += (totalModel.total_seaover_pending / seaover_unit);
                        real_stock -= (totalModel.total_reserved_stock / seaover_unit);
                        //real_stock -= (totalModel.total_excluded_qty / seaover_unit);

                        shipment_stock += (totalModel.total_shipment_qty / seaover_unit);
                        shipment_stock += (totalModel.total_unpending_qty_before / seaover_unit);

                        //메인품목
                        if (mainRowDic.ContainsKey(main_id))
                            main_row = mainRowDic[main_id];
                        if (main_row != null && row["main_id"].ToString() == main_row["main_id"].ToString())
                        {
                            //일반시세
                            row["average_purchase_price"] = main_row["average_purchase_price"].ToString();
                            //매출단가
                            row["sales_price"] = main_row["sales_price"].ToString();
                            //매입단가
                            row["purchase_price"] = main_row["purchase_price"].ToString();
                            //마진
                            row["margin"] = main_row["margin"].ToString();
                        }

                        row["pending_cost_price"] = totalModel.total_pending_cost / ((totalModel.total_shipment_qty + totalModel.total_unpending_qty_before) / seaover_unit);
                        main_row = null;
                    }
                    //그냥품목
                    else
                    {
                        //재고현황
                        real_stock += Convert.ToDouble(row["seaover_unpending"].ToString()) + Convert.ToDouble(row["seaover_pending"].ToString()) - Convert.ToDouble(row["reserved_stock"].ToString());
                        shipment_stock += Convert.ToDouble(row["shipment_qty"].ToString()) + Convert.ToDouble(row["unpending_qty_before"].ToString());   
                    }

                    row["real_stock"] = real_stock;
                    row["shipment_stock"] = shipment_stock;
                    row["total_stock"] = real_stock + shipment_stock;
                    row["total_real_stock"] = real_stock + shipment_stock;
                    //판매량, 회전율 등 계산
                    CalculateMonthAround(row);
                    //오퍼단가 계산===============================================================================================
                    double box_weight = seaover_unit;
                    double cost_unit = Convert.ToDouble(row["cost_unit"].ToString());

                    //계산방식
                    if (Convert.ToBoolean(row["tray_calculate"].ToString()))
                        unit = cost_unit;
                    else
                        unit = box_weight;
                    //단위수량
                    if (!double.TryParse(row["unit_count"].ToString(), out unit_count))
                        unit_count = 0;
                    if (unit_count == 0)
                        unit_count = 1;
                    double offer_price = Convert.ToDouble(row["offer_price"].ToString());
                    //과세,관세,부대비용,고지가
                    double tax;
                    if (!double.TryParse(row["tax"].ToString(), out tax))
                        tax = 0;
                    tax = tax / 100;
                    double custom;
                    if (!double.TryParse(row["custom"].ToString(), out custom))
                        custom = 0;
                    custom = custom / 100;
                    double incidental_expense;
                    if (!double.TryParse(row["incidental_expense"].ToString(), out incidental_expense))
                        incidental_expense = 0;
                    incidental_expense = incidental_expense / 100;
                    double fixed_tariff;
                    if (!double.TryParse(row["fixed_tariff"].ToString(), out fixed_tariff))
                        fixed_tariff = 0;
                    fixed_tariff /= 1000;
                    //동원
                    double dongwon = 0;
                    if (cbDongwon.Checked)
                        dongwon = 0.035;

                    //원가계산============================================================================
                    //일반 원가계산
                    if (rbCostprice.Checked)
                    {
                        double cost_price;
                        if (fixed_tariff > 0)
                        {
                            cost_price = offer_price * unit * exchange_rate;
                            cost_price += (fixed_tariff * unit * exchange_rate * custom);  //관세
                            cost_price *= (tax + 1);  //과세
                            cost_price *= (incidental_expense + 1);  //부대비용
                            cost_price *= (dongwon + 1);   //동원

                            cost_price /= unit_count;  //단품별단가
                        }
                        else
                        {
                            cost_price = offer_price * unit * exchange_rate;
                            cost_price *= (custom + 1);    //과세
                            cost_price *= (tax + 1);                    //과세
                            cost_price *= (incidental_expense + 1);     //부대비용
                            cost_price *= (dongwon + 1);                //동원
                            if (unit_count > 0)
                                cost_price /= unit_count;             //단품별단가
                        }

                        row["offer_cost_price"] = cost_price;
                    }
                    //TRQ원가계산
                    else if (rbTrq.Checked)
                    {
                        double trq;
                        if (!double.TryParse(txtTrq.Text.Replace(",", ""), out trq))
                            trq = 0;
                        double trq_price;
                        if (offer_price > 0)
                            trq_price = (offer_price * unit * (1 + tax + incidental_expense) * exchange_rate) + (box_weight * trq);
                        else
                            trq_price = 0;
                        row["offer_cost_price"] = trq_price / unit_count;
                    }


                    //오퍼원가 마진율
                    double offer_cost_price;
                    if (!double.TryParse(row["offer_cost_price"].ToString(), out offer_cost_price))
                        offer_cost_price = 0;
                    double margin = (sales_price - offer_cost_price) / sales_price * 100;
                    row["offer_cost_price_margin"] = margin;

                    //=======================================================================================================

                    //SEAOVER원가, 선적원가 평균 출력
                    //대표 이외 품목
                    if (main_id > 0 && sub_id != 9999 && productDt.Rows.Count > 0)
                    {
                        //단위
                        if (!double.TryParse(row["unit"].ToString(), out unit))
                            unit = 1;
                        //씨오버단위
                        double seaover_unit3;
                        if (!double.TryParse(row["seaover_unit"].ToString(), out seaover_unit3))
                            seaover_unit3 = 1;

                        //2023-06-15  선적원가는 이미 최소단위로 맞춰진 금액이고, 판매량은 seaover단위로 낱개개준으로 계산

                        //선적원가
                        if (!double.TryParse(row["pending_cost_price"].ToString(), out pending_cost_price))
                            pending_cost_price = 0;
                        pending_cost_price /= unit;
                        if (!double.TryParse(row["shipment_qty"].ToString(), out shipment_qty))
                            shipment_qty = 0;
                        shipment_qty *= seaover_unit3;
                        if (!double.TryParse(row["unpending_qty_before"].ToString(), out unpending_qty_before))
                            unpending_qty_before = 0;
                        unpending_qty_before *= seaover_unit3;

                        final_pending_cost += (pending_cost_price * (shipment_qty + unpending_qty_before));
                        final_pending_qty += (shipment_qty + unpending_qty_before);
                    }
                    //대표 품목
                    else if (sub_id == 9999)
                    {
                        //단위
                        if (!double.TryParse(row["unit"].ToString(), out unit))
                            unit = 1;
                        //선적원가
                        double main_pending_cost_price = (final_pending_cost / final_pending_qty) * unit;
                        if (double.IsNaN(main_pending_cost_price))
                            main_pending_cost_price = 0;
                        row["pending_cost_price"] = main_pending_cost_price;
                        final_pending_cost = 0;
                        final_pending_qty = 0;
                    }

                    //SEAOVER 원가계산===============================================================================================
                    //병합품목
                    string[] sub_product;
                    if (!string.IsNullOrEmpty(row["merge_product"].ToString()))
                        sub_product = row["merge_product"].ToString().Trim().Split('\n');
                    else
                    {
                        sub_product = new string[1];
                        sub_product[0] = row["product"].ToString()
                                    + "^" + row["origin"].ToString()
                                    + "^" + row["sizes"].ToString()
                                    + "^" + row["unit"].ToString()
                                    + "^" + row["price_unit"].ToString()
                                    + "^" + row["unit_count"].ToString()
                                    + "^" + row["seaover_unit"].ToString();
                    }


                    //팩일땐 단위를 사용
                    string price_unit = row["price_unit"].ToString();
                    //단위수량
                    if (!double.TryParse(row["unit_count"].ToString(), out unit_count))
                        unit_count = 1;
                    //단위
                    if (!double.TryParse(row["unit"].ToString(), out unit))
                        unit = 1;
                    double seaover_unit2;
                    if (!double.TryParse(row["seaover_unit"].ToString(), out seaover_unit2))
                        seaover_unit2 = 1;

                    string tooltip = "";
                    double total_average_cost = 0;
                    double total_qty = 0;
                    for (int j = 0; j < sub_product.Length; j++)
                    {
                        string[] products = sub_product[j].Trim().Split('^');
                        if (products.Length > 3)
                        {
                            string whr = "품명 = '" + products[0] + "'"
                                            + " AND 원산지 = '" + products[1] + "'"
                                            + " AND 규격 = '" + products[2] + "'"
                                            + " AND 단위 = '" + products[6] + "'";
                            DataRow[] dr = productDt.Select(whr);
                            if (dr.Length > 0)
                            {
                                for (int k = 0; k < dr.Length; k++)
                                {
                                    //재고단위
                                    double stock_unit;
                                    if (!double.TryParse(dr[k]["단위"].ToString(), out stock_unit))
                                        stock_unit = 1;
                                    //수량
                                    double qty = Convert.ToDouble(dr[k]["수량"].ToString());
                                    //매출원가
                                    double sales_cost = Convert.ToDouble(dr[k]["매출원가"].ToString());
                                    if(sales_cost == 0)
                                        sales_cost = Convert.ToDouble(dr[k]["매입금액"].ToString());

                                    //계산된 원가
                                    if (bool.TryParse(dr[k]["isPendingCalculate"].ToString(), out bool isPendingCalculate) && isPendingCalculate)
                                    {
                                        //동원시 추가
                                        if (dr[k]["AtoNo"].ToString().Contains("dw") || dr[k]["AtoNo"].ToString().Contains("DW")
                                            || dr[k]["AtoNo"].ToString().Contains("hs") || dr[k]["AtoNo"].ToString().Contains("HS")
                                            || dr[k]["AtoNo"].ToString().Contains("od") || dr[k]["AtoNo"].ToString().Contains("OD")
                                            || dr[k]["AtoNo"].ToString().Contains("ad") || dr[k]["AtoNo"].ToString().Contains("AD"))
                                            sales_cost = sales_cost * 1.025;
                                        else if (dr[k]["AtoNo"].ToString().Contains("jd") || dr[k]["AtoNo"].ToString().Contains("JD"))
                                            sales_cost = sales_cost * 1.02;

                                        //매출단가 * 단위
                                        sales_cost = sales_cost * unit;

                                        //냉장료
                                        double refrigeration_fee = 0;
                                        DateTime in_date;
                                        if (DateTime.TryParse(dr[k]["입고일자"].ToString(), out in_date))
                                        {
                                            TimeSpan ts = DateTime.Now - in_date;
                                            int days = ts.Days;

                                            int r_fee_day;
                                            /*if (stock_unit <= 5)
                                                r_fee_day = 4;
                                            else if (stock_unit <= 10)
                                                r_fee_day = 7;
                                            else if (stock_unit <= 15)
                                                r_fee_day = 12;
                                            else if (stock_unit <= 20)
                                                r_fee_day = 13;
                                            else
                                                r_fee_day = 15;

                                            refrigeration_fee = r_fee_day * days;*/
                                            refrigeration_fee = stock_unit / unit_count * days;
                                        }

                                        //연이자 추가
                                        double interest = 0;
                                        DateTime etd;
                                        if (DateTime.TryParse(dr[k]["etd"].ToString(), out etd))
                                        {
                                            TimeSpan ts = DateTime.Now - etd;
                                            int days = ts.Days;
                                            interest = sales_cost * 0.08 / 365 * days;
                                            if (interest < 0)
                                                interest = 0;
                                        }
                                        //설명
                                        if (qty > 0)
                                        {
                                            if (interest > 0 && refrigeration_fee > 0)
                                                tooltip += "\n AtoNo : " + dr[k]["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + $" + {interest.ToString("N2")}(이자) + {refrigeration_fee.ToString("#,##0")}(냉장료) = {(sales_cost + interest + refrigeration_fee).ToString("#,##0")} " + "  | 수량 :" + qty;
                                            else if (interest == 0 && refrigeration_fee > 0)
                                                tooltip += "\n AtoNo : " + dr[k]["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + $" + {refrigeration_fee.ToString("#,##0")}(냉장료) = {(sales_cost + refrigeration_fee).ToString("#,##0")} " + "  | 수량 :" + qty;
                                            else if (interest > 0 && refrigeration_fee == 0)
                                                tooltip += "\n AtoNo : " + dr[k]["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + $" + {interest.ToString("N2")}(이자) = {(sales_cost + interest).ToString("#,##0")} " + "  | 수량 :" + qty;
                                            else
                                                tooltip += "\n AtoNo : " + dr[k]["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + "  | 수량 :" + qty;

                                            //2024-01-25 매출원가가 0원으로 잡힌것들은 표시를 해야함
                                            if (sales_cost == 0 && qty > 0)
                                                tooltip += "   ***확인필요***";
                                        }
                                        //매출원가+이자+냉장료
                                        sales_cost += interest + refrigeration_fee;
                                    }
                                    //SEAOVER 원가
                                    else
                                    {
                                        //현재 단위로 맞춤
                                        sales_cost = sales_cost / stock_unit * unit;
                                        //설명
                                        tooltip += "\n AtoNo : " + dr[k]["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + "  | 수량 :" + qty;

                                        //2024-01-25 매출원가가 0원으로 잡힌것들은 표시를 해야함
                                        if (sales_cost == 0 && qty > 0)
                                            tooltip += "   ***확인필요***";
                                    }

                                    //대표품목 s원가
                                    total_average_cost += sales_cost * qty;
                                    total_qty += qty;

                                    //대표품목 s원가
                                    final_average_cost += sales_cost * qty;
                                    final_qty += qty;
                                }
                            }
                        }
                    }
                    //S원가
                    if (!double.TryParse(row["sales_cost_price"].ToString(), out seaover_cost_price))
                        seaover_cost_price = 0;
                    seaover_cost_price = (total_average_cost / total_qty);
                    if (double.IsNaN(seaover_cost_price))
                        seaover_cost_price = 0;
                    row["sales_cost_price"] = seaover_cost_price;
                    row["sales_cost_price_tooltip"] = "***** AtoNo 'AD,DW,OD,HS' 경우(JD : 2%) + 매출원가 2.5%, + 연이자 8% *****\n\n" + tooltip.Trim();

                    //선적수
                    if (!double.TryParse(row["shipment_qty"].ToString(), out shipment_qty))
                        shipment_qty = 0;
                    //배송중
                    if (!double.TryParse(row["unpending_qty_before"].ToString(), out unpending_qty_before))
                        unpending_qty_before = 0;
                    //선적원가
                    if (!double.TryParse(row["pending_cost_price"].ToString(), out pending_cost_price))
                        pending_cost_price = 0;

                    //미통관
                    if (!double.TryParse(row["seaover_unpending"].ToString(), out seaover_unpending))
                        seaover_unpending = 0;
                    //통관
                    if (!double.TryParse(row["seaover_pending"].ToString(), out seaover_pending))
                        seaover_pending = 0;
                    //예약
                    if (!double.TryParse(row["reserved_stock"].ToString(), out reserved_stock))
                        reserved_stock = 0;

                    //평균원가1
                    double total_cost_amount = seaover_cost_price * (seaover_unpending + seaover_pending - reserved_stock) + pending_cost_price * (shipment_qty + unpending_qty_before);
                    double total_cost_qty = seaover_unpending + seaover_pending - reserved_stock + shipment_qty + unpending_qty_before;

                    double average_sales_cost_price1 = total_cost_amount / total_cost_qty;
                    if (double.IsNaN(average_sales_cost_price1))
                        average_sales_cost_price1 = 0;
                    row["average_sales_cost_price1"] = average_sales_cost_price1;
                    //평균원가2
                    offer_cost_price = Convert.ToDouble(row["offer_cost_price"].ToString());
                    double offer_order_qty = Convert.ToDouble(row["order_qty"].ToString());
                    total_cost_amount = seaover_cost_price * (seaover_unpending + seaover_pending - reserved_stock) + pending_cost_price * (shipment_qty + unpending_qty_before) + (offer_cost_price * offer_order_qty);
                    total_cost_qty = seaover_unpending + seaover_pending + shipment_qty - reserved_stock + unpending_qty_before + offer_order_qty;
                    row["average_sales_cost_price2"] = total_cost_amount / total_cost_qty;

                    //마진
                    double costPrice;
                    if (!double.TryParse(row["sales_cost_price"].ToString(), out costPrice))
                        costPrice = 0;
                    margin = 0;
                    if (costPrice > 0)
                        margin = ((sales_price - costPrice) / sales_price) * 100;
                    row["margin"] = margin.ToString("#,##0.00") + "%";
                    row["margin_double"] = margin;


                    //평균원가1,2 마진율
                    double average_sales_cost_price2;
                    if (!double.TryParse(row["average_sales_cost_price2"].ToString(), out average_sales_cost_price2))
                        average_sales_cost_price2 = 0;
                    //비교단가
                    double temp_price;
                    if (rbSalesPriceMargin.Checked)
                    {
                        if (!double.TryParse(row["sales_price"].ToString(), out temp_price))
                            temp_price = 0;
                    }
                    else if (rbNormalPriceMargin.Checked)
                    {
                        if (!double.TryParse(row["average_purchase_price"].ToString(), out temp_price))
                            temp_price = 0;
                    }
                    else
                    {
                        if (!double.TryParse(row["purchase_price"].ToString(), out temp_price))
                            temp_price = 0;
                    }
                    double average_sales_cost_price1_margin, average_sales_cost_price2_margin;
                    average_sales_cost_price1_margin = (1 - (average_sales_cost_price1 / temp_price)) * 100;
                    if (double.IsNaN(average_sales_cost_price1_margin))
                        average_sales_cost_price1_margin = 0;
                    average_sales_cost_price2_margin = (1 - (average_sales_cost_price2 / temp_price)) * 100;
                    if (double.IsNaN(average_sales_cost_price2_margin))
                        average_sales_cost_price2_margin = 0;

                    row["average_sales_cost_price1_margin"] = average_sales_cost_price1_margin;
                    row["average_sales_cost_price2_margin"] = average_sales_cost_price2_margin;

                    //정렬데이터
                    DateTime exhausted_date;
                    if (row["exhausted_date"].ToString() == "1년이상 판매가능")
                        exhausted_date = new DateTime(2999, 12, 31);
                    else if (!DateTime.TryParse(row["exhausted_date"].ToString(), out exhausted_date))
                        exhausted_date = new DateTime(2999, 12, 31);

                    row["e_date"] = exhausted_date;

                    DateTime contract_date;
                    if (row["contract_date"].ToString() == "1년이상 판매가능")
                        contract_date = new DateTime(2999, 12, 31);
                    else if (!DateTime.TryParse(row["contract_date"].ToString(), out contract_date))
                        contract_date = new DateTime(2999, 12, 31);

                    row["c_date"] = contract_date;

                    double sizes3;
                    if (!double.TryParse(row["sizes3"].ToString(), out sizes3))
                        sizes3 = 0;

                    row["sizes3_double"] = sizes3;

                }
                resultDt.AcceptChanges();

                //정렬
                DataView dv = new DataView(resultDt);
                string sortTxt;
                switch (cbSortType.Text)
                {
                    case "대분류+품명+원산지+규격":
                        sortTxt = "category, product, origin, sizes3_double, sizes2, sizes";
                        break;
                    case "품명+원산지+규격":
                        sortTxt = "product, origin, sizes3_double, sizes2, sizes";
                        break;
                    case "원산지+품명+규격":
                        sortTxt = "origin, product, sizes3_double, sizes2, sizes";
                        break;
                    case "쇼트일자+품명+원산지+규격":
                        sortTxt = "e_date, product, origin, sizes3_double, sizes2, sizes";
                        break;
                    case "계약일자+품명+원산지+규격":
                        sortTxt = "c_date, product, origin, sizes3_double, sizes2, sizes";
                        break;
                    case "판매가능일+품명+원산지+규격":
                        sortTxt = "enable_sales_days DESC, product, origin, sizes3_double, sizes2, sizes";
                        break;
                    default:
                        sortTxt = "category, product, origin, sizes3_double, sizes2, sizes";
                        break;
                }
                dv.Sort = sortTxt;
                resultDt = dv.ToTable();
                resultDt.AcceptChanges();
            }
            return resultDt;
        }
        private DataTable MergeProduct3(DataTable resultDt)
        {
            if (resultDt.Rows.Count > 0)
            {
                DataRow main_row = null;

                double final_average_cost = 0;
                double final_qty = 0;
                double final_pending_cost = 0;
                double final_pending_qty = 0;

                Dictionary<int, DataRow> mainRowDic = new Dictionary<int, DataRow>();
                Dictionary<int, ProductTotalCountModel> mergeProductTotalCountDic = new Dictionary<int, ProductTotalCountModel>();

                for (int i = resultDt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = resultDt.Rows[i];

                    double sales_price;
                    if (!double.TryParse(row["sales_price"].ToString(), out sales_price))
                        sales_price = 0;

                    double seaover_unpending;
                    double seaover_pending;
                    double reserved_stock;
                    double shipment_qty;
                    double unpending_qty_before;
                    double unpending_qty_after;

                    //단위
                    double unit;
                    if (!double.TryParse(row["unit"].ToString(), out unit))
                        unit = 1;

                    //씨오버단위
                    double seaover_unit;
                    if (!double.TryParse(row["seaover_unit"].ToString(), out seaover_unit))
                        seaover_unit = 0;

                    //재고량                        
                    if (!double.TryParse(row["seaover_unpending"].ToString(), out seaover_unpending))
                        seaover_unpending = 0;
                    seaover_unpending = seaover_unpending * seaover_unit;

                    if (!double.TryParse(row["seaover_pending"].ToString(), out seaover_pending))
                        seaover_pending = 0;
                    seaover_pending = seaover_pending * seaover_unit;

                    //예약수
                    if (!double.TryParse(row["reserved_stock"].ToString(), out reserved_stock))
                        reserved_stock = 0;
                    reserved_stock = reserved_stock * seaover_unit;

                    //선적
                    if (!double.TryParse(row["shipment_qty"].ToString(), out shipment_qty))
                        shipment_qty = 0;
                    shipment_qty = shipment_qty * seaover_unit;

                    //배송
                    if (!double.TryParse(row["unpending_qty_before"].ToString(), out unpending_qty_before))
                        unpending_qty_before = 0;
                    unpending_qty_before = unpending_qty_before * seaover_unit;

                    //미통관
                    if (!double.TryParse(row["unpending_qty_after"].ToString(), out unpending_qty_after))
                        unpending_qty_after = 0;
                    unpending_qty_after = unpending_qty_after * seaover_unit;

                    //판매량
                    double sales_cnt;
                    if (!double.TryParse(row["sales_count"].ToString(), out sales_cnt))
                        sales_cnt = 0;
                    sales_cnt = sales_cnt * seaover_unit;

                    //매출제외
                    double excluded_qty;
                    if (!double.TryParse(row["excluded_qty"].ToString(), out excluded_qty))
                        excluded_qty = 0;
                    excluded_qty = excluded_qty * seaover_unit;

                    //메인. 서브품목
                    int sub_id;
                    if (!int.TryParse(row["sub_id"].ToString(), out sub_id))
                        sub_id = 0;
                    int main_id;
                    if (!int.TryParse(row["main_id"].ToString(), out main_id))
                        main_id = 0;

                    double real_stock = 0;
                    double shipment_stock = 0;

                    //선적원가
                    double pending_cost_price;
                    if (!double.TryParse(row["pending_cost_price"].ToString(), out pending_cost_price))
                        pending_cost_price = 0;

                    //서브품목
                    if (main_id > 0 && sub_id < 9999)
                    {
                        real_stock = Convert.ToDouble(row["seaover_unpending"].ToString()) + Convert.ToDouble(row["seaover_pending"].ToString()) - Convert.ToDouble(row["reserved_stock"].ToString());
                        shipment_stock = Convert.ToDouble(row["seaover_unpending"].ToString()) + Convert.ToDouble(row["seaover_pending"].ToString()) - Convert.ToDouble(row["reserved_stock"].ToString());

                        row["real_stock"] = real_stock;
                        row["shipment_stock"] = shipment_stock;
                        row["total_stock"] = real_stock + shipment_stock;

                        //Sum
                        ProductTotalCountModel totalModel;
                        if (!mergeProductTotalCountDic.ContainsKey(main_id))
                        {
                            totalModel = new ProductTotalCountModel();
                            totalModel.main_row_index = -1;
                            totalModel.total_seaover_unpending += seaover_unpending;
                            totalModel.total_seaover_pending += seaover_pending;
                            totalModel.total_reserved_stock += reserved_stock;
                            totalModel.total_shipment_qty += shipment_qty;
                            totalModel.total_unpending_qty_before += unpending_qty_before;
                            totalModel.total_unpending_qty_after += unpending_qty_after;
                            totalModel.total_sales_count += sales_cnt;
                            totalModel.total_excluded_qty += excluded_qty;

                            mergeProductTotalCountDic.Add(main_id, totalModel);
                        }
                        else
                        {
                            totalModel = mergeProductTotalCountDic[main_id];
                            totalModel.total_seaover_unpending += seaover_unpending;
                            totalModel.total_seaover_pending += seaover_pending;
                            totalModel.total_reserved_stock += reserved_stock;
                            totalModel.total_shipment_qty += shipment_qty;
                            totalModel.total_unpending_qty_before += unpending_qty_before;
                            totalModel.total_unpending_qty_after += unpending_qty_after;
                            totalModel.total_sales_count += sales_cnt;
                            totalModel.total_excluded_qty += excluded_qty;

                            mergeProductTotalCountDic[main_id] = totalModel;
                        }
                        //메인품목에 다시 적용
                        if (totalModel.main_row_index >= 0)
                        {
                            main_row = resultDt.Rows[totalModel.main_row_index];
                            //씨오버단위
                            double main_seaover_unit;
                            if (!double.TryParse(main_row["seaover_unit"].ToString(), out main_seaover_unit))
                                main_seaover_unit = 1;

                            main_row["seaover_unpending"] = (totalModel.total_seaover_unpending / main_seaover_unit);
                            main_row["seaover_pending"] = (totalModel.total_seaover_pending / main_seaover_unit);
                            main_row["reserved_stock"] = (totalModel.total_reserved_stock / main_seaover_unit);
                            main_row["shipment_qty"] = (totalModel.total_shipment_qty / main_seaover_unit);
                            main_row["unpending_qty_before"] = (totalModel.total_unpending_qty_before / main_seaover_unit);
                            main_row["unpending_qty_after"] = (totalModel.total_unpending_qty_after / main_seaover_unit);
                            main_row["sales_count"] = (totalModel.total_sales_count / main_seaover_unit);
                            main_row["excluded_qty"] = (totalModel.total_excluded_qty / main_seaover_unit);
                            //row.Cells["total_real_stock"].Value = row.Cells["real_stock"].Value;

                            real_stock = 0;
                            real_stock += (totalModel.total_seaover_unpending / main_seaover_unit);
                            real_stock += (totalModel.total_seaover_pending / main_seaover_unit);
                            real_stock -= (totalModel.total_reserved_stock / main_seaover_unit);

                            shipment_stock = 0;
                            shipment_stock += (totalModel.total_shipment_qty / main_seaover_unit);
                            shipment_stock += (totalModel.total_unpending_qty_before / main_seaover_unit);

                            main_row["real_stock"] = real_stock;
                            main_row["shipment_stock"] = shipment_stock;
                            main_row["total_stock"] = real_stock + shipment_stock;
                            main_row["total_real_stock"] = real_stock + shipment_stock;

                            //판매량, 회전율 등 계산
                            CalculateMonthAround(main_row);
                        }

                        //대표품목 묶지 않은 품목
                        if (sub_id == 0)
                        {
                            if (!mainRowDic.ContainsKey(main_id))
                                mainRowDic.Add(main_id, row);
                            main_row = row;
                        }
                    }
                    //대표품목
                    else if (main_id > 0 && sub_id == 9999)
                    {
                        ProductTotalCountModel totalModel;
                        if (!mergeProductTotalCountDic.ContainsKey(main_id))
                        {
                            totalModel = new ProductTotalCountModel();
                            totalModel.main_row_index = i;

                            mergeProductTotalCountDic.Add(main_id, totalModel);
                        }
                        else
                        {
                            totalModel = mergeProductTotalCountDic[main_id];
                            totalModel.main_row_index = i;
                        }
                        //메인품목 총합변환값 출력
                        row["seaover_unpending"] = (totalModel.total_seaover_unpending / seaover_unit);
                        row["seaover_pending"] = (totalModel.total_seaover_pending / seaover_unit);
                        row["reserved_stock"] = (totalModel.total_reserved_stock / seaover_unit);
                        row["shipment_qty"] = (totalModel.total_shipment_qty / seaover_unit);
                        row["unpending_qty_before"] = (totalModel.total_unpending_qty_before / seaover_unit);
                        row["unpending_qty_after"] = (totalModel.total_unpending_qty_after / seaover_unit);
                        row["sales_count"] = (totalModel.total_sales_count / seaover_unit);
                        row["excluded_qty"] = (totalModel.total_excluded_qty / seaover_unit);

                        //재고현황
                        real_stock += (totalModel.total_seaover_unpending / seaover_unit);
                        real_stock += (totalModel.total_seaover_pending / seaover_unit);
                        real_stock -= (totalModel.total_reserved_stock / seaover_unit);

                        shipment_stock += (totalModel.total_shipment_qty / seaover_unit);
                        shipment_stock += (totalModel.total_unpending_qty_before / seaover_unit);

                        //메인품목
                        if (mainRowDic.ContainsKey(main_id))
                            main_row = mainRowDic[main_id];
                        if (main_row != null && row["main_id"].ToString() == main_row["main_id"].ToString())
                        {
                            //일반시세
                            row["average_purchase_price"] = main_row["average_purchase_price"].ToString();
                            //매출단가
                            row["sales_price"] = main_row["sales_price"].ToString();
                            //매입단가
                            row["purchase_price"] = main_row["purchase_price"].ToString();
                            //마진
                            row["margin"] = main_row["margin"].ToString();
                        }
                        main_row = null;
                    }
                    //그냥품목
                    else
                    {
                        //재고현황
                        real_stock += Convert.ToDouble(row["seaover_unpending"].ToString()) 
                                    + Convert.ToDouble(row["seaover_pending"].ToString()) 
                                    - Convert.ToDouble(row["reserved_stock"].ToString());
                        shipment_stock += Convert.ToDouble(row["shipment_qty"].ToString()) + Convert.ToDouble(row["unpending_qty_before"].ToString());
                    }

                    row["real_stock"] = real_stock;
                    row["shipment_stock"] = shipment_stock;
                    row["total_stock"] = real_stock + shipment_stock;
                    row["total_real_stock"] = real_stock + shipment_stock;
                    
                    //오퍼단가 계산===============================================================================================
                    double box_weight = seaover_unit;
                    double cost_unit = Convert.ToDouble(row["cost_unit"].ToString());

                    //계산방식
                    if (Convert.ToBoolean(row["tray_calculate"].ToString()))
                        unit = cost_unit;
                    else
                        unit = box_weight;
                    //단위수량
                    double unit_count;
                    if (!double.TryParse(row["unit_count"].ToString(), out unit_count))
                        unit_count = 0;
                    double bundle_count;
                    if (!double.TryParse(row["bundle_count"].ToString(), out bundle_count))
                        bundle_count = 0;
                    if (bundle_count > unit_count)
                        bundle_count = unit_count;
                    if (unit_count == 0)
                        unit_count = 1;
                    double offer_price = Convert.ToDouble(row["offer_price"].ToString());
                    //과세,관세,부대비용,고지가
                    double tax;
                    if (!double.TryParse(row["tax"].ToString(), out tax))
                        tax = 0;
                    tax = tax / 100;
                    double custom;
                    if (!double.TryParse(row["custom"].ToString(), out custom))
                        custom = 0;
                    custom = custom / 100;
                    double incidental_expense;
                    if (!double.TryParse(row["incidental_expense"].ToString(), out incidental_expense))
                        incidental_expense = 0;
                    incidental_expense = incidental_expense / 100;
                    double fixed_tariff;
                    if (!double.TryParse(row["fixed_tariff"].ToString(), out fixed_tariff))
                        fixed_tariff = 0;
                    fixed_tariff /= 1000;
                    //동원
                    double dongwon = 0;
                    if (cbDongwon.Checked)
                        dongwon = 0.035;

                    //원가계산============================================================================
                    //일반 원가계산
                    if (rbCostprice.Checked)
                    {
                        double cost_price;
                        if (fixed_tariff > 0)
                        {
                            cost_price = offer_price * unit * exchange_rate;
                            cost_price += (fixed_tariff * unit * exchange_rate * custom);  //관세
                            cost_price *= (tax + 1);  //과세
                            cost_price *= (incidental_expense + 1);  //부대비용
                            cost_price *= (dongwon + 1);   //동원

                            cost_price /= unit_count;  //단품별단가
                        }
                        else
                        {
                            cost_price = offer_price * unit * exchange_rate;
                            cost_price *= (custom + 1);    //과세
                            cost_price *= (tax + 1);                    //과세
                            cost_price *= (incidental_expense + 1);     //부대비용
                            cost_price *= (dongwon + 1);                //동원
                            if (unit_count > 0)
                                cost_price /= unit_count;             //단품별단가
                        }

                        row["offer_cost_price"] = cost_price;
                    }
                    //TRQ원가계산
                    else if (rbTrq.Checked)
                    {
                        double trq;
                        if (!double.TryParse(txtTrq.Text.Replace(",", ""), out trq))
                            trq = 0;
                        double trq_price;
                        if (offer_price > 0)
                            trq_price = (offer_price * unit * (1 + tax + incidental_expense) * exchange_rate) + (box_weight * trq);
                        else
                            trq_price = 0;
                        row["offer_cost_price"] = trq_price / unit_count;
                    }


                    //오퍼원가 마진율
                    double offer_cost_price;
                    if (!double.TryParse(row["offer_cost_price"].ToString(), out offer_cost_price))
                        offer_cost_price = 0;
                    double margin = (sales_price - offer_cost_price) / sales_price * 100;
                    row["offer_cost_price_margin"] = margin;


                    //판매량, 회전율 등 계산
                    CalculateMonthAround(row);

                    //선적수
                    if (!double.TryParse(row["shipment_qty"].ToString(), out shipment_qty))
                        shipment_qty = 0;
                    //배송중
                    if (!double.TryParse(row["unpending_qty_before"].ToString(), out unpending_qty_before))
                        unpending_qty_before = 0;
                    //선적원가
                    if (!double.TryParse(row["pending_cost_price"].ToString(), out pending_cost_price))
                        pending_cost_price = 0;

                    //미통관
                    if (!double.TryParse(row["seaover_unpending"].ToString(), out seaover_unpending))
                        seaover_unpending = 0;
                    //통관
                    if (!double.TryParse(row["seaover_pending"].ToString(), out seaover_pending))
                        seaover_pending = 0;
                    //예약
                    if (!double.TryParse(row["reserved_stock"].ToString(), out reserved_stock))
                        reserved_stock = 0;
                    //씨오버 원가
                    double seaover_cost_price;
                    if (!double.TryParse(row["sales_cost_price"].ToString(), out seaover_cost_price))
                        seaover_cost_price = 0;

                    /*//평균원가1
                    double total_cost_amount = seaover_cost_price * (seaover_unpending + seaover_pending - reserved_stock) + pending_cost_price * (shipment_qty + unpending_qty_before);
                    double total_cost_qty = seaover_unpending + seaover_pending - reserved_stock + shipment_qty + unpending_qty_before;

                    double average_sales_cost_price1 = total_cost_amount / total_cost_qty;
                    if (double.IsNaN(average_sales_cost_price1))
                        average_sales_cost_price1 = 0;
                    row["average_sales_cost_price1"] = average_sales_cost_price1;
                    //평균원가2
                    offer_cost_price = Convert.ToDouble(row["offer_cost_price"].ToString());
                    double offer_order_qty = Convert.ToDouble(row["order_qty"].ToString());
                    total_cost_amount = seaover_cost_price * (seaover_unpending + seaover_pending - reserved_stock) + pending_cost_price * (shipment_qty + unpending_qty_before) + (offer_cost_price * offer_order_qty);
                    total_cost_qty = seaover_unpending + seaover_pending + shipment_qty - reserved_stock + unpending_qty_before + offer_order_qty;
                    row["average_sales_cost_price2"] = total_cost_amount / total_cost_qty;

                    //마진
                    double costPrice;
                    if (!double.TryParse(row["sales_cost_price"].ToString(), out costPrice))
                        costPrice = 0;
                    margin = 0;
                    if (costPrice > 0)
                        margin = ((sales_price - costPrice) / sales_price) * 100;
                    row["margin"] = margin.ToString("#,##0.00") + "%";
                    row["margin_double"] = margin;


                    //평균원가1,2 마진율
                    double average_sales_cost_price2;
                    if (!double.TryParse(row["average_sales_cost_price2"].ToString(), out average_sales_cost_price2))
                        average_sales_cost_price2 = 0;
                    //비교단가
                    double temp_price;
                    if (rbSalesPriceMargin.Checked)
                    {
                        if (!double.TryParse(row["sales_price"].ToString(), out temp_price))
                            temp_price = 0;
                    }
                    else if (rbNormalPriceMargin.Checked)
                    {
                        if (!double.TryParse(row["average_purchase_price"].ToString(), out temp_price))
                            temp_price = 0;
                    }
                    else
                    {
                        if (!double.TryParse(row["purchase_price"].ToString(), out temp_price))
                            temp_price = 0;
                    }
                    double average_sales_cost_price1_margin, average_sales_cost_price2_margin;
                    average_sales_cost_price1_margin = (1 - (average_sales_cost_price1 / temp_price)) * 100;
                    if (double.IsNaN(average_sales_cost_price1_margin))
                        average_sales_cost_price1_margin = 0;
                    average_sales_cost_price2_margin = (1 - (average_sales_cost_price2 / temp_price)) * 100;
                    if (double.IsNaN(average_sales_cost_price2_margin))
                        average_sales_cost_price2_margin = 0;

                    row["average_sales_cost_price1_margin"] = average_sales_cost_price1_margin;
                    row["average_sales_cost_price2_margin"] = average_sales_cost_price2_margin;*/

                    //정렬데이터
                    DateTime exhausted_date;
                    if (row["exhausted_date"].ToString() == "1년이상 판매가능")
                        exhausted_date = new DateTime(2999, 12, 31);
                    else if (!DateTime.TryParse(row["exhausted_date"].ToString(), out exhausted_date))
                        exhausted_date = new DateTime(2999, 12, 31);

                    row["e_date"] = exhausted_date;

                    DateTime contract_date;
                    if (row["contract_date"].ToString() == "1년이상 판매가능")
                        contract_date = new DateTime(2999, 12, 31);
                    else if (!DateTime.TryParse(row["contract_date"].ToString(), out contract_date))
                        contract_date = new DateTime(2999, 12, 31);

                    row["c_date"] = contract_date;

                    double sizes3;
                    if (!double.TryParse(row["sizes3"].ToString(), out sizes3))
                        sizes3 = 0;

                    row["sizes3_double"] = sizes3;

                }
                resultDt.AcceptChanges();

                //정렬
                DataView dv = new DataView(resultDt);
                string sortTxt;
                switch (cbSortType.Text)
                {
                    case "대분류+품명+원산지+규격":
                        sortTxt = "category, product, origin, sizes3_double, sizes2, sizes";
                        break;
                    case "품명+원산지+규격":
                        sortTxt = "product, origin, sizes3_double, sizes2, sizes";
                        break;
                    case "원산지+품명+규격":
                        sortTxt = "origin, product, sizes3_double, sizes2, sizes";
                        break;
                    case "쇼트일자+품명+원산지+규격":
                        sortTxt = "e_date, product, origin, sizes3_double, sizes2, sizes";
                        break;
                    case "계약일자+품명+원산지+규격":
                        sortTxt = "c_date, product, origin, sizes3_double, sizes2, sizes";
                        break;
                    case "판매가능일+품명+원산지+규격":
                        sortTxt = "enable_sales_days DESC, product, origin, sizes3_double, sizes2, sizes";
                        break;
                    default:
                        sortTxt = "category, product, origin, sizes3_double, sizes2, sizes";
                        break;
                }
                dv.Sort = sortTxt;
                resultDt = dv.ToTable();
                resultDt.AcceptChanges();
            }
            return resultDt;
        }
        private void UpDownColor(DataGridViewRow row)
        {
            row.Cells["down"].Value = "";
            row.Cells["up"].Value = "";
            //Updown
            if (Convert.ToInt16(row.Cells["up_lv"].Value) != 0 || Convert.ToInt16(row.Cells["down_lv"].Value) != 0)
            {
                switch (Convert.ToInt16(row.Cells["up_lv"].Value))
                {
                    case -1:
                        row.Cells["down"].Value = "▼";
                        row.Cells["down"].Style.ForeColor = Color.DarkGreen;

                        break;
                    case -2:
                        row.Cells["down"].Value = "▼";
                        row.Cells["down"].Style.ForeColor = Color.Green;
                        break;
                    case -3:
                        row.Cells["down"].Value = "▼";
                        row.Cells["down"].Style.ForeColor = Color.LightGreen;
                        break;
                    case -4:
                        row.Cells["up"].Value = "▲";
                        row.Cells["up"].Style.ForeColor = Color.Orange;
                        break;
                    case -5:
                        row.Cells["up"].Value = "▲";
                        row.Cells["up"].Style.ForeColor = Color.OrangeRed;
                        break;
                    case -6:
                        row.Cells["up"].Value = "▲";
                        row.Cells["up"].Style.ForeColor = Color.DarkOrange;
                        break;
                    case 1:
                        row.Cells["down"].Value = "▼";
                        row.Cells["down"].Style.ForeColor = Color.DarkBlue;
                        break;
                    case 2:
                        row.Cells["down"].Value = "▼";
                        row.Cells["down"].Style.ForeColor = Color.DeepSkyBlue;
                        break;
                    case 3:
                        row.Cells["down"].Value = "▼";
                        row.Cells["down"].Style.ForeColor = Color.LightBlue;
                        break;
                    case 4:
                        row.Cells["up"].Value = "▲";
                        row.Cells["up"].Style.ForeColor = Color.Pink;
                        break;
                    case 5:
                        row.Cells["up"].Value = "▲";
                        row.Cells["up"].Style.ForeColor = Color.Red;
                        break;
                    case 6:
                        row.Cells["up"].Value = "▲";
                        row.Cells["up"].Style.ForeColor = Color.DarkRed;
                        break;
                    default:
                        row.Cells["up"].Value = "";
                        row.Cells["up"].Style.ForeColor = Color.Black;
                        break;
                }
            }
            if (Convert.ToInt16(row.Cells["up_lv"].Value) == 0)
                row.Cells["up"].Value = "";
            if (Convert.ToInt16(row.Cells["down_lv"].Value) == 0)
                row.Cells["down"].Value = "";
            //추천계약일
            DateTime endDate = dtpEnddate.Value;
            DateTime contractDate;
            if (DateTime.TryParse(row.Cells["contract_date"].Value.ToString(), out contractDate))
            {
                //이미 지난 날짜
                if (contractDate <= endDate)
                    row.Cells["contract_date"].Style.ForeColor = Color.Red;
                //한달이내
                else if (contractDate <= endDate.AddMonths(1))
                    row.Cells["contract_date"].Style.ForeColor = Color.Red;
                //두달이내
                else if (contractDate <= endDate.AddMonths(2))
                    row.Cells["contract_date"].Style.ForeColor = Color.DarkRed;
                //세달이내
                else if (contractDate <= endDate.AddMonths(2))
                    row.Cells["contract_date"].Style.ForeColor = Color.HotPink;
            }
            //회전율
            double around;
            if (double.TryParse(row.Cells["month_around"].Value.ToString(), out around))
            {
                if (around == 0)
                    row.Cells["month_around"].Style.ForeColor = Color.Black;
            }
        }
        #endregion

        #region 즐겨찾기 품목
        public void MoveProductDelete(List<GroupModel> list)
        {
            if (list.Count > 0 && dgvProduct.Rows.Count > 0)
            {
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];

                    for (int j = 0; j < list.Count; j++)
                    {
                        if (row.Cells["product"].Value.ToString() == list[j].product
                            && row.Cells["origin"].Value.ToString() == list[j].origin
                            && row.Cells["sizes"].Value.ToString() == list[j].sizes
                            && row.Cells["unit"].Value.ToString() == list[j].unit
                            && row.Cells["price_unit"].Value.ToString() == list[j].price_unit
                            && row.Cells["unit_count"].Value.ToString() == list[j].unit_count
                            && row.Cells["seaover_unit"].Value.ToString() == list[j].seaover_unit
                            && row.Cells["division"].Value.ToString() == list[j].division)
                        {
                            dgvProduct.Rows.Remove(row);
                            break;
                        }
                    }
                }
            }
        }
        public void SetGroup(int id, string group_name)
        {
            isBookmarkMode = true;
            this.Text = "업체별시세현황(" + group_name + ")";
            txtGroupName.Text = group_name;
            txtGroupName.Enabled = true;
            group_id = id;
            lbGroupId.Text = group_id.ToString();
            GetData();
            btnBookmark.Text = "즐겨찾기 해제(F8)";
            btnBookmark.ForeColor = Color.Red;
        }
        #endregion

        #region 계산수식
        private void dgvProduct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                DataGridViewColumn col = dgvProduct.Columns[e.ColumnIndex];

                if (dgvProduct.Columns[e.ColumnIndex].Name == "real_stock"
                   || dgvProduct.Columns[e.ColumnIndex].Name == "shipment_stock")
                {
                    double real_stock;
                    if (!double.TryParse(row.Cells["real_stock"].Value.ToString(), out real_stock))
                        real_stock = 0;
                    double shipment_stock;
                    if (!double.TryParse(row.Cells["shipment_stock"].Value.ToString(), out shipment_stock))
                        shipment_stock = 0;

                    //오퍼수량
                    double order_qty;
                    if (!double.TryParse(row.Cells["order_qty"].Value.ToString(), out order_qty))
                        order_qty = 0;

                    //회전율
                    double sales_count = Convert.ToDouble(row.Cells["average_day_sales_count_double"].Value.ToString());
                    double enable_sales_day = Math.Ceiling((real_stock + order_qty) / sales_count);
                    double month_around = enable_sales_day / 21;
                    row.Cells["enable_sales_days"].Value = enable_sales_day;
                    row.Cells["month_around"].Value = month_around;

                    //회전율(선적포함)
                    real_stock += shipment_stock;
                    enable_sales_day = Math.Ceiling((real_stock + order_qty) / sales_count);
                    month_around = enable_sales_day / 21;
                    row.Cells["month_around_in_shipment"].Value = month_around;

                    //소진일자
                    if (dgvProduct.Columns[e.ColumnIndex].Name == "real_stock")
                    {
                        row.Cells["seaover_unpending"].Value = 0;
                        row.Cells["seaover_pending"].Value = real_stock;
                        SetData(row, order_qty, 0, real_stock);
                    }
                    else
                    {
                        row.Cells["shipment_qty"].Value = shipment_stock;
                        row.Cells["unpending_qty_before"].Value = 0;
                        row.Cells["unpending_qty_after"].Value = 0;
                        SetData(row, order_qty, 0, 0, shipment_stock);
                    }

                    CalculateAverageCostUnit(row);
                    if (cbAllTerm.Checked)
                        GetAllQtySumaryTerm();

                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "seaover_unpending"
                   || dgvProduct.Columns[e.ColumnIndex].Name == "seaover_pending"
                   || dgvProduct.Columns[e.ColumnIndex].Name == "shipment_qty"
                   || dgvProduct.Columns[e.ColumnIndex].Name == "unpending_qty_before"
                   || dgvProduct.Columns[e.ColumnIndex].Name == "unpending_qty_after")
                {
                    //재고수
                    double seaover_unpending;
                    if (!double.TryParse(row.Cells["seaover_unpending"].Value.ToString(), out seaover_unpending))
                        seaover_unpending = 0;
                    double seaover_pending;
                    if (!double.TryParse(row.Cells["seaover_pending"].Value.ToString(), out seaover_pending))
                        seaover_pending = 0;
                    double reserved_stock;
                    if (!double.TryParse(row.Cells["reserved_stock"].Value.ToString(), out reserved_stock))
                        reserved_stock = 0;


                    //통관대기품목
                    double shipment_qty;
                    if (!double.TryParse(row.Cells["shipment_qty"].Value.ToString(), out shipment_qty))
                        shipment_qty = 0;
                    double unpending_qty_before;
                    if (!double.TryParse(row.Cells["unpending_qty_before"].Value.ToString(), out unpending_qty_before))
                        unpending_qty_before = 0;

                    //실재고
                    double real_stock = seaover_unpending + seaover_pending - reserved_stock;
                    row.Cells["real_stock"].Value = real_stock;
                    //선적재고
                    double shipment_stock = shipment_qty + unpending_qty_before;
                    row.Cells["shipment_stock"].Value = shipment_stock;
                    //총재고
                    row.Cells["total_stock"].Value = real_stock + shipment_stock;

                    //오퍼수량
                    double order_qty;
                    if (!double.TryParse(row.Cells["order_qty"].Value.ToString(), out order_qty))
                        order_qty = 0;

                    //회전율
                    real_stock = seaover_unpending + seaover_pending - reserved_stock;
                    double sales_count = Convert.ToDouble(row.Cells["average_day_sales_count_double"].Value.ToString());
                    double enable_sales_day = Math.Ceiling((real_stock + order_qty) / sales_count);
                    double month_around = enable_sales_day / 21;
                    row.Cells["enable_sales_days"].Value = enable_sales_day;
                    row.Cells["month_around"].Value = month_around;

                    //회전율(선적포함)
                    real_stock += shipment_qty;
                    real_stock += unpending_qty_before;
                    enable_sales_day = Math.Ceiling((real_stock + order_qty) / sales_count);
                    month_around = enable_sales_day / 21;
                    row.Cells["month_around_in_shipment"].Value = month_around;

                    //소진일자
                    SetData(row, order_qty);

                    CalculateAverageCostUnit(row);
                    if (cbAllTerm.Checked)
                        GetAllQtySumaryTerm();
                }
                else if (col.Name == "average_day_sales_count")
                {
                    double real_stock = Convert.ToDouble(row.Cells["seaover_unpending"].Value.ToString()) + Convert.ToDouble(row.Cells["seaover_pending"].Value.ToString()) - Convert.ToDouble(row.Cells["reserved_stock"].Value.ToString());
                    row.Cells["average_day_sales_count_double"].Value = row.Cells["average_day_sales_count"].Value;

                    //오퍼원가
                    double order_qty;
                    if (!double.TryParse(row.Cells["order_qty"].Value.ToString(), out order_qty))
                        order_qty = 0;

                    //회전율
                    double sales_count = Convert.ToDouble(row.Cells["average_day_sales_count_double"].Value.ToString());
                    double enable_sales_day = Math.Ceiling((real_stock + order_qty) / sales_count);
                    double month_around = enable_sales_day / 21;
                    row.Cells["enable_sales_days"].Value = enable_sales_day;
                    row.Cells["month_around"].Value = month_around;


                    //회전율(선적포함)
                    real_stock += Convert.ToDouble(row.Cells["shipment_qty"].Value.ToString()) + Convert.ToDouble(row.Cells["unpending_qty_before"].Value.ToString());
                    enable_sales_day = Math.Ceiling((real_stock + order_qty) / sales_count);
                    month_around = enable_sales_day / 21;
                    row.Cells["month_around_in_shipment"].Value = month_around;

                    //평균원가
                    double sales_cost_price;
                    if (!double.TryParse(row.Cells["sales_cost_price"].Value.ToString(), out sales_cost_price))
                        sales_cost_price = 0;
                    //오퍼원가
                    double offer_cost_price;
                    if (!double.TryParse(row.Cells["offer_cost_price"].Value.ToString(), out offer_cost_price))
                        offer_cost_price = 0;

                    //평균원가
                    double total_cost_price = 0;
                    double total_stock = real_stock + order_qty;
                    if (real_stock > 0 && sales_cost_price > 0)
                        total_cost_price += sales_cost_price * real_stock;
                    if (order_qty != 0 && offer_cost_price > 0)
                        total_cost_price += offer_cost_price * order_qty;
                    row.Cells["total_real_stock"].Value = total_stock;
                    row.Cells["average_sales_cost_price2"].Value = total_cost_price / total_stock;

                    //소진일자
                    SetData(row, order_qty);

                    //Up down
                    int up_lv, down_lv;
                    double avg_day_sales = Convert.ToDouble(row.Cells["average_day_sales_count"].Value.ToString());
                    SetPriceUpDown(row, avg_day_sales, out up_lv, out down_lv);
                    row.Cells["up_lv"].Value = up_lv;
                    row.Cells["down_lv"].Value = down_lv;
                    UpDownColor(row);
                }
                else if (col.Name == "average_day_sales_count_double")
                {
                    double real_stock = Convert.ToDouble(row.Cells["seaover_unpending"].Value.ToString()) + Convert.ToDouble(row.Cells["seaover_pending"].Value.ToString()) - Convert.ToDouble(row.Cells["reserved_stock"].Value.ToString());
                    //오퍼원가
                    double order_qty;
                    if (!double.TryParse(row.Cells["order_qty"].Value.ToString(), out order_qty))
                        order_qty = 0;

                    //회전율
                    double sales_count = Convert.ToDouble(row.Cells["average_day_sales_count_double"].Value.ToString());
                    double enable_sales_day = Math.Ceiling((real_stock + order_qty) / sales_count);
                    double month_around = enable_sales_day / 21;
                    row.Cells["enable_sales_days"].Value = enable_sales_day;
                    row.Cells["month_around"].Value = month_around;

                    //회전율(선적포함)
                    real_stock += Convert.ToDouble(row.Cells["shipment_qty"].Value.ToString()) + Convert.ToDouble(row.Cells["unpending_qty_before"].Value.ToString());
                    enable_sales_day = Math.Ceiling((real_stock + order_qty) / sales_count);
                    month_around = enable_sales_day / 21;
                    row.Cells["month_around_in_shipment"].Value = month_around;

                    //평균원가
                    double sales_cost_price;
                    if (!double.TryParse(row.Cells["sales_cost_price"].Value.ToString(), out sales_cost_price))
                        sales_cost_price = 0;
                    //오퍼원가
                    double offer_cost_price;
                    if (!double.TryParse(row.Cells["offer_cost_price"].Value.ToString(), out offer_cost_price))
                        offer_cost_price = 0;

                    //평균원가
                    double total_cost_price = 0;
                    double total_stock = real_stock + order_qty;
                    if (real_stock > 0 && sales_cost_price > 0)
                        total_cost_price += sales_cost_price * real_stock;
                    if (order_qty != 0 && offer_cost_price > 0)
                        total_cost_price += offer_cost_price * order_qty;
                    row.Cells["total_real_stock"].Value = total_stock;
                    row.Cells["average_sales_cost_price2"].Value = total_cost_price / total_stock;

                    //소진일자
                    SetData(row, order_qty);

                    //Up down
                    int up_lv, down_lv;
                    double avg_day_sales = Convert.ToDouble(row.Cells["average_day_sales_count"].Value.ToString());
                    SetPriceUpDown(row, avg_day_sales, out up_lv, out down_lv);
                    row.Cells["up_lv"].Value = up_lv;
                    row.Cells["down_lv"].Value = down_lv;
                    UpDownColor(row);
                }
                else if (col.Name == "average_month_sales_count")
                {
                    double real_stock = Convert.ToDouble(row.Cells["seaover_unpending"].Value.ToString()) + Convert.ToDouble(row.Cells["seaover_pending"].Value.ToString()) - Convert.ToDouble(row.Cells["reserved_stock"].Value.ToString());
                    double average_month_sales_count;
                    if (!double.TryParse(row.Cells["average_month_sales_count"].Value.ToString(), out average_month_sales_count))
                        average_month_sales_count = 0;

                    double average_day_sales_count = average_month_sales_count / 21;

                    row.Cells["average_day_sales_count"].Value = (int)average_day_sales_count;
                    row.Cells["average_day_sales_count_double"].Value = average_day_sales_count;

                    //오퍼원가
                    double order_qty;
                    if (!double.TryParse(row.Cells["order_qty"].Value.ToString(), out order_qty))
                        order_qty = 0;

                    //회전율
                    double sales_count = Convert.ToDouble(row.Cells["average_day_sales_count_double"].Value.ToString());
                    double enable_sales_day = Math.Ceiling((real_stock + order_qty) / sales_count);
                    double month_around = enable_sales_day / 21;
                    row.Cells["enable_sales_days"].Value = enable_sales_day;
                    row.Cells["month_around"].Value = month_around;

                    //회전율(선적포함)
                    real_stock += Convert.ToDouble(row.Cells["shipment_qty"].Value.ToString()) + Convert.ToDouble(row.Cells["unpending_qty_before"].Value.ToString());
                    enable_sales_day = Math.Ceiling((real_stock + order_qty) / sales_count);
                    month_around = enable_sales_day / 21;
                    row.Cells["month_around_in_shipment"].Value = month_around;

                    //소진일자
                    SetData(row, order_qty, average_day_sales_count);

                    //Up down
                    int up_lv, down_lv;
                    double avg_day_sales = Convert.ToDouble(row.Cells["average_day_sales_count_double"].Value.ToString());
                    SetPriceUpDown(row, avg_day_sales, out up_lv, out down_lv);
                    row.Cells["up_lv"].Value = up_lv;
                    row.Cells["down_lv"].Value = down_lv;
                    UpDownColor(row);
                }
                //오퍼원가 계산
                else if (col.Name == "offer_price"
                    || col.Name == "tax"
                    || col.Name == "custom"
                    || col.Name == "incidental_expense"
                    || col.Name == "fixed_tariff")
                {
                    CalculateOfferCostUnit(row);
                    CalculateAverageCostUnit(row);
                    //마진율
                    double sales_price;
                    //매출단가
                    if (rbSalesPriceMargin.Checked)
                    {
                        if (row.Cells["sales_price"].Value == null || !double.TryParse(row.Cells["sales_price"].Value.ToString(), out sales_price))
                            sales_price = 0;
                    }
                    else if (rbNormalPriceMargin.Checked)
                    {
                        if (row.Cells["average_purchase_price"].Value == null || !double.TryParse(row.Cells["average_purchase_price"].Value.ToString(), out sales_price))
                            sales_price = 0;
                    }
                    else
                    {
                        if (row.Cells["purchase_price"].Value == null || !double.TryParse(row.Cells["purchase_price"].Value.ToString(), out sales_price))
                            sales_price = 0;
                    }
                    //평균원가
                    double sales_cost_price;
                    if (dgvProduct.Rows[e.RowIndex].Cells["average_sales_cost_price2"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["average_sales_cost_price2"].Value.ToString(), out sales_cost_price))
                        sales_cost_price = 0;
                    //평균원가2 마진
                    double margin_rate = Math.Round((sales_price - sales_cost_price) / sales_price * 100, 2);
                    dgvProduct.Rows[e.RowIndex].Cells["average_sales_cost_price2_margin"].Value = margin_rate;
                }
                //오더수량
                else if (col.Name == "order_qty")
                {
                    double cost_unit;
                    if (!double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["cost_unit"].Value.ToString(), out cost_unit))
                        cost_unit = 0;
                    double order_qty;
                    if (!double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["order_qty"].Value.ToString(), out order_qty))
                        order_qty = 0;
                    /*if (cost_unit > 0 && order_qty > 0)
                    {
                        
                        if (msgBox .Show(this, "트레이 품목입니다. 입력값을 중량값으로 변경하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            order_qty = order_qty / cost_unit;
                            dgvProduct.Rows[e.RowIndex].Cells["order_qty"].Value = order_qty;
                        }
                    }*/
                    //회전율
                    double real_stock = Convert.ToDouble(row.Cells["seaover_unpending"].Value.ToString()) + Convert.ToDouble(row.Cells["seaover_pending"].Value.ToString()) - Convert.ToDouble(row.Cells["reserved_stock"].Value.ToString());
                    double sales_count = Convert.ToDouble(row.Cells["average_day_sales_count_double"].Value.ToString());
                    double enable_sales_day = (real_stock) / sales_count;
                    double month_around = enable_sales_day / 21;
                    if (double.IsInfinity(month_around))
                        month_around = 9999;
                    row.Cells["enable_sales_days"].Value = enable_sales_day;
                    row.Cells["month_around"].Value = month_around;

                    //선적포함 회전율
                    double seaover_unpending;
                    if (row.Cells["seaover_unpending"].Value == null || !double.TryParse(row.Cells["seaover_unpending"].Value.ToString(), out seaover_unpending))
                        seaover_unpending = 0;
                    double seaover_pending;
                    if (row.Cells["seaover_pending"].Value == null || !double.TryParse(row.Cells["seaover_pending"].Value.ToString(), out seaover_pending))
                        seaover_pending = 0;
                    double unpending_qty_before;
                    if (row.Cells["unpending_qty_before"].Value == null || !double.TryParse(row.Cells["unpending_qty_before"].Value.ToString(), out unpending_qty_before))
                        unpending_qty_before = 0;
                    double shipment_qty;
                    if (row.Cells["shipment_qty"].Value == null || !double.TryParse(row.Cells["shipment_qty"].Value.ToString(), out shipment_qty))
                        shipment_qty = 0;
                    double reserved_stock;
                    if (row.Cells["reserved_stock"].Value == null || !double.TryParse(row.Cells["reserved_stock"].Value.ToString(), out reserved_stock))
                        reserved_stock = 0;
                    real_stock = seaover_unpending + seaover_pending + unpending_qty_before + shipment_qty - reserved_stock;

                    enable_sales_day = (real_stock + order_qty) / sales_count;
                    month_around = enable_sales_day / 21;
                    if (double.IsInfinity(month_around))
                        month_around = 9999;
                    //row.Cells["enable_sales_days"].Value = enable_sales_day;
                    row.Cells["month_around_in_shipment"].Value = month_around;

                    CalculateAverageCostUnit(row);
                    SetData(row, order_qty);
                    //GetAllQtySumaryTerm();
                    if (cbAllTerm.Checked)
                        GetAllQtySumaryTerm();
                    //총 오더수량
                    double total_order_qty = 0;
                    for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    {
                        if (dgvProduct.Rows[i].Cells["order_qty"].Value == null | !double.TryParse(dgvProduct.Rows[i].Cells["order_qty"].Value.ToString(), out order_qty))
                            order_qty = 0;
                        total_order_qty += order_qty;
                    }
                    txtTotalOrderQty.Text = total_order_qty.ToString("#,##0");
                    CalculateAverageSalesCostPriceMargin(row);
                }
                //오더 선적일
                else if (col.Name == "order_etd")
                {
                    CalculateAverageCostUnit(row);
                    double order_qty;
                    if (!double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["order_qty"].Value.ToString(), out order_qty))
                        order_qty = 0;
                    SetData(row, order_qty);
                    //GetAllQtySumaryTerm();
                    if (cbAllTerm.Checked)
                        GetAllQtySumaryTerm();
                    //총 오더수량
                    double total_order_qty = 0;
                    for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    {
                        if (dgvProduct.Rows[i].Cells["order_qty"].Value == null | !double.TryParse(dgvProduct.Rows[i].Cells["order_qty"].Value.ToString(), out order_qty))
                            order_qty = 0;
                        total_order_qty += order_qty;
                    }
                    txtTotalOrderQty.Text = total_order_qty.ToString("#,##0");
                    CalculateAverageSalesCostPriceMargin(row);
                }
                //오더수량
                else if (col.Name == "sales_price" || col.Name == "sales_cost_price" || col.Name == "average_purcahse_price" || col.Name == "average_purchase_price" || col.Name == "purchase_price")
                {
                    //마진율
                    double sales_price;
                    if (rbSalesPriceMargin.Checked)
                    {
                        if (row.Cells["sales_price"].Value == null || !double.TryParse(row.Cells["sales_price"].Value.ToString(), out sales_price))
                            sales_price = 0;
                    }
                    else if (rbNormalPriceMargin.Checked)
                    {
                        if (row.Cells["average_purchase_price"].Value == null || !double.TryParse(row.Cells["average_purchase_price"].Value.ToString(), out sales_price))
                            sales_price = 0;
                    }
                    else
                    {
                        if (row.Cells["purchase_price"].Value == null || !double.TryParse(row.Cells["purchase_price"].Value.ToString(), out sales_price))
                            sales_price = 0;
                    }

                    double sales_cost_price;
                    if (dgvProduct.Rows[e.RowIndex].Cells["sales_cost_price"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["sales_cost_price"].Value.ToString(), out sales_cost_price))
                        sales_cost_price = 0;

                    //마진율
                    double margin_rate = Math.Round((sales_price - sales_cost_price) / sales_price * 100, 2);
                    dgvProduct.Rows[e.RowIndex].Cells["margin"].Value = margin_rate.ToString("#,##0.0.0") + "%";

                    double average_sales_cost_price1;
                    if (dgvProduct.Rows[e.RowIndex].Cells["average_sales_cost_price1"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["average_sales_cost_price1"].Value.ToString(), out average_sales_cost_price1))
                        average_sales_cost_price1 = 0;

                    //마진율1
                    margin_rate = Math.Round((sales_price - average_sales_cost_price1) / sales_price * 100, 2);
                    dgvProduct.Rows[e.RowIndex].Cells["average_sales_cost_price1_margin"].Value = margin_rate;

                    //마진율2
                    double sales_cost_price2;
                    if (dgvProduct.Rows[e.RowIndex].Cells["average_sales_cost_price2"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["average_sales_cost_price2"].Value.ToString(), out sales_cost_price2))
                        sales_cost_price2 = 0;

                    margin_rate = Math.Round((sales_price - sales_cost_price) / sales_price * 100, 2);
                    dgvProduct.Rows[e.RowIndex].Cells["average_sales_cost_price2_margin"].Value = margin_rate;

                    //오퍼원가 마진율
                    double offer_price;
                    if (row.Cells["offer_cost_price"].Value == null || !double.TryParse(row.Cells["offer_cost_price"].Value.ToString(), out offer_price))
                        offer_price = 0;
                    double margin = (sales_price - offer_price) / sales_price * 100;
                    row.Cells["offer_cost_price_margin"].Value = margin;

                    //Up down
                    int up_lv, down_lv;
                    double avg_day_sales = Convert.ToDouble(row.Cells["average_day_sales_count_double"].Value.ToString());
                    SetPriceUpDown(row, avg_day_sales, out up_lv, out down_lv);
                    row.Cells["up_lv"].Value = up_lv;
                    row.Cells["down_lv"].Value = down_lv;
                    UpDownColor(row);
                    //SPO 계산
                    CalculateCostPriceMarginAmount(isSelectCalculate);
                }
                else if (col.Name == "average_sales_cost_price2")
                {
                    //마진율
                    double sales_price;
                    if (dgvProduct.Rows[e.RowIndex].Cells["sales_price"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["sales_price"].Value.ToString(), out sales_price))
                        sales_price = 0;
                    double sales_cost_price;
                    if (dgvProduct.Rows[e.RowIndex].Cells["average_sales_cost_price2"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["average_sales_cost_price2"].Value.ToString(), out sales_cost_price))
                        sales_cost_price = 0;

                    double margin_rate = Math.Round((sales_price - sales_cost_price) / sales_price * 100, 2);
                    dgvProduct.Rows[e.RowIndex].Cells["average_sales_cost_price2_margin"].Value = margin_rate;
                }
                //실재고 <== 수정X
                else if (col.Name == "real_stock")
                {

                    cbReservationDetails.Checked = true;


                    double seaover_unpending;
                    if (dgvProduct.Rows[e.RowIndex].Cells["seaover_unpending"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["seaover_unpending"].Value.ToString(), out seaover_unpending))
                        seaover_unpending = 0;
                    double seaover_pending;
                    if (dgvProduct.Rows[e.RowIndex].Cells["seaover_pending"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["seaover_pending"].Value.ToString(), out seaover_pending))
                        seaover_pending = 0;
                    double unpending_qty_before;
                    if (dgvProduct.Rows[e.RowIndex].Cells["unpending_qty_before"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["unpending_qty_before"].Value.ToString(), out unpending_qty_before))
                        unpending_qty_before = 0;
                    double shipment_qty;
                    if (dgvProduct.Rows[e.RowIndex].Cells["shipment_qty"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["shipment_qty"].Value.ToString(), out shipment_qty))
                        shipment_qty = 0;
                    double reserved_stock;
                    if (dgvProduct.Rows[e.RowIndex].Cells["reserved_stock"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["reserved_stock"].Value.ToString(), out reserved_stock))
                        reserved_stock = 0;

                    double real_stock = seaover_unpending + seaover_pending - reserved_stock;
                    double shipment_stock = shipment_qty + unpending_qty_before;

                    dgvProduct.Rows[e.RowIndex].Cells["real_stock"].Value = real_stock;
                    dgvProduct.Rows[e.RowIndex].Cells["shipment_stock"].Value = shipment_stock;
                    dgvProduct.Rows[e.RowIndex].Cells["total_stock"].Value = real_stock + shipment_stock;
                    //msgBox .Show(this,"선적/배송/미통관/통관 재고수량을 수정해주세요!");

                }
                //기준회전율 변경
                else if (col.Name == "base_around_month")
                {
                    int up_lv, down_lv;
                    double avg_sales_day;
                    if (row.Cells["average_day_sales_count_double"].Value == null || !double.TryParse(row.Cells["average_day_sales_count_double"].Value.ToString(), out avg_sales_day))
                        avg_sales_day = 0;
                    SetPriceUpDown(row, avg_sales_day, out up_lv, out down_lv);
                    row.Cells["up_lv"].Value = up_lv;
                    row.Cells["down_lv"].Value = down_lv;
                    dgvProduct.EndEdit();
                    for (int i = 0; i < dgvProduct.RowCount; i++)
                    {
                        UpDownColor(dgvProduct.Rows[i]);
                    }
                }
                //기간별 회전율 수정시 오더수량에 자동입력
                else if (col.Name == "month_around_1"
                    || col.Name == "month_around_45"
                    || col.Name == "month_around_2"
                    || col.Name == "month_around_3"
                    || col.Name == "month_around_6"
                    || col.Name == "month_around_12"
                    || col.Name == "month_around_18")
                {

                    if (dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Contains('('))
                    {
                        string[] month_around_split = dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Split('(');
                        if (month_around_split.Length > 0)
                        {
                            double month_around;
                            if (!double.TryParse(month_around_split[0].Trim(), out month_around))
                                month_around = 0;

                            double month_sale_qty;
                            if (!double.TryParse(month_around_split[1].Trim().Replace(")", ""), out month_sale_qty))
                                month_sale_qty = 0;

                            double real_stock;
                            if (dgvProduct.Rows[e.RowIndex].Cells["real_stock"].Value == null
                                || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["real_stock"].Value.ToString(), out real_stock))
                                real_stock = 0;

                            double pre_month_around = real_stock / month_sale_qty;

                            double needed_qty = (month_around - pre_month_around) * month_sale_qty;

                            dgvProduct.Rows[e.RowIndex].Cells["order_qty"].Value = needed_qty.ToString();
                        }
                    }
                }
                else if (col.Name == "average_sales_cost_price1" || col.Name == "average_sales_cost_price2")
                {
                    CalculateAverageSalesCostPriceMargin(row);
                }

                this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            }
        }



        private void CalculateAverageCostUnit(DataGridViewRow row)
        {
            //씨오버원가
            double sales_cost_price;
            if (!double.TryParse(row.Cells["sales_cost_price"].Value.ToString(), out sales_cost_price))
                sales_cost_price = 0;
            //선적원가
            double shipment_cost_price;
            if (!double.TryParse(row.Cells["pending_cost_price"].Value.ToString(), out shipment_cost_price))
                shipment_cost_price = 0;
            //오퍼원가
            double offer_cost_price;
            if (!double.TryParse(row.Cells["offer_cost_price"].Value.ToString(), out offer_cost_price))
                offer_cost_price = 0;


            //씨오버재고
            double seaover_unpending;
            if (!double.TryParse(row.Cells["seaover_unpending"].Value.ToString(), out seaover_unpending))
                seaover_unpending = 0;
            double seaover_pending;
            if (!double.TryParse(row.Cells["seaover_pending"].Value.ToString(), out seaover_pending))
                seaover_pending = 0;
            double reserved_stock;
            if (!double.TryParse(row.Cells["reserved_stock"].Value.ToString(), out reserved_stock))
                reserved_stock = 0;
            double real_stock = seaover_unpending + seaover_pending - reserved_stock;

            //선적재고
            double shipment_qty;
            if (!double.TryParse(row.Cells["shipment_qty"].Value.ToString(), out shipment_qty))
                shipment_qty = 0;
            double unpending_qty_before;
            if (!double.TryParse(row.Cells["unpending_qty_before"].Value.ToString(), out unpending_qty_before))
                unpending_qty_before = 0;
            double shipment_stock = shipment_qty + unpending_qty_before;

            //오퍼원가
            double order_qty;
            if (!double.TryParse(row.Cells["order_qty"].Value.ToString(), out order_qty))
                order_qty = 0;

            //평균원가
            double total_cost_price = 0;
            double total_stock = 0;
            if (real_stock > 0 && sales_cost_price > 0)
            {
                total_cost_price += sales_cost_price * real_stock;
                total_stock += real_stock;
            }
            if (shipment_stock > 0 && shipment_cost_price > 0)
            {
                total_cost_price += shipment_cost_price * shipment_stock;
                total_stock += shipment_stock;
            }
            //평균원가1
            double average_sales_cost_price1 = total_cost_price / total_stock;
            if (double.IsNaN(average_sales_cost_price1))
                average_sales_cost_price1 = 0;
            row.Cells["average_sales_cost_price1"].Value = average_sales_cost_price1;
            //평균원가2
            row.Cells["total_real_stock"].Value = order_qty + real_stock + shipment_stock;
            if (order_qty > 0 && offer_cost_price > 0)
            {
                total_cost_price += offer_cost_price * order_qty;
                total_stock += order_qty;
            }
            double average_sales_cost_price2 = total_cost_price / total_stock;
            if (double.IsNaN(average_sales_cost_price2))
                average_sales_cost_price2 = 0;
            row.Cells["average_sales_cost_price2"].Value = average_sales_cost_price2;
        }

        private void CalculateOfferCostUnit(DataGridViewRow row)
        {
            double box_weight;
            if (!double.TryParse(row.Cells["seaover_unit"].Value.ToString(), out box_weight))
                box_weight = 0;
            double cost_unit;
            if (!double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                cost_unit = 0;

            double unit;
            //계산방식
            if (Convert.ToBoolean(row.Cells["tray_calculate"].Value))
                unit = cost_unit;
            else
                unit = box_weight;

            double offer_price;
            if (!double.TryParse(row.Cells["offer_price"].Value.ToString(), out offer_price))
                offer_price = 0;
            double tax;
            if (!double.TryParse(row.Cells["tax"].Value.ToString(), out tax))
                tax = 0;
            tax = tax / 100;
            double custom;
            if (!double.TryParse(row.Cells["custom"].Value.ToString(), out custom))
                custom = 0;
            custom = custom / 100;
            double incidental_expense;
            if (!double.TryParse(row.Cells["incidental_expense"].Value.ToString(), out incidental_expense))
                incidental_expense = 0;
            incidental_expense = incidental_expense / 100;

            double fixed_tariff;
            if (!double.TryParse(row.Cells["fixed_tariff"].Value.ToString(), out fixed_tariff))
                fixed_tariff = 0;
            fixed_tariff /= 1000;

            double dongwon = 0;
            if (cbDongwon.Checked)
                dongwon = 0.0025;


            double unit_count = Convert.ToDouble(row.Cells["unit_count"].Value.ToString());
            //원가계산
            double cost_price;

            if (fixed_tariff > 0)
            {
                cost_price = unit * offer_price * exchange_rate;
                cost_price += fixed_tariff * unit * exchange_rate * custom;

                cost_price *= (tax + 1);
                cost_price *= (incidental_expense + 1);
                cost_price *= (dongwon + 1);
            }
            else
            {
                cost_price = unit * offer_price * exchange_rate;
                cost_price *= (custom + 1);

                cost_price *= (tax + 1);
                cost_price *= (incidental_expense + 1);
                cost_price *= (dongwon + 1);
            }
            if (unit_count > 0)
                cost_price /= unit_count;
            //원가계산
            row.Cells["offer_cost_price"].Value = cost_price;
            //TRQ원가계산
            if (rbTrq.Checked)
            {
                double trq = Convert.ToDouble(txtTrq.Text.Replace(",", ""));
                double trq_price;
                if (offer_price > 0)
                    trq_price = (offer_price * unit * (1 + tax + incidental_expense) * exchange_rate) + (box_weight * trq);
                else
                    trq_price = 0;
                if (unit_count > 0)
                    trq_price /= unit_count;
                row.Cells["offer_cost_price"].Value = trq_price;
                cost_price = trq_price;
            }

            //오퍼원가 마진
            double sales_price;
            if (rbSalesPriceMargin.Checked)
            {
                if (row.Cells["sales_price"].Value == null || !double.TryParse(row.Cells["sales_price"].Value.ToString(), out sales_price))
                    sales_price = 0;
            }
            else if (rbNormalPriceMargin.Checked)
            {
                if (row.Cells["average_purchase_price"].Value == null || !double.TryParse(row.Cells["average_purchase_price"].Value.ToString(), out sales_price))
                    sales_price = 0;
            }
            else
            {
                if (row.Cells["purchase_price"].Value == null || !double.TryParse(row.Cells["purchase_price"].Value.ToString(), out sales_price))
                    sales_price = 0;
            }

            double margin = (sales_price - cost_price) / sales_price * 100;
            row.Cells["offer_cost_price_margin"].Value = margin;
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

                




                if (dgvProduct.Rows.Count > 0)
                {
                    //int row = newDt.Rows.Count + 1;
                    int row = 1;
                    foreach (DataGridViewRow dr in dgvProduct.Rows)
                    {
                        if (dr.Visible)
                            row++;
                    }
                    int column = col_name.Count;
                    object[,] data = new object[row, column];

                    //Data
                    Excel.Range rng = workSheet.get_Range("A1", "CG" + row + 2);
                    object[,] only_data = (object[,])rng.get_Value();

                    data = only_data;

                    //Header
                    for (int i = 0; i < col_name.Count; i++)
                    {
                        //wk.Cells[1, i + 1].value = dgvProduct.Columns[col_name[i]].HeaderText;

                        data[1, i + 1] = dgvProduct.Columns[col_name[i]].HeaderText;
                    }
                    this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                    
                    for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    {
                        if (dgvProduct.Rows[i].Visible)
                        {
                            for (int j = 0; j < col_name.Count; j++)
                            {
                                /*if(col_name[j] == "sizes")
                                    wk.Cells[i + 2, j + 1].value = "'" + newDt.Rows[i][col_name[j]].ToString();
                                else
                                    wk.Cells[i + 2, j + 1].value = newDt.Rows[i][col_name[j]].ToString();*/

                                if (dgvProduct.Rows[i].Cells[col_name[j]].Value == null)
                                    dgvProduct.Rows[i].Cells[col_name[j]].Value = "";

                                if (col_name[j] == "sizes")
                                    only_data[rowIndex, j + 1] = "'" + dgvProduct.Rows[i].Cells[col_name[j]].Value.ToString();
                                else if (j == 25)
                                    only_data[rowIndex, j + 1] = dgvProduct.Rows[i].Cells[col_name[j]].Value.ToString();
                                else
                                    only_data[rowIndex, j + 1] = dgvProduct.Rows[i].Cells[col_name[j]].Value.ToString();
                            }
                            rowIndex++;
                        }
                    }
                    this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);

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

                rg1 = wk.Range[wk.Cells[2, 1], wk.Cells[dgvProduct.Rows.Count + 1, col_cnt]];
                rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                //속도개선 ON
                setAutomatic(excelApp, true);
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                msgBox.Show(this, ex.Message.ToString() + "\n 생성 중 에러가 발생하였습니다.");
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

        #region Closing event
        private void PriceComparison_FormClosing(object sender, FormClosingEventArgs e)
        {
            StyleSettingTxt();
            cookie.SaveSalesManagerSetting(styleDic);
        }
        #endregion
    }
}






