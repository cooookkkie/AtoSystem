using AdoNetWindow.Model;
using MySqlX.XDevAPI.Relational;
using Repositories;
using ScottPlot.Palettes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.DashboardForSales.MultiDashboard
{
    public partial class SelectProduct2 : Form
    {
        IPriceComparisonRepository priceComparisonRepository = new PriceComparisonRepository();
        IProductOtherCostRepository productOtherCostRepository = new ProductOtherCostRepository();
        IPurchasePriceRepository purchasePriceRepository = new PurchasePriceRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        UsersModel um;

        DataTable uppDt = null;
        class ProductModel
        {
            public string product { get; set; }
            public string origin { get; set; }
            public string sizes { get; set; }
            public string unit { get; set; }
            public string price_unit { get; set; }
            public string unit_count { get; set; }
            public string seaover_unit { get; set; }
        }

        ProductModel selectProduct = null;

        public SelectProduct2(UsersModel um, List<DataGridViewRow> productList)
        {
            InitializeComponent();
            this.um = um;
            foreach (DataGridViewRow row in productList)
            {
                int n = dgvSelectProduct.Rows.Add();
                dgvSelectProduct.Rows[n].Cells["select_product"].Value = row.Cells["product"].Value.ToString();
                dgvSelectProduct.Rows[n].Cells["select_origin"].Value = row.Cells["origin"].Value.ToString();
                dgvSelectProduct.Rows[n].Cells["select_sizes"].Value = row.Cells["sizes"].Value.ToString();
                dgvSelectProduct.Rows[n].Cells["select_unit"].Value = row.Cells["unit"].Value.ToString();
                dgvSelectProduct.Rows[n].Cells["select_price_unit"].Value = row.Cells["price_unit"].Value.ToString();
                dgvSelectProduct.Rows[n].Cells["select_unit_count"].Value = row.Cells["unit_count"].Value.ToString();
                dgvSelectProduct.Rows[n].Cells["select_seaover_unit"].Value = row.Cells["seaover_unit"].Value.ToString();
            }
        }
        private void SelectProduct2_Load(object sender, EventArgs e)
        {
            //팬딩내역
            uppDt = customsRepository.GetUnpendingProduct2("", "", "", "", false, false);
            CallProductProcedure();
            CallStockProcedure();
            SetHeaderStyle();
        }

        #region Method

        private void SetHeaderStyle()
        {
            DataGridView dgv = dgvWarehouseProduct;
            //재고내역========================================================================================
            Color lightBlue = Color.FromArgb(51, 102, 255);  //진파랑            
            dgv.Columns["shipment_stock"].HeaderCell.Style.BackColor = Color.LightGray;
            dgv.Columns["shipment_stock"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["shipment_stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["shipment_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgv.Columns["shipping_stock"].HeaderCell.Style.BackColor = Color.LightGray;
            dgv.Columns["shipping_stock"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["shipping_stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["shipping_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgv.Columns["unpending_stock"].HeaderCell.Style.BackColor = Color.LightGray;
            dgv.Columns["unpending_stock"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["unpending_stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["unpending_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgv.Columns["pending_stock"].HeaderCell.Style.BackColor = Color.LightGray;
            dgv.Columns["pending_stock"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["pending_stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["pending_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgv.Columns["reserved_stock"].HeaderCell.Style.BackColor = Color.LightGray;
            dgv.Columns["reserved_stock"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["reserved_stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["reserved_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgv.Columns["stock"].HeaderCell.Style.BackColor = Color.DarkGray;
            dgv.Columns["stock"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["stock"].DefaultCellStyle.Font = new Font("나눔고딕", 8, System.Drawing.FontStyle.Bold);
            dgv.Columns["stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            //판매내역========================================================================================
            Color lightRosybrown = Color.FromArgb(216, 190, 190);
            dgv.Columns["sales_total_count"].HeaderCell.Style.BackColor = lightRosybrown;
            dgv.Columns["sales_total_count"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["sales_total_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["sales_total_count"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sales_total_count"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);

            dgv.Columns["sales_day_count"].HeaderCell.Style.BackColor = lightRosybrown;
            dgv.Columns["sales_day_count"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["sales_day_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["sales_day_count"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sales_day_count"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);

            dgv.Columns["sales_month_count"].HeaderCell.Style.BackColor = lightRosybrown;
            dgv.Columns["sales_month_count"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["sales_month_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["sales_month_count"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sales_month_count"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);


            dgv.Columns["month_around"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            dgv.Columns["month_around"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
            dgv.Columns["month_around"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["month_around"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            dgv.Columns["month_around"].DefaultCellStyle.ForeColor = Color.Blue;
            dgv.Columns["month_around"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["month_around"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마
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
                MessageBox.Show(this, e.Message);
                this.Activate();
                return;
            }

        }
        private void CallStockProcedure()
        {
            //품명별재고현황 스토어프로시져 호출
            try
            {
                DateTime enddate = DateTime.Now;
                string sDate = enddate.AddYears(-2).ToString("yyyy-MM-dd");
                string eDate = enddate.ToString("yyyy-MM-dd");
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
                MessageBox.Show(this, e.Message);
                this.Activate();
                return;
            }

        }
        private void GetWarehouse(string product, string origin, string sizes, string unit)
        {
            pnWarehouse.Controls.Clear();
            DataTable warehouseDt = purchasePriceRepository.GetPurchaseShipperList("", product, origin, "", unit);
            for (int i = 0; i < warehouseDt.Rows.Count; i++)
            {
                CheckBox cbWarehouse = new CheckBox();
                cbWarehouse.Width = 200;
                cbWarehouse.Appearance = Appearance.Button;
                cbWarehouse.FlatStyle = FlatStyle.Flat;
                cbWarehouse.Name = warehouseDt.Rows[i]["company"].ToString();

                string companyTxt = warehouseDt.Rows[i]["company"].ToString();
                if (warehouseDt.Rows[i]["company"].ToString().Length > 20 && warehouseDt.Rows[i]["company"].ToString().Length < 40)
                    cbWarehouse.Font = new Font(cbWarehouse.Font.Name.ToString(), 8, FontStyle.Regular);
                else if (warehouseDt.Rows[i]["company"].ToString().Length >= 40)
                {
                    cbWarehouse.Width = cbWarehouse.Width * 2 + 6;
                    cbWarehouse.Font = new Font(cbWarehouse.Font.Name.ToString(), 8, FontStyle.Regular);
                    if (warehouseDt.Rows[i]["company"].ToString().Length >= 50)
                        companyTxt = companyTxt.Substring(0, 50) + "...";
                }
                cbWarehouse.Text = companyTxt;

                // 체크박스의 클릭 이벤트에 핸들러를 추가합니다.
                cbWarehouse.Click += (sender, e) =>
                {
                    CheckboxInit(cbWarehouse.Name);
                    GetProduct(false, cbWarehouse.Name);
                    lbWarehouse.Text = "* [" + cbWarehouse.Name + "] 공장의 취급 품목리스트";
                };
                pnWarehouse.Controls.Add(cbWarehouse);
            }
        }
        private void CheckboxInit(string cbName)
        {
            foreach (Control con in pnWarehouse.Controls)
            {
                if (con is CheckBox checkbox && checkbox.Name != cbName)
                    checkbox.Checked = false;
            }
        }

        private void GetProduct(bool isMsg = true, string warehouse = "")
        {
            //초기화
            dgvWarehouseProduct.Rows.Clear();
            //선택한 공장
            string selectWarehouse = "";
            foreach (CheckBox cBox in pnWarehouse.Controls)
            {
                if (cBox.Checked)
                { 
                    selectWarehouse = cBox.Name;
                    break;
                }
            }

            if (string.IsNullOrEmpty(selectWarehouse))
                return;

            //매출기간
            string sales_terms = cbSaleTerm.Text;
            sales_terms = sales_terms.Replace("개월", "").Replace("일", "");
            DateTime sDate = DateTime.Now.AddMonths(-Convert.ToInt16(sales_terms));
            if (sales_terms == "45")
                sDate = DateTime.Now.AddDays(-Convert.ToInt16(sales_terms));
            DateTime eDate = DateTime.Now.AddDays(-1);

            int workDays;
            common.GetWorkDay(sDate, eDate, out workDays);
            //데이터 만들기
            DataTable productDt = seaoverRepository.GetAllDataOrderBy(sDate.ToString("yyyy-MM-dd"), eDate.ToString("yyyy-MM-dd"), selectProduct.product, selectProduct.origin);
            if (productDt != null && productDt.Rows.Count > 0)
            {
                DataTable otherCostDt = productOtherCostRepository.GetProductOnlyOne("", "", "", "");
                if (otherCostDt != null)
                {
                    //씨오버 거래정보 + 아토전산 거래처
                    var joinResult = from p in productDt.AsEnumerable()
                                     join t in otherCostDt.AsEnumerable()
                                     on p.Field<string>("품목코드") equals t.Field<string>("품목코드")
                                     into outer
                                     from t in outer.DefaultIfEmpty()
                                     select new
                                     {
                                         product = p.Field<string>("품명"),
                                         origin = p.Field<string>("원산지"),
                                         sizes = p.Field<string>("규격"),
                                         sizes2 = p.Field<string>("규격2"),
                                         sizes3 = p.Field<double>("규격3"),
                                         sizes4 = p.Field<string>("규격4"),
                                         unit = p.Field<string>("단위"),
                                         price_unit = p.Field<string>("가격단위"),
                                         unit_count = p.Field<string>("단위수량"),
                                         seaover_unit = p.Field<string>("SEAOVER단위"),

                                         pending = p.Field<double>("통관"),
                                         unpending = p.Field<double>("미통관"),
                                         reserved= p.Field<double>("예약수"),
                                         sales_count = p.Field<double>("매출수"),

                                         manager = (t == null) ? "" : t.Field<string>("manager"),
                                         company = (t == null) ? "" : t.Field<string>("company"),
                                         sortIdx = p.Field<int>("sortIdx")
                                     };
                    productDt = common.ConvertListToDatatable(joinResult);
                    if (productDt != null)
                    {
                        productDt.AcceptChanges();

                        //담당자 검색
                        string whr = "";
                        if (!string.IsNullOrEmpty(selectWarehouse))
                        {
                            if (string.IsNullOrEmpty(whr))
                                whr += $"company LIKE '%{selectWarehouse}^%'";
                            else
                                whr = $" AND company LIKE '%{selectWarehouse}^%'";
                        }
                        DataRow[] dr = productDt.Select(whr);
                        if (dr.Length > 0)
                        {
                            productDt = dr.CopyToDataTable();
                            DataView dv = new DataView(productDt);
                            dv.Sort = "sortidx, product, origin, sizes3, sizes4, sizes, seaover_unit";
                            productDt = dv.ToTable();
                            for (int i = 0; i < productDt.Rows.Count; i++)
                            {
                                int n = dgvWarehouseProduct.Rows.Add();

                                if (productDt.Rows[i]["product"].ToString().Trim() == selectProduct.product.Trim()
                                    && productDt.Rows[i]["origin"].ToString().Trim() == selectProduct.origin.Trim()
                                    && productDt.Rows[i]["sizes"].ToString().Trim() == selectProduct.sizes.Trim()
                                    && productDt.Rows[i]["unit"].ToString().Trim() == selectProduct.unit.Trim()
                                    && productDt.Rows[i]["unit_count"].ToString().Trim() == selectProduct.unit_count.Trim()
                                    && productDt.Rows[i]["seaover_unit"].ToString().Trim() == selectProduct.seaover_unit.Trim())
                                    dgvWarehouseProduct.Rows[n].DefaultCellStyle.BackColor = Color.Beige;


                                dgvWarehouseProduct.Rows[n].Cells["product"].Value = productDt.Rows[i]["product"].ToString().Trim();
                                dgvWarehouseProduct.Rows[n].Cells["origin"].Value = productDt.Rows[i]["origin"].ToString().Trim();
                                dgvWarehouseProduct.Rows[n].Cells["sizes"].Value = productDt.Rows[i]["sizes"].ToString().Trim();
                                dgvWarehouseProduct.Rows[n].Cells["unit"].Value = productDt.Rows[i]["unit"].ToString().Trim();
                                dgvWarehouseProduct.Rows[n].Cells["price_unit"].Value = productDt.Rows[i]["price_unit"].ToString().Trim();
                                dgvWarehouseProduct.Rows[n].Cells["unit_count"].Value = productDt.Rows[i]["unit_count"].ToString().Trim();
                                dgvWarehouseProduct.Rows[n].Cells["seaover_unit"].Value = productDt.Rows[i]["seaover_unit"].ToString().Trim();
                                dgvWarehouseProduct.Rows[n].Cells["manager"].Value = productDt.Rows[i]["manager"].ToString().Trim();

                                //팬딩내역====================================================================================
                                double shipment_qty = 0;
                                double unpending_qty_before = 0;
                                double unpending_qty_after = 0;
                                DataRow[] uppRow = null;
                                if (uppDt != null && uppDt.Rows.Count > 0)
                                {
                                    whr = "product = '" + productDt.Rows[i]["product"].ToString().Trim() + "'"
                                        + " AND origin = '" + productDt.Rows[i]["origin"].ToString().Trim() + "'"
                                        + " AND sizes = '" + productDt.Rows[i]["sizes"].ToString().Trim() + "'"
                                        + " AND box_weight = '" + productDt.Rows[i]["seaover_unit"].ToString().Trim() + "'";
                                    uppRow = uppDt.Select(whr);
                                    if (uppRow.Length > 0)
                                    {
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
                                                shipment_qty += Convert.ToDouble(uppRow[j]["quantity_on_paper"]);
                                            //배송중인 상태
                                            else if (!string.IsNullOrEmpty(bl_no) && string.IsNullOrEmpty(warehousing_date))
                                                unpending_qty_before += Convert.ToDouble(uppRow[j]["quantity_on_paper"]);
                                            //입항은 했지만 미통관인 상태
                                            else if (!string.IsNullOrEmpty(bl_no) && !string.IsNullOrEmpty(warehousing_date))
                                                unpending_qty_after += Convert.ToDouble(uppRow[j]["quantity_on_paper"]);
                                        }
                                    }
                                }
                                dgvWarehouseProduct.Rows[n].Cells["shipment_stock"].Value = shipment_qty.ToString("#,##0");
                                dgvWarehouseProduct.Rows[n].Cells["shipping_stock"].Value = unpending_qty_before.ToString("#,##0");

                                double unpending_stock = Convert.ToDouble(productDt.Rows[i]["unpending"].ToString().Trim());
                                double pending_stock = Convert.ToDouble(productDt.Rows[i]["pending"].ToString().Trim());
                                double reserved_stock = Convert.ToDouble(productDt.Rows[i]["reserved"].ToString().Trim());
                                dgvWarehouseProduct.Rows[n].Cells["unpending_stock"].Value = unpending_stock.ToString("#,##0");
                                dgvWarehouseProduct.Rows[n].Cells["pending_stock"].Value = pending_stock.ToString("#,##0");
                                dgvWarehouseProduct.Rows[n].Cells["reserved_stock"].Value = reserved_stock.ToString("#,##0");
                                double stock = shipment_qty + unpending_qty_before + unpending_stock + pending_stock - reserved_stock;
                                dgvWarehouseProduct.Rows[n].Cells["stock"].Value = stock.ToString("#,##0");

                                double sales_count = Convert.ToDouble(productDt.Rows[i]["sales_count"].ToString().Trim());
                                dgvWarehouseProduct.Rows[n].Cells["sales_month_count"].Value = sales_count.ToString("#,##0");

                                double sales_day_count = sales_count / workDays;
                                dgvWarehouseProduct.Rows[n].Cells["sales_day_count"].Value = sales_day_count.ToString("#,##0");

                                double sales_month_count = sales_day_count * 21;
                                dgvWarehouseProduct.Rows[n].Cells["sales_month_count"].Value = sales_month_count.ToString("#,##0");

                                
                                double month_around = stock / sales_month_count;

                                if (double.IsNaN(month_around))
                                {
                                    if (stock == 0 && sales_month_count > 0)
                                        month_around = 0;
                                    else if (stock > 0 && sales_month_count <= 0)
                                        month_around = 9999;
                                    else
                                        month_around = 0;
                                }
                                dgvWarehouseProduct.Rows[n].Cells["month_around"].Value = month_around.ToString("#,##0.00");
                            }
                        }
                    }
                }
            }
            selectWarehouse = warehouse;
        }
        public void AddProduct(List<DataGridViewRow> productList)
        {
            foreach (DataGridViewRow row in productList)
            {
                int n = dgvSelectProduct.Rows.Add();
                dgvSelectProduct.Rows[n].Cells["select_product"].Value = row.Cells["product"].Value.ToString();
                dgvSelectProduct.Rows[n].Cells["select_origin"].Value = row.Cells["origin"].Value.ToString();
                dgvSelectProduct.Rows[n].Cells["select_sizes"].Value = row.Cells["sizes"].Value.ToString();
                dgvSelectProduct.Rows[n].Cells["select_unit"].Value = row.Cells["unit"].Value.ToString();
                dgvSelectProduct.Rows[n].Cells["select_price_unit"].Value = row.Cells["price_unit"].Value.ToString();
                dgvSelectProduct.Rows[n].Cells["select_unit_count"].Value = row.Cells["unit_count"].Value.ToString();
                dgvSelectProduct.Rows[n].Cells["select_seaover_unit"].Value = row.Cells["seaover_unit"].Value.ToString();
            }
        }
        #endregion

        private void dgvSelectProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvSelectProduct.Columns[e.ColumnIndex].Name == "btnSelect")
                {
                    selectProduct = new ProductModel();
                    selectProduct.product = dgvSelectProduct.Rows[e.RowIndex].Cells["select_product"].Value.ToString();
                    selectProduct.origin = dgvSelectProduct.Rows[e.RowIndex].Cells["select_origin"].Value.ToString();
                    selectProduct.sizes = dgvSelectProduct.Rows[e.RowIndex].Cells["select_sizes"].Value.ToString();
                    selectProduct.unit = dgvSelectProduct.Rows[e.RowIndex].Cells["select_unit"].Value.ToString();
                    selectProduct.price_unit = dgvSelectProduct.Rows[e.RowIndex].Cells["select_price_unit"].Value.ToString();
                    selectProduct.unit_count = dgvSelectProduct.Rows[e.RowIndex].Cells["select_unit_count"].Value.ToString();
                    selectProduct.seaover_unit = dgvSelectProduct.Rows[e.RowIndex].Cells["select_seaover_unit"].Value.ToString();

                    GetWarehouse(selectProduct.product, selectProduct.origin, selectProduct.sizes, "");


                    foreach (DataGridViewRow row in dgvSelectProduct.Rows)
                        row.DefaultCellStyle.BackColor = Color.White;
                    dgvSelectProduct.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Beige;

                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void SelectProduct2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
        }

        #region 우클릭 메뉴2
        private void dgvWarehouseProduct_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvWarehouseProduct.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;
                    if (dgvWarehouseProduct.SelectedRows.Count > 0)
                    {
                        ContextMenuStrip m = new ContextMenuStrip();
                        m.Items.Add("상세 대시보드(다중)");
                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked2);
                        m.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cms_PreviewKeyDown2);
                        //Create 
                        m.BackColor = Color.White;
                        m.Show(dgvWarehouseProduct, e.Location);
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
        private void cms_PreviewKeyDown2(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                //우클릭 단축키
                case Keys.A:
                    {
                        /*foreach (DataGridViewRow selectRow in dgvSelectProduct.SelectedRows)
                            db.DashboardAddProduct(selectRow);
                        //db.SetProductPanel();*/
                    }
                    break;
            }
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked2(object sender, ToolStripItemClickedEventArgs e)
        {
            if (dgvWarehouseProduct.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow dr = dgvWarehouseProduct.SelectedRows[0];
                    if (dr.Index < 0)
                        return;

                    switch (e.ClickedItem.Text)
                    {
                        case "상세 대시보드(다중)":
                            {
                                List<DataGridViewRow> productList = new List<DataGridViewRow>();
                                foreach (DataGridViewRow productRow in dgvWarehouseProduct.SelectedRows)
                                    productList.Add(productRow);
                                MultiDashBoard md = new MultiDashBoard(um, productList);
                                md.Show();
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
        }

        private void dgvWarehouseProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dgvWarehouseProduct.SelectedRows.Count <= 1)
                {
                    dgvWarehouseProduct.ClearSelection();
                    dgvWarehouseProduct.Rows[e.RowIndex].Selected = true;
                }
            }
        }


        #endregion

        private void cbSaleTerm_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetProduct();
        }
    }
}
