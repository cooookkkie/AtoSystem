using AdoNetWindow.Model;
using Repositories;
using Repositories.SaleProduct;
using Repositories.SEAOVER.Purchase;
using Repositories.SEAOVER.Sales;
using Repositories.SEAOVER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlX.XDevAPI.Relational;

namespace AdoNetWindow.DashboardForSales.MultiDashboard
{
    public partial class SelectProduct : Form
    {
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        IPurchasePriceRepository purchasePriceRepository = new PurchasePriceRepository();
        IProductOtherCostRepository productOtherCostRepository = new ProductOtherCostRepository();
        IPriceComparisonRepository priceComparisonRepository = new PriceComparisonRepository();
        ISalesRepository salesRepository = new SalesRepository();
        IProductExcludedSalesRepository productExcludedSalesRepository = new ProductExcludedSalesRepository();
        IProductGroupRepository productGroupRepository = new ProductGroupRepository();
        IPurchaseRepository purchaseRepository = new PurchaseRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        Libs.Tools.Common common = new Libs.Tools.Common();
        UsersModel um = new UsersModel();
        MultiDashBoard db;
        List<DataGridViewRow> productList;
        public SelectProduct(UsersModel um, MultiDashBoard db, List<DataGridViewRow> productList)
        {
            InitializeComponent();
            this.um = um;
            this.db = db;
            this.productList = productList;
            if (this.productList != null && this.productList.Count > 0)
            {
                for (int i = 0; i < this.productList.Count; i++)
                {
                    int n = dgvSelectProduct.Rows.Add();
                    dgvSelectProduct.Rows[n].Cells["product"].Value = this.productList[i].Cells["product"].Value;
                    dgvSelectProduct.Rows[n].Cells["origin"].Value = this.productList[i].Cells["origin"].Value;
                    dgvSelectProduct.Rows[n].Cells["sizes"].Value = this.productList[i].Cells["sizes"].Value;
                    dgvSelectProduct.Rows[n].Cells["unit"].Value = this.productList[i].Cells["unit"].Value;
                    dgvSelectProduct.Rows[n].Cells["price_unit"].Value = this.productList[i].Cells["price_unit"].Value;
                    dgvSelectProduct.Rows[n].Cells["unit_count"].Value = this.productList[i].Cells["unit_count"].Value;
                    dgvSelectProduct.Rows[n].Cells["seaover_unit"].Value = this.productList[i].Cells["seaover_unit"].Value;
                    /*dgvSelectProduct.Rows[n].Cells["select_manager1"].Value = this.productList[i].Cells["manager1"].Value;
                    dgvSelectProduct.Rows[n].Cells["select_manager2"].Value = this.productList[i].Cells["manager2"].Value;
                    dgvSelectProduct.Rows[n].Cells["select_manager3"].Value = this.productList[i].Cells["manager3"].Value;*/
                }
            }
        }

        public SelectProduct(UsersModel um, MultiDashBoard db, DataTable productDt)
        {
            InitializeComponent();
            this.um = um;
            this.db = db;
            if (productDt != null && productDt.Rows.Count > 0)
            {
                for (int i = 0; i < productDt.Rows.Count; i++)
                {
                    int n = dgvSelectProduct.Rows.Add();
                    dgvSelectProduct.Rows[n].Cells["product"].Value = productDt.Rows[i]["product"].ToString();
                    dgvSelectProduct.Rows[n].Cells["origin"].Value = productDt.Rows[i]["origin"].ToString();
                    dgvSelectProduct.Rows[n].Cells["sizes"].Value = productDt.Rows[i]["sizes"].ToString();
                    dgvSelectProduct.Rows[n].Cells["unit"].Value = productDt.Rows[i]["unit"].ToString();
                    dgvSelectProduct.Rows[n].Cells["price_unit"].Value = productDt.Rows[i]["price_unit"].ToString();
                    dgvSelectProduct.Rows[n].Cells["unit_count"].Value = productDt.Rows[i]["unit_count"].ToString();
                    dgvSelectProduct.Rows[n].Cells["seaover_unit"].Value = productDt.Rows[i]["seaover_unit"].ToString();
                }
            }
        }


        private void SelectProduct_Load(object sender, EventArgs e)
        {
            
        }

        #region Method
        private void AddProduct(List<DataGridViewRow> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                DataGridViewRow row = list[i];
                bool isExist = false;
                foreach (DataGridViewRow selectRow in dgvSelectProduct.Rows)
                {
                    if (selectRow.Cells["product"].Value == row.Cells["select_product"].Value
                         && selectRow.Cells["origin"].Value == row.Cells["select_origin"].Value
                         && selectRow.Cells["sizes"].Value == row.Cells["select_sizes"].Value
                         && selectRow.Cells["unit"].Value == row.Cells["select_unit"].Value
                         && selectRow.Cells["price_unit"].Value == row.Cells["select_price_unit"].Value
                         && selectRow.Cells["unit_count"].Value == row.Cells["select_unit_count"].Value
                         && selectRow.Cells["seaover_unit"].Value == row.Cells["select_seaover_unit"].Value)
                    {
                        isExist = true;
                        break;
                    }
                }

                if (!isExist)
                {
                    int n = dgvSelectProduct.Rows.Add();
                    dgvSelectProduct.Rows[n].Cells["product"].Value = row.Cells["select_product"].Value;
                    dgvSelectProduct.Rows[n].Cells["origin"].Value = row.Cells["select_origin"].Value;
                    dgvSelectProduct.Rows[n].Cells["sizes"].Value = row.Cells["select_sizes"].Value;
                    dgvSelectProduct.Rows[n].Cells["unit"].Value = row.Cells["select_unit"].Value;
                    dgvSelectProduct.Rows[n].Cells["price_unit"].Value = row.Cells["select_price_unit"].Value;
                    dgvSelectProduct.Rows[n].Cells["unit_count"].Value = row.Cells["select_unit_count"].Value;
                    dgvSelectProduct.Rows[n].Cells["seaover_unit"].Value = row.Cells["select_seaover_unit"].Value;
                }
            }
        }
        private void GetWarehouseByProduct(DataGridViewRow row)
        {
            pnWarehouse.Controls.Clear();
            DataTable warehouseDt = purchasePriceRepository.GetPurchaseShipperList(""
                                                                                , row.Cells["select_product"].Value.ToString()
                                                                                , row.Cells["select_origin"].Value.ToString()
                                                                                , row.Cells["select_sizes"].Value.ToString()
                                                                                , row.Cells["select_seaover_unit"].Value.ToString());
            for (int i = 0; i < warehouseDt.Rows.Count; i++)
            {
                CheckBox cbWarehouse = new CheckBox();
                cbWarehouse.Width = 150;
                cbWarehouse.Appearance = Appearance.Button;
                cbWarehouse.FlatStyle = FlatStyle.Flat;
                cbWarehouse.Name = warehouseDt.Rows[i]["company"].ToString();

                string companyTxt = warehouseDt.Rows[i]["company"].ToString();
                if (warehouseDt.Rows[i]["company"].ToString().Length > 20 && warehouseDt.Rows[i]["company"].ToString().Length < 30)
                    cbWarehouse.Font = new Font(cbWarehouse.Font.Name.ToString(), 8, FontStyle.Regular);
                else if (warehouseDt.Rows[i]["company"].ToString().Length > 30)
                {
                    cbWarehouse.Width = cbWarehouse.Width * 2 + 6;
                    cbWarehouse.Font = new Font(cbWarehouse.Font.Name.ToString(), 8, FontStyle.Regular);
                    if (warehouseDt.Rows[i]["company"].ToString().Length > 40)
                        companyTxt = companyTxt.Substring(0, 40) + "...";
                }
                cbWarehouse.Text = companyTxt;

                // 체크박스의 클릭 이벤트에 핸들러를 추가합니다.
                cbWarehouse.Click += (sender, e) =>
                {
                    CheckboxInit(cbWarehouse.Name);
                    GetProduct(false);
                    lbWarehouse.Text = cbWarehouse.Name;
                };
                pnWarehouse.Controls.Add(cbWarehouse);
            }
        }

        private void GetWarehouse()
        {
            pnWarehouse.Controls.Clear();
            if (!string.IsNullOrEmpty(txtWarehouse.Text.Trim()))
            {
                DataTable warehouseDt = purchasePriceRepository.GetPurchaseShipperList(txtWarehouse.Text.Trim());
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
                        lbWarehouse.Text = cbWarehouse.Name;
                        txtProduct.Focus();
                        cbInCompany.Checked = true;
                    };
                    pnWarehouse.Controls.Add(cbWarehouse);
                }
            }
        }
        private void GetWarehouse(string product, string origin, string unit)
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
                    lbWarehouse.Text = cbWarehouse.Name;
                    txtProduct.Focus();
                    cbInCompany.Checked = true;
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
            string selectWarehouse = lbWarehouse.Text;
            if (!string.IsNullOrEmpty(warehouse))
                selectWarehouse = warehouse;
            else if (!cbInCompany.Checked)
                selectWarehouse = string.Empty;
            //데이터 만들기
            dgvProduct.Rows.Clear();
            DataTable productDt = seaoverRepository.GetAllData(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtSeaoverUnit.Text, txtmanager1.Text, txtmanager2.Text, txtDivision.Text);
            if (productDt != null && productDt.Rows.Count > 0)
            {
                DataTable otherCostDt = productOtherCostRepository.GetProductOnlyOne(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtSeaoverUnit.Text);

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
                                         unit = p.Field<string>("단위"),
                                         price_unit = p.Field<string>("가격단위"),
                                         unit_count = p.Field<string>("단위수량"),
                                         seaover_unit = p.Field<string>("SEAOVER단위"),
                                         manager1 = p.Field<string>("담당자1"),
                                         manager2 = p.Field<string>("담당자2"),
                                         division = p.Field<string>("구분"),
                                         manager3 = (t == null) ? "" : t.Field<string>("manager"),
                                         company = (t == null) ? "" : t.Field<string>("company")
                                     };
                    productDt = common.ConvertListToDatatable(joinResult);
                    if (productDt != null)
                        productDt.AcceptChanges();

                    //담당자 검색
                    string whr = "";
                    if (!string.IsNullOrEmpty(txtManger3.Text.Trim()))
                        whr = $"manager LIKE '%{txtManger3.Text}%'";
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
                        for (int i = 0; i < dr.Length; i++)
                        {
                            int n = dgvProduct.Rows.Add();
                            dgvProduct.Rows[n].Cells["select_product"].Value = dr[i]["product"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_origin"].Value = dr[i]["origin"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_sizes"].Value = dr[i]["sizes"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_unit"].Value = dr[i]["unit"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_price_unit"].Value = dr[i]["price_unit"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_unit_count"].Value = dr[i]["unit_count"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_seaover_unit"].Value = dr[i]["seaover_unit"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_manager1"].Value = dr[i]["manager1"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_manager2"].Value = dr[i]["manager2"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_manager3"].Value = dr[i]["manager3"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["division"].Value = dr[i]["division"].ToString().Trim();
                        }
                    }
                }
            }
            selectWarehouse = warehouse;
        }

        private void GetSizes(string product, string origin, string unit)
        {
            //데이터 만들기
            dgvProduct.Rows.Clear();
            pnWarehouse.Controls.Clear();
            DataTable productDt = seaoverRepository.GetAllData(product, origin, "", unit, "", "", "", "");
            if (productDt != null && productDt.Rows.Count > 0)
            {
                DataTable otherCostDt = productOtherCostRepository.GetProductOnlyOne(product, origin, "", "");
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
                                         unit = p.Field<string>("단위"),
                                         price_unit = p.Field<string>("가격단위"),
                                         unit_count = p.Field<string>("단위수량"),
                                         seaover_unit = p.Field<string>("SEAOVER단위"),
                                         manager1 = p.Field<string>("담당자1"),
                                         manager2 = p.Field<string>("담당자2"),
                                         division = p.Field<string>("구분"),
                                         manager3 = (t == null) ? "" : t.Field<string>("manager"),
                                         company = (t == null) ? "" : t.Field<string>("company")
                                     };
                    productDt = common.ConvertListToDatatable(joinResult);
                    if (productDt != null)
                        productDt.AcceptChanges();

                    string whr = "1=1";
                    DataRow[] dr = productDt.Select(whr);
                    if (dr.Length > 0)
                    {
                        productDt = dr.CopyToDataTable();
                        for (int i = 0; i < dr.Length; i++)
                        {
                            int n = dgvProduct.Rows.Add();
                            dgvProduct.Rows[n].Cells["select_product"].Value = dr[i]["product"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_origin"].Value = dr[i]["origin"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_sizes"].Value = dr[i]["sizes"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_unit"].Value = dr[i]["unit"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_price_unit"].Value = dr[i]["price_unit"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_unit_count"].Value = dr[i]["unit_count"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_seaover_unit"].Value = dr[i]["seaover_unit"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_manager1"].Value = dr[i]["manager1"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_manager2"].Value = dr[i]["manager2"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_manager3"].Value = dr[i]["manager3"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["division"].Value = dr[i]["division"].ToString().Trim();
                        }
                    }
                }
            }
        }


        private void GetSizes(bool isMsg = true, string warehouse = "")
        {
            string selectWarehouse = lbWarehouse.Text;
            if (!string.IsNullOrEmpty(warehouse))
                selectWarehouse = warehouse;
            else if (!cbInCompany.Checked)
                selectWarehouse = string.Empty;
            //데이터 만들기
            dgvProduct.Rows.Clear();
            DataTable productDt = seaoverRepository.GetAllData(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtSeaoverUnit.Text, txtmanager1.Text, txtmanager2.Text, txtDivision.Text);
            if (productDt != null && productDt.Rows.Count > 0)
            {
                DataTable otherCostDt = productOtherCostRepository.GetProductOnlyOne(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtSeaoverUnit.Text);

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
                                         unit = p.Field<string>("단위"),
                                         price_unit = p.Field<string>("가격단위"),
                                         unit_count = p.Field<string>("단위수량"),
                                         seaover_unit = p.Field<string>("SEAOVER단위"),
                                         manager1 = p.Field<string>("담당자1"),
                                         manager2 = p.Field<string>("담당자2"),
                                         manager3 = (t == null) ? "" : t.Field<string>("manager"),
                                         company = (t == null) ? "" : t.Field<string>("company")
                                     };
                    productDt = common.ConvertListToDatatable(joinResult);
                    if (productDt != null)
                        productDt.AcceptChanges();

                    //담당자 검색
                    string whr = "";
                    if (!string.IsNullOrEmpty(txtManger3.Text.Trim()))
                        whr = $"manager LIKE '%{txtManger3.Text}%'";
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
                        for (int i = 0; i < dr.Length; i++)
                        {
                            int n = dgvProduct.Rows.Add();
                            dgvProduct.Rows[n].Cells["select_product"].Value = dr[i]["product"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_origin"].Value = dr[i]["origin"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_sizes"].Value = dr[i]["sizes"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_unit"].Value = dr[i]["unit"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_price_unit"].Value = dr[i]["price_unit"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_unit_count"].Value = dr[i]["unit_count"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_seaover_unit"].Value = dr[i]["seaover_unit"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_manager1"].Value = dr[i]["manager1"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_manager2"].Value = dr[i]["manager2"].ToString().Trim();
                            dgvProduct.Rows[n].Cells["select_manager3"].Value = dr[i]["manager3"].ToString().Trim();
                        }
                    }
                }
            }
            selectWarehouse = warehouse;
        }

        #endregion

        #region Key event
        private void SelectProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearch.PerformClick();
                        break;
                    case Keys.A:
                        btnAddProduct.PerformClick();
                        break;
                    case Keys.W:
                        btnDashboardInit.PerformClick();
                        break;
                    case Keys.M:
                        if (txtWarehouse.Focused)
                            txtProduct.Focus();
                        else
                            txtWarehouse.Focus();
                        break;
                    case Keys.N:
                        txtProduct.Text = string.Empty;
                        txtOrigin.Text = string.Empty;
                        txtSizes.Text = string.Empty;
                        txtUnit.Text = string.Empty;
                        txtSeaoverUnit.Text = string.Empty;
                        txtmanager1.Text = string.Empty;
                        txtmanager2.Text = string.Empty;
                        txtManger3.Text = string.Empty;
                        txtDivision.Text = string.Empty;
                        txtProduct.Focus();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        cbInCompany.Checked = !cbInCompany.Checked;
                        break;
                    case Keys.F5:
                        btnRefresh.PerformClick();
                        break;
                }
            }
        }
        private void txtWarehouse_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetWarehouse();
        }
        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetProduct();
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
                        m.Items.Add("거래처 검색");
                        m.Items.Add("품목 추가 (A)");
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
                case Keys.A:
                    {
                        btnAddProduct.PerformClick();
                    }
                    break;
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

                    switch (e.ClickedItem.Text)
                    {
                        case "거래처 검색":
                            {
                                GetWarehouseByProduct(dr);
                            }
                            break;
                        case "품목 추가 (A)":
                            {
                                List<DataGridViewRow> list = new List<DataGridViewRow>();
                                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                                {
                                    if (dgvProduct.Rows[i].Selected)
                                        list.Add(dgvProduct.Rows[i]);
                                }
                                AddProduct(list);
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

        private void dgvProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dgvProduct.SelectedRows.Count <= 1)
                {
                    dgvProduct.ClearSelection();
                    dgvProduct.Rows[e.RowIndex].Selected = true;
                }
            }
        }


        #endregion

        #region 우클릭 메뉴2
        private void dgvSelectProduct_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvSelectProduct.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;
                    if (dgvSelectProduct.SelectedRows.Count > 0)
                    {
                        ContextMenuStrip m = new ContextMenuStrip();
                        m.Items.Add("같은 품목의 다른 규격 검색(단위O)");
                        m.Items.Add("같은 품목의 다른 규격 검색(단위X)");
                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked2);
                        m.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cms_PreviewKeyDown2);
                        //Create 
                        m.BackColor = Color.White;
                        m.Show(dgvSelectProduct, e.Location);
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
            if (dgvSelectProduct.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow dr = dgvSelectProduct.SelectedRows[0];
                    if (dr.Index < 0)
                        return;

                    switch (e.ClickedItem.Text)
                    {
                        case "같은 품목의 다른 규격 검색(단위O)":
                            {
                                GetSizes(dr.Cells["product"].Value.ToString(), dr.Cells["origin"].Value.ToString(), dr.Cells["unit"].Value.ToString());
                                GetWarehouse(dr.Cells["product"].Value.ToString(), dr.Cells["origin"].Value.ToString(), dr.Cells["seaover_unit"].Value.ToString());
                            }
                            break;
                        case "같은 품목의 다른 규격 검색(단위X)":
                            {
                                GetSizes(dr.Cells["product"].Value.ToString(), dr.Cells["origin"].Value.ToString(), "");
                                GetWarehouse(dr.Cells["product"].Value.ToString(), dr.Cells["origin"].Value.ToString(), "");
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

        private void dgvSelectProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dgvSelectProduct.SelectedRows.Count <= 1)
                {
                    dgvSelectProduct.ClearSelection();
                    dgvSelectProduct.Rows[e.RowIndex].Selected = true;
                }
            }
        }


        #endregion





        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetProduct();
        }

        private void btnDashboardInit_Click(object sender, EventArgs e)
        {
            /*db.DashboardRefresh();
            foreach (DataGridViewRow selectRow in dgvSelectProduct.Rows)
                db.DashboardAddProduct(selectRow);*/
            //productList.Add(selectRow);

            List<DataGridViewRow> productList = new List<DataGridViewRow>();
            for (int i = 0; i < dgvSelectProduct.RowCount; i++)
                productList.Add(dgvSelectProduct.Rows[i]);

            db.DashboardRefresh(productList);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgvSelectProduct.Rows.Clear();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> list = new List<DataGridViewRow>();
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                if (dgvProduct.Rows[i].Selected)
                    list.Add(dgvProduct.Rows[i]);
            }
            AddProduct(list);
        }

        private void panel11_MouseClick(object sender, MouseEventArgs e)
        {
            List<DataGridViewRow> list = new List<DataGridViewRow>();
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                if (dgvProduct.Rows[i].Selected)
                    list.Add(dgvProduct.Rows[i]);
            }
            AddProduct(list);
        }
    }
}
