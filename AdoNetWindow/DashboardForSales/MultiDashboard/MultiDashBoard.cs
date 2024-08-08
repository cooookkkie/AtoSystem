using AdoNetWindow.Common.PrintManager;
using AdoNetWindow.Model;
using AdoNetWindow.SEAOVER;
using AdoNetWindow.SEAOVER.PriceComparison;
using Microsoft.Office.Interop.Excel;
using MySqlX.XDevAPI.Relational;
using Repositories;
using Repositories.Config;
using Repositories.SaleProduct;
using Repositories.SEAOVER;
using Repositories.SEAOVER.Purchase;
using Repositories.SEAOVER.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Ubiety.Dns.Core;
using static System.Windows.Forms.AxHost;
using Application = System.Windows.Forms.Application;
using CheckBox = System.Windows.Forms.CheckBox;
using DataTable = System.Data.DataTable;
using Font = System.Drawing.Font;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;

namespace AdoNetWindow.DashboardForSales.MultiDashboard
{
    public partial class MultiDashBoard : Form
    {
        IAuthorityRepository authorityRepository = new AuthorityRepository();
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

        DataTable marketDt = new DataTable();
        DataTable exchangeRateDt = new DataTable();
        DataTable customDt = new DataTable();
        DataTable excluedDt = new DataTable();
        DataTable productStockSalesDt = new DataTable();
        DataTable unpendingDt = new DataTable();
        List<DataGridViewRow> productList = new List<DataGridViewRow>(0);

        public MultiDashBoard(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
        }
        public MultiDashBoard(UsersModel um, List<DataGridViewRow> productList)
        {
            InitializeComponent();
            this.um = um;
            this.productList = productList;
        }

        private void MultiDashBoard_Load(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "다중 대시보드", "is_print"))
                    btnPrint.Visible = false;
            }

            printDocument.PrintPage += PrintDocument_PrintPage;
            //업체별시세현황 스토어프로시져 호출
            CallProductProcedure();

            txtSalesSttdate.Text = DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd");
            txtSalesEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            //업체별시세현황 
            marketDt = seaoverRepository.GetAllData(DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), "");

            //환율내역 Data
            DateTime purchaseSttdate = DateTime.Now.AddYears(-4);
            DateTime purchaseEnddate = DateTime.Now;
            exchangeRateDt = purchaseRepository.GetExchangeRate(purchaseSttdate.ToString("yyyy-MM-dd"), purchaseEnddate.ToString("yyyy-MM-dd"));
            //팬딩내역 Data
            customDt = customsRepository.GetDashboard(purchaseSttdate.ToString("yyyyMM"), purchaseEnddate.ToString("yyyyMM")
                                                         , "", "", "", "", "", false);

            //제외매출
            excluedDt = productExcludedSalesRepository.GetExcludedSalesByMonth(DateTime.Now.AddMonths(-19).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd")
                                                                            , ""
                                                                            , ""
                                                                            , ""
                                                                            , ""
                                                                            , ""
                                                                            , ""
                                                                            , ""
                                                                            , "", false);
            //품명별 재고,판매내역
            productStockSalesDt = seaoverRepository.GetStockAndSalesDetail2(""
                                                                            , ""
                                                                            , ""
                                                                            , ""
                                                                            , cbSaleTerm.Text
                                                                            , "", false);
            //선적대기중인 내역
            unpendingDt = customsRepository.GetUnpendingProduct3("", "", "", "", "", false, true, cbPendingDetail.Checked);

            if (productList.Count > 0)
            {
                foreach (DataGridViewRow productRow in productList)
                {
                    DashboardAddProduct(productRow);
                }
            }
            SelectProduct sp = new SelectProduct(um, this, productList);
            sp.Owner = this;
            sp.Show();
        }



        #region Panel scorll hide
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int ShowScrollBar(IntPtr hWnd, int wBar, int bShow);

        private enum ScrollBarDirection
        {
            SB_HORZ = 0,
            SB_VERT = 1,
            SB_BOTH = 3
        }

        private void HideScrollBar(IntPtr handle, ScrollBarDirection direction)
        {
            const int SB_DISABLE = 1;
            const int SB_HIDE = 0;

            ShowScrollBar(handle, (int)direction, SB_DISABLE);
            ShowScrollBar(handle, (int)direction, SB_HIDE);
        }
        #endregion

        #region Key event
        private void txtSalesSttdate_KeyDown_1(object sender, KeyEventArgs e)
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

                        if (DateTime.TryParse(txtSalesSttdate.Text, out DateTime sttdate) && DateTime.TryParse(txtSalesEnddate.Text, out DateTime enddate))
                        {
                            foreach (Control con in pnDashboard.Controls)
                            {
                                if (con.Name == "MultiDashboardUnit")
                                {
                                    MultiDashboardUnit unit = (MultiDashboardUnit)con;
                                    unit.GetMarketPriceByCompany(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));
                                }
                            }
                            //messageBox.Show(this, "완료");
                        }
                    }
                }
            }
        }
        private void MultiDashBoard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnControlRefresh.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.P:
                        btnPrint.PerformClick();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F4:
                        btnSelectProduct.PerformClick();
                        break;
                    case Keys.F5:
                        btnRefresh.PerformClick();
                        break;
                    case Keys.F12:
                        btnUnitSizeToggle.PerformClick();
                        break;
                }
            }
        }
        #endregion

        #region Method
        bool isMaximize = true;
        private void btnUnitSizeToggle_Click(object sender, EventArgs e)
        {
            foreach (MultiDashboardUnit unit in pnDashboard.Controls)
            {
                unit.AuthoUnitMaximize(isMaximize);
            }
            isMaximize = !isMaximize;
        }
        public void DashboardRefresh()
        { 
            pnDashboard.Controls.Clear();
        }

        public void DashboardRefresh(List<DataGridViewRow> productList)
        {
            List<MultiDashboardUnit> unitList = new List<MultiDashboardUnit>();
            if (pnDashboard.Controls.Count > 0)
            {
                foreach (MultiDashboardUnit unit in pnDashboard.Controls)
                    unitList.Add(unit);
            }

            pnDashboard.Controls.Clear();

            for (int i = 0; i < productList.Count; i++)
            {
                bool isExist = false;
                foreach (MultiDashboardUnit unit in unitList)
                {
                    string[] productInfo = unit.GetProductInfo();

                    if (productList[i].Cells["product"].Value.ToString() == productInfo[0]
                        && productList[i].Cells["origin"].Value.ToString() == productInfo[1]
                        && productList[i].Cells["sizes"].Value.ToString() == productInfo[2]
                        && productList[i].Cells["unit"].Value.ToString() == productInfo[3]
                        && productList[i].Cells["price_unit"].Value.ToString() == productInfo[4]
                        && productList[i].Cells["unit_count"].Value.ToString() == productInfo[5]
                        && productList[i].Cells["seaover_unit"].Value.ToString() == productInfo[6])
                    {
                        pnDashboard.Controls.Add(unit);
                        isExist = true;
                        break;
                    }
                }
                //기존에 없던 내역이면 신규로 추가
                if (!isExist)
                {
                    MultiDashboardUnit dUnit = new MultiDashboardUnit(um, this, productList[i].Cells["product"].Value.ToString(), productList[i].Cells["origin"].Value.ToString()
                    , productList[i].Cells["sizes"].Value.ToString(), productList[i].Cells["unit"].Value.ToString()
                    , productList[i].Cells["price_unit"].Value.ToString(), productList[i].Cells["unit_count"].Value.ToString()
                    , productList[i].Cells["seaover_unit"].Value.ToString()
                    , marketDt
                                                        , exchangeRateDt
                                                        , customDt
                                                        , excluedDt
                                                        , productStockSalesDt
                                                        , unpendingDt);
                    dUnit.Refresh(txtSalesSttdate.Text, txtSalesEnddate.Text, cbPurchasePriceType.Text, cbAtoSale.Checked, cbSaleTerm.Text
                                    , cbShipment.Checked, cbSeaoverUnpending.Checked, cbSeaoverPending.Checked, cbReserved.Checked);
                    pnDashboard.Controls.Add(dUnit);
                }
            }
        }


        public void DashboardAddProduct(DataGridViewRow row)
        {
            MultiDashboardUnit dUnit = new MultiDashboardUnit(um, this, row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString()
                                                            , row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString()
                                                            , row.Cells["price_unit"].Value.ToString(), row.Cells["unit_count"].Value.ToString()
                                                            , row.Cells["seaover_unit"].Value.ToString()
                                                            , marketDt
                                                            , exchangeRateDt
                                                            , customDt
                                                            , excluedDt
                                                            , productStockSalesDt
                                                            , unpendingDt);
            dUnit.Refresh(txtSalesSttdate.Text, txtSalesEnddate.Text, cbPurchasePriceType.Text, cbAtoSale.Checked, cbSaleTerm.Text
                            , cbShipment.Checked, cbSeaoverUnpending.Checked, cbSeaoverPending.Checked, cbReserved.Checked);
            pnDashboard.Controls.Add(dUnit);
        }
        public void DashboardAddProduct(DataRow row)
        {
            MultiDashboardUnit dUnit = new MultiDashboardUnit(um, this, row["product"].ToString(), row["origin"].ToString()
                                                            , row["sizes"].ToString(), row["unit"].ToString()
                                                            , row["price_unit"].ToString(), row["unit_count"].ToString()
                                                            , row["seaover_unit"].ToString()
                                                            , marketDt
                                                            , exchangeRateDt
                                                            , customDt
                                                            , excluedDt
                                                            , productStockSalesDt
                                                            , unpendingDt);
            dUnit.Refresh(txtSalesSttdate.Text, txtSalesEnddate.Text, cbPurchasePriceType.Text, cbAtoSale.Checked, cbSaleTerm.Text
                            , cbShipment.Checked, cbSeaoverUnpending.Checked, cbSeaoverPending.Checked, cbReserved.Checked);
            pnDashboard.Controls.Add(dUnit);
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
                    messageBox.Show(this, "호출 내용이 없음");
                    this.Activate();
                    return;
                }
            }
            catch (Exception e)
            {
                this.Activate();
                return;
            }

        }
        public void AddProduct(List<DataGridViewRow> productList)
        {
            foreach (DataGridViewRow productRow in productList)
                DashboardAddProduct(productRow);
        }

        #endregion

        #region Button
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnProductSort_Click(object sender, EventArgs e)
        {
            //Datatable 생성
            DataTable productDt = new DataTable();
            productDt.Columns.Add("product", typeof(string));
            productDt.Columns.Add("origin", typeof(string));
            productDt.Columns.Add("sizes", typeof(string));
            productDt.Columns.Add("unit", typeof(string));
            productDt.Columns.Add("price_unit", typeof(string));
            productDt.Columns.Add("unit_count", typeof(string));
            productDt.Columns.Add("seaover_unit", typeof(string));
            //Datarow 추가
            foreach (MultiDashboardUnit unit in pnDashboard.Controls)
            {
                string[] productInfo = unit.GetProductInfo();
                DataRow productDr = productDt.NewRow();
                productDr["product"] = productInfo[0];
                productDr["origin"] = productInfo[1];
                productDr["sizes"] = productInfo[2];
                productDr["unit"] = productInfo[3];
                productDr["price_unit"] = productInfo[4];
                productDr["unit_count"] = productInfo[5];
                productDr["seaover_unit"] = productInfo[6];
                productDt.Rows.Add(productDr);
            }
            productDt.AcceptChanges();
            //품목 정렬
            DataView dv = new DataView(productDt);
            dv.Sort = "product, origin, sizes, unit, price_unit, unit_count, seaover_unit ";
            productDt = dv.ToTable();
            //재출력
            /*for (int i = 0; i < productDt.Rows.Count; i++)
                DashboardAddProduct(productDt.Rows[i]);*/
            for (int i = productDt.Rows.Count - 1; i >= 0; i--)
            {
                foreach (MultiDashboardUnit unit in pnDashboard.Controls)
                {
                    string[] productInfo = unit.GetProductInfo();

                    if (productInfo[0] == productDt.Rows[i]["product"].ToString()
                        && productInfo[1] == productDt.Rows[i]["origin"].ToString()
                        && productInfo[2] == productDt.Rows[i]["sizes"].ToString()
                        && productInfo[3] == productDt.Rows[i]["unit"].ToString()
                        && productInfo[4] == productDt.Rows[i]["price_unit"].ToString()
                        && productInfo[5] == productDt.Rows[i]["unit_count"].ToString()
                        && productInfo[6] == productDt.Rows[i]["seaover_unit"].ToString())
                    {
                        unit.BringToFront();
                        break;
                    }
                }
            }
            //SetProductPanel();
        }
        private void btnSalesSttdate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
            {
                txtSalesSttdate.Text = sdate;
                if (DateTime.TryParse(txtSalesSttdate.Text, out DateTime sttdate) && DateTime.TryParse(txtSalesEnddate.Text, out DateTime enddate))
                {
                    foreach (Control con in pnDashboard.Controls)
                    {
                        if (con.Name == "MultiDashboardUnit")
                        {
                            MultiDashboardUnit unit = (MultiDashboardUnit)con;
                            unit.GetMarketPriceByCompany(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));
                        }
                    }
                    messageBox.Show(this, "완료");
                }
            }
        }
        private void btnSalesEnddate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
            {
                txtSalesEnddate.Text = sdate;
                if (DateTime.TryParse(txtSalesSttdate.Text, out DateTime sttdate) && DateTime.TryParse(txtSalesEnddate.Text, out DateTime enddate))
                {
                    foreach (Control con in pnDashboard.Controls)
                    {
                        if (con.Name == "MultiDashboardUnit")
                        {
                            MultiDashboardUnit unit = (MultiDashboardUnit)con;
                            unit.GetMarketPriceByCompany(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));
                        }
                    }
                    messageBox.Show(this, "완료");
                }
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            /*pnWarehouse.Controls.Clear();
            dgvProduct.Rows.Clear();
            lbWarehouse.Text = string.Empty;
            txtWarehouse.Text = string.Empty;
            txtProduct.Text = string.Empty;
            txtOrigin.Text = string.Empty;
            txtSizes.Text = string.Empty;
            txtUnit.Text = string.Empty;
            txtSeaoverUnit.Text = string.Empty;
            txtManger.Text = string.Empty;*/
            pnDashboard.Controls.Clear();
        }
        private void btnControlRefresh_Click(object sender, EventArgs e)
        {
            foreach (Control con in pnDashboard.Controls)
            {
                if (con.Name == "MultiDashboardUnit")
                {
                    MultiDashboardUnit unit = (MultiDashboardUnit)con;
                    unit.Refresh(txtSalesSttdate.Text, txtSalesEnddate.Text, cbPurchasePriceType.Text, cbAtoSale.Checked, cbSaleTerm.Text
                            , cbShipment.Checked, cbSeaoverUnpending.Checked, cbSeaoverPending.Checked, cbReserved.Checked);
                }
            }
            //SetProductPanel();
            //messageBox.Show(this, "최신화 완료");
        }
        
        private void btnSelectProduct_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                //if (frm.Name == "RecoveryPrincipalManager")
                if (frm.Name == "SelectProduct")
                {
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                    isFormActive = true;
                }
            }
            //새로열기
            if (!isFormActive)
            {
                DataTable productDt = new DataTable();
                productDt.Columns.Add("product", typeof(string));
                productDt.Columns.Add("origin", typeof(string));
                productDt.Columns.Add("sizes", typeof(string));
                productDt.Columns.Add("unit", typeof(string));
                productDt.Columns.Add("price_unit", typeof(string));
                productDt.Columns.Add("unit_count", typeof(string));
                productDt.Columns.Add("seaover_unit", typeof(string));

                if (pnDashboard.Controls.Count > 0)
                {
                    for (int i = 0; i < pnDashboard.Controls.Count; i++)
                    { 
                        MultiDashboardUnit unit = (MultiDashboardUnit)pnDashboard.Controls[i];
                        DataRow productDr = productDt.NewRow();

                        string[] productInfo = unit.GetProductInfo();
                        productDr["product"] = productInfo[0];
                        productDr["origin"] = productInfo[0];
                        productDr["sizes"] = productInfo[0];
                        productDr["unit"] = productInfo[0];
                        productDr["price_unit"] = productInfo[0];
                        productDr["unit_count"] = productInfo[0];
                        productDr["seaover_unit"] = productInfo[0];

                        productDt.Rows.Add(productDr);
                    }
                }

                SelectProduct sp = new SelectProduct(um, this, productDt);
                sp.Owner = this;
                sp.Show();
            }
        }
        #endregion

        #region Combobox, Checkbox event
        private void cbPurchasePriceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbPurchasePriceType.Text))
            {
                foreach (Control con in pnDashboard.Controls)
                {
                    if (con.Name == "MultiDashboardUnit")
                    {
                        MultiDashboardUnit unit = (MultiDashboardUnit)con;
                        unit.GetPurchase(cbPurchasePriceType.Text);
                    }
                }
                //messageBox.Show(this, "완료");
            }
        }

        private void cbSaleTerm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbSaleTerm.Text))
            {
                foreach (Control con in pnDashboard.Controls)
                {
                    if (con.Name == "MultiDashboardUnit")
                    {
                        MultiDashboardUnit unit = (MultiDashboardUnit)con;
                        unit.SetSaleTerms(cbSaleTerm.Text);
                        unit.OpenExhaustedManager();
                    }
                }
                messageBox.Show(this, "완료");
            }
        }
        private void cbPendingDetail_CheckedChanged(object sender, EventArgs e)
        {
            unpendingDt = customsRepository.GetUnpendingProduct3("", "", "", "", "", false, true, cbPendingDetail.Checked);
            foreach (MultiDashboardUnit unit in pnDashboard.Controls)
                unit.unpendingDtUpdate(unpendingDt);
        }
        private void cbShipment_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbSaleTerm.Text))
            {
                foreach (Control con in pnDashboard.Controls)
                {
                    if (con.Name == "MultiDashboardUnit")
                    {
                        MultiDashboardUnit unit = (MultiDashboardUnit)con;
                        unit.SetSaleTerms(cbSaleTerm.Text);
                        unit.OpenExhaustedManager();
                    }
                }
            }
        }

        private void cbAtoSale_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control con in pnDashboard.Controls)
            {
                if (con.Name == "MultiDashboardUnit")
                {
                    MultiDashboardUnit unit = (MultiDashboardUnit)con;
                    unit.GetSales(cbAtoSale.Checked);
                }
            }
        }
        #endregion

        #region Print
        int count = 0;
        int pageNo = 1;
        List<Bitmap> bitmaps = new List<Bitmap>();
        private void btnPrint_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "다중 대시보드", "is_print"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            printDocument = DashboardPrint();
            PrintManager pm = new PrintManager(this, printDocument, bitmaps.Count);
            pm.ShowDialog();
        }

        public PrintDocument DashboardPrint()
        {
            //프린터 설정
            count = 0;
            pageNo = 1;
            currentPageIndex = 0;

            //
            this.TopMost = true;
            Thread.Sleep(1000);
            this.Activate();
            this.Update();

            PrintDocument printDocument1 = new PrintDocument();
            bitmaps = new List<Bitmap>();
            List<Control> controls = new List<Control>(pnDashboard.Controls.Count);
            foreach (Control con in pnDashboard.Controls)
                controls.Add(con);


            //최대높이
            int maxHeight = this.pnDashboard.Height;
            int currentHeight = 0;
            int sttIdx = 0, endIdx = 0;

            for (int i = 0; i < pnDashboard.Controls.Count; i++)
            {
                //출력범위 이상 넘어갈때
                if (i == pnDashboard.Controls.Count - 1)
                {
                    Bitmap bitmap = new Bitmap(this.Width, this.Height);
                    if (currentHeight + pnDashboard.Controls[i].Height > maxHeight)
                    {
                        //마지막 idx제외하고 출력
                        endIdx = i;
                        for (int j = 0; j < pnDashboard.Controls.Count; j++)
                        {
                            if (j >= sttIdx && j < endIdx)
                                pnDashboard.Controls[j].Visible = true;
                            else
                                pnDashboard.Controls[j].Visible = false;
                        }
                        this.Update();
                        Thread.Sleep(500);
                        bitmap = CaptureForm(this);
                        bitmaps.Add(bitmap);

                        //마지막 idx만 다시 출력
                        for (int j = 0; j < pnDashboard.Controls.Count; j++)
                        {
                            if (j == i)
                                pnDashboard.Controls[j].Visible = true;
                            else
                                pnDashboard.Controls[j].Visible = false;
                        }

                        this.Update();
                        Thread.Sleep(500);
                        bitmap = CaptureForm(this);
                        bitmaps.Add(bitmap);

                    }
                    else
                    {
                        //마지막 idx제외하고 출력
                        endIdx = i;
                        for (int j = 0; j < pnDashboard.Controls.Count; j++)
                        {
                            if (j >= sttIdx && j <= endIdx)
                                pnDashboard.Controls[j].Visible = true;
                            else
                                pnDashboard.Controls[j].Visible = false;
                        }

                        this.Update();
                        Thread.Sleep(500);
                        bitmap = CaptureForm(this);
                        bitmaps.Add(bitmap);
                    }
                }
                else if (currentHeight + pnDashboard.Controls[i].Height > maxHeight)
                {
                    Bitmap bitmap = new Bitmap(this.Width, this.Height);
                    //마지막 idx제외하고 출력
                    endIdx = i - 1;
                    for (int j = 0; j < pnDashboard.Controls.Count; j++)
                    {
                        if (j >= sttIdx && j <= endIdx)
                            pnDashboard.Controls[j].Visible = true;
                        else
                            pnDashboard.Controls[j].Visible = false;
                    }
                    Thread.Sleep(100);
                    this.Update();
                    bitmap = CaptureForm(this);
                    bitmaps.Add(bitmap);

                    sttIdx = i;
                    currentHeight = pnDashboard.Controls[i].Height;
                }
                else
                    currentHeight += pnDashboard.Controls[i].Height;

            }
            //
            this.TopMost = false;

            for (int j = 0; j < pnDashboard.Controls.Count; j++)
                pnDashboard.Controls[j].Visible = true;

            printDocument1 = new PrintDocument();
            printDocument1.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169); // A4 용지 크기 (가로 방향)
            printDocument1.DefaultPageSettings.Landscape = true; // 가로 방향 설정
            printDocument1.PrintPage += PrintDocument_PrintPage;
            printDocument1.PrinterSettings.PrintRange = PrintRange.SomePages;
            printDocument1.PrinterSettings.FromPage = 1;
            printDocument1.PrinterSettings.ToPage = bitmaps.Count;

            return printDocument1;

            /*PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;
            printPreviewDialog.ShowDialog();*/
        }

        public PrintDocument DashboardPrint2()
        {
            //프린터 설정
            count = 0;
            pageNo = 1;
            currentPageIndex = 0;
            pnTop.Visible = false;
            pnBottom.Visible = false;

            //
            this.TopMost = true;
            this.Activate();
            this.Update();

            PrintDocument printDocument1 = new PrintDocument();
            bitmaps = new List<Bitmap>();
            List<Control> controls = new List<Control>(pnDashboard.Controls.Count);
            foreach (Control con in pnDashboard.Controls)
                controls.Add(con);

            for (int i = 0; i < pnDashboard.Controls.Count; i += 5)
            {
                Bitmap bitmap = new Bitmap(this.Width, this.Height);
                for (int j = 0; j < pnDashboard.Controls.Count; j++)
                {
                    if (!(j >= i && j <= i + 4))
                        pnDashboard.Controls[j].Visible = false;
                    else
                        pnDashboard.Controls[j].Visible = true;
                }
                this.Update();
                Thread.Sleep(100);


                bitmap = CaptureForm(this);
                bitmaps.Add(bitmap);
            }
            //
            this.TopMost = false;
            pnTop.Visible = true;
            pnBottom.Visible = true;

            for (int j = 0; j < pnDashboard.Controls.Count; j++)
                pnDashboard.Controls[j].Visible = true;

            printDocument1 = new PrintDocument();
            printDocument1.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169); // A4 용지 크기 (가로 방향)
            printDocument1.DefaultPageSettings.Landscape = true; // 가로 방향 설정
            printDocument1.PrintPage += PrintDocument_PrintPage;
            printDocument1.PrinterSettings.PrintRange = PrintRange.SomePages;
            printDocument1.PrinterSettings.FromPage = 1;
            printDocument1.PrinterSettings.ToPage = bitmaps.Count;

            return printDocument1;

            /*PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;
            printPreviewDialog.ShowDialog();*/
        }

        static Bitmap CaptureForm(Form form)
        {
            Rectangle bounds = form.Bounds;
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
            // 이미지 크기 2배 확대
            Bitmap highResolutionImage = ResampleImage(bitmap, 2);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            return bitmap;
        }

        // 이미지 재샘플링 메서드
        static Bitmap ResampleImage(Bitmap image, int scaleFactor)
        {
            int newWidth = image.Width * scaleFactor;
            int newHeight = image.Height * scaleFactor;

            Bitmap resizedImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight));
            }
            return resizedImage;
        }

        private PrintDocument printDocument = new PrintDocument();
        private int currentPageIndex = 0;
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (currentPageIndex < bitmaps.Count)
            {
                this.TopMost = true;
                Thread.Sleep(1000);
                Bitmap bitmap = bitmaps[currentPageIndex];
                Rectangle destRect = e.MarginBounds;

                // 비트맵 이미지를 출력 용지에 맞게 확대하여 출력
                destRect.Height = 827;
                destRect.Width = 1169;

                // 이미지를 가운데에 출력
                destRect.X += (e.MarginBounds.Width - destRect.Width) / 2;
                destRect.Y += (e.MarginBounds.Height - destRect.Height) / 2;

                e.Graphics.DrawImage(bitmap, destRect);

                currentPageIndex++;
                e.HasMorePages = currentPageIndex < bitmaps.Count;

                this.TopMost = false;
            }
        }
        //초기화
        public void InitVariable()
        {
            count = 0;
            pageNo = 1;
            DashboardPrint();
        }


        #endregion

        
    }
}
