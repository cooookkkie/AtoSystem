using AdoNetWindow.Common.PrintManager;
using AdoNetWindow.Model;
using AdoNetWindow.Pending;
using AdoNetWindow.Product;
using AdoNetWindow.SEAOVER;
using AdoNetWindow.SEAOVER.PriceComparison;
using Microsoft.Office.Interop.Excel;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Repositories;
using Repositories.Config;
using Repositories.ContractRecommendation;
using Repositories.Domestic;
using Repositories.Group;
using Repositories.SaleProduct;
using Repositories.SEAOVER;
using Repositories.SEAOVER.Purchase;
using Repositories.SEAOVER.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;
using Font = System.Drawing.Font;
using Point = System.Drawing.Point;

namespace AdoNetWindow.DashboardForSales
{
    public partial class DetailDashBoardForSales : Form
    {
        ICommonRepository commonRepository = new CommonRepository();
        ISalesRepository salesRepository = new SalesRepository();   
        IGroupRepository groupRepository = new GroupRepository();
        IPurchasePriceRepository purchasePriceRepository = new PurchasePriceRepository();
        IPurchaseRepository purchaseRepository = new PurchaseRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        IPriceComparisonRepository priceComparisonRepository = new PriceComparisonRepository();
        IProductOtherCostRepository productOtherCostRepository = new ProductOtherCostRepository();
        IContractRecommendationRepository contractRecommendationRepository = new ContractRecommendationRepository();
        IProductGroupRepository productGroupRepository = new ProductGroupRepository();
        IProductExcludedSalesRepository productExcludedSalesRepository = new ProductExcludedSalesRepository();
        IDomesticRepository domesticRepository = new DomesticRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        DataTable purchaseDt = new DataTable();
        UsersModel um;
        double seaover_average_cost_price;
        double total_average_cost_price;

        //캡쳐방지
        [System.Runtime.InteropServices.DllImport("user32.dll")]

        private static extern uint SetWindowDisplayAffinity(IntPtr windowHandle, uint affinity);
        private const uint WDA_NONE = 0;
        private const uint WDA_MONITOR = 1;

        //최근 매출량 기간
        string currentSaleTerms;
        public DetailDashBoardForSales(UsersModel uModel, int sales_terms = 45)
        {
            InitializeComponent();
            um = uModel;
            switch (sales_terms)
            {
                case 1:
                    cbSaleTerm.Text = "1개월";
                    break;
                case 45:
                    cbSaleTerm.Text = "45일";
                    break;
                case 2:
                    cbSaleTerm.Text = "2개월";
                    break;
                case 3:
                    cbSaleTerm.Text = "3개월";
                    break;
                case 6:
                    cbSaleTerm.Text = "6개월";
                    break;
                case 12:
                    cbSaleTerm.Text = "12개월";
                    break;
                case 18:
                    cbSaleTerm.Text = "18개월";
                    break;
            }
            currentSaleTerms = cbSaleTerm.Text;

            //날짜
            txtSttdate.Text = DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtOfferSttdate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            txtOfferEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            nudUntilDays.Value = DateTime.Now.Day;

            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "품명별 매출관리 대시보드", "is_print"))
                {
                    btnPrinting.Visible = false;
                }
            }
        }
        public DetailDashBoardForSales(UsersModel uModel, List<string[]> product, int sales_terms = 45)  
        {
            InitializeComponent();
            um = uModel;
            //환율가져오기
            double customRate = common.GetExchangeRateKEBBank("USD");
            txtExchangeRate.Text = customRate.ToString("#,##0.00");
            txtExchangeRate2.Text = customRate.ToString("#,##0.00");
            //날짜
            txtSttdate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtOfferSttdate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            txtOfferEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            nudUntilDays.Value = DateTime.Now.Day;
            InputClipboardData(product);

            switch (sales_terms)
            {
                case 1:
                    cbSaleTerm.Text = "1개월";
                    break;
                case 45:
                    cbSaleTerm.Text = "45일";
                    break;
                case 2:
                    cbSaleTerm.Text = "2개월";
                    break;
                case 3:
                    cbSaleTerm.Text = "3개월";
                    break;
                case 6:
                    cbSaleTerm.Text = "6개월";
                    break;
                case 12:
                    cbSaleTerm.Text = "12개월";
                    break;
                case 18:
                    cbSaleTerm.Text = "18개월";
                    break;
            }
            currentSaleTerms = cbSaleTerm.Text;
        }
        public void FirstIndexClick()
        {
            if (dgvProduct.Rows.Count > 0)
                dgvProduct.Rows[0].Cells["chk"].Value = true;
        }

        private void DetailDashBoardForSales_Load(object sender, EventArgs e)
        {
            this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            //프로시져 최신화
            CallProductProcedure();
            CallStockProcedure();
            //환율가져오기
            double customRate = common.GetExchangeRateKEBBank("USD");
            txtExchangeRate.Text = customRate.ToString("#,##0.00");
            txtExchangeRate2.Text = customRate.ToString("#,##0.00");

            //Get Trq
            double trq = commonRepository.GetTrq();
            txtTrq.Text = trq.ToString("#,##0");

            //날짜
            txtSttdate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtOfferSttdate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            txtOfferEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            nudUntilDays.Value = DateTime.Now.Day;

            dgvMarket.Columns["seaover_sales_price"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);
            dgvMarket.Columns["seaover_sales_price"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
            dgvMarket.Columns["seaover_purchase_price"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            dgvMarket.Columns["seaover_purchase_price"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);

            //품명헤더 단위
            dgvProduct.Columns["unit"].HeaderCell.Style.Font = new Font("나눔고딕", 7, FontStyle.Regular);
            dgvProduct.Columns["price_unit"].HeaderCell.Style.Font = new Font("나눔고딕", 7, FontStyle.Regular);
            dgvProduct.Columns["unit_count"].HeaderCell.Style.Font = new Font("나눔고딕", 7, FontStyle.Regular);
            dgvProduct.Columns["seaover_unit"].HeaderCell.Style.Font = new Font("나눔고딕", 7, FontStyle.Regular);

            dgvProduct.Columns["division"].HeaderCell.Style.Font = new Font("나눔고딕", 7, FontStyle.Regular);
            dgvProduct.Columns["manager1"].HeaderCell.Style.Font = new Font("나눔고딕", 7, FontStyle.Regular);
            dgvProduct.Columns["manager2"].HeaderCell.Style.Font = new Font("나눔고딕", 7, FontStyle.Regular);


            //정렬기능 막기
            foreach (DataGridViewColumn dgvc in dgvDetails.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn dgvc in dgvContract.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn dgvc in dgvDetails.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn dgvc in dgvDetails2.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);

            //캡쳐방지
            //if(um.auth_level < 90)
            //   SetWindowDisplayAffinity(this.Handle, WDA_MONITOR);
            //권한별 설정
            if (um.department.Contains("영업"))
                txtDivision.Text = "10 20 30";
        }

        #region 품목검색 Method
        private void SetDashboardSummary()
        {
            //매출단가
            double sales_price;
            if (!double.TryParse(txtSalesPrice.Text, out sales_price))
                sales_price = 0;

            double seaover_cost_price;
            if (dgvStockSales.Rows[0].Cells["seaover_cost_price"].Value == null || !double.TryParse(dgvStockSales.Rows[0].Cells["seaover_cost_price"].Value.ToString(), out seaover_cost_price))
                seaover_cost_price = 0;

            double pending_cost_price;
            if (dgvStockSales.Rows[0].Cells["pending_cost_price"].Value == null || !double.TryParse(dgvStockSales.Rows[0].Cells["pending_cost_price"].Value.ToString(), out pending_cost_price))
                pending_cost_price = 0;

            double total_cost_price;
            if (dgvStockSales.Rows[0].Cells["total_cost_price"].Value == null || !double.TryParse(dgvStockSales.Rows[0].Cells["total_cost_price"].Value.ToString(), out total_cost_price))
                total_cost_price = 0;

            txtSalesPriceSeaoverMargin.Text = ((sales_price - seaover_cost_price) / sales_price * 100).ToString("#,##0.00");
            txtSalesPricePendingMargin.Text = ((sales_price - pending_cost_price) / sales_price * 100).ToString("#,##0.00");
            txtSalesPriceAverageCostPriceMargin.Text = ((sales_price - total_cost_price) / sales_price * 100).ToString("#,##0.00");
            //최저단가
            double purchase_price;
            if (!double.TryParse(txtPurchasePrice.Text, out purchase_price))
                purchase_price = 0;
            txtPurchasePriceSeaoverMargin.Text = ((purchase_price - seaover_cost_price) / purchase_price * 100).ToString("#,##0.00");
            txtPurchasePricePendingMargin.Text = ((purchase_price - pending_cost_price) / purchase_price * 100).ToString("#,##0.00");
            txtPurchasePriceAverageCostPriceMargin.Text = ((purchase_price - total_cost_price) / purchase_price * 100).ToString("#,##0.00");
            //일반시세
            double normal_price;
            if (!double.TryParse(txtNormalPrice.Text, out normal_price))
                normal_price = 0;
            txtNormalPriceSeaoverMargin.Text = ((normal_price - seaover_cost_price) / normal_price * 100).ToString("#,##0.00");
            txtNormalPricePendingMargin.Text = ((normal_price - pending_cost_price) / normal_price * 100).ToString("#,##0.00");
            txtNormalPriceAverageCostPriceMargin.Text = ((normal_price - total_cost_price) / normal_price * 100).ToString("#,##0.00");

            this.Update();
        }
        private void GetDashboard(DataGridViewRow row)
        {
            this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
                dgvProduct.Rows[i].Cells["chk"].Value = false;

            row.Cells["chk"].Value = true;

            seaover_average_cost_price = 0;
            total_average_cost_price = 0;
            txtInQty.Text = "0";
            GetProductInfo(row);
            OpenExhaustedManager(row);
            GetMarketPriceByCompany(row);
            GetPurchase(row);
            GetSales(row);
            GetSales2(row);
            GetPurchasePriceList(row);
            GetOffer(row);
            SetDashboardSummary();
            this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }
        private void CallProductProcedure()
        {
            //업체별시세현황 스토어프로시져 호출
            try
            {
                string sDate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
                string eDate = DateTime.Now.ToString("yyyy-MM-dd");
                string user_id = um.seaover_id;
                ////업체별시세현황 스토어프로시져 호출
                if (seaoverRepository.CallStoredProcedure(user_id, sDate, eDate) == 0)
                {
                    MessageBox.Show(this,"호출 내용이 없음");
                    this.Activate();
                    return;
                }
                    
            }
            catch (Exception e)
            {
                MessageBox.Show(this,e.Message);
                this.Activate();
            }

        }
        private void CallStockProcedure()
        {
            //품명별재고현황 스토어프로시져 호출
            try
            {
                string sDate = DateTime.Now.ToString("yyyy-MM-dd");
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
                return;
            }
        }
        private void GetProduct(DataTable products = null)
        {
            if (txtProduct.Text.Trim() == string.Empty
                && txtOrigin.Text.Trim() == string.Empty
                && txtSizes.Text.Trim() == string.Empty
                && txtUnit.Text.Trim() == string.Empty
                && txtManager1.Text.Trim() == string.Empty
                && txtManager2.Text.Trim() == string.Empty
                && txtDivision.Text.Trim() == string.Empty)
            {
                MessageBox.Show(this, "검색항목이 전부 없을 경우 검색할 수 없습니다.");
                this.Activate();
                return;
            }
            priceComparisonRepository.SetSeaoverId(um.seaover_id);
            CallProductProcedure();

            this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);

            dgvProduct.Rows.Clear();
            DataTable productDt;
            productDt = priceComparisonRepository.GetPriceComparisonDataTable(DateTime.Now, 3, "", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtManager1.Text, txtManager2.Text, txtDivision.Text
                                                                , DateTime.Now.AddYears(-10), DateTime.Now, false, false, 0, 0, false, 0, 6, 1, products);
            if (productDt.Rows.Count > 0)
            {
                //대표품목
                DataTable pgDt = productGroupRepository.GetProductGroup(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
                //담당자1, 담당자2 전용 품목 DATA
                DataTable mngDt = seaoverRepository.GetProductTapble("", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtManager1.Text, txtManager2.Text, txtDivision.Text);
                //품목 추가정보
                DataTable tmDt = productOtherCostRepository.GetProductInfoAsOne(txtProduct.Text, txtOrigin.Text, txtSizes.Text, "", "");

                //머지품목 출력여부
                List<string> mergeList = new List<string>();
                string whr;
                DataRow[] dr;
                double unit;
                int n;
                DataGridViewRow row;
                for (int i = 0; i < productDt.Rows.Count; i++)
                {
                    bool isOutput = true;
                    //품목병합======================================================================================= 
                    string merge_product = "";
                    string main_id = "";
                    if (pgDt.Rows.Count > 0)
                    {
                        //1.서브품목으로 등록되있는지 확인
                        whr = "item_product = '" + productDt.Rows[i]["품명"].ToString().Trim() + "'"
                            + " AND item_origin = '" + productDt.Rows[i]["원산지"].ToString().Trim() + "'"
                            + " AND item_sizes = '" + productDt.Rows[i]["규격"].ToString().Trim() + "'"
                            + " AND item_unit = '" + productDt.Rows[i]["단위"].ToString().Trim() + "'"
                            + " AND item_price_unit = '" + productDt.Rows[i]["가격단위"].ToString().Trim() + "'"
                            + " AND item_unit_count = '" + productDt.Rows[i]["단위수량"].ToString().Trim() + "'"
                            + " AND item_seaover_unit = '" + productDt.Rows[i]["SEAOVER단위"].ToString().Trim() + "'";

                        DataRow[] dtRow = null;
                        dtRow = pgDt.Select(whr);
                        if (dtRow.Length > 0)
                        {
                            //3.병합품목의 서브 품목을 검색
                            whr = "product = '" + dtRow[0]["product"].ToString() + "'"
                            + " AND origin = '" + dtRow[0]["origin"].ToString() + "'"
                            + " AND sizes = '" + dtRow[0]["sizes"].ToString() + "'"
                            + " AND unit = '" + dtRow[0]["unit"].ToString() + "'"
                            + " AND price_unit = '" + dtRow[0]["price_unit"].ToString() + "'"
                            + " AND unit_count = '" + dtRow[0]["unit_count"].ToString() + "'"
                            + " AND seaover_unit = '" + dtRow[0]["seaover_unit"].ToString() + "'";
                            dtRow = pgDt.Select(whr);
                            if (dtRow.Length > 0)
                            {
                                main_id = dtRow[0]["main_id"].ToString();
                                for (int j = 0; j < dtRow.Length; j++)
                                {
                                    //병합된 품목에 추가
                                    merge_product += "\n" + dtRow[j]["item_product"].ToString()
                                                + "^" + dtRow[j]["item_origin"].ToString()
                                                + "^" + dtRow[j]["item_sizes"].ToString()
                                                + "^" + dtRow[j]["item_unit"].ToString()
                                                + "^" + dtRow[j]["item_price_unit"].ToString()
                                                + "^" + dtRow[j]["item_unit_count"].ToString()
                                                + "^" + dtRow[j]["item_seaover_unit"].ToString();
                                }
                            }
                        }
                            /*if (dtRow.Length > 0)
                        {
                            //isOutput = false;
                            string main_code = dtRow[0]["product"].ToString()
                                        + "^" + dtRow[0]["origin"].ToString()
                                        + "^" + dtRow[0]["sizes"].ToString()
                                        + "^" + dtRow[0]["unit"].ToString()
                                        + "^" + dtRow[0]["price_unit"].ToString()
                                        + "^" + dtRow[0]["unit_count"].ToString()
                                        + "^" + dtRow[0]["seaover_unit"].ToString();
                            //2.메인품목이 출력됬는지 확인
                            if (!mergeList.Contains(main_code))
                            {
                                int avg_sale_price_cnt = 0;
                                double avg_sale_price = 0;
                                int avg_normal_price_cnt = 0;
                                double avg_normal_price = 0;
                                //3.병합품목의 서브 품목을 검색
                                whr = "product = '" + dtRow[0]["product"].ToString() + "'"
                                + " AND origin = '" + dtRow[0]["origin"].ToString() + "'"
                                + " AND sizes = '" + dtRow[0]["sizes"].ToString() + "'"
                                + " AND unit = '" + dtRow[0]["unit"].ToString() + "'"
                                + " AND price_unit = '" + dtRow[0]["price_unit"].ToString() + "'"
                                + " AND unit_count = '" + dtRow[0]["unit_count"].ToString() + "'"
                                + " AND seaover_unit = '" + dtRow[0]["seaover_unit"].ToString() + "'";
                                dtRow = pgDt.Select(whr);
                                if (dtRow.Length > 0)
                                {   
                                    for (int j = 0; j < dtRow.Length; j++)
                                    {
                                        whr = "품명 = '" + dtRow[j]["item_product"].ToString() + "'"
                                            + " AND 원산지 = '" + dtRow[j]["item_origin"].ToString() + "'"
                                            + " AND 규격 = '" + dtRow[j]["item_sizes"].ToString() + "'"
                                            + " AND 단위 = '" + dtRow[j]["item_unit"].ToString() + "'"
                                            + " AND 가격단위 = '" + dtRow[j]["item_price_unit"].ToString() + "'"
                                            + " AND 단위수량 = '" + dtRow[j]["item_unit_count"].ToString() + "'"
                                            + " AND SEAOVER단위 = '" + dtRow[j]["item_seaover_unit"].ToString() + "'";
                                        DataRow[] subProductRow = productDt.Select(whr);
                                        if (subProductRow.Length > 0)
                                        {
                                            if (!double.TryParse(subProductRow[0]["단위"].ToString(), out unit))
                                                unit = 1;
                                            //매출단가
                                            double sale_price;
                                            if (!double.TryParse(subProductRow[0]["매출단가"].ToString(), out sale_price))
                                                sale_price = 0;
                                            if (sale_price > 0)
                                            {
                                                avg_sale_price += sale_price / unit;
                                                avg_sale_price_cnt++;
                                            }
                                            //일반시세
                                            double normal_price;
                                            if (!double.TryParse(subProductRow[0]["일반시세"].ToString(), out normal_price))
                                                normal_price = 0;
                                            if (normal_price > 0)
                                            {
                                                avg_normal_price += normal_price / unit;
                                                avg_normal_price_cnt++;
                                            }

                                            //병합된 품목에 추가
                                            merge_product += "\n" + dtRow[j]["item_product"].ToString()
                                                        + "^" + dtRow[j]["item_origin"].ToString()
                                                        + "^" + dtRow[j]["item_sizes"].ToString()
                                                        + "^" + dtRow[j]["item_unit"].ToString()
                                                        + "^" + dtRow[j]["item_price_unit"].ToString()
                                                        + "^" + dtRow[j]["item_unit_count"].ToString()
                                                        + "^" + dtRow[j]["item_seaover_unit"].ToString();
                                        }
                                    }
                                }
                                //4.매출단가, 시세 평균내서  메인품목만 출력
                                n = dgvProduct.Rows.Add();
                                row = dgvProduct.Rows[n];

                                row.Cells["category"].Value = productDt.Rows[i]["대분류1"].ToString();
                                row.Cells["category_code"].Value = productDt.Rows[i]["대분류"].ToString();
                                row.Cells["product_code"].Value = productDt.Rows[i]["품명코드"].ToString();
                                row.Cells["origin_code"].Value = productDt.Rows[i]["원산지코드"].ToString();
                                row.Cells["sizes_code"].Value = productDt.Rows[i]["규격코드"].ToString();
                                row.Cells["product"].Value = dtRow[0]["product"].ToString();
                                row.Cells["origin"].Value = dtRow[0]["origin"].ToString();
                                row.Cells["sizes"].Value = dtRow[0]["sizes"].ToString();
                                row.Cells["unit"].Value = dtRow[0]["unit"].ToString().Trim();
                                if (!double.TryParse(dtRow[0]["unit"].ToString(), out unit))
                                    unit = 1;
                                row.Cells["price_unit"].Value = dtRow[0]["price_unit"].ToString();
                                row.Cells["unit_count"].Value = dtRow[0]["unit_count"].ToString();
                                row.Cells["bundle_count"].Value = productDt.Rows[i]["묶음수"].ToString();
                                row.Cells["seaover_unit"].Value = dtRow[0]["seaover_unit"].ToString();
                                *//*row.Cells["manager1"].Value = productDt.Rows[i]["담당자1"].ToString();
                                row.Cells["manager2"].Value = productDt.Rows[i]["담당자2"].ToString();*//*
                                row.Cells["division"].Value = productDt.Rows[i]["구분"].ToString();
                                double seaover_current_sales_price = avg_sale_price / avg_sale_price_cnt * unit;
                                if (double.IsNaN(seaover_current_sales_price))
                                    seaover_current_sales_price = 0;
                                row.Cells["seaover_current_sales_price"].Value = seaover_current_sales_price.ToString("#,##0");
                                row.Cells["seaover_current_purchase_price"].Value = Convert.ToDouble(productDt.Rows[i]["최저단가"].ToString()).ToString("#,##0");
                                double seaover_current_normal_price = avg_normal_price / avg_normal_price_cnt * unit;
                                if (double.IsNaN(seaover_current_normal_price))
                                    seaover_current_normal_price = 0;
                                row.Cells["seaover_current_normal_price"].Value = seaover_current_normal_price.ToString("#,##0");
                                //담당자12
                                row.Cells["manager1"].Value = "";
                                row.Cells["manager2"].Value = "";

                                if (mngDt.Rows.Count > 0)
                                {
                                    whr = "품명 = '" + row.Cells["product"].Value.ToString() + "'"
                                    + " AND 원산지 = '" + row.Cells["origin"].Value.ToString() + "'"
                                    + " AND 규격 = '" + row.Cells["sizes"].Value.ToString() + "'"
                                    + " AND 단위 = '" + row.Cells["unit"].Value.ToString() + "'"
                                    + " AND 가격단위 = '" + row.Cells["price_unit"].Value.ToString() + "'"
                                    + " AND 단위수량 = " + row.Cells["unit_count"].Value.ToString()
                                    + " AND SEAOVER단위 = '" + row.Cells["seaover_unit"].Value.ToString() + "'";
                                    dr = mngDt.Select(whr);
                                    if (dr.Length > 0)
                                    {
                                        row.Cells["manager1"].Value = dr[0]["담당자1"].ToString();
                                        row.Cells["manager2"].Value = dr[0]["담당자2"].ToString();
                                    }
                                }
                                //품목 추가정보
                                if (tmDt.Rows.Count > 0)
                                {
                                    whr = "product = '" + row.Cells["product"].Value.ToString() + "'"
                                    + " AND origin = '" + row.Cells["origin"].Value.ToString() + "'"
                                    + " AND sizes = '" + row.Cells["sizes"].Value.ToString() + "'"
                                    + " AND (unit = '" + row.Cells["unit"].Value.ToString() + "'"
                                    + " OR unit = '" + row.Cells["seaover_unit"].Value.ToString() + "')";
                                    dr = tmDt.Select(whr);
                                    if (dr.Length > 0)
                                    {
                                        row.Cells["tax"].Value = dr[0]["tax"].ToString();
                                        row.Cells["custom"].Value = dr[0]["custom"].ToString();
                                        row.Cells["incidental_expense"].Value = dr[0]["incidental_expense"].ToString();
                                        row.Cells["production_days"].Value = dr[0]["production_days"].ToString();
                                        row.Cells["purchase_margin"].Value = dr[0]["purchase_margin"].ToString();
                                        row.Cells["delivery_days"].Value = dr[0]["delivery_days"].ToString();
                                        row.Cells["cost_unit"].Value = dr[0]["cost_unit"].ToString();

                                        bool weight_calculate;
                                        if (!bool.TryParse(dr[0]["weight_calculate"].ToString(), out weight_calculate))
                                            weight_calculate = true;
                                        row.Cells["weight_calculate"].Value = weight_calculate;
                                    }
                                }
                                //병합여부
                                row.Cells["is_merge"].Value = true;
                                row.Cells["merge_product"].Value = merge_product.Trim();
                                mergeList.Add(main_code);
                            }
                            else
                                isOutput = false;  //이미 메인품목이 출력된 것으로 판단해 더이상 출력X
                        }*/
                    }
                    //5.대표품목과 관련없으면 그냥 출력
                    if (isOutput)
                    {
                        n = dgvProduct.Rows.Add();
                        row = dgvProduct.Rows[n];

                        row.Cells["category"].Value = productDt.Rows[i]["대분류1"].ToString();
                        row.Cells["category_code"].Value = productDt.Rows[i]["대분류"].ToString();
                        row.Cells["product_code"].Value = productDt.Rows[i]["품명코드"].ToString();
                        row.Cells["origin_code"].Value = productDt.Rows[i]["원산지코드"].ToString();
                        row.Cells["sizes_code"].Value = productDt.Rows[i]["규격코드"].ToString();


                        row.Cells["product"].Value = productDt.Rows[i]["품명"].ToString();
                        row.Cells["origin"].Value = productDt.Rows[i]["원산지"].ToString();
                        row.Cells["sizes"].Value = productDt.Rows[i]["규격"].ToString();
                        row.Cells["unit"].Value = productDt.Rows[i]["단위"].ToString().Trim();
                        row.Cells["price_unit"].Value = productDt.Rows[i]["가격단위"].ToString();
                        row.Cells["unit_count"].Value = productDt.Rows[i]["단위수량"].ToString();
                        row.Cells["bundle_count"].Value = productDt.Rows[i]["묶음수"].ToString();
                        row.Cells["seaover_unit"].Value = productDt.Rows[i]["SEAOVEr단위"].ToString();
                        /*row.Cells["manager1"].Value = productDt.Rows[i]["담당자1"].ToString();
                        row.Cells["manager2"].Value = productDt.Rows[i]["담당자2"].ToString();*/
                        row.Cells["division"].Value = productDt.Rows[i]["구분"].ToString();
                        row.Cells["seaover_current_sales_price"].Value = Convert.ToDouble(productDt.Rows[i]["매출단가"].ToString()).ToString("#,##0");
                        row.Cells["seaover_current_purchase_price"].Value = Convert.ToDouble(productDt.Rows[i]["최저단가"].ToString()).ToString("#,##0");
                        row.Cells["seaover_current_normal_price"].Value = Convert.ToDouble(productDt.Rows[i]["일반시세"].ToString()).ToString("#,##0");

                        //담당자12
                        row.Cells["manager1"].Value = "";
                        row.Cells["manager2"].Value = "";

                        if (mngDt.Rows.Count > 0)
                        {
                            whr = "품명 = '" + row.Cells["product"].Value.ToString() + "'"
                            + " AND 원산지 = '" + row.Cells["origin"].Value.ToString() + "'"
                            + " AND 규격 = '" + row.Cells["sizes"].Value.ToString() + "'"
                            + " AND 단위 = '" + row.Cells["unit"].Value.ToString() + "'"
                            + " AND 가격단위 = '" + row.Cells["price_unit"].Value.ToString() + "'"
                            + " AND 단위수량 = " + row.Cells["unit_count"].Value.ToString()
                            + " AND SEAOVER단위 = '" + row.Cells["seaover_unit"].Value.ToString() + "'";
                            dr = mngDt.Select(whr);
                            if (dr.Length > 0)
                            {
                                row.Cells["manager1"].Value = dr[0]["담당자1"].ToString();
                                row.Cells["manager2"].Value = dr[0]["담당자2"].ToString();
                            }
                        }
                        //중량계산
                        row.Cells["weight_calculate"].Value = true;

                        //품목 추가정보
                        if (tmDt.Rows.Count > 0)
                        {
                            whr = "product = '" + row.Cells["product"].Value.ToString() + "'"
                            + " AND origin = '" + row.Cells["origin"].Value.ToString() + "'"
                            + " AND sizes = '" + row.Cells["sizes"].Value.ToString() + "'"
                            + " AND (unit = '" + row.Cells["unit"].Value.ToString() + "'"
                            + " OR unit = '" + row.Cells["seaover_unit"].Value.ToString() + "')";
                            dr = tmDt.Select(whr);
                            if (dr.Length > 0)
                            {
                                row.Cells["tax"].Value = dr[0]["tax"].ToString();
                                row.Cells["custom"].Value = dr[0]["custom"].ToString();
                                row.Cells["incidental_expense"].Value = dr[0]["incidental_expense"].ToString();
                                row.Cells["production_days"].Value = dr[0]["production_days"].ToString();
                                row.Cells["purchase_margin"].Value = dr[0]["purchase_margin"].ToString();
                                row.Cells["delivery_days"].Value = dr[0]["delivery_days"].ToString();
                                row.Cells["cost_unit"].Value = dr[0]["cost_unit"].ToString();

                                bool weight_calculate;
                                if (!bool.TryParse(dr[0]["weight_calculate"].ToString(), out weight_calculate))
                                    weight_calculate = true;
                                row.Cells["weight_calculate"].Value = weight_calculate;
                            }
                        }
                        //묶음품목
                        row.Cells["main_id"].Value = main_id;
                        row.Cells["merge_product"].Value = merge_product.Trim();
                        if(!string.IsNullOrEmpty(merge_product))
                            row.Cells["is_merge"].Value = true;
                        else
                            row.Cells["is_merge"].Value = false;
                        //체크박스 유효성
                        if (row.Cells["weight_calculate"].Value == null || row.Cells["weight_calculate"].Value.ToString() == String.Empty)
                            row.Cells["weight_calculate"].Value = true;
                    }
                }
                //첫번째 레코드 선택
                if (dgvProduct.Rows.Count > 0)
                {
                    dgvProduct.ClearSelection();
                    dgvProduct.Rows[0].Cells["chk"].Value = true;
                }
                this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            }
        }
        private void GetProductInfo(DataGridViewRow row = null)
        {
            if (row == null)
            {
                row = GetSelectProductDgvr();
                if (row == null)
                    return;
            }
            else if ((row.Cells["product"].Value == null || string.IsNullOrEmpty(row.Cells["product"].Value.ToString()))
                || (row.Cells["origin"].Value == null || string.IsNullOrEmpty(row.Cells["origin"].Value.ToString()))
                || (row.Cells["sizes"].Value == null || string.IsNullOrEmpty(row.Cells["sizes"].Value.ToString()))
                || (row.Cells["unit"].Value == null || string.IsNullOrEmpty(row.Cells["unit"].Value.ToString())))
            {
                MessageBox.Show(this, "품명정보가 올바르지 않습니다.");
                this.Activate();
                return;
            }

            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.Value == null)
                    cell.Value = string.Empty;
            }


            //초기화
            lbProduct.Text = "품명 : " + row.Cells["product"].Value.ToString();
            lbOrigin.Text = "원산지 : " + row.Cells["origin"].Value.ToString();
            lbSizes.Text = "규격 : " + row.Cells["sizes"].Value.ToString();
            lbUnit.Text = "단위 : " + row.Cells["unit"].Value.ToString();
            lbPriceUnit.Text = "가격단위 : " + row.Cells["price_unit"].Value.ToString();
            lbUnitCount.Text = "단위수량 : " + row.Cells["unit_count"].Value.ToString();
            lbSeaoverUnit.Text = "SEAOVER단위 : " + row.Cells["seaover_unit"].Value.ToString();

            //품목 기타정보
            DataTable tmDt = productOtherCostRepository.GetProductInfoAsOne(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), "", "");
            if (tmDt != null && tmDt.Rows.Count > 0)
            {
                DataRow[] dr = tmDt.Select($"unit = '{row.Cells["unit"].Value.ToString()}' OR unit = '{row.Cells["seaover_unit"].Value.ToString()}'");
                if (dr.Length > 0)
                {
                    txtCustom.Text = dr[0]["custom"].ToString();
                    txtTax.Text = dr[0]["tax"].ToString();
                    txtIncidentalExpense.Text = dr[0]["incidental_expense"].ToString();
                    

                    double delivery_days;
                    if (!double.TryParse(dr[0]["delivery_days"].ToString(), out delivery_days))
                        delivery_days = 15;
                    txtShippingTerm.Text = delivery_days.ToString();

                    double purchase_margin;
                    if (!double.TryParse(dr[0]["purchase_margin"].ToString(), out purchase_margin))
                        purchase_margin = 0;
                    txtPurchaseMargin.Text = purchase_margin.ToString();
                    if (purchase_margin > 0)
                        lbSetting.Visible = true;
                    else
                        lbSetting.Visible = false;

                    double production_days;
                    if (!double.TryParse(dr[0]["production_days"].ToString(), out production_days))
                        production_days = 20;
                    txtMakeTerm.Text = production_days.ToString();
                }
            }
            else
            {
                double custom;
                if (row.Cells["custom"].Value == null || !double.TryParse(row.Cells["custom"].Value.ToString(), out custom))
                    custom = 0;
                txtCustom.Text = custom.ToString();
                double tax;
                if (row.Cells["tax"].Value == null || !double.TryParse(row.Cells["tax"].Value.ToString(), out tax))
                    tax = 0;
                txtTax.Text = tax.ToString();
                double incidental_expense;
                if (row.Cells["incidental_expense"].Value == null || !double.TryParse(row.Cells["incidental_expense"].Value.ToString(), out incidental_expense))
                    incidental_expense = 0;
                txtIncidentalExpense.Text = incidental_expense.ToString();
                double delivery_days;
                if (row.Cells["delivery_days"].Value == null || !double.TryParse(row.Cells["delivery_days"].Value.ToString(), out delivery_days))
                    delivery_days = 15;
                txtShippingTerm.Text = delivery_days.ToString();
                double purchase_margin;
                if (row.Cells["purchase_margin"].Value == null || !double.TryParse(row.Cells["purchase_margin"].Value.ToString(), out purchase_margin))
                    purchase_margin = 0;
                if (purchase_margin > 0)
                    lbSetting.Visible = true;
                else
                    lbSetting.Visible = false;
                txtPurchaseMargin.Text = purchase_margin.ToString();

                double production_days;
                if (row.Cells["production_days"].Value == null || !double.TryParse(row.Cells["production_days"].Value.ToString(), out production_days))
                    production_days = 20;
                txtMakeTerm.Text = production_days.ToString();

            }

            //판매단가
            double seaover_current_sales_price;
            if (row.Cells["seaover_current_sales_price"].Value == null || !double.TryParse(row.Cells["seaover_current_sales_price"].Value.ToString(), out seaover_current_sales_price))
                seaover_current_sales_price = 20;
            txtSalesPrice.Text = seaover_current_sales_price.ToString("#,##0");
            //매입단가(최저단가)
            double seaover_current_purchase_price;
            if (row.Cells["seaover_current_purchase_price"].Value == null || !double.TryParse(row.Cells["seaover_current_purchase_price"].Value.ToString(), out seaover_current_purchase_price))
                seaover_current_purchase_price = 7;
            txtPurchasePrice.Text = seaover_current_purchase_price.ToString("#,##0");
            //일반시세
            double seaover_current_normal_price;
            if (row.Cells["seaover_current_normal_price"].Value == null || !double.TryParse(row.Cells["seaover_current_normal_price"].Value.ToString(), out seaover_current_normal_price))
                seaover_current_normal_price = 7;
            txtNormalPrice.Text = seaover_current_normal_price.ToString("#,##0");


            //초기화
            dgvMakePeriod.Rows.Clear();
            dgvMakePeriod.Rows.Add();
            dgvMakePeriod.ReadOnly = true;
            dgvMakePeriod.Enabled = false;
            dgvContractPeriod.Rows.Clear();
            dgvContractPeriod.Rows.Add();
            dgvContractPeriod.ReadOnly = true;
            dgvContractPeriod.Enabled = false;
            dgvMakePeriod.Rows[0].DefaultCellStyle.BackColor = Color.White;
            dgvContractPeriod.Rows[0].DefaultCellStyle.BackColor = Color.White;
            //조업시기, 생산시기
            DataTable dt = contractRecommendationRepository.GetRecommendAsOne(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString());
            dgvMakePeriod.Rows[0].Cells[0].Value = "조업시기";
            dgvContractPeriod.Rows[0].Cells[0].Value = "계약시기";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataGridView dgv;
                Color color;
                if (dt.Rows[i]["division"].ToString() == "조업시기")
                {
                    dgv = dgvMakePeriod;
                    if (dt.Rows[i]["recommend_level"].ToString() == "1")
                        color = Color.LightGreen;
                    else if (dt.Rows[i]["recommend_level"].ToString() == "2")
                        color = Color.Green;
                    else
                        color = Color.DarkGreen;
                }
                else
                {
                    dgv = dgvContractPeriod;
                    if (dt.Rows[i]["recommend_level"].ToString() == "1")
                        color = Color.Pink;
                    else if (dt.Rows[i]["recommend_level"].ToString() == "2")
                        color = Color.Red;
                    else
                        color = Color.DarkRed;
                }

                string month = dt.Rows[i]["month"].ToString();
                dgv.Rows[0].Cells[Convert.ToInt16(month)].Style.BackColor = color;
            }
            dgvMakePeriod.ClearSelection();
            dgvContractPeriod.ClearSelection();

            //병합품목
            dgvMergeProduct.Rows.Clear();
            if (row.Cells["merge_product"].Value != null && !string.IsNullOrEmpty(row.Cells["merge_product"].Value.ToString()))
            {
                pnMergeProduct.Visible = true;
                string merge_product = row.Cells["merge_product"].Value.ToString();
                string[] sub_product = merge_product.Trim().Split('\n');
                for (int i = 0; i < sub_product.Length; i++)
                {
                    string[] products = sub_product[i].Trim().Split('^');
                    if (products.Length > 5)
                    {
                        int n = dgvMergeProduct.Rows.Add();
                        dgvMergeProduct.Rows[n].Cells["item_product"].Value = products[0];
                        dgvMergeProduct.Rows[n].Cells["item_origin"].Value = products[1];
                        dgvMergeProduct.Rows[n].Cells["item_sizes"].Value = products[2];
                        dgvMergeProduct.Rows[n].Cells["item_unit"].Value = products[3];
                        dgvMergeProduct.Rows[n].Cells["item_price_unit"].Value = products[4];
                        dgvMergeProduct.Rows[n].Cells["item_unit_count"].Value = products[5];
                        dgvMergeProduct.Rows[n].Cells["item_seaover_unit"].Value = products[6];

                        if (row.Cells["product"].Value.ToString() == products[0]
                            && row.Cells["origin"].Value.ToString() == products[1]
                            && row.Cells["sizes"].Value.ToString() == products[2]
                            && row.Cells["unit"].Value.ToString() == products[3]
                            && row.Cells["price_unit"].Value.ToString() == products[4]
                            && row.Cells["unit_count"].Value.ToString() == products[5]
                            && row.Cells["seaover_unit"].Value.ToString() == products[6])
                            dgvMergeProduct.Rows[n].Cells["is_main"].Value = true;
                }
                }
            }
            else
                pnMergeProduct.Visible = false;
        }
        private DataGridViewRow GetSelectProductDgvr()
        {
            DataGridViewRow row = null;
            if (dgvProduct.Rows.Count > 0)
            {
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value))
                    {
                        row = dgvProduct.Rows[i];
                        break;
                    }
                }
            }
            return row;
        }
        private void dgvProduct_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "chk")
                {
                    if (Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                        dgvProduct.Rows[e.RowIndex].Cells["chk"].Style.BackColor = Color.Red;
                    else
                        dgvProduct.Rows[e.RowIndex].Cells["chk"].Style.BackColor = Color.White;
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "is_merge")
                {
                    if (Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                        dgvProduct.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Beige;
                    else
                        dgvProduct.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void dgvProduct_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);

                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    dgvProduct.Rows[i].Cells["chk"].Value = false;

                this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);

                //노바시같이 업체별에는 박스, kg인데 선적은 트레이로 하는 품목을 위해 박스단가 체크박스 체크여부, 중량계산일땐 역대 매입가 박스단가, 아닐땐 트레이단가
                if (dgvProduct.Rows[e.RowIndex].Cells["product"].Value.ToString().Contains("노바시새우") && !Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells["weight_calculate"].Value))
                {
                    cbMultiplyTray.Checked = true;
                    rbBoxPrice.Checked = true;
                }
                else if (!Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells["weight_calculate"].Value))
                {
                    cbMultiplyTray.Checked = false;
                    rbTrayPrice.Checked = true;
                }
                else
                    rbBoxPrice.Checked = true;
                //

                dgvProduct.Rows[e.RowIndex].Cells["chk"].Value = true;
            }
        }

        private void dgvProduct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "chk")
                {
                    if (Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                        dgvProduct.Rows[e.RowIndex].Cells["chk"].Style.BackColor = Color.Red;
                    else
                        dgvProduct.Rows[e.RowIndex].Cells["chk"].Style.BackColor = Color.White;

                    this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                    this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                    //초기화
                    seaover_average_cost_price = 0;
                    total_average_cost_price = 0;
                    txtInQty.Text = "0";
                    GetProductInfo(dgvProduct.Rows[e.RowIndex]);  
                    OpenExhaustedManager(dgvProduct.Rows[e.RowIndex]);
                    GetMarketPriceByCompany(dgvProduct.Rows[e.RowIndex]);
                    GetPurchase(dgvProduct.Rows[e.RowIndex]);
                    GetOffer(dgvProduct.Rows[e.RowIndex]);
                    GetPurchasePriceList(dgvProduct.Rows[e.RowIndex]);
                    GetSales(dgvProduct.Rows[e.RowIndex]);
                    GetSales2(dgvProduct.Rows[e.RowIndex]);
                    SetDashboardSummary();
                    this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                    this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "is_merge")
                {
                    bool is_merge;
                    if (dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !bool.TryParse(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out is_merge))
                        is_merge = false;

                    if (is_merge)
                        dgvProduct.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Beige;
                    else
                        dgvProduct.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }
        #endregion

        #region 통관내역 가져오기
        private void OpenExhaustedManager(DataGridViewRow row)
        {
            dgvContract.Rows.Clear();
            dgvStockSales.Rows.Clear();
            dgvStockSales.Rows.Add();
            //유효성검사
            if ((row.Cells["product"].Value == null || string.IsNullOrEmpty(row.Cells["product"].Value.ToString()))
                || (row.Cells["origin"].Value == null || string.IsNullOrEmpty(row.Cells["origin"].Value.ToString()))
                || (row.Cells["sizes"].Value == null || string.IsNullOrEmpty(row.Cells["sizes"].Value.ToString()))
                || (row.Cells["unit"].Value == null || string.IsNullOrEmpty(row.Cells["unit"].Value.ToString())))
                return;
            ShortManagerModel smModel = GetProductStockInfo(row);

            SetPendingList(smModel, row);
        }

        private ShortManagerModel GetProductStockInfo(DataGridViewRow row)
        {
            ShortManagerModel smModel = new ShortManagerModel();
            smModel.product = row.Cells["product"].Value.ToString();
            smModel.origin = row.Cells["origin"].Value.ToString();
            smModel.sizes = row.Cells["sizes"].Value.ToString();
            smModel.unit = row.Cells["seaover_unit"].Value.ToString();

            //영업일 수
            int salesMonth = 6;   //매출기간
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
            }
            int workDays = 0;
            string eDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string sDate = DateTime.Now.AddMonths(-salesMonth).ToString("yyyy-MM-dd");
            if(salesMonth == 45)
                sDate = DateTime.Now.AddDays(-salesMonth).ToString("yyyy-MM-dd");
            
            common.GetWorkDay(Convert.ToDateTime(sDate), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), out workDays);
            //2023-06-15 오늘로 시작해서 영업일 -1
            workDays--;


            //단위
            double unit;
            if (row.Cells["seaover_unit"].Value == null || !double.TryParse(row.Cells["seaover_unit"].Value.ToString(), out unit))
                unit = 1;

            //제외판매수량
            double excluded_qty = 0;
            //판매량
            double stock = 0, salesCnt = 0;
            //병합품목 순회
            //string[] merge_product = row.Cells["merge_product"].Value.ToString().Split('\n');
            string merge_product = "";
            bool isMerge = false;
            //머지품목 일때
            if (row.Cells["merge_product"].Value != null && !string.IsNullOrEmpty(row.Cells["merge_product"].Value.ToString()))
            {
                isMerge = true;
                merge_product = row.Cells["merge_product"].Value.ToString();
            }

            //데이터 출력
            double total_unpending_qty = 0;
            double total_pending_qty = 0;
            double total_reserved_qty = 0;
            DataTable productDt = seaoverRepository.GetStockAndSalesDetail2(row.Cells["product"].Value.ToString()
                                                                            , row.Cells["origin"].Value.ToString()
                                                                            , row.Cells["sizes"].Value.ToString()
                                                                            , row.Cells["seaover_unit"].Value.ToString()
                                                                            , salesMonth.ToString()
                                                                            , merge_product, isMerge);
            if (productDt.Rows.Count > 0)
            {
                for (int i = 0; i < productDt.Rows.Count; i++)
                {

                    double unpending_qty = Convert.ToDouble(productDt.Rows[i]["미통관"].ToString());
                    total_unpending_qty += unpending_qty;
                    double pending_qty = Convert.ToDouble(productDt.Rows[i]["통관"].ToString());
                    total_pending_qty += pending_qty;
                    double reserved_qty = Convert.ToDouble(productDt.Rows[i]["예약수"].ToString());
                    total_reserved_qty += reserved_qty;

                    //총 판매, 재고
                    if (cbSeaoverUnpending.Checked)
                        stock += unpending_qty;
                    if (cbSeaoverPending.Checked)
                        stock += pending_qty;
                    if (cbReserved.Checked)
                        stock -= reserved_qty;

                    salesCnt += Convert.ToDouble(productDt.Rows[i]["매출수"].ToString());
                    if (salesCnt < 0)
                        salesCnt = 0;
                }
            }
            //영업일 수
            DateTime sttdate, enddate;
            if (salesMonth == 45)
            {
                enddate = DateTime.Now;
                sttdate = DateTime.Now.AddDays(-salesMonth);
            }
            else
            {
                enddate = DateTime.Now;
                sttdate = DateTime.Now.AddMonths(-salesMonth);
            }

            //매출제외 수량===========================================================================
            DataTable eDt = productExcludedSalesRepository.GetExcludedSalesByMonth(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                                                                            , row.Cells["product"].Value.ToString()
                                                                            , row.Cells["origin"].Value.ToString()
                                                                            , row.Cells["sizes"].Value.ToString()
                                                                            , row.Cells["unit"].Value.ToString()
                                                                            , row.Cells["price_unit"].Value.ToString()
                                                                            , row.Cells["unit_count"].Value.ToString()
                                                                            , row.Cells["seaover_unit"].Value.ToString()
                                                                            , merge_product, isMerge);
            //제외
            DataRow[] eRow = null;
            if (eDt != null && eDt.Rows.Count > 0)
            {
                for (int i = 0; i < eDt.Rows.Count; i++)
                    excluded_qty += Convert.ToDouble(eDt.Rows[i]["sale_qty"].ToString());
            }
            salesCnt -= excluded_qty;
            if (double.IsNaN(salesCnt))
                salesCnt = 0;

            //재고
            dgvStockSales.Rows[0].Cells["seaover_unpending_qty"].Value = total_unpending_qty.ToString("#,##0");
            dgvStockSales.Rows[0].Cells["seaover_pending_qty"].Value = total_pending_qty.ToString("#,##0");
            dgvStockSales.Rows[0].Cells["reserved_qty"].Value = total_reserved_qty.ToString("#,##0");


            double avg_day_sales = salesCnt / workDays;
            double avg_month_sales = avg_day_sales * 21;
            dgvStockSales.Rows[0].Cells["day_sales_qty_double"].Value = avg_day_sales;
            dgvStockSales.Rows[0].Cells["day_sales_qty"].Value = avg_day_sales.ToString("#,##0");

            //2023-06-20 소희대리님께서 1개월 판매량은 나눌필요가 없이 그대로 반영 요청ㅇ
            if (salesMonth == 1)
                avg_month_sales = salesCnt;
            dgvStockSales.Rows[0].Cells["month_sales_qty"].Value = avg_month_sales.ToString("#,##0");

            //model 
            smModel.avg_sales_day = avg_day_sales;
            smModel.avg_sales_month = avg_month_sales;

            double eStock;
            if (!double.TryParse(txtExcludeStock.Text, out eStock))
                eStock = 0;

            smModel.real_stock = stock - eStock;
            //smModel.real_stock = Convert.ToDouble(row.Cells["seaover_unpending"].Value.ToString()) + Convert.ToDouble(row.Cells["seaover_pending"].Value.ToString());           //최초 소진일자 계산
            DateTime exhausted_date;
            double exhausted_cnt;

            //쇼트일자
            int standard;
            if (!int.TryParse(txtStandardOfShort.Text, out standard))
                standard = 0;
            common.GetExhausedDateDayd(DateTime.Now, smModel.real_stock, smModel.avg_sales_day, standard, null, out exhausted_date, out exhausted_cnt);

            smModel.exhaust_date = exhausted_date.ToString("yyyy-MM-dd");
            enddate = DateTime.Now;
            smModel.enddate = enddate.ToString("yyyy-MM-dd");

            return smModel;
        }


        private void SetPendingList(ShortManagerModel smModel, DataGridViewRow row)
        {
            SetData(smModel, row);
            GetUnpendingData(smModel);
            SetHeaderStyle();
            if (SortSetting())
            {
                CalculateStock(smModel);
            }
            RealStockCalculate();
        }

        private void RealStockCalculate()
        {
            if (dgvStockSales.Rows.Count > 0)
            {
                DataGridViewRow row = dgvStockSales.Rows[0];
                row.ReadOnly = false;
                double shipment_qty;
                if (row.Cells["shipment_qty"].Value == null || !double.TryParse(row.Cells["shipment_qty"].Value.ToString(), out shipment_qty))
                    shipment_qty = 0;
                double shipping_qty;
                if (row.Cells["shipping_qty"].Value == null || !double.TryParse(row.Cells["shipping_qty"].Value.ToString(), out shipping_qty))
                    shipping_qty = 0;
                double seaover_unpending_qty;
                if (row.Cells["seaover_unpending_qty"].Value == null || !double.TryParse(row.Cells["seaover_unpending_qty"].Value.ToString(), out seaover_unpending_qty))
                    seaover_unpending_qty = 0;
                double seaover_pending_qty;
                if (row.Cells["seaover_pending_qty"].Value == null || !double.TryParse(row.Cells["seaover_pending_qty"].Value.ToString(), out seaover_pending_qty))
                    seaover_pending_qty = 0;
                double reserved_qty;
                if (row.Cells["reserved_qty"].Value == null || !double.TryParse(row.Cells["reserved_qty"].Value.ToString(), out reserved_qty))
                    reserved_qty = 0;
                //실재고 반영 체크박스
                double total_qty = 0;
                if (cbShipment.Checked)
                    total_qty += shipment_qty + shipping_qty;
                if (cbSeaoverUnpending.Checked)
                    total_qty += seaover_unpending_qty;
                if (cbSeaoverPending.Checked)
                    total_qty += seaover_pending_qty;
                if (cbReserved.Checked)
                    total_qty -= reserved_qty;

                //일판매
                double day_sales_qty_double;
                if (row.Cells["day_sales_qty_double"].Value == null || !double.TryParse(row.Cells["day_sales_qty_double"].Value.ToString(), out day_sales_qty_double))
                    day_sales_qty_double = 0;
                /*if (!cbAllUnit.Checked)
                {
                    
                }
                else
                {
                    DataGridViewRow select_row = GetSelectProductDgvr();
                    day_sales_qty_double = 0;
                    string month = cbSaleTerm.Text.Replace("개월", "").Replace("일", "");
                    DataTable saleDt = salesRepository.GetIntegretionSaleQty(select_row.Cells["product"].Value.ToString()
                                                                        , select_row.Cells["origin"].Value.ToString()
                                                                        , select_row.Cells["sizes"].Value.ToString()
                                                                        , month);
                    if (saleDt.Rows.Count > 0)
                    {
                        double month_sales_qty_double;
                        if (!double.TryParse(saleDt.Rows[0]["매출수"].ToString(), out month_sales_qty_double))
                            month_sales_qty_double = 0;

                        int workDays = 0;
                        string sDate = DateTime.Now.AddDays(-1).AddMonths(-Convert.ToInt32(month)).ToString("yyyy-MM-dd");
                        if (Convert.ToInt32(month) == 45)
                            sDate = DateTime.Now.AddDays(-1).AddDays(-Convert.ToInt32(month)).ToString("yyyy-MM-dd");
                        string eDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                        common.GetWorkDay(Convert.ToDateTime(sDate), Convert.ToDateTime(eDate), out workDays);
                        day_sales_qty_double = month_sales_qty_double / workDays;
                    }
                }*/
                //월판매
                double enable_sales_days = total_qty / day_sales_qty_double;

                //회전율
                double month_around = enable_sales_days / 21;

                row = dgvStockSales.Rows[0];
                //실재고
                row.Cells["total_qty"].Value = total_qty.ToString("#,##0");

                row.Cells["shipment_qty"].Value = shipment_qty;
                row.Cells["shipping_qty"].Value = shipping_qty;
                row.Cells["seaover_unpending_qty"].Value = seaover_unpending_qty;
                row.Cells["seaover_pending_qty"].Value = seaover_pending_qty;
                row.Cells["reserved_qty"].Value = reserved_qty;

                row.Cells["day_sales_qty_double"].Value = day_sales_qty_double;
                row.Cells["day_sales_qty"].Value = day_sales_qty_double.ToString("#,##0");
                //row.Cells["month_sales_qty"].Value = (day_sales_qty_double * 21).ToString("#,##0");

                row.Cells["total_month_around"].Value = month_around.ToString("#,##0.00");
            }
        }
        private void ReplaceSalesCostToPendingCost(DataGridViewRow row, out double seaover_cost_price)
        {
            seaover_cost_price = 0;
            double exchange_rate;
            if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                exchange_rate = 0;

            //최종S원가
            double final_average_cost = 0;
            double final_qty = 0;
            //ToolTip txt
            string tooltip = "";

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

            if (sub_product.Length > 0)
            {
                //단위수량
                double unit_count;
                if (row.Cells["unit_count"].Value == null || !double.TryParse(row.Cells["unit_count"].Value.ToString(), out unit_count))
                    unit_count = 0;
                if (unit_count == 0)
                    unit_count = 1;

                DataTable productDt = priceComparisonRepository.GetNotSalesCostProduct(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, sub_product);
                DataTable pendingDt = customsRepository.GetProductForNotSalesCost(txtProduct.Text, txtOrigin.Text, txtSizes.Text, "", productDt, sub_product);

                if (productDt.Rows.Count > 0)
                {
                    productDt.Columns["매출원가"].ReadOnly = false;
                    productDt.Columns["환율"].ReadOnly = false;
                    productDt.Columns["오퍼가"].ReadOnly = false;
                    productDt.Columns["trq"].ReadOnly = false;
                    productDt.Columns["custom_id"].ReadOnly = false;
                    productDt.Columns["sub_id"].ReadOnly = false;
                    productDt.Columns["etd"].ReadOnly = false;
                    productDt.Columns["isPendingCalculate"].ReadOnly = false;
                    //데이터 조합
                    for (int i = 0; i < productDt.Rows.Count; i++)
                    {
                        if (Convert.ToDouble(productDt.Rows[i]["수량"].ToString()) > 0 && Convert.ToDouble(productDt.Rows[i]["매출원가"].ToString()) == 0 && pendingDt.Rows.Count > 0)
                        {
                            string whr = "ato_no = '" + productDt.Rows[i]["AtoNo"].ToString() + "'"
                                        + " AND product = '" + productDt.Rows[i]["품명"].ToString() + "'"
                                        + " AND origin = '" + productDt.Rows[i]["원산지"].ToString() + "'"
                                        + " AND sizes = '" + productDt.Rows[i]["규격"].ToString() + "'"
                                        + " AND unit = '" + productDt.Rows[i]["단위"].ToString() + "'";
                            DataRow[] dr = pendingDt.Select(whr);
                            if (dr.Length > 0)
                            {
                                for (int j = 0; j < dr.Length; j++)
                                {
                                    double unit_price = Convert.ToDouble(dr[j]["unit_price"].ToString());
                                    double box_weight = Convert.ToDouble(dr[j]["unit"].ToString());
                                    double cost_unit = Convert.ToDouble(dr[j]["cost_unit"].ToString());
                                    double trq_amount = Convert.ToDouble(dr[j]["trq_amount"].ToString());

                                    //단위맞추기
                                    unit_price = unit_price * (box_weight / Convert.ToDouble(row.Cells["seaover_unit"].Value.ToString())) / unit_count;
                                    productDt.Rows[i]["오퍼가"] = unit_price;   //달러인 오퍼가 등록
                                    //트레이단가 -> 중량 단가
                                    bool isWeight = Convert.ToBoolean(dr[j]["weight_calculate"].ToString());
                                    if (!isWeight && cost_unit > 0)
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

                                    double exchangeRate;
                                    if (!double.TryParse(dr[j]["exchange_rate"].ToString(), out exchangeRate))
                                        exchangeRate = exchange_rate;
                                    //
                                    double unit;
                                    if (row.Cells["seaover_unit"].Value == null || !double.TryParse(row.Cells["seaover_unit"].Value.ToString(), out unit))
                                        unit = 1;

                                    double sales_cost;
                                    //일반
                                    if (trq_amount == 0)
                                        sales_cost = unit_price * exchangeRate * (1 + custom + tax + incidental_expense) * unit;
                                    //trq
                                    else
                                        sales_cost = (unit_price * exchangeRate * (1 + tax + incidental_expense) * unit) + (box_weight * trq_amount);

                                    //입고일자
                                    string etd_txt = "";
                                    DateTime etd;
                                    if (DateTime.TryParse(dr[j]["etd2"].ToString(), out etd))
                                        etd_txt = etd.ToString("yyyy-MM-dd");

                                    productDt.Rows[i]["매출원가"] = sales_cost;
                                    productDt.Rows[i]["환율"] = exchangeRate;
                                    productDt.Rows[i]["trq"] = trq_amount;
                                    productDt.Rows[i]["custom_id"] = dr[j]["id"].ToString();
                                    productDt.Rows[i]["sub_id"] = dr[j]["sub_id"].ToString();
                                    productDt.Rows[i]["etd"] = etd_txt;
                                    productDt.Rows[i]["isPendingCalculate"] = "TRUE";
                                    break;
                                }
                            }
                        }

                        if (Convert.ToDouble(productDt.Rows[i]["매출원가"].ToString()) == 0)
                            productDt.Rows[i]["매출원가"] = Convert.ToDouble(productDt.Rows[i]["매입금액"].ToString());
                    }

                    productDt.AcceptChanges();
                }
                
                //데이터 출력
                for (int k = 0; k < sub_product.Length; k++)
                {    
                    string[] product = sub_product[k].Trim().Split('^');                    
                    if (productDt.Rows.Count > 0 || pendingDt.Rows.Count > 0)
                    {
                        productDt.Columns["수량"].ReadOnly = false;
                        productDt.Columns["환율"].ReadOnly = false;
                        productDt.Columns["매출원가"].ReadOnly = false;
                        productDt.Columns["etd"].ReadOnly = false;
                        //매출 평균원가 계산방법
                        DataTable seaoverPriceDt = new DataTable();
                        seaoverPriceDt.Columns.Add("seaover_custom_id", typeof(int));
                        seaoverPriceDt.Columns.Add("seaover_sub_id", typeof(int));
                        seaoverPriceDt.Columns.Add("seaover_ato_no", typeof(string));
                        seaoverPriceDt.Columns.Add("seaover_cost_price_usd", typeof(double));
                        seaoverPriceDt.Columns.Add("seaover_cost_price_krw", typeof(double));
                        seaoverPriceDt.Columns.Add("seaover_exchange_rate", typeof(double));
                        seaoverPriceDt.Columns.Add("agent_price", typeof(double));
                        seaoverPriceDt.Columns.Add("seaover_qty", typeof(double));
                        seaoverPriceDt.Columns.Add("seaover_trq", typeof(double));
                        seaoverPriceDt.Columns.Add("seaover_is_trq", typeof(bool));
                        seaoverPriceDt.Columns.Add("interest", typeof(double));
                        seaoverPriceDt.Columns.Add("refrigeration_fee", typeof(double));
                        seaoverPriceDt.Columns.Add("in_date", typeof(string));
                        seaoverPriceDt.Columns.Add("stock_etd", typeof(string));
                        seaoverPriceDt.Columns.Add("cnt", typeof(int));
                        seaoverPriceDt.Columns.Add("stock_status", typeof(string));
                        //출력
                        if (productDt.Rows.Count > 0)
                        {
                            //평균
                            double total_average_cost = 0;
                            double total_qty = 0;
                            //씨오버단위
                            double unit;
                            if (!double.TryParse(row.Cells["seaover_unit"].Value.ToString(), out unit))
                                unit = 1;

                            foreach (DataRow dr in productDt.Rows)
                            {
                                double stock_unit;
                                if (!double.TryParse(dr["단위"].ToString(), out stock_unit))
                                    stock_unit = 1;
                                double qty = Convert.ToDouble(dr["수량"].ToString());
                                double sales_cost = Convert.ToDouble(dr["매출원가"].ToString());

                                //계산된 매출원가
                                if (bool.TryParse(dr["isPendingCalculate"].ToString(), out bool isPendingCalculate) && isPendingCalculate)
                                {
                                    //동원시 추가
                                    if (dr["AtoNo"].ToString().Contains("dw") || dr["AtoNo"].ToString().Contains("DW")
                                        || dr["AtoNo"].ToString().Contains("hs") || dr["AtoNo"].ToString().Contains("HS")
                                        || dr["AtoNo"].ToString().Contains("od") || dr["AtoNo"].ToString().Contains("OD")
                                        || dr["AtoNo"].ToString().Contains("ad") || dr["AtoNo"].ToString().Contains("AD"))
                                        sales_cost = sales_cost * 1.025;
                                    else if (dr["AtoNo"].ToString().Contains("jd") || dr["AtoNo"].ToString().Contains("JD"))
                                        sales_cost = sales_cost * 1.02;

                                    //if (row.Cells["price_unit"].Value.ToString().Contains("팩") || row.Cells["price_unit"].Value.ToString().Contains("묶"))
                                    //    sales_cost /= unit_count;

                                    //2023-10-30 냉장료 포함
                                    double refrigeration_fee = 0;
                                    DateTime in_date;
                                    string in_date_txt = "";
                                    if (DateTime.TryParse(dr["입고일자"].ToString(), out in_date))
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
                                        in_date_txt = in_date.ToString("yyyy-MM-dd");
                                    }


                                    //2023-10-25 etd기준 연이자 8% 일발생
                                    double interest = 0;
                                    string etd_txt = "";
                                    if (DateTime.TryParse(dr["etd"].ToString(), out DateTime etd))
                                    {
                                        TimeSpan ts = DateTime.Now - etd;
                                        int days = ts.Days;
                                        interest = sales_cost * 0.08 / 365 * days;
                                        etd_txt = etd.ToString("yyyy-MM-dd");
                                        if (interest < 0)
                                            interest = 0;
                                    }
                                    

                                    //설명
                                    if (interest > 0 && refrigeration_fee > 0)
                                        tooltip += "\n AtoNo : " + dr["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + $" + {interest.ToString("N2")}(이자) + {refrigeration_fee.ToString("#,##0")}(냉장료) = {(sales_cost + interest + refrigeration_fee).ToString("#,##0")} " + "  | 수량 :" + qty;
                                    else if (interest == 0 && refrigeration_fee > 0)
                                        tooltip += "\n AtoNo : " + dr["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + $" + {refrigeration_fee.ToString("#,##0")}(냉장료) = {(sales_cost + refrigeration_fee).ToString("#,##0")} " + "  | 수량 :" + qty;
                                    else if (interest > 0 && refrigeration_fee == 0)
                                        tooltip += "\n AtoNo : " + dr["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + $" + {interest.ToString("N2")}(이자) = {(sales_cost + interest).ToString("#,##0")} " + "  | 수량 :" + qty;
                                    else
                                        tooltip += "\n AtoNo : " + dr["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + "  | 수량 :" + qty;

                                    sales_cost += interest + refrigeration_fee;
                                    total_average_cost += sales_cost * qty;
                                    total_qty += qty;

                                    //Dgv add
                                    DataRow newDr = seaoverPriceDt.NewRow();
                                    newDr["seaover_custom_id"] = Convert.ToInt32(dr["custom_id"].ToString());
                                    newDr["seaover_sub_id"] = Convert.ToInt32(dr["sub_id"].ToString());
                                    newDr["seaover_ato_no"] = dr["AtoNo"].ToString();
                                    newDr["seaover_cost_price_usd"] = Convert.ToDouble(dr["오퍼가"].ToString());
                                    newDr["seaover_cost_price_krw"] = sales_cost;
                                    newDr["seaover_exchange_rate"] = Convert.ToDouble(dr["환율"].ToString());
                                    newDr["seaover_qty"] = qty;
                                    newDr["seaover_trq"] = Convert.ToDouble(dr["trq"].ToString());
                                    newDr["interest"] = interest;
                                    newDr["refrigeration_fee"] = refrigeration_fee;
                                    newDr["in_date"] = in_date_txt;
                                    newDr["stock_etd"] = etd_txt;
                                    newDr["cnt"] = 1;
                                    newDr["stock_status"] = dr["통관"].ToString();
                                    seaoverPriceDt.Rows.Add(newDr);
                                }
                                //씨오버 매출원가
                                else
                                {
                                    sales_cost = sales_cost / stock_unit * unit / unit_count;
                                    total_average_cost += sales_cost * qty;
                                    total_qty += qty;
                                    //계산식
                                    tooltip += "\n AtoNo : " + dr["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + "  수량 :" + qty;
                                    //2024-01-25 매출원가가 0원으로 잡힌것들은 표시를 해야함
                                    if (sales_cost == 0 && qty > 0)
                                        tooltip += "   ***확인필요***";

                                    //Dgv add
                                    DataRow newDr = seaoverPriceDt.NewRow();
                                    newDr["seaover_ato_no"] = dr["AtoNo"].ToString();
                                    newDr["seaover_cost_price_krw"] = sales_cost;
                                    newDr["seaover_qty"] = qty;
                                    //newDr["seaover_trq"] = Convert.ToDouble(dr[j]["trq"].ToString());
                                    newDr["cnt"] = 2;
                                    newDr["stock_status"] = dr["통관"].ToString();
                                    seaoverPriceDt.Rows.Add(newDr);
                                }
                            }
                            //평균원가
                            if (total_average_cost > 0 || total_qty > 0)
                            {
                                final_average_cost += total_average_cost;
                                final_qty += total_qty;
                            }
                            //상세내역 
                            DataView dv = new DataView(seaoverPriceDt);
                            dv.Sort = "cnt";
                            seaoverPriceDt = dv.ToTable();
                            seaoverPriceDt.AcceptChanges();

                            this.dgvSeaoverCostPrice.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSeaoverCostPrice_CellContentClick);
                            dgvSeaoverCostPrice.Rows.Clear();
                            for (int i = 0; i < seaoverPriceDt.Rows.Count; i++)
                            {
                                int n = dgvSeaoverCostPrice.Rows.Add();
                                dgvSeaoverCostPrice.Rows[n].Cells["cost_price_chk"].Value = true;
                                dgvSeaoverCostPrice.Rows[n].Cells["seaover_ato_no"].Value = seaoverPriceDt.Rows[i]["seaover_ato_no"].ToString();
                                double seaover_cost_price_usd;
                                if (!double.TryParse(seaoverPriceDt.Rows[i]["seaover_cost_price_usd"].ToString(), out seaover_cost_price_usd))
                                    seaover_cost_price_usd = 0;
                                dgvSeaoverCostPrice.Rows[n].Cells["seaover_cost_price_usd"].Value = seaover_cost_price_usd;
                                
                                double seaover_qty;
                                if (!double.TryParse(seaoverPriceDt.Rows[i]["seaover_qty"].ToString(), out seaover_qty))
                                    seaover_qty = 0;
                                dgvSeaoverCostPrice.Rows[n].Cells["seaover_qty"].Value = seaover_qty.ToString("#,##0");
                                double seaover_exchange_rate;
                                if (!double.TryParse(seaoverPriceDt.Rows[i]["seaover_exchange_rate"].ToString(), out seaover_exchange_rate))
                                    seaover_exchange_rate = 0;
                                dgvSeaoverCostPrice.Rows[n].Cells["seaover_exchange_rate"].Value = seaover_exchange_rate;
                                int seaover_trq;
                                if (!int.TryParse(seaoverPriceDt.Rows[i]["seaover_trq"].ToString(), out seaover_trq))
                                    seaover_trq = 0;
                                if(seaover_trq > 0)
                                    dgvSeaoverCostPrice.Rows[n].Cells["seaover_is_trq"].Value = true;   
                                else
                                    dgvSeaoverCostPrice.Rows[n].Cells["seaover_is_trq"].Value = false;

                                double refrigeration_fee;
                                if (!double.TryParse(seaoverPriceDt.Rows[i]["refrigeration_fee"].ToString(), out refrigeration_fee))
                                    refrigeration_fee = 0;

                                dgvSeaoverCostPrice.Rows[n].Cells["refrigeration_fee"].Value = refrigeration_fee.ToString("#,##0");

                                double interest;
                                if (!double.TryParse(seaoverPriceDt.Rows[i]["interest"].ToString(), out interest))
                                    interest = 0;
                                dgvSeaoverCostPrice.Rows[n].Cells["interest"].Value = interest.ToString("N2");

                                double seaover_cost_price_krw;
                                if (!double.TryParse(seaoverPriceDt.Rows[i]["seaover_cost_price_krw"].ToString(), out seaover_cost_price_krw))
                                    seaover_cost_price_krw = 0;
                                dgvSeaoverCostPrice.Rows[n].Cells["final_seaover_cost_price_krw"].Value = seaover_cost_price_krw.ToString("#,##0");

                                dgvSeaoverCostPrice.Rows[n].Cells["seaover_cost_price_krw"].Value = (seaover_cost_price_krw - refrigeration_fee - interest).ToString("#,##0");

                                dgvSeaoverCostPrice.Rows[n].Cells["in_date"].Value = seaoverPriceDt.Rows[i]["in_date"].ToString();
                                dgvSeaoverCostPrice.Rows[n].Cells["stock_etd"].Value = seaoverPriceDt.Rows[i]["stock_etd"].ToString();
                                
                                int custom_id;
                                if (!int.TryParse(seaoverPriceDt.Rows[i]["seaover_custom_id"].ToString(), out custom_id))
                                    custom_id = 0;
                                //버튼추가
                                if (custom_id > 0)
                                {
                                    dgvSeaoverCostPrice.Rows[n].Cells["custom_id"].Value = custom_id;
                                    int sub_id;
                                    if (!int.TryParse(seaoverPriceDt.Rows[i]["seaover_sub_id"].ToString(), out sub_id))
                                        sub_id = 0;
                                    dgvSeaoverCostPrice.Rows[n].Cells["sub_id"].Value = sub_id;

                                    
                                    DataGridViewButtonCell btn = new DataGridViewButtonCell();
                                    btn.Value = "수정";
                                    btn.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    dgvSeaoverCostPrice.Rows[n].Cells["btnCustomUpdate"] = btn;
                                }

                                dgvSeaoverCostPrice.Rows[n].Cells["stock_status"].Value = seaoverPriceDt.Rows[i]["stock_status"].ToString();
                            }
                            this.dgvSeaoverCostPrice.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSeaoverCostPrice_CellContentClick);
                            CalculateSeaoverCostPrice();
                        }
                    }
                }
                //평균원가
                if (final_average_cost > 0 || final_qty > 0)
                    seaover_cost_price = (final_average_cost / final_qty);
                dgvStockSales.Rows[0].Cells["seaover_cost_price"].ToolTipText = "***** AtoNo 'AD,DW,OD,HS' 경우 + 매출원가 + 2.5% 'JD' 경우 + 매출원가 2%, + 연이자 8%% *****\n\n" + tooltip.Trim();
                if (tooltip.Contains("확인필요"))
                    dgvStockSales.Rows[0].Cells["seaover_cost_price"].Style.ForeColor = Color.Red;
            }
        }

        private void SetData(ShortManagerModel smModel, DataGridViewRow select_row)
        {
            //첫줄 추가=============================================================================================
            int n = dgvContract.Rows.Add();
            DataGridViewRow row = dgvContract.Rows[n];
            dgvContract.EndEdit();
            dgvContract.Rows[n].Cells["etd"].Value = "현재고";
            dgvContract.Rows[n].Cells["product_unit"].Value = smModel.unit;
            dgvContract.Rows[n].Cells["warehousing_date"].Value = DateTime.Now.ToString("yyyy-MM-dd");
            dgvContract.Rows[n].Cells["warehouse_qty"].Value = smModel.real_stock.ToString("#,##0");
            double month_around = smModel.real_stock / smModel.avg_sales_day / 21;
            if (double.IsNaN(month_around))
                month_around = 0;

            dgvContract.Rows[n].Cells["month_around"].Value = month_around.ToString("#,##0.00");

            int delivery_day;
            if (!int.TryParse(txtShippingTerm.Text, out delivery_day))
                delivery_day = 0;
            int pending_day;
            if (!int.TryParse(txtPendingTerm.Text, out pending_day))
                pending_day = 0;
            int production_day;
            if (!int.TryParse(txtMakeTerm.Text, out production_day))
                production_day = 0;

            //소진일자
            DateTime exhaust_date = Convert.ToDateTime(smModel.exhaust_date);
            dgvContract.Rows[n].Cells["after_qty_exhausted_date"].Value = exhaust_date.ToString("yyyy-MM-dd");
            //최소ETD
            DateTime etd = exhaust_date.AddDays(-(delivery_day + pending_day));
            dgvContract.Rows[n].Cells["recommend_etd"].Value = etd.ToString("yyyy-MM-dd");
            //추천 계약일자
            DateTime contract_date = exhaust_date.AddDays(-(production_day + delivery_day + pending_day));
            dgvContract.Rows[n].Cells["recommend_contract_date"].Value = contract_date.ToString("yyyy-MM-dd");

            //첫번째 씨오버원가
            double cost_price;
            ReplaceSalesCostToPendingCost(select_row, out cost_price);
            dgvContract.Rows[n].Cells["etd_cost_price"].Value = cost_price.ToString("#,##0");
            //txtSeaoverCostPrice.Text = cost_price.ToString("#,##0");
            dgvStockSales.Rows[0].Cells["seaover_cost_price"].Value = cost_price.ToString("#,##0");
            seaover_average_cost_price = cost_price;
        }
        //미통관내역 가져오기
        private void GetUnpendingData(ShortManagerModel model)
        {
            DataGridViewRow select_row = GetSelectProductDgvr();
            if (select_row == null)
                return;
            string sub_product;
            bool isMerge = false;
            if (select_row.Cells["merge_product"].Value == null || string.IsNullOrEmpty(select_row.Cells["merge_product"].Value.ToString()))
            {
                sub_product = "";
                isMerge = false;
            }
            else
            {
                sub_product = select_row.Cells["merge_product"].Value.ToString();
                isMerge = true;
            }
            //dgvContract.Rows.Clear();
            DataTable unpendingTable = customsRepository.GetUnpendingProduct3(model.product, model.origin, model.sizes, model.unit, sub_product, isMerge, true, cbSimulation.Checked);
            double shipment_qty, shipping_qty;
            if (unpendingTable.Rows.Count > 0)
            {
                for (int i = 0; i < unpendingTable.Rows.Count; i++)
                {
                    double select_unit;
                    if (!double.TryParse(model.unit, out select_unit))
                        select_unit = 0;
                    double unpending_unit;
                    if (!double.TryParse(unpendingTable.Rows[i]["box_weight"].ToString(), out unpending_unit))
                        unpending_unit = 0;

                    int domestic_id;
                    if (!int.TryParse(unpendingTable.Rows[i]["domestic_id"].ToString(), out domestic_id))
                        domestic_id = 0;

                    double cost_unit;
                    if (!double.TryParse(unpendingTable.Rows[i]["cost_unit"].ToString(), out cost_unit))
                        cost_unit = 0;


                    //다를경우만 수정
                    if (select_unit != unpending_unit && domestic_id <= 0)
                    {
                        double qty;
                        if (!double.TryParse(unpendingTable.Rows[i]["quantity_on_paper"].ToString(), out qty))
                            qty = 0;
                        qty = qty * unpending_unit / select_unit;
                        unpendingTable.Rows[i]["quantity_on_paper"] = qty;
                        /*double price;
                        if (!double.TryParse(unpendingTable.Rows[i]["unit_price"].ToString(), out price))
                            price = 0;
                        price = price / unpending_unit * select_unit;
                        unpendingTable.Rows[i]["unit_price"] = price;*/
                        unpendingTable.AcceptChanges();
                    }

                    string bl_no = unpendingTable.Rows[i]["bl_no"].ToString();
                    string warehousing_date = unpendingTable.Rows[i]["warehousing_date"].ToString();
                    //계약만 한 상태
                    if ((string.IsNullOrEmpty(bl_no) || !string.IsNullOrEmpty(bl_no) && string.IsNullOrEmpty(warehousing_date)) || domestic_id > 0)
                    {
                        //재고, 판매량 한눈에===============================================================================================
                        if (dgvStockSales.Rows[0].Cells["shipment_qty"].Value == null || !double.TryParse(dgvStockSales.Rows[0].Cells["shipment_qty"].Value.ToString(), out shipment_qty))
                            shipment_qty = 0;
                        if (dgvStockSales.Rows[0].Cells["shipping_qty"].Value == null || !double.TryParse(dgvStockSales.Rows[0].Cells["shipping_qty"].Value.ToString(), out shipping_qty))
                            shipping_qty = 0;

                        //선적
                        if (domestic_id > 0 || string.IsNullOrEmpty(bl_no))
                            shipment_qty += Convert.ToDouble(unpendingTable.Rows[i]["quantity_on_paper"].ToString());
                        //배송
                        else
                            shipping_qty += Convert.ToDouble(unpendingTable.Rows[i]["quantity_on_paper"].ToString());
                        dgvStockSales.Rows[0].Cells["shipment_qty"].Value = shipment_qty.ToString("#,##0");
                        dgvStockSales.Rows[0].Cells["shipping_qty"].Value = shipping_qty.ToString("#,##0");

                        //통관일정===============================================================================================
                        DateTime warehouse_date;
                        if (domestic_id > 0 && DateTime.TryParse(unpendingTable.Rows[i]["warehousing_date"].ToString(), out warehouse_date))
                        { }
                        else if (DateTime.TryParse(unpendingTable.Rows[i]["eta"].ToString(), out warehouse_date))
                        {
                            int pending_day;
                            if (!int.TryParse(txtPendingTerm.Text, out pending_day))
                                pending_day = 0;

                            //common.GetPlusDay(warehouse_date, pending_day, out warehouse_date);
                            warehouse_date = warehouse_date.AddDays(pending_day);
                        }
                        else if (DateTime.TryParse(unpendingTable.Rows[i]["etd"].ToString(), out warehouse_date))
                        {
                            int shipment_day;
                            if (!int.TryParse(txtShippingTerm.Text, out shipment_day))
                                shipment_day = 0;
                            int pending_day;
                            if (!int.TryParse(txtPendingTerm.Text, out pending_day))
                                pending_day = 0;

                            //common.GetPlusDay(warehouse_date, shipment_day + pending_day, out warehouse_date);
                            warehouse_date = warehouse_date.AddDays(shipment_day + pending_day);
                        }
                        else
                            warehouse_date = DateTime.Now;

                        if (warehouse_date <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                            warehouse_date = DateTime.Now.AddDays(5);

                        //공휴일인지 아닌지 확인
                        warehouse_date = CheckHoliday(warehouse_date);
                        unpendingTable.Rows[i]["warehousing_date"] = warehouse_date.ToString("yyyy-MM-dd");
                        //출력
                        int n = dgvContract.Rows.Add();
                        DataGridViewRow row = dgvContract.Rows[n];
                        dgvContract.Rows[n].Cells["pending_id"].Value = unpendingTable.Rows[i]["id"].ToString();
                        dgvContract.Rows[n].Cells["etd"].Value = unpendingTable.Rows[i]["etd"].ToString();
                        dgvContract.Rows[n].Cells["eta"].Value = unpendingTable.Rows[i]["eta"].ToString();
                        dgvContract.Rows[n].Cells["warehousing_date"].Value = unpendingTable.Rows[i]["warehousing_date"].ToString();
                        dgvContract.Rows[n].Cells["pending_term"].Value = Convert.ToInt16(txtShippingTerm.Text) + Convert.ToInt16(txtPendingTerm.Text);
                        dgvContract.Rows[n].Cells["qty"].Value = Convert.ToDouble(unpendingTable.Rows[i]["quantity_on_paper"].ToString()).ToString("#,##0");
                        dgvContract.Rows[n].Cells["ato_no"].Value = unpendingTable.Rows[i]["ato_no"].ToString();
                        double etd_offer_price;
                        if (!double.TryParse(unpendingTable.Rows[i]["unit_price"].ToString(), out etd_offer_price))
                            etd_offer_price = 0;
                        dgvContract.Rows[n].Cells["etd_offer_price"].Value = etd_offer_price.ToString("#,##0.00");
                        
                        //2023-10-16 추가
                        bool isWeight;
                        if (!bool.TryParse(unpendingTable.Rows[i]["weight_calculate"].ToString(), out isWeight))
                            isWeight = true;
                        dgvContract.Rows[n].Cells["pending_weight_calculate"].Value = isWeight;
                        dgvContract.Rows[n].Cells["pending_cost_unit"].Value = cost_unit;
                        dgvContract.Rows[n].Cells["product_unit"].Value = unpendingTable.Rows[i]["box_weight"].ToString();


                        dgvContract.Rows[n].Cells["domestic_id"].Value = domestic_id;
                        dgvContract.Rows[n].Cells["is_domestic"].Value = Convert.ToBoolean(unpendingTable.Rows[i]["is_domestic"].ToString());
                        if (domestic_id > 0)
                        {
                            double etd_cost_price;
                            if (!double.TryParse(unpendingTable.Rows[i]["cost_price"].ToString(), out etd_cost_price))
                                etd_cost_price = 0;
                            dgvContract.Rows[n].Cells["etd_cost_price"].Value = etd_cost_price.ToString("#,##0");

                            DataGridViewButtonCell cellDelete = new DataGridViewButtonCell();
                            cellDelete.Value = "수정";
                            cellDelete.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            cellDelete.Style.BackColor = Color.Red;

                            dgvContract.Rows[n].Cells["btnUpdate"] = cellDelete;
                        }
                        dgvContract.Rows[n].Cells["shipper"].Value = unpendingTable.Rows[i]["shipper"].ToString();
                    }
                }
            }
        }
        private DateTime CheckHoliday(DateTime dt)
        {
            int[] no1, no2;
            string[] name1, name2;
            DateTime tempDt1 = new DateTime(dt.Year, dt.Month, 1);
            DateTime tempDt2 = new DateTime(dt.Year, dt.Month, 1).AddDays(-1);
            common.getRedDay(tempDt1.Year, tempDt1.Month, out no1, out name1);
            common.getRedDay(tempDt2.Year, tempDt2.Month, out no2, out name2);
            retry:
            //주말일 경우 수정
            if (dt.DayOfWeek == DayOfWeek.Saturday)
            {
                dt = dt.AddDays(2);
            }
            else if (dt.DayOfWeek == DayOfWeek.Sunday)
            {
                dt = dt.AddDays(1);
            }


            //법정공휴일일 경우
            if (dt.Month == tempDt1.Month)
            {
                foreach (int n in no1)
                {
                    if (n == dt.Day)
                    {
                        dt = dt.AddDays(1);
                        goto retry;
                    }
                }
            }
            else
            {
                foreach (int n in no2)
                {
                    if (n == dt.Day)
                    {
                        dt = dt.AddDays(1);
                        goto retry;
                    }
                }
            }
            return dt;
        }

        //정렬하기
        private bool SortSetting()
        {
            if (dgvContract.Rows.Count == 0)
                return false;
            DataTable tb = Libs.Tools.Common.GetDataGridViewAsDataTable2(dgvContract);
            if (tb.Rows.Count > 0)
            {
                tb.Columns.Add("warehousing_date_sort", typeof(DateTime)).SetOrdinal(1);
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    DateTime warehousing_date = new DateTime();
                    if (!DateTime.TryParse(tb.Rows[i]["warehousing_date"].ToString(), out warehousing_date))
                        warehousing_date = new DateTime(2999, 12, 31);
                    tb.Rows[i]["warehousing_date_sort"] = warehousing_date.ToString("yyyy-MM-dd");
                }
                //Sorting
                DataView tv = new DataView(tb);
                tv.Sort = "warehousing_date_sort";
                tb = tv.ToTable();
                tb.AcceptChanges();
                //재출력
                dgvContract.Rows.Clear();
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    int n = dgvContract.Rows.Add();

                    dgvContract.Rows[n].Cells["pending_id"].Value = tb.Rows[i]["pending_id"].ToString();
                    dgvContract.Rows[n].Cells["etd"].Value = tb.Rows[i]["etd"].ToString();
                    dgvContract.Rows[n].Cells["pending_term"].Value = tb.Rows[i]["pending_term"].ToString();
                    dgvContract.Rows[n].Cells["eta"].Value = tb.Rows[i]["eta"].ToString();
                    dgvContract.Rows[n].Cells["warehousing_date"].Value = tb.Rows[i]["warehousing_date"].ToString();
                    dgvContract.Rows[n].Cells["qty"].Value = tb.Rows[i]["qty"].ToString();
                    dgvContract.Rows[n].Cells["warehouse_qty"].Value = tb.Rows[i]["warehouse_qty"].ToString();
                    dgvContract.Rows[n].Cells["exhausted_date"].Value = tb.Rows[i]["exhausted_date"].ToString();
                    dgvContract.Rows[n].Cells["exhausted_day_count"].Value = tb.Rows[i]["exhausted_day_count"].ToString();
                    dgvContract.Rows[n].Cells["after_qty_exhausted_date"].Value = tb.Rows[i]["after_qty_exhausted_date"].ToString();
                    dgvContract.Rows[n].Cells["month_around"].Value = tb.Rows[i]["month_around"].ToString();
                    dgvContract.Rows[n].Cells["recommend_etd"].Value = tb.Rows[i]["recommend_etd"].ToString();
                    dgvContract.Rows[n].Cells["recommend_contract_date"].Value = tb.Rows[i]["recommend_contract_date"].ToString();
                    dgvContract.Rows[n].Cells["ato_no"].Value = tb.Rows[i]["ato_no"].ToString();
                    dgvContract.Rows[n].Cells["pending_weight_calculate"].Value = Convert.ToBoolean(tb.Rows[i]["pending_weight_calculate"].ToString());
                    dgvContract.Rows[n].Cells["pending_cost_unit"].Value = tb.Rows[i]["pending_cost_unit"].ToString();
                    dgvContract.Rows[n].Cells["product_unit"].Value = tb.Rows[i]["product_unit"].ToString();

                    dgvContract.Rows[n].Cells["current_qty"].Value = tb.Rows[i]["current_qty"].ToString();
                    dgvContract.Rows[n].Cells["etd_offer_price"].Value = tb.Rows[i]["etd_offer_price"].ToString();
                    dgvContract.Rows[n].Cells["etd_cost_price"].Value = tb.Rows[i]["etd_cost_price"].ToString();
                    dgvContract.Rows[n].Cells["etd_average_cost_price"].Value = tb.Rows[i]["etd_average_cost_price"].ToString();
                    
                    dgvContract.Rows[n].Cells["btnRegister"].Value = tb.Rows[i]["btnRegister"].ToString();
                    dgvContract.Rows[n].Cells["btnUpdate"].Value = tb.Rows[i]["btnUpdate"].ToString();
                    dgvContract.Rows[n].Cells["domestic_id"].Value = tb.Rows[i]["domestic_id"].ToString();
                    dgvContract.Rows[n].Cells["is_domestic"].Value = Convert.ToBoolean(tb.Rows[i]["is_domestic"].ToString());
                    dgvContract.Rows[n].Cells["shipper"].Value = tb.Rows[i]["shipper"].ToString();
                }
                return true;
            }
            else
                return false;

        }
        private bool CalculateStock(ShortManagerModel model)
        {
            dgvContract.EndEdit();
            double stock = Convert.ToInt32(model.real_stock);
            if (dgvContract.Rows[0].Cells["after_qty_exhausted_date"].Value == null || dgvContract.Rows[0].Cells["after_qty_exhausted_date"].Value.ToString() == "")
                dgvContract.Rows[0].Cells["after_qty_exhausted_date"].Value = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime exhaust_date = Convert.ToDateTime(dgvContract.Rows[0].Cells["after_qty_exhausted_date"].Value.ToString());
            DateTime sttdate = DateTime.Now;
            DataTable tb = Libs.Tools.Common.GetDataGridViewAsDataTable2(dgvContract);
            dgvContract.Rows.Clear();

            double exchange_rate;
            if (!double.TryParse(txtExchangeRate2.Text, out exchange_rate))
                exchange_rate = 0;

            //전체합
            double total_qty = 0;
            double total_cost_price = 0;
            double total_cost_qty = 0;
            double total_shipping_qty = 0;
            double total_shipping_cost_price = 0;
            //재출력
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                int n = dgvContract.Rows.Add();
                //통합 && 단위가 다른 내역 표시
                if (cbAllUnit.Checked && model.unit != tb.Rows[i]["product_unit"].ToString())
                {
                    dgvContract.Rows[n].DefaultCellStyle.BackColor = Color.Linen;
                    for (int j = 0; j < dgvContract.Columns.Count; j++)
                        dgvContract.Rows[i].Cells[j].ToolTipText = "단위 : " + tb.Rows[i]["product_unit"].ToString();
                }

                dgvContract.Rows[n].Cells["pending_id"].Value = tb.Rows[i]["pending_id"].ToString();
                dgvContract.Rows[n].Cells["etd"].Value = tb.Rows[i]["etd"].ToString();
                dgvContract.Rows[n].Cells["eta"].Value = tb.Rows[i]["eta"].ToString();
                dgvContract.Rows[n].Cells["warehousing_date"].Value = tb.Rows[i]["warehousing_date"].ToString();
                dgvContract.Rows[n].Cells["pending_term"].Value = tb.Rows[i]["pending_term"].ToString();
                dgvContract.Rows[n].Cells["ato_no"].Value = tb.Rows[i]["ato_no"].ToString();
                if (tb.Rows[i]["ato_no"].ToString().Contains("AD")
                        || tb.Rows[i]["ato_no"].ToString().Contains("DW")
                        || tb.Rows[i]["ato_no"].ToString().Contains("OD")
                        || tb.Rows[i]["ato_no"].ToString().Contains("JD")
                        || tb.Rows[i]["ato_no"].ToString().Contains("HS"))
                    dgvContract.Rows[n].Cells["ato_no"].Value = tb.Rows[i]["ato_no"].ToString() + "(대)";

                /*dgvContract.Rows[n].Cells["contract_no"].Value = tb.Rows[i]["contract_no"].ToString();
                dgvContract.Rows[n].Cells["bl_no"].Value = tb.Rows[i]["bl_no"].ToString();*/

                DateTime warehousing_date = new DateTime();
                if (DateTime.TryParse(tb.Rows[i]["warehousing_date"].ToString(), out warehousing_date))
                {
                    //입고일자 계산
                    dgvContract.Rows[n].Cells["warehousing_date"].Value = warehousing_date.ToString("yyyy-MM-dd");
                    //소진일자 계산, 입고후 재고 계산
                    if (exhaust_date > warehousing_date)
                        common.GetStock(sttdate, warehousing_date.AddDays(-1), model.avg_sales_day, stock, out stock);
                    else
                    {
                        stock = 0;
                        int Days = 0;
                        common.GetWorkDay(exhaust_date, warehousing_date.AddDays(-1), out Days);
                        //2023-06-14 일수 / 쇼트갯수   
                        string txtDays = Days.ToString("#,##0").PadRight(5, ' ');
                        string txtCount = (Days * model.avg_sales_day).ToString("#,##0").PadLeft(5, ' ');
                        dgvContract.Rows[n].Cells["exhausted_day_count"].Value = txtDays + "/" + txtCount;
                        if (Days > 0)
                            dgvContract.Rows[n].Cells["exhausted_date"].Value = exhaust_date.ToString("yyyy-MM-dd") + " ~ " + warehousing_date.AddDays(-1).ToString("yyyy-MM-dd");
                    }
                    //입고수량
                    int qty;
                    if (tb.Rows[i]["qty"] == null || !int.TryParse(tb.Rows[i]["qty"].ToString().Replace(",", ""), out qty))
                        qty = 0;
                    //입고반영
                    stock += qty;
                    total_qty += qty;
                    //수량, 재고
                    dgvContract.Rows[n].Cells["qty"].Value = qty.ToString("#,##0");
                    dgvContract.Rows[n].Cells["warehouse_qty"].Value = stock.ToString("#,##0");
                    //회전율
                    dgvContract.Rows[n].Cells["month_around"].Value = ((stock / model.avg_sales_day) / 21).ToString("#,##0.00");
                    //소진일자 계산
                    sttdate = warehousing_date;
                    //그날 판매량 제외
                    double exhausted_cnt = 0;
                    //stock -= model.avg_sales_day;
                    int standard;
                    if (!int.TryParse(txtStandardOfShort.Text, out standard))
                        standard = 0;
                    common.GetExhausedDateDayd(sttdate, stock, model.avg_sales_day, standard, null, out exhaust_date, out exhausted_cnt);

                    if (exhaust_date.Year != 1900)
                    {
                        dgvContract.Rows[n].Cells["after_qty_exhausted_date"].Value = exhaust_date.ToString("yyyy-MM-dd");

                        int shipping_days;
                        if (!int.TryParse(txtShippingTerm.Text, out shipping_days))
                            shipping_days = 0;
                        int pending_days;
                        if (!int.TryParse(txtPendingTerm.Text, out pending_days))
                            pending_days = 0;
                        int production_days;
                        if (!int.TryParse(txtMakeTerm.Text, out production_days))
                            production_days = 0;
                        //최소ETD
                        DateTime tmp_etd = exhaust_date.AddDays(-(shipping_days + pending_days));
                        dgvContract.Rows[n].Cells["recommend_etd"].Value = tmp_etd.ToString("yyyy-MM-dd");
                        //추천 계약일자
                        DateTime contract_date = exhaust_date.AddDays(-(production_days + shipping_days + pending_days));
                        dgvContract.Rows[n].Cells["recommend_contract_date"].Value = contract_date.ToString("yyyy-MM-dd");
                    }
                }
                dgvContract.Rows[n].Cells["ato_no"].Value = tb.Rows[i]["ato_no"].ToString();
                //원가, 평균계산
                dgvContract.Rows[n].Cells["current_qty"].Value = stock.ToString("#,##0");
                //첫번째 행은 현재고라 변경사항없이 출력
                if (i == 0)
                {
                    dgvContract.Rows[n].Cells["etd_offer_price"].Value = tb.Rows[i]["etd_offer_price"].ToString();
                    dgvContract.Rows[n].Cells["etd_cost_price"].Value = tb.Rows[i]["etd_cost_price"].ToString();
                    dgvContract.Rows[n].Cells["etd_average_cost_price"].Value = tb.Rows[i]["etd_cost_price"].ToString();

                    //전체 평균원가
                    double cost_price;
                    if (!double.TryParse(tb.Rows[i]["etd_cost_price"].ToString(), out cost_price))
                        cost_price = 0;
                    double current_qty;
                    if (!double.TryParse(tb.Rows[i]["warehouse_qty"].ToString(), out current_qty))
                        current_qty = 0;

                    if (current_qty > 0)
                    {
                        total_cost_price += (cost_price * current_qty);
                        total_cost_qty += current_qty;
                    }
                }
                //두번째 행부터 원가계산, 평균원가 계산
                else
                {
                    //쇼트나기전 까지 판매가능한 수량
                    DateTime pre_exhaust_date;
                    if (dgvContract.Rows[n - 1].Cells["after_qty_exhausted_date"].Value != null 
                        && DateTime.TryParse(dgvContract.Rows[n - 1].Cells["after_qty_exhausted_date"].Value.ToString(), out pre_exhaust_date))
                    {
                        if (pre_exhaust_date > warehousing_date)
                        {
                            double pre_warehouse_qty;
                            if (!double.TryParse(tb.Rows[i - 1]["warehouse_qty"].ToString(), out pre_warehouse_qty))
                                pre_warehouse_qty = 0;

                            int enable_sale_day;
                            common.GetWorkDay(warehousing_date, pre_exhaust_date, out enable_sale_day);
                            if (--enable_sale_day < 0)
                                enable_sale_day = 0;
                            double enable_sale_qty = (model.avg_sales_day * enable_sale_day);
                            if (enable_sale_qty < 0)
                                enable_sale_qty = 0;
                            dgvContract.Rows[i - 1].Cells["enable_sale_qty"].Value = enable_sale_qty.ToString("#,##0");
                        }
                    }

                    double etd_offer_price;
                    if (!double.TryParse(tb.Rows[i]["etd_offer_price"].ToString(), out etd_offer_price))
                        etd_offer_price = 0;
                    dgvContract.Rows[n].Cells["etd_offer_price"].Value = etd_offer_price;

                    bool isWeight;
                    if (!bool.TryParse(tb.Rows[i]["pending_weight_calculate"].ToString(), out isWeight))
                        isWeight = true;
                    dgvContract.Rows[n].Cells["pending_weight_calculate"].Value = isWeight;

                    dgvContract.Rows[n].Cells["product_unit"].Value = tb.Rows[i]["product_unit"].ToString();
                    dgvContract.Rows[n].Cells["pending_cost_unit"].Value = tb.Rows[i]["pending_cost_unit"].ToString();

                    double cost_price;
                    if (!double.TryParse(tb.Rows[i]["etd_cost_price"].ToString(), out cost_price))
                        cost_price = 0;

                    bool trq;
                    if (!bool.TryParse(tb.Rows[i]["trq"].ToString(), out trq))
                        trq = false;

                    double pending_cost_unit;
                    if (!double.TryParse(tb.Rows[i]["pending_cost_unit"].ToString(), out pending_cost_unit))
                        pending_cost_unit = 0;

                    //2024-01-03 선적원가도 똑같이하는데 냉장료만 빼고 
                    if (tb.Rows[i]["ato_no"].ToString().Contains("AD")
                        || tb.Rows[i]["ato_no"].ToString().Contains("DW")
                        || tb.Rows[i]["ato_no"].ToString().Contains("OD")
                        || tb.Rows[i]["ato_no"].ToString().Contains("HS"))
                    {
                        cost_price = CalculateEtdCostPrice(tb.Rows[i]["etd"].ToString(), etd_offer_price, isWeight, trq, 0, pending_cost_unit);
                        double agent_cost_price = CalculateEtdCostPrice(tb.Rows[i]["etd"].ToString(), etd_offer_price * 1.025, isWeight, trq, 0, pending_cost_unit);
                        dgvContract.Rows[n].Cells["etd_cost_price"].ToolTipText = "'AD, DW, OD, HS' 대행건 매출단가 + 2.5%" + $"\n * 수수료 : {(agent_cost_price - cost_price).ToString("#,##0")}";
                        dgvContract.Rows[n].Cells["etd_cost_price"].Value = agent_cost_price.ToString("#,##0");
                        cost_price = agent_cost_price;
                    }
                    else if (tb.Rows[i]["ato_no"].ToString().Contains("JD"))
                    {
                        cost_price = CalculateEtdCostPrice(tb.Rows[i]["etd"].ToString(), etd_offer_price, isWeight, trq, 0, pending_cost_unit);
                        double agent_cost_price = CalculateEtdCostPrice(tb.Rows[i]["etd"].ToString(), etd_offer_price * 1.02, isWeight, trq, 0, pending_cost_unit);
                        dgvContract.Rows[n].Cells["etd_cost_price"].ToolTipText = "'JD' 대행건 매출단가 + 2%" + $"\n * 수수료 : {(agent_cost_price - cost_price).ToString("#,##0")}";
                        dgvContract.Rows[n].Cells["etd_cost_price"].Value = agent_cost_price.ToString("#,##0");
                        cost_price = agent_cost_price;
                    }
                    //대행X
                    else
                    {
                        cost_price = CalculateEtdCostPrice(tb.Rows[i]["etd"].ToString(), etd_offer_price, isWeight, trq, 0, pending_cost_unit);
                        dgvContract.Rows[n].Cells["etd_cost_price"].Value = cost_price.ToString("#,##0");
                    }

                    double current_qty;
                    if (dgvContract.Rows[n].Cells["qty"].Value == null || !double.TryParse(dgvContract.Rows[n].Cells["qty"].Value.ToString(), out current_qty))
                        current_qty = 0;

                    double before_cost_price;
                    if (dgvContract.Rows[n - 1].Cells["etd_cost_price"].Value == null || !double.TryParse(dgvContract.Rows[n - 1].Cells["etd_cost_price"].Value.ToString(), out before_cost_price))
                        before_cost_price = 0;
                    double before_qty;
                    if (dgvContract.Rows[n - 1].Cells["current_qty"].Value == null || !double.TryParse(dgvContract.Rows[n - 1].Cells["current_qty"].Value.ToString(), out before_qty))
                        before_qty = 0;

                    //평균원가
                    total_cost_price += (cost_price * current_qty);
                    total_cost_qty += current_qty;

                    double etd_average_cost_price;
                    if (before_qty == 0 || before_cost_price == 0)
                        etd_average_cost_price = cost_price;
                    else if (current_qty == 0 || cost_price == 0)
                        etd_average_cost_price = total_cost_price / total_cost_qty;
                    else
                        etd_average_cost_price = total_cost_price / total_cost_qty;

                    dgvContract.Rows[n].Cells["etd_average_cost_price"].Value = (total_cost_price / total_cost_qty).ToString("#,##0");
                    //선적 원가
                    if (cost_price > 0 && current_qty > 0)
                    {
                        total_shipping_cost_price += cost_price * current_qty;
                        total_shipping_qty += current_qty;
                    }
                    //거래처
                    dgvContract.Rows[n].Cells["shipper"].Value = tb.Rows[i]["shipper"].ToString();
                    dgvContract.Rows[n].Cells["ato_no"].ToolTipText = tb.Rows[i]["shipper"].ToString();

                    //국내수입
                    dgvContract.Rows[n].Cells["domestic_id"].Value = tb.Rows[i]["domestic_id"].ToString();
                    bool is_domestic = Convert.ToBoolean(tb.Rows[i]["is_domestic"].ToString());
                    dgvContract.Rows[n].Cells["is_domestic"].Value = is_domestic;

                    //등록, 수정, 삭제버튼
                    if (is_domestic)
                    {
                        dgvContract.Rows[n].Cells["warehousing_date"].Style.BackColor = Color.PeachPuff;
                        dgvContract.Rows[n].Cells["qty"].Style.BackColor = Color.PeachPuff;
                        dgvContract.Rows[n].Cells["etd_cost_price"].Style.BackColor = Color.PeachPuff;
                        //등록
                        /*DataGridViewCheckBoxCell registerCell = new DataGridViewCheckBoxCell();
                        registerCell.Value = false;
                        dgvContract.Rows[n].Cells["btnRegister"] = registerCell;*/
                        DataGridViewButtonCell registerCell = new DataGridViewButtonCell();
                        registerCell.Value = "등록";
                        registerCell.Style.BackColor = Color.Blue;
                        dgvContract.Rows[n].Cells["btnRegister"] = registerCell;
                        //수정
                        DataGridViewButtonCell updateCell = new DataGridViewButtonCell();
                        updateCell.Value = "상세";
                        updateCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        updateCell.Style.BackColor = Color.Red;
                        dgvContract.Rows[n].Cells["btnUpdate"] = updateCell;
                        //삭제
                        /*DataGridViewCheckBoxCell deleteCell = new DataGridViewCheckBoxCell();
                        deleteCell.Value = false;
                        dgvContract.Rows[n].Cells["isDelete"] = deleteCell;*/
                        DataGridViewButtonCell deleteCell = new DataGridViewButtonCell();
                        deleteCell.Value = "삭제";
                        deleteCell.Style.BackColor = Color.Red;
                        dgvContract.Rows[n].Cells["isDelete"] = deleteCell;
                    }
                }
            }

            //총합
            int m;
            if (dgvContract.Rows[dgvContract.RowCount - 1].Cells["warehousing_date"].Value == null || string.IsNullOrEmpty(dgvContract.Rows[dgvContract.RowCount - 1].Cells["warehousing_date"].Value.ToString()))
                m = dgvContract.RowCount - 1;
            else
                m = dgvContract.Rows.Add();
            dgvContract.Rows[m].DefaultCellStyle.BackColor = Color.LightGray;
            dgvContract.Rows[m].DefaultCellStyle.Font = new Font("나눔고딕", 10, FontStyle.Bold);
            dgvContract.Rows[m].Cells["qty"].Value = total_qty.ToString("#,##0");
            dgvContract.Rows[m].Cells["etd_average_cost_price"].Value = (total_cost_price / total_cost_qty).ToString("#,##0");
            dgvStockSales.Rows[0].Cells["total_cost_price"].Value = (total_cost_price / total_cost_qty).ToString("#,##0");
            

            double pending_cost_price = total_shipping_cost_price / total_shipping_qty;
            if (double.IsNaN(pending_cost_price))
                pending_cost_price = 0;
            //txtPendingCostPrice.Text = pending_cost_price.ToString("#,##0");
            dgvStockSales.Rows[0].Cells["pending_cost_price"].Value = pending_cost_price.ToString("#,##0");

            SetDashboardSummary();

            return true;
        }
        private bool CalculateStock()
        {
            dgvContract.EndEdit();
            if (dgvContract.Rows.Count > 0)
            {
                double stock;
                if (dgvContract.Rows[0].Cells["warehouse_qty"].Value == null || !double.TryParse(dgvContract.Rows[0].Cells["warehouse_qty"].Value.ToString(), out stock))
                    stock = 0;
                DateTime exhaust_date;
                if (dgvContract.Rows[0].Cells["after_qty_exhausted_date"].Value == null || !DateTime.TryParse(dgvContract.Rows[0].Cells["after_qty_exhausted_date"].Value.ToString(), out exhaust_date))
                    exhaust_date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                dgvContract.Rows[0].Cells["after_qty_exhausted_date"].Value = exhaust_date.ToString("yyyy-MM-dd");

                DateTime sttdate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                DataTable tb = Libs.Tools.Common.GetDataGridViewAsDataTable2(dgvContract);
                dgvContract.Rows.Clear();

                //전체합
                double total_qty = 0;
                double total_cost_price = 0;
                double total_cost_qty = 0;
                double total_shipping_qty = 0;
                double total_shipping_cost_price = 0;
                //재출력
                for (int i = 0; i < tb.Rows.Count - 1; i++)
                {
                    int n = dgvContract.Rows.Add();

                    dgvContract.Rows[n].Cells["pending_id"].Value = tb.Rows[i]["pending_id"].ToString();
                    dgvContract.Rows[n].Cells["etd"].Value = common.strDatetime(tb.Rows[i]["etd"].ToString());
                    dgvContract.Rows[n].Cells["eta"].Value = tb.Rows[i]["eta"].ToString();
                    dgvContract.Rows[n].Cells["warehousing_date"].Value = tb.Rows[i]["warehousing_date"].ToString();
                    dgvContract.Rows[n].Cells["pending_term"].Value = tb.Rows[i]["pending_term"].ToString();
                    dgvContract.Rows[n].Cells["ato_no"].Value = tb.Rows[i]["ato_no"].ToString();
                    if (tb.Rows[i]["ato_no"].ToString().Contains("AD")
                        || tb.Rows[i]["ato_no"].ToString().Contains("DW")
                        || tb.Rows[i]["ato_no"].ToString().Contains("OD")
                        || tb.Rows[i]["ato_no"].ToString().Contains("JD")
                        || tb.Rows[i]["ato_no"].ToString().Contains("HS"))
                        dgvContract.Rows[n].Cells["ato_no"].Value = tb.Rows[i]["ato_no"].ToString() + "(대)";
                    dgvContract.Rows[n].Cells["trq"].Value = tb.Rows[i]["trq"].ToString();
                    /*dgvContract.Rows[n].Cells["contract_no"].Value = tb.Rows[i]["contract_no"].ToString();
                    dgvContract.Rows[n].Cells["bl_no"].Value = tb.Rows[i]["bl_no"].ToString();*/

                    double avg_sales_day;
                    if (dgvStockSales.Rows[0].Cells["day_sales_qty_double"].Value == null || !double.TryParse(dgvStockSales.Rows[0].Cells["day_sales_qty_double"].Value.ToString(), out avg_sales_day))
                        avg_sales_day = 0;

                    DateTime warehousing_date = new DateTime();
                    if (DateTime.TryParse(tb.Rows[i]["warehousing_date"].ToString(), out warehousing_date))
                    {
                        //입고일자 계산
                        dgvContract.Rows[n].Cells["warehousing_date"].Value = warehousing_date.ToString("yyyy-MM-dd");
                        //소진일자 계산, 입고후 재고 계산
                        if (exhaust_date > warehousing_date)
                            common.GetStock(sttdate, warehousing_date.AddDays(-1), avg_sales_day, stock, out stock);
                        else
                        {
                            stock = 0;
                            int Days = 0;
                            common.GetWorkDay(exhaust_date, warehousing_date.AddDays(-1), out Days);
                            //2023-06-14 일수 / 쇼트갯수   
                            string txtDays = Days.ToString("#,##0").PadRight(5, ' ');
                            string txtCount = (Days * avg_sales_day).ToString("#,##0").PadLeft(5, ' ');
                            dgvContract.Rows[n].Cells["exhausted_day_count"].Value = txtDays + "/" + txtCount;
                            if (Days > 0)
                                dgvContract.Rows[n].Cells["exhausted_date"].Value = exhaust_date.ToString("yyyy-MM-dd") + " ~ " + warehousing_date.AddDays(-1).ToString("yyyy-MM-dd");
                        }
                        //입고수량
                        int qty;
                        if (tb.Rows[i]["qty"] == null || !int.TryParse(tb.Rows[i]["qty"].ToString().Replace(",", ""), out qty))
                            qty = 0;
                        //입고반영
                        stock += qty;
                        total_qty += qty;
                        //수량, 재고
                        dgvContract.Rows[n].Cells["qty"].Value = qty.ToString("#,##0");
                        dgvContract.Rows[n].Cells["warehouse_qty"].Value = stock.ToString("#,##0");
                        //회전율
                        dgvContract.Rows[n].Cells["month_around"].Value = ((stock / avg_sales_day) / 21).ToString("#,##0.00");
                        //소진일자 계산
                        sttdate = warehousing_date;
                        //그날 판매량 제외
                        double exhausted_cnt = 0;
                        //stock -= model.avg_sales_day;
                        int standard;
                        if (!int.TryParse(txtStandardOfShort.Text, out standard))
                            standard = 0;
                        common.GetExhausedDateDayd(sttdate, stock, avg_sales_day, standard, null, out exhaust_date, out exhausted_cnt);
                        if (exhaust_date.Year != 1900)
                        {
                            dgvContract.Rows[n].Cells["after_qty_exhausted_date"].Value = exhaust_date.ToString("yyyy-MM-dd");

                            int shipping_days;
                            if (!int.TryParse(txtShippingTerm.Text, out shipping_days))
                                shipping_days = 0;
                            int pending_days;
                            if (!int.TryParse(txtPendingTerm.Text, out pending_days))
                                pending_days = 0;
                            int production_days;
                            if (!int.TryParse(txtMakeTerm.Text, out production_days))
                                production_days = 0;
                            //최소ETD
                            DateTime tmp_etd = exhaust_date.AddDays(-(shipping_days + pending_days));
                            dgvContract.Rows[n].Cells["recommend_etd"].Value = tmp_etd.ToString("yyyy-MM-dd");
                            //추천 계약일자
                            DateTime contract_date = exhaust_date.AddDays(-(production_days + shipping_days + pending_days));
                            dgvContract.Rows[n].Cells["recommend_contract_date"].Value = contract_date.ToString("yyyy-MM-dd");
                        }
                    }
                    dgvContract.Rows[n].Cells["ato_no"].Value = tb.Rows[i]["ato_no"].ToString();
                    //원가, 평균계산

                    dgvContract.Rows[n].Cells["current_qty"].Value = stock.ToString("#,##0");
                    //첫번째 행은 현재고라 변경사항없이 출력
                    if (i == 0)
                    {
                        dgvContract.Rows[n].Cells["etd_offer_price"].Value = tb.Rows[i]["etd_offer_price"].ToString();
                        dgvContract.Rows[n].Cells["etd_cost_price"].Value = tb.Rows[i]["etd_cost_price"].ToString();
                        dgvContract.Rows[n].Cells["etd_cost_price"].ToolTipText = dgvStockSales.Rows[0].Cells["seaover_cost_price"].ToolTipText;
                        dgvContract.Rows[n].Cells["etd_average_cost_price"].Value = tb.Rows[i]["etd_cost_price"].ToString();

                        //전체 평균원가
                        double cost_price;
                        if (!double.TryParse(tb.Rows[i]["etd_cost_price"].ToString(), out cost_price))
                            cost_price = 0;
                        double current_qty;
                        if (!double.TryParse(tb.Rows[i]["warehouse_qty"].ToString(), out current_qty))
                            current_qty = 0;

                        if (current_qty > 0)
                        {
                            total_cost_price += cost_price * current_qty;
                            total_cost_qty += current_qty;
                        }
                    }
                    //두번째 행부터 원가계산, 평균원가 계산
                    else
                    {
                        //쇼트나기전 까지 판매가능한 수량
                        DateTime pre_exhaust_date;
                        if (DateTime.TryParse(tb.Rows[i - 1]["after_qty_exhausted_date"].ToString(), out pre_exhaust_date))
                        {
                            if (pre_exhaust_date > warehousing_date)
                            {
                                double pre_warehouse_qty;
                                if (!double.TryParse(tb.Rows[i - 1]["warehouse_qty"].ToString(), out pre_warehouse_qty))
                                    pre_warehouse_qty = 0;

                                int enable_sale_day;
                                common.GetWorkDay(warehousing_date, pre_exhaust_date, out enable_sale_day);
                                if (--enable_sale_day < 0)
                                    enable_sale_day = 0;
                                double enable_sale_qty = (avg_sales_day * enable_sale_day);
                                if (enable_sale_qty < 0)
                                    enable_sale_qty = 0;
                                dgvContract.Rows[i - 1].Cells["enable_sale_qty"].Value = enable_sale_qty.ToString("#,##0");
                            }
                        }
                        double etd_offer_price;
                        if (!double.TryParse(tb.Rows[i]["etd_offer_price"].ToString(), out etd_offer_price))
                            etd_offer_price = 0;
                        dgvContract.Rows[n].Cells["etd_offer_price"].Value = etd_offer_price;
                        bool isWeight;
                        if (!bool.TryParse(tb.Rows[i]["pending_weight_calculate"].ToString(), out isWeight))
                            isWeight = true;
                        dgvContract.Rows[n].Cells["pending_weight_calculate"].Value = isWeight;
                        dgvContract.Rows[n].Cells["product_unit"].Value = tb.Rows[i]["product_unit"].ToString();
                        dgvContract.Rows[n].Cells["pending_cost_unit"].Value = tb.Rows[i]["pending_cost_unit"].ToString();

                        bool trq;
                        if (!bool.TryParse(tb.Rows[i]["trq"].ToString(), out trq))
                            trq = false;
                        //if (cost_price == 0)
                        double cost_price;
                        /*if (!double.TryParse(tb.Rows[i]["etd_cost_price"].ToString(), out cost_price))
                            cost_price = 0;
                        cost_price = CalculateEtdCostPrice(etd_offer_price, isWeight, trq);*/


                        double cost_unit;
                        if (!double.TryParse(tb.Rows[i]["pending_cost_unit"].ToString(), out cost_unit))
                            cost_unit = 0;
                        if (cost_unit == 0)
                            cost_unit = 1;

                        //2024-01-03 대행일 경우 매출단가 + 2.0% OR 2.5%
                        if (tb.Rows[i]["ato_no"].ToString().Contains("AD")
                            || tb.Rows[i]["ato_no"].ToString().Contains("DW")
                            || tb.Rows[i]["ato_no"].ToString().Contains("OD")
                            || tb.Rows[i]["ato_no"].ToString().Contains("HS"))
                        {
                            cost_price = CalculateEtdCostPrice(tb.Rows[i]["etd"].ToString(), etd_offer_price, isWeight, trq, 0, cost_unit);
                            double agent_cost_price = CalculateEtdCostPrice(tb.Rows[i]["etd"].ToString(), etd_offer_price * 1.025, isWeight, trq, 0, cost_unit);
                            dgvContract.Rows[n].Cells["etd_cost_price"].ToolTipText = "'AD, DW, OD, HS' 대행건 매출단가 + 2.5%" + $"\n * 수수료 : {(agent_cost_price - cost_price).ToString("#,##0")}";
                            dgvContract.Rows[n].Cells["etd_cost_price"].Value = agent_cost_price.ToString("#,##0");

                            cost_price = agent_cost_price;
                        }
                        else if (tb.Rows[i]["ato_no"].ToString().Contains("JD"))
                        {
                            cost_price = CalculateEtdCostPrice(tb.Rows[i]["etd"].ToString(), etd_offer_price, isWeight, trq, 0, cost_unit);
                            double agent_cost_price = CalculateEtdCostPrice(tb.Rows[i]["etd"].ToString(), etd_offer_price * 1.02, isWeight, trq, 0, cost_unit);
                            dgvContract.Rows[n].Cells["etd_cost_price"].ToolTipText = "'JD' 대행건 매출단가 + 2%" + $"\n * 수수료 : {(agent_cost_price - cost_price).ToString("#,##0")}";
                            dgvContract.Rows[n].Cells["etd_cost_price"].Value = agent_cost_price.ToString("#,##0");

                            cost_price = agent_cost_price;
                        }
                        //대행X
                        else
                        {
                            cost_price = CalculateEtdCostPrice(tb.Rows[i]["etd"].ToString(), etd_offer_price, isWeight, trq, 0, cost_unit);
                            dgvContract.Rows[n].Cells["etd_cost_price"].Value = cost_price.ToString("#,##0");
                        }



                        //dgvContract.Rows[n].Cells["etd_cost_price"].Value = cost_price.ToString("#,##0");
                        double current_qty;
                        if (dgvContract.Rows[n].Cells["qty"].Value == null || !double.TryParse(dgvContract.Rows[n].Cells["qty"].Value.ToString(), out current_qty))
                            current_qty = 0;
                        double before_cost_price;
                        if (dgvContract.Rows[n - 1].Cells["etd_cost_price"].Value == null || !double.TryParse(dgvContract.Rows[n - 1].Cells["etd_cost_price"].Value.ToString(), out before_cost_price))
                            before_cost_price = 0;
                        double before_qty;
                        if (dgvContract.Rows[n - 1].Cells["current_qty"].Value == null || !double.TryParse(dgvContract.Rows[n - 1].Cells["current_qty"].Value.ToString(), out before_qty))
                            before_qty = 0;

                        //평균원가
                        total_cost_price += cost_price * current_qty;
                        total_cost_qty += current_qty;
                        double etd_average_cost_price;
                        if (before_qty == 0 || before_cost_price == 0)
                            etd_average_cost_price = cost_price;
                        else if (current_qty == 0 || cost_price == 0)
                            etd_average_cost_price = total_cost_price / total_cost_qty;
                        else
                            etd_average_cost_price = total_cost_price / total_cost_qty;

                        dgvContract.Rows[n].Cells["etd_average_cost_price"].Value = etd_average_cost_price.ToString("#,##0");

                        //선적 원가
                        if (cost_price > 0 && current_qty > 0)
                        {
                            total_shipping_cost_price += cost_price * current_qty;
                            total_shipping_qty += current_qty;
                        }
                        //국내수입
                        dgvContract.Rows[n].Cells["domestic_id"].Value = tb.Rows[i]["domestic_id"].ToString();
                        bool is_domestic = Convert.ToBoolean(tb.Rows[i]["is_domestic"].ToString());
                        dgvContract.Rows[n].Cells["is_domestic"].Value = is_domestic;

                        //등록, 수정, 삭제버튼
                        if (is_domestic)
                        {
                            dgvContract.Rows[n].Cells["warehousing_date"].Style.BackColor = Color.PeachPuff;
                            dgvContract.Rows[n].Cells["qty"].Style.BackColor = Color.PeachPuff;
                            dgvContract.Rows[n].Cells["etd_cost_price"].Style.BackColor = Color.PeachPuff;
                            //등록
                            /*DataGridViewCheckBoxCell registerCell = new DataGridViewCheckBoxCell();
                            registerCell.Value = false;
                            dgvContract.Rows[n].Cells["btnRegister"] = registerCell;*/
                            DataGridViewButtonCell registerCell = new DataGridViewButtonCell();
                            registerCell.Value = "등록";
                            registerCell.Style.BackColor = Color.Blue;
                            dgvContract.Rows[n].Cells["btnRegister"] = registerCell;
                            //수정
                            DataGridViewButtonCell updateCell = new DataGridViewButtonCell();
                            updateCell.Value = "상세";
                            updateCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            updateCell.Style.BackColor = Color.Red;
                            dgvContract.Rows[n].Cells["btnUpdate"] = updateCell;
                            //삭제
                            /*DataGridViewCheckBoxCell deleteCell = new DataGridViewCheckBoxCell();
                            deleteCell.Value = false;
                            dgvContract.Rows[n].Cells["isDelete"] = deleteCell;*/
                            DataGridViewButtonCell deleteCell = new DataGridViewButtonCell();
                            deleteCell.Value = "삭제";
                            deleteCell.Style.BackColor = Color.Red;
                            dgvContract.Rows[n].Cells["isDelete"] = deleteCell;
                        }
                    }
                }

                //총합
                int m = dgvContract.Rows.Add();
                dgvContract.Rows[m].DefaultCellStyle.BackColor = Color.LightGray;
                dgvContract.Rows[m].DefaultCellStyle.Font = new Font("나눔고딕", 10, FontStyle.Bold);
                dgvContract.Rows[m].Cells["qty"].Value = total_qty.ToString("#,##0");
                total_average_cost_price = (total_cost_price / total_cost_qty);
                dgvContract.Rows[m].Cells["etd_average_cost_price"].Value = total_average_cost_price.ToString("#,##0");
                double pending_cost_price = total_shipping_cost_price / total_shipping_qty;
                if (double.IsNaN(pending_cost_price))
                    pending_cost_price = 0;
                //txtPendingCostPrice.Text = pending_cost_price.ToString("#,##0");
                dgvStockSales.Rows[0].Cells["pending_cost_price"].Value = pending_cost_price.ToString("#,##0");
                dgvStockSales.Rows[0].Cells["total_cost_price"].Value = total_average_cost_price.ToString("#,##0");

                SetDashboardSummary();
            }
           
            return true;
        }
        private double CalculateEtdCostPrice(string etd, double offer_price, bool weight_calculate, bool trq, double exchangeRate = 0, double cost_unit = 0)
        { 
            double cost_price = 0;
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
            {
                double seaover_unit;
                if (row.Cells["seaover_unit"].Value == null || !double.TryParse(row.Cells["seaover_unit"].Value.ToString(), out seaover_unit))
                    seaover_unit = 0;
                if (cost_unit == 0)
                {
                    if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                        cost_unit = 0;
                }
                double unit_count;
                if (row.Cells["unit_count"].Value == null || !double.TryParse(row.Cells["unit_count"].Value.ToString(), out unit_count))
                    unit_count = 0;
                if (unit_count == 0)
                    unit_count = 1;

                double unit;
                
                if (weight_calculate)
                    unit = seaover_unit;
                else
                    unit = cost_unit;


                double exchange_rate;
                if (exchangeRate > 0)
                    exchange_rate = exchangeRate;
                else
                {
                    if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                        exchange_rate = 0;
                }
                /*bool weight_calculate;
                if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                    weight_calculate = true;*/
                if (!weight_calculate && cost_unit <= 0)
                    weight_calculate = true;

                double custom;
                if (!double.TryParse(txtCustom.Text, out custom))
                    custom = 0;
                custom /= 100;
                double tax;
                if (!double.TryParse(txtTax.Text, out tax))
                    tax = 0;
                tax /= 100;
                double incidental_expense;
                if (!double.TryParse(txtIncidentalExpense.Text, out incidental_expense))
                    incidental_expense = 0;
                incidental_expense /= 100;

                //2023-10-16 계산방법 txt
                if (trq)
                {
                    int trq_price;
                    if (!int.TryParse(txtTrq.Text.Replace(",", ""), out trq_price))
                        trq_price = 0;
                    cost_price = unit * offer_price;
                    //cost_price *= (1 + tax + incidental_expense);
                    cost_price *= (tax + 1);
                    cost_price *= (incidental_expense + 1);

                    cost_price *= exchange_rate;
                    cost_price += (trq_price * seaover_unit);
                    cost_price /= unit_count;


                    //calculate_txt = "(" + offer_price + " x " + unit + " x " + (1 + tax + incidental_expense) + " x " + exchange_rate + ") + (" + trq_price + seaover_unit + ")"

                }
                else
                {
                    cost_price = unit * offer_price;
                    //cost_price *= (1 + tax + custom + incidental_expense);
                    //과세는 관세를 포함, 부대비용은 과세까지 포함
                    cost_price *= (custom + 1);
                    cost_price *= (tax + 1);
                    cost_price *= (incidental_expense + 1);
                    cost_price *= exchange_rate;
                    cost_price /= unit_count;
                }

                /*if (trq)
                {
                    int trq_price;
                    if (!int.TryParse(txtTrq.Text.Replace(",", ""), out trq_price))
                        trq_price = 0;

                    else if (weight_calculate)
                        cost_price = (offer_price * seaover_unit * (1 + tax + incidental_expense) * exchange_rate) + (seaover_unit * trq_price);
                    else
                    {
                        if (cbMultiplyTray.Checked)
                            cost_price = (offer_price * cost_unit * (1 + tax + incidental_expense) * exchange_rate) + (seaover_unit * trq_price);
                        else
                            cost_price = (offer_price * (1 + custom + tax + incidental_expense) * exchange_rate) + (seaover_unit * trq_price);
                    }
                }
                else if (weight_calculate)
                    cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate * seaover_unit;
                else
                {
                    if(cbMultiplyTray.Checked)
                        cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate * cost_unit;
                    else
                        cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate;
                }*/
            }
            //연이자 추가
            double interest = 0;
            DateTime etd_dt;
            if (DateTime.TryParse(etd, out etd_dt))
            {
                TimeSpan ts = DateTime.Now - etd_dt;
                int days = ts.Days;
                interest = cost_price * 0.08 / 365 * days;
                if (interest < 0)
                    interest = 0;
            }
            cost_price += interest;

            return cost_price;
        }
        private void SetHeaderStyle()
        {
            dgvContract.Init();
            DataGridView dgv = dgvContract;
            //헤더 디자인
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgv.Columns["etd"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);
            dgv.Columns["eta"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);
            dgv.Columns["exhausted_date"].DefaultCellStyle.ForeColor = Color.Red;
            dgv.Columns["exhausted_day_count"].DefaultCellStyle.ForeColor = Color.Red;
            dgv.Columns["recommend_etd"].DefaultCellStyle.ForeColor = Color.Red;
            dgv.Columns["recommend_contract_date"].DefaultCellStyle.ForeColor = Color.Red;

            // 전체 컬럼의 Sorting 기능 차단 
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void dgvContract_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 & e.ColumnIndex >= 0)
            {
                this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                if (dgvContract.Columns[e.ColumnIndex].Name == "etd"
                    || dgvContract.Columns[e.ColumnIndex].Name == "pending_term"
                    || dgvContract.Columns[e.ColumnIndex].Name == "warehousing_date"
                    || dgvContract.Columns[e.ColumnIndex].Name == "qty"
                    || dgvContract.Columns[e.ColumnIndex].Name == "etd_offer_price"
                    || dgvContract.Columns[e.ColumnIndex].Name == "etd_cost_price")
                {
                    if (dgvContract.Columns[e.ColumnIndex].Name == "etd")
                    {
                        dgvContract.Rows[e.RowIndex].Cells["etd"].Style.ForeColor = Color.Black;

                        if (dgvContract.Rows[e.RowIndex].Cells["etd"].Value != null)
                        {
                            string etd = common.strDatetime(dgvContract.Rows[e.RowIndex].Cells["etd"].Value.ToString());
                            dgvContract.Rows[e.RowIndex].Cells["etd"].Value = etd;
                            dgvContract.Update();
                        }
                    }

                    else if (dgvContract.Columns[e.RowIndex].Name == "warehousing_date" && dgvContract.Rows[e.RowIndex].Cells["warehousing_date"].Value != null)
                        dgvContract.Rows[e.RowIndex].Cells["warehousing_date"].Value = common.strDatetime(dgvContract.Rows[e.RowIndex].Cells["warehousing_date"].Value.ToString());


                    if ((dgvContract.Columns[e.ColumnIndex].Name == "etd" || dgvContract.Columns[e.ColumnIndex].Name == "pending_term") && dgvContract.Rows[e.RowIndex].Cells["etd"].Value != null && dgvContract.Rows[e.RowIndex].Cells["pending_term"].Value != null)
                    {
                        int pending_day;
                        if (int.TryParse(dgvContract.Rows[e.RowIndex].Cells["pending_term"].Value.ToString(), out pending_day))
                        {
                            DateTime dt;
                            if (dgvContract.Rows[e.RowIndex].Cells[1].Value != null && DateTime.TryParse(dgvContract.Rows[e.RowIndex].Cells[1].Value.ToString(), out dt))
                            {
                                dt = dt.AddDays(pending_day);
                                //common.GetPlusDay(dt, pending_day, out dt);
                                dgvContract.Rows[e.RowIndex].Cells["warehousing_date"].Value = dt.ToString("yyyy-MM-dd");
                            }
                        }
                    }
                    if (SortSetting())
                        CalculateStock();
                }
                else if (dgvContract.Columns[e.ColumnIndex].Name == "weight_calculate")
                {
                    if (SortSetting())
                        CalculateStock();
                }
                this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            }
        }
        private void dgvStockSales_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            dgvStockSales.EndEdit();
            if (dgvStockSales.Rows.Count > 0)
            {
                if (dgvStockSales.Columns[e.ColumnIndex].Name == "day_sales_qty")
                {
                    double day_sales_qty;
                    if (dgvStockSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !double.TryParse(dgvStockSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out day_sales_qty))
                        day_sales_qty = 0;
                    dgvStockSales.Rows[e.RowIndex].Cells["day_sales_qty_double"].Value = day_sales_qty;
                    dgvStockSales.Rows[e.RowIndex].Cells["month_sales_qty"].Value = (day_sales_qty * 21).ToString("#,##0");
                    CalculateStock();
                }
                else if (dgvStockSales.Columns[e.ColumnIndex].Name == "month_sales_qty")
                {
                    double month_sales_qty;
                    if (dgvStockSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !double.TryParse(dgvStockSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out month_sales_qty))
                        month_sales_qty = 0;

                    double day_sales_qty = month_sales_qty / 21;
                    dgvStockSales.Rows[e.RowIndex].Cells["day_sales_qty"].Value = day_sales_qty.ToString("#,##0");
                    dgvStockSales.Rows[e.RowIndex].Cells["day_sales_qty_double"].Value = day_sales_qty;
                    CalculateStock();
                }

                dgvStockSales.EndEdit();
                double shipment_qty;
                if (dgvStockSales.Rows[0].Cells["shipment_qty"].Value == null || !double.TryParse(dgvStockSales.Rows[0].Cells["shipment_qty"].Value.ToString(), out shipment_qty))
                    shipment_qty = 0;
                double shipping_qty;
                if (dgvStockSales.Rows[0].Cells["shipping_qty"].Value == null || !double.TryParse(dgvStockSales.Rows[0].Cells["shipping_qty"].Value.ToString(), out shipping_qty))
                    shipping_qty = 0;
                double seaover_unpending_qty;
                if (dgvStockSales.Rows[0].Cells["seaover_unpending_qty"].Value == null || !double.TryParse(dgvStockSales.Rows[0].Cells["seaover_unpending_qty"].Value.ToString(), out seaover_unpending_qty))
                    seaover_unpending_qty = 0;
                double seaover_pending_qty;
                if (dgvStockSales.Rows[0].Cells["seaover_pending_qty"].Value == null || !double.TryParse(dgvStockSales.Rows[0].Cells["seaover_pending_qty"].Value.ToString(), out seaover_pending_qty))
                    seaover_pending_qty = 0;
                double reserved_qty;
                if (dgvStockSales.Rows[0].Cells["reserved_qty"].Value == null || !double.TryParse(dgvStockSales.Rows[0].Cells["reserved_qty"].Value.ToString(), out reserved_qty))
                    reserved_qty = 0;
                double inQty;
                if (!double.TryParse(txtInQty.Text, out inQty))
                    inQty = 0;
                //실재고
                double total_qty = 0;
                if (cbShipment.Checked)
                    total_qty += shipment_qty + shipping_qty;
                if (cbSeaoverUnpending.Checked)
                    total_qty += seaover_unpending_qty;
                if (cbSeaoverPending.Checked)
                    total_qty += seaover_pending_qty;
                if (cbReserved.Checked)
                    total_qty -= reserved_qty;
                //total_qty += inQty;
                dgvStockSales.Rows[0].Cells["total_qty"].Value = total_qty.ToString("#,##0");
                //일판매
                double day_sales_qty_double;
                if (dgvStockSales.Rows[0].Cells["day_sales_qty_double"].Value == null || !double.TryParse(dgvStockSales.Rows[0].Cells["day_sales_qty_double"].Value.ToString(), out day_sales_qty_double))
                    day_sales_qty_double = 0;
                double enable_sale_days = total_qty / day_sales_qty_double;
                double month_around = enable_sale_days / 21;

                dgvStockSales.Rows[0].Cells["total_month_around"].Value = month_around.ToString("#,##0.00");

                txtAroundRate.Text = (((total_qty + inQty) / day_sales_qty_double) / 21).ToString("#,##0.00");

                DataGridViewRow row = GetSelectProductDgvr();
                if (row != null)
                    OpenExhaustedManager(row);
            }
            this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
        }
        private void cbShipment_CheckedChanged(object sender, EventArgs e)
        {
            //RealStockCalculate();
            this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);

            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
                OpenExhaustedManager(row);

            /*ShortManagerModel smModel = GetProductStockInfo(GetSelectProductDgvr());
            if (SortSetting())
                CalculateStock(smModel);*/
            this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
        }
        private void cbSaleTerm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentSaleTerms))
            {
                if (messageBox.Show(this, "새창으로 대시보드를 여시겠습니까?" + "\n Yes : 새창       No : 기존창", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    NewDashboard();

                    this.cbSaleTerm.SelectedIndexChanged -= new System.EventHandler(this.cbSaleTerm_SelectedIndexChanged);
                    cbSaleTerm.Text = currentSaleTerms;
                    this.cbSaleTerm.SelectedIndexChanged += new System.EventHandler(this.cbSaleTerm_SelectedIndexChanged);
                }
                else
                {
                    this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                    this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                    DataGridViewRow row = GetSelectProductDgvr();
                    if (row != null)
                    {
                        //GetProductStockInfo(row);
                        OpenExhaustedManager(row);
                    }
                    this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                    this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                    currentSaleTerms = cbSaleTerm.Text;
                }
            }
        }
        private void NewDashboard()
        {
            DataGridViewRow dgvRow = GetSelectProductDgvr();
            if (dgvRow != null)
            {
                string[] productInfo = new string[30];

                productInfo[0] = dgvRow.Cells["product"].Value.ToString();
                productInfo[1] = dgvRow.Cells["origin"].Value.ToString();
                productInfo[2] = dgvRow.Cells["sizes"].Value.ToString();
                productInfo[3] = dgvRow.Cells["unit"].Value.ToString();
                productInfo[4] = dgvRow.Cells["cost_unit"].Value.ToString();
                productInfo[5] = dgvRow.Cells["seaover_current_purchase_price"].Value.ToString();

                productInfo[6] = dgvRow.Cells["custom"].Value.ToString();
                productInfo[7] = dgvRow.Cells["tax"].Value.ToString();
                productInfo[8] = dgvRow.Cells["incidental_expense"].Value.ToString();
                productInfo[9] = dgvRow.Cells["purchase_margin"].Value.ToString();
                productInfo[10] = dgvRow.Cells["production_days"].Value.ToString();

                productInfo[11] = "";
                productInfo[12] = "";
                productInfo[13] = "";
                productInfo[14] = "";

                productInfo[15] = dgvRow.Cells["product_code"].Value.ToString();
                productInfo[16] = dgvRow.Cells["origin_code"].Value.ToString();
                productInfo[17] = dgvRow.Cells["sizes_code"].Value.ToString();

                productInfo[18] = "";
                productInfo[19] = dgvRow.Cells["weight_calculate"].Value.ToString();

                productInfo[20] = dgvRow.Cells["price_unit"].Value.ToString();
                productInfo[21] = dgvRow.Cells["unit_count"].Value.ToString();
                productInfo[22] = dgvRow.Cells["seaover_unit"].Value.ToString();
                productInfo[23] = dgvRow.Cells["manager1"].Value.ToString();
                productInfo[24] = dgvRow.Cells["manager2"].Value.ToString();
                productInfo[25] = dgvRow.Cells["seaover_current_sales_price"].Value.ToString();
                productInfo[26] = dgvRow.Cells["division"].Value.ToString();
                productInfo[27] = dgvRow.Cells["delivery_days"].Value.ToString();

                productInfo[28] = dgvRow.Cells["seaover_current_normal_price"].Value.ToString();
                productInfo[29] = dgvRow.Cells["bundle_count"].Value.ToString();

                List<string[]> productInfoList = new List<string[]>();
                productInfoList.Add(productInfo);

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

                try
                {
                    DetailDashBoardForSales dd = new DetailDashBoardForSales(um, productInfoList, saleTerm);
                    dd.FirstIndexClick();
                    dd.Show();
                }
                catch
                { }

            }
        }

        #endregion

        #region 업체별시세관리 가져오기
        private void GetMarketPriceByCompany(DataGridViewRow select_row)
        {
            DateTime sttdate, enddate;
            if (!DateTime.TryParse(txtSttdate.Text, out sttdate) || !DateTime.TryParse(txtEnddate.Text, out enddate))
            {
                MessageBox.Show(this, "검색기간 값을 다시 확인해주세요.");
                this.Activate();
                return;
            }
            dgvMarket.Rows.Clear();
            string unit = select_row.Cells["unit"].Value.ToString();

            double purchase_max = 0, purchase_min = 9999999;
            double sale_max = 0, sale_min = 9999999;

            //머지품목이 아닐때
            if (select_row.Cells["merge_product"].Value == null || string.IsNullOrEmpty(select_row.Cells["merge_product"].Value.ToString()))
            {
                DataTable marketDt = seaoverRepository.GetAllData(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                                                                , select_row.Cells["product"].Value.ToString()
                                                                , select_row.Cells["origin"].Value.ToString()
                                                                , select_row.Cells["sizes"].Value.ToString()
                                                                , unit);

                if (marketDt.Rows.Count > 0)
                {
                    for (int i = 0; i < marketDt.Rows.Count; i++)
                    {
                        int n = dgvMarket.Rows.Add();
                        DataGridViewRow row = dgvMarket.Rows[n];
                        row.Cells["company"].Value = marketDt.Rows[i]["매입처"].ToString();

                        double sales_price;
                        if (!double.TryParse(marketDt.Rows[i]["매출단가"].ToString(), out sales_price))
                            sales_price = 0;

                        double purchase_price;
                        if (!double.TryParse(marketDt.Rows[i]["매입단가"].ToString(), out purchase_price))
                            purchase_price = 0;

                        double unit_count;
                        if (!double.TryParse(marketDt.Rows[i]["단위수량"].ToString(), out unit_count))
                            unit_count = 0;

                        double margin = (sales_price - purchase_price) / sales_price * 100;

                        /*sales_price *= unit_count;
                        if(purchase_price > 10)
                            purchase_price *= unit_count;*/

                        row.Cells["seaover_sales_price"].Value = sales_price.ToString("#,##0");
                        row.Cells["seaover_purchase_price"].Value = purchase_price.ToString("#,##0");
                        row.Cells["seaover_margin"].Value = margin.ToString("#,##0.0") + "%";
                        row.Cells["warehouse"].Value = marketDt.Rows[i]["보관처"].ToString();
                        row.Cells["seaover_updatetime"].Value = Convert.ToDateTime(marketDt.Rows[i]["매입일자"].ToString()).ToString("yy-MM-dd");
                        row.Cells["seaover_remark"].Value = marketDt.Rows[i]["적요"].ToString();
                        row.Cells["product_unit2"].Value = marketDt.Rows[i]["단위"].ToString();

                        if (purchase_price >= purchase_max)
                            purchase_max = purchase_price;
                        if (purchase_price <= purchase_min)
                            purchase_min = purchase_price;
                        if (sales_price >= sale_max)
                            sale_max = sales_price;
                        if (sales_price <= sale_min)
                            sale_min = sales_price;

                        //20일 보다 더오래된 단가라면
                        if (Convert.ToDateTime(marketDt.Rows[i]["매입일자"].ToString()) < Convert.ToDateTime(DateTime.Now.AddDays(-20).ToString("yyyy-MM-dd")))
                            row.DefaultCellStyle.ForeColor = Color.Gray;
                    }
                }
            }
            //머지품목일때
            else
            {
                double main_unit;
                if (select_row.Cells["unit"].Value == null || !double.TryParse(select_row.Cells["unit"].Value.ToString(), out main_unit))
                    main_unit = 1;
                double main_seaover_unit;
                if (select_row.Cells["seaover_unit"].Value == null || !double.TryParse(select_row.Cells["seaover_unit"].Value.ToString(), out main_seaover_unit))
                    main_seaover_unit = 1;


                string[] merge_product = select_row.Cells["merge_product"].Value.ToString().Trim().Split('\n');
                if (merge_product.Length > 0)
                {
                    for (int j = 0; j < merge_product.Length; j++)
                    {
                        string[] product = merge_product[j].Trim().Split('^');
                        if (product.Length > 0)
                        { 
                            DataTable marketDt = seaoverRepository.GetAllData(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                                                                , product[0], product[1], product[2], product[3]);

                            if (marketDt.Rows.Count > 0)
                            {
                                for (int i = 0; i < marketDt.Rows.Count; i++)
                                {
                                    //이미출력된 시장단가는 제외
                                    if (!isExistProduct(marketDt.Rows[i]))
                                    {
                                        int n = dgvMarket.Rows.Add();
                                        DataGridViewRow row = dgvMarket.Rows[n];
                                        row.Cells["company"].Value = marketDt.Rows[i]["매입처"].ToString();

                                        double sales_price;
                                        if (!double.TryParse(marketDt.Rows[i]["매출단가"].ToString(), out sales_price))
                                            sales_price = 0;

                                        double purchase_price;
                                        if (!double.TryParse(marketDt.Rows[i]["매입단가"].ToString(), out purchase_price))
                                            purchase_price = 0;

                                        double unit_count;
                                        if (!double.TryParse(marketDt.Rows[i]["단위수량"].ToString(), out unit_count))
                                            unit_count = 0;

                                        double margin = (sales_price - purchase_price) / sales_price * 100;

                                        /*sales_price *= unit_count;
                                        if(purchase_price > 10)
                                            purchase_price *= unit_count;*/

                                        row.Cells["seaover_sales_price"].Value = sales_price.ToString("#,##0");
                                        row.Cells["seaover_purchase_price"].Value = purchase_price.ToString("#,##0");
                                        row.Cells["seaover_margin"].Value = margin.ToString("#,##0.0") + "%";
                                        row.Cells["warehouse"].Value = marketDt.Rows[i]["보관처"].ToString();
                                        row.Cells["seaover_updatetime"].Value = Convert.ToDateTime(marketDt.Rows[i]["매입일자"].ToString()).ToString("yy-MM-dd");
                                        row.Cells["seaover_remark"].Value = marketDt.Rows[i]["적요"].ToString();
                                        row.Cells["product_unit2"].Value = marketDt.Rows[i]["단위"].ToString();

                                        if (purchase_price >= purchase_max)
                                            purchase_max = purchase_price;
                                        if (purchase_price <= purchase_min)
                                            purchase_min = purchase_price;
                                        if (sales_price >= sale_max)
                                            sale_max = sales_price;
                                        if (sales_price <= sale_min)
                                            sale_min = sales_price;

                                        //20일 보다 더오래된 단가라면
                                        if (Convert.ToDateTime(marketDt.Rows[i]["매입일자"].ToString()) < Convert.ToDateTime(DateTime.Now.AddDays(-20).ToString("yyyy-MM-dd")))
                                            row.DefaultCellStyle.ForeColor = Color.Gray;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            lbMaxMin2.Text = "매입단가 " + purchase_max.ToString("#,##0") + " ~ " + purchase_min.ToString("#,##0")
                + "\n매출단가 " + sale_max.ToString("#,##0") + " ~ " + sale_min.ToString("#,##0");
        }

        private bool isExistProduct(DataRow dr)
        {
            bool isExist = false;

            double sales_price;
            if (!double.TryParse(dr["매출단가"].ToString(), out sales_price))
                sales_price = 0;

            double purchase_price;
            if (!double.TryParse(dr["매입단가"].ToString(), out purchase_price))
                purchase_price = 0;

            for (int i = 0; i < dgvMarket.RowCount; i++)
            {
                if (dgvMarket.Rows[i].Cells["product_unit2"].Value.ToString() == dr["단위"].ToString()
                    && dgvMarket.Rows[i].Cells["company"].Value.ToString() == dr["매입처"].ToString()
                    && dgvMarket.Rows[i].Cells["seaover_sales_price"].Value.ToString() == sales_price.ToString("#,##0")
                    && dgvMarket.Rows[i].Cells["seaover_purchase_price"].Value.ToString() == purchase_price.ToString("#,##0")
                    && dgvMarket.Rows[i].Cells["warehouse"].Value.ToString() == dr["보관처"].ToString()
                    && dgvMarket.Rows[i].Cells["seaover_updatetime"].Value.ToString() == Convert.ToDateTime(dr["매입일자"].ToString()).ToString("yy-MM-dd"))
                {
                    isExist = true;
                    break;
                }
            }
            return isExist;
        }

        private void SetMarketChart(DataGridViewRow select_row)
        {
            if (dgvPurchaseUsd.Rows.Count > 0)
            {
                double max_krw = 0, min_krw = 9999999, max_usd = 0, min_usd = 9999999;
                for (int i = 0; i < dgvMarket.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvPurchaseUsd.Rows[i];
                    double price = Convert.ToDouble(row.Cells["price"].Value.ToString());
                    double price_krw = Convert.ToDouble(row.Cells["price_krw"].Value.ToString());
                    double offer_price = Convert.ToDouble(row.Cells["price_usd"].Value.ToString());
                    double comparison_price;
                    //Chart   MAX,Min
                    if (select_row.Cells["origin"].Value.ToString() == "국산")
                        comparison_price = Convert.ToDouble(row.Cells["price"].Value.ToString());
                    else
                        comparison_price = Convert.ToDouble(row.Cells["price"].Value.ToString());
                    //최대, 최소
                    if (max_krw < comparison_price)
                        max_krw = comparison_price;
                    if (min_krw > comparison_price)
                        min_krw = comparison_price;
                    if (max_usd < offer_price)
                        max_usd = offer_price;
                    if (min_usd > offer_price)
                        min_usd = offer_price;
                }
                lbMaxMin.Text = "구매단가 " + min_krw.ToString("#,##0") + " ~ " + max_krw.ToString("#,##0");
            }
        }
        #endregion

        #region 오퍼가 리스트, 평균원가 계산
        private void GetPurchasePriceList(DataGridViewRow row)
        {
            //유효성검사
            DateTime sttdate, enddate;
            if (!DateTime.TryParse(txtOfferSttdate.Text, out sttdate) || !DateTime.TryParse(txtOfferEnddate.Text, out enddate))
            {
                MessageBox.Show(this, "검색기간의 값을 다시 확인해주세요.");
                return;
            }
            //초기화
            cbPurchasePriceList.DataSource = null;

            //데이터 출력
            string unit = row.Cells["seaover_unit"].Value.ToString();
            if (cbAllUnit.Checked)
                unit = "";
            DataTable list = purchasePriceRepository.GetPurchasePriceList(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), unit);

            Dictionary<string, string> listDic = new Dictionary<string, string>();
            System.ComponentModel.BindingList<object> cmbList = new System.ComponentModel.BindingList<object>();
            for (int i = 0; i < list.Rows.Count; i++)
            {
                DateTime updatetime;
                if (DateTime.TryParse(list.Rows[i]["updatetime"].ToString(), out updatetime))
                {
                    string item_txt = updatetime.ToString("yyyy-MM-dd") + " - (" + list.Rows[i]["purchase_price"].ToString() + ")" + list.Rows[i]["company"].ToString();
                    cmbList.Add(new { Display = item_txt, Value = list.Rows[i]["id"].ToString() });
                }
                
            }

            //초기화
            txtOfferUpdatetime.Text = string.Empty;
            txtOfferCompany.Text = string.Empty;
            txtOfferPrice.Text = "0";
            txtInQty.Text = "0";
            txtAverageCostPrice.Text = "0";
            txtOfferCostPrice.Text = "0";
            txtAroundRate.Text = "0";

            cbPurchasePriceList.DataSource = cmbList;
            if (cmbList.Count > 0)
            {
                cbPurchasePriceList.DisplayMember = "Display";
                cbPurchasePriceList.ValueMember = "Value";
                cbPurchasePriceList.SelectedIndex = 0;

                double purchase_id;
                if (double.TryParse(cbPurchasePriceList.SelectedValue.ToString(), out purchase_id))
                {
                    DataTable purchaseAsOneDt = purchasePriceRepository.GetPurchasePriceList("", "", "", "", "", "", purchase_id.ToString());
                    if (purchaseAsOneDt.Rows.Count > 0)
                    {
                        txtOfferUpdatetime.Text = Convert.ToDateTime(purchaseAsOneDt.Rows[0]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                        txtOfferCompany.Text = purchaseAsOneDt.Rows[0]["company"].ToString();
                        double offer_price;
                        if (!double.TryParse(purchaseAsOneDt.Rows[0]["purchase_price"].ToString(), out offer_price))
                            offer_price = 0;
                        txtOfferPrice.Text = offer_price.ToString("#,##0.00");
                        txtOfferCostPrice.Text = calculateCostPrice(offer_price).ToString("#,##0");
                        CalculateAverageCostPrice(); ;
                    }
                }
            }
        }

        private void CalculateAverageCostPrice()
        {
            double total_cost_price = 0;
            double total_qty = 0;
            for (int i = 0; i < dgvContract.Rows.Count; i++)
            {
                DataGridViewRow row = dgvContract.Rows[i];
                double cost_price = 0, qty = 0;
                if (i == 0)
                {
                    if (row.Cells["etd_cost_price"].Value == null || !double.TryParse(row.Cells["etd_cost_price"].Value.ToString(), out cost_price))
                        cost_price = 0;

                    if (row.Cells["current_qty"].Value == null || !double.TryParse(row.Cells["current_qty"].Value.ToString(), out qty))
                        qty = 0;
                }
                else
                {
                    if (row.Cells["etd_cost_price"].Value == null || !double.TryParse(row.Cells["etd_cost_price"].Value.ToString(), out cost_price))
                        cost_price = 0;

                    if (row.Cells["qty"].Value == null || !double.TryParse(row.Cells["qty"].Value.ToString(), out qty))
                        qty = 0;
                }
                //전체 평균
                if (cost_price > 0 && qty > 0)
                {
                    total_cost_price += cost_price * qty;
                    total_qty += qty;
                }
            }

            double offer_cost_price;
            if (!double.TryParse(txtOfferCostPrice.Text, out offer_cost_price))
                offer_cost_price = 0;
            double in_qty;
            if (!double.TryParse(txtInQty.Text, out in_qty))
                in_qty = 0;
            if (offer_cost_price > 0 || in_qty > 0)
            {
                total_cost_price += offer_cost_price * in_qty;
                total_qty += in_qty;
            }

            txtAverageCostPrice.Text = (total_cost_price / total_qty).ToString("#,##0");
        }
        private void cbPurchasePriceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPurchasePriceList.SelectedIndex >= 0)
            {
                double purchase_id;
                if (double.TryParse(cbPurchasePriceList.SelectedValue.ToString(), out purchase_id))
                { 

                    DataTable purchaseAsOneDt = purchasePriceRepository.GetPurchasePriceList("", "", "", "", "", "", purchase_id.ToString());
                    if (purchaseAsOneDt.Rows.Count > 0)
                    {
                        txtOfferUpdatetime.Text = Convert.ToDateTime(purchaseAsOneDt.Rows[0]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                        txtOfferCompany.Text = purchaseAsOneDt.Rows[0]["company"].ToString();
                        double offer_price;
                        if (!double.TryParse(purchaseAsOneDt.Rows[0]["purchase_price"].ToString(), out offer_price))
                            offer_price = 0;
                        txtOfferPrice.Text = offer_price.ToString("#,##0.00");
                        txtOfferCostPrice.Text = calculateCostPrice(offer_price).ToString("#,##0");
                        CalculateAverageCostPrice(); ;
                    }
                }
            }
        }
        private double calculateCostPrice(double offer_price)
        {
            dgvProduct.EndEdit();
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
            { 
                double custom;
                if (!double.TryParse(txtCustom.Text, out custom))
                    custom = 0;
                custom /= 100;
                double tax;
                if (!double.TryParse(txtTax.Text, out tax))
                    tax = 0;
                tax /= 100;
                double incidental_expense;
                if (!double.TryParse(txtIncidentalExpense.Text, out incidental_expense))
                    incidental_expense = 0;
                incidental_expense /= 100;
                double cost_unit;
                if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                    cost_unit = 0;
                double unit;
                if (row.Cells["unit"].Value == null || !double.TryParse(row.Cells["unit"].Value.ToString(), out unit))
                    unit = 0;
                bool weight_calculate;
                if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                    weight_calculate = true;
                if (!weight_calculate && cost_unit < 0)
                    weight_calculate = true;
                double exchange_rate;
                if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                    exchange_rate = 0;

                //원가계산
                double cost_price;
                if (weight_calculate)
                    cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate * unit;
                else
                {
                    cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate;
                    if (cbMultiplyTray.Checked)
                        cost_price *= cost_unit;
                }
                    



                return cost_price;
            }
            else
                return 0;

        }
        private double calculateCostPrice(double offer_price, double exchange_rate)
        {
            dgvProduct.EndEdit();
            DataGridViewRow row = GetSelectProductDgvr();
            double custom;
            if (!double.TryParse(txtCustom.Text, out custom))
                custom = 0;
            custom /= 100;
            double tax;
            if (!double.TryParse(txtTax.Text, out tax))
                tax = 0;
            tax /= 100;
            double incidental_expense;
            if (!double.TryParse(txtIncidentalExpense.Text, out incidental_expense))
                incidental_expense = 0;
            incidental_expense /= 100;
            double cost_unit;
            if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                cost_unit = 0;
            double unit_count;
            if (row.Cells["unit_count"].Value == null || !double.TryParse(row.Cells["unit_count"].Value.ToString(), out unit_count))
                unit_count = 0;
            double unit;
            if (row.Cells["unit"].Value == null || !double.TryParse(row.Cells["unit"].Value.ToString(), out unit))
                unit = 0;
            double seaover_unit;
            if (row.Cells["seaover_unit"].Value == null || !double.TryParse(row.Cells["seaover_unit"].Value.ToString(), out seaover_unit))
                seaover_unit = 0;
            bool weight_calculate;
            if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                weight_calculate = true;
            if (!weight_calculate && cost_unit <= 0)
                weight_calculate = true;

            /*//원가계산
            double cost_price = cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate * seaover_unit;

*/
            //원가계산
            double cost_price;
            if (weight_calculate)
                cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate * unit;
            else
            {
                cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate;
                if (cbMultiplyTray.Checked)
                    cost_price *= cost_unit;
            }

            return cost_price;

        }
        #endregion

        #region 오퍼내역 가져오기
        private void dgvPurchaseUsd_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvPurchaseUsd.Columns[e.ColumnIndex].Name == "exchangeRate" || dgvPurchaseUsd.Columns[e.ColumnIndex].Name == "price_usd")
                {
                    DataGridViewRow row = GetSelectProductDgvr();
                    if (row != null)
                    {
                        double custom;
                        if (!double.TryParse(txtCustom.Text, out custom))
                            custom = 0;
                        custom = custom / 100;
                        double tax;
                        if (!double.TryParse(txtTax.Text, out tax))
                            tax = 0;
                        tax = tax / 100;
                        double incidental_expense;
                        if (!double.TryParse(txtIncidentalExpense.Text, out incidental_expense))
                            incidental_expense = 0;
                        incidental_expense = incidental_expense / 100;
                        double unit;
                        if (row.Cells["unit"].Value == null || !double.TryParse(row.Cells["unit"].Value.ToString(), out unit))
                            unit = 0;
                        double seaover_unit;
                        if (row.Cells["seaover_unit"].Value == null || !double.TryParse(row.Cells["seaover_unit"].Value.ToString(), out seaover_unit))
                            seaover_unit = 0;
                        double cost_unit;
                        if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                            cost_unit = 0;
                        bool isWeight = Convert.ToBoolean(row.Cells["weight_calculate"].Value);
                        if (!isWeight && cost_unit == 0)
                            isWeight = true;

                        double exchange_rate;
                        if (dgvPurchaseUsd.Rows[e.RowIndex].Cells["exchangeRate"].Value == null || !double.TryParse(dgvPurchaseUsd.Rows[e.RowIndex].Cells["exchangeRate"].Value.ToString(), out exchange_rate))
                            exchange_rate = 0;
                        double price_usd;
                        if (dgvPurchaseUsd.Rows[e.RowIndex].Cells["price_usd"].Value == null || !double.TryParse(dgvPurchaseUsd.Rows[e.RowIndex].Cells["price_usd"].Value.ToString(), out price_usd))
                            price_usd = 0;
                        //USD 매입단가
                        double offer_price;
                        if (!isWeight)
                        {
                            offer_price = price_usd * exchange_rate * cost_unit * (1 + tax + custom + incidental_expense);
                        }
                        else
                            offer_price = price_usd * exchange_rate * seaover_unit * (1 + tax + custom + incidental_expense);

                        dgvPurchaseUsd.Rows[e.RowIndex].Cells["price_krw"].Value = offer_price.ToString("#,##0");
                    }
                }
            }
        }
        private void GetPurchase(DataGridViewRow row, bool isUserSettingPurchaseMaring = false)
        {
            //초기화
            dgvPurchaseUsd.Rows.Clear();
            //유효성검사
            if ((row.Cells["product"].Value == null || string.IsNullOrEmpty(row.Cells["product"].Value.ToString()))
                || (row.Cells["origin"].Value == null || string.IsNullOrEmpty(row.Cells["origin"].Value.ToString()))
                || (row.Cells["sizes"].Value == null || string.IsNullOrEmpty(row.Cells["sizes"].Value.ToString()))
                || (row.Cells["unit"].Value == null || string.IsNullOrEmpty(row.Cells["unit"].Value.ToString())))
                return;
            //Seoaverid
            purchaseRepository.SetSeaoverId(um.seaover_id);
            DateTime sttdate = DateTime.Now.AddYears(-4);
            DateTime enddate = DateTime.Now;

            //관세
            double custom;
            if (!double.TryParse(txtCustom.Text, out custom))
                custom = 0;
            custom = custom / 100;
            //과세
            double tax;
            if (!double.TryParse(txtTax.Text, out tax))
                tax = 0;
            tax = tax / 100;
            //부대비용
            double incidental_expense;
            if (!double.TryParse(txtIncidentalExpense.Text, out incidental_expense))
                incidental_expense = 0;
            incidental_expense = incidental_expense / 100;
            //매입마진
            double purchase_margin;
            if (isUserSettingPurchaseMaring || string.IsNullOrEmpty(txtPurchaseMargin.Text))
            {
                if (row.Cells["purchase_margin"].Value == null || !double.TryParse(row.Cells["purchase_margin"].Value.ToString(), out purchase_margin))
                    purchase_margin = 0;
            }
            else
            {
                if (!double.TryParse(txtPurchaseMargin.Text, out purchase_margin))
                    purchase_margin = 0;
            }
            txtPurchaseMargin.Text = purchase_margin.ToString();
            purchase_margin = purchase_margin / 100;
            //단위
            double unit;
            if (row.Cells["unit"].Value == null || !double.TryParse(row.Cells["unit"].Value.ToString(), out unit))
                unit = 0;
            //씨오버단위
            double seaover_unit;
            if (row.Cells["seaover_unit"].Value == null || !double.TryParse(row.Cells["seaover_unit"].Value.ToString(), out seaover_unit))
                seaover_unit = 0;
            //트레이
            double cost_unit;
            if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                cost_unit = 0;
            //단위수량
            double unit_count;
            if (row.Cells["unit_count"].Value == null || !double.TryParse(row.Cells["unit_count"].Value.ToString(), out unit_count))
                unit_count = 0;
            if (unit_count == 0)
                unit_count = 1;
            //계산방식
            bool isWeight;
            if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out isWeight))
                isWeight = true;
            if (!isWeight && cost_unit == 0)
                isWeight = true;
            //매입내역 Data
            string select_unit = row.Cells["unit"].Value.ToString();

            //머지품목 아닐때
            string merge_product;
            bool isMerge;
            if (row.Cells["merge_product"].Value == null || string.IsNullOrEmpty(row.Cells["merge_product"].Value.ToString()))
            {
                merge_product = "";
                isMerge = false;
            }
            //머지품목 일때
            else
            {
                merge_product = row.Cells["merge_product"].Value.ToString();
                isMerge = true;
            }

            //SEAOVER 매입내역
            purchaseDt = purchaseRepository.GetPurchaseProductForDashboard(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                    , row.Cells["product"].Value.ToString()
                    , row.Cells["origin"].Value.ToString()
                    , row.Cells["sizes"].Value.ToString()
                    , seaover_unit.ToString(), merge_product, isMerge, "");

            //오퍼내역 Data
            DataTable offerDt = purchasePriceRepository.GetPurhcasePriceAverage(row.Cells["product"].Value.ToString()
                                                                                , row.Cells["origin"].Value.ToString()
                                                                                , row.Cells["sizes"].Value.ToString()
                                                                                , seaover_unit.ToString(), merge_product, isMerge);
            //환율내역 Data
            DataTable exchangeRateDt = purchaseRepository.GetExchangeRate(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));
            //팬딩내역 Data
            DataTable customDt = customsRepository.GetDashboard(sttdate.ToString("yyyyMM"), enddate.ToString("yyyyMM")
                                                         , row.Cells["product"].Value.ToString()
                                                         , row.Cells["origin"].Value.ToString()
                                                         , row.Cells["sizes"].Value.ToString()
                                                         , seaover_unit.ToString()
                                                         , merge_product, isMerge);
            //출력==========================================================================================================================================

            DataTable resultDt = new DataTable();
            resultDt.Columns.Add("base_date", typeof(string));
            resultDt.Columns.Add("division", typeof(int));
            resultDt.Columns.Add("price", typeof(double));
            resultDt.Columns.Add("in_purchase", typeof(double));
            resultDt.Columns.Add("out_purchase", typeof(double));
            resultDt.Columns.Add("price_krw", typeof(double));
            resultDt.Columns.Add("price_usd", typeof(double));
            resultDt.Columns.Add("exchangeRate", typeof(double));

            string whr;
            //씨오버 매입내역
            if (purchaseDt.Rows.Count > 0 && cbPurchasePriceType.Text.Contains("매입"))
            {
                for (int i = 0; i < purchaseDt.Rows.Count; i++)
                {
                    //환율
                    DataRow[] dr = null;
                    double exchange_rate = 0;
                    if (exchangeRateDt.Rows.Count > 0)
                    {
                        whr = "base_date = '" + purchaseDt.Rows[i]["매입일자"].ToString() + "'";
                        dr = exchangeRateDt.Select(whr);
                        if (dr.Length > 0)
                            exchange_rate = Convert.ToDouble(dr[0]["exchange_rate"].ToString());
                        else if (purchaseDt.Rows[i]["매입일자"].ToString() == DateTime.Now.ToString("yyyyMM"))
                        {
                            if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                                exchange_rate = 0;
                        }
                    }
                    //오퍼가 내역이 있으면 대체
                    int division = 0;
                    double purcahse = 0, price_krw = 0, offer_price = 0;
                    double in_purchase = 0, out_purchase = 0;
                    purcahse = Convert.ToDouble(purchaseDt.Rows[i]["단가"].ToString());
                    //KRW 국내매입
                    in_purchase = Convert.ToDouble(purchaseDt.Rows[i]["단가2"].ToString()) * (1 - purchase_margin);
                    //KRW 수입매입
                    out_purchase = Convert.ToDouble(purchaseDt.Rows[i]["단가3"].ToString()) * (1 - purchase_margin);
                    //매입단가 평균
                    if (in_purchase > 0 && out_purchase > 0)
                        price_krw = (in_purchase + out_purchase) / 2;
                    else if (in_purchase > 0 && out_purchase == 0)
                        price_krw = in_purchase;
                    else if (in_purchase == 0 && out_purchase > 0)
                        price_krw = out_purchase;
                    else
                        price_krw = purcahse;
                    //USD 매입단가
                    if (!isWeight)
                        offer_price = price_krw / exchange_rate / cost_unit / (1 + tax + custom + incidental_expense);
                    else
                        offer_price = price_krw / exchange_rate / seaover_unit / (1 + tax + custom + incidental_expense);

                    if (price_krw > 0 && offer_price > 0)
                    {
                        string sdate = purchaseDt.Rows[i]["매입일자"].ToString().Substring(0, 4) + "-" + purchaseDt.Rows[i]["매입일자"].ToString().Substring(4, 2) + "-01";

                        //2023-10-12 중량단가 일때만 단위수량으로 나눔
                        if (rbTrayPrice.Checked && cost_unit > 0)
                        {
                            purcahse /= cost_unit;
                            in_purchase /= cost_unit;
                            out_purchase /= cost_unit;
                            price_krw /= cost_unit;
                        }
                        else if (unit_count > 0)
                        {
                            purcahse /= unit_count;
                            in_purchase /= unit_count;
                            out_purchase /= unit_count;
                            price_krw /= unit_count;
                        }

                        DataRow resultDr = resultDt.NewRow();
                        resultDr["base_date"] = sdate;
                        resultDr["division"] = division;
                        resultDr["price"] = purcahse;
                        resultDr["in_purchase"] = in_purchase;
                        resultDr["out_purchase"] = out_purchase;
                        resultDr["price_krw"] = price_krw;
                        resultDr["price_usd"] = offer_price;
                        resultDr["exchangeRate"] = exchange_rate;
                        resultDt.Rows.Add(resultDr);
                    }
                }
            }
            //팬딩내역
            if (customDt.Rows.Count > 0 && cbPurchasePriceType.Text.Contains("팬딩"))
            {
                for (int i = 0; i < customDt.Rows.Count; i++)
                {
                    //환율
                    DataRow[] dr = null;
                    double exchange_rate = 0;
                    if (exchangeRateDt.Rows.Count > 0)
                    {
                        whr = "base_date = '" + customDt.Rows[i]["eta"].ToString() + "'";
                        dr = exchangeRateDt.Select(whr);
                        if (dr.Length > 0)
                            exchange_rate = Convert.ToDouble(dr[0]["exchange_rate"].ToString());
                        else if (customDt.Rows[i]["eta"].ToString() == DateTime.Now.ToString("yyyyMM"))
                        {
                            if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                                exchange_rate = 0;
                        }
                    }

                    int division = 0;
                    double purcahse = 0, price_krw = 0, offer_price = 0;
                    double in_purchase = 0, out_purchase = 0;

                    offer_price = Convert.ToDouble(customDt.Rows[i]["unit_price"].ToString());

                    if (!isWeight)
                        price_krw = offer_price * exchange_rate * (1 + tax + custom + incidental_expense) * cost_unit;
                    else
                        price_krw = offer_price * exchange_rate * (1 + tax + custom + incidental_expense) * seaover_unit;

                    purcahse = price_krw;
                    division = 1;

                    if (price_krw > 0 && offer_price > 0)
                    {
                        string sdate = customDt.Rows[i]["eta"].ToString().Substring(0, 4) + "-" + customDt.Rows[i]["eta"].ToString().Substring(4, 2) + "-01";

                        //2023-10-12 중량단가 일때만 단위수량으로 나눔
                        if (rbTrayPrice.Checked && cost_unit > 0)
                        {
                            purcahse /= cost_unit;
                            in_purchase /= cost_unit;
                            out_purchase /= cost_unit;
                            price_krw /= cost_unit;
                        }
                        else if (unit_count > 0)
                        {
                            purcahse /= unit_count;
                            in_purchase /= unit_count;
                            out_purchase /= unit_count;
                            price_krw /= unit_count;
                        }

                        DataRow resultDr = resultDt.NewRow();
                        resultDr["base_date"] = sdate;
                        resultDr["division"] = division;
                        resultDr["price"] = purcahse;
                        resultDr["in_purchase"] = in_purchase;
                        resultDr["out_purchase"] = out_purchase;
                        resultDr["price_krw"] = price_krw;
                        resultDr["price_usd"] = offer_price;
                        resultDr["exchangeRate"] = exchange_rate;
                        resultDt.Rows.Add(resultDr);
                    }
                }
            }
            //오퍼내역
            if (offerDt.Rows.Count > 0 && cbPurchasePriceType.Text.Contains("오퍼가"))
            {
                for (int i = 0; i < offerDt.Rows.Count; i++)
                {
                    //환율
                    DataRow[] dr = null;
                    double exchange_rate = 0;
                    if (exchangeRateDt.Rows.Count > 0)
                    {
                        whr = "base_date = '" + offerDt.Rows[i]["updatetime"].ToString() + "'";
                        dr = exchangeRateDt.Select(whr);
                        if (dr.Length > 0)
                            exchange_rate = Convert.ToDouble(dr[0]["exchange_rate"].ToString());
                        else if (offerDt.Rows[i]["updatetime"].ToString() == DateTime.Now.ToString("yyyyMM"))
                        {
                            if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                                exchange_rate = 0;
                        }
                    }
                    //오퍼가 내역이 있으면 대체
                    int division = 1;
                    double purcahse = 0, price_krw = 0, offer_price = 0;
                    double in_purchase = 0, out_purchase = 0;

                    offer_price = Convert.ToDouble(offerDt.Rows[i]["purchase_price"].ToString());

                    //고지가
                    double fixed_tariff;
                    if (!double.TryParse(offerDt.Rows[i]["fixed_tariff"].ToString(), out fixed_tariff))
                        fixed_tariff = 0;
                    fixed_tariff /= 1000;

                    if (!isWeight)
                    {
                        price_krw = cost_unit * offer_price * exchange_rate;
                        price_krw *= (custom + 1);

                        price_krw *= (tax + 1);
                        price_krw *= (incidental_expense + 1);

                    }
                    else
                    {
                        if (fixed_tariff > 0)
                        {
                            price_krw = offer_price * exchange_rate * unit;
                            price_krw += (fixed_tariff * unit * exchange_rate * custom);

                            price_krw *= (tax + 1);
                            price_krw *= (incidental_expense + 1);
                        }
                        else
                        {
                            price_krw = unit * offer_price * exchange_rate;
                            price_krw *= (custom + 1);
                            price_krw *= (tax + 1);
                            price_krw *= (incidental_expense + 1);
                        }
                    }
                    purcahse = price_krw;
                    division = 2;


                    if (price_krw > 0 && offer_price > 0)
                    {
                        string sdate = offerDt.Rows[i]["updatetime"].ToString().Substring(0, 4) + "-" + offerDt.Rows[i]["updatetime"].ToString().Substring(4, 2) + "-01";

                        //2023-10-12 중량단가 일때만 단위수량으로 나눔
                        if (rbTrayPrice.Checked && cost_unit > 0)
                        {
                            purcahse /= cost_unit;
                            in_purchase /= cost_unit;
                            out_purchase /= cost_unit;
                            price_krw /= cost_unit;
                        }

                        DataRow resultDr = resultDt.NewRow();
                        resultDr["base_date"] = sdate;
                        resultDr["division"] = division;
                        resultDr["price"] = purcahse;
                        resultDr["in_purchase"] = in_purchase;
                        resultDr["out_purchase"] = out_purchase;
                        resultDr["price_krw"] = price_krw;
                        resultDr["price_usd"] = offer_price;
                        resultDr["exchangeRate"] = exchange_rate;
                        resultDt.Rows.Add(resultDr);
                    }
                }
            }


            DataView dv = new DataView(resultDt);
            dv.Sort = "base_date DESC, division DESC, price_usd ASC ";
            resultDt = dv.ToTable();

            this.dgvPurchaseUsd.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseUsd_CellValueChanged);
            if (resultDt != null && resultDt.Rows.Count > 0)
            {
                DateTime max_date = Convert.ToDateTime(resultDt.Rows[0]["base_date"].ToString());
                DateTime min_date = Convert.ToDateTime(resultDt.Rows[resultDt.Rows.Count - 1]["base_date"].ToString());

                for (DateTime temp_date = max_date; temp_date >= min_date; temp_date = temp_date.AddMonths(-1))
                {
                    whr = $"base_date = '{temp_date.ToString("yyyy-MM-dd")}'";
                    DataRow[] dr = resultDt.Select(whr);
                    if (dr.Length > 0)
                    {
                        int n = dgvPurchaseUsd.Rows.Add();
                        string sdate = dr[0]["base_date"].ToString().Substring(2, 2) + "/" + dr[0]["base_date"].ToString().Substring(5, 2);
                        dgvPurchaseUsd.Rows[n].Cells["purchase_date"].Value = sdate;
                        dgvPurchaseUsd.Rows[n].Cells["price"].Value = Convert.ToDouble(dr[0]["price"].ToString()).ToString("#,##0");
                        dgvPurchaseUsd.Rows[n].Cells["price_krw"].Value = Convert.ToDouble(dr[0]["price_krw"].ToString()).ToString("#,##0");
                        dgvPurchaseUsd.Rows[n].Cells["exchangeRate"].Value = Convert.ToDouble(dr[0]["exchangeRate"].ToString()).ToString("#,##0.00");
                        dgvPurchaseUsd.Rows[n].Cells["price_usd"].Value = Convert.ToDouble(dr[0]["price_usd"].ToString()).ToString("#,##0.00");
                        //dgvPurchaseUsd.Rows[n].Cells["division"].Value = division;

                        switch (dr[0]["division"].ToString())
                        {
                            case "1":
                                dgvPurchaseUsd.Rows[n].DefaultCellStyle.ForeColor = Color.Black;
                                dgvPurchaseUsd.Rows[n].DefaultCellStyle.Font = new Font("고딕", 9, FontStyle.Italic);
                                break;
                            case "2":
                                dgvPurchaseUsd.Rows[n].DefaultCellStyle.ForeColor = Color.Black;
                                dgvPurchaseUsd.Rows[n].DefaultCellStyle.Font = new Font("고딕", 9, FontStyle.Regular);
                                break;
                            default:
                                dgvPurchaseUsd.Rows[n].DefaultCellStyle.ForeColor = Color.Gray;
                                dgvPurchaseUsd.Rows[n].DefaultCellStyle.Font = new Font("고딕", 8, FontStyle.Regular);
                                break;

                        }
                    }
                }
            }
            this.dgvPurchaseUsd.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseUsd_CellValueChanged);

            //this.dgvPurchaseUsd.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseUsd_CellValueChanged);
            /*switch (cbPurchasePriceType.Text)
            {
                case "오퍼가평균":
                    {
                        if (offerDt.Rows.Count > 0)
                        {
                            for (int i = 0; i < offerDt.Rows.Count; i++)
                            {
                                //환율
                                DataRow[] dr = null;
                                double exchange_rate = 0;
                                if (exchangeRateDt.Rows.Count > 0)
                                {
                                    string whr = "base_date = '" + offerDt.Rows[i]["updatetime"].ToString() + "'";
                                    dr = exchangeRateDt.Select(whr);
                                    if (dr.Length > 0)
                                        exchange_rate = Convert.ToDouble(dr[0]["exchange_rate"].ToString());
                                    else if (offerDt.Rows[i]["updatetime"].ToString() == DateTime.Now.ToString("yyyyMM"))
                                    {
                                        if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                                            exchange_rate = 0;
                                    }
                                }
                                //오퍼가 내역이 있으면 대체
                                int division = 0;
                                double purcahse = 0, price_krw = 0, offer_price = 0;

                                offer_price = Convert.ToDouble(offerDt.Rows[i]["purchase_price"].ToString());

                                //고지가
                                double fixed_tariff;
                                if (!double.TryParse(offerDt.Rows[i]["fixed_tariff"].ToString(), out fixed_tariff))
                                    fixed_tariff = 0;
                                fixed_tariff /= 1000;

                                if (!isWeight)
                                {
                                    price_krw = cost_unit * offer_price * exchange_rate;
                                    price_krw *= (custom + 1);
                                    price_krw *= (tax + 1);
                                    price_krw *= (incidental_expense  + 1);
                                }
                                else
                                {
                                    if (fixed_tariff > 0)
                                    {
                                        price_krw = offer_price * exchange_rate * unit;
                                        price_krw += (fixed_tariff * unit * exchange_rate * custom);

                                        price_krw *= (tax + 1);
                                        price_krw *= (incidental_expense + 1);
                                    }
                                    else
                                    {
                                        price_krw = unit * offer_price * exchange_rate;
                                        price_krw *= (custom + 1);
                                        price_krw *= (tax + 1);
                                        price_krw *= (incidental_expense + 1);
                                    }
                                }
                                    

                                purcahse = price_krw;
                                division = 1;

                                if (price_krw > 0 && offer_price > 0)
                                {
                                    int n = dgvPurchaseUsd.Rows.Add();
                                    string sdate = offerDt.Rows[i]["updatetime"].ToString().Substring(2, 2) + "/" + offerDt.Rows[i]["updatetime"].ToString().Substring(4, 2);
                                    dgvPurchaseUsd.Rows[n].Cells["purchase_date"].Value = sdate;
                                    dgvPurchaseUsd.Rows[n].Cells["price"].Value = purcahse.ToString("#,##0");
                                    dgvPurchaseUsd.Rows[n].Cells["price_krw"].Value = price_krw.ToString("#,##0");
                                    dgvPurchaseUsd.Rows[n].Cells["exchangeRate"].Value = exchange_rate.ToString("#,##0.00");
                                    dgvPurchaseUsd.Rows[n].Cells["price_usd"].Value = offer_price.ToString("#,##0.00");
                                    //dgvPurchaseUsd.Rows[n].Cells["division"].Value = division;
                                    if (division == 1)
                                    {
                                        dgvPurchaseUsd.Rows[n].DefaultCellStyle.ForeColor = Color.Black;
                                        dgvPurchaseUsd.Rows[n].DefaultCellStyle.Font = new Font("고딕", 9, FontStyle.Regular);
                                    }
                                    else
                                    {
                                        dgvPurchaseUsd.Rows[n].DefaultCellStyle.ForeColor = Color.Gray;
                                        dgvPurchaseUsd.Rows[n].DefaultCellStyle.Font = new Font("고딕", 8, FontStyle.Regular);
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "오퍼가+팬딩":
                    {
                        if (customDt.Rows.Count > 0)
                        {
                            for (int i = 0; i < customDt.Rows.Count; i++)
                            {
                                //환율
                                DataRow[] dr = null;
                                double exchange_rate = 0;
                                if (exchangeRateDt.Rows.Count > 0)
                                {
                                    string whr = "base_date = '" + customDt.Rows[i]["eta"].ToString() + "'";
                                    dr = exchangeRateDt.Select(whr);
                                    if (dr.Length > 0)
                                        exchange_rate = Convert.ToDouble(dr[0]["exchange_rate"].ToString());
                                    else if (customDt.Rows[i]["eta"].ToString() == DateTime.Now.ToString("yyyyMM"))
                                    {
                                        if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                                            exchange_rate = 0;
                                    }
                                }
                                //오퍼가 내역이 있으면 대체
                                int division = 0;
                                double purcahse = 0, price_krw = 0, offer_price = 0;
                                double in_purchase = 0, out_purchase = 0;
                                if (cbPurchasePriceType.Text != "매입내역" && offerDt.Rows.Count > 0)
                                {
                                    string whr = "updatetime = '" + customDt.Rows[i]["eta"].ToString() + "'";
                                    dr = offerDt.Select(whr);
                                    if (dr.Length > 0)
                                    {
                                        offer_price = Convert.ToDouble(dr[0]["purchase_price"].ToString());

                                        //고지가
                                        double fixed_tariff;
                                        if (!double.TryParse(dr[0]["fixed_tariff"].ToString(), out fixed_tariff))
                                            fixed_tariff = 0;
                                        fixed_tariff /= 1000;

                                        if (!isWeight)
                                        {
                                            price_krw = cost_unit * offer_price * exchange_rate;
                                            price_krw *= (custom + 1);

                                            price_krw *= (tax + 1);
                                            price_krw *= (incidental_expense + 1);

                                        }
                                        else
                                        {
                                            if (fixed_tariff > 0)
                                            {
                                                price_krw = offer_price * exchange_rate * unit;
                                                price_krw += (fixed_tariff * unit * exchange_rate * custom);

                                                price_krw *= (tax + 1);
                                                price_krw *= (incidental_expense + 1);
                                            }
                                            else
                                            {
                                                price_krw = unit * offer_price * exchange_rate;
                                                price_krw *= (custom + 1);
                                                price_krw *= (tax + 1);
                                                price_krw *= (incidental_expense + 1);
                                            }
                                        }


                                        purcahse = price_krw;
                                        division = 1;
                                    }
                                }
                                //팬딩내역
                                if (division == 0)
                                {
                                    string whr = "eta = '" + customDt.Rows[i]["eta"].ToString() + "'";
                                    dr = customDt.Select(whr);
                                    if (dr.Length > 0)
                                    {
                                        offer_price = Convert.ToDouble(dr[0]["unit_price"].ToString());

                                        if (!isWeight)
                                            price_krw = offer_price * exchange_rate * (1 + tax + custom + incidental_expense) * cost_unit;
                                        else
                                            price_krw = offer_price * exchange_rate * (1 + tax + custom + incidental_expense) * seaover_unit;

                                        purcahse = price_krw;
                                        division = 2;
                                    }
                                }
                                if (price_krw > 0 && offer_price > 0)
                                {
                                    this.dgvPurchaseUsd.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseUsd_CellValueChanged);
                                    int n = dgvPurchaseUsd.Rows.Add();
                                    string sdate = customDt.Rows[i]["eta"].ToString().Substring(2, 2) + "/" + customDt.Rows[i]["eta"].ToString().Substring(4, 2);
                                    dgvPurchaseUsd.Rows[n].Cells["purchase_date"].Value = sdate;

                                    //2023-10-12 중량단가 일때만 단위수량으로 나눔
                                    *//*if (isWeight)
                                    {
                                        purcahse /= unit_count;
                                        price_krw /= unit_count;
                                    }
                                    else*//*
                                    if (rbTrayPrice.Checked && cost_unit > 0)
                                    {
                                        purcahse /= cost_unit;
                                        in_purchase /= cost_unit;
                                        out_purchase /= cost_unit;
                                        price_krw /= cost_unit;
                                    }

                                    dgvPurchaseUsd.Rows[n].Cells["price"].Value = purcahse.ToString("#,##0");
                                    dgvPurchaseUsd.Rows[n].Cells["in_purchase"].Value = in_purchase.ToString("#,##0");
                                    dgvPurchaseUsd.Rows[n].Cells["out_purchase"].Value = out_purchase.ToString("#,##0");
                                    dgvPurchaseUsd.Rows[n].Cells["price_krw"].Value = price_krw.ToString("#,##0");
                                    dgvPurchaseUsd.Rows[n].Cells["exchangeRate"].Value = exchange_rate.ToString("#,##0.00");
                                    dgvPurchaseUsd.Rows[n].Cells["price_usd"].Value = offer_price.ToString("#,##0.00");

                                    //dgvPurchaseUsd.Rows[n].Cells["division"].Value = division;
                                    if (division == 1)
                                    {
                                        dgvPurchaseUsd.Rows[n].DefaultCellStyle.ForeColor = Color.Black;
                                        dgvPurchaseUsd.Rows[n].DefaultCellStyle.Font = new Font("고딕", 9, FontStyle.Regular);
                                    }
                                    else if (division == 2)
                                    {
                                        dgvPurchaseUsd.Rows[n].DefaultCellStyle.ForeColor = Color.Black;
                                        dgvPurchaseUsd.Rows[n].DefaultCellStyle.Font = new Font("고딕", 9, FontStyle.Italic);
                                    }
                                    else
                                    {
                                        dgvPurchaseUsd.Rows[n].DefaultCellStyle.ForeColor = Color.Gray;
                                        dgvPurchaseUsd.Rows[n].DefaultCellStyle.Font = new Font("고딕", 8, FontStyle.Regular);

                                    }
                                    this.dgvPurchaseUsd.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseUsd_CellValueChanged);
                                }
                            }
                        }
                    }
                    break;
                default:
                    {
                        if (purchaseDt.Rows.Count > 0)
                        {
                            for (int i = 0; i < purchaseDt.Rows.Count; i++)
                            {
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
                                    {
                                        if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                                            exchange_rate = 0;
                                    }
                                }
                                //오퍼가 내역이 있으면 대체
                                int division = 0;
                                double purcahse = 0, price_krw = 0, offer_price = 0;
                                double in_purchase = 0, out_purchase = 0;
                                if (cbPurchasePriceType.Text != "매입내역" && offerDt.Rows.Count > 0)
                                {
                                    string whr = "updatetime = '" + purchaseDt.Rows[i]["매입일자"].ToString() + "'";
                                    dr = offerDt.Select(whr);
                                    if (dr.Length > 0)
                                    {
                                        offer_price = Convert.ToDouble(dr[0]["purchase_price"].ToString());

                                        //고지가
                                        double fixed_tariff;
                                        if (!double.TryParse(dr[0]["fixed_tariff"].ToString(), out fixed_tariff))
                                            fixed_tariff = 0;
                                        fixed_tariff /= 1000;

                                        if (!isWeight)
                                        {
                                            price_krw = cost_unit * offer_price * exchange_rate;
                                            price_krw *= (custom + 1);

                                            price_krw *= (tax + 1);
                                            price_krw *= (incidental_expense + 1);

                                        }
                                        else
                                        {
                                            if (fixed_tariff > 0)
                                            {
                                                price_krw = offer_price * exchange_rate * unit;
                                                price_krw += (fixed_tariff * unit * exchange_rate * custom);

                                                price_krw *= (tax + 1);
                                                price_krw *= (incidental_expense + 1);
                                            }
                                            else
                                            {
                                                price_krw = unit * offer_price * exchange_rate;
                                                price_krw *= (custom + 1);
                                                price_krw *= (tax + 1);
                                                price_krw *= (incidental_expense + 1);
                                            }
                                        }
                                            

                                        purcahse = price_krw;
                                        division = 1;
                                    }
                                }
                                //팬딩내역
                                if (cbPurchasePriceType.Text == "오퍼가+팬딩+매입" && division == 0)
                                {

                                    string whr = "eta = '" + purchaseDt.Rows[i]["매입일자"].ToString() + "'";
                                    dr = customDt.Select(whr);
                                    if (dr.Length > 0)
                                    {
                                        offer_price = Convert.ToDouble(dr[0]["unit_price"].ToString());

                                        if (!isWeight)
                                            price_krw = offer_price * exchange_rate * (1 + tax + custom + incidental_expense) * cost_unit;
                                        else
                                            price_krw = offer_price * exchange_rate * (1 + tax + custom + incidental_expense) * seaover_unit;

                                        purcahse = price_krw;
                                        division = 2;
                                    }
                                }


                                //오퍼내역이 없을때
                                if (division == 0)
                                {
                                    purcahse = Convert.ToDouble(purchaseDt.Rows[i]["단가"].ToString());
                                    //KRW 국내매입
                                    in_purchase = Convert.ToDouble(purchaseDt.Rows[i]["단가2"].ToString()) * (1 - purchase_margin);
                                    //KRW 수입매입
                                    out_purchase = Convert.ToDouble(purchaseDt.Rows[i]["단가3"].ToString()) * (1 - purchase_margin);
                                    //매입단가 평균
                                    if (in_purchase > 0 && out_purchase > 0)
                                        price_krw = (in_purchase + out_purchase) / 2;
                                    else if (in_purchase > 0 && out_purchase == 0)
                                        price_krw = in_purchase;
                                    else if (in_purchase == 0 && out_purchase > 0)
                                        price_krw = out_purchase;
                                    else
                                        price_krw = purcahse;
                                    //USD 매입단가
                                    if (!isWeight)
                                        offer_price = price_krw / exchange_rate / cost_unit / (1 + tax + custom + incidental_expense);
                                    else
                                        offer_price = price_krw / exchange_rate / seaover_unit / (1 + tax + custom + incidental_expense);
                                }                               

                                if (price_krw > 0 && offer_price > 0)
                                {
                                    this.dgvPurchaseUsd.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseUsd_CellValueChanged);
                                    int n = dgvPurchaseUsd.Rows.Add();
                                    string sdate = purchaseDt.Rows[i]["매입일자"].ToString().Substring(2, 2) + "/" + purchaseDt.Rows[i]["매입일자"].ToString().Substring(4, 2);
                                    dgvPurchaseUsd.Rows[n].Cells["purchase_date"].Value = sdate;

                                    //2023-10-12 중량단가 일때만 단위수량으로 나눔
                                    *//*if (isWeight)
                                    {
                                        purcahse /= unit_count;
                                        price_krw /= unit_count;
                                    }
                                    else*//* if (rbTrayPrice.Checked && cost_unit > 0)
                                    {
                                        purcahse /= cost_unit;
                                        in_purchase /= cost_unit;
                                        out_purchase /= cost_unit;
                                        price_krw /= cost_unit;
                                    }

                                    dgvPurchaseUsd.Rows[n].Cells["price"].Value = purcahse.ToString("#,##0");
                                    dgvPurchaseUsd.Rows[n].Cells["in_purchase"].Value = in_purchase.ToString("#,##0");
                                    dgvPurchaseUsd.Rows[n].Cells["out_purchase"].Value = out_purchase.ToString("#,##0");
                                    dgvPurchaseUsd.Rows[n].Cells["price_krw"].Value = price_krw.ToString("#,##0");
                                    dgvPurchaseUsd.Rows[n].Cells["exchangeRate"].Value = exchange_rate.ToString("#,##0.00");
                                    dgvPurchaseUsd.Rows[n].Cells["price_usd"].Value = offer_price.ToString("#,##0.00");

                                    //dgvPurchaseUsd.Rows[n].Cells["division"].Value = division;
                                    if (division == 1)
                                    {
                                        dgvPurchaseUsd.Rows[n].DefaultCellStyle.ForeColor = Color.Black;
                                        dgvPurchaseUsd.Rows[n].DefaultCellStyle.Font = new Font("고딕", 9, FontStyle.Regular);
                                    }
                                    else if (division == 2)
                                    {
                                        dgvPurchaseUsd.Rows[n].DefaultCellStyle.ForeColor = Color.Black;
                                        dgvPurchaseUsd.Rows[n].DefaultCellStyle.Font = new Font("고딕", 9, FontStyle.Italic);
                                    }
                                    else
                                    {
                                        dgvPurchaseUsd.Rows[n].DefaultCellStyle.ForeColor = Color.Gray;
                                        dgvPurchaseUsd.Rows[n].DefaultCellStyle.Font = new Font("고딕", 8, FontStyle.Regular);

                                    }
                                    this.dgvPurchaseUsd.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseUsd_CellValueChanged);
                                }
                            }
                        }
                    }
                    break;

            }*/
            //this.dgvPurchaseUsd.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseUsd_CellValueChanged);
            SetPurchaseChart(row);
        }
        private void GetRank(string rank_type, double price, out double ranking, out string rank_detail)
        {
            ranking = 0;
            if (dgvPurchaseUsd.Rows.Count > 0)
            {
                double[] rank = new double[dgvPurchaseUsd.Rows.Count + 1];
                for (int i = 0; i < dgvPurchaseUsd.Rows.Count; i++)
                {
                    if (rank_type == "USD")
                        rank[i] = Convert.ToDouble(dgvPurchaseUsd.Rows[i].Cells["price_usd"].Value.ToString());
                    else
                        rank[i] = Convert.ToDouble(dgvPurchaseUsd.Rows[i].Cells["price_krw"].Value.ToString());
                }
                rank[dgvPurchaseUsd.Rows.Count] = price;

                Array.Sort(rank);
                for (int i = 0; i < rank.Length; i++)
                {
                    if (rank[i] == price)
                    {
                        ranking = i;
                        break;
                    }
                }
            }

            rank_detail = "▼" + ranking.ToString() + "  ▲" + (dgvPurchaseUsd.Rows.Count - ranking).ToString();
            ranking = ranking / dgvPurchaseUsd.Rows.Count * 100;
        }
        private void SetPurchaseChart(DataGridViewRow select_row)
        {
            if (dgvPurchaseUsd.Rows.Count > 0)
            {
                double max_krw = 0, min_krw = 9999999, max_usd = 0, min_usd = 9999999;
                for (int i = 0; i < dgvPurchaseUsd.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvPurchaseUsd.Rows[i];
                    double price = Convert.ToDouble(row.Cells["price"].Value.ToString());
                    double price_krw = Convert.ToDouble(row.Cells["price_krw"].Value.ToString());
                    double offer_price = Convert.ToDouble(row.Cells["price_usd"].Value.ToString());
                    double comparison_price;
                    //Chart   MAX,Min
                    if (select_row.Cells["origin"].Value.ToString() == "국산")
                        comparison_price = Convert.ToDouble(row.Cells["price"].Value.ToString());                        
                    else
                        comparison_price = Convert.ToDouble(row.Cells["price"].Value.ToString());
                    //최대, 최소
                    if (max_krw < comparison_price)
                        max_krw = comparison_price;
                    if (min_krw > comparison_price)
                        min_krw = comparison_price;
                    if (max_usd < offer_price)
                        max_usd = offer_price;
                    if (min_usd > offer_price)
                        min_usd = offer_price;
                }
                lbMaxMin.Text =  min_krw.ToString("#,##0") + " ~ " + max_krw.ToString("#,##0");
            }
        }
        #endregion

        #region Key event
        private void txtPurchaseMargin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double purchase_margin;
                if (!double.TryParse(txtPurchaseMargin.Text, out purchase_margin))
                {
                    MessageBox.Show(this,"매입마진의 값이 숫자형식이 아닙니다.");
                    this.Activate();
                    return;
                }

                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];
                    if (Convert.ToBoolean(row.Cells["chk"].Value))
                    {
                        row.Cells["purchase_margin"].Value = purchase_margin;
                        GetPurchase(row);
                        return;
                    }
                }
            }
        }
        private void txtStandardOfShort_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                DataGridViewRow row = GetSelectProductDgvr();
                dgvContract.Rows.Clear();
                dgvStockSales.Rows.Clear();
                dgvStockSales.Rows.Add();
                //유효성검사
                if ((row.Cells["product"].Value == null || string.IsNullOrEmpty(row.Cells["product"].Value.ToString()))
                    || (row.Cells["origin"].Value == null || string.IsNullOrEmpty(row.Cells["origin"].Value.ToString()))
                    || (row.Cells["sizes"].Value == null || string.IsNullOrEmpty(row.Cells["sizes"].Value.ToString()))
                    || (row.Cells["unit"].Value == null || string.IsNullOrEmpty(row.Cells["unit"].Value.ToString())))
                    return;
                ShortManagerModel smModel = GetProductStockInfo(row);

                SetPendingList(smModel, row);
                this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            }
        }
        private void txtExcludeStock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                DataGridViewRow row = GetSelectProductDgvr();
                dgvContract.Rows.Clear();
                dgvStockSales.Rows.Clear();
                dgvStockSales.Rows.Add();
                //유효성검사
                if ((row.Cells["product"].Value == null || string.IsNullOrEmpty(row.Cells["product"].Value.ToString()))
                    || (row.Cells["origin"].Value == null || string.IsNullOrEmpty(row.Cells["origin"].Value.ToString()))
                    || (row.Cells["sizes"].Value == null || string.IsNullOrEmpty(row.Cells["sizes"].Value.ToString()))
                    || (row.Cells["unit"].Value == null || string.IsNullOrEmpty(row.Cells["unit"].Value.ToString())))
                    return;
                ShortManagerModel smModel = GetProductStockInfo(row);

                SetPendingList(smModel, row);
                this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            }
        }
        private void nudUntilDays_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataGridViewRow row = GetSelectProductDgvr();
                if (row != null)
                    GetSales2(row);
            }
        }
        private void txtTrq_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                DataGridViewRow row = GetSelectProductDgvr();
                if (row != null)
                {
                    dgvContract.Rows.Clear();
                    dgvStockSales.Rows.Clear();
                    dgvStockSales.Rows.Add();
                    //유효성검사
                    if ((row.Cells["product"].Value == null || string.IsNullOrEmpty(row.Cells["product"].Value.ToString()))
                        || (row.Cells["origin"].Value == null || string.IsNullOrEmpty(row.Cells["origin"].Value.ToString()))
                        || (row.Cells["sizes"].Value == null || string.IsNullOrEmpty(row.Cells["sizes"].Value.ToString()))
                        || (row.Cells["unit"].Value == null || string.IsNullOrEmpty(row.Cells["unit"].Value.ToString())))
                        return;
                    ShortManagerModel smModel = GetProductStockInfo(row);

                    SetPendingList(smModel, row);

                }
                this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                //수정
                if (MessageBox.Show(this, "TRQ 기준금액을 변경하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    double trq_price;
                    if (!double.TryParse(txtTrq.Text.Replace(",", ""), out trq_price))
                        trq_price = 0;

                    if (commonRepository.UpdateTrq(trq_price) == -1)
                        MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                    else
                        MessageBox.Show(this, "완료");
                    this.Activate();
                }
            }
        }
        private void txtExchangeRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                DataGridViewRow row = GetSelectProductDgvr();

                ShortManagerModel smModel = GetProductStockInfo(row);
                if (SortSetting())
                    CalculateStock(smModel);

                /*if (row != null)
                {
                    dgvContract.Rows.Clear();
                    dgvStockSales.Rows.Clear();
                    dgvStockSales.Rows.Add();
                    //유효성검사
                    if ((row.Cells["product"].Value == null || string.IsNullOrEmpty(row.Cells["product"].Value.ToString()))
                        || (row.Cells["origin"].Value == null || string.IsNullOrEmpty(row.Cells["origin"].Value.ToString()))
                        || (row.Cells["sizes"].Value == null || string.IsNullOrEmpty(row.Cells["sizes"].Value.ToString()))
                        || (row.Cells["unit"].Value == null || string.IsNullOrEmpty(row.Cells["unit"].Value.ToString())))
                        return;
                    ShortManagerModel smModel = GetProductStockInfo(row);

                    SetPendingList(smModel, row);    
                }*/


                this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            }
        }
        private void txtOfferSttdate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetPurchasePriceList(GetSelectProductDgvr());
            }
        }
        private void txtSttdate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetMarketPriceByCompany(GetSelectProductDgvr());
            }
        }
        private void txtOfferPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double offer_price;
                if (!double.TryParse(txtOfferPrice.Text, out offer_price))
                    offer_price = 0;
                double exchange_rate;
                if (!double.TryParse(txtExchangeRate2.Text, out exchange_rate))
                    exchange_rate = 0;
                txtOfferCostPrice.Text = calculateCostPrice(offer_price, exchange_rate).ToString("#,##0");
                CalculateAverageCostPrice();
            }
        }

        private void txtInQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CalculateAverageCostPrice();
            }
        }
        private void DetailDashBoard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        GetProduct();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                    case Keys.M:
                        txtProduct.Focus();
                        break;
                    case Keys.N:
                        txtProduct.Text = String.Empty;
                        txtOrigin.Text = String.Empty;
                        txtSizes.Text = String.Empty;
                        txtUnit.Text = String.Empty;
                        txtManager1.Text = String.Empty;
                        txtManager2.Text = String.Empty;
                        txtDivision.Text = String.Empty;
                        txtProduct.Focus();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                int rowindex;
                switch (e.KeyCode)
                {
                    case Keys.P:
                        Printing();
                        break;
                    case Keys.Up:
                        dgvProduct.ClearSelection();
                        rowindex = -1;
                        for (int i = 0; i < dgvProduct.Rows.Count; i++)
                        {
                            bool isChecked = Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value);
                            if (dgvProduct.Rows[i].Visible && isChecked)
                            {
                                rowindex = i;
                                dgvProduct.Rows[i].Cells["chk"].Value = false;
                            }
                        }

                        dgvProduct.ClearSelection();
                        if (rowindex - 1 < 0)
                        {
                            dgvProduct.Rows[dgvProduct.Rows.Count - 1].Cells["chk"].Value = true;
                            dgvProduct.Rows[dgvProduct.Rows.Count - 1].Selected = true;
                            dgvProduct.FirstDisplayedScrollingRowIndex = dgvProduct.Rows.Count - 1;
                        }
                        else
                        {
                            dgvProduct.Rows[rowindex - 1].Cells["chk"].Value = true;
                            dgvProduct.Rows[rowindex - 1].Selected = true;
                            dgvProduct.FirstDisplayedScrollingRowIndex = rowindex - 1;
                        }
                        e.Handled = true;

                        break;
                    case Keys.Down:
                        dgvProduct.ClearSelection();
                        rowindex = -1;
                        for (int i = 0; i < dgvProduct.Rows.Count; i++)
                        {
                            bool isChecked = Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value);
                            if (dgvProduct.Rows[i].Visible && isChecked)
                            {
                                rowindex = i;
                                dgvProduct.Rows[i].Cells["chk"].Value = false;
                            }
                        }

                        dgvProduct.ClearSelection();
                        if (rowindex + 1 >= dgvProduct.Rows.Count)
                        {
                            dgvProduct.Rows[0].Cells["chk"].Value = true;
                            dgvProduct.Rows[0].Selected = true;
                            dgvProduct.FirstDisplayedScrollingRowIndex = dgvProduct.Rows.Count - 1;
                        }
                        else
                        {
                            dgvProduct.Rows[rowindex + 1].Cells["chk"].Value = true;
                            dgvProduct.Rows[rowindex + 1].Selected = true;
                            dgvProduct.FirstDisplayedScrollingRowIndex = rowindex + 1;
                        }
                        e.Handled = true;
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        break;
                    case Keys.F2:
                        break;
                    case Keys.F5:
                        btnRefresh.PerformClick();
                        break;
                    case Keys.F8:
                        btnBookmark.PerformClick();
                        break;
                }
            }
        }
        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetProduct();
        }
        #endregion

        #region Button, Checkbox
        private void cbAtoSale_CheckedChanged(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
            {
                this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                GetSales(row);
                GetSales2(row);
                SetDashboardSummary();
                this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            }
        }

        private void cbMultiplyTray_CheckedChanged(object sender, EventArgs e)
        {
            this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            DataGridViewRow row = GetSelectProductDgvr();
            dgvContract.Rows.Clear();
            dgvStockSales.Rows.Clear();
            dgvStockSales.Rows.Add();
            if (row != null)
            {
                //유효성검사
                if ((row.Cells["product"].Value == null || string.IsNullOrEmpty(row.Cells["product"].Value.ToString()))
                    || (row.Cells["origin"].Value == null || string.IsNullOrEmpty(row.Cells["origin"].Value.ToString()))
                    || (row.Cells["sizes"].Value == null || string.IsNullOrEmpty(row.Cells["sizes"].Value.ToString()))
                    || (row.Cells["unit"].Value == null || string.IsNullOrEmpty(row.Cells["unit"].Value.ToString())))
                    return;
                ShortManagerModel smModel = GetProductStockInfo(row);

                SetPendingList(smModel, row);
                //오퍼매입가 
                GetOffer(row);
                GetPurchase(row);
            }
            this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);

        }

        private void rbTrayPrice_CheckedChanged(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
                GetPurchase(row);
        }
        private void btnHide_Click(object sender, EventArgs e)
        {
            pnTooltip.Visible = false;
            pnTooltip.Enabled = false;

            double total_unpending_qty = 0, total_pending_qty = 0;
            foreach (DataGridViewRow row in dgvSeaoverCostPrice.Rows)
            {
                if (row.Cells["cost_price_chk"].Value != null && bool.TryParse(row.Cells["cost_price_chk"].Value.ToString(), out bool isChecked) && isChecked)
                {
                    double qty;
                    if (row.Cells["seaover_qty"].Value == null || !double.TryParse(row.Cells["seaover_qty"].Value.ToString(), out qty))
                        qty = 0;

                    if (row.Cells["seaover_qty"].Value != null && row.Cells["seaover_qty"].Value.ToString().Equals("통관"))
                        total_pending_qty += qty;
                    else
                        total_unpending_qty += qty;
                }
            }

            if (dgvStockSales.Rows.Count > 0)
            {
                dgvStockSales.Rows[0].Cells["seaover_cost_price"].Value = txtSeaoverCostPrice.Text;
                dgvStockSales.Rows[0].Cells["seaover_unpending_qty"].Value = total_unpending_qty.ToString("#,##0");
                dgvStockSales.Rows[0].Cells["seaover_pending_qty"].Value = total_pending_qty.ToString("#,##0");

                double reserved_qty;
                if (dgvStockSales.Rows[0].Cells["reserved_qty"].Value == null || !double.TryParse(dgvStockSales.Rows[0].Cells["reserved_qty"].Value.ToString(), out reserved_qty))
                    reserved_qty = 0;

                double total_qty = total_unpending_qty + total_pending_qty;
                if (cbReserved.Checked)
                    total_qty -= reserved_qty;
                    
                dgvStockSales.Rows[0].Cells["total_qty"].Value = total_qty.ToString("#,##0");

                this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                dgvContract.Rows[0].Cells["etd_cost_price"].Value = txtSeaoverCostPrice.Text;
                dgvContract.Rows[0].Cells["warehouse_qty"].Value = total_qty.ToString("#,##0");
                CalculateStock();
                this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            }
        }
        private void btnContractDelete_Click(object sender, EventArgs e)
        {
            if (dgvContract.Rows.Count > 2)
            {
                if (MessageBox.Show(this, "임의로 등록한 모든 국내수입 내역을 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;

                this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);

                List<StringBuilder> sqlList = new List<StringBuilder>();
                for (int i = 0; i < dgvContract.Rows.Count; i++)
                {
                    int id;
                    if (dgvContract.Rows[i].Cells["domestic_id"].Value == null || !int.TryParse(dgvContract.Rows[i].Cells["domestic_id"].Value.ToString(), out id))
                        id = 0;
                    if (id > 0)
                    {
                        DomesticModel model = new DomesticModel();
                        model.id = id;

                        StringBuilder sql = domesticRepository.DeleteDomestic(model);
                        sqlList.Add(sql);
                    }
                }
                //Execute
                if (sqlList.Count > 0 && commonRepository.UpdateTran(sqlList) == -1)
                {
                    MessageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                    this.Activate();
                    return;
                }

                //dgv 반영
                dgvContract.Rows.Remove(dgvContract.Rows[dgvContract.Rows.Count - 1]);
                for (int i = dgvContract.Rows.Count - 1; i >= 0; i--)
                {
                    int id;
                    if (dgvContract.Rows[i].Cells["domestic_id"].Value == null || !int.TryParse(dgvContract.Rows[i].Cells["domestic_id"].Value.ToString(), out id))
                        id = 0;
                    if (id > 0)
                        dgvContract.Rows.Remove(dgvContract.Rows[i]);
                }
                ShortManagerModel smModel = GetProductStockInfo(GetSelectProductDgvr());
                if (SortSetting())
                    CalculateStock(smModel);

                this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            }
        }

        private void btnRegisters_Click(object sender, EventArgs e)
        {
            if (dgvContract.Rows.Count > 2)
            {
                DataGridViewRow row = GetSelectProductDgvr();
                if (row == null)
                {
                    MessageBox.Show(this, "품목정보를 찾을 수 없습니다. 품목을 다시 조회해주세요");
                    this.Activate();
                    return;
                }

                if (MessageBox.Show(this, "국내수입 내역을 저장하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;

                int newid = commonRepository.GetNextId("t_domestic", "id");
                List<StringBuilder> sqlList = new List<StringBuilder>();
                for (int i = 0; i < dgvContract.Rows.Count; i++)
                {
                    bool isDomestic = Convert.ToBoolean(dgvContract.Rows[i].Cells["is_domestic"].Value);
                    if (isDomestic)
                    {
                        DomesticModel model = new DomesticModel();
                        //ID
                        int id;
                        if (dgvContract.Rows[i].Cells["domestic_id"].Value == null || !int.TryParse(dgvContract.Rows[i].Cells["domestic_id"].Value.ToString(), out id))
                            id = 0;
                        if (id == 0)
                        {
                            id = newid;
                            newid++;
                        }
                        dgvContract.Rows[i].Cells["domestic_id"].Value = model.id;
                        //입고일자
                        DateTime warehousing_date;
                        if (dgvContract.Rows[i].Cells["warehousing_date"].Value == null || !DateTime.TryParse(dgvContract.Rows[i].Cells["warehousing_date"].Value.ToString(), out warehousing_date))
                        {
                            dgvContract.CurrentCell = dgvContract.Rows[i].Cells["warehousing_date"];
                            MessageBox.Show(this, "입고일자를 입력해주세요.");
                            this.Activate();
                            return;
                        }
                        //입고수량
                        double qty;
                        if (dgvContract.Rows[i].Cells["qty"].Value == null || !double.TryParse(dgvContract.Rows[i].Cells["qty"].Value.ToString(), out qty))
                        {
                            MessageBox.Show(this, "수량을 입력해주세요.");
                            this.Activate();
                            return;
                        }
                        //원가
                        double cost_price;
                        if (dgvContract.Rows[i].Cells["etd_cost_price"].Value == null || !double.TryParse(dgvContract.Rows[i].Cells["etd_cost_price"].Value.ToString(), out cost_price))
                            cost_price = 0;

                        model.id = id;
                        model.sub_id = 1;
                        model.status = "선적전";
                        model.warehousing_date = warehousing_date.ToString("yyyy-MM-dd");
                        model.product = row.Cells["product"].Value.ToString();
                        model.origin = row.Cells["origin"].Value.ToString();
                        model.sizes = row.Cells["sizes"].Value.ToString();
                        model.unit = row.Cells["unit"].Value.ToString();
                        model.price_unit = row.Cells["price_unit"].Value.ToString();
                        model.unit_count = row.Cells["unit_count"].Value.ToString();
                        model.seaover_unit = row.Cells["seaover_unit"].Value.ToString();
                        model.qty = qty;
                        model.cost_price = cost_price;
                        model.edit_user = um.user_name;
                        model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        StringBuilder sql = domesticRepository.DeleteDomestic(model);
                        sqlList.Add(sql);
                        sql = domesticRepository.InsertDomestic(model);
                        sqlList.Add(sql);
                    }

                }
                if (commonRepository.UpdateTran(sqlList) == -1)
                    MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                else
                    MessageBox.Show(this, "수정완료");
                this.Activate();
            }
        }
        private void btnPrinting_Click(object sender, EventArgs e)
        {
            Printing();
        }
        private void btnProductInfo_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
            {
                ContractRecommendationManager2 crm = new ContractRecommendationManager2(um, row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString());
                crm.Show();
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
            {
                row.Cells["chk"].Value = false;
                row.Cells["chk"].Value = true;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetProduct();
        }
        private void btnCalendarOfferSttdate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
            {
                txtOfferSttdate.Text = sdate;
                GetPurchasePriceList(GetSelectProductDgvr());
            }
        }

        private void btnCalendarOfferEnddate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
            {
                txtOfferEnddate.Text = sdate;
                GetPurchasePriceList(GetSelectProductDgvr());
            }
        }
        private void cbPurchasePriceType_SelectedIndexChanged(object sender, EventArgs e)
        {


            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value))
                {
                    GetPurchase(dgvProduct.Rows[i]);
                    break;
                }
            }
        }
        private void btnCalendarSttdate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
            {
                txtSttdate.Text = sdate;
                GetMarketPriceByCompany(GetSelectProductDgvr());
            }
        }

        private void btnCalendarEnddate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
            {
                txtEnddate.Text = sdate;
                GetMarketPriceByCompany(GetSelectProductDgvr());
            }
                
        }
        private void btnBookmark_Click(object sender, EventArgs e)
        {
            BookmarkManager bm = new BookmarkManager(um, this);
            bm.StartPosition = FormStartPosition.CenterParent;
            bm.ShowDialog();
        }
        private void btnPlus_Click(object sender, EventArgs e)
        {
            int n = dgvContract.Rows.Add();
            int term;
            try
            {
                term = Convert.ToInt16(txtShippingTerm.Text) + Convert.ToInt16(txtPendingTerm.Text);
            }
            catch
            {
                term = 0;
            }
            dgvContract.Rows[n].Cells["pending_term"].Value = term;
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            if (dgvContract.Rows.Count > 0)
            {
                dgvContract.Rows.Remove(dgvContract.Rows[dgvContract.Rows.Count - 1]);
            }
        }

        #endregion

        #region 그룹 데이터 가져오기
        public void SetGroupData(int id)
        {
            string whr;
            double exchange_rate;
            if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                exchange_rate = 0;

            dgvProduct.Rows.Clear();
            DataTable groupDt = groupRepository.GetGroup(id);
            if (groupDt.Rows.Count > 0)
            {   //매출단가, 최저단가, 일반시세
                priceComparisonRepository.SetSeaoverId(um.seaover_id);
                DataTable priceDt = priceComparisonRepository.GetPriceComparisonDataTable(DateTime.Now, 3, "", "", "", "", "", "", "", ""
                                                                , DateTime.Now.AddDays(-15), DateTime.Now
                                                                , false);
                //대표품목
                DataTable pgDt = productGroupRepository.GetProductGroup(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
                //담당자1, 담당자2 전용 품목 DATA
                DataTable mngDt = seaoverRepository.GetProductTapble("", txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtManager1.Text, txtManager2.Text, txtDivision.Text);
                List<string[]> productInfoList = new List<string[]>();
                for (int i = 0; i < groupDt.Rows.Count; i++)
                {
                    int n = dgvProduct.Rows.Add();
                    DataGridViewRow row = dgvProduct.Rows[n];
                    row.Cells["category"].Value = groupDt.Rows[i]["category"].ToString();
                    row.Cells["category_code"].Value = "";
                    row.Cells["product"].Value = groupDt.Rows[i]["product"].ToString();
                    row.Cells["product_code"].Value = groupDt.Rows[i]["product_code"].ToString();
                    row.Cells["origin"].Value = groupDt.Rows[i]["origin"].ToString();
                    row.Cells["origin_code"].Value = groupDt.Rows[i]["origin_code"].ToString();
                    row.Cells["sizes"].Value = groupDt.Rows[i]["sizes"].ToString();
                    row.Cells["sizes_code"].Value = groupDt.Rows[i]["sizes_code"].ToString();
                    row.Cells["unit"].Value = groupDt.Rows[i]["unit"].ToString();
                    row.Cells["price_unit"].Value = groupDt.Rows[i]["price_unit"].ToString();
                    row.Cells["unit_count"].Value = groupDt.Rows[i]["unit_count"].ToString();
                    row.Cells["seaover_unit"].Value = groupDt.Rows[i]["seaover_unit"].ToString();
                    row.Cells["cost_unit"].Value = groupDt.Rows[i]["cost_unit"].ToString();

                    row.Cells["tax"].Value = groupDt.Rows[i]["tax"].ToString();
                    row.Cells["custom"].Value = groupDt.Rows[i]["custom"].ToString();
                    row.Cells["incidental_expense"].Value = groupDt.Rows[i]["incidental_expense"].ToString();

                    bool isWeight = Convert.ToBoolean(groupDt.Rows[i]["weight_calculate"].ToString());
                    double unit = Convert.ToDouble(groupDt.Rows[i]["unit"].ToString());
                    double cost_unit;
                    if (!double.TryParse(groupDt.Rows[i]["cost_unit"].ToString(), out cost_unit))
                        cost_unit = 0;
                    if (!isWeight && cost_unit == 0)
                        isWeight = true;
                    row.Cells["weight_calculate"].Value = isWeight;

                    double tax = Convert.ToDouble(groupDt.Rows[i]["tax"].ToString()) / 100;
                    double custom = Convert.ToDouble(groupDt.Rows[i]["custom"].ToString()) / 100;
                    double incidental_expense = Convert.ToDouble(groupDt.Rows[i]["incidental_expense"].ToString()) / 100;

                    row.Cells["purchase_margin"].Value = groupDt.Rows[i]["purchase_margin"].ToString();
                    row.Cells["production_days"].Value = groupDt.Rows[i]["production_days"].ToString();
                    //초기화
                    row.Cells["manager1"].Value = "";
                    row.Cells["manager2"].Value = "";
                    row.Cells["division"].Value = "";
                    row.Cells["seaover_current_sales_price"].Value = "0";
                    row.Cells["seaover_current_purchase_price"].Value = "0";
                    row.Cells["seaover_current_normal_price"].Value = "0";

                    string merge_product = "";
                    if (pgDt.Rows.Count > 0)
                    {
                        //1.서브품목으로 등록되있는지 확인
                        whr = "item_product = '" + groupDt.Rows[i]["product"].ToString().Trim() + "'"
                            + " AND item_origin = '" + groupDt.Rows[i]["origin"].ToString().Trim() + "'"
                            + " AND item_sizes = '" + groupDt.Rows[i]["sizes"].ToString().Trim() + "'"
                            + " AND item_unit = '" + groupDt.Rows[i]["unit"].ToString().Trim() + "'"
                            + " AND item_price_unit = '" + groupDt.Rows[i]["price_unit"].ToString().Trim() + "'"
                            + " AND item_unit_count = '" + groupDt.Rows[i]["unit_count"].ToString().Trim() + "'"
                            + " AND item_seaover_unit = '" + groupDt.Rows[i]["seaover_unit"].ToString().Trim() + "'";

                        DataRow[] dtRow = null;
                        dtRow = pgDt.Select(whr);
                        if (dtRow.Length > 0)
                        {
                            //3.병합품목의 서브 품목을 검색
                            whr = "product = '" + dtRow[0]["product"].ToString() + "'"
                            + " AND origin = '" + dtRow[0]["origin"].ToString() + "'"
                            + " AND sizes = '" + dtRow[0]["sizes"].ToString() + "'"
                            + " AND unit = '" + dtRow[0]["unit"].ToString() + "'"
                            + " AND price_unit = '" + dtRow[0]["price_unit"].ToString() + "'"
                            + " AND unit_count = '" + dtRow[0]["unit_count"].ToString() + "'"
                            + " AND seaover_unit = '" + dtRow[0]["seaover_unit"].ToString() + "'";
                            dtRow = pgDt.Select(whr);
                            if (dtRow.Length > 0)
                            {
                                for (int j = 0; j < dtRow.Length; j++)
                                {
                                    //병합된 품목에 추가
                                    merge_product += "\n" + dtRow[j]["item_product"].ToString()
                                                + "^" + dtRow[j]["item_origin"].ToString()
                                                + "^" + dtRow[j]["item_sizes"].ToString()
                                                + "^" + dtRow[j]["item_unit"].ToString()
                                                + "^" + dtRow[j]["item_price_unit"].ToString()
                                                + "^" + dtRow[j]["item_unit_count"].ToString()
                                                + "^" + dtRow[j]["item_seaover_unit"].ToString();
                                }
                            }
                        }
                    }
                    row.Cells["merge_product"].Value = merge_product;
                    if (!string.IsNullOrEmpty(merge_product))
                        row.Cells["is_merge"].Value = true;
                    else
                        row.Cells["is_merge"].Value = false;
                    //row.Cells["manager"].Value = groupDt.Rows[i]["manager"].ToString();
                    //담당자1, 2=======================================================================================
                    DataRow[] dr;
                    
                    if (mngDt.Rows.Count > 0)
                    {
                        whr = "품명 = '" + row.Cells["product"].Value.ToString() + "'"
                        + " AND 원산지 = '" + row.Cells["origin"].Value.ToString() + "'"
                        + " AND 규격 = '" + row.Cells["sizes"].Value.ToString() + "'"
                        + " AND 단위 = '" + row.Cells["unit"].Value.ToString() + "'"
                        + " AND 가격단위 = '" + row.Cells["price_unit"].Value.ToString() + "'"
                        + " AND 단위수량 = " + row.Cells["unit_count"].Value.ToString()
                        + " AND SEAOVER단위 = '" + row.Cells["seaover_unit"].Value.ToString() + "'";
                        dr = mngDt.Select(whr);
                        if (dr.Length > 0)
                        {
                            row.Cells["manager1"].Value = dr[0]["담당자1"].ToString();
                            row.Cells["manager2"].Value = dr[0]["담당자2"].ToString();
                            row.Cells["division"].Value = dr[0]["구분"].ToString();
                        }
                    }
                    //매출, 최저, 일반시세=======================================================================================
                    if (priceDt.Rows.Count > 0)
                    {
                        whr = "품명 = '" + row.Cells["product"].Value.ToString() + "'"
                        + " AND 원산지 = '" + row.Cells["origin"].Value.ToString() + "'"
                        + " AND 규격 = '" + row.Cells["sizes"].Value.ToString() + "'"
                        + " AND 단위 = '" + row.Cells["unit"].Value.ToString() + "'"
                        + " AND 가격단위 = '" + row.Cells["price_unit"].Value.ToString() + "'"
                        + " AND 단위수량 = " + row.Cells["unit_count"].Value.ToString()
                        + " AND SEAOVER단위 = '" + row.Cells["seaover_unit"].Value.ToString() + "'"
                        + " AND 최저단가 > 10 ";
                        dr = priceDt.Select(whr);
                        if (dr.Length > 0)
                        {
                            double sales_price;
                            if (!double.TryParse(dr[0]["매출단가"].ToString(), out sales_price))
                                sales_price = 0;
                            row.Cells["seaover_current_sales_price"].Value = sales_price.ToString("#,##0");
                            double purchase_price;
                            if (!double.TryParse(dr[0]["최저단가"].ToString(), out purchase_price))
                                purchase_price = 0;
                            row.Cells["seaover_current_purchase_price"].Value = purchase_price.ToString("#,##0");
                            double normal_price;
                            if (!double.TryParse(dr[0]["일반시세"].ToString(), out normal_price))
                                normal_price = 0;
                            row.Cells["seaover_current_normal_price"].Value = normal_price.ToString("#,##0");
                        }
                        else
                        {
                            whr = "품명 = '" + row.Cells["product"].Value.ToString() + "'"
                            + " AND 원산지 = '" + row.Cells["origin"].Value.ToString() + "'"
                            + " AND 규격 = '" + row.Cells["sizes"].Value.ToString() + "'"
                            + " AND 단위 = '" + row.Cells["unit"].Value.ToString() + "'"
                            + " AND 가격단위 = '" + row.Cells["price_unit"].Value.ToString() + "'"
                            + " AND 단위수량 = " + row.Cells["unit_count"].Value.ToString()
                            + " AND SEAOVER단위 = '" + row.Cells["seaover_unit"].Value.ToString() + "'";
                            dr = priceDt.Select(whr);
                            if (dr.Length > 0)
                            {
                                double sales_price;
                                if (!double.TryParse(dr[0]["매출단가"].ToString(), out sales_price))
                                    sales_price = 0;
                                row.Cells["seaover_current_sales_price"].Value = sales_price.ToString("#,##0");
                                double purchase_price;
                                if (!double.TryParse(dr[0]["최저단가"].ToString(), out purchase_price))
                                    purchase_price = 0;
                                row.Cells["seaover_current_purchase_price"].Value = purchase_price.ToString("#,##0");
                                double normal_price;
                                if (!double.TryParse(dr[0]["일반시세"].ToString(), out normal_price))
                                    normal_price = 0;
                                row.Cells["seaover_current_normal_price"].Value = normal_price.ToString("#,##0");
                            }
                        }
                    }
                }
                //첫번째 레코드 선택
                if (dgvProduct.Rows.Count > 0)
                {
                    dgvProduct.ClearSelection();
                    //dgvProduct.Rows[0].Cells["chk"].Value = true;
                }
            }
        }
        #endregion

        #region 역대 판매내역 Method
        public void GetSales(DataGridViewRow row)
        {
            //초기화
            dgvDetails.Rows.Clear();
            dgvDetails.Columns.Clear();
            //기준년월
            DataGridViewColumn col = new DataGridViewTextBoxColumn();
            col.Name = "base_date";
            col.HeaderText = "기준";
            col.Width = 40;
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetails.Columns.Add(col);
            int stt_year = DateTime.Now.AddYears(-3).Year;
            int end_year = DateTime.Now.Year;
            DateTime sttdate = new DateTime(stt_year, 1, 1);
            DateTime enddate = new DateTime(end_year, 12, 1);

            int terms_type;
            switch (cbTermsType.Text)
            {
                case "초~말":
                    terms_type = 1;
                    break;
                case "5일":
                    terms_type = 5;
                    break;
                case "10일":
                    terms_type = 10;
                    break;
                case "15일":
                    terms_type = 15;
                    break;
                case "20일":
                    terms_type = 20;
                    break;
                case "25일":
                    terms_type = 25;
                    break;
                default:
                    terms_type = 1;
                    break;
            }

            string merge_product = "";
            bool isMerge = false;
            if (row.Cells["merge_product"].Value != null && !string.IsNullOrEmpty(row.Cells["merge_product"].Value.ToString()))
            {
                merge_product = row.Cells["merge_product"].Value.ToString();
                isMerge = true;
            }

            //Get Data
            //제외매출
            DataTable eDt = productExcludedSalesRepository.GetExcludedSalesByMonth(DateTime.Now.AddMonths(-19).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd")
                                                                            , row.Cells["product"].Value.ToString()
                                                                            , row.Cells["origin"].Value.ToString()
                                                                            , row.Cells["sizes"].Value.ToString()
                                                                            , row.Cells["unit"].Value.ToString()
                                                                            , row.Cells["price_unit"].Value.ToString()
                                                                            , row.Cells["unit_count"].Value.ToString()
                                                                            , row.Cells["seaover_unit"].Value.ToString()
                                                                            , merge_product, isMerge);
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
            //매출량
            DataTable salesDt = salesRepository.GetSalesGroupMonth(sttdate, enddate
                            , row.Cells["product"].Value.ToString()
                            , row.Cells["origin"].Value.ToString()
                            , row.Cells["sizes"].Value.ToString()
                            , row.Cells["seaover_unit"].Value.ToString(), merge_product, isMerge, terms_type, cbAtoSale.Checked);
            if (salesDt.Rows.Count > 0)
            {
                for (int i = stt_year; i <= end_year; i++)
                {
                    //매출수
                    DataGridViewColumn col1 = new DataGridViewTextBoxColumn();
                    col1.Name = "sales_qty_" + i;
                    col1.HeaderText = i + "년";
                    col1.ToolTipText = i.ToString();
                    col1.Width = 60;
                    col1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvDetails.Columns.Add(col1);

                    DataGridViewColumn col2 = new DataGridViewTextBoxColumn();
                    col2.Name = "prepare_pre_month_sales_qty_" + i;
                    col2.HeaderText = "전월";
                    col2.ToolTipText = i.ToString();
                    col2.Width = 50;
                    col2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col2.DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);
                    col2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvDetails.Columns.Add(col2);
                    col2.Visible = false;
                }
                //마지막 년도는 항상 띄워놓기
                DataGridViewColumn col3 = new DataGridViewTextBoxColumn();
                col3.Name = "prepare_pre_month_sales_qty";
                col3.HeaderText = "전월";
                col3.Width = 60;
                col3.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvDetails.Columns.Add(col3);

                DataGridViewColumn col4 = new DataGridViewTextBoxColumn();
                col4.Name = "prepare_pre_year_sales_qty";
                col4.HeaderText = "전년";
                col4.Width = 60;
                col4.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvDetails.Columns.Add(col4);

                //월출력
                int n;
                for (int i = 1; i <= 12; i++)
                {
                    n = dgvDetails.Rows.Add();
                    dgvDetails.Rows[n].Cells["base_date"].Value = i.ToString() + "월";
                    n = dgvDetails.Rows.Add();
                    dgvDetails.Rows[n].Cells["base_date"].Value = "전년";
                    dgvDetails.Rows[n].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
                    dgvDetails.Rows[n].Visible = false;
                }
                /*n = dgvDetails.Rows.Add();
                dgvDetails.Rows[n].Cells["base_date"].Value = "합계";*/

                //데이터출력
                Dictionary<DateTime, double> salesDic = new Dictionary<DateTime, double>();
                for (int i = 0; i < salesDt.Rows.Count; i++)
                {
                    DateTime base_date = Convert.ToDateTime(salesDt.Rows[i]["기준일자"].ToString().Substring(0, 4) + "-" + salesDt.Rows[i]["기준일자"].ToString().Substring(4, 2) + "-" + "01");
                    double sales;
                    if (!double.TryParse(salesDt.Rows[i]["매출수"].ToString(), out sales))
                        sales = 0;

                    //제외매출
                    if (eDt.Rows.Count > 0)
                    {
                        DateTime sDate = base_date;
                        DateTime eDate = base_date.AddMonths(1).AddDays(-1);

                        DataRow[] eDr = eDt.Select($"sale_date_dt >= '{sDate.ToString("yyyy-MM-dd")}' AND sale_date_dt <= '{eDate.ToString("yyyy-MM-dd")}'");
                        if (eDr.Length > 0)
                        {
                            for (int j = 0; j < eDr.Length; j++)
                            {
                                double eQty;
                                if (!double.TryParse(eDr[j]["sale_qty"].ToString(), out eQty))
                                    eQty = 0;
                                sales -= eQty;
                            }
                        }
                    }
                    //출력
                    dgvDetails.Rows[(base_date.Month - 1) * 2].Cells["sales_qty_" + base_date.Year.ToString()].Value = sales.ToString("#,##0");
                    if (cbTermsType.Text == "초~말")
                        dgvDetails.Rows[(base_date.Month - 1) * 2].Cells["sales_qty_" + base_date.Year.ToString()].ToolTipText = "당월 1일부터 말일"
                            + "\n * 전체사업장, 사업장끼리 X, 관리자 매출 X";
                    else
                        dgvDetails.Rows[(base_date.Month - 1) * 2].Cells["sales_qty_" + base_date.Year.ToString()].ToolTipText = "전월 " + cbTermsType.Text + "부터 당월 " + cbTermsType.Text + " 까지"
                            + "\n * 전체사업장, 사업장끼리 X, 관리자 매출 X";
                    //Dictionary 추가
                    salesDic.Add(base_date, sales);
                }
                //전월, 전년
                if (dgvDetails.Columns.Count > 1)
                {
                    for (int i = 1; i < dgvDetails.Columns.Count - 2; i = i + 2)
                    {
                        double total_amount = 0;
                        for (int j = 0; j < dgvDetails.Rows.Count - 1; j = j + 2)
                        {
                            DateTime base_date = Convert.ToDateTime(dgvDetails.Columns[i].ToolTipText + "-" + dgvDetails.Rows[j].Cells[0].Value.ToString().Replace("월", "") + "-" + "01");
                            if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) >= base_date)
                            {
                                double qty;
                                if (dgvDetails.Rows[j].Cells[i].Value == null || !double.TryParse(dgvDetails.Rows[j].Cells[i].Value.ToString(), out qty))
                                    qty = 0;
                                total_amount += qty;
                                //전월==================================================================================================================================
                                double change_rate = 0;
                                string change_rate_txt = "";
                                DateTime before_month_date = base_date.AddMonths(-1);
                                if (salesDic.ContainsKey(before_month_date))
                                {
                                    //전월 매출량
                                    double before_qty = salesDic[before_month_date];
                                    //변화폭
                                    if (cbChangeType.Text == "증감")
                                    {
                                        change_rate = (qty - before_qty) / before_qty;  //증감률
                                        change_rate_txt = change_rate.ToString("#,##0.0");
                                    }
                                    else
                                    {
                                        change_rate = qty - before_qty;                 //수차
                                        change_rate_txt = change_rate.ToString("#,##0");
                                    }
                                    //출력
                                    if (!double.IsNaN(change_rate))
                                    { 
                                        if (change_rate < 0)
                                            dgvDetails.Rows[j].Cells[i + 1].Style.ForeColor = Color.Blue;
                                        else
                                            dgvDetails.Rows[j].Cells[i + 1].Style.ForeColor = Color.Red;
                                        dgvDetails.Rows[j].Cells[i + 1].Value = change_rate_txt;
                                    }
                                }
                                //당해년도 전월
                                if (dgvDetails.Columns[i].HeaderText == DateTime.Now.Year.ToString() + "년")
                                {
                                    if (!double.IsNaN(change_rate))
                                    {
                                        if (change_rate < 0)
                                            dgvDetails.Rows[j].Cells[dgvDetails.Columns.Count - 2].Style.ForeColor = Color.Blue;
                                        else
                                            dgvDetails.Rows[j].Cells[dgvDetails.Columns.Count - 2].Style.ForeColor = Color.Red;
                                        dgvDetails.Rows[j].Cells[dgvDetails.Columns.Count - 2].Value = change_rate_txt;
                                    }
                                }
                                //전년==================================================================================================================================
                                change_rate = 0;
                                change_rate_txt = "";
                                DateTime before_year_date = base_date.AddYears(-1);
                                if (salesDic.ContainsKey(before_year_date))
                                {
                                    double before_qty = salesDic[before_year_date];
                                    //변화폭
                                    if (cbChangeType.Text == "증감")
                                    {
                                        change_rate = (qty - before_qty) / before_qty;  //증감률
                                        change_rate_txt = change_rate.ToString("#,##0.0");
                                    }
                                    else
                                    {
                                        change_rate = qty - before_qty;                 //수차
                                        change_rate_txt = change_rate.ToString("#,##0");
                                    }
                                        
                                    if (!double.IsNaN(change_rate))
                                    { 
                                        if (change_rate < 0)
                                            dgvDetails.Rows[j + 1].Cells[i].Style.ForeColor = Color.Blue;                                            
                                        else
                                            dgvDetails.Rows[j + 1].Cells[i].Style.ForeColor = Color.Red;
                                        dgvDetails.Rows[j + 1].Cells[i].Value = change_rate_txt;
                                    }
                                }
                                //당해년도 전월
                                if (dgvDetails.Columns[i].HeaderText == DateTime.Now.Year.ToString() + "년")
                                {
                                    if (!double.IsNaN(change_rate))
                                    {
                                        if (change_rate < 0)
                                            dgvDetails.Rows[j].Cells[dgvDetails.Columns.Count - 1].Style.ForeColor = Color.Blue;
                                        else
                                            dgvDetails.Rows[j].Cells[dgvDetails.Columns.Count - 1].Style.ForeColor = Color.Red;
                                        dgvDetails.Rows[j + 1].Cells[dgvDetails.Columns.Count - 1].Value = change_rate_txt;
                                    }
                                }
                            }
                        }
                        //합계
                        /*dgvDetails.Rows[dgvDetails.Rows.Count - 1].Cells[i].Value = total_amount.ToString("#,##0");
                        dgvDetails.Rows[dgvDetails.Rows.Count - 1].Cells[i].Style.Font = new Font("나눔고딕", 9, FontStyle.Bold);*/
                    }
                }
            }

            GetAverageSaleQty(row);
            //GetSalesByWeek(row);
        }
        public void GetSales2(DataGridViewRow row)
        {
            //초기화
            dgvDetails2.Rows.Clear();
            dgvDetails2.Columns.Clear();
            //기준년월
            DataGridViewColumn col = new DataGridViewTextBoxColumn();
            col.Name = "base_date2";
            col.HeaderText = "기준";
            col.Width = 40;
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetails2.Columns.Add(col);
            int stt_year = DateTime.Now.AddYears(-3).Year;
            int end_year = DateTime.Now.Year;
            DateTime sttdate = new DateTime(stt_year, 1, 1);
            DateTime enddate = new DateTime(end_year, 12, 1);

            int terms_type = (int)nudUntilDays.Value;

            string merge_product = "";
            bool isMerge = false;
            if (row.Cells["merge_product"].Value != null && !string.IsNullOrEmpty(row.Cells["merge_product"].Value.ToString()))
            {
                merge_product = row.Cells["merge_product"].Value.ToString();
                isMerge = true;
            }

            //Get Data
            //제외매출
            DataTable eDt = productExcludedSalesRepository.GetExcludedSalesByMonth(DateTime.Now.AddMonths(-19).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd")
                                                                            , row.Cells["product"].Value.ToString()
                                                                            , row.Cells["origin"].Value.ToString()
                                                                            , row.Cells["sizes"].Value.ToString()
                                                                            , row.Cells["unit"].Value.ToString()
                                                                            , row.Cells["price_unit"].Value.ToString()
                                                                            , row.Cells["unit_count"].Value.ToString()
                                                                            , row.Cells["seaover_unit"].Value.ToString()
                                                                            , merge_product, isMerge);
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
            //매출량
            DataTable salesDt = salesRepository.GetSalesGroupMonth(sttdate, enddate, terms_type
                                                    , row.Cells["product"].Value.ToString()
                                                    , row.Cells["origin"].Value.ToString()
                                                    , row.Cells["sizes"].Value.ToString()
                                                    , row.Cells["seaover_unit"].Value.ToString()
                                                    , merge_product
                                                    , isMerge, cbAtoSale.Checked);
            if (salesDt.Rows.Count > 0)
            {
                for (int i = stt_year; i <= end_year; i++)
                {
                    //매출수
                    DataGridViewColumn col1 = new DataGridViewTextBoxColumn();
                    col1.Name = "sales_qty_2_" + i;
                    col1.HeaderText = i + "년";
                    col1.ToolTipText = i.ToString();
                    col1.Width = 60;
                    col1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvDetails2.Columns.Add(col1);

                    DataGridViewColumn col2 = new DataGridViewTextBoxColumn();
                    col2.Name = "prepare_pre_month_sales_qty_2_" + i;
                    col2.HeaderText = "전월";
                    col2.ToolTipText = i.ToString();
                    col2.Width = 50;
                    col2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col2.DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);
                    col2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvDetails2.Columns.Add(col2);
                    col2.Visible = false;
                }
                //마지막 년도는 항상 띄워놓기
                DataGridViewColumn col3 = new DataGridViewTextBoxColumn();
                col3.Name = "prepare_pre_month_sales_qty_2";
                col3.HeaderText = "전월";
                col3.Width = 60;
                col3.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvDetails2.Columns.Add(col3);

                DataGridViewColumn col4 = new DataGridViewTextBoxColumn();
                col4.Name = "prepare_pre_year_sales_qty_2";
                col4.HeaderText = "전년";
                col4.Width = 60;
                col4.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvDetails2.Columns.Add(col4);

                //월출력
                int n;
                for (int i = 1; i <= 12; i++)
                {
                    n = dgvDetails2.Rows.Add();
                    dgvDetails2.Rows[n].Cells["base_date2"].Value = i.ToString() + "월";
                    n = dgvDetails2.Rows.Add();
                    dgvDetails2.Rows[n].Cells["base_date2"].Value = "전년";
                    dgvDetails2.Rows[n].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
                    dgvDetails2.Rows[n].Visible = false;
                }
                /*n = dgvDetails2.Rows.Add();
                dgvDetails2.Rows[n].Cells["base_date2"].Value = "합계";*/

                //데이터출력
                Dictionary<DateTime, double> salesDic = new Dictionary<DateTime, double>();
                for (int i = 0; i < salesDt.Rows.Count; i++)
                {
                    DateTime base_date = Convert.ToDateTime(salesDt.Rows[i]["기준일자"].ToString().Substring(0, 4) + "-" + salesDt.Rows[i]["기준일자"].ToString().Substring(4, 2) + "-" + "01");
                    double sales;
                    if (!double.TryParse(salesDt.Rows[i]["매출수"].ToString(), out sales))
                        sales = 0;

                    //제외매출
                    if (eDt.Rows.Count > 0)
                    {
                        DateTime sDate = base_date;
                        int days = (int)nudUntilDays.Value;
                        if (sDate.AddMonths(1).AddDays(-1).Day < days)
                            days = sDate.AddMonths(1).AddDays(-1).Day;
                        DateTime eDate = new DateTime(sDate.Year, sDate.Month, days);

                        DataRow[] eDr = eDt.Select($"sale_date_dt >= '{sDate.ToString("yyyy-MM-dd")}' AND sale_date_dt <= '{eDate.ToString("yyyy-MM-dd")}'");
                        if (eDr.Length > 0)
                        {
                            for (int j = 0; j < eDr.Length; j++)
                            {
                                double eQty;
                                if (!double.TryParse(eDr[j]["sale_qty"].ToString(), out eQty))
                                    eQty = 0;
                                sales -= eQty;
                            }
                        }
                    }


                    dgvDetails2.Rows[(base_date.Month - 1) * 2].Cells["sales_qty_2_" + base_date.Year.ToString()].Value = sales.ToString("#,##0");

                    dgvDetails2.Rows[(base_date.Month - 1) * 2].Cells["sales_qty_2_" + base_date.Year.ToString()].ToolTipText = "각월 1일 부터 " + nudUntilDays.Value.ToString() + "일까지(말일 보다 높을 경우 말일까지)"
                         + "\n * 전체사업장, 사업장끼리 X, 관리자 매출 X";
                    //Dictionary 추가
                    salesDic.Add(base_date, sales);
                }
                //전월, 전년
                if (dgvDetails2.Columns.Count > 1)
                {
                    for (int i = 1; i < dgvDetails2.Columns.Count - 2; i = i + 2)
                    {
                        double total_amount = 0;
                        for (int j = 0; j < dgvDetails2.Rows.Count - 1; j = j + 2)
                        {
                            DateTime base_date = Convert.ToDateTime(dgvDetails2.Columns[i].ToolTipText + "-" + dgvDetails2.Rows[j].Cells[0].Value.ToString().Replace("월", "") + "-" + "01");
                            if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) >= base_date)
                            {
                                double qty;
                                if (dgvDetails2.Rows[j].Cells[i].Value == null || !double.TryParse(dgvDetails2.Rows[j].Cells[i].Value.ToString(), out qty))
                                    qty = 0;
                                total_amount += qty;
                                //전월==================================================================================================================================
                                double change_rate = 0;
                                string change_rate_txt = "";
                                DateTime before_month_date = base_date.AddMonths(-1);
                                if (salesDic.ContainsKey(before_month_date))
                                {
                                    //전월 매출량
                                    double before_qty = salesDic[before_month_date];
                                    //변화폭
                                    if (cbChangeType2.Text == "증감")
                                    {
                                        change_rate = (qty - before_qty) / before_qty;  //증감률
                                        change_rate_txt = change_rate.ToString("#,##0.0");
                                    }
                                    else
                                    {
                                        change_rate = qty - before_qty;                 //수차
                                        change_rate_txt = change_rate.ToString("#,##0");
                                    }
                                    //출력
                                    if (!double.IsNaN(change_rate))
                                    {
                                        if (change_rate < 0)
                                            dgvDetails2.Rows[j].Cells[i + 1].Style.ForeColor = Color.Blue;
                                        else
                                            dgvDetails2.Rows[j].Cells[i + 1].Style.ForeColor = Color.Red;
                                        dgvDetails2.Rows[j].Cells[i + 1].Value = change_rate_txt;
                                    }
                                }
                                //당해년도 전월
                                if (dgvDetails2.Columns[i].HeaderText == DateTime.Now.Year.ToString() + "년")
                                {
                                    if (!double.IsNaN(change_rate))
                                    {
                                        if (change_rate < 0)
                                            dgvDetails2.Rows[j].Cells[dgvDetails2.Columns.Count - 2].Style.ForeColor = Color.Blue;
                                        else
                                            dgvDetails2.Rows[j].Cells[dgvDetails2.Columns.Count - 2].Style.ForeColor = Color.Red;
                                        dgvDetails2.Rows[j].Cells[dgvDetails2.Columns.Count - 2].Value = change_rate_txt;
                                    }
                                }
                                //전년==================================================================================================================================
                                change_rate = 0;
                                change_rate_txt = "";
                                DateTime before_year_date = base_date.AddYears(-1);
                                if (salesDic.ContainsKey(before_year_date))
                                {
                                    double before_qty = salesDic[before_year_date];
                                    //변화폭
                                    if (cbChangeType2.Text == "증감")
                                    {
                                        change_rate = (qty - before_qty) / before_qty;  //증감률
                                        change_rate_txt = change_rate.ToString("#,##0.0");
                                    }
                                    else
                                    {
                                        change_rate = qty - before_qty;                 //수차
                                        change_rate_txt = change_rate.ToString("#,##0");
                                    }

                                    if (!double.IsNaN(change_rate))
                                    {
                                        if (change_rate < 0)
                                            dgvDetails2.Rows[j + 1].Cells[i].Style.ForeColor = Color.Blue;
                                        else
                                            dgvDetails2.Rows[j + 1].Cells[i].Style.ForeColor = Color.Red;
                                        dgvDetails2.Rows[j + 1].Cells[i].Value = change_rate_txt;
                                    }
                                }
                                //당해년도 전월
                                if (dgvDetails2.Columns[i].HeaderText == DateTime.Now.Year.ToString() + "년")
                                {
                                    if (!double.IsNaN(change_rate))
                                    {
                                        if (change_rate < 0)
                                            dgvDetails2.Rows[j].Cells[dgvDetails2.Columns.Count - 1].Style.ForeColor = Color.Blue;
                                        else
                                            dgvDetails2.Rows[j].Cells[dgvDetails2.Columns.Count - 1].Style.ForeColor = Color.Red;
                                        dgvDetails2.Rows[j + 1].Cells[dgvDetails2.Columns.Count - 1].Value = change_rate_txt;
                                    }
                                }
                            }
                        }
                        //합계
                        /*dgvDetails2.Rows[dgvDetails2.Rows.Count - 1].Cells[i].Value = total_amount.ToString("#,##0");
                        dgvDetails2.Rows[dgvDetails2.Rows.Count - 1].Cells[i].Style.Font = new Font("나눔고딕", 9, FontStyle.Bold);*/
                    }
                }
            }

            //GetAverageSaleQty(row);
            //GetSalesByWeek(row);
        }
        /*public void GetSalesByWeek(DataGridViewRow row)
        {
            //초기화
            dgvDetailsByWeek.Rows.Clear();
            //기준년월
            DateTime sttdate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime enddate = sttdate.AddMonths(-3);

            //Get Data
            DataTable salesDt = salesRepository.GetSales(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"), row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["seaover_unit"].Value.ToString(), cbAllUnit.Checked);
            if (salesDt.Rows.Count > 0)
            {
                //주차출력
                for (int i = 0; i <= 12; i++)
                {
                    int n = dgvDetailsByWeek.Rows.Add();
                    dgvDetailsByWeek.Rows[n].Cells[0].Value = i.ToString() + "주전";
                }

                //데이터출력
                for (int i = 0; i < salesDt.Rows.Count; i++)
                {
                    int idx = Convert.ToInt32(salesDt.Rows[i]["주차"].ToString());
                    if (idx <= 12)
                    {
                        double qty;
                        if (!double.TryParse(salesDt.Rows[i]["매출수"].ToString(), out qty))
                            qty = 0;
                        dgvDetailsByWeek.Rows[idx].Cells["sales_qty"].Value = qty.ToString("#,##0");
                        double sales_amount;
                        if (!double.TryParse(salesDt.Rows[i]["매출금액"].ToString(), out sales_amount))
                            sales_amount = 0;
                        dgvDetailsByWeek.Rows[idx].Cells["sales_price"].Value = sales_amount.ToString("#,##0");
                        double margin;
                        if (!double.TryParse(salesDt.Rows[i]["마진율"].ToString(), out margin))
                            margin = 0;
                        dgvDetailsByWeek.Rows[idx].Cells["margin"].Value = margin.ToString("#,##0.0") + "%";
                        double margin_amount;
                        if (!double.TryParse(salesDt.Rows[i]["마진금액"].ToString(), out margin_amount))
                            margin_amount = 0;
                        dgvDetailsByWeek.Rows[idx].Cells["margin_amount"].Value = margin_amount.ToString("#,##0");
                    }
                }

                //추이표시
                double before_qty = 0;
                for (int i = dgvDetailsByWeek.Rows.Count - 1; i >= 0; i--)
                {
                    double sales_qty;
                    if (dgvDetailsByWeek.Rows[i].Cells["sales_qty"].Value == null || !double.TryParse(dgvDetailsByWeek.Rows[i].Cells["sales_qty"].Value.ToString(), out sales_qty))
                        sales_qty = 0;
                    double rate_change;
                    if (cbChangeType.Text == "증감")
                        rate_change = (sales_qty - before_qty) / before_qty * 100;   //증감률
                    else
                        rate_change = sales_qty - before_qty;                        //수차


                    if (double.IsInfinity(rate_change))
                        rate_change = 0;
                    string rate_change_arrow;
                    if (rate_change <= 0)
                        dgvDetailsByWeek.Rows[i].Cells["rate_change"].Style.ForeColor = Color.Blue;
                    else
                        dgvDetailsByWeek.Rows[i].Cells["rate_change"].Style.ForeColor = Color.Red;
                    dgvDetailsByWeek.Rows[i].Cells["rate_change"].Value = rate_change.ToString("#,##0.0");

                    before_qty = sales_qty;
                }
            }
        }*/

        private void GetAverageSaleQty(DataGridViewRow row)
        {
            dgvSalesByMonth.Rows.Clear();
            dgvSalesByMonth.Rows.Add();

            string merge_product = "";
            bool isMerge = true;
            if (row.Cells["merge_product"].Value != null && !string.IsNullOrEmpty(row.Cells["merge_product"].Value.ToString()))
                merge_product = row.Cells["merge_product"].Value.ToString();
            else 
            {
                merge_product = row.Cells["product"].Value.ToString()
                        + "^" + row.Cells["origin"].Value.ToString()
                        + "^" + row.Cells["sizes"].Value.ToString()
                        + "^" + row.Cells["unit"].Value.ToString()
                        + "^" + row.Cells["price_unit"].Value.ToString()
                        + "^" + row.Cells["unit_count"].Value.ToString()
                        + "^" + row.Cells["seaover_unit"].Value.ToString();
            }

            DataTable avgDt = salesRepository.GetAverageSalesByMonth2(DateTime.Now, row.Cells["product"].Value.ToString()
                                                                    , row.Cells["origin"].Value.ToString()
                                                                    , row.Cells["sizes"].Value.ToString()
                                                                    , row.Cells["seaover_unit"].Value.ToString()
                                                                    , merge_product, isMerge);

            //매출제외 수량===========================================================================
            DataTable eDt = productExcludedSalesRepository.GetExcludedSalesByMonth(DateTime.Now.AddMonths(-19).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd")
                                                                            , row.Cells["product"].Value.ToString()
                                                                            , row.Cells["origin"].Value.ToString()
                                                                            , row.Cells["sizes"].Value.ToString()
                                                                            , row.Cells["unit"].Value.ToString()
                                                                            , row.Cells["price_unit"].Value.ToString()
                                                                            , row.Cells["unit_count"].Value.ToString()
                                                                            , row.Cells["seaover_unit"].Value.ToString()
                                                                            , merge_product, isMerge);
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

            double main_unit;
            if (row.Cells["seaover_unit"].Value == null || !double.TryParse(row.Cells["seaover_unit"].Value.ToString(), out main_unit))
                main_unit = 1;

            if (avgDt.Rows.Count > 0)
            {
                int wDays;
                //common.GetWorkDay(DateTime.Now.AddDays(-1).AddMonths(-1), DateTime.Now.AddDays(-1), out wDays);

                //1개월==============================================================================================
                double qty;
                if (!double.TryParse(avgDt.Rows[0][0].ToString(), out qty))
                    qty = 0;

                DateTime enddate = DateTime.Now.AddDays(-1);
                DateTime sttdate = DateTime.Now.AddMonths(-1);
                
                string whr;
                DataRow[] dr;
                if (eDt.Rows.Count > 0)
                { 
                    whr = $"sale_date_dt >= '{sttdate.ToString("yyyy-MM-dd")}' AND sale_date_dt <= '{enddate.ToString("yyyy-MM-dd")}'";
                    dr = eDt.Select(whr);
                    if (dr.Length > 0)
                    {
                        for (int i = 0; i < dr.Length; i++)
                        {
                            double sub_unit;
                            if (!double.TryParse(dr[i]["seaover_unit"].ToString(), out sub_unit))
                                sub_unit = 1;

                            double eQty;
                            if (!double.TryParse(dr[i]["sale_qty"].ToString(), out eQty))
                                eQty = 0;
                            qty -= (eQty * sub_unit / main_unit);
                        }
                    }
                }
                dgvSalesByMonth.Rows[0].Cells[0].Value = qty.ToString("#,##0");

                //45일==============================================================================================
                if (!double.TryParse(avgDt.Rows[0][1].ToString(), out qty))
                    qty = 0;
                sttdate = DateTime.Now.AddDays(-45);
                if (eDt.Rows.Count > 0)
                {
                    whr = $"sale_date_dt >= '{sttdate.ToString("yyyy-MM-dd")}' AND sale_date_dt <= '{enddate.ToString("yyyy-MM-dd")}'";
                    dr = eDt.Select(whr);
                    if (dr.Length > 0)
                    {
                        for (int i = 0; i < dr.Length; i++)
                        {
                            double sub_unit;
                            if (!double.TryParse(dr[i]["seaover_unit"].ToString(), out sub_unit))
                                sub_unit = 1;

                            double eQty;
                            if (!double.TryParse(dr[i]["sale_qty"].ToString(), out eQty))
                                eQty = 0;
                            qty -= (eQty * sub_unit / main_unit);
                        }
                    }
                }
                common.GetWorkDay(DateTime.Now.AddDays(-45), DateTime.Now, out wDays);
                //2023-06-15 오늘로 시작해서 영업일 -1
                wDays--;
                dgvSalesByMonth.Rows[0].Cells[1].Value = (qty / wDays * 21).ToString("#,##0");

                //2개월==============================================================================================
                if (!double.TryParse(avgDt.Rows[0][2].ToString(), out qty))
                    qty = 0;
                sttdate = DateTime.Now.AddMonths(-2);
                if (eDt.Rows.Count > 0)
                {
                    whr = $"sale_date_dt >= '{sttdate.ToString("yyyy-MM-dd")}' AND sale_date_dt <= '{enddate.ToString("yyyy-MM-dd")}'";
                    dr = eDt.Select(whr);
                    if (dr.Length > 0)
                    {
                        for (int i = 0; i < dr.Length; i++)
                        {
                            double sub_unit;
                            if (!double.TryParse(dr[i]["seaover_unit"].ToString(), out sub_unit))
                                sub_unit = 1;

                            double eQty;
                            if (!double.TryParse(dr[i]["sale_qty"].ToString(), out eQty))
                                eQty = 0;
                            qty -= (eQty * sub_unit / main_unit);
                        }
                    }
                }
                common.GetWorkDay(DateTime.Now.AddMonths(-2), DateTime.Now, out wDays);
                wDays--;
                dgvSalesByMonth.Rows[0].Cells[2].Value = (qty / wDays * 21).ToString("#,##0");

                //3개월==============================================================================================
                if (!double.TryParse(avgDt.Rows[0][3].ToString(), out qty))
                    qty = 0;
                sttdate = DateTime.Now.AddMonths(-3);
                if (eDt.Rows.Count > 0)
                {
                    whr = $"sale_date_dt >= '{sttdate.ToString("yyyy-MM-dd")}' AND sale_date_dt <= '{enddate.ToString("yyyy-MM-dd")}'";
                    dr = eDt.Select(whr);
                    if (dr.Length > 0)
                    {
                        for (int i = 0; i < dr.Length; i++)
                        {
                            double sub_unit;
                            if (!double.TryParse(dr[i]["seaover_unit"].ToString(), out sub_unit))
                                sub_unit = 1;

                            double eQty;
                            if (!double.TryParse(dr[i]["sale_qty"].ToString(), out eQty))
                                eQty = 0;
                            qty -= (eQty * sub_unit / main_unit);
                        }
                    }
                }
                common.GetWorkDay(DateTime.Now.AddMonths(-3), DateTime.Now, out wDays);
                wDays--;
                dgvSalesByMonth.Rows[0].Cells[3].Value = (qty / wDays * 21).ToString("#,##0");

                //6개월==============================================================================================
                if (!double.TryParse(avgDt.Rows[0][4].ToString(), out qty))
                    qty = 0;
                sttdate = DateTime.Now.AddMonths(-6);
                if (eDt.Rows.Count > 0)
                {
                    whr = $"sale_date_dt >= '{sttdate.ToString("yyyy-MM-dd")}' AND sale_date_dt <= '{enddate.ToString("yyyy-MM-dd")}'";
                    dr = eDt.Select(whr);
                    if (dr.Length > 0)
                    {
                        for (int i = 0; i < dr.Length; i++)
                        {
                            double sub_unit;
                            if (!double.TryParse(dr[i]["seaover_unit"].ToString(), out sub_unit))
                                sub_unit = 1;

                            double eQty;
                            if (!double.TryParse(dr[i]["sale_qty"].ToString(), out eQty))
                                eQty = 0;
                            qty -= (eQty * sub_unit / main_unit);
                        }
                    }
                }
                common.GetWorkDay(DateTime.Now.AddMonths(-6), DateTime.Now, out wDays);
                wDays--;
                dgvSalesByMonth.Rows[0].Cells[4].Value = (qty / wDays * 21).ToString("#,##0");

                //12개월==============================================================================================
                
                //매출수
                if (!double.TryParse(avgDt.Rows[0][5].ToString(), out qty))
                    qty = 0;
                //제외매출
                sttdate = DateTime.Now.AddMonths(-12);
                if (eDt.Rows.Count > 0)
                {
                    whr = $"sale_date_dt >= '{sttdate.ToString("yyyy-MM-dd")}' AND sale_date_dt <= '{enddate.ToString("yyyy-MM-dd")}'";
                    dr = eDt.Select(whr);
                    if (dr.Length > 0)
                    {
                        for (int i = 0; i < dr.Length; i++)
                        {
                            double sub_unit;
                            if (!double.TryParse(dr[i]["seaover_unit"].ToString(), out sub_unit))
                                sub_unit = 1;

                            double eQty;
                            if (!double.TryParse(dr[i]["sale_qty"].ToString(), out eQty))
                                eQty = 0;
                            qty -= (eQty * sub_unit / main_unit);
                        }
                    }
                }
                
                common.GetWorkDay(DateTime.Now.AddMonths(-12), DateTime.Now, out wDays);
                wDays--;
                dgvSalesByMonth.Rows[0].Cells[5].Value = (qty / wDays * 21).ToString("#,##0");

                //18개월==============================================================================================
                if (!double.TryParse(avgDt.Rows[0][6].ToString(), out qty))
                    qty = 0;
                sttdate = DateTime.Now.AddMonths(-18);
                //매출수
                if (!double.TryParse(avgDt.Rows[0][6].ToString(), out qty))
                    qty = 0;
                //제외매출
                if (eDt.Rows.Count > 0)
                {
                    whr = $"sale_date_dt >= '{sttdate.ToString("yyyy-MM-dd")}' AND sale_date_dt <= '{enddate.ToString("yyyy-MM-dd")}'";
                    dr = eDt.Select(whr);
                    if (dr.Length > 0)
                    {
                        for (int i = 0; i < dr.Length; i++)
                        {
                            double sub_unit;
                            if (!double.TryParse(dr[i]["seaover_unit"].ToString(), out sub_unit))
                                sub_unit = 1;

                            double eQty;
                            if (!double.TryParse(dr[i]["sale_qty"].ToString(), out eQty))
                                eQty = 0;
                            qty -= (eQty * sub_unit / main_unit);
                        }
                    }
                }
                common.GetWorkDay(DateTime.Now.AddMonths(-18), DateTime.Now, out wDays);
                wDays--;
                dgvSalesByMonth.Rows[0].Cells[6].Value = (qty / wDays * 21).ToString("#,##0");
            }
        }

        /*private void GetSales(DataGridViewRow row)
        {
            //초기화
            dgvSales.Rows.Clear();
            dgvSalesByMonth.Rows.Clear();
            dgvSalesByMonth.Rows.Add();
            //유효성검사
            if ((row.Cells["product"].Value == null || string.IsNullOrEmpty(row.Cells["product"].Value.ToString()))
                || (row.Cells["origin"].Value == null || string.IsNullOrEmpty(row.Cells["origin"].Value.ToString()))
                || (row.Cells["sizes"].Value == null || string.IsNullOrEmpty(row.Cells["sizes"].Value.ToString()))
                || (row.Cells["unit"].Value == null || string.IsNullOrEmpty(row.Cells["unit"].Value.ToString())))
                return;
            //Seoaverid
            salesRepository.SetSeaoverId(um.seaover_id);
            DateTime sttdate = DateTime.Now.AddYears(-4);
            DateTime enddate = DateTime.Now;
            //매출내역
            DataTable salesDt = salesRepository.GetSalesProduct(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["seaover_unit"].Value.ToString(), "");
            //출력==========================================================================================================================================
            if (salesDt.Rows.Count > 0)
            {
                double max = 0, min = 0;
                double total_margin = 0, total_qty = 0;
                for (int i = 0; i < salesDt.Rows.Count; i++)
                {
                    int n = dgvSales.Rows.Add();
                    string sdate = salesDt.Rows[i]["매출일자"].ToString().Substring(2, 2) + "/" + salesDt.Rows[i]["매출일자"].ToString().Substring(4, 2);
                    dgvSales.Rows[n].Cells["sale_date"].Value = sdate;
                    dgvSales.Rows[n].Cells["purchase_price"].Value = salesDt.Rows[i]["매출금액"].ToString();
                    dgvSales.Rows[n].Cells["sales_qty"].Value = Convert.ToDouble(salesDt.Rows[i]["매출수"].ToString()).ToString("#,##0");
                    dgvSales.Rows[n].Cells["sales_price"].Value = Convert.ToDouble(salesDt.Rows[i]["매출금액"].ToString()).ToString("#,##0");
                    dgvSales.Rows[n].Cells["margin"].Value = Convert.ToDouble(salesDt.Rows[i]["마진율"].ToString()).ToString("#,##0.00") + "%";
                    total_margin += Convert.ToDouble(salesDt.Rows[i]["마진율"].ToString());
                    total_qty++;
                    dgvSales.Rows[n].Cells["margin_amount"].Value = Convert.ToDouble(salesDt.Rows[i]["마진금액"].ToString()).ToString("#,##0");

                    //Chart series 추가
                    double tmp = Convert.ToDouble(salesDt.Rows[i]["매출수"].ToString());
                    //최대, 최소
                    if (i == 0)
                    {
                        max = tmp;
                        min = tmp;
                    }
                    else
                    {
                        if (max < tmp)
                            max = tmp;
                        if (min > tmp)
                            min = tmp;
                    }
                }
                //평균마진
                lbAverage.Text = "평균 마진율 : " + (total_margin / total_qty).ToString("#,##0.0") + "%";
                //추이표시
                double before_qty = 0;
                for (int i = salesDt.Rows.Count - 1; i >= 0; i--)
                {
                    double sales_qty;
                    if (dgvSales.Rows[i].Cells["sales_qty"].Value == null || !double.TryParse(dgvSales.Rows[i].Cells["sales_qty"].Value.ToString(), out sales_qty))
                        sales_qty = 0;
                    double rate_change = (sales_qty - before_qty) / before_qty * 100;
                    if (double.IsInfinity(rate_change))
                        rate_change = 0;
                    string rate_change_arrow;
                    if (rate_change <= 0)
                        dgvSales.Rows[i].Cells["rate_change"].Style.ForeColor = Color.Blue;
                    else
                        dgvSales.Rows[i].Cells["rate_change"].Style.ForeColor = Color.Red;
                    dgvSales.Rows[i].Cells["rate_change"].Value = rate_change.ToString("#,##0.0");

                    before_qty = sales_qty;
                }

                
            }
        }*/

        #endregion

        #region 매입마진 불러오기
        private void btnPurchaseMargin_Click(object sender, EventArgs e)
        {
            //설정한 매입마진율
            double settingMargin = 0;
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
            { 
                if (row.Cells["purchase_margin"].Value == null || !double.TryParse(row.Cells["purchase_margin"].Value.ToString(), out settingMargin))
                    settingMargin = 0;
                //
                double divMargin = GetPurchaseOfferDivisionMarginRate();

                ContextMenuStrip menu = new ContextMenuStrip();

                ToolStripMenuItem item03 = new ToolStripMenuItem("매입단가-오퍼가 마진 가져오기 : " + divMargin.ToString("#,##0.00") + "%");
                item03.Click += SeaoverPurchaseMargin;
                ToolStripMenuItem item01 = new ToolStripMenuItem("설정한 매입마진율 가져오기 : " + settingMargin.ToString("#,##0.00") + "%");
                item01.Click += GetUserSettingPurchaseMargin;

                menu.Items.Add(item03);
                menu.Items.Add(item01);
                menu.Show(MousePosition);
            }
        }
        private double GetPurchaseOfferDivisionMarginRate()
        {
            double margin = 0;
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
            {
                if (purchaseDt.Rows.Count > 0)
                {
                    DateTime sttdate = DateTime.Now.AddYears(-4);
                    DateTime enddate = DateTime.Now;
                    //매입내역
                    double custom;
                    if (!double.TryParse(txtCustom.Text, out custom))
                        custom = 0;
                    custom = custom / 100;
                    double tax;
                    if (!double.TryParse(txtTax.Text, out tax))
                        tax = 0;
                    tax = tax / 100;
                    double incidental_expense;
                    if (!double.TryParse(txtIncidentalExpense.Text, out incidental_expense))
                        incidental_expense = 0;
                    incidental_expense = incidental_expense / 100;
                    double purchase_margin;
                    if (row.Cells["purchase_margin"].Value == null || !double.TryParse(row.Cells["purchase_margin"].Value.ToString(), out purchase_margin))
                        purchase_margin = 0;
                    txtPurchaseMargin.Text = purchase_margin.ToString();
                    purchase_margin = purchase_margin / 100;
                    double unit;
                    if (row.Cells["unit"].Value == null || !double.TryParse(row.Cells["unit"].Value.ToString(), out unit))
                        unit = 0;
                    double cost_unit;
                    if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                        cost_unit = 0;
                    bool isWeight;
                    if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out isWeight))
                        isWeight = true;
                    if (!isWeight && cost_unit == 0)
                        isWeight = true;
                    //오퍼내역
                    string select_unit = unit.ToString();
                    if (cbAllUnit.Checked)
                        select_unit = "";
                    //DataTable offerDt = purchasePriceRepository.GetPurhcasePriceAverage(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString(), select_unit);
                    DataTable offerDt = new DataTable();
                    //환율내역
                    DataTable exchangeRateDt = purchaseRepository.GetExchangeRate(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));
                    double average_purchase_margin = 0;
                    for (int i = 0; i < purchaseDt.Rows.Count; i++)
                    {
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
                            {
                                if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                                    exchange_rate = 0;
                            }
                        }
                        //씨오버 매입단가
                        double purcahse = Convert.ToDouble(purchaseDt.Rows[i]["단가"].ToString());
                        if (purcahse > 0)
                        {
                            //오퍼가 매입단가
                            double price_krw = 0, offer_price = 0;
                            if (cbPurchasePriceType.Text != "매입내역" && offerDt.Rows.Count > 0)
                            {
                                string whr = "updatetime = '" + purchaseDt.Rows[i]["매입일자"].ToString() + "'";
                                dr = offerDt.Select(whr);
                                if (dr.Length > 0)
                                {
                                    offer_price = Convert.ToDouble(dr[0]["purchase_price"].ToString());

                                    if (!isWeight)
                                        price_krw = offer_price * exchange_rate * (1 + tax + custom + incidental_expense) * cost_unit;
                                    else
                                        price_krw = offer_price * exchange_rate * (1 + tax + custom + incidental_expense) * unit;


                                    double one_purchase_margin = (purcahse - price_krw) / purcahse * 100;
                                    if (average_purchase_margin == 0)
                                        average_purchase_margin = one_purchase_margin;
                                    else
                                        average_purchase_margin = (average_purchase_margin + one_purchase_margin) / 2;
                                }
                            }
                        }
                    }
                    margin = average_purchase_margin;
                }

            }
            return margin;
        }
        private void SettingPurchaseMargin(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
            {
                double purchase_margin;
                if (!double.TryParse(txtPurchaseMargin.Text, out purchase_margin))
                {
                    MessageBox.Show(this, "매입마진 값이 숫자값이 아닙니다.");
                    this.Activate();
                    return;
                }

                StringBuilder sql = productOtherCostRepository.UpdateProductPurchaseMargin(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString()
                                                                                        , row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString()
                                                                                        , purchase_margin);
                List<StringBuilder> sqlList = new List<StringBuilder>();
                sqlList.Add(sql);

                if (commonRepository.UpdateTran(sqlList) == -1)
                    MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                else
                {
                    MessageBox.Show(this, "등록완료.");
                    lbSetting.Visible = true;
                }
            }
            else
                MessageBox.Show(this, "품목을 다시 선택해주세요.");
            this.Activate();
        }
        private void GetUserSettingPurchaseMargin(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
            {
                double purchase_margin;
                if (row.Cells["purchase_margin"].Value == null || !double.TryParse(row.Cells["purchase_margin"].Value.ToString(), out purchase_margin))
                    purchase_margin = 0;
                txtPurchaseMargin.Text = purchase_margin.ToString("#,##0.0");
                txtPurchaseMargin.Update();
                GetPurchase(row);
                lbSetting.Visible = true;
            }
        }
        private void SeaoverPurchaseMargin(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
            {
                if (purchaseDt.Rows.Count > 0)
                {
                    DateTime sttdate = DateTime.Now.AddYears(-4);
                    DateTime enddate = DateTime.Now;
                    //매입내역
                    double custom;
                    if (!double.TryParse(txtCustom.Text, out custom))
                        custom = 0;
                    custom = custom / 100;
                    double tax;
                    if (!double.TryParse(txtTax.Text, out tax))
                        tax = 0;
                    tax = tax / 100;
                    double incidental_expense;
                    if (!double.TryParse(txtIncidentalExpense.Text, out incidental_expense))
                        incidental_expense = 0;
                    incidental_expense = incidental_expense / 100;
                    double purchase_margin;
                    if (row.Cells["purchase_margin"].Value == null || !double.TryParse(row.Cells["purchase_margin"].Value.ToString(), out purchase_margin))
                        purchase_margin = 0;
                    txtPurchaseMargin.Text = purchase_margin.ToString();
                    purchase_margin = purchase_margin / 100;
                    double unit;
                    if (row.Cells["unit"].Value == null || !double.TryParse(row.Cells["unit"].Value.ToString(), out unit))
                        unit = 0;
                    double cost_unit;
                    if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                        cost_unit = 0;
                    bool isWeight;
                    if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out isWeight))
                        isWeight = true;
                    if (!isWeight && cost_unit == 0)
                        isWeight = true;
                    //오퍼내역
                    string select_unit = unit.ToString();
                    if (cbAllUnit.Checked)
                        select_unit = "";
                    //DataTable offerDt = purchasePriceRepository.GetPurhcasePriceAverage(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString(), select_unit);
                    DataTable offerDt = new DataTable();
                    //환율내역
                    DataTable exchangeRateDt = purchaseRepository.GetExchangeRate(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));
                    double average_purchase_margin = 0;
                    for (int i = 0; i < purchaseDt.Rows.Count; i++)
                    {
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
                            {
                                if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                                    exchange_rate = 0;
                            }
                        }
                        //씨오버 매입단가
                        double purcahse = Convert.ToDouble(purchaseDt.Rows[i]["단가"].ToString());
                        if (purcahse > 0)
                        {
                            //오퍼가 매입단가
                            double price_krw = 0, offer_price = 0;
                            if (cbPurchasePriceType.Text != "매입내역" && offerDt.Rows.Count > 0)
                            {
                                string whr = "updatetime = '" + purchaseDt.Rows[i]["매입일자"].ToString() + "'";
                                dr = offerDt.Select(whr);
                                if (dr.Length > 0)
                                {
                                    offer_price = Convert.ToDouble(dr[0]["purchase_price"].ToString());

                                    if (!isWeight)
                                        price_krw = offer_price * exchange_rate * (1 + tax + custom + incidental_expense) * cost_unit;
                                    else
                                        price_krw = offer_price * exchange_rate * (1 + tax + custom + incidental_expense) * unit;


                                    double one_purchase_margin = (purcahse - price_krw) / purcahse * 100;
                                    if (average_purchase_margin == 0)
                                        average_purchase_margin = one_purchase_margin;
                                    else
                                        average_purchase_margin = (average_purchase_margin + one_purchase_margin) / 2;
                                }
                            }
                        }
                    }
                    txtPurchaseMargin.Text = average_purchase_margin.ToString("#,##0.00");
                    lbSetting.Visible = false;
                }

            }
            txtPurchaseMargin.Update();
            GetPurchase(row);
        }
        #endregion

        #region 업체별시세현황에서 데이터 받아오기
        public void InputClipboardData(List<string[]> product)
        {
            double exchange_rate = common.GetExchangeRateKEBBank("USD");
            if (product.Count > 0)
            {
                for (int i = 0; i < product.Count; i++)
                {
                    int n = dgvProduct.Rows.Add();
                    dgvProduct.Rows[n].Cells["product"].Value = product[i][0];
                    dgvProduct.Rows[n].Cells["origin"].Value = product[i][1];
                    dgvProduct.Rows[n].Cells["sizes"].Value = product[i][2];
                    dgvProduct.Rows[n].Cells["unit"].Value = product[i][3];
                    dgvProduct.Rows[n].Cells["cost_unit"].Value = product[i][4];

                    double seaover_current_purchase_price;
                    if (!double.TryParse(product[i][5], out seaover_current_purchase_price))
                        seaover_current_purchase_price = 0;
                    dgvProduct.Rows[n].Cells["seaover_current_purchase_price"].Value = seaover_current_purchase_price.ToString("#,##0");
                    double seaover_current_normal_price;
                    if (!double.TryParse(product[i][28], out seaover_current_normal_price))
                        seaover_current_normal_price = 0;
                    dgvProduct.Rows[n].Cells["seaover_current_normal_price"].Value = seaover_current_normal_price.ToString("#,##0");
                    double seaover_current_sales_price;
                    if (!double.TryParse(product[i][25], out seaover_current_sales_price))
                        seaover_current_sales_price = 0;
                    dgvProduct.Rows[n].Cells["seaover_current_sales_price"].Value = seaover_current_sales_price.ToString("#,##0");

                    dgvProduct.Rows[n].Cells["custom"].Value = product[i][6];
                    dgvProduct.Rows[n].Cells["tax"].Value = product[i][7];
                    dgvProduct.Rows[n].Cells["incidental_expense"].Value = product[i][8];
                    dgvProduct.Rows[n].Cells["purchase_margin"].Value = product[i][9];
                    dgvProduct.Rows[n].Cells["production_days"].Value = product[i][10];

                    //dgvProduct.Rows[n].Cells["exchange_rate"].Value = exchange_rate.ToString("#,##0");

                    bool weight_calculate;
                    if (!bool.TryParse(product[i][19], out weight_calculate))
                        weight_calculate = true;

                    dgvProduct.Rows[n].Cells["weight_calculate"].Value = weight_calculate;

                    double offer_price;
                    if (!double.TryParse(product[i][11], out offer_price))
                        offer_price = 0;
                    double custom;
                    if (!double.TryParse(product[i][6], out custom))
                        custom = 0;
                    custom /= 100;
                    double tax;
                    if (!double.TryParse(product[i][7], out tax))
                        tax = 0;
                    tax /= 100;
                    double incidental_expense;
                    if (!double.TryParse(product[i][8], out incidental_expense))
                        incidental_expense = 0;
                    incidental_expense /= 100;
                    double unit;
                    if (!double.TryParse(product[i][3], out unit))
                        unit = 0;
                    double cost_unit;
                    if (!double.TryParse(product[i][4], out cost_unit))
                        cost_unit = 0;

                    double cost_price;
                    if (!weight_calculate && cost_unit > 0)
                        cost_price = offer_price * (1 + tax + custom + incidental_expense) * exchange_rate * cost_unit;
                    else
                        cost_price = offer_price * (1 + tax + custom + incidental_expense) * exchange_rate * unit;

                    dgvProduct.Rows[n].Cells["price_unit"].Value = product[i][20];
                    dgvProduct.Rows[n].Cells["unit_count"].Value = product[i][21];
                    dgvProduct.Rows[n].Cells["bundle_count"].Value = product[i][29];
                    dgvProduct.Rows[n].Cells["seaover_unit"].Value = product[i][22];
                    dgvProduct.Rows[n].Cells["manager1"].Value = product[i][23];
                    dgvProduct.Rows[n].Cells["manager2"].Value = product[i][24];
                    
                    dgvProduct.Rows[n].Cells["division"].Value = product[i][26];
                    dgvProduct.Rows[n].Cells["delivery_days"].Value = product[i][27];

                    //머지품목(sub)
                    DataTable groupProductDt = productGroupRepository.GetSubProduct(product[i][0], product[i][1], product[i][2], product[i][3], product[i][20], product[i][21], product[i][22]);
                    if (groupProductDt.Rows.Count > 0)
                    {
                        string sub_product = "";
                        for (int j = 0; j < groupProductDt.Rows.Count; j++)
                        {
                            sub_product += " \n" + groupProductDt.Rows[j]["item_product"].ToString()
                                        + "^" + groupProductDt.Rows[j]["item_origin"].ToString()
                                        + "^" + groupProductDt.Rows[j]["item_sizes"].ToString()
                                        + "^" + groupProductDt.Rows[j]["item_unit"].ToString()
                                        + "^" + groupProductDt.Rows[j]["item_price_unit"].ToString()
                                        + "^" + groupProductDt.Rows[j]["item_unit_count"].ToString()
                                        + "^" + groupProductDt.Rows[j]["item_seaover_unit"].ToString();
                        }
                        sub_product = sub_product.Trim();
                        dgvProduct.Rows[n].Cells["is_merge"].Value = true;
                        dgvProduct.Rows[n].Cells["merge_product"].Value = sub_product;
                    }
                    else
                        dgvProduct.Rows[n].Cells["is_merge"].Value = false;

                }
                dgvProduct.Rows[0].Selected = true;
            }
        }

        #endregion

        #region 역대 오퍼가 Method
        private void GetOffer(DataGridViewRow row, bool isCompanyRefresh = true)
        {
            //초기화
            dgvOfferPrice.Rows.Clear();
            //유효성검사
            if ((row.Cells["product"].Value == null || string.IsNullOrEmpty(row.Cells["product"].Value.ToString()))
                || (row.Cells["origin"].Value == null || string.IsNullOrEmpty(row.Cells["origin"].Value.ToString()))
                || (row.Cells["sizes"].Value == null || string.IsNullOrEmpty(row.Cells["sizes"].Value.ToString()))
                || (row.Cells["unit"].Value == null || string.IsNullOrEmpty(row.Cells["unit"].Value.ToString())))
                return;
            //Seoaverid
            DateTime sttdate = DateTime.Now.AddMonths(-6);
            DateTime enddate = DateTime.Now;

            //매입내역
            double custom;
            if (!double.TryParse(txtCustom.Text, out custom))
                custom = 0;
            custom = custom / 100;
            double tax;
            if (!double.TryParse(txtTax.Text, out tax))
                tax = 0;
            tax = tax / 100;
            double incidental_expense;
            if (!double.TryParse(txtIncidentalExpense.Text, out incidental_expense))
                incidental_expense = 0;
            incidental_expense = incidental_expense / 100;
            double unit;
            if (row.Cells["unit"].Value == null || !double.TryParse(row.Cells["unit"].Value.ToString(), out unit))
                unit = 0;
            double seaover_unit;
            if (row.Cells["seaover_unit"].Value == null || !double.TryParse(row.Cells["seaover_unit"].Value.ToString(), out seaover_unit))
                seaover_unit = 0;
            double cost_unit;
            if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                cost_unit = 0;
            bool isWeight;
            if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out isWeight))
                isWeight = true;
            if (!isWeight && cost_unit == 0)
                isWeight = true;
            //현재환율
            double exchange_rate;
            if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                exchange_rate = 0;

            string sub_product = "";
            bool isMerge = false;
            if (row.Cells["merge_product"].Value == null || string.IsNullOrEmpty(row.Cells["merge_product"].Value.ToString()))
            {
                sub_product = "";
                isMerge = false;
            }
            else
            {
                sub_product = row.Cells["merge_product"].Value.ToString();
                isMerge = true;
            }
            //오퍼내역
            DataTable offerDt = purchasePriceRepository.GetPurchasePriceForChartDay(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), seaover_unit.ToString()
                , sub_product, isMerge);
            //거래처리스트 최신화
            if (isCompanyRefresh && offerDt.Rows.Count > 0)
            {
                List<string> companyList = new List<string>();
                companyList.Add("전체");
                for (int i = 0; i < offerDt.Rows.Count; i++)
                {
                    companyList.Add(offerDt.Rows[i]["cname"].ToString());
                }
                //cbOfferCompany.Text = cbOfferCompany.Items[0].ToString();
            }
            //실제데이터 뿌리기
            if (offerDt.Rows.Count > 0)
            {
                bool isFindSameOffer = false;
                for (int i = 0; i < offerDt.Rows.Count; i++)
                {
                    bool isOutput = true;
                    if (isOutput)
                    {
                        int n = dgvOfferPrice.Rows.Add();

                        dgvOfferPrice.Rows[n].Cells["offer_date"].Value = Convert.ToDateTime(offerDt.Rows[i]["updatetime"].ToString()).ToString("yy/MM/dd");
                        dgvOfferPrice.Rows[n].Cells["offer_company"].Value = offerDt.Rows[i]["cname"].ToString();
                        dgvOfferPrice.Rows[n].Cells["offer_unit"].Value = offerDt.Rows[i]["unit"].ToString() + " -> " + seaover_unit.ToString();

                        double purchase_price = Convert.ToDouble(offerDt.Rows[i]["purchase_price"].ToString());
                        dgvOfferPrice.Rows[n].Cells["unit_price"].Value = purchase_price.ToString("#,##0.00");

                        //고지가
                        double fixed_tariff;
                        if (!double.TryParse(offerDt.Rows[i]["fixed_tariff"].ToString(), out fixed_tariff))
                            fixed_tariff = 0;
                        fixed_tariff /= 1000;

                        double cost_price;
                        if (isWeight)
                        {
                            if (fixed_tariff > 0)
                            {
                                cost_price = purchase_price * exchange_rate * unit;
                                cost_price += (fixed_tariff * unit * exchange_rate * custom);

                                cost_price *= (tax + 1);
                                cost_price *= (incidental_expense + 1);
                            }
                            else
                            {
                                cost_price = unit * purchase_price * exchange_rate;
                                cost_price *= (custom + 1);
                                cost_price *= (tax + 1);
                                cost_price *= (incidental_expense + 1);
                            }
                        }
                        else
                        {
                            cost_price = cost_unit * purchase_price * exchange_rate;
                            cost_price *= (custom + 1);

                            cost_price *= (tax + 1);
                            cost_price *= (incidental_expense + 1);
                        }

                        //박스단가 맞추기
                        if (!isWeight && !cbMultiplyTray.Checked)
                            cost_price /= cost_unit;

                        dgvOfferPrice.Rows[n].Cells["unit_cost_price"].Value = cost_price.ToString("#,##0");
                        //매입가
                        double offer_purchase_price = 0;
                        if (dgvProduct.Rows.Count > 0)
                        {
                            for (int j = 0; j < dgvProduct.Rows.Count; j++)
                            {
                                if (Convert.ToBoolean(dgvProduct.Rows[j].Cells["chk"].Value))
                                {
                                    offer_purchase_price = Convert.ToDouble(dgvProduct.Rows[j].Cells["seaover_current_purchase_price"].Value.ToString());

                                    /*if (!isWeight && !cbMultiplyTray.Checked)
                                        offer_purchase_price *= cost_unit;*/
                                    break;
                                }
                            }
                        }
                        dgvOfferPrice.Rows[n].Cells["offer_purchase_price"].Value = offer_purchase_price.ToString("#,##0");
                        
                        //마진
                        dgvOfferPrice.Rows[n].Cells["offer_margin"].Value = ((offer_purchase_price - cost_price) / offer_purchase_price * 100).ToString("#,##0.0") + "%";
                    }
                }
            }
            SetOfferChart(row);
        }
        private void SetOfferChart(DataGridViewRow select_row)
        {
            if (dgvPurchaseUsd.Rows.Count > 0)
            {
                double max_krw = 0, min_krw = 9999999, max_usd = 0, min_usd = 9999999;
                //데이터 출력
                for (int i = 0; i < dgvOfferPrice.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvOfferPrice.Rows[i];
                    double price_krw = Convert.ToDouble(row.Cells["unit_cost_price"].Value.ToString());
                    double offer_price = Convert.ToDouble(row.Cells["unit_price"].Value.ToString());
                    string offer_date = Convert.ToDateTime(row.Cells["offer_date"].Value.ToString()).ToString("yyyy-MM-dd");

                }
            }
        }

        #endregion

        #region Change event
        private void dgvContract_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvContract.Columns[e.ColumnIndex].Name == "etd" || dgvContract.Columns[e.ColumnIndex].Name == "warehousing_date")
                {
                    DateTime sdate;
                    if (dgvContract.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && DateTime.TryParse(dgvContract.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out sdate))
                    {
                        Common.Calendar calendar = new Common.Calendar(sdate);
                        string sDt = calendar.GetDate(true);
                        if (sDt != null)
                            dgvContract.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = sDt;
                    }
                }
            }
        }
        private void cbSimulation_CheckedChanged(object sender, EventArgs e)
        {
            this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
                OpenExhaustedManager(row);
            this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
        }
        private void cbChangeType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
                GetSales2(row);
            this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
        }
        private void cbCompareMonth2_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvDetails2.Columns.Count > 1)
            {
                for (int i = 2; i < dgvDetails2.Columns.Count - 2; i = i + 2)
                {
                    dgvDetails2.Columns[i].Visible = cbCompareMonth2.Checked;
                }

                if (!cbCompareMonth2.Checked && !cbCompareYear2.Checked)
                {
                    dgvDetails2.Columns["prepare_pre_month_sales_qty_2"].Visible = true;
                    dgvDetails2.Columns["prepare_pre_year_sales_qty_2"].Visible = true;
                }
                else
                {
                    dgvDetails2.Columns["prepare_pre_month_sales_qty_2"].Visible = false;
                    dgvDetails2.Columns["prepare_pre_year_sales_qty_2"].Visible = false;
                }
            }
        }

        private void cbCompareYear2_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvDetails2.Rows.Count > 1)
            {
                for (int i = 1; i < dgvDetails2.Rows.Count; i = i + 2)
                {
                    dgvDetails2.Rows[i].Visible = cbCompareYear2.Checked;
                }

                if (!cbCompareMonth2.Checked && !cbCompareYear2.Checked)
                {
                    dgvDetails2.Columns["prepare_pre_month_sales_qty_2"].Visible = true;
                    dgvDetails2.Columns["prepare_pre_year_sales_qty_2"].Visible = true;
                }
                else
                {
                    dgvDetails2.Columns["prepare_pre_month_sales_qty_2"].Visible = false;
                    dgvDetails2.Columns["prepare_pre_year_sales_qty_2"].Visible = false;
                }
            }
        }
        private void cbTermsType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetSales(GetSelectProductDgvr());
        }
        private void cbCompareMonth_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvDetails.Columns.Count > 1)
            {
                for (int i = 2; i < dgvDetails.Columns.Count - 2; i = i + 2)
                {
                    dgvDetails.Columns[i].Visible = cbCompareMonth.Checked;
                }

                if (!cbCompareMonth.Checked && !cbCompareYear.Checked)
                {
                    dgvDetails.Columns["prepare_pre_month_sales_qty"].Visible = true;
                    dgvDetails.Columns["prepare_pre_year_sales_qty"].Visible = true;
                }
                else
                {
                    dgvDetails.Columns["prepare_pre_month_sales_qty"].Visible = false;
                    dgvDetails.Columns["prepare_pre_year_sales_qty"].Visible = false;
                }
            }
        }

        private void cbCompareYear_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvDetails.Rows.Count > 1)
            {
                for (int i = 1; i < dgvDetails.Rows.Count; i = i + 2)
                {
                    dgvDetails.Rows[i].Visible = cbCompareYear.Checked;
                }

                if (!cbCompareMonth.Checked && !cbCompareYear.Checked)
                {
                    dgvDetails.Columns["prepare_pre_month_sales_qty"].Visible = true;
                    dgvDetails.Columns["prepare_pre_year_sales_qty"].Visible = true;
                }
                else
                {
                    dgvDetails.Columns["prepare_pre_month_sales_qty"].Visible = false;
                    dgvDetails.Columns["prepare_pre_year_sales_qty"].Visible = false;
                }
            }
        }

        private void txtAverageCostPrice_TextChanged(object sender, EventArgs e)
        {
            if (dgvStockSales.Rows.Count > 0)
            {
                double total_cost_price;
                if (!double.TryParse(txtAverageCostPrice.Text, out total_cost_price))
                    total_cost_price = 0;
                dgvStockSales.Rows[0].Cells["total_cost_price"].Value = total_cost_price.ToString("#,##0");
            }
        }
        private void cbAllUnit_CheckedChanged(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
            {
                this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                this.dgvPurchaseUsd.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseUsd_CellValueChanged);
                
                //초기화
                seaover_average_cost_price = 0;
                total_average_cost_price = 0;
                txtInQty.Text = "0";
                GetProductInfo(row);
                OpenExhaustedManager(row);
                GetMarketPriceByCompany(row);
                GetPurchase(row);
                GetSales(row);
                GetSales2(row);
                GetPurchasePriceList(row);
                GetOffer(row);
                SetDashboardSummary();
                this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                this.dgvPurchaseUsd.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseUsd_CellValueChanged);
            }
        }
        private void cbChangeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
            {
                GetSales(row);
                GetSales2(row);
            }
        }
        #endregion

        #region Datagridview 정렬
        private void dgvPurchaseUsd_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            DataGridView dgv = dgvPurchaseUsd;
            int i = e.Column.Index;
            if (dgv.Columns[i].Name != "purchase_date")
            {
                string cell_value1 = e.CellValue1.ToString();
                cell_value1 = cell_value1.Replace("%", "");
                string cell_value2 = e.CellValue2.ToString();
                cell_value2 = cell_value2.Replace("%", "");

                double a;
                if (cell_value1 == null)
                    a = 0;
                else
                    a = double.Parse(cell_value1.ToString());

                double b;
                if (cell_value2 == null)
                    b = 0;
                else
                    b = double.Parse(cell_value2.ToString());

                e.SortResult = a.CompareTo(b);
                e.Handled = true;
            }
        }

        private void dgvOfferPrice_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            DataGridView dgv = dgvOfferPrice;
            int i = e.Column.Index;
            if (dgv.Columns[i].Name != "offer_date" && dgv.Columns[i].Name != "offer_company")
            {
                string cell_value1 = e.CellValue1.ToString();
                cell_value1 = cell_value1.Replace("%", "");
                string cell_value2 = e.CellValue2.ToString();
                cell_value2 = cell_value2.Replace("%", "");

                double a;
                if (cell_value1 == null)
                    a = 0;
                else
                    a = double.Parse(cell_value1.ToString());

                double b;
                if (cell_value2 == null)
                    b = 0;
                else
                    b = double.Parse(cell_value2.ToString());

                e.SortResult = a.CompareTo(b);
                e.Handled = true;
            }
        }

        private void dgvMarket_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            DataGridView dgv = dgvMarket;
            int i = e.Column.Index;
            if (dgv.Columns[i].Name == "seaover_sales_price" || dgv.Columns[i].Name == "seaover_margin" || dgv.Columns[i].Name == "seaover_purchase_price")
            {
                string cell_value1 = e.CellValue1.ToString();
                cell_value1 = cell_value1.Replace("%", "");
                string cell_value2 = e.CellValue2.ToString();
                cell_value2 = cell_value2.Replace("%", "");

                double a;
                if (cell_value1 == null)
                    a = 0;
                else
                    a = double.Parse(cell_value1.ToString());

                double b;
                if (cell_value2 == null)
                    b = 0;
                else
                    b = double.Parse(cell_value2.ToString());

                e.SortResult = a.CompareTo(b);
                e.Handled = true;
            }
        }
        #endregion

        #region 우클릭 메뉴
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
                    if (dgvProduct.SelectedRows.Count > 0)
                    {
                        ContextMenuStrip m = new ContextMenuStrip();
                        m.Items.Add("선택 품목캡쳐");
                        m.Items.Add("즐겨찾기 등록");
                        if(Convert.ToBoolean(dgvProduct.Rows[row].Cells["is_merge"].Value))
                            m.Items.Add("대표품목 취소");
                        else
                            m.Items.Add("대표품목 설정");
                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked2);
                        //Create 
                        m.BackColor = Color.White;
                        m.Show(dgvProduct, e.Location);
                        //Selection
                        /*PendingList.ClearSelection();
                        DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                        selectRow.Selected = !selectRow.Selected;*/
                    }
                }
            }
            catch
            {
            }
        }
        //우클릭 메뉴 Event Handler
        Libs.ftpCommon ftp = new Libs.ftpCommon();
        void m_ItemClicked2(object sender, ToolStripItemClickedEventArgs e)
        {
            if (dgvProduct.SelectedRows.Count > 0)
            {
                int errCnt = 0;
                try
                {
                    DataGridViewRow row = GetSelectProductDgvr();
                    DataGridViewRow dr = dgvProduct.SelectedRows[0];
                    if (dr.Index < 0)
                        return;
                    //Function
                    switch (e.ClickedItem.Text)
                    {
                        case "선택 품목캡쳐":
                            {
                                if (MessageBox.Show(this, "저장 위치는 바탕화면 TEMP 폴더입니다.", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                    return;
                                string save_path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\TEMP";
                                DirectoryInfo di = new DirectoryInfo(save_path);
                                if (di.Exists == false)
                                    di.Create();
                                errCnt++;
                                int cnt = 1;
                                //pnProductInfo.Visible = true;
                                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                                {
                                    if (dgvProduct.Rows[i].Selected)
                                    {
                                        GetDashboard(dgvProduct.Rows[i]);
                                        this.Update();
                                        Thread.Sleep(2);
                                        string file_name = cnt.ToString()
                                            + "_" + ftp.ReplaceName(dgvProduct.Rows[i].Cells["product"].Value.ToString())
                                            + "_" + ftp.ReplaceName(dgvProduct.Rows[i].Cells["origin"].Value.ToString())
                                            + "_" + ftp.ReplaceName(dgvProduct.Rows[i].Cells["sizes"].Value.ToString())
                                            + "_" + ftp.ReplaceName(dgvProduct.Rows[i].Cells["seaover_unit"].Value.ToString())
                                            + ".png";

                                        common.ScreenCaptureForm(new Point(this.Left, this.Top), this.Size, save_path + @"\" + file_name);

                                        cnt++;
                                    }
                                }
                                //pnProductInfo.Visible = false;
                                errCnt++;
                                System.Diagnostics.Process.Start(save_path);
                            }
                            break;
                        case "즐겨찾기 등록":

                            {
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
                                            double cost_unit;
                                            if (dgvRow.Cells["cost_unit"].Value == null || !double.TryParse(dgvRow.Cells["cost_unit"].Value.ToString(), out cost_unit))
                                                cost_unit = 0;
                                            gModel.cost_unit = cost_unit.ToString();
                                            gModel.month_around = 0;
                                            gModel.enable_days = 0;
                                            gModel.price_unit = dgvRow.Cells["price_unit"].Value.ToString();
                                            gModel.unit_count = dgvRow.Cells["unit_count"].Value.ToString();
                                            gModel.seaover_unit = dgvRow.Cells["seaover_unit"].Value.ToString();
                                            gModel.division = dgvRow.Cells["division"].Value.ToString();

                                            list.Add(gModel);
                                        }
                                    }

                                    if (list.Count > 0)
                                    {
                                        SEAOVER.BookmarkManager bm = new BookmarkManager(um, this, list, 1);
                                        bm.ShowDialog();
                                    }
                                }
                            }
                            break;
                        case "대표품목 설정":
                            {
                                if (dgvProduct.SelectedRows.Count >= 2)
                                    SetMainProduct();
                                else
                                    messageBox.Show(this, "대표품목으로 묶을 품목을 선택해주세요.");
                            }
                            break;
                        case "대표품목 취소":
                            //권한확인
                            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                            if (authorityDt != null && authorityDt.Rows.Count > 0)
                            {
                                if (!common.CheckAuthority(authorityDt, "씨오버", "품명별 매출관리 대시보드", "is_update"))
                                {
                                    messageBox.Show(this, "권한이 없습니다!");
                                    return;
                                }
                            }

                            int main_id;
                            if (row.Cells["main_id"].Value == null || !int.TryParse(row.Cells["main_id"].Value.ToString(), out main_id))
                            {
                                messageBox.Show(this, "대표품목으로 묶이지 않은 품목입니다!");
                                return;
                            }

                            //설정데이터 삭제
                            ProductGroupModel model = new ProductGroupModel();
                            model.main_id = main_id;
                            model.product = row.Cells["product"].Value.ToString();
                            model.origin = row.Cells["origin"].Value.ToString();
                            model.sizes = row.Cells["sizes"].Value.ToString();
                            model.unit = row.Cells["unit"].Value.ToString();
                            model.price_unit = row.Cells["price_unit"].Value.ToString();
                            model.unit_count = row.Cells["unit_count"].Value.ToString();
                            model.seaover_unit = row.Cells["seaover_unit"].Value.ToString();

                            StringBuilder sql = productGroupRepository.DeleteProductGroup(model);
                            List<StringBuilder> sqlList = new List<StringBuilder>();
                            sqlList.Add(sql);

                            if (productGroupRepository.UpdateTrans(sqlList) == -1)
                            {
                                messageBox.Show(this, "삭제실패!");
                                this.Activate();
                            }
                            else
                            {
                                foreach (DataGridViewRow productRow in dgvProduct.Rows)
                                {
                                    if (productRow.Cells["main_id"].Value != null && productRow.Cells["main_id"].Value.ToString().Equals(main_id.ToString()))
                                    {
                                        productRow.Cells["main_id"].Value = "";
                                        productRow.Cells["merge_product"].Value = "";
                                        productRow.Cells["is_merge"].Value = false;
                                    }
                                }

                                messageBox.Show(this, "삭제완료");
                                this.Activate();
                            }
                            
                            break;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(this,ex.Message + "\n" + errCnt);
                }
            }
        }

        private void SetMainProduct()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "품명별 매출관리 대시보드", "is_update"))
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


        public void SetMergeProduct(DataGridView resultDt, int main_id, string merge_product)
        {
            if (resultDt.RowCount > 0)
            {
                foreach (DataGridViewRow row in dgvProduct.Rows)
                {
                    foreach (DataGridViewRow resultRow in resultDt.Rows)
                    {
                        if (row.Cells["product"].Value.ToString() == resultRow.Cells["product"].Value.ToString()
                            && row.Cells["origin"].Value.ToString() == resultRow.Cells["origin"].Value.ToString()
                            && row.Cells["sizes"].Value.ToString() == resultRow.Cells["sizes"].Value.ToString()
                            && row.Cells["unit"].Value.ToString() == resultRow.Cells["unit"].Value.ToString()
                            && row.Cells["price_unit"].Value.ToString() == resultRow.Cells["price_unit"].Value.ToString()
                            && row.Cells["unit_count"].Value.ToString() == resultRow.Cells["unit_count"].Value.ToString()
                            && row.Cells["seaover_unit"].Value.ToString() == resultRow.Cells["seaover_unit"].Value.ToString())
                        {
                            row.Cells["is_merge"].Value = true;
                            row.Cells["main_id"].Value = main_id.ToString();
                            row.Cells["merge_product"].Value = merge_product;
                        }
                    }
                }
            }
        }
        



        #endregion

        #region 인쇄, 미리보기

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        private Bitmap memoryImage;

        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        private void Printing()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "품명별 매출관리 대시보드", "is_print"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            CaptureScreen();
            printDocument1.DefaultPageSettings.Landscape = true;
            //printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A3", 297 * 4, 420 * 5);
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4", this.Height, this.Width);
            PrintManager pm = new PrintManager(printDocument1);
            pm.ShowDialog();
        }

        private void CaptureScreen()
        {
            Graphics mygraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height, mygraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            IntPtr dc1 = mygraphics.GetHdc();
            IntPtr dc2 = memoryGraphics.GetHdc();
            BitBlt(dc2, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, dc1, 0, 0, 13369376);
            mygraphics.ReleaseHdc(dc1);
            memoryGraphics.ReleaseHdc(dc2);
        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bmp = new Bitmap(this.Width, this.Height);

            float tgtWidthMM = 297;  //A4 paper size
            float tgtHeightMM = 210;
            float tgtWidthInches = tgtWidthMM / 25.4f;
            float tgtHeightInches = tgtHeightMM / 25.4f;
            float srcWidthPx = bmp.Width;
            float srcHeightPx = bmp.Height;
            float dpiX = srcWidthPx / tgtWidthInches;
            float dpiY = srcHeightPx / tgtHeightInches;

            bmp.SetResolution(dpiX, dpiY);

            this.DrawToBitmap(bmp, this.ClientRectangle);

            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            e.Graphics.DrawImage(bmp, 0, 0, tgtWidthMM, tgtHeightMM);













            /*int height = 210;
            int width = 297;
            Size size = new Size(width, height);
            Bitmap bmp = new Bitmap(memoryImage, size);
            e.Graphics.DrawImage(memoryImage, 0, 0);*/
        }


        #endregion

        #region 국내수입 등록/삭제
        private void btnAddDomestic_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectProductDgvr();
            if (row == null || dgvContract.Rows.Count == 0)
                return;
            this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            //생산, 배송, 팬딩기간
            double shippingTerm;
            if (!double.TryParse(txtShippingTerm.Text, out shippingTerm))
                shippingTerm = 0;
            double pendingTerm;
            if (!double.TryParse(txtPendingTerm.Text, out pendingTerm))
                pendingTerm = 0;
            double productionTerm;
            if (!double.TryParse(txtMakeTerm.Text, out productionTerm))
                productionTerm = 0;
            //새행 추가
            dgvContract.Rows.Insert(dgvContract.Rows.Count - 1, 1);
            //계산방식
            bool isWeight;
            if (dgvContract.Rows[dgvContract.Rows.Count - 3].Cells["pending_weight_calculate"].Value == null || !bool.TryParse(dgvContract.Rows[dgvContract.Rows.Count - 3].Cells["pending_weight_calculate"].Value.ToString(), out isWeight))
                isWeight = true;
            //실재고
            double total_qty;
            if (dgvStockSales.Rows[0].Cells["total_qty"].Value == null || !double.TryParse(dgvStockSales.Rows[0].Cells["total_qty"].Value.ToString(), out total_qty))
                total_qty = 0;
            //실재고도 없고 팬딩도 없는 경우 현시점기준 계약했다고 치고 다시 계산
            if (total_qty == 0 && dgvContract.Rows.Count <= 3)
            {
                DateTime etd = DateTime.Now.AddDays(productionTerm);
                DateTime warehousing_date = DateTime.Now.AddDays(productionTerm + shippingTerm + pendingTerm);
                dgvContract.Rows[dgvContract.Rows.Count - 2].Cells["etd"].Value = etd.ToString("yyyy-MM-dd");
                dgvContract.Rows[dgvContract.Rows.Count - 2].Cells["warehousing_date"].Value = warehousing_date.ToString("yyyy-MM-dd");
            }
            //아닌 경우 전팬딩 내역을 가져오기
            else
            {
                dgvContract.Rows[dgvContract.Rows.Count - 2].Cells["etd"].Value = dgvContract.Rows[dgvContract.Rows.Count - 3].Cells["recommend_etd"].Value;
                dgvContract.Rows[dgvContract.Rows.Count - 2].Cells["warehousing_date"].Value = dgvContract.Rows[dgvContract.Rows.Count - 3].Cells["after_qty_exhausted_date"].Value;
            }

            dgvContract.Rows[dgvContract.Rows.Count - 2].Cells["pending_weight_calculate"].Value = isWeight;
            dgvContract.Rows[dgvContract.Rows.Count - 2].Cells["pending_term"].Value = (shippingTerm + pendingTerm).ToString();
            dgvContract.Rows[dgvContract.Rows.Count - 2].Cells["btnRegister"].Value = "등록";
            dgvContract.Rows[dgvContract.Rows.Count - 2].Cells["btnUpdate"].Value = "수정";
            dgvContract.Rows[dgvContract.Rows.Count - 2].Cells["is_domestic"].Value = true;
            dgvContract.Rows.Remove(dgvContract.Rows[dgvContract.Rows.Count - 1]);

            ShortManagerModel smModel = GetProductStockInfo(GetSelectProductDgvr());
            if (SortSetting())
                CalculateStock(smModel);


            dgvContract.Rows[dgvContract.Rows.Count - 2].Cells["etd"].Style.ForeColor = Color.Gray;
            this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
        }

        private void dgvContract_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                this.dgvPurchaseUsd.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseUsd_CellValueChanged);
                this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);

                if (dgvContract.Columns[e.ColumnIndex].Name.Equals("btnRegister"))
                {
                    if (dgvContract.Rows[e.RowIndex].Cells["btnRegister"].Value != null
                        && !string.IsNullOrEmpty(dgvContract.Rows[e.RowIndex].Cells["btnRegister"].Value.ToString()))
                    {
                        DataGridViewRow select_row = GetSelectProductDgvr();
                        if (select_row == null)
                        {
                            this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                            this.dgvPurchaseUsd.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseUsd_CellValueChanged);
                            this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                            return;
                        }
                        string btn_value = dgvContract.Rows[e.RowIndex].Cells["btnRegister"].Value.ToString();

                        DomesticModel model = new DomesticModel();
                        int id;
                        if (dgvContract.Rows[e.RowIndex].Cells["domestic_id"].Value == null || !int.TryParse(dgvContract.Rows[e.RowIndex].Cells["domestic_id"].Value.ToString(), out id))
                            id = 0;
                        if (id == 0)
                            id = commonRepository.GetNextId("t_domestic", "id");


                        DateTime warehousing_date;
                        if (dgvContract.Rows[e.RowIndex].Cells["warehousing_date"].Value == null || !DateTime.TryParse(dgvContract.Rows[e.RowIndex].Cells["warehousing_date"].Value.ToString(), out warehousing_date))
                        {
                            MessageBox.Show(this, "입고일자를 입력해주세요.");
                            this.Activate();
                            return;
                        }


                        DataGridViewRow row = GetSelectProductDgvr();
                        if (row == null)
                            return;

                        model.id = id;
                        model.sub_id = 1;
                        model.status = "선적전";
                        if (dgvContract.Rows[e.RowIndex].Cells["etd"].Value == null)
                            dgvContract.Rows[e.RowIndex].Cells["etd"].Value = string.Empty;
                        model.etd = dgvContract.Rows[e.RowIndex].Cells["etd"].Value.ToString();
                        model.warehousing_date = warehousing_date.ToString("yyyy-MM-dd");
                        model.product = row.Cells["product"].Value.ToString();
                        model.origin = row.Cells["origin"].Value.ToString();
                        model.sizes = row.Cells["sizes"].Value.ToString();
                        model.unit = row.Cells["unit"].Value.ToString();
                        model.price_unit = row.Cells["price_unit"].Value.ToString();
                        model.unit_count = row.Cells["unit_count"].Value.ToString();
                        model.seaover_unit = row.Cells["seaover_unit"].Value.ToString();

                        double qty;
                        if (dgvContract.Rows[e.RowIndex].Cells["qty"].Value == null || !double.TryParse(dgvContract.Rows[e.RowIndex].Cells["qty"].Value.ToString(), out qty))
                        {
                            MessageBox.Show(this, "수량을 입력해주세요.");
                            this.Activate();
                            return;
                        }
                        model.qty = qty;

                        double cost_price;
                        if (dgvContract.Rows[e.RowIndex].Cells["etd_cost_price"].Value == null || !double.TryParse(dgvContract.Rows[e.RowIndex].Cells["etd_cost_price"].Value.ToString(), out cost_price))
                            cost_price = 0;
                        model.cost_price = cost_price;

                        model.edit_user = um.user_name;
                        model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


                        List<StringBuilder> sqlList = new List<StringBuilder>();
                        StringBuilder sql = domesticRepository.DeleteDomestic(model);
                        sqlList.Add(sql);

                        sql = domesticRepository.InsertDomestic(model);
                        sqlList.Add(sql);


                        if (MessageBox.Show(this, "국내수입 내역을 저장하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                            return;


                        if (commonRepository.UpdateTran(sqlList) == -1)
                            MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                        else
                        {
                            dgvContract.Rows[e.RowIndex].Cells["domestic_id"].Value = model.id;
                            MessageBox.Show(this, "등록완료");
                        }
                        this.Activate();
                    }
                }
                else if (dgvContract.Columns[e.ColumnIndex].Name.Equals("isDelete"))
                {
                    if (MessageBox.Show(this, "선택한 국내수입 내역을 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                        this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                        this.dgvPurchaseUsd.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseUsd_CellValueChanged);
                        this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                        return;
                    }
                    List<StringBuilder> sqlList = new List<StringBuilder>();
                    int id;
                    if (dgvContract.Rows[e.RowIndex].Cells["domestic_id"].Value == null || !int.TryParse(dgvContract.Rows[e.RowIndex].Cells["domestic_id"].Value.ToString(), out id))
                        id = 0;
                    if (id > 0)
                    {
                        DomesticModel model = new DomesticModel();
                        model.id = id;

                        StringBuilder sql = domesticRepository.DeleteDomestic(model);
                        sqlList.Add(sql);
                    }
                    //Execute
                    if (sqlList.Count > 0 && commonRepository.UpdateTran(sqlList) == -1)
                    {
                        MessageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                        this.Activate();
                        return;
                    }

                    //dgv 반영
                    dgvContract.Rows.Remove(dgvContract.Rows[e.RowIndex]);
                    dgvContract.Rows.Remove(dgvContract.Rows[dgvContract.Rows.Count - 1]);
                    ShortManagerModel smModel = GetProductStockInfo(GetSelectProductDgvr());
                    if (SortSetting())
                        CalculateStock(smModel);
                }
                else if (dgvContract.Columns[e.ColumnIndex].Name.Equals("btnUpdate"))
                {
                    DataGridViewRow select_row = GetSelectProductDgvr();
                    if (select_row == null)
                    {
                        this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                        this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                        this.dgvPurchaseUsd.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseUsd_CellValueChanged);
                        this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                        return;
                    }

                    Domestic.SimpleDomesticManager dm = new Domestic.SimpleDomesticManager(um
                            , select_row.Cells["product"].Value.ToString(), select_row.Cells["origin"].Value.ToString(), select_row.Cells["sizes"].Value.ToString()
                            , select_row.Cells["unit"].Value.ToString(), select_row.Cells["price_unit"].Value.ToString(), select_row.Cells["unit_count"].Value.ToString(), select_row.Cells["seaover_unit"].Value.ToString()
                            , dgvContract.Rows[e.RowIndex]);
                    dm.Show();
                    return;


                }
                else if (dgvContract.Columns[e.ColumnIndex].Name.Equals("trq") && e.RowIndex > 0)
                {
                    if (e.RowIndex == dgvContract.Rows.Count - 1)
                    {
                        bool isChecked;
                        if (dgvContract.Rows[0].Cells[e.ColumnIndex].Value == null || !Convert.ToBoolean(dgvContract.Rows[0].Cells[e.ColumnIndex].Value))
                            isChecked = true;
                        else
                            isChecked = false;

                        foreach (DataGridViewRow row in dgvContract.Rows)
                            row.Cells["trq"].Value = isChecked;
                    }
                    dgvContract.EndEdit();
                    CalculateStock();
                }
                else if (dgvContract.Columns[e.ColumnIndex].Name.Equals("pending_weight_calculate") && e.RowIndex > 0)
                {
                    dgvContract.EndEdit();
                    CalculateStock();
                }
                    
                this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                this.dgvPurchaseUsd.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseUsd_CellValueChanged);
            }
        }

        #endregion

        private void DetailDashBoardForSales_FormClosing(object sender, FormClosingEventArgs e)
        {
            SetWindowDisplayAffinity(this.Handle, WDA_NONE);
        }

        

        #region Seaover 원가상세
        private void dgvSeaoverCostPrice_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvSeaoverCostPrice.EndEdit();
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = GetSelectProductDgvr();
                if (dgvSeaoverCostPrice.Columns[e.ColumnIndex].Name == "seaover_is_trq" && row != null)
                {
                    bool isChecked;
                    if (dgvSeaoverCostPrice.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !bool.TryParse(dgvSeaoverCostPrice.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out isChecked))
                        isChecked = false;

                    double offer_price;
                    if (dgvSeaoverCostPrice.Rows[e.RowIndex].Cells["seaover_cost_price_usd"].Value == null || !double.TryParse(dgvSeaoverCostPrice.Rows[e.RowIndex].Cells["seaover_cost_price_usd"].Value.ToString(), out offer_price))
                        offer_price = 0;

                    if (offer_price > 0)
                    {
                        double exchange_rate;
                        if (dgvSeaoverCostPrice.Rows[e.RowIndex].Cells["seaover_exchange_rate"].Value == null || !double.TryParse(dgvSeaoverCostPrice.Rows[e.RowIndex].Cells["seaover_exchange_rate"].Value.ToString(), out exchange_rate))
                            exchange_rate = 0;

                        bool weight_calculate;
                        if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                            weight_calculate = false;

                        //밑에서 이자랑 냉장료를 따로 계산
                        double cost_price = CalculateEtdCostPrice("", offer_price, weight_calculate, isChecked, exchange_rate);
                        dgvSeaoverCostPrice.Rows[e.RowIndex].Cells["seaover_cost_price_krw"].Value = cost_price.ToString("#,##0");


                        string ato_no = dgvSeaoverCostPrice.Rows[e.RowIndex].Cells["seaover_ato_no"].Value.ToString();

                        /*if (ato_no.Contains("AD") || ato_no.Contains("DW") || ato_no.Contains("OD") || ato_no.Contains("JD") || ato_no.Contains("HS"))
                            cost_price *= 1.035;*/

                        double stock_unit;
                        if (row.Cells["seaover_unit"].Value == null || !double.TryParse(row.Cells["seaover_unit"].Value.ToString(), out stock_unit))
                            stock_unit = 0;
                        double unit_count;
                        if (row.Cells["unit_count"].Value == null || !double.TryParse(row.Cells["unit_count"].Value.ToString(), out unit_count))
                            unit_count = 0;

                        //2023-10-30 무역부 요청으로 수정
                        //2023-10-30 냉장료 포함
                        double refrigeration_fee = 0;
                        DateTime in_date;
                        string in_date_txt = "";
                        if (dgvSeaoverCostPrice.Rows[e.RowIndex].Cells["in_date"].Value != null && DateTime.TryParse(dgvSeaoverCostPrice.Rows[e.RowIndex].Cells["in_date"].Value.ToString(), out in_date))
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
                            in_date_txt = in_date.ToString("yyyy-MM-dd");
                        }

                        //2023-10-25 etd기준 연이자 8% 일발생
                        double interest = 0;
                        string etd_txt = "";
                        if (dgvSeaoverCostPrice.Rows[e.RowIndex].Cells["stock_etd"].Value != null && DateTime.TryParse(dgvSeaoverCostPrice.Rows[e.RowIndex].Cells["stock_etd"].Value.ToString(), out DateTime etd))
                        {
                            TimeSpan ts = DateTime.Now - etd;
                            int days = ts.Days;
                            interest = cost_price * 0.08 / 365 * days;
                            etd_txt = etd.ToString("yyyy-MM-dd");
                            if (interest < 0)
                                interest = 0;
                        }

                        dgvSeaoverCostPrice.Rows[e.RowIndex].Cells["final_seaover_cost_price_krw"].Value = (cost_price + interest + refrigeration_fee).ToString("#,##0");

                    }
                }
                else if (dgvSeaoverCostPrice.Columns[e.ColumnIndex].Name == "btnCustomUpdate" && row != null)
                {
                    dgvSeaoverCostPrice.EndEdit();
                    int custom_id;
                    if (dgvSeaoverCostPrice.Rows[e.RowIndex].Cells["custom_id"].Value == null || !int.TryParse(dgvSeaoverCostPrice.Rows[e.RowIndex].Cells["custom_id"].Value.ToString(), out custom_id))
                        custom_id = 0;
                    int sub_id;
                    if (dgvSeaoverCostPrice.Rows[e.RowIndex].Cells["sub_id"].Value == null || !int.TryParse(dgvSeaoverCostPrice.Rows[e.RowIndex].Cells["sub_id"].Value.ToString(), out sub_id))
                        sub_id = 0;
                    double trq_price;
                    if (!double.TryParse(txtTrq.Text, out trq_price))
                        trq_price = 0;

                    bool isTrq;
                    if (dgvSeaoverCostPrice.Rows[e.RowIndex].Cells["seaover_is_trq"].Value == null || !bool.TryParse(dgvSeaoverCostPrice.Rows[e.RowIndex].Cells["seaover_is_trq"].Value.ToString(), out isTrq))
                        isTrq = false;
                    if (!isTrq)
                        trq_price = 0;

                    if (custom_id > 0 && sub_id > 0 & (um.department.Contains("무역") || um.department.Contains("관리부") || um.department.Contains("전산")))
                    {
                        StringBuilder sql = commonRepository.UpdateData("t_customs", $"trq_amount = {trq_price.ToString()}", $"id = {custom_id} AND sub_id = {sub_id}");
                        List<StringBuilder> sqlList = new List<StringBuilder>();
                        sqlList.Add(sql);

                        if (commonRepository.UpdateTran(sqlList) == -1)
                            MessageBox.Show(this, "수정중 에러가 발생하였습니다.");
                        else
                        {
                            this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                            this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                            OpenExhaustedManager(GetSelectProductDgvr());
                            this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                            this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                            
                            pnTooltip.Visible = false;
                            pnTooltip.Enabled = false;
                            MessageBox.Show(this, "완료");
                        }
                    }

                }
                CalculateSeaoverCostPrice();
                this.Activate();
            }
        }
        private void CalculateSeaoverCostPrice()
        {
            double total_cost_price = 0, total_qty = 0;
            foreach (DataGridViewRow row in dgvSeaoverCostPrice.Rows)
            {
                if (row.Cells["cost_price_chk"].Value != null && bool.TryParse(row.Cells["cost_price_chk"].Value.ToString(), out bool isChecked) && isChecked)
                {
                    double cost_price;
                    if (row.Cells["final_seaover_cost_price_krw"].Value == null || !double.TryParse(row.Cells["final_seaover_cost_price_krw"].Value.ToString(), out cost_price))
                        cost_price = 0;
                    double qty;
                    if (row.Cells["seaover_qty"].Value == null || !double.TryParse(row.Cells["seaover_qty"].Value.ToString(), out qty))
                        qty = 0;

                    total_cost_price += cost_price * qty;
                    total_qty += qty;
                }
            }

            if (total_cost_price > 0 && total_qty > 0)
                txtSeaoverCostPrice.Text = (total_cost_price / total_qty).ToString("#,##0");

        }

        private void dgvContract_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0 && e.ColumnIndex >= 0)
            {
                if (dgvContract.Columns[e.ColumnIndex].Name == "etd_cost_price")
                {
                    pnTooltip.Visible = true;
                    pnTooltip.Enabled = true;
                }
            }
        }

        private void dgvStockSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0 && e.ColumnIndex >= 0)
            {
                if (dgvStockSales.Columns[e.ColumnIndex].Name == "seaover_cost_price")
                {
                    pnTooltip.Visible = true;
                    pnTooltip.Enabled = true;
                }
            }

        }
        #endregion

        private void dgvProduct_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = false;
        }

        
        
        private void dgvContract_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvContract.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    dgvContract.ClearSelection();
                    dgvContract.Rows[row].Cells["ato_no"].Selected = true;

                    int id;
                    if (dgvContract.Rows[row].Cells["pending_id"].Value == null || !Int32.TryParse(dgvContract.Rows[row].Cells["pending_id"].Value.ToString(), out id))
                        id = 0;

                    if (id > 0 && (um.department.Contains("무역") || um.department.Contains("관리부") || um.department.Contains("전산")))
                    {
                        ContextMenuStrip m = new ContextMenuStrip();
                        m.Items.Add("팬딩 열기");
                        m.Items.Add("중량 계산");
                        m.Items.Add("트레이 계산");
                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked3);
                        //Create 
                        m.BackColor = Color.White;
                        m.Show(dgvContract, e.Location);
                        //Selection
                        /*PendingList.ClearSelection();
                        DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                        selectRow.Selected = !selectRow.Selected;*/
                    }
                }
            }
            catch
            {
            }
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked3(object sender, ToolStripItemClickedEventArgs e)
        {
            if (dgvContract.SelectedCells.Count > 0 && (um.department.Contains("무역") || um.department.Contains("관리부") || um.department.Contains("전산")))
            {
                int errCnt = 0;
                try
                {
                    DataGridViewRow row = GetSelectProductDgvr();
                    DataGridViewRow dr = dgvContract.Rows[dgvContract.SelectedCells[0].RowIndex];
                    if (dr.Index < 0)
                        return;
                    //Function
                    switch (e.ClickedItem.Text)
                    {
                        case "팬딩 열기":
                            int id;
                            if (dr.Cells["pending_id"].Value == null || !Int32.TryParse(dr.Cells["pending_id"].Value.ToString(), out id))
                                id = 0;
                            if (id > 0)
                            {
                                UnPendingManager upm = new UnPendingManager(um, id);
                                upm.StartPosition = FormStartPosition.CenterParent;
                                upm.ShowDialog();
                            }
                            break;
                        case "중량 계산":
                            {
                                dr.Cells["weight_calculate"].Value = true;
                            }
                            break;
                        case "트레이 계산":
                            {
                                dr.Cells["weight_calculate"].Value = false;
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this,ex.Message + "\n" + errCnt);
                }
            }
        }

        private void cbPendingDetail_CheckedChanged(object sender, EventArgs e)
        {
            dgvContract.Columns["pending_weight_calculate"].Visible = cbPendingDetail.Checked;
            dgvContract.Columns["product_unit"].Visible = cbPendingDetail.Checked;
            dgvContract.Columns["pending_cost_unit"].Visible = cbPendingDetail.Checked;
        }

        private void txtSalesPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SetDashboardSummary();
        }
    }
}

