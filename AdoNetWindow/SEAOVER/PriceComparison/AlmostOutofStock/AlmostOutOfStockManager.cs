using AdoNetWindow.SaleManagement.SalesManagerModule;
using AdoNetWindow.SaleManagement;
using MySqlX.XDevAPI.Common;
using Repositories.Company;
using Repositories;
using Repositories.SEAOVER.PriceComparison;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Libs.Tools;
using System.Net;
using System.Runtime.InteropServices;
using AdoNetWindow.Model;
using Org.BouncyCastle.Utilities;
using System.Runtime.InteropServices.ComTypes;
using Repositories.SEAOVER.Purchase;
using AdoNetWindow.CalendarModule;
using AdoNetWindow.DashboardForSales.MultiDashboard;

namespace AdoNetWindow.SEAOVER.PriceComparison.AlmostOutofStock
{
    public partial class AlmostOutOfStockManager : Form
    {
        IPriceComparisonTempRepository priceComparisonTempRepository = new PriceComparisonTempRepository();
        ICommonRepository commonRepository = new CommonRepository();
        IPurchaseRepository purchaseRepository = new PurchaseRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        DataTable yesterdayDt;
        calendar cal;
        UsersModel um;
        DataTable mainDt = new DataTable();
        public AlmostOutOfStockManager(calendar cal, UsersModel um)
        {
            InitializeComponent();
            this.um = um;
            this.cal = cal;
            txtManager.Text = um.user_name;
            yesterdayDt = priceComparisonTempRepository.GetPrieComparisonTempData(DateTime.Now.AddDays(-1), 20);
        }
        private void AlmostOutOfStockManager_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn col in dgvProduct.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        #region Method
        public void CalendarOpenAlarm(bool isMsg = false)
        {
            //SetColumnHeaderStyleSetting();
            tabDgv.SelectedIndex = 0;
            int cnt = SetNewOutProductDataTable();
            if (cnt > 0)
            {
                this.Show();
                this.Owner = cal;
            }

            else if (isMsg)
            {
                MessageBox.Show(this, "임박 품목내역이 없습니다.");
                this.Activate();
            }
        }
        private int SetNewOutProductDataTable()
        {
            dgvProduct.DataSource = null;
            //임박 기준일자(판매가능일)
            int limitDays = (int)nudLimitDays.Value;
            if (rbNotLimitDaYs.Checked)
                limitDays = 0;
            //오늘 기준데이터
            DataTable todayDt = priceComparisonTempRepository.GetPrieComparisonTempData(DateTime.Now, limitDays
                                                                        , txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text
                                                                        , txtDivision.Text, txtManager.Text, txtContetns.Text, txtRemark.Text);
            // LINQ를 사용하여 어제 기준 새로나온 품목만 정의
            DataTable newDt = todayDt.Clone();
            var result = from rowA in todayDt.AsEnumerable()
                         join rowB in yesterdayDt.AsEnumerable() on rowA.Field<string>("product_code") equals rowB.Field<string>("product_code") into gj
                         from subRow in gj.DefaultIfEmpty()
                         where subRow == null
                         select rowA;
            if (result != null)
            {
                // 결과 출력
                foreach (DataRow row in result)
                {
                    newDt.ImportRow(row);
                    Console.WriteLine("ID: " + row["product"] + ", Name: " + row["origin"]);
                }
            }

            //다시 new만 붙임
            var query = from row1 in todayDt.AsEnumerable()
                        join row2 in result.AsEnumerable() on row1.Field<string>("product_code") equals row2.Field<string>("product_code") into gj
                        from subRow in gj.DefaultIfEmpty()
                        select new
                        {
                            is_new = subRow != null ? "new" : "",
                            product_code = row1.Field<string>("product_code"),
                            product_code2 = row1.Field<string>("product_code2"),
                            product = row1.Field<string>("product"),
                            origin = row1.Field<string>("origin"),
                            sizes = row1.Field<string>("sizes"),
                            unit = row1.Field<string>("unit"),
                            price_unit = row1.Field<string>("price_unit"),
                            unit_count = row1.Field<string>("unit_count"),
                            seaover_unit = row1.Field<string>("seaover_unit"),
                            stock1 = row1.Field<double>("stock1"),
                            shipment_qty = row1.Field<double>("shipment_qty"),
                            unpending_qty_before = row1.Field<double>("unpending_qty_before"),
                            stock2 = row1.Field<double>("stock2"),
                            seaover_unpending = row1.Field<double>("seaover_unpending"),
                            seaover_pending = row1.Field<double>("seaover_pending"),
                            reserved_stock = row1.Field<double>("reserved_stock"),
                            stock3 = row1.Field<double>("stock3"),
                            min_sales_count = row1.Field<double>("min_sales_count"),
                            
                            sales_count_month1 = row1.Field<double>("sales_count_month1"),
                            sales_count_month45 = row1.Field<double>("sales_count_month45"),
                            sales_count_month2 = row1.Field<double>("sales_count_month2"),
                            sales_count_month3 = row1.Field<double>("sales_count_month3"),
                            sales_count_month6 = row1.Field<double>("sales_count_month6"),
                            sales_count_month12 = row1.Field<double>("sales_count_month12"),
                            sales_count_month18 = row1.Field<double>("sales_count_month18"),

                            sales_count_day1 = row1.Field<double>("sales_count_day1"),
                            sales_count_day45 = row1.Field<double>("sales_count_day45"),
                            sales_count_day2 = row1.Field<double>("sales_count_day2"),
                            sales_count_day3 = row1.Field<double>("sales_count_day3"),
                            sales_count_day6 = row1.Field<double>("sales_count_day6"),
                            sales_count_day12 = row1.Field<double>("sales_count_day12"),
                            sales_count_day18 = row1.Field<double>("sales_count_day18"),

                            sales_count1 = row1.Field<double>("sales_count1"),
                            sales_count45 = row1.Field<double>("sales_count45"),
                            sales_count2 = row1.Field<double>("sales_count2"),
                            sales_count3 = row1.Field<double>("sales_count3"),
                            sales_count6 = row1.Field<double>("sales_count6"),
                            sales_count12 = row1.Field<double>("sales_count12"),
                            sales_count18 = row1.Field<double>("sales_count18"),

                            min_enable_sales_days = row1.Field<double>("min_enable_sales_days"),
                            enable_sales_days1 = row1.Field<double>("enable_sales_days1"),
                            enable_sales_days45 = row1.Field<double>("enable_sales_days45"),
                            enable_sales_days2 = row1.Field<double>("enable_sales_days2"),
                            enable_sales_days3 = row1.Field<double>("enable_sales_days3"),
                            enable_sales_days6 = row1.Field<double>("enable_sales_days6"),
                            enable_sales_days12 = row1.Field<double>("enable_sales_days12"),
                            enable_sales_days18 = row1.Field<double>("enable_sales_days18"),
                            min_exhausted_date = row1.Field<double>("min_sales_count"),

                            exhausted_date1 = row1.Field<string>("exhausted_date1"),
                            exhausted_date45 = row1.Field<string>("exhausted_date45"),
                            exhausted_date2 = row1.Field<string>("exhausted_date2"),
                            exhausted_date3 = row1.Field<string>("exhausted_date3"),
                            exhausted_date6 = row1.Field<string>("exhausted_date6"),
                            exhausted_date12 = row1.Field<string>("exhausted_date12"),
                            exhausted_date18 = row1.Field<string>("exhausted_date18"),

                            exhausted_date_until_days1 = row1.Field<string>("exhausted_date_until_days1"),
                            exhausted_date_until_days45 = row1.Field<string>("exhausted_date_until_days45"),
                            exhausted_date_until_days2 = row1.Field<string>("exhausted_date_until_days2"),
                            exhausted_date_until_days3 = row1.Field<string>("exhausted_date_until_days3"),
                            exhausted_date_until_days6 = row1.Field<string>("exhausted_date_until_days6"),
                            exhausted_date_until_days12 = row1.Field<string>("exhausted_date_until_days12"),
                            exhausted_date_until_days18 = row1.Field<string>("exhausted_date_until_days18"),
                            
                            etd1 = row1.Field<string>("etd1"),
                            etd45 = row1.Field<string>("etd45"),
                            etd2 = row1.Field<string>("etd2"),
                            etd3 = row1.Field<string>("etd3"),
                            etd6 = row1.Field<string>("etd6"),
                            etd12 = row1.Field<string>("etd12"),
                            etd18 = row1.Field<string>("etd18"),

                            etd_until_days1 = row1.Field<string>("etd_until_days1"),
                            etd_until_days45 = row1.Field<string>("etd_until_days45"),
                            etd_until_days2 = row1.Field<string>("etd_until_days2"),
                            etd_until_days3 = row1.Field<string>("etd_until_days3"),
                            etd_until_days6 = row1.Field<string>("etd_until_days6"),
                            etd_until_days12 = row1.Field<string>("etd_until_days12"),
                            etd_until_days18 = row1.Field<string>("etd_until_days18"),

                            contract_date1 = row1.Field<string>("contract_date1"),
                            contract_date45 = row1.Field<string>("contract_date45"),
                            contract_date2 = row1.Field<string>("contract_date2"),
                            contract_date3 = row1.Field<string>("contract_date3"),
                            contract_date6 = row1.Field<string>("contract_date6"),
                            contract_date12 = row1.Field<string>("contract_date12"),
                            contract_date18 = row1.Field<string>("contract_date18"),

                            contract_until_days1 = row1.Field<string>("contract_until_days1"),
                            contract_until_days45 = row1.Field<string>("contract_until_days45"),
                            contract_until_days2 = row1.Field<string>("contract_until_days2"),
                            contract_until_days3 = row1.Field<string>("contract_until_days3"),
                            contract_until_days6 = row1.Field<string>("contract_until_days6"),
                            contract_until_days12 = row1.Field<string>("contract_until_days12"),
                            contract_until_days18 = row1.Field<string>("contract_until_days18"),

                            delivery_days = row1.Field<double>("delivery_days"),
                            production_days = row1.Field<double>("production_days"),
                            contents = row1.Field<string>("contents"),
                            remark = row1.Field<string>("remark"),
                            division = row1.Field<string>("division"),
                            manager1 = row1.Field<string>("manager1"),
                            manager2 = row1.Field<string>("manager2"),
                            manager3 = row1.Field<string>("manager3"),
                            updatetime = row1.Field<string>("updatetime"),
                            main_id = row1.Field<int>("main_id"),
                            sub_id = row1.Field<int>("sub_id"),
                            is_hide = row1.Field<string>("is_hide"),
                            confirmation_date = row1.Field<string>("confirmation_date"),
                            sheet_type = row1.Field<string>("sheet_type")
                            //is_confirmation = row1.Field<string>("is_confirmation")
                        };
            DataTable stepDt1 = null;
            if (query != null)
            {
                stepDt1 = ConvertListToDatatable(query);
                mainDt = stepDt1;
            }
            //수입했던 품목=========================================================================================================================
            if (stepDt1 != null)
            {
                DataTable incomeDt = purchaseRepository.GetPurchaseIncomProduct(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
                var query2 = from row1 in stepDt1.AsEnumerable()
                             join row2 in incomeDt.AsEnumerable() on row1.Field<string>("product_code2") equals row2.Field<string>("품목코드") into gj
                             from subRow in gj.DefaultIfEmpty()
                             select new
                             {
                                 is_new = row1.Field<string>("is_new"),
                                 is_income = subRow != null ? "TRUE" : "FALSE",
                                 product = row1.Field<string>("product"),
                                 origin = row1.Field<string>("origin"),
                                 sizes = row1.Field<string>("sizes"),
                                 unit = row1.Field<string>("unit"),
                                 price_unit = row1.Field<string>("price_unit"),
                                 unit_count = row1.Field<string>("unit_count"),
                                 seaover_unit = row1.Field<string>("seaover_unit"),
                                 product_code = row1.Field<string>("product_code"),
                                 stock1 = row1.Field<double>("stock1"),
                                 shipment_qty = row1.Field<double>("shipment_qty"),
                                 unpending_qty_before = row1.Field<double>("unpending_qty_before"),
                                 stock2 = row1.Field<double>("stock2"),
                                 seaover_unpending = row1.Field<double>("seaover_unpending"),
                                 seaover_pending = row1.Field<double>("seaover_pending"),
                                 reserved_stock = row1.Field<double>("reserved_stock"),
                                 stock3 = row1.Field<double>("stock3"),
                                 min_sales_count = row1.Field<double>("min_sales_count"),

                                 sales_count_month1 = row1.Field<double>("sales_count_month1"),
                                 sales_count_month45 = row1.Field<double>("sales_count_month45"),
                                 sales_count_month2 = row1.Field<double>("sales_count_month2"),
                                 sales_count_month3 = row1.Field<double>("sales_count_month3"),
                                 sales_count_month6 = row1.Field<double>("sales_count_month6"),
                                 sales_count_month12 = row1.Field<double>("sales_count_month12"),
                                 sales_count_month18 = row1.Field<double>("sales_count_month18"),

                                 sales_count_day1 = row1.Field<double>("sales_count_day1"),
                                 sales_count_day45 = row1.Field<double>("sales_count_day45"),
                                 sales_count_day2 = row1.Field<double>("sales_count_day2"),
                                 sales_count_day3 = row1.Field<double>("sales_count_day3"),
                                 sales_count_day6 = row1.Field<double>("sales_count_day6"),
                                 sales_count_day12 = row1.Field<double>("sales_count_day12"),
                                 sales_count_day18 = row1.Field<double>("sales_count_day18"),

                                 sales_count1 = row1.Field<double>("sales_count1"),
                                 sales_count45 = row1.Field<double>("sales_count45"),
                                 sales_count2 = row1.Field<double>("sales_count2"),
                                 sales_count3 = row1.Field<double>("sales_count3"),
                                 sales_count6 = row1.Field<double>("sales_count6"),
                                 sales_count12 = row1.Field<double>("sales_count12"),
                                 sales_count18 = row1.Field<double>("sales_count18"),
                                 min_exhausted_date = row1.Field<double>("min_sales_count"),

                                 enable_sales_days1 = row1.Field<double>("enable_sales_days1"),
                                 enable_sales_days45 = row1.Field<double>("enable_sales_days45"),
                                 enable_sales_days2 = row1.Field<double>("enable_sales_days2"),
                                 enable_sales_days3 = row1.Field<double>("enable_sales_days3"),
                                 enable_sales_days6 = row1.Field<double>("enable_sales_days6"),
                                 enable_sales_days12 = row1.Field<double>("enable_sales_days12"),
                                 enable_sales_days18 = row1.Field<double>("enable_sales_days18"),
                                 min_enable_sales_days = row1.Field<double>("min_enable_sales_days"),

                                 exhausted_date1 = row1.Field<string>("exhausted_date1"),
                                 exhausted_date45 = row1.Field<string>("exhausted_date45"),
                                 exhausted_date2 = row1.Field<string>("exhausted_date2"),
                                 exhausted_date3 = row1.Field<string>("exhausted_date3"),
                                 exhausted_date6 = row1.Field<string>("exhausted_date6"),
                                 exhausted_date12 = row1.Field<string>("exhausted_date12"),
                                 exhausted_date18 = row1.Field<string>("exhausted_date18"),

                                 exhausted_date_until_days1 = row1.Field<string>("exhausted_date_until_days1"),
                                 exhausted_date_until_days45 = row1.Field<string>("exhausted_date_until_days45"),
                                 exhausted_date_until_days2 = row1.Field<string>("exhausted_date_until_days2"),
                                 exhausted_date_until_days3 = row1.Field<string>("exhausted_date_until_days3"),
                                 exhausted_date_until_days6 = row1.Field<string>("exhausted_date_until_days6"),
                                 exhausted_date_until_days12 = row1.Field<string>("exhausted_date_until_days12"),
                                 exhausted_date_until_days18 = row1.Field<string>("exhausted_date_until_days18"),

                                 etd1 = row1.Field<string>("etd1"),
                                 etd45 = row1.Field<string>("etd45"),
                                 etd2 = row1.Field<string>("etd2"),
                                 etd3 = row1.Field<string>("etd3"),
                                 etd6 = row1.Field<string>("etd6"),
                                 etd12 = row1.Field<string>("etd12"),
                                 etd18 = row1.Field<string>("etd18"),

                                 etd_until_days1 = row1.Field<string>("etd_until_days1"),
                                 etd_until_days45 = row1.Field<string>("etd_until_days45"),
                                 etd_until_days2 = row1.Field<string>("etd_until_days2"),
                                 etd_until_days3 = row1.Field<string>("etd_until_days3"),
                                 etd_until_days6 = row1.Field<string>("etd_until_days6"),
                                 etd_until_days12 = row1.Field<string>("etd_until_days12"),
                                 etd_until_days18 = row1.Field<string>("etd_until_days18"),

                                 contract_date1 = row1.Field<string>("contract_date1"),
                                 contract_date45 = row1.Field<string>("contract_date45"),
                                 contract_date2 = row1.Field<string>("contract_date2"),
                                 contract_date3 = row1.Field<string>("contract_date3"),
                                 contract_date6 = row1.Field<string>("contract_date6"),
                                 contract_date12 = row1.Field<string>("contract_date12"),
                                 contract_date18 = row1.Field<string>("contract_date18"),

                                 contract_until_days1 = row1.Field<string>("contract_until_days1"),
                                 contract_until_days45 = row1.Field<string>("contract_until_days45"),
                                 contract_until_days2 = row1.Field<string>("contract_until_days2"),
                                 contract_until_days3 = row1.Field<string>("contract_until_days3"),
                                 contract_until_days6 = row1.Field<string>("contract_until_days6"),
                                 contract_until_days12 = row1.Field<string>("contract_until_days12"),
                                 contract_until_days18 = row1.Field<string>("contract_until_days18"),

                                 delivery_days = row1.Field<double>("delivery_days"),
                                 production_days = row1.Field<double>("production_days"),
                                 contents = row1.Field<string>("contents"),
                                 remark = row1.Field<string>("remark"),
                                 division = row1.Field<string>("division"),
                                 manager1 = row1.Field<string>("manager1"),
                                 manager2 = row1.Field<string>("manager2"),
                                 manager3 = row1.Field<string>("manager3"),
                                 updatetime = row1.Field<string>("updatetime"),
                                 main_id = row1.Field<int>("main_id"),
                                 sub_id = row1.Field<int>("sub_id"),
                                 is_hide = row1.Field<string>("is_hide"),
                                 confirmation_date = row1.Field<string>("confirmation_date"),
                                 sheet_type = row1.Field<string>("sheet_type")
                                 //is_confirmation = row1.Field<string>("is_confirmation")
                             };
                DataTable stepDt2 = null;
                if (query2 != null)
                {
                    stepDt2 = ConvertListToDatatable(query2);
                    mainDt = stepDt2;
                }
            }
            //탭별 검색조건================================================================================================================
            if (mainDt != null)
            {
                string whrStr = "";
                if (tabDgv.SelectedTab.Name == "tpNew")
                    whrStr = $"sheet_type = 0 AND is_hide = 'FALSE' ";
                else if (tabDgv.SelectedTab.Name == "tpShortHold")
                    whrStr = $"sheet_type = 1 AND is_hide = 'FALSE' ";
                else if (tabDgv.SelectedTab.Name == "tpLongHold")
                    whrStr = $"sheet_type = 2 AND is_hide = 'FALSE' ";
                else if (tabDgv.SelectedTab.Name == "tpHide")
                    whrStr = $"is_hide = 'TRUE'";
                else if (tabDgv.SelectedTab.Name == "tpAll")
                    whrStr = $"is_hide = 'FALSE'";
                //수입여부
                if (rbIncome.Checked)
                    whrStr += " AND is_income = 'TRUE'";
                else if (rbNew.Checked)
                    whrStr += " AND is_income = 'FALSE'";
                //Select
                DataRow[] dr = mainDt.Select(whrStr);
                if (dr.Length > 0)
                {
                    mainDt = dr.CopyToDataTable();
                    //정렬
                    DataView dv = new DataView(mainDt);
                    string sortStr = sortString(cbSortType.Text);
                    if (!string.IsNullOrEmpty(sortStr))
                        sortStr = sortStr + ", product, origin, sizes, unit, price_unit, unit_count, seaover_unit";
                    else
                        sortStr = "is_new DESC, product, origin, sizes, unit, price_unit, unit_count, seaover_unit";
                    dv.Sort = sortStr;
                    mainDt = dv.ToTable();

                    DataTable cloneDt = mainDt.Clone();

                    cloneDt.Columns["exhausted_date_until_days1"].DataType = typeof(System.Int32);
                    cloneDt.Columns["exhausted_date_until_days45"].DataType = typeof(System.Int32);
                    cloneDt.Columns["exhausted_date_until_days2"].DataType = typeof(System.Int32);
                    cloneDt.Columns["exhausted_date_until_days3"].DataType = typeof(System.Int32);
                    cloneDt.Columns["exhausted_date_until_days6"].DataType = typeof(System.Int32);
                    cloneDt.Columns["exhausted_date_until_days12"].DataType = typeof(System.Int32);
                    cloneDt.Columns["exhausted_date_until_days18"].DataType = typeof(System.Int32);

                    cloneDt.Columns["etd_until_days1"].DataType = typeof(System.Int32);
                    cloneDt.Columns["etd_until_days45"].DataType = typeof(System.Int32);
                    cloneDt.Columns["etd_until_days2"].DataType = typeof(System.Int32);
                    cloneDt.Columns["etd_until_days3"].DataType = typeof(System.Int32);
                    cloneDt.Columns["etd_until_days6"].DataType = typeof(System.Int32);
                    cloneDt.Columns["etd_until_days12"].DataType = typeof(System.Int32);
                    cloneDt.Columns["etd_until_days18"].DataType = typeof(System.Int32);

                    cloneDt.Columns["contract_until_days1"].DataType = typeof(System.Int32);
                    cloneDt.Columns["contract_until_days45"].DataType = typeof(System.Int32);
                    cloneDt.Columns["contract_until_days2"].DataType = typeof(System.Int32);
                    cloneDt.Columns["contract_until_days3"].DataType = typeof(System.Int32);
                    cloneDt.Columns["contract_until_days6"].DataType = typeof(System.Int32);
                    cloneDt.Columns["contract_until_days12"].DataType = typeof(System.Int32);
                    cloneDt.Columns["contract_until_days18"].DataType = typeof(System.Int32);

                    foreach (DataRow row in mainDt.Rows)
                    {
                        // 임시 Table의 원본 DataRow내용을 가져온다.
                        cloneDt.ImportRow(row);
                    }
                    mainDt = cloneDt;
                    dgvProduct.DataSource = mainDt;

                    SetHeaderStyle();
                }
            }
            txtCurrentRecord.Text = "0";
            txtTotalRecord.Text = dgvProduct.Rows.Count.ToString("#,##0");

            return dgvProduct.Rows.Count;
        }
        private void SetHeaderStyle()
        {
            dgvProduct.Columns["is_new"].Visible = false;
            dgvProduct.Columns["product_code"].Visible = false;
            //dgvProduct.Columns["product_code2"].Visible = false;
            dgvProduct.Columns["is_income"].Visible = false;
            dgvProduct.Columns["delivery_days"].Visible = false;
            dgvProduct.Columns["production_days"].Visible = false;
            
            dgvProduct.Columns["updatetime"].Visible = false;
            dgvProduct.Columns["main_id"].Visible = false;
            dgvProduct.Columns["sub_id"].Visible = false;
            dgvProduct.Columns["is_hide"].Visible = false;
            dgvProduct.Columns["confirmation_date"].Visible = false;
            dgvProduct.Columns["sheet_type"].Visible = false;
            //dgvProduct.Columns["is_confirmation"].Visible = false;

            dgvProduct.Columns["shipment_qty"].Visible = cbStockDetail.Checked;
            dgvProduct.Columns["unpending_qty_before"].Visible = cbStockDetail.Checked;
            dgvProduct.Columns["seaover_unpending"].Visible = cbStockDetail.Checked;
            dgvProduct.Columns["seaover_pending"].Visible = cbStockDetail.Checked;
            dgvProduct.Columns["reserved_stock"].Visible = cbStockDetail.Checked;

            dgvProduct.Columns["shipment_qty"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["unpending_qty_before"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["seaover_unpending"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["seaover_pending"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["reserved_stock"].DefaultCellStyle.Format = "#,##0";

            dgvProduct.Columns["stock1"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["stock2"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["stock3"].DefaultCellStyle.Format = "#,##0";

            dgvProduct.Columns["min_sales_count"].Visible = false;
            dgvProduct.Columns["sales_count1"].Visible = false;
            dgvProduct.Columns["sales_count45"].Visible = false;
            dgvProduct.Columns["sales_count2"].Visible = false;
            dgvProduct.Columns["sales_count3"].Visible = false;
            dgvProduct.Columns["sales_count6"].Visible = false;
            dgvProduct.Columns["sales_count12"].Visible = false;
            dgvProduct.Columns["sales_count18"].Visible = false;

            dgvProduct.Columns["min_sales_count"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count1"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count45"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count2"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count3"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count6"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count12"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count18"].DefaultCellStyle.Format = "#,##0";

            dgvProduct.Columns["sales_count_day1"].Visible = false;
            dgvProduct.Columns["sales_count_day45"].Visible = false;
            dgvProduct.Columns["sales_count_day2"].Visible = false;
            dgvProduct.Columns["sales_count_day3"].Visible = false;
            dgvProduct.Columns["sales_count_day6"].Visible = false;
            dgvProduct.Columns["sales_count_day12"].Visible = false;
            dgvProduct.Columns["sales_count_day18"].Visible = false;

            dgvProduct.Columns["sales_count_day1"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count_day45"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count_day2"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count_day3"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count_day6"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count_day12"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count_day18"].DefaultCellStyle.Format = "#,##0";

            dgvProduct.Columns["sales_count_month1"].Visible = cbAllSaleTerms.Checked;
            dgvProduct.Columns["sales_count_month45"].Visible = cbAllSaleTerms.Checked;
            dgvProduct.Columns["sales_count_month2"].Visible = cbAllSaleTerms.Checked;
            dgvProduct.Columns["sales_count_month3"].Visible = cbAllSaleTerms.Checked;
            dgvProduct.Columns["sales_count_month6"].Visible = cbAllSaleTerms.Checked;
            dgvProduct.Columns["sales_count_month12"].Visible = cbAllSaleTerms.Checked;
            dgvProduct.Columns["sales_count_month18"].Visible = cbAllSaleTerms.Checked;

            dgvProduct.Columns["sales_count_month1"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count_month45"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count_month2"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count_month3"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count_month6"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count_month12"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["sales_count_month18"].DefaultCellStyle.Format = "#,##0";

            dgvProduct.Columns["min_enable_sales_days"].Visible = false;
            dgvProduct.Columns["enable_sales_days1"].Visible = false;
            dgvProduct.Columns["enable_sales_days45"].Visible = false;
            dgvProduct.Columns["enable_sales_days2"].Visible = false;
            dgvProduct.Columns["enable_sales_days3"].Visible = false;
            dgvProduct.Columns["enable_sales_days6"].Visible = false;
            dgvProduct.Columns["enable_sales_days12"].Visible = false;
            dgvProduct.Columns["enable_sales_days18"].Visible = false;

            dgvProduct.Columns["min_enable_sales_days"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["enable_sales_days1"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["enable_sales_days45"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["enable_sales_days2"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["enable_sales_days3"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["enable_sales_days6"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["enable_sales_days12"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["enable_sales_days18"].DefaultCellStyle.Format = "#,##0";

            dgvProduct.Columns["min_exhausted_date"].Visible = false;
            dgvProduct.Columns["exhausted_date1"].Visible = false;
            dgvProduct.Columns["exhausted_date45"].Visible = false;
            dgvProduct.Columns["exhausted_date2"].Visible = false;
            dgvProduct.Columns["exhausted_date3"].Visible = false;
            dgvProduct.Columns["exhausted_date6"].Visible = false;
            dgvProduct.Columns["exhausted_date12"].Visible = false;
            dgvProduct.Columns["exhausted_date18"].Visible = false;

            dgvProduct.Columns["etd1"].Visible = false;
            dgvProduct.Columns["etd45"].Visible = false;
            dgvProduct.Columns["etd2"].Visible = false;
            dgvProduct.Columns["etd3"].Visible = false;
            dgvProduct.Columns["etd6"].Visible = false;
            dgvProduct.Columns["etd12"].Visible = false;
            dgvProduct.Columns["etd18"].Visible = false;

            dgvProduct.Columns["contract_date1"].Visible = false;
            dgvProduct.Columns["contract_date45"].Visible = false;
            dgvProduct.Columns["contract_date2"].Visible = false;
            dgvProduct.Columns["contract_date3"].Visible = false;
            dgvProduct.Columns["contract_date6"].Visible = false;
            dgvProduct.Columns["contract_date12"].Visible = false;
            dgvProduct.Columns["contract_date18"].Visible = false;

            dgvProduct.Columns["exhausted_date_until_days1"].Visible = false;
            dgvProduct.Columns["exhausted_date_until_days45"].Visible = false;
            dgvProduct.Columns["exhausted_date_until_days2"].Visible = false;
            dgvProduct.Columns["exhausted_date_until_days3"].Visible = false;
            dgvProduct.Columns["exhausted_date_until_days6"].Visible = false;
            dgvProduct.Columns["exhausted_date_until_days12"].Visible = false;
            dgvProduct.Columns["exhausted_date_until_days18"].Visible = false;

            dgvProduct.Columns["exhausted_date_until_days1"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["exhausted_date_until_days45"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["exhausted_date_until_days2"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["exhausted_date_until_days3"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["exhausted_date_until_days6"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["exhausted_date_until_days12"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["exhausted_date_until_days18"].DefaultCellStyle.Format = "#,##0";

            dgvProduct.Columns["etd_until_days1"].Visible = false;
            dgvProduct.Columns["etd_until_days45"].Visible = false;
            dgvProduct.Columns["etd_until_days2"].Visible = false;
            dgvProduct.Columns["etd_until_days3"].Visible = false;
            dgvProduct.Columns["etd_until_days6"].Visible = false;
            dgvProduct.Columns["etd_until_days12"].Visible = false;
            dgvProduct.Columns["etd_until_days18"].Visible = false;

            dgvProduct.Columns["etd_until_days1"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["etd_until_days45"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["etd_until_days2"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["etd_until_days3"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["etd_until_days6"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["etd_until_days12"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["etd_until_days18"].DefaultCellStyle.Format = "#,##0";

            dgvProduct.Columns["contract_until_days1"].Visible = false;
            dgvProduct.Columns["contract_until_days45"].Visible = false;
            dgvProduct.Columns["contract_until_days2"].Visible = false;
            dgvProduct.Columns["contract_until_days3"].Visible = false;
            dgvProduct.Columns["contract_until_days6"].Visible = false;
            dgvProduct.Columns["contract_until_days12"].Visible = false;
            dgvProduct.Columns["contract_until_days18"].Visible = false;

            dgvProduct.Columns["contract_until_days1"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["contract_until_days45"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["contract_until_days2"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["contract_until_days3"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["contract_until_days6"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["contract_until_days12"].DefaultCellStyle.Format = "#,##0";
            dgvProduct.Columns["contract_until_days18"].DefaultCellStyle.Format = "#,##0";


            switch (cbSaleTerm.Text)
            {
                case "1개월":
                    
                    dgvProduct.Columns["sales_count_month1"].Visible = true;
                    if (cbSalesDetail.Checked)
                    {
                        dgvProduct.Columns["sales_count1"].Visible = true;
                        dgvProduct.Columns["sales_count_day1"].Visible = true;
                    }

                    dgvProduct.Columns["enable_sales_days1"].Visible = true;
                    dgvProduct.Columns["exhausted_date1"].Visible = true;
                    dgvProduct.Columns["etd1"].Visible = true;
                    dgvProduct.Columns["contract_date1"].Visible = true;

                    dgvProduct.Columns["exhausted_date_until_days1"].Visible = true;
                    dgvProduct.Columns["etd_until_days1"].Visible = true;
                    dgvProduct.Columns["contract_until_days1"].Visible = true;

                    break;
                case "45일":
                    dgvProduct.Columns["sales_count_month45"].Visible = true;
                    if (cbSalesDetail.Checked)
                    {
                        dgvProduct.Columns["sales_count_day45"].Visible = true;
                        dgvProduct.Columns["sales_count45"].Visible = true;
                    }
                    dgvProduct.Columns["enable_sales_days45"].Visible = true;
                    dgvProduct.Columns["exhausted_date45"].Visible = true;
                    dgvProduct.Columns["etd45"].Visible = true;
                    dgvProduct.Columns["contract_date45"].Visible = true;

                    dgvProduct.Columns["exhausted_date_until_days45"].Visible = true;
                    dgvProduct.Columns["etd_until_days45"].Visible = true;
                    dgvProduct.Columns["contract_until_days45"].Visible = true;
                    break;
                case "2개월":
                    dgvProduct.Columns["sales_count_month2"].Visible = true;
                    
                    if (cbSalesDetail.Checked)
                    {
                        dgvProduct.Columns["sales_count_day2"].Visible = true;
                        dgvProduct.Columns["sales_count2"].Visible = true;

                    }
                    dgvProduct.Columns["enable_sales_days2"].Visible = true;
                    dgvProduct.Columns["exhausted_date2"].Visible = true;
                    dgvProduct.Columns["etd2"].Visible = true;
                    dgvProduct.Columns["contract_date2"].Visible = true;

                    dgvProduct.Columns["exhausted_date_until_days2"].Visible = true;
                    dgvProduct.Columns["etd_until_days2"].Visible = true;
                    dgvProduct.Columns["contract_until_days2"].Visible = true;
                    break;
                case "3개월":
                    dgvProduct.Columns["sales_count_month3"].Visible = true;
                    if (cbSalesDetail.Checked)
                    {
                        dgvProduct.Columns["sales_count_day3"].Visible = true;
                        dgvProduct.Columns["sales_count3"].Visible = true;
                    }
                    dgvProduct.Columns["enable_sales_days3"].Visible = true;
                    dgvProduct.Columns["exhausted_date3"].Visible = true;
                    dgvProduct.Columns["etd3"].Visible = true;
                    dgvProduct.Columns["contract_date3"].Visible = true;

                    dgvProduct.Columns["exhausted_date_until_days3"].Visible = true;
                    dgvProduct.Columns["etd_until_days3"].Visible = true;
                    dgvProduct.Columns["contract_until_days3"].Visible = true;
                    break;
                case "6개월":
                    dgvProduct.Columns["sales_count_month6"].Visible = true;
                    if (cbSalesDetail.Checked)
                    {
                        dgvProduct.Columns["sales_count_day6"].Visible = true;
                        dgvProduct.Columns["sales_count6"].Visible = true;
                    }
                    dgvProduct.Columns["enable_sales_days6"].Visible = true;
                    dgvProduct.Columns["exhausted_date6"].Visible = true;
                    dgvProduct.Columns["etd6"].Visible = true;
                    dgvProduct.Columns["contract_date6"].Visible = true;

                    dgvProduct.Columns["exhausted_date_until_days6"].Visible = true;
                    dgvProduct.Columns["etd_until_days6"].Visible = true;
                    dgvProduct.Columns["contract_until_days6"].Visible = true;
                    break;
                case "12개월":
                    
                    dgvProduct.Columns["sales_count_month12"].Visible = true;
                    if (cbSalesDetail.Checked)
                    {
                        dgvProduct.Columns["sales_count_day12"].Visible = true;
                        dgvProduct.Columns["sales_count12"].Visible = true;
                    }
                    dgvProduct.Columns["enable_sales_days12"].Visible = true;
                    dgvProduct.Columns["exhausted_date12"].Visible = true;
                    dgvProduct.Columns["etd12"].Visible = true;
                    dgvProduct.Columns["contract_date12"].Visible = true;

                    dgvProduct.Columns["exhausted_date_until_days12"].Visible = true;
                    dgvProduct.Columns["etd_until_days12"].Visible = true;
                    dgvProduct.Columns["contract_until_days12"].Visible = true;
                    break;
                case "18개월":
                    dgvProduct.Columns["sales_count_month18"].Visible = true;
                    
                    if (cbSalesDetail.Checked)
                    {
                        dgvProduct.Columns["sales_count_day18"].Visible = true;
                        dgvProduct.Columns["sales_count18"].Visible = true;
                    }
                    dgvProduct.Columns["enable_sales_days18"].Visible = true;
                    dgvProduct.Columns["exhausted_date18"].Visible = true;
                    dgvProduct.Columns["etd18"].Visible = true;
                    dgvProduct.Columns["contract_date18"].Visible = true;

                    dgvProduct.Columns["exhausted_date_until_days18"].Visible = true;
                    dgvProduct.Columns["etd_until_days18"].Visible = true;
                    dgvProduct.Columns["contract_until_days18"].Visible = true;
                    break;
            }

            if (tabDgv.SelectedTab.Name == "tpShortHold" || tabDgv.SelectedTab.Name == "tpLongHold")
                dgvProduct.Columns["confirmation_date"].Visible = true;
            

            dgvProduct.Columns["product"].HeaderText = "품목";
            dgvProduct.Columns["origin"].HeaderText = "원산지";
            dgvProduct.Columns["sizes"].HeaderText = "규격";
            dgvProduct.Columns["unit"].HeaderText = "단위";
            dgvProduct.Columns["price_unit"].HeaderText = "가격단위";
            dgvProduct.Columns["unit_count"].HeaderText = "단위수량";
            dgvProduct.Columns["seaover_unit"].HeaderText = "S단위";
            dgvProduct.Columns["shipment_qty"].HeaderText = "선적전";
            dgvProduct.Columns["unpending_qty_before"].HeaderText = "배송중";
            dgvProduct.Columns["seaover_unpending"].HeaderText = "미통관";
            dgvProduct.Columns["seaover_pending"].HeaderText = "통관";
            dgvProduct.Columns["reserved_stock"].HeaderText = "예약수";

            dgvProduct.Columns["confirmation_date"].HeaderText = "보류일자";

            dgvProduct.Columns["stock1"].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
            dgvProduct.Columns["stock1"].HeaderText = "선적전";
            dgvProduct.Columns["stock2"].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
            dgvProduct.Columns["stock2"].HeaderText = "선적후";
            dgvProduct.Columns["stock3"].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
            dgvProduct.Columns["stock3"].HeaderText = "총재고";

            dgvProduct.Columns["sales_count1"].HeaderText = "총판매량";
            dgvProduct.Columns["sales_count45"].HeaderText = "총판매량";
            dgvProduct.Columns["sales_count2"].HeaderText = "총판매량";
            dgvProduct.Columns["sales_count3"].HeaderText = "총판매량";
            dgvProduct.Columns["sales_count6"].HeaderText = "총판매량";
            dgvProduct.Columns["sales_count12"].HeaderText = "총판매량";
            dgvProduct.Columns["sales_count18"].HeaderText = "총판매량";

            dgvProduct.Columns["sales_count_day1"].HeaderText = "日판매";
            dgvProduct.Columns["sales_count_day45"].HeaderText = "日판매";
            dgvProduct.Columns["sales_count_day2"].HeaderText = "日판매";
            dgvProduct.Columns["sales_count_day3"].HeaderText = "日판매";
            dgvProduct.Columns["sales_count_day6"].HeaderText = "日판매"; ;
            dgvProduct.Columns["sales_count_day12"].HeaderText = "日판매";
            dgvProduct.Columns["sales_count_day18"].HeaderText = "日판매";

            dgvProduct.Columns["sales_count_month1"].HeaderText = "月판매(1개월)";
            dgvProduct.Columns["sales_count_month45"].HeaderText = "月판매(45일)";
            dgvProduct.Columns["sales_count_month2"].HeaderText = "月판매(2개월)";
            dgvProduct.Columns["sales_count_month3"].HeaderText = "月판매(3개월)";
            dgvProduct.Columns["sales_count_month6"].HeaderText = "月판매(6개월)";
            dgvProduct.Columns["sales_count_month12"].HeaderText = "月판매(12개월)";
            dgvProduct.Columns["sales_count_month18"].HeaderText = "月판매(18개월)";

            dgvProduct.Columns["min_enable_sales_days"].HeaderText = "최소 판매가능일";
            dgvProduct.Columns["enable_sales_days1"].HeaderText = "판매가능일(1개월)";
            dgvProduct.Columns["enable_sales_days45"].HeaderText = "판매가능일(45일)";
            dgvProduct.Columns["enable_sales_days2"].HeaderText = "판매가능일(2개월)";
            dgvProduct.Columns["enable_sales_days3"].HeaderText = "판매가능일(3개월)";
            dgvProduct.Columns["enable_sales_days6"].HeaderText = "판매가능일(6개월)";
            dgvProduct.Columns["enable_sales_days12"].HeaderText = "판매가능일(12개월)";
            dgvProduct.Columns["enable_sales_days18"].HeaderText = "판매가능일(18개월)";

            dgvProduct.Columns["min_exhausted_date"].HeaderText = "최소 쇼트일자";
            dgvProduct.Columns["exhausted_date1"].HeaderText = "쇼트일자(1개월)";
            dgvProduct.Columns["exhausted_date45"].HeaderText = "쇼트일자(45일)";
            dgvProduct.Columns["exhausted_date2"].HeaderText = "쇼트일자(2개월)";
            dgvProduct.Columns["exhausted_date3"].HeaderText = "쇼트일자(3개월)";
            dgvProduct.Columns["exhausted_date6"].HeaderText = "쇼트일자(6개월)";
            dgvProduct.Columns["exhausted_date12"].HeaderText = "쇼트일자(12개월)";
            dgvProduct.Columns["exhausted_date18"].HeaderText = "쇼트일자(18개월)";

            dgvProduct.Columns["exhausted_date_until_days1"].HeaderText = "남은일";
            dgvProduct.Columns["exhausted_date_until_days45"].HeaderText = "남은일";
            dgvProduct.Columns["exhausted_date_until_days2"].HeaderText = "남은일";
            dgvProduct.Columns["exhausted_date_until_days3"].HeaderText = "남은일";
            dgvProduct.Columns["exhausted_date_until_days6"].HeaderText = "남은일";
            dgvProduct.Columns["exhausted_date_until_days12"].HeaderText = "남은일";
            dgvProduct.Columns["exhausted_date_until_days18"].HeaderText = "남은일";

            dgvProduct.Columns["etd1"].HeaderText = "최소ETD(1개월)";
            dgvProduct.Columns["etd45"].HeaderText = "최소ETD(45일)";
            dgvProduct.Columns["etd2"].HeaderText = "최소ETD(2개월)";
            dgvProduct.Columns["etd3"].HeaderText = "최소ETD(3개월)";
            dgvProduct.Columns["etd6"].HeaderText = "최소ETD(6개월)";
            dgvProduct.Columns["etd12"].HeaderText = "최소ETD(12개월)";
            dgvProduct.Columns["etd18"].HeaderText = "최소ETD(18개월)";

            dgvProduct.Columns["etd_until_days1"].HeaderText = "남은일";
            dgvProduct.Columns["etd_until_days45"].HeaderText = "남은일";
            dgvProduct.Columns["etd_until_days2"].HeaderText = "남은일";
            dgvProduct.Columns["etd_until_days3"].HeaderText = "남은일";
            dgvProduct.Columns["etd_until_days6"].HeaderText = "남은일";
            dgvProduct.Columns["etd_until_days12"].HeaderText = "남은일";
            dgvProduct.Columns["etd_until_days18"].HeaderText = "남은일";

            dgvProduct.Columns["contract_date1"].HeaderText = "추천계약일자(1개월)";
            dgvProduct.Columns["contract_date45"].HeaderText = "추천계약일자(45일)";
            dgvProduct.Columns["contract_date2"].HeaderText = "추천계약일자(2개월)";
            dgvProduct.Columns["contract_date3"].HeaderText = "추천계약일자(3개월)";
            dgvProduct.Columns["contract_date6"].HeaderText = "추천계약일자(6개월)";
            dgvProduct.Columns["contract_date12"].HeaderText = "추천계약일자(12개월)";
            dgvProduct.Columns["contract_date18"].HeaderText = "추천계약일자(18개월)";

            dgvProduct.Columns["contract_until_days1"].HeaderText = "남은일";
            dgvProduct.Columns["contract_until_days45"].HeaderText = "남은일";
            dgvProduct.Columns["contract_until_days2"].HeaderText = "남은일";
            dgvProduct.Columns["contract_until_days3"].HeaderText = "남은일";
            dgvProduct.Columns["contract_until_days6"].HeaderText = "남은일";
            dgvProduct.Columns["contract_until_days12"].HeaderText = "남은일";
            dgvProduct.Columns["contract_until_days18"].HeaderText = "남은일";

            dgvProduct.Columns["division"].HeaderText = "구분";
            dgvProduct.Columns["manager1"].HeaderText = "담당1";
            dgvProduct.Columns["manager2"].HeaderText = "담당2";
            dgvProduct.Columns["manager3"].HeaderText = "무역담당";

            dgvProduct.Columns["contents"].HeaderText = "내용";
            dgvProduct.Columns["remark"].HeaderText = "비고";

            //넓이
            foreach (DataGridViewColumn col in dgvProduct.Columns)
                col.Width = 50;

            dgvProduct.Columns["product"].Width = 150;
            dgvProduct.Columns["origin"].Width = 70;
            dgvProduct.Columns["sizes"].Width = 100;
            dgvProduct.Columns["confirmation_date"].Width = 70;

            dgvProduct.Columns["min_exhausted_date"].Width = 70;
            dgvProduct.Columns["exhausted_date1"].Width = 70;
            dgvProduct.Columns["exhausted_date45"].Width = 70;
            dgvProduct.Columns["exhausted_date2"].Width = 70;
            dgvProduct.Columns["exhausted_date3"].Width = 70;
            dgvProduct.Columns["exhausted_date6"].Width = 70;
            dgvProduct.Columns["exhausted_date12"].Width = 70;
            dgvProduct.Columns["exhausted_date18"].Width = 70;

            dgvProduct.Columns["etd1"].Width = 70;
            dgvProduct.Columns["etd45"].Width = 70;
            dgvProduct.Columns["etd2"].Width = 70;
            dgvProduct.Columns["etd3"].Width = 70;
            dgvProduct.Columns["etd6"].Width = 70;
            dgvProduct.Columns["etd12"].Width = 70;
            dgvProduct.Columns["etd18"].Width = 70;

            dgvProduct.Columns["contract_date1"].Width = 70;
            dgvProduct.Columns["contract_date45"].Width = 70;
            dgvProduct.Columns["contract_date2"].Width = 70;
            dgvProduct.Columns["contract_date3"].Width = 70;
            dgvProduct.Columns["contract_date6"].Width = 70;
            dgvProduct.Columns["contract_date12"].Width = 70;
            dgvProduct.Columns["contract_date18"].Width = 70;

            dgvProduct.Columns["contents"].Width = 100;
            dgvProduct.Columns["remark"].Width = 150;



            //헤더 디자인
            dgvProduct.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            //기본(회색)
            Color darkBlue = Color.FromArgb(43, 94, 170);    //남색
            dgvProduct.ColumnHeadersDefaultCellStyle.BackColor = darkBlue;
            dgvProduct.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProduct.Columns["unit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["price_unit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["unit_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["seaover_unit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            //재고내역========================================================================================
            Color lightBlue = Color.FromArgb(51, 102, 255);  //진파랑            
            dgvProduct.Columns["shipment_qty"].HeaderCell.Style.BackColor = Color.LightGray;
            dgvProduct.Columns["shipment_qty"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["shipment_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["shipment_qty"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["unpending_qty_before"].HeaderCell.Style.BackColor = Color.LightGray;
            dgvProduct.Columns["unpending_qty_before"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["unpending_qty_before"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["unpending_qty_before"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["seaover_unpending"].HeaderCell.Style.BackColor = Color.LightGray;
            dgvProduct.Columns["seaover_unpending"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["seaover_unpending"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["seaover_unpending"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["seaover_pending"].HeaderCell.Style.BackColor = Color.LightGray;
            dgvProduct.Columns["seaover_pending"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["seaover_pending"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["seaover_pending"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["reserved_stock"].HeaderCell.Style.BackColor = Color.LightGray;
            dgvProduct.Columns["reserved_stock"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["reserved_stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["reserved_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["stock1"].HeaderCell.Style.BackColor = Color.DarkGray;
            dgvProduct.Columns["stock1"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["stock1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["stock1"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["stock2"].HeaderCell.Style.BackColor = Color.DarkGray;
            dgvProduct.Columns["stock2"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["stock2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["stock2"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["stock3"].HeaderCell.Style.BackColor = Color.DarkGray;
            dgvProduct.Columns["stock3"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["stock3"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["stock3"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            //판매내역========================================================================================
            Color lightRosybrown = Color.FromArgb(216, 190, 190);
            dgvProduct.Columns["sales_count_month1"].HeaderCell.Style.BackColor = lightRosybrown;
            dgvProduct.Columns["sales_count_month1"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count_month1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count_month1"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count_month45"].HeaderCell.Style.BackColor = lightRosybrown;
            dgvProduct.Columns["sales_count_month45"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count_month45"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count_month45"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count_month2"].HeaderCell.Style.BackColor = lightRosybrown;
            dgvProduct.Columns["sales_count_month2"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count_month2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count_month2"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count_month3"].HeaderCell.Style.BackColor = lightRosybrown;
            dgvProduct.Columns["sales_count_month3"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count_month3"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count_month3"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count_month6"].HeaderCell.Style.BackColor = lightRosybrown;
            dgvProduct.Columns["sales_count_month6"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count_month6"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count_month6"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count_month12"].HeaderCell.Style.BackColor = lightRosybrown;
            dgvProduct.Columns["sales_count_month12"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count_month12"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count_month12"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count_month18"].HeaderCell.Style.BackColor = lightRosybrown;
            dgvProduct.Columns["sales_count_month18"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count_month18"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count_month18"].DefaultCellStyle.Format = "#,###,##0"; // 콤마


            dgvProduct.Columns["sales_count_day1"].HeaderCell.Style.BackColor = Color.Beige;
            dgvProduct.Columns["sales_count_day1"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count_day1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count_day1"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count_day45"].HeaderCell.Style.BackColor = Color.Beige;
            dgvProduct.Columns["sales_count_day45"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count_day45"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count_day45"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count_day2"].HeaderCell.Style.BackColor = Color.Beige;
            dgvProduct.Columns["sales_count_day2"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count_day2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count_day2"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count_day3"].HeaderCell.Style.BackColor = Color.Beige;
            dgvProduct.Columns["sales_count_day3"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count_day3"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count_day3"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count_day6"].HeaderCell.Style.BackColor = Color.Beige;
            dgvProduct.Columns["sales_count_day6"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count_day6"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count_day6"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count_day12"].HeaderCell.Style.BackColor = Color.Beige;
            dgvProduct.Columns["sales_count_day12"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count_day12"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count_day12"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count_day18"].HeaderCell.Style.BackColor = Color.Beige;
            dgvProduct.Columns["sales_count_day18"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count_day18"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count_day18"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count1"].HeaderCell.Style.BackColor = Color.Beige;
            dgvProduct.Columns["sales_count1"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count1"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count45"].HeaderCell.Style.BackColor = Color.Beige;
            dgvProduct.Columns["sales_count45"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count45"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count45"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count2"].HeaderCell.Style.BackColor = Color.Beige;
            dgvProduct.Columns["sales_count2"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count2"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count3"].HeaderCell.Style.BackColor = Color.Beige;
            dgvProduct.Columns["sales_count3"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count3"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count3"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count6"].HeaderCell.Style.BackColor = Color.Beige;
            dgvProduct.Columns["sales_count6"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count6"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count6"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count12"].HeaderCell.Style.BackColor = Color.Beige;
            dgvProduct.Columns["sales_count12"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count12"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count12"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["sales_count18"].HeaderCell.Style.BackColor = Color.Beige;
            dgvProduct.Columns["sales_count18"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["sales_count18"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["sales_count18"].DefaultCellStyle.Format = "#,###,##0"; // 콤마


            dgvProduct.Columns["enable_sales_days1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["enable_sales_days1"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["enable_sales_days45"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["enable_sales_days45"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["enable_sales_days2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["enable_sales_days2"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["enable_sales_days3"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["enable_sales_days3"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["enable_sales_days6"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["enable_sales_days6"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["enable_sales_days12"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["enable_sales_days12"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["enable_sales_days18"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["enable_sales_days18"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgvProduct.Columns["contents"].HeaderCell.Style.BackColor = Color.Ivory;
            dgvProduct.Columns["contents"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["contents"].DefaultCellStyle.BackColor = Color.Beige;

            dgvProduct.Columns["remark"].HeaderCell.Style.BackColor = Color.Ivory;
            dgvProduct.Columns["remark"].HeaderCell.Style.ForeColor = Color.Black;
            dgvProduct.Columns["remark"].DefaultCellStyle.BackColor = Color.Beige;
        }
        private string sortString(string sort)
        {
            string sortStr = "";
            string[] sorts = sort.Split('+');
            if(sorts.Length > 0 ) 
            {
                foreach (string s in sorts)
                {
                    switch (s)
                    {
                        case "신규":
                            if (string.IsNullOrEmpty(sortStr))
                                sortStr = "is_new DESC";
                            else
                                sortStr += ",is_new DESC";
                            break;
                        case "추천계약일":
                            if (string.IsNullOrEmpty(sortStr))
                                sortStr = "contract_date" + cbSaleTerm.Text.Replace("개월", "").Replace("일", "");
                            else
                                sortStr += ",contract_date" + cbSaleTerm.Text.Replace("개월", "").Replace("일", "");
                            break;
                        case "쇼트일자":
                            if (string.IsNullOrEmpty(sortStr))
                                sortStr = "exhausted_date" + cbSaleTerm.Text.Replace("개월", "").Replace("일", "");
                            else
                                sortStr += ",exhausted_date" + cbSaleTerm.Text.Replace("개월", "").Replace("일", "");
                            break;
                        case "품목":
                            if (string.IsNullOrEmpty(sortStr))
                                sortStr = "product";
                            else
                                sortStr += ",product";
                            break;
                        case "원산지":
                            if (string.IsNullOrEmpty(sortStr))
                                sortStr = "origin";
                            else
                                sortStr += ",origin";
                            break;
                        case "규격":
                            if (string.IsNullOrEmpty(sortStr))
                                sortStr = "sizes";
                            else
                                sortStr += ",sizes";
                            break;
                        case "단위":
                            if (string.IsNullOrEmpty(sortStr))
                                sortStr = "unit";
                            else
                                sortStr += ",unit";
                            break;

                    }
                }
            }
            return sortStr;
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
        private void ChangeToCheckList(bool isFlag)
        {
            if (isFlag)
            {
                /*if (messageBox.Show(this, "선택한 품목을 '확인중'시트로 옮기시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;*/
            }
            else
            {
                if (messageBox.Show(this, "선택한 품목을 '확인중'시트에서 내보내시겠습니가?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }

            int days = 0;
            if (isFlag)
            {
                SelectDate sd = new SelectDate();
                days = sd.GetDays();
                if (days == 0)
                    return;
            }

            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();
            foreach (DataGridViewRow row in dgvProduct.SelectedRows)
            {
                PriceComparisonTempSettingModel model = new PriceComparisonTempSettingModel();
                model.product_code = row.Cells["product"].Value.ToString()
                            + "_" + row.Cells["origin"].Value.ToString()
                            + "_" + row.Cells["sizes"].Value.ToString()
                            + "_" + row.Cells["unit"].Value.ToString()
                            + "_" + row.Cells["price_unit"].Value.ToString()
                            + "_" + row.Cells["unit_count"].Value.ToString()
                            + "_" + row.Cells["seaover_unit"].Value.ToString();
                model.is_hide = false;
                if (isFlag)
                    model.confirmation_date = DateTime.Now.AddDays(days).ToString("yyyy-MM-dd");
                else
                    model.confirmation_date = new DateTime(1900, 1, 1).ToString("yyyy-MM-dd");

                if (row.Cells["contents"].Value != null)
                    model.contents = row.Cells["contents"].Value.ToString();
                if (row.Cells["remark"].Value != null)
                    model.remark = row.Cells["remark"].Value.ToString();
                model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                model.edit_user = um.user_name;
                //삭제
                sql = priceComparisonTempRepository.DeleteSettingData(model.product_code);
                sqlList.Add(sql);
                //재등록
                sql = priceComparisonTempRepository.InsertSettingData(model);
                sqlList.Add(sql);
            }

            if (commonRepository.UpdateTran(sqlList) == -1)
                messageBox.Show(this, "등록중 에러가 발생하였습니다.");
            else
            {
                for (int i = dgvProduct.Rows.Count - 1; i >= 0; i--)
                {
                    if (dgvProduct.Rows[i].Selected)
                        dgvProduct.Rows.Remove(dgvProduct.Rows[i]);
                }
            }
        }
        private void ChangeToNotHandled(bool isFlag)
        {
            if (isFlag)
            {
                if (messageBox.Show(this, "선택한 품목을 '취급X'시트로 옮기시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }
            else
            {
                if (messageBox.Show(this, "선택한 품목을 '취급X'시트에서 내보내시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }

            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();
            foreach (DataGridViewRow row in dgvProduct.SelectedRows)
            {
                PriceComparisonTempSettingModel model = new PriceComparisonTempSettingModel();
                model.product_code = row.Cells["product"].Value.ToString()
                            + "_" + row.Cells["origin"].Value.ToString()
                            + "_" + row.Cells["sizes"].Value.ToString()
                            + "_" + row.Cells["unit"].Value.ToString()
                            + "_" + row.Cells["price_unit"].Value.ToString()
                            + "_" + row.Cells["unit_count"].Value.ToString()
                            + "_" + row.Cells["seaover_unit"].Value.ToString();

                if (isFlag)
                    model.is_hide = true;
                else
                    model.is_hide = false;

                if (row.Cells["confirmation_date"].Value != null)
                    model.confirmation_date = row.Cells["confirmation_date"].Value.ToString();
                if (row.Cells["contents"].Value != null)
                    model.contents = row.Cells["contents"].Value.ToString();
                if (row.Cells["remark"].Value != null)
                    model.remark = row.Cells["remark"].Value.ToString();
                model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                model.edit_user = um.user_name;
                //삭제
                sql = priceComparisonTempRepository.DeleteSettingData(model.product_code);
                sqlList.Add(sql);
                //재등록
                sql = priceComparisonTempRepository.InsertSettingData(model);
                sqlList.Add(sql);
            }

            if (commonRepository.UpdateTran(sqlList) == -1)
                messageBox.Show(this, "등록중 에러가 발생하였습니다.");
            else
            {
                for (int i = dgvProduct.Rows.Count - 1; i >= 0; i--)
                {
                    if (dgvProduct.Rows[i].Selected)
                        dgvProduct.Rows.Remove(dgvProduct.Rows[i]);
                }
            }
        }
        private void UpdateContentRemark(DataGridViewRow row)
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            dgvProduct.EndEdit();
            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();
            PriceComparisonTempSettingModel model = new PriceComparisonTempSettingModel();
            model.product_code = row.Cells["product"].Value.ToString()
                        + "_" + row.Cells["origin"].Value.ToString()
                        + "_" + row.Cells["sizes"].Value.ToString()
                        + "_" + row.Cells["unit"].Value.ToString()
                        + "_" + row.Cells["price_unit"].Value.ToString()
                        + "_" + row.Cells["unit_count"].Value.ToString()
                        + "_" + row.Cells["seaover_unit"].Value.ToString();

            bool is_hide;
            if (row.Cells["is_hide"].Value == null || !bool.TryParse(row.Cells["is_hide"].Value.ToString(), out is_hide))
                is_hide = false;
            model.is_hide = is_hide;
            DateTime confirmation_date;
            if (row.Cells["confirmation_date"].Value == null || !DateTime.TryParse(row.Cells["confirmation_date"].Value.ToString(), out confirmation_date))
                confirmation_date = new DateTime(1900, 1, 1);
            model.confirmation_date = confirmation_date.ToString("yyyy-MM-dd");
            if (row.Cells["contents"].Value != null)
                model.contents = row.Cells["contents"].Value.ToString();
            if (row.Cells["remark"].Value != null)
                model.remark = row.Cells["remark"].Value.ToString();
            model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            model.edit_user = um.user_name;
            //삭제
            sql = priceComparisonTempRepository.DeleteSettingData(model.product_code);
            sqlList.Add(sql);
            //재등록
            sql = priceComparisonTempRepository.InsertSettingData(model);
            sqlList.Add(sql);

            if (commonRepository.UpdateTran(sqlList) == -1)
                messageBox.Show(this, "등록중 에러가 발생하였습니다.");
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }
        #endregion

        #region 우클릭 메뉴
        ContextMenuStrip m;
        private void dgvProduct_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    if (dgvProduct.SelectedRows.Count == 0)
                        return;
                    //한영변환
                    ChangeIME(false);

                    hitTestInfo = dgvProduct.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    m = new ContextMenuStrip();
                    if (tabDgv.SelectedTab.Name == "tpNew" || tabDgv.SelectedTab.Name == "tpAll")
                    {
                        m.Items.Add("'보류'시트로 옮기기");
                        m.Items.Add("'취급X'시트로 옮기기");
                    }
                    else if (tabDgv.SelectedTab.Name == "tpShortHold" || tabDgv.SelectedTab.Name == "tpLongHold")
                    {
                        m.Items.Add("'보류'시트로 옮기기");
                        m.Items.Add("'보류'시트에서 내보내기");
                        m.Items.Add("'취급X'시트로 옮기기");
                    }
                    else if (tabDgv.SelectedTab.Name == "tpHide")
                    {
                        m.Items.Add("'보류'시트로 옮기기");
                        m.Items.Add("'취급X'시트에서 내보내기");
                    }
                    ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
                    toolStripSeparator.Name = "toolStripSeparator";
                    toolStripSeparator.Size = new System.Drawing.Size(119, 6);
                    m.Items.Add(toolStripSeparator);
                    m.Items.Add("상세 대시보드(다중)");
                    m.Items.Add("공장별 취급품목 대시보드");

                    /*ToolStripSeparator toolStripSeparator0 = new ToolStripSeparator();
                    toolStripSeparator0.Name = "toolStripSeparator";
                    toolStripSeparator0.Size = new System.Drawing.Size(119, 6);
                    m.Items.Add(toolStripSeparator0);*/

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
            catch (Exception ex)
            {
                messageBox.Show(this, ex.Message);
                this.Activate();
            }
        }
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (dgvProduct.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow dr = dgvProduct.SelectedRows[0];
                    if (dr.Index < 0)
                        return;
                    int rowindex = dr.Index;

                    //함수호출
                    switch (e.ClickedItem.Text)
                    {
                        case "'보류'시트로 옮기기":
                            ChangeToCheckList(true);
                            break;
                        case "'취급X'시트로 옮기기":
                            ChangeToNotHandled(true);
                            break;
                        case "'보류'시트에서 내보내기":
                            ChangeToCheckList(false);
                            break;
                        case "'취급X'시트에서 내보내기":
                            ChangeToNotHandled(false);
                            break;
                        case "상세 대시보드(다중)":
                            {
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
                                        if (messageBox.Show(this, "새창으로 대시보드를 여시겠습니까?" + "\n Yes : 새창       No : 기존창", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                        case "공장별 취급품목 대시보드":
                            {
                                List<DataGridViewRow> productList = new List<DataGridViewRow>();
                                foreach (DataGridViewRow productRow in dgvProduct.SelectedRows)
                                    productList.Add(productRow);
                                
                                SelectProduct2 sp = null;
                                FormCollection fc = Application.OpenForms;
                                foreach (Form frm in fc)
                                {
                                    //iterate through
                                    if (frm.Name == "SelectProduct2")
                                    {
                                        if (messageBox.Show(this, "새창으로 대시보드를 여시겠습니까?" + "\n Yes : 새창       No : 기존창", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                            sp = null;
                                        else
                                        {
                                            sp = (SelectProduct2)frm;
                                            sp.AddProduct(productList);
                                            sp.Activate();
                                        }

                                        break;
                                    }
                                }
                                //새로열기
                                if (sp == null)
                                {
                                    try
                                    {
                                        sp = new SelectProduct2(um, productList);
                                        sp.Show();
                                    }
                                    catch
                                    { }
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
                case Keys.Enter:
                    break;
            }
        }

        private void dgvProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && dgvProduct.SelectedRows.Count <= 1)
                {
                    dgvProduct.ClearSelection();
                    dgvProduct.Rows[e.RowIndex].Selected = true;
                }
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

        #endregion

        #region Button, CheckBox, ComboBox
        private void rbAll_AppearanceChanged(object sender, EventArgs e)
        {
            if (rbAll.Checked)
                SetNewOutProductDataTable();
            else if (rbIncome.Checked)
                SetNewOutProductDataTable();
            else if (rbNew.Checked)
                SetNewOutProductDataTable();
        }
        private void cbSaleTerm_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetHeaderStyle();
        }
        private void cbStockDetail_CheckedChanged(object sender, EventArgs e)
        {
            SetHeaderStyle();
        }
        private void cbSalesDetail_CheckedChanged(object sender, EventArgs e)
        {
            SetHeaderStyle();
        }

        private void cbAllSaleTerms_CheckedChanged(object sender, EventArgs e)
        {
            SetHeaderStyle();
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            SetNewOutProductDataTable();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void tabDgv_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabDgv.SelectedTab.Controls.Add(this.pnRecord);
            tabDgv.SelectedTab.Controls.Add(this.dgvProduct);
            this.dgvProduct.Dock = DockStyle.Fill;
            this.dgvProduct.BringToFront();
            SetNewOutProductDataTable();
        }
        private void cbSortType_TextChanged(object sender, EventArgs e)
        {
            //탭별 검색조건
            string whrStr = "";
            if (tabDgv.SelectedTab.Name == "tpNew")
                whrStr = $"sheet_type = 0 AND is_hide = 'FALSE' ";
            else if (tabDgv.SelectedTab.Name == "tpShortHold")
                whrStr = $"sheet_type = 1 AND is_hide = 'FALSE' ";
            else if (tabDgv.SelectedTab.Name == "tpLongHold")
                whrStr = $"sheet_type = 2 AND is_hide = 'FALSE' ";
            else if (tabDgv.SelectedTab.Name == "tpHide")
                whrStr = $"is_hide = 'TRUE'";
            else if (tabDgv.SelectedTab.Name == "tpAll")
                whrStr = $"is_hide = 'FALSE'";

            DataRow[] dr = mainDt.Select(whrStr);
            if (dr.Length > 0)
            {
                mainDt = dr.CopyToDataTable();
                //정렬
                DataView dv = new DataView(mainDt);
                string sortStr = sortString(cbSortType.Text);
                if (!string.IsNullOrEmpty(sortStr))
                    sortStr = sortStr + ", product, origin, sizes, unit, price_unit, unit_count, seaover_unit";
                else
                    sortStr = "is_new DESC, product, origin, sizes, unit, price_unit, unit_count, seaover_unit";
                dv.Sort = sortStr;
                mainDt = dv.ToTable();
                dgvProduct.DataSource = mainDt;
            }
        }

        #endregion

        #region Key event
        private void AlmostOutOfStockManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        SetNewOutProductDataTable();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.M:
                        txtProduct.Focus();
                        break;
                    case Keys.N:
                        txtProduct.Text = string.Empty;
                        txtOrigin.Text = string.Empty;
                        txtSizes.Text = string.Empty;
                        txtUnit.Text = string.Empty;
                        txtDivision.Text = string.Empty;
                        txtManager.Text = string.Empty;
                        txtContetns.Text = string.Empty;
                        txtRemark.Text = string.Empty;
                        txtProduct.Focus();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        cbStockDetail.Checked = !cbStockDetail.Checked;
                        break;
                    case Keys.F3:
                        cbSalesDetail.Checked = !cbSalesDetail.Checked;
                        break;
                    case Keys.F4:
                        cbAllSaleTerms.Checked = !cbAllSaleTerms.Checked;
                        break;
                }
            }
        }

        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
            }
            else if (e.Modifiers == Keys.Alt)
            {
            }
            else 
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        SetNewOutProductDataTable();
                        break;
                }
            }
        }
        #endregion

        #region Datagridview evnet
        private void dgvProduct_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string numberString = dgvProduct.Rows[e.RowIndex].Cells["is_new"].Value as string;
                if (numberString == "new")
                {
                    dgvProduct.Rows[e.RowIndex].HeaderCell.Style.Font = new Font("굴림", 9, FontStyle.Bold);
                    dgvProduct.Rows[e.RowIndex].HeaderCell.Style.ForeColor = Color.Red;
                    dgvProduct.Rows[e.RowIndex].HeaderCell.Value = "NEW";
                }
            }
        }

        private void dgvProduct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "contents" || dgvProduct.Columns[e.ColumnIndex].Name == "remark")
                    UpdateContentRemark(dgvProduct.Rows[e.RowIndex]);
            }
        }
        private void dgvProduct_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProduct.CurrentCell != null)
                txtCurrentRecord.Text = (dgvProduct.CurrentCell.RowIndex + 1).ToString("#,##0");
        }
        #endregion

        private void dgvProduct_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            int a = int.Parse(e.CellValue1.ToString()), b = int.Parse(e.CellValue2.ToString());
            e.SortResult = a.CompareTo(b);
            e.Handled = true;
        }

        string isSortDirc = "ASC";
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
            }
            else
            {
                dgvProduct.Sort(
                    dgvProduct.Columns[e.ColumnIndex],
                    ListSortDirection.Descending
                );
            }
            //다시 변경
            if (isSortDirc == "ASC")
                isSortDirc = "DESC";
            else
                isSortDirc = "ASC";
        }

        
    }
}
