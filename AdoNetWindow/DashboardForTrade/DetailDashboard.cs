using AdoNetWindow.Common.PrintManager;
using AdoNetWindow.Dashboard;
using AdoNetWindow.DashboardForTrade;
using AdoNetWindow.Model;
using AdoNetWindow.Product;
using AdoNetWindow.PurchaseManager;
using Repositories;
using Repositories.Config;
using Repositories.ContractRecommendation;
using Repositories.Group;
using Repositories.SaleProduct;
using Repositories.SEAOVER;
using Repositories.SEAOVER.Purchase;
using Repositories.SEAOVER.Sales;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AdoNetWindow.SEAOVER.PriceComparison
{
    public partial class DetailDashboard : Form
    {
        Libs.Tools.Common common = new Libs.Tools.Common();
        IProductExcludedSalesRepository productExcludedSalesRepository = new ProductExcludedSalesRepository();
        IProductOtherCostRepository productOtherCostRepository = new ProductOtherCostRepository();
        IContractRecommendationRepository contractRecommendationRepository = new ContractRecommendationRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        IGroupRepository groupRepository = new GroupRepository();
        IPurchaseRepository purchaseRepository = new PurchaseRepository();
        IPriceComparisonRepository priceComparisonRepository = new PriceComparisonRepository();
        ISalesRepository salesRepository = new SalesRepository();
        IPurchasePriceRepository purchasePriceRepository = new PurchasePriceRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        IConfigRepository configRepository = new ConfigRepository();
        ICommonRepository commonRepository = new CommonRepository();
        IProductGroupRepository productGroupRepository = new ProductGroupRepository();
        UsersModel um;
        int workDays;
        DataTable purchaseDt = new DataTable();
        List<DataGridViewRow> clipboardList = new List<DataGridViewRow>();

        delegate void GetProductDeligate(DataGridViewRow row);

        //캡쳐방지
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern uint SetWindowDisplayAffinity(IntPtr windowHandle, uint affinity);
        private const uint WDA_NONE = 0;
        private const uint WDA_MONITOR = 1;

        public DetailDashboard(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
            txtSttdate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtOfferSttdate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            txtOfferEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
        public DetailDashboard(UsersModel uModel, List<string[]> product)
        {
            InitializeComponent();
            um = uModel;

            txtSttdate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtOfferSttdate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            txtOfferEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            InputClipboardData(product);
        }
        private void DetailDashboard_Load(object sender, EventArgs e)
        {
            double exchange_rate = common.GetExchangeRateKEBBank("USD");
            txtExchangeRate.Text = exchange_rate.ToString("#,##0.00");
            txtExchangeRate2.Text = exchange_rate.ToString("#,##0.00");
            CallProductProcedure();
            CallStockProcedure();
            //영업일수
            common.GetWorkDay(DateTime.Now.AddDays(-45), DateTime.Now, out workDays);
            workDays--;
            //환율가져오기
            double customRate = common.GetExchangeRateKEBBank("USD");
            //차트 마우스휠 확대,축소
            //chtPurchase.MouseWheel += MouseWheelOnChart;   
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "원가 및 재고 대시보드", "is_print"))
                {
                    btnPrinting.Visible = false;
                }
            }
            //드레그 dgv
            dgvDragData.Rows.Add();
            dgvDragData.Rows.Add();
            dgvDragData.Rows[0].Cells["div"].Value = "매출수";
            dgvDragData.Rows[0].Cells["div2"].Value = "매출금액";
            dgvDragData.Rows[1].Cells["div"].Value = "마진률";
            dgvDragData.Rows[1].Cells["div2"].Value = "마진금액";
            //정렬기능 막기
            foreach (DataGridViewColumn dgvc in dgvContract.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //캡쳐방지
            if (um.auth_level < 90)
                SetWindowDisplayAffinity(this.Handle, WDA_MONITOR);
        }

        #region 기타 Method
        private void GetProductCode(DataGridViewRow row, DataTable codeDt)
        {
            DataRow[] dr = codeDt.Select("품명 = '" + row.Cells["product"].Value.ToString() + "'"
                                    + " AND 원산지 = '" + row.Cells["origin"].Value.ToString() + "'"
                                    + " AND 규격 = '" + row.Cells["sizes"].Value.ToString() + "'");
            if (dr.Length >= 1)
            {
                row.Cells["product_code"].Value = dr[0]["품목코드"].ToString();
                row.Cells["sizes_code"].Value = dr[0]["규격코드"].ToString();
                row.HeaderCell.Style.BackColor = Color.LightGray;
            }
        }
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
                    dgvProduct.Rows[n].Cells["seaover_purchsae_price"].Value = product[i][5];

                    dgvProduct.Rows[n].Cells["custom"].Value = product[i][6];
                    dgvProduct.Rows[n].Cells["tax"].Value = product[i][7];
                    dgvProduct.Rows[n].Cells["incidental_expense"].Value = product[i][8];
                    dgvProduct.Rows[n].Cells["purchase_margin"].Value = product[i][9];
                    dgvProduct.Rows[n].Cells["production_days"].Value = product[i][10];

                    dgvProduct.Rows[n].Cells["exchange_rate"].Value = exchange_rate.ToString("#,##0");
                    dgvProduct.Rows[n].Cells["offer_price"].Value = product[i][11];
                    dgvProduct.Rows[n].Cells["shipper"].Value = product[i][12];

                    dgvProduct.Rows[n].Cells["updatetime"].Value = product[i][18];

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


                    dgvProduct.Rows[n].Cells["cost_price"].Value = cost_price.ToString("#,##0");
                    dgvProduct.Rows[n].Cells["manager"].Value = product[i][14];

                    dgvProduct.Rows[n].Cells["product_code"].Value = product[i][15];
                    dgvProduct.Rows[n].Cells["origin_code"].Value = product[i][16];
                    dgvProduct.Rows[n].Cells["sizes_code"].Value = product[i][17];

                    //머지품목(sub)
                    DataTable groupProductDt = productGroupRepository.GetSubProduct(product[i][0], product[i][1], product[i][2], "", "", "", product[i][3]);
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

                }
                SetPrice();
                dgvProduct.Rows[0].Selected = true;
            }
        }
        #endregion

        #region 최신단가 불러오기 이벤트
        private void GetCurrentPrice(DataGridViewRow row)
        {
            DataTable currentDt = purchasePriceRepository.GetCurrentPrice();
            if (currentDt.Rows.Count > 0)
            {
                DataRow[] dr = currentDt.Select($"product = '{row.Cells["product"].Value.ToString()}'"
                    + $" AND origin = '{row.Cells["origin"].Value.ToString()}'"
                    + $" AND sizes = '{row.Cells["sizes"].Value.ToString()}'"
                    + $" AND unit = '{row.Cells["unit"].Value.ToString()}'");
                if (dr.Length > 0)
                {
                    row.Cells["updatetime"].Value = Convert.ToDateTime(dr[0]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                    row.Cells["offer_price"].Value = Convert.ToDouble(dr[0]["purchase_price"].ToString()).ToString("#,##0.00");
                    row.Cells["company_id"].Value = dr[0]["cid"].ToString();
                    row.Cells["shipper"].Value = dr[0]["company"].ToString();
                    calculateCostPrice(row);
                }
            }
        }
        private void calculateCostPrice(DataGridViewRow row)
        {
            dgvProduct.EndEdit();
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
            if (row.Cells["exchange_rate"].Value == null || !double.TryParse(row.Cells["exchange_rate"].Value.ToString(), out exchange_rate))
                exchange_rate = 0;

            double offer_price;
            if (row.Cells["offer_price"].Value == null || !double.TryParse(row.Cells["offer_price"].Value.ToString(), out offer_price))
                offer_price = 0;
            //원가계산
            double cost_price;
            if (weight_calculate)
                cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate * unit;
            else
                cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate * cost_unit;
            row.Cells["cost_price"].Value = cost_price.ToString("#,##0");
            //마진율
            double seaover_purchsae_price;
            if (row.Cells["seaover_purchsae_price"].Value == null || !double.TryParse(row.Cells["seaover_purchsae_price"].Value.ToString(), out seaover_purchsae_price))
                seaover_purchsae_price = 0;
            double margin = ((seaover_purchsae_price - cost_price) / seaover_purchsae_price * 100);
            row.Cells["margin_rate"].Value = margin.ToString("#,##0.00") + "%";
        }
        private double calculateCostPrice(double offer_price)
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
                cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate * cost_unit;

            return cost_price;
            
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
            double unit;
            if (row.Cells["unit"].Value == null || !double.TryParse(row.Cells["unit"].Value.ToString(), out unit))
                unit = 0;
            bool weight_calculate;
            if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                weight_calculate = true;
            if (!weight_calculate && cost_unit < 0)
                weight_calculate = true;

            //원가계산
            double cost_price;
            if (weight_calculate)
                cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate * unit / cost_unit;
            else
                cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate * cost_unit;

            return cost_price;

        }
        #endregion

        #region Chart event
        protected void MouseWheelOnChart(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var yAxis = chart.ChartAreas[0].AxisY;
            double y, yMin, yMax, yMin2, yMax2;

            yMin = yAxis.ScaleView.ViewMinimum;
            yMax = yAxis.ScaleView.ViewMaximum;
            y = yAxis.PixelPositionToValue(e.Location.Y);

            if (e.Delta < 0) // Scrolled down.
            {
                yMin2 = y - (y - yMin) / 0.9;
                yMax2 = y + (yMax - y) / 0.9;
            }
            else //if (e.Delta > 0) // Scrolled up.
            {
                yMin2 = y - (y - yMin) * 0.9;
                yMax2 = y + (yMax - y) * 0.9;
            }

            if (yMax2 > 5) yMax2 = 5;
            if (yMin2 < 0) yMin2 = 0;

            chart.ChartAreas[0].AxisY.Maximum = yMax2;
            chart.ChartAreas[0].AxisY.Minimum = yMin2;
        }
        #endregion

        #region 그룹 가져오기
        public void SetGroupData(int id)
        {
            double exchange_rate;
            if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                exchange_rate = 0;

            dgvProduct.Rows.Clear();
            DataTable groupDt = groupRepository.GetGroup(id);
            List<string[]> productInfoList = new List<string[]>();
            for (int i = 0; i < groupDt.Rows.Count; i++)
            {
                int n = dgvProduct.Rows.Add();
                DataGridViewRow row = dgvProduct.Rows[n];
                row.Cells["product"].Value = groupDt.Rows[i]["product"].ToString();
                row.Cells["product_code"].Value = groupDt.Rows[i]["product_code"].ToString();
                row.Cells["origin"].Value = groupDt.Rows[i]["origin"].ToString();
                row.Cells["sizes"].Value = groupDt.Rows[i]["sizes"].ToString();
                row.Cells["sizes_code"].Value = groupDt.Rows[i]["sizes_code"].ToString();
                row.Cells["unit"].Value = groupDt.Rows[i]["unit"].ToString();
                row.Cells["cost_unit"].Value = groupDt.Rows[i]["cost_unit"].ToString();

                row.Cells["tax"].Value = groupDt.Rows[i]["tax"].ToString();
                row.Cells["custom"].Value = groupDt.Rows[i]["custom"].ToString();
                row.Cells["incidental_expense"].Value = groupDt.Rows[i]["incidental_expense"].ToString();
                row.Cells["exchange_rate"].Value = exchange_rate.ToString("#,##0.00");

                bool isWeight = Convert.ToBoolean(groupDt.Rows[i]["weight_calculate"].ToString());
                double unit = Convert.ToDouble(groupDt.Rows[i]["unit"].ToString());
                double cost_unit = Convert.ToDouble(groupDt.Rows[i]["cost_unit"].ToString());
                if (!isWeight && cost_unit == 0)
                    isWeight = true;
                row.Cells["weight_calculate"].Value = isWeight;

                double tax = Convert.ToDouble(groupDt.Rows[i]["tax"].ToString()) / 100;
                double custom = Convert.ToDouble(groupDt.Rows[i]["custom"].ToString()) / 100;
                double incidental_expense = Convert.ToDouble(groupDt.Rows[i]["incidental_expense"].ToString()) / 100;

                row.Cells["purchase_margin"].Value = groupDt.Rows[i]["purchase_margin"].ToString();
                row.Cells["production_days"].Value = groupDt.Rows[i]["production_days"].ToString();
                double offer_price = Convert.ToDouble(groupDt.Rows[i]["offer_price"].ToString());
                row.Cells["offer_price"].Value = offer_price.ToString("#,##0.00");
                row.Cells["updatetime"].Value = Convert.ToDateTime(groupDt.Rows[i]["offer_updatetime"].ToString()).ToString("yyyy-MM-dd");
                row.Cells["shipper"].Value = groupDt.Rows[i]["offer_company"].ToString();
                row.Cells["manager"].Value = groupDt.Rows[i]["manager"].ToString();

                if (!isWeight)
                    unit = cost_unit;

                double cost_price = offer_price * exchange_rate * (1 + tax + custom + incidental_expense) * unit;
                row.Cells["cost_price"].Value = cost_price.ToString("#,##0");
            }
            //단가
            SetPrice();
            //첫번째 레코드 선택
            if (dgvProduct.Rows.Count > 0)
            { 
                dgvProduct.ClearSelection();
                dgvProduct.Rows[0].Cells["chk"].Value = true;
            }
        }

        public void SetPrice()
        {
            //Seaover 품목 단가정보
            string sttdate = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
            string enddate = DateTime.Now.ToString("yyyy-MM-dd");
            DataTable sDt = seaoverRepository.GetPriceForCostAccount(sttdate, enddate, "", "", "", "");
            if (dgvProduct.Rows.Count > 0)
            {
                string whr;
                DataRow[] dtRow;
                //데이터출력=========================================================================
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];
                    if ((row.Cells["product_code"].Value == null || string.IsNullOrEmpty(row.Cells["product_code"].Value.ToString()))
                        && (row.Cells["sizes_code"].Value == null || string.IsNullOrEmpty(row.Cells["sizes_code"].Value.ToString())))
                    {
                        row.HeaderCell.Style.BackColor = Color.Red;
                        row.HeaderCell.ToolTipText = "코드가 저장되지 않은 품목입니다.";
                    }
                    else
                        row.HeaderCell.Style.BackColor = Color.LightGray;


                    //SEAOVER 품목
                    double sales_price = 0;
                    double purchase_price1 = 0;
                    double box_weight;
                    if (row.Cells["unit"].Value == null || !double.TryParse(row.Cells["unit"].Value.ToString(), out box_weight))
                        box_weight = 0;
                    double cost_unit;
                    if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                        cost_unit = 0;
                    double sales_unit = box_weight;
                    //매출단가========================================================================================
                    whr = "품명 = '" + row.Cells["product"].Value.ToString() + "'"
                            + " AND 원산지 = '" + row.Cells["origin"].Value.ToString() + "'"
                            + " AND 규격 = '" + row.Cells["sizes"].Value.ToString() + "'"
                            + " AND (SEAOVER단위 = '" + row.Cells["unit"].Value.ToString() + "' OR " + "계산단위 = '" + row.Cells["unit"].Value.ToString() + "')"
                            + " AND 매출단가 > 0";
                    dtRow = sDt.Select(whr);
                    if (dtRow.Length > 0)
                    {
                        sales_unit = Convert.ToDouble(dtRow[0]["단위"]);
                        sales_price = Convert.ToDouble(dtRow[0]["매출단가"]);
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
                            sales_unit = Convert.ToDouble(dtRow[0]["단위"]);
                            sales_price = Convert.ToDouble(dtRow[0]["매출단가"]);
                        }
                    }
                    //원금
                    //row.Cells["seaover_purchase_price"].Value = sales_price.ToString("#,##0");
                    //row.Cells["sales_unit"].Value = sales_unit;
                    //매입단가========================================================================================
                    double purchase_unit = box_weight;

                    whr = "품명 = '" + row.Cells["product"].Value.ToString() + "'"
                            + " AND 원산지 = '" + row.Cells["origin"].Value.ToString() + "'"
                            + " AND 규격 = '" + row.Cells["sizes"].Value.ToString() + "'"
                            + " AND (SEAOVER단위 = '" + row.Cells["unit"].Value.ToString() + "' OR " + "계산단위 = '" + row.Cells["unit"].Value.ToString() + "')"
                            + " AND 매입단가 > 6";
                    dtRow = sDt.Select(whr);
                    if (dtRow.Length > 0)
                    {
                        purchase_unit = Convert.ToDouble(dtRow[0]["단위"]);
                        purchase_price1 = Convert.ToDouble(dtRow[0]["매입단가"]);
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
                            purchase_unit = Convert.ToDouble(dtRow[0]["단위"]);
                            purchase_price1 = Convert.ToDouble(dtRow[0]["매입단가"]);
                        }
                    }
                    //없으면 매출단가로 변경(빨간색)
                    if (purchase_price1 == 0)
                    {
                        purchase_price1 = sales_price;
                        purchase_unit = sales_unit;
                        row.Cells["seaover_purchsae_price"].Style.ForeColor = Color.Red;
                    }
                    //원금
                    purchase_price1 = purchase_price1 / purchase_unit * box_weight;
                    row.Cells["seaover_purchsae_price"].Value = purchase_price1.ToString("#,##0");


                    //원가계산                    
                    bool isWeight;
                    if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out isWeight))
                        isWeight = true;

                    double unit;
                    if (isWeight)
                        unit = box_weight;
                    else if (!isWeight && cost_unit == 0)
                        unit = box_weight;
                    else
                        unit = cost_unit;

                    double offer_price;
                    if (row.Cells["offer_price"].Value == null || !double.TryParse(row.Cells["offer_price"].Value.ToString(), out offer_price))
                        offer_price = 0;

                    double tax;
                    if (row.Cells["tax"].Value == null || !double.TryParse(row.Cells["tax"].Value.ToString(), out tax))
                        tax = 0;
                    tax /= 100;
                    double custom;
                    if (row.Cells["custom"].Value == null || !double.TryParse(row.Cells["custom"].Value.ToString(), out custom))
                        custom = 0;
                    custom /= 100;
                    double incidental_expense;
                    if (row.Cells["incidental_expense"].Value == null || !double.TryParse(row.Cells["incidental_expense"].Value.ToString(), out incidental_expense))
                        incidental_expense = 0;
                    incidental_expense /= 100;


                    double cost_price = unit * offer_price;
                    if (tax > 0)
                        cost_price += (unit * offer_price) * tax;
                    if (custom > 0)
                        cost_price += (unit * offer_price) * custom;
                    if (incidental_expense > 0)
                        cost_price += (unit * offer_price) * incidental_expense;
                    double exchange_rate = Convert.ToDouble(row.Cells["exchange_rate"].Value);
                    cost_price = cost_price * exchange_rate;

                    row.Cells["cost_price"].Value = cost_price.ToString("#,##0");

                    //마진율
                    double margin = ((purchase_price1 - cost_price) / purchase_price1 * 100);
                    row.Cells["margin_rate"].Value = margin.ToString("#,##0.00") + "%";
                    //row.Cells["purchase_unit"].Value = purchase_unit;
                }
            }
            else
            {
                MessageBox.Show(this, "검색내역이 없습니다.");
                this.Activate();
            }
            //씨오버원가 들고오기
            ReplaceSalesCostToPendingCost();
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
                            + "^" 
                            + "^" 
                            + "^" 
                            + "^" + row.Cells["unit"].Value.ToString();
            }
            else
                sub_product = row.Cells["merge_product"].Value.ToString().Trim().Split('\n');

            if (sub_product.Length > 0)
            {
                DataTable productDt = priceComparisonRepository.GetNotSalesCostProduct(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
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
                                unit_price = unit_price * box_weight / Convert.ToDouble(row.Cells["unit"].Value.ToString());

                                bool isWeight = Convert.ToBoolean(dr[0]["weight_calculate"].ToString());
                                //트레이단가 -> 중량 단가
                                if (!isWeight)
                                    unit_price = (unit_price * cost_unit) / box_weight;
                                double custom = Convert.ToDouble(dr[0]["custom"].ToString()) / 100;
                                double tax = Convert.ToDouble(dr[0]["tax"].ToString()) / 100;
                                double incidental_expense = Convert.ToDouble(dr[0]["incidental_expense"].ToString()) / 100;
                                //대행일 경우 부대비용 3%
                                if (dr[0]["ato_no"].ToString().Contains("ad") || dr[0]["ato_no"].ToString().Contains("AD")
                                    || dr[0]["ato_no"].ToString().Contains("dw") || dr[0]["ato_no"].ToString().Contains("DW")
                                    || dr[0]["ato_no"].ToString().Contains("od") || dr[0]["ato_no"].ToString().Contains("OD")
                                    || dr[0]["ato_no"].ToString().Contains("hs") || dr[0]["ato_no"].ToString().Contains("HS"))
                                    incidental_expense = 0.025;
                                else if(dr[0]["ato_no"].ToString().Contains("jd") || dr[0]["ato_no"].ToString().Contains("JD"))
                                    incidental_expense = 0.02;


                                double qty = Convert.ToDouble(dr[0]["qty"].ToString());
                                double exchangeRate;
                                if (!double.TryParse(dr[0]["exchange_rate"].ToString(), out exchangeRate))
                                    exchangeRate = exchange_rate;

                                double sales_cost = unit_price * exchangeRate;

                                sales_cost *= (1 + custom);
                                sales_cost *= (1 + tax);
                                sales_cost *= (1 + incidental_expense);


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
                        if (!double.TryParse(row.Cells["unit"].Value.ToString(), out unit))
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
                                if(sales_cost == 0)
                                    sales_cost = Convert.ToDouble(dr[j]["매입금액"].ToString());


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
                                        if (interest < 0)
                                            interest = 0;
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
                                    sales_cost = sales_cost / stock_unit * unit / cost_unit;
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
                dgvStockSales.Rows[0].Cells["seaover_cost"].ToolTipText = "***** AtoNo 'AD,DW,OD,JD,HS' 경우 + 매출원가 2.5%, + 연이자 8% *****\n\n" + tooltip.Trim();
            }
        }
        private void ReplaceSalesCostToPendingCost()
        {
            double exchange_rate;
            if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                exchange_rate = 0;

            if (dgvProduct.Rows.Count == 0)
                return;

            string merge_product = "";
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                if (dgvProduct.Rows[i].Cells["merge_product"].Value == null || string.IsNullOrEmpty(dgvProduct.Rows[i].Cells["merge_product"].Value.ToString()))
                {
                    merge_product += "\n" + dgvProduct.Rows[i].Cells["product"].Value.ToString()
                                    + "^" + dgvProduct.Rows[i].Cells["origin"].Value.ToString()
                                    + "^" + dgvProduct.Rows[i].Cells["sizes"].Value.ToString()
                                    + "^"
                                    + "^"
                                    + "^"
                                    + "^" + dgvProduct.Rows[i].Cells["unit"].Value.ToString();
                }
                else
                {
                    merge_product += "\n" + dgvProduct.Rows[i].Cells["merge_product"].Value.ToString().Trim();
                }
            }

            string[] sub_product = merge_product.Trim().Split('\n');
            DataTable productDt = priceComparisonRepository.GetNotSalesCostProduct(txtProduct.Text + " " + txtProduct2.Text, txtOrigin.Text, txtSizes.Text + " " + txtSizes2.Text, "", sub_product);
            DataTable productDt2 = priceComparisonRepository.GetNotSalesCostProduct2(txtProduct.Text + " " + txtProduct2.Text, txtOrigin.Text, txtSizes.Text + " " + txtSizes2.Text, "", sub_product);
            DataTable pendingDt = customsRepository.GetProductForNotSalesCost(txtProduct.Text + " " + txtProduct2.Text, txtOrigin.Text, txtSizes.Text + " " + txtSizes2.Text, "", productDt, sub_product);
            if (productDt.Rows.Count > 0 || productDt2.Rows.Count > 0 || pendingDt.Rows.Count > 0)
            {
                productDt.Columns["수량"].ReadOnly = false;
                productDt.Columns["환율"].ReadOnly = false;
                productDt.Columns["매출원가"].ReadOnly = false;

                

                //데이터 세팅
                if (productDt.Rows.Count > 0 || pendingDt.Rows.Count > 0)
                {
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
                                double sales_cost = unit_price * exchangeRate * (1 + custom + tax + incidental_expense);

                                productDt.Rows[i]["매출원가"] = sales_cost;
                                productDt.Rows[i]["환율"] = exchangeRate;
                                break;
                            }
                        }
                    }
                    productDt.AcceptChanges();
                }

                //데이터 조합
                if (productDt.Rows.Count > 0 || productDt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    {
                        DataGridViewRow select_row = dgvProduct.Rows[i];
                        if (select_row.Cells["merge_product"].Value == null || string.IsNullOrEmpty(select_row.Cells["merge_product"].Value.ToString()))
                        {
                            sub_product = new string[1];
                            sub_product[0] = select_row.Cells["product"].Value.ToString()
                                        + "^" + select_row.Cells["origin"].Value.ToString()
                                        + "^" + select_row.Cells["sizes"].Value.ToString()
                                        + "^" 
                                        + "^" 
                                        + "^" 
                                        + "^" + select_row.Cells["unit"].Value.ToString();
                        }
                        else
                            sub_product = select_row.Cells["merge_product"].Value.ToString().Trim().Split('\n');

                        double total_average_cost = 0;
                        double total_qty = 0;
                        string tooltip = "";
                        //순회
                        for (int k = 0; k < sub_product.Length; k++)
                        {
                            string[] product = sub_product[k].Trim().Split('^');
                            //씨오버단위
                            double unit;
                            if (!double.TryParse(dgvProduct.Rows[i].Cells["unit"].Value.ToString(), out unit))
                                unit = 1;

                           
                            string whr = "품명 = '" + product[0] + "'"
                                            + " AND 원산지 = '" + product[1] + "'"
                                            + " AND 규격 = '" + product[2] + "'"
                                            + " AND 단위 = '" + product[6] + "'"
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
                                    //동원시 추가
                                    if (dr[j]["AtoNo"].ToString().Contains("dw") || dr[j]["AtoNo"].ToString().Contains("DW")
                                        || dr[j]["AtoNo"].ToString().Contains("hs") || dr[j]["AtoNo"].ToString().Contains("HS")
                                        || dr[j]["AtoNo"].ToString().Contains("od") || dr[j]["AtoNo"].ToString().Contains("OD")
                                        || dr[j]["AtoNo"].ToString().Contains("ad") || dr[j]["AtoNo"].ToString().Contains("AD")
                                        || dr[j]["AtoNo"].ToString().Contains("jd") || dr[j]["AtoNo"].ToString().Contains("JD"))
                                        sales_cost = sales_cost * 1.035;

                                    total_average_cost += sales_cost * qty;
                                    total_qty += qty;

                                    tooltip += "\n AtoNo :" + dr[j]["AtoNo"].ToString() + "    매출원가 :" + sales_cost.ToString("#,##0") + " (" + dr[j]["환율"].ToString() + ")  수량 :" + qty;
                                }
                            }

                            whr = "품명 = '" + product[0] + "'"
                                    + " AND 원산지 = '" + product[1] + "'"
                                    + " AND 규격 = '" + product[2] + "'"
                                    + " AND 단위 = '" + product[6] + "'";
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
                                    total_average_cost += sales_cost * qty;
                                    total_qty += qty;

                                    tooltip += "\n AtoNo : " + dr[j]["AtoNo"].ToString() + "    매출원가 : " + sales_cost.ToString("#,##0") + "  수량 :" + qty;
                                }
                            }
                            
                        }
                        //평균원가
                        if (total_average_cost > 0 && total_qty > 0)
                        {
                            //S원가
                            double seaover_cost_price = (total_average_cost / total_qty);
                            dgvProduct.Rows[i].Cells["seaover_cost_price"].Value = seaover_cost_price.ToString("#,##0");
                            dgvProduct.Rows[i].Cells["seaover_cost_price"].ToolTipText = " " + tooltip.Trim();
                        }
                    }
                }
            }
        }

        #endregion

        #region Procedure Method
        private void CallStockProcedure()
        {
            //품명별재고현황 스토어프로시져 호출
            try
            {
                string sDate = DateTime.Now.ToString("yyyy-MM-dd");
                string eDate = DateTime.Now.ToString("yyyy-MM-dd");
                string user_id = um.seaover_id;
                if (priceComparisonRepository.CallStoredProcedureSTOCK(user_id, eDate) == 0)
                    MessageBox.Show(this, "호출 내용이 없음");
                this.Activate();
            }
            catch (Exception e)
            {
                MessageBox.Show(this,e.Message);
                this.Activate();
            }
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
        #endregion

        #region 역대 오퍼가 Method
        private void GetOffer(DataGridViewRow row, bool isCompanyRefresh = true)
        {
            //초기화
            dgvOfferPrice.Rows.Clear();
            chtOffer.Series.Clear();
            if (isCompanyRefresh)
            {
                cbOfferCompany.Items.Clear();
                cbOfferCompany.Text = String.Empty;
            }
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
            double fixed_tariff;
            if (row.Cells["fixed_tariff"].Value == null || !double.TryParse(row.Cells["fixed_tariff"].Value.ToString(), out fixed_tariff))
                fixed_tariff = 0;
            fixed_tariff /= 1000;
            double unit;
            if (row.Cells["unit"].Value == null || !double.TryParse(row.Cells["unit"].Value.ToString(), out unit))
                unit = 0;
            double cost_unit;
            if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                cost_unit = 0;
            bool isWeight = Convert.ToBoolean(row.Cells["weight_calculate"].Value);
            if (!isWeight && cost_unit == 0)
                isWeight = true;
            //현재환율
            double exchange_rate;
            if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                exchange_rate = 0;
            //오퍼내역
            string merge_product = "";
            bool isMerge = false;
            if (row.Cells["merge_product"].Value != null && !string.IsNullOrEmpty(row.Cells["merge_product"].Value.ToString()))
            {
                merge_product = row.Cells["merge_product"].Value.ToString();
                isMerge = true;
            }
            DataTable offerDt = purchasePriceRepository.GetPurchasePriceForChartDay(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                    , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString()
                    , merge_product, isMerge);
            //거래처리스트 최신화
            if (isCompanyRefresh && offerDt.Rows.Count > 0)
            {
                List<string> companyList = new List<string>();
                companyList.Add("전체");
                for (int i = 0; i < offerDt.Rows.Count; i++)
                {
                    companyList.Add(offerDt.Rows[i]["cname"].ToString());
                }
                //중복제거
                companyList = companyList.Distinct().ToList();
                for (int i = 0; i < companyList.Count; i++)
                {
                    cbOfferCompany.Items.Add(companyList[i].ToString());
                }
                //cbOfferCompany.Text = cbOfferCompany.Items[0].ToString();
            }
            //거래처필터
            string company = cbOfferCompany.Text;
            if (company == "전체")
                company = string.Empty;

            //실제데이터 뿌리기
            if (offerDt.Rows.Count > 0)
            {
                bool isFindSameOffer = false;
                for (int i = 0; i < offerDt.Rows.Count; i++)
                {
                    bool isOutput = true;
                    if (!string.IsNullOrEmpty(company) && company != offerDt.Rows[i]["cname"].ToString())
                        isOutput = false;

                    if (isOutput)
                    {
                        int n = dgvOfferPrice.Rows.Add();
                        //선택한 내역일 경우
                        if (row.Cells["offer_price"].Value.ToString() == Convert.ToDouble(offerDt.Rows[i]["purchase_price"].ToString()).ToString("#,##0.00")
                            && row.Cells["shipper"].Value != null && row.Cells["shipper"].Value.ToString() == offerDt.Rows[i]["cname"].ToString()
                            && !isFindSameOffer)
                        {
                            dgvOfferPrice.Rows[n].DefaultCellStyle.BackColor = Color.Beige;
                            isFindSameOffer = true;
                        }
                            

                        dgvOfferPrice.Rows[n].Cells["offer_date"].Value = Convert.ToDateTime(offerDt.Rows[i]["updatetime"].ToString()).ToString("yy/MM/dd");
                        dgvOfferPrice.Rows[n].Cells["offer_company"].Value = offerDt.Rows[i]["cname"].ToString();
                        double purchase_price = Convert.ToDouble(offerDt.Rows[i]["purchase_price"].ToString());
                        dgvOfferPrice.Rows[n].Cells["unit_price"].Value = purchase_price.ToString("#,##0.00");
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
                            //cost_price = (purchase_price * exchange_rate * (1 + tax + custom + incidental_expense) * cost_unit);
                            //cost_price = (purchase_price * exchange_rate * (1 + tax + custom + incidental_expense));
                            cost_price = purchase_price * exchange_rate;
                            cost_price *= (custom + 1);
                            cost_price *= (tax + 1);
                            cost_price *= (incidental_expense + 1);
                        }

                        dgvOfferPrice.Rows[n].Cells["unit_cost_price"].Value = cost_price.ToString("#,##0");
                        //매입가
                        double offer_purchase_price = 0;
                        if (cbPurchasePrice.Text == "매입가")
                        {
                            dgvPurchaseUsd.EndEdit();
                            DataTable purchasePriceDt = common.ConvertDgvToDataTable(dgvPurchaseUsd);
                            if (purchasePriceDt.Rows.Count > 0)
                            {
                                DataRow[] dr = null;
                                string whr = "purchase_date = '" + Convert.ToDateTime(offerDt.Rows[i]["updatetime"].ToString()).ToString("yy") + "/" + Convert.ToDateTime(offerDt.Rows[i]["updatetime"].ToString()).ToString("MM") + "'";
                                dr = purchasePriceDt.Select(whr);
                                if (dr.Length > 0)
                                    offer_purchase_price = Convert.ToDouble(dr[0]["price"].ToString());
                            }
                        }
                        //최저가
                        else
                        {
                            if (dgvProduct.Rows.Count > 0)
                            {
                                for (int j = 0; j < dgvProduct.Rows.Count; j++)
                                {
                                    if (Convert.ToBoolean(dgvProduct.Rows[j].Cells["chk"].Value))
                                    {
                                        offer_purchase_price = Convert.ToDouble(dgvProduct.Rows[j].Cells["seaover_purchsae_price"].Value.ToString());
                                        if (!isWeight)
                                            offer_purchase_price /= cost_unit;
                                        break;
                                    }
                                }
                            }
                        }
                        dgvOfferPrice.Rows[n].Cells["offer_purchase_price"].Value = offer_purchase_price.ToString("#,##0");
                        //마진
                        dgvOfferPrice.Rows[n].Cells["offer_margin"].Value = ((offer_purchase_price - cost_price) / offer_purchase_price * 100).ToString("#,##0.00") + "%";
                    }
                }
            }
            SetOfferChart(row);
        }
        private void SetOfferChart(DataGridViewRow select_row)
        {
            Chart cht = chtOffer;
            cht.Series.Clear();
            if (dgvPurchaseUsd.Rows.Count > 0)
            {
                double max_krw = 0, min_krw = 9999999, max_usd = 0, min_usd = 9999999;
                Series series = new Series();
                Series select_series = new Series();
                //데이터 출력
                for (int i = 0; i < dgvOfferPrice.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvOfferPrice.Rows[i];
                    double price_krw = Convert.ToDouble(row.Cells["unit_cost_price"].Value.ToString());
                    double offer_price = Convert.ToDouble(row.Cells["unit_price"].Value.ToString());
                    string offer_date = Convert.ToDateTime(row.Cells["offer_date"].Value.ToString()).ToString("yyyy-MM-dd");
                    //Chart
                    if (cbChtPurchaseType2.Text == "오퍼가")
                    {
                        series.Points.AddXY(offer_date, offer_price);
                        select_series.Points.AddXY(offer_date, Convert.ToDouble(select_row.Cells["offer_price"].Value.ToString()));
                    }
                    else
                    {
                        series.Points.AddXY(offer_date, price_krw);
                        select_series.Points.AddXY(offer_date, Convert.ToDouble(select_row.Cells["cost_price"].Value.ToString()));
                    }

                    //MAX,Min
                    if (max_krw < price_krw)
                        max_krw = price_krw;
                    if (min_krw > price_krw)
                        min_krw = price_krw;
                    if (max_usd < offer_price)
                        max_usd = offer_price;
                    if (min_usd > offer_price)
                        min_usd = offer_price;
                }

                //매입내역 데이터
                if (cbChartType2.Text == "막대")
                    series.ChartType = SeriesChartType.Column;
                else
                    series.ChartType = SeriesChartType.Line;
                //선택한 단가
                select_series.Color = Color.Red;
                select_series.ChartType = SeriesChartType.Line;

                //출력범위
                cht.ChartAreas[0].AxisY.IsLogarithmic = false; // true 면 maximum minimum 조절안됨
                if (cbChtPurchaseType2.Text == "오퍼가")
                {
                    cht.ChartAreas[0].AxisY.Maximum = Math.Ceiling(max_usd) * 1.1;
                    cht.ChartAreas[0].AxisY.Minimum = 0;
                }
                else
                {
                    cht.ChartAreas[0].AxisY.Maximum = Math.Ceiling(max_krw * 1.1);
                    cht.ChartAreas[0].AxisY.Minimum = 0;
                }
                //차트추가
                cht.Series.Add(series);
                cht.Series.Add(select_series);
                //최대,최소 및 통계
                double rank_krw;
                string rank_krw_detail;
                GetRank2("KRW", Convert.ToDouble(select_row.Cells["cost_price"].Value.ToString()), out rank_krw, out rank_krw_detail);
                if (rank_krw < 10)
                {
                    lbUpdown3.Text = "▲";
                    lbUpdown3.ForeColor = Color.Red;
                }
                else if (rank_krw < 30)
                {
                    lbUpdown3.Text = "▲";
                    lbUpdown3.ForeColor = Color.Pink;
                }
                else if (rank_krw > 90)
                {
                    lbUpdown3.Text = "▼";
                    lbUpdown3.ForeColor = Color.DarkViolet;
                }
                else if (rank_krw > 70)
                {
                    lbUpdown3.Text = "▼";
                    lbUpdown3.ForeColor = Color.Blue;
                }
                else if (rank_krw > 50)
                {
                    lbUpdown3.Text = "▼";
                    lbUpdown3.ForeColor = Color.SkyBlue;
                }
                else
                    lbUpdown3.Text = "";
                double rank_usd;
                string rank_usd_detail;
                GetRank2("USD", Convert.ToDouble(select_row.Cells["offer_price"].Value.ToString()), out rank_usd, out rank_usd_detail);
                if (rank_usd < 10)
                {
                    lbUpdown4.Text = "▲";
                    lbUpdown4.ForeColor = Color.Red;
                }
                else if (rank_usd < 30)
                {
                    lbUpdown4.Text = "▲";
                    lbUpdown4.ForeColor = Color.Pink;
                }
                else if (rank_usd > 90)
                {
                    lbUpdown4.Text = "▼";
                    lbUpdown4.ForeColor = Color.DarkViolet;
                }
                else if (rank_usd > 70)
                {
                    lbUpdown4.Text = "▼";
                    lbUpdown4.ForeColor = Color.Blue;
                }
                else if (rank_usd > 50)
                {
                    lbUpdown4.Text = "▼";
                    lbUpdown4.ForeColor = Color.SkyBlue;
                }
                else
                    lbUpdown4.Text = "";

                lbOfferMaxMin.Text = "상위 " + rank_krw.ToString("#,##0.0") + "%   KRW : " + min_krw.ToString("#,##0") + " ~ " + max_krw.ToString("#,##0")
                               + "\n상위 " + rank_usd.ToString("#,##0.0") + "%    USD : " + min_usd.ToString("#,##0.00") + " ~ " + max_usd.ToString("#,##0.00");

                lbSelectPrice1.Text = "원가계산 :       " + Convert.ToDouble(select_row.Cells["cost_price"].Value.ToString()).ToString("#,##0")
                    + "\n오퍼가 :          " + Convert.ToDouble(select_row.Cells["offer_price"].Value.ToString()).ToString("#,##0.00");
                lbSelectPrice2.Text = "원가계산 :       " + Convert.ToDouble(select_row.Cells["cost_price"].Value.ToString()).ToString("#,##0")
                    + "\n오퍼가 :          " + Convert.ToDouble(select_row.Cells["offer_price"].Value.ToString()).ToString("#,##0.00");

                lbPriceUpDown2.Text = rank_krw_detail + "\n" + rank_usd_detail;

            }
        }
        private void GetRank2(string rank_type, double price, out double ranking, out string rank_detail)
        {
            ranking = 0;
            if (dgvOfferPrice.Rows.Count > 0)
            {
                double[] rank = new double[dgvOfferPrice.Rows.Count + 1];
                for (int i = 0; i < dgvOfferPrice.Rows.Count; i++)
                {
                    if (rank_type == "USD")
                        rank[i] = Convert.ToDouble(dgvOfferPrice.Rows[i].Cells["unit_price"].Value.ToString());
                    else
                        rank[i] = Convert.ToDouble(dgvOfferPrice.Rows[i].Cells["unit_cost_price"].Value.ToString());
                }
                rank[dgvOfferPrice.Rows.Count] = price;

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
        #endregion

        #region 품목정보 불러오기
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

            txtInQty.Text = "0";
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

            CountryModel cModel = new CountryModel();
            cModel = configRepository.GetCountryConfigAsOne(row.Cells["origin"].Value.ToString());
            //배송기간
            int delivery_day;
            if (cModel != null)
                delivery_day = cModel.delivery_days;
            else
                delivery_day = 15;
            txtShippingTerm.Text = delivery_day.ToString();
            //생산일
            txtMakeTerm.Text = row.Cells["production_days"].Value.ToString();

            //오퍼내역
            double offer_price;
            if (row.Cells["offer_price"].Value == null || !double.TryParse(row.Cells["offer_price"].Value.ToString(), out offer_price))
                offer_price = 0;
            txtOfferPrice.Text = offer_price.ToString("#,##0.00");
            txtOfferUpdatetime.Text = row.Cells["updatetime"].Value.ToString();
            txtOfferCompany.Text = row.Cells["shipper"].Value.ToString();
            //오퍼리스트
            GetPurchasePriceList(row);
            txtOfferCostPrice.Text = calculateCostPrice(offer_price).ToString("#,##0");
            //조업시기, 생산시기
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

            DataTable dt = contractRecommendationRepository.GetRecommendAsOne(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString());
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
                dgv.Rows[0].Cells[Convert.ToInt16(month) - 1].Style.BackColor = color;
            }
            dgvMakePeriod.ClearSelection();
            dgvContractPeriod.ClearSelection();
        }
        #endregion

        #region 역대 매입내역 Method
        private void dgvPurchaseUsd_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvPurchaseUsd.Columns[e.ColumnIndex].Name == "exchangeRate" || dgvPurchaseUsd.Columns[e.ColumnIndex].Name == "price_krw")
                {
                    dgvPurchaseUsd.EndEdit();
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
                        double cost_unit;
                        if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                            cost_unit = 0;
                        bool isWeight = Convert.ToBoolean(row.Cells["weight_calculate"].Value);
                        if (!isWeight && cost_unit == 0)
                            isWeight = true;

                        double exchange_rate;
                        if (dgvPurchaseUsd.Rows[e.RowIndex].Cells["exchangeRate"].Value == null || !double.TryParse(dgvPurchaseUsd.Rows[e.RowIndex].Cells["exchangeRate"].Value.ToString(), out exchange_rate))
                            exchange_rate = 0;
                        double price_krw;
                        if (dgvPurchaseUsd.Rows[e.RowIndex].Cells["price_krw"].Value == null || !double.TryParse(dgvPurchaseUsd.Rows[e.RowIndex].Cells["price_krw"].Value.ToString(), out price_krw))
                            price_krw = 0;
                        //USD 매입단가
                        double price_usd;
                        if (!isWeight)
                            price_usd = (price_krw - price_krw * cost_unit * (tax + custom + incidental_expense)) / exchange_rate;
                        else
                            price_usd = (price_krw - price_krw * unit * (tax + custom + incidental_expense)) / exchange_rate;

                        dgvPurchaseUsd.Rows[e.RowIndex].Cells["price_usd"].Value = price_usd.ToString("#,##0");
                    }
                }
            }
        }
        private void GetPurchase(DataGridViewRow row, bool isUserSettingPurchaseMaring = false)
        {
            //초기화
            dgvPurchaseUsd.Rows.Clear();
            chtPurchase.Series.Clear();
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
                if (purchase_margin > 0)
                    lbSetting.Visible = true;
                else
                    lbSetting.Visible = false;
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
            //트레이
            double cost_unit;
            if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                cost_unit = 0;
            //계산방식
            bool isWeight = Convert.ToBoolean(row.Cells["weight_calculate"].Value);
            if (!isWeight && cost_unit == 0)
                isWeight = true;

            //품명코드, 규격코드가 있으면 코드검색
            string product_code = "";
            if (row.Cells["product_code"].Value != null && !string.IsNullOrEmpty(row.Cells["product_code"].Value.ToString().Trim()))
                product_code = row.Cells["product_code"].Value.ToString().Trim();
            string sizes_code = "";
            if (row.Cells["sizes_code"].Value != null && !string.IsNullOrEmpty(row.Cells["sizes_code"].Value.ToString().Trim()))
                sizes_code = row.Cells["sizes_code"].Value.ToString().Trim();
            //매입내역 Data
            purchaseDt = purchaseRepository.GetPurchaseProduct(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), unit.ToString(), "", product_code, sizes_code);
            //오퍼내역 Data
            string select_unit = unit.ToString();
            DataTable offerDt = purchasePriceRepository.GetPurhcasePriceAverage(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), unit.ToString(), "", false);
            //DataTable offerDt = new DataTable();
            //환율내역 Data
            DataTable exchangeRateDt = purchaseRepository.GetExchangeRate(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));
            //팬딩내역 Data
            string merge_product = "";
            bool isMerge = false;
            if (row.Cells["merge_product"].Value != null && !string.IsNullOrEmpty(row.Cells["merge_product"].Value.ToString()))
            {
                merge_product = row.Cells["merge_product"].Value.ToString();
                isMerge = true;
            }
            DataTable customDt = customsRepository.GetDashboard(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                                                         , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), unit.ToString()
                                                         , merge_product, isMerge);
            lbMaxMin.Text = "";
            //출력==========================================================================================================================================
            this.dgvPurchaseUsd.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseUsd_CellValueChanged);
            switch (cbPurchasePriceType.Text)
            {
                case "오퍼가평균":
                    {
                        //offerDt = purchasePriceRepository.GetPurhcasePriceAverage(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), "", unit.ToString());
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

                                if (price_krw > 0 && offer_price > 0)
                                {
                                    int n = dgvPurchaseUsd.Rows.Add();
                                    string sdate = offerDt.Rows[i]["updatetime"].ToString().Substring(2, 2) + "/" + offerDt.Rows[i]["updatetime"].ToString().Substring(4, 2);
                                    dgvPurchaseUsd.Rows[n].Cells["purchase_date"].Value = sdate;
                                    dgvPurchaseUsd.Rows[n].Cells["price"].Value = purcahse.ToString("#,##0");
                                    dgvPurchaseUsd.Rows[n].Cells["price_krw"].Value = price_krw.ToString("#,##0");
                                    dgvPurchaseUsd.Rows[n].Cells["exchangeRate"].Value = exchange_rate.ToString("#,##0.00");
                                    dgvPurchaseUsd.Rows[n].Cells["price_usd"].Value = offer_price.ToString("#,##0.00");
                                    dgvPurchaseUsd.Rows[n].Cells["division"].Value = division;
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
                                            price_krw = offer_price * exchange_rate * (1 + tax + custom + incidental_expense) * unit;
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
                                    else
                                        price_krw = out_purchase;
                                    //USD 매입단가
                                    if (!isWeight)
                                        offer_price = price_krw / exchange_rate / cost_unit / (1 + tax + custom + incidental_expense);
                                    else
                                        offer_price = price_krw / exchange_rate / unit / (1 + tax + custom + incidental_expense);
                                }

                                if (price_krw > 0 && offer_price > 0)
                                {
                                    int n = dgvPurchaseUsd.Rows.Add();
                                    string sdate = purchaseDt.Rows[i]["매입일자"].ToString().Substring(2, 2) + "/" + purchaseDt.Rows[i]["매입일자"].ToString().Substring(4, 2);
                                    dgvPurchaseUsd.Rows[n].Cells["purchase_date"].Value = sdate;

                                    //2023-08-22 팩일경우 팩단가로 변경
                                    if (!isWeight)
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
                                    dgvPurchaseUsd.Rows[n].Cells["division"].Value = division;
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
                                }
                            }   
                        }
                    }
                    break;
                
            }
            this.dgvPurchaseUsd.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseUsd_CellValueChanged);
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
            Chart cht = chtPurchase;
            cht.Series.Clear();
            if (dgvPurchaseUsd.Rows.Count > 0)
            {
                double max_krw = 0, min_krw = 9999999, max_usd = 0, min_usd = 9999999;
                Series series = new Series();
                Series select_series = new Series();
                for (int i = 0; i < dgvPurchaseUsd.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvPurchaseUsd.Rows[i];
                    double price = Convert.ToDouble(row.Cells["price"].Value.ToString());
                    double price_krw = Convert.ToDouble(row.Cells["price_krw"].Value.ToString());
                    double offer_price = Convert.ToDouble(row.Cells["price_usd"].Value.ToString());
                    double comparison_price;
                    //Chart   MAX,Min
                    if (select_row.Cells["origin"].Value.ToString() == "국산")
                    {
                        comparison_price = Convert.ToDouble(row.Cells["price"].Value.ToString());
                        series.Points.AddXY(row.Cells["purchase_date"].Value.ToString(), comparison_price);
                        select_series.Points.AddXY(row.Cells["purchase_date"].Value.ToString(), Convert.ToDouble(select_row.Cells["cost_price"].Value.ToString()));
                    }
                    else if (cbChtPurchaseType.Text == "단가($)")
                    {
                        comparison_price = Convert.ToDouble(row.Cells["price_usd"].Value.ToString());
                        series.Points.AddXY(row.Cells["purchase_date"].Value.ToString(), comparison_price);
                        select_series.Points.AddXY(row.Cells["purchase_date"].Value.ToString(), Convert.ToDouble(select_row.Cells["offer_price"].Value.ToString()));

                    }

                    else if (cbChtPurchaseType.Text == @"단가(\)")
                    {
                        comparison_price = Convert.ToDouble(row.Cells["price_krw"].Value.ToString());
                        series.Points.AddXY(row.Cells["purchase_date"].Value.ToString(), comparison_price);
                        select_series.Points.AddXY(row.Cells["purchase_date"].Value.ToString(), Convert.ToDouble(select_row.Cells["cost_price"].Value.ToString()));
                    }
                    else
                    {
                        comparison_price = Convert.ToDouble(row.Cells["price"].Value.ToString());
                        series.Points.AddXY(row.Cells["purchase_date"].Value.ToString(), comparison_price);
                        select_series.Points.AddXY(row.Cells["purchase_date"].Value.ToString(), Convert.ToDouble(select_row.Cells["cost_price"].Value.ToString()));
                    }
                        


                    if (max_krw < comparison_price)
                        max_krw = comparison_price;
                    if (min_krw > comparison_price)
                        min_krw = comparison_price;
                    if (max_usd < offer_price)
                        max_usd = offer_price;
                    if (min_usd > offer_price)
                        min_usd = offer_price;
                }
                //매입내역 데이터
                if (cbChartType.Text == "막대")
                    series.ChartType = SeriesChartType.Column;
                else
                    series.ChartType = SeriesChartType.Line;
                //선택한 단가
                select_series.Color = Color.Red;
                select_series.ChartType = SeriesChartType.Line;

                //출력범위
                cht.ChartAreas[0].AxisY.IsLogarithmic = false; // true 면 maximum minimum 조절안됨
                if (cbChtPurchaseType.Text == "단가($)")
                {
                    cht.ChartAreas[0].AxisY.Maximum = Math.Ceiling(max_usd) * 1.1;
                    cht.ChartAreas[0].AxisY.Minimum = 0;
                }
                else 
                {
                    cht.ChartAreas[0].AxisY.Maximum = Math.Ceiling(max_krw * 1.1);
                    cht.ChartAreas[0].AxisY.Minimum = 0;
                }
                //차트추가
                cht.Series.Add(series);
                cht.Series.Add(select_series);
                //최대최소 및 통계
                double rank_krw;
                string rank_krw_detial;

                GetRank("KRW", Convert.ToDouble(select_row.Cells["cost_price"].Value.ToString()), out rank_krw, out rank_krw_detial);
                if (rank_krw < 10)
                {
                    lbUpdown1.Text = "▲";
                    lbUpdown1.ForeColor = Color.Red;
                }
                else if (rank_krw < 30)
                {
                    lbUpdown1.Text = "▲";
                    lbUpdown1.ForeColor = Color.Pink;
                }
                else if (rank_krw > 90)
                {
                    lbUpdown1.Text = "▼";
                    lbUpdown1.ForeColor = Color.DarkViolet;
                }
                else if (rank_krw > 70)
                {
                    lbUpdown1.Text = "▼";
                    lbUpdown1.ForeColor = Color.Blue;
                }
                else if (rank_krw > 50)
                {
                    lbUpdown1.Text = "▼";
                    lbUpdown1.ForeColor = Color.SkyBlue;
                }
                else
                    lbUpdown1.Text = "";



                double rank_usd;
                string rank_usd_detial;
                GetRank("USD", Convert.ToDouble(select_row.Cells["offer_price"].Value.ToString()), out rank_usd, out rank_usd_detial);
                if (rank_usd < 10)
                {
                    lbUpdown2.Text = "▲";
                    lbUpdown2.ForeColor = Color.Red;
                }
                else if (rank_usd < 30)
                {
                    lbUpdown2.Text = "▲";
                    lbUpdown2.ForeColor = Color.Pink;
                }
                else if (rank_usd > 90)
                {
                    lbUpdown2.Text = "▼";
                    lbUpdown2.ForeColor = Color.DarkViolet;
                }
                else if (rank_usd > 70)
                {
                    lbUpdown2.Text = "▼";
                    lbUpdown2.ForeColor = Color.Blue;
                }
                else if (rank_usd > 50)
                {
                    lbUpdown2.Text = "▼";
                    lbUpdown2.ForeColor = Color.SkyBlue;
                }
                else
                    lbUpdown2.Text = "";


                lbMaxMin.Text = "상위 " + rank_krw.ToString("#,##0.0") + "%  : " + min_krw.ToString("#,##0") + " ~ " + max_krw.ToString("#,##0")
                                           + "\n상위 " + rank_usd.ToString("#,##0.0") + "%  : " + min_usd.ToString("#,##0.00") + " ~ " + max_usd.ToString("#,##0.00");
                lbPriceUpDown1.Text = rank_krw_detial + "\n" + rank_usd_detial;
            }
        }
        #endregion

        #region 역대 판매내역 Method
        private void GetSales(DataGridViewRow row)
        {
            //초기화
            dgvSales.Rows.Clear();
            dgvSalesByMonth.Rows.Clear();
            dgvSalesByMonth.Rows.Add();
            chtSales.Series.Clear();
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
            string merge_product = "";
            bool isMerge = false;
            if (row.Cells["merge_product"].Value != null && !string.IsNullOrEmpty(row.Cells["merge_product"].Value.ToString()))
            {
                merge_product = row.Cells["merge_product"].Value.ToString();
                isMerge = true;
            }
            DataTable salesDt = salesRepository.GetSalesProduct(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString()
                , merge_product, isMerge, "");
            //제외매출
            DataTable eDt = productExcludedSalesRepository.GetExcludedSalesByMonth(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                                                , row.Cells["product"].Value.ToString()
                                                , row.Cells["origin"].Value.ToString()
                                                , row.Cells["sizes"].Value.ToString()
                                                , "", "", "", row.Cells["unit"].Value.ToString()
                                                , merge_product, isMerge);

            //출력==========================================================================================================================================
            if (salesDt.Rows.Count > 0)
            {
                double max = 0, min = 0;
                Series series = new Series();
                for (int i = 0; i < salesDt.Rows.Count; i++)
                {
                    int n = dgvSales.Rows.Add();
                    string sdate = salesDt.Rows[i]["매출일자"].ToString().Substring(2, 2) + "/" + salesDt.Rows[i]["매출일자"].ToString().Substring(4, 2);
                    dgvSales.Rows[n].Cells["sale_date"].Value = sdate;
                    dgvSales.Rows[n].Cells["purchase_price"].Value = salesDt.Rows[i]["매출금액"].ToString();

                    double sales_qty;
                    if (!double.TryParse(salesDt.Rows[i]["단품매출수"].ToString(), out sales_qty))
                        sales_qty = 0;

                    double unit;
                    if (row.Cells["unit"].Value == null || !double.TryParse(row.Cells["unit"].Value.ToString(), out unit))
                        unit = 1;
                    sales_qty /= unit;

                    if (eDt.Rows.Count > 0)
                    {
                        for (int j = 0; j < eDt.Rows.Count; j++)
                        {
                            if (salesDt.Rows[i]["매출일자"].ToString() == eDt.Rows[j]["sale_date"].ToString())
                            {
                                double exclude_qty;
                                if (!double.TryParse(eDt.Rows[0]["sale_qty"].ToString(), out exclude_qty))
                                    exclude_qty = 0;
                                sales_qty -= exclude_qty;
                            }
                        }
                        
                    }


                    dgvSales.Rows[n].Cells["sales_qty"].Value = sales_qty.ToString("#,##0");
                    dgvSales.Rows[n].Cells["sales_price"].Value = Convert.ToDouble(salesDt.Rows[i]["매출금액"].ToString()).ToString("#,##0");
                    dgvSales.Rows[n].Cells["margin"].Value = Convert.ToDouble(salesDt.Rows[i]["마진율"].ToString()).ToString("#,##0.00") + "%";
                    dgvSales.Rows[n].Cells["margin_amount"].Value = Convert.ToDouble(salesDt.Rows[i]["마진금액"].ToString()).ToString("#,##0");

                    //Chart series 추가
                    double tmp;
                    if (cbSalesChart.Text == "마진율")
                        tmp = Convert.ToDouble(salesDt.Rows[i]["마진율"].ToString());
                    else if (cbSalesChart.Text == "매출금액")
                        tmp = Convert.ToDouble(salesDt.Rows[i]["매출금액"].ToString());
                    else
                        tmp = Convert.ToDouble(salesDt.Rows[i]["매출수"].ToString());
                    series.Points.AddXY(salesDt.Rows[i]["매출일자"].ToString(), tmp);
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
                //출력범위
                chtSales.ChartAreas[0].AxisY.IsLogarithmic = false; // true 면 maximum minimum 조절안됨
                if (cbChtPurchaseType2.Text == "오퍼가")
                {
                    chtSales.ChartAreas[0].AxisY.Maximum = Math.Ceiling(max) * 1.1;
                    chtSales.ChartAreas[0].AxisY.Minimum = min;
                }
                else
                {
                    chtSales.ChartAreas[0].AxisY.Maximum = Math.Ceiling(max * 1.1);
                    chtSales.ChartAreas[0].AxisY.Minimum = min;
                }
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
                chtSales.Series.Add(series);


                //평균매출===============================================================================================================================
                DataTable avgDt = salesRepository.GetAverageSalesByMonth2(DateTime.Now, row.Cells["product"].Value.ToString()
                                                                        , row.Cells["origin"].Value.ToString()
                                                                        , row.Cells["sizes"].Value.ToString()
                                                                        , row.Cells["unit"].Value.ToString()
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
                if (row.Cells["unit"].Value == null || !double.TryParse(row.Cells["unit"].Value.ToString(), out main_unit))
                    main_unit = 1;

                if (avgDt.Rows.Count > 0)
                {
                    int wDays;
                    //common.GetWorkDay(DateTime.Now.AddDays(-1).AddMonths(-1), DateTime.Now.AddDays(-1), out wDays);

                    //1개월
                    double qty;
                    if (!double.TryParse(avgDt.Rows[0][0].ToString(), out qty))
                        qty = 0;
                    sttdate = DateTime.Now.AddMonths(-1);
                    enddate = DateTime.Now;
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

                    //45일
                    if (!double.TryParse(avgDt.Rows[0][1].ToString(), out qty))
                        qty = 0;
                    sttdate = DateTime.Now.AddDays(-45);
                    enddate = DateTime.Now;
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
                    wDays--;
                    dgvSalesByMonth.Rows[0].Cells[1].Value = (qty / wDays * 21).ToString("#,##0");
                    //2개월
                    if (!double.TryParse(avgDt.Rows[0][2].ToString(), out qty))
                        qty = 0;
                    sttdate = DateTime.Now.AddMonths(-2);
                    enddate = DateTime.Now;
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
                    //3개월
                    if (!double.TryParse(avgDt.Rows[0][3].ToString(), out qty))
                        qty = 0;
                    sttdate = DateTime.Now.AddMonths(-3);
                    enddate = DateTime.Now;
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
                    //6개월
                    if (!double.TryParse(avgDt.Rows[0][4].ToString(), out qty))
                        qty = 0;
                    sttdate = DateTime.Now.AddMonths(-6);
                    enddate = DateTime.Now;
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
                    //12개월
                    if (!double.TryParse(avgDt.Rows[0][5].ToString(), out qty))
                        qty = 0;
                    sttdate = DateTime.Now.AddMonths(-12);
                    enddate = DateTime.Now;
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
                    //18개월
                    if (!double.TryParse(avgDt.Rows[0][6].ToString(), out qty))
                        qty = 0;
                    sttdate = DateTime.Now.AddMonths(-18);
                    enddate = DateTime.Now;
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
            smModel.unit = row.Cells["unit"].Value.ToString();

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
            string sDate = DateTime.Now.AddMonths(-salesMonth).ToString("yyyy-MM-dd");
            if(salesMonth == 45)
                sDate = DateTime.Now.AddDays(-salesMonth).ToString("yyyy-MM-dd");
            string eDate = DateTime.Now.ToString("yyyy-MM-dd");
            common.GetWorkDay(Convert.ToDateTime(sDate), Convert.ToDateTime(eDate), out workDays);
            workDays--;


            string[] merge_product;
            //머지품목 아닐때
            if (row.Cells["merge_product"].Value == null || string.IsNullOrEmpty(row.Cells["merge_product"].Value.ToString()))
            {
                merge_product = new string[1];
                merge_product[0] = row.Cells["product"].Value.ToString()
                                + "^" + row.Cells["origin"].Value.ToString()
                                + "^" + row.Cells["sizes"].Value.ToString()
                                + "^" 
                                + "^" 
                                + "^" 
                                + "^" + row.Cells["unit"].Value.ToString();
            }
            //머지품목 일때
            else
                merge_product = row.Cells["merge_product"].Value.ToString().Split('\n');

            //데이터 출력
            double total_unpending_qty = 0;
            double total_pending_qty = 0;
            double total_reserved_qty = 0;
            double unit;
            //판매량
            double stock = 0, salesCnt = 0;
            //영업일 수
            DateTime sttdate, enddate;
            for (int i = 0; i < merge_product.Length; i++)
            {
                string[] product = merge_product[i].Split('^');
                //재고, 매출량
                //DataTable productDt = seaoverRepository.GetStockAndSalesDetail(product[0], product[1], product[2], product[3], salesMonth.ToString(), cbAllUnit.Checked);
                DataTable productDt = seaoverRepository.GetStockAndSalesDetail(product[0], product[1], product[2], product[6], salesMonth.ToString());
                if (productDt.Rows.Count > 0)
                {

                    if (row.Cells["unit"].Value == null || !double.TryParse(row.Cells["unit"].Value.ToString(), out unit))
                        unit = 1;

                    double unpending_qty = Convert.ToDouble(productDt.Rows[0]["미통관"].ToString()) * Convert.ToDouble(product[6]) / unit;
                    total_unpending_qty += unpending_qty;
                    double pending_qty = Convert.ToDouble(productDt.Rows[0]["통관"].ToString()) * Convert.ToDouble(product[6]) / unit;
                    total_pending_qty += pending_qty;
                    double reserved_qty = Convert.ToDouble(productDt.Rows[0]["예약수"].ToString()) * Convert.ToDouble(product[6]) / unit;
                    total_reserved_qty += reserved_qty;

                    //총 판매, 재고
                    stock += (unpending_qty + pending_qty - reserved_qty);
                    salesCnt += Convert.ToDouble(productDt.Rows[0]["매출수"].ToString()) * Convert.ToDouble(product[6]) / unit;
                    if (salesCnt < 0)
                        salesCnt = 0;
                }

                //영업일 수
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
                DataTable eDt = productExcludedSalesRepository.GetExcludedSalesAsOne(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"), product[0], product[1], product[2], product[3], product[4], product[5], product[6]);
                DataRow[] eRow = null;
                double excluded_qty = 0;
                if (eDt != null && eDt.Rows.Count > 0)
                {
                    string whr = "product = '" + product[0] + "'"
                            + " AND origin = '" + product[1] + "'"
                            + " AND sizes = '" + product[2] + "'"
                            + " AND seaover_unit = '" + product[6] + "'";

                    eRow = eDt.Select(whr);
                    if (eRow.Length > 0)
                    {
                        if (!double.TryParse(eRow[0]["seaover_unit"].ToString(), out unit))
                            unit = 1;
                        excluded_qty += Convert.ToDouble(eRow[0]["sale_qty"].ToString()) * Convert.ToDouble(product[6]) / unit;
                    }
                    salesCnt -= excluded_qty;
                }
            }

            dgvStockSales.Rows[0].Cells["seaover_unpending_qty"].Value = total_unpending_qty.ToString("#,##0");
            dgvStockSales.Rows[0].Cells["seaover_pending_qty"].Value = total_pending_qty.ToString("#,##0");
            dgvStockSales.Rows[0].Cells["reserved_qty"].Value = total_reserved_qty.ToString("#,##0");

            double avg_day_sales = salesCnt / workDays;
            double avg_month_sales = avg_day_sales * 21;
            dgvStockSales.Rows[0].Cells["day_sales_qty_double"].Value = avg_day_sales;
            dgvStockSales.Rows[0].Cells["day_sales_qty"].Value = avg_day_sales.ToString("#,##0");

            //2023-06-02 소희대리님이 1개월 판매량은 굳이 나눌필요없이 그대로 반영요청
            if (salesMonth == 1)
                avg_month_sales = salesCnt;

            dgvStockSales.Rows[0].Cells["month_sales_qty"].Value = avg_month_sales.ToString("#,##0");

            //model 
            smModel.avg_sales_day = avg_day_sales;
            smModel.avg_sales_month = avg_month_sales;
            smModel.real_stock = stock;
            //smModel.real_stock = Convert.ToDouble(row.Cells["seaover_unpending"].Value.ToString()) + Convert.ToDouble(row.Cells["seaover_pending"].Value.ToString());           //최초 소진일자 계산
            DateTime exhausted_date;
            double exhausted_cnt;
            common.GetExhausedDateDayd(DateTime.Now, smModel.real_stock, smModel.avg_sales_day, 0, null, out exhausted_date, out exhausted_cnt);

            smModel.exhaust_date = exhausted_date.ToString("yyyy-MM-dd");
            enddate = DateTime.Now.AddDays(-1);
            smModel.enddate = enddate.ToString("yyyy-MM-dd");

            return smModel;
        }

        private void SetPendingList(ShortManagerModel smModel, DataGridViewRow row)
        {
            this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            SetData(smModel, row);
            GetUnpendingData(smModel);
            SetHeaderStyle();
            if (SortSetting())
            {
                CalculateStock(smModel);
            }
            RealStockCalculate();
            this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
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

                    dgvContract.Rows[n].Cells["etd"].Value = tb.Rows[i]["etd"].ToString();
                    dgvContract.Rows[n].Cells["eta"].Value = tb.Rows[i]["eta"].ToString();
                    dgvContract.Rows[n].Cells["warehousing_date"].Value = tb.Rows[i]["warehousing_date"].ToString();
                    dgvContract.Rows[n].Cells["pending_term"].Value = tb.Rows[i]["pending_term"].ToString();
                    dgvContract.Rows[n].Cells["ato_no"].Value = tb.Rows[i]["ato_no"].ToString();
                    /*dgvContract.Rows[n].Cells["contract_no"].Value = tb.Rows[i]["contract_no"].ToString();
                    dgvContract.Rows[n].Cells["bl_no"].Value = tb.Rows[i]["bl_no"].ToString();*/

                    DateTime warehousing_date = new DateTime();
                    if (DateTime.TryParse(tb.Rows[i]["warehousing_date"].ToString(), out warehousing_date))
                    {
                        //입고일자 계산
                        dgvContract.Rows[n].Cells["warehousing_date"].Value = warehousing_date.ToString("yyyy-MM-dd");
                        //소진일자 계산, 입고후 재고 계산
                        double avg_sales_day;
                        if (dgvStockSales.Rows[0].Cells["day_sales_qty_double"].Value == null || !double.TryParse(dgvStockSales.Rows[0].Cells["day_sales_qty_double"].Value.ToString(), out avg_sales_day))
                            avg_sales_day = 0;

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
                        double etd_offer_price;
                        if (!double.TryParse(tb.Rows[i]["etd_offer_price"].ToString(), out etd_offer_price))
                            etd_offer_price = 0;

                        //2024-01-03 대행인 경우 매출단가 + 2%, 2.5%
                        if (tb.Rows[i]["etd_offer_price"].ToString().Contains("AD")
                            || tb.Rows[i]["etd_offer_price"].ToString().Contains("DW")
                            || tb.Rows[i]["etd_offer_price"].ToString().Contains("OD")
                            || tb.Rows[i]["etd_offer_price"].ToString().Contains("HS"))
                            etd_offer_price *= 1.025;
                        else if (tb.Rows[i]["etd_offer_price"].ToString().Contains("JD"))
                            etd_offer_price *= 1.02;

                        dgvContract.Rows[n].Cells["etd_offer_price"].Value = etd_offer_price.ToString("#,##0.00");
                        double cost_price = CalculateEtdCostPrice(dgvContract.Rows[n].Cells["etd"].Value.ToString(), etd_offer_price);
                        dgvContract.Rows[n].Cells["etd_cost_price"].Value = cost_price.ToString("#,##0");
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
                        dgvStockSales.Rows[0].Cells["total_cost_price"].Value = etd_average_cost_price.ToString("#,##0");

                        //선적 원가
                        if (cost_price > 0 && current_qty > 0)
                        {
                            total_shipping_cost_price += cost_price * current_qty;
                            total_shipping_qty += current_qty;
                        }
                    }
                }

                //총합
                int m = dgvContract.Rows.Add();
                dgvContract.Rows[m].DefaultCellStyle.BackColor = Color.LightGray;
                dgvContract.Rows[m].DefaultCellStyle.Font = new Font("나눔고딕", 10, FontStyle.Bold);
                dgvContract.Rows[m].Cells["qty"].Value = total_qty.ToString("#,##0");
                double pending_cost_price = total_shipping_cost_price / total_shipping_qty;
                if (double.IsNaN(pending_cost_price))
                    pending_cost_price = 0;
                //txtPendingCostPrice.Text = pending_cost_price.ToString("#,##0");
                dgvStockSales.Rows[0].Cells["pending_cost"].Value = pending_cost_price.ToString("#,##0");
            }
            return true;
        }

        private void RealStockCalculate()
        {
            if (dgvStockSales.Rows.Count > 0)
            {
                //this.dgvStockSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
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
                //월판매
                double enable_sales_days = total_qty / day_sales_qty_double;
            
                //회전율
                double month_around = enable_sales_days / 21;

                //평균원가
                double avg_cost_price;
                if (dgvContract.Rows[dgvContract.Rows.Count - 1].Cells["etd_average_cost_price"].Value == null || !double.TryParse(dgvContract.Rows[dgvContract.Rows.Count - 1].Cells["etd_average_cost_price"].Value.ToString(), out avg_cost_price))
                    avg_cost_price = 0;

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
                row.Cells["month_sales_qty"].Value = (day_sales_qty_double * 21).ToString("#,##0");
                row.Cells["total_cost_price"].Value = avg_cost_price.ToString("#,##0");
                
                row.Cells["total_month_around"].Value = month_around.ToString("#,##0.00");
                //this.dgvStockSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStockSales_CellValueChanged);
            }
        }


        private void SetData(ShortManagerModel smModel, DataGridViewRow select_row)
        {
            //첫줄 추가=============================================================================================
            int n = dgvContract.Rows.Add();
            DataGridViewRow row = dgvContract.Rows[n];

            row.Cells["etd"].Value = "현재고";
            row.Cells["warehousing_date"].Value = DateTime.Now.ToString("yyyy-MM-dd");
            row.Cells["warehouse_qty"].Value = smModel.real_stock.ToString("#,##0");
            double month_around = smModel.real_stock / smModel.avg_sales_day / 21;
            if (double.IsNaN(month_around))
                month_around = 0;

            row.Cells["month_around"].Value = month_around.ToString("#,##0.00");

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
            row.Cells["after_qty_exhausted_date"].Value = exhaust_date.ToString("yyyy-MM-dd");
            //최소ETD
            DateTime etd = exhaust_date.AddDays(-(delivery_day + pending_day));
            row.Cells["recommend_etd"].Value = etd.ToString("yyyy-MM-dd");
            //추천 계약일자
            DateTime contract_date = exhaust_date.AddDays(-(production_day + delivery_day + pending_day));
            row.Cells["recommend_contract_date"].Value = contract_date.ToString("yyyy-MM-dd");

            //첫번째 씨오버원가
            double cost_price;
            ReplaceSalesCostToPendingCost(select_row, out cost_price);
            row.Cells["etd_cost_price"].Value = cost_price.ToString("#,##0");
            dgvStockSales.Rows[0].Cells["seaover_cost"].Value = cost_price.ToString("#,##0");
            dgvStockSales.Rows[0].Cells["seaover_cost"].ToolTipText = select_row.Cells["seaover_cost_price"].ToolTipText;
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
            DataTable unpendingTable = customsRepository.GetUnpendingProduct3(model.product, model.origin, model.sizes, model.unit, sub_product, isMerge, true);
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

                    //다를경우만 수정
                    if (select_unit != unpending_unit)
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
                    if (string.IsNullOrEmpty(bl_no) || !string.IsNullOrEmpty(bl_no) && string.IsNullOrEmpty(warehousing_date))
                    {
                        //재고, 판매량 한눈에===============================================================================================
                        if (dgvStockSales.Rows[0].Cells["shipment_qty"].Value == null || !double.TryParse(dgvStockSales.Rows[0].Cells["shipment_qty"].Value.ToString(), out shipment_qty))
                            shipment_qty = 0;
                        if (dgvStockSales.Rows[0].Cells["shipping_qty"].Value == null || !double.TryParse(dgvStockSales.Rows[0].Cells["shipping_qty"].Value.ToString(), out shipping_qty))
                            shipping_qty = 0;

                        //선적
                        if (string.IsNullOrEmpty(bl_no))
                            shipment_qty += Convert.ToDouble(unpendingTable.Rows[i]["quantity_on_paper"].ToString());
                        //배송
                        else
                            shipping_qty += Convert.ToDouble(unpendingTable.Rows[i]["quantity_on_paper"].ToString());
                        dgvStockSales.Rows[0].Cells["shipment_qty"].Value = shipment_qty.ToString("#,##0");
                        dgvStockSales.Rows[0].Cells["shipping_qty"].Value = shipping_qty.ToString("#,##0");

                        //통관일정===============================================================================================
                        DateTime warehouse_date;
                        if (DateTime.TryParse(unpendingTable.Rows[i]["eta"].ToString(), out warehouse_date))
                        {
                            int pending_day;
                            if (!int.TryParse(txtPendingTerm.Text, out pending_day))
                                pending_day = 0;
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

                            warehouse_date = warehouse_date.AddDays(shipment_day + pending_day);
                        }
                        else
                            warehouse_date = DateTime.Now;

                        if (warehouse_date <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                            warehouse_date = DateTime.Now.AddDays(5);
                        unpendingTable.Rows[i]["warehousing_date"] = warehouse_date.ToString("yyyy-MM-dd");

                        int n = dgvContract.Rows.Add();

                        DataGridViewRow row = dgvContract.Rows[n];
                        row.Cells["etd"].Value = unpendingTable.Rows[i]["etd"].ToString();
                        row.Cells["eta"].Value = unpendingTable.Rows[i]["eta"].ToString();
                        row.Cells["warehousing_date"].Value = unpendingTable.Rows[i]["warehousing_date"].ToString();
                        row.Cells["pending_term"].Value = Convert.ToInt16(txtShippingTerm.Text) + Convert.ToInt16(txtPendingTerm.Text);
                        row.Cells["qty"].Value = unpendingTable.Rows[i]["quantity_on_paper"].ToString();
                        row.Cells["ato_no"].Value = unpendingTable.Rows[i]["ato_no"].ToString();
                        row.Cells["etd_offer_price"].Value = Convert.ToDouble(unpendingTable.Rows[i]["unit_price"].ToString()).ToString("#,##0.00");

                        bool isWeight;
                        if (!bool.TryParse(unpendingTable.Rows[i]["weight_calculate"].ToString(), out isWeight))
                            isWeight = true;
                        row.Cells["pending_weight_calculate"].Value = isWeight;
                    }
                }
            }
        }
        //정렬하기
        private bool SortSetting()
        {
            DataTable tb = Libs.Tools.Common.GetDataGridViewAsDataTable2(dgvContract);
            if (tb.Rows.Count > 0)
            {
                tb.Columns.Add("warehousing_date_sort", typeof(DateTime)).SetOrdinal(1);
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    DateTime warehousing_date = new DateTime();
                    if (!DateTime.TryParse(tb.Rows[i]["warehousing_date"].ToString(), out warehousing_date))
                    {
                        warehousing_date = new DateTime(2999, 12, 31);
                    }
                    tb.Rows[i]["warehousing_date_sort"] = warehousing_date.ToString("yyyy-MM-dd");
                }
                //Sorting
                DataView tv = new DataView(tb);
                tv.Sort = "warehousing_date_sort";
                tb = tv.ToTable();
                //재출력
                dgvContract.Rows.Clear();
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    int n = dgvContract.Rows.Add();

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
                    
                    dgvContract.Rows[n].Cells["current_qty"].Value = tb.Rows[i]["current_qty"].ToString();
                    dgvContract.Rows[n].Cells["etd_offer_price"].Value = tb.Rows[i]["etd_offer_price"].ToString();
                    dgvContract.Rows[n].Cells["etd_cost_price"].Value = tb.Rows[i]["etd_cost_price"].ToString();
                    dgvContract.Rows[n].Cells["etd_average_cost_price"].Value = tb.Rows[i]["etd_average_cost_price"].ToString();
                }
                return true;
            }
            else
                return false;
        }
        //계산하기
        private bool CalculateStock(ShortManagerModel model)
        {
            dgvContract.EndEdit();
            double stock = Convert.ToInt32(model.real_stock);
            if (dgvContract.Rows[0].Cells["after_qty_exhausted_date"].Value.ToString() == "")
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

                dgvContract.Rows[n].Cells["etd"].Value = tb.Rows[i]["etd"].ToString();
                dgvContract.Rows[n].Cells["eta"].Value = tb.Rows[i]["eta"].ToString();
                dgvContract.Rows[n].Cells["warehousing_date"].Value = tb.Rows[i]["warehousing_date"].ToString();
                dgvContract.Rows[n].Cells["pending_term"].Value = tb.Rows[i]["pending_term"].ToString();
                dgvContract.Rows[n].Cells["ato_no"].Value = tb.Rows[i]["ato_no"].ToString();
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
                        dgvContract.Rows[n].Cells["exhausted_day_count"].Value = Days.ToString("#,##0");
                        if (Days > 0)
                            dgvContract.Rows[n].Cells["exhausted_date"].Value = exhaust_date.ToString("yyyy-MM-dd") + " ~ " + warehousing_date.AddDays(-1).ToString("yyyy-MM-dd");
                    }
                    //입고수량
                    double qty;
                    if (tb.Rows[i]["qty"] == null || !double.TryParse(tb.Rows[i]["qty"].ToString().Replace(",", ""), out qty))
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
                //계산방식
                dgvContract.Rows[n].Cells["pending_weight_calculate"].Value = Convert.ToBoolean(tb.Rows[i]["pending_weight_calculate"].ToString());


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
                        total_cost_price += cost_price * current_qty;
                        total_cost_qty += current_qty;
                    }
                }
                //두번째 행부터 원가계산, 평균원가 계산
                else
                {
                    double etd_offer_price;
                    if (!double.TryParse(tb.Rows[i]["etd_offer_price"].ToString(), out etd_offer_price))
                        etd_offer_price = 0;

                    dgvContract.Rows[n].Cells["etd_offer_price"].Value = etd_offer_price.ToString("#,##0.00");

                    bool isWeight;
                    if (dgvContract.Rows[n].Cells["pending_weight_calculate"].Value == null || !bool.TryParse(dgvContract.Rows[n].Cells["pending_weight_calculate"].Value.ToString(), out isWeight))
                        isWeight = true;

                    //2024-01-03 대행일 경우 매출단가 + 2.5%, 2%
                    if (tb.Rows[i]["ato_no"].ToString().Contains("AD")
                        || tb.Rows[i]["ato_no"].ToString().Contains("DW")
                        || tb.Rows[i]["ato_no"].ToString().Contains("OD")
                        || tb.Rows[i]["ato_no"].ToString().Contains("HS"))
                        etd_offer_price *= 1.025;
                    else if (tb.Rows[i]["ato_no"].ToString().Contains("JD"))
                        etd_offer_price *= 1.02;

                    //선적원가
                    double cost_price = CalculateEtdCostPrice(dgvContract.Rows[n].Cells["etd"].Value.ToString(), etd_offer_price, exchange_rate, isWeight);
                    dgvContract.Rows[n].Cells["etd_cost_price"].Value = cost_price.ToString("#,##0");
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
                }  
            }

            //총합
            int m = dgvContract.Rows.Add();
            dgvContract.Rows[m].DefaultCellStyle.BackColor = Color.LightGray;
            dgvContract.Rows[m].DefaultCellStyle.Font = new Font("나눔고딕", 10, FontStyle.Bold);
            dgvContract.Rows[m].Cells["qty"].Value = total_qty.ToString("#,##0");
            dgvContract.Rows[m].Cells["etd_average_cost_price"].Value = (total_cost_price / total_cost_qty).ToString("#,##0");

            double pending_cost_price = total_shipping_cost_price / total_shipping_qty;
            if (double.IsNaN(pending_cost_price))
                pending_cost_price = 0;
            //txtPendingCostPrice.Text = pending_cost_price.ToString("#,##0");
            dgvStockSales.Rows[0].Cells["pending_cost"].Value = pending_cost_price.ToString("#,##0");


            return true;
        }
        //선적원가계산
        private double CalculateEtdCostPrice(string etd, double offer_price)
        {
            double cost_price = 0;
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
            {
                double unit;
                if (row.Cells["unit"].Value == null || !double.TryParse(row.Cells["unit"].Value.ToString(), out unit))
                    unit = 0;
                double cost_unit;
                if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                    cost_unit = 0;
                double exchange_rate;
                if (row.Cells["exchange_rate"].Value == null || !double.TryParse(row.Cells["exchange_rate"].Value.ToString(), out exchange_rate))
                    exchange_rate = 0;
                bool weight_calculate;
                if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                    weight_calculate = true;
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

                
                if (weight_calculate)
                    cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate * unit;
                else
                    cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate * cost_unit;

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

            }
            return cost_price;
        }
        private double CalculateEtdCostPrice(string etd, double offer_price, double exchange_rate, bool weight_calculate)
        {
            double cost_price = 0;
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
            {
                double unit;
                if (row.Cells["unit"].Value == null || !double.TryParse(row.Cells["unit"].Value.ToString(), out unit))
                    unit = 0;
                double cost_unit;
                if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                    cost_unit = 0;
                if (cost_unit == 0)
                    cost_unit = 1;
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


                if (weight_calculate)
                    cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate * unit / cost_unit;
                else
                {
                    if (cbMultiplyTray.Checked)
                        cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate * cost_unit;
                    else
                        cost_price = offer_price * (1 + custom + tax + incidental_expense) * exchange_rate;
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
            }
            return cost_price;
        }

        //Datagridview Header style 
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

        #region 오퍼가 리스트, 평균원가 계산
        private void GetPurchasePriceList(DataGridViewRow row)
        {
            //유효성검사
            DateTime sttdate, enddate;
            if (!DateTime.TryParse(txtSttdate.Text, out sttdate) || !DateTime.TryParse(txtEnddate.Text, out enddate))
            {
                MessageBox.Show(this, "검색기간의 값을 다시 확인해주세요.");
                this.Activate();
                return;
            }
            //초기화
            cbPurchasePriceList.DataSource = null;
            txtOfferUpdatetime.Text = string.Empty;
            txtOfferCompany.Text = string.Empty;
            txtOfferPrice.Text = string.Empty;
            txtInQty.Text = "0";
            txtOfferCostPrice.Text = string.Empty;
            txtAverageCostPrice.Text = string.Empty;

            //데이터 출력
            DataTable list = purchasePriceRepository.GetPurchasePriceList(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString());

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
            if (cmbList.Count > 0)
            {
                cbPurchasePriceList.DataSource = cmbList;
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
            if (offer_cost_price > 0 & in_qty > 0)
            {
                total_cost_price += offer_cost_price * in_qty;
                total_qty += in_qty;
            }

            txtAverageCostPrice.Text = (total_cost_price / total_qty).ToString("#,##0");
            if (dgvStockSales.Rows.Count > 0)
                dgvStockSales.Rows[0].Cells["total_cost_price"].Value = (total_cost_price / total_qty).ToString("#,##0");
        }

        #endregion


        #region Datagridview event
        private void dgvPurchaseUsd_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    dgvPurchaseUsd.ClearSelection();
                    dgvPurchaseUsd.Rows[e.RowIndex].Selected = true;
                }
            }
        }
        private void dgvSales_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvSales.SelectedCells.Count > 0)
            {
                double sales_qty = 0, total_amount = 0, row_cnt = 0;
                double total_margin_rate = 0, total_margin_amount = 0;
                for (int i = 0; i < dgvSales.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvSales.Columns.Count; j++)
                    {
                        if (dgvSales.Rows[i].Cells[j].Selected)
                        {
                            double qty;
                            if (dgvSales.Rows[i].Cells["sales_qty"].Value == null || !double.TryParse(dgvSales.Rows[i].Cells["sales_qty"].Value.ToString(), out qty))
                                qty = 0;
                            double amount;
                            if (dgvSales.Rows[i].Cells["sales_price"].Value == null || !double.TryParse(dgvSales.Rows[i].Cells["sales_price"].Value.ToString(), out amount))
                                amount = 0;

                            double margin_rate;
                            if (dgvSales.Rows[i].Cells["margin"].Value == null || !double.TryParse(dgvSales.Rows[i].Cells["margin"].Value.ToString().Replace("%", ""), out margin_rate))
                                margin_rate = 0;

                            double margin_amount;
                            if (dgvSales.Rows[i].Cells["margin_amount"].Value == null || !double.TryParse(dgvSales.Rows[i].Cells["margin_amount"].Value.ToString(), out margin_amount))
                                margin_amount = 0;

                            sales_qty += qty;
                            total_amount += amount;
                            total_margin_rate += margin_rate;
                            total_margin_amount += margin_amount;
                            row_cnt++;
                            break;
                        }
                    }
                }

                //출력
                dgvDragData.Rows[0].Cells["sum"].Value = sales_qty.ToString("#,##0");
                dgvDragData.Rows[0].Cells["avg"].Value = (sales_qty / row_cnt).ToString("#,##0");

                dgvDragData.Rows[0].Cells["sum2"].Value = total_amount.ToString("#,##0");
                dgvDragData.Rows[0].Cells["avg2"].Value = (total_amount / row_cnt).ToString("#,##0");

                dgvDragData.Rows[1].Cells["sum"].Value = total_margin_rate.ToString("#,##0.00") + "%";
                dgvDragData.Rows[1].Cells["avg"].Value = (total_margin_rate / row_cnt).ToString("#,##0.00") + "%";

                dgvDragData.Rows[1].Cells["sum2"].Value = total_margin_amount.ToString("#,##0");
                dgvDragData.Rows[1].Cells["avg2"].Value = (total_margin_amount / row_cnt).ToString("#,##0");
            }
        }

        private void dgvMakePeriod_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
            {
                ContractRecommendationManager2 crm = new ContractRecommendationManager2(um, row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString());
                crm.Show();
            }
        }
        private void dgvStockSales_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            dgvStockSales.EndEdit();
            if (dgvStockSales.Rows.Count > 0)
            {
                if (dgvStockSales.Columns[e.ColumnIndex].Name == "day_sales_qty")
                {
                    double day_sales_qty;
                    if (dgvStockSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !double.TryParse(dgvStockSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out day_sales_qty))
                        day_sales_qty = 0;
                    dgvStockSales.Rows[e.RowIndex].Cells["day_sales_qty_double"].Value = day_sales_qty;
                }
                else if (dgvStockSales.Columns[e.ColumnIndex].Name == "month_sales_qty")
                {
                    double month_sales_qty;
                    if (dgvStockSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !double.TryParse(dgvStockSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out month_sales_qty))
                        month_sales_qty = 0;

                    double day_sales_qty = month_sales_qty / 21;
                    dgvStockSales.Rows[e.RowIndex].Cells["day_sales_qty"].Value = day_sales_qty.ToString("#,##0");
                    dgvStockSales.Rows[e.RowIndex].Cells["day_sales_qty_double"].Value = day_sales_qty;
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
                double total_qty = shipment_qty + shipping_qty + seaover_unpending_qty + seaover_pending_qty - reserved_qty + inQty;
                dgvStockSales.Rows[0].Cells["total_qty"].Value = total_qty.ToString("#,##0");
                //일판매
                double day_sales_qty_double;
                if (dgvStockSales.Rows[0].Cells["day_sales_qty_double"].Value == null || !double.TryParse(dgvStockSales.Rows[0].Cells["day_sales_qty_double"].Value.ToString(), out day_sales_qty_double))
                    day_sales_qty_double = 0;
                double enable_sale_days = total_qty / day_sales_qty_double;
                double month_around = enable_sale_days / 21;

                dgvStockSales.Rows[0].Cells["total_month_around"].Value = month_around.ToString("#,##0.00");
            }
        }
        private void dgvProduct_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgvProduct.ClearSelection();
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    dgvProduct.Rows[i].Cells["chk"].Value = false;
                    dgvProduct.Rows[i].Cells["chk"].Style.BackColor = Color.White;
                }
                dgvProduct.Rows[e.RowIndex].Cells["chk"].Value = true;
                dgvProduct.Rows[e.RowIndex].Cells["chk"].Style.BackColor = Color.Red;
            }
        }
        private void dgvProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if(dgvProduct.SelectedRows.Count <= 1)
                        dgvProduct.ClearSelection();
                    dgvProduct.Rows[e.RowIndex].Selected = true;
                }
            }
        }
        private void dgvProduct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                if (dgvProduct.Columns[e.ColumnIndex].Name == "chk")
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    if (isChecked)
                    {
                        GetProductInfo(dgvProduct.Rows[e.RowIndex]);
                        GetPurchase(dgvProduct.Rows[e.RowIndex], true);
                        GetOffer(dgvProduct.Rows[e.RowIndex]);
                        GetSales(dgvProduct.Rows[e.RowIndex]);
                        OpenExhaustedManager(dgvProduct.Rows[e.RowIndex]);
                        CalculateAverageCostPrice();
                    }
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "exchange_rate"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "seavoer_purchase_price"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "offer_price")
                {
                    calculateCostPrice(dgvProduct.Rows[e.RowIndex]);
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
        #endregion

        #region Button
        private void btnPrinting_Click(object sender, EventArgs e)
        {
            Printing();
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
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetCostAccounting();
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
        private void btnProductInfo_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
            {
                ContractRecommendationManager2 crm = new ContractRecommendationManager2(um, row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString());
                crm.Show();
            }
        }
        private void btnGetProductCode_Click(object sender, EventArgs e)
        {
            DataTable codeDt = seaoverRepository.GetProductCode("", "", "");
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
                GetProductCode(dgvProduct.Rows[i], codeDt);
        }
        private void btnExpansion_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
            {
                SalesDetail sd = new SalesDetail(um, row.Cells["product"].Value.ToString()
                                                , row.Cells["origin"].Value.ToString()
                                                , row.Cells["sizes"].Value.ToString()
                                                , row.Cells["unit"].Value.ToString());
                sd.Show();
            }
        }
        private void btnCalendarSttdate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
                txtSttdate.Text = sdate;
        }

        private void btnCalendarEnddate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
                txtEnddate.Text = sdate;
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
        
        private void btnProduct_Click(object sender, EventArgs e)
        {
            CostAccounting ca = new CostAccounting(um);
            ca.Show();
        }
        private void btnBookmark_Click(object sender, EventArgs e)
        {
            BookmarkManager bm = new BookmarkManager(um, this);
            bm.StartPosition = FormStartPosition.CenterParent;
            bm.ShowDialog();
        }
        #endregion

        #region Combox
        private void cbSaleTerm_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectProductDgvr();
            if (row != null)
            {
                GetProductStockInfo(row);
                OpenExhaustedManager(row);
            }
        }
        private void cbChtPurchaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvProduct.Rows.Count > 0)
            {
                DataGridViewRow row;
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value);
                    if (isChecked)
                    {
                        row = dgvProduct.Rows[i];
                        SetPurchaseChart(row);
                        break;
                    }
                }
            }
        }
        private void cbChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvProduct.Rows.Count > 0)
            {
                DataGridViewRow row;
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value);
                    if (isChecked)
                    {
                        row = dgvProduct.Rows[i];
                        SetPurchaseChart(row);
                        break;
                    }
                }
            }
        }

        private void cbChtPurchaseType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvProduct.Rows.Count > 0)
            {
                DataGridViewRow row;
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value);
                    if (isChecked)
                    {
                        row = dgvProduct.Rows[i];
                        SetOfferChart(row);
                        break;
                    }
                }
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

        private void cbOfferCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value))
                {
                    GetOffer(dgvProduct.Rows[i], false);
                    break;
                }
            }
        }
        private void cbShipment_CheckedChanged(object sender, EventArgs e)
        {
            RealStockCalculate();
        }

        private void cbPurchasePrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvProduct.Rows.Count > 0)
            {
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value))
                    {
                        GetOffer(dgvProduct.Rows[i]);
                        break;
                    }
                }
            }
        }

        private void cbSalesChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvProduct.Rows.Count > 0)
            {
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value))
                    {
                        GetSales(dgvProduct.Rows[i]);
                        break;
                    }
                }
            }
        }
        #endregion

        #region Key event
        private void txtCustom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                CalculateStock();
                this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            }
        }
        private void txtOfferSttdate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DateTime sttDt, endDt;
                if (DateTime.TryParse(txtOfferSttdate.Text, out sttDt) && DateTime.TryParse(txtOfferEnddate.Text, out endDt))
                {
                    txtOfferSttdate.Text = sttDt.ToString("yyyy-MM-dd");
                    txtOfferEnddate.Text = endDt.ToString("yyyy-MM-dd");
                    GetPurchasePriceList(GetSelectProductDgvr());
                }
            }
        }
        private void txtOfferPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                double offer_price;
                if (!double.TryParse(txtOfferPrice.Text, out offer_price))
                    offer_price = 0;
                double exchange_rate;
                if (!double.TryParse(txtExchangeRate2.Text, out exchange_rate))
                    exchange_rate = 0;
                txtOfferCostPrice.Text = calculateCostPrice(offer_price, exchange_rate).ToString("#,##0");
                CalculateAverageCostPrice();


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
                this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                SetPendingList(smModel, row);
            }
        }

        private void txtInQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CalculateAverageCostPrice();
            }
        }
        private void txtExchangeRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (dgvProduct.Rows.Count > 0)
                {
                    double exchange_rate;
                    if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                        exchange_rate = 0;
                    txtExchangeRate.Text = exchange_rate.ToString("#,##0");
                    for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    {
                        dgvProduct.Rows[i].Cells["exchange_rate"].Value = exchange_rate.ToString("#,##0");
                        calculateCostPrice(dgvProduct.Rows[i]);
                    }
                }
            }
        }

        private void txtSttdate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox tbb = (TextBox)sender;
                tbb.Text = common.strDatetime(tbb.Text);
                if (tbb.Name != "txtSttdate" && tbb.Name != "txtEnddate")
                {
                    DateTime dt;
                    if (DateTime.TryParse(tbb.Text, out dt))
                        tbb.Text = dt.ToString("yyyy-MM-dd");
                }
            }
        }
        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetCostAccounting();
        }

        private void GetCostAccounting()
        {
            if (txtProduct.Text.Trim() == string.Empty
                && txtProduct2.Text.Trim() == string.Empty
                && txtOrigin.Text.Trim() == string.Empty
                && txtSizes.Text.Trim() == string.Empty
                && txtSizes2.Text.Trim() == string.Empty
                && txtUnit.Text.Trim() == string.Empty
                && txtCompany.Text.Trim() == string.Empty
                && txtManager.Text.Trim() == string.Empty)
            {
                MessageBox.Show(this, "검색항목이 전부 없을 경우 검색할 수 없습니다.");
                this.Activate();
                return;
            }
            DateTime sttdate, enddate;
            if(!DateTime.TryParse(txtSttdate.Text, out sttdate) || !DateTime.TryParse(txtEnddate.Text, out enddate))
            {
                MessageBox.Show(this, "검색기간의 값을 다시 확인해주세요.");
                this.Activate();
                return;
            }

            dgvProduct.Rows.Clear();
            //단가정보 
            DataTable pDt = purchasePriceRepository.GetCostAccounting(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                                                                , txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text
                                                                , txtCompany.Text, cbExactly.Checked, txtManager.Text
                                                                , 1, false, cbCurrentPrice.Checked);

            if (pDt.Rows.Count > 0)
            {
                string whr;
                List<string> mergeList = new List<string>();
                double unit;
                int n;
                //대표품목
                DataTable pgDt = productGroupRepository.GetProductGroup(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
                //데이터 출력
                for (int i = 0; i < pDt.Rows.Count; i++)
                {
                    string merge_product = "";
                    bool is_merge = false;
                    if (pgDt.Rows.Count > 0)
                    {
                        //1.대표품목으로 등록되있는지 확인
                        whr = "product = '" + pDt.Rows[i]["product"].ToString() + "'"
                            + " AND origin = '" + pDt.Rows[i]["origin"].ToString() + "'"
                            + " AND sizes = '" + pDt.Rows[i]["sizes"].ToString() + "'"
                            + " AND seaover_unit = '" + pDt.Rows[i]["unit"].ToString() + "'";

                        DataRow[] dtRow = null;
                        dtRow = pgDt.Select(whr);
                        if (dtRow.Length > 0)
                        {
                            string main_code = dtRow[0]["product"].ToString()
                                        + "^" + dtRow[0]["origin"].ToString()
                                        + "^" + dtRow[0]["sizes"].ToString()
                                        + "^" + dtRow[0]["unit"].ToString()
                                        + "^" + dtRow[0]["price_unit"].ToString()
                                        + "^" + dtRow[0]["unit_count"].ToString()
                                        + "^" + dtRow[0]["seaover_unit"].ToString();
                            //2.메인품목이 출력됬는지 확인
                            /*if (!mergeList.Contains(main_code))
                            {*/
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
                                        is_merge = true;
                                    }
                                }
                                if (!string.IsNullOrEmpty(main_code))
                                    mergeList.Add(main_code);
                            /*}*/
                        }
                    }
                    n = dgvProduct.Rows.Add();
                    dgvProduct.Rows[n].Cells["product"].Value = pDt.Rows[i]["product"];
                    dgvProduct.Rows[n].Cells["origin"].Value = pDt.Rows[i]["origin"];
                    dgvProduct.Rows[n].Cells["sizes"].Value = pDt.Rows[i]["sizes"];
                    dgvProduct.Rows[n].Cells["unit"].Value = pDt.Rows[i]["unit"];
                    dgvProduct.Rows[n].Cells["cost_unit"].Value = pDt.Rows[i]["cost_unit"];

                    dgvProduct.Rows[n].Cells["updatetime"].Value = Convert.ToDateTime(pDt.Rows[i]["updatetime"]).ToString("yyyy-MM-dd");
                    dgvProduct.Rows[n].Cells["offer_price"].Value = pDt.Rows[i]["purchase_price"];
                    dgvProduct.Rows[n].Cells["shipper"].Value = pDt.Rows[i]["cname"];
                    dgvProduct.Rows[n].Cells["exchange_rate"].Value = txtExchangeRate.Text;
                    dgvProduct.Rows[n].Cells["custom"].Value = pDt.Rows[i]["custom"];
                    dgvProduct.Rows[n].Cells["tax"].Value = pDt.Rows[i]["tax"];
                    dgvProduct.Rows[n].Cells["incidental_expense"].Value = pDt.Rows[i]["incidental_expense"];
                    dgvProduct.Rows[n].Cells["fixed_tariff"].Value = pDt.Rows[i]["fixed_tariff"];
                    dgvProduct.Rows[n].Cells["production_days"].Value = pDt.Rows[i]["production_days"];
                    dgvProduct.Rows[n].Cells["purchase_margin"].Value = pDt.Rows[i]["purchase_margin"];
                    dgvProduct.Rows[n].Cells["manager"].Value = pDt.Rows[i]["manager"];

                    bool weight_calculate = Convert.ToBoolean(pDt.Rows[i]["weight_calculate"].ToString());
                    bool tray_calculate = Convert.ToBoolean(pDt.Rows[i]["tray_calculate"].ToString());
                    if (weight_calculate == tray_calculate)
                        dgvProduct.Rows[n].Cells["weight_calculate"].Value = true;
                    else
                        dgvProduct.Rows[n].Cells["weight_calculate"].Value = weight_calculate;

                    //병합여부
                    dgvProduct.Rows[n].Cells["is_merge"].Value = is_merge;
                    dgvProduct.Rows[n].Cells["merge_product"].Value = merge_product.Trim();
                }
            }
            SetPrice();
        }
        private void DetailDashboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        GetCostAccounting();    
                        break;
                    case Keys.M:
                        txtProduct.Focus();
                        break;
                    case Keys.N:
                        txtProduct.Text = String.Empty;
                        txtProduct2.Text = String.Empty;
                        txtOrigin.Text = String.Empty;
                        txtSizes.Text = String.Empty;
                        txtSizes2.Text = String.Empty;
                        txtUnit.Text = String.Empty;
                        txtCompany.Text = String.Empty;
                        txtProduct.Focus();
                        break;
                    case Keys.X:
                        this.Dispose();
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
                        cbCurrentPrice.Checked = !cbCurrentPrice.Checked;
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
        

        private void txtPurchaseMargin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double purchase_margin;
                if (!double.TryParse(txtPurchaseMargin.Text, out purchase_margin))
                {
                    MessageBox.Show(this, "매입마진의 값이 숫자형식이 아닙니다.");
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

        #endregion

        #region 재고 및 회전율 조회 event
        public void InputCostAccountingData(List<DataGridViewRow> list)
        {
            clipboardList = list;
        }
        public void inputProduct()
        {
            if (clipboardList != null && clipboardList.Count > 0)
            {
                double exchange_rate;
                if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                    exchange_rate = 0;

                this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                dgvProduct.AutoPaste(false);
                dgvProduct.EndEdit();
                for (int i = 0; i < clipboardList.Count; i++)
                {
                    DataGridViewRow copyRow = clipboardList[i];
                    int n = dgvProduct.Rows.Add();
                    DataGridViewRow row = this.dgvProduct.Rows[n];
                    row.Cells["chk"].Value = false;
                    row.Cells["product"].Value = copyRow.Cells["product"].Value.ToString();
                    row.Cells["origin"].Value = copyRow.Cells["origin"].Value.ToString();
                    row.Cells["sizes"].Value = copyRow.Cells["sizes"].Value.ToString();
                    row.Cells["unit"].Value = copyRow.Cells["box_weight"].Value.ToString();
                    row.Cells["cost_unit"].Value = copyRow.Cells["cost_unit"].Value.ToString();
                    row.Cells["weight_calculate"].Value = Convert.ToBoolean(copyRow.Cells["weight_calculate"].Value.ToString());

                    row.Cells["tax"].Value = copyRow.Cells["tax"].Value.ToString();
                    row.Cells["custom"].Value = copyRow.Cells["custom"].Value.ToString();
                    row.Cells["incidental_expense"].Value = copyRow.Cells["incidental_expense"].Value.ToString();
                    row.Cells["fixed_tariff"].Value = copyRow.Cells["fixed_tariff"].Value.ToString();
                    row.Cells["exchange_rate"].Value = exchange_rate.ToString("#,##0.00");

                    row.Cells["updatetime"].Value = copyRow.Cells["updatetime"].Value.ToString();
                    row.Cells["offer_price"].Value = copyRow.Cells["unit_price"].Value.ToString();
                    row.Cells["cost_price"].Value = copyRow.Cells["cost_price"].Value.ToString();
                    row.Cells["seaover_purchsae_price"].Value = copyRow.Cells["purchase_price1"].Value.ToString();
                    row.Cells["shipper"].Value = copyRow.Cells["company"].Value.ToString();
                    row.Cells["margin_rate"].Value = copyRow.Cells["margin_rate"].Value.ToString();

                    string[] col = new string[3];
                    col[0] = "product";
                    col[1] = "origin";
                    col[2] = "sizes";
                    string[] val = new string[3];
                    val[0] = "'" + copyRow.Cells["product"].Value.ToString() + "'";
                    val[1] = "'" + copyRow.Cells["origin"].Value.ToString() + "'";
                    val[2] = "'" + copyRow.Cells["sizes"].Value.ToString() + "'";
                    DataTable productotherDt = commonRepository.SelectData("IFNULL(production_days, 20) AS production_days, IFNULL(purchase_margin, 0) AS purchase_margin, manager", "t_product_other_cost", col, val);
                    if (productotherDt.Rows.Count > 0)
                    {
                        row.Cells["production_days"].Value = Convert.ToDouble(productotherDt.Rows[0]["production_days"].ToString());
                        row.Cells["purchase_margin"].Value = Convert.ToDouble(productotherDt.Rows[0]["purchase_margin"].ToString());
                        row.Cells["manager"].Value = productotherDt.Rows[0]["manager"].ToString();

                        for (int j = 0; j < productotherDt.Rows.Count; j++)
                        {
                            double production_days;
                            if (!double.TryParse(productotherDt.Rows[j]["production_days"].ToString(), out production_days))
                                production_days = 0;
                            if(production_days > 0)
                                row.Cells["production_days"].Value = production_days.ToString();
                            double purchase_margin;
                            if (!double.TryParse(productotherDt.Rows[j]["purchase_margin"].ToString(), out purchase_margin))
                                purchase_margin = 0;
                            if (purchase_margin > 0)
                                row.Cells["purchase_margin"].Value = purchase_margin.ToString();

                            if(!string.IsNullOrEmpty(productotherDt.Rows[j]["manager"].ToString().Trim()))
                                row.Cells["manager"].Value = productotherDt.Rows[j]["manager"].ToString();
                        }
                    }
                    else
                    {
                        row.Cells["production_days"].Value = 20;
                        row.Cells["purchase_margin"].Value = 0;
                        row.Cells["manager"].Value = "";
                    }


                }
                //dgvProduct.AutoPaste();
                this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                SetPrice();
                
            }
        }
        //key event
        private void dgvProduct_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                { 
                    case Keys.V:
                        inputProduct();
                        break;
                }
            }
        }
        //Value changed 재계산
        private void dgvContract_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 & e.ColumnIndex >= 0)
            {
                this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
                if (dgvContract.Columns[e.ColumnIndex].Name == "etd"
                    || dgvContract.Columns[e.ColumnIndex].Name == "pending_term"
                    || dgvContract.Columns[e.ColumnIndex].Name == "warehouseing_date"
                    || dgvContract.Columns[e.ColumnIndex].Name == "qty"
                    || dgvContract.Columns[e.ColumnIndex].Name == "etd_offer_price")
                {
                    if (dgvContract.Rows[e.RowIndex].Cells["etd"].Value != null && dgvContract.Rows[e.RowIndex].Cells["pending_term"].Value != null)
                    {
                        int pending_day;
                        if (int.TryParse(dgvContract.Rows[e.RowIndex].Cells["pending_term"].Value.ToString(), out pending_day))
                        {
                            DateTime dt;
                            if (DateTime.TryParse(dgvContract.Rows[e.RowIndex].Cells[0].Value.ToString(), out dt))
                            {
                                dt = dt.AddDays(pending_day);
                                dgvContract.Rows[e.RowIndex].Cells["warehousing_date"].Value = dt.ToString("yyyy-MM-dd");
                            }
                        }
                    }
                    CalculateStock();
                }
                else
                    CalculateStock();
                this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            }
        }
        #endregion

        #region 매입마진 ContextMenu
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
                ToolStripMenuItem item02 = new ToolStripMenuItem("매입마진으로 설정하기");
                item02.Click += SettingPurchaseMargin;
            

                menu.Items.Add(item03);
                menu.Items.Add(item01);
                menu.Items.Add(item02);
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
                    bool isWeight = Convert.ToBoolean(row.Cells["weight_calculate"].Value);
                    if (!isWeight && cost_unit == 0)
                        isWeight = true;
                    //오퍼내역
                    //DataTable offerDt = purchasePriceRepository.GetPurhcasePriceAverage(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString(), "");
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
                {
                    MessageBox.Show(this,"등록중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    MessageBox.Show(this, "등록완료.");
                    this.Activate();
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
                    bool isWeight = Convert.ToBoolean(row.Cells["weight_calculate"].Value);
                    if (!isWeight && cost_unit == 0)
                        isWeight = true;
                    //오퍼내역
                    //DataTable offerDt = purchasePriceRepository.GetPurhcasePriceAverage(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString(), "");
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
        #endregion

        #region 매입가 우클릭 메뉴
        private void dgvPurchaseUsd_MouseUp(object sender, MouseEventArgs e)
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
                    if (dgvPurchaseUsd.SelectedRows.Count > 0)
                    {
                        ContextMenuStrip m = new ContextMenuStrip();
                        m.Items.Add("매입내역");
                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                        //Create 
                        m.BackColor = Color.White;
                        m.Show(dgvPurchaseUsd, e.Location);
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
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (dgvPurchaseUsd.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow dr = dgvPurchaseUsd.SelectedRows[0];
                    if (dr.Index < 0)
                        return;
                    //Function
                    switch (e.ClickedItem.Text)
                    {
                        case "매입내역":

                            DataGridViewRow row = GetSelectProductDgvr();

                            if (row != null)
                            {
                                PurchasePriceDetail ppd = new PurchasePriceDetail(um, dr.Cells["purchase_date"].Value.ToString()
                                    , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString()
                                    , Convert.ToDouble(txtPurchaseMargin.Text));
                                ppd.Show();
                            }
                            break;
                    }
                }
                catch
                {
                }
            }
        }
        #endregion

        #region 품목 우클릭 메뉴
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
                        m.Items.Add("삭제");
                        ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
                        toolStripSeparator.Name = "toolStripSeparator";
                        toolStripSeparator.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator);
                        m.Items.Add("같은 품목의 다른 거래처");
                        m.Items.Add("같은 거래처의 다른 품목");
                        m.Items.Add("같은 품목의 다른 규격");
                        m.Items.Add("매입내역 매트릭스 조회");
                        m.Items.Add("매입내역 매트릭스 조회2");
                        ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
                        toolStripSeparator1.Name = "toolStripSeparator";
                        toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator1);
                        m.Items.Add("최신단가로 변경");
                        ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
                        toolStripSeparator2.Name = "toolStripSeparator";
                        toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator2);
                        m.Items.Add("즐겨찾기 등록");
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
        void m_ItemClicked2(object sender, ToolStripItemClickedEventArgs e)
        {
            if (dgvProduct.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow row = GetSelectProductDgvr();
                    DataGridViewRow dr = dgvProduct.SelectedRows[0];
                    CostAccounting ca;
                    if (dr.Index < 0)
                        return;
                    //Function
                    switch (e.ClickedItem.Text)
                    {
                        case "삭제":
                            for (int i = dgvProduct.Rows.Count - 1; i >= 0; i--)
                            {
                                if (dgvProduct.Rows[i].Selected)
                                    dgvProduct.Rows.Remove(dgvProduct.Rows[i]);
                            }
                                   
                            break;

                        case "같은 품목의 다른 거래처":
                            ca = new CostAccounting(um, null, dr.Cells["product"].Value.ToString()
                                                                            , dr.Cells["origin"].Value.ToString()
                                                                            , dr.Cells["sizes"].Value.ToString());
                            ca.Show();
                            ca.GetProduct();
                            break;
                        case "같은 거래처의 다른 품목":
                            ca = new CostAccounting(um, null, ""
                                                            , ""
                                                            , ""
                                                            , dr.Cells["shipper"].Value.ToString(), true);
                            ca.Show();
                            ca.GetProduct();
                            break;
                        case "같은 품목의 다른 규격":
                            ca = new CostAccounting(um, null, dr.Cells["product"].Value.ToString()
                                                            , ""
                                                            , ""
                                                            , dr.Cells["shipper"].Value.ToString(), true);
                            ca.Show();
                            ca.GetProduct();
                            break;
                        case "매입내역 매트릭스 조회":

                            if (dgvProduct.Rows.Count > 0)
                            {
                                Dictionary<string, int> productDic = new Dictionary<string, int>();
                                List<string[]> productList = new List<string[]>();
                                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                                {
                                    if (dgvProduct.Rows[i].Selected)
                                    {
                                        //품목리스트
                                        string[] product = new string[4];
                                        product[0] = dgvProduct.Rows[i].Cells["product"].Value.ToString();
                                        product[1] = dgvProduct.Rows[i].Cells["origin"].Value.ToString();
                                        product[2] = dgvProduct.Rows[i].Cells["sizes"].Value.ToString();
                                        product[3] = dgvProduct.Rows[i].Cells["unit"].Value.ToString();

                                        string productKey = product[0] + "^" + product[1] + "^" + product[2] + "^" + product[3];

                                        bool keyExists = productDic.ContainsKey(productKey);
                                        if (!keyExists)
                                        {
                                            productDic.Add(productKey, 0);
                                            productList.Add(product);
                                        }
                                    }
                                }
                                if(productList.Count > 0)
                                {
                                    PurchaseDashboard ppd = new PurchaseDashboard(um, productList);
                                    ppd.GetData();
                                    ppd.Show();
                                }
                            }
                            
                            break;
                        case "매입내역 매트릭스 조회2":
                            if (dgvProduct.Rows.Count > 0)
                            {
                                Dictionary<string, int> productDic = new Dictionary<string, int>();
                                List<string[]> productList = new List<string[]>();
                                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                                {
                                    if (dgvProduct.Rows[i].Selected)
                                    {
                                        //품목리스트
                                        string[] product = new string[4];
                                        product[0] = dgvProduct.Rows[i].Cells["product"].Value.ToString();
                                        product[1] = dgvProduct.Rows[i].Cells["origin"].Value.ToString();
                                        product[2] = dgvProduct.Rows[i].Cells["sizes"].Value.ToString();
                                        product[3] = dgvProduct.Rows[i].Cells["unit"].Value.ToString();

                                        string productKey = product[0] + "^" + product[1] + "^" + product[2] + "^" + product[3];

                                        bool keyExists = productDic.ContainsKey(productKey);
                                        if (!keyExists)
                                        {
                                            productDic.Add(productKey, 0);
                                            productList.Add(product);
                                        }
                                    }
                                }
                                if (productList.Count > 0)
                                {
                                    PurchaseDashboard2 ppd2 = new PurchaseDashboard2(um, productList);
                                    ppd2.GetData();
                                    ppd2.Show();
                                }
                            }
                            break;
                        case "최신단가로 변경":
                            {
                                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                                {
                                    if (dgvProduct.Rows[i].Selected)
                                        GetCurrentPrice(dgvProduct.Rows[i]);
                                }
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
                                        gModel.unit = dgvRow.Cells["unit"].Value.ToString();
                                        gModel.cost_unit = dgvRow.Cells["cost_unit"].Value.ToString();
                                        gModel.month_around = 0;

                                        gModel.offer_price = Convert.ToDouble(dgvRow.Cells["offer_price"].Value.ToString());
                                        gModel.offer_cost_price = Convert.ToDouble(dgvRow.Cells["cost_price"].Value.ToString());
                                        gModel.offer_company = dgvRow.Cells["shipper"].Value.ToString();
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
                    }
                }
                catch
                {
                }
            }
        }
        #endregion

        #region change event
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
        #endregion

        #region Datagridview 정렬
        private void dgvSales_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            DataGridView dgv = dgvSales;
            int i = e.Column.Index;
            if (dgv.Columns[i].Name != "sale_date")
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
            if (dgv.Columns[i].Name != "offer_company" && dgv.Columns[i].Name != "offer_date")
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
                if (!common.CheckAuthority(authorityDt, "수입관리", "원가 및 재고 대시보드", "is_print"))
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
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }
        #endregion

        private void DetailDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            SetWindowDisplayAffinity(this.Handle, WDA_NONE);
        }

        private void btnAddDomestic_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectProductDgvr();
            if (row == null || dgvContract.Rows.Count == 0)
                return;
            this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            dgvContract.Rows.Insert(dgvContract.Rows.Count - 1, 1);
            dgvContract.Rows[dgvContract.Rows.Count - 2].Cells["warehousing_date"].Value = dgvContract.Rows[dgvContract.Rows.Count - 3].Cells["after_qty_exhausted_date"].Value;
            dgvContract.Rows[dgvContract.Rows.Count - 2].Cells["pending_term"].Value = dgvContract.Rows[dgvContract.Rows.Count - 3].Cells["pending_term"].Value;
            dgvContract.Rows.Remove(dgvContract.Rows[dgvContract.Rows.Count - 1]);

            ShortManagerModel smModel = GetProductStockInfo(GetSelectProductDgvr());
            if (SortSetting())
                CalculateStock(smModel);
            this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
        }

        private void cbMultiplyTray_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvProduct.Rows.Count > 0)
            {
                double exchange_rate;
                if (!double.TryParse(txtExchangeRate.Text, out exchange_rate))
                    exchange_rate = 0;
                txtExchangeRate.Text = exchange_rate.ToString("#,##0");
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    dgvProduct.Rows[i].Cells["exchange_rate"].Value = exchange_rate.ToString("#,##0");
                    calculateCostPrice(dgvProduct.Rows[i]);
                }
            }
        }
    }
}


