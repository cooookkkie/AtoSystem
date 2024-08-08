using AdoNetWindow.Model;
using AdoNetWindow.Pending;
using AdoNetWindow.SaleManagement;
using AdoNetWindow.SEAOVER.PriceComparison;
using Google.Api;
using iTextSharp.text.pdf.parser.clipper;
using Microsoft.Office.Interop.Excel;
using MySqlX.XDevAPI.Relational;
using Org.BouncyCastle.Asn1.Cmp;
using Repositories;
using Repositories.Config;
using Repositories.ContractRecommendation;
using Repositories.Domestic;
using Repositories.SaleProduct;
using Repositories.SEAOVER;
using Repositories.SEAOVER.Purchase;
using Repositories.SEAOVER.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;
using DataTable = System.Data.DataTable;
using Font = System.Drawing.Font;

namespace AdoNetWindow.DashboardForSales.MultiDashboard
{
        public partial class MultiDashboardUnit : UserControl
    {
        class ProductModel
        {
            public string product { get; set; }
            public string origin { get; set; }
            public string sizes { get; set; }
            public string unit { get; set; }
            public string price_unit { get; set; }
            public string unit_count { get; set; }
            public string seaover_unit { get; set; }
            public bool weight_calculate { get; set; }
            public bool isMerge { get; set; }
            public string sub_products { get; set; }
        }
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        IPriceComparisonRepository priceComparisonRepository = new PriceComparisonRepository();
        ISalesRepository salesRepository = new SalesRepository();
        IProductExcludedSalesRepository productExcludedSalesRepository = new ProductExcludedSalesRepository();
        IPurchasePriceRepository purchasePriceRepository = new PurchasePriceRepository();
        IProductOtherCostRepository productOtherCostRepository = new ProductOtherCostRepository();
        IProductGroupRepository productGroupRepository = new ProductGroupRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        IPurchaseRepository purchaseRepository = new PurchaseRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        ICommonRepository commonRepository = new CommonRepository();
        IDomesticRepository domesticRepository = new DomesticRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        ProductModel this_product = new ProductModel();
        List<ProductModel> product = new List<ProductModel>();
        UsersModel um;
        MultiDashBoard db;
        public DataTable marketDt = new DataTable();
        public DataTable exchangeRateDt = new DataTable();
        public DataTable customDt = new DataTable();
        public DataTable excluedDt = new DataTable();
        public DataTable productStockSalesDt = new DataTable();
        public DataTable unpendingDt = new DataTable();
        public MultiDashboardUnit(UsersModel um, MultiDashBoard db, string product, string origin, string sizes, string unit, string price_unit, string unit_count, string seaover_unit
                , DataTable marketDt, DataTable exchangeRateDt, DataTable customDt, DataTable excluedDt, DataTable productStockSalesDt, DataTable unpendingDt)
        {
            InitializeComponent();
            this.um = um;
            this.db = db;
            //품목정보
            this_product.product = product;
            lbProduct.Text = product;
            this_product.origin = origin;
            lbOrigin.Text = origin;
            this_product.sizes = sizes;
            lbSizes.Text = sizes;
            this_product.unit = unit;
            lbUnit.Text = unit;
            this_product.price_unit = price_unit;
            lbPriceUnit.Text = price_unit;
            this_product.unit_count = unit_count;
            lbUnitCount.Text = unit_count;
            this_product.seaover_unit = seaover_unit;
            lbSeaoverUnit.Text = seaover_unit;

            /*lbProduct.Text = "품명 : " + product + "      원산지 : " + origin + "      규격 : " + sizes;
            lbUnit.Text = "단위 : " + unit + "      가격단위 : " + price_unit + "      단위수량 : " + unit_count + "      S단위 : " + seaover_unit;*/
            DataTable productGroupDt = productGroupRepository.GetSubProduct(product, origin, sizes, unit, price_unit, unit_count, seaover_unit);
            //병합된 품목
            if (productGroupDt != null && productGroupDt.Rows.Count > 0)
            {
                //서브품목 정보
                string sub_products = "";
                foreach (DataRow dr in productGroupDt.Rows)
                {
                    if (!string.IsNullOrEmpty(sub_products))
                        sub_products += "\n" + dr["item_product"].ToString()
                            + "^" + dr["item_origin"].ToString()
                            + "^" + dr["item_sizes"].ToString()
                            + "^" + dr["item_unit"].ToString()
                            + "^" + dr["item_price_unit"].ToString()
                            + "^" + dr["item_unit_count"].ToString()
                            + "^" + dr["item_seaover_unit"].ToString();
                    else
                    {
                        sub_products = dr["item_product"].ToString()
                            + "^" + dr["item_origin"].ToString()
                            + "^" + dr["item_sizes"].ToString()
                            + "^" + dr["item_unit"].ToString()
                            + "^" + dr["item_price_unit"].ToString()
                            + "^" + dr["item_unit_count"].ToString()
                            + "^" + dr["item_seaover_unit"].ToString();
                    }
                }

                //메인품목 먼저 등록
                foreach (DataRow dr in productGroupDt.Rows)
                {   
                    if (dr["product"].ToString() == dr["item_product"].ToString() && dr["origin"].ToString() == dr["item_origin"].ToString()
                        && dr["sizes"].ToString() == dr["item_sizes"].ToString() && dr["unit"].ToString() == dr["item_unit"].ToString()
                        && dr["price_unit"].ToString() == dr["item_price_unit"].ToString() && dr["unit_count"].ToString() == dr["item_unit_count"].ToString()
                        && dr["seaover_unit"].ToString() == dr["item_seaover_unit"].ToString())
                    {
                        ProductModel model = new ProductModel();
                        model.product = dr["item_product"].ToString();
                        model.origin = dr["item_origin"].ToString();
                        model.sizes = dr["item_sizes"].ToString();
                        model.unit = dr["item_unit"].ToString();
                        model.price_unit = dr["item_price_unit"].ToString();
                        model.unit_count = dr["item_unit_count"].ToString();
                        model.seaover_unit = dr["item_seaover_unit"].ToString();
                        model.isMerge = true;
                        model.sub_products = sub_products;
                        this.product.Add(model);
                        break;
                    }
                }

                

                //서브품목 등록
                foreach (DataRow dr in productGroupDt.Rows)
                {
                    if (!(dr["product"].ToString() == dr["item_product"].ToString() && dr["origin"].ToString() == dr["item_origin"].ToString()
                    && dr["sizes"].ToString() == dr["item_sizes"].ToString() && dr["unit"].ToString() == dr["item_unit"].ToString()
                    && dr["price_unit"].ToString() == dr["item_price_unit"].ToString() && dr["unit_count"].ToString() == dr["item_unit_count"].ToString()
                        && dr["seaover_unit"].ToString() == dr["item_seaover_unit"].ToString()))
                    {
                        ProductModel model = new ProductModel();
                        model.product = dr["item_product"].ToString();
                        model.origin = dr["item_origin"].ToString();
                        model.sizes = dr["item_sizes"].ToString();
                        model.unit = dr["item_unit"].ToString();
                        model.price_unit = dr["item_price_unit"].ToString();
                        model.unit_count = dr["item_unit_count"].ToString();
                        model.seaover_unit = dr["item_seaover_unit"].ToString();
                        model.isMerge = true;
                        model.sub_products = sub_products;
                        this.product.Add(model);
                    }
                }
            }
            //병합되지 않은 품목
            else
            {
                string sub_products = product
                            + "^" + origin
                            + "^" + sizes
                            + "^" + unit
                            + "^" + price_unit
                            + "^" + unit_count
                            + "^" + seaover_unit;

                ProductModel model = new ProductModel();
                model.product = product;
                model.origin = origin;
                model.sizes = sizes;
                model.unit = unit;
                model.price_unit = price_unit;
                model.unit_count = unit_count;
                model.seaover_unit = seaover_unit;
                model.isMerge = false;
                model.sub_products = sub_products;
                this.product.Add(model);
            }

            //환율가져오기
            double customRate = common.GetExchangeRateKEBBank("USD");
            txtExchangeRate.Text = customRate.ToString("#,##0.00");

            //기타정보
            DataTable productDt = productOtherCostRepository.GetProductInfoAsOneExactly(this.product[0].product, this.product[0].origin, this.product[0].sizes, this.product[0].seaover_unit);
            if(productDt != null && productDt.Rows.Count > 0)
            {
                txtCustom.Text = productDt.Rows[0]["custom"].ToString();
                txtTax.Text = productDt.Rows[0]["tax"].ToString();
                txtIncidentalExpense.Text = productDt.Rows[0]["incidental_expense"].ToString();
                txtMakeTerm.Text = productDt.Rows[0]["production_days"].ToString();
                txtShippingTerm.Text = productDt.Rows[0]["delivery_days"].ToString();
                txtPurchaseMargin.Text = productDt.Rows[0]["purchase_margin"].ToString();

                foreach (ProductModel model in this.product)
                    model.weight_calculate = Convert.ToBoolean(productDt.Rows[0]["weight_calculate"].ToString());
            }

            this.marketDt = marketDt;
            this.exchangeRateDt = exchangeRateDt;
            this.customDt = customDt;
            this.excluedDt = excluedDt;
            this.productStockSalesDt = productStockSalesDt;
            this.unpendingDt = unpendingDt;

            priceComparisonRepository.SetSeaoverId("200009");
        }


        private void MultiDashboardUnit_Load(object sender, EventArgs e)
        {
            dgvMarket.Columns["company_sales_price"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);
            dgvMarket.Columns["company_sales_price"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
            dgvMarket.Columns["company_purchase_price"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            dgvMarket.Columns["company_purchase_price"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
        }

        #region Method

        public void AuthoUnitMaximize(bool isMaximize = false)
        {
            int default_height = 192;
            if (isMaximize)
            {
                int maximizeHeight = pnTop.Height;
                maximizeHeight += dgvContract.ColumnHeadersHeight;
                maximizeHeight += 20 * dgvContract.RowCount;
                if (maximizeHeight < 192)
                    maximizeHeight = 192;

                this.Height = maximizeHeight;
            }
            else
                this.Height = default_height;
        }

        public void unpendingDtUpdate(DataTable unpendingDt)
        {
            this.unpendingDt = unpendingDt;
            OpenExhaustedManager();
        }
        public string[] GetProductInfo()
        {
            string[] productInfo = new string[7];
            productInfo[0] = this_product.product;
            productInfo[1] = this_product.origin;
            productInfo[2] = this_product.sizes;
            productInfo[3] = this_product.unit;
            productInfo[4] = this_product.price_unit;
            productInfo[5] = this_product.unit_count;
            productInfo[6] = this_product.seaover_unit;

            return productInfo;
        }
        string sale_terms;
        bool isShipment, isUnpending, isPending, isReserved;
        public void Refresh(string sttdate, string enddate, string purchaseType, bool isAtoSales, string sale_terms
                            , bool isShipment, bool isUnpending, bool isPending, bool isReserved)
        {
            this.sale_terms = sale_terms;
            this.isShipment = isShipment;
            this.isUnpending = isUnpending;
            this.isPending = isPending;
            this.isReserved = isReserved;

            try
            {
                GetMarketPriceByCompany(sttdate, enddate);
                GetPurchase(purchaseType);
                GetSales(isAtoSales);
                GetAverageSaleQty();
                OpenExhaustedManager();

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            dgvSalesByMonth.ClearSelection();
            dgvMarket.ClearSelection();
            dgvPurchase.ClearSelection();
            dgvSales.ClearSelection();
            dgvContract.ClearSelection();
            dgvStockSales.ClearSelection();
        }
        public void InitStock(string sale_terms, bool isShipment, bool isUnpending, bool isPending, bool isReserved)
        {
            this.sale_terms = sale_terms;
            this.isShipment = isShipment;
            this.isUnpending = isUnpending;
            this.isPending = isPending;
            this.isReserved = isReserved;
            GetAverageSaleQty();
            OpenExhaustedManager();
        }

        #region 업체별시세관리 가져오기
        public void GetMarketPriceByCompany(string sdate, string edate)
        {
            DateTime sttdate, enddate;
            if (!DateTime.TryParse(sdate, out sttdate))
                sttdate = DateTime.Now.AddMonths(-2);
            if (!DateTime.TryParse(edate, out enddate))
                enddate = DateTime.Now;

            //초기화
            dgvMarket.Rows.Clear();

            double main_unit;
            if (!double.TryParse(product[0].unit, out main_unit))
                main_unit = 1;
            double main_seaover_unit;
            if (!double.TryParse(product[0].seaover_unit, out main_seaover_unit))
                main_seaover_unit = 1;


            DataTable marketCloneDt = marketDt.Copy();
            string[] sub_product = product[0].sub_products.Split('\n');
            string whr = "";
            foreach(string sub in sub_product)
            {
                string[] s = sub.Split('^');
                if (s.Length > 6)
                {
                    if (string.IsNullOrEmpty(whr))
                        whr = $"(품명 = '{s[0]}' " +
                            $" AND 원산지 = '{s[1]}'" +
                            $" AND 규격 = '{s[2]}'" +
                            $" AND 단위 = '{s[3]}'" +
                            $" AND 가격단위 = '{s[4]}'" +
                            $" AND 단위수량 = '{s[5]}'" +
                            $" AND SEAOVER단위 = '{s[6]}')";
                    else
                        whr += $" OR (품명 = '{s[0]}' " +
                            $" AND 원산지 = '{s[1]}'" +
                            $" AND 규격 = '{s[2]}'" +
                            $" AND 단위 = '{s[3]}'" +
                            $" AND 가격단위 = '{s[4]}'" +
                            $" AND 단위수량 = '{s[5]}'" +
                            $" AND SEAOVER단위 = '{s[6]}')";
                }   
            }
            DataRow[] selectDr = marketCloneDt.Select(whr);
            if (selectDr.Length > 0)
            {
                marketCloneDt = selectDr.CopyToDataTable();
                //DataTable marketDt = seaoverRepository.GetAllData(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"), product[0].sub_products);
                if (marketCloneDt.Rows.Count > 0)
                {
                    foreach (DataRow dr in marketCloneDt.Rows)
                    {
                        //이미출력된 시장단가는 제외
                        if (!isExistProduct(dr))
                        {
                            int n = dgvMarket.Rows.Add();
                            DataGridViewRow row = dgvMarket.Rows[n];
                            row.Cells["company"].Value = dr["매입처"].ToString();

                            double sales_price;
                            if (!double.TryParse(dr["매출단가"].ToString(), out sales_price))
                                sales_price = 0;
                            double purchase_price;
                            if (!double.TryParse(dr["매입단가"].ToString(), out purchase_price))
                                purchase_price = 0;
                            double unit_count;
                            if (!double.TryParse(dr["단위수량"].ToString(), out unit_count))
                                unit_count = 0;

                            row.Cells["company_sales_price"].Value = sales_price.ToString("#,##0");
                            row.Cells["company_purchase_price"].Value = purchase_price.ToString("#,##0");
                            row.Cells["sales_price_updatetime"].Value = Convert.ToDateTime(dr["매입일자"].ToString()).ToString("yy/MM/dd");

                            //20일 보다 더오래된 단가라면
                            if (Convert.ToDateTime(dr["매입일자"].ToString()) < Convert.ToDateTime(DateTime.Now.AddDays(-20).ToString("yyyy-MM-dd")))
                            {
                                row.DefaultCellStyle.ForeColor = Color.Gray;
                                row.Cells["company_sales_price"].ToolTipText = "20일전 단가입니다.";
                            }
                        }
                    }
                }
            }


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
                if (dgvMarket.Rows[i].Cells["company"].Value.ToString() == dr["매입처"].ToString()
                    && dgvMarket.Rows[i].Cells["company_sales_price"].Value.ToString() == sales_price.ToString("#,##0")
                    && dgvMarket.Rows[i].Cells["sales_price_updatetime"].Value.ToString() == Convert.ToDateTime(dr["매입일자"].ToString()).ToString("yy-MM-dd"))
                {
                    isExist = true;
                    break;
                }
            }
            return isExist;
        }
        #endregion

        #region 매입정보 가져오기

        public void GetPurchase(string purchasePriceType)
        {
            //초기화
            dgvPurchase.Rows.Clear();
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
            if (!double.TryParse(txtPurchaseMargin.Text, out purchase_margin))
                purchase_margin = 5;
            purchase_margin = purchase_margin / 100;
            //단위
            double unit;
            if (!double.TryParse(product[0].unit, out unit))
                unit = 0;
            //씨오버단위
            double seaover_unit;
            if (!double.TryParse(product[0].seaover_unit, out seaover_unit))
                seaover_unit = 0;
            //트레이
            double cost_unit;
            if (!double.TryParse(product[0].unit_count, out cost_unit))
                cost_unit = 0;
            //단위수량
            double unit_count;
            if (!double.TryParse(product[0].unit_count, out unit_count))
                unit_count = 0;
            if (unit_count == 0)
                unit_count = 1;
            //계산방식
            bool isWeight = product[0].weight_calculate;
            if (!isWeight && cost_unit == 0)
                isWeight = true;
            //매입내역 Data
            string select_unit = product[0].unit;

            //SEAOVER 매입내역
            DataTable purchaseDt = purchaseRepository.GetPurchaseProductForDashboard(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                                                                                    , product[0].product
                                                                                    , product[0].origin
                                                                                    , product[0].sizes
                                                                                    , seaover_unit.ToString(), product[0].sub_products, product[0].isMerge, "");

            //오퍼내역 Data
            DataTable offerDt = purchasePriceRepository.GetPurhcasePriceAverage(product[0].product
                                                                                , product[0].origin
                                                                                , product[0].sizes
                                                                                , seaover_unit.ToString(), product[0].sub_products, product[0].isMerge);

            DataTable customCloneDt = customDt.Copy();
            DataTable customResultDt = new DataTable();
            string[] sub_product = product[0].sub_products.Split('\n');
            string whr = "";
            foreach (string sub in sub_product)
            {
                string[] s = sub.Split('^');
                if (s.Length > 6)
                {
                    if (string.IsNullOrEmpty(whr))
                        whr = $"(product = '{s[0]}' " +
                            $" AND origin = '{s[1]}'" +
                            $" AND sizes = '{s[2]}'" +
                            $" AND unit = '{s[6]}')";
                    else
                        whr += $" OR (product = '{s[0]}' " +
                            $" AND origin = '{s[1]}'" +
                            $" AND sizes = '{s[2]}'" +
                            $" AND unit = '{s[6]}')";
                }   
            }
            DataRow[] selectDr = customCloneDt.Select(whr);
            if(selectDr.Length > 0)
                customResultDt = selectDr.CopyToDataTable();
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

            //씨오버 매입내역
            if (purchaseDt.Rows.Count > 0 && purchasePriceType.Contains("매입"))
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

                        //단위수량으로 나눔
                        purcahse /= unit_count;
                        in_purchase /= unit_count;
                        out_purchase /= unit_count;
                        price_krw /= unit_count;

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
            if (customResultDt.Rows.Count > 0 && purchasePriceType.Contains("팬딩"))
            {
                for (int i = 0; i < customResultDt.Rows.Count; i++)
                {
                    //환율
                    DataRow[] dr = null;
                    double exchange_rate = 0;
                    if (exchangeRateDt.Rows.Count > 0)
                    {
                        whr = "base_date = '" + customResultDt.Rows[i]["eta"].ToString() + "'";
                        dr = exchangeRateDt.Select(whr);
                        if (dr.Length > 0)
                            exchange_rate = Convert.ToDouble(dr[0]["exchange_rate"].ToString());
                        else if (customResultDt.Rows[i]["eta"].ToString() == DateTime.Now.ToString("yyyyMM"))
                        {
                            if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                                exchange_rate = 0;
                        }
                    }

                    int division = 0;
                    double purcahse = 0, price_krw = 0, offer_price = 0;
                    double in_purchase = 0, out_purchase = 0;

                    offer_price = Convert.ToDouble(customResultDt.Rows[i]["unit_price"].ToString());

                    if (!isWeight)
                        price_krw = offer_price * exchange_rate * (1 + tax + custom + incidental_expense) * cost_unit;
                    else
                        price_krw = offer_price * exchange_rate * (1 + tax + custom + incidental_expense) * seaover_unit;

                    purcahse = price_krw;
                    division = 1;

                    if (price_krw > 0 && offer_price > 0)
                    {
                        string sdate = customResultDt.Rows[i]["eta"].ToString().Substring(0, 4) + "-" + customResultDt.Rows[i]["eta"].ToString().Substring(4, 2) + "-01";

                        //2023-10-12 중량단가 일때만 단위수량으로 나눔
                        purcahse /= unit_count;
                        in_purchase /= unit_count;
                        out_purchase /= unit_count;
                        price_krw /= unit_count;

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
            if (offerDt.Rows.Count > 0 && purchasePriceType.Contains("오퍼가"))
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

            //정렬
            DataView dv = new DataView(resultDt);
            dv.Sort = "base_date DESC, division DESC, price_usd ASC ";
            resultDt = dv.ToTable();
            //출력
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
                        int n = dgvPurchase.Rows.Add();
                        string sdate = dr[0]["base_date"].ToString().Substring(2, 2) + "/" + dr[0]["base_date"].ToString().Substring(5, 2);
                        dgvPurchase.Rows[n].Cells["purchase_date"].Value = sdate;
                        dgvPurchase.Rows[n].Cells["price_krw"].Value = Convert.ToDouble(dr[0]["price_krw"].ToString()).ToString("#,##0");
                        dgvPurchase.Rows[n].Cells["price_usd"].Value = Convert.ToDouble(dr[0]["price_usd"].ToString()).ToString("#,##0.00");

                        Font originFont = dgvPurchase.Rows[n].DefaultCellStyle.Font;

                        switch (dr[0]["division"].ToString())
                        {
                            case "1":
                                dgvPurchase.Rows[n].DefaultCellStyle.ForeColor = Color.Black;
                                dgvPurchase.Rows[n].DefaultCellStyle.Font = new Font(originFont.Name, originFont.Size, FontStyle.Italic);
                                break;
                            case "2":
                                dgvPurchase.Rows[n].DefaultCellStyle.ForeColor = Color.Black;
                                dgvPurchase.Rows[n].DefaultCellStyle.Font = new Font(originFont.Name, originFont.Size, FontStyle.Regular);
                                break;
                            default:
                                dgvPurchase.Rows[n].DefaultCellStyle.ForeColor = Color.Gray;
                                dgvPurchase.Rows[n].DefaultCellStyle.Font = new Font(originFont.Name, originFont.Size, FontStyle.Regular);
                                break;

                        }
                    }
                }
            }
        }
        #endregion

        #region 매출량 가져오기
        public void GetSales(bool isAtoSales)
        {
            //초기화
            dgvSales.Rows.Clear();

            int stt_year = DateTime.Now.AddYears(-3).Year;
            int end_year = DateTime.Now.Year;
            DateTime sttdate = new DateTime(stt_year, 1, 1);
            DateTime enddate = new DateTime(end_year, 12, 1);

            int terms_type = 1;
            /*case "초~말":
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
                break;*/

            //제외
            string whr = "";
            string[] sub_product = product[0].sub_products.Split('\n');
            foreach (string sub in sub_product)
            {
                string[] s = sub.Split('^');
                if (s.Length > 6)
                {
                    if (string.IsNullOrEmpty(whr))
                        whr = $"(product = '{s[0]}' " +
                            $" AND origin = '{s[1]}'" +
                            $" AND sizes = '{s[2]}'" +
                            $" AND unit = '{s[3]}'" +
                            $" AND price_unit = '{s[4]}'" +
                            $" AND unit_count = '{s[5]}'" +
                            $" AND seaover_unit = '{s[6]}')";
                    else
                        whr += $" OR (product = '{s[0]}' " +
                            $" AND origin = '{s[1]}'" +
                            $" AND sizes = '{s[2]}'" +
                            $" AND unit = '{s[3]}'" +
                            $" AND price_unit = '{s[4]}'" +
                            $" AND unit_count = '{s[5]}'" +
                            $" AND seaover_unit = '{s[6]}')";
                }

            }
            DataRow[] eRow = excluedDt.Select(whr);
            DataTable eDt = new DataTable();
            if (eRow .Length> 0)
                eDt = eRow.CopyToDataTable();
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
            double main_unit = Convert.ToDouble(product[0].seaover_unit);
            //매출량
            DataTable salesDt = salesRepository.GetSalesGroupMonth(sttdate, enddate
                                                                , product[0].product
                                                                , product[0].origin
                                                                , product[0].sizes
                                                                , product[0].seaover_unit, product[0].sub_products, product[0].isMerge, terms_type, isAtoSales);
            if (salesDt.Rows.Count > 0)
            {
                salesDt.Columns["매출수"].ReadOnly = false;
                //Dictionary 추가
                Dictionary<DateTime, double> salesDic = new Dictionary<DateTime, double>();
                foreach (DataRow salesDr in salesDt.Rows)
                {
                    DateTime base_date = Convert.ToDateTime(salesDr["기준일자"].ToString().Substring(0, 4) + "-" + salesDr["기준일자"].ToString().Substring(4, 2) + "-" + "01");
                    double sales;
                    if (!double.TryParse(salesDr["매출수"].ToString(), out sales))
                        sales = 0;

                    //제외매출반영
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
                                sales -= (eQty * sub_unit / main_unit);
                            }
                        }
                    }
                    salesDr["매출수"] = sales;
                    salesDic.Add(base_date, sales);
                }
                salesDt.AcceptChanges();
                //데이터 출력
                for (int i = salesDt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = salesDt.Rows[i];
                    DateTime base_date = Convert.ToDateTime(dr["기준일자"].ToString().Substring(0, 4) + "-" + dr["기준일자"].ToString().Substring(4, 2) + "-" + "01");
                    if (base_date <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                        int n = dgvSales.Rows.Add();

                        dgvSales.Rows[n].Cells["sales_date"].Value = base_date.ToString("yy/MM");
                        double sales;
                        if (!double.TryParse(dr["매출수"].ToString(), out sales))
                            sales = 0;
                        dgvSales.Rows[n].Cells["sales_count"].Value = sales.ToString("#,##0");

                        //전월
                        if (salesDic.ContainsKey(base_date.AddMonths(-1)))
                        {
                            double pre_month_sales = salesDic[base_date.AddMonths(-1)];
                            double pre_month_div_rate = (sales - pre_month_sales) / pre_month_sales * 100;
                            if (double.IsNaN(pre_month_div_rate))
                                pre_month_div_rate = 0;
                            dgvSales.Rows[n].Cells["compared_pre_month"].Value = pre_month_div_rate.ToString("#,##0") + "%";
                            if (pre_month_div_rate > 0)
                                dgvSales.Rows[n].Cells["compared_pre_month"].Style.ForeColor = Color.LightPink;
                            else
                                dgvSales.Rows[n].Cells["compared_pre_month"].Style.ForeColor = Color.LightSkyBlue;
                        }

                        //전년
                        if (salesDic.ContainsKey(base_date.AddYears(-1)))
                        {
                            double pre_year_sales = salesDic[base_date.AddYears(-1)];
                            double pre_year_div_rate = (sales - pre_year_sales) / pre_year_sales * 100;
                            if (double.IsNaN(pre_year_div_rate))
                                pre_year_div_rate = 0;
                            dgvSales.Rows[n].Cells["compared_pre_year"].Value = pre_year_div_rate.ToString("#,##0") + "%";
                            if (pre_year_div_rate > 0)
                                dgvSales.Rows[n].Cells["compared_pre_year"].Style.ForeColor = Color.LightPink;
                            else
                                dgvSales.Rows[n].Cells["compared_pre_year"].Style.ForeColor = Color.LightSkyBlue;
                        }
                    }
                }
            }
        }
        public void GetAverageSaleQty()
        {
            this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            dgvSalesByMonth.Rows.Clear();
            dgvSalesByMonth.Rows.Add();

            DataTable avgDt = salesRepository.GetAverageSalesByMonth2(DateTime.Now
                                                                    , product[0].product
                                                                    , product[0].origin
                                                                    , product[0].sizes
                                                                    , product[0].seaover_unit
                                                                    , product[0].sub_products, product[0].isMerge);

            //매출제외 수량===========================================================================
            DataTable eDt = productExcludedSalesRepository.GetExcludedSalesByMonth(DateTime.Now.AddMonths(-19).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd")
                                                                            , product[0].product
                                                                            , product[0].origin
                                                                            , product[0].sizes
                                                                            , product[0].unit
                                                                            , product[0].price_unit
                                                                            , product[0].unit_count
                                                                            , product[0].seaover_unit
                                                                            , product[0].sub_products, product[0].isMerge);
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
            if (!double.TryParse(product[0].seaover_unit, out main_unit))
                main_unit = 1;

            if (avgDt.Rows.Count > 0)
            {
                int wDays;
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
            this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
        }
        #endregion

        #region 통관내역 가져오기
        public void OpenExhaustedManager()
        {
            this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);

            dgvContract.Rows.Clear();
            dgvStockSales.Rows.Clear();
            dgvStockSales.Rows.Add();
            ShortManagerModel smModel = GetProductStockInfo();

            SetPendingList(smModel);
            this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);

        }
        public void SetSaleTerms(string sale_terms)
        {
            this.sale_terms = sale_terms;
        }
        private ShortManagerModel GetProductStockInfo()
        {
            ShortManagerModel smModel = new ShortManagerModel();
            smModel.product = product[0].product;
            smModel.origin = product[0].origin;
            smModel.sizes = product[0].sizes;
            smModel.unit = product[0].seaover_unit;
            double main_unit = Convert.ToDouble(smModel.unit);

            //매출기간
            int salesMonth = 6;   
            switch (sale_terms)
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
            //영업일 수
            int workDays = 0;
            string eDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string sDate = DateTime.Now.AddMonths(-salesMonth).ToString("yyyy-MM-dd");
            if (salesMonth == 45)
                sDate = DateTime.Now.AddDays(-salesMonth).ToString("yyyy-MM-dd");

            //2023-06-15 오늘로 시작해서 영업일 -1
            common.GetWorkDay(Convert.ToDateTime(sDate), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), out workDays);
            workDays--;

            DateTime sttdate, enddate;
            
            //제외판매수량
            double excluded_qty = 0;
            //판매량
            double stock = 0, salesCnt = 0;


            //데이터 출력
            double total_unpending_qty = 0;
            double total_pending_qty = 0;
            double total_reserved_qty = 0;
            DataTable productDt = seaoverRepository.GetStockAndSalesDetail2(product[0].product
                                                                            , product[0].origin
                                                                            , product[0].sizes
                                                                            , product[0].seaover_unit
                                                                            , salesMonth.ToString()
                                                                            , product[0].sub_products, product[0].isMerge);


            /*DataTable productStockSalesCloneDt = productStockSalesDt.Copy();
            DataTable productDt = new DataTable();*/
            string[] sub_product = product[0].sub_products.Split('\n');
            string whr = "";
            foreach (string sub in sub_product)
            {
                string[] s = sub.Split('^');
                if (s.Length > 6)
                {
                    if (string.IsNullOrEmpty(whr))
                        whr = $"(품명 = '{s[0]}' " +
                            $" AND 원산지 = '{s[1]}'" +
                            $" AND 규격 = '{s[2]}'" +
                            $" AND 단위 = '{s[6]}')";
                    else
                        whr += $" OR (품명 = '{s[0]}' " +
                            $" AND 원산지 = '{s[1]}'" +
                            $" AND 규격 = '{s[2]}'" +
                            $" AND 단위 = '{s[6]}')";
                }
            }
            /*DataRow[] selectDr = productStockSalesCloneDt.Select(whr);
            if (selectDr.Length > 0)*/
            {
                /*productDt = selectDr.CopyToDataTable();*/
                if (productDt.Rows.Count > 0)
                {
                    for (int i = 0; i < productDt.Rows.Count; i++)
                    {
                        double unit = Convert.ToDouble(productDt.Rows[i]["단위"].ToString());


                        double unpending_qty = Convert.ToDouble(productDt.Rows[i]["미통관"].ToString());
                        total_unpending_qty += (unpending_qty / unit * main_unit);
                        double pending_qty = Convert.ToDouble(productDt.Rows[i]["통관"].ToString());
                        total_pending_qty += (pending_qty / unit * main_unit);
                        double reserved_qty = Convert.ToDouble(productDt.Rows[i]["예약수"].ToString());
                        total_reserved_qty += (reserved_qty / unit * main_unit);

                        //총 판매, 재고
                        if (isUnpending)
                            stock += unpending_qty;
                        if (isPending)
                            stock += pending_qty;
                        if (isReserved)
                            stock -= reserved_qty;

                        salesCnt += (Convert.ToDouble(productDt.Rows[i]["매출수"].ToString()) / unit * main_unit);
                        if (salesCnt < 0)
                            salesCnt = 0;
                    }
                }
            }
            smModel.real_stock = stock;
            //영업일 수
            if (salesMonth == 45)
            {
                enddate = DateTime.Now.AddDays(-1);
                sttdate = DateTime.Now.AddDays(-salesMonth);
            }
            else
            {
                enddate = DateTime.Now.AddDays(-1);
                sttdate = DateTime.Now.AddMonths(-salesMonth);
            }


            //제외
            whr = "";
            foreach (string sub in sub_product)
            {
                string[] s = sub.Split('^');
                if (s.Length > 6)
                {
                    if (string.IsNullOrEmpty(whr))
                        whr = $"(product = '{s[0]}' " +
                            $" AND origin = '{s[1]}'" +
                            $" AND sizes = '{s[2]}'" +
                            $" AND unit = '{s[3]}'" +
                            $" AND price_unit = '{s[4]}'" +
                            $" AND unit_count = '{s[5]}'" +
                            $" AND seaover_unit = '{s[6]}'" +
                            $" AND 'sale_date' >= '{sttdate.ToString("yyyy-MM-dd")}'" +
                            $" AND 'sale_date' <= '{enddate.ToString("yyyy-MM-dd")}')";
                    else
                        whr += $" OR (product = '{s[0]}' " +
                            $" AND origin = '{s[1]}'" +
                            $" AND sizes = '{s[2]}'" +
                            $" AND unit = '{s[3]}'" +
                            $" AND price_unit = '{s[4]}'" +
                            $" AND unit_count = '{s[5]}'" +
                            $" AND seaover_unit = '{s[6]}'" +
                            $" AND 'sale_date' >= '{sttdate.ToString("yyyy-MM-dd")}'" +
                            $" AND 'sale_date' <= '{enddate.ToString("yyyy-MM-dd")}')";
                }
                
            }
            DataRow[] eRow = excluedDt.Select(whr);
            DataTable eDt = new DataTable();            
            if (eRow.Length > 0)
                eDt = eRow.CopyToDataTable();
            if (eDt != null && eDt.Rows.Count > 0)
            {
                for (int i = 0; i < eDt.Rows.Count; i++)
                {
                    double unit = Convert.ToDouble(eDt.Rows[i]["seaover_unit"].ToString());
                    excluded_qty += Convert.ToDouble(eDt.Rows[i]["sale_qty"].ToString()) / unit * main_unit;
                }
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

            //쇼트일자
            DateTime exhausted_date;
            double exhausted_cnt;
            common.GetExhausedDateDayd(DateTime.Now, smModel.real_stock, smModel.avg_sales_day, 0, null, out exhausted_date, out exhausted_cnt);

            smModel.exhaust_date = exhausted_date.ToString("yyyy-MM-dd");
            enddate = DateTime.Now;
            smModel.enddate = enddate.ToString("yyyy-MM-dd");

            return smModel;
        }


        private void SetPendingList(ShortManagerModel smModel)
        {
            SetData(smModel);
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
                if (isShipment)
                    total_qty += shipment_qty + shipping_qty;
                if (isUnpending)
                    total_qty += seaover_unpending_qty;
                if (isPending)
                    total_qty += seaover_pending_qty;
                if (isReserved)
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
        private void ReplaceSalesCostToPendingCost(out double seaover_cost_price)
        {
            seaover_cost_price = 0;
            string tooltip = "";
            //환율
            double exchange_rate;
            if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                exchange_rate = 0;

            //최종S원가
            double final_average_cost = 0;
            double final_qty = 0;

            //품목정보
            bool isMerge = false;
            string products = "";
            string[] sub_product;
            if (product.Count <= 1)
            {
                sub_product = new string[1];
                sub_product[0] = product[0].product
                            + "^" + product[0].origin
                            + "^" + product[0].sizes
                            + "^" + product[0].unit
                            + "^" + product[0].price_unit
                            + "^" + product[0].unit_count
                            + "^" + product[0].seaover_unit;
            }
            else
            {
                isMerge = true;
                sub_product = new string[product.Count];

                for (int i = 0; i < product.Count; i++)
                {
                    sub_product[i] = product[i].product
                            + "^" + product[i].origin
                            + "^" + product[i].sizes
                            + "^" + product[i].unit
                            + "^" + product[i].price_unit
                            + "^" + product[i].unit_count
                            + "^" + product[i].seaover_unit;
                }
            }
            //출력
            if (sub_product.Length > 0)
            {
                //단위수량
                double unit_count;
                if (!double.TryParse(product[0].unit_count, out unit_count))
                    unit_count = 0;
                if (unit_count == 0)
                    unit_count = 1;
                //메인 S단위
                double seaover_unit;
                if (!double.TryParse(product[0].seaover_unit, out seaover_unit))
                    seaover_unit = 0;
                if (seaover_unit == 0)
                    seaover_unit = 1;

                DataTable productDt = priceComparisonRepository.GetNotSalesCostProduct("", "", "", "", sub_product);
                DataTable pendingDt = customsRepository.GetProductForNotSalesCost("", "", "", "", productDt, sub_product);

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
                                    unit_price = unit_price * (box_weight / seaover_unit) / unit_count;
                                    productDt.Rows[i]["오퍼가"] = unit_price;   //달러인 오퍼가 등록
                                    //트레이단가 -> 중량 단가
                                    bool isWeight = Convert.ToBoolean(dr[j]["weight_calculate"].ToString());
                                    if (!isWeight && cost_unit > 0)
                                        unit_price = (unit_price * cost_unit) / box_weight;


                                    double custom = Convert.ToDouble(dr[j]["custom"].ToString()) / 100;
                                    double tax = Convert.ToDouble(dr[j]["tax"].ToString()) / 100;
                                    double incidental_expense = Convert.ToDouble(dr[j]["incidental_expense"].ToString()) / 100;

                                    double exchangeRate;
                                    if (!double.TryParse(dr[j]["exchange_rate"].ToString(), out exchangeRate))
                                        exchangeRate = exchange_rate;
                                    //원가계산
                                    double sales_cost;
                                    //일반
                                    if (trq_amount == 0)
                                        sales_cost = unit_price * exchangeRate * (1 + custom + tax + incidental_expense) * seaover_unit;
                                    //trq
                                    else
                                        sales_cost = (unit_price * exchangeRate * (1 + tax + incidental_expense) * seaover_unit) + (box_weight * trq_amount);

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
                                    sales_cost = sales_cost / stock_unit * seaover_unit / unit_count;
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
        double seaover_average_cost_price;
        private void SetData(ShortManagerModel smModel)
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
            ReplaceSalesCostToPendingCost(out cost_price);
            dgvContract.Rows[n].Cells["etd_cost_price"].Value = cost_price.ToString("#,##0");
            //txtSeaoverCostPrice.Text = cost_price.ToString("#,##0");
            dgvStockSales.Rows[0].Cells["seaover_cost_price"].Value = cost_price.ToString("#,##0");
            seaover_average_cost_price = cost_price;
        }
        //미통관내역 가져오기
        private void GetUnpendingData(ShortManagerModel model)
        {
            //dgvContract.Rows.Clear();
            //DataTable unpendingTable = customsRepository.GetUnpendingProduct3(model.product, model.origin, model.sizes, model.unit, products, isMerge, true, cbSimulation.Checked);
            //DataTable unpendingTable = customsRepository.GetUnpendingProduct3(model.product, model.origin, model.sizes, model.unit, product[0].sub_products, product[0].isMerge, true, false);

            DataTable unpendingCloneDt = unpendingDt.Copy();
            string whr = "";
            string[] sub_product = product[0].sub_products.Split('\n');
            foreach (string sub in sub_product)
            {
                string[] s = sub.Split('^');
                if (s.Length > 6)
                {
                    if (string.IsNullOrEmpty(whr))
                        whr = $"(product = '{s[0]}' " +
                            $" AND origin = '{s[1]}'" +
                            $" AND sizes = '{s[2]}'" +
                            $" AND box_weight = '{s[6]}')";
                    else
                        whr += $" OR (product = '{s[0]}' " +
                            $" AND origin = '{s[1]}'" +
                            $" AND sizes = '{s[2]}'" +
                            $" AND box_weight = '{s[6]}')";
                }
            }
            DataRow[] unpendingCloneDr = unpendingCloneDt.Select(whr);
            DataTable unpendingTable = new DataTable();
            if (unpendingCloneDr.Length > 0)
                unpendingTable = unpendingCloneDr.CopyToDataTable();

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
            if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
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
                /*//통합 && 단위가 다른 내역 표시
                if (cbAllUnit.Checked && model.unit != tb.Rows[i]["product_unit"].ToString())
                {
                    dgvContract.Rows[n].DefaultCellStyle.BackColor = Color.Linen;
                    for (int j = 0; j < dgvContract.Columns.Count; j++)
                        dgvContract.Rows[i].Cells[j].ToolTipText = "단위 : " + tb.Rows[i]["product_unit"].ToString();
                }*/

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
                        /*string txtDays = Days.ToString("#,##0").PadRight(5, ' ');
                        string txtCount = (Days * model.avg_sales_day).ToString("#,##0").PadLeft(5, ' ');*/
                        string txtDays = Days.ToString("#,##0");
                        string txtCount = (Days * model.avg_sales_day).ToString("#,##0");
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
                    /*int standard;
                    if (!int.TryParse(txtStandardOfShort.Text, out standard))
                        standard = 0;*/
                    common.GetExhausedDateDayd(sttdate, stock, model.avg_sales_day, 0, null, out exhaust_date, out exhausted_cnt);

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
                    if (dgvContract.Rows[n - 1].Cells["after_qty_exhausted_date"].Value != null && DateTime.TryParse(dgvContract.Rows[n - 1].Cells["after_qty_exhausted_date"].Value.ToString(), out pre_exhaust_date))
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
            dgvContract.Rows[m].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            dgvContract.Rows[m].Cells["qty"].Value = total_qty.ToString("#,##0");
            dgvContract.Rows[m].Cells["etd_average_cost_price"].Value = (total_cost_price / total_cost_qty).ToString("#,##0");
            DataGridViewButtonCell newRowCell = new DataGridViewButtonCell();
            newRowCell.Value = "추가";
            newRowCell.Style.BackColor = Color.Blue;
            dgvContract.Rows[m].Cells["btnRegister"] = newRowCell;

            dgvStockSales.Rows[0].Cells["total_cost_price"].Value = (total_cost_price / total_cost_qty).ToString("#,##0");


            double pending_cost_price = total_shipping_cost_price / total_shipping_qty;
            if (double.IsNaN(pending_cost_price))
                pending_cost_price = 0;
            //txtPendingCostPrice.Text = pending_cost_price.ToString("#,##0");
            dgvStockSales.Rows[0].Cells["pending_cost_price"].Value = pending_cost_price.ToString("#,##0");

            //SetDashboardSummary();

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
                            dgvContract.Rows[n].Cells["exhausted_day_count"].Value = Days.ToString("#,##0");
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
                        common.GetExhausedDateDayd(sttdate, stock, avg_sales_day, 0, null, out exhaust_date, out exhausted_cnt);
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
                dgvContract.Rows[m].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                dgvContract.Rows[m].Cells["qty"].Value = total_qty.ToString("#,##0");
                DataGridViewButtonCell newRowCell = new DataGridViewButtonCell();
                newRowCell.Value = "추가";
                newRowCell.Style.BackColor = Color.Blue;
                dgvContract.Rows[m].Cells["btnRegister"] = newRowCell;
                double total_average_cost_price = (total_cost_price / total_cost_qty);
                dgvContract.Rows[m].Cells["etd_average_cost_price"].Value = total_average_cost_price.ToString("#,##0");
                double pending_cost_price = total_shipping_cost_price / total_shipping_qty;
                if (double.IsNaN(pending_cost_price))
                    pending_cost_price = 0;
                //txtPendingCostPrice.Text = pending_cost_price.ToString("#,##0");
                dgvStockSales.Rows[0].Cells["pending_cost_price"].Value = pending_cost_price.ToString("#,##0");
                dgvStockSales.Rows[0].Cells["total_cost_price"].Value = total_average_cost_price.ToString("#,##0");

                //SetDashboardSummary();
                if (currentRow >= 0 && currentRow < dgvContract.RowCount && currentColumn >= 0 && currentColumn < dgvContract.ColumnCount)
                    dgvContract.CurrentCell = dgvContract.Rows[currentRow].Cells[currentColumn];

            }

            return true;
        }

        int currentRow = -1, currentColumn = -1;
        private void dgvContract_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 & e.ColumnIndex >= 0)
            {
                currentRow = e.RowIndex;
                currentColumn = e.ColumnIndex;

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
                this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            }
        }

        private void dgvStockSales_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
            dgvStockSales.EndEdit();
            if (dgvStockSales.Rows.Count > 0)
            {
                this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
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
                double inQty = 0;
                    
                //실재고
                double total_qty = 0;
                if (isShipment)
                    total_qty += shipment_qty + shipping_qty;
                if (isUnpending)
                    total_qty += seaover_unpending_qty;
                if (isPending)
                    total_qty += seaover_pending_qty;
                if (isReserved)
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
                this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            }
        }
        
        private void txtTrq_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);

                ShortManagerModel smModel = GetProductStockInfo();
                if (SortSetting())
                    CalculateStock(smModel);

                this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            }
        }

        private void dgvContract_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
                this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);

                if (dgvContract.Columns[e.ColumnIndex].Name.Equals("btnRegister"))
                {
                    //행추가
                    if (e.RowIndex == dgvContract.Rows.Count - 1)
                    {
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

                        ShortManagerModel smModel = GetProductStockInfo();
                        if (SortSetting())
                            CalculateStock(smModel);


                        dgvContract.Rows[dgvContract.Rows.Count - 2].Cells["etd"].Style.ForeColor = Color.Gray;
                    }
                    //데이터 등록
                    else
                    {
                        if (dgvContract.Rows[e.RowIndex].Cells["btnRegister"].Value != null
                            && !string.IsNullOrEmpty(dgvContract.Rows[e.RowIndex].Cells["btnRegister"].Value.ToString()))
                        {
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
                                return;
                            }

                            model.id = id;
                            model.sub_id = 1;
                            model.status = "선적전";
                            if (dgvContract.Rows[e.RowIndex].Cells["etd"].Value == null)
                                dgvContract.Rows[e.RowIndex].Cells["etd"].Value = string.Empty;
                            model.etd = dgvContract.Rows[e.RowIndex].Cells["etd"].Value.ToString();
                            model.warehousing_date = warehousing_date.ToString("yyyy-MM-dd");
                            model.product = this.product[0].product;
                            model.origin = this.product[0].origin;
                            model.sizes = this.product[0].sizes;
                            model.unit = this.product[0].unit;
                            model.price_unit = this.product[0].price_unit;
                            model.unit_count = this.product[0].unit_count;
                            model.seaover_unit = this.product[0].seaover_unit;

                            double qty;
                            if (dgvContract.Rows[e.RowIndex].Cells["qty"].Value == null || !double.TryParse(dgvContract.Rows[e.RowIndex].Cells["qty"].Value.ToString(), out qty))
                            {
                                MessageBox.Show(this, "수량을 입력해주세요.");
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
                        }
                    }
                }
                else if (dgvContract.Columns[e.ColumnIndex].Name.Equals("isDelete"))
                {
                    if (MessageBox.Show(this, "선택한 국내수입 내역을 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
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
                        return;
                    }

                    //dgv 반영
                    dgvContract.Rows.Remove(dgvContract.Rows[e.RowIndex]);
                    dgvContract.Rows.Remove(dgvContract.Rows[dgvContract.Rows.Count - 1]);
                    ShortManagerModel smModel = GetProductStockInfo();
                    if (SortSetting())
                        CalculateStock(smModel);
                }
                else if (dgvContract.Columns[e.ColumnIndex].Name.Equals("btnUpdate"))
                {
                    DataGridViewRow select_row = dgvContract.Rows[e.RowIndex];
                    Domestic.SimpleDomesticManager dm = new Domestic.SimpleDomesticManager(um
                            , this.product[0].product, this.product[0].origin, this.product[0].sizes
                            , this.product[0].unit, this.product[0].price_unit, this.product[0].unit_count, this.product[0].seaover_unit
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
                this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            }
        }

        private void btnRegisters_Click(object sender, EventArgs e)
        {
            if (dgvContract.Rows.Count > 2)
            {
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
                            return;
                        }
                        //입고수량
                        double qty;
                        if (dgvContract.Rows[i].Cells["qty"].Value == null || !double.TryParse(dgvContract.Rows[i].Cells["qty"].Value.ToString(), out qty))
                        {
                            MessageBox.Show(this, "수량을 입력해주세요.");
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
                        model.product = this.product[0].product;
                        model.origin = this.product[0].origin;
                        model.sizes = this.product[0].sizes;
                        model.unit = this.product[0].unit;
                        model.price_unit = this.product[0].price_unit;
                        model.unit_count = this.product[0].unit_count;
                        model.seaover_unit = this.product[0].seaover_unit;
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
            }
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
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message + "\n" + errCnt);
                }
            }
        }

        private void btnDashboardForSales_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "품명별 매출관리 대시보드", "is_visible"))
                {
                    MessageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            List<string[]> productInfoList = new List<string[]>();
            string[] productInfo = new string[30];

            productInfo[0] = product[0].product;
            productInfo[1] = product[0].origin;
            productInfo[2] = product[0].sizes;
            productInfo[3] = product[0].unit;
            productInfo[4] = product[0].unit_count;


            productInfo[6] = txtCustom.Text;
            productInfo[7] = txtTax.Text;
            productInfo[8] = txtIncidentalExpense.Text;
            productInfo[9] = txtPurchaseMargin.Text;
            productInfo[10] = txtMakeTerm.Text;

            productInfo[19] = product[0].weight_calculate.ToString();

            productInfo[20] = product[0].price_unit;
            productInfo[21] = product[0].unit_count;
            productInfo[22] = product[0].seaover_unit;
            productInfo[27] = txtShippingTerm.Text;
            
            productInfoList.Add(productInfo);
            int saleTerm;
            switch (sale_terms)
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

        private double CalculateEtdCostPrice(string etd, double offer_price, bool weight_calculate, bool trq, double exchangeRate = 0, double cost_unit = 0)
        {
            double cost_price = 0;            
            {
                double seaover_unit;
                if (!double.TryParse(product[0].seaover_unit, out seaover_unit))
                    seaover_unit = 0;
                if (cost_unit == 0)
                {
                    if (!double.TryParse(product[0].unit_count, out cost_unit))
                        cost_unit = 0;
                }
                double unit_count;
                if (!double.TryParse(product[0].unit_count, out unit_count))
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
                    //과세는 관세를 포함금액, 부대비용은 과세까지 포함
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
                    //과세는 관세를 포함금액, 부대비용은 과세까지 포함
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
        #endregion
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

    }
}
