using AdoNetWindow.Common;
using AdoNetWindow.Model;
using AdoNetWindow.Product;
using AdoNetWindow.PurchaseManager;
using Repositories;
using Repositories.SEAOVER;
using Repositories.SEAOVER.Purchase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace AdoNetWindow.SEAOVER.PriceComparison
{
    public partial class AlarmContractItem : Form
    {
        IProductGroupRepository productGroupRepository = new ProductGroupRepository();
        IPriceComparisonRepository priceComparisonRepository = new PriceComparisonRepository();
        IProductOtherCostRepository productOtherCostRepository = new ProductOtherCostRepository();
        IPurchasePriceRepository purchasePriceRepository = new PurchasePriceRepository();
        IPurchaseRepository purchaseRepository = new PurchaseRepository();
        UsersModel um;
        Libs.Tools.Common common = new Libs.Tools.Common();
        CalendarModule.calendar cal;

        DataTable managementDt = new DataTable();
        DataTable monthDt = new DataTable();
        DataTable operatingDt = new DataTable();
        DataTable contractDt = new DataTable();
        DataTable hideDt = new DataTable();
        DataTable allDt = new DataTable();
        DataTable productDt = new DataTable();
        DataTable copDt = new DataTable();

        public AlarmContractItem(CalendarModule.calendar calendar, UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
            cal = calendar;
            txtManager.Text = um.user_name;
            //최근오퍼일자
            copDt = purchasePriceRepository.GetCurrentUpdatetime();
        }

        private void AlarmContractItem_Load(object sender, EventArgs e)
        {
            SetHeaderStyle();
            SetCbMonthCheck();
        }
        
        #region Method
        private void SetHeaderStyle()
        {
            DataGridView dgv = dgvProduct;
            //헤더 디자인
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.SeaGreen;

            Color darkBlue = Color.FromArgb(43, 94, 170);    //남색
            dgv.ColumnHeadersDefaultCellStyle.BackColor = darkBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
        }
        public void CalendarOpenAlarm(bool isMsg = false)
        {
            //SetColumnHeaderStyleSetting();
            tabDgv.SelectedIndex = 0;
            int cnt = GetData();
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


        private void SetCbMonthCheck()
        {
            int month = DateTime.Now.Month;

            switch (month)
            {
                case 1:
                    cbDec.Checked = true;
                    cbJan.Checked = true;
                    cbFeb.Checked = true;

                    cbConDec.Checked = true;
                    cbConJan.Checked = true;
                    cbConFeb.Checked = true;
                    break;
                case 2:
                    cbJan.Checked = true;
                    cbFeb.Checked = true;
                    cbMar.Checked = true;

                    cbConJan.Checked = true;
                    cbConFeb.Checked = true;
                    cbConMar.Checked = true;
                    break;
                case 3:
                    cbFeb.Checked = true;
                    cbMar.Checked = true;
                    cbApr.Checked = true;

                    cbConFeb.Checked = true;
                    cbConMar.Checked = true;
                    cbConApr.Checked = true;
                    break;
                case 4:
                    cbMar.Checked = true;
                    cbApr.Checked = true;
                    cbMay.Checked = true;

                    cbConMar.Checked = true;
                    cbConApr.Checked = true;
                    cbConMay.Checked = true;
                    break;
                case 5:
                    cbApr.Checked = true;
                    cbMay.Checked = true;
                    cbJun.Checked = true;

                    cbConApr.Checked = true;
                    cbConMay.Checked = true;
                    cbConJun.Checked = true;
                    break;
                case 6:
                    cbMay.Checked = true;
                    cbJun.Checked = true;
                    cbJul.Checked = true;

                    cbConMay.Checked = true;
                    cbConJun.Checked = true;
                    cbConJul.Checked = true;
                    break;
                case 7:
                    cbJun.Checked = true;
                    cbJul.Checked = true;
                    cbAug.Checked = true;

                    cbConJun.Checked = true;
                    cbConJul.Checked = true;
                    cbConAug.Checked = true;
                    break;
                case 8:
                    cbJul.Checked = true;
                    cbAug.Checked = true;
                    cbSep.Checked = true;

                    cbConJul.Checked = true;
                    cbConAug.Checked = true;
                    cbConSep.Checked = true;
                    break;
                case 9:
                    cbAug.Checked = true;
                    cbSep.Checked = true;
                    cbOct.Checked = true;

                    cbConAug.Checked = true;
                    cbConSep.Checked = true;
                    cbConOct.Checked = true;
                    break;
                case 10:
                    cbSep.Checked = true;
                    cbOct.Checked = true;
                    cbNov.Checked = true;

                    cbConSep.Checked = true;
                    cbConOct.Checked = true;
                    cbConNov.Checked = true;
                    break;
                case 11:
                    cbOct.Checked = true;
                    cbNov.Checked = true;
                    cbDec.Checked = true;

                    cbConOct.Checked = true;
                    cbConNov.Checked = true;
                    cbConDec.Checked = true;
                    break;
                case 12:
                    cbNov.Checked = true;
                    cbDec.Checked = true;
                    cbJan.Checked = true;

                    cbConNov.Checked = true;
                    cbConDec.Checked = true;
                    cbConJan.Checked = true;
                    break;
            }
        }



        public int GetData()
        {
            //조업시기 검색
            string operating = "";
            if (tabDgv.SelectedTab.Name == "tabOperating")
            { 
                if (cbJan.Checked)
                    operating += " 1";
                if (cbFeb.Checked)
                    operating += " 2";
                if (cbMar.Checked)
                    operating += " 3";
                if (cbApr.Checked)
                    operating += " 4";
                if (cbMay.Checked)
                    operating += " 5";
                if (cbJun.Checked)
                    operating += " 6";
                if (cbJul.Checked)
                    operating += " 7";
                if (cbAug.Checked)
                    operating += " 8";
                if (cbSep.Checked)
                    operating += " 9";
                if (cbOct.Checked)
                    operating += " 10";
                if (cbNov.Checked)
                    operating += " 11";
                if (cbDec.Checked)
                    operating += " 12";
            }
            //계약시기 검색
            string contract = "";
            if (tabDgv.SelectedTab.Name == "tabContract")
            {
                if (cbConJan.Checked)
                    contract += " 1";
                if (cbConFeb.Checked)
                    contract += " 2";
                if (cbConMar.Checked)
                    contract += " 3";
                if (cbConApr.Checked)
                    contract += " 4";
                if (cbConMay.Checked)
                    contract += " 5";
                if (cbConJun.Checked)
                    contract += " 6";
                if (cbConJul.Checked)
                    contract += " 7";
                if (cbConAug.Checked)
                    contract += " 8";
                if (cbConSep.Checked)
                    contract += " 9";
                if (cbConOct.Checked)
                    contract += " 10";
                if (cbConNov.Checked)
                    contract += " 11";
                if (cbConDec.Checked)
                    contract += " 12";
            }

            //정렬기준
            int sortType;
            switch (cbSortType.Text)
            {
                case "알람+계약시즌+품명+원산지+규격":
                    sortType = 0;
                    break;
                case "추천계약일+품명+원산지+규격":
                    sortType = 1;
                    break;
                case "추천계약일+月판매+품명+원산지+규격":
                    sortType = 2;
                    break;
                case "추천계약일+재고+품명+원산지+규격":
                    sortType = 3;
                    break;
                case "최소ETD+품명+원산지+규격":
                    sortType = 4;
                    break;
                case "최소ETD+月판매+품명+원산지+규격":
                    sortType = 5;
                    break;
                case "최소ETD+재고+품명+원산지+규격":
                    sortType = 6;
                    break;
                case "품명+원산지+규격":
                    sortType = 7;
                    break;
                default:
                    sortType = 0;
                    break;
            }

            //매출기간
            int sales_term = 6;
            int workDays = 0;
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
            if (sales_term == 45)
            {
                enddate = DateTime.Now.AddDays(-1);
                sttdate = DateTime.Now.AddDays(-1).AddDays(-sales_term);
            }
            else
            {
                enddate = DateTime.Now.AddDays(-1);
                sttdate = DateTime.Now.AddDays(-1).AddMonths(-sales_term);
            }
            common.GetWorkDay(sttdate, enddate, out workDays);

            //탭
            int tabType;
            switch (tabDgv.SelectedTab.Text)
            {
                case "전체":
                    tabType = 1;
                    allDt = priceComparisonRepository.GetTempPriceComparison(DateTime.Now, txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtManager.Text, txtDivision.Text, cbNoStock.Checked, operating, contract, sortType, tabType, false, false, sales_term, true);
                    allDt = SetProductDt(allDt);
                    productDt = allDt;
                    break;
                case "관심":
                    tabType = 2;
                    managementDt = priceComparisonRepository.GetTempPriceComparison(DateTime.Now, txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtManager.Text, txtDivision.Text, cbNoStock.Checked, operating, contract, sortType, tabType, cbUserSetting.Checked, cbContract.Checked, sales_term);
                    managementDt = SetProductDt(managementDt);
                    productDt = managementDt;
                    break;
                case "월별":
                    tabType = 3;
                    monthDt = priceComparisonRepository.GetTempPriceComparison(DateTime.Now, txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtManager.Text, txtDivision.Text, cbNoStock.Checked, operating, contract, sortType, tabType, cbUserSetting2.Checked, cbContract2.Checked, sales_term);
                    monthDt = SetProductDt(monthDt);
                    productDt = monthDt;
                    break;
                case "조업시즌":
                    tabType = 4;
                    operatingDt = priceComparisonRepository.GetTempPriceComparison(DateTime.Now, txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtManager.Text, txtDivision.Text, cbNoStock.Checked, operating, contract, sortType, tabType, false, false, sales_term);
                    operatingDt = SetProductDt(operatingDt);
                    productDt = operatingDt;
                    break;
                case "계약시즌":
                    tabType = 5;
                    contractDt = priceComparisonRepository.GetTempPriceComparison(DateTime.Now, txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtManager.Text, txtDivision.Text, cbNoStock.Checked, operating, contract, sortType, tabType, false, false, sales_term);
                    contractDt = SetProductDt(contractDt);
                    productDt = contractDt;
                    break;
                case "알람":
                    tabType = 6;
                    break;
                case "숨김품목":
                    tabType = 7;
                    hideDt = priceComparisonRepository.GetTempPriceComparison(DateTime.Now, txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtManager.Text, txtDivision.Text, cbNoStock.Checked, operating, contract, sortType, tabType, false, false, sales_term);
                    hideDt = SetProductDt(hideDt);
                    productDt = hideDt;
                    break;
                default:
                    tabType = 1;
                    allDt = priceComparisonRepository.GetTempPriceComparison(DateTime.Now, txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtManager.Text, txtDivision.Text, cbNoStock.Checked, operating, contract, sortType, tabType, false, false, sales_term);
                    allDt = SetProductDt(allDt);
                    productDt = allDt;
                    break;
            }

            

            
            //데이터 출력
            dgvProduct.DataSource = null;

            if (productDt == null)
            {
                productDt = new DataTable();
                DataColumn imgColumn = new DataColumn();
                imgColumn.DataType = typeof(Image);
                imgColumn.ColumnName = "img";
                productDt.Columns.Add(imgColumn);
                //productDt.Columns["offer_updatetime"].MaxLength = 12;
                productDt.AcceptChanges();
            }

            dgvProduct.DataSource = productDt;
            SetHeaderStyleSetting();

            for (int i = dgvProduct.Rows.Count - 1; i >= 0 ; i--)
            {
                if (dgvProduct.Rows[i].Cells["main_id"].Value != null && Convert.ToInt16(dgvProduct.Rows[i].Cells["main_id"].Value.ToString()) > 0)
                {
                    int sub_id;
                    if (int.TryParse(dgvProduct.Rows[i].Cells["sub_id"].Value.ToString(), out sub_id))
                    {
                        if (sub_id == 9999)
                        {
                            dgvProduct.Rows[i].HeaderCell.Value = "+";
                            dgvProduct.Rows[i].HeaderCell.Style.ForeColor = Color.Yellow;
                            dgvProduct.Rows[i].HeaderCell.Style.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                            dgvProduct.Rows[i].Cells["product"].Style.BackColor = Color.DarkOrange;
                            dgvProduct.Rows[i].Cells["product"].Style.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                        }
                        else
                        {
                            dgvProduct.Rows[i].Visible = false;
                        }
                    }
                }
            }

            if(productDt == null)
                return 0;
            else
                return productDt.Rows.Count;
            //return productDt.Rows.Count;
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

        private DataTable SetProductDt(DataTable productDt)
        {
            //수입했던 품목만 OR 신규품목만
            if (rbIncome.Checked || rbNew.Checked)
            {
                DataTable incomeDt = purchaseRepository.GetPurchaseIncomProduct(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);


                var product = (from p in productDt.AsEnumerable()
                               join t in incomeDt.AsEnumerable()
                               on p.Field<string>("product_code") equals t.Field<string>("품목코드")
                               into outer
                               from t in outer.DefaultIfEmpty()
                               select new
                               {
                                   main_id = p.Field<int>("main_id"),
                                   sub_id = p.Field<int>("sub_id"),
                                   product = p.Field<string>("product"),
                                   origin = p.Field<string>("origin"),
                                   sizes = p.Field<string>("sizes"),
                                   unit = p.Field<string>("unit"),
                                   price_unit = p.Field<string>("price_unit"),
                                   unit_count = p.Field<string>("unit_count"),
                                   seaover_unit = p.Field<string>("seaover_unit"),
                                   shipment_stock = p.Field<double>("shipment_stock"),
                                   stock = p.Field<double>("stock"),
                                   sales_count = p.Field<double>("sales_count"),
                                   month_sales_count = p.Field<double>("month_sales_count"),
                                   works_day = p.Field<double>("works_day"),
                                   month_around = p.Field<double>("month_around"),
                                   exhaustion_date = p.Field<string>("exhaustion_date"),
                                   contract_date = p.Field<string>("contract_date"),
                                   until_days1 = p.Field<string>("until_days1"),
                                   min_etd = p.Field<string>("min_etd"),
                                   until_days2 = p.Field<string>("until_days2"),
                                   isMonth = p.Field<string>("isMonth"),
                                   show_sttdate = p.Field<string>("show_sttdate"),
                                   show_enddate = p.Field<string>("show_enddate"),
                                   hide_date = p.Field<string>("hide_date"),
                                   trade_manager = p.Field<string>("trade_manager"),
                                   contract = p.Field<string>("contract"),
                                   operating = p.Field<string>("operating"),
                                   show_product = p.Field<string>("show_product"),
                                   contract_product = p.Field<string>("contract_product"),
                                   month_product = p.Field<string>("month_product"),
                                   offer_updatetime = p.Field<string>("offer_updatetime"),
                                   isIncome = (t == null) ? "FALSE" : "TRUE"
                               }).ToList();

                DataTable tempDt = ConvertListToDatatable(product);
                if (tempDt != null)
                {
                    for (int i = tempDt.Rows.Count - 1; i >= 0; i--)
                    {
                        if (rbIncome.Checked)
                        {
                            if (!Convert.ToBoolean(tempDt.Rows[i]["isIncome"].ToString()) && !(Convert.ToInt16(tempDt.Rows[i]["main_id"].ToString()) > 0 && Convert.ToInt16(tempDt.Rows[i]["sub_id"].ToString()) < 9999))
                                tempDt.Rows.Remove(tempDt.Rows[i]);
                        }
                        else if (rbNew.Checked)
                        {
                            if (Convert.ToBoolean(tempDt.Rows[i]["isIncome"].ToString()) && !(Convert.ToInt16(tempDt.Rows[i]["main_id"].ToString()) > 0 && Convert.ToInt16(tempDt.Rows[i]["sub_id"].ToString()) < 9999))
                                tempDt.Rows.Remove(tempDt.Rows[i]);
                        }
                    }
                    tempDt.AcceptChanges();
                    DataColumn imgColumn = new DataColumn();
                    imgColumn.DataType = typeof(Image);
                    imgColumn.ColumnName = "img";
                    tempDt.Columns.Add(imgColumn);
                    tempDt.Columns["offer_updatetime"].MaxLength = 12;
                    tempDt.AcceptChanges();
                    for (int i = 0; i < tempDt.Rows.Count; i++)
                    {
                        tempDt.Rows[i]["month_sales_count"] = Convert.ToDouble(tempDt.Rows[i]["month_sales_count"].ToString()).ToString("#,#00");
                        tempDt.Rows[i]["month_around"] = Convert.ToDouble(tempDt.Rows[i]["month_around"].ToString()).ToString("#,#00.00");
                        tempDt.Rows[i]["contract"] = tempDt.Rows[i]["contract"].ToString().Replace("^", "").Replace(",", ", ");
                        tempDt.Rows[i]["operating"] = tempDt.Rows[i]["operating"].ToString().Replace("^", "").Replace(",", ", ");
                        //남은일
                        int workDays;
                        //추천계약일
                        DateTime contract_date;
                        if (DateTime.TryParse(tempDt.Rows[i]["contract_date"].ToString(), out contract_date))
                        {
                            if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) <= contract_date)
                            {
                                common.GetWorkDay(DateTime.Now, contract_date, out workDays);
                                tempDt.Columns["until_days1"].MaxLength = 10;
                                tempDt.Rows[i]["until_days1"] = workDays.ToString("#,##0");
                            }
                            else
                            {
                                common.GetWorkDay(contract_date, DateTime.Now, out workDays);
                                tempDt.Columns["until_days1"].MaxLength = 10;
                                tempDt.Rows[i]["until_days1"] = (-workDays).ToString("#,##0");
                            }
                        }

                        //최소 ETD
                        DateTime minimal_etd;
                        if (DateTime.TryParse(tempDt.Rows[i]["min_etd"].ToString(), out minimal_etd))
                        {
                            //남은일
                            if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) <= minimal_etd)
                            {
                                common.GetWorkDay(DateTime.Now, minimal_etd, out workDays);
                                tempDt.Columns["until_days2"].MaxLength = 10;
                                tempDt.Rows[i]["until_days2"] = workDays.ToString("#,##0");
                            }
                            else
                            {
                                common.GetWorkDay(minimal_etd, DateTime.Now, out workDays);
                                tempDt.Columns["until_days2"].MaxLength = 10;
                                tempDt.Rows[i]["until_days2"] = (-workDays).ToString("#,##0");
                            }
                        }


                        //최근오퍼일자
                        if (copDt.Rows.Count > 0)
                        {
                            string whr = "product = '" + tempDt.Rows[i]["product"].ToString() + "'"
                                            + " AND origin = '" + tempDt.Rows[i]["origin"].ToString() + "'"
                                            + " AND sizes = '" + tempDt.Rows[i]["sizes"].ToString() + "'"
                                            + " AND unit = '" + tempDt.Rows[i]["seaover_unit"].ToString() + "'";
                            DataRow[] dr = copDt.Select(whr);
                            if (dr.Length > 0)
                            {
                                tempDt.Rows[i]["offer_updatetime"] = Convert.ToDateTime(dr[0]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                            }
                        }

                        //이미지
                        if (tempDt.Rows[i]["show_product"].ToString() == "1")
                            tempDt.Rows[i]["img"] = Properties.Resources.Clock_Icon;
                        else if (tempDt.Rows[i]["contract_product"].ToString() == "1")
                            tempDt.Rows[i]["img"] = Properties.Resources.Paper_icon;
                        else
                            tempDt.Rows[i]["img"] = null;
                    }
                }

                productDt = tempDt;
            }
            else
            {
                DataColumn imgColumn = new DataColumn();
                imgColumn.DataType = typeof(Image);
                imgColumn.ColumnName = "img";
                productDt.Columns.Add(imgColumn);
                productDt.Columns["offer_updatetime"].MaxLength = 12;
                productDt.AcceptChanges();
            }
            /*//신규품목만
            else if (rbNew.Checked)
            {
                DataTable incomeDt = purchaseRepository.GetPurchaseIncomProduct(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
                 var product = (from p in productDt.AsEnumerable()
                               join t in incomeDt.AsEnumerable()
                               on p.Field<string>("product_code") equals t.Field<string>("품목코드")
                               into outer
                               from t in outer.DefaultIfEmpty()
                               select new
                               {
                                   main_id = p.Field<int>("main_id"),
                                   sub_id = p.Field<int>("sub_id"),
                                   product = p.Field<string>("product"),
                                   origin = p.Field<string>("origin"),
                                   sizes = p.Field<string>("sizes"),
                                   unit = p.Field<string>("unit"),
                                   price_unit = p.Field<string>("price_unit"),
                                   unit_count = p.Field<string>("unit_count"),
                                   seaover_unit = p.Field<string>("seaover_unit"),
                                   shipment_stock = p.Field<double>("shipment_stock"),
                                   stock = p.Field<double>("stock"),
                                   sales_count = p.Field<double>("sales_count"),
                                   month_sales_count = p.Field<double>("month_sales_count"),
                                   works_day = p.Field<double>("works_day"),
                                   month_around = p.Field<double>("month_around"),
                                   exhaustion_date = p.Field<string>("exhaustion_date"),
                                   contract_date = p.Field<string>("contract_date"),
                                   until_days1 = p.Field<string>("until_days1"),
                                   min_etd = p.Field<string>("min_etd"),
                                   until_days2 = p.Field<string>("until_days2"),
                                   isMonth = p.Field<string>("isMonth"),
                                   show_sttdate = p.Field<string>("show_sttdate"),
                                   show_enddate = p.Field<string>("show_enddate"),
                                   hide_date = p.Field<string>("hide_date"),
                                   trade_manager = p.Field<string>("trade_manager"),
                                   contract = p.Field<string>("contract"),
                                   operating = p.Field<string>("operating"),
                                   show_product = p.Field<string>("show_product"),
                                   contract_product = p.Field<string>("contract_product"),
                                   month_product = p.Field<string>("month_product"),
                                   offer_updatetime = p.Field<string>("offer_updatetime"),
                                   isDelete = (t == null) ? "FALSE" : "TRUE"
                               }).ToList();

                DataTable tempDt = ConvertListToDatatable(product);
                productDt = tempDt;


                for (int i = productDt.Rows.Count - 1; i >= 0; i--)
                { 
                    bool isDelete = Convert.ToBoolean(productDt.Rows[i]["isDelete"].ToString());
                    if (isDelete)
                        productDt.Rows.Remove(productDt.Rows[i]);
                }
                productDt.AcceptChanges();
            }*/


            
            return productDt;
        }

        private void SetHeaderStyleSetting()
        {
            DataGridView dgv = this.dgvProduct;
            dgv.Update();
            if (dgv.ColumnCount > 1)
            {
                dgv.Columns["product"].HeaderText = "품명";
                dgv.Columns["origin"].HeaderText = "원산지";
                dgv.Columns["sizes"].HeaderText = "규격";
                dgv.Columns["unit"].HeaderText = "단위";
                dgv.Columns["price_unit"].HeaderText = "가격단위";
                dgv.Columns["unit_count"].HeaderText = "단위수량";
                dgv.Columns["seaover_unit"].HeaderText = "S단위";
                dgv.Columns["shipment_stock"].HeaderText = "선적";
                dgv.Columns["shipment_stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["shipment_stock"].DefaultCellStyle.Format = "#,##0";
                dgv.Columns["stock"].HeaderText = "재고";
                dgv.Columns["stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["stock"].DefaultCellStyle.Format = "#,##0";
                dgv.Columns["sales_count"].HeaderText = "매출";
                dgv.Columns["sales_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["sales_count"].DefaultCellStyle.Format = "#,##0";
                dgv.Columns["month_sales_count"].HeaderText = "月매출";
                dgv.Columns["month_sales_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["month_sales_count"].DefaultCellStyle.Format = "#,##0";
                dgv.Columns["month_around"].HeaderText = "회전율";
                dgv.Columns["month_around"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["exhaustion_date"].HeaderText = "쇼트일자";
                dgv.Columns["contract_date"].HeaderText = "추천계약일";
                dgv.Columns["min_etd"].HeaderText = "최소ETD";
                dgv.Columns["until_days1"].HeaderText = "남은일1";
                dgv.Columns["until_days1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["until_days2"].HeaderText = "남은일2";
                dgv.Columns["until_days2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["trade_manager"].HeaderText = "담당자";

                dgv.Columns["offer_updatetime"].HeaderText = "최근오퍼일";

                dgv.Columns["contract"].HeaderText = "계약시즌";
                dgv.Columns["operating"].HeaderText = "조업시즌";
                dgv.Columns["contract"].Visible = cbContractMakeTerm.Checked;
                dgv.Columns["operating"].Visible = cbContractMakeTerm.Checked;

                dgv.Columns["main_id"].Visible = false;
                dgv.Columns["sub_id"].Visible = false;
                dgv.Columns["sales_count"].Visible = false;
                dgv.Columns["works_day"].Visible = false;
                dgv.Columns["isMonth"].Visible = false;
                dgv.Columns["show_sttdate"].Visible = false;
                dgv.Columns["show_enddate"].Visible = false;
                dgv.Columns["hide_date"].Visible = false;
                dgv.Columns["show_product"].Visible = false;
                dgv.Columns["contract_product"].Visible = false;
                dgv.Columns["month_product"].Visible = false;

                if(rbAll.Checked)
                    dgv.Columns["product_code"].Visible = false;
                else 
                    dgv.Columns["isIncome"].Visible = false;

                dgv.Columns["product"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgv.Columns["contract"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgv.Columns["operating"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dgv.Columns["origin"].Width = 100;
                dgv.Columns["sizes"].Width = 100;
                dgv.Columns["unit"].Width = 60;
                dgv.Columns["price_unit"].Width = 60;
                dgv.Columns["unit_count"].Width = 60;
                dgv.Columns["seaover_unit"].Width = 60;
                dgv.Columns["shipment_stock"].Width = 60;
                dgv.Columns["stock"].Width = 60;
                dgv.Columns["sales_count"].Width = 60;
                dgv.Columns["month_sales_count"].Width = 60;
                dgv.Columns["month_around"].Width = 60;
                dgv.Columns["exhaustion_date"].Width = 80;
                dgv.Columns["contract_date"].Width = 80;
                dgv.Columns["min_etd"].Width = 80;
                dgv.Columns["until_days1"].Width = 60;
                dgv.Columns["until_days2"].Width = 60;
                dgv.Columns["trade_manager"].Width = 60;
                dgv.Columns["offer_updatetime"].Width = 70;
                try
                {
                    dgv.Columns["img"].Width = 30;
                    dgv.Columns["img"].HeaderText = "";
                    dgv.Columns["img"].DisplayIndex = 0;
                    dgv.Columns["img"].ValueType = typeof(Image);
                    DataGridViewImageColumn img = (DataGridViewImageColumn)dgv.Columns["img"];
                    img.ImageLayout = DataGridViewImageCellLayout.Zoom;
                    dgv.Columns["img"].DefaultCellStyle.NullValue = null;

                    switch (tabDgv.SelectedTab.Text)
                    {
                        case "전체":
                            dgv.Columns["img"].Visible = true;
                            break;
                        case "관심":
                            dgv.Columns["img"].Visible = true;
                            break;
                        case "숨김품목":
                            dgv.Columns["img"].Visible = true;
                            break;
                        default:
                            dgv.Columns["img"].Visible = false;
                            break;
                    }
                }
                catch
                { }
            }
        }
        #endregion

        #region Key event
        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetData();
        }
        private void AlarmContractItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        GetData();
                        break;
                    case Keys.M:
                        txtProduct.Focus();
                        break;
                    case Keys.N:
                        txtProduct.Text = String.Empty;
                        txtOrigin.Text = String.Empty;
                        txtSizes.Text = String.Empty;
                        txtUnit.Text = String.Empty;
                        txtManager.Text = String.Empty;
                        txtDivision.Text = String.Empty;
                        txtProduct.Focus();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
            else
            {
            }
        }
        #endregion

        #region Radio, Button
        private void cbContractMakeTerm_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvProduct.ColumnCount > 0)
            {
                dgvProduct.Columns["contract"].Visible = cbContractMakeTerm.Checked;
                dgvProduct.Columns["operating"].Visible = cbContractMakeTerm.Checked;
            }
        }
        private void rbEtd_CheckedChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Datagridview, Tab event
        private void dgvProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dgvProduct.SelectedRows.Count == 1)
                {
                    dgvProduct.ClearSelection();
                    dgvProduct.Rows[e.RowIndex].Selected = true;
                }
            }
        }

        private void dgvProduct_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    double until_days1;
                    if (dgvProduct.Rows[e.RowIndex].Cells["until_days1"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["until_days1"].Value.ToString(), out until_days1))
                        until_days1 = 0;

                    if (until_days1 < 0)
                        dgvProduct.Rows[e.RowIndex].Cells["until_days1"].Style.ForeColor = Color.Red;
                    else
                        dgvProduct.Rows[e.RowIndex].Cells["until_days1"].Style.ForeColor = Color.Black;

                    double until_days2;
                    if (dgvProduct.Rows[e.RowIndex].Cells["until_days2"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["until_days2"].Value.ToString(), out until_days2))
                        until_days2 = 0;

                    if (until_days2 < 0)
                        dgvProduct.Rows[e.RowIndex].Cells["until_days2"].Style.ForeColor = Color.Red;
                    else
                        dgvProduct.Rows[e.RowIndex].Cells["until_days2"].Style.ForeColor = Color.Black;
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "offer_updatetime")
                {
                    DateTime dt;
                    if (dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null
                        || !DateTime.TryParse(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out dt))
                        return;
                    else
                    {
                        if (dt < DateTime.Now.AddMonths(-1))
                        {
                            dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
                        }
                    }
                }
            }
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabDgv.SelectedTab.Controls.Add(this.dgvProduct);
            this.dgvProduct.BringToFront();

            //탭
            int tabType;
            switch (tabDgv.SelectedTab.Text)
            {
                case "전체":
                    productDt = allDt;
                    break;
                case "관심":
                    productDt = managementDt;
                    break;
                case "월별":
                    productDt = monthDt;
                    break;
                case "조업시즌":
                    productDt = operatingDt;
                    break;
                case "계약시즌":
                    productDt = contractDt;
                    break;
                case "알람":
                    tabType = 6;
                    break;
                case "숨김품목":
                    productDt = hideDt;
                    break;
                default:
                    productDt = allDt;
                    break;
            }

            dgvProduct.DataSource = null;
            dgvProduct.DataSource = productDt;
            SetHeaderStyleSetting();
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
                        if (tabDgv.SelectedTab.Text == "월별")
                            m.Items.Add("월별 해제");
                        else
                            m.Items.Add("월별 등록");

                        if (tabDgv.SelectedTab.Text == "숨김품목")
                            m.Items.Add("숨김품목 해제");
                        else
                            m.Items.Add("숨김품목 등록");

                        //사용자정의
                        if(dgvProduct.Rows[row].Cells["show_product"].Value.ToString() == "1")
                            m.Items.Add("알람 해제");
                        else
                            m.Items.Add("알람 등록");
                        ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
                        toolStripSeparator.Name = "toolStripSeparator";
                        toolStripSeparator.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator);
                        m.Items.Add("조업/계약시즌 설정");
                        ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
                        toolStripSeparator1.Name = "toolStripSeparator";
                        toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator1);
                        m.Items.Add("오퍼단가 등록");
                        m.Items.Add("오퍼단가 조회");

                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
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
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (dgvProduct.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow dr = dgvProduct.SelectedRows[0];
                    if (dr.Index < 0)
                        return;
                    //탭
                    int tabType;
                    switch (tabDgv.SelectedTab.Text)
                    {
                        case "전체":
                            tabType = 1;
                            break;
                        case "관심":
                            tabType = 2;
                            break;
                        case "월별":
                            tabType = 3;
                            break;
                        case "조업시즌":
                            tabType = 4;
                            break;
                        case "계약시즌":
                            tabType = 5;
                            break;
                        case "알람":
                            tabType = 6;
                            break;
                        default:
                            tabType = 1;
                            break;
                    }
                    //Function
                    StringBuilder sql = new StringBuilder();
                    List<StringBuilder> sqlList = new List<StringBuilder>();
                    DataTable productDt = new DataTable();
                    Common.Calendar cd = new Common.Calendar();
                    bool isVal = true;
                    switch (e.ClickedItem.Text)
                    {
                        case "오퍼단가 등록":
                            if (dgvProduct.SelectedRows.Count > 0)
                            {
                                List<DataGridViewRow> productList = new List<DataGridViewRow>();
                                PurchaseUnitPriceInfo pup = new PurchaseUnitPriceInfo(um);
                                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                                {
                                    if (dgvProduct.Rows[i].Selected)
                                    {
                                        productList.Add(dgvProduct.Rows[i]);
                                    }
                                }
                                if (productList.Count > 0)
                                {
                                    pup.InputProduct(productList);
                                    pup.Show();
                                }
                            }
                            break;
                        case "오퍼단가 조회":
                            if (dgvProduct.SelectedRows.Count > 0)
                            {
                                List<DataGridViewRow> productList = new List<DataGridViewRow>();
                                CostAccounting ca = new CostAccounting(um);
                                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                                {
                                    if (dgvProduct.Rows[i].Selected)
                                    {
                                        DataGridViewRow row = dgvProduct.Rows[i];
                                        ca.AddProduct("", row.Cells["product"].Value.ToString()
                                                    , row.Cells["origin"].Value.ToString()
                                                    , row.Cells["sizes"].Value.ToString()
                                                    , row.Cells["seaover_unit"].Value.ToString()
                                                    , "");
                                    }
                                }
                                ca.Show();
                            }
                            break;
                        case "월별 등록":
                            foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                            { 
                                productDt = productOtherCostRepository.GetProductAsOne(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString()
                                                      , row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString(), row.Cells["seaover_unit"].Value.ToString());
                                if (productDt.Rows.Count > 0)
                                {
                                    isVal = true;
                                    sql = productOtherCostRepository.UpdateProduct(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString(), row.Cells["seaover_unit"].Value.ToString()
                                        , 1, isVal, "", "");
                                }
                                else
                                {
                                    ProductOtherCostModel model = new ProductOtherCostModel();
                                    model.product = row.Cells["product"].Value.ToString();
                                    model.origin = row.Cells["origin"].Value.ToString();
                                    model.sizes = row.Cells["sizes"].Value.ToString();
                                    model.unit = row.Cells["seaover_unit"].Value.ToString();
                                    model.cost_unit = "0";
                                    model.weight_calculate = true;
                                    model.tray_calculate = false;
                                    model.custom = 0;
                                    model.tax = 0;
                                    model.incidental_expense = 0;
                                    model.manager = "";
                                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                                    model.edit_user = um.user_name;
                                    model.production_days = 20;
                                    model.isMonth = isVal;
                                    model.show_sttdate = "";
                                    model.show_enddate = "";
                                    model.hide_date = "";
                                    sql = productOtherCostRepository.InsertProduct2(model);
                                }
                                sqlList.Add(sql);
                                dgvProduct.Rows.Remove(row);
                            }
                            //Execute
                            if (productOtherCostRepository.UpdateTrans(sqlList) == -1)
                            {
                                MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                this.Activate();
                            }
                            break;
                        case "월별 해제":
                            foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                            {
                                productDt = productOtherCostRepository.GetProductAsOne(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString()
                                                      , row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString(), row.Cells["seaover_unit"].Value.ToString());
                                if (productDt.Rows.Count > 0)
                                {
                                    isVal = false;
                                    sql = productOtherCostRepository.UpdateProduct(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString(), row.Cells["seaover_unit"].Value.ToString()
                                        , 1, isVal, "", "");
                                }
                                else
                                {
                                    ProductOtherCostModel model = new ProductOtherCostModel();
                                    model.product = row.Cells["product"].Value.ToString();
                                    model.origin = row.Cells["origin"].Value.ToString();
                                    model.sizes = row.Cells["sizes"].Value.ToString();
                                    model.unit = row.Cells["seaover_unit"].Value.ToString();
                                    model.cost_unit = "0";
                                    model.weight_calculate = true;
                                    model.tray_calculate = false;
                                    model.custom = 0;
                                    model.tax = 0;
                                    model.incidental_expense = 0;
                                    model.manager = "";
                                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                                    model.edit_user = um.user_name;
                                    model.production_days = 20;
                                    model.isMonth = isVal;
                                    model.show_sttdate = "";
                                    model.show_enddate = "";
                                    model.hide_date = "";
                                    sql = productOtherCostRepository.InsertProduct2(model);
                                    
                                }
                                sqlList.Add(sql);
                                dgvProduct.Rows.Remove(row);
                            }
                            //Execute
                            if (productOtherCostRepository.UpdateTrans(sqlList) == -1)
                            {
                                MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                this.Activate();
                            }
                            break;
                        case "알람 등록":
                            string show_sttdate, show_enddate;
                            GetDateToDate gdd = new GetDateToDate();
                            gdd.GetTerm(out show_sttdate, out show_enddate);

                            if (!string.IsNullOrEmpty(show_sttdate) && !string.IsNullOrEmpty(show_enddate))
                            {
                                foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                                {
                                    productDt = productOtherCostRepository.GetProductAsOne(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString()
                                                     , row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString(), row.Cells["seaover_unit"].Value.ToString());
                                    if (productDt.Rows.Count > 0)
                                    {
                                        sql = productOtherCostRepository.UpdateProduct(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString(), row.Cells["seaover_unit"].Value.ToString()
                                            , 2, isVal, show_sttdate, show_enddate);
                                    }
                                    else
                                    {
                                        ProductOtherCostModel model = new ProductOtherCostModel();
                                        model.group_name = row.Cells["group_name"].Value.ToString();
                                        model.product = row.Cells["product"].Value.ToString();
                                        model.origin = row.Cells["origin"].Value.ToString();
                                        model.sizes = row.Cells["sizes"].Value.ToString();
                                        model.unit = row.Cells["seaover_unit"].Value.ToString();
                                        model.cost_unit = "0";
                                        model.weight_calculate = true;
                                        model.tray_calculate = false;
                                        model.custom = 0;
                                        model.tax = 0;
                                        model.incidental_expense = 0;
                                        model.manager = "";
                                        model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                                        model.edit_user = um.user_name;
                                        model.production_days = 20;
                                        model.isMonth = false;
                                        model.show_sttdate = show_sttdate;
                                        model.show_enddate = show_enddate;
                                        model.hide_date = "";
                                        sql = productOtherCostRepository.InsertProduct2(model);
                                    }
                                    sqlList.Add(sql);
                                    dgvProduct.Rows.Remove(row);
                                }

                                if (productOtherCostRepository.UpdateTrans(sqlList) == -1)
                                {
                                    MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                    this.Activate();
                                }
                            }
                            break;
                        case "알람 해제":

                            foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                            {
                                sql = productOtherCostRepository.UpdateProduct(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString(), row.Cells["seaover_unit"].Value.ToString()
                                , 2, isVal, "", "");

                                sqlList.Add(sql);
                                dgvProduct.Rows.Remove(row);
                            }
                            if (productOtherCostRepository.UpdateTrans(sqlList) == -1)
                            {
                                MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                this.Activate();
                            }
                            break;
                        case "숨김품목 등록":
                            string hide_date = cd.GetDate(true);
                            if (hide_date != null)
                            {
                                foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                                {
                                    productDt = productOtherCostRepository.GetProductAsOne(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString()
                                                     , row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString(), row.Cells["seaover_unit"].Value.ToString());
                                    if (productDt.Rows.Count > 0)
                                    {
                                        sql = productOtherCostRepository.UpdateProduct(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString(), row.Cells["seaover_unit"].Value.ToString()
                                            , 3, isVal, hide_date, "");
                                    }
                                    else
                                    {
                                        ProductOtherCostModel model = new ProductOtherCostModel();
                                        model.product = row.Cells["product"].Value.ToString();
                                        model.origin = row.Cells["origin"].Value.ToString();
                                        model.sizes = row.Cells["sizes"].Value.ToString();
                                        model.unit = row.Cells["seaover_unit"].Value.ToString();
                                        model.cost_unit = "0";
                                        model.weight_calculate = true;
                                        model.tray_calculate = false;
                                        model.custom = 0;
                                        model.tax = 0;
                                        model.incidental_expense = 0;
                                        model.manager = "";
                                        model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                                        model.edit_user = um.user_name;
                                        model.production_days = 20;
                                        model.isMonth = false;
                                        model.show_sttdate = "";
                                        model.show_enddate = "";
                                        model.hide_date = hide_date;
                                        sql = productOtherCostRepository.InsertProduct2(model);
                                    }
                                    sqlList.Add(sql);
                                    dgvProduct.Rows.Remove(row);
                                }
                                //Execute
                                if (productOtherCostRepository.UpdateTrans(sqlList) == -1)
                                {
                                    MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                    this.Activate();
                                }
                            }
                            break;
                        case "숨김품목 해제":
                            foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                            {
                                sql = productOtherCostRepository.UpdateProduct(row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString(), row.Cells["seaover_unit"].Value.ToString()
                                , 3, isVal, "", "");
                                sqlList.Add(sql);
                                dgvProduct.Rows.Remove(row);
                            }
                            if (productOtherCostRepository.UpdateTrans(sqlList) == -1)
                            {
                                MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                this.Activate();
                            }
                            break;
                        case "조업/계약시즌 설정":
                            ContractRecommendationManager2 crm = new ContractRecommendationManager2(um, dr.Cells["product"].Value.ToString(), dr.Cells["origin"].Value.ToString());
                            crm.Show();
                            break;
                    }
                }
                catch
                {
                }
            }
        }

        #endregion

        private void dgvProduct_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                if (dgvProduct.Rows[e.RowIndex].HeaderCell.Value != null && !string.IsNullOrEmpty(dgvProduct.Rows[e.RowIndex].HeaderCell.Value.ToString()))
                {
                    if (dgvProduct.Rows[e.RowIndex].HeaderCell.Value.ToString() == "+")
                    {
                        List<DataRow> subRowList = new List<DataRow>();
                        dgvProduct.Rows[e.RowIndex].HeaderCell.Value = "-";

                        string main_id = dgvProduct.Rows[e.RowIndex].Cells["main_id"].Value.ToString();
                        for (int i = productDt.Rows.Count - 1; i >= 0; i--)
                        {
                            if (productDt.Rows[i]["main_id"].ToString() == main_id
                                && productDt.Rows[i]["sub_id"].ToString() != "9999")
                            {
                                DataRow subRow = productDt.NewRow();
                                subRow.ItemArray = productDt.Rows[i].ItemArray;
                                subRowList.Add(subRow);
                                productDt.Rows.RemoveAt(i);
                            }
                        }

                        //메인품목 밑으로 출력
                        if (subRowList.Count > 0)
                        {
                            int rowindex = 0;
                            for (int i = productDt.Rows.Count - 1; i >= 0; i--)
                            {
                                if (productDt.Rows[i]["main_id"].ToString() == main_id && productDt.Rows[i]["sub_id"].ToString() == "9999")
                                {
                                    rowindex = i;
                                    break;
                                }

                            }

                            for (int i = 0; i < subRowList.Count; i++)
                            {
                                productDt.Rows.InsertAt(subRowList[i], rowindex + 1);
                                dgvProduct.Rows[rowindex + 1].Visible = true;
                                dgvProduct.Rows[rowindex + 1].DefaultCellStyle.BackColor = Color.LightGray;
                            }
                        }

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
    }
}
